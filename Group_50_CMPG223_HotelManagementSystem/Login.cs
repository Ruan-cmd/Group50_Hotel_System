using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Group_50_CMPG223_HotelManagementSystem
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            AddDefaultEmployee();
        }

        private void AddDefaultEmployee()
        {
            using (SqlConnection connection = new SqlConnection(SessionManager.ConnectionString))
            {
                connection.Open();

                string checkRoleQuery = "SELECT COUNT(*) FROM Roles WHERE Role_ID = 1";
                SqlCommand checkRoleCommand = new SqlCommand(checkRoleQuery, connection);
                int roleCount = (int)checkRoleCommand.ExecuteScalar();

                if (roleCount == 0)
                {
                    string insertRoleQuery = @"
                    INSERT INTO Roles (Role_Desc)
                    VALUES ('DefaultRole')";
                    SqlCommand insertRoleCommand = new SqlCommand(insertRoleQuery, connection);
                    insertRoleCommand.ExecuteNonQuery();
                }

                string checkEmployeeQuery = "SELECT COUNT(*) FROM Employees WHERE Username = @Username";
                SqlCommand checkEmployeeCommand = new SqlCommand(checkEmployeeQuery, connection);
                checkEmployeeCommand.Parameters.AddWithValue("@Username", "Default");

                int employeeCount = (int)checkEmployeeCommand.ExecuteScalar();

                if (employeeCount == 0)
                {
                    string insertEmployeeQuery = @"
                    INSERT INTO Employees (Role_ID, Username, Surname, First_Name, Password)
                    VALUES (1, 'Default', 'Admin', 'Admin', 'Default')";

                    SqlCommand insertEmployeeCommand = new SqlCommand(insertEmployeeQuery, connection);
                    insertEmployeeCommand.ExecuteNonQuery();
                }
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(SessionManager.ConnectionString))
            {
                connection.Open();

                string query = "SELECT Employee_ID FROM Employees WHERE Username = @Username AND Password = @Password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", txtUsername.Text);
                command.Parameters.AddWithValue("@Password", txtPassword.Text);

                object result = command.ExecuteScalar();

                if (result != null)
                {
                    int employeeID = Convert.ToInt32(result);
                    SessionManager.LoggedInEmployeeID = employeeID;

                    this.Hide();
                    Main_Form MainForm = new Main_Form();
                    MainForm.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid username or password. Please try again.");
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

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }
    }
}