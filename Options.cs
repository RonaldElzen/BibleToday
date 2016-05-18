using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace BibleToday
{
    public partial class Options : Form
    {
        private readonly Sql sql = new Sql();

        /// <summary>
        /// Constructor for options
        /// </summary>
        public Options()
        {
            InitializeComponent();


            //Link to support
            LinkLabel.Link link = new LinkLabel.Link();
            link.LinkData = "http://www.elzen-ict.nl/bibletoday/";
            linkLabel1.Links.Add(link);
        }

        /// <summary>
        ///     Button click method to update preferences
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                //Save the new preferences
                sql.updatePreferences(textBoxTranslation.Text, textBoxName.Text);
                MessageBox.Show("Preferences saved");
            }

            catch
            {
                MessageBox.Show("Could not save new preferences, please try again.");
            }

            Close();
        }


        /// <summary>
        ///     Method to display new form1 en close options form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Options_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
        
        }

        /// <summary>
        /// Method to open support webpage when link is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData.ToString());
        }
    }
   }
