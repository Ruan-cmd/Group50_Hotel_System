using Group_50_CMPG223_HotelManagementSystem;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Group50_Hotel_System
{
    public partial class Booking_Form : Form
    {
        private bool isUpdateMode = false;
        private int selectedBookingID;
        private int existingGuestID = 0;

        public Booking_Form()
        {
            InitializeComponent();
            LoadRooms(DateTime.Now, DateTime.Now.AddDays(1));
            LoadGuestsNotCheckedIn();
            btnQuestUpdate.Visible = false;
        }

        private void ClearBookingForm()
        {
            txtBookingName.Clear();
            txtBookingSurname.Clear();
            txtBookingContactNum.Clear();
            txtBookingEmail.Clear();
            txtBookingIDnum.Clear();
            txtBookingStreet.Clear();
            txtBookingCity.Clear();

            lblBookingRSelected.Text = "No Room Selected!";

            dtpBookInDate.Value = DateTime.Now;
            dtpBookOutDate.Value = DateTime.Now;
        }

        private void LoadGuestsNotCheckedIn(string sortOrder = "Asc")
        {
            using (SqlConnection connection = new SqlConnection(SessionManager.ConnectionString))
            {
                connection.Open();

                string query = @"
                    SELECT 
                        gb.Booking_ID,
                        g.First_Name,
                        g.Last_Name,
                        g.Contact_Num,
                        g.Email,
                        g.ID_Number,
                        r.Room_Number,
                        gb.CheckIn_Date,
                        gb.CheckOut_Date,
                        a.Street_Name AS Street,
                        a.Town_City AS City
                    FROM 
                        Guest_Booking gb
                    JOIN 
                        Guests g ON gb.Guest_ID = g.Guest_ID
                    JOIN 
                        Rooms r ON gb.Room_ID = r.Room_ID
                    LEFT JOIN 
                        Address a ON g.Address_ID = a.Address_ID
                    WHERE 
                        gb.Is_CheckedIn = 0";

                if (sortOrder == "Asc")
                {
                    query += " ORDER BY gb.CheckIn_Date ASC";
                }
                else if (sortOrder == "Desc")
                {
                    query += " ORDER BY gb.CheckIn_Date DESC";
                }

                SqlDataAdapter da = new SqlDataAdapter(query, connection);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dtvBookingOverview.DataSource = dt;
            }
        }

        private void LoadRooms(DateTime checkInDate, DateTime checkOutDate)
        {
            using (SqlConnection connection = new SqlConnection(SessionManager.ConnectionString))
            {
                connection.Open();

                string query = @"
                    SELECT r.Room_Number, r.Room_Type, r.Room_Status
                    FROM Rooms r
                    WHERE r.Room_ID NOT IN (
                        SELECT gb.Room_ID
                        FROM Guest_Booking gb
                        WHERE @CheckInDate < gb.CheckOut_Date AND @CheckOutDate > gb.CheckIn_Date
                    )";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CheckInDate", checkInDate);
                command.Parameters.AddWithValue("@CheckOutDate", checkOutDate);

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvBookingRooms.DataSource = dt;
            }
        }

        private void btnBookingBookIn_Click(object sender, EventArgs e)
        {
            if (lblBookingRSelected.Text == "No Room Selected!")
            {
                MessageBox.Show("Please select a room before booking.", "Room Not Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtBookingIDnum.Text.Length != 13 || !long.TryParse(txtBookingIDnum.Text, out _))
            {
                MessageBox.Show("The ID number must be exactly 13 digits.", "Invalid ID Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection connection = new SqlConnection(SessionManager.ConnectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    int guestID = 0; // Initialize guestID

                    // Check if the guest exists in the database
                    string checkGuestQuery = "SELECT Guest_ID FROM Guests WHERE ID_Number = @IDNumber";
                    SqlCommand checkGuestCommand = new SqlCommand(checkGuestQuery, connection, transaction);
                    checkGuestCommand.Parameters.AddWithValue("@IDNumber", txtBookingIDnum.Text);

                    object result = checkGuestCommand.ExecuteScalar();
                    if (result != null)
                    {
                        guestID = Convert.ToInt32(result); // Guest exists, use existing ID
                    }
                    else
                    {
                        // Guest doesn't exist, so create a new guest record
                        int addressID = 0;

                        // Check if the address exists in the database
                        string addressQuery = @"SELECT Address_ID FROM Address WHERE Street_Name = @Street AND Town_City = @City";
                        SqlCommand addressCommand = new SqlCommand(addressQuery, connection, transaction);
                        addressCommand.Parameters.AddWithValue("@Street", txtBookingStreet.Text);
                        addressCommand.Parameters.AddWithValue("@City", txtBookingCity.Text);

                        object addressResult = addressCommand.ExecuteScalar();

                        if (addressResult != null)
                        {
                            addressID = Convert.ToInt32(addressResult);
                        }
                        else
                        {
                            // Insert a new address if it doesn't exist
                            string insertAddressQuery = @"INSERT INTO Address (Street_Name, Town_City) VALUES (@Street, @City); SELECT SCOPE_IDENTITY();";
                            SqlCommand insertAddressCommand = new SqlCommand(insertAddressQuery, connection, transaction);
                            insertAddressCommand.Parameters.AddWithValue("@Street", txtBookingStreet.Text);
                            insertAddressCommand.Parameters.AddWithValue("@City", txtBookingCity.Text);

                            addressID = Convert.ToInt32(insertAddressCommand.ExecuteScalar());
                        }

                        // Insert the new guest
                        string insertGuestQuery = @"INSERT INTO Guests (First_Name, Last_Name, Contact_Num, Email, ID_Number, Address_ID) VALUES (@FirstName, @LastName, @ContactNum, @Email, @IDNumber, @AddressID); SELECT SCOPE_IDENTITY();";
                        SqlCommand guestCommand = new SqlCommand(insertGuestQuery, connection, transaction);
                        guestCommand.Parameters.AddWithValue("@FirstName", txtBookingName.Text);
                        guestCommand.Parameters.AddWithValue("@LastName", txtBookingSurname.Text);
                        guestCommand.Parameters.AddWithValue("@ContactNum", txtBookingContactNum.Text);
                        guestCommand.Parameters.AddWithValue("@Email", txtBookingEmail.Text);
                        guestCommand.Parameters.AddWithValue("@IDNumber", txtBookingIDnum.Text);
                        guestCommand.Parameters.AddWithValue("@AddressID", addressID);

                        guestID = Convert.ToInt32(guestCommand.ExecuteScalar());
                    }

                    // Proceed with the booking
                    string roomNumber = lblBookingRSelected.Text.Replace("Room Selected: ", "");
                    string getRoomIDQuery = "SELECT Room_ID FROM Rooms WHERE Room_Number = @RoomNumber";
                    SqlCommand getRoomIDCommand = new SqlCommand(getRoomIDQuery, connection, transaction);
                    getRoomIDCommand.Parameters.AddWithValue("@RoomNumber", roomNumber);

                    int roomID = Convert.ToInt32(getRoomIDCommand.ExecuteScalar());

                    DateTime checkInDate = dtpBookInDate.Value;
                    DateTime checkOutDate = dtpBookOutDate.Value;

                    if (CheckRoomAvailability(roomID, checkInDate, checkOutDate))
                    {
                        string insertBookingQuery = @"INSERT INTO Guest_Booking (Room_ID, Guest_ID, Employee_ID, CheckIn_Date, CheckOut_Date, Is_CheckedIn) VALUES (@RoomID, @GuestID, @EmployeeID, @CheckInDate, @CheckOutDate, 0)";
                        SqlCommand bookingCommand = new SqlCommand(insertBookingQuery, connection, transaction);
                        bookingCommand.Parameters.AddWithValue("@RoomID", roomID);
                        bookingCommand.Parameters.AddWithValue("@GuestID", guestID);
                        bookingCommand.Parameters.AddWithValue("@EmployeeID", SessionManager.LoggedInEmployeeID);
                        bookingCommand.Parameters.AddWithValue("@CheckInDate", checkInDate);
                        bookingCommand.Parameters.AddWithValue("@CheckOutDate", checkOutDate);

                        bookingCommand.ExecuteNonQuery();

                        transaction.Commit();

                        MessageBox.Show("Guest booked successfully!");
                        LoadGuestsNotCheckedIn();
                        ClearBookingForm();
                        LoadRooms(DateTime.Now, DateTime.Now.AddDays(1));
                    }
                    else
                    {
                        MessageBox.Show("The selected room is not available for the chosen dates. Please choose a different room or modify the dates.");
                        transaction.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("An error occurred while booking the guest: " + ex.Message);
                }
            }
        }


        private bool CheckForChanges(int bookingID)
        {
            using (SqlConnection connection = new SqlConnection(SessionManager.ConnectionString))
            {
                connection.Open();

                string query = @"
                    SELECT 
                        g.First_Name,
                        g.Last_Name,
                        g.Contact_Num,
                        g.Email,
                        g.ID_Number,
                        a.Street_Name AS Street,
                        a.Town_City AS City,
                        gb.CheckIn_Date,
                        gb.CheckOut_Date,
                        r.Room_Number
                    FROM 
                        Guest_Booking gb
                    JOIN 
                        Guests g ON gb.Guest_ID = g.Guest_ID
                    JOIN 
                        Rooms r ON gb.Room_ID = r.Room_ID
                    LEFT JOIN 
                        Address a ON g.Address_ID = a.Address_ID
                    WHERE 
                        gb.Booking_ID = @BookingID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BookingID", bookingID);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (txtBookingName.Text != reader["First_Name"].ToString() ||
                            txtBookingSurname.Text != reader["Last_Name"].ToString() ||
                            txtBookingContactNum.Text != reader["Contact_Num"].ToString() ||
                            txtBookingEmail.Text != reader["Email"].ToString() ||
                            txtBookingIDnum.Text != reader["ID_Number"].ToString() ||
                            txtBookingStreet.Text != reader["Street"].ToString() ||
                            txtBookingCity.Text != reader["City"].ToString() ||
                            dtpBookInDate.Value != Convert.ToDateTime(reader["CheckIn_Date"]) ||
                            dtpBookOutDate.Value != Convert.ToDateTime(reader["CheckOut_Date"]) ||
                            lblBookingRSelected.Text.Replace("Room Selected: ", "") != reader["Room_Number"].ToString())
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void UpdateBookingDetails(int bookingID)
        {
            using (SqlConnection connection = new SqlConnection(SessionManager.ConnectionString))
            {
                connection.Open();

                string updateGuestQuery = @"
                    UPDATE Guests SET 
                        First_Name = @FirstName,
                        Last_Name = @LastName,
                        Contact_Num = @ContactNum,
                        Email = @Email,
                        ID_Number = @IDNumber,
                        Address_ID = @AddressID
                    WHERE Guest_ID = (SELECT Guest_ID FROM Guest_Booking WHERE Booking_ID = @BookingID)";

                SqlCommand updateGuestCommand = new SqlCommand(updateGuestQuery, connection);
                updateGuestCommand.Parameters.AddWithValue("@FirstName", txtBookingName.Text);
                updateGuestCommand.Parameters.AddWithValue("@LastName", txtBookingSurname.Text);
                updateGuestCommand.Parameters.AddWithValue("@ContactNum", txtBookingContactNum.Text);
                updateGuestCommand.Parameters.AddWithValue("@Email", txtBookingEmail.Text);
                updateGuestCommand.Parameters.AddWithValue("@IDNumber", txtBookingIDnum.Text);
                updateGuestCommand.Parameters.AddWithValue("@AddressID", DBNull.Value);
                updateGuestCommand.Parameters.AddWithValue("@BookingID", bookingID);
                updateGuestCommand.ExecuteNonQuery();

                string updateBookingQuery = @"
                    UPDATE Guest_Booking SET 
                        CheckIn_Date = @CheckInDate,
                        CheckOut_Date = @CheckOutDate,
                        Room_ID = (SELECT Room_ID FROM Rooms WHERE Room_Number = @RoomNumber)
                    WHERE Booking_ID = @BookingID";

                SqlCommand updateBookingCommand = new SqlCommand(updateBookingQuery, connection);
                updateBookingCommand.Parameters.AddWithValue("@CheckInDate", dtpBookInDate.Value);
                updateBookingCommand.Parameters.AddWithValue("@CheckOutDate", dtpBookOutDate.Value);
                updateBookingCommand.Parameters.AddWithValue("@RoomNumber", lblBookingRSelected.Text.Replace("Room Selected: ", ""));
                updateBookingCommand.Parameters.AddWithValue("@BookingID", bookingID);
                updateBookingCommand.ExecuteNonQuery();
            }
        }

        private void ResetToOverview()
        {
            tpOverview.Enabled = true;
            tabControlBooking.SelectedTab = tpOverview;
            ClearBookingForm();
            isUpdateMode = false;
            LoadGuestsNotCheckedIn();
        }

        private bool CheckRoomAvailability(int roomID, DateTime checkInDate, DateTime checkOutDate)
        {
            using (SqlConnection connection = new SqlConnection(SessionManager.ConnectionString))
            {
                connection.Open();

                string query = @"
                    SELECT COUNT(*)
                    FROM Guest_Booking
                    WHERE Room_ID = @RoomID AND @CheckInDate < CheckOut_Date AND @CheckOutDate > CheckIn_Date";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RoomID", roomID);
                command.Parameters.AddWithValue("@CheckInDate", checkInDate);
                command.Parameters.AddWithValue("@CheckOutDate", checkOutDate);

                int count = Convert.ToInt32(command.ExecuteScalar());

                return count == 0;
            }
        }

        private void dgvBookingRooms_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvBookingRooms.Rows[e.RowIndex];
                string roomNumber = row.Cells["Room_Number"].Value.ToString();
                lblBookingRSelected.Text = $"Room Selected: {roomNumber}";
            }
        }

        private void btnBookingClearControls_Click(object sender, EventArgs e)
        {
            ClearBookingForm();
        }

        private void dtvBookingOverview_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dtvBookingOverview.Rows[e.RowIndex];

                if (selectedRow.Cells["Booking_ID"].Value != DBNull.Value)
                {
                    selectedBookingID = Convert.ToInt32(selectedRow.Cells["Booking_ID"].Value);

                    string guestName = $"{selectedRow.Cells["First_Name"].Value} {selectedRow.Cells["Last_Name"].Value}";
                    lblSelectedGuest.Text = $"Selected Guest: {guestName}";
                }
                else
                {
                    selectedBookingID = 0;
                    lblSelectedGuest.Text = "No valid guest selected.";
                }
            }
        }

        private void txtBookingIDnum_TextChanged_1(object sender, EventArgs e)
        {
            // Get the ID number entered by the user
            string idNumber = txtBookingIDnum.Text.Trim();

            // Only proceed if the ID number is exactly 13 digits long
            if (idNumber.Length == 13 && long.TryParse(idNumber, out _))
            {
                // Call the method to check the database and retrieve guest information
                RetrieveAndFillGuestInformation(idNumber);
            }
        }

        private void RetrieveAndFillGuestInformation(string idNumber)
        {
            try
            {
                // Example connection string (use your actual connection string)
                using (SqlConnection conn = new SqlConnection(SessionManager.ConnectionString))
                {
                    conn.Open();

                    // SQL query to find a guest with the entered ID number
                    string query = "SELECT First_Name, Last_Name, Contact_Num, Email, Address_ID FROM Guests WHERE ID_Number = @IDNumber";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IDNumber", idNumber);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Fill in the guest's details if a match is found
                                txtBookingName.Text = reader["First_Name"].ToString();
                                txtBookingSurname.Text = reader["Last_Name"].ToString();
                                txtBookingContactNum.Text = reader["Contact_Num"].ToString();
                                txtBookingEmail.Text = reader["Email"].ToString();

                                // You may also need to retrieve the address based on the Address_ID
                                int addressId = Convert.ToInt32(reader["Address_ID"]);
                                RetrieveAndFillGuestAddress(addressId);
                            }
                            else
                            {
                                // Clear fields if no guest is found
                                ClearGuestFields();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while retrieving guest information: {ex.Message}");
            }
        }

        private void RetrieveAndFillGuestAddress(int addressId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(SessionManager.ConnectionString))
                {
                    conn.Open();

                    // SQL query to find the address based on the Address_ID
                    string query = "SELECT Street_Name, Town_City FROM Address WHERE Address_ID = @AddressID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@AddressID", addressId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Fill in the guest's address details
                                txtBookingStreet.Text = reader["Street_Name"].ToString();
                                txtBookingCity.Text = reader["Town_City"].ToString();
                            }
                            else
                            {
                                // Clear address fields if no address is found
                                txtBookingStreet.Text = string.Empty;
                                txtBookingCity.Text = string.Empty;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while retrieving address information: {ex.Message}");
            }
        }

        private void ClearGuestFields()
        {
            // Clear all the text fields related to guest information
            txtBookingName.Text = string.Empty;
            txtBookingSurname.Text = string.Empty;
            txtBookingContactNum.Text = string.Empty;
            txtBookingEmail.Text = string.Empty;
            txtBookingStreet.Text = string.Empty;
            txtBookingCity.Text = string.Empty;
        }


        private void removeDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to clear the entire database including all tables? This action cannot be undone.", "Confirm Database Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                using (SqlConnection connection = new SqlConnection(SessionManager.ConnectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // Clear Guest_Booking table
                        string clearGuestBookingQuery = "DELETE FROM Guest_Booking";
                        SqlCommand clearGuestBookingCommand = new SqlCommand(clearGuestBookingQuery, connection, transaction);
                        clearGuestBookingCommand.ExecuteNonQuery();

                        // Clear BankingDetails table
                        string clearBankingDetailsQuery = "DELETE FROM BankingDetails";
                        SqlCommand clearBankingDetailsCommand = new SqlCommand(clearBankingDetailsQuery, connection, transaction);
                        clearBankingDetailsCommand.ExecuteNonQuery();

                        // Clear Guests table
                        string clearGuestsQuery = "DELETE FROM Guests";
                        SqlCommand clearGuestsCommand = new SqlCommand(clearGuestsQuery, connection, transaction);
                        clearGuestsCommand.ExecuteNonQuery();

                        // Clear Address table
                        string clearAddressQuery = "DELETE FROM Address";
                        SqlCommand clearAddressCommand = new SqlCommand(clearAddressQuery, connection, transaction);
                        clearAddressCommand.ExecuteNonQuery();

                        // Clear Employees table except for the default employee
                        string clearEmployeesQuery = "DELETE FROM Employees WHERE Username <> 'Default'";
                        SqlCommand clearEmployeesCommand = new SqlCommand(clearEmployeesQuery, connection, transaction);
                        clearEmployeesCommand.ExecuteNonQuery();

                        // Clear Rooms table
                        string clearRoomsQuery = "DELETE FROM Rooms";
                        SqlCommand clearRoomsCommand = new SqlCommand(clearRoomsQuery, connection, transaction);
                        clearRoomsCommand.ExecuteNonQuery();

                        // Reset Room_Status in Rooms table (if Rooms are not deleted and just reset status)
                        string updateRoomsStatusQuery = "UPDATE Rooms SET Room_Status = 0";
                        SqlCommand updateRoomsStatusCommand = new SqlCommand(updateRoomsStatusQuery, connection, transaction);
                        updateRoomsStatusCommand.ExecuteNonQuery();

                        // Clear Roles table except for the default role
                        string clearRolesQuery = "DELETE FROM Roles WHERE Role_ID <> 1";
                        SqlCommand clearRolesCommand = new SqlCommand(clearRolesQuery, connection, transaction);
                        clearRolesCommand.ExecuteNonQuery();

                        // Commit the transaction
                        transaction.Commit();

                        MessageBox.Show("Database cleared successfully! All data has been removed.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("An error occurred while clearing the database: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Database clear operation canceled.");
            }
        }

        private void dtpBookInDate_ValueChanged(object sender, EventArgs e)
        {
            LoadRooms(dtpBookInDate.Value, dtpBookOutDate.Value);
        }

        private void dtpBookOutDate_ValueChanged(object sender, EventArgs e)
        {
            LoadRooms(dtpBookInDate.Value, dtpBookOutDate.Value);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Main_Form main_Form = new Main_Form();
            main_Form.ShowDialog();
            this.Close();
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (radBookingAsc.Checked)
            {
                ApplyBookingFilter("Asc");
            }
            else if (radBookingDesc.Checked)
            {
                ApplyBookingFilter("Desc");
            }
        }

        private void ApplyBookingFilter(string sortOrder)
        {
            LoadGuestsNotCheckedIn(sortOrder);
        }

        private void btnRemoveBooking_Click(object sender, EventArgs e)
        {
            if (selectedBookingID != 0)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this guest's booking?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    using (SqlConnection connection = new SqlConnection(SessionManager.ConnectionString))
                    {
                        connection.Open();

                        string getRoomIDQuery = "SELECT Room_ID FROM Guest_Booking WHERE Booking_ID = @BookingID";
                        SqlCommand getRoomIDCommand = new SqlCommand(getRoomIDQuery, connection);
                        getRoomIDCommand.Parameters.AddWithValue("@BookingID", selectedBookingID);
                        int roomID = Convert.ToInt32(getRoomIDCommand.ExecuteScalar());

                        string deleteBookingQuery = "DELETE FROM Guest_Booking WHERE Booking_ID = @BookingID";
                        SqlCommand deleteCommand = new SqlCommand(deleteBookingQuery, connection);
                        deleteCommand.Parameters.AddWithValue("@BookingID", selectedBookingID);
                        deleteCommand.ExecuteNonQuery();

                        string updateRoomStatusQuery = "UPDATE Rooms SET Room_Status = 0 WHERE Room_ID = @RoomID";
                        SqlCommand updateRoomStatusCommand = new SqlCommand(updateRoomStatusQuery, connection);
                        updateRoomStatusCommand.Parameters.AddWithValue("@RoomID", roomID);
                        updateRoomStatusCommand.ExecuteNonQuery();

                        string reorderPKQuery = @"
                            DECLARE @MaxID INT;
                            SELECT @MaxID = ISNULL(MAX(Booking_ID), 0) FROM Guest_Booking;
                            DBCC CHECKIDENT ('Guest_Booking', RESEED, @MaxID)";
                        SqlCommand reorderPKCommand = new SqlCommand(reorderPKQuery, connection);
                        reorderPKCommand.ExecuteNonQuery();

                        MessageBox.Show("Guest booking deleted successfully!");

                        LoadGuestsNotCheckedIn();
                        LoadRooms(DateTime.Now, DateTime.Now.AddDays(1));

                        selectedBookingID = 0;
                        lblSelectedGuest.Text = "No Guest Selected";
                    }
                }
                else
                {
                    MessageBox.Show("Guest booking deletion canceled.");
                }
            }
            else
            {
                MessageBox.Show("Please select a guest booking to delete.");
            }
        }

        private void btnUpdateBooking_Click(object sender, EventArgs e)
        {
            if (selectedBookingID != 0)
            {
                tabControlBooking.SelectedTab = tpAddBooking;
                btnBookingBookIn.Visible = false;
                btnQuestUpdate.Visible = true;
                tpOverview.Enabled = false;
                isUpdateMode = true;
                LoadSelectedGuestDetails(selectedBookingID);
            }
            else
            {
                MessageBox.Show("Please select a guest to update.", "No Guest Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadSelectedGuestDetails(int bookingID)
        {
            using (SqlConnection connection = new SqlConnection(SessionManager.ConnectionString))
            {
                connection.Open();

                string query = @"
                    SELECT 
                        g.First_Name,
                        g.Last_Name,
                        g.Contact_Num,
                        g.Email,
                        g.ID_Number,
                        g.Address_ID,
                        a.Street_Name AS Street,
                        a.Town_City AS City,
                        gb.CheckIn_Date,
                        gb.CheckOut_Date,
                        r.Room_Number
                    FROM 
                        Guest_Booking gb
                    JOIN 
                        Guests g ON gb.Guest_ID = g.Guest_ID
                    JOIN 
                        Rooms r ON gb.Room_ID = r.Room_ID
                    LEFT JOIN 
                        Address a ON g.Address_ID = a.Address_ID
                    WHERE 
                        gb.Booking_ID = @BookingID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BookingID", bookingID);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Prefill the form fields with the guest's details
                        txtBookingName.Text = reader["First_Name"].ToString();
                        txtBookingSurname.Text = reader["Last_Name"].ToString();
                        txtBookingContactNum.Text = reader["Contact_Num"].ToString();
                        txtBookingEmail.Text = reader["Email"].ToString();
                        txtBookingIDnum.Text = reader["ID_Number"].ToString();

                        // Load Address if available
                        if (reader["Address_ID"] != DBNull.Value)
                        {
                            txtBookingStreet.Text = reader["Street"].ToString();
                            txtBookingCity.Text = reader["City"].ToString();
                        }
                        else
                        {
                            txtBookingStreet.Clear();
                            txtBookingCity.Clear();
                        }

                        dtpBookInDate.Value = Convert.ToDateTime(reader["CheckIn_Date"]);
                        dtpBookOutDate.Value = Convert.ToDateTime(reader["CheckOut_Date"]);
                        lblBookingRSelected.Text = $"Room Selected: {reader["Room_Number"].ToString()}";
                    }
                    else
                    {
                        MessageBox.Show("No guest details found for the selected booking.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        // Event handler for the btnQuestUpdate button click
        private void btnQuestUpdate_Click(object sender, EventArgs e)
        {
            // Retrieve the updated guest information from the text boxes
            string guestIDNumber = txtBookingIDnum.Text;
            string guestName = txtBookingName.Text;
            string guestSurname = txtBookingSurname.Text;
            string guestContactNumber = txtBookingContactNum.Text;
            string guestEmail = txtBookingEmail.Text;
            string guestStreet = txtBookingStreet.Text;
            string guestCity = txtBookingCity.Text;

            // Call the method to update the guest information in the database
            bool success = UpdateGuestInfo(guestIDNumber, guestName, guestSurname, guestContactNumber, guestEmail, guestStreet, guestCity);

            // Provide feedback to the user based on the result
            if (success)
            {
                MessageBox.Show("Guest information updated successfully!", "Update Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Optionally, refresh the DataGridView to show updated information
                LoadBookingOverview();
            }
            else
            {
                MessageBox.Show("Failed to update guest information.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Method to update guest information in the database
        private bool UpdateGuestInfo(string idNumber, string name, string surname, string contactNumber, string email, string street, string city)
        {
            try
            {
                // Assume you're using ADO.NET for database operations
                using (SqlConnection conn = new SqlConnection(SessionManager.ConnectionString))
                {
                    conn.Open();

                    // SQL query to update guest information
                    string query = "UPDATE Guests SET First_Name = @FirstName, Last_Name = @LastName, Contact_Num = @ContactNumber, " +
                                   "Email = @Email, Street_Name = @Street, Town_City = @City WHERE ID_Number = @IDNumber";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Add parameters to the SQL query
                        cmd.Parameters.AddWithValue("@FirstName", name);
                        cmd.Parameters.AddWithValue("@LastName", surname);
                        cmd.Parameters.AddWithValue("@ContactNumber", contactNumber);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Street", street);
                        cmd.Parameters.AddWithValue("@City", city);
                        cmd.Parameters.AddWithValue("@IDNumber", idNumber);

                        // Execute the query and check if any rows were affected
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Return true if at least one row was updated
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occurred during the update
                MessageBox.Show("An error occurred while updating the guest information: " + ex.Message, "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Method to load the booking overview (refresh the DataGridView)
        private void LoadBookingOverview()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(SessionManager.ConnectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Guest_Booking"; // Example query to fetch bookings
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dtvBookingOverview.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading the booking overview: " + ex.Message, "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}