using Group_50_CMPG223_HotelManagementSystem;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Group50_Hotel_System
{
    public partial class CheckInOut_Form : Form
    {
        public CheckInOut_Form()
        {
            InitializeComponent();
        }

        private void CheckInOut_Form_Load(object sender, EventArgs e)
        {
            LoadBookedGuests();
            LoadCheckedInGuests();
        }

        private void btnCheckin_Click(object sender, EventArgs e)
        {
            // Step 1: Validate if a booking is selected
            int bookingID = GetSelectedBookingID();
            if (bookingID == -1)
            {
                MessageBox.Show("Please select a booking to check in.");
                return;
            }

            // Step 2: Retrieve and load guest details into the Check-In tab controls
            LoadGuestDetailsForCheckIn(bookingID);

            // Step 3: Load available rooms based on the selected dates and the guest's room ID
            int selectedRoomID = (int)lblCheckInRSelected.Tag; // Assuming lblCheckInRSelected.Tag holds the Room_ID
            LoadAvailableRooms(dtpCheckInDate.Value, dtpCheckOutDate.Value, selectedRoomID);

            // Step 4: Switch to the Check-In tab and set focus
            tbCheckinForm.SelectedTab = tpCheckin;
            tpCheckin.Focus(); // Ensures the tab is focused
        }

        private void LoadBookedGuests()
        {
            using (SqlConnection connection = new SqlConnection(SessionManager.ConnectionString))
            {
                string query = @"
                SELECT 
                    gb.Booking_ID, 
                    g.First_Name, 
                    g.Last_Name, 
                    g.Contact_Num, 
                    g.Email, 
                    a.Street_Name, 
                    a.Town_City, 
                    r.Room_Number, 
                    gb.CheckIn_Date, 
                    gb.CheckOut_Date,
                    e.First_Name AS Employee_First_Name,
                    e.Surname AS Employee_Surname
                FROM 
                    Guest_Booking gb
                INNER JOIN 
                    Guests g ON gb.Guest_ID = g.Guest_ID
                INNER JOIN 
                    Address a ON g.Address_ID = a.Address_ID
                INNER JOIN 
                    Rooms r ON gb.Room_ID = r.Room_ID
                INNER JOIN 
                    Employees e ON gb.Employee_ID = e.Employee_ID
                WHERE 
                    gb.Is_CheckedIn = 0";

                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dgvCheckBooking.DataSource = table;
            }
        }

        private void LoadCheckedInGuests()
        {
            using (SqlConnection connection = new SqlConnection(SessionManager.ConnectionString))
            {
                string query = @"
                SELECT 
                    gb.Booking_ID, 
                    g.First_Name, 
                    g.Last_Name, 
                    g.Contact_Num, 
                    g.Email, 
                    a.Street_Name, 
                    a.Town_City, 
                    r.Room_Number, 
                    gb.CheckIn_Date, 
                    gb.CheckOut_Date,
                    gb.Payment_Date,
                    e.First_Name AS Employee_First_Name,
                    e.Surname AS Employee_Surname
                FROM 
                    Guest_Booking gb
                INNER JOIN 
                    Guests g ON gb.Guest_ID = g.Guest_ID
                INNER JOIN 
                    Address a ON g.Address_ID = a.Address_ID
                INNER JOIN 
                    Rooms r ON gb.Room_ID = r.Room_ID
                INNER JOIN 
                    Employees e ON gb.Employee_ID = e.Employee_ID
                WHERE 
                    gb.Is_CheckedIn = 1";

                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dgvCheckedCheckin.DataSource = table;
            }
        }

        private int GetSelectedBookingID()
        {
            if (dgvCheckBooking.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvCheckBooking.SelectedRows[0];
                return int.Parse(row.Cells["Booking_ID"].Value.ToString());
            }
            else if (dgvCheckBooking.CurrentRow != null)
            {
                DataGridViewRow row = dgvCheckBooking.CurrentRow;
                return int.Parse(row.Cells["Booking_ID"].Value.ToString());
            }
            else
            {
                MessageBox.Show("No booking selected.");
                return -1; // Return an invalid ID if no booking is selected
            }
        }

        private void LoadGuestDetailsForCheckIn(int bookingID)
        {
            using (SqlConnection connection = new SqlConnection(SessionManager.ConnectionString))
            {
                string query = @"
                    SELECT 
                        g.ID_Number, 
                        g.First_Name, 
                        g.Last_Name, 
                        g.Contact_Num, 
                        g.Email, 
                        a.Street_Name, 
                        a.Town_City,
                        r.Room_ID,
                        r.Room_Number,
                        r.Room_Type,
                        gb.CheckIn_Date,
                        gb.CheckOut_Date
                    FROM 
                        Guests g
                    INNER JOIN 
                        Address a ON g.Address_ID = a.Address_ID
                    INNER JOIN 
                        Guest_Booking gb ON gb.Guest_ID = g.Guest_ID
                    INNER JOIN 
                        Rooms r ON gb.Room_ID = r.Room_ID
                    WHERE 
                        gb.Booking_ID = @BookingID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BookingID", bookingID);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    // Set guest details (make these fields read-only)
                    txtCheckIDNum.Text = reader["ID_Number"].ToString();
                    txtCheckinName.Text = reader["First_Name"].ToString();
                    txtCheckinSurname.Text = reader["Last_Name"].ToString();
                    txtCheckinContactNum.Text = reader["Contact_Num"].ToString();
                    txtCheckinEmail.Text = reader["Email"].ToString();
                    txtCheckinStreet.Text = reader["Street_Name"].ToString();
                    txtCheckinCity.Text = reader["Town_City"].ToString();
                    lblCheckInRSelected.Text = "Room Number: " + reader["Room_Number"].ToString() + " (" + reader["Room_Type"].ToString() + ")";

                    // Disable the fields to prevent editing
                    txtCheckIDNum.Enabled = false;
                    txtCheckinName.Enabled = false;
                    txtCheckinSurname.Enabled = false;
                    txtCheckinContactNum.Enabled = false;
                    txtCheckinEmail.Enabled = false;
                    txtCheckinStreet.Enabled = false;
                    txtCheckinCity.Enabled = false;
                    lblCheckInRSelected.Enabled = false;

                    // Store the room ID for further reference
                    lblCheckInRSelected.Tag = reader["Room_ID"];
                }
                reader.Close();
            }
        }

        private void LoadAvailableRooms(DateTime checkInDate, DateTime checkOutDate, int selectedRoomID)
        {
            using (SqlConnection connection = new SqlConnection(SessionManager.ConnectionString))
            {
                string query = @"
                    SELECT Room_ID, Room_Number, Room_Type 
                    FROM Rooms 
                    WHERE Room_ID NOT IN (
                        SELECT Room_ID 
                        FROM Guest_Booking 
                        WHERE 
                            (Room_ID != @SelectedRoomID AND 
                            ((@CheckInDate BETWEEN CheckIn_Date AND CheckOut_Date)
                            OR 
                            (@CheckOutDate BETWEEN CheckIn_Date AND CheckOut_Date)
                            OR 
                            (CheckIn_Date BETWEEN @CheckInDate AND @CheckOutDate)))
                    )";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CheckInDate", checkInDate);
                command.Parameters.AddWithValue("@CheckOutDate", checkOutDate);
                command.Parameters.AddWithValue("@SelectedRoomID", selectedRoomID); // Exclude the guest's current room

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dgvCheckinRooms.DataSource = table;
            }
        }

        private void ClearCheckInControls()
        {
            txtCheckIDNum.Clear();
            txtCheckinName.Clear();
            txtCheckinSurname.Clear();
            txtCheckinContactNum.Clear();
            txtCheckinEmail.Clear();
            txtCheckinStreet.Clear();
            txtCheckinCity.Clear();
            lblCheckInRSelected.Text = "No Room Selected!";
            dtpCheckInDate.Value = DateTime.Now;
            dtpCheckOutDate.Value = DateTime.Now;
            cbBankType.SelectedIndex = -1;
            txtCardNumber.Clear();
            cbCardType.SelectedIndex = -1;
            cbMonth.SelectedIndex = -1;
            cbYear.SelectedIndex = -1;
            radDebit.Checked = false;
            radCredit.Checked = false;

            // Re-enable fields for the next use
            txtCheckIDNum.Enabled = true;
            txtCheckinName.Enabled = true;
            txtCheckinSurname.Enabled = true;
            txtCheckinContactNum.Enabled = true;
            txtCheckinEmail.Enabled = true;
            txtCheckinStreet.Enabled = true;
            txtCheckinCity.Enabled = true;
            lblCheckInRSelected.Enabled = true;
        }

        private void dgvCheckBooking_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvCheckBooking.Rows[e.RowIndex].Cells["Booking_ID"].Value != DBNull.Value)
            {
                DataGridViewRow row = dgvCheckBooking.Rows[e.RowIndex];
                int bookingID = int.Parse(row.Cells["Booking_ID"].Value.ToString());

                // Display guest name and surname
                lblSelectedGuest.Text = "Selected Guest: " + row.Cells["First_Name"].Value.ToString() + " " + row.Cells["Last_Name"].Value.ToString();

                LoadGuestDetailsForCheckIn(bookingID);
            }
            else
            {
                // Set label to indicate no valid guest is selected
                lblSelectedGuest.Text = "No valid guest selected.";
            }
        }



        private void dgvCheckedCheckin_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvCheckedCheckin.Rows[e.RowIndex].Cells["Booking_ID"].Value != DBNull.Value)
            {
                DataGridViewRow row = dgvCheckedCheckin.Rows[e.RowIndex];
                int bookingID = int.Parse(row.Cells["Booking_ID"].Value.ToString());

                lblSelectedGuest.Text = "Selected Guest: " + row.Cells["First_Name"].Value.ToString() + " " + row.Cells["Last_Name"].Value.ToString();

                LoadGuestDetailsForCheckIn(bookingID);
                LoadBankingDetailsForCheckedInGuest(bookingID);
            }
            else
            {
                // Set label to indicate no valid guest is selected
                lblSelectedGuest.Text = "No valid guest selected.";
            }
        }


        private void dgvCheckinRooms_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvCheckinRooms.Rows[e.RowIndex].Cells["Room_ID"].Value != DBNull.Value)
            {
                DataGridViewRow row = dgvCheckinRooms.Rows[e.RowIndex];
                string roomNumber = row.Cells["Room_Number"].Value.ToString();
                lblCheckInRSelected.Text = $"Selected Room: {roomNumber} ({row.Cells["Room_Type"].ToString()})";

                // Store Room_ID for later use
                lblCheckInRSelected.Tag = row.Cells["Room_ID"].Value;
            }
            else
            {
                // Set label to indicate no valid room is selected
                lblCheckInRSelected.Text = "No Room Selected!";
            }
        }



        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            int bookingID = int.Parse(lblSelectedGuest.Text.Replace("Selected Guest: ", ""));
            try
            {
                using (SqlConnection connection = new SqlConnection(SessionManager.ConnectionString))
                {
                    connection.Open();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            SqlCommand command = new SqlCommand("UPDATE Guest_Booking SET Is_CheckedOut = 1 WHERE Booking_ID = @BookingID", connection, transaction);
                            command.Parameters.AddWithValue("@BookingID", bookingID);
                            command.ExecuteNonQuery();

                            transaction.Commit();
                            MessageBox.Show("Guest checked out successfully!");

                            LoadCheckedInGuests();
                            ClearCheckInControls();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show("An error occurred while checking out the guest: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while connecting to the database: " + ex.Message);
            }
        }

        private void btnCheckedClearControls_Click(object sender, EventArgs e)
        {
            ClearCheckInControls();
        }

        private void LoadBankingDetailsForCheckedInGuest(int bookingID)
        {
            using (SqlConnection connection = new SqlConnection(SessionManager.ConnectionString))
            {
                string query = @"
        SELECT 
            Banking_ID, 
            Card_Type, 
            Bank, 
            Card_Num, 
            CASE WHEN Debit_Credit = 0 THEN 'Debit' ELSE 'Credit' END AS DebitOrCredit,
            Card_Holder, 
            Expiration_Date
        FROM 
            BankingDetails 
        WHERE 
            Banking_ID = (SELECT Banking_ID FROM Guest_Booking WHERE Booking_ID = @BookingID)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BookingID", bookingID);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dgvCheckedBanking.DataSource = table;
            }
        }


        private void btnCheckInCheckedIn_Click(object sender, EventArgs e)
        {
            // Unlock the controls when the Check-In button is clicked
            UnlockCheckInControls();

            // Step 1: Validate if all banking details are entered
            if (cbBankType.SelectedIndex == -1 || string.IsNullOrWhiteSpace(txtCardNumber.Text) ||
                cbCardType.SelectedIndex == -1 || cbMonth.SelectedIndex == -1 ||
                cbYear.SelectedIndex == -1 || (!radDebit.Checked && !radCredit.Checked))
            {
                MessageBox.Show("Please ensure all banking details are entered correctly.");
                return;
            }

            // Step 2: Validate if a booking is selected
            int bookingID = GetSelectedBookingID();
            if (bookingID == -1)
            {
                MessageBox.Show("Please select a booking to check in.");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(SessionManager.ConnectionString))
                {
                    connection.Open();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Step 3: Update the booking to set Is_CheckedIn to true and Payment_Date to today's date
                            SqlCommand command = new SqlCommand(
                                "UPDATE Guest_Booking SET Is_CheckedIn = 1, Payment_Date = @PaymentDate WHERE Booking_ID = @BookingID",
                                connection, transaction);
                            command.Parameters.AddWithValue("@BookingID", bookingID);
                            command.Parameters.AddWithValue("@PaymentDate", DateTime.Today); // Store today's date as Payment_Date
                            command.CommandTimeout = 120;  // Increase timeout if needed
                            command.ExecuteNonQuery();

                            // Step 4: Prepare and save banking details
                            DateTime expirationDate = new DateTime(int.Parse(cbYear.SelectedItem.ToString()), int.Parse(cbMonth.SelectedItem.ToString()), 1);

                            SqlCommand bankingCommand = new SqlCommand(@"
                        INSERT INTO BankingDetails (Card_Type, Bank, Card_Num, Debit_Credit, Card_Holder, Expiration_Date)
                        VALUES (@CardType, @Bank, @CardNum, @DebitCredit, @CardHolder, @ExpirationDate); 
                        SELECT SCOPE_IDENTITY();", connection, transaction);

                            bankingCommand.Parameters.AddWithValue("@CardType", cbCardType.SelectedItem.ToString());
                            bankingCommand.Parameters.AddWithValue("@Bank", cbBankType.SelectedItem.ToString());
                            bankingCommand.Parameters.AddWithValue("@CardNum", txtCardNumber.Text);

                            // Store 0 for Debit and 1 for Credit
                            bankingCommand.Parameters.AddWithValue("@DebitCredit", radDebit.Checked ? 0 : 1);

                            bankingCommand.Parameters.AddWithValue("@CardHolder", txtCheckinName.Text + " " + txtCheckinSurname.Text);
                            bankingCommand.Parameters.AddWithValue("@ExpirationDate", expirationDate); // Store as a Date

                            bankingCommand.CommandTimeout = 120;  // Increase timeout if needed
                            int bankingID = Convert.ToInt32(bankingCommand.ExecuteScalar());

                            // Step 5: Update the Guest_Booking table with the new Banking_ID
                            SqlCommand updateBookingCommand = new SqlCommand("UPDATE Guest_Booking SET Banking_ID = @BankingID WHERE Booking_ID = @BookingID", connection, transaction);
                            updateBookingCommand.Parameters.AddWithValue("@BankingID", bankingID);
                            updateBookingCommand.Parameters.AddWithValue("@BookingID", bookingID);
                            updateBookingCommand.ExecuteNonQuery();

                            // Commit the transaction
                            transaction.Commit();
                            MessageBox.Show("Guest checked in successfully!");

                            // Step 6: Refresh all DataGridViews
                            LoadBookedGuests();      // Refresh the list of booked guests
                            LoadCheckedInGuests();   // Refresh the list of checked-in guests
                            LoadAvailableRooms(dtpCheckInDate.Value, dtpCheckOutDate.Value, (int)lblCheckInRSelected.Tag); // Refresh the available rooms

                            // Step 7: Switch back to the overview tab
                            tbCheckinForm.SelectedTab = tpOverview;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show("An error occurred while checking in the guest: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while connecting to the database: " + ex.Message);
            }
        }

        private void UnlockCheckInControls()
        {
            // Enable guest information controls
            txtCheckIDNum.Enabled = true;
            txtCheckinName.Enabled = true;
            txtCheckinSurname.Enabled = true;
            txtCheckinContactNum.Enabled = true;
            txtCheckinEmail.Enabled = true;
            txtCheckinStreet.Enabled = true;
            txtCheckinCity.Enabled = true;
            lblCheckInRSelected.Enabled = true;

            // Enable other check-in controls
            dtpCheckInDate.Enabled = true;
            dtpCheckOutDate.Enabled = true;
            cbBankType.Enabled = true;
            txtCardNumber.Enabled = true;
            cbCardType.Enabled = true;
            cbMonth.Enabled = true;
            cbYear.Enabled = true;
            radDebit.Enabled = true;
            radCredit.Enabled = true;
        }


    }
}
