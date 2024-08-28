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
            CreateDatabase();  // Create the database and tables
            AddDefaultEmployee();  // Add the default employee
        }

        private void CreateDatabase()
        {
            using (SqlConnection connection = new SqlConnection(SessionManager.ConnectionString))
            {
                connection.Open();

                // Create Address table
                string createAddressTableQuery = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Address' AND xtype='U')
                BEGIN
                    CREATE TABLE [dbo].[Address] (
                        [Address_ID]  INT           IDENTITY (1, 1) NOT NULL,
                        [Street_Name] VARCHAR (100) NULL,
                        [Town_City]   VARCHAR (50)  NULL,
                        PRIMARY KEY CLUSTERED ([Address_ID] ASC)
                    );
                END";
                SqlCommand createAddressTableCommand = new SqlCommand(createAddressTableQuery, connection);
                createAddressTableCommand.ExecuteNonQuery();

                // Create BankingDetails table
                string createBankingDetailsTableQuery = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='BankingDetails' AND xtype='U')
                BEGIN
                    CREATE TABLE [dbo].[BankingDetails] (
                        [Banking_ID]      INT          IDENTITY (1, 1) NOT NULL,
                        [Card_Type]       VARCHAR (20) NULL,
                        [Bank]            VARCHAR (50) NULL,
                        [Card_Num]        VARCHAR (16) NULL,
                        [Debit_Credit]    BIT          NULL,
                        [Card_Holder]     VARCHAR (50) NULL,
                        [Expiration_Date] DATE         NULL,
                        PRIMARY KEY CLUSTERED ([Banking_ID] ASC)
                    );
                END";
                SqlCommand createBankingDetailsTableCommand = new SqlCommand(createBankingDetailsTableQuery, connection);
                createBankingDetailsTableCommand.ExecuteNonQuery();

                // Create Roles table
                string createRolesTableQuery = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Roles' AND xtype='U')
                BEGIN
                    CREATE TABLE [dbo].[Roles] (
                        [Role_ID]   INT          IDENTITY (1, 1) NOT NULL,
                        [Role_Desc] VARCHAR (50) NULL,
                        PRIMARY KEY CLUSTERED ([Role_ID] ASC)
                    );
                END";
                SqlCommand createRolesTableCommand = new SqlCommand(createRolesTableQuery, connection);
                createRolesTableCommand.ExecuteNonQuery();

                // Create Employees table
                string createEmployeesTableQuery = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Employees' AND xtype='U')
                BEGIN
                    CREATE TABLE [dbo].[Employees] (
                        [Employee_ID] INT           IDENTITY (1, 1) NOT NULL,
                        [Role_ID]     INT           NULL,
                        [Username]    VARCHAR (50)  NULL,
                        [Surname]     VARCHAR (50)  NULL,
                        [First_Name]  VARCHAR (50)  NULL,
                        [Password]    VARCHAR (255) NULL,
                        PRIMARY KEY CLUSTERED ([Employee_ID] ASC),
                        UNIQUE NONCLUSTERED ([Username] ASC),
                        FOREIGN KEY ([Role_ID]) REFERENCES [dbo].[Roles] ([Role_ID])
                    );
                END";
                SqlCommand createEmployeesTableCommand = new SqlCommand(createEmployeesTableQuery, connection);
                createEmployeesTableCommand.ExecuteNonQuery();

                // Create Guests table
                string createGuestsTableQuery = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Guests' AND xtype='U')
                BEGIN
                    CREATE TABLE [dbo].[Guests] (
                        [Guest_ID]    INT          IDENTITY (1, 1) NOT NULL,
                        [Address_ID]  INT          NULL,
                        [First_Name]  VARCHAR (50) NULL,
                        [Last_Name]   VARCHAR (50) NULL,
                        [Contact_Num] VARCHAR (15) NULL,
                        [Email]       VARCHAR (50) NULL,
                        [ID_Number]   CHAR (13)    NULL,
                        PRIMARY KEY CLUSTERED ([Guest_ID] ASC),
                        FOREIGN KEY ([Address_ID]) REFERENCES [dbo].[Address] ([Address_ID])
                    );
                END";
                SqlCommand createGuestsTableCommand = new SqlCommand(createGuestsTableQuery, connection);
                createGuestsTableCommand.ExecuteNonQuery();

                // Create Rooms table
                string createRoomsTableQuery = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Rooms' AND xtype='U')
                BEGIN
                    CREATE TABLE [dbo].[Rooms] (
                        [Room_ID]     INT          IDENTITY (1, 1) NOT NULL,
                        [Room_Number] VARCHAR (10) NULL,
                        [Room_Type]   VARCHAR (50) NULL,
                        [Room_Status] BIT          NULL,
                        PRIMARY KEY CLUSTERED ([Room_ID] ASC)
                    );
                END";
                SqlCommand createRoomsTableCommand = new SqlCommand(createRoomsTableQuery, connection);
                createRoomsTableCommand.ExecuteNonQuery();

                // Create Guest_Booking table
                string createGuestBookingTableQuery = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Guest_Booking' AND xtype='U')
                BEGIN
                    CREATE TABLE [dbo].[Guest_Booking] (
                        [Booking_ID]    INT           IDENTITY (1, 1) NOT NULL,
                        [Room_ID]       INT           NULL,
                        [Guest_ID]      INT           NULL,
                        [Employee_ID]   INT           NULL,
                        [Banking_ID]    INT           NULL,
                        [CheckIn_Date]  DATE          NULL,
                        [CheckOut_Date] DATE          NULL,
                        [Payment_Date]  DATE          NULL,
                        [Review_Hotel]  VARCHAR (255) NULL,
                        [Is_CheckedIn]  BIT           DEFAULT ((0)) NULL,
                        [Is_CheckedOut] BIT           DEFAULT ((0)) NULL,
                        PRIMARY KEY CLUSTERED ([Booking_ID] ASC),
                        FOREIGN KEY ([Room_ID]) REFERENCES [dbo].[Rooms] ([Room_ID]),
                        FOREIGN KEY ([Guest_ID]) REFERENCES [dbo].[Guests] ([Guest_ID]),
                        FOREIGN KEY ([Employee_ID]) REFERENCES [dbo].[Employees] ([Employee_ID]),
                        FOREIGN KEY ([Banking_ID]) REFERENCES [dbo].[BankingDetails] ([Banking_ID])
                    );
                END";
                SqlCommand createGuestBookingTableCommand = new SqlCommand(createGuestBookingTableQuery, connection);
                createGuestBookingTableCommand.ExecuteNonQuery();

                MessageBox.Show("Database created successfully.");
            }
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

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}