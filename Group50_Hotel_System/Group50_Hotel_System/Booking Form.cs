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

        private void LoadGuestsNotCheckedIn(string sortOrder = "Asc", string searchTerm = "")
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
                gb.Is_CheckedIn = 0
                AND (
                    g.First_Name LIKE @SearchTerm
                    OR g.Last_Name LIKE @SearchTerm
                    OR g.Contact_Num LIKE @SearchTerm
                    OR g.Email LIKE @SearchTerm
                    OR g.ID_Number LIKE @SearchTerm
                )";

                if (sortOrder == "Asc")
                {
                    query += " ORDER BY gb.CheckIn_Date ASC";
                }
                else if (sortOrder == "Desc")
                {
                    query += " ORDER BY gb.CheckIn_Date DESC";
                }

                SqlDataAdapter da = new SqlDataAdapter(query, connection);
                da.SelectCommand.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
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

        private bool IsGuestCurrentlyBookedOrCheckedIn(string idNumber)
        {
            using (SqlConnection connection = new SqlConnection(SessionManager.ConnectionString))
            {
                connection.Open();
                string query = @"
            SELECT COUNT(*) 
            FROM Guest_Booking gb
            INNER JOIN Guests g ON gb.Guest_ID = g.Guest_ID
            WHERE g.ID_Number = @IDNumber AND (gb.Is_CheckedIn = 1 OR gb.Is_CheckedOut = 0)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IDNumber", idNumber);

                int count = (int)command.ExecuteScalar();
                return count > 0;
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

            if (IsGuestCurrentlyBookedOrCheckedIn(txtBookingIDnum.Text))
            {
                MessageBox.Show("This guest is either currently booked in or checked in and cannot be added again.", "Invalid Operation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection connection = new SqlConnection(SessionManager.ConnectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    int guestID = 0;
                    int addressID = 0;

                    string checkGuestQuery = "SELECT Guest_ID, Address_ID FROM Guests WHERE ID_Number = @IDNumber";
                    SqlCommand checkGuestCommand = new SqlCommand(checkGuestQuery, connection, transaction);
                    checkGuestCommand.Parameters.AddWithValue("@IDNumber", txtBookingIDnum.Text);

                    using (SqlDataReader reader = checkGuestCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            guestID = Convert.ToInt32(reader["Guest_ID"]);
                            addressID = reader["Address_ID"] != DBNull.Value ? Convert.ToInt32(reader["Address_ID"]) : 0;
                        }
                    }

                    if (guestID == 0)
                    {
                        if (addressID == 0)
                        {
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
                                string insertAddressQuery = @"INSERT INTO Address (Street_Name, Town_City) VALUES (@Street, @City); SELECT SCOPE_IDENTITY();";
                                SqlCommand insertAddressCommand = new SqlCommand(insertAddressQuery, connection, transaction);
                                insertAddressCommand.Parameters.AddWithValue("@Street", txtBookingStreet.Text);
                                insertAddressCommand.Parameters.AddWithValue("@City", txtBookingCity.Text);

                                addressID = Convert.ToInt32(insertAddressCommand.ExecuteScalar());
                            }
                        }
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
                    ResetToOverview();
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

                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    int addressID = 0;
                    string checkAddressQuery = @"
                SELECT Address_ID 
                FROM Address 
                WHERE Street_Name = @Street AND Town_City = @City";
                    SqlCommand checkAddressCommand = new SqlCommand(checkAddressQuery, connection, transaction);
                    checkAddressCommand.Parameters.AddWithValue("@Street", txtBookingStreet.Text);
                    checkAddressCommand.Parameters.AddWithValue("@City", txtBookingCity.Text);

                    object addressResult = checkAddressCommand.ExecuteScalar();

                    if (addressResult != null)
                    {
                        addressID = Convert.ToInt32(addressResult);
                    }
                    else
                    {
                        string insertAddressQuery = @"
                    INSERT INTO Address (Street_Name, Town_City) 
                    VALUES (@Street, @City); 
                    SELECT SCOPE_IDENTITY();";
                        SqlCommand insertAddressCommand = new SqlCommand(insertAddressQuery, connection, transaction);
                        insertAddressCommand.Parameters.AddWithValue("@Street", txtBookingStreet.Text);
                        insertAddressCommand.Parameters.AddWithValue("@City", txtBookingCity.Text);

                        addressID = Convert.ToInt32(insertAddressCommand.ExecuteScalar());
                    }
                    string updateGuestQuery = @"
                UPDATE Guests SET 
                    First_Name = @FirstName,
                    Last_Name = @LastName,
                    Contact_Num = @ContactNum,
                    Email = @Email,
                    ID_Number = @IDNumber,
                    Address_ID = @AddressID
                WHERE Guest_ID = (SELECT Guest_ID FROM Guest_Booking WHERE Booking_ID = @BookingID)";

                    SqlCommand updateGuestCommand = new SqlCommand(updateGuestQuery, connection, transaction);
                    updateGuestCommand.Parameters.AddWithValue("@FirstName", txtBookingName.Text);
                    updateGuestCommand.Parameters.AddWithValue("@LastName", txtBookingSurname.Text);
                    updateGuestCommand.Parameters.AddWithValue("@ContactNum", txtBookingContactNum.Text);
                    updateGuestCommand.Parameters.AddWithValue("@Email", txtBookingEmail.Text);
                    updateGuestCommand.Parameters.AddWithValue("@IDNumber", txtBookingIDnum.Text);
                    updateGuestCommand.Parameters.AddWithValue("@AddressID", addressID);
                    updateGuestCommand.Parameters.AddWithValue("@BookingID", bookingID);
                    updateGuestCommand.ExecuteNonQuery();

                    string updateBookingQuery = @"
                UPDATE Guest_Booking SET 
                    CheckIn_Date = @CheckInDate,
                    CheckOut_Date = @CheckOutDate,
                    Room_ID = (SELECT Room_ID FROM Rooms WHERE Room_Number = @RoomNumber)
                WHERE Booking_ID = @BookingID";

                    SqlCommand updateBookingCommand = new SqlCommand(updateBookingQuery, connection, transaction);
                    updateBookingCommand.Parameters.AddWithValue("@CheckInDate", dtpBookInDate.Value);
                    updateBookingCommand.Parameters.AddWithValue("@CheckOutDate", dtpBookOutDate.Value);
                    updateBookingCommand.Parameters.AddWithValue("@RoomNumber", lblBookingRSelected.Text.Replace("Room Selected: ", ""));
                    updateBookingCommand.Parameters.AddWithValue("@BookingID", bookingID);
                    updateBookingCommand.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show($"An error occurred while updating booking details: {ex.Message}");
                }
            }
        }

        private void ResetToOverview()
        {
            tpOverview.Enabled = true;
            tabControlBooking.SelectedTab = tpOverview;
            ClearBookingForm();
            isUpdateMode = false;
            LoadGuestsNotCheckedIn();
            tabControlBooking.TabPages[tpAddBooking.Name].Text = "Book In";
            btnBookingBookIn.Visible = true;
            btnQuestUpdate.Visible = false;
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
            string idNumber = txtBookingIDnum.Text.Trim();

            if (idNumber.Length == 13 && long.TryParse(idNumber, out _))
            {
                RetrieveAndFillGuestInformation(idNumber);
            }
        }

        private void RetrieveAndFillGuestInformation(string idNumber)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(SessionManager.ConnectionString))
                {
                    conn.Open();

                    string query = @"
            SELECT 
                g.First_Name, 
                g.Last_Name, 
                g.Contact_Num, 
                g.Email, 
                g.ID_Number, 
                a.Street_Name AS Street, 
                a.Town_City AS City 
            FROM 
                Guests g
            LEFT JOIN 
                Address a ON g.Address_ID = a.Address_ID
            WHERE 
                g.ID_Number = @IDNumber";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IDNumber", idNumber);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtBookingName.Text = reader["First_Name"] != DBNull.Value ? reader["First_Name"].ToString() : string.Empty;
                                txtBookingSurname.Text = reader["Last_Name"] != DBNull.Value ? reader["Last_Name"].ToString() : string.Empty;
                                txtBookingContactNum.Text = reader["Contact_Num"] != DBNull.Value ? reader["Contact_Num"].ToString() : string.Empty;
                                txtBookingEmail.Text = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : string.Empty;

                                txtBookingStreet.Text = reader["Street"] != DBNull.Value ? reader["Street"].ToString() : string.Empty;
                                txtBookingCity.Text = reader["City"] != DBNull.Value ? reader["City"].ToString() : string.Empty;
                            }
                            else
                            {
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





        private void ClearGuestFields()
        {
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
                        string clearGuestBookingQuery = "DELETE FROM Guest_Booking";
                        SqlCommand clearGuestBookingCommand = new SqlCommand(clearGuestBookingQuery, connection, transaction);
                        clearGuestBookingCommand.ExecuteNonQuery();

                        string clearBankingDetailsQuery = "DELETE FROM BankingDetails";
                        SqlCommand clearBankingDetailsCommand = new SqlCommand(clearBankingDetailsQuery, connection, transaction);
                        clearBankingDetailsCommand.ExecuteNonQuery();

                        string clearGuestsQuery = "DELETE FROM Guests";
                        SqlCommand clearGuestsCommand = new SqlCommand(clearGuestsQuery, connection, transaction);
                        clearGuestsCommand.ExecuteNonQuery();

                        string clearAddressQuery = "DELETE FROM Address";
                        SqlCommand clearAddressCommand = new SqlCommand(clearAddressQuery, connection, transaction);
                        clearAddressCommand.ExecuteNonQuery();

                        string clearEmployeesQuery = "DELETE FROM Employees WHERE Username <> 'Default'";
                        SqlCommand clearEmployeesCommand = new SqlCommand(clearEmployeesQuery, connection, transaction);
                        clearEmployeesCommand.ExecuteNonQuery();

                        string clearRoomsQuery = "DELETE FROM Rooms";
                        SqlCommand clearRoomsCommand = new SqlCommand(clearRoomsQuery, connection, transaction);
                        clearRoomsCommand.ExecuteNonQuery();

                        string updateRoomsStatusQuery = "UPDATE Rooms SET Room_Status = 0";
                        SqlCommand updateRoomsStatusCommand = new SqlCommand(updateRoomsStatusQuery, connection, transaction);
                        updateRoomsStatusCommand.ExecuteNonQuery();

                        string clearRolesQuery = "DELETE FROM Roles WHERE Role_ID <> 1";
                        SqlCommand clearRolesCommand = new SqlCommand(clearRolesQuery, connection, transaction);
                        clearRolesCommand.ExecuteNonQuery();

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
            if (dtpBookOutDate.Value <= dtpBookInDate.Value)
            {
                dtpBookOutDate.Value = dtpBookInDate.Value.AddDays(1);
            }
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
            string sortOrder = radBookingAsc.Checked ? "Asc" : "Desc";
            string searchTerm = txtBookingSearch.Text.Trim();
            ApplyBookingFilter(sortOrder, searchTerm);
        }

        private void ApplyBookingFilter(string sortOrder, string searchTerm)
        {
            LoadGuestsNotCheckedIn(sortOrder, searchTerm);
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
                        txtBookingName.Text = reader["First_Name"].ToString();
                        txtBookingSurname.Text = reader["Last_Name"].ToString();
                        txtBookingContactNum.Text = reader["Contact_Num"].ToString();
                        txtBookingEmail.Text = reader["Email"].ToString();
                        txtBookingIDnum.Text = reader["ID_Number"].ToString();

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
        private void btnQuestUpdate_Click(object sender, EventArgs e)
        {
            if (selectedBookingID == 0)
            {
                MessageBox.Show("No valid guest selected for updating.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult confirmResult = MessageBox.Show("Are you sure you want to update the information?", "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmResult == DialogResult.No)
            {
                ResetToOverview();
                return;
            }

            if (!CheckForChanges(selectedBookingID))
            {
                DialogResult result = MessageBox.Show("No changes detected. Do you want to cancel the update?", "No Changes Detected", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    ResetToOverview();
                    return;
                }
            }

            UpdateBookingDetails(selectedBookingID);
            LoadBookingOverview();
            ResetToOverview();
        }





        private void LoadBookingOverview()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(SessionManager.ConnectionString))
                {
                    conn.Open();

                    string query = @"
                SELECT 
                    g.First_Name,
                    g.Last_Name,
                    g.Contact_Num,
                    g.Email,
                    g.ID_Number,
                    r.Room_Number,
                    gb.CheckIn_Date,
                    gb.CheckOut_Date
                FROM 
                    Guest_Booking gb
                JOIN 
                    Guests g ON gb.Guest_ID = g.Guest_ID
                JOIN 
                    Rooms r ON gb.Room_ID = r.Room_ID";

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
        private void Booking_Form_Load(object sender, EventArgs e)
        {

        }

        private void txtBookingSearch_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtBookingSearch.Text.Trim();
            string sortOrder = radBookingAsc.Checked ? "Asc" : "Desc";
            ApplyBookingFilter(sortOrder, searchTerm);
        }
    }
}