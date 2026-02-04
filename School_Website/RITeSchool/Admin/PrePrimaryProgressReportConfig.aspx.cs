// File Name  : PrePrimaryProgressReportConfig.aspx.cs
// Created By : Shankar
// Date       : 22/10/2007   
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using BusinessLogic.Exceptions;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Text;
using BusinessLogic;
using Utility;
using System.Xml;

/// <summary>
/// This Class is used to add and edit holiday management configuration.
/// </summary>
public partial class PrePrimaryProgressReportConfig : SchoolBase
{

    #region Constants
    private string S_ADD_DEVELOPMENT_AREA = "Add Development Area";
    private string S_ADD_SKILL = "Add Skill"; 
    private string S_EDIT_DEVELOPMENT = "Edit Development Area";
    private string S_EDIT_SKILL = "Edit Skill";
    private string S_DEVELOPMENT_AREA = "Development Area :";
    private string S_DEVELOPMENT_AREA_NOT_BLANK = "Development Area should not be blank.";
    private string S_SKILL = "Skill :";
    private string S_SKILL_SHOULD_NOT_BLANK = "Skill should not be blank.";
	private string S_SAVE_DEVELOPMENT_AREA = "Development area saved sucessfully !!!";
	private string S_UPDATE_DEVELOPMENT_AREA = "Development area updated sucessfully !!!";
	private string S_SAVE_SKILL = "Skill saved sucessfully !!!";
	private string S_UPDATE_SKILL ="Skill updated sucessfully !!!";
    #endregion
    #region Event

    /// <summary>
    /// This method is used to decrypt query string and fill grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                Initialise();
                FillDetailsAccordingToMode();
                FillTestCombobox();
            }
            SetHeaderAccordingToMode();
            lblmdtCmb.Style.Add("visibility", "hidden");
            chkIsDescription.Attributes.Add("onclick", "Enable(this,'" + cmbTests.ClientID + "','" + lblmdtCmb.ClientID + "')");            
            ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel});
            btnCancel.Attributes.Add("onclick", "CloseWindow();");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to save,update data & transfer control to HolidaysManagementConfiguration page on Sucess.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Int32 iInsertOrUpdateFlag = 0;
        lblErrorMsg.Text = " ";
        string sXmlNotApplicableExam = GetXmlForNotApplicableExam();
        try
        {
            if (IsCommentForApplicableExam())
            {
                PrePrimaryProgressSheetConfigBL oPrePrimaryProgressSheetConfigBL = SetAllFieldToHolidayMaster();
                string sReturn = ReferenceBL.CheckDependenciesAndGetErrorMessages(Convert.ToInt32(Constants.ReferenceId.PrePrimaryProgrssSheetConf), Convert.ToInt32(hidStandardId.Value), "", miAcademicYearId);
                if (sReturn.Equals(""))
                {
                    // To insert  data into Holidays_Master.            
                    if (hidActionFlag.Value == Convert.ToString(Constants.I_ZERO))
                    {
                        oPrePrimaryProgressSheetConfigBL.InsertPrePrimaryProgressSheetConfig(sXmlNotApplicableExam);
                        iInsertOrUpdateFlag = 1;
						ShowSucessfullMessage();
                    }
                    else  //To Update data into Holidays_Master
                    {
                        oPrePrimaryProgressSheetConfigBL.UpdatePrePrimaryProgressSheetConfig(sXmlNotApplicableExam);
                        iInsertOrUpdateFlag = 1;
						ShowSucessfullMessage();
                    }
                }
                else
                {
                    throw new BusinessLogic.Exceptions.ReferenceExceptions(sReturn);
                }
                if (iInsertOrUpdateFlag == 1)
                {
                    if (hidIsConfig.Value != "Y")
                    {
                        SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.PrePrimaryProgrssSheetConf));
                    }

                }
                txtNameofHoliday.Text = "";
            }
            else
                lblErrorMsg.Text = "You cannot add the comment for non applicable exam : "+ cmbTests.SelectedItem.Text + ".";
            
        }
        catch (BusinessLogic.Exceptions.ReferenceExceptions ex)
        {  
            lblErrorMsg.Text = ex.Message;
        }
        catch (BusinessLogic.Exceptions.RecordAlreadyExists ex)
        {
            if (HidParentHeadingId.Value == "0")
            {   
			     lblUpdate.Text="";
                lblErrorMsg.Text = "Development Area " + ex.Message + ".";
            }
            else
            {
			    lblUpdate.Text="";
                lblErrorMsg.Text = "Skill " + ex.Message + ".";
            }
        }

        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    private bool IsCommentForApplicableExam()
    {
        if (chkIsDescription.Checked)
        {
            int iExamID = Convert.ToInt32(cmbTests.SelectedValue);

            for (int iCnt = 0; iCnt < chklstExam.Items.Count; iCnt++)
            {
                if(!chklstExam.Items[iCnt].Selected && chklstExam.Items[iCnt].Value == cmbTests.SelectedValue)
                    return false;                    
            }
        }
        return true;
    }

    private string GetXmlForNotApplicableExam()
    {
        const string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("ExamDetails");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "ExamDetails", "");

        // Loop through all the list view items.
        for (int iCount = 0; iCount < chklstExam.Items.Count; iCount++)
        {
            if (!(chklstExam.Items[iCount].Selected))
            {
                // Create root xml element.
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "ExamDetails", "");

                string sAtrrName = "TestID";
                XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = (chklstExam.Items[iCount].Value).ToString();
                oXmlNode.Attributes.Append(attr);
                // Add the node to root node.
                oXmlRootNode.AppendChild(oXmlNode);
            }
        }
        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);
        // return the string generated.
        return root.InnerXml;
    }

    protected void chkIsDescription_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkIsDescription.Checked)
            {
                cmbTests.SelectedIndex = 0;
                cmbTests.Enabled = true;
            }
            else
            {
                cmbTests.SelectedIndex = 0;
                cmbTests.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method

    /// <summary>
    /// This method initialises variables.
    /// </summary>
    private void Initialise()
    {
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        btnSave.Attributes.Add("onclick", "ClearErrorLabel()");
        btnCancel.Attributes.Add("onclick", "if(!(closewindow())){return false};");
    }

    /// <summary>
    /// This method is used to set all fields of HolidayMaster.
    /// </summary>
    private PrePrimaryProgressSheetConfigBL SetAllFieldToHolidayMaster()
    {
        PrePrimaryProgressSheetConfigBL oPrePrimaryProgressSheetConfigBL = new PrePrimaryProgressSheetConfigBL();
        oPrePrimaryProgressSheetConfigBL.school_Id = miSchoolId;
        oPrePrimaryProgressSheetConfigBL.Academic_Year_Id = miAcademicYearId;
        oPrePrimaryProgressSheetConfigBL.Heading_Id = Convert.ToInt32(hidHeaderId.Value);
        oPrePrimaryProgressSheetConfigBL.Heading_Parent_Id = Convert.ToInt32(HidParentHeadingId.Value);
        oPrePrimaryProgressSheetConfigBL.standard_Id = Convert.ToInt32(hidStandardId.Value);
        oPrePrimaryProgressSheetConfigBL.Heading_Text = txtNameofHoliday.Text;
        oPrePrimaryProgressSheetConfigBL.Update_Date = System.DateTime.Now;
        oPrePrimaryProgressSheetConfigBL.Inserted_By_id =Convert.ToString(miUserId);
        oPrePrimaryProgressSheetConfigBL.Updated_By_Id = Convert.ToString(miUserId);

        if (chkIsDescription.Checked)
        {
            oPrePrimaryProgressSheetConfigBL.Is_Description = Constants.C_YES.ToString();
            oPrePrimaryProgressSheetConfigBL.Description_For_Test_Id = Convert.ToInt32(cmbTests.SelectedValue);
        }
        else
        {
            oPrePrimaryProgressSheetConfigBL.Is_Description = Constants.C_NO.ToString();
        }

        return oPrePrimaryProgressSheetConfigBL;
    }

    /// <summary>
    /// This method is used to decrypt querystring.
    /// </summary>
    private void ReadQueryString()
    {
       if (QueryString["StandardId"] != null)
         hidStandardId.Value = QueryString["StandardId"];
       
	   if (QueryString["HeaderId"] != null)
         hidHeaderId.Value = QueryString["HeaderId"];
       
	   if (QueryString["IsConfig"] != null)
         hidIsConfig.Value = QueryString["IsConfig"];
       
	   if (QueryString["ParentHeading_Id"] != null)
         HidParentHeadingId.Value = QueryString["ParentHeading_Id"];
       
	   if (QueryString["ParentHeading_Id"] != null && QueryString["Mode"] == "SubHeader")
         {
           trCheckBox.Visible = false;
           trTest.Visible = false;
           trApplicableExam.Visible = false;
         }
       hidUrl.Value = "PrePrimaryProgressReportConfigList.aspx?" + Request.QueryString.ToString();
    }

    /// <summary>
    /// This method  retrives the data for selected holiday.
    /// And sets the form fields accordingly.
    /// </summary>
    private void FillHeaderData()
    {
        Int32 iHeaderID = Convert.ToInt32(hidHeaderId.Value);
        PrePrimaryProgressSheetConfigBL oPrePrimaryProgressSheetConfigBL = new PrePrimaryProgressSheetConfigBL(iHeaderID);
        txtNameofHoliday.Text = oPrePrimaryProgressSheetConfigBL.Heading_Text;
        if (oPrePrimaryProgressSheetConfigBL.Is_Description == Constants.C_YES.ToString())
        {
            chkIsDescription.Checked = true;
            cmbTests.SelectedValue = oPrePrimaryProgressSheetConfigBL.Description_For_Test_Id.ToString();
            cmbTests.Enabled = true;
        }
        else
            chkIsDescription.Checked = false;
    }

    /// <summary>
    /// This method decides the display mode (add or edit).
    /// and sets the form fields accordingly.
    /// </summary>
    private void FillDetailsAccordingToMode()
    {
        ReadQueryString();
        if (hidHeaderId.Value != "0")
        {
            FillHeaderData();
            hidActionFlag.Value = Convert.ToString(Constants.I_ONE);
        }
        else
        {
            SetNewModeHeaderInformation();
        }
    }

    /// <summary>
    /// This method sets the form fields for new mode.
    /// It sets 
    /// 1. default start and end date (as current date).
    /// 2. default value for total days = 1.
    /// 3. and hidden variable for mode to zero.
    /// </summary>
    private void SetNewModeHeaderInformation()
    {
        hidHeaderId.Value = Convert.ToString(Constants.I_ZERO);
        hidActionFlag.Value = Convert.ToString(Constants.I_ZERO);
    }

    /// <summary>
    /// This method fills the combobox for the tests.
    /// </summary>
    private void FillTestCombobox()
    {
        if (CheckPreCondition())
        {
            int iHeaderId = Convert.ToInt32(hidHeaderId.Value);
            TestCollectionBL oTestCollectionBL = new TestCollectionBL(miSchoolId, miAcademicYearId);

            DataTable oDSAllTests = oTestCollectionBL.GetAllTestsForStandard(Convert.ToInt32(hidStandardId.Value), iHeaderId);
            ControlUtility.FillDropDownList(oDSAllTests, ref cmbTests,
                                           Constants.S_TEST_ID_FIELD,
                                           Constants.S_TEST_NAME_FIELD,
                                           Constants.S_SELECT);
            ControlUtility.FillCheckBoxList(oDSAllTests, ref chklstExam,
                                           Constants.S_TEST_ID_FIELD,
                                           Constants.S_TEST_NAME_FIELD,
                                           true);

            for (int iCount = 0; iCount < oDSAllTests.Rows.Count; iCount++)
            {
                if (Convert.ToBoolean(oDSAllTests.Rows[iCount]["IsApplicable"]))
                    chklstExam.Items[iCount].Selected = false;
            }
        }

    }

    /// <summary>
    /// This function checks the preconditons of Exams.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = true;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.TestNames);
        return bReturn;
    }

    /// <summary>
    ///  this method is used to set Header according to mode
    /// </summary>
    private void SetHeaderAccordingToMode()
    {
        if (hidHeaderId.Value != Constants.S_ZERO)
        {
            if (HidParentHeadingId.Value == Constants.S_ZERO)
            {
                lblHeader.Text = S_EDIT_DEVELOPMENT;
                lblHolidayStartDate.Text = S_DEVELOPMENT_AREA;
                reqdHeaderText.ErrorMessage = S_DEVELOPMENT_AREA_NOT_BLANK;
            }
            else
            {
                lblHeader.Text = S_EDIT_SKILL;
                lblHolidayStartDate.Text = S_SKILL;
                reqdHeaderText.ErrorMessage = S_SKILL_SHOULD_NOT_BLANK;
				
            }
            btnSave.Text = "Update";

        }
        else
        {
            if (HidParentHeadingId.Value == Constants.S_ZERO)
            {
                lblHeader.Text = S_ADD_DEVELOPMENT_AREA;
                lblHolidayStartDate.Text = S_DEVELOPMENT_AREA;
                reqdHeaderText.ErrorMessage = S_DEVELOPMENT_AREA_NOT_BLANK;
				lblUpdate.Text="";
            }
            else
            {
                lblHeader.Text = S_ADD_SKILL;
                lblHolidayStartDate.Text = S_SKILL;
                reqdHeaderText.ErrorMessage = S_SKILL_SHOULD_NOT_BLANK;
            }
        }
    }

	/// <summary>
	/// This method is used to show Sucessfull Message
	/// </summary>
	private void ShowSucessfullMessage()
	{
	   if (hidHeaderId.Value != Constants.S_ZERO)
        {
		if (HidParentHeadingId.Value == Constants.S_ZERO)
			lblUpdate.Text = S_UPDATE_DEVELOPMENT_AREA;
			else
		    lblUpdate.Text = S_UPDATE_SKILL;
		}
		else
		{
		 if (HidParentHeadingId.Value == Constants.S_ZERO)
			 lblUpdate.Text = S_SAVE_DEVELOPMENT_AREA;
			 else
		     lblUpdate.Text = S_SAVE_SKILL;
		}
	}
    #endregion
}

