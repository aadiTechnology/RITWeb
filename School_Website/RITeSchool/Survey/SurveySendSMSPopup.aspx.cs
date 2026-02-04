using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;

public partial class SurveySendSMSPopup : SchoolBase
{
    #region Data Member(s)
    
    private SurveyStudentBL moSurveyStudentBL; 

    #endregion

    #region Event(s)
    
    /// <summary>
    /// This event is used to fill up categories and standards.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moSurveyStudentBL = new SurveyStudentBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                FillCategoryCombo();
                FillStandars();
                SetJavascriptAttributes();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to send SMS.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSendSMS_Click(object sender, EventArgs e)
    {
        try
        {
            int iCount = SendSMS();
            base.DisplayMessage("SMS sent successfully to " + iCount + " student(s).", false, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

   

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to send SMS.
    /// </summary>
    /// <returns></returns>
    private int SendSMS()
    {
        string sStandardList = GetStandardList();
        List<SurveyStudentDetails> lstStudents = moSurveyStudentBL.GetAllStudents(cmbCategroryGroup.SelectedValue.ToInt(), sStandardList);

        lstStudents.ForEach(
                st =>
                {
                    string sSubject = "DrawingCompetitionSMS";

                    SchoolBL oSchoolBL = new SchoolBL(miSchoolId);
                    Hashtable oHTUsersMobileNo = new Hashtable();

                    if (st.MobileNo1 != string.Empty)
                        oHTUsersMobileNo[st.MobileNo1] = st.MobileNo1;
                    if (st.MobileNo2 != string.Empty)
                    {
                        oHTUsersMobileNo[st.MobileNo2] = st.MobileNo2;
                    }

                    SMS oSMS = new SMS();
                    oSMS.InsertedByID = -9999;
                    oSMS.Sender = oSchoolBL.SMSSenderName;

                    oSMS.SenderRoleID = Convert.ToInt32(Constants.UserRoles.Admin);
                    oSMS.SenderID = oSchoolBL.AdminId;

                    oSMS.School_Name = oSchoolBL.SchoolName + "::" + sSubject;
                    oSMS.SMSText = txtSMSText.Text.Trim();
                    oSMS.AcademicYearID = miAcademicYearId;
                    oSMS.SchoolID = miSchoolId;
                    oSMS.DisplayText = st.Name;
                    oSMS.ToManualNumbers = oHTUsersMobileNo;

                    oSMS.Send();

                    oHTUsersMobileNo.Clear();
                }

            );
        return lstStudents.Count;
    } 

    /// <summary>
    /// THis method is used to set java script attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnSendSMS });
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
    }

    /// <summary>
    /// THis method is used to fill category combo box.
    /// </summary>
    private void FillCategoryCombo()
    {
        string[] sArrCategories = Enum.GetNames(typeof(Constants.StudentSurveyRegistrationCategory));

        ListItem listItem = new ListItem(Constants.S_SELECT, Constants.S_ZERO);
        cmbCategroryGroup.Items.Add(listItem);

        foreach (string sItem in sArrCategories)
        {
            int iValue = (int)Enum.Parse(typeof(Constants.StudentSurveyRegistrationCategory), sItem);
            listItem = new ListItem(sItem, iValue.ToString());
            cmbCategroryGroup.Items.Add(listItem);
        }
    }

    /// <summary>
    /// This method is used to fill up standard checkbox lsit.
    /// </summary>
    public void FillStandars()
    {
        SurveyStudentBL oSurveyStudentBL = new SurveyStudentBL();
        DataTable oDTStandards = oSurveyStudentBL.GetStandardList(miSchoolId, miAcademicYearId);
        chkListStandards.DataSource = oDTStandards;
        chkListStandards.DataTextField = "StandardName";
        chkListStandards.DataValueField = "StandardId";
        chkListStandards.DataBind();

    }

    /// <summary>
    /// This method is used to return selected standard list.
    /// </summary>
    /// <returns></returns>
    private string GetStandardList()
    {
        StringBuilder obj = new StringBuilder();

        for (int iListIndex = 0; iListIndex < chkListStandards.Items.Count; iListIndex++)
        {
            if (chkListStandards.Items[iListIndex].Selected == true)
                obj.Append("," + chkListStandards.Items[iListIndex].Value);
        }

        if (obj.ToString().StartsWith(","))
            return obj.ToString().Substring(1);
        else
            return obj.ToString();
    } 

    #endregion
}