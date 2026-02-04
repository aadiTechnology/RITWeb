// File Name     : TestNamesList.aspx.cs
// Modified By   : Amit 
// Modified Date : 23/09/2009
// Description   : This class is used configure exam names for current acadamic year.

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

public partial class TestNamesList : SchoolBase
{

    #region " Constants "

    const string S_DATAKEY_SCHOOLWISE_TEST_NAME = "SchoolWise_Test_Name";
    const string S_DATAKEY_SCHOOLWISE_TEST_ID = "SchoolWise_Test_Id";
    const string S_DATAKEY_ORG_SCHOOLWISE_TEST_ID = "Original_SchoolWise_Test_Id";
    const string S_DATAKEY_TERM_ID = "Term_Id";
    const string S_DATAKEY_SCHOOL_ID = "School_Id";
    const string S_DATAKEY_ISFINALEXAM = "IsFinalExam";
    //const string S_SELECT_AT_LEAST_ONE_GROUP = "At least one exam name should be selected for saving.";
    
    #endregion " Constants "

    #region " Event "
    
    /// <summary>
    /// This event is used to fill exam name grid and set default properties to page controls.
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
                
                DesignSettingAccordingLanguage();
                SetDefaultProperties();
                FillExamNameGrid();
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                DesignSettingAccordingLanguage();
            }
        
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill configured exam names and active check in grid. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdGroupDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= 0)
            {
                // Set the exam name in the textbox.
                int iCurrentRowIndex = e.Row.RowIndex;
                string sName = grdTestNames.DataKeys[e.Row.RowIndex][S_DATAKEY_SCHOOLWISE_TEST_NAME].ToString();
                CheckBox oChkBoxDelete = ((CheckBox)e.Row.Cells[1].FindControl("ChkBoxDelete"));
                TextBox txttest_nameName = ((TextBox)e.Row.Cells[1].FindControl("txttest_nameName"));
                RadioButton oOptfinalexam = ((RadioButton)e.Row.Cells[1].FindControl("optfinalexam"));
                DropDownList oCmbTerm = ((DropDownList)e.Row.Cells[1].FindControl("cmbTerm"));
                txttest_nameName.Text = sName;
                ((DropDownList)e.Row.Cells[2].FindControl("cmbTerm")).SelectedValue = grdTestNames.DataKeys[e.Row.RowIndex]["Term_Id"].ToString();
                
                // If the school id is not the default id i.e. -9999 
                // that means the test_name is already assigned
                // to the school. Thus check the checkbox.
                if (grdTestNames.DataKeys[e.Row.RowIndex][S_DATAKEY_SCHOOL_ID].ToString() != Constants.S_DEFAUL_SCHOOL_ID)
                    ((CheckBox)e.Row.FindControl("ChkBoxDelete")).Checked = true;


                oChkBoxDelete.Attributes.Add("onclick", "EnableDisableEntirRow(this," + (iCurrentRowIndex + 2)  + ");");
                oOptfinalexam.Attributes.Add("onclick", "SelectFinalExam(this," + iCurrentRowIndex + ");");
                oCmbTerm.Attributes.Add("onchange", "Verify(this," + iCurrentRowIndex + ");");

                if (grdTestNames.DataKeys[e.Row.RowIndex][S_DATAKEY_ISFINALEXAM].ToBool())
                {
                    ((RadioButton)e.Row.FindControl("optfinalexam")).Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save exam name configuration for school.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        // Save the changes to database.
        try
        {
            int iIsFinalExam = 0;
            CheckBox Deleteflag = new CheckBox();
            CheckBox Isfinalexam = new CheckBox();
            Collection<SchoolWiseTestMasterBL> oTests = new Collection<SchoolWiseTestMasterBL>();
            int iRowCount = grdTestNames.Rows.Count;
            for (int i = 0; i < iRowCount; i++)
            {
                Deleteflag = (CheckBox)grdTestNames.Rows[i].FindControl("ChkBoxDelete");
                TextBox txtPrefixEdit = ((TextBox)grdTestNames.Rows[i].FindControl("txttest_nameName"));
                DropDownList cmbTerm = ((DropDownList)grdTestNames.Rows[i].FindControl("cmbTerm"));
                Isfinalexam = (RadioButton)grdTestNames.Rows[i].FindControl("optfinalexam");

                if (Isfinalexam.Checked == true)
                    iIsFinalExam = 1;
                else
                    iIsFinalExam = 0;
                

                // Check if new test_name is being inserted.
                // I.e. If the checkbox is checked and the school id is -9999
                // then it is the new test_name being introduced.
                if (Deleteflag.Checked == true && grdTestNames.DataKeys[i][S_DATAKEY_SCHOOL_ID].ToString() == Constants.S_DEFAUL_SCHOOL_ID)
                {
                    SchoolWiseTestMasterBL oSchoolWiseTestMasterBL = GetCommonSchoolWiseTestMasterBL(txtPrefixEdit.Text,
                    Convert.ToInt32(grdTestNames.DataKeys[i][S_DATAKEY_ORG_SCHOOLWISE_TEST_ID].ToString()), Convert.ToInt32(cmbTerm.SelectedValue), iIsFinalExam);
                    oSchoolWiseTestMasterBL.ConfigurationAction = Constants.Action.Insert;
                    oTests.Add(oSchoolWiseTestMasterBL);
                }

                // Check if existing test_name name is being updated.
                // I.e. If the checkbox is checked and the school is not -9999 and the value in text box differs 
                // from the value in the test_name name column then update the existing test_name name.
                else if (Deleteflag.Checked == true &&
                        grdTestNames.DataKeys[i][S_DATAKEY_SCHOOL_ID].ToString() != Constants.S_DEFAUL_SCHOOL_ID &&
                        (grdTestNames.DataKeys[i][S_DATAKEY_SCHOOLWISE_TEST_NAME].ToString() != txtPrefixEdit.Text.Trim()
                        || cmbTerm.SelectedValue != grdTestNames.DataKeys[i][S_DATAKEY_TERM_ID].ToString() || iIsFinalExam != grdTestNames.DataKeys[i][S_DATAKEY_ISFINALEXAM].ToInt()))
                {
                    SchoolWiseTestMasterBL oSchoolWiseTestMasterBL = GetCommonSchoolWiseTestMasterBL(txtPrefixEdit.Text,
                                                                             Convert.ToInt32(grdTestNames.DataKeys[i][S_DATAKEY_ORG_SCHOOLWISE_TEST_ID].ToString()), Convert.ToInt32(cmbTerm.SelectedValue), iIsFinalExam);
                    oSchoolWiseTestMasterBL.ConfigurationAction = Constants.Action.Update;
                    oSchoolWiseTestMasterBL.SchoolWiseTestId = Convert.ToInt32(grdTestNames.DataKeys[i][S_DATAKEY_SCHOOLWISE_TEST_ID].ToString());
                    oTests.Add(oSchoolWiseTestMasterBL);
                }

                // Check if existing test_name is being removed.
                // I.e. If the checkbox is NOT checked and the school id is not -9999. 
                // In such case need to check if any of the related data is entered for the unchecked test_name then
                // the warning message should be given to user and the related data should be removed from db.
                else if (Deleteflag.Checked == false && grdTestNames.DataKeys[i][S_DATAKEY_SCHOOL_ID].ToString() != Constants.S_DEFAUL_SCHOOL_ID)
                {
                    SchoolWiseTestMasterBL oSchoolWiseTestMasterBL = GetCommonSchoolWiseTestMasterBL(txtPrefixEdit.Text,
                                                                             Convert.ToInt32(grdTestNames.DataKeys[i][S_DATAKEY_ORG_SCHOOLWISE_TEST_ID].ToString()), Convert.ToInt32(cmbTerm.SelectedValue), iIsFinalExam);
                    oSchoolWiseTestMasterBL.ConfigurationAction = Constants.Action.Delete;
                    oSchoolWiseTestMasterBL.SchoolWiseTestId = Convert.ToInt32(grdTestNames.DataKeys[i][S_DATAKEY_SCHOOLWISE_TEST_ID].ToString());
                    oTests.Add(oSchoolWiseTestMasterBL);
                }
            }

            // If there are test_names to be deleted then give warning message to user about the same.
            // Update database with the configured test_names.
            if (oTests.Count > 0)
            {
                TestCollectionBL oTestCollectionBL = new TestCollectionBL(miSchoolId);
                oTestCollectionBL.UpdateTests(oTests, miAcademicYearId);
            }

            if (!ReadQuerystring())
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.TestNames));
           
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Exam_Related)));
        }
        catch (BusinessLogic.Exceptions.ReferenceExceptions ex)
        {

            lblError.Text = CommonUtility.ModifyExceptionMessage(ex.Message, " ", " ", "can not be removed since associated with", Resources.LocalizedResources.CanNotRemovedSinceAssociatedWithStandardExams);
            FillExamNameGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }     
    }
    /// <summary>
    /// This event is used to capture selected index change of user role combo box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbUserRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillExamNameGrid();
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
    protected void btnCancel_Click(object sender, EventArgs e)
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
 
    #region " Private Methods "

    /// <summary>
    /// This method is used to set default properties and java script for page controls.
    /// </summary>
    private void SetDefaultProperties()
    {
        MasterPage oMasterPage = (MasterPage)this.Page.Master;
        oMasterPage.SetParentNodeURL(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Exam_Related)));        
        ApplyMouseHoverEffect(new List<Button> { btnCancel, btnSave });
        btnCancel.Attributes["onclick"] = "javascript:DisableButtons()";
        btnSave.Attributes.Add("onclick", "if(!SelectedCount(0)){return false;}");
    }

    /// <summary>
    /// This method is used to populate SchoolWiseTestMasterBL object 
    /// which is used to save exam name configuration.
    /// </summary>
    /// <param name="asTestName"></param>
    /// <param name="aiTestId"></param>
    /// <returns></returns>
    private SchoolWiseTestMasterBL GetCommonSchoolWiseTestMasterBL(string asTestName, int aiTestId, int aiTermId, int aiIsFinalExam)
    {
        // This method creates the default object for the configuration and returns the same.
        SchoolWiseTestMasterBL oSchoolWiseTestMasterBL = new SchoolWiseTestMasterBL();
        oSchoolWiseTestMasterBL.SchoolId = miSchoolId;
        oSchoolWiseTestMasterBL.UpdatedById = miUserId;
        oSchoolWiseTestMasterBL.InsertedByid = miUserId;
        oSchoolWiseTestMasterBL.SchoolWiseTestName = asTestName;
        oSchoolWiseTestMasterBL.AcademicYearId = miAcademicYearId;
        oSchoolWiseTestMasterBL.OriginalSchoolWiseTestId = aiTestId;
        oSchoolWiseTestMasterBL.TermId = aiTermId;
        oSchoolWiseTestMasterBL.IsFinalExam = aiIsFinalExam;
        return oSchoolWiseTestMasterBL;
    }

    /// <summary>
    /// This method is used to decrypt encrypted querystring.
    /// </summary>
    private bool ReadQuerystring()
    {
        try
        {
            return QueryString[Constants.S_IS_CONFIGURED] != null && QueryString[Constants.S_IS_CONFIGURED] == Constants.S_YES;
        }
        catch (Exception)
        {
            MasterPage oMasterPage = (MasterPage)this.Master; 
            oMasterPage.RedirectToNextPage(Constants.S_PAGE_ERROR);
        }
        
		return false;
    }

    /// <summary>
    /// This method is used to fill exam name grid.
    /// </summary>
    private void FillExamNameGrid()
    {
        TestCollectionBL oTestCollectionBL = new TestCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtUserDetails = oTestCollectionBL.GetAllTests();
        grdTestNames.DataSource = oDtUserDetails.DefaultView;
        grdTestNames.DataBind();
        hidgrdTestNamesRowCount.Value = grdTestNames.Rows.Count.ToString();
        btnSave.Attributes.Add("Onclick", "if(!(ConfirmAction('" + grdTestNames.AllowPaging + "','" + Resources.LocalizedResources.AtLeastOneExamNameShouldBeSelected + "'))){return false;}");
    }
    /// <summary>
    /// This method is used to set design according to selected language.
    /// </summary>
    private void DesignSettingAccordingLanguage()
    {
        btnSave.Attributes.Add("Onclick", "if(!(ConfirmAction('" + grdTestNames.AllowPaging + "','" + Resources.LocalizedResources.AtLeastOneExamNameShouldBeSelected + "'))){return false;}");
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
    }
    #endregion " Private Methods "
}
