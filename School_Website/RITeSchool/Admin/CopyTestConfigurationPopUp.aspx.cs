// File Name     : CopyTestConfigurationPopUp.aspx.cs
// Modified By   :  
// Modified Date : 11/09/2009
// Description   : This class is used to copy exam configuration.

using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using System.Collections.Generic;
using System.Reflection;
using BusinessLogic;
using Utility;
using System.Text;

public partial class CopyTestConfigurationPopUp : SchoolBase
{
    #region Constants

    const Int32 I_STANDARD_ID_COLUMN_NUMBER = 1;
    const Int32 I_STANDARD_DIVISION_ID_COLUMN_NUMBER = 2;
    const Int32 I_STANDARD_NAME_COLUMN_NUMBER = 3;
    const int I_SUBJECTTEST_TABLE_INDEX = 2;
    const Int32 I_START_COUNT = 4;

    const string S_QUERY_STRING_STANDARD_DIVISION_ID = "StandardDivisionId";
    const string S_QUERY_STRING_SUBJECT_ID = "SubjectId";

    private const string S_COLUMN_SUBJECT_ID = "Subject_Id";
    private const string S_COLUMN_SUBJECT_NAME = "Subject_Name";

    #endregion

    #region Data Members

    private int miSubjectId;
    int miStandardDivisionId;
    CheckBox oChk;

    #endregion

    #region Event Handlers

    /// <summary>
    /// This event is used to read querystring and to fill classwise subject exam configuration in grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
              
                if (ReadQuerystring())
                {
                    GetStandardDivisionSubjectName();
                    btnCopy.Attributes.Add("onclick", "if(!saveChk(this)){return false;}");
                }
                else
                    VisibleOrHideControls();                
                ApplyMouseHoverEffect(new List<Button> { btnBack, btnCopy });

                GetTestConfiguration();    
            }
            bool bIsUseSubmitBehavior = CommonUtility.CheckCancelOrBackClickEvent(this.Page);
            if (bIsUseSubmitBehavior == true)
                FillStandardwiseDivisionsGrid();
            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
   
    /// <summary>
    /// This event is used to move back to previous page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)this.Master; 
            oMasterPage.RedirectToNextPage(Constants.S_PAGE_SUBJECT_TEST_CONFIGURATION_DISPLAY);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is fired when user clicks on save button.
    /// This saves the desired configuration.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCopy_Click(object sender, EventArgs e)
    {
        try
        { 
            FillSubjectTestconfiguration();
            MasterPage oMasterPage = (MasterPage)this.Master; 
            oMasterPage.RedirectToNextPage(Constants.S_PAGE_SUBJECT_TEST_CONFIGURATION_DISPLAY);
        }

        catch (System.Data.SqlClient.SqlException ex)
        {
            lblError.Text = ex.Message;
        }
        catch (BusinessLogic.Exceptions.ReferenceExceptions ex)
        {
            lblError.Text = ex.Message;
        }

        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

   

    #endregion

    #region Grid Events

    /// <summary>
    /// This event is used to set standard name in textbox and applies CSS class for column header.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdDivisions_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            e.Row.Cells[0].CssClass = "locked";
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].CssClass = "Llocked";
                e.Row.Cells[0].Style.Add("left", grdDivisions.Style["scrollLeft"]);
                e.Row.Cells[0].Wrap = false;
            }

            if (e.Row.RowIndex >= 0)
            {
                // Set the standard name in the textbox.
                string sName = Convert.ToString(e.Row.Cells[I_STANDARD_NAME_COLUMN_NUMBER].Text);
                CheckBox chkCheckAll = ((CheckBox)e.Row.Cells[1].FindControl("CheckAllForRow"));
                chkCheckAll.Text = sName;
                int iRowNo = e.Row.RowIndex;
                chkCheckAll.Attributes.Add("onclick", "CheckUncheckAllInRow(this," + iRowNo + ")");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region private methods

    /// <summary>
    /// This method is used to decrypt querystring.
    /// </summary>
    /// <returns></returns>
    private bool ReadQuerystring()
    {
        if (Request.QueryString.ToString() != Constants.S_EMPTY_STRING)
        {
            Session["SUBJECT_ID"] = QueryString[S_QUERY_STRING_SUBJECT_ID];
            Session["STANDARD_DIVISION_ID"] = QueryString[S_QUERY_STRING_STANDARD_DIVISION_ID];
            Session["STANDARD_ID"] = QueryString["StandardId"];
            Session["IS_GRADE"] = QueryString["IsGrade"];
            return true;
        }

        return false;
    }

    /// <summary>
    /// This method is used to populate SubjectMasterBL for the configuration and returns the same.
    /// </summary>
    /// <param name="aiStandardDivisionId"></param>
    /// <param name="aiSubjectId"></param>
    /// <returns></returns>
    private SubjectMasterBL SetSubjectMasterBL(int aiStandardDivisionId, int aiSubjectId)
    {
        // This method creates the default object for the configuration and returns the same.
        SubjectMasterBL oSubjectMasterBL = new SubjectMasterBL();

        oSubjectMasterBL.StandardDivisionId = aiStandardDivisionId;
        oSubjectMasterBL.SubjectId = aiSubjectId;
        oSubjectMasterBL.AcademicYearId = miAcademicYearId;
        oSubjectMasterBL.SchoolId = miSchoolId;
        oSubjectMasterBL.UpdatedById = miUserId;

        return oSubjectMasterBL;
    }

   /// <summary>
    /// This method is used to get all subjects for school and puts them in newly generated columns.
   /// </summary>
    private void GenerateSubjectColumnsOfGrid()
    {
        const int I_SUBJECT_TABLE_INDEX = 1;
        DataSet oDs = (DataSet)grdDivisions.DataSource;
        DataTable oDtSubjects = oDs.Tables[I_SUBJECT_TABLE_INDEX];
        DataTable oDtTests = oDs.Tables[I_SUBJECTTEST_TABLE_INDEX];

        int iSubjectCount = oDtSubjects.Rows.Count;//no of columns to be generated
        int iStandardDivisionCount = grdDivisions.Rows.Count;//no of rows 
        int iCellIndex = 0;
        int iHeaderCellNo = 0;
        // header row
        int iSubjectIndex;
        //generate other columns
        for (int iRowIndex = 0; iRowIndex < iStandardDivisionCount; iRowIndex++)
        {
            int iStandardId = Convert.ToInt32(grdDivisions.Rows[iRowIndex].Cells[I_STANDARD_ID_COLUMN_NUMBER].Text);

            for (iSubjectIndex = 0; iSubjectIndex < iSubjectCount; iSubjectIndex++)
            {
                if (iRowIndex == 0)
                {
                    DataControlFieldHeaderCell oTHeader = new DataControlFieldHeaderCell(null);
                    oTHeader.Style.Add(HtmlTextWriterStyle.PaddingLeft, "2");
                    oTHeader.Style.Add(HtmlTextWriterStyle.PaddingRight, "2");
                    oTHeader.Text = oDtSubjects.Rows[iSubjectIndex][S_COLUMN_SUBJECT_ID].ToString();
                    oTHeader.HorizontalAlign = HorizontalAlign.Center;
                    oTHeader.Wrap = false;
                    iHeaderCellNo = grdDivisions.HeaderRow.Cells.Add(oTHeader);

                    if (iHeaderCellNo > 0)
                    {
                        CheckBox oChkHeader = new CheckBox();
                        oChkHeader.Text = oDtSubjects.Rows[iSubjectIndex][S_COLUMN_SUBJECT_NAME].ToString();
                        grdDivisions.HeaderRow.Cells[iHeaderCellNo].Controls.Add(oChkHeader);
                        iHeaderCellNo = (iHeaderCellNo - 4) * 2;
                        oChkHeader.Attributes.Add("onclick", "CheckAll(this, " + iHeaderCellNo + ")");
                    }
                }
                TableCell oT = new TableCell();
                oChk = new CheckBox();
                HiddenField ohidId = new HiddenField();
                ohidId.Value = "0";
                oT.Text = oDtSubjects.Rows[iSubjectIndex][S_COLUMN_SUBJECT_ID].ToString();
                oT.Attributes.Add("title", oDs.Tables[0].Rows[iRowIndex]["StandardDivision"].ToString() + " [" + oDtSubjects.Rows[iSubjectIndex][S_COLUMN_SUBJECT_NAME].ToString() + "]");
                iCellIndex = grdDivisions.Rows[iRowIndex].Cells.Add(oT);
                miSubjectId = Convert.ToInt32(grdDivisions.Rows[iRowIndex].Cells[iCellIndex].Text);
                miStandardDivisionId = Convert.ToInt32(grdDivisions.Rows[iRowIndex].Cells[I_STANDARD_DIVISION_ID_COLUMN_NUMBER].Text);
                DataRow[] oDrClassSubjects = oDtTests.Select("Standard_Division_Id = " + miStandardDivisionId.ToString() + " AND subject_id=" + miSubjectId.ToString());

                if (oDrClassSubjects.Length > 0)
                {
                    if (!oDrClassSubjects[0]["Testwise_Subject_Marks_Id"].ToString().Equals("0"))
                    {
                        oChk.Visible = true;
                        grdDivisions.Rows[iRowIndex].Cells[iCellIndex].CssClass = "ClsNotAssignDark";
                        HideSubjectCellFromWhichCopyTestConfiguration(iRowIndex, iCellIndex);
                        ohidId.Value = oDrClassSubjects[0]["Testwise_Subject_Marks_Id"].ToString();
                    }
                }
                else
                    oChk.Visible = false;
                if (Session["IS_GRADE"].ToString().Equals(Constants.C_YES.ToString()))
                {
                    if (iStandardId != Convert.ToInt32(Session["STANDARD_ID"]))
                        oChk.Visible = false;
                }
                oT.HorizontalAlign = HorizontalAlign.Center;
                grdDivisions.Rows[iRowIndex].Cells[iCellIndex].Controls.Add(oChk);
                grdDivisions.Rows[iRowIndex].Cells[iCellIndex].Controls.Add(ohidId);
            }
        }
    }

   /// <summary>
   /// This method is used to hide checkbox when division not assosiated with standard.
   /// </summary>
   /// <param name="aiRowIndex"></param>
   /// <param name="aiCellIndex"></param>
    private void HideSubjectCellFromWhichCopyTestConfiguration(int aiRowIndex, int aiCellIndex)
    {
        string sStandardDivisionId = Session["STANDARD_DIVISION_ID"].ToString();
        int iSubjectId = Convert.ToInt32(Session["SUBJECT_ID"]);

        if (grdDivisions.Rows[aiRowIndex].Cells[I_STANDARD_DIVISION_ID_COLUMN_NUMBER].Text == sStandardDivisionId
            && miSubjectId == iSubjectId)
        {
            oChk.Visible = false;
            grdDivisions.Rows[aiRowIndex].Cells[aiCellIndex].CssClass = "ClsHilightBG";
        }
    }
    
    /// <summary>
    /// This method is used to fill grid with standard-division with their respective subjects.
    /// </summary>
    private void FillStandardwiseDivisionsGrid()
    {
        VisibleOrHideColumnsofDivisionsGrid(true);
        SubjectTestConfigurationCollectionBL obj = new SubjectTestConfigurationCollectionBL(miSchoolId,miAcademicYearId);
        DataSet oDs = obj.GetClassSubjectTestsAssociation(0,0);
        grdDivisions.DataSource = oDs;
        grdDivisions.DataBind();
        VisibleOrHideColumnsofDivisionsGrid(false);
        GenerateSubjectColumnsOfGrid();
    }

    /// <summary>
    /// This method is used show/hide grid columns. 
    /// </summary>
    /// <param name="abAction"></param>
    private void VisibleOrHideColumnsofDivisionsGrid(bool abAction)
    {
        // This method hides the Groupid column from Gridview grdDivisions.
        grdDivisions.Columns[I_STANDARD_DIVISION_ID_COLUMN_NUMBER].Visible = abAction;
        grdDivisions.Columns[I_STANDARD_ID_COLUMN_NUMBER].Visible = abAction;
        grdDivisions.Columns[I_STANDARD_NAME_COLUMN_NUMBER].Visible = abAction;
    }

    /// <summary>
    /// This method is used show/hide copy configuration grid and other controls.
    /// </summary>
    private void VisibleOrHideControls()
    {
        chkAll.Visible = false;
        btnCopy.Visible = false;
        grdDivisions.Visible = false;
        tblLegend.Visible = false;
        tdCheckAll.Visible = false;
    }

    /// <summary>
    /// This method is used to set standard-division and subject name to label.
    /// </summary>
    private void GetStandardDivisionSubjectName()
    {
        lblStandardDivision.Text = QueryString["StdDiv"];
        lblSubject.Text = QueryString["SubjectName"].Replace("~", "&");
    }



    /// <summary>
    /// This method is used to copy exam configuration. 
    /// </summary>
    private void FillSubjectTestconfiguration()
    {       
        SubjectTestConfigurationBL oSubjectTestConfigurationBL = new SubjectTestConfigurationBL();
        string sTestConfigration =
                CommonUtility.GetXMLStringFromGridRows(grdDivisions, "TestConfiguarations", "TestConfiguaration"
                              , I_START_COUNT, I_STANDARD_DIVISION_ID_COLUMN_NUMBER);
        int iStandardDivisionId = Convert.ToInt32(Session["STANDARD_DIVISION_ID"]);
        int iSubjectId = Convert.ToInt32(Session["SUBJECT_ID"]);
       
        Hashtable oHash = GetModifiedRecords();
        string sIds = GetSelectedStandardDivList();
        oSubjectTestConfigurationBL.CopyTestConfiguration(iStandardDivisionId, iSubjectId, sTestConfigration,miUserId,miAcademicYearId, oHash,sIds);
    }

    /// <summary>
    /// This method gets id and name of the records which are to be modified.
    /// i.e. the recs already have some configuration
    /// </summary>
    /// <returns></returns>
    private Hashtable GetModifiedRecords()
    {
        DataSet oDs = (DataSet)grdDivisions.DataSource;
        DataTable oDtTests = oDs.Tables[I_SUBJECTTEST_TABLE_INDEX];

        Hashtable oHashReturn = new Hashtable();
        int iRowCnt = grdDivisions.Rows.Count;
        if (iRowCnt > 0)
        {
            int iColsCnt = grdDivisions.Rows[0].Cells.Count;
            for (int iRowIndex = 0; iRowIndex < iRowCnt; iRowIndex++)
            {
                for (int iColIndex = I_START_COUNT; iColIndex < iColsCnt; iColIndex++)
                {
                    CheckBox oChk = (CheckBox)grdDivisions.Rows[iRowIndex].Cells[iColIndex].Controls[0];
                    HiddenField oHidId = (HiddenField)grdDivisions.Rows[iRowIndex].Cells[iColIndex].Controls[1];

                    if (!oHidId.Value.Equals("0") && ((oChk.Visible) && oChk.Checked == true))
                    {
                        int iSubjectId = Convert.ToInt32(grdDivisions.Rows[iRowIndex].Cells[iColIndex].Text);
                        int iStandardDivisionId = Convert.ToInt32(grdDivisions.Rows[iRowIndex].Cells[I_STANDARD_DIVISION_ID_COLUMN_NUMBER].Text);
                        string sClass = grdDivisions.Rows[iRowIndex].Cells[I_STANDARD_NAME_COLUMN_NUMBER].Text;
                        CheckBox oChkHeader = (CheckBox)grdDivisions.HeaderRow.Cells[iColIndex].Controls[0];
                        string sSubject = oChkHeader.Text;

                        string sClassSubject = sClass + " : " + sSubject;
                        oHashReturn.Add(oHidId.Value, sClassSubject);
                        DataRow[] oDr = oDtTests.Select("Standard_Division_Id = " + iStandardDivisionId.ToString() + " AND subject_id=" + iSubjectId.ToString() + " AND Testwise_Subject_Marks_Id<>0");
                        for (int i = 1; i < oDr.Length; i++)
                        {
                            oHashReturn.Add(oDr[i]["Testwise_Subject_Marks_Id"].ToString(), sClassSubject);
                        }
                    }
                }
            }
        }
        return oHashReturn;
    }
	
	private void GetTestConfiguration()    
    {
        SubjectTestConfigurationCollectionBL oSubjectTestConfigurationCollectionBL = new SubjectTestConfigurationCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtTestConfiguration = oSubjectTestConfigurationCollectionBL.RetriveAllTestConfiguration(Session["STANDARD_DIVISION_ID"].ToInt(), Session["SUBJECT_ID"].ToInt());
        lstStdDiv.DataSource = oDtTestConfiguration;
        lstStdDiv.DataBind();
        
	}
    
    private string GetSelectedStandardDivList()
    {
        string sCommaSeperatedIds = "";
        foreach (ListViewDataItem oCurrentItem in lstStdDiv.Items)
        {
            int iRowId = oCurrentItem.DisplayIndex.ToInt();
            CheckBox chkSelect = oCurrentItem.FindControl("ChkSelectAll") as CheckBox;
            if (chkSelect.Checked == true )
            {
                sCommaSeperatedIds = sCommaSeperatedIds + lstStdDiv.DataKeys[iRowId]["TestWise_Subject_Marks_Id"].ToString() + "," ;
                 
            }
        }
        sCommaSeperatedIds = sCommaSeperatedIds.TrimEnd(',');
        return sCommaSeperatedIds;

    }

    #endregion
}


