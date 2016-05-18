using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace BibleToday
{
    public partial class Help : Form
    {
        /// <summary>
        ///     Constructor for help class
        /// </summary>
        public Help()
        {
            InitializeComponent();
            var link = new LinkLabel.Link();
            link.LinkData = "http://www.elzen-ict.nl/bibletoday/";
            linkLabel1.Links.Add(link);
        }

        /// <summary>
        ///     Method to open link in webbrowser
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData.ToString());
        }
    }
}