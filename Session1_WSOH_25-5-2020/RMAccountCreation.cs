using System;
using System.Linq;
using System.Windows.Forms;

namespace Session1_WSOH_25_5_2020
{
    public partial class RMAccountCreation : Form
    {
        public RMAccountCreation()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RMAccountCreation_Load(object sender, EventArgs e)
        {
            //getting user types
            using (var db = new Session1_WSOHEntities())
            {
                var listUserTypes = db.User_Type.ToList();
                cbUserType.DataSource = new BindingSource(listUserTypes, null);
                cbUserType.ValueMember = "userTypeId";
                cbUserType.DisplayMember = "userTypeName";
            }
        }

        private void btnCreateAcct_Click(object sender, EventArgs e)
        {
            var selectedUserType = ((User_Type)cbUserType.SelectedItem).userTypeId;

            if (tbUsername.Text.Length != 0 || tbUserID.Text.Length != 0 || tbPassword.Text.Length != 0 || tbRtpassword.Text.Length != 0)
            {
                if (tbUserID.Text.Length < 8)
                {
                    MessageBox.Show("User Id must be more than 8 characters in length");
                }
                else
                {
                    if(tbPassword.Text != tbRtpassword.Text)
                    {
                        MessageBox.Show("Mismatch Passwords");
                    }
                    else
                    {
                        using(var db = new Session1_WSOHEntities())
                        {
                            var checkUserId = db.Users.Where(a => a.userId == tbUserID.Text).FirstOrDefault();
                            if(checkUserId == null)
                            {
                                var newUser = new User()
                                {
                                    userId = tbUserID.Text,
                                    userName = tbUsername.Text,
                                    userPw = tbPassword.Text,
                                    userTypeIdFK = selectedUserType
                                };

                                db.Users.Add(newUser);
                                db.SaveChanges();
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Sorry, UserID has been taken");
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Missing Fields");
            }
        }
    }
}
