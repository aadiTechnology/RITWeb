/* File Name :- StandardExamScheduleConfigurationUI.aspx.cs
 * Modified By :- Sachin
 * Modified Date :- 21-Sept-2009
 * Purpose :- Code Review.
 * Class Description :- This class displays summerised information about exam schedule.
*/
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using System.Threading;
using Utility;
using System.Collections.Generic;

public partial class StandardExamScheduleConfigurationUI : SchoolBase
{
    #region Data Members

    string IsConfig; 

    #endregion

    #region Constants

    const int I_DATAKEY_STANDARD_ID = 0;
    const string S_CSS_CLASS_EDIT_CLASS = "ClsUpdate";
    const int I_ALLEXAMS_TABLE_INDEX = 1;
    const string S_IMG_FOR_STANDARD_DIVISION = "~/RITeSchool/images/GridHead_Std_Exam.gif";
    
    #endregion

    #region Events

    /// <summary>
    /// YThis event is used to set postback url to back button,fill student grid by checking submit behavior.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                grdStandards.Columns[0].HeaderImageUrl = S_IMG_FOR_STANDARD_DIVISION;
                grdStandards.Columns[0].HeaderText = string.Empty;
                SetJavascriptAttributes();
            }
            CheckSubmitBehavior();
            GetExamScheduleInformation();
            if ((Constants.UserRoles)Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] == Constants.UserRoles.Teacher)
            {
                btnBack.Visible = false;
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
    /// this function locked Header row, scroll.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStandards_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            e.Row.Cells[0].CssClass = "locked";
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].CssClass = "Llocked";
                e.Row.Cells[0].Style.Add("left", grdStandards.Style["scrollLeft"]);
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

    #region Methods
    /// <summary>
    /// This method is use to display popup window 
    /// </summary>
    private void GetExamScheduleInformation()
    {
        string sQuerystring = "../Student/StandardwiseExamScheduleList.aspx?";
        lnkbtnExamSchedulePopUp.Attributes.Add("onclick", "GetExamScheduleInformation('" + sQuerystring + "');return false;");
    }
    /// <summary>
    /// This method is used to decrypt querystring.
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
    /// This method is used to check precondition.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;     
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.StandardwiseExamScheduleConfig);
        if (sLinks.Equals(string.Empty))
        {
            divErr.Visible = false;
            bReturn = true;
        }
        else
        {
            divErr.InnerHtml = sLinks;
            LegendTable.Visible = false;
            GridViewScrollContainer.Visible = false;
        }
        return bReturn;
    }

    /// <summary>
    /// This method is used to fill standardwise exam schedule details in gridview.
    /// </summary>
    private void FillStandardwiseExamScheduleGrid()
    {
        if (CheckPreCondition())
        {
            
            DataSet oDSStandardwiseExamSchedule = SchoolwiseStandardExamScheduleMasterBL.GetStdExamSchedule(miSchoolId, miAcademicYearId);
            grdStandards.DataSource = oDSStandardwiseExamSchedule;
            grdStandards.DataBind();
            GenerateColumnsOfGrid();
        }
    }

    /// <summary>
    /// This method is used to add test name  columns to the grid.
    /// </summary>
    /// <param name="aoDSAllTests"> dataset containing all divisions in school
    /// </param>
    private void AddTestColumnsToHeaderRow()
    {
        DataSet oDSStandardwiseExamSChedule = (DataSet)grdStandards.DataSource;
        DataTable oDtTests = oDSStandardwiseExamSChedule.Tables[I_ALLEXAMS_TABLE_INDEX];
        int iCount = oDtTests.Rows.Count;
        //Loop to add Divisions in Header ROw 
        for (int iColumnIndex = 0; iColumnIndex < iCount; iColumnIndex++)
        {
            DataControlFieldHeaderCell oTableCell1 = new DataControlFieldHeaderCell(null);
            oTableCell1.CssClass = "locked";
            oTableCell1.HorizontalAlign = HorizontalAlign.Center;
            oTableCell1.Wrap = false;
            oTableCell1.Style.Add(HtmlTextWriterStyle.PaddingLeft, "5px");
            oTableCell1.Style.Add(HtmlTextWriterStyle.PaddingRight, "7px");
            oTableCell1.Text = oDtTests.Rows[iColumnIndex]["SchoolWise_Test_Name"].ToString();
            grdStandards.HeaderRow.Cells.Add(oTableCell1);
        }
    }

    /// <summary>
    /// This method is used to add columns of Standard in the grid.
    /// </summary>
    private void GenerateColumnsOfGrid()
    {
        //add columns of test name to the grid header row 
        AddTestColumnsToHeaderRow();
        /// This method set Standard columns to rows other than header row.
        /// and exam configration Date format type.
        AddStandardColumnsToOtherRows();
    }

    /// <summary>
    /// This method is used to check submit bahavior and fill student grid.
    /// </summary>
    private void CheckSubmitBehavior()
    {
        bool bIsUseSubmitBehavior = CommonUtility.CheckCancelOrBackClickEvent(this.Page);
        if (bIsUseSubmitBehavior == true)
            FillStandardwiseExamScheduleGrid();
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnBack });
    }

    /// <summary>
    /// This function Create QueryString For Edit Popup.
    /// </summary>
    /// <param name="aoDr"></param>
    /// <param name="asTestName"></param>
    /// <param name="asStandardName"></param>
    /// <returns></returns>
    private string CreateQueryStringForEdit(DataRow aoDr, string asTestName, string asStandardName)
    {
        string sQuerystring = "Standard_Id=" + aoDr["Standard_Id"]
                             + "&Schoolwise_Test_Id=" + aoDr["Schoolwise_Test_Id"]
                             + "&Test_Name=" + asTestName
                             + "&Standard_Name=" + asStandardName
                             + "&Standard_Test_Id=" + aoDr["Standard_Test_Id"]
                             + "&Is_Configured=" + IsConfig;
        sQuerystring = sQuerystring + "&Schoolwise_Standard_Exam_Schedule_Id="
                        + aoDr["Schoolwise_Standard_Exam_Schedule_Id"];
        sQuerystring = sQuerystring + "&Mode=" + "EDIT";

        return sQuerystring;

    }

    /// <summary>
    /// This function Create QueryString For NewMode Popup .
    /// </summary>
    /// <param name="aoDr"></param>
    /// <param name="asTestName"></param>
    /// <param name="asStandardName"></param>
    /// <returns></returns>
    private string CreateQueryStringForNewMode(DataRow aoDr, string asTestName, string asStandardName)
    {
        string sQuerystring = "Standard_Id=" + aoDr["Standard_Id"]
                             + "&Schoolwise_Test_Id=" + aoDr["Schoolwise_Test_Id"]
                             + "&Test_Name=" + asTestName
                             + "&Standard_Name=" + asStandardName
                             + "&Standard_Test_Id=" + aoDr["Schoolwise_Standard_Test_Id"]
                             + "&Is_Configured=" + IsConfig;
        sQuerystring = sQuerystring + "&Schoolwise_Standard_Exam_Schedule_Id=0";

        sQuerystring = sQuerystring + "&Mode=" + "INSERT";

        return sQuerystring;

    }

    /// <summary>
    /// This method adds standard columns to rows other than header row.
    /// and exam configration Date format type with position Else return N/A Configuration.
    /// </summary>
    /// <param name="aoDSAllTests"></param>
    private void AddStandardColumnsToOtherRows()
    {   
        const int I_STDEXAMS_TABLE_INDEX = 2;
        const int I_EXAMSCONFIG_TABLE_INDEX = 3;
        const string S_CSS_CLASS_NOT_APPLICABLE = "ClsGridNA";

        DataSet oDSStandardwiseExamSchedule = (DataSet)grdStandards.DataSource;
        DataTable oDTTests = oDSStandardwiseExamSchedule.Tables[I_ALLEXAMS_TABLE_INDEX];
        DataTable oDTStandardTests = oDSStandardwiseExamSchedule.Tables[I_STDEXAMS_TABLE_INDEX];
        DataTable oDTConfig = oDSStandardwiseExamSchedule.Tables[I_EXAMSCONFIG_TABLE_INDEX];

        int iSchoolId = Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]);
        int iAcademicYearId = Convert.ToInt32(Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID]);
        int iRowCount = grdStandards.Rows.Count;
        int iCount = oDTTests.Rows.Count;
        int iCellIndex = 0;
        int iStandardId = 0;
        //loop through rows
        for (int iRowIndex = 0; iRowIndex < iRowCount; iRowIndex++)
        {
            iStandardId = Convert.ToInt32(grdStandards.DataKeys[iRowIndex][I_DATAKEY_STANDARD_ID].ToString());
            //loop through columns
            for (int iColIndex = 0; iColIndex < iCount; iColIndex++)
            {
                TableCell oTableCell = new TableCell();
                int iTestIdId = Convert.ToInt32(oDTTests.Rows[iColIndex]["SchoolWise_Test_Id"].ToString());
                iCellIndex = grdStandards.Rows[iRowIndex].Cells.Add(oTableCell);
                oTableCell.Attributes.Add("title", "Std. " + oDSStandardwiseExamSchedule.Tables[0].Rows[iRowIndex]["Standard_Name"].ToString() + " [" + oDTTests.Rows[iColIndex]["Schoolwise_Test_Name"].ToString() + "]");
                oTableCell.Wrap = false;
                oTableCell.Style.Add(HtmlTextWriterStyle.PaddingLeft, "5px");
                oTableCell.Style.Add(HtmlTextWriterStyle.PaddingRight, "7px");
                grdStandards.Rows[iRowIndex].Cells[iCellIndex].HorizontalAlign = HorizontalAlign.Center;
                DataRow[] oDrStdExams = oDTStandardTests.Select("Standard_Id = " + iStandardId + " AND SchoolWise_Test_Id = " + iTestIdId);
                if (oDrStdExams.Length > 0)
                {
                    grdStandards.Rows[iRowIndex].Cells[iCellIndex].HorizontalAlign = HorizontalAlign.Center;
                    DataRow[] oDrSchedule = oDTConfig.Select("Standard_Id = " + iStandardId + " AND SchoolWise_Test_Id = " + iTestIdId);
                    string sQueryString = "";
                    DecryptQuerystring();
                    if (oDrSchedule.Length == 0)
                    {
                        sQueryString = CreateQueryStringForNewMode(oDrStdExams[0], grdStandards.HeaderRow.Cells[iCellIndex].Text, grdStandards.Rows[iRowIndex].Cells[0].Text);
                        /// This function call link to add new Exam Configration.
                        AddNewModeScheduleConfigurationLink(iRowIndex, iCellIndex, sQueryString);
                    }
                    else
                    {
                        sQueryString = CreateQueryStringForEdit(oDrSchedule[0], grdStandards.HeaderRow.Cells[iCellIndex].Text, grdStandards.Rows[iRowIndex].Cells[0].Text);
                        DateTime oDt = Convert.ToDateTime(oDrSchedule[0]["Exam_Start_Date"]);
                        string sScheduleDates = oDt.ToString("dd MMM yyyy");
                        oDt = Convert.ToDateTime(oDrSchedule[0]["Exam_End_Date"]);
                        sScheduleDates = sScheduleDates + " - " + oDt.ToString("dd MMM yyyy");
                        /// This function  call link to Edit Exam Configration.
                        AddEditModeScheduleConfigurationLink(iRowIndex, iCellIndex, sScheduleDates, sQueryString);
                    }
                }
                else
                {
                    grdStandards.Rows[iRowIndex].Cells[iCellIndex].CssClass = S_CSS_CLASS_NOT_APPLICABLE;
                    grdStandards.Rows[iRowIndex].Cells[iCellIndex].Text = "N/A";
                }
            }
        }
    }

    /// <summary>
    /// This method is used to set configuration link for edit mode.
    /// </summary>
    /// <param name="aiRowIndex"></param>
    /// <param name="aiCellIndex"></param>
    /// <param name="asSchedule"></param>
    /// <param name="asQueryString"></param>
    private void AddEditModeScheduleConfigurationLink(int aiRowIndex, int aiCellIndex, string asSchedule, string asQueryString)
    {   
        Label oLabel = new Label();
        int iStandardId = Convert.ToInt32(grdStandards.DataKeys[aiRowIndex][I_DATAKEY_STANDARD_ID].ToString());
        oLabel.CssClass = "IconSpacing";
        oLabel.Text = asSchedule;
        oLabel.CssClass = S_CSS_CLASS_EDIT_CLASS;
        string sEncrypt = Utility.CommonUtility.EncryptQuerystring(asQueryString);
        oLabel.Style.Add(HtmlTextWriterStyle.Cursor, "Hand");
        oLabel.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");
        oLabel.Style.Add(HtmlTextWriterStyle.TextDecoration, "underline");
        oLabel.Attributes.Add("onclick", "window.open('../Admin/StandardwiseExamSchedulePopUp.aspx?" + sEncrypt
                                  + "' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=1000,height=650');return false;");
        grdStandards.Rows[aiRowIndex].Cells[aiCellIndex].Controls.Add(oLabel);
        grdStandards.Rows[aiRowIndex].Cells[aiCellIndex].Wrap = false;
        grdStandards.Rows[aiRowIndex].Cells[aiCellIndex].CssClass = S_CSS_CLASS_EDIT_CLASS;

    }

    /// <summary>
    /// This method is used to set configuration link for new mode.
    /// </summary>
    /// <param name="aiRowIndex"></param>
    /// <param name="aiCellIndex"></param>
    /// <param name="asSchedule"></param>
    /// <param name="asQueryString"></param>
    private void AddNewModeScheduleConfigurationLink(int aiRowIndex, int aiCellIndex, string asQueryString)
    {
        string S_CSS_CLASS_NEW_CLASS = "ClsConfigNotdone";
        HyperLink oHyperLink = new HyperLink();
        Label oLbl = new Label();
        int iStandardId = Convert.ToInt32(grdStandards.DataKeys[aiRowIndex][I_DATAKEY_STANDARD_ID].ToString());
        oLbl.CssClass = "IconSpacing";

        oLbl.CssClass = S_CSS_CLASS_EDIT_CLASS;
        string sEncrypt = Utility.CommonUtility.EncryptQuerystring(asQueryString);
        oLbl.Style.Add(HtmlTextWriterStyle.Cursor, "Hand");
        oLbl.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");
        oLbl.Width = 110;
        oLbl.ToolTip = "Add";
        oLbl.CssClass = "IconSpacing " + S_CSS_CLASS_NEW_CLASS;
        oLbl.Text = "Not Configured";
        oLbl.Style.Add(HtmlTextWriterStyle.TextDecoration, "underline");
        oLbl.Attributes.Add("onclick", "window.open('../Admin/StandardwiseExamSchedulePopUp.aspx?" + sEncrypt
                                 + "' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=1000,height=650');return false;");
        grdStandards.Rows[aiRowIndex].Cells[aiCellIndex].Controls.Add(oLbl);
        grdStandards.Rows[aiRowIndex].Cells[aiCellIndex].Style.Add(HtmlTextWriterStyle.PaddingLeft, "5px");
        grdStandards.Rows[aiRowIndex].Cells[aiCellIndex].Style.Add(HtmlTextWriterStyle.PaddingRight, "7px");
        grdStandards.Rows[aiRowIndex].Cells[aiCellIndex].Wrap = false;
        grdStandards.Rows[aiRowIndex].Cells[aiCellIndex].CssClass = S_CSS_CLASS_NEW_CLASS;
    }

    #endregion
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Exam_Related)), false);
    }
}