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

        private void btnCheckInCheckedIn_Click(object sender, EventArgs e)
        {
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
                            SqlCommand command = new SqlCommand("UPDATE Guest_Booking SET Is_CheckedIn = 1 WHERE Booking_ID = @BookingID", connection, transaction);
                            command.Parameters.AddWithValue("@BookingID", bookingID);
                            command.ExecuteNonQuery();

                            transaction.Commit();
                            MessageBox.Show("Guest checked in successfully!");

                            LoadBookedGuests();
                            LoadCheckedInGuests();
                            ClearCheckInControls();
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

        private void dgvCheckBooking_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCheckBooking.Rows[e.RowIndex];
                int guestID = GetGuestIDForBooking(row.Cells["Booking_ID"].Value.ToString());
                lblSelectedGuest.Text = "Selected Guest: " + guestID.ToString();

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
            }
        }

        private void dgvCheckedBanking_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = tgvCheckedBanking.Rows[e.RowIndex];
                string cardHolder = row.Cells["Card_Holder"].Value.ToString();
                lblSelectedGuest.Text = "Selected Card Holder: " + cardHolder;
            }
        }

        private void btnUpdateCheckin_Click(object sender, EventArgs e)
        {
            int guestID = int.Parse(lblSelectedGuest.Text.Replace("Selected Guest: ", ""));
            // Logic to update the checked-in guest details using guestID
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
            return -1; // Return an invalid ID if no booking is selected
        }

        private int GetGuestIDForBooking(string bookingID)
        {
            int guestID = -1;
            using (SqlConnection connection = new SqlConnection(SessionManager.ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Guest_ID FROM Guest_Booking WHERE Booking_ID = @BookingID", connection);
                command.Parameters.AddWithValue("@BookingID", bookingID);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    guestID = int.Parse(reader["Guest_ID"].ToString());
                }
                reader.Close();
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
                Address.Town_City
            FROM 
                Guests 
            INNER JOIN 
                Address ON Guests.Address_ID = Address.Address_ID
            WHERE 
                Guests.Guest_ID = @GuestID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@GuestID", guestID);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    txtCheckIDNum.Text = reader["ID_Number"].ToString();
                    txtCheckinName.Text = reader["First_Name"].ToString();
                    txtCheckinSurname.Text = reader["Last_Name"].ToString();
                    txtCheckinContactNum.Text = reader["Contact_Num"].ToString();
                    txtCheckinEmail.Text = reader["Email"].ToString();
                    txtCheckinStreet.Text = reader["Street_Name"].ToString();
                    txtCheckinCity.Text = reader["Town_City"].ToString();
                }
                reader.Close();
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
        }
    }
}
