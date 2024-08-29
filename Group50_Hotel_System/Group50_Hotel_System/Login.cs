using Group_50_CMPG223_HotelManagementSystem;
using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Group50_Hotel_System
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            // Load event logic if needed
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string enteredUsername = txtUsername.Text;
            string enteredPassword = txtPassword.Text;

            if (string.IsNullOrEmpty(enteredUsername) || string.IsNullOrEmpty(enteredPassword))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            string hashedEnteredPassword = HashPassword(enteredPassword);  // Hash the entered password

            using (SqlConnection connection = new SqlConnection(SessionManager.ConnectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"
                        SELECT Employee_ID, First_Name, Surname, Password 
                        FROM Employees 
                        WHERE Username = @Username";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", enteredUsername);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedHashedPassword = reader["Password"] as string;

                                if (hashedEnteredPassword == storedHashedPassword)
                                {
                                    // Set the session manager properties
                                    SessionManager.LoggedInEmployeeID = Convert.ToInt32(reader["Employee_ID"]);
                                    SessionManager.LoggedInEmployeeUsername = enteredUsername;
                                    SessionManager.LoggedInEmployeeName = reader["First_Name"].ToString();
                                    SessionManager.LoggedInEmployeeSurname = reader["Surname"].ToString();

                                    // Proceed to main form
                                    this.Hide();
                                    Main_Form main_Form = new Main_Form();
                                    main_Form.ShowDialog();
                                    this.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Incorrect password.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Username not found.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void btnForget_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please contact administration at HTI@gmail.com");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void cbView_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !cbView.Checked;
        }
    }
}
