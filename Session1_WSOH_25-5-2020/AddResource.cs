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
    public partial class AddResource : Form
    {
        public AddResource()
        {
            InitializeComponent();
        }

        private void AddResource_Load(object sender, EventArgs e)
        {
            using(var db = new Session1_WSOHEntities())
            {
                var resTypes = db.Resource_Type.ToList();
                cbResType.DataSource = new BindingSource(resTypes, null);
                cbResType.DisplayMember = "resTypeName";
                cbResType.ValueMember = "resTypeId";
            }
            if(Convert.ToInt32(nudAvailableQty.Value) == 0)
            {
                clbAllocatedSkills.Visible = false;
            }
            loadAllocatedSkills();
        }

        private void btnAddresource_Click(object sender, EventArgs e)
        {
            var resourceName = tbResname.Text;
            var resourceType = ((Resource_Type)cbResType.SelectedItem).resTypeId;
            var availableQty = Convert.ToInt32(nudAvailableQty.Value);
            if(resourceName.Length == 0)
            {
                MessageBox.Show("Please enter a valid Resource name");
            }
            else
            {
                using(var db = new Session1_WSOHEntities())
                {
                    //check if the resource exists first
                    var checkResource = db.Resources.Where(a => a.resName == resourceName).FirstOrDefault();
                    if (checkResource != null)
                    {
                        MessageBox.Show("Sorry, this item exists in the records already");
                    }
                    else
                    {
                        var newResource = new Resource()
                        {
                            resName = resourceName,
                            resTypeIdFK = resourceType,
                            remainingQuantity = availableQty
                        };
                        db.Resources.Add(newResource);
                        foreach(var selectedSkill in clbAllocatedSkills.CheckedItems)
                        {
                            var skillName = (string)selectedSkill;
                            var skillid = db.Skills.Where(a => a.skillName == skillName).Select(a => a.skillId).FirstOrDefault();
                            var newResAloc = new Resource_Allocation()
                            {
                                resIdFK = newResource.resId,
                                skillIdFK = skillid
                            };
                            db.Resource_Allocation.Add(newResAloc);
                        }
                        try
                        {
                            db.SaveChanges();
                            MessageBox.Show("New Resource successfully created and saved to database");
                            tbResname.Clear();
                            nudAvailableQty.Value = 0;
                            this.AddResource_Load(sender, e);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Oops, an error occured");
                            throw;
                        }
                    }
                }
            }
        }
        private void loadAllocatedSkills()
        {
            //Function to load allocated skills into list checkbox
            using(var db = new Session1_WSOHEntities())
            {
                var listSkills = db.Skills.Select(a => a.skillName).ToList();
                clbAllocatedSkills.Items.Clear();
                foreach(var item in listSkills)
                {
                    clbAllocatedSkills.Items.Add(item);
                }
            }
        }

        private void nudAvailableQty_ValueChanged(object sender, EventArgs e)
        {
            var quantity = Convert.ToInt32(nudAvailableQty.Value);
            if(quantity == 0)
            {
                clbAllocatedSkills.Visible = false;
            }
            else
            {
                clbAllocatedSkills.Visible = true;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
