using Group_50_CMPG223_HotelManagementSystem;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Group50_Hotel_System
{
    public partial class Request_Report : Form
    {
        public Request_Report()
        {
            InitializeComponent();
        }

        //Top Guest 
        private void GetTop5Guests(DateTime startDate, DateTime endDate)
        {
            if (rdoDESC.Checked)
            {
                
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
                    AND gb.Is_CheckedIn  = 1
               
                GROUP BY 
                    g.Guest_ID, 
                    g.First_Name, 
                    g.Last_Name
                ORDER BY 
                    Total DESC;";

                using (SqlConnection conn = new SqlConnection(SessionManager.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        
                        cmd.Parameters.Add(new SqlParameter("@StartDate", startDate));
                        cmd.Parameters.Add(new SqlParameter("@EndDate", endDate));

                        try
                        {
                            conn.Open();

                            
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

            if (rdoASC.Checked)
            {
               
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
                    AND gb.Is_CheckedIn  = 1
               
                GROUP BY 
                    g.Guest_ID, 
                    g.First_Name, 
                    g.Last_Name
                ORDER BY 
                    Total ASC;";

                using (SqlConnection conn = new SqlConnection(SessionManager.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        
                        cmd.Parameters.Add(new SqlParameter("@StartDate", startDate));
                        cmd.Parameters.Add(new SqlParameter("@EndDate", endDate));

                        try
                        {
                            conn.Open();

                           
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
            else
            {
                MessageBox.Show("Please select a Filter Option");
            }
        }

        private void GetTop5GuestsLongest(DateTime startDate, DateTime endDate)
        {
            if (rdoDESC.Checked) { 
            
            string query = @"
                SELECT TOP 5
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
                    AND gb.Is_CheckedIn  = 1
                GROUP BY 
                    g.Guest_ID, 
                    g.First_Name, 
                    g.Last_Name
                ORDER BY 
                    Days_Stayed DESC";

                using (SqlConnection conn = new SqlConnection(SessionManager.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        
                        cmd.Parameters.Add(new SqlParameter("@StartDate", startDate));
                        cmd.Parameters.Add(new SqlParameter("@EndDate", endDate));

                        try
                        {
                            conn.Open();

                           
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
            else if (rdoASC.Checked)
            {
               
                string query = @"
                SELECT TOP 5
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
                    AND gb.Is_CheckedIn  = 1
                GROUP BY 
                    g.Guest_ID, 
                    g.First_Name, 
                    g.Last_Name
                ORDER BY 
                    Days_Stayed ASC";

                using (SqlConnection conn = new SqlConnection(SessionManager.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        
                        cmd.Parameters.Add(new SqlParameter("@StartDate", startDate));
                        cmd.Parameters.Add(new SqlParameter("@EndDate", endDate));

                        try
                        {
                            conn.Open();

                            
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
            else
            {
                MessageBox.Show("Please select a Filter Option");
            }
        }

        //Top Weeks
        private void GetBusiestWeeks(DateTime startDate, DateTime endDate)
        {
            
            string query = @"
                SELECT TOP 10
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

            using (SqlConnection conn = new SqlConnection(SessionManager.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                   
                    cmd.Parameters.Add(new SqlParameter("@StartDate", startDate));
                    cmd.Parameters.Add(new SqlParameter("@EndDate", endDate));

                    try
                    {
                        conn.Open();

                        
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

        //Hotel Reviews by Year
        private void GetHotelReviewsYear(DateTime startDate, DateTime endDate)
        {
            
            string query = @"
                SELECT 
                    DATEPART(YEAR, CheckOut_Date) AS Year,
                    AVG(CAST(Review_Hotel AS FLOAT)) as Rating
                FROM 
                    Guest_Booking
                WHERE 
                    CheckOut_Date BETWEEN @StartDate AND @EndDate
                GROUP BY 
                    DATEPART(YEAR, CheckOut_Date)
                ORDER BY 
                    Year";

            using (SqlConnection conn = new SqlConnection(SessionManager.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    
                    cmd.Parameters.Add(new SqlParameter("@StartDate", startDate));
                    cmd.Parameters.Add(new SqlParameter("@EndDate", endDate));

                    try
                    {
                        conn.Open();

                        
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

        //Hotel Reviews by Month
        private void GetHotelReviewsMonth(DateTime startDate, DateTime endDate)
        {
            
            string query = @"
                SELECT 
                    DATEPART(MONTH, CheckOut_Date) as Month_Number,
                    DATENAME(MONTH, CheckOut_Date) as Month_Name,
                    AVG(CAST(Review_Hotel AS FLOAT)) as Rating
                FROM 
                    Guest_Booking
                WHERE 
                    CheckOut_Date BETWEEN @StartDate AND @EndDate
                GROUP BY 
                    DATEPART(MONTH, CheckOut_Date),
                    DATENAME(MONTH, CheckOut_Date)
                ORDER BY 
                    Month_Number";

            using (SqlConnection conn = new SqlConnection(SessionManager.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    
                    cmd.Parameters.Add(new SqlParameter("@StartDate", startDate));
                    cmd.Parameters.Add(new SqlParameter("@EndDate", endDate));

                    try
                    {
                        conn.Open();

                       
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

        //Hotel Reviews by Week
        private void GetHotelReviewsWeek(DateTime startDate, DateTime endDate)
        {
           
            string query = @"
                SELECT 
                    DATEPART(WEEK, CheckOut_Date) AS Week,
                   CONVERT(VARCHAR(10), DATEADD(DAY, -(DATEPART(WEEKDAY, CheckOut_Date) - 1), CheckOut_Date), 120) 
                    + ' - ' + 
                   CONVERT(VARCHAR(10), DATEADD(DAY, 7 - DATEPART(WEEKDAY, CheckOut_Date), CheckOut_Date), 120) 
                   AS Week_Dates,
                   AVG(CAST(Review_Hotel AS FLOAT)) AS Rating
                    FROM 
                    Guest_Booking
                    WHERE 
                    CheckOut_Date BETWEEN @StartDate AND @EndDate
                    GROUP BY 
                       DATEPART(WEEK, CheckOut_Date) AS Week
                  
                    ORDER BY 
                    DATEPART(WEEK, CheckOut_Date)";

            using (SqlConnection conn = new SqlConnection(SessionManager.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    
                    cmd.Parameters.Add(new SqlParameter("@StartDate", startDate));
                    cmd.Parameters.Add(new SqlParameter("@EndDate", endDate));

                    try
                    {
                        conn.Open();

                       
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

        private void Request_Report_Load(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            
            DateTime selectedDate1 = dateTimePickerStart.Value;
            DateTime selectedDate2 = dateTimePickerEnd.Value;
            if (selectedDate1 >= DateTime.Today.AddDays(1) || selectedDate1 > selectedDate2 || selectedDate2 < selectedDate1 || selectedDate2 >= DateTime.Today.AddDays(1))
            {
                MessageBox.Show("Date Validation: \n Please make sure you select a start date and an end date \n Start date or End date can't be a date in the future \n End date can't be earlier than selected start date ");
            }
            else if (selectedDate1 != dateTimePickerStart.MinDate && selectedDate2 != dateTimePickerEnd.MinDate && selectedDate1 < selectedDate2)
            {
               
                DateTime startDate = dateTimePickerStart.Value;
                DateTime endDate = dateTimePickerEnd.Value;

                
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
           
            DateTime selectedDate1 = dateTimePicker3.Value;
            DateTime selectedDate2 = dateTimePicker4.Value;
            if (selectedDate1 >= DateTime.Today.AddDays(1) || selectedDate1 > selectedDate2 || selectedDate2 < selectedDate1 || selectedDate2 >= DateTime.Today.AddDays(1))
            {
                MessageBox.Show("Date Validation: \n Please make sure you select a start date and an end date \n Start date or End date can't be a date in the future \n End date can't be earlier than selected start date ");
            }
            else if (selectedDate1 != dateTimePicker3.MinDate && selectedDate2 != dateTimePicker4.MinDate && selectedDate1 < selectedDate2)
            {
              
                DateTime startDate = dateTimePicker3.Value;
                DateTime endDate = dateTimePicker4.Value;

                
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
                MessageBox.Show("Date Validation: \n Please make sure you select a start date and an end date \n Start date or End date can't be a date in the future \n End date can't be earlier than selected start date ");
            }
            else if (selectedDate1 != dateTimePicker5.MinDate && selectedDate2 != dateTimePicker6.MinDate && selectedDate1 < selectedDate2)
            {
                
                DateTime startDate = dateTimePicker5.Value;
                DateTime endDate = dateTimePicker6.Value;

              
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

        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Main_Form main_Form = new Main_Form();
            main_Form.ShowDialog();
            this.Close();
        }
    }
}
