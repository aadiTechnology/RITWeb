/* File Name :- AssignClassTeacherForDivisionPopUp.aspx
 * Modified By :- Sachin
 * Modified Date :- 18-Sept-2009
 * Purpose :- Code Review.
 * Class Description :- This class is used to manipulate class teacher assignment.
*/
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Script.Serialization;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using XseedReportEntities;

public partial class AssignClassTeacherForDivisionPopUp : SchoolBase
{   
    #region Data Members
    
   private SchoolWiseStandardDivisionTeacherAssignmentMasterBL moAssignClassTreacher;

    #endregion

    #region Events

    /// <summary>
    /// This event is used to display standard, division and fill teacher comboboxes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            InitializeMemberVariables();
            if (!IsPostBack)
            {
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                ReadQuerystring();
                RefreshValues();
                DisplayStandardAndDivisionName();
                SetDisplayMenbers();
                FillTeacherCombobox();                
                SetJavascriptAttributes();
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValues();
            }
            cmbTeacherName.Focus();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

	/// <summary>
	/// This event is used to 
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Write("<Script language='Javascript'>window.opener.location.reload(true); window.close();window.opener.focus(); </Script>");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This  event is used to save class teacher assignment.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string sErrorMsg = string.Empty;
            int iTeacherId = Convert.ToInt32(cmbTeacherName.SelectedValue);
            moAssignClassTreacher = GetObjectForAssignClassTreacher();
            moAssignClassTreacher.TeacherId = iTeacherId;

            // Selected Value of will be 0 only in case of "Remove assignment".
			if (cmbTeacherName.SelectedValue == "0")
				sErrorMsg = DeleteTeacherAssignement();
			else
				InsertOrUpdateClassTeacherDetails();
            if (string.IsNullOrEmpty(sErrorMsg))
            {
                if (chkAddTeacher.Checked)
                {
                    iTeacherId = Convert.ToInt32(cmbAddTeacherName.SelectedValue);
                    moAssignClassTreacher = GetObjectForAssignClassTreacher();
                    moAssignClassTreacher.TeacherId = iTeacherId;

                    // Selected Value of will be 0 only in case of "Remove assignment".
                    if (cmbAddTeacherName.SelectedValue == "0")
                        DeleteAddedTeacherAssignement();
                    else
                        InsertOrUpdateAddClassTeacherDetails();
                }
            }

            if (hidIsConfig.Value != "Y")
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.ClassTeacher));
                if (sErrorMsg.Equals(string.Empty))
                    Response.Write("<Script language='Javascript'>window.opener.location.reload(true); window.close();window.opener.focus(); </Script>");
                else
                {
                    lblError.Visible = true;
                    lblError.Text = sErrorMsg;
                    SetAssignedTeachers();
                }
            }
       
        catch (BusinessLogic.Exceptions.ReferenceExceptions ex)
        {
            lblErr.Text = ex.Message;
            FillTeacherCombobox();
            cmbTeacherName.Focus();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Methods 

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        chkAddTeacher.Attributes["onclick"] = "javascript:DisableAdditionalCombo();";        
        btnSave.Attributes["onclick"] = "javascript:DisableButtons()";        
        ApplyMouseHoverEffect(new List<System.Web.UI.WebControls.Button> { btnSave, btnClose });
        btnSave.Attributes["onclick"] = "ResetUpdateLbl()";
    }

    /// <summary>
    /// This method is used to set display member of teacher combobox.
    /// </summary>
    private void SetDisplayMenbers()
    {
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        if (hidTeacherId.Value != Constants.S_EMPTY_STRING)
        {
            hidDisplayMember.Value = Constants.S_NOT_ASSIGN;
            if (hidTeacherId.Value.Split(',').Length == 2)
                hidAddDisplayMember.Value = Constants.S_NOT_ASSIGN;
            else
                hidAddDisplayMember.Value = Constants.S_SELECT;
        }
        else
        {
            hidDisplayMember.Value = Constants.S_SELECT;
            hidAddDisplayMember.Value = Constants.S_SELECT;
        }
    }

    /// <summary>
    /// This method is used to initialize SchoolWiseStandardDivisionTeacherAssignmentMasterBL object.
    /// </summary>
    /// <returns></returns>
    private SchoolWiseStandardDivisionTeacherAssignmentMasterBL GetObjectForAssignClassTreacher()
    {
        SchoolWiseStandardDivisionTeacherAssignmentMasterBL oAssignClassTreacher = new SchoolWiseStandardDivisionTeacherAssignmentMasterBL();         
        oAssignClassTreacher.SchoolId = miSchoolId; 
        oAssignClassTreacher.AcademicYearId = miAcademicYearId;
        oAssignClassTreacher.StandardId = Convert.ToInt32(hidStandardId.Value);
        oAssignClassTreacher.DivisionId = Convert.ToInt32(hidDivisionId.Value);
        oAssignClassTreacher.IsClassTeacher = Constants.C_YES;
        oAssignClassTreacher.InsertedByid = miUserId;
        oAssignClassTreacher.UpdatedById = miUserId;
        return oAssignClassTreacher;
    }

    /// <summary>
    /// This method is used to display standard and division.
    /// </summary>
    private void DisplayStandardAndDivisionName()
    {
        int iStandardId = Convert.ToInt32(hidStandardId.Value);
        int iDivisionId = Convert.ToInt32(hidDivisionId.Value);
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        lblStandardName.Text = oMasterDataCollectionBL.GetStandardName(miSchoolId, iStandardId);
        lblDivisionName.Text = oMasterDataCollectionBL.GetClassName(miSchoolId, iStandardId, iDivisionId);
    }

    /// <summary>
    /// This method is used to fill teacher combobox.
    /// </summary>
    private void FillTeacherCombobox()
    {
        int iStandardId = Convert.ToInt32(hidStandardId.Value);
	    MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
		List<ClassTeacher> lstTeachers = oMasterDataCollectionBL.FillTeacherNameComboBox(miSchoolId,
	                                                                                            miAcademicYearId,
	                                                                                            iStandardId);

		ListSource.FillDropDownList(lstTeachers, cmbTeacherName, Constants.S_TEACHER_NAME_FIELD, "TeacherId", hidDisplayMember.Value);
		ListSource.FillDropDownList(lstTeachers, cmbAddTeacherName, Constants.S_TEACHER_NAME_FIELD, "TeacherId", hidAddDisplayMember.Value);
		List<ClassTeacher> lstClassTeachers = new List<ClassTeacher>();
		for (int iTeacherIndex = 0; iTeacherIndex < lstTeachers.Count; iTeacherIndex++)
		{
			if (lstTeachers[iTeacherIndex].IsClassTeacher)
			{
				// set background color to dropdown list if teacher is already a class teacher.
				cmbAddTeacherName.Items[iTeacherIndex + 1].Attributes.Add("style", "background-color:#FEEABA;");
				cmbTeacherName.Items[iTeacherIndex + 1].Attributes.Add("style", "background-color:#FEEABA;");
				lstClassTeachers.Add(lstTeachers[iTeacherIndex]);
			}
		}

	    var jsSerializer = new JavaScriptSerializer();
		hidClassTeacherJsonObject.Value = jsSerializer.Serialize(lstClassTeachers);
		SetAssignedTeachers();        
    }

    /// <summary>
    /// This method is used to set assigned teacher.
    /// </summary>
    private void SetAssignedTeachers()
    {
        // Teacher ids will be stored in comma separated foramt.
        if (hidTeacherId.Value != Constants.S_EMPTY_STRING)
        {
            string[] sArrTeachers = hidTeacherId.Value.Split(',');
            cmp_TeacherName.Visible = false;
            lblMandatory.Visible = false;
            cmbTeacherName.SelectedValue = sArrTeachers[0];
            hidTeacherId.Value = sArrTeachers[0];
            hidTeacherName.Value = cmbTeacherName.SelectedItem.Text;
           
            if (sArrTeachers.Length == 2)
            { 
                chkAddTeacher.Checked = true;
                chkAddTeacher.Enabled = false;              
                cmbAddTeacherName.Enabled = true;
                cmbAddTeacherName.SelectedValue = sArrTeachers[1];
                hidAddTeacherId.Value = sArrTeachers[1];
                hidAddTeacherName.Value = cmbAddTeacherName.SelectedItem.Text;
            }
        }
    }

    /// <summary>
    /// This method is used to decrypt querystring.
    /// </summary>
    private void ReadQuerystring()
    {
	    if (Request.QueryString.ToString() == Constants.S_EMPTY_STRING)
		    return;
	    
		if (QueryString["StandardId"] != null)
		    hidStandardId.Value = QueryString["StandardId"];
            
	    if (QueryString["DivisionId"] != null)
		    hidDivisionId.Value = QueryString["DivisionId"];
            
	    if (QueryString["TeacherId"] != null)
		    hidTeacherId.Value = QueryString["TeacherId"];
            
	    if (QueryString["Is_Configured"] != null)
		    hidIsConfig.Value = QueryString["Is_Configured"];
    }

	/// <summary>
    /// This method is used to remove teacher assignment.
    /// </summary>
    private string  DeleteTeacherAssignement()
    {
        string sErrorMsg = string.Empty;
        if (hidTeacherId.Value != Constants.S_EMPTY_STRING && chkAddTeacher.Checked == false)
        {
            moAssignClassTreacher.OrgTeacherId = Convert.ToInt32(hidTeacherId.Value);
            moAssignClassTreacher.DeleteAssignTeacherForStandardDivision(hidTeacherName.Value);
        }
        else
        {
            sErrorMsg = Resources.LocalizedResources.Teacher + " " + hidTeacherName.Value + " " + Resources.LocalizedResources.ErrTeacherAdditional;
        }

        return sErrorMsg;
    }

    /// <summary>
    /// This method is used to delete additional teacher assignment assignment.
    /// </summary>
    private void DeleteAddedTeacherAssignement()
    {
        if (hidAddTeacherId.Value != Constants.S_EMPTY_STRING)
        {
            moAssignClassTreacher.OrgTeacherId = Convert.ToInt32(hidAddTeacherId.Value);
            moAssignClassTreacher.DeleteAssignTeacherForStandardDivision(hidAddTeacherName.Value);
        }
    }

    /// <summary>
    /// This method is used to isert/update class teacher assignment details.
    /// </summary>
    private void InsertOrUpdateClassTeacherDetails()
    {
        if (string.IsNullOrEmpty(hidTeacherId.Value))
            moAssignClassTreacher.InsertSchoolWiseStandardDivisionSubjectTeacherAssignmentMaster();
        else if (!hidTeacherId.Value.Equals(cmbTeacherName.SelectedValue))
        {
            moAssignClassTreacher.OrgTeacherId = Convert.ToInt32(hidTeacherId.Value);
            moAssignClassTreacher.UpdateTeacherDetailsForStandardDivision(hidTeacherName.Value);
        }
    }

    /// <summary>
    /// This method is used to insert/update additional teacher details.
    /// </summary>
    private void InsertOrUpdateAddClassTeacherDetails()
    {
        if (string.IsNullOrEmpty(hidAddTeacherId.Value))
            moAssignClassTreacher.InsertSchoolWiseStandardDivisionSubjectTeacherAssignmentMaster();
        else if (!hidAddTeacherId.Value.Equals(cmbAddTeacherName.SelectedValue))
        {
            moAssignClassTreacher.OrgTeacherId = Convert.ToInt32(hidAddTeacherId.Value);
            moAssignClassTreacher.UpdateTeacherDetailsForStandardDivision(hidAddTeacherName.Value);
        }
    }

    /// <summary>
    /// This Method used to change value of messgae according to culture
    /// </summary>
    private void RefreshValues()
    {
        hidvalTeachersBoth.Value = Resources.LocalizedResources.valTeachersBoth;
        hidValTeacherAdditional.Value = Resources.LocalizedResources.ValTeacherAdditional;
        hidValTeacherOf.Value = Resources.LocalizedResources.ValTeacherOf;
        hidAreYouSureYouWantToContinue.Value = Resources.LocalizedResources.AreYouSureYouWantToContinue;
    }
    
    #endregion
}