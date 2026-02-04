using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using System.Reflection;
using BusinessLogic;
using System.Data;
using Utility;
using SchoolEntities;
using System.IO;
using System.Xml.Serialization;

public partial class StudentBulkEmailUI : SchoolBase
{
    #region "Constants"

    const string S_SAVE_MESSAGE = "Student bulk email saved successfully !!!";
    const string S_POSTBACK_URL = "../Admin/AllStudentsUI.aspx";

    #endregion

    #region "Events"

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                FillStandardCombobox();
                SetJavaScriptAttributres();
                SetButtonState(false);
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
            }
            else
            {
                FillDivisionCombobox();
                FillStudentDetailsListview();
            }
            SetButtonState(true);
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
    /// This event is used to List View Intem Data Bound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStudentEmail_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                             
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save the records.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            UpdateEmailAddress();
            lblMessage.Text = S_SAVE_MESSAGE;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    ///This methos is used to fill standard combobox.
    /// </summary>
    private void FillStandardCombobox()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
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
        DivisionCollectionBL oDivisionCollectionBL = new DivisionCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDSStandardCollection = oDivisionCollectionBL.GetAllDivisionsForStandard(iStandardId);

        ControlUtility.FillDropDownList(oDSStandardCollection, ref cmbDivision,
                                       Constants.S_DIVISION_ID_FIELD,
                                       Constants.S_DIVISION_NAME_FIELD,
                                       string.Empty);
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
    /// This method is used to fill listview.
    /// </summary>
    private void FillStudentDetailsListview()
    {   
        StudentBL oStudentBL = new StudentBL();
        List<StudentsBulkEmail> lstStudentsBulkEmail = oStudentBL.GetStudentDetailsForBulkEmail(miSchoolId, miAcademicYearId, cmbStandard.SelectedValue.ToInt(), cmbDivision.SelectedValue.ToInt());
        if (lstStudentsBulkEmail.Count > Constants.I_ZERO)
        {
            hidTotalStudentCount.Value = lstStudentsBulkEmail.Count.ToString();
            lstvwStudentEmail.DataSource = lstStudentsBulkEmail;
            lstvwStudentEmail.DataBind();
        }
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
    /// This event is used to Update Email Address to students.
    /// </summary>
    public void UpdateEmailAddress()
    {
        StudentBL oStudentBL = new StudentBL();
        int iUserId = Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]);
        string sXml = GenrateBulkEmailXml(Populate());
        oStudentBL.UpdateStudentsEmailInBulk(miSchoolId, miUserId, sXml);
    }

    /// <summary>
    /// This method is used to populate Second language Details.
    /// </summary>
    /// <returns></returns>
    private List<StudentsBulkEmail> Populate()
    {
        List<StudentsBulkEmail> lstStudentsBulkEmail = new List<StudentsBulkEmail>();
        StudentsBulkEmail oStudentsBulkEmail = null;

        foreach (ListViewDataItem oCurrentItem in lstvwStudentEmail.Items)
        {
            int iRowId = oCurrentItem.DisplayIndex;           

            TextBox txtEmailAddress = oCurrentItem.FindControl("txtEmailAddress") as TextBox;

            if (txtEmailAddress.Text.TrimAll() != string.Empty)
            {
                oStudentsBulkEmail = new StudentsBulkEmail()
                {
                    StudentId = Convert.ToInt32(lstvwStudentEmail.DataKeys[iRowId]["StudentId"]),
                    EmailAddress = txtEmailAddress.Text.TrimAll()
                };
                lstStudentsBulkEmail.Add(oStudentsBulkEmail);
            }
        }
        return lstStudentsBulkEmail;
    }

    /// <summary>
    /// This method is used to generate xml.0
    /// </summary>
    /// <param name="lstStudentDetails"></param>
    /// <returns></returns>
    private string GenrateBulkEmailXml(List<StudentsBulkEmail> lstStudentsBulkEmail)
    {
        StringWriter sw = new StringWriter();
        new XmlSerializer(lstStudentsBulkEmail.GetType()).Serialize(sw, lstStudentsBulkEmail);
        string sXml = sw.ToString();
        return sXml;
    }

    #endregion
}