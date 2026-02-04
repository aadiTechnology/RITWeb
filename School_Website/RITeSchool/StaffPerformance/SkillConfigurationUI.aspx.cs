// File Name:-PerformanceSkillConfigUI.aspx.cs
// Created by:- Ashish
// Created Date:-15 Sept 2013
// Description:-This class is uesd to configure performance grades.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using StaffPerformanceEntity;
using Utility;
using System.Data.SqlClient;
using System.Linq;

public partial class SkillConfigurationUI : SchoolBase
{
    #region Data Member(s)
    
    private int miRowCount = 0;
    private PerformanceSkillBL moPerformanceSkillBL;
    private List<InputType> mlstInputTypes = new List<InputType>();

    #endregion

    #region "Events"

    /// <summary>
    /// This event is used to set java script attributes for buttons, set default values to controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moPerformanceSkillBL = new PerformanceSkillBL(miSchoolId, miUserId);
            if (!IsPostBack)
            {
                FillSkillListView();
                SetJavaScriptAttributes();                
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
 

    /// <summary>
    /// This event is used to set values to list view columns.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwSkillConfiguration_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                CheckBox chkIsSelected = (CheckBox)e.Item.FindControl("ChkSelect");
                TextBox txtGradeName = (TextBox)e.Item.FindControl("txtSkillName");
                DropDownList  cmbSortOrder=(DropDownList)e.Item.FindControl("cmbSortOrder");
                DropDownList cmbInputType = (DropDownList)e.Item.FindControl("cmbInputType");
                Label lblSrNo = (Label)e.Item.FindControl("lblSrNo");
                lblSrNo.Text = (e.Item.DisplayIndex + 1).ToString();
                
                ListSource.FillDropDownList(mlstInputTypes, cmbInputType, "Name", "Id", Constants.S_SELECT);

                PerformanceSkill oPerformanceSkill = oCurrentItem.DataItem as PerformanceSkill;
                if (lstvwSkillConfiguration.DataKeys[iRowId]["SchoolId"].ToString() != Constants.S_DEFAUL_SCHOOL_ID)
                    chkIsSelected.Checked = true;
                else
                    chkIsSelected.Checked = false;
                FillSortOrderCombo(oCurrentItem);
                cmbSortOrder.SelectedValue = lstvwSkillConfiguration.DataKeys[iRowId]["SortOrder"].ToString();
                cmbInputType.SelectedValue = lstvwSkillConfiguration.DataKeys[iRowId]["InputTypeId"].ToString();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set no. of list view item count to hidden variable. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwSkillConfiguration_DataBound(object sender, EventArgs e)
    {
        try
        {
            hidRowCnt.Value = lstvwSkillConfiguration.Items.Count.ToString();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save skill configuration details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Save();
            bool bIsConfigured = QueryString[Constants.S_IS_CONFIGURED] != Constants.S_YES;
            if (bIsConfigured)
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.PerformanceSkill));
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.StaffPerformanceRelated)));
        }
        catch (SqlException ex)
        {
            lblErr.Text = ex.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
   
    #endregion "Events"

    #region "Private Methods"

    /// <summary>
    /// This method is used to fill skill details in list view.
    /// </summary>
    private void FillSkillListView()    
    {
        mlstInputTypes = moPerformanceSkillBL.GetInputTypes();
        mlstInputTypes = mlstInputTypes.Where(tp => tp.Id != Constants.FeedbackInputTypes.Both.ToInt()).ToList();

        List<PerformanceSkill> lstPerformanceSkill = moPerformanceSkillBL.GetAll();
        lstvwSkillConfiguration.DataSource = lstPerformanceSkill;
        miRowCount = lstPerformanceSkill.Count;
        lstvwSkillConfiguration.DataBind();

    }
    
    /// <summary>
    /// This method is used to save Skill details.
    /// </summary>
    private void Save()
    {
        List<PerformanceSkill> lstPerformanceSkill = Populate();
        moPerformanceSkillBL.Insert(base.GenerateXml(lstPerformanceSkill));       
    }

    /// <summary>
    /// This method is used to populate skill details.
    /// </summary>
    /// <returns></returns>
    private List<PerformanceSkill> Populate()
    {
        List<PerformanceSkill> lstSkills = new List<PerformanceSkill>();
        PerformanceSkill oPerformanceSkill = null;

        foreach (ListViewDataItem oCurrentItem in lstvwSkillConfiguration.Items)
        {
            oPerformanceSkill = new PerformanceSkill();            
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);

            CheckBox chkSelect = oCurrentItem.FindControl("ChkSelect") as CheckBox;
            TextBox txtSkillName = oCurrentItem.FindControl("txtSkillName") as TextBox;
            DropDownList cmbSortOrder = oCurrentItem.FindControl("cmbSortOrder") as DropDownList;
            DropDownList cmbInputType = oCurrentItem.FindControl("cmbInputType") as DropDownList;
            if (chkSelect.Checked == true || Convert.ToInt32(lstvwSkillConfiguration.DataKeys[oCurrentItem.DisplayIndex]["SchoolId"].ToString()) == miSchoolId)
            {
                oPerformanceSkill.SkillId = Convert.ToInt32(lstvwSkillConfiguration.DataKeys[oCurrentItem.DisplayIndex]["SkillId"].ToString());
                oPerformanceSkill.SkillName = txtSkillName.Text.Trim();
                oPerformanceSkill.SortOrder = Convert.ToInt32(cmbSortOrder.SelectedValue);
                oPerformanceSkill.InputTypeId = Convert.ToInt32(cmbInputType.SelectedValue);
                oPerformanceSkill.OriginalSkillId = Convert.ToInt32(lstvwSkillConfiguration.DataKeys[oCurrentItem.DisplayIndex]["OriginalSkillId"].ToString());
                oPerformanceSkill.IsDeleted = false;
                oPerformanceSkill.Action = !chkSelect.Checked ? Constants.Action.Delete : Constants.Action.Insert;
                oPerformanceSkill.School_Id = Convert.ToInt32(lstvwSkillConfiguration.DataKeys[oCurrentItem.DisplayIndex]["SchoolId"].ToString());

                lstSkills.Add(oPerformanceSkill);
            }
        }

        return lstSkills;
    }

    /// <summary>
    /// This method is used to set java script attributes to controls.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnCancel, btnSave });
        btnCancel.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.StaffPerformanceRelated));
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
    }

    /// <summary>
    /// This method is used to fill sort order combo box.
    /// </summary>
    /// <param name="aoCurrentItem"></param>
    private void FillSortOrderCombo(ListViewDataItem aoCurrentItem)
    {
        DropDownList cmbSortOrder = aoCurrentItem.FindControl("cmbSortOrder") as DropDownList;
        cmbSortOrder.Items.Add(new ListItem(Constants.S_SELECT, "0"));
        for (int iRowNo = 1; iRowNo <= miRowCount; iRowNo++)
            cmbSortOrder.Items.Add(iRowNo.ToString());
    }

    #endregion "Private Method"
}