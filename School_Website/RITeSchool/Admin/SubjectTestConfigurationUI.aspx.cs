// File Name     : SubjectTestConfigurationUI.aspx.cs
// Modified By   : Amit 
// Modified Date : 23/09/2009
// Description   : This class is used to subjectwise exam configuration for class.

using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Reflection;
using BusinessLogic.Exceptions;
using BusinessLogic;
using Utility;

public partial class SubjectTestConfigurationUI : SchoolBase
{
    #region Constants

    private const Int32 I_STANDARD_ID_COLUMN_NUMBER = 0;
    private const Int32 I_STANDARD_DIVISION_ID_COLUMN_NUMBER = 1;

    private const string S_IMG_FOR_STANDARD_DIVISION = "~/RITeSchool/images/GridHeader_StdDiv_Sub_Title.gif";
    private const string S_COLUMN_SUBJECT_ID = "Subject_Id";
    private const string S_COLUMN_SUBJECT_NAME = "Subject_Name";
    private const string S_DB_COLUMN_STANDARDDIVISION = "StandardDivision";

    #endregion Constants

    #region " Events "
    private SubjectTestConfigurationBL moSubjectTestConfigurationBL;

    /// <summary>
    /// This event is used to fill grid with standard-division and subjects. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moSubjectTestConfigurationBL = new SubjectTestConfigurationBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                ClearTempararySession();
                InitializePage();
                FillStandardCombobox();
                FillSubjectCombobox();
                ReadQueryString();
            }
            bool bIsUseSubmitBehavior = CommonUtility.CheckCancelOrBackClickEvent(this.Page);
            if (bIsUseSubmitBehavior == true)
                FillStandardwiseDivisionsAndSubjectsInGrid();
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
    protected void btnBack_Click(object sender, EventArgs e)
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

    #endregion " Events "

    #region " Grid Event "

    protected void cmbStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CmbSubject.Items.Clear();
            CmbSubject.Items.Add(new ListItem(Constants.S_SELECT, Constants.I_ZERO.ToString()));
           if (Convert.ToInt32(cmbStandard.SelectedValue) == Constants.I_ZERO)
            {

                grdSubjects.DataSource = null;
                grdSubjects.DataBind();
            }
            else
            {
                  FillSubjectCombobox();
                  FillStandardwiseDivisionsGrid();
                  GenerateSubjectColumnsOfGrid();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
     }

    protected void CmbSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        { 
            FillStandardwiseDivisionsGrid();
            GenerateSubjectColumnsOfGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to apply CSS class for grid header.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdSubjects_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            e.Row.Cells[0].CssClass = "locked";
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].CssClass = "Llocked";
                e.Row.Cells[0].Style.Add("left", grdSubjects.Style["scrollLeft"]);
                e.Row.Cells[0].Style.Add("padding-right", "50px");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion " Grid Events "

    #region " Helping Methods "

    /// <summary>
    /// This method is used to initialize page controls.
    /// </summary>
    private void InitializePage()
    {
        grdSubjects.Columns[0].HeaderImageUrl = S_IMG_FOR_STANDARD_DIVISION;
        grdSubjects.Columns[0].HeaderText = "";        
        ApplyMouseHoverEffect(new List<Button> { btnBack});
    }

    /// <summary>
    /// This method is used to clear session variable. 
    /// </summary>
    private void ClearTempararySession()
    {
        Session[Constants.S_TEMP_SESSION_DS] = null;
    }

    /// <summary>
    /// This method is used to fill standard- divisions and subjects grid. 
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
        if (cmbStandard.SelectedValue != Constants.S_ZERO)
        {
            SubjectTestConfigurationCollectionBL obj = new SubjectTestConfigurationCollectionBL(miSchoolId, miAcademicYearId);
            DataSet oDs = obj.GetClassSubjectTestsAssociation(cmbStandard.SelectedValue.ToInt(), CmbSubject.SelectedValue.ToInt());
            grdSubjects.DataSource = oDs;
            grdSubjects.DataBind();
        }
        else
        {
            grdSubjects.DataSource = null;
            grdSubjects.DataBind();
        }
    }

    /// <summary>
    /// This method is used to generate columns of subjects dynamically added to the 
    /// grid of standard-division.
    /// </summary>
    private void GenerateSubjectColumnsOfGrid()
    {
        const int I_SUBJECT_TABLE_INDEX = 1;
        const int I_SUBJECTTEST_TABLE_INDEX = 2;
        string sQuerystring = string.Empty;

        DataSet oDs = (DataSet)grdSubjects.DataSource;
        DataTable oDtSubjects = oDs.Tables[I_SUBJECT_TABLE_INDEX];
        DataTable oDtTests = oDs.Tables[I_SUBJECTTEST_TABLE_INDEX];
        int iCellIndex = 0;
        int iHeaderCellNo = 0;
        int iSubjectIndex;
        int cnt = 0;
        bool bIsConfigured = false;
        DataRow[] oDrConfigured = oDtTests.Select("Testwise_Subject_Marks_Id<>0");
        if (oDrConfigured.Length > 0)
        {
            bIsConfigured = true;
        }
        //This loop is for header of the grid.
        //This loop is for generating new table cells for respective subjects and standard-division.
        for (int iRowIndex = 0; iRowIndex < grdSubjects.Rows.Count; iRowIndex++)
        {
            int iStandardId = Convert.ToInt32(grdSubjects.DataKeys[iRowIndex][I_STANDARD_ID_COLUMN_NUMBER].ToString());
            for (iSubjectIndex = 0; iSubjectIndex < oDtSubjects.Rows.Count; iSubjectIndex++)
            {
                DataControlFieldHeaderCell oTHeader = new DataControlFieldHeaderCell(null);
                if (iRowIndex == 0)
                {
                    oTHeader.Style.Add(HtmlTextWriterStyle.PaddingLeft, "3px");
                    oTHeader.Style.Add(HtmlTextWriterStyle.PaddingRight, "3px");

                    oTHeader.Text = oDtSubjects.Rows[iSubjectIndex][S_COLUMN_SUBJECT_NAME].ToString();
                    oTHeader.HorizontalAlign = HorizontalAlign.Center;
                    oTHeader.Wrap = false;

                    iHeaderCellNo = grdSubjects.HeaderRow.Cells.Add(oTHeader);
                }

                TableCell oTableCell = new TableCell();
                oTableCell.Style.Add(HtmlTextWriterStyle.PaddingLeft, "3px");
                oTableCell.Style.Add(HtmlTextWriterStyle.PaddingRight, "3px");
                oTableCell.Wrap = false;
                oTableCell.Text = oDtSubjects.Rows[iSubjectIndex][S_COLUMN_SUBJECT_ID].ToString();
                oTableCell.Attributes.Add("title", "Class " + oDs.Tables[0].Rows[iRowIndex][S_DB_COLUMN_STANDARDDIVISION].ToString() + " [" + oDtSubjects.Rows[iSubjectIndex][S_COLUMN_SUBJECT_NAME].ToString() + "]");
                iCellIndex = grdSubjects.Rows[iRowIndex].Cells.Add(oTableCell);
                grdSubjects.Rows[iRowIndex].Cells[iCellIndex].HorizontalAlign = HorizontalAlign.Center;

                int iSubjectId = Convert.ToInt32(grdSubjects.Rows[iRowIndex].Cells[iCellIndex].Text);

                int iStandardDivisionId = Convert.ToInt32(grdSubjects.DataKeys[iRowIndex][I_STANDARD_DIVISION_ID_COLUMN_NUMBER].ToString());
                DataRow[] oDrClassSubjects = oDtTests.Select("Standard_Division_Id = " + iStandardDivisionId.ToString() + " AND subject_id=" + iSubjectId.ToString());
                //if class subject applicable
                if (oDrClassSubjects.Length > 0)
                {
                    ////Check that test configuration is done or not. 
                    if (!oDrClassSubjects[0]["Testwise_Subject_Marks_Id"].ToString().Equals("0"))
                    {
                        sQuerystring = "StandardDivisionId=" + iStandardDivisionId
                                          + "&SubjectId=" + iSubjectId
                                          + "&ViewMode=" + Constants.ViewMode.Edit.ToString()
                                          + "&SubjectName=" + oDtSubjects.Rows[iSubjectIndex][S_COLUMN_SUBJECT_NAME].ToString().Replace("&", "~")
                                          + "&StdDivName=" + grdSubjects.Rows[iRowIndex].Cells[0].Text
                                          + "&StdId=" + iStandardId.ToString()
                                          + "&IsConfig=" + bIsConfigured.ToString()
                                          + "&SelectedStdId=" + cmbStandard.SelectedValue
                                          + "&SelectedSubjectId=" + CmbSubject.SelectedValue;
                        Label oLbl = AddLinkToUpdateTestConfiguration(sQuerystring);

                        grdSubjects.Rows[iRowIndex].Cells[iCellIndex].Controls.Add(oLbl);
                        grdSubjects.Rows[iRowIndex].Cells[iCellIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "#aae2cd");
                        cnt++;
                    }
                    else
                    {
                        sQuerystring = "StandardDivisionId=" + iStandardDivisionId
                                 + "&SubjectId=" + iSubjectId
                                 + "&ViewMode=" + Constants.ViewMode.New.ToString()
                                 + "&SubjectName=" + oDtSubjects.Rows[iSubjectIndex][S_COLUMN_SUBJECT_NAME].ToString().Replace("&", "~")
                                 + "&StdDivName=" + grdSubjects.Rows[iRowIndex].Cells[0].Text
                                 + "&StdId=" + iStandardId.ToString()
                                 + "&IsConfig=" + bIsConfigured.ToString()
                                 + "&SelectedStdId=" + cmbStandard.SelectedValue
                                 + "&SelectedSubjectId=" + CmbSubject.SelectedValue;
                        Label oLbl = AddLinkForTestConfiguration(sQuerystring);
                        grdSubjects.Rows[iRowIndex].Cells[iCellIndex].Controls.Add(oLbl);
                        grdSubjects.Rows[iRowIndex].Cells[iCellIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "#5dad8e");
                    }
                }
                else
                {
                    oTableCell.Text = Constants.S_EMPTY_STRING;
                    grdSubjects.Rows[iRowIndex].Cells[iCellIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "#eaeaea");
                }
            }
        }
        if (cnt == 0)
            DeleteConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.SubjectExamConfig));
    }

    /// <summary>
    /// This method is used to add hyperlink to the table cell
    /// if exam configuration not done.
    /// </summary>
    private Label AddLinkForTestConfiguration(string asQuerystring)
    {
        Label oLbl = new Label();

        string sEncrypt = Utility.CommonUtility.EncryptQuerystring(asQuerystring);
        string sURL = Constants.S_PAGE_SUBJECT_TEST_CONFIGURATION + "?" + sEncrypt;
        oLbl.Text = Resources.LocalizedResources.NotConfigured;
        oLbl.ForeColor = System.Drawing.Color.White;
        oLbl.Font.Bold = true;
        oLbl.Style.Add(HtmlTextWriterStyle.Cursor, "Hand");
        oLbl.Style.Add(HtmlTextWriterStyle.Cursor, "Pointer");
        oLbl.Style.Add(HtmlTextWriterStyle.TextDecoration, "underline");
        oLbl.Attributes.Add("onclick", "window.open('" + sURL
                                  + "' , '_self','scrollbars=yes,resizable=no,top=0,left=0,width=900,height=610'); return false;");
        return oLbl;
    }

    /// <summary>
    /// This method is used to add hyperlink to the table cell
    /// if exam configuration allready done.
    /// </summary>
    private Label AddLinkToUpdateTestConfiguration(string asQuerystring)
    {
        Label oLbl = new Label();

        string sEncrypt = Utility.CommonUtility.EncryptQuerystring(asQuerystring);
        string sURL = Constants.S_PAGE_SUBJECT_TEST_CONFIGURATION + "?" + sEncrypt;
        oLbl.Text = Resources.LocalizedResources.ExamConfiguration;
        oLbl.ForeColor = System.Drawing.Color.Black;
        oLbl.Font.Bold = true;
        oLbl.Style.Add(HtmlTextWriterStyle.Cursor, "Hand");
        oLbl.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");
        oLbl.Style.Add(HtmlTextWriterStyle.TextDecoration, "underline");
        oLbl.Attributes.Add("onclick", "window.open('" + sURL.Replace("~","..")
                                  + "' , '_self','scrollbars=yes,resizable=no,top=0,left=0,width=900,height=610'); return false;");
        return oLbl;
    }


    /// <summary>
    /// This function checks the preconditons of Configured Subjects for Subject Group criteria.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.SubjectExamConfig);

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
        grdSubjects.Visible = false;
        LegendTable.Visible = false;
        tdGrid.Visible = false;
    }

    /// <summary>
    /// This method is used to read query string details.
    /// </summary>
    private void ReadQueryString()
    {
        if (QueryString["SelectedStdId"] != null)
        {
            cmbStandard.SelectedValue = QueryString["SelectedStdId"].ToString();
            FillSubjectCombobox();
        }

        if (QueryString["SelectedSubjectId"] != null)
            CmbSubject.SelectedValue = QueryString["SelectedSubjectId"].ToString();
    }

    /// <summary>
    /// This method is used to fill standard dropdownlist.
    /// </summary>
    private void FillStandardCombobox()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtStandard = oStandardCollectionBL.GetAssociatedStandards();
        ListSource.FillDropDownList(oDtStandard, cmbStandard, "standard_name", "standard_id", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill subject dropdownlist.
    /// </summary>
    private void FillSubjectCombobox()
    {
        DataTable dtSubjects = moSubjectTestConfigurationBL.GetFillStandardWiseSubjects(cmbStandard.SelectedValue.ToInt());
        CmbSubject.Bind(dtSubjects, "Subject_Id", "Subject_Name", Constants.S_ALL);
    }

    #endregion
    
}


