// File Name:-PerformanceGradeConfigUI.aspx.cs
// Created by:- Sachin
// Created Date:-15 Sept 2013
// Description:-This class is uesd to configure performance grades.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using StaffPerformanceEntity;
using Utility;

public partial class PerformanceGradeConfigUI : SchoolBase
{
    #region Data Member(s)
    
    private int miRowCount = 0;
    private PerformanceGradeBL moPerformanceGradeBL;

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
            moPerformanceGradeBL = new PerformanceGradeBL(miSchoolId, miUserId);
            if (!IsPostBack)
            {
                FillGradeListView();
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
    protected void lstvwGradeConfiguration_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                
                Label lblRowNo = (Label)e.Item.FindControl("lblRowNo");
                CheckBox chkIsSelected = (CheckBox)e.Item.FindControl("ChkSelect");
                TextBox txtGradeName = (TextBox)e.Item.FindControl("txtGradeName");
                TextBox txtShortName = (TextBox)e.Item.FindControl("txtShortName");
                TextBox txtGradeDescription = (TextBox)e.Item.FindControl("txtGradeDescription");
                DropDownList cmbSortOrder = e.Item.FindControl("cmbSortOrder") as DropDownList;
                PerformanceGrade oPerformanceGrade = oCurrentItem.DataItem as PerformanceGrade;

                if (lstvwGradeConfiguration.DataKeys[oCurrentItem.DisplayIndex]["SchoolId"].ToString() != Constants.S_DEFAUL_SCHOOL_ID)
                    chkIsSelected.Checked = true;
                else
                {
                    chkIsSelected.Checked = false;
                    txtShortName.Text = string.Empty;
                    txtGradeDescription.Text = string.Empty;
                }

                FillSortOrderCombo(oCurrentItem);
                cmbSortOrder.SelectedValue = lstvwGradeConfiguration.DataKeys[oCurrentItem.DisplayIndex]["SortOrder"].ToString();
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
    protected void lstvwGradeConfiguration_DataBound(object sender, EventArgs e)
    {
        try
        {
            hidRowCnt.Value = lstvwGradeConfiguration.Items.Count.ToString();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save grade configuration details.
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
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.PerformanceGrade));

            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.StaffPerformanceRelated)));
        }
        catch (ReferenceExceptions ex)
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
    /// This method is used to fill grade details in list view.
    /// </summary>
    private void FillGradeListView()
    {   
        List<PerformanceGrade> lstPerformanceGrades = moPerformanceGradeBL.GetAll();
        lstvwGradeConfiguration.DataSource = lstPerformanceGrades;
        miRowCount = lstPerformanceGrades.Count();
        lstvwGradeConfiguration.DataBind();
    }
    
    /// <summary>
    /// This method is used to save grade details.
    /// </summary>
    private void Save()
    {   
        List<PerformanceGrade> lstPerformanceGrades = Populate();
        moPerformanceGradeBL.Insert(base.GenerateXml(lstPerformanceGrades));        
    }

    /// <summary>
    /// This method is used to populate grade details.
    /// </summary>
    /// <returns></returns>
    private List<PerformanceGrade> Populate()
    {
        List<PerformanceGrade> lstGrades = new List<PerformanceGrade>();
        PerformanceGrade oPerformanceGrade = null;

        foreach (ListViewDataItem oCurrentItem in lstvwGradeConfiguration.Items)
        {
            oPerformanceGrade = new PerformanceGrade();            
            
            CheckBox chkSelect = oCurrentItem.FindControl("ChkSelect") as CheckBox;
            TextBox txtGradeName = oCurrentItem.FindControl("txtGradeName") as TextBox;
            TextBox txtShortName = oCurrentItem.FindControl("txtShortName") as TextBox;
            TextBox txtGradeDescription = oCurrentItem.FindControl("txtGradeDescription") as TextBox;
            DropDownList cmbSortOrder = oCurrentItem.FindControl("cmbSortOrder") as DropDownList;
            if (chkSelect.Checked == true || Convert.ToInt32(lstvwGradeConfiguration.DataKeys[oCurrentItem.DisplayIndex]["SchoolId"].ToString()) == miSchoolId)
            {
                oPerformanceGrade.GradeId = Convert.ToInt32(lstvwGradeConfiguration.DataKeys[oCurrentItem.DisplayIndex]["GradeId"].ToString());
                oPerformanceGrade.GradeName = txtGradeName.Text.Trim();
                oPerformanceGrade.ShortName = txtShortName.Text.Trim();
                oPerformanceGrade.Description = txtGradeDescription.Text.Trim();
                oPerformanceGrade.SortOrder = Convert.ToInt32(cmbSortOrder.SelectedValue);
                oPerformanceGrade.OriginalGradeId = Convert.ToInt32(lstvwGradeConfiguration.DataKeys[oCurrentItem.DisplayIndex]["OriginalGradeId"].ToString());
                oPerformanceGrade.IsDeleted = false;
                oPerformanceGrade.Action = !chkSelect.Checked ? Constants.Action.Delete : Constants.Action.Insert;
                oPerformanceGrade.School_Id = Convert.ToInt32(lstvwGradeConfiguration.DataKeys[oCurrentItem.DisplayIndex]["SchoolId"].ToString());

                lstGrades.Add(oPerformanceGrade);
            }
        }

        return lstGrades;
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