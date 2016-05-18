using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BibleToday
{
    public partial class AddPlan : Form
    {
        Sql sql = new Sql();

        /// <summary>
        ///     Constructor for add plan
        /// </summary>
        public AddPlan()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Load method for form add plan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddPlan_Load(object sender, EventArgs e)
        {
            List<String> readingPlans = new List<String>();
            readingPlans = sql.listPlans();
            listBox1.DataSource = readingPlans;


        }

        /// <summary>
        ///     Button to add a new plan to planprogress in MySQL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            int daysToGo = int.Parse(labelNumberOfVerses.Text);
            //Parse planID from listbox item
            int planIndex = int.Parse(listBox1.SelectedItem.ToString().Substring(0,1));
            sql.addPlan(planIndex, daysToGo);
            MessageBox.Show("Plan added to your plans! ");
                 
     
 
        }

        /// <summary>
        ///     Method to give details of a reading plan every time a new index is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<String> list = new List<string>();
            
            //Parse planID from listbox item
            int planIndex = int.Parse(listBox1.SelectedItem.ToString().Substring(0, 1));
            list =  sql.loadPlanDetails(planIndex);
            labelDescription.Text = list[2];
            labelNumberOfVerses.Text = list[3];
        

        
        }
    }
}
