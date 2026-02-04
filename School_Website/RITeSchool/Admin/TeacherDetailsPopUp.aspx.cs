/* File Name :- TeacherDetailsPopUp.aspx.cs
 * Modified By :- Sachin
 * Modified Date :- 22-Sept-2009
 * Purpose :- Code Review.
 * Class Description :- This class is used to display teachers all details in single view.
 * Modified By : Rohini
 * Date : 20 Jan 2012
 * Description: Removed Joining date.
*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

/// <summary>
/// This method is used to display teacher details.
/// </summary>
public partial class TeacherDetailsPopUp : SchoolBase
{
    #region Constants

    private const int I_TBL_TEACHER_INDEX = 0;

    #endregion

    #region Data Members

    private int miTeacherId;    
    private DataSet moDsTeacherInfo;
    private int Gardepay;

    #endregion

    #region Events

    /// <summary>
    /// This event is used for following purposes :-
    /// 1. Fetch teacher details.
    /// 2. Display teacher personal details.
    /// 3. Display teacher education details.
    /// 4. Display standard as well as subject details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                FetchTeacherDetails();
                DisplayTeacherPersonalDetails();
                DisplayTeacherEducationDetails();
                DisplayTeacherExperienceDetails();
                DisplaySubjectDetails();
                DisplayStandardDetails();
                DisplayEmployeeDetails();
                SetJavascriptAttributes();
            }
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
   ///</summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnClose });
    }

    /// <summary>
    /// This method is used to fetch teacher details.
    /// </summary>
    private void FetchTeacherDetails()
    {
        miTeacherId = QueryString["TeacherId"].ToInt();
        miUserId = QueryString["UserId"].ToInt();
        
        moDsTeacherInfo = SchoolWiseTeacherMasterBL.FetchAllTeacherDetails(miTeacherId, miAcademicYearId, miSchoolId, miUserId);
    }

    /// <summary>
    /// This method is used to display teacher's personal details.
    /// </summary>
    private void DisplayTeacherPersonalDetails()
    {
        DisplayTeacherName();
        DisplayPhoneNumbers();
        TeacherAddressDetails();
        TeacherSchoolRelatedInfo();
        DisplayEmailAddress();

        lblReligion.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Religion_Name"]);
        lblCasteSubCaste.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Caste_SubCaste"]);
        lblCategory.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Category_Name"]);
        lblNationality.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Nationality"]);

        string sIsTemporary = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Is_Temporary"]);
        lblServiceType.Text = sIsTemporary == Constants.I_ONE.ToString() ? "Permanent"
            :(sIsTemporary == Constants.I_TWO.ToString() ? "Temporary" : (sIsTemporary == Constants.I_THREE.ToString() ? "Probation" : "-"));
        lblDesignation.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Teacher_Designation_Name"]);
        DateTime dtDateOfBirth = Convert.ToDateTime(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Date_of_Birth"]);
        lblDateofBirth.Text = dtDateOfBirth.ToString(Constants.S_STANDARD_DATE_FORMAT);
        DateTime dtRetirement = Convert.ToDateTime(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Date_of_Retirement"]);
        lblDateofRetirement.Text = dtRetirement.ToString(Constants.S_STANDARD_DATE_FORMAT);
        
        if (Settings.ShowGradePayOnStaffProfileScreen)
        {
            tdGrade1.Visible = true;
            tdGrade2.Visible = true;
            lblGrade.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["GradePay"]);
        }       
    }

    /// <summary>
    /// This method is used to display Email address.
    /// </summary>
    private void DisplayEmailAddress()
    {
        SchoolUserBL oSchoolUserBL = new SchoolUserBL(miUserId);
        lblEmail.Text = oSchoolUserBL.Email;
    }

    /// <summary>
    /// This method is used to display teacher name.
    /// </summary>
    private void DisplayTeacherName()
    {
        string sMiddleName = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Teacher_Middle_Name"]);
        if (sMiddleName != Constants.S_EMPTY_STRING)
            lblTeacherName.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Salutation_Name"]) + " "
                                + Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Teacher_First_Name"]) + " "
                                + sMiddleName + (sMiddleName.Length > 1?" " : ".")
                                + Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Teacher_Last_Name"]);
        else
            lblTeacherName.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Salutation_Name"]) + " "
                                + Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Teacher_First_Name"]) + " "
                                + Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Teacher_Last_Name"]);
    }

    /// <summary>
    /// This method is used to display phone number.
    /// </summary>
    private void DisplayPhoneNumbers()
    {
        lblPhoneNumber.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Phone_Number"]);
        lblResultMobileNumber.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Mobile_Number"]);
    }

    /// <summary>
    /// This method is used to display address.
    /// </summary>
    private void TeacherAddressDetails()
    {
        lblLocalAddress.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Local_Address"]);
        lblLocalCity.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Local_City"]);
        lblLocalState.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Local_State"]);
        lblLocalPincode.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Local_Pincode"]);
        string sPermanantAddress = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Permanent_Address"]);
        string sPermanantCity = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Permanent_City"]);
        string sPermanentPIN = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Permanent_Pincode"]);
        if (sPermanantAddress == Constants.S_EMPTY_STRING &&
            sPermanantCity == Constants.S_EMPTY_STRING &&
            sPermanentPIN.Equals(Constants.S_ZERO))
            tblPerAddress.Visible = false;
        else
        {
            if (sPermanantAddress != Constants.S_EMPTY_STRING)
                lblPerAddress.Text = sPermanantAddress;
            if (sPermanantCity != Constants.S_EMPTY_STRING)
                lblPerCity.Text = sPermanantCity;
            lblPerState.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Permanent_State"]);
            if (!sPermanentPIN.Equals(Constants.S_ZERO))
                lblPerPincode.Text = sPermanentPIN;
        }
    }

    /// <summary>
    /// This method is used to display school related information.
    /// </summary>
    private void TeacherSchoolRelatedInfo()
    {
        const string S_NO_EXPRIENCE = "00";
        lblYears.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Exprince_In_Years"]);
        if (lblYears.Text.Equals(Constants.S_ZERO))
            lblYears.Text = S_NO_EXPRIENCE;

        lblMonths.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Exprince_In_Months"]);
        if (lblMonths.Text.Equals(Constants.S_ZERO))
            lblMonths.Text = S_NO_EXPRIENCE;
            lblAchivements.Text = Convert.ToString(moDsTeacherInfo.Tables[I_TBL_TEACHER_INDEX].Rows[0]["Achivements"]);
    }

    /// <summary>
    /// This method is used to display teachers educational details.
    /// </summary>
    private void DisplayTeacherEducationDetails()
    {
        const int I_TBL_EDU_INDEX = 1;
        grdvwEducationDetails.DataSource = moDsTeacherInfo.Tables[I_TBL_EDU_INDEX].DefaultView;
        grdvwEducationDetails.DataBind();
    }

    private void DisplayTeacherExperienceDetails()
    {
        const int I_TBL_EXP_INDEX = 5;
        lstvwExpDetails.DataSource = moDsTeacherInfo.Tables[I_TBL_EXP_INDEX].DefaultView;
        lstvwExpDetails.DataBind();
    }

    /// <summary>
    /// This function is used to get the teachers basic details.
    /// </summary>
    private void DisplayEmployeeDetails()
    {
        const int I_TBL_EMPLOYEE_DETAILS = 6;
        string sJoiningDate=moDsTeacherInfo.Tables[I_TBL_EMPLOYEE_DETAILS].Rows[0]["JoiningDate"].ToString();
        string sPermanentDate=moDsTeacherInfo.Tables[I_TBL_EMPLOYEE_DETAILS].Rows[0]["PermanentDate"].ToString();
        string sResignationDate=moDsTeacherInfo.Tables[I_TBL_EMPLOYEE_DETAILS].Rows[0]["ResignationDate"].ToString();
        lblJobType.Text = moDsTeacherInfo.Tables[I_TBL_EMPLOYEE_DETAILS].Rows[0]["StatusName"].ToString();
        lblPanNo.Text = moDsTeacherInfo.Tables[I_TBL_EMPLOYEE_DETAILS].Rows[0]["PanNo"].ToString();
        if ( !sJoiningDate.IsNullOrEmpty())       
            lblJoiningDate.Text = Convert.ToDateTime(sJoiningDate).ToString(Constants.S_STANDARD_DATE_FORMAT);
        
        if (!sPermanentDate.IsNullOrEmpty())
            lblPermanentDate.Text = Convert.ToDateTime(sPermanentDate).ToString(Constants.S_STANDARD_DATE_FORMAT);
        
        if (!sResignationDate.IsNullOrEmpty())
            lblResignationDate.Text = Convert.ToDateTime(sResignationDate).ToString(Constants.S_STANDARD_DATE_FORMAT);
    }

    /// <summary>
    /// This method is used to display subject details.
    /// </summary>
    private void DisplaySubjectDetails()
    {
        const int I_TBL_SUBJECT_INDEX = 2;
        DataRow[] oDrSubjectDetails = moDsTeacherInfo.Tables[I_TBL_SUBJECT_INDEX].Select("Teacher_Id=" + miTeacherId.ToString());
        for (int iSubjectCount = 0; iSubjectCount < oDrSubjectDetails.Length; iSubjectCount++)
            lblSubjectLists.Text += Convert.ToString(oDrSubjectDetails[iSubjectCount]["Subject_Name"]) + ", ";

        if (lblSubjectLists.Text.Length >= 2)
            lblSubjectLists.Text = lblSubjectLists.Text.Remove(lblSubjectLists.Text.Length - 2);
    }

    /// <summary>
    /// This method is used to display standard details.
    /// </summary>
    private void DisplayStandardDetails()
    {
        const int I_TBL_STD_INDEX = 3;
        DataRow[] oStandardDetails = moDsTeacherInfo.Tables[I_TBL_STD_INDEX].Select("Teacher_Id=" + miTeacherId.ToString());
        foreach (DataRow oT in oStandardDetails)
            lblStandardsList.Text += Convert.ToString(oT["Standard_Name"]) + ", ";

        if (lblStandardsList.Text.Length >= 2)
            lblStandardsList.Text = lblStandardsList.Text.Remove(lblStandardsList.Text.Length - 2);
    }

    #endregion
}
