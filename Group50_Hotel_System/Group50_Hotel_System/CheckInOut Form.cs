using Group_50_CMPG223_HotelManagementSystem;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Group50_Hotel_System
{
    public partial class CheckInOut_Form : Form
    {
        private int selectedBookingID = -1;
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
            // Clear previous banking details
            ClearBankingDetails();

            // Step 1: Validate if a booking is selected
            int bookingID = GetSelectedBookingID();
            if (bookingID == -1)
            {
                MessageBox.Show("Please select a booking to check in.");
                return;
            }

            // Step 2: Retrieve and load guest details into the Check-In tab controls
            LoadGuestDetailsForCheckIn(bookingID);

            // Step 3: Load the guest's room and lock the room DataGridView
            LoadAvailableRooms(bookingID);
            dgvCheckinRooms.Enabled = false;  // Lock the room selection

            // Step 4: Switch to the Check-In tab and set focus
            tbcCheckinForm.SelectedTab = tpCheckin;
            tpCheckin.Focus();

            // Step 5: Hide the "Update" button and show "Check-In"
            btnQuestsUpdate.Visible = false;
            btnCheckInCheckedIn.Visible = true;
        }

        private void ClearBankingDetails()
        {
            cbBankType.SelectedIndex = -1;
            txtCardNumber.Clear();
            cbCardType.SelectedIndex = -1;
            cbMonth.SelectedIndex = -1;
            cbYear.SelectedIndex = -1;
            radDebit.Checked = false;
            radCredit.Checked = false;
            txtCardHolder.Clear();
        }


        private void LoadBookedGuests(string searchTerm = "")
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
                gb.Is_CheckedIn = 0 AND 
                (g.First_Name LIKE @SearchTerm OR g.Last_Name LIKE @SearchTerm OR g.Contact_Num LIKE @SearchTerm)";

                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");

                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dgvCheckBooking.DataSource = table;
            }
        }

        private void LoadCheckedInGuests(string searchTerm = "")
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
                gb.Is_CheckedIn = 1 AND gb.Is_CheckedOut = 0 AND 
                (g.First_Name LIKE @SearchTerm OR g.Last_Name LIKE @SearchTerm OR g.Contact_Num LIKE @SearchTerm)";

                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");

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
                    // Set guest details (do not lock the fields)
                    txtCheckIDNum.Text = reader["ID_Number"].ToString();
                    txtCheckinName.Text = reader["First_Name"].ToString();
                    txtCheckinSurname.Text = reader["Last_Name"].ToString();
                    txtCheckinContactNum.Text = reader["Contact_Num"].ToString();
                    txtCheckinEmail.Text = reader["Email"].ToString();
                    txtCheckinStreet.Text = reader["Street_Name"].ToString();
                    txtCheckinCity.Text = reader["Town_City"].ToString();
                    lblCheckInRSelected.Text = "Room Number: " + reader["Room_Number"].ToString() + " (" + reader["Room_Type"].ToString() + ")";

                    // Store the room ID for further reference
                    lblCheckInRSelected.Tag = reader["Room_ID"];

                    // Populate the date pickers with the check-in and check-out dates
                    dtpCheckInDate.Value = Convert.ToDateTime(reader["CheckIn_Date"]);
                    dtpCheckOutDate.Value = Convert.ToDateTime(reader["CheckOut_Date"]);
                }
                reader.Close();
            }
        }


        private void LoadAvailableRooms(int bookingID)
        {
            using (SqlConnection connection = new SqlConnection(SessionManager.ConnectionString))
            {
                string query = @"
            SELECT r.Room_ID, r.Room_Number, r.Room_Type
            FROM Rooms r
            INNER JOIN Guest_Booking gb ON r.Room_ID = gb.Room_ID
            WHERE gb.Booking_ID = @BookingID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BookingID", bookingID);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);
                dgvCheckinRooms.DataSource = table;

                // Automatically select the room in the DataGridView
                if (table.Rows.Count > 0)
                {
                    lblCheckInRSelected.Text = $"Selected Room: {table.Rows[0]["Room_Number"]} ({table.Rows[0]["Room_Type"]})";
                    lblCheckInRSelected.Tag = table.Rows[0]["Room_ID"];
                }
                else
                {
                    lblCheckInRSelected.Text = "No Room Selected!";
                    lblCheckInRSelected.Tag = null;
                }
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
            txtCardHolder.Clear();  // Ensure the card holder textbox is cleared

            // Re-enable fields for the next use
            txtCheckIDNum.Enabled = true;
            txtCheckinName.Enabled = true;
            txtCheckinSurname.Enabled = true;
            txtCheckinContactNum.Enabled = true;
            txtCheckinEmail.Enabled = true;
            txtCheckinStreet.Enabled = true;
            txtCheckinCity.Enabled = true;
            lblCheckInRSelected.Enabled = true;

            // By default, show "Check-In" button and hide "Update" button
            btnCheckInCheckedIn.Visible = true;
            btnQuestsUpdate.Visible = false;
        }


        private void dgvCheckBooking_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvCheckBooking.Rows[e.RowIndex].Cells["Booking_ID"].Value != DBNull.Value)
            {
                DataGridViewRow row = dgvCheckBooking.Rows[e.RowIndex];
                selectedBookingID = int.Parse(row.Cells["Booking_ID"].Value.ToString());

                // Display guest name and surname
                lblSelectedGuest.Text = "Selected Guest: " + row.Cells["First_Name"].Value.ToString() + " " + row.Cells["Last_Name"].Value.ToString();

                // Lock Check-In buttons and unlock Booked buttons
                gbCheckinButtons.Enabled = false;
                gbBookedButtons.Enabled = true;
            }
            else
            {
                // Set label to indicate no valid guest is selected
                lblSelectedGuest.Text = "No valid guest selected.";
                selectedBookingID = -1; // Reset the selected booking ID
            }
        }



        private void dgvCheckedCheckin_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvCheckedCheckin.Rows[e.RowIndex].Cells["Booking_ID"].Value != DBNull.Value)
            {
                DataGridViewRow row = dgvCheckedCheckin.Rows[e.RowIndex];
                selectedBookingID = int.Parse(row.Cells["Booking_ID"].Value.ToString());

                // Just update the selectedBookingID and enable the relevant buttons, do not populate textboxes
                gbCheckinButtons.Enabled = true;
                gbBookedButtons.Enabled = false;
            }
            else
            {
                // Set label to indicate no valid guest is selected
                lblSelectedGuest.Text = "No valid guest selected.";
                selectedBookingID = -1; // Reset the selected booking ID
            }
        }

        private void LoadBankingDetailsIntoDGV(int bookingID)
        {
            using (SqlConnection connection = new SqlConnection(SessionManager.ConnectionString))
            {
                string query = @"
        SELECT 
            bd.Banking_ID, 
            bd.Card_Type, 
            bd.Bank, 
            bd.Card_Num, 
            CASE WHEN bd.Debit_Credit = 0 THEN 'Debit' ELSE 'Credit' END AS DebitOrCredit,
            bd.Card_Holder, 
            bd.Expiration_Date
        FROM 
            BankingDetails bd
        INNER JOIN 
            Guest_Booking gb ON bd.Banking_ID = gb.Banking_ID
        WHERE 
            gb.Booking_ID = @BookingID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BookingID", bookingID);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                System.Data.DataTable table = new System.Data.DataTable();
                adapter.Fill(table);

                dgvCheckedBanking.DataSource = table;

                // Update the txtCardHolder textbox with the cardholder name
                if (table.Rows.Count > 0)
                {
                    txtCardHolder.Text = table.Rows[0]["Card_Holder"].ToString();
                }
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
            // Step 1: Validate if a booking is selected
            int bookingID = GetSelectedBookingIDFromCheckedIn();
            if (bookingID == -1)
            {
                MessageBox.Show("Please select a checked-in guest to check out.");
                return;
            }


            // Step 2: Confirm check-out action
            DialogResult result = MessageBox.Show("Are you sure you want to check out this guest?", "Confirm Check-Out", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                // Step 3: Display the review form
                using (Review_Hotel reviewForm = new Review_Hotel())
                {
                    if (reviewForm.ShowDialog() == DialogResult.OK)
                    {
                        int rating = reviewForm.SelectedRating; // Get the selected rating from the review form

                        try
                        {
                            using (SqlConnection connection = new SqlConnection(SessionManager.ConnectionString))
                            {
                                connection.Open();
                                using (SqlTransaction transaction = connection.BeginTransaction())
                                {
                                    try
                                    {
                                        // Step 4: Update the booking to set Is_CheckedOut to true and store the rating
                                        SqlCommand command = new SqlCommand(
                                            "UPDATE Guest_Booking SET Is_CheckedOut = 1, Review_Hotel = @Rating WHERE Booking_ID = @BookingID",
                                            connection, transaction);
                                        command.Parameters.AddWithValue("@BookingID", bookingID);
                                        command.Parameters.AddWithValue("@Rating", rating); // Store the rating
                                        command.ExecuteNonQuery();

                                        // Step 5: Commit the transaction
                                        transaction.Commit();
                                        MessageBox.Show("Guest checked out successfully!");

                                        // Step 6: Refresh the DataGridViews
                                        LoadCheckedInGuests(); // Refresh the list of checked-in guests
                                        ClearCheckInControls(); // Clear the input controls
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
                }
            }
        }
        private int GetSelectedBookingIDFromCheckedIn()
        {
            if (dgvCheckedCheckin.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvCheckedCheckin.SelectedRows[0];
                return int.Parse(row.Cells["Booking_ID"].Value.ToString());
            }
            else if (dgvCheckedCheckin.CurrentRow != null)
            {
                DataGridViewRow row = dgvCheckedCheckin.CurrentRow;
                return int.Parse(row.Cells["Booking_ID"].Value.ToString());
            }
            else
            {
                return -1; // Return an invalid ID if no booking is selected
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
            bd.Card_Type, 
            bd.Bank, 
            bd.Card_Num, 
            bd.Debit_Credit, 
            bd.Card_Holder, 
            bd.Expiration_Date
        FROM 
            BankingDetails bd
        INNER JOIN 
            Guest_Booking gb ON bd.Banking_ID = gb.Banking_ID
        WHERE 
            gb.Booking_ID = @BookingID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BookingID", bookingID);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    // Populate the banking details controls
                    cbCardType.SelectedItem = reader["Card_Type"].ToString();
                    cbBankType.SelectedItem = reader["Bank"].ToString();
                    txtCardNumber.Text = reader["Card_Num"].ToString();

                    // Assign the cardholder name to the txtCardHolder TextBox
                    txtCardHolder.Text = reader["Card_Holder"].ToString();

                    // Check if Debit_Credit is not null and cast accordingly
                    if (reader["Debit_Credit"] != DBNull.Value)
                    {
                        bool isDebit = Convert.ToInt32(reader["Debit_Credit"]) == 0;
                        radDebit.Checked = isDebit;
                        radCredit.Checked = !isDebit;
                    }

                    // Expiration Date (assume format is yyyy-MM-dd)
                    if (reader["Expiration_Date"] != DBNull.Value)
                    {
                        DateTime expirationDate = Convert.ToDateTime(reader["Expiration_Date"]);
                        cbMonth.SelectedItem = expirationDate.Month.ToString("00");
                        cbYear.SelectedItem = expirationDate.Year.ToString();
                    }
                }
                reader.Close();
            }
        }



        private void btnCheckInCheckedIn_Click(object sender, EventArgs e)
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
                            // Step 3: Update the booking to set Is_CheckedIn to true and save Payment_Date
                            SqlCommand command = new SqlCommand(
                                "UPDATE Guest_Booking SET Is_CheckedIn = 1, Payment_Date = @PaymentDate WHERE Booking_ID = @BookingID",
                                connection, transaction);
                            command.Parameters.AddWithValue("@BookingID", bookingID);
                            command.Parameters.AddWithValue("@PaymentDate", DateTime.Today);
                            command.CommandTimeout = 120;
                            command.ExecuteNonQuery();

                            // Step 4: Save banking details
                            DateTime expirationDate = new DateTime(int.Parse(cbYear.SelectedItem.ToString()), int.Parse(cbMonth.SelectedItem.ToString()), 1);
                            SqlCommand bankingCommand = new SqlCommand(@"
                    INSERT INTO BankingDetails (Card_Type, Bank, Card_Num, Debit_Credit, Card_Holder, Expiration_Date)
                    VALUES (@CardType, @Bank, @CardNum, @DebitCredit, @CardHolder, @ExpirationDate); 
                    SELECT SCOPE_IDENTITY();", connection, transaction);
                            bankingCommand.Parameters.AddWithValue("@CardType", cbCardType.SelectedItem.ToString());
                            bankingCommand.Parameters.AddWithValue("@Bank", cbBankType.SelectedItem.ToString());
                            bankingCommand.Parameters.AddWithValue("@CardNum", txtCardNumber.Text);
                            bankingCommand.Parameters.AddWithValue("@DebitCredit", radDebit.Checked ? 0 : 1);
                            bankingCommand.Parameters.AddWithValue("@CardHolder", txtCardHolder.Text);
                            bankingCommand.Parameters.AddWithValue("@ExpirationDate", expirationDate);
                            bankingCommand.CommandTimeout = 120;
                            int bankingID = Convert.ToInt32(bankingCommand.ExecuteScalar());

                            // Step 5: Update Guest_Booking with the new Banking_ID
                            SqlCommand updateBookingCommand = new SqlCommand(
                                "UPDATE Guest_Booking SET Banking_ID = @BankingID WHERE Booking_ID = @BookingID",
                                connection, transaction);
                            updateBookingCommand.Parameters.AddWithValue("@BankingID", bankingID);
                            updateBookingCommand.Parameters.AddWithValue("@BookingID", bookingID);
                            updateBookingCommand.ExecuteNonQuery();

                            // Commit transaction
                            transaction.Commit();
                            MessageBox.Show("Guest checked in successfully!");

                            // Step 6: Refresh DataGridViews
                            LoadBookedGuests();
                            LoadCheckedInGuests();

                            // Step 7: Clear controls and switch back to the overview tab
                            ClearCheckInControls();
                            tbcCheckinForm.SelectedTab = tpOverview;
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

            // By default, show "Check-In" button and hide "Update" button
            btnCheckInCheckedIn.Visible = true;
            btnQuestsUpdate.Visible = false;
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Main_Form main_Form = new Main_Form();
            main_Form.ShowDialog();
            this.Close();
        }


        private bool IsGuestDetailsModified()
        {
            // Compare current guest details with the ones stored to see if any changes were made
            // Implement this logic according to your application's requirements
            return true; // This should return true if modifications were detected
        }

        private void UpdateGuestDetailsInDatabase(int bookingID, SqlConnection connection, SqlTransaction transaction)
        {
            // Implement the logic to update guest details in the database
            SqlCommand command = new SqlCommand(@"UPDATE Guests SET First_Name = @FirstName, Last_Name = @LastName, 
                                          Contact_Num = @ContactNum, Email = @Email WHERE Guest_ID = 
                                          (SELECT Guest_ID FROM Guest_Booking WHERE Booking_ID = @BookingID)", connection, transaction);

            command.Parameters.AddWithValue("@FirstName", txtCheckinName.Text);
            command.Parameters.AddWithValue("@LastName", txtCheckinSurname.Text);
            command.Parameters.AddWithValue("@ContactNum", txtCheckinContactNum.Text);
            command.Parameters.AddWithValue("@Email", txtCheckinEmail.Text);
            command.Parameters.AddWithValue("@BookingID", bookingID);

            command.ExecuteNonQuery();
        }



        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            LoadBookedGuests(searchTerm);
            LoadCheckedInGuests(searchTerm);
        }

        private void btnUpdateCheckin_Click(object sender, EventArgs e)
        {
            // Step 1: Validate if a guest is selected
            if (selectedBookingID == -1)
            {
                MessageBox.Show("Please select a guest to update.");
                return;
            }

            // Step 2: Ensure a guest is selected
            if (string.IsNullOrWhiteSpace(lblSelectedGuest.Text) || lblSelectedGuest.Text == "No valid guest selected.")
            {
                MessageBox.Show("Please ensure a valid guest is selected.");
                return;
            }

            // Step 3: Load guest and banking details
            LoadGuestDetailsForCheckIn(selectedBookingID);
            LoadBankingDetailsForCheckedInGuest(selectedBookingID);

            // Step 4: Lock room DataGridView
            dgvCheckinRooms.Enabled = false;

            // Step 5: Change the tab text to "Update Check In"
            tbcCheckinForm.TabPages[tpCheckin.Name].Text = "Update Check In";

            // Step 6: Hide the "Check-In" button and show the "Update" button
            btnCheckInCheckedIn.Visible = false;
            btnQuestsUpdate.Visible = true;

            // Step 7: Switch to the Check-In tab
            tbcCheckinForm.SelectedTab = tpCheckin;
            tpCheckin.Focus();
        }

        private void btnQuestsUpdate_Click(object sender, EventArgs e)
        {
            // Validate if a guest is selected
            if (selectedBookingID == -1)
            {
                MessageBox.Show("Please select a guest to update.");
                return;
            }

            // Ensure a guest is selected
            if (string.IsNullOrWhiteSpace(lblSelectedGuest.Text) || lblSelectedGuest.Text == "No valid guest selected.")
            {
                MessageBox.Show("Please ensure a valid guest is selected.");
                return;
            }

            // Validate if any changes were made
            if (!IsGuestDetailsModified())
            {
                MessageBox.Show("No changes detected.");
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
                            // Update guest details in the database
                            UpdateGuestDetailsInDatabase(selectedBookingID, connection, transaction);

                            // Update banking details
                            UpdateBankingDetailsForGuest(selectedBookingID, connection, transaction);

                            // Update the check-in and check-out dates
                            SqlCommand updateDatesCommand = new SqlCommand(
                                "UPDATE Guest_Booking SET CheckIn_Date = @CheckInDate, CheckOut_Date = @CheckOutDate WHERE Booking_ID = @BookingID",
                                connection, transaction);
                            updateDatesCommand.Parameters.AddWithValue("@CheckInDate", dtpCheckInDate.Value);
                            updateDatesCommand.Parameters.AddWithValue("@CheckOutDate", dtpCheckOutDate.Value);
                            updateDatesCommand.Parameters.AddWithValue("@BookingID", selectedBookingID);
                            updateDatesCommand.ExecuteNonQuery();

                            // Commit transaction
                            transaction.Commit();
                            MessageBox.Show("Guest information updated successfully!");

                            // Refresh DataGridViews
                            LoadBookedGuests();
                            LoadCheckedInGuests();

                            // Clear controls and switch back to the overview tab
                            ClearCheckInControls();
                            tbcCheckinForm.SelectedTab = tpOverview;
                            tpCheckin.Text = "Check in";

                            // Show the "Check-In" button and hide the "Update" button
                            btnCheckInCheckedIn.Visible = true;
                            btnQuestsUpdate.Visible = false;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show("An error occurred while updating guest information: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while connecting to the database: " + ex.Message);
            }
        }


        private void UpdateBankingDetailsForGuest(int bookingID, SqlConnection connection, SqlTransaction transaction)
        {
            string query = @"
        UPDATE BankingDetails
        SET 
            Card_Type = @CardType, 
            Bank = @Bank, 
            Card_Num = @CardNum, 
            Debit_Credit = @DebitCredit, 
            Card_Holder = @CardHolder, 
            Expiration_Date = @ExpirationDate
        WHERE 
            Banking_ID = (SELECT Banking_ID FROM Guest_Booking WHERE Booking_ID = @BookingID)";

            SqlCommand command = new SqlCommand(query, connection, transaction);

            command.Parameters.AddWithValue("@CardType", cbCardType.SelectedItem.ToString());
            command.Parameters.AddWithValue("@Bank", cbBankType.SelectedItem.ToString());
            command.Parameters.AddWithValue("@CardNum", txtCardNumber.Text);
            command.Parameters.AddWithValue("@DebitCredit", radDebit.Checked ? 0 : 1);
            command.Parameters.AddWithValue("@CardHolder", txtCardHolder.Text);
            command.Parameters.AddWithValue("@ExpirationDate", new DateTime(int.Parse(cbYear.SelectedItem.ToString()), int.Parse(cbMonth.SelectedItem.ToString()), 1));
            command.Parameters.AddWithValue("@BookingID", bookingID);

            command.ExecuteNonQuery();
        }

        private void LockCheckInControls()
        {
            // Lock the controls related to guest check-in (Booked Guests)
            txtCheckIDNum.Enabled = false;
            txtCheckinName.Enabled = false;
            txtCheckinSurname.Enabled = false;
            txtCheckinContactNum.Enabled = false;
            txtCheckinEmail.Enabled = false;
            txtCheckinStreet.Enabled = false;
            txtCheckinCity.Enabled = false;
            lblCheckInRSelected.Enabled = false;

            // Lock other check-in controls
            dtpCheckInDate.Enabled = false;
            dtpCheckOutDate.Enabled = false;
            cbBankType.Enabled = false;
            txtCardNumber.Enabled = false;
            cbCardType.Enabled = false;
            cbMonth.Enabled = false;
            cbYear.Enabled = false;
            radDebit.Enabled = false;
            radCredit.Enabled = false;

            // Hide the "Check-In" button and show the "Update" button
            btnCheckInCheckedIn.Visible = false;
            btnQuestsUpdate.Visible = true;
        }

        private void LockCheckedInControls()
        {
            // Lock the controls related to checked-in guests
            txtCheckIDNum.Enabled = false;
            txtCheckinName.Enabled = false;
            txtCheckinSurname.Enabled = false;
            txtCheckinContactNum.Enabled = false;
            txtCheckinEmail.Enabled = false;
            txtCheckinStreet.Enabled = false;
            txtCheckinCity.Enabled = false;
            lblCheckInRSelected.Enabled = false;

            // Lock other check-in controls
            dtpCheckInDate.Enabled = false;
            dtpCheckOutDate.Enabled = false;
            cbBankType.Enabled = false;
            txtCardNumber.Enabled = false;
            cbCardType.Enabled = false;
            cbMonth.Enabled = false;
            cbYear.Enabled = false;
            radDebit.Enabled = false;
            radCredit.Enabled = false;

            // Hide the "Check-In" button and show the "Update" button
            btnCheckInCheckedIn.Visible = false;
            btnQuestsUpdate.Visible = true;
        }



        private void btnBookOut_Click_1(object sender, EventArgs e)
        {
            int bookingID = GetSelectedBookingID();

            if (bookingID == -1)
            {
                MessageBox.Show("Please select a booking to book out.");
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
                            // Step 1: Delete the banking details related to the booking
                            SqlCommand deleteBankingCommand = new SqlCommand(
                                @"DELETE FROM BankingDetails 
                  WHERE Banking_ID = (SELECT Banking_ID FROM Guest_Booking WHERE Booking_ID = @BookingID)",
                                connection, transaction);
                            deleteBankingCommand.Parameters.AddWithValue("@BookingID", bookingID);
                            deleteBankingCommand.ExecuteNonQuery();

                            // Step 2: Delete the booking from the Guest_Booking table
                            SqlCommand deleteBookingCommand = new SqlCommand(
                                "DELETE FROM Guest_Booking WHERE Booking_ID = @BookingID",
                                connection, transaction);
                            deleteBookingCommand.Parameters.AddWithValue("@BookingID", bookingID);
                            deleteBookingCommand.ExecuteNonQuery();

                            // Step 3: Delete the guest's address
                            SqlCommand deleteAddressCommand = new SqlCommand(
                                @"DELETE FROM Address 
                  WHERE Address_ID = (SELECT Address_ID FROM Guests WHERE Guest_ID = (SELECT Guest_ID FROM Guest_Booking WHERE Booking_ID = @BookingID))",
                                connection, transaction);
                            deleteAddressCommand.Parameters.AddWithValue("@BookingID", bookingID);
                            deleteAddressCommand.ExecuteNonQuery();

                            // Step 4: Delete the guest from the Guests table
                            SqlCommand deleteGuestCommand = new SqlCommand(
                                "DELETE FROM Guests WHERE Guest_ID = (SELECT Guest_ID FROM Guest_Booking WHERE Booking_ID = @BookingID)",
                                connection, transaction);
                            deleteGuestCommand.Parameters.AddWithValue("@BookingID", bookingID);
                            deleteGuestCommand.ExecuteNonQuery();

                            // Commit the transaction
                            transaction.Commit();
                            MessageBox.Show("Booking and related guest information successfully deleted.");

                            // Refresh the DataGridViews
                            LoadBookedGuests();
                            LoadCheckedInGuests();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show("An error occurred while deleting the booking: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while connecting to the database: " + ex.Message);
            }
        }



        private void dgvCheckedBanking_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked cell is valid and contains a Banking_ID value
            if (e.RowIndex >= 0 && dgvCheckedBanking.Rows[e.RowIndex].Cells["Banking_ID"].Value != DBNull.Value)
            {
                // Do nothing; this just prevents errors when clicking on an empty cell
            }
        }

        private void tbcCheckinForm_TabIndexChanged(object sender, EventArgs e)
        {
            if (tbcCheckinForm.SelectedTab == tpCheckin)
            {
                if (dgvCheckBooking.SelectedRows.Count > 0 || dgvCheckBooking.CurrentRow != null)
                {
                    LockCheckInControls();
                    gbBookedButtons.Enabled = true;
                }
                else if (dgvCheckedCheckin.SelectedRows.Count > 0 || dgvCheckedCheckin.CurrentRow != null)
                {
                    LockCheckedInControls();
                    gbBookedButtons.Enabled = false;
                }
                else
                {
                    UnlockCheckInControls();
                    gbBookedButtons.Enabled = false;
                }
            }
            else if (tbcCheckinForm.SelectedTab == tpOverview)
            {
                UnlockCheckInControls();
            }
        }

    }
}