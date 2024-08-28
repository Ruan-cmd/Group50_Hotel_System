using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Group_50_CMPG223_HotelManagementSystem
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
            int guestID = GetGuestIDForBooking(bookingID.ToString());

            // Load guest details and get the selected room ID
            LoadGuestDetailsForCheckIn(guestID);
            int selectedRoomID = (int)lblCheckInRSelected.Tag; // Assuming lblCheckInRSelected.Tag holds the Room_ID

            // Step 3: Load available rooms based on the selected dates and the guest's room ID
            LoadAvailableRooms(dtpCheckInDate.Value, dtpCheckOutDate.Value, selectedRoomID);

            // Step 4: Switch to the Check-In tab and set focus
            tbCheckinForm.SelectedTab = tpCheckin;
            tpCheckin.Focus(); // Ensures the tab is focused
        }

        private void btnCheckInCheckedIn_Click_1(object sender, EventArgs e)
        {
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
                            // Step 3: Update the booking to set Is_CheckedIn to true
                            SqlCommand command = new SqlCommand("UPDATE Guest_Booking SET Is_CheckedIn = 1 WHERE Booking_ID = @BookingID", connection, transaction);
                            command.Parameters.AddWithValue("@BookingID", bookingID);
                            command.CommandTimeout = 120;  // Increase timeout if needed
                            command.ExecuteNonQuery();

                            // Step 4: Prepare and save banking details
                            DateTime expirationDate = new DateTime(int.Parse(cbYear.SelectedItem.ToString()), int.Parse(cbMonth.SelectedItem.ToString()), 1);

                            SqlCommand bankingCommand = new SqlCommand(@"
                        INSERT INTO BankingDetails (Guest_ID, Card_Type, Bank, Card_Num, Debit_Credit, Card_Holder, Expiration_Date)
                        VALUES (@GuestID, @CardType, @Bank, @CardNum, @DebitCredit, @CardHolder, @ExpirationDate)", connection, transaction);

                            bankingCommand.Parameters.AddWithValue("@GuestID", GetGuestIDForBooking(bookingID.ToString()));
                            bankingCommand.Parameters.AddWithValue("@CardType", cbCardType.SelectedItem.ToString());
                            bankingCommand.Parameters.AddWithValue("@Bank", cbBankType.SelectedItem.ToString());
                            bankingCommand.Parameters.AddWithValue("@CardNum", txtCardNumber.Text);

                            // Store 0 for Debit and 1 for Credit
                            bankingCommand.Parameters.AddWithValue("@DebitCredit", radDebit.Checked ? 0 : 1);

                            bankingCommand.Parameters.AddWithValue("@CardHolder", txtCheckinName.Text + " " + txtCheckinSurname.Text);
                            bankingCommand.Parameters.AddWithValue("@ExpirationDate", expirationDate); // Store as a Date

                            bankingCommand.CommandTimeout = 120;  // Increase timeout if needed
                            bankingCommand.ExecuteNonQuery();

                            // Commit the transaction
                            transaction.Commit();
                            MessageBox.Show("Guest checked in successfully!");

                            // Step 5: Switch back to the overview tab and refresh the data
                            tbCheckinForm.SelectedTab = tpOverview;
                            LoadCheckedInGuests();  // Refresh the list of checked-in guests
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            LogDetailedError(ex, "SQL Command Execution Error");
                            MessageBox.Show("An error occurred while checking in the guest: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogDetailedError(ex, "Database Connection Error");
                MessageBox.Show("An error occurred while connecting to the database: " + ex.Message);
            }
        }

        private void LogDetailedError(Exception ex, string context)
        {
            string detailedMessage = $"Context: {context}\n" +
                                     $"Message: {ex.Message}\n" +
                                     $"Stack Trace: {ex.StackTrace}";

            if (ex.InnerException != null)
            {
                detailedMessage += $"\nInner Exception Message: {ex.InnerException.Message}\n" +
                                   $"Inner Exception Stack Trace: {ex.InnerException.StackTrace}";
            }

            MessageBox.Show(detailedMessage, "Detailed Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private int GetGuestIDForBooking(string bookingID)
        {
            int guestID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(SessionManager.ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT Guest_ID FROM Guest_Booking WHERE Booking_ID = @BookingID", connection);
                    command.Parameters.AddWithValue("@BookingID", bookingID);
                    command.CommandTimeout = 30;  // Reduced timeout to detect issues faster

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            guestID = int.Parse(reader["Guest_ID"].ToString());
                        }
                        else
                        {
                            MessageBox.Show($"No Guest ID found for Booking ID: {bookingID}", "Guest Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                LogDetailedError(ex, "SQL Error in GetGuestIDForBooking");
                MessageBox.Show("A database error occurred while retrieving the Guest ID. Please try again later.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                LogDetailedError(ex, "Error in GetGuestIDForBooking");
                MessageBox.Show("An unexpected error occurred while retrieving the Guest ID. Please try again.", "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return guestID;
        }



        private void LoadGuestDetailsForCheckIn(int guestID)
        {
            using (SqlConnection connection = new SqlConnection(SessionManager.ConnectionString))
            {
                string query = @"
                    SELECT 
                        Guests.ID_Number, 
                        Guests.First_Name, 
                        Guests.Last_Name, 
                        Guests.Contact_Num, 
                        Guests.Email, 
                        Address.Street_Name, 
                        Address.Town_City,
                        Rooms.Room_ID,
                        Rooms.Room_Number,
                        Rooms.Room_Type,
                        gb.CheckIn_Date,
                        gb.CheckOut_Date
                    FROM 
                        Guests 
                    INNER JOIN 
                        Address ON Guests.Address_ID = Address.Address_ID
                    INNER JOIN 
                        Guest_Booking gb ON gb.Guest_ID = Guests.Guest_ID
                    INNER JOIN 
                        Rooms ON gb.Room_ID = Rooms.Room_ID
                    WHERE 
                        Guests.Guest_ID = @GuestID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@GuestID", guestID);
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
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCheckBooking.Rows[e.RowIndex];
                int guestID = GetGuestIDForBooking(row.Cells["Booking_ID"].Value.ToString());

                // Display guest name and surname
                lblSelectedGuest.Text = "Selected Guest: " + row.Cells["First_Name"].Value.ToString() + " " + row.Cells["Last_Name"].Value.ToString();

                LoadGuestDetailsForCheckIn(guestID);
            }
        }

        private void dgvCheckedCheckin_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCheckedCheckin.Rows[e.RowIndex];
                int guestID = int.Parse(row.Cells["Guest_ID"].Value.ToString());
                lblSelectedGuest.Text = "Selected Guest: " + guestID.ToString();

                LoadGuestDetailsForCheckIn(guestID);
                LoadBankingDetailsForCheckedInGuest(guestID);
            }
        }

        private void dgvCheckinRooms_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCheckinRooms.Rows[e.RowIndex];
                string roomNumber = row.Cells["Room_Number"].Value.ToString();
                lblCheckInRSelected.Text = $"Selected Room: {roomNumber} ({row.Cells["Room_Type"].ToString()})";

                // Store Room_ID for later use
                lblCheckInRSelected.Tag = row.Cells["Room_ID"].Value;
            }
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            int guestID = int.Parse(lblSelectedGuest.Text.Replace("Selected Guest: ", ""));
            try
            {
                using (SqlConnection connection = new SqlConnection(SessionManager.ConnectionString))
                {
                    connection.Open();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            SqlCommand command = new SqlCommand("UPDATE Guest_Booking SET Is_CheckedOut = 1 WHERE Guest_ID = @GuestID", connection, transaction);
                            command.Parameters.AddWithValue("@GuestID", guestID);
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

        private void LoadBankingDetailsForCheckedInGuest(int guestID)
        {
            using (SqlConnection connection = new SqlConnection(SessionManager.ConnectionString))
            {
                string query = "SELECT * FROM BankingDetails WHERE Guest_ID = @GuestID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@GuestID", guestID);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dgvCheckedBanking.DataSource = table;
            }
        }

        // This method will handle the 'Update Check-in' button click event.
        private void btnUpdateCheckin_Click(object sender, EventArgs e)
        {
            // Code to update check-in details will go here.
        }

        // This method will handle the 'Book Out' button click event.
        private void btnBookOut_Click(object sender, EventArgs e)
        {
            // Code to handle guest check-out will go here.
        }







    }
}
