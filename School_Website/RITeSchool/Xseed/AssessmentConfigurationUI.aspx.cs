/* File Name = AssessmentConfigurationUI.aspx.cs
 * Created Date - 
 * Modified Date  - 24 May 2011
 * Created by - Vipul
 * Class Description - This class is defined to manage assessment details.*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BusinessLogic.Exceptions;
using XseedReportEntities;
using Utility;
using BusinessLogic;
using System.Resources;
using System.Globalization;

public partial class AssessmentConfigurationUI : SchoolBase
{
    private ResourceManager oResourceManager = new ResourceManager(typeof(Resources.LocalizedResources));
    #region "Constants"

    const string S_DEFAULT_SORT_EXP = "Name";
    const string S_COMMAND_REMOVE = "RemoveAssessment";
    const string S_COMMAND_UPDATE = "UpdateAssessment";
    const string S_ASSESSMENT_ID = "AssessmentId";
    const string S_NAME = "Name";
    const string S_ACTION = "Action";
    #endregion "Constants"

    #region "Events"

    /// <summary>
    /// This event is used to set default controls and fill assessment details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                SetDefaultFields();
                FillAssessmentDetailsListView();
                SetJavaScriptAttributres();
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                RefreshValue();
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValue();
            }

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save assessment details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            AssessmentMasterBL oAssessmentMasterBL = new AssessmentMasterBL();
            oAssessmentMasterBL.AssessmentMaster = PopulateAssessmentMaster();
            lblErrorMsg.Text = string.Empty;
            if (hidMode.Value != "Update")
            {
                oAssessmentMasterBL.InsertAssessmentMaster();
                lblUpdateSucess.Text = Resources.LocalizedResources.SuccessMsgAssessment;
            }
            else
            {
                oAssessmentMasterBL.UpdateAssessmentMaster();
                lblUpdateSucess.Text = Resources.LocalizedResources.UpdateMsgAssessment;
            }
            bool bIsConfigured = QueryString[Constants.S_IS_CONFIGURED] == Constants.S_YES;
            if (!bIsConfigured)
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.AssessmentConfiguration));
            FillAssessmentDetailsListView();
            lblCheckDependency.Text = string.Empty;
            lblUpdateSucess.Visible = true;
            if (lblErrorMsg.Text == string.Empty)
                SetDefaultFields();
            AddSortImage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set default controls and add sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            SetDefaultFields();
            AddSortImage();
            lblErrorMsg.Text = string.Empty;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to edit or delete assessment.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwAssessmentDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName != "Sort")
            {
                lblErrorMsg.Text = string.Empty;
                lblUpdateSucess.Text = string.Empty;
                lblCheckDependency.Text = string.Empty;
                AssessmentMaster oAssessmentMaster;
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iListIndex = oCurrentItem.DisplayIndex;
                int iAssessmentId = Convert.ToInt32(lstvwAssessmentDetails.DataKeys[iListIndex][S_ASSESSMENT_ID]);
                hidAssesmentId.Value = iAssessmentId.ToString();
                hidAssesmentName.Value = lstvwAssessmentDetails.DataKeys[iListIndex][S_NAME].ToString();
                hidRowNo.Value = (oCurrentItem.DisplayIndex + 1).ToString();
                if (e.CommandName == S_COMMAND_REMOVE)
                {
                    SetDefaultFields();

                    oAssessmentMaster = new AssessmentMaster()
                    {
                        AssessmentId = iAssessmentId,
                        Name = lstvwAssessmentDetails.DataKeys[iListIndex][S_NAME].ToString(),
                        Action = Constants.Action.Delete
                    };
                    List<AssessmentMaster> lstAssessmentMaster = new List<AssessmentMaster>();
                    lstAssessmentMaster.Add(oAssessmentMaster);
                    DeleteAssessmentDetails(lstAssessmentMaster);
                }
                else if (e.CommandName == S_COMMAND_UPDATE)
                    LoadAssessmentDetails();
                AddSortImage();
            }
        }
        catch (ReferenceExceptions ex)
        {
            lblErrorMsg.Text = CommonUtility.ModifyExceptionMessage(ex.Message, "Assessment", Resources.LocalizedResources.Assessment, "can not be removed since associated with", Resources.LocalizedResources.valRemoveText);
            FillAssessmentDetailsListView();
            AddSortImage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to add confirmation message while deleting existing assessment.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwAssessmentDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                ImageButton oimgbtnDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
                oimgbtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to sort assessment list view by assessment name, start date, end date.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwAssessmentDetails_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            hidSortDirection.Value = (hidSortDirection.Value == Constants.S_DESCENDING || hidSortExpression.Value != e.SortExpression)? Constants.S_ASCENDING
                                                : Constants.S_DESCENDING;
            hidSortExpression.Value = (e.SortExpression != string.Empty) ? e.SortExpression
                                                : S_DEFAULT_SORT_EXP;
            FillAssessmentDetailsListView();
            AddSortImage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to set list view row count.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwAssessmentDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            hidRowCount.Value = lstvwAssessmentDetails.Items.Count.ToString();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion "Events"

    #region "Private Methods"

    /// <summary>
    /// This method is used to set default controls.
    /// </summary>
    private void SetDefaultFields()
    {
        txtAssessment.Focus();
        btnSave.Text = Resources.LocalizedResources.Save;
        hidMode.Value = "Save";
        txtAssessment.Text = string.Empty;
        txtEndDate.Text = string.Empty;
        txtStartDate.Text = string.Empty;
        lblErrorMsg.Text = string.Empty;
        hidRowNo.Value = "0";
    }

    /// <summary>
    ///  This method is used to fill assessment details.
    /// </summary>
    private void FillAssessmentDetailsListView()
    {
        AssessmentMasterBL oAssessmentMasterBL = new AssessmentMasterBL();
        string sSortExpression = (string.IsNullOrEmpty(hidSortExpression.Value)) ? S_DEFAULT_SORT_EXP : hidSortExpression.Value;
        string sSortOrder = (string.IsNullOrEmpty(hidSortDirection.Value)) ? Constants.S_ASCENDING : hidSortDirection.Value;
        List<AssessmentMaster> lstAssessmentMaster = oAssessmentMasterBL.GetAssessmentDetailsList(sSortExpression, sSortOrder, miSchoolId, miAcademicYearId);
        lstvwAssessmentDetails.DataSource = lstAssessmentMaster;
        lstvwAssessmentDetails.DataBind();

        if (lstvwAssessmentDetails.Items.Count > 0)
        {
            divAssessmentDetails.Visible = true;
            SetDivHeight();
        }
        else
            divAssessmentDetails.Visible = false;
        hidAcademicYearStartDate.Value = oAssessmentMasterBL.SchoolwiseAcademicYrDates.AcademicYearStartDate.ToString("dd/MMM/yyyy",new CultureInfo("en"));
        hidAcademicYearEndDate.Value = oAssessmentMasterBL.SchoolwiseAcademicYrDates.AcademicYearEndDate.ToString("dd/MMM/yyyy", new CultureInfo("en"));
    }

    /// <summary>
    ///  This method is used to populate AssessmentMaster class
    /// </summary>
    /// <returns></returns>
    private AssessmentMaster PopulateAssessmentMaster()
    {
        AssessmentMaster oAssessmentMaster = new AssessmentMaster();
        oAssessmentMaster.EndDate = Convert.ToDateTime(txtEndDate.Text);
        oAssessmentMaster.StartDate = Convert.ToDateTime(txtStartDate.Text);
        oAssessmentMaster.Name = txtAssessment.Text.Trim();
        oAssessmentMaster.SchoolId = miSchoolId;
        oAssessmentMaster.AcademicYearId = miAcademicYearId;
        oAssessmentMaster.InsertedById = miUserId;
        oAssessmentMaster.InsertDate = DateTime.Now.ToString(Constants.S_DATE_FORMAT_MARATHI);
        if (hidMode.Value != "Save")
        {
            oAssessmentMaster.AssessmentId = Convert.ToInt32(hidAssesmentId.Value);
            oAssessmentMaster.UpdatedById = miUserId;
            oAssessmentMaster.UpdateDate = DateTime.Now.ToString(Constants.S_DATE_FORMAT_MARATHI);
        }
        return oAssessmentMaster;
    }
  

    /// <summary>
    /// This method is used to delete assessment details.
    /// </summary>
    /// <param name="lstAssessmentMaster"></param>
    private void DeleteAssessmentDetails(List<AssessmentMaster> lstAssessmentMaster)
    {
        AssessmentMasterBL oAssessmentMasterBL = new AssessmentMasterBL();
        string sMessage = CheckDependencies(lstAssessmentMaster);
        if (string.IsNullOrEmpty(sMessage))
            oAssessmentMasterBL.DeleteAssessmentMaster(Convert.ToInt32(hidAssesmentId.Value),miUserId);
        else
            throw new ReferenceExceptions(sMessage);
        lblCheckDependency.Text = string.Empty;
        FillAssessmentDetailsListView();
        if (lstvwAssessmentDetails.Items.Count == 0)
            DeleteConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.AssessmentConfiguration));
    }

    /// <summary>
    /// This method is used to check dependencies.
    /// </summary>
    /// <param name="lstAssessmentMaster"></param>
    /// <param name="aiAcademicYearId"></param>
    /// <returns></returns>
    private string CheckDependencies(List<AssessmentMaster> lstAssessmentMaster)
    {
        GenericReferenceList<AssessmentMaster> objStdRefereces = new GenericReferenceList<AssessmentMaster>(lstAssessmentMaster, miAcademicYearId);
        return objStdRefereces.CheckDependenciesForList(S_ASSESSMENT_ID, S_NAME, S_ACTION, Constants.ReferenceId.AssessmentConfiguration, false);
    }

    /// <summary>
    /// This method is used to load assessment details.
    /// </summary>
    private void LoadAssessmentDetails()
    {
        AssessmentMasterBL oAssessmentMasterBL = new AssessmentMasterBL(Convert.ToInt32(hidAssesmentId.Value), miSchoolId, miAcademicYearId);
        txtAssessment.Text = oAssessmentMasterBL.AssessmentMaster.Name;
        txtStartDate.Text = oAssessmentMasterBL.AssessmentMaster.StartDate.ToString("dd-MMM-yyyy", new CultureInfo("en") );
        txtEndDate.Text = oAssessmentMasterBL.AssessmentMaster.EndDate.ToString("dd-MMM-yyyy", new CultureInfo("en"));
        btnSave.Text = Resources.LocalizedResources.Update;
        hidMode.Value = "Update";
    }

    /// <summary>
    /// This method is used to set javascript attributes for buttons.
    /// </summary>
    private void SetJavaScriptAttributres()
    {
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Xseed_Report_Related));
        btnSave.Attributes["onclick"] = "javascript:btnsaveonclick('" + btnSave.ClientID + "',this);";
        ApplyMouseHoverEffect(new List<Button> {btnCancel, btnSave,btnBack});
        AddSortImage();
    }

    /// <summary>
    /// This method is used set sort variables.
    /// </summary>
    private void SetSortVariables()
    {
        hidSortDirection.Value = (hidSortDirection.Value == Constants.S_DESCENDING) ? hidSortDirection.Value = Constants.S_ASCENDING
                                        : hidSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to set sorting image to list view headers.
    /// </summary>
    private void AddSortImage()
    {
        if (string.IsNullOrEmpty(hidSortDirection.Value))
            hidSortDirection.Value = Constants.S_ASCENDING;
        if (string.IsNullOrEmpty(hidSortExpression.Value))
            hidSortExpression.Value = S_DEFAULT_SORT_EXP;
        HtmlTableRow oHtmlTableHeaderRow = lstvwAssessmentDetails.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    /// <summary>
    /// This method is used to set div height according to records in the assessment listview.
    /// </summary>
    private void SetDivHeight()
    {
        if (lstvwAssessmentDetails.Items.Count < 6)
            divAssessmentDetails.Style.Add(Constants.S_HEIGHT, Convert.ToString(200));
        else if (lstvwAssessmentDetails.Items.Count < 10)
            divAssessmentDetails.Style.Add(Constants.S_HEIGHT, Convert.ToString(300));
        else
            divAssessmentDetails.Style.Add(Constants.S_HEIGHT, Convert.ToString(400));
    }

    private void RefreshValue()
    { 
        btnSave.Text = oResourceManager.GetString(hidMode.Value.Replace(" ", string.Empty));
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        hidAlertForAssessment.Value = Resources.LocalizedResources.AlertForAssessment;
        hidvalForEndDateGreater.Value = Resources.LocalizedResources.valForEndDateGreater;
        hidValForDuplicateAssessment.Value = Resources.LocalizedResources.ValForDuplicateAssessment;
        hidValStartDateEndDate.Value = Resources.LocalizedResources.ValStartDateEndDate;
        hidAnd.Value = Resources.LocalizedResources.And;
    }

    #endregion "Private Methods"
}