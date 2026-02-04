using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using System.Reflection;
using BusinessLogic.Exceptions;
using BusinessLogic;
using MasterEntities;
using StudentEntities;
using Utility;
using System.Text;
/// <summary>
/// This class is used to set second language to the students.
/// </summary>

public partial class SecondLanguageUI : SchoolBase
{
    #region "Constants"

    const string S_SAVE_MESSAGE = "Second language saved successfully !!!";
    const string S_POSTBACK_URL = "AllStudentsUI.aspx";

    #endregion

    #region "Data Members"

    SecondLanguageBL moSecondLanguageBL = null;
    List<SubjectMaster> mlstLanguageSubjects = null;    
    
    #endregion

    #region "Events"

    /// <summary>
    /// This event is used to default values.
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
                FillStandardCombobox();
                SetJavaScriptAttributres();
                cmbStandard.Focus();
                RefreshValue();
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValue();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill division combobox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(cmbStandard.SelectedValue) == Constants.I_ZERO)
            {
                cmbDivision.Items.Clear();
                cmbDivision.Items.Add(new ListItem(Constants.S_SELECT, Constants.I_ZERO.ToString()));
                lstvwSecondLanguage.Visible = false;
                SetButtonState(false);
            }
            else
            {
                FillDivisionCombobox();
                FillStudentDetailsListview();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to fill list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillStudentDetailsListview();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to display student datails.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <summary>
    /// This event is used to bind data to listview columns.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwSecondLanguage_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                DropDownList cmbSecondLanguage = oCurrentItem.FindControl("cmbSecondLanguage") as DropDownList;
                DropDownList cmbThirdLanguage = oCurrentItem.FindControl("cmbThirdLanguage") as DropDownList;

                int iRowId = oCurrentItem.DisplayIndex;
                StudentInfo oStudentInfo = oCurrentItem.DataItem as StudentInfo;

                var lstSecondLanguage = mlstLanguageSubjects.Where(sl => sl.LanguageGroupId == Constants.LanguageMode.SecondLanguage.ToInt()).ToList();
                ListSource.FillDropDownList(lstSecondLanguage, cmbSecondLanguage, "SubjectName", "SubjectId", Constants.S_SELECT);
               cmbSecondLanguage.SelectedValue = oStudentInfo.SecondLanguageSubjectId.ToString();

               var lstThirdLanguage = mlstLanguageSubjects.Where(sl => sl.LanguageGroupId == Constants.LanguageMode.ThirdLanguage.ToInt()).ToList();

               if (lstSecondLanguage.Count > 0 && lstThirdLanguage.Count == 0)
                   ListSource.FillDropDownList(lstSecondLanguage, cmbThirdLanguage, "SubjectName", "SubjectId", Constants.S_SELECT);
               else
                   ListSource.FillDropDownList(lstThirdLanguage, cmbThirdLanguage, "SubjectName", "SubjectId", Constants.S_SELECT);

               cmbThirdLanguage.SelectedValue = oStudentInfo.ThirdLanguageSubjectId.ToString();
              // cmbSecondLanguage.Attributes.Add("onchange", "if(!ChangeSecondAndThirdLanguage(" + Constants.I_ONE + "," + oCurrentItem.DisplayIndex + ")) return false;");
             //cmbThirdLanguage.Attributes.Add("onchange", "if(!ChangeSecondAndThirdLanguage(" + Constants.I_TWO + "," + oCurrentItem.DisplayIndex + ")) return false;");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to save second language of the student. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            UpdateSecondLanguage();
            lblMessage.Text = Resources.LocalizedResources.SecondLanguageSavedSuccessfully;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region "Private methods"

    /// <summary>
    ///This methos is used to fill standard combobox.
    /// </summary>
    private void FillStandardCombobox()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId , miAcademicYearId);
        DataTable oDtStandardCollection = oStandardCollectionBL.GetAssociatedStandards();
        ControlUtility.FillDropDownList(oDtStandardCollection, ref cmbStandard,
                                       Constants.S_STANDARD_ID_FIELD,
                                       Constants.S_STANDARD_NAME_FIELD,
                                       Constants.S_SELECT);
    }
    /// <summary>
    /// This method is used to fill division combobox.
    /// </summary>
    private void FillDivisionCombobox()
    {
        int iStandardId = Convert.ToInt32(cmbStandard.SelectedValue); 
        DivisionCollectionBL oDivisionCollectionBL = new DivisionCollectionBL(miSchoolId , miAcademicYearId);
        DataTable oDSStandardCollection = oDivisionCollectionBL.GetAllDivisionsForStandard(iStandardId);

        ControlUtility.FillDropDownList(oDSStandardCollection, ref cmbDivision,
                                       Constants.S_DIVISION_ID_FIELD,
                                       Constants.S_DIVISION_NAME_FIELD,
                                       string.Empty);
    }
    /// <summary>
    /// This method is used to fill listview.
    /// </summary>
    private void FillStudentDetailsListview()
    {
        lstvwSecondLanguage.Visible = true;
        int iStandardId = Convert.ToInt32(cmbStandard.SelectedValue);
        int iDivisionId = Convert.ToInt32(cmbDivision.SelectedValue);
        StudentBL oStudentBL = new StudentBL();
        moSecondLanguageBL=new SecondLanguageBL(miSchoolId,miAcademicYearId);
        
        mlstLanguageSubjects = moSecondLanguageBL.GetAll(iStandardId, iDivisionId);

        StringBuilder oStringBuilder = new StringBuilder();
        mlstLanguageSubjects.ForEach(
            sl =>
                {
                    oStringBuilder.Append("$" + sl.SubjectId + "," + sl.SubjectGroupId);
                }
            );

        if (oStringBuilder.Length > 0)
            hidSubjectGroupIds.Value = oStringBuilder.ToString().Substring(1);
        else
            hidSubjectGroupIds.Value = string.Empty;

        oStringBuilder.Clear();

        if (mlstLanguageSubjects.Any(s => s.SecondThirdId != 0))
        {
            mlstLanguageSubjects.Where(s => s.SecondThirdId == 0).ToList().ForEach
                (
                    sub =>
                    {
                        var thirdLang = mlstLanguageSubjects.Where(s => s.SubjectGroupId == sub.SubjectGroupId && s.SubjectId != sub.SubjectId).FirstOrDefault();
                        var ss = mlstLanguageSubjects.Where(s => s.LanguageGroupId == thirdLang.LanguageGroupId && s.SubjectId != thirdLang.SubjectId).FirstOrDefault();
                        oStringBuilder.Append("$" + sub.SubjectId + "," + ss.SubjectId);
                    }
                );
            hidPrimarySection.Value = Constants.S_NO;

            if (oStringBuilder.Length > 0)
                hidLanguageGroupIds.Value = oStringBuilder.ToString().Substring(1);
            else
                hidLanguageGroupIds.Value = string.Empty;
        }
        else
        {
            var subjects = mlstLanguageSubjects.Select(s => s.SubjectId).Distinct().ToList();
            hidLanguageGroupIds.Value = subjects[0] + "," + subjects[1];
            hidPrimarySection.Value = Constants.S_YES;
        }

        

        List<StudentInfo> lstStudentDetails = oStudentBL.GetStudentDetails(miSchoolId , miAcademicYearId, iStandardId, iDivisionId);
        lstvwSecondLanguage.DataSource = lstStudentDetails;
        lstvwSecondLanguage.DataBind();
        if (lstvwSecondLanguage.Items.Count > Constants.I_ZERO)
        {
            lstvwSecondLanguage.Visible = true;
            SetButtonState(true);
        }
        else
        {
            lstvwSecondLanguage.Visible = false;
            SetButtonState(false);
        }

        //if (moSecondLanguageBL.IsAnyExamPublished(iStandardId, iDivisionId))
        //{
        //    btnSave.Enabled = false;
        //    btnSaveUp.Enabled = false;
        //    lstvwSecondLanguage.Enabled = false;
        //}
    }
    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavaScriptAttributres()
    {
        btnBack.PostBackUrl = S_POSTBACK_URL;
        btnBackUp.PostBackUrl = S_POSTBACK_URL;
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnBack, btnSaveUp, btnBackUp });
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
    }
    /// <summary>
    /// This method is used to populate Second language Details.
    /// </summary>
    /// <returns></returns>
    private List<StudentInfo> PopulateSecondLanguageDetails()
    {
        List<StudentInfo> lstStudentDetail = new List<StudentInfo>();
        StudentInfo oStudentInfo = null;
        foreach (ListViewDataItem oCurrentItem in lstvwSecondLanguage.Items)
        {
            int iRowId = oCurrentItem.DisplayIndex;
            DropDownList cmbSecondLanguage = oCurrentItem.FindControl("cmbSecondLanguage") as DropDownList;
            DropDownList cmbThirdLanguage = oCurrentItem.FindControl("cmbThirdLanguage") as DropDownList;
            oStudentInfo = new StudentInfo()
            {
                SecondLanguageSubjectId = Convert.ToInt32(cmbSecondLanguage.SelectedValue),
                ThirdLanguageSubjectId = Convert.ToInt32(cmbThirdLanguage.SelectedValue),
                SchoolwiseStudentId = Convert.ToInt32(lstvwSecondLanguage.DataKeys[iRowId]["SchoolwiseStudentId"])
            };
            lstStudentDetail.Add(oStudentInfo);
        }
        return lstStudentDetail;
    }
    /// <summary>
    /// This method is used to generate xml.0
    /// </summary>
    /// <param name="lstStudentDetails"></param>
    /// <returns></returns>
    private string GenrateSecondLanguageXml(List<StudentInfo> lstStudentDetails)
    {
        StringWriter sw = new StringWriter();
        new XmlSerializer(lstStudentDetails.GetType()).Serialize(sw, lstStudentDetails);
        string sXml = sw.ToString();
        return sXml;
    }
    /// <summary>
    /// This method is used to save second language of the student.
    /// </summary>
    private void UpdateSecondLanguage()
    {
        int iUserId = Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]);
        moSecondLanguageBL = new SecondLanguageBL(miSchoolId , miAcademicYearId);
        string sXml = GenrateSecondLanguageXml(PopulateSecondLanguageDetails());
        moSecondLanguageBL.Update(sXml, iUserId, cmbStandard.SelectedValue.ToInt(), cmbDivision.SelectedValue.ToInt());
    }
    /// <summary>
    /// This method is used to set button status.
    /// </summary>
    /// <param name="aFlag"></param>
    private void SetButtonState(bool aFlag)
    {
        btnSave.Visible = aFlag;
        btnSaveUp.Visible = aFlag;
        btnBackUp.Visible = aFlag;
    }
    /// <summary>
    /// This method used to refresh value based on Culture
    /// </summary>
    private void RefreshValue()
    {
       // lblMessage.Text = Resources.LocalizedResources.SecondLanguageSavedSuccessfully;
    }
    #endregion

}
