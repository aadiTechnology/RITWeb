//File Name:-GradeConfigurationUI.aspx
//Created by:-
//Created Date:-5 May 2011
//Description:-This class is uesd to configure the grade.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BusinessLogic;
using System.Xml;
using System.Data;
using BusinessLogic.Exceptions;
using Utility;
using XseedReportEntities;
using System.Xml.Serialization;

public partial class GradeConfigurationUI : SchoolBase
{
    #region "Events"
    int miRowCount = 0;
    /// <summary>
    /// This event is used to set javascript attributes for buttons, set default values to controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                RefreshValues();
                FillGradeListView();
                SetJavaScriptAttributes();                
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValues();
            }
            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set values to listview columns.
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
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                
                CheckBox chkIsSelected = (CheckBox)e.Item.FindControl("ChkSelect");
                TextBox txtGradeName = (TextBox)e.Item.FindControl("txtGradeName");
                txtGradeName.Attributes.Add("onkeyup", "OnGridKeyUp(this,event);");


                TextBox txtShortName = (TextBox)e.Item.FindControl("txtShortName");
                txtShortName.Attributes.Add("onkeyup", "OnGridKeyUp(this,event);");

                TextBox txtGradeDescription = (TextBox)e.Item.FindControl("txtGradeDescription");
                txtGradeDescription.Attributes.Add("onkeyup", "OnGridKeyUp(this,event);");

                GradeMaster oGradeMaster = oCurrentItem.DataItem as XseedReportEntities.GradeMaster;

                if (lstvwGradeConfiguration.DataKeys[iRowId]["SchoolId"].ToString() != Constants.S_DEFAUL_SCHOOL_ID)
                {
                    if (oGradeMaster.IsDeleted.ToString() == "N")
                        chkIsSelected.Checked = true;
                    else
                        chkIsSelected.Checked = false;
                }
                else
                {
                    chkIsSelected.Checked = false;
                    txtShortName.Text = string.Empty;
                    txtGradeDescription.Text = string.Empty;
                }
                if (Convert.ToInt32(lstvwGradeConfiguration.DataKeys[iRowId]["ConsideredAsAbsent"]) == 1 || Convert.ToInt32(lstvwGradeConfiguration.DataKeys[iRowId]["ConsideredAsExempted"]) == 1)
                {
                    chkIsSelected.Checked = true;
                    chkIsSelected.Enabled = false;
                    txtGradeName.Enabled = false;
                    txtShortName.Enabled = false;
                    txtGradeDescription.Enabled = false;
                }
                DropDownList cmbSortOrder = oCurrentItem.FindControl("cmbSortOrder") as DropDownList;
                cmbSortOrder.Items.Add(new ListItem(Constants.S_SELECT, "0"));
                for (int iRowNo = 1; iRowNo <= miRowCount; iRowNo++)
                    cmbSortOrder.Items.Add(iRowNo.ToString());
                cmbSortOrder.SelectedValue = lstvwGradeConfiguration.DataKeys[iRowId]["SortOrder"].ToString();
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
            // This method is used to decrypt query string.
            bool bIsConfigured = QueryString[Constants.S_IS_CONFIGURED] != Constants.S_YES;
            if (bIsConfigured)
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.GradeConfiguration));
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Xseed_Report_Related)));
        }
        catch (ReferenceExceptions ex)
        {
            lblErr.Text = CommonUtility.ModifyExceptionMessage(ex.Message, "Grade", Resources.LocalizedResources.Grade, "can not be removed since associated with", Resources.LocalizedResources.valRemoveText);
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
        GradeMasterBL oGradeMasterBL = new GradeMasterBL(miSchoolId);
        List<GradeMaster> lstGradeMaster = oGradeMasterBL.GetAllGradeDetails();
        lstvwGradeConfiguration.DataSource = lstGradeMaster;
        miRowCount = lstGradeMaster.Count();
        lstvwGradeConfiguration.DataBind();
    }
    
    /// <summary>
    /// This method is used to save grade details.
    /// </summary>
    private void Save()
    {
        GradeMasterBL oGradeMasterBL = new GradeMasterBL(miSchoolId);
        List<GradeMaster> lstGradeMaster = PopulateGradeDetails();
        string sMessage = CheckDependencies(lstGradeMaster.Where(Sub => Sub.Action == Constants.Action.Delete).ToList());
        if (string.IsNullOrEmpty(sMessage))
            oGradeMasterBL.InsertGradeDetails(GetGradeDetailXML(lstGradeMaster), miUserId);
        else
            throw new ReferenceExceptions(sMessage);
    }

    /// <summary>
    /// This method is used to check dependencies.
    /// </summary>
    /// <param name="lstSubjectSectionConfigurationMaster"></param>
    /// <param name="aiAcademicYearId"></param>
    /// <returns></returns>
    private string CheckDependencies(List<GradeMaster> lstGradeMaster)
    {
        GenericReferenceList<GradeMaster> objStdRefereces = new GenericReferenceList<GradeMaster>(lstGradeMaster, miAcademicYearId);
        return objStdRefereces.CheckDependenciesForList("GradeId", "GradeName", "Action", Constants.ReferenceId.GradeConfiguration, false);
    }

    /// <summary>
    /// This method is used to populate grade details.
    /// </summary>
    /// <returns></returns>
    private List<GradeMaster> PopulateGradeDetails()
    {
        List<GradeMaster> lstGradeInfo = new List<GradeMaster>();
        GradeMaster oGradeMaster = null;

        for (int iRowCount = 0; iRowCount < lstvwGradeConfiguration.Items.Count; iRowCount++)
        {
            oGradeMaster = new GradeMaster();
            ListViewDataItem oCurrentItem = lstvwGradeConfiguration.Items[iRowCount] as ListViewDataItem;
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);

            CheckBox chkSelect = lstvwGradeConfiguration.Items[iRowCount].FindControl("ChkSelect") as CheckBox;
            TextBox txtGradeName = lstvwGradeConfiguration.Items[iRowCount].FindControl("txtGradeName") as TextBox;
            TextBox txtShortName = lstvwGradeConfiguration.Items[iRowCount].FindControl("txtShortName") as TextBox;
            TextBox txtGradeDescription = lstvwGradeConfiguration.Items[iRowCount].FindControl("txtGradeDescription") as TextBox;
            DropDownList cmbSortOrder = oCurrentItem.FindControl("cmbSortOrder") as DropDownList;
            if (chkSelect.Checked == true || Convert.ToInt32(lstvwGradeConfiguration.DataKeys[iRowCount]["SchoolId"].ToString()) == miSchoolId)
            {
                oGradeMaster.GradeId = Convert.ToInt32(lstvwGradeConfiguration.DataKeys[iRowCount]["GradeId"].ToString());
                oGradeMaster.GradeName = txtGradeName.Text.Trim();
                oGradeMaster.ShortName = txtShortName.Text.Trim();
                oGradeMaster.Description = txtGradeDescription.Text.Trim();
                oGradeMaster.SortOrder = Convert.ToInt32(cmbSortOrder.SelectedValue);
                oGradeMaster.OriginalGradeId = Convert.ToInt32(lstvwGradeConfiguration.DataKeys[iRowCount]["OriginalGradeId"].ToString());
                oGradeMaster.IsDeleted = Constants.C_NO.ToString();
                oGradeMaster.Action = !chkSelect.Checked ? Constants.Action.Delete : Constants.Action.Insert;
                oGradeMaster.SchoolId = Convert.ToInt32(lstvwGradeConfiguration.DataKeys[iRowCount]["SchoolId"].ToString());

                lstGradeInfo.Add(oGradeMaster);
            }
        }
        return lstGradeInfo;
    }

    /// <summary>
    /// This method is used to generate grade details XML.
    /// </summary>
    /// <param name="lstGradeDetails"></param>
    /// <returns></returns>
    private string GetGradeDetailXML(List<GradeMaster> lstGradeDetails)
    {
        StringWriter sw = new StringWriter();
        new XmlSerializer(lstGradeDetails.GetType()).Serialize(sw, lstGradeDetails);
        string sXML = sw.ToString();
        sXML = sXML.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", string.Empty);
        return sXML;
    }

    /// <summary>
    /// This method is used to set javascript attributes to controls.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> {btnCancel, btnSave});
        btnSave.Attributes.Add("onclick", "if(!CheckAtListOne()) return false;");
        btnCancel.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Xseed_Report_Related));
    }

    /// <summary>
    /// This Method used to change value of messgae according to culture
    /// </summary>
    private void RefreshValues()
    {
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        hidValGradeNameDuplicated.Value=Resources.LocalizedResources.ValGradeNameDuplicated;
        hidValShortNameDuplicated.Value = Resources.LocalizedResources.ValShortNameDuplicated;
        hidValGradeNameBlank.Value = Resources.LocalizedResources.ValGradeNameBlank;
        hidValShortNameBlank.Value = Resources.LocalizedResources.ValShortNameBlank;
        hidValGradeDescriptioneBlank.Value = Resources.LocalizedResources.ValGradeDescriptioneBlank;
        hidValAtLeastOneGrade.Value = Resources.LocalizedResources.ValAtLeastOneGrade;
        hidValGradeShortOrder.Value = Resources.LocalizedResources.ValGradeShortOrder;
        hidValGradeShortOrderSelected.Value = Resources.LocalizedResources.ValGradeShortOrderSelected;
    }

    #endregion "Private Method"

}