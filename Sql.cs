using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Class uses Mysql data from Nuget

namespace BibleToday
{
    class Sql
    {
        Boolean isConnected;
        MySqlCommand cmd;
        //String with SQL connection parameters
        public static string connection = @"server=localhost;userid=root;password='';database=bibletoday; Allow Zero Datetime=true; ConvertZeroDateTime=true;";

        //Create new MySql connection and reader
        MySqlConnection conn = new MySqlConnection(connection);
        MySqlDataReader rdr = null;

        public Sql()
        {

            //Make new connection with database
            conn = null;
            conn = new MySqlConnection(connection);

            //Open Connection
            //Set isConnected to true if connection is succes. Else set isConnected to false
            try
            {
              
                conn.Open();
                isConnected = true;
            }
            catch
            {
                isConnected = false;
            }
        }

        public Boolean Connected()
        {
            if (isConnected == true) { return true; }
            else { return false; }
        }


        /// <summary>
        /// Method to load the preferences from MySQL
        /// </summary>
        /// <returns>string array with preferences</returns>
        public string[] getPreferences()
        {
            string[] values = new String[3];

            //Always check if isConnected is true. If not errors will appear and program wil crash. Important for all SQL methods!
            if (isConnected == true)
            {
                try
                {
                    //Set SQL 
                    string query = "select * from preferences";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    rdr = cmd.ExecuteReader();

                    //read returned data
                    while (rdr.Read())
                    {
                        values[0] = rdr.GetString(0);
                        values[1] = rdr.GetString(1);
                        values[2] = rdr.GetString(2);
                        values[3] = rdr.GetString(3);
                    }
                    return values;
                }

             
                catch
                {
                    Console.WriteLine("Reader error");
                }
                //Close reader
                finally
                {
                    if (rdr != null)
                    {
                        rdr.Close();
                    }
                }
            }
            return values;
        }


        /// <summary>
        /// Insert new preferences in MySQL database
        /// </summary>
        /// <param name="name"></param>
        /// <param name="translation"></param>
        /// <param name="votd"></param>
        /// <param name="timeVotd"></param>
        /// 

        public void setPreferences(string name, string translation)
        {
            if(isConnected == true)
            {
                cmd = new MySqlCommand();
                cmd.CommandText = "INSERT INTO `preferences`(`Name`, `Translation`) VALUES (@name, @translation)";
                cmd.Connection = conn;
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@translation", translation);
                cmd.ExecuteNonQuery();
          
            }
        }

        public void updatePreferences(string translation, string name)
        {
            if (isConnected == true)
            {
                //Method to update the preferences
                MySqlCommand cmd = new MySqlCommand();
                // cmd.CommandText = "INSERT INTO `Preferences`(`city`, `intervalRate`, `unit`) VALUES (@city,@intervalrate,@units)";
                cmd.CommandText = "UPDATE `preferences` SET `name`=@name,`translation`=@translation";
                cmd.Connection = conn;
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@translation", translation);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.ExecuteNonQuery();
            
            }

       
        }

        /// <summary>
        /// Save searched verse in MySQL
        /// </summary>
        /// <param name="book"></param>
        /// <param name="chapter"></param>
        /// <param name="verseFrom"></param>
        /// <param name="verseTo"></param>
        ///
        public void saveVerse(string book,int chapter, int verseFrom, int verseTo)
        {
            cmd = new MySqlCommand();
            cmd.CommandText = "INSERT INTO `savedverses`(`Book`, `Chapter`, `VerseFrom`, `VerseTo`) VALUES (@book,@chapter,@VerseFrom,@VerseTo)";
            cmd.Connection = conn;
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@book", book);
            cmd.Parameters.AddWithValue("@chapter", chapter);
            cmd.Parameters.AddWithValue("@VerseFrom", verseFrom);
            cmd.Parameters.AddWithValue("@VerseTo", verseTo);
            cmd.ExecuteNonQuery();
         
        }

        /// <summary>
        /// Load saved verses
        /// </summary>
        /// <returns>String List with saved verses</returns>
        public List<string> loadVerses()
        {
            List<string> list = new List<String>();
            string query = "select * from savedverses";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {  //Add plandata to list.
                list.Add(rdr.GetString(0) + rdr.GetInt32(1).ToString() + " : " + rdr.GetInt32(2).ToString() + " - " +  rdr.GetInt32(3).ToString());

            }
            rdr.Close();
            return list;
        }

        /// <summary>
        /// Load a list with reading plans
        /// </summary>
        /// <returns>List with reading plans</returns>
        public List<string> listPlans()
        {
            List<string> list = new List<String>();
            string query = "SELECT planID, name from readingplans";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                list.Add(rdr.GetInt32(0).ToString() + " " + rdr.GetString(1));
            }
            rdr.Close();
            return list;
        }

        /// <summary>
        /// Load details of a reading plan. Reading plan is searched by planID (index).
        /// </summary>
        /// <param name="index"></param>
        /// <returns>List with plan details</returns>
        public List<string> loadPlanDetails(int index)
        {
            List<string> list = new List<String>();
            string query = "SELECT * from readingplans WHERE planID = " + index;
            MySqlCommand cmd = new MySqlCommand(query, conn);
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                list.Add(rdr.GetInt32(0).ToString());
                list.Add(rdr.GetString(1));
                list.Add(rdr.GetString(2));
                list.Add(rdr.GetInt32(3).ToString());

            }
            rdr.Close();
            return list;

        }

        /// <summary>
        /// Load all saved plans which the user has not yet finished.
        /// </summary>
        /// <returns>List with not finished plans</returns>
        public List<string> loadSavedPlans()
        {
            List<string> list = new List<String>();
            string query = "SELECT planID, planname from planprogress where finished=" + "'No'";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                list.Add(rdr.GetInt32(0) + " " + rdr.GetString(1));

            }
            rdr.Close();
            return list;

        }

        /// <summary>
        /// Method to load all the plans completed by user.
        /// </summary>
        /// <returns>List with completed plans</returns>
        public List<string> loadCompletedPlans()
        {
            List<string> list = new List<String>();
            string query = "SELECT planID, planname from planprogress where finished=" + "'Yes'";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                list.Add(rdr.GetInt32(0) + " " + rdr.GetString(1));

            }
            rdr.Close();
            return list;

        }

        /// <summary>
        /// Load a specified plan
        /// </summary>
        /// <param name="index"></param>
        /// <returns>List with plan data</returns>
        public List<String> loadPlan(int index)
        {
            List<String> planValues = new List<String>();
            string query = "SELECT  dateStarted, `lastVerse`,  `day`, `daysToGo`, `finished` from planprogress WHERE planID =" + index;
            MySqlCommand cmd = new MySqlCommand(query, conn);
            rdr = cmd.ExecuteReader();
         
            while (rdr.Read())
            {
                //Put values on list. This will go static (no loop), because a datetime and some int's must be converted to strings
                planValues.Add(rdr.GetDateTime(0).ToString());
                planValues.Add(rdr.GetString(1));
                planValues.Add(rdr.GetInt32(2).ToString());
                planValues.Add(rdr.GetInt32(3).ToString());
                planValues.Add(rdr.GetString(4));


            }
            rdr.Close();
            return planValues;
        }

        /// <summary>
        /// Method to add a user selected plan to MySQL
        /// </summary>
        /// <param name="index"></param>
        /// <param name="daysToGo"></param>
        public void addPlan(int index, int daysToGo)
        {
            string name = "" ;
            if (isConnected == true)
            {
                string query = "SELECT name from readingplans WHERE planID =" + index;
                MySqlCommand cmd = new MySqlCommand(query, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    name = rdr.GetString(0);

                }
                rdr.Close();

                cmd = new MySqlCommand();
                cmd.CommandText = "INSERT INTO `planprogress`(`planID`, `planname`, day, dateStarted, `daysToGo`) VALUES (@planID,@planName,@day,@dateStarted,@daysToGo)";
                cmd.Connection = conn;
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@planID", index);
                cmd.Parameters.AddWithValue("@dateStarted", DateTime.Now);
                cmd.Parameters.AddWithValue("@day", 1);
                cmd.Parameters.AddWithValue("@planname", name);
                cmd.Parameters.AddWithValue("@daysToGo", daysToGo);

                cmd.ExecuteNonQuery();
                rdr.Close();
            }
        }

        /// <summary>
        /// Query to select a verse of a plan. searched by planID and day
        /// </summary>
        /// <param name="planID"></param>
        /// <param name="day"></param>
        /// <returns>List with Verse of a plan</returns>
        public List<String> getPlanVerse(int planID, int day)
        {

            List<string> list = new List<String>();
            string query = "select book,chapter, startverse,endverse from planverses where planID =" + planID + " and day =" +  day;
            MySqlCommand cmd = new MySqlCommand(query, conn);
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                list.Add(rdr.GetString(0));
                list.Add(rdr.GetInt32(1).ToString());
                list.Add(rdr.GetInt32(2).ToString());
                list.Add(rdr.GetInt32(3).ToString());

            }
            rdr.Close();
            return list;
        }

        /// <summary>
        /// Method to update the plan progress in MySQL
        /// </summary>
        /// <param name="index"></param>
        /// <param name="day"></param>
        /// <param name="daystogo"></param>
        /// <param name="finished"></param>
        public void updatePlanProgress(int index, int day, int daystogo ,string finished)
        {
           
            string query = " UPDATE `planprogress` SET `day`=" + day + ", `daysToGo`="+  daystogo+ ",`finished`= '" + finished + "' WHERE `planID` = " + index;
            MySqlCommand cmd = new MySqlCommand(query, conn);
            rdr = cmd.ExecuteReader();
            rdr.Close();
        }


    }
}
