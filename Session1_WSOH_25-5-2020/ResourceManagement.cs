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
    public partial class ResourceManagement : Form
    {
        List<string> resType = new List<string>();
        List<string> skilllist = new List<string>();
        public ResourceManagement()
        {
            InitializeComponent();
        }

        private void ResourceManagement_Load(object sender, EventArgs e)
        {
            loadPickers();
        }

        private void loadPickers()
        {
            using(var db = new Session1_WSOHEntities())
            {
                var listResType = db.Resource_Type.ToList();
                var listSkills = db.Skills.ToList();
                resType.Add("Type");
                skilllist.Add("Skill");
                foreach(var item in listResType)
                {
                    resType.Add(item.resTypeName);
                }
                foreach(var item in listSkills)
                {
                    skilllist.Add(item.skillName);
                }
                cbType.DataSource = new BindingSource(resType, null);
                cbSkill.DataSource = new BindingSource(skilllist, null);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string availability ="";
            var selectedType = (string)cbType.SelectedValue;
            var selectedSkill = (string)cbSkill.SelectedValue;
            if(selectedType == "Type")
            {
                if(selectedSkill == "Skill")
                {
                    using(var db = new Session1_WSOHEntities())
                    {
                        //get all resources
                        var listResources = db.Resources.OrderByDescending(a => a.remainingQuantity).ToList();
                        dgvResources.Rows.Clear();
                        foreach(var res in listResources)
                        {
                            var allocatedSkills = string.Join(",", res.Resource_Allocation.Select(a => a.Skill.skillName).ToList());
                            if(res.remainingQuantity > 5)
                            {
                                availability = "Sufficient";
                            }
                            else if(res.remainingQuantity >= 1 && res.remainingQuantity <5)
                            {
                                availability = "Low Stock";
                            }
                            else
                            {
                                availability = "Not Available";
                            }
                            object[] row = new object[]
                            {
                                res.resName,
                                res.Resource_Type.resTypeName,
                                res.Resource_Allocation.Count(),
                                allocatedSkills,
                                availability,
                                res.resId
                            };
                            dgvResources.Rows.Add(row);
                        }
                    }
                }
                else
                {
                    using(var db = new Session1_WSOHEntities())
                    {
                        //get resources according to the selected skill
                        var listResources = db.Resource_Allocation.OrderByDescending(a => a.Resource.remainingQuantity).Where(a => a.Skill.skillName == selectedSkill).ToList();
                        dgvResources.Rows.Clear();
                        foreach(var res in listResources)
                        {
                            var allocatedSkills = string.Join(",", res.Skill.skillName.ToList());
                            if(res.Resource.remainingQuantity > 5)
                            {
                                availability = "Sufficient";
                            }
                            else if(res.Resource.remainingQuantity >= 1 && res.Resource.remainingQuantity < 5)
                            {
                                availability = "Low Stock";
                            }
                            else
                            {
                                availability = "Not Available";
                            }
                            object[] row = new object[]
                            {
                                res.Resource.resName,
                                res.Resource.Resource_Type.resTypeName,
                                res.Resource.Resource_Allocation.Count(),
                                allocatedSkills,
                                availability,
                                res.resIdFK
                            };
                            dgvResources.Rows.Add(row);
                        }
                    }
                }
            }
            else
            {
                if(selectedSkill == "Skill")
                {
                    using (var db = new Session1_WSOHEntities())
                    {
                        var listResources = db.Resources.Where(a => a.Resource_Type.resTypeName == selectedType).OrderByDescending(a => a.remainingQuantity).ToList();
                        dgvResources.Rows.Clear();
                        foreach(var res in listResources)
                        {
                            var allocatedSkills = string.Join(",", res.Resource_Allocation.Select(s => s.Skill.skillName).ToList());
                            if (res.remainingQuantity > 5)
                            {
                                availability = "Sufficient";
                            }
                            else if (res.remainingQuantity >= 1 && res.remainingQuantity < 5)
                            {
                                availability = "Low Stock";
                            }
                            else
                            {
                                availability = "Not Available";
                            }
                            object[] row = new object[]
                            {
                                res.resName,
                                res.Resource_Type.resTypeName,
                                res.Resource_Allocation.Count(),
                                allocatedSkills,
                                availability,
                                res.resId
                            };
                            dgvResources.Rows.Add(row);
                        }
                    }
                }
                else
                {
                    using(var db = new Session1_WSOHEntities())
                    {
                        var listResources = db.Resource_Allocation.Where(a => a.Skill.skillName == selectedSkill && a.Resource.Resource_Type.resTypeName == selectedType).ToList();
                        dgvResources.Rows.Clear();
                        foreach (var res in listResources)
                        {
                            var allocatedSkills = string.Join(",", res.Resource.Resource_Allocation.Select(a=> a.Skill.skillName).ToList());
                            if (res.Resource.remainingQuantity > 5)
                            {
                                availability = "Sufficient";
                            }
                            else if (res.Resource.remainingQuantity >= 1 && res.Resource.remainingQuantity < 5)
                            {
                                availability = "Low Stock";
                            }
                            else
                            {
                                availability = "Not Available";
                            }
                            object[] row = new object[]
                            {
                                res.Resource.resName,
                                res.Resource.Resource_Type.resTypeName,
                                res.Resource.Resource_Allocation.Count(),
                                allocatedSkills,
                                availability,
                                res.resIdFK
                            };
                            dgvResources.Rows.Add(row);
                        }
                    }
                }
            }
        }

        private void cbSkill_SelectedIndexChanged(object sender, EventArgs e)
        {
            string availability = "";
            var selectedType = (string)cbType.SelectedValue;
            var selectedSkill = (string)cbSkill.SelectedValue;
            if (selectedType == "Type")
            {
                if (selectedSkill == "Skill")
                {
                    using (var db = new Session1_WSOHEntities())
                    {
                        //get all resources
                        var listResources = db.Resources.OrderByDescending(a => a.remainingQuantity).ToList();
                        dgvResources.Rows.Clear();
                        foreach (var res in listResources)
                        {
                            var allocatedSkills = string.Join(",", res.Resource_Allocation.Select(a => a.Skill.skillName).ToList());
                            if (res.remainingQuantity > 5)
                            {
                                availability = "Sufficient";
                            }
                            else if (res.remainingQuantity >= 1 && res.remainingQuantity < 5)
                            {
                                availability = "Low Stock";
                            }
                            else
                            {
                                availability = "Not Available";
                            }
                            object[] row = new object[]
                            {
                                res.resName,
                                res.Resource_Type.resTypeName,
                                res.Resource_Allocation.Count(),
                                allocatedSkills,
                                availability,
                                res.resId
                            };
                            dgvResources.Rows.Add(row);
                        }
                    }
                }
                else
                {
                    using (var db = new Session1_WSOHEntities())
                    {
                        //get resources according to the selected skill
                        var listResources = db.Resource_Allocation.OrderByDescending(a => a.Resource.remainingQuantity).Where(a => a.Skill.skillName == selectedSkill).ToList();
                        dgvResources.Rows.Clear();
                        foreach (var res in listResources)
                        {
                            var allocatedSkills = string.Join(",", res.Resource.Resource_Allocation.Select(s => s.Skill.skillName).ToList());
                            if (res.Resource.remainingQuantity > 5)
                            {
                                availability = "Sufficient";
                            }
                            else if (res.Resource.remainingQuantity >= 1 && res.Resource.remainingQuantity < 5)
                            {
                                availability = "Low Stock";
                            }
                            else
                            {
                                availability = "Not Available";
                            }
                            object[] row = new object[]
                            {
                                res.Resource.resName,
                                res.Resource.Resource_Type.resTypeName,
                                res.Resource.Resource_Allocation.Count(),
                                allocatedSkills,
                                availability,
                                res.resIdFK
                            };
                            dgvResources.Rows.Add(row);
                        }
                    }
                }
            }
            else
            {
                if (selectedSkill == "Skill")
                {
                    using (var db = new Session1_WSOHEntities())
                    {
                        var listResources = db.Resources.Where(a => a.Resource_Type.resTypeName == selectedType).OrderByDescending(a => a.remainingQuantity).ToList();
                        dgvResources.Rows.Clear();
                        foreach (var res in listResources)
                        {
                            var allocatedSkills = string.Join(",", res.Resource_Allocation.Select(s => s.Skill.skillName).ToList());
                            if (res.remainingQuantity > 5)
                            {
                                availability = "Sufficient";
                            }
                            else if (res.remainingQuantity >= 1 && res.remainingQuantity < 5)
                            {
                                availability = "Low Stock";
                            }
                            else
                            {
                                availability = "Not Available";
                            }
                            object[] row = new object[]
                            {
                                res.resName,
                                res.Resource_Type.resTypeName,
                                res.Resource_Allocation.Count(),
                                allocatedSkills,
                                availability,
                                res.resId
                            };
                            dgvResources.Rows.Add(row);
                        }
                    }
                }
                else
                {
                    using (var db = new Session1_WSOHEntities())
                    {
                        var listResources = db.Resource_Allocation.Where(a => a.Skill.skillName == selectedSkill && a.Resource.Resource_Type.resTypeName == selectedType).ToList();
                        dgvResources.Rows.Clear();
                        foreach (var res in listResources)
                        {
                            var allocatedSkills = string.Join(",", res.Resource.Resource_Allocation.Select(a=> a.Skill.skillName).ToList());
                            if (res.Resource.remainingQuantity > 5)
                            {
                                availability = "Sufficient";
                            }
                            else if (res.Resource.remainingQuantity >= 1 && res.Resource.remainingQuantity < 5)
                            {
                                availability = "Low Stock";
                            }
                            else
                            {
                                availability = "Not Available";
                            }
                            object[] row = new object[]
                            {
                                res.Resource.resName,
                                res.Resource.Resource_Type.resTypeName,
                                res.Resource.Resource_Allocation.Count(),
                                allocatedSkills,
                                availability,
                                res.resIdFK
                            };
                            dgvResources.Rows.Add(row);
                        }
                    }
                }
            }
        }

        private void dgvResources_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            foreach(DataGridViewRow row in dgvResources.Rows)
            {
                var availableQty = row.Cells[4].Value.ToString();
                if(availableQty == "Not Available")
                {
                    row.DefaultCellStyle.BackColor = Color.Red;
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //get the id from the current row
            var newResource = new AddResource();
            this.Hide();
            newResource.ShowDialog();
            this.Show();
            this.ResourceManagement_Load(sender, e);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            var resID = Convert.ToInt32(dgvResources.CurrentRow.Cells[5].Value);
            var editResource = new EditResource(resID);
            this.Hide();
            editResource.ShowDialog();
            this.Show();
            this.ResourceManagement_Load(sender, e);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var selectedResource = Convert.ToInt32(dgvResources.CurrentRow.Cells[5].Value);
            using(var db = new Session1_WSOHEntities())
            {
                var resourceTBD = db.Resources.Where(a => a.resId == selectedResource).FirstOrDefault();
                foreach(var r in resourceTBD.Resource_Allocation.ToList())
                {
                    db.Resource_Allocation.Remove(r);
                }
                db.Resources.Remove(resourceTBD);
                try
                {
                    db.SaveChanges();
                    MessageBox.Show("Resource Successfully deleted");
                    this.ResourceManagement_Load(sender, e);
                }
                catch (Exception)
                {
                    MessageBox.Show("Whoops! An error occured while deleting this resource");
                }
            }

        }
    }
}
