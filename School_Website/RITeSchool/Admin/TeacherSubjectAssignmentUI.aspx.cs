/* File Name :- TeacherSubjectAssignmentUI.aspx.cs
 * Modified By :- Sachin
 * Modified Date :- 24-Sept-2009
 * Purpose :- Code Review.
 * Class Description :- This class is used to set association between teacher and class-subject.
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

public partial class TeacherSubjectAssignmentUI : SchoolBase
{
    #region Constants

    const int I_STANDARD_DIVISION = 0;
    const int I_SUBJECT = 1;
    const int I_TEACHER = 2;
    const int I_STANDARD_DIVISION_SUBJECT_ASSOCIATION = 3;
    const int I_TEACHER_STANDARD_DIVISION_SUBJECT_ASSOCIATION = 4;

    const string S_COLUMN_SUBJECT_NAME = "Subject_Name";
    const string S_IMG_FOR_STANDARD_DIVISION = "~/RITeSchool/images/GridHeader_StdDiv_Sub_Title.gif";

    #endregion
    #region Datamembers

    string msQuerystring;
    Label moHyperLink;
    DataSet moDSAllStdandardDivisions = null;
    string msIsConfig;

    #endregion

    #region Events

    /// <summary>
    /// This method is used to fill grid with standard-division and subjects. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SetStandardDivisionImage();
                FillStandardCombo();
                ReadQuerystring();
                FillStandardwiseDivisionsGrid();                 
            }
            CheckSubmitBehavior();   
            SetJavascriptAttributes();
            SetPostbackURL();            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set style to first column of gridview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStandardDivision_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            e.Row.Cells[0].CssClass = "locked";
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.Cells[0].CssClass = "Llocked";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to fill division's combo.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            SetStandardDivisionImage();
            FillStandardwiseDivisionsAndSubjectsInGrid();                          
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
          //  txtSearch.Text = hidTeacherName.Value;
            SetStandardDivisionImage();
            FillStandardwiseDivisionsAndSubjectsInGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
        
    }

    #endregion

    #region Methods
    /// <summary>
    /// This method is used to fill standard's combo.
    /// </summary>
    private void FillStandardCombo()
    {
       YearWIseStudentsBL oYearWiseSTudentInfoBL = new YearWIseStudentsBL();
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDSStandardCollection = oStandardCollectionBL.GetAssociatedStandards();
        ControlUtility.FillDropDownList(oDSStandardCollection, ref ddlStandard,
                                       Constants.S_STANDARD_ID_FIELD,
                                       Constants.S_STANDARD_NAME_FIELD,
                                       Constants.S_ALL);
        if (QueryString["StandardId"] != null && QueryString["StandardId"].ToString() != string.Empty)
        {
            ddlStandard.SelectedValue = QueryString["StandardId"].ToString();
            ddlCategory.SelectedValue = QueryString["CategoryId"].ToString();
            txtSearch.Text = QueryString["Name"].ToString();
        }
        else
        {
            if (Settings.ShowAllClassesForStdClassAssignment == false && ddlStandard.Items.Count >= 1)
                ddlStandard.SelectedIndex = 1;
        }     
    }

    /// <summary>
    /// This method is used to decrypt querystring.
    /// </summary>
    private void ReadQuerystring()
    {
        msIsConfig = QueryString["Is_Configured"];           
    }

    /// <summary>
    /// This function checks the preconditons of Configured Teachers and Std-Divisions for Class Teacher- Subject Assignment criteria.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.AssignedTeacherToSub);

        if (sLinks.Equals(string.Empty))
        {
            divErr.Visible = false;
            bReturn = true;
        }
        else
        {
            divErr.InnerHtml = sLinks;
            HideControls();
        }
        return bReturn;
    }

    /// <summary>
    /// This method is used to fill standard- divisions and subjects 
    /// </summary>
    private void FillStandardwiseDivisionsAndSubjectsInGrid()
    {
        if (CheckPreCondition())
        {
            FillStandardwiseDivisionsGrid();
            GenerateSubjectColumnsOfGrid();
        }
    }
    /// <summary>
    /// This method is used to fill grid with standard-divisions.
    /// </summary>
    private void FillStandardwiseDivisionsGrid()
    {
        TeacherSubjectAssignmentCollectionBL oTeacherSubjectAssignmentCollectionBL = new TeacherSubjectAssignmentCollectionBL();
        moDSAllStdandardDivisions = oTeacherSubjectAssignmentCollectionBL.GetTeacherSubjectAssociation(miSchoolId, miAcademicYearId, Convert.ToInt32(ddlStandard.SelectedValue), txtSearch.Text.Trim(),ddlCategory.SelectedValue);
        grdStandardDivision.DataSource = moDSAllStdandardDivisions.Tables[I_STANDARD_DIVISION];
        grdStandardDivision.DataBind();        
    }

    /// <summary>
    /// This method is used to set postback URL.
    /// </summary>
    private void SetPostbackURL()
    {
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Teacher_Related));
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnBack });
       
    }

    /// <summary>
    /// This method is used to check fill gridview by checking submitbehavior property.
    /// </summary>
    private void CheckSubmitBehavior()
    {
        bool bIsUseSubmitBehavior = CommonUtility.CheckCancelOrBackClickEvent(this.Page);
        if (bIsUseSubmitBehavior == true)
            FillStandardwiseDivisionsAndSubjectsInGrid();
    }

    /// <summary>
    /// This method is used to set standard division image to header of grid column.
    /// </summary>
    private void SetStandardDivisionImage()
    {        
        grdStandardDivision.Columns[0].HeaderImageUrl = S_IMG_FOR_STANDARD_DIVISION;
        grdStandardDivision.Columns[0].HeaderText = string.Empty;
    }

    /// <summary>
    /// This method is used to generate columnn for subject-teacher assigment w.r.t. standard-division.
    /// </summary>
    private void GenerateSubjectColumnsOfGrid()
    {
        const Int32 I_STANDARD_DIVISION_ID_COLUMN_INDEX = 1;
        const string S_COLUMN_TEACHER_ID = "Teacher_Id";
        const string S_COLUMN_TEACHER_NAME = "TeacherName";        

        SubjectCollectionBL oSubjectCollectionBL = new SubjectCollectionBL(miSchoolId, miAcademicYearId);
        int iCellIndex = 0;        
        int iSubjectIndex;
        int iTeacherAssignmentCount = 0;        
        CreateGridHeader();

        var oCombinationSet = new HashSet<string>(
            moDSAllStdandardDivisions.Tables[I_TEACHER].AsEnumerable().Select(r => r.Field<int>("SchoolWise_Standard_Division_Id") + "|" + r.Field<int>("Subject_Id"))
        );
        
        //this loop is used for generating new table cells for respective subjects and standard-division.
        for (int iRowIndex = 0; iRowIndex < grdStandardDivision.Rows.Count; iRowIndex++)
        {
            int iStandardDivisionId = Convert.ToInt32(grdStandardDivision.DataKeys[iRowIndex][I_STANDARD_DIVISION_ID_COLUMN_INDEX].ToString());
            TableCell oTableCell = null;
            for (iSubjectIndex = 0; iSubjectIndex < moDSAllStdandardDivisions.Tables[I_SUBJECT].Rows.Count; iSubjectIndex++)
            {
                oTableCell = new TableCell();           
                oTableCell.Wrap = false;
                oTableCell.Style.Add(HtmlTextWriterStyle.PaddingLeft, "3px");
                oTableCell.Style.Add(HtmlTextWriterStyle.PaddingRight, "3px");
                oTableCell.Text = moDSAllStdandardDivisions.Tables[I_SUBJECT].Rows[iSubjectIndex]["Subject_Id"].ToString();
                oTableCell.Attributes.Add("title", Resources.LocalizedResources.Class +" "+ moDSAllStdandardDivisions.Tables[I_STANDARD_DIVISION].Rows[iRowIndex]["StandardDivision"].ToString() + " [" + moDSAllStdandardDivisions.Tables[I_SUBJECT].Rows[iSubjectIndex][S_COLUMN_SUBJECT_NAME].ToString() + "]");
                iCellIndex = grdStandardDivision.Rows[iRowIndex].Cells.Add(oTableCell);
                grdStandardDivision.Rows[iRowIndex].Cells[iCellIndex].HorizontalAlign = HorizontalAlign.Center;

                int iSubjectId = Convert.ToInt32(grdStandardDivision.Rows[iRowIndex].Cells[iCellIndex].Text);               
                DataRow[] oArrDivSubjects = moDSAllStdandardDivisions.Tables[I_STANDARD_DIVISION_SUBJECT_ASSOCIATION].Select("Standard_Division_Id =" + iStandardDivisionId + " AND subject_id = " + iSubjectId);
                if (oArrDivSubjects.Length > 0)
                {  
                    //DataRow[] oArrTeacherSubjects = moDSAllStdandardDivisions.Tables[I_TEACHER].Select("SchoolWise_Standard_Division_Id =" + iStandardDivisionId + " AND subject_id = " + iSubjectId);
                    string sKey = iStandardDivisionId + "|" + iSubjectId;                    
                    if (oCombinationSet.Contains(sKey))
                    {
                        DataRow[] oArrSubjectTeachers = moDSAllStdandardDivisions.Tables[I_TEACHER_STANDARD_DIVISION_SUBJECT_ASSOCIATION].Select("Standard_Division_Id =" + iStandardDivisionId + " AND subject_id = " + iSubjectId);                     
                        if (oArrSubjectTeachers.Length > 0)
                        {
                            string iTeacherId = string.Empty;
                            string iSubjectTeacherId = string.Empty;
                            if (oArrSubjectTeachers.Length == 1)
                            {
                                iTeacherId = (oArrSubjectTeachers[0][S_COLUMN_TEACHER_ID]).ToString();
                                hidTeacherName.Value = oArrSubjectTeachers[0][S_COLUMN_TEACHER_NAME].ToString();
                                iSubjectTeacherId = (oArrSubjectTeachers[0]["Teacher_Subject_Id"]).ToString();
                            }
                            else
                            {
                                hidTeacherName.Value = string.Empty;
                                foreach (DataRow oRow in oArrSubjectTeachers)
                                {
                                    iTeacherId += oRow[S_COLUMN_TEACHER_ID].ToString() + ",";
                                    hidTeacherName.Value += oRow[S_COLUMN_TEACHER_NAME].ToString() + " / ";
                                    iSubjectTeacherId += oRow["Teacher_Subject_Id"].ToString() + ",";
                                }
                                iSubjectTeacherId = iSubjectTeacherId.Substring(0, iSubjectTeacherId.LastIndexOf(","));
                                iTeacherId = iTeacherId.Substring(0, iTeacherId.LastIndexOf(","));
                                hidTeacherName.Value = hidTeacherName.Value.Substring(0, hidTeacherName.Value.LastIndexOf(" / "));                                
                            }
                            
                            ReadQuerystring();
                            msQuerystring = "StandardDivisionId=" + iStandardDivisionId
                                               + "&SubjectId=" + iSubjectId
                                               + "&TeacherId=" + iTeacherId
                                               + "&TeacherSubjectId=" + iSubjectTeacherId
                                               + "&Is_Configured=" + msIsConfig
                                               + "&StandardId=" + ddlStandard.SelectedValue
                                               + "&CategoryId=" + ddlCategory.SelectedValue
                                               + "&Name=" + txtSearch.Text.Trim();

                            AddLinkToModifyTeacherAssignment();
                            moHyperLink.Visible = true;
                            grdStandardDivision.Rows[iRowIndex].Cells[iCellIndex].Controls.Add(moHyperLink);
                            grdStandardDivision.Rows[iRowIndex].Cells[iCellIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor,"#aae2cd");
                            iTeacherAssignmentCount++;
                        }
                        else
                        {
                            ReadQuerystring();
                            msQuerystring = "StandardDivisionId=" + iStandardDivisionId
                                            + "&SubjectId=" + iSubjectId
                                            + "&TeacherSubjectId=0"
                                            + "&Is_Configured=" + msIsConfig
                                            + "&StandardId="+ddlStandard.SelectedValue
                                            + "&CategoryId=" + ddlCategory.SelectedValue
                                            + "&Name=" + txtSearch.Text.Trim();
                            AddLinkToAssignTeacher();
                            grdStandardDivision.Rows[iRowIndex].Cells[iCellIndex].Controls.Add(moHyperLink);
                            grdStandardDivision.Rows[iRowIndex].Cells[iCellIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "#5DAD8E");
                        }
                    }
                    else
                    {
                        // Teacher not available
                        oTableCell.Text = Constants.S_EMPTY_STRING;
                        grdStandardDivision.Rows[iRowIndex].Cells[iCellIndex].BackColor = System.Drawing.Color.FromArgb(230, 233, 199);
                    }
                }
                else
                {
                    oTableCell.Text = Constants.S_EMPTY_STRING;
                    grdStandardDivision.Rows[iRowIndex].Cells[iCellIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "#eaeaea");
                }
            }
        }
        if (iTeacherAssignmentCount == 0)
            DeleteConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.AssignedTeacherToSub));
    }

    /// <summary>
    /// This method is used to create grid header.
    /// </summary>
    private void CreateGridHeader()
    {        
        for (int iSubjectIndex = 0; iSubjectIndex < moDSAllStdandardDivisions.Tables[I_SUBJECT].Rows.Count; iSubjectIndex++)
        {
            DataControlFieldHeaderCell oTHeader = new DataControlFieldHeaderCell(null);
            oTHeader.CssClass = "locked";
            oTHeader.HorizontalAlign = HorizontalAlign.Center;
            oTHeader.Style.Add(HtmlTextWriterStyle.PaddingLeft, "3px");
            oTHeader.Style.Add(HtmlTextWriterStyle.PaddingRight, "3px");
            oTHeader.Wrap = false;
            oTHeader.Text = moDSAllStdandardDivisions.Tables[I_SUBJECT].Rows[iSubjectIndex][S_COLUMN_SUBJECT_NAME].ToString();
            grdStandardDivision.HeaderRow.Cells.Add(oTHeader);
        }
    }

    /// <summary>
    /// This method is used to add hyperlink to the table cell where we have to assign teacher.
    /// </summary>
    private void AddLinkToAssignTeacher()
    {
        moHyperLink = new Label();
        moHyperLink.Text = Resources.LocalizedResources.AssignTeacher;
        moHyperLink.ForeColor = System.Drawing.Color.White;
        moHyperLink.Font.Bold = false;
        moHyperLink.Font.Size = 9;
        moHyperLink.Font.Name = "Arial";
        moHyperLink.Style.Add(HtmlTextWriterStyle.TextDecoration, "Underline");
        moHyperLink.Style.Add(HtmlTextWriterStyle.Cursor, "Hand");
        moHyperLink.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");        
        moHyperLink.Attributes.Add("onclick", "window.open('TeacherSubjectAssignmentPopUp.aspx?" + CommonUtility.EncryptQuerystring(msQuerystring)
                                    + "' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=700,height=450'); return false;");

    }

    /// <summary>
    /// This method is used to provide link to remove assignment of teacher or add new teacher.
    /// </summary>
    private void AddLinkToModifyTeacherAssignment()
    {
        moHyperLink = new Label();
        moHyperLink.Text = hidTeacherName.Value;
        moHyperLink.ForeColor = System.Drawing.Color.Black;
        moHyperLink.Font.Size = 8;
        moHyperLink.Font.Name = "verdana";
        moHyperLink.Style.Add(HtmlTextWriterStyle.TextDecoration, "Underline");
        moHyperLink.Font.Bold = true;
        moHyperLink.Style.Add(HtmlTextWriterStyle.Cursor, "Hand");
        moHyperLink.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");        
        moHyperLink.Attributes.Add("onclick", "window.open('TeacherSubjectAssignmentPopUp.aspx?" + CommonUtility.EncryptQuerystring(msQuerystring)
                                    + "' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=700,height=450'); return false;");
    }

    /// <summary>
    /// This method is used to hide controls as per requirement.
    /// </summary>
    private void HideControls()
    {
        GridViewScrollContainer.Visible = false;
        LegendTable.Visible = false;
    }

    
    #endregion
   
}
