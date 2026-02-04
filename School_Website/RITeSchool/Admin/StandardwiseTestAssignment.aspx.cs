// File Name  : StandardwiseTestAssignment.aspx.cs
// Created By : Anugandha
// Date       : 31/01/2008
//Description :This Form is used to assign different tests 
//             to particular standard of a particular school.

using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Reflection;
using BusinessLogic.Exceptions;
using BusinessLogic;
using Utility;

/// <summary>
/// This class is used to assign different tests to a particular standard of a particular school.
/// </summary>
public partial class StandardwiseTestAssignment : SchoolBase
{
    #region Constants

   // const string S_SELECT_AT_LEAST_ONE_TEST_FOR_STANDARD = "Atleast one exam should be assigned for each standard.";
    //const string S_SELECT_AT_LEAST_ONE_STANDARD_FOR_TESTS = "Atleast one standard should be assigned for each exam.";

    const Int32 I_STANDARD_ID_DATAKEY_NAME = 0;
    const Int32 I_STANDARD_NAME_DATAKEY_NAME = 2;
    const Int32 I_START_COUNT = 1;
    const int I_STDTEST_TABLE_INDEX = 2;

    private const string S_COLUMN_TEST_ID = "Schoolwise_Test_Id";
    private const string S_COLUMN_TEST_NAME = "Schoolwise_Test_Name";

    #endregion

    #region " Date Member "
    private string IsConfig;
    #endregion

    #region Events
    
    /// <summary>
    ///This method is used to fill the grid of standards,generating columns of tests
    /// as per grid and set validations on "save" click.
    /// </summary>
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
                DesignSettingAccordingLanguage();
                hlnkSortOrder.Attributes.Add("onclick", "window.open('TestsSortOrderPopUp.Aspx?" + Server.UrlDecode(Request.QueryString.ToString())
                                                          + "', '_blank','scrollbars=yes,resizable=no,top=0,left=0,width=650,height=530');return false;");
                btnCancel.Attributes["onclick"] = "javascript:DisableButtons()";
                btnCancel.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Exam_Related));                            
            }
            btnSave.Attributes.Add("onclick", "if(!saveChk('" + Resources.LocalizedResources.AtleastOneExamShouldBeAssignedForEachStandard + "' , '"
                                       + Resources.LocalizedResources.AtleastOneStandardShouldBeAssignedForEachExam + "',this)){return false}");
            ApplyMouseHoverEffect(new List<Button> { btnCancel, btnSave });
            grdStandards.Columns[0].HeaderText = "";

            if (Session[Constants.S_SESSION_LANGUAGE] != null)
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                DesignSettingAccordingLanguage();
            }
            FillGridWithStandardsAndtests();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to save all checked values of standards and tests to the database.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            int iStandardId;
            CheckBox ochkTests;
            int iColumnIndex = Convert.ToInt32(hidColumnCount.Value);
            Collection<StandardMasterBL> oStandardCollection = new Collection<StandardMasterBL>();
            DataSet oDsGrdSrc = (DataSet)grdStandards.DataSource;
            DataTable oDSTestId = oDsGrdSrc.Tables[I_STDTEST_TABLE_INDEX];
            Collection<SchoolwiseStandardTestMasterBL> oAllTestCollection = new Collection<SchoolwiseStandardTestMasterBL>();
            for (int iRowCount = 0; iRowCount < grdStandards.Rows.Count; iRowCount++)
            {
                StandardMasterBL oStandardMasterBL = new StandardMasterBL();

                Collection<SchoolwiseStandardTestMasterBL> oTestCollection = new Collection<SchoolwiseStandardTestMasterBL>();
                SchoolwiseStandardTestMasterCollectionBL oSchoolwiseStandardTestMasterCollectionBL = new SchoolwiseStandardTestMasterCollectionBL(miSchoolId, miAcademicYearId);
                iStandardId = Convert.ToInt32(grdStandards.DataKeys[iRowCount][I_STANDARD_ID_DATAKEY_NAME].ToString());

                for (int iColumnCount = I_START_COUNT; iColumnCount <= iColumnIndex; iColumnCount++)
                {
                    ochkTests = (CheckBox)(grdStandards.Rows[iRowCount].Cells[iColumnCount].Controls[0]);
                    int iTestId = Convert.ToInt32(grdStandards.Rows[iRowCount].Cells[iColumnCount].Text);
                    string sTestName = "Standard - " + grdStandards.DataKeys[iRowCount][I_STANDARD_NAME_DATAKEY_NAME].ToString() + " : " +
                                        grdStandards.HeaderRow.Cells[iColumnCount].Text;
                    DataRow[] oDrs = oDSTestId.Select("Standard_Id=" + iStandardId.ToString() + " AND " + S_COLUMN_TEST_ID + "=" + iTestId.ToString());
                    if (ochkTests.Checked == true && (oDrs.Length == 0))
                    {
                        SchoolwiseStandardTestMasterBL oSchoolwiseStandardTestMasterBL = SetSchoolwiseStandardTestMasterBL(iStandardId, iTestId, sTestName,0);
                        oSchoolwiseStandardTestMasterBL.ConfigurationAction = Constants.Action.Insert;
                        oTestCollection.Add(oSchoolwiseStandardTestMasterBL);

                    }
                    else if (ochkTests.Checked == false && (oDrs.Length > 0))
                    {
                        SchoolwiseStandardTestMasterBL oSchoolwiseStandardTestMasterBL = SetSchoolwiseStandardTestMasterBL(iStandardId, iTestId, sTestName, Convert.ToInt32(oDrs[0]["Schoolwise_Standard_Test_Id"]));
                        oSchoolwiseStandardTestMasterBL.ConfigurationAction = Constants.Action.Delete;
                        oTestCollection.Add(oSchoolwiseStandardTestMasterBL);
                        oAllTestCollection.Add(oSchoolwiseStandardTestMasterBL);
                    }
                }

                if (oTestCollection.Count > 0)
                {
                    
                    oStandardMasterBL.StandardName = (grdStandards.DataKeys[iRowCount][I_STANDARD_NAME_DATAKEY_NAME].ToString());
                    oStandardMasterBL.TestCollection = oTestCollection;
                    oStandardCollection.Add(oStandardMasterBL);
                }
            }

            if (oStandardCollection.Count > 0)
            {
                CheckDependencies(oAllTestCollection);
                StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId);
                oStandardCollectionBL.UpdateStandardTests(oStandardCollection);
            }

            ReadQuerystring();
            if (IsConfig != "Y")
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.StandardwiseTests));
            
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Exam_Related)));
        }
        catch (BusinessLogic.Exceptions.ReferenceExceptions ex)
        {
            lblError.Text = CommonUtility.ModifyExceptionMessage(ex.Message, " ", " ", "can not be removed since associated with", Resources.LocalizedResources.CanNotRemovedSinceAssociatedWithExamSchedule);
            FillGridWithStandardsAndtests();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Grid Events

    /// <summary>
    /// This method is used to set the standard name in the first column and also
    /// set attributes property to each row of the grid.
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
            if (e.Row.RowIndex >= 0)
            {
                string sName = grdStandards.DataKeys[e.Row.RowIndex][I_STANDARD_NAME_DATAKEY_NAME].ToString();
                CheckBox chkCheckAll = ((CheckBox)e.Row.Cells[0].FindControl("CheckAllForRow"));
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

    /// <summary>
    /// This event is for Allow Paging property of the grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStandards_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdStandards.PageIndex = e.NewPageIndex;
            FillGridWithStandardsAndtests();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used for implementing paging style.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStandards_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                Table PagerTable = (Table)e.Row.Cells[0].Controls[0];
                PagerTable.CssClass = "ClsNwGridPaging";
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    #endregion

    #region Private Methods
    
    private void CheckDependencies(Collection<SchoolwiseStandardTestMasterBL> aoStdTestCollection)
    {
        if (aoStdTestCollection.Count > 0)
        {
            GenericReferenceList<SchoolwiseStandardTestMasterBL> objStdTestRefereces = new GenericReferenceList<SchoolwiseStandardTestMasterBL>(aoStdTestCollection, miAcademicYearId);
            objStdTestRefereces.CheckDependenciesAndThrowException("Schoolwise_Standard_Test_Id", "Standard_Test_Name", Constants.ReferenceId.StandardExams);
        }
    }
    /// <summary>
    /// This method is used to decrypt encrypted querystring.
    /// </summary>
    private void ReadQuerystring()
    {
        try
        {
            IsConfig = QueryString["Is_Configured"];
        }
        catch (Exception)
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
			oMasterPage.RedirectToNextPage(Constants.S_PAGE_ERROR);
        }
    }

    /// <summary>
    /// This function checks the preconditons of Configured Subjects for Subject Group criteria.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.StandardwiseTests);

        if (sLinks.Equals(""))
        {
            divErr.Visible = false;
            bReturn = true;
        }
        else
        {
            divErr.InnerHtml = sLinks;
            VisibleOrHideControls();
        }
        return bReturn;
    }

    /// <summary>
    /// This method is used to visible or hide controls depends 
    /// on configuration is done or not.
    /// </summary>
    private void VisibleOrHideControls()
    {

        chkAll.Visible = false;
        btnSave.Visible = false;
        btnCancel.Text = "Back";
        divToprLinkHlilight.Visible = false;
        GridViewScrollContainer.Visible = false;
    }

    /// <summary>
    /// This method is used to fill grid with standard names.
    /// </summary>
    private void FillStandardsGrid()
    {
        SchoolwiseStandardTestMasterCollectionBL obj = new SchoolwiseStandardTestMasterCollectionBL(miSchoolId, miAcademicYearId);
        DataSet oDs = obj.GetStdExamAssociation();
        grdStandards.DataSource = oDs;
        grdStandards.DataBind();
    }

    /// <summary>
    /// This method is used to fill grid with standards and the generate columns of tests
    /// dynamically to the grid after checking all required configurations.
    /// </summary>
    private void FillGridWithStandardsAndtests()
    {
        if (CheckPreCondition())
        {
            FillStandardsGrid();
            GenerateTestColumnsOfGrid();
        }
    }

    /// <summary>
    /// This method is used to generate columns of tests of grid dynamically
    /// which is attached to grid one by one and show checkbox is checked true when the test
    /// is already assigned to that standard.
    /// </summary>
    private void GenerateTestColumnsOfGrid()
    {
        int iCellIndex = 0;
        const int I_TEST_TABLE_INDEX = 1;

        DataSet oDs = (DataSet)grdStandards.DataSource;
        DataTable oDtTests = oDs.Tables[I_TEST_TABLE_INDEX];
        DataTable oDtStdTests = oDs.Tables[I_STDTEST_TABLE_INDEX];
        int iTestCount = oDtTests.Rows.Count;
        for (int iCount = 0; iCount < iTestCount; iCount++)
        {
            TableCell oTableCell = new TableCell();
            oTableCell.Text = oDtTests.Rows[iCount][S_COLUMN_TEST_NAME].ToString();
            oTableCell.HorizontalAlign = HorizontalAlign.Center;
            grdStandards.HeaderRow.Cells.Add(oTableCell);

            CheckBox ocheckb = new CheckBox();
            ocheckb.Text = oDtTests.Rows[iCount][S_COLUMN_TEST_NAME].ToString();

            int headerCellNo = iCount;
            ocheckb.Attributes.Add("onclick", "CheckAll(this, " + headerCellNo + ")");
            oTableCell.Wrap = false;            
            oTableCell.Controls.Add(ocheckb);
            int iRowCount = grdStandards.Rows.Count;
            for (int iRowIndex = 0; iRowIndex < iRowCount; iRowIndex++)
            {
                int iStandardId = Convert.ToInt32(grdStandards.DataKeys[iRowIndex][I_STANDARD_ID_DATAKEY_NAME].ToString());
                oTableCell = new TableCell();
                CheckBox oChk = new CheckBox();
                oTableCell.Text = oDtTests.Rows[iCount][S_COLUMN_TEST_ID].ToString();
                oTableCell.Attributes.Add("title", "Std. " + oDs.Tables[0].Rows[iRowIndex]["Standard_Name"].ToString() + " [" + oDtTests.Rows[iCount][S_COLUMN_TEST_NAME].ToString() + "]");
                iCellIndex = grdStandards.Rows[iRowIndex].Cells.Add(oTableCell);
                int iTestId = Convert.ToInt32(oTableCell.Text);
                DataRow[] oDr = oDtStdTests.Select("Standard_Id=" + iStandardId.ToString() + " AND " + S_COLUMN_TEST_ID + "=" + iTestId.ToString());
                if (oDr.Length > 0)
                {
                    oChk.Checked = true;
                }
                oTableCell.HorizontalAlign = HorizontalAlign.Center;
                grdStandards.Rows[iRowIndex].Cells[iCellIndex].Controls.Add(oChk);
            }
        }
        hidColumnCount.Value = iCellIndex.ToString();
    }

    /// <summary>
    /// This method is used to set the properties of SchoolwiseStandardTestMasterBL class.
    /// is already assigned to that standard.
    /// </summary>
    private SchoolwiseStandardTestMasterBL SetSchoolwiseStandardTestMasterBL(int aiStandardId, int aiTestId, string asTestName, int aiStdTestId)
    {
        SchoolwiseStandardTestMasterBL oSchoolwiseStandardTestMasterBL = new SchoolwiseStandardTestMasterBL();
        oSchoolwiseStandardTestMasterBL.Standard_Id = aiStandardId;
        oSchoolwiseStandardTestMasterBL.SchoolWise_Test_Id = aiTestId;
        oSchoolwiseStandardTestMasterBL.School_Id = miSchoolId;
        oSchoolwiseStandardTestMasterBL.academic_Year_Id = miAcademicYearId;
        oSchoolwiseStandardTestMasterBL.Standard_Test_Name = asTestName;
        oSchoolwiseStandardTestMasterBL.Inserted_By_id = Convert.ToString(miUserId);
        oSchoolwiseStandardTestMasterBL.Updated_By_Id = Convert.ToString(miUserId);
        oSchoolwiseStandardTestMasterBL.Schoolwise_Standard_Test_Id = aiStdTestId;
        return oSchoolwiseStandardTestMasterBL;

    }
    /// <summary>
    /// This method is used to set design according to the language selected.
    /// </summary>
    private void DesignSettingAccordingLanguage()
    {
        hidPleaseFixFollowingErrors.Value = Resources.LocalizedResources.PleaseFixFollowingError;
    }
    #endregion
}
