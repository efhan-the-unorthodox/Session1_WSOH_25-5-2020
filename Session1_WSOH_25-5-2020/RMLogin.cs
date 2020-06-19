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
    public partial class RMLogin : Form
    {
        public RMLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var userId = tbUserID.Text;
            var password = tbPassword.Text;

            //authentication
            using(var db = new Session1_WSOHEntities())
            {
                var authentication = db.Users.Where(a => a.userId == userId).FirstOrDefault();
                if(authentication != null)
                {
                    if(authentication.userPw == password)
                    {
                        var resManagement = new ResourceManagement();
                        this.Hide();
                        resManagement.ShowDialog();
                        this.Show();
                    }
                    else
                    {
                        MessageBox.Show("Incorrect Password");
                    }
                }
                else
                {
                    MessageBox.Show("User does not exist");
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
