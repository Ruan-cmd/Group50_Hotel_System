﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group_50_CMPG223_HotelManagementSystem
{
    public static class SessionManager
    {
        public static int LoggedInEmployeeID { get; set; }
        public static string LoggedInEmployeeUsername { get; set; }
        public static string LoggedInEmployeeName { get; set; }
        public static string LoggedInEmployeeSurname { get; set; }

        public static string ConnectionString { get; set; } =
            "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\User\\OneDrive\\Documents\\GITHUB\\RuanCMD\\Group50_Hotel_System\\Group50_Hotel_System\\Group50_Hotel_System\\HotelManagementSystem.mdf;Integrated Security=True";
    }
}