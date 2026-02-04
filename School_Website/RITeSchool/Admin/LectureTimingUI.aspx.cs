// File Name  : LectureTimingUI.aspx.cs
// Created By : Ashish
// Date       : 28/11/2008
// Description: This class is used to add, edit or delete the lecture timing details.

using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using Utility;

public partial class LectureTimingUI : System.Web.UI.Page
{
    #region " Constant "

    const int I_COLUMN_INDEX_START_TIME = 1;
    const int I_COLUMN_INDEX_END_TIME = 2;
    const int I_COLUMN_INDEX_EDIT = 3;
    const int I_COLUMN_INDEX_DELETE = 4;
    const string S_LECTURE_TIMING_DETAIL_DATAKEYNAME = "School_LectureTimings_Detail_Id";
    const string S_LECTURE_TIMING_DATAKEYNAME = "School_LectureTimings_Id";
    const string S_LECTURE_NUMBER_DATAKEYNAME = "Lecture_No";
    const string S_DESCRIPTION_DATAKEYNAME = "Description";
    const string S_CMD_NAME_EDIT_LECTURE_TIMING = "EDIT_LECTURE_TIMING";
    const string S_CMD_NAME_DELETE_LECTURE_TIMING = "DELETE_LECTURE_TIMING";
    const string S_LECTURE_NUMBER_ERROR_MSG = "Lecture number is already added.";
    const string S_LECTURE_TIME_ERROR_MSG = "Time slot is already assign to some other lecture.";

    #endregion

    #region " Date Member "
    private string IsConfig;
    #endregion

    #region " Event "

    /// <summary>
    /// This event is used to fill grid and set java script attributes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (CheckPreCondition())
                {
                    SetDefaultProperties();
                    FillSectionComboBox();
                    FillLectureNoComboBox();
                    FillLectureTimingGridview();
                }
            }
            SetClientScriptAttributes();
            btnAdd.Attributes["onclick"] = "javascript:DisableButtons(this)";
        }
        catch (Exception ex)
        {
              BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
              (ex.Message + Constants.S_TRACE + ex.StackTrace,
              System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
              Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    /// <summary>
    /// This event is used to add, update lecture timing in the database.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            string sErrorMsg = IsLectureAssigned();
            if (sErrorMsg.Equals(string.Empty))
            {
                if (CheckIsDuplicateLectureTiming())
                {
                    if (hidIsAddMode.Value == "true")
                    {
                        AddLectureTimingDetails();
                    }
                    else
                    {
                        UpdateLectureTimingDetails();
                    }
                    FillLectureTimingGridview();
                    ResetControls();
                    SetDefaultLectureNo();
                    EnableDisableDescription();
                }
            }
            else
            {
                ShowErrorMessage(sErrorMsg);
            }

        }
        catch (Exception ex)
        {
               BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
              (ex.Message + Constants.S_TRACE + ex.StackTrace,
              System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
              Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    /// <summary>
    /// This event is used for going the SchoolConfigurationControlPanel screen.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Timetable_Related)));
        }
        catch (Exception ex)
        {
              BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
              (ex.Message + Constants.S_TRACE + ex.StackTrace,
              System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
              Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    /// <summary>
    /// This event is used to reset all the controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ResetControls();
            SetDefaultLectureNo();
            EnableDisableDescription();
        }
        catch (Exception ex)
        {
              BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
             (ex.Message + Constants.S_TRACE + ex.StackTrace,
             System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
             Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    /// <summary>
    /// This event is used to fill lecture number combo box, grid view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillLectureNoComboBox();
            FillLectureTimingGridview();
            DisplayStandardName();
            ResetControls();
            SetDefaultLectureNo();
        }
        catch (Exception ex)
        {
             BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
             (ex.Message + Constants.S_TRACE + ex.StackTrace,
             System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
             Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    /// <summary>
    /// This method is used to enable/disable description textbox and 
    /// also check lecture no is not "break" then show error message.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlLectureNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ResetControls();
            EnableDisableDescription();
            string sErrorMsg = IsLectureAssigned();
            if (sErrorMsg != string.Empty)
            {
                ShowErrorMessage(sErrorMsg);
            }
        }
        catch (Exception ex)
        {
              BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
              (ex.Message + Constants.S_TRACE + ex.StackTrace,
              System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
              Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    #endregion

    #region " GridView Event "

    /// <summary>
    /// This event is used to add  sort direction image to the appropriate column header.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwLectureTiming_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView sGridviewName = ((System.Web.UI.WebControls.GridView)(sender));

            if (e.Row.RowType == DataControlRowType.Header)
            {
                // Call the GetSortColumnIndex helper method to determine
                // the index of the column being sorted.
                int sortColumnIndex = CommonUtility.GetSortColumnIndex(sGridviewName, hidSortExpression.Value);

                if (sortColumnIndex != -1)
                {
                    // Call the AddSortImage helper method to add
                    // a sort direction image to the appropriate
                    // column header. 
                    CommonUtility.AddSortImage(sortColumnIndex, e.Row, hidSortDirection.Value);
                }
            }
        }
        catch (Exception ex)
        {
              BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
              (ex.Message + Constants.S_TRACE + ex.StackTrace,
              System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
              Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    /// <summary>
    /// This event is used for sorting lecture timing grid view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwLectureTiming_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            SetSortVariables();
            FillLectureTimingGridview();
        }
        catch (Exception ex)
        {
             BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
             (ex.Message + Constants.S_TRACE + ex.StackTrace,
             System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
             Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    /// <summary>
    /// This method is used to bound javascript function for delete lecture timing.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwLectureTiming_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            SetRowData(e.Row);
        }
        catch (Exception ex)
        {
                BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
               (ex.Message + Constants.S_TRACE + ex.StackTrace,
               System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
               Convert.ToInt32(Session[Constants.S_SESSION_USER_ID])); 
        }
    }

    /// <summary>
    /// This event is used for editing or deleting lecture timing records.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwLectureTiming_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case S_CMD_NAME_EDIT_LECTURE_TIMING:
                    {
                        Int32 iRowIndex = Convert.ToInt32(e.CommandArgument);
                        EditLectureTiming(iRowIndex);
                        EnableDisableDescription();
                    }
                    break;
                case S_CMD_NAME_DELETE_LECTURE_TIMING:
                    {
                        Int32 iRowIndex = Convert.ToInt32(e.CommandArgument);
                        DeleteLectureTiming(iRowIndex);
                        FillLectureTimingGridview();
                        ResetControls();
                        SetDefaultLectureNo();
                    }
                    break;
            }

        }
        catch (Exception ex)
        {
             BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
             (ex.Message + Constants.S_TRACE + ex.StackTrace,
             System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
             Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    #endregion

    #region " Private Method "

    ///<Summary>
    ///This method is used to set default properties of controls.
    ///</Summary>  
    private void SetDefaultProperties()
    {
        valSumLectureTiming.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        hidSortExpression.Value = grdvwLectureTiming.Columns[I_COLUMN_INDEX_START_TIME].SortExpression;
        hidSortDirection.Value = Constants.S_ASCENDING;
        hidIsAddMode.Value = "true";
        DateTime oTodayDt = DateTime.Today;
        hidServerDate.Value = oTodayDt.ToString("dd-MMM-yyyy");
        //This object is used to set start and end time formate.
        BoundField oDate = (BoundField)grdvwLectureTiming.Columns[I_COLUMN_INDEX_START_TIME];
        oDate.HtmlEncode = false;
        oDate.DataFormatString = Constants.S_STANDARD_GRID_TIME_FORMAT;
        oDate = (BoundField)grdvwLectureTiming.Columns[I_COLUMN_INDEX_END_TIME];
        oDate.HtmlEncode = false;
        oDate.DataFormatString = Constants.S_STANDARD_GRID_TIME_FORMAT;

    }

    /// <summary>
    /// This method is used to fill grid view.
    /// </summary>
    private void FillLectureTimingGridview()
    {
        LectureTimingBL oLectureTimingBL = InitializeLectureTimingBL();
        oLectureTimingBL.Section = Convert.ToInt32(ddlSection.SelectedValue);
        DataTable oDTLectureTiming = oLectureTimingBL.RetrieveLectureTimingDetails();
        oDTLectureTiming.DefaultView.Sort = hidSortExpression.Value + " " + hidSortDirection.Value;
        grdvwLectureTiming.DataSource = oDTLectureTiming.DefaultView;
        grdvwLectureTiming.DataBind();
    }

    /// <summary>
    /// This method is used to update lecture timing.
    /// </summary>
    private void UpdateLectureTimingDetails()
    {
        LectureTimingBL oLectureTimingBL = InitializeLectureTimingBLForSave();
        oLectureTimingBL.LectureTimingDetailsId = Convert.ToInt32(hidLibraryTimingDetailId.Value);
        oLectureTimingBL.UpdateLectureTimingDetails();
    }

    /// <summary>
    /// This method is used to add lecture timing to the database.
    /// </summary>
    private void AddLectureTimingDetails()
    {
        LectureTimingBL oLectureTimingBL = InitializeLectureTimingBLForSave();
        //oLectureTimingBL.LectureTimingId = Convert.ToInt32(grdvwLectureTiming.DataKeys[Constants.I_ZERO][S_LECTURE_TIMING_DATAKEYNAME].ToString());
        oLectureTimingBL.AddLectureTimingDetails();
        DecryptQuerystring();
        if (IsConfig != "Y")
        {
            AddStdConfigDetails();
        }
    }

    /// <summary>
    /// This method is used to check start and end timing as per lecture number.
    /// </summary>
    /// <returns></returns>
    private bool CheckIsDuplicateLectureTiming()
    {
        string sMsg = string.Empty;
        
        //This is used to get start time in array string formate. like 12PM/12AM/1PM/1AM
        string[] sTime = ddlStartHr.SelectedValue.Split(' ');
        int iSHr = GetStartEndHour(sTime);

        //This is used to get end time in array string formate. like 12PM/12AM/1PM/1AM
        string[] eTime = ddlEndHr.SelectedValue.Split(' ');
        int iEHr = GetStartEndHour(eTime);

        //get start/end time in min and add 1 min in start time for comparing purpose.
        int iSMin = Convert.ToInt32(ddlStartMin.SelectedValue) + 1;
        int iEMin = Convert.ToInt32(ddlEndMin.SelectedValue);

        //get start date and end date in ( dd/mm/yyyy/hr/min) this formate.
        //iStartDt = input start date and iEndDt = input end date.
        System.DateTime idtStartDt = getDateTime(iSHr, iSMin);
        System.DateTime idtEndDt = getDateTime(iEHr, iEMin);

        sMsg = GetTimeSlotErrorMsg(idtStartDt, idtEndDt);
        
        // if sMsg is not empty means time slot is already assign to some other lecture no.
        if (sMsg.Equals(string.Empty))
        {
            return true;
        }
        else
        {
            ShowErrorMessage(sMsg);
            return false;
        }
    }

    /// <summary>
    /// This method is used to get error message if start/end time is already alloted to other lecture.
    /// </summary>
    /// <param name="iStartDt"></param>
    /// <param name="iEndDt"></param>
    /// <returns></returns>
    private string GetTimeSlotErrorMsg(DateTime iStartDt, DateTime iEndDt)
    {
        string sMsg = string.Empty;
        //Use for loop for compairing start time and end time which is already assign to the grid view.
        for (int iCount = 0; iCount < grdvwLectureTiming.Rows.Count; iCount++)
        {
            if (hidRowIndex.Value != iCount.ToString())
            {
                //get start time and end time from grid view.
                System.DateTime ogrdStartDt = Convert.ToDateTime(grdvwLectureTiming.Rows[iCount].Cells[I_COLUMN_INDEX_START_TIME].Text);
                System.DateTime ogrdEndDt = Convert.ToDateTime(grdvwLectureTiming.Rows[iCount].Cells[I_COLUMN_INDEX_END_TIME].Text);

                //Split (gridview) start time in hour and min.
                int sStartHr = ogrdStartDt.TimeOfDay.Hours;
                int sStartMin = ogrdStartDt.TimeOfDay.Minutes;
                //Split (gridview) end time in hour and min.
                int sEndHr = ogrdEndDt.TimeOfDay.Hours;
                int sEndMin = ogrdEndDt.TimeOfDay.Minutes;

                //Concatenate gridview time with today date. and get existing start date with time
                System.DateTime odtgrdStartDt = getDateTime(sStartHr, sStartMin);

                //Concatenate gridview time with today date. and get existing end date with time
                System.DateTime odtgrdEndDt = getDateTime(sEndHr, sEndMin);

                if (iStartDt > iEndDt)
                {
                    sMsg = "Start time should not be greater than end time.";
                    break;
                }
                else if (odtgrdEndDt >= iStartDt && iStartDt >= odtgrdStartDt)
                {
                    sMsg = S_LECTURE_TIME_ERROR_MSG;
                    break;
                }
                else if (iEndDt > odtgrdStartDt && iEndDt <= odtgrdEndDt)
                {
                    sMsg = S_LECTURE_TIME_ERROR_MSG;
                    break;
                }
                else if (iStartDt >= odtgrdStartDt && iEndDt <= odtgrdEndDt)
                {
                    sMsg = S_LECTURE_TIME_ERROR_MSG;
                    break;
                }
                else if (iStartDt <= odtgrdStartDt && iEndDt >= odtgrdEndDt)
                {
                    sMsg = S_LECTURE_TIME_ERROR_MSG;
                    break;
                }
            }
        }
        return sMsg;
    }

    /// <summary>
    /// This method is used to get date in small date time.
    /// </summary>
    /// <param name="iYear"></param>
    /// <param name="iMonth"></param>
    /// <param name="iDay"></param>
    /// <param name="iHr"></param>
    /// <param name="iMin"></param>
    /// <returns></returns>
    private DateTime getDateTime(int iHr, int iMin)
    {
        System.DateTime dtTodayDate = System.DateTime.Today;
        System.DateTime dtGetDate = new DateTime(dtTodayDate.Year, dtTodayDate.Month, dtTodayDate.Day,
                                                     iHr, iMin, 0);
        return dtGetDate;        
    }

    /// <summary>
    /// This method is used to insert standard configuration details for updating lecture timing
    /// configuration in database table.
    /// </summary>
    private void AddStdConfigDetails()
    {
        ConfigurationSchoolMasterBL oConfiguration = new ConfigurationSchoolMasterBL();
        oConfiguration.OriginalConfigId = Convert.ToInt32(Constants.SchoolConfigurations.LectureTiming);
        oConfiguration.SchoolId = Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]);
        oConfiguration.AcademicYearId = Convert.ToInt32(Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID]);
        oConfiguration.IsConfigure = Constants.C_YES;
        oConfiguration.InsertedById = Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]);
        oConfiguration.UpdateById = Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]);
        oConfiguration.InsertConfigurationSchoolMaster();
    }

    /// <summary>
    /// This method is used to decrypt encrypted querystring.
    /// </summary>
    private void DecryptQuerystring()
    {
        try
        {
            if (Request.QueryString.ToString() != Constants.S_EMPTY_STRING)
            {
                string sTestDecrypt = Server.UrlDecode(Request.QueryString.ToString());

                string msQueryString = Utility.CommonUtility.DecryptQuerystring(sTestDecrypt);
                HttpRequest moHttpRequest = new HttpRequest(Page.Request.FilePath.ToString(),
                                                Page.Request.Url.ToString(),
                                                msQueryString);
                IsConfig = moHttpRequest.QueryString["Is_Configured"];

            }
        }
        catch (Exception)
        {
            MasterPage oMasterPage = (MasterPage)this.Master; oMasterPage.RedirectToNextPage(Constants.S_PAGE_ERROR);
        }
    }

    /// <summary>
    /// This method is used to fill lecture number combo box.
    /// </summary>
    private void FillLectureNoComboBox()
    {
        if (ddlLectureNo.Items.Count > 0)
        {
            ddlLectureNo.Items.Clear();
        }
        LectureTimingBL oLectureTimingBL = InitializeLectureTimingBL();
        oLectureTimingBL.Section = Convert.ToInt32(ddlSection.SelectedValue);
        int iLectureNumber = oLectureTimingBL.RetrieveLectureNumber();

        for (int iCount = 0; iCount <= iLectureNumber; iCount++)
        {
            if (iCount != 0)
            {
                ListItem iListItem = new ListItem(Convert.ToString(iCount));
                ddlLectureNo.Items.Add(iListItem);
            }
            else
            {
                ListItem iListItem = new ListItem(" Break ");
                ddlLectureNo.Items.Add(iListItem);
            }
           
        }
        EnableDisableDescription();
    }

    /// <summary>
    /// This method is used to check precondition for lecture timing.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.LectureTiming);

        if (!sLinks.Equals(""))
        {
            divErr.InnerHtml = sLinks;
            VisibleOrHideControls();
        }
        else
        {
            divErr.Visible = false;
            bReturn = true;
        }
        return bReturn;
    }

    /// <summary>
    /// This method is used to visible or hide controls as per requirement.
    /// </summary>
    private void VisibleOrHideControls()
    {
        grdvwLectureTiming.Visible = false;
        tblLectureCntrol.Visible = false;
        tblValSum.Visible = false;
    }

    /// <summary>
    /// This method is used to enable or disable description textbox.
    /// </summary>
    private void EnableDisableDescription()
    {
        if (ddlLectureNo.SelectedIndex != 0)
        {
            txtDescription.Text = string.Empty;
            txtDescription.Enabled = false;
            ShowHideMandatoryMarks(false);
        }
        else
        {
            txtDescription.Enabled = true;
            ShowHideMandatoryMarks(true);
        }
    }

    /// <summary>
    /// This method is used to check lecture no is already assign or not.
    /// </summary>
    private string IsLectureAssigned()
    {
        string sErrorMsg = string.Empty;
        string sLectureNo = ddlLectureNo.SelectedValue;
        for (int iCount = 0; iCount < grdvwLectureTiming.Rows.Count; iCount++)
        {
            string sgrdLectureNo = grdvwLectureTiming.DataKeys[iCount][S_LECTURE_NUMBER_DATAKEYNAME].ToString();
            if ((sgrdLectureNo.Trim() == sLectureNo.Trim()) && sgrdLectureNo != "0"
                && hidIsAddMode.Value == "true")
            {
                sErrorMsg = S_LECTURE_NUMBER_ERROR_MSG;
                break;
            }
        }
        return sErrorMsg;
    }

    /// <summary>
    /// This method is used to show or hide madatory marks with lagent.
    /// </summary>
    private void ShowHideMandatoryMarks(bool bFlag)
    {
        lblMdtStarDescription.Visible = bFlag;
        lblMandatoryLegent.Visible = bFlag;
    }

    /// <summary>
    /// This method is used to fill section combo box.
    /// </summary>
    private void FillSectionComboBox()
    {
        LectureTimingBL oLectureTimingBL = InitializeLectureTimingBL();
        DataTable oDTSection = oLectureTimingBL.GetSectionAndStandardName();
        if (ViewState["DTSection"] == null)
            ViewState["DTSection"] = oDTSection;
        ddlSection.DataSource = oDTSection;
        ddlSection.DataTextField = "SectionName";
        ddlSection.DataValueField= "section";
        ddlSection.DataBind();

        int iSectionIndex = ddlSection.SelectedIndex;
        lblStandardName.Text = oDTSection.Rows[iSectionIndex][Constants.I_TWO].ToString();
    }

    /// <summary>
    /// This method is used to initialized lecture timing BL class property (insert/update time).
    /// </summary>
    /// <returns></returns>
    private LectureTimingBL InitializeLectureTimingBLForSave()
    {
        LectureTimingBL oLectureTimingBL = InitializeLectureTimingBL();
        oLectureTimingBL.Section = Convert.ToInt32(ddlSection.SelectedValue);
        oLectureTimingBL.LectureNumber = Convert.ToInt32(ddlLectureNo.SelectedIndex);
        oLectureTimingBL.Description = Convert.ToString(txtDescription.Text);

        string[] sTime = ddlStartHr.SelectedValue.Split(' ');
        int iSHr = GetStartEndHour(sTime);

        string[] eTime = ddlEndHr.SelectedValue.Split(' ');
        int iEHr = GetStartEndHour(eTime);
        
        int iSMin = Convert.ToInt32(ddlStartMin.SelectedValue);
        int iEMin = Convert.ToInt32(ddlEndMin.SelectedValue);

        oLectureTimingBL.StartTime = getDateTime(iSHr, iSMin);

        oLectureTimingBL.EndTime = getDateTime(iEHr, iEMin);

        return oLectureTimingBL;
    }

    /// <summary>
    /// This method is used to get hour in integer form.
    /// </summary>
    /// <param name="eTime"></param>
    /// <returns></returns>
    private int GetStartEndHour(string[] eTime)
    {
        int iHr = Convert.ToInt32(eTime[0]);
        if (eTime[1].Equals("PM") && iHr != 12)
            iHr += 12;

        return iHr;
    }


    /// <summary>
    /// This method is used to initialized lecture timing BL class property.
    /// </summary>
    /// <returns></returns>
    private LectureTimingBL InitializeLectureTimingBL()
    {
        LectureTimingBL oLectureTimingBL = new LectureTimingBL();
        oLectureTimingBL.SchoolId = Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]);
        oLectureTimingBL.AcademicYearId = Convert.ToInt32(Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID]);
        oLectureTimingBL.InsertedById = Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]);
        oLectureTimingBL.UpdatedById = Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]);
        oLectureTimingBL.UpdatedDate = Convert.ToDateTime(System.DateTime.Today);

        return oLectureTimingBL;
    }

    /// <summary>
    /// This method is used to set javascript attribute on page load event.
    /// </summary>
    private void SetClientScriptAttributes()
    {
        ddlSection.Focus();
        lblErr.Visible = false;
        btnAdd.Attributes.Add("onclick", "if(!ClearValSum()){return false;}");
        btnCancel.Attributes.Add("onclick", "if(!ClearValSum()){return false;}");

        btnBack.Attributes["onmouseover"] = "javascript:fnover('" + btnBack.ClientID + "',this);";
        btnBack.Attributes["onmouseout"] = "javascript:fnout('" + btnBack.ClientID + "',this);";
        btnAdd.Attributes["onmouseover"] = "javascript:fnover('" + btnAdd.ClientID + "',this);";
        btnAdd.Attributes["onmouseout"] = "javascript:fnout('" + btnAdd.ClientID + "',this);";
        btnCancel.Attributes["onmouseover"] = "javascript:fnover('" + btnCancel.ClientID + "',this);";
        btnCancel.Attributes["onmouseout"] = "javascript:fnout('" + btnCancel.ClientID + "',this);";
    }

    /// <summary>
    /// This method is used to reset all the input controls.
    /// </summary>
    private void ResetControls()
    {
        txtDescription.Text = string.Empty;
        hidRowIndex.Value = "";
        btnAdd.Text = "Add";
        hidIsAddMode.Value = "true";
        hidLibraryTimingDetailId.Value = "";
        ddlSection.Enabled = true;
        ddlLectureNo.Enabled = true;
        ddlStartHr.SelectedIndex = 7;
        ddlStartMin.SelectedIndex = 0;
        ddlEndHr.SelectedIndex = 8;
        ddlEndMin.SelectedIndex = 0;
    }

    /// <summary>
    /// This method is used to display standard name as per section.
    /// </summary>
    private void DisplayStandardName()
    {
        DataTable oDTSection = (DataTable)ViewState["DTSection"];
        int iSection = ddlSection.SelectedIndex;
        lblStandardName.Text = oDTSection.Rows[iSection][Constants.I_TWO].ToString();
    }

    /// <summary>
    /// This method is used to delete lecture timing details. And also check is last record for deleting or not.
    /// </summary>
    /// <param name="iRowIndex"></param>
    private void DeleteLectureTiming(int iRowIndex)
    {
        Char sIsLastRecord = Constants.C_NO;
        if (grdvwLectureTiming.Rows.Count <= 1)
        {
            sIsLastRecord = Constants.C_YES;
        }
        LectureTimingBL oLectureTimingBL = InitializeLectureTimingBL();
        oLectureTimingBL.LectureTimingId = Convert.ToInt32(grdvwLectureTiming.DataKeys[iRowIndex][S_LECTURE_TIMING_DATAKEYNAME].ToString());
        oLectureTimingBL.LectureTimingDetailsId = Convert.ToInt32(grdvwLectureTiming.DataKeys[iRowIndex][S_LECTURE_TIMING_DETAIL_DATAKEYNAME].ToString());
        oLectureTimingBL.DeleteLectureTiming(sIsLastRecord);
    }

    /// <summary>
    /// This method is used to edit lecture timing details in tha appropiate input controls.
    /// </summary>
    /// <param name="iRowIndex"></param>
    private void EditLectureTiming(int iRowIndex)
    {
        hidLibraryTimingDetailId.Value = grdvwLectureTiming.DataKeys[iRowIndex][S_LECTURE_TIMING_DETAIL_DATAKEYNAME].ToString();
        hidRowIndex.Value = Convert.ToString(iRowIndex);
        int iLectureTimingDetailId = Convert.ToInt32(hidLibraryTimingDetailId.Value);
        LectureTimingBL oLectureTimingBL = new LectureTimingBL(iLectureTimingDetailId);

        string sTime;
        int iHr = oLectureTimingBL.StartTime.Hour;
        if (iHr > 12)
            sTime = Convert.ToString(iHr - 12) + " PM";
        else if (iHr == 12)
            sTime = Convert.ToString(iHr) + " PM";
        else
            sTime = Convert.ToString(iHr) + " AM";
        ddlStartHr.SelectedValue = sTime;
        ddlStartMin.SelectedValue = Convert.ToString(oLectureTimingBL.StartTime.Minute).Length == 1 ? "0" + Convert.ToString(oLectureTimingBL.StartTime.Minute) :
            Convert.ToString(oLectureTimingBL.StartTime.Minute);

        iHr = oLectureTimingBL.EndTime.Hour;
        if (iHr > 12)
            sTime = Convert.ToString(iHr - 12) + " PM";
        else if (iHr == 12)
            sTime = Convert.ToString(iHr) + " PM";
        else
            sTime = Convert.ToString(iHr) + " AM";
        ddlEndHr.SelectedValue = sTime;
        ddlEndMin.SelectedValue = Convert.ToString(oLectureTimingBL.EndTime.Minute).Length == 1 ? "0" + Convert.ToString(oLectureTimingBL.EndTime.Minute) :
            Convert.ToString(oLectureTimingBL.EndTime.Minute);

        int iLectureNo = Convert.ToInt32(grdvwLectureTiming.DataKeys[iRowIndex][S_LECTURE_NUMBER_DATAKEYNAME].ToString());
        if (iLectureNo == 0)
        {
            txtDescription.Enabled = true;
            ddlLectureNo.SelectedIndex = iLectureNo;
        }
        else
        {
            txtDescription.Enabled = false;
            ddlLectureNo.SelectedIndex = iLectureNo;
        }
        txtDescription.Text = grdvwLectureTiming.DataKeys[iRowIndex][S_DESCRIPTION_DATAKEYNAME].ToString();

        SetEditModeControls();
    }

    /// <summary>
    /// This method is used to set edit mode controls.
    /// </summary>
    private void SetEditModeControls()
    {
        ddlSection.Enabled = false;
        ddlLectureNo.Enabled = false;
        hidIsAddMode.Value = "false";
        btnAdd.Text = "Update";
    }

    /// <summary>
    /// This method is used to sort grid
    /// </summary>
    private void SetSortVariables()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to bound java script function.
    /// </summary>
    /// <param name="gridViewRow"></param>
    private void SetRowData(GridViewRow gridViewRow)
    {
        int iRowIndex = gridViewRow.RowIndex;
        if (iRowIndex >= 0)
        {
            ImageButton oDelete = (ImageButton)gridViewRow.FindControl("btnDelete");
            oDelete.Attributes.Add("Onclick", "if(!ConfirmDelete()){return false;}");
            ImageButton oEdit = (ImageButton)gridViewRow.FindControl("btnEdit");
            oEdit.Attributes.Add("onclick", "if(!ClearValSum()){return false;}");
          
            int iLectureNo = Convert.ToInt32(grdvwLectureTiming.DataKeys[iRowIndex][S_LECTURE_NUMBER_DATAKEYNAME].ToString());
            string sDescription = grdvwLectureTiming.DataKeys[iRowIndex][S_DESCRIPTION_DATAKEYNAME].ToString();
            if (iLectureNo == Constants.I_ZERO)
            {
                gridViewRow.Cells[0].Text = sDescription;
                gridViewRow.CssClass = "ClsGridRow paddingL ClsConfigText";
            }
        }
    }

    /// <summary>
    /// This method is used to show error message.
    /// </summary>
    /// <param name="sErrorMsg"></param>
    private void ShowErrorMessage(string sErrorMsg)
    {
        lblErr.Visible = true;
        lblErr.Text = sErrorMsg;
    }

    /// <summary>
    /// This method is used to set default lecture number index value.
    /// </summary>
    private void SetDefaultLectureNo()
    {
        ddlLectureNo.SelectedIndex = 0;
    }

    #endregion

}
