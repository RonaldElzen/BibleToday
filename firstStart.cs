using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace BibleToday
{
    public partial class firstStart : Form
    {
        /// <summary>
        /// Constructor for firststart
        /// </summary>
        public firstStart()
        {
            InitializeComponent();

            //Link to support
            LinkLabel.Link link = new LinkLabel.Link();
            link.LinkData = "http://www.elzen-ict.nl/bibletoday/";
            linkLabel1.Links.Add(link);
        }

        /// <summary>
        ///     Method to submit the preferences to MySQL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            Sql sql = new Sql();
            
            if (textBoxName.Text != "" && textBoxTranslation.Text != "")
            {
                //Set new preferences in MySQL
                sql.setPreferences(textBoxName.Text, textBoxTranslation.Text);
                MessageBox.Show("If you want to change your preferences later, you can go to the options menu");
                var form1 = new Form1();
                form1.Show();
                Close();
            }

            else
            {
                MessageBox.Show("Please fill in all fields");
            }
        }

        /// <summary>
        /// Method to open support webpage when link is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
          
            Process.Start(e.Link.LinkData as string);
        }
    }
}