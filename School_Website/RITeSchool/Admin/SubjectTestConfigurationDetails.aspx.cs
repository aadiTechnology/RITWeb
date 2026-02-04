// File Name     :- SubjectTestConfigurationDetails.aspx.cs
// Modified By   :- Amit
// Modified Date :- 22-09-2009
// Description   :- This class is used to set classwise subject test configuration.

// Modified By   :- Vipul
// Modified Date :- 5-Dec.-2011
// Description   :- To give facility of "Out OF Marks". It is used for exam result and total on screens and reports.

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using MasterEntities;
using Utility;
using StandardWiseExamConfigurationEntities;

/// <summary>
/// This page is opened from
/// This class 
/// 1. display the configuration for all tests of a subject(for specific standrd-division)
/// 2. Add subject test configuration (for each test seperately)  
/// 2. Edit subject test configuration (for each test seperately) 
/// 3. Delete subject test configuration (for each test seperately) 
/// </summary>
public partial class SubjectTestConfigurationDetails : SchoolBase
{
	#region -- CONSTANT(s) --

	// Datakeys
	private const int I_DATAKEY_TOTALMARKS = 1;
	private const int I_DATAKEY_PASSINGMARKS = 2;
	private const int I_DATAKEY_OUT_OF_MARKS = 3;
	private const int I_DATAKEY_RSLT = 3;
	
	// Column Indices
	private const int I_TEST_NAME_COLUMN_INDEX = 1;
	private const int I_TOTAL_MARKS_COLUMN_INDEX = 2;
	private const int I_PASSING_MARKS_COLUMN_INDEX = 3;
	private const int I_OUT_OF_MARKS_COLUMN_INDEX = 4;
	private const int I_DELETE_COLUMN_INDEX = 5;
	private const int I_DELETE_EXAM_MARKS_INDEX = 6;

	// Grades
	private const string S_SHORT_GRADES = "G";
	private const string S_SHORT_MARKS = "M";

	private const string S_DEFAULT = "0";
	private const int I_DEFAULT = 0;

	private const string S_ERROR_NO_TEST_CONFIGURED = "At least one exam should be configured.";
	private const string S_EXAM_DELETE_SUCCESSFUL_MSG = "Exam configuration deleted successfully!!!";
	private const string S_MARK_DELETE_SUCCESSFUL_MSG = "Exam marks are deleted successfully!!!";
	private const string S_EXAM_SAVED_SUCCESSFUL_MSG = "Exam configuration saved successfully!!!";
	private const string S_EXAM_UPDATED_SUCCESSFUL_MSG = "Exam configuration updated successfully!!!";
	private const string S_RESULT_REFACTOR_MSG = "Final result factor can not be modified since final result of the class has been published.";
	private const string S_CLASS_ERROR = "LblErrorMsg";

	private const string S_DELETE_ROW = "DeleteRow";
	private const string S_DELETE_EXAM_MARKS = "DELETE_EXAM_MARKS";

	private const string S_ADD = "Add";
	private const string S_UPDATE = "Update";

	private const string S_DEFAULT_FACTOR = "1.0";

    private const string S_ALLOW_DECIMAL = "AllowDecimal";
	private const string S_PASSING_GRADE = "Passing Grade";
	private const string S_TOTAL_PASSING_MARK = "Total Passing Marks";
	private const string S_ERROR_EXAM_NOT_CONFIGURED = "You have not configured exams for this standard.";

	#endregion -- CONSTANT(s) --

	#region -- MEMBER(s) --

	private int miStandardDivisionId;
	private int miSubjectId;
	private int miTestMarksId;
	private List<StandardMaster> mlstStandardsWithOnlyGrades;
	private List<SubjectMaster> mlstSecondLanguageSubjects;
	private bool mbIsDisplayGradeApplicable;

	#endregion -- MEMBER(s) --

	#region -- EVENT HANDLER(s) --

	/// <summary>
	/// This event is used to fill all page test marks controls.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ae"></param>
	protected void Page_Load(object sender, EventArgs ae)
	{
		try
		{            
			bool bIsUseSubmitBehavior = CommonUtility.CheckCancelOrBackClickEvent(Page);
			if (bIsUseSubmitBehavior)
			{
				cmbExams.Focus();
				ReadQueryString();
			}

			if (!IsPostBack)
            {
                if (CheckPrecondition())
                {
                    InitializeFields();
                    SetControlsDefaultValues();
                    FillTestTypeGrid(miTestMarksId);
                    FillSubjectTestGrid();
                    SetClientScriptAttributes();
                }
             			}

			mbIsDisplayGradeApplicable = !hidIsDisplayGradeApplicable.Value.IsNullOrEmpty() && hidIsDisplayGradeApplicable.Value.ToBool();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            SubjectTestConfigurationBL moSubjectTestConfigurationBL = new SubjectTestConfigurationBL();
            moSubjectTestConfigurationBL.DeleteAllExams(miStandardDivisionId, miSubjectId,miUserId,miAcademicYearId,miSchoolId);
            lblSuccessMsg.Text = "Configuration of all exams deleted successfully!!!";
            FillSubjectTestGrid();
          
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
       
	/// <summary>
	/// This event is used to delete subject from final result.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void grdSubjectTestConfiguration_rowCommand(object sender, GridViewCommandEventArgs e)
	{
		try
		{
			int iRowIndex;
			if (int.TryParse(e.CommandArgument.ToString(), out iRowIndex))
			{
				miTestMarksId = Convert.ToInt32(grdSubjectTestConfiguration.DataKeys[iRowIndex]["TestWise_Subject_Marks_Id"].ToString());
				SubjectTestConfigurationBL oSubjectTestConfigurationBL = SetSubjectTestConfigurationBL();
				switch (e.CommandName)
				{
					case S_DELETE_ROW:
						{
							oSubjectTestConfigurationBL.DeleteSubjectTestConfiguration(grdSubjectTestConfiguration.Rows[iRowIndex].Cells[I_DEFAULT].Text);
							lblSuccessMsg.Text = S_EXAM_DELETE_SUCCESSFUL_MSG;
							FillSubjectTestGrid();
							ResetFormFields();
							if (optGrade.Checked)
								cmbPassingGrade.SelectedIndex = Constants.I_ZERO;
							if (grdSubjectTestConfiguration.Rows.Count == Constants.I_ZERO)
								optGrade.Enabled = optMarks.Enabled = true;
							break;
						}

					case S_DELETE_EXAM_MARKS:
						{
							oSubjectTestConfigurationBL.DeleteTestExamMarkDetails(Convert.ToBoolean(hidDeleteStudentWiseSavedMarks.Value));
							lblSuccessMsg.Text = S_MARK_DELETE_SUCCESSFUL_MSG;
							FillSubjectTestGrid();
							break;
						}
				}
			}
		}
		catch (BusinessLogic.Exceptions.ReferenceExceptions ex)
		{
			FillSubjectTestGrid();
			lblError.CssClass = S_CLASS_ERROR;
			lblError.Text = ex.Message;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}
  

	/// <summary>
	/// This event is used to set grid column as per grade.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void grdSubjectTestConfiguration_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		try
		{
			int iRowIndex = e.Row.RowIndex;
			if (iRowIndex >= Constants.I_ZERO)
			{
				ImageButton oImgDelete = (ImageButton)e.Row.Cells[I_DELETE_COLUMN_INDEX].Controls[I_DEFAULT];
				oImgDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
				string sIsPublish = grdSubjectTestConfiguration.DataKeys[iRowIndex]["IsPublished"].ToString();
				string sIsSubmit = grdSubjectTestConfiguration.DataKeys[iRowIndex]["IsSubmitted"].ToString();
				string sIsExamMarkEntered = grdSubjectTestConfiguration.DataKeys[iRowIndex]["IsExamMarkEntered"].ToString();
				string sIsStudentWiseProgressReportPublished = grdSubjectTestConfiguration.DataKeys[iRowIndex]["IsStudentWiseProgressReportPublished"].ToString();
				string sExamName = e.Row.Cells[I_TEST_NAME_COLUMN_INDEX].Text;
				Button oBtnDelete = (Button)e.Row.Cells[I_DELETE_EXAM_MARKS_INDEX].Controls[I_DEFAULT];
				oBtnDelete.Attributes.Add("onclick", "if(!ConfirmDeleteExamMarksMessage('" + sIsPublish + "' , '" + sIsSubmit + "', '" + sExamName + "', '" + sIsStudentWiseProgressReportPublished + "'  )) {return false;}");
                ApplyMouseHoverEffect(new List<Button> { oBtnDelete });
				Image oImg = (Image)e.Row.FindControl("imgConsider");
				oImg.ImageUrl = grdSubjectTestConfiguration.DataKeys[iRowIndex][I_DATAKEY_RSLT].ToString().Trim().Equals("N") ? "~/RITeSchool/images/IconGrid_AssignFalse.gif" : "~/RITeSchool/images/IconGrid_AssignTrue.gif";

				grdSubjectTestConfiguration.HeaderRow.Cells[3].Text = optGrade.Checked ? S_PASSING_GRADE : S_TOTAL_PASSING_MARK;

				// Check whether exam mark are not entered
				oBtnDelete.Enabled = sIsExamMarkEntered == Constants.S_YES ? true : false;
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to sorting on subject test configuration grid.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void grdSubjectTestConfiguration_Sorting(object sender, GridViewSortEventArgs e)
	{
		try
		{
			hidSortExpression.Value = e.SortExpression;
			SetSortVariables();
			FillSubjectTestGrid();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to create a row to add default sort image. 
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void grdSubjectTestConfiguration_RowCreated(object sender, GridViewRowEventArgs e)
	{
		try
		{
			GridView sGridviewName = (GridView)sender;

			if (e.Row.RowType == DataControlRowType.Header)
			{
				// Call the GetSortColumnIndex helper method to determine
				// the index of the column being sorted.
				int iSortColumnIndex = CommonUtility.GetSortColumnIndex(sGridviewName, hidSortExpression.Value);
				if (iSortColumnIndex != -1)
				{
					// Call the AddSortImage helper method to add
					// a sort direction image to the appropriate column header. 
					CommonUtility.AddSortImage(iSortColumnIndex, e.Row, hidSortDirection.Value);
				}
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to set java script attributes to grid controls as per configure mode. 
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void grdTestTypes_rowDatabound(object sender, GridViewRowEventArgs e)
	{
		try
		{
			int iRowIndex = e.Row.RowIndex;
			if (iRowIndex >= Constants.I_ZERO)
			{
				CheckBox oChkTestType = (CheckBox)e.Row.Cells[1].FindControl("chkTestType");

                ((Label)e.Row.Cells[1].FindControl("lblTestTypeName")).Text = grdTestTypes.DataKeys[iRowIndex]["TestType_Name"].ToString();

				// total marks
				TextBox oTxtTotMarks = (TextBox)e.Row.Cells[1].FindControl("txtTotMarks");
				oTxtTotMarks.Text = grdTestTypes.DataKeys[iRowIndex][I_DATAKEY_TOTALMARKS].ToString();
				oTxtTotMarks.Attributes.Add("onChange", "SetTotals('txtTotMarks')");
				
				TextBox oTxtPassingMarks = (TextBox)e.Row.Cells[1].FindControl("txtPassingMarks");
                oTxtPassingMarks.Text = grdTestTypes.DataKeys[iRowIndex][S_ALLOW_DECIMAL].ToBool() ? grdTestTypes.DataKeys[iRowIndex][I_DATAKEY_PASSINGMARKS].ToString() : grdTestTypes.DataKeys[iRowIndex][I_DATAKEY_PASSINGMARKS].ToInt().ToString();
				oTxtPassingMarks.Attributes.Add("onChange", "SetTotals('txtPassingMarks')");

				TextBox oTxtOutOfMarks = (TextBox)e.Row.Cells[1].FindControl("txtOutOfMarks");
				oTxtOutOfMarks.Text = grdTestTypes.DataKeys[iRowIndex][I_DATAKEY_OUT_OF_MARKS].ToString();
				oTxtOutOfMarks.Attributes.Add("onChange", "SetOutOfMarks(this, 'TestTypeOutOfMarks')");

				oChkTestType.Attributes.Add("onclick", "EnableDisableGridTextBox(this," + iRowIndex + "); chkTestTypeOnClick(this);");
				if (Convert.ToInt32(grdTestTypes.DataKeys[iRowIndex][I_DATAKEY_TOTALMARKS]) > Constants.I_ZERO)
				{
					oChkTestType.Checked = true;
					oTxtTotMarks.Enabled = true;
					oTxtPassingMarks.Enabled = true;
					oTxtOutOfMarks.Enabled = true;
					if (!txtTestOutOfMarks.Enabled)
						txtTestOutOfMarks.Enabled = true;
				}
				else
				{
					oTxtOutOfMarks.Enabled = false;
					oChkTestType.Checked = false;
					oTxtTotMarks.Enabled = false;
					oTxtPassingMarks.Enabled = false;
				}
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}


	/// <summary>
	/// This event is used to fill exam type grid as per exam.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ae"></param>
	protected void cmbExams_SelectedIndexChanged(object sender, EventArgs ae)
	{
		try
		{
			txtTestOutOfMarks.Text = string.Empty;
			txtTestOutOfMarks.Enabled = false;

			GetTestMarksId();
			if (cmbExams.SelectedIndex == Constants.I_ZERO)
			{
				if (optMarks.Checked)
					ResetGrid();
				else
					cmbPassingGrade.SelectedIndex = Constants.I_ZERO;
				btnAdd.Text = S_ADD;

				chkExamStatus.Checked = true;
				chkExamStatus.Enabled = false;
                chkAllowDecimal.Enabled = false;
			}
			else
				SetFormFields();
			FillTestTypeGrid(miTestMarksId);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	#region -- CLICK EVENT(s) --

	/// <summary>
	/// This event is used to add/update exam type mark configuration for class subject. 
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ae"></param>
	protected void btnAdd_Click(object sender, EventArgs ae)
	{
		try
		{
			// check for duplicate
			int iRowCount = grdSubjectTestConfiguration.Rows.Count;
			ClearErrorLabel();
			GetTestMarksId();

			string sXmlString = GetXmlString();

			// set master object
			SubjectTestConfigurationBL oSubjectTestConfigurationBL = SetSubjectTestConfigurationBL();

			// if edit mode
			if (hidMode.Value.Equals(Constants.ViewMode.Edit.ToString()))
			{
				oSubjectTestConfigurationBL.Update(sXmlString, cmbExams.SelectedItem.Text);
				lblSuccessMsg.Text = S_EXAM_UPDATED_SUCCESSFUL_MSG;
			}

			// new mode
			else 
			{
				oSubjectTestConfigurationBL.AddSubjectTestConfiguration(sXmlString);
                if (!Convert.ToBoolean(QueryString["IsConfig"]))
                    SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.SubjectExamConfig));
				lblSuccessMsg.Text = S_EXAM_SAVED_SUCCESSFUL_MSG;
			}

			FillSubjectTestGrid();
			ResetFormFields();
			if (optGrade.Checked)
				cmbPassingGrade.SelectedIndex = Constants.I_ZERO;
			if (grdSubjectTestConfiguration.Rows.Count > Constants.I_ZERO)
				optMarks.Enabled = optGrade.Enabled = false;
		}
		catch (BusinessLogic.Exceptions.ReferenceExceptions oEx)
		{
			lblError.CssClass = S_CLASS_ERROR;
			lblError.Text = oEx.Message;            
			FillSubjectTestGrid();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to go to exam configuration screen.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ae"></param>
	protected void btnBack_Click(object sender, EventArgs ae)
	{
		try
		{
			MasterPage oMasterPage = (MasterPage)Master;
            oMasterPage.RedirectToNextPage(Constants.S_PAGE_SUBJECT_TEST_CONFIGURATION_DISPLAY + "?" + CommonUtility.EncryptQuerystring("SelectedStdId=" + hidSelectedStdId.Value + "&SelectedSubjectId=" + hidSelectedSubjectId.Value));
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to create querystring and move to copy exam configuration screen.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ae"></param>
	protected void btnCopy_Click(object sender, EventArgs ae)
	{
		try
		{
			if (grdSubjectTestConfiguration.Rows.Count == Constants.I_ZERO)
			{
				lblError.CssClass = S_CLASS_ERROR;
				lblError.Text = S_ERROR_NO_TEST_CONFIGURED;
			}
			else
			{
				string sGradeConfiguration = Constants.C_NO.ToString();
				string sQueryString = "NewMode=" + Constants.C_YES;
				sQueryString = sQueryString + "&StandardDivisionId=" + miStandardDivisionId;
				sQueryString = sQueryString + "&SubjectId=" + miSubjectId;
				sQueryString = sQueryString + "&StandardId=" + hidStandardId.Value;
				sQueryString = sQueryString + "&IsGrade=" + sGradeConfiguration;
				sQueryString = sQueryString + "&SubjectName=" + lblSubjectValue.Text.Replace("&", "~");
				sQueryString = sQueryString + "&StdDiv=" + lblStdDivValue.Text;

				string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
				string sUrl = Constants.S_PAGE_COPY_SUBJECT_TEST_CONFIGURATION + "?" + sEncrypt;
				MasterPage oMasterPage = (MasterPage)Master;
				oMasterPage.RedirectToNextPage(sUrl);
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to update final result factor used for final result.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ae"></param>
	protected void BtnUpdateFactor_Click(object sender, EventArgs ae)
	{
		try
		{
			if (!IsResultPublishedForStdDivId(miStandardDivisionId))
			{
				SubjectTestConfigurationBL oSubjectTestConfigurationBL = new SubjectTestConfigurationBL
																			 {
																				 SchoolId = miSchoolId,
																				 AcademicYearId = miAcademicYearId,
																				 StandardDivisionId = miStandardDivisionId,
																				 SubjectId = miSubjectId,
																				 RsltFactor = Convert.ToDouble(txtFactor.Text)
																			 };
				oSubjectTestConfigurationBL.UpdateResultFactor();
				GetTestConfiguration();
			}
			else
			{
				lblError.CssClass = S_CLASS_ERROR;
				lblError.Text = S_RESULT_REFACTOR_MSG;
			}

			DisplayAnnualRsltConfig();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

    /// <summary>
    /// This method use to go on Exam configuration page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClose_Click(object sender, EventArgs e)
    {

        try
        {
            MasterPage oMasterPage = (MasterPage)Master;
            oMasterPage.RedirectToNextPage(Constants.S_PAGE_SUBJECT_TEST_CONFIGURATION_DISPLAY);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
	/// <summary>
	/// This event is used to configure exam result by mark system.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ae"></param>
	protected void optMarks_CheckedChanged(object sender, EventArgs ae)
	{
		try
		{
			chkTotalConsider.Checked = true;
			chkTotalConsider.Enabled = true;
            chkAllowDecimal.Enabled = cmbExams.SelectedValue != Constants.S_ZERO;
            tblARFactor.Visible = true;
			trConsiderAR.Visible = true;
            trDecimal.Visible = true;
			tblTestTypeGrid.Visible = true;
			trGrade.Visible = false;
			trDisplayGradeRow.Visible = mbIsDisplayGradeApplicable;
			txtFactor.Text = S_DEFAULT_FACTOR;
			tdtxtOutOfMarks.Visible = true;
			divOutOfMarksNote.Visible = true;
			
			FillTestTypeGrid(miTestMarksId);
			FillSubjectTestGrid();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to configure exam result by grade system.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ae"></param>
	protected void optGrades_CheckedChanged(object sender, EventArgs ae)
	{
		try
		{
			txtFactor.Text = S_DEFAULT_FACTOR;
			chkTotalConsider.Checked = false;
			chkTotalConsider.Enabled = false;
			tblARFactor.Visible = false;
            trDecimal.Visible = false;
			trConsiderAR.Visible = false;
			tblTestTypeGrid.Visible = false;
			trGrade.Visible = true;
			trDisplayGradeRow.Visible = mbIsDisplayGradeApplicable;
			tdtxtOutOfMarks.Visible = false;
			divOutOfMarksNote.Visible = false;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to check/uncheck checkbox as exam considered in total final marks. 
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ae"></param>
	protected void ChkRslt_CheckedChanged(object sender, EventArgs ae)
	{
		try
		{
			chkTotalConsider.Checked = ChkRslt.Checked;
			chkTotalConsider.Enabled = ChkRslt.Checked;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

    ///// <summary>
    ///// This event is used to check/uncheck checkbox as exam considered in total final marks. 
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="ae"></param>
    //protected void chkAllowDecimal_CheckedChanged(object sender, EventArgs ae)
    //{
    //    try
    //    {
    //        chkTotalConsider.Checked = ChkRslt.Checked;
    //        chkTotalConsider.Enabled = ChkRslt.Checked;
    //    }
    //    catch (Exception ex)
    //    {
    //        ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
    //    }
    //}

	#endregion -- CLICK EVENT(s) --

	#endregion -- EVENT HANDLER(s) --

	#region -- PRIVATE METHOD(s) --
    /// <summary>
    /// This method use to check whether grade configuration set to specific standard before Exam Configuration
    /// </summary>
    private bool CheckPrecondition()
    {
        SubjectTestConfigurationCollectionBL oSubjectTestConfigurationCollectionBL = new SubjectTestConfigurationCollectionBL(miSchoolId, miAcademicYearId);
        StandardGradeConfiguration oStandardGradeConfiguration = oSubjectTestConfigurationCollectionBL.CheckPreConditioOfGrades(miStandardDivisionId);
        int StanardId = oStandardGradeConfiguration.StandardId;
        bool bIsConfigure=true;
        String sQueryString = null;
        if (oStandardGradeConfiguration.IsCocoricularConfigure.Equals(Constants.S_NO) || oStandardGradeConfiguration.IsSubjectConfigure.Equals(Constants.S_NO) || oStandardGradeConfiguration.IsFailCriteriaNotConfigure.Equals(Constants.S_YES))
        {
            bool bIsCocoricularConfig = oStandardGradeConfiguration.IsCocoricularConfigure.Equals(Constants.S_NO) || oStandardGradeConfiguration.IsSubjectConfigure.Equals(Constants.S_NO);
            bool bisFailCriteriaNotConfigure = oStandardGradeConfiguration.IsFailCriteriaNotConfigure.Equals(Constants.S_YES);
            ShowPreconditionFields(bIsCocoricularConfig, bisFailCriteriaNotConfigure);
            if (oStandardGradeConfiguration.IsCocoricularConfigure.Equals(Constants.S_NO) && oStandardGradeConfiguration.IsSubjectConfigure.Equals(Constants.S_YES))
                sQueryString = CommonUtility.EncryptQuerystring("Standard_Id=" + StanardId + "&IsCocuricularConfigure=" +Constants.S_NO);
            if (oStandardGradeConfiguration.IsCocoricularConfigure.Equals(Constants.S_YES) && oStandardGradeConfiguration.IsSubjectConfigure.Equals(Constants.S_NO))
                sQueryString = CommonUtility.EncryptQuerystring("Standard_Id=" + StanardId + "&IsCocuricularConfigure=" +Constants.S_YES);
            if (oStandardGradeConfiguration.IsCocoricularConfigure.Equals(Constants.S_NO) && oStandardGradeConfiguration.IsSubjectConfigure.Equals(Constants.S_NO))
                sQueryString = CommonUtility.EncryptQuerystring("Standard_Id=" + StanardId);
            hlGradeConfiguration.NavigateUrl = "../Admin/MarksGradeConfiguration.aspx?" +sQueryString;
            hlFailCriteria.NavigateUrl = "../Admin/FailCriteriaUI.aspx";
            bIsConfigure = false;
         }
        return bIsConfigure;

    }
    /// <summary>
    /// This method use to hide main controls that require when precondition become true
    /// </summary>
    private void ShowPreconditionFields(bool abIsCocoricularConfig, bool abisFailCriteriaNotConfigure)
    {
        divErr.Visible = true;
        if (abIsCocoricularConfig)
        {
            hlGradeConfiguration.Visible = true;
            hlFailCriteria.Visible = false;
        }
        if (!abIsCocoricularConfig)
           if(abisFailCriteriaNotConfigure)
            hlFailCriteria.Visible = true;
        mainDiv.Visible = false;
        btnClose.Visible = true;
    }
	/// <summary>
	/// This method is used to check the result for that test is published or not.
	/// </summary>
	/// <param name="aiStandardDivisionId"></param>
	/// <returns></returns>
	private bool IsResultPublishedForStdDivId(int aiStandardDivisionId)
	{
		SchoolWiseAnnualResultPublishBL oSwStdDivResultPublishBL = new SchoolWiseAnnualResultPublishBL(miSchoolId, miAcademicYearId, aiStandardDivisionId);
		return oSwStdDivResultPublishBL.AnnualResult_publish_Id != Constants.I_ZERO;
	}

	/// <summary>
	/// This method is used to set java scripts to page controls.
	/// </summary>
	private void SetClientScriptAttributes()
	{
        ApplyMouseHoverEffect(new List<Button> { btnAdd, btnBack, btnCopy, BtnUpdateFactor,btnClose ,btnDelete});
		btnAdd.Attributes.Add("onclick", "return disableButtons(this);");
		cmbExams.Attributes.Add("onchange", "VisibleSummary()");
		grdSubjectTestConfiguration.Columns[I_DEFAULT].HeaderText = string.Empty;
		txtTestOutOfMarks.Attributes.Add("onChange", "SetOutOfMarks(this, 'TestOutOfMarks')");
		SiteMap.SiteMapResolve += SiteMap_SiteMapResolve;
	}

	/// <summary>
	/// This method sets total marks and passing marks values to 0.
	/// </summary>
	private void ResetTotalFields()
	{
		txtAllTotalMarks.Text = S_DEFAULT;
		txtAlltotPassingMarks.Text = S_DEFAULT;
	}

	/// <summary>
	/// This method is used to set sort variables.
	/// </summary>
	private void SetSortVariables()
	{
		hidSortDirection.Value = hidSortDirection.Value == Constants.S_DESCENDING ? Constants.S_ASCENDING : Constants.S_DESCENDING;
	}

	/// <summary>
	/// This method sets all the properties of SubjectTestConfigurationBL object.
	/// </summary>
	/// <returns></returns>
	private SubjectTestConfigurationBL SetSubjectTestConfigurationBL()
	{
		SubjectTestConfigurationBL oSubjectTestConfigurationBL = new SubjectTestConfigurationBL {
																	SchoolId = miSchoolId,
																	AcademicYearId = miAcademicYearId,
																	StandardDivisionId = miStandardDivisionId,
																	SubjectId = miSubjectId,
																	InsertedByid = miUserId,
																	TestWiseSubjectMarksId = miTestMarksId,
																	SchoolWiseTestId = Convert.ToInt32(cmbExams.SelectedValue),
																	IsExamStatusApplicable = chkExamStatus.Checked,
																	DisplayGrade = chkDisplayGrade.Checked
																 };

		if (optMarks.Checked)
		{
			CalculateTotals();
			oSubjectTestConfigurationBL.PassingTotalMarks = Convert.ToDecimal(txtAlltotPassingMarks.Text);
			oSubjectTestConfigurationBL.SubjectTotalMarks = Convert.ToInt32(txtAllTotalMarks.Text);
			if (!string.IsNullOrEmpty(txtTestOutOfMarks.Text.Trim()) && hidTestOutOfMarksApplicable.Value == Constants.S_YES)
				oSubjectTestConfigurationBL.OutOfMarks = Convert.ToInt32(txtTestOutOfMarks.Text);
			if ((string.IsNullOrEmpty(txtTestOutOfMarks.Text.Trim()) || txtTestOutOfMarks.Text == Constants.I_ZERO.ToString()) && hidTestOutOfMarksApplicable.Value == Constants.S_YES)
				oSubjectTestConfigurationBL.OutOfMarks = Convert.ToInt32(txtAllTotalMarks.Text);
			if (!string.IsNullOrEmpty(txtTestOutOfMarks.Text.Trim()) && txtTestOutOfMarks.Text != Constants.I_ZERO.ToString())
				oSubjectTestConfigurationBL.OutOfMarks = Convert.ToInt32(txtTestOutOfMarks.Text);
			oSubjectTestConfigurationBL.GradeOrMarks = S_SHORT_MARKS;
			oSubjectTestConfigurationBL.PassingGradeId = Constants.I_ZERO;
			oSubjectTestConfigurationBL.ResultConsideration = ChkRslt.Checked ? Constants.C_YES.ToString() : Constants.C_NO.ToString();
			oSubjectTestConfigurationBL.RsltFactor = Convert.ToDouble(txtFactor.Text);
			oSubjectTestConfigurationBL.TotalConsideration = chkTotalConsider.Checked ? Constants.C_YES.ToString() : Constants.C_NO.ToString();
            oSubjectTestConfigurationBL.AllowDecimal = chkAllowDecimal.Checked;
		}
		else
		{
			oSubjectTestConfigurationBL.GradeOrMarks = S_SHORT_GRADES;
			oSubjectTestConfigurationBL.PassingGradeId = Convert.ToInt32(cmbPassingGrade.SelectedItem.Value);
			oSubjectTestConfigurationBL.PassingTotalMarks = Constants.I_ZERO;
			oSubjectTestConfigurationBL.SubjectTotalMarks = Constants.I_ZERO;
			oSubjectTestConfigurationBL.ResultConsideration = Constants.C_NO.ToString();
			oSubjectTestConfigurationBL.RsltFactor = Constants.I_ZERO;
			oSubjectTestConfigurationBL.TotalConsideration = Constants.C_NO.ToString();
		}

		return oSubjectTestConfigurationBL;
	}

	/// <summary>
	/// This method is used to fill passsing grade combo.
	/// </summary>
	private void FillPassingGradeCombo()
	{
        DataTable oDtGradesForStandard = MarksGradesConfigurationBL.GetAllGradesForStandard(miSchoolId, miAcademicYearId, Convert.ToInt32(hidStandardId.Value), miSubjectId);
		ControlUtility.FillDropDownList(oDtGradesForStandard, ref cmbPassingGrade, "Marks_Grades_Configuration_Detail_ID", "Grade_Name", Constants.S_SELECT);
	}

	/// <summary>
	/// This method is used to fill exam type grid.
	/// </summary>
	/// <param name="aiTestMarksId"></param>
	private void FillTestTypeGrid(int aiTestMarksId)
	{
		DataTable oDtTestConfiguration = null;
        oDtTestConfiguration = SubjectTestTypeConfigurationBL.FetchAllTestSubjectMarksDetailsDataFromDatabase(aiTestMarksId, miSubjectId);
		DataView oDtItemView = oDtTestConfiguration.DefaultView;
		grdTestTypes.DataSource = oDtItemView;
		grdTestTypes.DataBind();
	}

	/// <summary>
	/// This method is used to get testwise subject mark id. 
	/// </summary>
	private void GetTestMarksId()
	{
		DataTable oDtTestConfiguration = (DataTable)ViewState[Constants.S_TEMP_SESSION_DS];

		string sTestId = cmbExams.SelectedValue;
		DataRow oDtRow = GetValueFromDataTable(oDtTestConfiguration, "SchoolWise_Test_Id", sTestId);
		miTestMarksId = oDtRow != null ? Convert.ToInt32(oDtRow["TestWise_Subject_Marks_Id"].ToString()) : Constants.I_ZERO;
	}

	/// <summary>
	/// This method sets all the form fields with the values in data row.
	/// </summary>
	private void SetFormFields()
	{
		DataTable oDtTestConfiguration = (DataTable)ViewState[Constants.S_TEMP_SESSION_DS];
		string sTestId = cmbExams.SelectedValue;
		DataRow oDtRow = GetValueFromDataTable(oDtTestConfiguration, "SchoolWise_Test_Id", sTestId);

		// if the record for the test exists in database.
		// i.e. test is configured
		if (oDtRow != null)
		{
			ChkRslt.Enabled = true;
			chkExamStatus.Enabled = true;
            chkAllowDecimal.Enabled = true;
			optGrade.Enabled = false;
			optMarks.Enabled = false;

			// Radio buttons
			string sGradeOrMarks = oDtRow["Grade_Or_Marks"].ToString();

			if (sGradeOrMarks.Equals(S_SHORT_MARKS))
			{
				txtAllTotalMarks.Text = oDtRow["Subject_Total_Marks"].ToString();
                txtAlltotPassingMarks.Text = oDtRow[S_ALLOW_DECIMAL].ToBool() ? oDtRow["Passing_Total_Marks"].ToString() : oDtRow["Passing_Total_Marks"].ToDecimal().ToInt().ToString();
				grdTestTypes.Enabled = true;
				trGrade.Visible = false;
				trDisplayGradeRow.Visible = mbIsDisplayGradeApplicable;
				chkDisplayGrade.Enabled = true;
				optMarks.Checked = true;
				
				trRslt.Visible = true;
				if (oDtRow["Total_Consideration"].ToString() == Constants.C_YES.ToString())
					chkTotalConsider.Checked = true;

				if (!string.IsNullOrEmpty(oDtRow["TestOutOfMarks"].ToString()))
					txtTestOutOfMarks.Text = oDtRow["TestOutOfMarks"].ToString();
			}
			else
			{
				ResetTotalFields();
				ResetGrid();
				chkTotalConsider.Checked = false;
				chkTotalConsider.Enabled = false;
				optGrade.Checked = true;
                trDecimal.Visible = false;
                chkAllowDecimal.Enabled = false;
				tblARFactor.Visible = false;
				trConsiderAR.Visible = false;
				tblTestTypeGrid.Visible = false;
				cmbPassingGrade.SelectedIndex = -1;
				ListItem oListItem = cmbPassingGrade.Items.FindByText(oDtRow["Grade_Name"].ToString());
				if (oListItem != null)
					oListItem.Selected = true;
				trRslt.Visible = false;
				trDisplayGradeRow.Visible = false;
			}

			miTestMarksId = Convert.ToInt32(oDtRow["TestWise_Subject_Marks_Id"].ToString());
			btnAdd.Text = S_UPDATE;
			txtFactor.Text = oDtRow["Result_Factor"].ToString();
			hidMode.Value = Constants.ViewMode.Edit.ToString();
			ChkRslt.Checked = oDtRow["Result_Consideration"].ToString().Equals("Y");
			chkExamStatus.Checked = Convert.ToBoolean(oDtRow["IsExamStatusApplicable"]);
			chkDisplayGrade.Checked = oDtRow["DisplayGrade"].ToBool();
            chkAllowDecimal.Checked = oDtRow[S_ALLOW_DECIMAL].ToBool();
		}

		// if test is not configured
		else
		{
			if (cmbExams.SelectedIndex != Constants.I_ZERO)
			{
				grdTestTypes.Enabled = true;
				ChkRslt.Enabled = true;
				chkExamStatus.Enabled = true;
				trDisplayGradeRow.Visible = mbIsDisplayGradeApplicable && optMarks.Checked;
				chkDisplayGrade.Enabled = true;
				chkDisplayGrade.Checked = false;
                chkAllowDecimal.Enabled = true;
                chkAllowDecimal.Checked = false;
			}
			else
			{
				chkDisplayGrade.Enabled = false;
				chkDisplayGrade.Checked = false;
			}

			btnAdd.Text = S_ADD;
			hidMode.Value = Constants.ViewMode.New.ToString();
			ResetTotalFields();
			miTestMarksId = Constants.I_ZERO;
			ChkRslt.Checked = true;
			chkExamStatus.Checked = true;
			trDisplayGradeRow.Visible = trDisplayGradeRow.Visible && mbIsDisplayGradeApplicable && optMarks.Checked;
		}
	}

	/// <summary>
	/// This method resets all the fields.
	/// 1. Sets combobox to default 0.
	/// 2. totals to 0
	/// 3. Button label to "Add"
	/// </summary>
	private void ResetFormFields()
	{
		cmbExams.SelectedIndex = Constants.I_ZERO;
		ResetGrid();
		ResetTotalFields();
		btnAdd.Text = S_ADD;
		hidMode.Value = Constants.ViewMode.New.ToString();
		if (!IsPostBack)
			optMarks.Checked = true;
		ChkRslt.Enabled = false;
		chkExamStatus.Enabled = false;
        chkAllowDecimal.Enabled = false;
	}

	/// <summary>
	/// This method resets the exam type grid
	/// 1. Sets all text boxes in grid to 0.
	/// 2. uncheck all checkboxes
	/// 3. disables the grid
	/// </summary>
	private void ResetGrid()
	{
		for (int iRowIndex = 0; iRowIndex < grdTestTypes.Rows.Count; iRowIndex++)
		{
			CheckBox oChkTestType = (CheckBox)grdTestTypes.Rows[iRowIndex].FindControl("chkTestType");
			oChkTestType.Checked = false;
			TextBox oTxtTotMarks = (TextBox)grdTestTypes.Rows[iRowIndex].FindControl("txtTotMarks");
			oTxtTotMarks.Text = S_DEFAULT;
			TextBox oTxtPassingMarks = (TextBox)grdTestTypes.Rows[iRowIndex].FindControl("txtPassingMarks");
			oTxtPassingMarks.Text = S_DEFAULT;
			TextBox oTxtOutOfMarks = (TextBox)grdTestTypes.Rows[iRowIndex].FindControl("txtOutOfMarks");
			oTxtOutOfMarks.Text = S_DEFAULT;
		}

		grdTestTypes.Enabled = false;
		ResetTotalFields();
		ChkRslt.Enabled = false;
		chkExamStatus.Checked = true;
		chkExamStatus.Enabled = false;
		txtTestOutOfMarks.Text = string.Empty;
		txtTestOutOfMarks.Enabled = false;
		trDisplayGradeRow.Visible = mbIsDisplayGradeApplicable;
		chkDisplayGrade.Enabled = false;
		chkDisplayGrade.Checked = false;
	}

	/// <summary>
	/// This method binds subject test configuration grid.
	/// </summary>
	private void FillSubjectTestGrid()
	{
		GetTestConfiguration();

		if (ViewState[Constants.S_TEMP_SESSION_DS] != null)
		{
			DataTable oDtTestConfiguration = (DataTable)ViewState[Constants.S_TEMP_SESSION_DS];
			hidSortExpression.Value = grdSubjectTestConfiguration.Columns[I_TEST_NAME_COLUMN_INDEX].SortExpression;
			DataView oDtItemView = new DataView(oDtTestConfiguration, string.Empty, hidSortExpression.Value + " " + hidSortDirection.Value, DataViewRowState.OriginalRows);



			if (optGrade.Checked)
			{
				grdSubjectTestConfiguration.Columns[I_DEFAULT].Visible = true;
				grdSubjectTestConfiguration.Columns[I_TOTAL_MARKS_COLUMN_INDEX].Visible = true;
			}

			grdSubjectTestConfiguration.DataSource = oDtItemView;
			grdSubjectTestConfiguration.DataBind();


            DataRow[] oDrDate = oDtTestConfiguration.Select("IsExamMarkEntered='N'") as DataRow[];
            
            if (oDrDate.Length > 0)
                btnDelete.Enabled = true;
            else
                btnDelete.Enabled = false;

			if (grdSubjectTestConfiguration.Rows.Count > Constants.I_ZERO)
			{
				BtnUpdateFactor.Enabled = true;
				btnCopy.Enabled = true;
				if (optGrade.Checked)
				{
					grdSubjectTestConfiguration.Columns[I_DEFAULT].Visible = false;
					grdSubjectTestConfiguration.Columns[I_TOTAL_MARKS_COLUMN_INDEX].Visible = false;
					grdSubjectTestConfiguration.Columns[I_OUT_OF_MARKS_COLUMN_INDEX].Visible = false;
					grdSubjectTestConfiguration.HeaderRow.Cells[3].Text = S_PASSING_GRADE;

					tdtxtOutOfMarks.Visible = false;
					divOutOfMarksNote.Visible = false;
				}
				else
				{
					trRslt.Visible = true;
					grdSubjectTestConfiguration.Columns[I_DEFAULT].Visible = true;
					grdSubjectTestConfiguration.Columns[I_TOTAL_MARKS_COLUMN_INDEX].Visible = true;
					grdSubjectTestConfiguration.Columns[I_OUT_OF_MARKS_COLUMN_INDEX].Visible = true;
					grdSubjectTestConfiguration.HeaderRow.Cells[3].Text = S_TOTAL_PASSING_MARK;

					if (txtTestOutOfMarks.Enabled)
					{
						DataRow oDrRow = GetValueFromDataTable(oDtTestConfiguration, "SchoolWise_Test_Id", cmbExams.SelectedValue);
						if (oDrRow != null)
						{
							int iOutOfMarks = Convert.ToInt32(oDrRow["TestOutOfMarks"]);
							txtTestOutOfMarks.Text = iOutOfMarks.ToString();
						}
					}
				}
			}
			else
			{
				BtnUpdateFactor.Enabled = false;
				btnCopy.Enabled = false;
				trRslt.Visible = false;
			}
		}
       
         
       

		DisplayAnnualRsltConfig();
	}

	/// <summary>
	/// This method is used to display annual configuration.
	/// </summary>
	private void DisplayAnnualRsltConfig()
	{
        int iTotMax = Constants.I_ZERO;
        decimal iTotPassing = Constants.I_ZERO;
		int iRowCnt = grdSubjectTestConfiguration.Rows.Count;
		int iTestCnt = Constants.I_ZERO;
		for (int i = 0; i < iRowCnt; i++)
		{
			if (grdSubjectTestConfiguration.DataKeys[i][I_DATAKEY_RSLT].ToString().Trim().Equals("Y"))
			{
                iTotMax += grdSubjectTestConfiguration.Rows[i].Cells[I_TOTAL_MARKS_COLUMN_INDEX].Text.ToInt();
                Label olblPassingMarks = (Label)grdSubjectTestConfiguration.Rows[i].Cells[I_PASSING_MARKS_COLUMN_INDEX].FindControl("lblPassingMarks");
                iTotPassing += olblPassingMarks.Text.ToDecimal();
				iTestCnt++;
			}
		}

		decimal iRsltFactor = Convert.ToDecimal(txtFactor.Text);
		decimal iRsltTot = Constants.I_ZERO;
		decimal iRsltPassingTot = Constants.I_ZERO;
		if (iRsltFactor != Constants.I_ZERO)
		{
			iRsltTot = iTotMax / iRsltFactor;
			iRsltPassingTot = iTotPassing / iRsltFactor;
		}

		iRsltFactor = decimal.Round(iRsltFactor, 2);
		iRsltTot = decimal.Round(iRsltTot, 2);
		iRsltPassingTot = decimal.Round(iRsltPassingTot, 2);
		if (iRowCnt > Constants.I_ZERO && iTotMax > Constants.I_ZERO && iRsltFactor > Constants.I_ZERO)
		{
			trRslt.Visible = true;
			txtRsltTot.Text = iTotMax + "/" + iRsltFactor + " = " + iRsltTot;
			txtRsltPassing.Text = iTotPassing + "/" + iRsltFactor + " = " + iRsltPassingTot;
		}
		else
		{
			trRslt.Visible = false;
			txtRsltTot.Text = string.Empty;
			txtRsltPassing.Text = string.Empty;
		}
	}

	/// <summary>
	/// This method calculates the sum of:
	/// 1. Total marks
	/// 2. Passing marks
	/// And assigns these sums to respective textboxes. 
	/// </summary>
	private void CalculateTotals()
	{
		int iTotMax = Constants.I_ZERO;
		decimal dTotPassing = Constants.I_ZERO;

		for (int i = 0; i < grdTestTypes.Rows.Count; i++)
		{
			TextBox oTxtTotMarks = (TextBox)grdTestTypes.Rows[i].FindControl("txtTotMarks");
			iTotMax += Convert.ToInt32(oTxtTotMarks.Text);

			TextBox oTxtPassingMarks = (TextBox)grdTestTypes.Rows[i].FindControl("txtPassingMarks");
            dTotPassing += oTxtPassingMarks.Text.ToDecimal();
		}

		txtAllTotalMarks.Text = iTotMax.ToString();
        txtAlltotPassingMarks.Text = dTotPassing.ToString();
	}

	/// <summary>
	/// This method initialises member variables of the class.
	/// </summary>
	private void ReadQueryString()
	{
        if (QueryString["StandardDivisionId"] != null)
            miStandardDivisionId = QueryString["StandardDivisionId"].ToInt();
        
		if (QueryString["SubjectId"] != null)
            miSubjectId = QueryString["SubjectId"].ToInt();

        if (QueryString["SelectedStdId"] != null)
            hidSelectedStdId.Value = QueryString["SelectedStdId"].ToString();

        if (QueryString["SelectedSubjectId"] != null)
            hidSelectedSubjectId.Value = QueryString["SelectedSubjectId"].ToString();
	}

	/// <summary>
	/// This method clears the error label.
	/// </summary>
	private void ClearErrorLabel()
	{
		lblError.CssClass = string.Empty;
		lblError.Text = string.Empty;
	}

	/// <summary>
	/// This method is used to fill grid as per test configured or not.
	/// </summary>
	private void SearchConfiguredTest()
	{
		int iTestCount = cmbExams.Items.Count;
		int iRowIndex;
		DataTable oDtTestConfiguration = (DataTable)ViewState[Constants.S_TEMP_SESSION_DS]; 

		// loop through the combobox items.
		for (iRowIndex = 0; iRowIndex < iTestCount; iRowIndex++)
		{
			string sTestId = cmbExams.Items[iRowIndex].Value;
			DataRow oDrRow = GetValueFromDataTable(oDtTestConfiguration, "SchoolWise_Test_Id", sTestId);

			// The test is configured.
			if (oDrRow != null)
			{
				cmbExams.Items[iRowIndex].Selected = true;
				miTestMarksId = Convert.ToInt32(oDrRow["TestWise_Subject_Marks_Id"].ToString());
				ChkRslt.Enabled = true;
				break;
			}
		}

		if (iRowIndex != iTestCount)
			SetFormFields();
	}

	/// <summary>
	/// This method is called on page load to initialize all the fields.    
	/// </summary>
	private void InitializeFields()
	{
		ClearErrorLabel();
		SetHeaderLabel();
		FillTestCombobox();

        if (QueryString["ViewMode"] != null)
            hidMode.Value = QueryString["ViewMode"];
		FillPassingGradeCombo();
        hidIsDisplayGradeApplicable.Value = IsDisplayGradeApplicable().ToString();
		mbIsDisplayGradeApplicable = !hidIsDisplayGradeApplicable.Value.IsNullOrEmpty() && hidIsDisplayGradeApplicable.Value.ToBool();
       
		// if in edit mode
		if (hidMode.Value.Equals(Constants.ViewMode.Edit.ToString()))
		{
			GetTestConfiguration();
			SearchConfiguredTest();
		}

		// In new mode 
		else
		{
			txtFactor.Text = S_DEFAULT_FACTOR;
			ResetFormFields();
			trGrade.Visible = false;
			trDisplayGradeRow.Visible = mbIsDisplayGradeApplicable;
			chkDisplayGrade.Enabled = false;
			chkTotalConsider.Checked = true;
		}

		valsumCopyConfig.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
		valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
		valSumUpdateFactor.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
		btnCopy.Attributes.Add("onclick", "if(!ConfirmCopy()){return false;}");
        btnDelete.Attributes.Add("onclick", "if(!ConfirmCopy1()){return false;}");
		cmbExams.Attributes.Add("onchange", "resetErrorLabel()");
		optGrade.Attributes.Add("onclick", "resetValSummery()");
		optMarks.Attributes.Add("onclick", "resetValSummery()");
        chkAllowDecimal.Attributes.Add("onclick", "SetControlProperties()");
	}

	/// <summary>
	/// This method gets the dataset for test configuration in session.
	/// </summary>
	private void GetTestConfiguration()
	{
		SubjectTestConfigurationCollectionBL oSubjectTestConfigurationCollectionBL = new SubjectTestConfigurationCollectionBL(miSchoolId, miAcademicYearId);
		DataTable oDtTestConfiguration = oSubjectTestConfigurationCollectionBL.RetriveAllTestConfiguration(miStandardDivisionId, miSubjectId);
		ViewState[Constants.S_TEMP_SESSION_DS] = oDtTestConfiguration;
	}


    
	/// <summary>
	/// This method sets the text for header label.
	/// It retrives standard-division name, subject name and displays them on the header label.
	/// </summary>
	private void SetHeaderLabel()
	{
        if (QueryString.Count > Constants.I_ZERO)
        {
            string sSubjectName = QueryString["SubjectName"];
            string sStandardDivisionName = QueryString["StdDivName"];
            lblStdDivValue.Text = sStandardDivisionName;
            lblSubjectValue.Text = sSubjectName.Replace("~", "&");
            hidStandardId.Value = QueryString["StdId"];
        }
	}

	/// <summary>
	/// This function looks for the given value in the dataset
	/// and returns the 1st datarow of containing the value.
	/// </summary>
	/// <param name="aoDt"></param>
	/// <param name="asFieldToCheck">field to which the value may belong</param>
	/// <param name="asValueToCheck">the value to search for</param>
	/// <returns>data row containing the value of the field.</returns>
	private DataRow GetValueFromDataTable(DataTable aoDtParam, string asFieldToCheck, string asValueToCheck)
	{
		if (aoDtParam == null) return null;
		if (aoDtParam.Rows.Count == Constants.I_ZERO) return null;
		DataRow[] oDrCheckValue = aoDtParam.Select(asFieldToCheck + " = " + asValueToCheck);
		if (oDrCheckValue.Length > Constants.I_ZERO)
			return oDrCheckValue[I_DEFAULT];
		return null;
	}

	/// <summary>
	/// This method is used to fill exam combo.
	/// </summary>
	private void FillTestCombobox()
	{
		SchoolwiseStandardTestMasterCollectionBL oTestCollectionBL = new SchoolwiseStandardTestMasterCollectionBL(miSchoolId, miAcademicYearId);
		DataTable oDtAllTests = oTestCollectionBL.GetAllTestsForStandard(Convert.ToInt32(hidStandardId.Value));
		ControlUtility.FillDropDownList(oDtAllTests, ref cmbExams, Constants.S_TEST_ID_FIELD, Constants.S_TEST_NAME_FIELD, Constants.S_SELECT);
		if (cmbExams.Items.Count < 2)
		{
			lblError.Visible = true;
			lblError.CssClass = S_CLASS_ERROR;
			lblError.Text = S_ERROR_EXAM_NOT_CONFIGURED;
		}
	}

	/// <summary>
	/// This function formulates the XML string from  values in grid rows.
	/// </summary>
	/// <returns></returns>
	private string GetXmlString()
	{
		const string S_ELEMENT = "element";
		XmlDocument oDoc = new XmlDocument();

		// Create a root level element.
		XmlElement oRoot = oDoc.CreateElement("SchoolWiseTestSubjectMarksDetails");
		XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "SchoolWiseTestSubjectMarksDetails", string.Empty);

		// Loop through all the grid rows.
		for (int iRowCount = 0; iRowCount <= grdTestTypes.Rows.Count - 1; iRowCount++)
		{
			CheckBox oChkTestType = (CheckBox)grdTestTypes.Rows[iRowCount].FindControl("chkTestType");

			if (oChkTestType.Checked)
			{
				// Create root xml element.
				XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "SchoolWiseTestSubjectMarksDetail", string.Empty);

				string sAtrrName = "TestWise_Subject_Marks_Id";
				XmlAttribute oAttr = oDoc.CreateAttribute(sAtrrName);
				oAttr.Value = miTestMarksId.ToString();
				oXmlNode.Attributes.Append(oAttr);

				sAtrrName = "TestType_Id";
				oAttr = oDoc.CreateAttribute(sAtrrName);
				oAttr.Value = grdTestTypes.DataKeys[iRowCount]["TestType_Id"].ToString();
				oXmlNode.Attributes.Append(oAttr);

				sAtrrName = "TestType_Total_Marks";
				oAttr = oDoc.CreateAttribute(sAtrrName);
				TextBox oTxtTotalMarks = (TextBox)grdTestTypes.Rows[iRowCount].FindControl("txtTotMarks");
				oAttr.Value = oTxtTotalMarks.Text;
				oXmlNode.Attributes.Append(oAttr);

				sAtrrName = "TestType_Passing_Marks";
				oAttr = oDoc.CreateAttribute(sAtrrName);
				TextBox oTxtPassingMarks = (TextBox)grdTestTypes.Rows[iRowCount].FindControl("txtPassingMarks");
				oAttr.Value = oTxtPassingMarks.Text;
				oXmlNode.Attributes.Append(oAttr);

				TextBox oTxtOutOfMarks = (TextBox)grdTestTypes.Rows[iRowCount].FindControl("txtOutOfMarks");
				sAtrrName = "OutOfMarks";
				oAttr = oDoc.CreateAttribute(sAtrrName);
				if (hidTestTypeOutOfMarksApplicable.Value == Constants.S_YES && (oTxtOutOfMarks.Text == string.Empty || oTxtOutOfMarks.Text == Constants.I_ZERO.ToString()))
					oTxtOutOfMarks.Text = oTxtTotalMarks.Text;
				oAttr.Value = oTxtOutOfMarks.Text;
				oXmlNode.Attributes.Append(oAttr);

				// Add the node to root node.
				oXmlRootNode.AppendChild(oXmlNode);
			}
		}

		// Add the root node to document element. 
		oRoot.AppendChild(oXmlRootNode);

		// return the string generated.
		return oRoot.InnerXml;
	}

	/// <summary>
	/// This method is used to set hidden sort variables.
	/// </summary>
	private void SetControlsDefaultValues()
	{
		hidSortExpression.Value = grdSubjectTestConfiguration.Columns[I_TEST_NAME_COLUMN_INDEX].SortExpression;
		hidSortDirection.Value = Constants.S_ASCENDING;       
	}

	/// <summary>
	/// This event is used to set sitemap value.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ae"></param>
	/// <returns></returns>
	private SiteMapNode SiteMap_SiteMapResolve(object sender, SiteMapResolveEventArgs ae)
	{
		SiteMapNode oSiteMapNode = SiteMap.CurrentNode.Clone(true);
		SiteMapNode oTempSiteMapNode = oSiteMapNode;
		SiteMapNode oSiteMapParentNode = oTempSiteMapNode.ParentNode;

		try
		{
			if (HidField_URL.Value != string.Empty && HidField_URL.Value.Contains(oTempSiteMapNode.Url))
				oTempSiteMapNode.Url = HidField_URL.Value;
			else if (HidField_URL.Value != string.Empty && HidField_URL.Value.Contains(oSiteMapParentNode.Url))
				oSiteMapParentNode.Url = HidField_URL.Value;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}

		return oSiteMapNode;
	}
	
	/// <summary>
	///		Determines if DisplayGrade settings are applicable for the current class.
	/// </summary>
	/// <returns></returns>
	private bool IsDisplayGradeApplicable()
	{
		if (mlstStandardsWithOnlyGrades.IsNull() || mlstStandardsWithOnlyGrades.Count == 0)
			mlstStandardsWithOnlyGrades = StandardMasterBL.GetStandardsWithOnlyGradeSetting(miSchoolId, miAcademicYearId);
	
		if (mlstStandardsWithOnlyGrades.IsNull() || mlstStandardsWithOnlyGrades.Count == 0 || hidStandardId.Value.IsNullOrEmpty())
			return false;

		int iStandardId = hidStandardId.Value.ToInt();
		if (iStandardId != 0 && !mlstStandardsWithOnlyGrades.Any(std => std.StandardId == iStandardId))
		{
			if (mlstSecondLanguageSubjects.IsNull() || mlstSecondLanguageSubjects.Count == 0)
			{
				var oSecondLanguageBL = new SecondLanguageBL(miSchoolId, miAcademicYearId);
				mlstSecondLanguageSubjects = oSecondLanguageBL.GetAll(miStandardDivisionId);
			}

			if (mlstSecondLanguageSubjects.IsNull() || mlstSecondLanguageSubjects.Count == 0)
				return false;
			
			return mlstSecondLanguageSubjects.Any(sub => sub.Original_Subject_Id == miSubjectId);
		}

		return false;
	}

	#endregion -- PRIVATE METHOD(s) --

  
	
}