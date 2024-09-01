using Group_50_CMPG223_HotelManagementSystem;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Group50_Hotel_System
{
    public partial class Manage_Employees : Form
    {
        private DataTable dataTable;
        private int hiddenEmployeeId;

        public Manage_Employees()
        {
            InitializeComponent();
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            string selectQuery = @"
            SELECT Employees.Employee_ID, Employees.Username, Employees.Surname, Employees.First_Name, Employees.Password, Roles.Role_Desc
            FROM Employees
            JOIN Roles ON Employees.Role_ID = Roles.Role_ID;";

            using (SqlConnection connection = new SqlConnection(SessionManager.ConnectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(selectQuery, connection);
                dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dgvEmployees.DataSource = dataTable;
            }
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

        private string PromptForPassword(string prompt)
        {
            using (var promptForm = new Form())
            {
                var textBox = new TextBox { PasswordChar = '*', Dock = DockStyle.Fill };
                var buttonOk = new Button { Text = "OK", DialogResult = DialogResult.OK, Dock = DockStyle.Bottom };
                var buttonCancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Dock = DockStyle.Bottom };

                var layout = new FlowLayoutPanel { Dock = DockStyle.Fill };
                layout.Controls.Add(new Label { Text = prompt });
                layout.Controls.Add(textBox);
                layout.Controls.Add(buttonOk);
                layout.Controls.Add(buttonCancel);

                promptForm.Controls.Add(layout);
                promptForm.AcceptButton = buttonOk;
                promptForm.CancelButton = buttonCancel;

                return promptForm.ShowDialog() == DialogResult.OK ? textBox.Text : null;
            }
        }

        private void Manage_Employees_Load(object sender, EventArgs e)
        {
            LoadEmployees();
        }

        private void dgvEmployees_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dgvEmployees.Rows[e.RowIndex].Selected = true;
                string employeeName = dgvEmployees.Rows[e.RowIndex].Cells["First_Name"].Value.ToString();
                lblDisplay.ForeColor = Color.Blue;
                lblDisplay.Text = $" {employeeName}";
            }
        }

        private void btnAddEployee_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = addEmployees;
        }

        private void btnDeleteEmployees_Click(object sender, EventArgs e)
        {
            if (dgvEmployees.SelectedRows.Count > 0)
            {
                int selectedEmployeeId = (int)dgvEmployees.SelectedRows[0].Cells["Employee_ID"].Value;

                DialogResult result = MessageBox.Show(
                    "Are you sure you want to delete this employee?",
                    "Confirm Deletion",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    string deleteEmployeeQuery = "DELETE FROM Employees WHERE Employee_ID = @Employee_ID";

                    using (SqlConnection connection = new SqlConnection(SessionManager.ConnectionString))
                    {
                        try
                        {
                            connection.Open();

                            using (SqlCommand command = new SqlCommand(deleteEmployeeQuery, connection))
                            {
                                command.Parameters.AddWithValue("@Employee_ID", selectedEmployeeId);

                                int rowsAffected = command.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Employee deleted successfully.");
                                    LoadEmployees();
                                }
                                else
                                {
                                    MessageBox.Show("Employee deletion failed.");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"An error occurred: {ex.Message}");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Deleting was canceled.");
                }
            }
            else
            {
                MessageBox.Show("Please select an employee from the list to delete.");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvEmployees.SelectedRows.Count > 0)
            {
                tabControl1.SelectedTab = updateEmployees;

                DataGridViewRow selectedRow = dgvEmployees.SelectedRows[0];
                txtUpdateName.Text = selectedRow.Cells["First_Name"].Value.ToString();
                txtUpdateSurname.Text = selectedRow.Cells["Surname"].Value.ToString();
                txtUpdateUsername.Text = selectedRow.Cells["Username"].Value.ToString();
                cbUpdate.SelectedItem = selectedRow.Cells["Role_Desc"].Value.ToString();
                hiddenEmployeeId = (int)selectedRow.Cells["Employee_ID"].Value;
            }
            else
            {
                MessageBox.Show("Please select an employee from the list to update.");
            }
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            if (dgvEmployees.SelectedRows.Count > 0)
            {
                int selectedEmployeeId = (int)dgvEmployees.SelectedRows[0].Cells["Employee_ID"].Value;
                string newPassword = PromptForPassword("Enter the new password:");

                if (string.IsNullOrEmpty(newPassword))
                {
                    MessageBox.Show("Password cannot be empty.");
                    return;
                }

                string hashedPassword = HashPassword(newPassword);
                string updatePasswordQuery = "UPDATE Employees SET Password = @Password WHERE Employee_ID = @Employee_ID";

                using (SqlConnection connection = new SqlConnection(SessionManager.ConnectionString))
                {
                    try
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(updatePasswordQuery, connection))
                        {
                            command.Parameters.AddWithValue("@Password", hashedPassword);
                            command.Parameters.AddWithValue("@Employee_ID", selectedEmployeeId);

                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Password reset successfully.");
                            }
                            else
                            {
                                MessageBox.Show("Password reset failed.");
                            }
                        }

                        LoadEmployees();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select an employee from the list.");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateEmployeeFields())
            {
                return; // If validation fails, exit the method.
            }

            string name = txtName.Text;
            string surname = txtSurname.Text;
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string role = cbRole.SelectedItem?.ToString();

            using (SqlConnection connection = new SqlConnection(SessionManager.ConnectionString))
            {
                try
                {
                    connection.Open();

                    string checkUsernameQuery = "SELECT COUNT(*) FROM Employees WHERE Username = @Username";
                    using (SqlCommand checkUsernameCommand = new SqlCommand(checkUsernameQuery, connection))
                    {
                        checkUsernameCommand.Parameters.AddWithValue("@Username", username);
                        int usernameCount = (int)checkUsernameCommand.ExecuteScalar();

                        if (usernameCount > 0)
                        {
                            MessageBox.Show("Username already taken. Please choose a different username.");
                            return;
                        }
                    }

                    string hashedPassword = HashPassword(password);

                    string insertRoleQuery = @"
            IF NOT EXISTS (SELECT 1 FROM Roles WHERE Role_Desc = @Role_Desc)
            BEGIN
                INSERT INTO Roles (Role_Desc) VALUES (@Role_Desc)
            END";

                    using (SqlCommand roleCommand = new SqlCommand(insertRoleQuery, connection))
                    {
                        roleCommand.Parameters.AddWithValue("@Role_Desc", role);
                        roleCommand.ExecuteNonQuery();
                    }

                    string getRoleIdQuery = "SELECT Role_ID FROM Roles WHERE Role_Desc = @Role_Desc";
                    int roleId;

                    using (SqlCommand roleIdCommand = new SqlCommand(getRoleIdQuery, connection))
                    {
                        roleIdCommand.Parameters.AddWithValue("@Role_Desc", role);
                        roleId = (int)roleIdCommand.ExecuteScalar();
                    }

                    string insertEmployeeQuery = @"
            INSERT INTO Employees (Role_ID, Username, Surname, First_Name, Password)
            VALUES (@Role_ID, @Username, @Surname, @First_Name, @Password)";

                    using (SqlCommand employeeCommand = new SqlCommand(insertEmployeeQuery, connection))
                    {
                        employeeCommand.Parameters.AddWithValue("@Role_ID", roleId);
                        employeeCommand.Parameters.AddWithValue("@Username", username);
                        employeeCommand.Parameters.AddWithValue("@Surname", surname);
                        employeeCommand.Parameters.AddWithValue("@First_Name", name);
                        employeeCommand.Parameters.AddWithValue("@Password", hashedPassword);
                        employeeCommand.ExecuteNonQuery();
                    }

                    MessageBox.Show("Employee information saved successfully!");
                    LoadEmployees();

                    ResetFormFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }


        private bool ValidateEmployeeFields()
        {
            errorProvider1.Clear();
            bool isValid = true;

            if (string.IsNullOrEmpty(txtName.Text))
            {
                errorProvider1.SetError(txtName, "Name is required.");
                isValid = false;
            }

            if (string.IsNullOrEmpty(txtSurname.Text))
            {
                errorProvider1.SetError(txtSurname, "Surname is required.");
                isValid = false;
            }

            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                errorProvider1.SetError(txtUsername, "Username is required.");
                isValid = false;
            }

            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                errorProvider1.SetError(txtPassword, "Password is required.");
                isValid = false;
            }

            if (cbRole.SelectedItem == null)
            {
                errorProvider1.SetError(cbRole, "Role is required.");
                isValid = false;
            }

            return isValid;
        }



        private void ResetFormFields()
        {
            txtName.Text = "";
            txtSurname.Text = "";
            txtUsername.Text = "";
            txtPassword.Text = "";
            cbRole.SelectedIndex = -1;
        }



        private void btnUpdateEmployees_Click(object sender, EventArgs e)
        {
            string name = txtUpdateName.Text;
            string surname = txtUpdateSurname.Text;
            string username = txtUpdateUsername.Text;
            string role = cbUpdate.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Please fill all the fields.");
                return;
            }

            bool changesMade = false;

            using (SqlConnection connection = new SqlConnection(SessionManager.ConnectionString))
            {
                connection.Open();

                string selectQuery = "SELECT Username, Surname, First_Name, Role_ID FROM Employees WHERE Employee_ID = @Employee_ID";
                SqlCommand selectCommand = new SqlCommand(selectQuery, connection);
                selectCommand.Parameters.AddWithValue("@Employee_ID", hiddenEmployeeId);

                SqlDataReader reader = selectCommand.ExecuteReader();
                if (reader.Read())
                {
                    string currentUsername = reader["Username"].ToString();
                    string currentSurname = reader["Surname"].ToString();
                    string currentName = reader["First_Name"].ToString();
                    int currentRoleId = (int)reader["Role_ID"];
                    reader.Close();

                    if (username != currentUsername || surname != currentSurname || name != currentName || role != cbUpdate.SelectedItem.ToString())
                    {
                        changesMade = true;

                        string getRoleIdQuery = "SELECT Role_ID FROM Roles WHERE Role_Desc = @Role_Desc";
                        SqlCommand roleIdCommand = new SqlCommand(getRoleIdQuery, connection);
                        roleIdCommand.Parameters.AddWithValue("@Role_Desc", role);
                        int roleId = (int)roleIdCommand.ExecuteScalar();

                        string updateQuery = @"
                        UPDATE Employees
                        SET Username = @Username, Surname = @Surname, First_Name = @First_Name, Role_ID = @Role_ID
                        WHERE Employee_ID = @Employee_ID";
                        SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                        updateCommand.Parameters.AddWithValue("@Username", username);
                        updateCommand.Parameters.AddWithValue("@Surname", surname);
                        updateCommand.Parameters.AddWithValue("@First_Name", name);
                        updateCommand.Parameters.AddWithValue("@Role_ID", roleId);
                        updateCommand.Parameters.AddWithValue("@Employee_ID", hiddenEmployeeId);
                        updateCommand.ExecuteNonQuery();
                    }
                }
                reader.Close();

                if (changesMade)
                {
                    MessageBox.Show("Employee information updated successfully.");
                }
                else
                {
                    MessageBox.Show("No updates were made.");
                }

                tabControl1.SelectedTab = Overview;
                LoadEmployees();
            }
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Main_Form mainForm = new Main_Form();
            mainForm.ShowDialog();
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (dataTable != null)
            {
                string searchText = textBox1.Text.ToLower();
                string filterExpression = $"Username LIKE '%{searchText}%' OR Surname LIKE '%{searchText}%' OR First_Name LIKE '%{searchText}%'";
                DataView dataView = new DataView(dataTable)
                {
                    RowFilter = filterExpression
                };
                dgvEmployees.DataSource = dataView;
            }
        }
    }
}
