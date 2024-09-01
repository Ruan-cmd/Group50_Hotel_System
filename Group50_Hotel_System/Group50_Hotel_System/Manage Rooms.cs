using Group_50_CMPG223_HotelManagementSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Group50_Hotel_System
{
    public partial class Manage_Rooms : Form
    {
        private int selectedRoomID = -1;
        public Manage_Rooms()
        {
            InitializeComponent();
        }

        private void dgvRoom_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvRoom.Rows[e.RowIndex];
                selectedRoomID = Convert.ToInt32(row.Cells["Room_ID"].Value);
                lblRoomSelected.Text = $"Room {row.Cells["Room_Number"].Value} - {row.Cells["Room_Type"].Value}";
                btnAddRoomUpdate.Visible = true;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(SessionManager.ConnectionString))
            {
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Rooms WHERE Room_Number LIKE @search OR Room_Type LIKE @search", con);
                da.SelectCommand.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvRoom.DataSource = dt;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedRoomID != -1)
            {
                using (SqlConnection con = new SqlConnection(SessionManager.ConnectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Rooms WHERE Room_ID = @Room_ID", con);
                    cmd.Parameters.AddWithValue("@Room_ID", selectedRoomID);
                    cmd.ExecuteNonQuery();
                }
                LoadRooms();
                lblRoomSelected.Text = "Room deleted";
                selectedRoomID = -1;
                btnAddRoomUpdate.Visible = false;
            }
            else
            {
                MessageBox.Show("Please select a room to delete.");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedRoomID == -1)
            {
                MessageBox.Show("Please select a room to update.");
                return;
            }
            tcRooms.SelectedTab = tpAddRooms;
            tcRooms.TabPages[0].Text = "Update Room";
            btnAddRooms.Visible = false;
            btnAddRoomUpdate.Visible = true;

            using (SqlConnection con = new SqlConnection(SessionManager.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Rooms WHERE Room_ID = @Room_ID", con);
                cmd.Parameters.AddWithValue("@Room_ID", selectedRoomID);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtRoomNum.Text = reader["Room_Number"].ToString();
                    txtRoomType.Text = reader["Room_Type"].ToString();
                }
            }
        }

        private void btnAddRooms_Click(object sender, EventArgs e)
        {
            if (!ValidateRoomFields())
            {
                return;
            }

            using (SqlConnection con = new SqlConnection(SessionManager.ConnectionString))
            {
                con.Open();
                SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Rooms WHERE Room_Number = @Room_Number", con);
                checkCmd.Parameters.AddWithValue("@Room_Number", txtRoomNum.Text);
                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    MessageBox.Show("This room number already exists.");
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO Rooms (Room_Number, Room_Type) VALUES (@Room_Number, @Room_Type)", con);
                    cmd.Parameters.AddWithValue("@Room_Number", txtRoomNum.Text);
                    cmd.Parameters.AddWithValue("@Room_Type", txtRoomType.Text);
                    cmd.ExecuteNonQuery();
                    LoadRooms();
                    txtRoomNum.Clear();
                    txtRoomType.Clear();
                    MessageBox.Show("Room added successfully.");
                }
            }
        }


        private void btnAddRoomUpdate_Click(object sender, EventArgs e)
        {
            if (!ValidateRoomFields())
            {
                return;
            }

            using (SqlConnection con = new SqlConnection(SessionManager.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Rooms SET Room_Number = @Room_Number, Room_Type = @Room_Type WHERE Room_ID = @Room_ID", con);
                cmd.Parameters.AddWithValue("@Room_Number", txtRoomNum.Text);
                cmd.Parameters.AddWithValue("@Room_Type", txtRoomType.Text);
                cmd.Parameters.AddWithValue("@Room_ID", selectedRoomID);
                cmd.ExecuteNonQuery();
            }

            LoadRooms();
            tcRooms.SelectedTab = tpOverview;
            tcRooms.TabPages[0].Text = "Add Rooms";
            btnAddRooms.Visible = true;
            btnAddRoomUpdate.Visible = false;
            txtRoomNum.Clear();
            txtRoomType.Clear();
            lblRoomSelected.Text = "Room updated successfully.";
            EnableOverviewControls();
        }

        private bool ValidateRoomFields()
        {
            errorProvider1.Clear();
            bool isValid = true;

            if (string.IsNullOrEmpty(txtRoomNum.Text))
            {
                errorProvider1.SetError(txtRoomNum, "Room Number is required.");
                isValid = false;
            }

            if (string.IsNullOrEmpty(txtRoomType.Text))
            {
                errorProvider1.SetError(txtRoomType, "Room Type is required.");
                isValid = false;
            }

            return isValid;
        }


        private void Manage_Rooms_Load(object sender, EventArgs e)
        {
            LoadRooms();
            btnAddRoomUpdate.Visible = false;
        }
        private void LoadRooms()
        {
            using (SqlConnection con = new SqlConnection(SessionManager.ConnectionString))
            {
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Rooms", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvRoom.DataSource = dt;
            }
        }

        private void EnableOverviewControls()
        {
            btnDelete.Enabled = true;
            dgvRoom.Enabled = true;
            txtSearch.Enabled = true;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Main_Form main_Form = new Main_Form();
            main_Form.ShowDialog();
            this.Close();
        }
    }
}
