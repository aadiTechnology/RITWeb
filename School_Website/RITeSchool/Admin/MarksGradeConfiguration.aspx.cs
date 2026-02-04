using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

/// <summary>
/// This class for MarksGradeConfiguration
/// </summary>
public partial class MarksGradeConfiguration : SchoolBase
{
    #region Constants

    const int I_REMARKS_COLUMN_NUMBER = 4;
    const int I_GRADE_NAME_COLUMN_NUMBER = 3;
    const int I_ENDING_MARKS_COLUMN_NUMBER = 2;
    const int I_STARTING_MARKS_COLUMN_NUMBER = 1;

    const int I_DATAKEY_STARTING_MARKS_RANGE = 0;
    const int I_DATAKEY_ENDING_MARKS_RANGE = 1;
    const int I_DATAKEY_GRADE_NAME = 2;
    const int I_DATAKEY_REMARKS = 3;
    const int I_DATAKEY_ACADEMIC_YR_ID = 5;
    const int I_DATAKEY_ORIGINAL_CONFIG_ID = 6;
    #endregion 
    #region protected
    /// <summary>
    /// this page load event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            ddlStandards.Focus();
           
            if (!IsPostBack)
            {
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                DesignSettingAccordingLanguage();
                optSubjects.Checked = true;
                imgBtnSave.Visible = false;
                DisplayControls();
                ReadQuerystringForGrade();
            }

            if (Session[Constants.S_SESSION_LANGUAGE] != null)
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                DesignSettingAccordingLanguage();
            }
          
            SetJavascriptAttributes();
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method for gridview Row DataBound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdMarkGrades_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= 0)
            {
                int iRowNo = e.Row.RowIndex + 2;

                // Set the data in the textbox.
                TextBox oTxtStartingMarks = (TextBox)e.Row.Cells[I_STARTING_MARKS_COLUMN_NUMBER].FindControl("txtStartingMarks");
                oTxtStartingMarks.Text = grdMarkGrades.DataKeys[e.Row.RowIndex][I_DATAKEY_STARTING_MARKS_RANGE].ToString();
                oTxtStartingMarks.Attributes.Add("onkeyup", "OnGridKeyUpNumber(this,0,false,event);");
                TextBox oTxtEndingMarks = (TextBox)e.Row.Cells[I_ENDING_MARKS_COLUMN_NUMBER].FindControl("txtEndingMarks");
                oTxtEndingMarks.Text = grdMarkGrades.DataKeys[e.Row.RowIndex][I_DATAKEY_ENDING_MARKS_RANGE].ToString();
                oTxtEndingMarks.Attributes.Add("onkeyup", "OnGridKeyUpNumber(this,0,false,event);");
                TextBox oTxtGradeName = (TextBox)e.Row.Cells[I_GRADE_NAME_COLUMN_NUMBER].FindControl("txtGradeName");
                oTxtGradeName.Text = grdMarkGrades.DataKeys[e.Row.RowIndex][I_DATAKEY_GRADE_NAME].ToString();
                oTxtGradeName.Attributes.Add("onkeyup", "OnGridKeyUp(this,event);");
                TextBox oTxtRemarks = (TextBox)e.Row.Cells[I_REMARKS_COLUMN_NUMBER].FindControl("txtRemarks");
                oTxtRemarks.Text = grdMarkGrades.DataKeys[e.Row.RowIndex][I_DATAKEY_REMARKS].ToString();
                oTxtRemarks.Attributes.Add("onkeyup", "OnGridKeyUp(this,event);");
                CheckBox oChkSelected = (CheckBox)e.Row.Cells[0].FindControl("ChkBoxDelete");
                oChkSelected.Attributes.Add("onclick", "EnableOrDisableRelatedControl(this," + iRowNo + ")");
                oChkSelected.Checked = !grdMarkGrades.DataKeys[e.Row.RowIndex][I_DATAKEY_ACADEMIC_YR_ID].ToString().Equals(string.Empty);
                if (oChkSelected.Checked == false)
                {
                    oTxtStartingMarks.Enabled = false;
                    oTxtEndingMarks.Enabled = false;
                    oTxtGradeName.Enabled = false;
                    oTxtRemarks.Enabled = false;
                }
            }
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This is for Button Save
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgBtnSave_Click(object sender, EventArgs e)
    {
        // Save the changes to database.
        

        string sErrMsg = string.Empty;
        try
        {
            MarksGradesConfigurationBL oMarksGradesConfigurationBL = new MarksGradesConfigurationBL();
            string sMarkGrades = GetMarkGradesXML();
            oMarksGradesConfigurationBL.Academic_Year_Id = miAcademicYearId;
            oMarksGradesConfigurationBL.Inserted_By_Id = miUserId;
            oMarksGradesConfigurationBL.School_Id = miSchoolId;
            oMarksGradesConfigurationBL.MarksGradesConfigurationDetails = sMarkGrades;
            oMarksGradesConfigurationBL.Standard_Id = Convert.ToInt32(ddlStandards.SelectedValue);
            oMarksGradesConfigurationBL.IsCoCurricularSubjects = Convert.ToBoolean(optSubjects.Checked ? false : true);
            if (hidMode.Value.Equals("New"))
            {
                oMarksGradesConfigurationBL.InsertMarksGradesConfiguration();
               string sIsConfig = ReadQuerystring();
                if (sIsConfig !=Constants.S_YES)
                    SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.MarksGrade));
                

                divLblMsg.Visible = true;
                LblMsg.Text = Resources.LocalizedResources.PercentageGradesSavedSuccessfully;
            }
            else
            {
                string sStdName = ddlStandards.SelectedItem.Text;
                if (sErrMsg.Equals(string.Empty))
                {
                    oMarksGradesConfigurationBL.Marks_Grades_Configuration_Id = Convert.ToInt32(hidMarkGradeConfigId.Value);
                    oMarksGradesConfigurationBL.UpdateMarksGradeConfigurationDetails();
                    divLblMsg.Visible = true;
                    LblMsg.Text = Resources.LocalizedResources.PercentageGradesSavedSuccessfully;
                }
                else
                {
                    lblErrors.Visible = true;
                    lblErrors.Text = sErrMsg;
                }

            }

            FillMarkGradeGridView();

        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This for Button cancel
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
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This for Standard Combox Selected Index Change
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlStandards_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillMarkGradeGridView();
            divLblMsg.Visible = false;
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This is for radioButton changed event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optSubjects_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FillMarkGradeGridView();
            divLblMsg.Visible = false;

        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

 #endregion

    #region Private methods
   
   /// <summary>
   /// This method use read query string and show grid according to configuration
   /// </summary>
    private void ReadQuerystringForGrade()
    {
        if (QueryString.Count > 0)
        {
            int iStandardId = QueryString["Standard_Id"].ToInt();
            if (QueryString.Count == 2)
            {
                if (QueryString["IsCocuricularConfigure"].ToString() == "N")
                {
                    optCocurricular.Checked = true;
                    optSubjects.Checked = false;
                }
                else if (QueryString["IsCocuricularConfigure"].ToString()=="Y")
                {
                    optCocurricular.Checked = false;
                    optSubjects.Checked = true;
                }
            }
            if (iStandardId != 0)
            {
                ddlStandards.SelectedValue = iStandardId.ToString();
                ddlStandards_SelectedIndexChanged(ddlStandards, new EventArgs());
            }
          
        }

    } 
     
    /// <summary>
    /// This method is used to decrypt encrypted querystring.
    /// </summary>
    private string ReadQuerystring()
    {
        try
        {
            return QueryString["Is_Configured"];
        }
        catch (Exception)
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Constants.S_PAGE_ERROR);
        }

		return String.Empty;
    }

    /// <summary>
    /// This method for HideGridIfStdNotSelected
    /// </summary>
    private void HideGridIfStdNotSelected()
    {
        if (ddlStandards.SelectedValue == Constants.S_ZERO)
        {
            grdMarkGrades.Visible = false;
            imgBtnSave.Visible = false;
        }
        else
        {
            grdMarkGrades.Visible = true;
            grdMarkGrades.Enabled = true;
            imgBtnSave.Visible = true;
        }
    }

    /// <summary>
    /// This method for DisplayControls If Standards Are Configured
    /// </summary>
    private void DisplayControls()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable ODSStandards = oStandardCollectionBL.GetAssociatedStandards();
        if (ODSStandards.Rows.Count == 0)
        {
            // If no standard is configured then hide the controls.
            divError.Visible = true;
            lblError.Text = Resources.LocalizedResources.PleaseConfigureFollowingDetailsForSchool;
            btnCancel1.Text = "Back";
            divMain.Visible = false;
        }
        else
        {
            divMain.Visible = true;
            divError.Visible = false;
            ControlUtility.FillDropDownList(ODSStandards, ref ddlStandards, "standard_id", "standard_name", Constants.S_SELECT);
        }
    }

    /// <summary>
    /// This method for FillMarkGradeGridView
    /// </summary>
    /// <param name="abIsCoCurricularSubjects"></param>
  
    private void FillMarkGradeGridView()
    {
        // This method fills the Grid with available Group details.
        MarksGradesConfigurationBL oMarksGradesConfigurationBL = new MarksGradesConfigurationBL();
        oMarksGradesConfigurationBL.Academic_Year_Id = miAcademicYearId;
        oMarksGradesConfigurationBL.Inserted_By_Id = miUserId;
        oMarksGradesConfigurationBL.School_Id = miSchoolId;
        oMarksGradesConfigurationBL.Standard_Id = Convert.ToInt32(ddlStandards.SelectedValue);
        oMarksGradesConfigurationBL.IsCoCurricularSubjects = Convert.ToBoolean(optSubjects.Checked?false:true);
        DataSet oDsMarkGrades = oMarksGradesConfigurationBL.FetchMarksGradesConfigurationDetails();//abIsCoCurricularSubjects
        grdMarkGrades.DataSource = oDsMarkGrades.Tables[1].DefaultView;
        grdMarkGrades.DataBind();
        hidConfigDependancy.Value = oDsMarkGrades.Tables[2].Rows.Count > 0 && oDsMarkGrades.Tables[2].Rows[0]["ExamConfigurationDone"].ToBool() ? Constants.S_YES : Constants.S_NO;
        if (oDsMarkGrades.Tables[0].Rows.Count > 0)
        {
            hidMode.Value = Constants.S_EDIT_MODE;
            hidMarkGradeConfigId.Value = Convert.ToString(oDsMarkGrades.Tables[0].Rows[0]["Marks_Grades_Configuration_Id"]);
        }
        else
            hidMode.Value = Constants.S_NEW_MODE;
        

        HideGridIfStdNotSelected();
    }

    /// <summary>
    /// This method for GetMarkGradesXML
    /// </summary>
    /// <returns></returns>
    private string GetMarkGradesXML()
    {
        CheckBox oChkSelected; 
        TextBox oTxtBox;
        const string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();

        // Create a root level element.
        XmlElement oRoot = oDoc.CreateElement("MarksGradesConfigurationDetails");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "MarksGradesConfigurationDetails", string.Empty);

        for (int iRowIndex = 0; iRowIndex < grdMarkGrades.Rows.Count; iRowIndex++)
        {
            oChkSelected = (CheckBox)grdMarkGrades.Rows[iRowIndex].FindControl("ChkBoxDelete");
            if (oChkSelected.Checked)
            {
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "MarksGradesConfigurationDetail", string.Empty);
                string sAtrrName = "Starting_Marks_Range";
                XmlAttribute oAttr = oDoc.CreateAttribute(sAtrrName);
                oTxtBox = (TextBox)grdMarkGrades.Rows[iRowIndex].Cells[I_STARTING_MARKS_COLUMN_NUMBER].FindControl("txtStartingMarks");
                oAttr.Value = oTxtBox.Text;
                oXmlNode.Attributes.Append(oAttr);

                sAtrrName = "Ending_Marks_Range";
                oAttr = oDoc.CreateAttribute(sAtrrName);
                oTxtBox = (TextBox)grdMarkGrades.Rows[iRowIndex].Cells[I_ENDING_MARKS_COLUMN_NUMBER].FindControl("txtEndingMarks");
                oAttr.Value = oTxtBox.Text;
                oXmlNode.Attributes.Append(oAttr);

                sAtrrName = "Actual_Ending_Marks_Range";
                oAttr = oDoc.CreateAttribute(sAtrrName);

                if (Convert.ToInt32(oTxtBox.Text) == 100)
                    oAttr.Value = oTxtBox.Text;
                else
                    oAttr.Value = oTxtBox.Text + ".99";

                oXmlNode.Attributes.Append(oAttr);

                sAtrrName = "Grade_Name";
                oAttr = oDoc.CreateAttribute(sAtrrName);
                oTxtBox = (TextBox)grdMarkGrades.Rows[iRowIndex].Cells[I_GRADE_NAME_COLUMN_NUMBER].FindControl("txtGradeName");
                oAttr.Value = oTxtBox.Text;
                oXmlNode.Attributes.Append(oAttr);

                sAtrrName = "Remarks";
                oAttr = oDoc.CreateAttribute(sAtrrName);
                oTxtBox = (TextBox)grdMarkGrades.Rows[iRowIndex].Cells[I_REMARKS_COLUMN_NUMBER].FindControl("txtRemarks");
                oAttr.Value = oTxtBox.Text;
                oXmlNode.Attributes.Append(oAttr);

                sAtrrName = "Original_Config_Id";
                oAttr = oDoc.CreateAttribute(sAtrrName);
                oAttr.Value = grdMarkGrades.DataKeys[iRowIndex][I_DATAKEY_ORIGINAL_CONFIG_ID].ToString();
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
    /// This method to set javascript attributes
    /// </summary>
    private void SetJavascriptAttributes()
    {
        optSubjects.Attributes["onclick"] = "javascript:ClearErrorLabels()";
        optCocurricular.Attributes["onclick"] = "javascript:ClearErrorLabels()";
        btnCancel.Attributes["onclick"] = "javascript:DisableButtons()";
        ApplyMouseHoverEffect(new List<Button> { btnCancel, btnCancel1, imgBtnSave });
        imgBtnSave.Attributes.Add("Onclick", "if(!(ValidateInput('" + grdMarkGrades.AllowPaging + "','" + Resources.LocalizedResources.AtLeastOneRangeShouldBeSelected +"') && CheckExamConfiguration())){return false;}");
    }
    /// <summary>
    /// This method is used to set design according to selected language.
    /// </summary>
    private void DesignSettingAccordingLanguage()
    {
        LblMsg.Text = Resources.LocalizedResources.PercentageGradesSavedSuccessfully;
        hidPleaseFixFollowingErrors.Value = Resources.LocalizedResources.PleaseFixFollowingError;
        hidStartingPercentageEndingPercentageShouldNotBeSame.Value = Resources.LocalizedResources.StartingPercentageEndingPercentageShouldNotBeSame;
        hidDuplicatePercentageRangeAreNotAllowed.Value = Resources.LocalizedResources.DuplicatePercentageRangesAreNotAllowed;
        hidStartingPercentageOverlap.Value = Resources.LocalizedResources.StartingPercentageOverlap;
        hidEndingPercentageOverlap.Value = Resources.LocalizedResources.EndingPercentageOverlap;
        hidSomeOfTheStartingEndingPerRangeAreMissing.Value = Resources.LocalizedResources.SomeOfTheStartingEndingPerRangeMissing;
        hidMinimumStartingPerShouldBe.Value = Resources.LocalizedResources.MinimumStartingPerShoulBe;
        hidMaximumEndingPerShouldBe.Value = Resources.LocalizedResources.MaximumEndingPerShouldBe;
        hidRowNumber.Value = Resources.LocalizedResources.RowNumber;
        hidStartingPerForFollowingRowsShouldNotBeBlank.Value = Resources.LocalizedResources.StartingPerForFollowingRowsShouldNotBeBlank;
        hidEndingPerForFollowingRowsShouldNotBeBlank.Value = Resources.LocalizedResources.EndingPerForFollowingRowsShouldNotBeBlank;
        hidGradeNameForFollowingRowsShouldNotBeBlank.Value = Resources.LocalizedResources.GradeNameForFollowingRowsShouldNotBeBlank;
        hidStartingPerForFollowingRowsShouldBeLessThanEndingPer.Value = Resources.LocalizedResources.StartingPerForFollowingRowsShouldBeLessThanEndingPer;
        hidRemarksForFollowingRowsShouldNotBeBlank.Value = Resources.LocalizedResources.RemarksForFollowingRowsShouldNotBeBlank;
        hidDuplicateGradeNameIsNotAllowed.Value = Resources.LocalizedResources.DuplicateGradeNameIsNotAllowed;
        hidPercentageGradeForStandard.Value = Resources.LocalizedResources.PercentageGradeForStandard;
        hidCanNotBeModifiedAsExamConfigurationAlreadyDone.Value = Resources.LocalizedResources.CanNotBeModifiedAsExamConfigurationAlreadyDone;
    }

    #endregion
}
