using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using FeeEntities;
using RJS.Web.WebControl;
using SchoolEntities;
using Utility;
using System.Globalization;

/// <summary>
/// 	This page is opened from This class 
///		1. display the configuration for all tests of a subject(for specific standrd-division) 
///		2. Add subject test configuration (for each test seperately) 
///		3. Edit subject test configuration (for each test seperately) 
///		4. Delete subject test configuration (for each test seperately)
/// </summary>
public partial class LateFeeSettingsDetalis : SchoolBase
{
    #region -- CONSTANT(s) --

    private const string S_LATE_FEE_DETAILS = "LateFeeDetails";
    private const string S_INTERVAL_DETAILS = "IntervalDetails";    
    private const string S_FEE_PAID_RI_MESSAGE = "Some students have already paid %FEETYPE% fees. The Configuration cannot be modified.";    
    private string msFeeTypeName = string.Empty;
    private const int I_INSTALLMENT_NAME = 1;
    private const int I_DUEDATE = 2;
    private const int I_INSTALLMENT_START_DATE = 3;
    private const int I_INSTALLMENT_END_DATE = 4;
    private const int I_ORIGIONAL_FEETYPE_ID = 1;
    private const int I_DATAKEY_FEETYPE = 2;
    private const int I_DATAKEY_FEETYPE_ID = 0;

    #endregion -- CONSTANT(s) --

    #region -- MEMBER(s) --

    private int miOriginalFeeTypeID;    
    private List<LateFeeTypes> mlstLateFeeTypes = null;
    private List<LateFeeDetails> mlstLateFeeDetails = null;
    private List<LateFeeConfiguration> mlstLateFeeConfiguration = null;
    private List<FeeTypeInterval> mlstlstFeeTypeIntervals = null;
    private DateTime mdtDefaultDueDate = DateTime.Parse("1/1/1900 12:00:00 AM");
    private string msCssClass = "ClsGridAltRow";
    private int miFeeTypeId = 0;
    private bool mbChecked = false;
    private bool mbReturn = false;

    #endregion-- MEMBER(s) --

    #region -- EVENT HANDLER(s) --

    /// <summary>
    /// 	This event is fired to initialize the Functions.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
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
                ReadQuerystring();                
                GetStandardName();                
                if (mbReturn)
                    return;
                SetAcademicYearDates();
                FillControlsWithValues();
                SetControlsStates();
                RefreshValue();
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValue();
            }
            GetInitialValues();
            ApplyMouseHoverEffect(new List<Button> { btn_Cancel, btn_Save, btnSaveDeactivationSettings });
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// 	This event is for saving the entered data from screens.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void btn_Save_Click(object sender, EventArgs e)
    {
        try
        {
            int iValueCnt = 0;            
            string sAllFeeTypes = string.Empty;
            string sFeeTypeName = string.Empty,sIntervalName=string.Empty;

            List<LateFeeDetails> lstLateFeeDetails = GetLateFeeXML();
            ValidateLateFeeRepeatCount(lstLateFeeDetails);

            List<LateFeeConfiguration> lstLateFeeConfiguration = ViewState[S_LATE_FEE_DETAILS] as List<LateFeeConfiguration>;
            List<FeeTypeInterval> lstFeeTypeIntervals = ViewState[S_INTERVAL_DETAILS] as List<FeeTypeInterval>;
            var oLateFeeConfigDetails = from Fee in lstLateFeeConfiguration
                                join Interval in lstFeeTypeIntervals
                                on Fee.StandardwiseFeeTypeId equals Interval.StandardwiseFeeTypeId
                                select new
                                {
                                    Fee.IsStudentFeeCount,
                                    Fee.Late_Fee,
                                    Fee.LateFeePerTypeId,
                                    Fee.ValueForType,
                                    Interval.DueDateDetailsId,                            
                                };    

            var oSchoolWiseStandardLateFeeDueDatesMasterBL = new SchoolWiseStandardLateFeeDueDatesMasterBL(miSchoolId, miAcademicYearId)
                {
                    School_Id = miSchoolId,
                    Academic_Year_Id = miAcademicYearId,
                    Standard_Id = hidStandardID.Value.ToInt(),
                    SchoolWise_Standard_LateFee_DueDates_Id = hidLateFeeId.Value.ToInt()
                };

            for (int iCount = 0; iCount < grdLateFeeTypeConfig.Rows.Count; iCount++)
            {
                string sPeriod = string.Empty, sLateFeeType = string.Empty;
                double dLateFeeAmt = 0.0;
                DateTime dtDueDate = DateTime.Now;
                DateTime dtIntStartDate = DateTime.MaxValue;
                DateTime dtIntEndDate=DateTime.MaxValue;

                int iIsStudentFeeCount = 0;
                if (lstLateFeeConfiguration.Count > 0)
                {
                    FeeTypeInterval oFeeTypeInterval = lstFeeTypeIntervals[iCount];
                    dtDueDate = oFeeTypeInterval.Due_Date.ToDateTime();
                    oSchoolWiseStandardLateFeeDueDatesMasterBL.StandardwiseFeeTypeId = oFeeTypeInterval.StandardwiseFeeTypeId;
                    oSchoolWiseStandardLateFeeDueDatesMasterBL.DueDateDetailsId = oFeeTypeInterval.DueDateDetailsId;
					oSchoolWiseStandardLateFeeDueDatesMasterBL.SchoolWise_Standard_LateFee_DueDates_Id = oFeeTypeInterval.DueDateDetailsId;
                    sIntervalName = oFeeTypeInterval.IntervalName;
                    var oLateFeeDetails = oLateFeeConfigDetails.Where(LateFeeDetails => LateFeeDetails.DueDateDetailsId == oFeeTypeInterval.DueDateDetailsId).FirstOrDefault();
                    sPeriod = oLateFeeDetails.ValueForType.ToString();
                    sLateFeeType = oLateFeeDetails.LateFeePerTypeId.ToString();
                    dtIntStartDate = oFeeTypeInterval.IntervalStartDate;
                    dtIntEndDate = oFeeTypeInterval.IntervalEndDate;

                    if (!oLateFeeDetails.Late_Fee.IsNullOrEmpty())
                        dLateFeeAmt = Convert.ToDouble(oLateFeeDetails.Late_Fee);
                    else
                        dLateFeeAmt = 0.0;
                    iIsStudentFeeCount = oLateFeeDetails.IsStudentFeeCount;
                }

                hidFeeTypeID.Value = grdLateFeeTypeConfig.DataKeys[iCount][I_DATAKEY_FEETYPE_ID].ToString();
                sFeeTypeName = grdLateFeeTypeConfig.DataKeys[iCount][I_DATAKEY_FEETYPE].ToString();
                int iFeeTypeID = hidFeeTypeID.Value.ToInt();
                oSchoolWiseStandardLateFeeDueDatesMasterBL.Fee_Type_Id = iFeeTypeID;

                TextBox txtIntervalName = grdLateFeeTypeConfig.Rows[iCount].FindControl("txtIntervalName") as TextBox;
                DateTime dttxtDueDate = (grdLateFeeTypeConfig.Rows[iCount].FindControl("txtDueDate") as TextBox).Text.ToDateTime();
                DateTime dtIntervalStartDate = (grdLateFeeTypeConfig.Rows[iCount].FindControl("txtIntervalStartDate") as TextBox).Text.ToDateTime();
                DateTime dtIntervalEndDate = (grdLateFeeTypeConfig.Rows[iCount].FindControl("txtIntervalEndDate") as TextBox).Text.ToDateTime();

                if (msFeeTypeName != sFeeTypeName)
                {
                    CheckLateFeeConfiguration(iFeeTypeID);
                    msFeeTypeName = sFeeTypeName;
                    
                    oSchoolWiseStandardLateFeeDueDatesMasterBL.Late_Fee = 0;
                    oSchoolWiseStandardLateFeeDueDatesMasterBL.Late_Fee_Type_Id = 0;
                    oSchoolWiseStandardLateFeeDueDatesMasterBL.Late_Fee_Type_Period = 0;
                }
                
                oSchoolWiseStandardLateFeeDueDatesMasterBL.IntervalName = txtIntervalName.Text;
                oSchoolWiseStandardLateFeeDueDatesMasterBL.DueDate = dttxtDueDate.ToDateTime();
                oSchoolWiseStandardLateFeeDueDatesMasterBL.IntervalStartDate = dtIntervalStartDate.ToDateTime();
                oSchoolWiseStandardLateFeeDueDatesMasterBL.IntervalEndDate = dtIntervalEndDate.ToDateTime();

                if (!(((iIsStudentFeeCount > 0) && (dtDueDate == dttxtDueDate) && (dLateFeeAmt == oSchoolWiseStandardLateFeeDueDatesMasterBL.Late_Fee) && (sIntervalName == oSchoolWiseStandardLateFeeDueDatesMasterBL.IntervalName) &&
                   (sPeriod == oSchoolWiseStandardLateFeeDueDatesMasterBL.Late_Fee_Type_Period.ToString()) && (sLateFeeType == oSchoolWiseStandardLateFeeDueDatesMasterBL.Late_Fee_Type_Id.ToString()) &&
                    (oSchoolWiseStandardLateFeeDueDatesMasterBL.IntervalStartDate == dtIntStartDate) && (oSchoolWiseStandardLateFeeDueDatesMasterBL.IntervalEndDate == dtIntEndDate)) || (iIsStudentFeeCount == 0)
                   ))
                {
                    iValueCnt = 1;
                    if (!sAllFeeTypes.Contains(msFeeTypeName.Replace("Fees",string.Empty).Trim()))
                        sAllFeeTypes = sAllFeeTypes + (msFeeTypeName.Replace("Fees",string.Empty)).Trim() + ", ";
                }

                oSchoolWiseStandardLateFeeDueDatesMasterBL.LateFeeDetailsXML = base.GenerateXml(lstLateFeeDetails);
                
                if (iIsStudentFeeCount == 0 && sAllFeeTypes.IsNullOrEmpty())
                {
                    if (hidLateFeeDueDate.Value == Convert.ToString(true))
                        oSchoolWiseStandardLateFeeDueDatesMasterBL.InsertSchoolWiseStandardLateFeeDueDatesMaster();
                    else
                    {
                        string sStdID = hidStandardID.Value;
                        oSchoolWiseStandardLateFeeDueDatesMasterBL.UpdateFeeDueDatesOfStudent(sStdID.ToInt(), iFeeTypeID);
                        oSchoolWiseStandardLateFeeDueDatesMasterBL.InsertSchoolWiseStandardLateFeeDueDatesMaster();
                    }
                }
            }

            if (iValueCnt == 0)
            {
                if (hidIsConfigured.Value != Constants.S_YES)
                    ConfigureLateFeeForGivenSchool();

                btnSaveDeactivationSettings_Click(null, null);

                Response.Write("<Script language='Javascript'>window.opener.location.href = window.opener.location.href;window.close();window.opener.focus();</Script>");
            }
            else
            {
                lblErrorMsg.Text = Resources.LocalizedResources.SomeStudentsHaveAlreadyPaidFeesTheConfigurationCannotBeModified.Replace("% FEETYPE%", sAllFeeTypes.Substring(0, sAllFeeTypes.Length - 2));
                lblUpateMessage.Visible = false;
                FillControlsWithValues();
            }
        }
        catch (SqlException ex1)
        {
            btnSaveDeactivationSettings_Click(null, null);
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex1.Message;
            lblUpateMessage.Visible = false;
        }
        catch (ApplicationException ex1)
        {   
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex1.Message;
            lblUpateMessage.Visible = false;
        }
        catch (ReferenceExceptions ex)
        {
            btnSaveDeactivationSettings_Click(null, null);
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
            lblUpateMessage.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to validate late fee details.
    /// </summary>
    /// <param name="alstLateFeeDetails"></param>
    private void ValidateLateFeeRepeatCount(List<LateFeeDetails> alstLateFeeDetails)
    {
        string sMessage = string.Empty;
        List<int> lstFeeTypeId = new List<int>();
        List<int> lstFeeTypeIdForAmount = new List<int>();
        List<int> lstRepeatCountOrder = new List<int>();

        var oLateFeeCount = alstLateFeeDetails.GroupBy(lfd => lfd.FeeTypeId).Select(lfd => new { FeeTypeId = lfd.Key, TotalCount = lfd.Count() }).Where(lft => lft.TotalCount > 1);

        foreach (var oLateFee in oLateFeeCount)
        {
            int iMaxSortOrder = alstLateFeeDetails.Where(lft => lft.FeeTypeId == oLateFee.FeeTypeId).Max(lt => lt.SortOrder);

            if (alstLateFeeDetails.Any(lfd => lfd.FeeTypeId == oLateFee.FeeTypeId && lfd.SortOrder != iMaxSortOrder && lfd.RepeatCount == 0))
                lstFeeTypeId.AddRange(alstLateFeeDetails.Where(lfd => lfd.FeeTypeId == oLateFee.FeeTypeId && lfd.SortOrder != iMaxSortOrder && lfd.RepeatCount == 0).Select(lfd => lfd.SrNo));
                
            if (alstLateFeeDetails.Any(lfd => lfd.FeeTypeId == oLateFee.FeeTypeId && lfd.SortOrder == iMaxSortOrder && lfd.RepeatCount != 0))
                lstRepeatCountOrder.AddRange(alstLateFeeDetails.Where(lfd => lfd.FeeTypeId == oLateFee.FeeTypeId && lfd.SortOrder == iMaxSortOrder && lfd.RepeatCount != 0).Select(lfd => lfd.SrNo));
                
            if (alstLateFeeDetails.Any(lfd => lfd.FeeTypeId == oLateFee.FeeTypeId && lfd.Amount == 0))
                lstFeeTypeIdForAmount.AddRange(alstLateFeeDetails.Where(lfd => lfd.FeeTypeId == oLateFee.FeeTypeId && lfd.Amount == 0).Select(lfd => lfd.SrNo));
        }

        if (lstFeeTypeIdForAmount.Count > 0)
        {   
            string sFeeTypesForAmount = string.Join(",", lstFeeTypeIdForAmount);
            sMessage = "Amount should not be zero for row(s) : " + sFeeTypesForAmount;
        }

        if (lstFeeTypeId.Count > 0)
        {   
            string sFeeTypes = string.Join(",", lstFeeTypeId);
            sMessage = (sMessage == string.Empty ? string.Empty : sMessage + "<BR />") + "Repeat Count should not be zero for row(s) : " + sFeeTypes;
        }

        if (lstRepeatCountOrder.Count > 0)
        {   
            string sFeeTypesForCount = string.Join(",", lstRepeatCountOrder);
            sMessage = (sMessage == string.Empty ? string.Empty : sMessage + "<BR />") + "Repeat Count should  be zero for row(s) : " + sFeeTypesForCount;
        }

        if (sMessage != string.Empty)
            throw new ApplicationException(sMessage);
    }

    /// <summary>
    /// 	This event is handled for Allow Paging property of grid.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void grdLateFeeTypeConfig_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdLateFeeTypeConfig.PageIndex = e.NewPageIndex;
            FillGridViewWithFeeType();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// 	This event is used for implementing paging style.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void grdLateFeeTypeConfig_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                var oPagerTable = e.Row.Cells[0].Controls[0] as Table;
                oPagerTable.CssClass = "ClsNwGridPaging";
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This is used to hide the specific columns of a grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdLateFeeTypeConfig_DataBound(object sender, EventArgs e)
    {
        for (int iRowIndex = grdLateFeeTypeConfig.Rows.Count - 2; iRowIndex >= 0; iRowIndex--)
        {
            GridViewRow gvRow = grdLateFeeTypeConfig.Rows[iRowIndex];
            GridViewRow gvPreviousRow = grdLateFeeTypeConfig.Rows[iRowIndex + 1];
            for (int iColumnNo = 0; iColumnNo < gvRow.Cells.Count; iColumnNo++)
            {
                if (iColumnNo != I_INSTALLMENT_NAME && iColumnNo != I_DUEDATE && iColumnNo != I_INSTALLMENT_START_DATE && iColumnNo != I_INSTALLMENT_END_DATE)
                {
                    if (gvRow.Cells[0].Text == gvPreviousRow.Cells[0].Text)
                    {
                        if (gvPreviousRow.Cells[iColumnNo].RowSpan < Constants.I_TWO)
                        {
                            gvRow.Cells[iColumnNo].RowSpan = 2;
                        }
                        else
                        {
                            gvRow.Cells[iColumnNo].RowSpan =
                                gvPreviousRow.Cells[iColumnNo].RowSpan + 1;
                        }

                        gvPreviousRow.Cells[iColumnNo].Visible = false;
                    }
                }
            }
        }

        if (grdLateFeeTypeConfig.Rows.Count == 0)
            btn_Save.Enabled = false;
    }

    /// <summary>
    /// This procedure is used to set the css classes acording to the row.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdLateFeeTypeConfig_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int iFeeTypeId = grdLateFeeTypeConfig.DataKeys[e.Row.RowIndex]["Fee_Type_Id"].ToInt();
            if (miFeeTypeId != iFeeTypeId)
            {
                miFeeTypeId = iFeeTypeId;
                if (msCssClass == "ClsGridRow")
                    msCssClass = "ClsGridAltRow";
                else
                    msCssClass = "ClsGridRow";
            }

            e.Row.CssClass = msCssClass;
        }
    }

    /// <summary>
    ///		Disables controls on the row if Deactivate user checkbox is unchecked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwFeesDeactivationSettings_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var oCurrentItem = e.Item as ListViewDataItem;

                bool bIsConfigured = lstvwFeesDeactivationSettings.DataKeys[oCurrentItem.DisplayIndex]["IsConfigured"].ToInt() == Constants.I_ONE;
                var chkSelect = oCurrentItem.FindControl("chkSelect") as CheckBox;

                if (!bIsConfigured || !chkSelect.Checked)
                {
                    if (!bIsConfigured)
                        chkSelect.InputAttributes["disabled"] = "disabled";

                    var txtThresholdMonths = oCurrentItem.FindControl("txtThresholdMonths") as TextBox;
                    var txtThresholdDays = oCurrentItem.FindControl("txtThresholdDays") as TextBox;
                    var txtReminderDays = oCurrentItem.FindControl("txtReminderDays") as TextBox;
                    var txtReminderInterval = oCurrentItem.FindControl("txtReminderInterval") as TextBox;
                    var txtReminderSMS = oCurrentItem.FindControl("txtReminderSMS") as TextBox;

                    txtThresholdMonths.Text = string.Empty;
                    txtThresholdDays.Text = string.Empty;
                    txtReminderDays.Text = string.Empty;
                    txtReminderInterval.Text = string.Empty;
                    txtReminderSMS.Text = string.Empty;

                    txtThresholdMonths.Enabled = false;
                    txtThresholdDays.Enabled = false;
                    txtReminderDays.Enabled = false;
                    txtReminderInterval.Enabled = false;
                    txtReminderSMS.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///		Checks / Unchecks the SelectAll checkbox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwFeesDeactivationSettings_DataBound(object sender, EventArgs e)
    {
        try
        {
        var chkSelectAll = lstvwFeesDeactivationSettings.FindControl("chkSelectAll") as CheckBox;        
    
        if(chkSelectAll != null)
            chkSelectAll.Checked = mbChecked;

        if (lstvwFeesDeactivationSettings.Items.Count == 0)
            btnSaveDeactivationSettings.Enabled = false;    
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///		Saves the Deactivation settings to database.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveDeactivationSettings_Click(object sender, EventArgs e)
    {
        try
        {
            List<LateFeeDeactivationSettings> lstDeactivationSettings = GetDeactivationSettings();

            if (lstDeactivationSettings.Count > 0)
            {
                SchoolWiseStandardLateFeeDueDatesMasterBL.SaveDeactivationSettings(lstDeactivationSettings);

                // This check is performed since this event is explicitly called from the btn_Save_Click
                if (!sender.IsNull())
                {
                    lblUpateMessage.Text = Resources.LocalizedResources.DeactivationSettingsSavedSuccessfully;
                    lblUpateMessage.Visible = true;
                    lblErrorMsg.Visible = false;
                }

                FillControlsWithValues();
            }
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = Resources.LocalizedResources.ThereWasAnErrorSavingDeactivationSettings;
            lblErrorMsg.Visible = true;
            lblUpateMessage.Visible = false;
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set attributes to list view fields.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwFeeTypes_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                LateFeeDetails oLateFeeDetails = e.Item.DataItem as LateFeeDetails;

                Label lblSrNo = e.Item.FindControl("lblSrNo") as Label;
                lblSrNo.Text = (e.Item.DisplayIndex + 1).ToString();

                DropDownList cmbLateFeeType = e.Item.FindControl("cmbLateFeeType") as DropDownList;
                ListSource.FillDropDownList(mlstLateFeeTypes, cmbLateFeeType, "LateFeeType", "LateFeeTypeId", Constants.S_SELECT);
                cmbLateFeeType.SelectedValue = oLateFeeDetails.LateFeePerTypeId.ToString();

                DropDownList cmbFeeType = e.Item.FindControl("cmbFeeType") as DropDownList;
                ListSource.FillDropDownList(mlstLateFeeConfiguration, cmbFeeType, "Fee_Type", "Fee_Type_Id", Constants.S_SELECT);
                cmbFeeType.SelectedValue = oLateFeeDetails.FeeTypeId.ToString();

                CheckBox chkSelect = e.Item.FindControl("chkSelect") as CheckBox;
                chkSelect.Attributes.Add("onclick", "EnableDisableFields(" + e.Item.DisplayIndex + ",this)");
                if (oLateFeeDetails.Id != 0)
                    chkSelect.Checked = true;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to add multiple rows in list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddMoreRows_Click(object sender, EventArgs e)
    {
        try
        {
            int iStartingNumber = hidLastRecordNumber.Value.ToInt();
            for (int iIndex = iStartingNumber; iIndex < iStartingNumber + 3; iIndex++)
                lstvwFeeTypes.Items[iIndex].Visible = true;

            hidLastRecordNumber.Value = (iStartingNumber + 3).ToString();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion -- EVENT HANDLER(s) --

    #region -- PRIVATE METHOD(s) --

    /// <summary>
    ///		Initializes attributes of controls on the page.
    /// </summary>
    private void GetInitialValues()
    {
        DataTable odtDates = SchoolWiseStandardLateFeeDueDatesMasterBL.GetDateList(miSchoolId, miAcademicYearId, hidStandardID.Value.ToInt());
        hidTermName.Value = odtDates.Rows[0]["TotalDays"].ToString();
        btn_Save.Attributes.Add("onclick", "if(!ValidateLateFeeSettings()){return false;}");
    }

    /// <summary>
    ///		Sets the focus on the first input control.
    /// </summary>
    private void SetControlsStates()
    {
        valsumLateFee.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        deactivationValidationSummary.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;           

        if (grdLateFeeTypeConfig.Rows.Count == Constants.I_ZERO)
            return;

        TextBox txtIntervalNames = grdLateFeeTypeConfig.Rows[0].FindControl("txtDueDate") as TextBox;
        txtIntervalNames.Focus();
        SetDefaultButton(btn_Save);
    }

    /// <summary>
    /// 	This function is used to fill the controls with the values which are all ready assigned.
    /// </summary>
    private void FillControlsWithValues()
    {
        string sStdID = hidStandardID.Value;
        if (hidIsConfigured.Value == Constants.S_YES)
        {            
            mlstLateFeeConfiguration = SchoolWiseStandardLateFeeDueDatesMasterCollectionBL.GetStandardwiseLateFeeDueDatesMasterDetails(sStdID, miSchoolId.ToString(), miAcademicYearId.ToString(), out mlstLateFeeTypes, out mlstLateFeeDetails);            

            if (mlstLateFeeConfiguration.Count > Constants.I_ZERO)
            {                
                FillGridWithValues();
                ViewState[S_LATE_FEE_DETAILS] = mlstLateFeeConfiguration;
                ViewState[S_INTERVAL_DETAILS] = mlstlstFeeTypeIntervals;
            }
            else
                FillGridViewWithFeeType();
        }
        else
            FillGridViewWithFeeType();
    }

    /// <summary>
    /// 	This function is used to fill the controls with the values which are all ready assigned.
    /// </summary>
    /// <param name="oDataSet"> </param>    
    private void FillGridWithValues()
    {
        mlstlstFeeTypeIntervals = new List<FeeTypeInterval>();
        mlstLateFeeConfiguration.ForEach(lst => mlstlstFeeTypeIntervals.AddRange(lst.LateFeeIntervals));

        var oLateFeeDetails = from LateFee in mlstLateFeeConfiguration
                        join Interval in mlstlstFeeTypeIntervals
                        on LateFee.StandardwiseFeeTypeId equals Interval.StandardwiseFeeTypeId
                        select new {
                            LateFee.Academic_Year_Id,
                                    LateFee.Day,
                                    LateFee.DeactivateUser,
                                    LateFee.Fee_Type,
                                    LateFee.Fee_Type_Id,
                                    LateFee.IsStudentFeeCount,
                                    LateFee.Late_Fee,
                                    LateFee.Interval,
                                    LateFee.LateFeePerTypeId,
                                    LateFee.Original_Fee_Type_Id,
                                    LateFee.School_Id,
                                    LateFee.Standard_Id,
                                    LateFee.StandardwiseFeeTypeId,
                                    LateFee.ValueForType,
                                    Interval.DueDateDetailsId,
                                    Interval.Due_Date,
                                    Interval.IntervalEndDate,
                                    Interval.IntervalName,
                                    Interval.IntervalStartDate                            
                                   };

        grdLateFeeTypeConfig.DataSource = oLateFeeDetails;
        grdLateFeeTypeConfig.DataBind();

        var oLateFeeConfiguration = mlstLateFeeConfiguration.Select(a => new
        {
            a.Fee_Type,
            a.Fee_Type_Id,
            a.IsConfigured,
            a.LateFeePerTypeId,
            a.ReminderDays,
            a.ReminderInterval,
            a.ReminderSMS,
            a.StandardwiseFeeTypeId,
            a.ThresholdDays,
            a.ThresholdMonths,
            a.DeactivateUser
        }).Distinct().ToList();

        int iCheckedCount = oLateFeeConfiguration.FindAll(b => b.DeactivateUser == 1).Count;
        mbChecked = iCheckedCount == oLateFeeConfiguration.Count;

        lstvwFeesDeactivationSettings.DataSource = oLateFeeConfiguration;
        lstvwFeesDeactivationSettings.DataBind();

        string sFeeType = string.Empty;

        for (int iCount = 0; iCount < grdLateFeeTypeConfig.Rows.Count; iCount++)
        {
            int iOrigFeeTypeID = grdLateFeeTypeConfig.DataKeys[iCount][I_ORIGIONAL_FEETYPE_ID].ToString().ToInt();
            int iDueDateDetailsId = grdLateFeeTypeConfig.DataKeys[iCount]["DueDateDetailsId"].ToInt();

            miOriginalFeeTypeID = iOrigFeeTypeID;
            TextBox txtIntervalNames = grdLateFeeTypeConfig.Rows[iCount].FindControl("txtIntervalName") as TextBox;
            TextBox txtDueDate = grdLateFeeTypeConfig.Rows[iCount].FindControl("txtDueDate") as TextBox;                        
            TextBox txtIntervalStartDate = grdLateFeeTypeConfig.Rows[iCount].FindControl("txtIntervalStartDate") as TextBox;
            HiddenField hidIntervalStart = grdLateFeeTypeConfig.Rows[iCount].FindControl("hidIntervalStart") as HiddenField;
            TextBox txtIntervalEndDate = grdLateFeeTypeConfig.Rows[iCount].FindControl("txtIntervalEndDate") as TextBox;           
            HiddenField hidFeeType = grdLateFeeTypeConfig.Rows[iCount].FindControl("hidFeeType") as HiddenField;

            txtIntervalNames.Text = mlstlstFeeTypeIntervals[iCount].IntervalName;

            if (mlstlstFeeTypeIntervals[iCount].Due_Date != Constants.S_DEFAULT_DATE_5.ToDateTime())
                txtDueDate.Text = mlstlstFeeTypeIntervals[iCount].Due_Date.ToString("dd-MMM-yyyy", new CultureInfo("en"));

            if (mlstlstFeeTypeIntervals[iCount].IntervalStartDate != Constants.S_DEFAULT_DATE_5.ToDateTime())
            {
                txtIntervalStartDate.Text = mlstlstFeeTypeIntervals[iCount].IntervalStartDate.ToString("dd-MMM-yyyy", new CultureInfo("en"));
                hidIntervalStart.Value = mlstlstFeeTypeIntervals[iCount].IntervalStartDate.ToString("dd-MMM-yyyy");
            }

            if (mlstlstFeeTypeIntervals[iCount].IntervalEndDate != Constants.S_DEFAULT_DATE_5.ToDateTime())
                txtIntervalEndDate.Text = mlstlstFeeTypeIntervals[iCount].IntervalEndDate.ToString("dd-MMM-yyyy", new CultureInfo("en"));
            
            txtIntervalEndDate.Attributes.Add("onchange", "fun(this," + iCount + ")");
            if (sFeeType == hidFeeType.Value.ToString() && iCount != Constants.I_ZERO)
            {
                PopCalendar PopCalendar = grdLateFeeTypeConfig.Rows[iCount].FindControl("PopCalendar2") as PopCalendar;
                TextBox txtIntervalStartDate1 = grdLateFeeTypeConfig.Rows[iCount].FindControl("txtIntervalStartDate") as TextBox;                
                txtIntervalStartDate1.ReadOnly = true;
                PopCalendar.Enabled = false;
            }

            if (sFeeType != hidFeeType.Value.ToString())
                sFeeType = hidFeeType.Value.ToString();           
           
            if (mlstLateFeeTypes.Count > Constants.I_ZERO)
            {
                int iLateFeePerTypeId = oLateFeeDetails.Where(LateFeeDetails => LateFeeDetails.DueDateDetailsId == iDueDateDetailsId).Select(LateFeeDetails => LateFeeDetails.LateFeePerTypeId).FirstOrDefault();
                int iValueForType = oLateFeeDetails.Where(LateFeeDetails => LateFeeDetails.DueDateDetailsId == iDueDateDetailsId).Select(LateFeeDetails => LateFeeDetails.ValueForType).FirstOrDefault();
            }
        }

        FillLateFeeDetails();
    }

    /// <summary>
    /// This method is sued to fill up late fee details.
    /// </summary>
    private void FillLateFeeDetails()
    {   
        int iOriginalCount = mlstLateFeeDetails.Count;

        for (int iIndex = iOriginalCount; iIndex < 15; iIndex++)
        {
            mlstLateFeeDetails.Add
                (
                    new LateFeeDetails
                    {
                        Amount = 0,
                        ExcludeHolidays = false,
                        ExcludeWeekends = false,
                        Id = 0,
                        LateFeeId = 0,
                        LateFeePerTypeId = 0,
                        RepeatCount = 0,
                        SortOrder = 0,
                        FeeTypeId = 0,
                        ValueForType = 0
                    }
                );
        }
        
        lstvwFeeTypes.DataSource = mlstLateFeeDetails;
        lstvwFeeTypes.DataBind();

        //hidLastRecordNumber.Value = "10";
        //for (int iIndex = iOriginalCount; iIndex < 25; iIndex++)
        //    lstvwFeeTypes.Items[iIndex].Visible = false;
    }

    /// <summary>
    /// This is used to set Academic Year Start and End Date
    /// </summary>
    private void SetAcademicYearDates()
    {
        hidYearStartDate.Value = Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE].ToString();
        hidYearEndDate.Value = Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE].ToString();
    }

    /// <summary>
    /// 	This function is to assign the value to the standard.
    /// </summary>
    private void GetStandardName()
    {
        string sStdID = hidStandardID.Value;
        DataTable oDataTable = StandardMasterBL.GetStandardDetails(sStdID, miSchoolId.ToString(), miAcademicYearId.ToString());
        if (oDataTable.Rows.Count == Constants.I_ZERO)
        {
            btn_Save.Visible = false;
            btnSaveDeactivationSettings.Visible = false;
            grdLateFeeTypeConfig.DataSource = null;
            grdLateFeeTypeConfig.DataBind();
            lstvwFeesDeactivationSettings.DataSource = null;
            lstvwFeesDeactivationSettings.DataBind();
            mbReturn = true;
            return;
        }

        lblStandard.Text = oDataTable.Rows[0]["Standard_Name"].ToString();        
        hidAcademicYearStartDate.Value = oDataTable.Rows[0]["StartDate"].ToDateTime().ToString("dd-MMM-yyyy",new CultureInfo("en"));
        hidAcademicYearEndDate.Value = oDataTable.Rows[0]["EndDate"].ToDateTime().ToString("dd-MMM-yyyy",new CultureInfo("en"));
    }
    
    /// <summary>
    /// The function is for Decrypting the querystring.
    /// </summary>
    private void ReadQuerystring()
    {
        if (Request.QueryString.ToString() == Constants.S_EMPTY_STRING)
            return;

        hidStandardID.Value = QueryString["StandardId"];
        if (QueryString["LateFeeId"] != null)
            hidLateFeeId.Value = QueryString["LateFeeId"];
        hidIsConfigured.Value = QueryString["Is_Configured"];
        hidAcademicYearStartDate.Value = Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE].ToString();
    }

    /// <summary>
    /// 	This function is to fill the grid view with the Fee type.
    /// </summary>
    private void FillGridViewWithFeeType()
    {
        string sStdID = hidStandardID.Value;        
        mlstLateFeeConfiguration = SchoolWiseStandardLateFeeDueDatesMasterCollectionBL.GetStandardwiseLateFeeDueDatesMasterDetails(sStdID, miSchoolId.ToString(), miAcademicYearId.ToString(), out mlstLateFeeTypes,out mlstLateFeeDetails);        
        FillGridWithValues();
        ViewState[S_LATE_FEE_DETAILS] = mlstLateFeeConfiguration;
        ViewState[S_INTERVAL_DETAILS] = mlstlstFeeTypeIntervals;
        lstvwFeesDeactivationSettings.DataSource = null;
        lstvwFeesDeactivationSettings.DataBind();
        btnSaveDeactivationSettings.Enabled = false;
    }   

    /// <summary>
    /// 	This method is used to check whether Weekdays are configure or not.
    /// </summary>
    private void ConfigureLateFeeForGivenSchool()
    {
        var oConfiguration = new ConfigurationSchoolMasterBL
            {
                SchoolId = miSchoolId,
                OriginalConfigId = Constants.SchoolConfigurations.LateFeeSettings.ToInt(),
                AcademicYearId = miAcademicYearId
            };
        if (!oConfiguration.IsSchoolConfigured())
            PopulateSchoolConfigurationBL();
    }

    /// <summary>
    /// 	This method is used to populate school configuration data.
    /// </summary>
    private void PopulateSchoolConfigurationBL()
    {
        var oConfiguration = new ConfigurationSchoolMasterBL
            {
                OriginalConfigId = Constants.SchoolConfigurations.LateFeeSettings.ToInt(),
                SchoolId = miSchoolId,
                AcademicYearId = miAcademicYearId,
                IsConfigure = Constants.C_YES,
                InsertedById = miUserId,
                UpdateById = miUserId
            };
        oConfiguration.InsertConfigurationSchoolMaster();
    }

    /// <summary>
    /// 	This is for checking the configuration of the late fee settings.
    /// </summary>
    /// <param name="iFeeTypeID"> </param>
    private void CheckLateFeeConfiguration(int iFeeTypeID)
    {
        string sStdID = hidStandardID.Value;
        var oSchoolWiseStandardLateFeeDueDatesMasterBL = new SchoolWiseStandardLateFeeDueDatesMasterBL(miSchoolId);
        int lateFeeDueDateID = oSchoolWiseStandardLateFeeDueDatesMasterBL.CheckWhetherTheLateFeeConfigurationIsDone(Convert.ToString(miSchoolId), Convert.ToString(miAcademicYearId), sStdID, Convert.ToString(iFeeTypeID));
        hidLateFeeDueDate.Value = Convert.ToString(lateFeeDueDateID == 0);
    }

    /// <summary>
    ///		Returns the deactivation settings from the page in an xml format.
    /// </summary>
    /// <returns>An XML string representing the deactivation settings.</returns>
    private List<LateFeeDeactivationSettings> GetDeactivationSettings()
    {
        return (from item in lstvwFeesDeactivationSettings.Items
                let chkSelect = item.FindControl("chkSelect") as CheckBox
                let txtThresholdMonths = item.FindControl("txtThresholdMonths") as TextBox
                let txtThresholdDays = item.FindControl("txtThresholdDays") as TextBox
                let txtReminderDays = item.FindControl("txtReminderDays") as TextBox
                let txtReminderInterval = item.FindControl("txtReminderInterval") as TextBox
                let txtReminderSMS = item.FindControl("txtReminderSMS") as TextBox
                select new LateFeeDeactivationSettings
                          {
                              SchoolId = miSchoolId,
                              AcademicYearId = miAcademicYearId,
                              StandardId = hidStandardID.Value.ToInt(),
                              FeeTypeId = lstvwFeesDeactivationSettings.DataKeys[item.DisplayIndex]["Fee_Type_Id"].ToInt(),
                              DeactivateUser = chkSelect.Checked,
                              ThresholdMonths = chkSelect.Checked && !txtThresholdMonths.Text.IsNullOrEmpty() ? txtThresholdMonths.Text.ToInt() : 0,
                              ThresholdDays = chkSelect.Checked && !txtThresholdDays.Text.IsNullOrEmpty() ? txtThresholdDays.Text.ToInt() : 0,
                              ReminderDays = chkSelect.Checked && !txtReminderDays.Text.IsNullOrEmpty() ? txtReminderDays.Text.ToInt() : 0,
                              ReminderInterval = chkSelect.Checked && !txtReminderInterval.Text.IsNullOrEmpty() ? txtReminderInterval.Text.ToInt() : 0,
                              ReminderSMS = chkSelect.Checked && !txtReminderSMS.Text.IsNullOrEmpty() ? txtReminderSMS.Text.ToInt() : 0
                          }).ToList();
    }
    /// <summary>
    /// This method used to value based on Culture
    /// </summary>
    private void RefreshValue()
    {
        hidDueDatesShouldBeSelectedFor.Value = Resources.LocalizedResources.DueDatesShouldBeSelectedFor;
        hidDueDateshouldBeInTheValidFormatFor.Value = Resources.LocalizedResources.DueDateshouldBeInTheValidFormatFor;
        hidInstallmentStartDatesShouldBeInValidFormatFor.Value = Resources.LocalizedResources.InstallmentStartDatesShouldBeInValidFormatFor;
        hidInstallmentStartDatesShouldBeSelectedFor.Value = Resources.LocalizedResources.InstallmentStartDatesShouldBeSelectedFor;
        hidInstallmentEndDateShouldBeSelectedFor.Value = Resources.LocalizedResources.InstallmentEndDateShouldBeSelectedFor;
        hidInstallmentEndDatesShouldBeInValidFormatFor.Value = Resources.LocalizedResources.InstallmentEndDatesShouldBeInValidFormatFor;
        hidInstallmentNamesShouldNotBeBlankFor.Value = Resources.LocalizedResources.InstallmentNamesShouldNotBeBlankFor;
        hidValueForTypesShouldNotBeblankFor.Value = Resources.LocalizedResources.ValueForTypesShouldNotBeblankFor;
        hidValueForTypesShouldNotBeblankFor.Value = Resources.LocalizedResources.ValueForTypesShouldNotBeblankFor;
        hidLateFeeTypeShouldBeSelectedFor.Value = Resources.LocalizedResources.LateFeeTypeShouldBeSelectedFor;
        hidInstallmentNamesShouldNotBeDuplicatedFor.Value = Resources.LocalizedResources.InstallmentNamesShouldNotBeDuplicatedFor;
        hidInstallmentDatesShouldBeWithinTheCurrentAcademicYear.Value = Resources.LocalizedResources.InstallmentDatesShouldBeWithinTheCurrentAcademicYear;
        hidDueDateShouldBeLessThanOrEqualToInstallmentEndDateFor.Value = Resources.LocalizedResources.DueDateShouldBeLessThanOrEqualToInstallmentEndDateFor;
        hidInstallmentEndDateShouldBeGreaterThanInstallmentStartDateFor.Value = Resources.LocalizedResources.InstallmentEndDateShouldBeGreaterThanInstallmentStartDateFor;
        hidSelectedDateFor.Value = Resources.LocalizedResources.SelectedDateFor;
        hidIsAHoliday.Value = Resources.LocalizedResources.IsAHoliday;
        hidIsNotAWorkingDay.Value = Resources.LocalizedResources.IsNotAWorkingDay;
        hidPleaseSelectAmountGreaterThanZero.Value = Resources.LocalizedResources.PleaseSelectAmountGreaterThanZero;
        hidDoYouWantToContinue.Value = Resources.LocalizedResources.DoYouWantToContinue;
        hidHoliday.Value = Resources.LocalizedResources.Holiday;
        hidMonthsAndDaysShouldBeSpecifiedForDeactivationThresholdForFeeTypes.Value = Resources.LocalizedResources.MonthsAndDaysShouldBeSpecifiedForDeactivationThresholdForFeeTypes;
        hidMonthsAndDaysBothShouldNotBeZeroForDeactivationThresholdForFeeTypes.Value = Resources.LocalizedResources.MonthsAndDaysBothShouldNotBeZeroForDeactivationThresholdForFeeTypes;
        hidDaysIntervalAndSMSShouldNotBezeroForReminderForFeeTypes.Value = Resources.LocalizedResources.DaysIntervalAndSMSShouldNotBezeroForReminderForFeeTypes;
        hidIntervalShouldNotBeGreaterThanDaysForFeeTypes.Value = Resources.LocalizedResources.IntervalShouldNotBeGreaterThanDaysForFeeTypes;
        hidReminderDaysShouldNotBeGreaterThanDeactivationThresholdForFeeTypes.Value = Resources.LocalizedResources.ReminderDaysShouldNotBeGreaterThanDeactivationThresholdForFeeTypes;
        hidReminderSMSShouldNotBeGreaterThanDeactivationThresholdForFeeTypes.Value = Resources.LocalizedResources.ReminderSMSShouldNotBeGreaterThanDeactivationThresholdForFeeTypes;
        HidTo.Value = Resources.LocalizedResources.To;
        HidFor1.Value = Resources.LocalizedResources.for1;
        hidAmountRsShouldNotBeBlankFor.Value = Resources.LocalizedResources.AmountRsShouldNotBeBlankFor;
        hidAtleastOneFeeTypeSelectedForSaving.Value = Resources.LocalizedResources.AtleastOneFeeTypeSelectedForSaving;
    }

    /// <summary>
    /// This method is used to return late fee details xml.
    /// </summary>
    /// <returns></returns>
    private List<LateFeeDetails> GetLateFeeXML()
    {
        List<LateFeeDetails> lstLateFeeDetails = new List<LateFeeDetails>();
        foreach (ListViewDataItem oItem in lstvwFeeTypes.Items)
        {
            CheckBox chkSelect = oItem.FindControl("chkSelect") as CheckBox;
            if (chkSelect.Checked)
            {
                int iId = lstvwFeeTypes.DataKeys[oItem.DisplayIndex]["Id"].ToInt();
                int iLateFeeId = lstvwFeeTypes.DataKeys[oItem.DisplayIndex]["LateFeeId"].ToInt();

                DropDownList cmbFeeType = oItem.FindControl("cmbFeeType") as DropDownList;
                DropDownList cmbLateFeeType = oItem.FindControl("cmbLateFeeType") as DropDownList;
                CheckBox chkExcludeHolidays = oItem.FindControl("chkExcludeHolidays") as CheckBox;
                CheckBox chkExcludeWeekends = oItem.FindControl("chkExcludeWeekends") as CheckBox;

                TextBox txtValueForType = oItem.FindControl("txtValueForType") as TextBox;
                TextBox txtAmount = oItem.FindControl("txtAmount") as TextBox;
                TextBox txtRepeatCount = oItem.FindControl("txtRepeatCount") as TextBox;
                TextBox txtSortOrder = oItem.FindControl("txtSortOrder") as TextBox;

                lstLateFeeDetails.Add
                   (
                       new LateFeeDetails
                       {
                           Amount = txtAmount.Text.ToInt(),
                           ExcludeHolidays = chkExcludeHolidays.Checked,
                           ExcludeWeekends = chkExcludeWeekends.Checked,
                           Id = iId,
                           LateFeeId = iLateFeeId,
                           LateFeePerTypeId = cmbLateFeeType.SelectedValue.ToInt(),
                           RepeatCount = txtRepeatCount.Text.ToInt(),
                           SortOrder = txtSortOrder.Text.ToInt(),
                           FeeTypeId = cmbFeeType.SelectedValue.ToInt(),
                           ValueForType = txtValueForType.Text.ToInt(),
                           SrNo = oItem.DisplayIndex + 1
                       }
                   );
            }
        }
        return lstLateFeeDetails;
    }

    #endregion -- PRIVATE METHOD(s) --
}