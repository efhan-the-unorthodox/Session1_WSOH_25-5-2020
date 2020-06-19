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
    public partial class EditResource : Form
    {
        int resId;
        public EditResource(int resID)
        {
            InitializeComponent();
            resId = resID;
        }

        private void EditResource_Load(object sender, EventArgs e)
        {
            loadAllocatedSkills();
            using(var db = new Session1_WSOHEntities())
            {
                var resource = db.Resources.Where(a => a.resId == resId).FirstOrDefault();
                lblResourceName.Text = resource.resName.ToString();
                lblResourceType.Text = resource.Resource_Type.resTypeName.ToString();
                nudAvailableQty.Value = Convert.ToDecimal(resource.remainingQuantity);
                if (Convert.ToInt32(nudAvailableQty.Value) == 0)
                {
                    clbAllocatedSkills.Visible = false;
                }
                else
                {
                    foreach (var skills in resource.Resource_Allocation.ToList()) 
                    {
                        var skillName = skills.Skill.skillName;
                        for(var i = 0; i <clbAllocatedSkills.Items.Count; i++)
                        {
                            var cbSkillName = clbAllocatedSkills.Items[i].ToString();
                            if(cbSkillName == skillName)
                            {
                                clbAllocatedSkills.SetItemChecked(i, true);
                            }
                        }
                    }
                }
            }
        }
        private void loadAllocatedSkills()
        {
            //Function to load allocated skills into list checkbox
            using (var db = new Session1_WSOHEntities())
            {
                var listSkills = db.Skills.Select(a => a.skillName).ToList();
                clbAllocatedSkills.Items.Clear();
                foreach (var item in listSkills)
                {
                    clbAllocatedSkills.Items.Add(item);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            using(var db = new Session1_WSOHEntities())
            {
                var currentResource = db.Resources.Where(a => a.resId == resId).FirstOrDefault();
                var qty = Convert.ToInt32(nudAvailableQty.Value);
                currentResource.remainingQuantity = qty;
                foreach(var skill in currentResource.Resource_Allocation.ToList())
                {
                    db.Resource_Allocation.Remove(skill);
                }
                foreach(var updatedSkill in clbAllocatedSkills.CheckedItems)
                {
                    var skillId = db.Skills.Where(a => a.skillName == updatedSkill.ToString()).Select(a => a.skillId).FirstOrDefault();
                    var newAllocatedResources = new Resource_Allocation()
                    {
                        resIdFK = resId,
                        skillIdFK = skillId
                    };
                    db.Resource_Allocation.Add(newAllocatedResources);
                }
                try
                {
                    db.SaveChanges();
                    MessageBox.Show("Changes have been successfully made to the resource");
                    this.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Oops, something went wrong while saving your resource changes");
                    throw;
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
