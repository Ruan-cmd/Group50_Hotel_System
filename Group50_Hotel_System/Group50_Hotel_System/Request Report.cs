using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Group50_Hotel_System
{
    public partial class Request_Report : Form
    {
        public Request_Report()
        {
            InitializeComponent();
        }
        private string connectionString = "";
        //Top Guest 
        private void GetTop5Guests(DateTime startDate, DateTime endDate)
        {
            // Define the query 
            string query = @"
                SELECT TOP 5 
                    g.Guest_ID, 
                    g.First_Name, 
                    g.Last_Name, 
                    COUNT(gb.Booking_ID) AS Total
                FROM 
                    Guests g
                JOIN 
                    Guest_Booking gb ON g.Guest_ID = gb.Guest_ID
                WHERE 
                    gb.CheckIn_Date BETWEEN @StartDate AND @EndDate
                GROUP BY 
                    g.Guest_ID, 
                    g.First_Name, 
                    g.Last_Name
                ORDER BY 
                    Total DESC;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Add parameters 
                    cmd.Parameters.Add(new SqlParameter("@StartDate", startDate));
                    cmd.Parameters.Add(new SqlParameter("@EndDate", endDate));

                    try
                    {
                        conn.Open();

                        // Execute the command and fill the DataGridView
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridViewTopGuests.DataSource = dt;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                    conn.Close();
                }
            }
        }
        private void GetTop5GuestsLongest(DateTime startDate, DateTime endDate)
        {
            // Define the query 
            string query = @"
                SELECT 
                    g.Guest_ID, 
                    g.First_Name, 
                    g.Last_Name, 
                    SUM(DATEDIFF(DAY, gb.CheckIn_Date, gb.CheckOut_Date)) AS Days_Stayed
                FROM 
                    Guests g
                JOIN 
                    Guest_Booking gb ON g.Guest_ID = gb.Guest_ID
                WHERE 
                    gb.CheckIn_Date BETWEEN @StartDate AND @EndDate
                    AND gb.CheckOut_Date BETWEEN @StartDate AND @EndDate
                GROUP BY 
                    g.Guest_ID, 
                    g.First_Name, 
                    g.Last_Name
                ORDER BY 
                    Days_Stayed DESC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Add parameters 
                    cmd.Parameters.Add(new SqlParameter("@StartDate", startDate));
                    cmd.Parameters.Add(new SqlParameter("@EndDate", endDate));

                    try
                    {
                        conn.Open();

                        // Execute the command and fill the DataGridView
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridViewLongeststayed.DataSource = dt;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                    conn.Close();
                }
            }
        }



        //Top Weeks
        private void GetBusiestWeeks(DateTime startDate, DateTime endDate)
        {

            // Define the query 
            string query = @"
                SELECT 
                    DATEPART(YEAR, CheckIn_Date) AS Year,
                    DATEPART(WEEK, CheckIn_Date) AS Week,
                    COUNT(Booking_ID) AS Total
                FROM 
                    Guest_Booking
                WHERE 
                    CheckIn_Date BETWEEN @StartDate AND @EndDate
                GROUP BY 
                    DATEPART(YEAR, CheckIn_Date),
                    DATEPART(WEEK, CheckIn_Date)
                ORDER BY 
                    Total DESC;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Add parameters to the command
                    cmd.Parameters.Add(new SqlParameter("@StartDate", startDate));
                    cmd.Parameters.Add(new SqlParameter("@EndDate", endDate));

                    try
                    {
                        conn.Open();

                        // Execute the command and fill the DataGridView with the results
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridViewTopWeeks.DataSource = dt;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }




                }
            }
        }

        //Hotel Reviews

        private void GetHotelReviewsYear(DateTime startDate, DateTime endDate)
        {

            // Define the query 
            string query = @"
                SELECT 
                    DATEPART(YEAR, CheckOut_Date) AS Year,
                    AVG(CAST(Review_Hotel AS FLOAT))as Rating
                    
                    
                FROM 
                    Guest_Booking
                WHERE 
                    CheckOut_Date BETWEEN @StartDate AND @EndDate
              GROUP BY 
                    DATEPART(YEAR, CheckIn_Date),
            
              ORDER BY 
                    Year";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Add parameters to the command
                    cmd.Parameters.Add(new SqlParameter("@StartDate", startDate));
                    cmd.Parameters.Add(new SqlParameter("@EndDate", endDate));

                    try
                    {
                        conn.Open();

                        // Execute the command and fill the DataGridView with the results
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridViewYear.DataSource = dt;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }




                }
            }
        }

        private void GetHotelReviewsMonth(DateTime startDate, DateTime endDate)
        {

            // Define the query 
            string query = @"
                SELECT 
                 
                    DATEPART(MONTH, CheckOut_Date) as Month,
                    AVG(CAST(Review_Hotel AS FLOAT))as Rating
                    
                FROM 
                    Guest_Booking
                WHERE 
                    CheckOut_Date BETWEEN @StartDate AND @EndDate
              GROUP BY 
                    
                    DATEPART(MONTH, CheckIn_Date)
                    
              ORDER BY 
                    Month";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Add parameters to the command
                    cmd.Parameters.Add(new SqlParameter("@StartDate", startDate));
                    cmd.Parameters.Add(new SqlParameter("@EndDate", endDate));

                    try
                    {
                        conn.Open();

                        // Execute the command and fill the DataGridView with the results
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridViewMonth.DataSource = dt;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }




                }
            }
        }

        private void GetHotelReviewsWeek(DateTime startDate, DateTime endDate)
        {
            int rating = 0;
            // Define the query 
            string query = @"
                SELECT 
                    
                    DATEPART(WEEK, CheckOut_Date) AS Week,
                    AVG(CAST(Review_Hotel AS FLOAT))as Rating
                    
                FROM 
                    Guest_Booking
                WHERE 
                    CheckOut_Date BETWEEN @StartDate AND @EndDate
              GROUP BY 
                 
                    DATEPART(WEEK, CheckIn_Date)
              ORDER BY 
                    Week";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Add parameters to the command
                    cmd.Parameters.Add(new SqlParameter("@StartDate", startDate));
                    cmd.Parameters.Add(new SqlParameter("@EndDate", endDate));

                    try
                    {
                        conn.Open();

                        // Execute the command and fill the DataGridView with the results
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridViewWeek.DataSource = dt;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }




                }
            }
        }





        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Main_Form main_Form = new Main_Form();
            main_Form.ShowDialog();
            this.Close();
        }

        private void Request_Report_Load(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click_1(object sender, EventArgs e)
        {


            // Get the selected date from the DateTimePicker
            DateTime selectedDate1 = dateTimePickerStart.Value;
            DateTime selectedDate2 = dateTimePickerEnd.Value;
            if (selectedDate1 >= DateTime.Today.AddDays(1) || selectedDate1 > selectedDate2 || selectedDate2 < selectedDate1 || selectedDate2 >= DateTime.Today.AddDays(1))
            {
                MessageBox.Show("Date Validation: \n Please make sure you select a startdate and an enddate \n Startdate or Enddate cant be a date in the future  \n Enddate cant be earlier than selcted startdate ");
            }




            // Check if the dates are the same 

            else if (selectedDate1 != dateTimePickerStart.MinDate && selectedDate2 != dateTimePickerEnd.MinDate && selectedDate1 < selectedDate2)
            {
                // Get the dates from the DateTimePickers
                DateTime startDate = dateTimePickerStart.Value;
                DateTime endDate = dateTimePickerEnd.Value;

                // Call the method 
                GetTop5Guests(startDate, endDate);
                GetTop5GuestsLongest(startDate, endDate);
            }
        }

        private void btnClear_Click_1(object sender, EventArgs e)
        {
            dataGridViewTopGuests.DataSource = null;
            dataGridViewTopGuests.Rows.Clear();


            dateTimePickerStart.Focus();
        }

        private void btnSearchWeeks_Click_1(object sender, EventArgs e)
        {
            // Get the selected date from the DateTimePicker
            DateTime selectedDate1 = dateTimePicker3.Value;
            DateTime selectedDate2 = dateTimePicker4.Value;
            if (selectedDate1 >= DateTime.Today.AddDays(1) || selectedDate1 > selectedDate2 || selectedDate2 < selectedDate1 || selectedDate2 >= DateTime.Today.AddDays(1))
            {
                MessageBox.Show("Date Validation: \n Please make sure you select a startdate and an enddate \n Startdate or Enddate cant be a date in the future  \n Enddate cant be earlier than selcted startdate ");
            }




            // Check if the dates are the same 

            else if (selectedDate1 != dateTimePicker3.MinDate && selectedDate2 != dateTimePicker4.MinDate && selectedDate1 < selectedDate2)
            {
                // Get the dates from the DateTimePickers
                DateTime startDate = dateTimePicker3.Value;
                DateTime endDate = dateTimePicker4.Value;

                // Call the method 

                GetBusiestWeeks(startDate, endDate);
            }
        }

        private void btnClearWeeks_Click_1(object sender, EventArgs e)
        {
            dataGridViewTopWeeks.DataSource = null;
            dataGridViewTopWeeks.Rows.Clear();


            dateTimePicker3.Focus();
        }

        private void btnSearchReview_Click_1(object sender, EventArgs e)
        {
            DateTime selectedDate1 = dateTimePicker5.Value;
            DateTime selectedDate2 = dateTimePicker6.Value;
            if (selectedDate1 >= DateTime.Today.AddDays(1) || selectedDate1 > selectedDate2 || selectedDate2 < selectedDate1 || selectedDate2 >= DateTime.Today.AddDays(1))
            {
                MessageBox.Show("Date Validation: \n Please make sure you select a startdate and an enddate \n Startdate or Enddate cant be a date in the future  \n Enddate cant be earlier than selcted startdate ");
            }




            // Check if the dates are the same 

            else if (selectedDate1 != dateTimePicker5.MinDate && selectedDate2 != dateTimePicker6.MinDate && selectedDate1 < selectedDate2)
            {
                // Get the dates from the DateTimePickers
                DateTime startDate = dateTimePicker5.Value;
                DateTime endDate = dateTimePicker6.Value;

                // Call the method 
                GetHotelReviewsYear(startDate, endDate);
                GetHotelReviewsMonth(startDate, endDate);
                GetHotelReviewsWeek(startDate, endDate);
            }
        }

        private void btnClearReview_Click_1(object sender, EventArgs e)
        {
            dataGridViewWeek.DataSource = null;
            dataGridViewWeek.Rows.Clear();
            dataGridViewMonth.DataSource = null;
            dataGridViewMonth.Rows.Clear();
            dataGridViewYear.DataSource = null;
            dataGridViewYear.Rows.Clear();

            dateTimePicker5.Focus();
        }
    }
}
