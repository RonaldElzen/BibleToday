using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BibleToday
{
    public partial class Form1 : Form
    {
        //create new api object
        private readonly Api apiConn = new Api();

        //Create new sql object
        private Sql sql = new Sql();

        //Variables
        private int beginVerse;
        private string book;
        private int chapter;
        private int endVerse;
        private List<string> finishedPlans = new List<string>();
        private List<string> planDetails = new List<string>();
        private string[] preferences;
        private List<string> readingPlans = new List<string>();
        private List<string> savedVerses = new List<string>();

        /// <summary>
        ///     Constructor for form1
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            load();
        }

        /// <summary>
        ///     Loading method of the program.
        ///     Method will read the MySQL database and place it in form fields.
        /// </summary>
        public void load()
        {
            //Check to see if SQl connection is made. Exit program if not.
            if (sql.Connected() != true)
            {
                MessageBox.Show(
                    "Database connection failed, please check your internet connection and restart the application");
                Application.Exit();
            }

            //Check to see if preferences are set. If not show firststart
            if (sql.Connected())
            {
                preferences = sql.getPreferences();

                if (preferences[0] == null)
                {
                    var firststart = new firstStart();

                    firststart.Show();

                    //Hide form1 (this way because hide() won't work this early in the code
                    WindowState = FormWindowState.Minimized;
                    Visible = false;
                    ShowInTaskbar = false;
                }

                else
                {
                    //Load saved verses
                    savedVerses = sql.loadVerses();
                    listBoxSaved.DataSource = savedVerses;

                    //Load user reading plans
                    readingPlans = sql.loadSavedPlans();
                    finishedPlans = sql.loadCompletedPlans();
                    listBoxReadingPlans.DataSource = readingPlans;
                    listBoxFinishedPlans.DataSource = finishedPlans;
                    labelName.Text += " " + preferences[0];
                }
            }
        }


        /// <summary>
        ///     Button click method to search a verse in the api
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void buttonSearch_Click(object sender, EventArgs e)
        {
            //Get data from form
            string translation;
            book = textBoxBook.Text;

            try
            {
                chapter = int.Parse(textBoxChapter.Text);
                beginVerse = int.Parse(textBoxVerse.Text);
                if (textBoxVerse2.Text != "")
                {
                    endVerse = int.Parse(textBoxVerse2.Text);
                }
            }

            catch (Exception)
            {
                MessageBox.Show("Chapter and verses must be numbers");
            }
            //Check if an translation is filled in. 
            if (textBoxTranslation.Text == "")
            {
   
                translation = "&version=" + preferences[1];
            }

            //If translation is filled in, put &version for it and read textboxTranslation in it.
            else
            {
                translation = "&version=" + textBoxTranslation.Text;
            }


            //If not, set endverse to 0.
            if (textBoxVerse2.Text == "")
            {
                endVerse = 0;
            }
            //Send data to api object and read returned data in richtextbox.

            try
            {
                richTextBox1.Text = await apiConn.getVerses(book, chapter, beginVerse, endVerse, translation);
            }

            catch (Exception)
            {
                MessageBox.Show("We couldn't find any verses with your terms. Please check your spelling and try again");
            }
        }


        /// <summary>
        ///     A button for a little help
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click_1(object sender, EventArgs e)
        {
            //A little help :)
            MessageBox.Show(
                "If you're not familiar with the bible it's the best you start with the new testament. We recommend the book of Luke which tells about the life of Jesus.");
        }


        /// <summary>
        ///     Button to save the searched verse.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSave_Click(object sender, EventArgs e)
        {
            //Save verse to my veres
            sql.saveVerse(book, chapter, beginVerse, endVerse);

            //Refresh listBoxSaved
            savedVerses = sql.loadVerses();
            listBoxSaved.DataSource = savedVerses;
            listBoxSaved.Refresh();
            MessageBox.Show("Saved to my verses!");
        }


        /// <summary>
        ///     Button to load a saved verse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void buttonLoad_Click(object sender, EventArgs e)
        {
            Console.WriteLine(listBoxSaved.SelectedItem.ToString());

            //Get selected data from listbox in a string
            var selectedVerse = listBoxSaved.SelectedItem.ToString();


            //Split the numbers from the string and place them in a array
            var numbers = Regex.Split(selectedVerse, @"\D+");

            //Get data from array and parse them to int
            //Starts with 1 because 0 is empty.
            var chapter = int.Parse(numbers[1]);
            var beginVerse = int.Parse(numbers[2]);
            var endVerse = int.Parse(numbers[3]);
            var book = Regex.Replace(selectedVerse, @"[^A-Z^a-z]+", string.Empty);
            Console.WriteLine(book);


            richTextBox1.Text = await apiConn.getVerses(book, chapter, beginVerse, endVerse, preferences[1]);
        }

        /// <summary>
        ///     Button to open Addplan form where a user can select a new plan.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAddPlan_Click(object sender, EventArgs e)
        {
            var addplan = new AddPlan();
            addplan.Show();
        }

        /// <summary>
        ///     Method for changed index in listBoxReadingPlans
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxReadingPlans_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateplanData();
        }

        /// <summary>
        ///     Method to update the plan information on the form.
        /// </summary>
        public void updateplanData()
        {
            if (readingPlans.Count != 0)
            {
                //Substring the index from listbox and use it for sql
                var planIndex = int.Parse(listBoxReadingPlans.SelectedItem.ToString().Substring(0, 1));
                planDetails = sql.loadPlan(planIndex);
                labelStarted.Text = planDetails[0];
                labelDay.Text = planDetails[2];
                labelDaysToGo.Text = planDetails[3];
            }
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     Button to read a new verse of a plan.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void buttonReadVerse_Click(object sender, EventArgs e)
        {
            if (listBoxReadingPlans.Items.Count != 0)
            {
                var daysToGo = int.Parse(labelDaysToGo.Text);
                var day = int.Parse(labelDay.Text);
                var finished = "No";
                var list = new List<string>();

                //Parse id from listbox selected item
                var planIndex = int.Parse(listBoxReadingPlans.SelectedItem.ToString().Substring(0, 1));

                list = sql.getPlanVerse(planIndex, day);


                //Load verse
                richTextBox1.Text =
                    await apiConn.getVerses(list[0], int.Parse(list[1]), int.Parse(list[2]), int.Parse(list[3]), "");


                //Set day + 1 and days to go -1
                if (day <= daysToGo)
                {
                    day++;
                    daysToGo--;
                }


                else
                {
                 //Set the plan as finished
                 finished = "Yes";
                 MessageBox.Show("Good Job! Please refresh using refresh button.");
                }

                //Update plan progress
                sql.updatePlanProgress(planIndex, day, daysToGo, finished);

                updateplanData();

                //Refresh listboxes
                readingPlans = sql.listPlans();
                listBoxReadingPlans.DataSource = readingPlans;
                listBoxReadingPlans.Refresh();
            }

            else
            {
                MessageBox.Show("Please add a plan first");
            }
        }


        /// <summary>
        ///     Click method for toolstrip to open help form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var help = new Help();
            help.Show();
        }

        /// <summary>
        ///     Method to exit te program if exit is clicked in toolstrip
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        ///     Method to show options form if options is clicked in toolstrip
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void optionsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var options = new Options();
            options.Show();
            Hide();
        }

        /// <summary>
        ///     Method to open new about box when toolstrip item is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var about = new AboutBox();
            about.Show();
        }

        /// <summary>
        ///     Method to exit application after closing the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        ///     Refresh button to refresh verses listbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            readingPlans = sql.loadSavedPlans();
            listBoxReadingPlans.DataSource = readingPlans;
            listBoxReadingPlans.Refresh();
            finishedPlans = sql.loadCompletedPlans();
            listBoxFinishedPlans.DataSource = finishedPlans;
            listBoxFinishedPlans.Update();
            updateplanData();
        }
    }
}