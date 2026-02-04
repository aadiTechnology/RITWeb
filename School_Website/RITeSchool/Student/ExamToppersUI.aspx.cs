using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using System.Collections.Generic;

/// <summary>
/// This class is used to display first three subject and class/standard toppers.
/// </summary>
public partial class ExamToppersUI : SchoolBase
{
    #region Constants
    
    private const int I_CLASS_COLUMN_INDEX = 1;
    private const string S_ANNUAL_RESULT_TYPE = "-1";
    private const string S_ANNUAL_RESULT = "Final Result";

    #endregion

    #region Members
    int miSelectedAcademicYearId = 0;
    int miTestId;
    int miStdId;
    int miToppersType;
	bool mIsOldyrToppers;
    DataSet modsToppers;
    
    #endregion

    #region Events

    /// <summary>
    /// This method is used for following purposes :-
    ///     1) To fill standard and class, test combobox.
    ///     2) To display toppers.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
          
			GettQuerystring();
			Initialize();
            cmbStandard.Focus();
            if (!IsPostBack)
            {
                
                SetStandardDivisionId();
                if (IsAdmin())
                {
                    FillStdComboBox();
                    FillStdDivComboBox();
                    if (miToppersType == 1)
                    {
                        cmbStandard.Visible = false;
                        FillClassTestCombobox();
                    }
                    else
                        FillTestCombobox();
                    SetStdDivComboDefaultValues();
                    LoadToppersData();
                    lblToppers.Text = rbtnClassToppers.Checked ? rbtnClassToppers.Text : rbtnStdToppers.Text;
                }
                else
                    Response.Write("<Script language='Javascript'>window.close();window.opener.focus(); </Script>");
            }
            else
            {
                if (miSelectedAcademicYearId.ToString() == "0")
                {
                    if (cmbTests.Items.Count>0)
                        miTestId = Convert.ToInt32(cmbTests.SelectedValue);
                    if (cmbStandard.Items.Count>0)
                        miStdId = Convert.ToInt32(cmbStandard.SelectedValue);
                    if (cmbStandardDivision.Items.Count>0)
                        hidStdDivId.Value = cmbStandardDivision.SelectedValue;
                }
                else
                {
                    miTestId = Convert.ToInt32(cmbTests.SelectedValue);
                    miStdId = Convert.ToInt32(hidStandardId.Value);
                    hidStdDivId.Value = hidStdDivId.Value;
                }
            }
			ApplyMouseHoverEffect(new List<Button>() { img_Back });
        }
        catch (BusinessLogic.Exceptions.ResultNotAvailableForOtherDiv ex)
        {
            tblGrid.Visible = true;
            tblMsg.Visible = true;
            trSubTitle.Visible = false;
            lblErrorsMsg.Text = ex.Message;
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
        finally
        {
            img_Back.Attributes.Add("onclick", "return closewindow();");
        }
    }
      

    /// <summary>
    /// This method is used to bind standered to DropDownList combo.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            miStdId = Convert.ToInt32(cmbStandard.SelectedValue);
            hidStandardId.Value = miStdId.ToString();
            FillTestCombobox();
            LoadToppersData();
        }
        catch (BusinessLogic.Exceptions.ResultNotAvailableForOtherDiv ex)
        {
            tblGrid.Visible = true;
            tblMsg.Visible = true;
            trSubTitle.Visible = false;
            lblErrorsMsg.Text = ex.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display first three subject and class toppers.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rbtnClassToppers_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblToppers.Text = rbtnClassToppers.Text;

			if (moUserRole == Constants.UserRoles.Admin || (Boolean.Parse(hidUserHasFullAccess.Value)))
            {
                cmbStandard.SelectedValue = hidStandardId.Value;
                if (rbtnClassToppers.Checked)
                {
                    cmbStandard.Visible = false;
                    cmbStandardDivision.Visible = true;
                    FillClassTestCombobox();
                    
                }
                else
                {
                    cmbStandard.Visible = true;
                    cmbStandardDivision.Visible = false;
                    FillTestCombobox();
                    
                }
            }
            LoadToppersData();
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display first three subject and standard toppers.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rbtnStdToppers_CheckedChanged(object sender, EventArgs e)
    {
        try
        
        {
            lblToppers.Text = rbtnStdToppers.Text;

			if (moUserRole == Constants.UserRoles.Admin|| (Boolean.Parse(hidUserHasFullAccess.Value)))
            {
                int iStandardDivisionId = Convert.ToInt32(hidStdDivId.Value);
                int iSchoolId = Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]);
                miStdId = SchoolWiseStanderedDivisionTestMasterBL.GetStandardId(iSchoolId, miAcademicYearId, iStandardDivisionId);
                cmbStandard.SelectedValue = miStdId.ToString(); ;
                hidStandardId.Value = miStdId.ToString();
                if (rbtnClassToppers.Checked)
                    FillClassTestCombobox();
                else
                    FillTestCombobox();
            }
            LoadToppersData();
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to show toppers for the selected test
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbTests_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            miTestId = Convert.ToInt32(cmbTests.SelectedValue);
            LoadToppersData();
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to bind standered division to DropDownList combo.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStandardDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hidStdDivId.Value  = cmbStandardDivision.SelectedValue;            
            FillClassTestCombobox();
            LoadToppersData();
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region  Helping Methods

    /// <summary>
    /// This method is used to set default exam.
    /// </summary>
    private void SetDefaultExam(bool abIsClasswise)
    {
        int iStdDivId = Convert.ToInt32(hidStdDivId.Value);
        int iSchoolId = Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]);
        ListItem oListItem = new ListItem("- " + S_ANNUAL_RESULT + "  -", S_ANNUAL_RESULT_TYPE);
        if (cmbTests.Items.Contains(oListItem))
            cmbTests.SelectedIndex = cmbTests.Items.Count - 1;
        else
        {
            if(abIsClasswise)
                cmbTests.SelectedValue = SchoolWiseTestMasterBL.GetLatestExamId(iSchoolId, miAcademicYearId, iStdDivId,0).ToString();
            else
            {
                int iStandardId = Convert.ToInt32(hidStandardId.Value);
                int iLatestExamId = SchoolWiseTestMasterBL.GetLatestExamId(iSchoolId, miAcademicYearId, 0, iStandardId);
                if (iLatestExamId != 0)
                    cmbTests.SelectedValue = iLatestExamId.ToString();
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void SetStdDivComboDefaultValues()
    {
        cmbStandard.SelectedValue = miStdId.ToString();
        cmbStandardDivision.SelectedValue = hidStdDivId.Value;
        cmbTests.SelectedValue = miTestId.ToString();
    }

    /// <summary>
    /// This method is used to initialize member variables.
    /// </summary>
    private void Initialize()
    {
		InitializeMemberVariables();
        if (Session[Constants.S_SESSION_SELECTED_ACADEMIC_YEAR_ID] != null)
            miSelectedAcademicYearId = Convert.ToInt32(Session[Constants.S_SESSION_SELECTED_ACADEMIC_YEAR_ID]);
		if (miSelectedAcademicYearId != 0 && mIsOldyrToppers)
            miAcademicYearId = miSelectedAcademicYearId;
    }

    /// <summary>
    /// Check the cmb selections and show approprite toppers.
    /// </summary>
    private void LoadToppersData()
    {
        if ((cmbTests.SelectedValue != S_ANNUAL_RESULT_TYPE) && (rbtnClassToppers.Checked))
        {
            EnableDisableControls(true);
            FillClassWiseToppersGrid();
            IsAdmin();
        }
        else if (cmbTests.SelectedValue != S_ANNUAL_RESULT_TYPE)
        {
            EnableDisableControls(false);
            FillStdWiseTopperGrid();
            IsAdmin();
        }
        else if ((cmbTests.SelectedValue == S_ANNUAL_RESULT_TYPE) && (rbtnClassToppers.Checked))
        {
            EnableDisableControls(true);
            FillFinalResultToppersGrid();
            IsAdmin();
        }
        else if (cmbTests.SelectedValue == S_ANNUAL_RESULT_TYPE)
        {
            EnableDisableControls(false);
            FillTestToppersGrid();
            IsAdmin();
        }
    }

    /// <summary>
    /// This method is used to set cmbos for the exam mode
    /// </summary>
    private void EnableDisableControls(Boolean bIsExam)
    {
        grdTestToppers.Columns[I_CLASS_COLUMN_INDEX].Visible = !bIsExam;
        grdSubjectTopper.Columns[I_CLASS_COLUMN_INDEX].Visible = !bIsExam;
        tdcmbStandard.Visible = !bIsExam;
        cmbStandard.Visible = !bIsExam;
        tdlblStdDiv.Visible = !bIsExam;
        tdcmbClass.Visible = bIsExam;
        tdClass.Visible = bIsExam;
    }

    /// <summary>
    /// This method is used to decrypt querystring passed to this page.
    /// </summary>
    private void GettQuerystring()
    {

		if (moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher)
                 hidUserHasFullAccess.Value = CommonUtility.IsUserHasScreenAccess(Constants.SchoolConfigurations.ExamResults).ToString();

	    if (QueryString.Count <= 0)
		    return;
	    
		if (QueryString["StdId"] != null)
		    miStdId = QueryString["StdId"].ToInt();
	    if (QueryString["StdDivId"] != null)
		    hidStdDivId.Value = QueryString["StdDivId"];
	    if (QueryString["TestId"] != null)
		    miTestId = QueryString["TestId"].ToInt();
	    if (QueryString["ToppersType"] != null)
		    miToppersType = QueryString["ToppersType"].ToInt();
		if (QueryString["IsOldYear"] != null)
			mIsOldyrToppers = QueryString["IsOldYear"].ToBool();
    }

    /// <summary>
    /// This method is used to set standarddivision id.
    /// </summary>
    private void SetStandardDivisionId()
    {   
        if (Session[Constants.S_SESSION_STUDENT_ID] != null)
        {
            int iStudentId = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_ID]);
            hidStdDivId.Value = SchoolWiseStanderedDivisionTestMasterBL.GetStandardDivisionIdOfYear(miSchoolId, miAcademicYearId, iStudentId).ToString();
        }
    }
    
    /// <summary>
    /// This method fills the combobox for the tests.
    /// </summary>
    private void FillTestCombobox()
    {
        int iStudentId = 0;
        int iStandardId = 0;
        if (Session[Constants.S_SESSION_STUDENT_ID] == null)
            iStandardId = miStdId;
        else
        {
            iStudentId = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_ID]);
            iStandardId = SchoolWiseStandardSubjectMasterBL.GetStandardOfYear(miSchoolId, miAcademicYearId, iStudentId);
        }
        hidStandardId.Value = iStandardId.ToString();
        TestCollectionBL oTestCollectionBL = new TestCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtAllTests = oTestCollectionBL.GetAllpublishedTestsForStandard(iStandardId, iStudentId);
        ControlUtility.FillDropDownList(oDtAllTests, ref cmbTests,
                                       Constants.S_TEST_ID_FIELD,
                                       Constants.S_TEST_NAME_FIELD,
                                       "");

        if (moUserRole == Constants.UserRoles.Student ? CheckIsResultPublished(true) : IsAtleastOneResultGeneratedForStdDiv(Constants.I_ZERO, iStandardId))
            cmbTests.Items.Add(new ListItem("- " + S_ANNUAL_RESULT + "  -", S_ANNUAL_RESULT_TYPE));
        if (cmbTests.Items.Count > 0)
            SetDefaultExam(false);
    }

    /// <summary>
    /// This method fills the combobox for the tests.
    /// </summary>
    private void FillClassTestCombobox()
    {
        int iStandardId = Convert.ToInt32(hidStandardId.Value);
        int iStdDivId = 0;
        if (cmbStandard.Visible == true)
        {
            cmbStandardDivision.SelectedValue = SchoolWiseStanderedDivisionTestMasterBL.GetStandardDivisionId(miSchoolId, miAcademicYearId, iStandardId).ToString();
            hidStdDivId.Value = cmbStandardDivision.SelectedValue;
        }
        iStdDivId = Convert.ToInt32(cmbStandardDivision.SelectedValue);
        TestCollectionBL oTestCollectionBL = new TestCollectionBL(miSchoolId, miAcademicYearId, iStdDivId);
        DataTable oDtAllTests = oTestCollectionBL.GetAllTestsForWhichToppersGenerated();
        ControlUtility.FillDropDownList(oDtAllTests, ref cmbTests,
                                       Constants.S_TEST_ID_FIELD,
                                       Constants.S_TEST_NAME_FIELD,
                                       "");
        if (IsAtleastOneResultGeneratedForStdDiv(iStdDivId, Constants.I_ZERO))
            cmbTests.Items.Add(new ListItem("- " + S_ANNUAL_RESULT + "  -", S_ANNUAL_RESULT_TYPE));
        if (cmbTests.Items.Count > 0)
            SetDefaultExam(true);
    }

    /// <summary>
    /// This for Check one result is Generated for Standed division
    /// </summary>
    /// <param name="aiStdDivId"></param>
    /// <returns></returns>
    private bool IsAtleastOneResultGeneratedForStdDiv(int aiStdDivId, int aiStandardId)
    {
        SchoolWiseStanderedDivisionTestMasterBL oSWStdDivTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL();
        oSWStdDivTestMasterBL.School_id = miSchoolId;
        oSWStdDivTestMasterBL.Acadmic_year_id = miAcademicYearId;
        oSWStdDivTestMasterBL.Standerd_division_Id = aiStdDivId;
        oSWStdDivTestMasterBL.StanderdId = aiStandardId;
        DataTable oDtResultDetails = oSWStdDivTestMasterBL.IsAtleastOneResultGeneratedForStdDiv();
        if (oDtResultDetails.IsNonEmpty())
            return oDtResultDetails.Rows[0]["AllowPublish"].ToBool();
        return false;
    }

    /// <summary>
    /// This method is used ot insert standered division combo box.
    /// </summary>
    private void FillStdComboBox()
    {
        StandardDivisionCollectionBL oStandardDivisionCollectionBL = new StandardDivisionCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDataTable = oStandardDivisionCollectionBL.GetAssociatedStandardsGorTest();
        ControlUtility.FillDropDownList(oDataTable, ref cmbStandard,
                                       "Standard_Id",
                                       "Standard_Name",
                                       "");
        if (cmbStandard.Items.Count == 0)
            miStdId = 0;
        else if (miStdId == 0)
            miStdId = Convert.ToInt32(cmbStandard.SelectedValue);
    }

    /// <summary>
    /// This method is used ot insert standered division combo box.
    /// </summary>
    private void FillStdDivComboBox()
    {
        StandardDivisionCollectionBL oStandardDivisionCollectionBL = new StandardDivisionCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDataTable = oStandardDivisionCollectionBL.GetAssociatedStandardsDivisionsGorTest();
        ControlUtility.FillDropDownList(oDataTable, ref cmbStandardDivision,
                                       "SchoolWise_Standard_Division_id",
                                       "StandardDivision",
                                       "");
        if (cmbStandardDivision.Items.Count == 0)
            hidStdDivId.Value = "0";
        else
            cmbStandardDivision.SelectedValue = hidStdDivId.Value;
    }

    /// <summary>
    /// This method is used to fill test toppers grid.
    /// </summary>
    private void FillTestToppersGrid()
    {
        try
        {
            SchoolWiseStanderedDivisionTestMasterBL oSWStdDivTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL(miSchoolId, miAcademicYearId, hidStdDivId.Value.ToInt());
            oSWStdDivTestMasterBL.School_id = miSchoolId;
            oSWStdDivTestMasterBL.Acadmic_year_id = miAcademicYearId;
            DataTable oDtResultDetails = oSWStdDivTestMasterBL.IsAtleastOneResultGeneratedForStdDiv();
            if (oDtResultDetails.IsNonEmpty())
            {
                grdTestToppers.Visible = false;
            }
           
            modsToppers = StudentBL.GetAnnualStanderedResult(miSchoolId, miAcademicYearId, miStdId, 3);
           
            grdTestToppers.DataSource = modsToppers.Tables[0].DefaultView;
            grdTestToppers.DataBind();
            grdTestToppers.Visible = true;
            lblTestName.Text = S_ANNUAL_RESULT;
            if (modsToppers.Tables.Count < 2)
                trSubTitle.Visible = false;
            else
                trSubTitle.Visible = true;
            CreateSubjectToppersGrids();
        }
        catch (BusinessLogic.Exceptions.ResultNotAvailableForOtherDiv ex)
        {
            tblGrid.Visible = true;
            tblMsg.Visible = true;
            trSubTitle.Visible = false;
            lblErrorsMsg.Text = ex.Message;
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to fill test toppers grid.
    /// </summary>
    private void FillStdWiseTopperGrid()
    {
        try
        {
            SchoolWiseStanderedDivisionTestMasterBL oSchoolWiseStanderedDivisionTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL();
            oSchoolWiseStanderedDivisionTestMasterBL.School_id = miSchoolId;
            oSchoolWiseStanderedDivisionTestMasterBL.Acadmic_year_id = miAcademicYearId;
            oSchoolWiseStanderedDivisionTestMasterBL.SchoolWise_Test_Id = Convert.ToInt32(cmbTests.SelectedValue);
            if (moUserRole == Constants.UserRoles.Student && !oSchoolWiseStanderedDivisionTestMasterBL.isTestPublishedForAllDivs(miStdId))
                grdTestToppers.Visible = false;
            modsToppers = oSchoolWiseStanderedDivisionTestMasterBL.GetTestAndSubjectStdToppers(miStdId, 3);
            if (modsToppers.Tables.Count > 0)
            {
                grdTestToppers.DataSource = modsToppers.Tables[0].DefaultView;
                grdTestToppers.DataBind();
                if (modsToppers.Tables[0].Rows.Count > 0)
                    lblTestName.Text = cmbTests.SelectedItem.Text;
                else
                {
                    lblTestName.Text = "";
                    tblMsg.Visible = true;
                    lblErrorsMsg.Text = "No Records Found.";
                }
            }
            else
            {
                tblMsg.Visible = true;
                lblErrorsMsg.Text = "No Records Found.";
            }
            if (modsToppers.Tables.Count < 2)
                trSubTitle.Visible = false;
            else
                trSubTitle.Visible = true;
            CreateSubjectToppersGrids();
        }
        catch (BusinessLogic.Exceptions.ResultNotAvailableForOtherDiv ex)
        {
            tblGrid.Visible = true;
            tblMsg.Visible = true;
            trSubTitle.Visible = false;
            lblErrorsMsg.Text = ex.Message;
        }
        catch (Exception ex)
        {
            tblGrid.Visible = true;
            tblMsg.Visible = true;
            trSubTitle.Visible = false;
            lblErrorsMsg.Text = ex.Message;
            ExceptionHandler.WriteExceptionToErrorLog(ex,System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to fill Annual test toppers for the class grid.
    /// </summary>
    private void FillFinalResultToppersGrid()
    {

        if (hidStdDivId.Value == "0")
            hidStdDivId.Value = cmbStandardDivision.SelectedValue;
        modsToppers = StudentBL.GetAnnualResult(miSchoolId, miAcademicYearId, Convert.ToInt32(hidStdDivId.Value), 3);
        grdTestToppers.DataSource = modsToppers.Tables[0].DefaultView;
        grdTestToppers.DataBind();
        lblTestName.Text = S_ANNUAL_RESULT;
        if (modsToppers.Tables.Count < 2)
            trSubTitle.Visible = false;
        else
            trSubTitle.Visible = true;
        CreateSubjectToppersGrids();
    }

    /// <summary>
    /// This method is used to fill test toppers grid.
    /// </summary>
    private void FillClassWiseToppersGrid()
    {
        try
        {
            int iStdDivId = Convert.ToInt32(hidStdDivId.Value);
            SchoolWiseStanderedDivisionTestMasterBL oSchoolWiseStanderedDivisionTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL();
            oSchoolWiseStanderedDivisionTestMasterBL.School_id = miSchoolId;
            oSchoolWiseStanderedDivisionTestMasterBL.Acadmic_year_id = miAcademicYearId;
            if (cmbTests.Items.Count>0)
                oSchoolWiseStanderedDivisionTestMasterBL.SchoolWise_Test_Id = Convert.ToInt32(cmbTests.SelectedValue);

            if (moUserRole == Constants.UserRoles.Student && !oSchoolWiseStanderedDivisionTestMasterBL.isTestPublishedForDivs(miStdId, iStdDivId))
                grdTestToppers.Visible = false;
            //oSchoolWiseStanderedDivisionTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL(miSchoolId, miAcademicYearId, iStdDivId, Convert.ToInt32(cmbTests.SelectedValue));

            oSchoolWiseStanderedDivisionTestMasterBL.Standerd_division_Id = iStdDivId;
            modsToppers = oSchoolWiseStanderedDivisionTestMasterBL.GetTestAndSubjectToppers(3);
            if (modsToppers.Tables.Count > 0)
            {
                grdTestToppers.DataSource = modsToppers.Tables[0].DefaultView;
                grdTestToppers.DataBind();
                if (modsToppers.Tables[0].Rows.Count > 0)
                    lblTestName.Text = cmbTests.SelectedItem.Text;
                else
                {
                    lblTestName.Text = "";
                    tblMsg.Visible = true;
                    lblErrorsMsg.Text = "No Records Found.";
                }
            }
            else
            {
                tblMsg.Visible = true;
                lblErrorsMsg.Text = "No Records Found.";
            }
            if (modsToppers.Tables.Count < 2)
                trSubTitle.Visible = false;
            else
                trSubTitle.Visible = true;
            CreateSubjectToppersGrids();
        }
        catch (BusinessLogic.Exceptions.ResultNotAvailableForOtherDiv ex)
        {
            tblGrid.Visible = true;
            tblMsg.Visible = true;
            trSubTitle.Visible = false;
            lblErrorsMsg.Text = ex.Message;
        }
        catch (Exception ex)
        {
            tblGrid.Visible = true;
            tblMsg.Visible = true;
            trSubTitle.Visible = false;
            lblErrorsMsg.Text = ex.Message;
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to create new subject toppers grid.
    /// </summary>
    private void CreateSubjectToppersGrids()
    {
        int i = 1;
        int rowCount = 0;
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        tblGrid.Rows.Add(oHtmlTableRow);
        oHtmlTableRow = new HtmlTableRow();
        tblGrid.Rows.Add(oHtmlTableRow);
        oHtmlTableRow = new HtmlTableRow();
        tblGrid.Rows.Add(oHtmlTableRow);
        //mosToppers is grid of standardwise final result,after this for loop grids are generatated dynamatically for eacd subject toppers
        for (; i < modsToppers.Tables.Count; i++)
        {
            DataTable oTable = modsToppers.Tables[i];
            CreateSubjectGrid(oTable, i, rowCount);
            //keep only 3 grids on each row and create a new row for next 3.
            if (i % 2 == 0)
            {
                AddBlankRowCell(rowCount + 2);
                oHtmlTableRow = new HtmlTableRow();
                tblGrid.Rows.Add(oHtmlTableRow);
                oHtmlTableRow = new HtmlTableRow();
                tblGrid.Rows.Add(oHtmlTableRow);
                oHtmlTableRow = new HtmlTableRow();
                tblGrid.Rows.Add(oHtmlTableRow);
                rowCount += 3;
            }
            else
            {
                HtmlTableCell oHtmlTableCell = new HtmlTableCell();
                oHtmlTableCell.Align = "center";                
                oHtmlTableCell.VAlign = "top";
                oHtmlTableCell.Width = "20px";                
                tblGrid.Rows[rowCount].Cells.Add(oHtmlTableCell);

                oHtmlTableCell = new HtmlTableCell();
                oHtmlTableCell.Align = "center";
                oHtmlTableCell.VAlign = "top";
                oHtmlTableCell.Width = "20px";
                tblGrid.Rows[rowCount + 1].Cells.Add(oHtmlTableCell);
            }
        }
    }

    /// <summary>
    /// This method is used to create new grid scheema.
    /// </summary>
    private void CreateSubjectGrid(DataTable oDataTable, int aiGridCount, int iRowCount)
    {
        GridView oGridView = new GridView();
        oGridView.Visible = true;
        oGridView.Width = grdSubjectTopper.Width;
        oGridView.AutoGenerateColumns = grdSubjectTopper.AutoGenerateColumns;
        oGridView.PageSize = grdSubjectTopper.PageSize;
        oGridView.AllowPaging = grdSubjectTopper.AllowPaging;
        oGridView.CellPadding = grdSubjectTopper.CellPadding;
        oGridView.CellSpacing = grdSubjectTopper.CellSpacing;
        oGridView.ForeColor = grdSubjectTopper.ForeColor;
        oGridView.GridLines = grdSubjectTopper.GridLines;
        oGridView.RowStyle.CssClass = grdSubjectTopper.RowStyle.CssClass;
        oGridView.HeaderStyle.CssClass = grdSubjectTopper.HeaderStyle.CssClass;
        oGridView.AlternatingRowStyle.CssClass = grdSubjectTopper.AlternatingRowStyle.CssClass;
        oGridView.EmptyDataRowStyle.CssClass = grdSubjectTopper.EmptyDataRowStyle.CssClass;
        foreach (DataControlField oDataControlField in grdSubjectTopper.Columns.CloneFields())
        {
            oGridView.Columns.Add(oDataControlField);
        }
        oGridView.DataSource = oDataTable;
        oGridView.DataBind();
        Label oLabel = new Label();
        oLabel.CssClass = lblSubjectName.CssClass;
        if ((oDataTable.Rows.Count > 0) && (oDataTable.Rows[0]["Subject_Name"] != DBNull.Value))
            oLabel.Text = oDataTable.Rows[0]["Subject_Name"].ToString();

        HtmlTableCell oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.Align = "center";
        oHtmlTableCell.VAlign = "top";
        oHtmlTableCell.Attributes.Add("class", "ToprBGlghtTxtBlk");
        oHtmlTableCell.Controls.Add(oLabel);
        tblGrid.Rows[iRowCount].Cells.Add(oHtmlTableCell);
        oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.Align = "center";
        oHtmlTableCell.VAlign = "top";
        oHtmlTableCell.Controls.Add(oGridView);
        tblGrid.Rows[iRowCount + 1].Cells.Add(oHtmlTableCell);
    }

    /// <summary>
    /// This method is used to add blank row.
    /// </summary>
    /// <param name="aiRowCount"></param>
    private void AddBlankRowCell(int aiRowCount)
    {
        HtmlTableCell oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.EnableViewState = false;
        oHtmlTableCell.Align = "center";
        oHtmlTableCell.VAlign = "top";
        oHtmlTableCell.ColSpan = 4;
        oHtmlTableCell.InnerHtml = "&nbsp;";
        oHtmlTableCell.Width = "100%";
        tblGrid.Rows[aiRowCount].Cells.Add(oHtmlTableCell);
    }

    /// <summary>
    /// This method is used to set controls resp to roles 
    /// </summary>
    private Boolean IsAdmin()
    {
        bool bResult = false;
		if (moUserRole == Constants.UserRoles.Admin || (Boolean.Parse(hidUserHasFullAccess.Value)))
            bResult = true;
		else if (moUserRole == Constants.UserRoles.Teacher || moUserRole == Constants.UserRoles.Student)
        {
            VisibleHideCombo(false);
            bResult = true;
        }
        return bResult;
    }

    /// <summary>
    /// This method is used to make visible or hide test combo for diff logins.
    /// </summary>
    /// <param name="abAction"></param>
    private void VisibleHideCombo(bool abAction)
    {
        tdlblStdDiv.Visible = abAction;
        tdClass.Visible = abAction;
        tdcmbClass.Visible = abAction;
        tdcmbStandard.Visible = abAction;
    }
    
    /// <summary>
    /// This method is used to check annual result is published or not.
    /// </summary>
    /// <param name="bIsStandard"></param>
    /// <returns></returns>
    private Boolean CheckIsResultPublished(bool bIsStandard)
    {
        if (bIsStandard)
            return SchoolWiseAnnualResultPublishBL.IsResultPublished(miSchoolId, miAcademicYearId, miStdId, 0);
        else
            return SchoolWiseAnnualResultPublishBL.IsResultPublished(miSchoolId, miAcademicYearId, 0, Convert.ToInt32(hidStdDivId.Value));
    }

    #endregion
}