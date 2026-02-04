


/* File Name :- AllStudentsUI.aspx.cs
 * Modified By :- Sachin
 * Modified Date :- 18-Sept-2009
 * Purpose :- Code Review.
 * Class Description :-This class displays summerised information about students in every class and also takes user 
 *  1. View detail list of stdents.
 *  2. Add student in the class.
 *  3. Update roll numbers.
 *  4. Import students.
 *  5. Set second language.
*/
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using BusinessLogic;
using Utility;
using BusinessLogic.Exceptions;
using System.Reflection;
using System.Linq;

public partial class AllStudentsUI : SchoolBase
{

    #region Constants

    const int I_DIV_TABLE_INDEX = 1;
    const Int32 I_STANDARD_NAME_COLUMN_NUMBER = 0;
    const string S_IMG_FOR_STANDARD_DIVISION = "~/RITeSchool/images/GridHeader_StdDiv_Title.gif";
    const string S_IMG_ADD_URL = "~/RITeSchool/images/GridIconSml_Add.gif";
    const string S_IMG_VIEW_URL = "~/RITeSchool/images/GridIconSml_View.gif";

    #endregion

    #region Data Members

    public string IsConfig;
    public string sIsManagementUser;

    #endregion

    #region Events

    /// <summary>
    /// This Event is used to set the MasterPage based on the logged in user.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        try
        {
            base.OnPreInit(e);

			ReadQueryString();            
            if (sIsManagementUser == "Y")
                this.Page.MasterPageFile = "../MasterPages/PopupMaster.master";
          
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set view acccording to role and fill standard gridview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            CheckReffererURL();
            if (sIsManagementUser == "Y")
            {
                trStudent.Visible = true;
                trLegend.Visible = false;
            }
            else
            {
                trLegend.Visible = true;
                trStudent.Visible = false;
            }
            
            if (!IsPostBack)
            {
				if (sIsManagementUser != Constants.S_YES)
					ReadQuerystring();
                CheckRoleAndAssignDisplayView();
                SetQuerystring();
                Session.Remove("ClassId");
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
            }
            hidTotalCount.Value = Resources.LocalizedResources.TotalCount;
            bool bIsUseSubmitBehavior = CommonUtility.CheckCancelOrBackClickEvent(this.Page);
            if (bIsUseSubmitBehavior == true)
                FillStandardGrid();
            SetJavascriptAttributes();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to come back to the previous page. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)this.Master;

            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Other_User_Related)));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// This method is used to check refferer URL.
    /// </summary>
    private void CheckReffererURL()
    {
        if (Request.UrlReferrer != null)
        {
            string sUrl = Request.UrlReferrer.AbsolutePath;
            sUrl = sUrl.Substring(sUrl.LastIndexOf("/") + 1);
            if (sUrl.Equals("StudentUI.aspx"))
            {
                string sURL = "../Admin/StandardwiseFeeConfigurationUI.aspx?" + Convert.ToString(Request.QueryString);
                Response.Redirect(sURL, false);
            }
            else if (sUrl == "StudentRollNosGeneration.aspx" || sUrl == "ImportStudentUI.aspx" || sUrl == "RegenarateRollNoUI.aspx")
                btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Other_User_Related));

            else if (sUrl == "ControlPanel.aspx")
            {
                btnBack.Visible = false;
            }   
        }
    }

    /// <summary>
    /// This ethod i sused to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        string sIsEditMode = Constants.S_NO;
        ApplyMouseHoverEffect(new List<Button>{btnStudent, btnBack, btnClose});
        if (moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher)
            sIsEditMode = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.RegenerateReassignRollNos).ToString();
        if (moUserRole == Constants.UserRoles.Admin ||
                  ((moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher)
                      && sIsEditMode == Constants.S_YES))
        {
            tdRollNosGeneration.Visible = true;
        }
        else
        {
            tdRollNosGeneration.Visible = false;
        }
    }

    /// <summary>
    /// This method is used to check login role and access.    
    /// </summary>
    private void CheckRoleAndAssignDisplayView()
    {
        grdStandards.Columns[0].HeaderImageUrl = S_IMG_FOR_STANDARD_DIVISION;
        grdStandards.Columns[0].HeaderText = string.Empty;
        if (moUserRole== Constants.UserRoles.Supervisor || moUserRole== Constants.UserRoles.Teacher)
        {
            hidCanEdit.Value = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.Student).ToString();
            hyperlnk.Visible = false;
            tdRollNosGeneration.Visible = false;
            hyperlnkSanctionLeave.Visible = false;
            hlnkHouseAssignmentDiv.Visible = false;
            hyperLnkSecondLanguage.Visible = false;
            btnClose.Visible = false;
        }
        if ((sIsManagementUser == "Y"))
        {
            hidCanEdit.Value = "N";
            hyperlnk.Visible = false;
            tdRollNosGeneration.Visible = false;
            hyperlnkSanctionLeave.Visible = false;
            hlnkHouseAssignmentDiv.Visible = false;
            hyperLnkSecondLanguage.Visible = false;
            btnClose.Visible = true;
            btnBack.Visible = false;
            btnStudent.Visible = false;
        }
    }

    /// <summary>
    /// This method checks the preconditons of Configured Subjects for Subject Group criteria.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.Student);
        if (sLinks.Equals(string.Empty))
        {
            divErr.Visible = false;
            bReturn = true;
        }
        else
        {
            divErr.InnerHtml = sLinks;
            LegendTable.Visible = false;
            divGridView.Visible = false;
            hyperlnkSanctionLeave.Visible = false;
            hlnkHouseAssignmentDiv.Visible = false;
            hyperLnkSecondLanguage.Visible = false;
            btnStudent.Visible = false;
        }
        return bReturn;
    }

    /// <summary>
    /// This method is used to fill stadards of school in the grid.
    /// </summary>
    private void FillStandardGrid()
    {
        if (CheckPreCondition())
        {
            StudentCollectionBL oStudents = new StudentCollectionBL(miSchoolId, miAcademicYearId);
            DataSet oDs = oStudents.GetClassStudentsAssociation();
            grdStandards.DataSource = oDs;
            grdStandards.DataBind();
            GenerateColumnsOfGrid();
        }
    }

    /// <summary>
    /// This method is used to add columns (of Divisions) to the grid
    /// </summary>
    /// <param name="aoDSAllDivisions"> dataset containing all divisions in school
    /// </param>
    private void AddDivisionColumnsToHeaderRow()
    {
        DataSet oDsStandards = (DataSet)grdStandards.DataSource;
        DataTable oDtDivisions = oDsStandards.Tables[I_DIV_TABLE_INDEX];
        int iDivisionCount = oDtDivisions.Rows.Count;
        //Loop to add Divisions in Header ROw 
        TableCell oTableCell1;
        int iColIndex = 0;
        for (; iColIndex < iDivisionCount; iColIndex++)
        {
            oTableCell1 = new TableCell();
            oTableCell1.HorizontalAlign = HorizontalAlign.Center;

            oTableCell1.Width = Unit.Point(900);
            oTableCell1.Wrap = false;
            oTableCell1.Text = oDtDivisions.Rows[iColIndex][Constants.S_DIVISION_NAME_FIELD].ToString();

            grdStandards.HeaderRow.Cells.Add(oTableCell1);
        }

        oTableCell1 = new TableCell();
        oTableCell1.HorizontalAlign = HorizontalAlign.Center;

        oTableCell1.Width = Unit.Point(600);
        oTableCell1.Wrap = false;
        oTableCell1.Text = Resources .LocalizedResources.TotalCount;

        grdStandards.HeaderRow.Cells.Add(oTableCell1);
    }

    /// <summary>
    /// This method adds a label (to display no. of students in a class) in grid
    /// </summary>
    /// <param name="aiStudentCount"></param>
    /// <param name="aiRowIndex"></param>
    /// <param name="aiCellIndex"></param>
    private void AddStudentCountLabel(int aiStudentCount, int aiRowIndex, int aiCellIndex, int aiBoysCount, int aiGirlsCount)
    {
        Label oLblCount = new Label { Text = "(" + aiStudentCount + ")" };
        oLblCount.ToolTip = "Boys: " + aiBoysCount + " " + "Girls: " + aiGirlsCount;
        grdStandards.Rows[aiRowIndex].Cells[aiCellIndex].Controls.Add(oLblCount);
    }

    /// <summary>
    /// This method is used to decrypt encrypted querystring.
    /// </summary>
    private void ReadQuerystring()
    {
        try
        {
            if (IsConfig.IsNullOrEmpty() && Request.QueryString.ToString() != Constants.S_EMPTY_STRING)
            {
                if (QueryString["Is_Configured"] != null)
                    IsConfig = QueryString["Is_Configured"];
            }
        }
        catch (Exception ex)
        {	
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
            MasterPage oMasterPage = (MasterPage)Master;
			oMasterPage.RedirectToNextPage(Constants.S_PAGE_ERROR);
        }
    }

    /// <summary>
    /// This method is used to add columns of divisions in the grid.
    /// </summary>
    private void GenerateColumnsOfGrid()
    {
        //add columns of divisions to the grid header row 
        AddDivisionColumnsToHeaderRow();
        //add columns of divisions to other rows of the grid
        AddDivisionColumnsToOtherRows();
    }

    /// <summary>
    /// This method adds division columns to rows other than header row.
    /// </summary>
    /// <param name="aoDSAllDivisions"></param>
    private void AddDivisionColumnsToOtherRows()
    {
        const int I_STANDARD_TABLE_INDEX = 0;
        const int I_STDDIV_TABLE_INDEX = 2;
        const int I_CLASS_STUDENT_TABLE_INDEX = 3;

        DataSet oDsStandards = (DataSet)grdStandards.DataSource;
        DataTable oDtDivisions = oDsStandards.Tables[I_DIV_TABLE_INDEX];
        DataTable oDtClassStudents = oDsStandards.Tables[I_CLASS_STUDENT_TABLE_INDEX];
        DataTable oDtStdDiv = oDsStandards.Tables[I_STDDIV_TABLE_INDEX];

        int iRowCount = grdStandards.Rows.Count;
        int iDivisionCount = oDtDivisions.Rows.Count;
        int iCellIndex = 0;
        int iCount = 0;
        int iStudentCount = 0;
        int iBoysCount = 0;
        int iGirlsCount = 0;
        int iColIndex = 0;
        int iRowIndex = 0;
        TableCell oTableCell;

        //loop through rows
        for (; iRowIndex < iRowCount; iRowIndex++)
        {
            int iStandardId = Convert.ToInt32(grdStandards.DataKeys[iRowIndex][0].ToString());

            //loop through columns
            iColIndex = 0;
            for (; iColIndex < iDivisionCount; iColIndex++)
            {
                oTableCell = new TableCell();
                int iDivId = Convert.ToInt32(oDtDivisions.Rows[iColIndex][Constants.S_DIVISION_ID_FIELD].ToString());
                iCellIndex = grdStandards.Rows[iRowIndex].Cells.Add(oTableCell);

                grdStandards.Rows[iRowIndex].Cells[iCellIndex].HorizontalAlign = HorizontalAlign.Center;
                //if division is applicable (i.e. the division is assigned to the standard.
                DataRow[] oDrStdDiv = oDtStdDiv.Select("division_Id = " + iDivId.ToString() + " AND Standard_Id = " + iStandardId.ToString());

                if (oDrStdDiv.Length > 0)
                {
                    DataRow[] oDrStudents = oDtClassStudents.Select("division_Id = " + iDivId.ToString() + " AND Standard_Id = " + iStandardId.ToString());
                    iStudentCount = 0;
                    iBoysCount = 0;
                    iGirlsCount = 0;
                    if (oDrStudents.Length > 0)
                    {
                        iStudentCount = Convert.ToInt32(oDrStudents[0]["studentCount"]);
                        iBoysCount = Convert.ToInt32(oDrStudents[0]["BoysCount"]);
                        iGirlsCount = Convert.ToInt32(oDrStudents[0]["GirlsCount"]); ;
                    }
                        

                    string sDivisionName = oDtDivisions.Rows[iColIndex][Constants.S_DIVISION_NAME_FIELD].ToString();

                    if (moUserRole == Constants.UserRoles.Admin || ((moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher)  && Convert.ToChar(hidCanEdit.Value) == Constants.C_YES))
                    {
                        if (sIsManagementUser != "Y")
                        {
                            AddNewStudentLink(iRowIndex, iCellIndex, iDivId, sDivisionName);
                            hyperlnk.Visible = true;
                            tdRollNosGeneration.Visible = true;
                            hyperlnkSanctionLeave.Visible = true;
                            hlnkHouseAssignmentDiv.Visible = true;
                            hyperLnkSecondLanguage.Visible = true;
                            btnClose.Visible = false;
                            btnBack.Visible = true;
                            btnStudent.Visible = true;

                            string sQuerystring = CommonUtility.EncryptQuerystring("From=2");
                            hlnkHouseAssignment.NavigateUrl = "~/RITeSchool/Admin/StudentsHouseAssignmentUI.aspx?" + sQuerystring;
                        }
                    }

                    //if there are students in this class
                    //if (iStudentCount > 0)
                    //{
                        iCount++;
                        //add view students link to the class 
                        //this link will take user to list  of students in this class
                        if (sIsManagementUser != "Y")
                        {
                            AddViewStudentLink(iRowIndex, iCellIndex, iDivId);
                        }
                    //}

                    //Add label to display no. of students in the class
                    AddStudentCountLabel(iStudentCount, iRowIndex, iCellIndex, iBoysCount, iGirlsCount);
                }
                else
                {
                    iStudentCount = Convert.ToInt32(oDsStandards.Tables[I_STANDARD_TABLE_INDEX].Rows[iRowIndex]["Original_Standard_Id"]);
                    if (iStudentCount == 9999)
                    {
                        Label otLblCount = new Label();
                        otLblCount.Text = Convert.ToString(oDsStandards.Tables[I_DIV_TABLE_INDEX].Rows[iColIndex]["StudentCount"]);
                        otLblCount.ToolTip = "Boys: " + Convert.ToString(oDsStandards.Tables[I_DIV_TABLE_INDEX].Rows[iColIndex]["DivisionBoys"]) + " " + "Girls: " + Convert.ToString(oDsStandards.Tables[I_DIV_TABLE_INDEX].Rows[iColIndex]["DivisionGirls"]);
                        grdStandards.Rows[iRowIndex].Cells[iCellIndex].Controls.Add(otLblCount);
                        grdStandards.Rows[iRowIndex].Cells[iCellIndex].CssClass = "TotalCount";
                        grdStandards.Rows[iRowIndex].Cells[0].Font.Bold = true;
                    }
                    else
                    {
                        // This is for division not applicable. 
                        grdStandards.Rows[iRowIndex].Cells[iCellIndex].CssClass = "ClsGridNA";
                    }
                }
            }
            //Add total coloumn
            if (iColIndex > 0)
            {
                oTableCell = new TableCell();
                iCellIndex = grdStandards.Rows[iRowIndex].Cells.Add(oTableCell);
                iStudentCount = Convert.ToInt32(oDsStandards.Tables[I_STANDARD_TABLE_INDEX].Rows[iRowIndex]["studentCount"]);
                iBoysCount = Convert.ToInt32(oDsStandards.Tables[I_STANDARD_TABLE_INDEX].Rows[iRowIndex]["StandardBoysCount"]);
                iGirlsCount = Convert.ToInt32(oDsStandards.Tables[I_STANDARD_TABLE_INDEX].Rows[iRowIndex]["StandardGirlsCount"]);
                grdStandards.Rows[iRowIndex].Cells[iCellIndex].HorizontalAlign = HorizontalAlign.Center;
                Label oLblCount = new Label { Text = iStudentCount.ToString() };
                if (iRowIndex != iRowCount - 1)
                    oLblCount.ToolTip = "Boys: " + iBoysCount + " " + "Girls: " + iGirlsCount;
                else
                { 
                    DataTable dtStandard = new DataTable();
                    dtStandard = oDsStandards.Tables[I_STANDARD_TABLE_INDEX];
                    int iAllBoys = Convert.ToInt32(dtStandard.AsEnumerable().Sum(row => row.Field<int>("StandardBoysCount")));
                    int iAllGirls = Convert.ToInt32(dtStandard.AsEnumerable().Sum(row => row.Field<int>("StandardGirlsCount")));
                    oLblCount.ToolTip = "Boys: " + iAllBoys + " " + "Girls: " + iAllGirls;
                }
                grdStandards.Rows[iRowIndex].Cells[iCellIndex].CssClass = "TotalCount";
                grdStandards.Rows[iRowIndex].Cells[iCellIndex].Controls.Add(oLblCount);
                if (iRowIndex == iRowCount - 1)
                {
                    grdStandards.Rows[iRowIndex].Cells[iCellIndex].Font.Bold = true;
                    grdStandards.Rows[iRowIndex].Cells[iCellIndex].Attributes.Add("style", "border: #000 1px solid");
                    grdStandards.Rows[iRowIndex].Cells[iCellIndex].CssClass = "ClsHilightBG";
                }
            }
        }
        if (iCount == 0)
            base.DeleteConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.Student));
    }

    /// <summary>
    /// This method is used to read querystring.
    /// </summary>
    private void ReadQueryString()
    {
        if (QueryString["IsManagementUser"] != null)
            sIsManagementUser = QueryString["IsManagementUser"];
    }

    /// <summary>
    /// This function sets link to add  New Student
    /// </summary>
    private void AddNewStudentLink(int aiRowIndex, int aiCellIndex, int aiDivisionId, string asDivisionName)
    {
        ReadQuerystring();
        string sQuerystring = "StandardId=" + grdStandards.DataKeys[aiRowIndex][0].ToString()
                                  + "&DivisionId=" + aiDivisionId
                                  + "&NewMode=" + Constants.C_YES
                    + "&standardName=" + grdStandards.Rows[aiRowIndex].Cells[I_STANDARD_NAME_COLUMN_NUMBER].Text
                    + "&DivisionName=" + asDivisionName + "&Is_Configured=" + IsConfig; ;

        Image oImg = new Image();
        oImg.ImageUrl = S_IMG_ADD_URL;
        oImg.CssClass = "IconSpacing";
        string sEncrypt = Utility.CommonUtility.EncryptQuerystring(sQuerystring);
        oImg.Attributes.Add("onclick", "window.open('../Teacher/StudentUI.aspx?" + sEncrypt
                                  + "' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=1000,height=700'); return false;");
        oImg.Style.Add(HtmlTextWriterStyle.Cursor, "Hand");
        oImg.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");
        grdStandards.Rows[aiRowIndex].Cells[aiCellIndex].Controls.Add(oImg);
    }

    /// <summary>
    /// This function sets link to add or view All Students
    /// </summary>
    private void AddViewStudentLink(int aiRowIndex, int aiCellIndex, int aiDivisionId)
    {
        int iStandardId = Convert.ToInt32(grdStandards.DataKeys[aiRowIndex][0].ToString());
        string sQuerystring = "StandardId=" + iStandardId
                              + "&DivisionId=" + aiDivisionId
                              + "&NewMode=" + Constants.C_YES
                              + "&Is_Configured=" + IsConfig;
        Image oImg = new Image();
        oImg.ImageUrl = S_IMG_VIEW_URL;
        oImg.CssClass = "IconSpacing";
        oImg.ToolTip = "View students";
        oImg.Style.Add(HtmlTextWriterStyle.Cursor, "Hand");
        oImg.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");
        string sEncrypt = Utility.CommonUtility.EncryptQuerystring(sQuerystring);
        oImg.Attributes.Add("onclick", "window.open('../Teacher/StudentsListUI.aspx?" + sEncrypt
                                  + "' , '_new','scrollbars=yes,resizable=yes,top=0,left=0,width=1000,height=700'); return false;");
        grdStandards.Rows[aiRowIndex].Cells[aiCellIndex].Controls.Add(oImg);

    }

    private void SetQuerystring()
    {
        string sQuerystring = "StandardId=" + 0
                              + "&DivisionId=" + 0
                              + "&NewMode=" + Constants.C_YES
                              + "&Is_Configured=" + IsConfig;

        string sEncrypt = Utility.CommonUtility.EncryptQuerystring(sQuerystring);
        btnStudent.Attributes.Add("onclick", "window.open('../Teacher/StudentsListUI.aspx?" + sEncrypt
                                  + "' , '_new','scrollbars=yes,resizable=yes,top=0,left=0,width=1000,height=700'); return false;");

		hlnkStudentRollNos.NavigateUrl = hlnkStudentRollNos.NavigateUrl + "?" + CommonUtility.EncryptQuerystring("Is_Configured=" + (IsConfig.IsNullOrEmpty() ? Constants.S_NO : IsConfig));


        hlnkStudentAdditionalDetails.NavigateUrl = hlnkStudentAdditionalDetails.NavigateUrl + "?" + CommonUtility.EncryptQuerystring("IsCallFromStudentCountScreen=1");
    }

    #endregion
}