using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using SchoolEntities;
using System.Reflection;
using System.Data;


public partial class TeacherAdditionalDetailsUI : SchoolBase
{

    #region Constant(s)

    private const string S_SAVE_TEXT = "Save";
    private const string S_SAVE_MESSAGE = "Teachers additional details saved successfully. !!!";

    #endregion

    #region DataMember

    private TeacherAdditionalDetailsBL moTeacherAdditionalDetailsBL;
    private List<TeacherAdditionalDetails> molstTeacherAdditionalDetails;

    #endregion

    #region Enum

    private enum TeacherQuestionDetails
    {
        TeacherTypes = 8,
        AcademicQualification = 10,
        ProfessionalQualification = 11,
        ClassesTaught = 12,
        AppointedSubjects = 13,
        MainSubjects = 14,
        AdditionalSubjects = 15,
        BRC = 16,
        CRC = 17,
        DIET = 18,
        Other = 19,
        NeedTraining = 20,
        NonteachingAssignment = 21,
        MathsScience = 22,
        EnglishStudied = 23,
        SocialStudied = 24,
        PresentSchoolYEar = 25,
        Diability = 26,
        CWSNTrained = 27,
        ComputerTrained = 28
    }

    #endregion

    #region Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moTeacherAdditionalDetailsBL = new TeacherAdditionalDetailsBL(miSchoolId, miAcademicYearId, miUserId);
            molstTeacherAdditionalDetails = new List<TeacherAdditionalDetails>();
            if (!IsPostBack)
            {
                ReadQueryString();
                FillAllCombobox();
                GetTeacherAdditionalDetails();
                SetJavaScriptAttribute();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This Event is 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string sReturnValue = Populate();
            moTeacherAdditionalDetailsBL.save(hidTeacherId.Value.ToInt(), sReturnValue);

            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Constants.S_PAGE_TEACHER_INFO + "?" + string.Empty);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This Event is Used to button clear click.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            ClearFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method's

    /// <summary>
    /// This method is used to Populate all the details for Save.
    /// </summary>
    /// <returns></returns>
    public string Populate()
    {
        List<TeacherAdditionalDetails> lstTeacherAdditionalDetails = new List<TeacherAdditionalDetails>();

        lstTeacherAdditionalDetails.Add(AssignValuesToList(TeacherQuestionDetails.TeacherTypes.ToInt(), cmbTeacherTypes.SelectedValue.ToInt(), string.Empty));
        lstTeacherAdditionalDetails.Add(AssignValuesToList(TeacherQuestionDetails.AcademicQualification.ToInt(), cmbAcademicQualification.SelectedValue.ToInt(), string.Empty));
        lstTeacherAdditionalDetails.Add(AssignValuesToList(TeacherQuestionDetails.ProfessionalQualification.ToInt(), cmbProfessionalQualification.SelectedValue.ToInt(), string.Empty));
        lstTeacherAdditionalDetails.Add(AssignValuesToList(TeacherQuestionDetails.ClassesTaught.ToInt(), cmbClassesTaught.SelectedValue.ToInt(), string.Empty));
        lstTeacherAdditionalDetails.Add(AssignValuesToList(TeacherQuestionDetails.AppointedSubjects.ToInt(), Constants.I_ZERO, txtAppointedSubject.Text.Trim()));
        lstTeacherAdditionalDetails.Add(AssignValuesToList(TeacherQuestionDetails.MainSubjects.ToInt(), cmbMainSubject.SelectedValue.ToInt(), string.Empty));
        lstTeacherAdditionalDetails.Add(AssignValuesToList(TeacherQuestionDetails.AdditionalSubjects.ToInt(), cmbAdditionalSubjects.SelectedValue.ToInt(), string.Empty));
        lstTeacherAdditionalDetails.Add(AssignValuesToList(TeacherQuestionDetails.BRC.ToInt(), Constants.I_ZERO, txtTotalDaysBRC.Text.Trim()));
        lstTeacherAdditionalDetails.Add(AssignValuesToList(TeacherQuestionDetails.CRC.ToInt(), Constants.I_ZERO, txtTotalDaysCRC.Text.Trim()));
        lstTeacherAdditionalDetails.Add(AssignValuesToList(TeacherQuestionDetails.DIET.ToInt(), Constants.I_ZERO, txtTotalDaysDIET.Text.Trim()));
        lstTeacherAdditionalDetails.Add(AssignValuesToList(TeacherQuestionDetails.Other.ToInt(), Constants.I_ZERO, txtOtherCount.Text.Trim()));
        lstTeacherAdditionalDetails.Add(AssignValuesToList(TeacherQuestionDetails.NeedTraining.ToInt(), cmbTypesOfTraining.SelectedValue.ToInt(), string.Empty));
        lstTeacherAdditionalDetails.Add(AssignValuesToList(TeacherQuestionDetails.NonteachingAssignment.ToInt(), Constants.I_ZERO, txtNonTeachingAssignment.Text.Trim()));
        lstTeacherAdditionalDetails.Add(AssignValuesToList(TeacherQuestionDetails.MathsScience.ToInt(), Constants.I_ZERO, txtMathsScienceStudiedUpto.Text.Trim()));
        lstTeacherAdditionalDetails.Add(AssignValuesToList(TeacherQuestionDetails.EnglishStudied.ToInt(), Constants.I_ZERO, txtEnglishStudiedUpto.Text.Trim()));
        lstTeacherAdditionalDetails.Add(AssignValuesToList(TeacherQuestionDetails.SocialStudied.ToInt(), Constants.I_ZERO, txtSocialStudiedUpto.Text.Trim()));
        lstTeacherAdditionalDetails.Add(AssignValuesToList(TeacherQuestionDetails.PresentSchoolYEar.ToInt(), Constants.I_ZERO, txtPresentSchoolYear.Text.Trim()));
        lstTeacherAdditionalDetails.Add(AssignValuesToList(TeacherQuestionDetails.Diability.ToInt(), cmbDisability.SelectedValue.ToInt(), string.Empty));
        if (rdoIsCWSNTrainedYes.Checked)
            lstTeacherAdditionalDetails.Add(AssignValuesToList(TeacherQuestionDetails.CWSNTrained.ToInt(), Constants.I_ONE, string.Empty));
        else
            lstTeacherAdditionalDetails.Add(AssignValuesToList(TeacherQuestionDetails.CWSNTrained.ToInt(), Constants.I_TWO, string.Empty));

        if (rdoIsComputerTrainedYes.Checked)
            lstTeacherAdditionalDetails.Add(AssignValuesToList(TeacherQuestionDetails.ComputerTrained.ToInt(), Constants.I_ONE, string.Empty));
        else
            lstTeacherAdditionalDetails.Add(AssignValuesToList(TeacherQuestionDetails.ComputerTrained.ToInt(), Constants.I_TWO, string.Empty));

        return GenerateXml(lstTeacherAdditionalDetails);
    }

    /// <summary>
    /// This Method is used to clear all fields.
    /// </summary>
    private void ClearFields()
    {
        cmbTeacherTypes.ClearSelection();
        txtTotalDaysBRC.Text = string.Empty;
        txtTotalDaysCRC.Text = string.Empty;
        txtTotalDaysDIET.Text = string.Empty;
        txtOtherCount.Text = string.Empty;
        cmbTypesOfTraining.ClearSelection();
        cmbAcademicQualification.ClearSelection();
        cmbProfessionalQualification.ClearSelection();
        cmbClassesTaught.ClearSelection();
        txtAppointedSubject.Text = string.Empty;
        cmbMainSubject.ClearSelection();
        cmbAdditionalSubjects.ClearSelection();
        txtNonTeachingAssignment.Text = string.Empty;
        txtMathsScienceStudiedUpto.Text = string.Empty;
        txtEnglishStudiedUpto.Text = string.Empty;
        txtSocialStudiedUpto.Text = string.Empty;
        txtPresentSchoolYear.Text = string.Empty;
        cmbDisability.ClearSelection();
        rdoIsComputerTrainedNo.Checked = true;
        rdoIsCWSNTrainedNo.Checked = true;
    }

    /// <summary>
    /// This Method is used to Read query string.
    /// </summary>
    private void ReadQueryString()
    {
        if (QueryString["TeacherId"] != null)
            hidTeacherId.Value = QueryString["TeacherId"].ToString();
    }

    /// <summary>
    /// This Mehod is used to get all Teacher additional Details.
    /// </summary>
    private void GetTeacherAdditionalDetails()
    {
        molstTeacherAdditionalDetails = moTeacherAdditionalDetailsBL.Get(hidTeacherId.Value.ToInt());

        if (molstTeacherAdditionalDetails.Count > Constants.I_ZERO)
        {
            if (molstTeacherAdditionalDetails.Count == Constants.I_ONE)
            {
                lblTeacherName.Text = molstTeacherAdditionalDetails[0].TeacherName;
                hidTeacherId.Value = molstTeacherAdditionalDetails[0].TeacherId.ToString();
                rdoIsCWSNTrainedNo.Checked = true;
                rdoIsComputerTrainedNo.Checked = true;                
            }
            else
            {
                lblTeacherName.Text = molstTeacherAdditionalDetails[0].TeacherName;
                hidTeacherId.Value = molstTeacherAdditionalDetails[0].TeacherId.ToString();
                cmbTeacherTypes.SelectedValue = AssignValuesToControls(TeacherQuestionDetails.TeacherTypes.ToInt(), Constants.I_ONE);
                cmbAcademicQualification.SelectedValue = AssignValuesToControls(TeacherQuestionDetails.AcademicQualification.ToInt(), Constants.I_ONE);
                cmbProfessionalQualification.SelectedValue = AssignValuesToControls(TeacherQuestionDetails.ProfessionalQualification.ToInt(), Constants.I_ONE);
                cmbClassesTaught.SelectedValue = AssignValuesToControls(TeacherQuestionDetails.ClassesTaught.ToInt(), Constants.I_ONE);
                txtAppointedSubject.Text = AssignValuesToControls(TeacherQuestionDetails.AppointedSubjects.ToInt(), Constants.I_ZERO);
                cmbMainSubject.SelectedValue = AssignValuesToControls(TeacherQuestionDetails.MainSubjects.ToInt(), Constants.I_ONE);
                cmbAdditionalSubjects.SelectedValue = AssignValuesToControls(TeacherQuestionDetails.AdditionalSubjects.ToInt(), Constants.I_ONE);
                txtTotalDaysBRC.Text = AssignValuesToControls(TeacherQuestionDetails.BRC.ToInt(), Constants.I_ZERO);
                txtTotalDaysCRC.Text = AssignValuesToControls(TeacherQuestionDetails.CRC.ToInt(), Constants.I_ZERO);
                txtTotalDaysDIET.Text = AssignValuesToControls(TeacherQuestionDetails.DIET.ToInt(), Constants.I_ZERO);
                txtOtherCount.Text = AssignValuesToControls(TeacherQuestionDetails.Other.ToInt(), Constants.I_ZERO);
                cmbTypesOfTraining.SelectedValue = AssignValuesToControls(TeacherQuestionDetails.NeedTraining.ToInt(), Constants.I_ONE);
                txtNonTeachingAssignment.Text = AssignValuesToControls(TeacherQuestionDetails.NonteachingAssignment.ToInt(), Constants.I_ZERO);
                txtMathsScienceStudiedUpto.Text = AssignValuesToControls(TeacherQuestionDetails.MathsScience.ToInt(), Constants.I_ZERO);
                txtEnglishStudiedUpto.Text = AssignValuesToControls(TeacherQuestionDetails.EnglishStudied.ToInt(), Constants.I_ZERO);
                txtSocialStudiedUpto.Text = AssignValuesToControls(TeacherQuestionDetails.SocialStudied.ToInt(), Constants.I_ZERO);
                txtPresentSchoolYear.Text = AssignValuesToControls(TeacherQuestionDetails.PresentSchoolYEar.ToInt(), Constants.I_ZERO);
                cmbDisability.SelectedValue = AssignValuesToControls(TeacherQuestionDetails.Diability.ToInt(), Constants.I_ONE);
                string asIsCWSNTrained = AssignValuesToControls(TeacherQuestionDetails.CWSNTrained.ToInt(), Constants.I_ONE);
                if (asIsCWSNTrained == Constants.S_ONE)
                    rdoIsCWSNTrainedYes.Checked = true;
                else
                    rdoIsCWSNTrainedNo.Checked = true;

                string asIsComputerTrained = AssignValuesToControls(TeacherQuestionDetails.ComputerTrained.ToInt(), Constants.I_ONE);
                if (asIsComputerTrained == Constants.S_ONE)
                    rdoIsComputerTrainedYes.Checked = true;
                else
                    rdoIsComputerTrainedNo.Checked = true;              
            }
        }
    }

    /// <summary>
    /// This Method is used to Set Javascript Attributes.
    /// </summary>
    private void SetJavaScriptAttribute()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnCancel, btnSave });
        valTeacherDetails.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
    }

    /// <summary>
    /// This method is used to fill all comboboex 
    /// </summary>
    private void FillAllCombobox()
    {
        DataSet dtAllDetails = new DataSet();
        dtAllDetails = moTeacherAdditionalDetailsBL.GetAllMasterDetailsForUDISEForm();

        DataTable dtTypesOfTeachers = dtAllDetails.Tables[0] as DataTable;
        DataTable dtAcademicQualificatin = dtAllDetails.Tables[1] as DataTable;
        DataTable dtProfessionalQualification = dtAllDetails.Tables[2] as DataTable;
        DataTable dtMainSubjects = dtAllDetails.Tables[3] as DataTable;
        DataTable dtDisability = dtAllDetails.Tables[4] as DataTable;
        DataTable dtClassesTaught = dtAllDetails.Tables[5] as DataTable;
        DataTable dtTypesOfTrainig = dtAllDetails.Tables[6] as DataTable;

        cmbTeacherTypes.Bind(dtTypesOfTeachers, "Id", "Types", Constants.S_SELECT);
        cmbAcademicQualification.Bind(dtAcademicQualificatin, "Id", "AcademicQualification", Constants.S_SELECT);
        cmbProfessionalQualification.Bind(dtProfessionalQualification, "Id", "ProfessionalQualification", Constants.S_SELECT);
        cmbMainSubject.Bind(dtMainSubjects, "Id", "MainSubjects", Constants.S_SELECT);
        cmbAdditionalSubjects.Bind(dtMainSubjects, "Id", "MainSubjects", Constants.S_SELECT);
        cmbDisability.Bind(dtDisability, "Id", "Disability", Constants.S_SELECT);
        cmbClassesTaught.Bind(dtClassesTaught, "Id", "ClassName", Constants.S_SELECT);
        cmbTypesOfTraining.Bind(dtTypesOfTrainig, "Id", "TypesOfTraining", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to set the values to list for saving.
    /// </summary>
    /// <param name="iQuestionId"></param>
    /// <param name="ivalue"></param>
    /// <param name="sAnswer"></param>
    /// <returns></returns>
    private TeacherAdditionalDetails AssignValuesToList(int aiQuestionId, int aivalue, string asAnswer)
    {
        TeacherAdditionalDetails oTeacherAdditionalDetails = new TeacherAdditionalDetails();

        oTeacherAdditionalDetails.QuestionId = aiQuestionId;
        oTeacherAdditionalDetails.AnswerId = aivalue;
        oTeacherAdditionalDetails.AnswerText = asAnswer;

        return oTeacherAdditionalDetails;
    }

    /// <summary>
    /// This method is used to assign values to control.
    /// </summary>
    /// <param name="aiQuestionId"></param>
    /// <param name="lstTeacherAdditionalDetails"></param>
    /// <param name="aiIsValue"></param>
    /// <returns></returns>
    private string AssignValuesToControls(int aiQuestionId, int aiIsValue)
    {
        if (aiIsValue == Constants.I_ONE)
            return molstTeacherAdditionalDetails.Where(st => st.QuestionId == aiQuestionId).Select(aa => aa.AnswerId).FirstOrDefault().ToString();
        else
            return molstTeacherAdditionalDetails.Where(st => st.QuestionId == aiQuestionId).Select(aa => aa.AnswerText).FirstOrDefault().ToString();
    }

    #endregion
}

