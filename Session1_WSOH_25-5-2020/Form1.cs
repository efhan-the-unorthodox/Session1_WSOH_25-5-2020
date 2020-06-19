using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Session1_WSOH_25_5_2020
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCreateAcct_Click(object sender, EventArgs e)
        {
            var newAcct = new RMAccountCreation();
            this.Hide();
            newAcct.ShowDialog();
            this.Show();
        }

        private void btnRmLogin_Click(object sender, EventArgs e)
        {
            var rmLogin = new RMLogin();
            this.Hide();
            rmLogin.ShowDialog();
            this.Show();
        }
    }
}
