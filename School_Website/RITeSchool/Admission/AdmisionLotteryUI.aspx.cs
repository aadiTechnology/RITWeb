// File Name   : AdmisionLotteryUI.aspx.cs
// Created By  : Shankar
// Date        : 08/12/2009
// Description : This class is used to withdraw lottery from the list of student admission forms.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Threading;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using CrystalDecisions.Shared;
using Utility;
using SchoolAutoSearchService.Service;
using System.Linq;
using SchoolEntities;

public partial class AdmisionLotteryUI : SchoolBase
{
    #region " Constants "

    private const string S_DEFAULT_SORT_EXP = "Form_Number";
	private const string S_SUCCESS_MSG = "SMS has been sent successfully !!!";
	private const string S_FAILED_MESG = "SMS is not sent to students of Form no : ";
	private const string S_SORT_DIRECTION_DESC = "Descending";
	private const string S_SORT_DIRECTION_ASC = "Ascending";
    string S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR = "N";
    #endregion " Constants "

    #region " Events Handlers "

    /// <summary>
    /// This event is used to fill details of newly admitted student.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {          
            S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR = Settings.ShowAdmissionForCurrentYear ? Constants.S_YES : Constants.S_NO;
            if (!IsPostBack)
            {
                if (CheckPreCondition())
                {
                    GetNewAcadamicYearID();
                    FillStandardCombobox();
                    SetDefaultProperties();
                    FillSiblingFilter();
					GetScreenConfigDetails();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    private void FillSiblingFilter()
    {
        StudentAdmissionsBL oStudentAdmissionsBL = new StudentAdmissionsBL();
        List<SiblingFilter> lstSiblingFilters = oStudentAdmissionsBL.GetAllSiblingFilters();
        ListSource.FillDropDownList(lstSiblingFilters, cmbSiblings, "Name", "Id", string.Empty);
    }

    /// <summary>
    /// This Event is used to fill listviews according to name or form No.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            DisplayStudentLists();
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

   

    /// <summary>
    /// This event is used to cancel regeneration of lottery.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ddlStandard.SelectedIndex = 0;
            ResetListviews();
            DtPgCount.Visible = false;
            DtWaitingListPgCount.Visible = false;
            SetControlsForGenerateLottery(false);
            DisplayMainListDetails(false);
            DisplayWaitingListDetails(false);
			chkSendSMS.Visible = chkSendSMS.Checked = chkDisplayWaitingListConfirmed.Checked = chkDisplayMainListConfirmed.Checked = false;
			txtMainListCount.Text = txtName.Text = txtWaitingListCount.Text = string.Empty;
			divErr.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to handle standard drop down change event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlStandard.SelectedValue != "0")
            {
                rdoFinal.Checked = true;
                FillStandardPopupCombo(Convert.ToInt32(ddlStandard.SelectedValue));
                DisplayStudentLists();
                Showlistviews(true);
                string sAdmissionConfirmSMS = GetSMSTemplate(Convert.ToInt32(Constants.SMSTemplate.SelectedInLotterySMS));
                btnSendSmsToMainListStudent.Attributes.Add("onclick", "if(!ConfirmSMSSending('" + sAdmissionConfirmSMS + "')) return false;");
                btnSendSmsToWaitingListStudent.Attributes.Add("onclick", "if(!ConfirmSMSSending('" + sAdmissionConfirmSMS + "')) return false;");
                SetStudentListbuttonState();            
            }
            else
            {
                ResetListviews();
                DisplayMainListDetails(false);
                DisplayWaitingListDetails(false);
                btnConsolidatedStudentList.Visible = false;
            }

            divErr.Visible = false;
            trPrecondition.Visible = false;
        }
        catch (Exception ex)
        {
          ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to calculate total students of selected locations.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chklstLivingLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GetTotalNewStudentCountOfStd();
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display report of main list.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPrintMainList_Click(object sender, EventArgs e)
    {
        try
        {
            DisplayReport("M", Convert.ToInt32(chkDisplayMainListConfirmed.Checked));
        }
        catch (ThreadAbortException)
        { }
        catch (Exception ex)
        {
          ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display report of waiting list.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPrintWaitingList_Click(object sender, EventArgs e)
    {
        try
        {
            DisplayReport("W", Convert.ToInt32(chkDisplayWaitingListConfirmed.Checked));
        }
        catch (ThreadAbortException)
        { }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to regenarate lottery.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRegenerate_Click(object sender, EventArgs e)
    {
        try
        {
            SetControlsForGenerateLottery(true);
            btnCancel.Visible = true;
            btnAddWaitingList.Visible = false;
            btnPrintWaitingList.Visible = false;
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to genarate lottery.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        int iAcademicYearID = Convert.ToInt32(hidNextAcademiYearId.Value);
        int iStandard = Convert.ToInt32(ddlStandard.SelectedValue);
        int iMainListCount = 0;
        int iWaitingListCount = 0;

        if (txtMainListCount.Text != string.Empty)
            iMainListCount = Convert.ToInt32(txtMainListCount.Text);

        if (txtWaitingListCount.Text != string.Empty)
            iWaitingListCount = Convert.ToInt32(txtWaitingListCount.Text);

        string sResidenceIds = GetResidenceIds();
        string sLocationIds = hidLocationIds.Value;        
        StudentAdmissionsBL oStudentAdmissionsBL = new StudentAdmissionsBL();
        oStudentAdmissionsBL.GenerateAdmissionLottery(iMainListCount, iWaitingListCount, miSchoolId, iAcademicYearID, iStandard, sLocationIds, cmbSiblings.SelectedValue.ToInt(), sResidenceIds);
        hidRenerateLottery.Value = "N";
        SetControlsForGenerateLottery(false);
        DisplayStudentLists();
      
    }

    /// <summary>
    /// This event is used to fill listview page foolter and add sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwMainList_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwMainList.Items.Count > 0 && ddlStandard.SelectedValue != Constants.I_ZERO.ToString())            
			{
                ControlUtility.FillListViewPagerFooter(lstvwMainList, DtPgCount);
				AddSortImage();
				if (DtPgCount.TotalRowCount > Constants.I_GRID_PAGE_COUNT)
					DtPgCount.Visible = true;
            }
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to ser sort expression and direction.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwMainList_Sorting(object sender, ListViewSortEventArgs e)
    {
		hidMainSortExpression.Value = e.SortExpression;
		hidMainSortDirection.Value = e.SortDirection.ToString() == S_SORT_DIRECTION_DESC ? Constants.S_DESCENDING : Constants.S_ASCENDING;
    }

    /// <summary>
    /// This event is used to fill listview pager footer and set sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwWaitingList_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwWaitingList.Items.Count > 0 && ddlStandard.SelectedValue != Constants.I_ZERO.ToString())
            {
                ControlUtility.FillListViewPagerFooter(lstvwWaitingList, DtWaitingListPgCount);
				AddSortImageForWailtingList();
				if (DtWaitingListPgCount.TotalRowCount > Constants.I_GRID_PAGE_COUNT)
					DtWaitingListPgCount.Visible = true;
            }
            else
                DtWaitingListPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set values to listview's page number label.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwMainList);
        }
        catch (Exception ex)
        {
          ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

	/// <summary>
	/// This event is used to set values to listview's page number label.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    protected void ddlCnt1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwWaitingList);
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set sort expresion and sort direction.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwWaitingList_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {            
            hidWaitingSortExpression.Value = e.SortExpression;
			hidWaitingSortDirection.Value = e.SortDirection.ToString() == S_SORT_DIRECTION_DESC ? Constants.S_DESCENDING : Constants.S_ASCENDING;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set controls to columns.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwMainList_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                DataRowView oDataRowView = (System.Data.DataRowView)oCurrentItem.DataItem;

                CheckBox oCheckBox = (CheckBox)oCurrentItem.FindControl("chkSelect");
                bool bIsConfirmed = Convert.ToBoolean(oDataRowView["IsConfirmed"]);
                if (bIsConfirmed)
                {
                    oCheckBox.Visible = false;
                    Image imgConfirm = (Image)oCurrentItem.FindControl("imgConfirm");
                    imgConfirm.Visible = true;
                }

                btnAdd.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set controls to columns.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwWaitingList_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                DataRowView oDataRowView = (System.Data.DataRowView)oCurrentItem.DataItem;
                CheckBox oCheckBox = (CheckBox)oCurrentItem.FindControl("chkSelect");
                bool bIsConfirmed = Convert.ToBoolean(oDataRowView["IsConfirmed"]);
                if (bIsConfirmed)
                {
                    oCheckBox.Visible = false;
                    Image imgConfirm = (Image)oCurrentItem.FindControl("imgConfirm");
                    imgConfirm.Visible = true;
                }

                btnAddWaitingList.Visible = true;
            }
        }
        catch (Exception ex)
        {
      ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

   

	/// <summary>
	/// This event is used to send sms to students in main list.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnSendSmsToMainListStudent_Click(object sender, EventArgs e)
	{
		try
		{
			string sMessage = string.Empty;
			if (hidSendMsg.Value == Constants.S_YES)
			{
				sMessage = SendSMS("M");
				if (sMessage != string.Empty)
					lblErrorMsg.Text = S_FAILED_MESG + sMessage;
				else
				{
					lblErrorMsg.Text = S_SUCCESS_MSG;
					lblErrorMsg.ForeColor = System.Drawing.Color.Blue;
					lblErrorMsg.Font.Bold = true;
				}
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to send sms to students in waiting list.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnSendSmsToWaitingListStudent_Click(object sender, EventArgs e)
	{
		try
		{
			string sMessage = string.Empty;
			if (hidSendMsg.Value == Constants.S_YES)
			{
				sMessage = SendSMS("W");
				if (sMessage != string.Empty)
					lblErrorMsg.Text = S_FAILED_MESG + sMessage;
				else
				{
					lblErrorMsg.Text = S_SUCCESS_MSG;
					lblErrorMsg.ForeColor = System.Drawing.Color.Blue;
					lblErrorMsg.Font.Bold = true;
				}
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}
	
	/// <summary>
	/// This event is used to publish the lottery.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnPublish_Click(object sender, EventArgs e)
	{
        try
        {
            string sMessage = string.Empty;
            if (Convert.ToBoolean(hidSendPublishSMS.Value))
            {
                sMessage = SendSMS(string.Empty);
                if (sMessage != string.Empty)
                    lblErrorMsg.Text = "SMS is not sent to students of form no : " + sMessage;
                else
                {
                    lblErrorMsg.Text = "SMS has been sent successfully!!!";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Blue;
                }
            }

            int iAcademicYearId = Convert.ToInt32(hidNextAcademiYearId.Value);
            int iStandardId = Convert.ToInt32(ddlStandard.SelectedValue);
            UpdateConfirmationStatus(miSchoolId, iAcademicYearId, iStandardId);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
	}

	/// <summary>
	/// This event is used to display confirmed students in main list.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void chkDisplayMainListConfirmed_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			DisplayStudentLists();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

    /// <summary>
    /// This event is used to open student admission list report.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnConsolidatedStudentList_Click(object sender, EventArgs e)
    {
        try
        {
           DisplayReport();
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save selected details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (hidIsWaitingList.Value == Constants.S_ONE)
            {
                SaveStudentDetails(lstvwWaitingList);
                if (hidIsConfigured.Value == Constants.S_NO)
                {
                    SaveConfig(Constants.SchoolConfigurations.Student.ToInt());
                    hidIsConfigured.Value = Constants.S_YES;
                }
            }
            else
            {
                SaveStudentDetails(lstvwMainList);
                if (hidIsConfigured.Value == Constants.S_NO)
                {
                    SaveConfig(Constants.SchoolConfigurations.Student.ToInt());
                    hidIsConfigured.Value = Constants.S_YES;
                }
            }

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion " Events "

    #region " Private Methods "

	/// <summary>
	/// This method is used to check weather studentui is configured or not. If student is confirm then set is_configured flag to 'Y' for studentui.
	/// </summary>
	private void GetScreenConfigDetails()
	{
		int iScreenLevel = Convert.ToInt32(Constants.ScreenLevel.Configuration);
		MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
		DataTable oDSUserDetails = oMasterDataCollectionBL.GetConfigurationDetails(miSchoolId, hidNextAcademiYearId.Value.ToInt(), miFinancialYearId, Constants.SchoolConfigMenuId.Other_User_Related.ToInt(), iScreenLevel, miUserId, moUserRole.ToInt());
		if (oDSUserDetails.Rows.Count > 0 && oDSUserDetails.Rows[0]["Is_Configure"] != null)
			hidIsConfigured.Value = oDSUserDetails.Rows[0]["Is_Configure"].ToString();
	}

    /// <summary>
    /// This method is used to fill all combo with all standard of school.
    /// </summary>
    private void FillStandardCombobox()
    {
        const int I_STANDARDS = 0;
        const int I_LIVING_LOCATIONS = 1;
        const int I_RESIDENCE_TYPES = 2;
        int iAcademicYearId = Convert.ToInt32(hidNextAcademiYearId.Value);
        DataSet oDSStandardsAndLocations = StudentAdmissionsBL.GetStandardAndLocations(miSchoolId, iAcademicYearId);
        if (oDSStandardsAndLocations != null && oDSStandardsAndLocations.Tables.Count > 0)
        {
            DataTable oDTStandards = oDSStandardsAndLocations.Tables[I_STANDARDS];
			ControlUtility.FillDropDownList(oDTStandards, ref ddlStandard, Constants.S_STANDARD_ID_FIELD, Constants.S_STANDARD_NAME_FIELD, Constants.S_SELECT);
			ControlUtility.FillCheckBoxList(oDSStandardsAndLocations.Tables[I_LIVING_LOCATIONS], ref chklstLivingLocation, "LivingLocationId", "LivingLocationName", true);
            ControlUtility.FillCheckBoxList(oDSStandardsAndLocations.Tables[I_RESIDENCE_TYPES] , ref chklstResidenceTypes, "ResidenceTypeId", "Name", true);
        }
        
        ResetListviews();
    }

    /// <summary>
    /// This method is used to reset listview.
    /// </summary>
    private void ResetListviews()
    {
        lstvwMainList.DataSourceID = null;        
        lstvwWaitingList.DataSourceID = null;        
    }

    /// <summary>
    /// This method is used to set default properties of controls. 
    /// </summary>
    private void SetDefaultProperties()
    {
        ApplyMouseHoverEffect(
            new List<Button>
            {
             btnRegenerate,
             btnGenerate,
             btnAdd,
             btnAddWaitingList,
             btnShow,
             btnCancel,
             btnSendSmsToMainListStudent,
             btnSendSmsToWaitingListStudent,
             btnPrintMainList,
             btnPrintWaitingList,
             btnPublish,
             });
        valSumGenerateErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        valSumErrorMsg0.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        hidMainSortDirection.Value = Constants.S_ASCENDING;
        hidMainSortExpression.Value = S_DEFAULT_SORT_EXP;
        hidWaitingSortDirection.Value = Constants.S_ASCENDING;
        hidWaitingSortExpression.Value = S_DEFAULT_SORT_EXP;
        string sSMSTemplateText = GetSMSTemplate(Convert.ToInt32(Constants.SMSTemplate.AdmissionConfirmationSMS));
        hidProvisionalConfirmation.Value = GetSMSTemplate(Convert.ToInt32(Constants.SMSTemplate.AdmissionProvisionalConfirmationSMS));   

        SetFieldVisibility();

        btnSave.Attributes.Add("onclick", "if(!ShowConfirmation(this,'" + sSMSTemplateText.Trim().Replace("'", "\\'").Replace("\"", "\\'\\'") + "'))return false;");
        btnAdd.Attributes.Add("onclick", "OpenDisionSelectionPopup(0); return false;");
        btnAddWaitingList.Attributes.Add("onclick", "OpenDisionSelectionPopup(1); return false;");
    }

    private void SetFieldVisibility()
    {
        SchoolBL oSchoolBL = new SchoolBL();
        Dictionary<int, YearwiseSchoolSettings> dictAllAcademicYearSettings = oSchoolBL.GetSchoolSettings(miSchoolId);
        YearwiseSchoolSettings oYearwiseSchoolSettings = dictAllAcademicYearSettings[hidNextAcademiYearId.Value.ToInt()];
        spnMainCountStar.Visible = spnWaitingCountStar.Visible = oYearwiseSchoolSettings.GenerateCountBasedLottery;
        hidShowCountValidation.Value = (oYearwiseSchoolSettings.GenerateCountBasedLottery ? Constants.S_YES : Constants.S_NO);
        if (!oYearwiseSchoolSettings.GenerateCountBasedLottery)
        {
            txtMainListCount.Text = Constants.S_ZERO;
            txtWaitingListCount.Text = Constants.S_ZERO;
            txtMainListCount.Enabled = false;
            txtWaitingListCount.Enabled = false;
        }
    }

    /// <summary>
    /// This method is used to fill listwiews.
    /// </summary>
    private void DisplayStudentLists()
    {
        GetTotalNewStudentCountOfStd();
        DataPager pager = lstvwMainList.FindControl("DtPgDropDown") as DataPager;
        if (pager != null)
            pager.SetPageProperties(0, pager.PageSize, true);
        lstvwMainList.DataSourceID = lstvwObjDS.ID;
        lstvwMainList.DataBind();
        hidMainListItemCount.Value = lstvwMainList.Items.Count.ToString();

        pager = lstvwWaitingList.FindControl("DtPgDropDown") as DataPager;
        if (pager != null)
            pager.SetPageProperties(0, pager.PageSize, true);
        lstvwWaitingList.DataSourceID = lstvwWaitingListObj.ID;
        lstvwWaitingList.DataBind();
        hidWaitingListItemCount.Value = lstvwWaitingList.Items.Count.ToString();

        if (lstvwMainList.Items.Count > 0 || lstvwWaitingList.Items.Count > 0)
            chkSendSMS.Visible = true;
        else
            chkSendSMS.Visible = false;

		if (lstvwWaitingList.Items.Count == 0)
			DisplayWaitingListDetails(false);
		else
			DisplayWaitingListDetails(true);

		if (lstvwMainList.Items.Count == 0)
			DisplayMainListDetails(false);
		else
			DisplayMainListDetails(true);
    }

    /// <summary>
    /// This method is used to fill Division combo box.
    /// </summary>
    /// <param name="aiStandardId"></param>
    private void FillStandardPopupCombo(int aiStandardId)
    {
        int iAcademicYearID = Convert.ToInt32(hidNextAcademiYearId.Value);
        DivisionCollectionBL oDivisionMasterBL = new DivisionCollectionBL(miSchoolId, iAcademicYearID);
        DataTable oDtDivisionCollection = oDivisionMasterBL.GetAllDivisionsForStandard(aiStandardId);
        ControlUtility.FillDropDownList(oDtDivisionCollection, ref cmbStandardNamePopup, Constants.S_DIVISION_ID_FIELD, Constants.S_DIVISION_NAME_FIELD, string.Empty);
    }
    /// <summary>
    /// This method is used to get count of new student for selected standard.
    /// </summary>
    private void GetTotalNewStudentCountOfStd()
    {
        if (ddlStandard.SelectedValue != "0")
        {
            int iAcademicYearID = Convert.ToInt32(hidNextAcademiYearId.Value);
            string sLocationIds = GetLocationIds();
            hidLocationIds.Value = string.Empty;
            hidLocationIds.Value = sLocationIds;
            string sResidenceIds = GetResidenceIds();           
            if (sLocationIds != string.Empty)
            {
                DataSet oDataSet = StudentBL.GetNewAdmissionsCount(miSchoolId, iAcademicYearID, Convert.ToInt32(ddlStandard.SelectedValue), 0, sLocationIds,sResidenceIds);
                if (oDataSet != null && oDataSet.Tables.Count > 0)
                {
                    if (oDataSet.Tables[0].Rows.Count > 0 && oDataSet.Tables[0].Rows[0][0] != DBNull.Value)
                        hidTotalStudentOfStd.Value = oDataSet.Tables[0].Rows[0][0].ToString();
                    if (oDataSet.Tables[2].Rows.Count > 0 && oDataSet.Tables[2].Rows[0][0] != DBNull.Value)
                    {
                        int iRecordCount = Convert.ToInt32(oDataSet.Tables[2].Rows[0][0]);
                        if (iRecordCount > 0)
                            hidRenerateLottery.Value = "N";
                        else
                        {
                            hidRenerateLottery.Value = "Y";
                            SetControlsForGenerateLottery(true);
                            btnCancel.Visible = true;
                            btnAddWaitingList.Visible = false;
                            btnPrintWaitingList.Visible = false;
                        }
                    }

                    if (oDataSet.Tables[1].Rows.Count > 0 && oDataSet.Tables[1].Rows[0][0] != DBNull.Value)
                    {
                        int iRecordCount = Convert.ToInt32(oDataSet.Tables[1].Rows[0][0]);
                        if (iRecordCount > 0)
                            hidRenerateLottery.Value = "Y";
                    }                   
                }
            }
        }
    }

    /// <summary>
    /// This method is used to collect selected locations.
    /// </summary>
    /// <returns></returns>
    private string GetLocationIds()
    {
        string sLocationIds = string.Empty;
        int iLocationCount = chklstLivingLocation.Items.Count;
        for (int iItemIndex = 0; iItemIndex < iLocationCount; iItemIndex++)
        {
            if (chklstLivingLocation.Items[iItemIndex].Selected)
                sLocationIds = sLocationIds + "," + chklstLivingLocation.Items[iItemIndex].Value;
        }

        if (sLocationIds.Length > 0)
            sLocationIds = sLocationIds.Substring(1);
        return sLocationIds;
    }
    /// <summary>
    ///  This method is used to Collect Selected Residence Types.
    /// </summary>
    /// <returns></returns>
    private string GetResidenceIds()
    {
        string sResidenceIds = string.Empty;
        if (chklstResidenceTypes.Visible)
        {   
            for (int iItemIndex = 0; iItemIndex < chklstResidenceTypes.Items.Count; iItemIndex++)
            {
                if (chklstResidenceTypes.Items[iItemIndex].Selected)
                    sResidenceIds = sResidenceIds + "," + chklstResidenceTypes.Items[iItemIndex].Value;
            }
        }
        else
        {   
            for (int iItemIndex = 0; iItemIndex < chklstResidenceTypes.Items.Count; iItemIndex++)
              sResidenceIds = sResidenceIds + "," + chklstResidenceTypes.Items[iItemIndex].Value;
        }

        if (sResidenceIds.Length > 0)
            sResidenceIds = sResidenceIds.Substring(1);
        return sResidenceIds;
    }
    /// <summary>
    /// This method is used set sorting direction.
    /// </summary>
    private void SetMainListSortVariables()
    {
		hidMainSortDirection.Value = hidMainSortDirection.Value == Constants.S_DESCENDING ? Constants.S_ASCENDING : Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used set sorting direction.
    /// </summary>
    private void SetWaitingListSortVariables()
    {
        if (hidWaitingSortDirection.Value == Constants.S_DESCENDING)
            hidWaitingSortDirection.Value = Constants.S_ASCENDING;
        else
            hidWaitingSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to ger new academic year id.
    /// </summary>
    private void GetNewAcadamicYearID()
    {
        // Table Indices
        const int S_TBL_NEW_ACADAMIC_YEAR = 0;
        SchoolWiseAcademicYearMasterBL oSchoolWiseAcademicYearMasterBL = new SchoolWiseAcademicYearMasterBL();
        
        DataSet oDSNextAcdemic = oSchoolWiseAcademicYearMasterBL.GetNextConfiguredAcademicYear(miSchoolId, S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR);
        if (oDSNextAcdemic != null && oDSNextAcdemic.Tables[S_TBL_NEW_ACADAMIC_YEAR].Rows.Count > 0)
        {
            if (oDSNextAcdemic.Tables[S_TBL_NEW_ACADAMIC_YEAR].Rows[0]["Academic_Year_Id"] != DBNull.Value)
                hidNextAcademiYearId.Value = oDSNextAcdemic.Tables[S_TBL_NEW_ACADAMIC_YEAR].Rows[0]["Academic_Year_Id"].ToString();
            else
                hidNextAcademiYearId.Value = "0";
        }
        else
            hidNextAcademiYearId.Value = "0";
    }

    /// <summary>
    /// This method is used to hide/show controls.
    /// </summary>
    /// <param name="bGenerate"></param>
    /// 
    private void SetControlsForGenerateLottery(bool bGenerate)
    {
        btnGenerate.Visible = bGenerate;
        if (hidRenerateLottery.Value == "Y")
        {
            btnRegenerate.Visible = false;
            btnPublish.Visible = false;
        }
        else
        {
            btnRegenerate.Visible = !bGenerate;
			btnPublish.Visible = !bGenerate;
        }

        btnConsolidatedStudentList.Visible = !bGenerate;
        trGenerateLottery.Visible = bGenerate;
        trCheckboxList.Visible = bGenerate;
        
        if (hidShowCountValidation.Value == Constants.S_NO)
        {
            trSiblingFilter.Visible = bGenerate;
            trResidenceList.Visible = bGenerate;
        }
        else
        {
            trSiblingFilter.Visible = false;
            trResidenceList.Visible = false;
        }

        trShowList.Visible = !bGenerate;
        btnCancel.Visible = false;
        txtName.Text = string.Empty;
        ddlStandard.Enabled = !bGenerate;
        trListviews.Visible = !bGenerate;

        for (int iItemIndex = 0; iItemIndex < chklstResidenceTypes.Items.Count; iItemIndex++)
            chklstResidenceTypes.Items[iItemIndex].Selected = true;

		if (lstvwMainList.Items.Count == 0)
			DisplayMainListDetails(false);
		else
			DisplayMainListDetails(true);

		if (lstvwWaitingList.Items.Count == 0)
			DisplayWaitingListDetails(false);
		else
			DisplayWaitingListDetails(true);
    }

	/// <summary>
	/// This method is used to show or hide main list controls.
	/// </summary>
	/// <param name="abAction"></param>
    private void DisplayMainListDetails(bool abAction)
    {
        trMainListSmsButton.Visible = abAction;
        btnPrintMainList.Visible = abAction;
        btnAdd.Visible = abAction;        
    }

	/// <summary>
	/// This method is used to show or hide waiting list controls.
	/// </summary>
	/// <param name="abAction"></param>
    private void DisplayWaitingListDetails(bool abAction)
    {
        trWaitingListSmsButton.Visible = abAction;
        btnPrintWaitingList.Visible = abAction;
        btnAddWaitingList.Visible = abAction;        
    }

	/// <summary>
    /// This method is used to show listviews.
    /// </summary>
    /// <param name="abAction"></param>
    private void Showlistviews(bool abAction)
    {
        trListviews.Visible = abAction;

        if (hidRenerateLottery.Value == "Y")
        {
            btnRegenerate.Visible = false;
            btnPublish.Visible = false;
        }
        else
        {
            btnRegenerate.Visible = abAction;
            btnPublish.Visible = abAction;
        }
    }

    /// <summary>
    /// This method is used to save student details.
    /// </summary>
    private void SaveStudentDetails(ListView aoListView)
    {
        if (CheckPreConditionForStandard())
        {
            StudentCollectionBL oStudentCollectionBL = new StudentCollectionBL();
            int iAcademicYearId = Convert.ToInt32(hidNextAcademiYearId.Value);
            int iStandardId = Convert.ToInt32(ddlStandard.SelectedValue);
            int iDivisionId = Convert.ToInt32(cmbStandardNamePopup.SelectedValue);
            int iUserRoleId = Convert.ToInt32(Constants.UserRoles.Student);
            string sStudentDetails = GenerateXml(iUserRoleId, aoListView);            
            DataTable oDataTable = oStudentCollectionBL.InsertMultipleStudents(miSchoolId, iAcademicYearId, miUserId, iStandardId, iDivisionId, sStudentDetails, iUserRoleId, S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR, Settings.AutoCalculateEnrolmentNo);

            if (miSchoolId == Constants.SchoolId.SVNP.ToInt())
                oDataTable = GetUserEnrolmentNumber(oDataTable);

            SendSMS(oDataTable);
            RefreshStudentCache(oDataTable);
            if (aoListView.ID == lstvwMainList.ID)
            {
                lstvwMainList.DataSourceID = lstvwObjDS.ID;
                lstvwMainList.DataBind();

                if (lstvwMainList.Items.Count == 0)
                    DisplayMainListDetails(false);
                else
                    DisplayMainListDetails(true);
            }
            else
            {
                lstvwWaitingList.DataSourceID = lstvwWaitingListObj.ID;
                lstvwWaitingList.DataBind();

				if (lstvwWaitingList.Items.Count == 0)
					DisplayWaitingListDetails(false);
				else
					DisplayWaitingListDetails(true);
            }

            if (lstvwMainList.Items.Count == 0 && lstvwWaitingList.Items.Count == 0)
                chkSendSMS.Visible = false;
            else
                chkSendSMS.Checked = false;			
        }
    }

    /// <summary>
    /// This Method is used to Update Students User Name & Password For SVNP school only.
    /// </summary>
    /// <param name="oDataTable"></param>
    /// <returns></returns>
    private DataTable GetUserEnrolmentNumber(DataTable oDataTable)
    {
        SchoolUserBL oSchoolUserBL = new SchoolUserBL();         
        string sEnrolmentNo = string.Empty;
        int iUserId = Constants.I_ZERO;
        string sPassword = string.Empty;
        string UpdatedPass = string.Empty;
        string sUserLogin = string.Empty;
        List<UserLoginDetails> lstUserLoginDetails = new List<UserLoginDetails>();

        for (int iRowCount = 0; iRowCount <= oDataTable.Rows.Count - 1; iRowCount++)
        {
            UserLoginDetails oUserLoginDetails = new UserLoginDetails();

            sEnrolmentNo = oDataTable.Rows[iRowCount]["Enrolment_Number"].ToString();
            iUserId = oDataTable.Rows[iRowCount]["UserId"].ToInt();
            sUserLogin = oDataTable.Rows[iRowCount]["UserLogin"].ToString();
            sPassword = CommonUtility.GetDecryptedPassword(sUserLogin, oDataTable.Rows[iRowCount]["UserPassword"].ToString());
            UpdatedPass = Utility.CommonUtility.GetEncryptedPassword(sEnrolmentNo, sPassword);            
            oDataTable.Rows[iRowCount]["UserLogin"] = sEnrolmentNo;
            oDataTable.Rows[iRowCount]["UserPassword"] = UpdatedPass;

            oUserLoginDetails.UserId = iUserId;
            oUserLoginDetails.UserLogin = sEnrolmentNo;
            oUserLoginDetails.Password = UpdatedPass;

            lstUserLoginDetails.Add(oUserLoginDetails);
        }

        oSchoolUserBL.UpdateStudentLoginDetails(lstUserLoginDetails);

        return oDataTable;
    }
	
	/// <summary>
	/// This method is used to get SMS Template.
	/// </summary>
	/// <param name="iSmsId"></param>
	/// <returns></returns>
    private string GetSMSTemplate(int iSmsId)
    {
        string sAdmissionConfirmSMS = string.Empty;
                
        int iAcademicYearId = Convert.ToInt32(hidNextAcademiYearId.Value);
        int iStandardId = Convert.ToInt32(ddlStandard.SelectedValue);
        DataTable oDTTemplate = SmsTemplateBL.GetTemplate(iSmsId, miSchoolId);
        if (oDTTemplate.Rows.Count != 0)
        {
            if (oDTTemplate.Rows[0][2] != DBNull.Value)
            {
                sAdmissionConfirmSMS = Convert.ToString(oDTTemplate.Rows[0][2]);
            }
        }

        if (iSmsId == Convert.ToInt32(Constants.SMSTemplate.SelectedInLotterySMS))
        {
            StudentAdmissionsBL oStudentAdmissionsBL = new StudentAdmissionsBL();
            DataTable oDataTable = oStudentAdmissionsBL.GetAdmissonDetailsOfAllStudents(miSchoolId, iAcademicYearId, iStandardId, "M", Constants.I_ONE);
            if (oDataTable.Rows.Count != 0)
            {
                if (oDataTable.Rows[0]["AdmissionLastDate"] != DBNull.Value)
                {
                    string sAdmissionLastDate = Convert.ToDateTime(oDataTable.Rows[0]["AdmissionLastDate"]).ToString("dd MMM yyyy");
                    sAdmissionConfirmSMS = sAdmissionConfirmSMS.Replace("%ADMISSIONDATE%", sAdmissionLastDate);
                }
            }
            else
            {
                oDataTable = oStudentAdmissionsBL.GetAdmissonDetailsOfAllStudents(miSchoolId, iAcademicYearId, iStandardId, "M", Constants.I_ZERO);
                if (oDataTable.Rows.Count != 0)
                {
                    if (oDataTable.Rows[0]["AdmissionLastDate"] != DBNull.Value)
                    {
                        string sAdmissionLastDate = Convert.ToDateTime(oDataTable.Rows[0]["AdmissionLastDate"]).ToString("dd MMM yyyy");
                        sAdmissionConfirmSMS = sAdmissionConfirmSMS.Replace("%ADMISSIONDATE%", sAdmissionLastDate);
                    }
                }
            }
        }

        return sAdmissionConfirmSMS;
    }

    /// <summary>
    /// This method is used to send SMS.
    /// </summary>
    /// <param name="oDataTable"></param>
    private void SendSMS(DataTable oDataTable)
    {
        string sAdmissionConfirmSMS = string.Empty;
        string sTemplateRegistrationId = string.Empty;
        string sSmsSubject = string.Empty;

        if (chkSendSMS.Checked)
        {
            if (oDataTable != null && oDataTable.Rows.Count > 0 && oDataTable.Rows[0][0] != DBNull.Value)
            {
                int iRowCount = oDataTable.Rows.Count;
                int iSMSType = 0;
                int iSmsId = Constants.I_ZERO;

                if(rdoFinal.Checked)
                    iSmsId = Convert.ToInt32(Constants.SMSTemplate.AdmissionConfirmationSMS);
                else if(rdoProvisional.Checked)
                    iSmsId = Convert.ToInt32(Constants.SMSTemplate.AdmissionProvisionalConfirmationSMS);

                DataTable oDTTemplate = SmsTemplateBL.GetTemplate(iSmsId, miSchoolId);
                if (oDTTemplate.Rows.Count != 0)
                {
                    if (oDTTemplate.Rows[0][2] != DBNull.Value)
                    {
                        sAdmissionConfirmSMS = Convert.ToString(oDTTemplate.Rows[0][2]);

                        if (oDTTemplate.Rows[0]["TemplateRegistrationId"] != DBNull.Value)
                            sTemplateRegistrationId = oDTTemplate.Rows[0]["TemplateRegistrationId"].ToString();

                        sSmsSubject = Convert.ToString(oDTTemplate.Rows[0][1]);
                    }

                    if (oDTTemplate.Rows[0][3] != DBNull.Value)
                        iSMSType = oDTTemplate.Rows[0][3].ToInt();
                }

                SchoolBL oSchoolBL = new SchoolBL(miSchoolId);
                string sSMSSenderName = oSchoolBL.SMSSenderName;
                foreach (DataRow oDR in oDataTable.Rows)
                {
                    int iUserId = Convert.ToInt32(oDR["UserId"]);
                    string sMobileNo = Convert.ToString(oDR["MobileNo"]);
                    string sDisplayText = Convert.ToString(oDR["DisplayText"]);
                    SMS oSMS = new SMS();
                    oSMS.Sender = sSMSSenderName;
                    oSMS.SMSText = sAdmissionConfirmSMS;
                    oSMS.TemplateRegistrationId = sTemplateRegistrationId;
                    oSMS.School_Name = oSchoolBL.SchoolName + "::" + sSmsSubject;
                    oSMS.DisplayText = sDisplayText;
                    oSMS.SMSType = iSMSType;
                    oSMS.SchoolID = miSchoolId; 
                    oSMS.AcademicYearID = Convert.ToInt32(hidNextAcademiYearId.Value);
                    oSMS.To.Add(iUserId, sMobileNo);
                    oSMS.Send();
                }
            }
        }
    }

    /// <summary>
    /// This method is used to generate xml.
    /// </summary>
    /// <returns></returns>
    private string GenerateXml(int aiUserRoleId, ListView aoListView)
    {
        Random oRandomNo = new Random((int)DateTime.Now.Ticks);
        int iItemCount = aoListView.Items.Count;
        int iLoginId = StudentBL.GetNextLoginId(miSchoolId, aiUserRoleId);

        if (iLoginId == Constants.I_ZERO || iLoginId == Constants.I_ONE)
            iLoginId = 10000;
        iLoginId++;        
        
        const string S_ELEMENT = "element";
        string sAttribute;
        XmlDocument oDoc = new XmlDocument();

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("StudentDetails");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "StudentDetails", "");

        // Loop through all the grid rows.
        for (int iRowCount = 0; iRowCount < iItemCount; iRowCount++)
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)aoListView.Items[iRowCount];
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
            DataRowView oDataRowView = (System.Data.DataRowView)oCurrentItem.DataItem;

            XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "StudentDetails", "");

            CheckBox chkSelect = (CheckBox)oCurrentItem.FindControl("chkSelect");

            if (chkSelect.Checked)
            {
                sAttribute = "Form_Number";
                XmlAttribute attr = oDoc.CreateAttribute(sAttribute);
                attr.Value = aoListView.DataKeys[iRowCount]["Form_Number"].ToString();
                oXmlNode.Attributes.Append(attr);

                sAttribute = "User_Login";
                attr = oDoc.CreateAttribute(sAttribute);
                attr.Value = iLoginId.ToString();
                oXmlNode.Attributes.Append(attr);

                sAttribute = "User_Password";
                attr = oDoc.CreateAttribute(sAttribute);
                string sPassword = Utility.CommonUtility.GetEncryptedPassword(iLoginId.ToString(), oRandomNo.Next(100000, 999999).ToString());
                attr.Value = sPassword;
                oXmlNode.Attributes.Append(attr);

                iLoginId++;

                // Add the node to root node.
                oXmlRootNode.AppendChild(oXmlNode);
            }
        }

        // Add the root node to document element.
        root.AppendChild(oXmlRootNode);

        // return the string generated.
        return root.InnerXml;
    }

    /// <summary>
    /// This method is used to set sorting image in list view column header.
    /// </summary>
    private void SetWaitingListSortImage(string asSortExpression)
    {
		if (lstvwWaitingList.SortDirection.ToString() == S_SORT_DIRECTION_ASC || lstvwMainList.SortDirection.ToString() == string.Empty)
            hidWaitingSortDirection.Value = Constants.S_DESCENDING;
        else
            hidWaitingSortDirection.Value = Constants.S_ASCENDING;
        if (lstvwWaitingList.SortExpression != string.Empty)
            hidWaitingSortExpression.Value = lstvwWaitingList.SortExpression.ToString();
        else
            hidWaitingSortExpression.Value = asSortExpression;

        HtmlTableRow oHtmlTableHeaderRow = lstvwWaitingList.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidWaitingSortExpression.Value, hidWaitingSortDirection.Value);
    }

	/// <summary>
	/// This method is used to add sort image.
	/// </summary>
	private void AddSortImage()
	{
		if (string.IsNullOrEmpty(hidMainSortExpression.Value))
			hidMainSortExpression.Value = S_DEFAULT_SORT_EXP;
		var oHtmlTableHeaderRow = lstvwMainList.FindControl("trHeader") as HtmlTableRow;
		if (oHtmlTableHeaderRow != null)
			CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidMainSortExpression.Value, hidMainSortDirection.Value);
	}

	/// <summary>
	/// This method is used to add sort image.
	/// </summary>
	private void AddSortImageForWailtingList()
	{
		if (string.IsNullOrEmpty(hidMainSortExpression.Value))
			hidMainSortExpression.Value = S_DEFAULT_SORT_EXP;
		var oHtmlTableHeaderRow = lstvwWaitingList.FindControl("trHeader") as HtmlTableRow;
		if (oHtmlTableHeaderRow != null)
			CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidWaitingSortExpression.Value, hidWaitingSortDirection.Value);
	}

	/// <summary>
	/// This method is used to check pre-condition to configure association.
	/// </summary>
	/// <returns></returns>
	private bool CheckPreConditionForStandard()
	{
		bool bReturn = false;
		string sLinks = null;
		int iAcademicYearId = Convert.ToInt32(hidNextAcademiYearId.Value);
		int iStandardId = Convert.ToInt32(ddlStandard.SelectedValue);
		DataTable oDataTable = StudentAdmissionsBL.GetPreConditionMsg(miSchoolId, iAcademicYearId, iStandardId);
		sLinks = FormatData(oDataTable);
		if (!sLinks.Equals(string.Empty))
		{
			trPrecondition.Visible = true;
			divErr.InnerHtml = sLinks;
			divErr.Visible = true;
		}
		else
		{
			divErr.Visible = false;
			trPrecondition.Visible = false;
			bReturn = true;
		}

		return bReturn;
	}

	/// <summary>
	/// This method is used to format data.
	/// </summary>
	/// <param name="aoDataTable"></param>
	/// <returns></returns>
	private string FormatData(DataTable aoDataTable)
	{
		string sReturn = string.Empty;
		char cIsCurrentAcademicYear = 'N';
		if (miAcademicYearId.ToString() == hidNextAcademiYearId.Value)
			cIsCurrentAcademicYear = 'Y';
		string sHeaderMessage = "Please configure following details in mid year.";
		int iRowCount = aoDataTable.Rows.Count;
		if (cIsCurrentAcademicYear == 'Y')
			sHeaderMessage = "Please configure following details.";
		if (iRowCount > 0)
		{
			sReturn = "<table class=\"LblNoRecord\"><tr><td class=\"ClsConfigText\">" + sHeaderMessage + "</td></tr>";
			for (int i = 0; i < aoDataTable.Rows.Count; i++)
			{
				if (cIsCurrentAcademicYear == 'Y')
					sReturn = sReturn + "<tr><td><a class=\"ClsConfigLink\" href=" + aoDataTable.Rows[i]["NavigateURL"].ToString() + ">" + aoDataTable.Rows[i]["Configure_Name"] + "</a></td></tr>";
				else
					sReturn = sReturn + "<tr><td><a class=\"ClsConfigLink\" href='' Enabled='false' onclick ='return false;'>" + aoDataTable.Rows[i]["Configure_Name"] + "</a></td></tr>";
			}

			sReturn = sReturn + "</table>";
		}

		return sReturn;
	}

    /// <summary>
    /// This method is used to refresh student cache.
    /// </summary>
    private void RefreshStudentCache(DataTable aoDataTable)
    {
        var oDatarows = from dr in aoDataTable.AsEnumerable()
                        select Convert.ToInt32(dr["StudentId"]);
        List<int> lstStudentIds = new List<int>();
        if (oDatarows.Any())
            lstStudentIds = oDatarows.ToList();

        AutoSearchService oAutoSearchService = new AutoSearchService();
        oAutoSearchService.RefreshStudentCache(miSchoolId, miAcademicYearId, lstStudentIds, Constants.Action.Insert);
    }

    #endregion " Private Methods "

   

    #region " Report Methods "
    /// <summary>
    /// This method is used to display report.
    /// </summary>
    /// <param name="asListType"></param>
    private void DisplayReport(string asListType, int aiDisplayConfirmed)
    {
		ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.AdmissionLotteryDetails, GetFilterString(asListType, aiDisplayConfirmed), ExportFormatType.PortableDocFormat);
        oReportDisplay.DisplayReport();
    }

    /// <summary>
    /// This method is used to display report.
    /// </summary>
    /// <param name="asListType"></param>
    private void DisplayReport()
    {
        ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.ConsolidatedStudentAdmissionList, GetFilterString(), ExportFormatType.PortableDocFormat);
        oReportDisplay.DisplayReport();
    }

    /// <summary>
    /// This method is used to get filter string.
    /// </summary>
    /// <returns></returns>
    private string GetFilterString()
    {
        SchoolWiseAcademicYearMasterBL oSchoolAcademicYearBL = new SchoolWiseAcademicYearMasterBL();
        int iAcadYearID = Convert.ToInt32(hidNextAcademiYearId.Value);
        DataTable oDTSchoolInfo = oSchoolAcademicYearBL.GetSchoolInfo(miSchoolId, iAcadYearID);
        string sAcademicYearName = "Year " + oDTSchoolInfo.Rows[Constants.I_ZERO]["Year"].ToString();
        string sOrgName = oDTSchoolInfo.Rows[Constants.I_ZERO]["School_Orgn_Name"].ToString();
        string sSchoolName = Session[Constants.S_SESSION_SCHOOL_NAME].ToString();

        int iAcademicYearId = Convert.ToInt32(hidNextAcademiYearId.Value);
        int iStandardId = Convert.ToInt32(ddlStandard.SelectedValue);
        string sRecordSelectionFormula = "(usp_GetConsolidatedStudentAdmissionList.School_Id}=" + miSchoolId + " AND  usp_GetConsolidatedStudentAdmissionList.Academic_Year_Id} =" + iAcademicYearId +
              " AND usp_GetConsolidatedStudentAdmissionList.StandardId}=" + iStandardId +               
               " AND  usp_GetAllStudentOfAdmissionsLottery.SchoolName} =" + sSchoolName +
               " AND  usp_GetAllStudentOfAdmissionsLottery.AcademicYear} =" + sAcademicYearName +
               " AND  usp_GetAllStudentOfAdmissionsLottery.OrganisationName} =" + sOrgName + ")" + "@";
        return sRecordSelectionFormula; 
    }

	/// <summary>
	/// This method is used to get filter string.
	/// </summary>
	/// <param name="asListType"></param>
	/// <param name="aiDisplayConfirmed"></param>
	/// <returns></returns>
    private string GetFilterString(string asListType, int aiDisplayConfirmed)
    {
            SchoolWiseAcademicYearMasterBL oSchoolAcademicYearBL = new SchoolWiseAcademicYearMasterBL();
            int iAcadYearID = Convert.ToInt32(hidNextAcademiYearId.Value);
            DataTable oDTSchoolInfo = oSchoolAcademicYearBL.GetSchoolInfo(miSchoolId, iAcadYearID);
            string sAcademicYearName = "Year " + oDTSchoolInfo.Rows[Constants.I_ZERO]["Year"].ToString();
            string sOrgName = oDTSchoolInfo.Rows[Constants.I_ZERO]["School_Orgn_Name"].ToString();
            string sSchoolName = Session[Constants.S_SESSION_SCHOOL_NAME].ToString();
            
            string sListName = "Main List of Student Admission";
            if (asListType == "W")
                sListName = "Waiting List of Student Admission";
        int iAcademicYearId = Convert.ToInt32(hidNextAcademiYearId.Value);
        int iStandardId = Convert.ToInt32(ddlStandard.SelectedValue);
        string sRecordSelectionFormula = "(usp_GetAllStudentOfAdmissionsLottery.School_Id}=" + miSchoolId + " AND  usp_GetAllStudentOfAdmissionsLottery.Academic_Year_Id} =" + iAcademicYearId +
              " AND usp_GetAllStudentOfAdmissionsLottery.Standard_Id}=" + iStandardId + " AND  usp_GetAllStudentOfAdmissionsLottery.cSelectedInLottery} =" + asListType
               + " AND  usp_GetAllStudentOfAdmissionsLottery.IsConfirmed} =" + aiDisplayConfirmed +
               " AND  usp_GetAllStudentOfAdmissionsLottery.SchoolName} =" + sSchoolName +
               " AND  usp_GetAllStudentOfAdmissionsLottery.AcademicYear} =" + sAcademicYearName +
               " AND  usp_GetAllStudentOfAdmissionsLottery.OrganisationName} =" + sOrgName +
               " AND  usp_GetAllStudentOfAdmissionsLottery.ListName} =" + sListName + ")" + "@";
        return sRecordSelectionFormula;        
    }

    /// <summary>
    /// This method is used to send sms of admission login details.
    /// </summary>
    /// <param name="aiAdmissionId"></param>
    /// <returns></returns>
    private string SendSMS(string acSelectedInLottery)
    {
        string sMessage = string.Empty;
        string sMobileNumber = string.Empty;
        string sMobileNumber2 = string.Empty;
        string sForm_Number = string.Empty;
        bool bIsAtleastOneSMSSent = false;
        StudentAdmissionsBL oStudentAdmissionsBL = new StudentAdmissionsBL();
        int iAcademicYearId = Convert.ToInt32(hidNextAcademiYearId.Value);
        int iStandardId = Convert.ToInt32(ddlStandard.SelectedValue);
        string sLoginDetailsSmsText = string.Empty;
        string sTemplateRegistrationId = string.Empty;
        string sSmsSubject = string.Empty;
        int iSmsId = Convert.ToInt32(Constants.SMSTemplate.SelectedInLotterySMS);
        DataTable oDTTemplate = SmsTemplateBL.GetTemplate(iSmsId, miSchoolId);

        if (oDTTemplate.Rows.Count != 0)
        {
            if (oDTTemplate.Rows[0][2] != DBNull.Value)
            {
                sLoginDetailsSmsText = Convert.ToString(oDTTemplate.Rows[0][2]);

                if (oDTTemplate.Rows[0]["TemplateRegistrationId"] != DBNull.Value)
                    sTemplateRegistrationId = oDTTemplate.Rows[0]["TemplateRegistrationId"].ToString();

                sSmsSubject = Convert.ToString(oDTTemplate.Rows[0][1]);
            }
        }

		DataTable oDataTable = null;
		if (chkDisplayMainListConfirmed.Checked)
			oDataTable = oStudentAdmissionsBL.GetAdmissonDetailsOfAllStudents(miSchoolId, iAcademicYearId, iStandardId, acSelectedInLottery, Constants.I_ONE);
		else
			oDataTable = oStudentAdmissionsBL.GetAdmissonDetailsOfAllStudents(miSchoolId, iAcademicYearId, iStandardId, acSelectedInLottery, Constants.I_ZERO);
        if (oDataTable.Rows.Count > 0)
        {
            string sListType = "Main List";
            if (acSelectedInLottery == "W")
                sListType = "Waiting List";

                    if (oDataTable.Rows[0]["AdmissionLastDate"] != DBNull.Value)
                    {
                        string sAdmissionLastDate = Convert.ToDateTime(oDataTable.Rows[0]["AdmissionLastDate"]).ToString("dd MMM yyyy");

                        SchoolBL oSchoolBL = new SchoolBL(miSchoolId);
                        string sDisplayText = string.Empty;
                        sLoginDetailsSmsText = sLoginDetailsSmsText.Replace("%LISTTYPE%", sListType);
                        sLoginDetailsSmsText = sLoginDetailsSmsText.Replace("%ADMISSIONDATE%", sAdmissionLastDate);
                        int iAdminlId = 0;
                        Hashtable moManualMobileNo = new Hashtable();
                        foreach (DataRow oDR in oDataTable.Rows)
                        {
                            try
                            {
                                sMobileNumber = Convert.ToString(oDR["MobileNumber"]);
                                sMobileNumber2 = Convert.ToString(oDR["MobileNumber2"]);
                                sForm_Number = Convert.ToString(oDR["Form_Number"]);
                                iAdminlId = Convert.ToInt32(oDR["AdminlID"]);
                                sDisplayText = sDisplayText + "," + sMobileNumber;
                                if (sMobileNumber != string.Empty)
                                    moManualMobileNo[sMobileNumber] = sMobileNumber;
                                if (sMobileNumber2 != string.Empty && sMobileNumber2 != "0")
                                {
                                    sDisplayText = sDisplayText + "," + sMobileNumber2;
                                    moManualMobileNo[sMobileNumber2] = sMobileNumber2;
                                }
                            }
                            catch (Exception)
                            {
                                sMessage = sMessage + "," + sForm_Number;
                            }
                        }

                        if (sDisplayText.Length > 1)
                        {
                            sDisplayText = sDisplayText.Substring(1);
                        }

                        SMS oSMS = new SMS();
                        oSMS.SenderRoleID = Convert.ToInt32(Constants.UserRoles.Admin);
                        oSMS.SenderID = miUserId;
                        oSMS.InsertedByID = -9999;
                        oSMS.Sender = oSchoolBL.SMSSenderName;
                        oSMS.TemplateRegistrationId = sTemplateRegistrationId;
                        oSMS.School_Name = oSchoolBL.SchoolName + "::" + sSmsSubject;
                        oSMS.SMSText = sLoginDetailsSmsText;
                        oSMS.AcademicYearID = iAcademicYearId;
                        oSMS.SchoolID = miSchoolId;
                        oSMS.DisplayText = sDisplayText;
                        oSMS.ToManualNumbers = moManualMobileNo;
                        oSMS.Send();
                        bIsAtleastOneSMSSent = true;
                     }
            }

            if (bIsAtleastOneSMSSent && acSelectedInLottery != string.Empty)
            {
                UpdateConfirmationStatus(miSchoolId, iAcademicYearId, iStandardId);
            }

            if (sMessage.Length > 1)
                sMessage = sMessage.Substring(1);
            return sMessage;
        
    }
	
    /// <summary>
    /// This method is used to check pre-condition to configure association.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.AdmissionLottery);

        if (!sLinks.Equals(string.Empty))
        {
            trPrecondition.Visible = true;
            divErr.InnerHtml = sLinks;
            trLotttery.Visible = false;
        }
        else
        {
            divErr.Visible = false;
            bReturn = true;
        }

        return bReturn;
    }

    /// <summary>
    /// This method is used to publish and updated confirmed status.
    /// </summary>
    /// <param name="iSchoolId"></param>
    /// <param name="iAcademicYearId"></param>
    /// <param name="iStandardId"></param>
    private void UpdateConfirmationStatus(int iSchoolId, int iAcademicYearId, int iStandardId)
    {
        StudentAdmissionsBL.UpdateConfirmationStatus(iSchoolId, iAcademicYearId, iStandardId);
        btnRegenerate.Visible = false;
        btnPublish.Visible = false;
    }

	/// <summary>
	/// This method is used to 
	/// </summary>
	/// <param name="aiOriginalConfigId"></param>
	private void SaveConfig(int aiOriginalConfigId)
	{
		ConfigurationSchoolMasterBL oConfigurationSchoolMasterBL = PopulateSchoolDeatails(aiOriginalConfigId);
		oConfigurationSchoolMasterBL.InsertConfigurationSchoolMaster();
	}

	/// <summary>
	///		This method is used to initailze configuration details.
	/// </summary>
	/// <param name="aiOriginalConfigId"></param>
	private ConfigurationSchoolMasterBL PopulateSchoolDeatails(int aiOriginalConfigId)
	{
		return new ConfigurationSchoolMasterBL
		{
			OriginalConfigId = aiOriginalConfigId,
			SchoolId = miSchoolId,
			AcademicYearId = hidNextAcademiYearId.Value.ToInt(),
			IsConfigure = Constants.C_YES,
			InsertedById = miUserId,
			UpdateById = miUserId,
			FinancialYearId = miFinancialYearId
		};
	}

    /// <summary>
    /// This method is used to set student list button visibility.
    /// </summary>
    private void SetStudentListbuttonState()
    {
        StudentAdmissionsBL oStudentAdmissionsBL = new StudentAdmissionsBL();
        int iAcademicYearId = Convert.ToInt32(hidNextAcademiYearId.Value);
        int iStandardId = Convert.ToInt32(ddlStandard.SelectedValue);
        btnConsolidatedStudentList.Visible = oStudentAdmissionsBL.IsConfirmedStudentExist(miSchoolId, iAcademicYearId, iStandardId);
    }

    #endregion
    
    
}
