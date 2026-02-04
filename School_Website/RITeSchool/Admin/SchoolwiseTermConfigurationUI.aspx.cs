
// File Name    :SchoolwiseTermConfigurationUI.aspx.cs
// Created By   : Vinod 
// Created Date : 2/21/2011
// Description  : This class is used to configure Term start date and end date as per the standard.

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using BusinessLogic;
using BusinessLogic.Exceptions;
using TermEntities;
using Utility;
using System.Globalization;
using System.Data.SqlClient;

public partial class SchoolwiseTermConfigurationUI : SchoolBase
{
    #region "Constants"

    const string S_DEFAULT_DATETIME = "1/1/1900 12:00:00 AM";

    #endregion "Constants"

    #region "Members"

    public int iCnt = 0;
    public List<StandardwiseAcademicYearDates> lstStandardwiseAcademicYearDates = null;
    
    #endregion "Members"

    #region "Events"

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
                DesignSettingAccordingLanguage();

                if (CheckPreCondition())
                    FillTermConfigurationListView();
                SetJavaScriptAttributes();                
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                DesignSettingAccordingLanguage();
            }
  
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    } 

    /// <summary>
    /// This event is used to save term configuration details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            int iCount = Save();
            if (iCount == 0)
            {
                string sIsConfigured = ReadQuerystring();
                if (sIsConfigured != "Y")
                    SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.SchoolwiseTermConfiguration));
            }
        }
        catch (SqlException ex)
        {   
            lblMessage.Visible = true;
            lblMessage.Text = "Attendance is found out of given dates for classes '" + ex.Message + "' so you cannot set / change date(s). Please change date(s) or remove attendance of respective day(s) and class(s).";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            lblMessage.Font.Bold = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to move on school configuration screen.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Exam_Related)));
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
    protected void lstvwTermConfiguration_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = oCurrentItem.DisplayIndex;
                DataRowView oDataRowView = oCurrentItem.DataItem as DataRowView;
                TextBox txtT1StartDate = oCurrentItem.FindControl("txtTerm1StartDate") as TextBox;
                TextBox txtT2StartDate = oCurrentItem.FindControl("txtTerm2StartDate") as TextBox;
                TextBox txtT1EndDate = oCurrentItem.FindControl("txtTerm1EndDate") as TextBox;
                TextBox txtT2EndDate = oCurrentItem.FindControl("txtTerm2EndDate") as TextBox;
                HiddenField StartDate = oCurrentItem.FindControl("hidStartDate") as HiddenField;
                HiddenField EndDate = oCurrentItem.FindControl("hidEndDate") as HiddenField;

                if (((TermEntities.TermConfigurationDetails)((TermEntities.SchoolwiseTermConfigurationDetails)(oCurrentItem.DataItem)).TermIInfo).TermStartDate.ToDateTime() == S_DEFAULT_DATETIME.ToDateTime())
                    txtT1StartDate.Text = string.Empty;
               
               if (((TermEntities.TermConfigurationDetails)((TermEntities.SchoolwiseTermConfigurationDetails)(oCurrentItem.DataItem)).TermIInfo).TermEndDate.ToDateTime() == S_DEFAULT_DATETIME.ToDateTime())
                    txtT1EndDate.Text = string.Empty;
              
                if (((TermEntities.TermConfigurationDetails)((TermEntities.SchoolwiseTermConfigurationDetails)(oCurrentItem.DataItem)).TermIIInfo).TermStartDate.ToDateTime() == S_DEFAULT_DATETIME.ToDateTime())
                    txtT2StartDate.Text = string.Empty;
               
               if (((TermEntities.TermConfigurationDetails)((TermEntities.SchoolwiseTermConfigurationDetails)(oCurrentItem.DataItem)).TermIIInfo).TermEndDate.ToDateTime() == S_DEFAULT_DATETIME.ToDateTime())
                    txtT2EndDate.Text = string.Empty;
                 StartDate.Value = lstStandardwiseAcademicYearDates[iCnt].StartDate.ToString("dd-MMM-yyyy", new CultureInfo("en"));
                EndDate.Value = lstStandardwiseAcademicYearDates[iCnt].EndDate.ToString("dd-MMM-yyyy", new CultureInfo("en"));
                iCnt++;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This function checks the preconditons for Standardwise Term Configuration Details configuration.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.SchoolwiseTermConfiguration);
        if (sLinks.Equals(""))
        {
            divErr.Visible = false;
            bReturn = true;
        }
        else
        {
            divErr.InnerHtml = sLinks;
            VisibleOrHideControls();
        }
        return bReturn;
    }

    /// <summary>
    /// This method is used to visible or hide controls depends on configuration is done or not.
    /// </summary>
    private void VisibleOrHideControls()
    {
        lstvwTermConfiguration.Visible = false;
        btnSave.Visible = false;
    }

    #endregion "Events"

    #region "private Methods"

    /// <summary>
    /// This method is used to fill SchoolwiseTermConfiguration listview.
    /// </summary>
    private void FillTermConfigurationListView()
    {
        SchoolwiseTermConfigurationMasterBL oSchoolwiseTermConfigurationMasterBL = new SchoolwiseTermConfigurationMasterBL(miSchoolId, miAcademicYearId);
        oSchoolwiseTermConfigurationMasterBL.GetAllTermDetails();
        lstStandardwiseAcademicYearDates = oSchoolwiseTermConfigurationMasterBL.lstStandardwiseAcademicYearDates;
        
        lstvwTermConfiguration.DataSource = oSchoolwiseTermConfigurationMasterBL.lstSchoolwiseTermConfigurationDetails;
        lstvwTermConfiguration.DataBind();
        hidRowCount.Value = lstvwTermConfiguration.Items.Count.ToString();
    }

    /// <summary>
    /// This mehod is used to save Schoolwise Term configuration details.
    /// </summary>
    private int Save()
    {
        SchoolwiseTermConfigurationMasterBL oSchoolwiseTermConfigurationMasterBL = new SchoolwiseTermConfigurationMasterBL(miSchoolId, miAcademicYearId);
        int iCount = oSchoolwiseTermConfigurationMasterBL.SaveSchoolwiseTermDetails(GetTermConfigDetailsXML(PopulateTermInfo()), miUserId,Convert.ToInt32(Constants.SchoolConfigurations.SchoolwiseTermConfiguration));

        lblMessage.Visible = true;
        lblMessage.Text = Resources.LocalizedResources.TermConfigurationSavedSuccessfully;
        lblMessage.ForeColor = System.Drawing.Color.Blue;
        lblMessage.Font.Bold = true;

        FillTermConfigurationListView();        
        return iCount;
    }

    /// <summary>
    /// This method is used to fill list of schoolwise term configuration details
    /// </summary>
    /// <returns></returns>
    private List<SchoolwiseTermConfigurationDetails> PopulateTermInfo()
    {
        SchoolwiseTermConfigurationDetails oSchoolwiseTermConfigurationDetails = null;
        List<SchoolwiseTermConfigurationDetails> lstTermInfo = new List<SchoolwiseTermConfigurationDetails>();
        
        for (int iRowNo = 0; iRowNo < lstvwTermConfiguration.Items.Count; iRowNo++)
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)lstvwTermConfiguration.Items[iRowNo];
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
            
            HiddenField iTermIId = oCurrentItem.FindControl("hidTrmIId") as HiddenField;
            HiddenField iSchoolwiseTermIId = oCurrentItem.FindControl("hidSchoolwiseTermIId") as HiddenField;
            HiddenField iTermIIId = oCurrentItem.FindControl("hidTrmIIId") as HiddenField;
            HiddenField iSchoolwiseTermIIId = oCurrentItem.FindControl("hidSchoolwiseTermIIId") as HiddenField;
            TextBox txtTerm1StartDate = oCurrentItem.FindControl("txtTerm1StartDate") as TextBox;
            TextBox txtTerm2StartDate = oCurrentItem.FindControl("txtTerm2StartDate") as TextBox;
            TextBox txtTerm1EndDate = oCurrentItem.FindControl("txtTerm1EndDate") as TextBox;
            TextBox txtTerm2EndDate = oCurrentItem.FindControl("txtTerm2EndDate") as TextBox;
            int iStandardId = Convert.ToInt32(lstvwTermConfiguration.DataKeys[iRowId]["StandardId"]);

            if (!string.IsNullOrEmpty(txtTerm1StartDate.Text) || !string.IsNullOrEmpty(txtTerm1EndDate.Text)
               || !string.IsNullOrEmpty(txtTerm2StartDate.Text) || !string.IsNullOrEmpty(txtTerm2EndDate.Text))
            {
                oSchoolwiseTermConfigurationDetails = new SchoolwiseTermConfigurationDetails();
                oSchoolwiseTermConfigurationDetails.StandardId = iStandardId;

                // Populate TermConfigurationDetails Object with Term1 details.
                if (!string.IsNullOrEmpty(txtTerm1StartDate.Text) || !string.IsNullOrEmpty(txtTerm1EndDate.Text))
                {
                    TermConfigurationDetails oTerm1ConfigurationDetails = new TermConfigurationDetails();
                    
                    oTerm1ConfigurationDetails.TermId = Convert.ToInt32(iTermIId.Value.ToString());
                    oTerm1ConfigurationDetails.SchoolwiseTermId = Convert.ToInt32(iSchoolwiseTermIId.Value);
                    if (!string.IsNullOrEmpty(txtTerm1StartDate.Text))
                        oTerm1ConfigurationDetails.TermStartDate = Convert.ToDateTime(txtTerm1StartDate.Text);
                    else
                        oTerm1ConfigurationDetails.TermStartDate = Convert.ToDateTime(S_DEFAULT_DATETIME);
                    if (!string.IsNullOrEmpty(txtTerm1EndDate.Text))
                        oTerm1ConfigurationDetails.TermEndDate = Convert.ToDateTime(txtTerm1EndDate.Text);
                    else
                        oTerm1ConfigurationDetails.TermEndDate = Convert.ToDateTime(S_DEFAULT_DATETIME);
                    oTerm1ConfigurationDetails.Is_Deleted = false;

                    oSchoolwiseTermConfigurationDetails.TermIInfo = oTerm1ConfigurationDetails;
                }
                // Populate TermConfigurationDetails Object with Term2 details.
                if (!string.IsNullOrEmpty(txtTerm2StartDate.Text) || !string.IsNullOrEmpty(txtTerm2EndDate.Text))
                {
                    TermConfigurationDetails oTerm2ConfigurationDetails = new TermConfigurationDetails();              
                   
                    oTerm2ConfigurationDetails.TermId = Convert.ToInt32(iTermIIId.Value.ToString());
                    oTerm2ConfigurationDetails.SchoolwiseTermId = Convert.ToInt32(iSchoolwiseTermIId.Value);
                    if (!string.IsNullOrEmpty(txtTerm2StartDate.Text))
                        oTerm2ConfigurationDetails.TermStartDate = Convert.ToDateTime(txtTerm2StartDate.Text);
                    else
                        oTerm2ConfigurationDetails.TermStartDate = Convert.ToDateTime(S_DEFAULT_DATETIME);
                    if (!string.IsNullOrEmpty(txtTerm2EndDate.Text))
                        oTerm2ConfigurationDetails.TermEndDate = Convert.ToDateTime(txtTerm2EndDate.Text);
                    else
                        oTerm2ConfigurationDetails.TermEndDate = Convert.ToDateTime(S_DEFAULT_DATETIME);
                    oTerm2ConfigurationDetails.Is_Deleted = false;
                    
                    oSchoolwiseTermConfigurationDetails.TermIIInfo = oTerm2ConfigurationDetails;                    
                }
                lstTermInfo.Add(oSchoolwiseTermConfigurationDetails);
            }
        }
        return lstTermInfo;
    }

    /// <summary>
    /// This method is used to generate XML.
    /// </summary>
    /// <param name="oSchoolwiseTermConfigurationDetails"></param>
    /// <returns></returns>
    private string GetTermConfigDetailsXML(List<SchoolwiseTermConfigurationDetails> lstSchoolwiseTermConfigurationDetails)
    {
        StringWriter sw = new StringWriter();
        new XmlSerializer(lstSchoolwiseTermConfigurationDetails.GetType()).Serialize(sw, lstSchoolwiseTermConfigurationDetails);
        string sXML = sw.ToString();
        sXML = sXML.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", string.Empty);
        return sXML;
    }

    /// <summary>
    /// This method is used to decrypt query string.
    /// </summary>
    /// <returns></returns>
    private string ReadQuerystring()
    {
        return QueryString["Is_Configured"];
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        TextBox txtT1StartDate = lstvwTermConfiguration.Items[0].FindControl("txtTerm1StartDate") as TextBox;
        txtT1StartDate.Focus();
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel });
        btnSave.Attributes.Add("onclick", "ClearMessage()");
    }
    /// <summary>
    /// This method is used to change the design according to selected language.
    /// </summary>
    private void DesignSettingAccordingLanguage()
    {
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        hidValTermIEndDateShouldBeGreaterThanTermIStartDate.Value =Resources.LocalizedResources.ValTermIEndDateShouldBegreaterThanTermIStartDate;
        hidValTermIIEndDateShouldBeGreaterThanTermIIStartDate.Value = Resources.LocalizedResources.ValTermIIEndDateShouldBegreaterThanTermIIStartDate;
        hidValTermIIStartDateShouldBeGreaterThanTermIEndDate.Value = Resources.LocalizedResources.ValTermIIStartDateShouldBeGreaterThanTermIEndDate;
        hidAnd.Value = Resources.LocalizedResources.And;
        hidForStandard.Value = Resources.LocalizedResources.ForStandard;
        hidTermIEndDateShouldBeInBetween.Value = Resources.LocalizedResources.TermIEndDateShouldBeInBetween;
        hidTermIIStartDateShouldBeInBetween.Value = Resources.LocalizedResources.TermIIStartDateShouldBeInBetween;
        hidTermIIEndDateShouldBeInBetween.Value = Resources.LocalizedResources.TermIIEndDateShouldBeInBetween;
        hidValTermIStartDateShouldBeInBetween.Value = Resources.LocalizedResources.ValTermIStartDateShouldBeInBetween;
    }
    #endregion "Private Method"
}
