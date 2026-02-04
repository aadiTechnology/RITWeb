/* File Name    :   DisplayAssignedClassTeacherUI.aspx.cs
 * Purpose      :   This class is used to display all class teachers assigned
 *                  to division.
 * File Modified:   13-Nov-2007
 */

using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Reflection;
using BusinessLogic.Exceptions;
using BusinessLogic;
using Utility;

public partial class DisplayAssignedClassTeacherUI : SchoolBase
{
    #region Constants

    private const Int32 I_STANDARD_NAME_COLUMN_NUMBER = 0;
    private const Int32 I_DATAKEY_STANDARD_ID = 0;
    private const string S_COLUMN_TEACHER_ID = "Teacher_Id";
    private const string S_COLUMN_TEACHER_NAME = "TeacherName";
    private const string S_STANDARDS = "- Standards";
    private const string S_DIVISIONS = "- Divisions";
    private const string S_STANDARDWISE_DIVISION = "- Standardwise Divisions";
    private const string S_LNK_ASSIGN_CLASS_TEACHER = " Assign Class Teacher";

    private const string S_IMG_FOR_STANDARD_DIVISION = "~/RITeSchool/images/GridHeader_StdDiv_Title.gif";
    #endregion

    #region Datamembers

    private SchoolWiseTeacherMasterBL moSchoolWiseTeacherMasterBL;
    private string msQuerystring;
    private string IsConfig;
    #endregion

    #region Events

    /// <summary>
    ///  This method is used to fill grid with standards and their respective
    ///  divisions,also shows class teacher to respective class of the school.
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
                ReadQuerystring();             
            }
            grdStandards.Columns[0].HeaderImageUrl = S_IMG_FOR_STANDARD_DIVISION;
            grdStandards.Columns[0].HeaderText = "";
            bool bIsUseSubmitBehavior = CommonUtility.CheckCancelOrBackClickEvent(this.Page);
            if (bIsUseSubmitBehavior == true)
            {                
                FillStandardGrid();
            }           
                    
            ApplyMouseHoverEffect(new List<Button> { btnBack });
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();                
            }
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
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Teacher_Related)));            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Grid Event

    protected void grdStandards_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].CssClass = "locked";
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[0].CssClass = "Llocked";
            e.Row.Cells[0].Style.Add("left", grdStandards.Style["scrollLeft"]);
        }
    }

    #endregion

    #region Helping Methods


    /// <summary>
    /// This method is used to decrypt encrypted querystring.
    /// </summary>
    private void ReadQuerystring()
    {
        try
        {
            if (Request.QueryString.ToString() != Constants.S_EMPTY_STRING)
            {
                IsConfig = QueryString["Is_Configured"];
            }
        }
        catch (Exception)
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
			oMasterPage.RedirectToNextPage(Constants.S_PAGE_ERROR);
        }
    }


    /// <summary>
    /// This function checks the preconditons of Configured Std-Divisions for Class Teacher Assignment criteria.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.ClassTeacher);
        if (sLinks.Equals(""))
        {
            divErr.Visible = false;
            bReturn = true;
        }
        else
        {
            divErr.InnerHtml = sLinks;
            LegendTable.Visible = false;
            grdStandards.Visible = false;
            tdGrid.Visible = false;
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
            DataSet oDS = SchoolWiseStandardDivisionTeacherAssignmentMasterBL.GetStdDivTeacherAssociation(miSchoolId, miAcademicYearId);
            grdStandards.DataSource = oDS;
            grdStandards.DataBind();
            GenerateColumnsOfGrid();
        }
    }

    /// <summary>
    /// This method is used to add columns of divisions in the grid.
    /// </summary>
    private void GenerateColumnsOfGrid()
    {

        moSchoolWiseTeacherMasterBL = new SchoolWiseTeacherMasterBL();
        const string S_CSS_NA = "ClsNotAssignDark";
        const int I_DIV_TABLE_INDEX = 1;// the table for divisions
        const int I_STDDIV_TABLE_INDEX = 2;// the table for std-divisions
        const int I_STDDIVTEACHER_TABLE_INDEX = 3;// the table for assigned classteachers
        const int I_STDTEACHER_TABLE_INDEX = 4;// the table for teacher-std association

        DataSet oDs = (DataSet)grdStandards.DataSource;
        DataTable oDtDivisions = oDs.Tables[I_DIV_TABLE_INDEX];
        DataTable oDtStdDivTeachers = oDs.Tables[I_STDDIVTEACHER_TABLE_INDEX];
        DataTable oDtStdDivs = oDs.Tables[I_STDDIV_TABLE_INDEX];
        DataTable oDtStdTeachers = oDs.Tables[I_STDTEACHER_TABLE_INDEX];
        //The dataset contains 
        //std_id and no. pf associated teachers
        TeacherStandardDetailsCollectionBL oTeacherStds = new TeacherStandardDetailsCollectionBL(miSchoolId, miAcademicYearId);

        int iCount = oDtDivisions.Rows.Count;
        int iCellIndex = 0;
        int cnt = 0;
        for (int iRowIndex = 0; iRowIndex < iCount; iRowIndex++)
        {
            DataControlFieldHeaderCell oTableCell1 = new DataControlFieldHeaderCell(null);
            oTableCell1.CssClass = "locked";
            oTableCell1.HorizontalAlign = HorizontalAlign.Center;

            oTableCell1.Width = System.Web.UI.WebControls.Unit.Point(900);
            oTableCell1.Wrap = false;
            oTableCell1.Text = oDtDivisions.Rows[iRowIndex][Constants.S_DIVISION_NAME_FIELD].ToString();
            grdStandards.HeaderRow.Cells.Add(oTableCell1);
            TableCell oTableCell;

            for (int iRowCount = 0; iRowCount < grdStandards.Rows.Count; iRowCount++)
            {
                int iStandardId = Convert.ToInt32(grdStandards.DataKeys[iRowCount][I_DATAKEY_STANDARD_ID].ToString());

                oTableCell = new TableCell();
                int iDivId = Convert.ToInt32(oDtDivisions.Rows[iRowIndex][Constants.S_DIVISION_ID_FIELD].ToString());
                oTableCell.Wrap = false;
                oTableCell.HorizontalAlign = HorizontalAlign.Center;
                oTableCell.Attributes.Add("title", Resources.LocalizedResources.Standard +" - " + oDs.Tables[0].Rows[iRowCount]["Standard_Name"].ToString() + " [" + oDtDivisions.Rows[iRowIndex]["Division_Name"].ToString()+"]");
                iCellIndex = grdStandards.Rows[iRowCount].Cells.Add(oTableCell);

                DataRow[] oDrClassTeachers = oDtStdDivTeachers.Select("standard_Id = " + iStandardId.ToString() + " AND Division_Id = " + iDivId.ToString());
                grdStandards.Rows[iRowCount].Cells[iCellIndex].HorizontalAlign = HorizontalAlign.Center;
                Label oLbl;
                //check if class teacher is assigned
                if (oDrClassTeachers.Length > 0)
                {
                    string iTeacherId="";
                    if (oDrClassTeachers.Length == 1)
                    {
                        iTeacherId = oDrClassTeachers[0][S_COLUMN_TEACHER_ID].ToString();
                        hidTeacherName.Value = oDrClassTeachers[0][S_COLUMN_TEACHER_NAME].ToString();
                    }
                    else
                    {
                        hidTeacherName.Value = "";
                        foreach (DataRow oRow in oDrClassTeachers)
                        {
                            iTeacherId += oRow[S_COLUMN_TEACHER_ID].ToString() + ",";
                            hidTeacherName.Value += oRow[S_COLUMN_TEACHER_NAME].ToString() + " / ";
                        }
                        iTeacherId = iTeacherId.Substring(0, iTeacherId.LastIndexOf(","));
                        hidTeacherName.Value = hidTeacherName.Value.Substring(0, hidTeacherName.Value.LastIndexOf(" / "));
                    }
                        ReadQuerystring();
                    msQuerystring = "StandardId=" + iStandardId
                                       + "&DivisionId=" + iDivId
                                       + "&TeacherId=" + iTeacherId
                                       +"&Is_Configured=" + IsConfig;
                    oLbl = AddLinkToRemoveAssignClassTeacher();
                    grdStandards.Rows[iRowCount].Cells[iCellIndex].Controls.Add(oLbl);
                     grdStandards.Rows[iRowCount].Cells[iCellIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "#aae2cd");
                    cnt++;
                }
                // if class teacher is not assigned
                else
                {
                    DataRow[] oDrDivisions = oDtStdDivs.Select("standard_Id = " + iStandardId.ToString() + " AND Division_Id = " + iDivId.ToString());
                    // if class division applicable to std
                    if (oDrDivisions.Length > 0)
                    {
                        DataRow[] oDr = oDtStdTeachers.Select("Standard_Id =" + iStandardId.ToString());
                        // if the teacher is associated to the std.
                        if (oDr.Length > 0)
                        {
                            //This is for teacher not assigned.
                            ReadQuerystring();
                            msQuerystring = "StandardId=" + iStandardId
                                           + "&DivisionId=" + iDivId
                                           + "&Is_Configured=" + IsConfig;
                            oLbl = AddLinkToAssignClassTeacher();
                            grdStandards.Rows[iRowCount].Cells[iCellIndex].HorizontalAlign = HorizontalAlign.Center;
                            grdStandards.Rows[iRowCount].Cells[iCellIndex].Controls.Add(oLbl);
                            grdStandards.Rows[iRowCount].Cells[iCellIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "#5dad8e");
                           

                        }
                        else
                        {
                            grdStandards.Rows[iRowCount].Cells[iCellIndex].CssClass = S_CSS_NA;
                        }
                    }
                    else
                    {
                        // This is for division not applicable. 
                        grdStandards.Rows[iRowCount].Cells[iCellIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "#eaeaea");
                    }
                }
            }
        }
        if (cnt == 0)
            DeleteConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.ClassTeacher));
    }

    private Label AddLinkToAssignClassTeacher()
    {
        Label oLbl = new Label();
        oLbl.Text = Resources.LocalizedResources.AssignClassTeacher;// S_LNK_ASSIGN_CLASS_TEACHER;
        string sEncrypt = Utility.CommonUtility.EncryptQuerystring(msQuerystring);
        oLbl.ForeColor = System.Drawing.Color.White;
        oLbl.Font.Bold = false; 
        oLbl.Font.Size = 9;
        oLbl.Font.Name = "Arial";
        oLbl.Style.Add(HtmlTextWriterStyle.TextDecoration, "underline");
        oLbl.Style.Add(HtmlTextWriterStyle.Cursor, "Hand");
        oLbl.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");
        oLbl.Style.Add(HtmlTextWriterStyle.TextAlign, "Center");
        oLbl.Attributes.Add("onclick", "window.open('AssignClassTeacherForDivisionPopUp.aspx?" + sEncrypt
                                    + "' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=700,height=400'); return false;");
        return oLbl;
    }

    private Label AddLinkToRemoveAssignClassTeacher()
    {
        Label oLbl = new Label();
        oLbl.Text = hidTeacherName.Value;
        string sEncrypt = Utility.CommonUtility.EncryptQuerystring(msQuerystring);
        oLbl.ForeColor = System.Drawing.Color.Black;
        oLbl.Font.Size = 8;
        oLbl.Font.Name = "verdana";
        oLbl.Style.Add(HtmlTextWriterStyle.TextDecoration, "underline");
        oLbl.Font.Bold = true;
        oLbl.Style.Add(HtmlTextWriterStyle.Cursor, "Hand");
        oLbl.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");
        oLbl.Attributes.Add("onclick", "window.open('AssignClassTeacherForDivisionPopUp.aspx?" + sEncrypt
                                    + "' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=700,height=400'); return false;");
        return oLbl;
    }

    #endregion
}