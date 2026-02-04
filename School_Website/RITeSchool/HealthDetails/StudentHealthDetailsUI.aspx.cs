/* File Name :- StudentHealthDetailsUI.aspx.cs
 * Created Date :- 22-Nov-2018
 * Class Description :- This class is used to Add Student Health Details.
 * Created By :- Dnyaneshwar Shinde.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;
using BusinessLogic;
using System.Data;
using BusinessLogic.Exceptions;
using SchoolEntities;
using System.Web.UI.HtmlControls;

public partial class StudentHealthDetailsUI : SchoolBase
{
    #region Constant(s)

    private const string S_SUBMIT = "Submit";
    private const string S_UNSUBMIT = "UnSubmit";
    private const string S_SAVE_MESSAGE = "Student health details saved successfully !!!";
    private const string S_SUBMIT_MESSAGE = "Student health details submitted successfully !!!";
    private const string S_UNSUBMIT_MESSAGE = "Student health details unsubmitted successfully !!!";

    #endregion

    #region DataMember

    private HealthDetailsBL moHealthDetailsBL;

    #endregion

    #region Event's

    /// <summary>
    /// This Event is used to load the all controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moHealthDetailsBL = new HealthDetailsBL(miSchoolId, miAcademicYearId,miUserId);
            if (!IsPostBack)
            {
                ReadQueryString();
                SetJavascriptAttribute();
                GetStudentDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }    

    /// <summary>
    /// This Event is used to save Student Health details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string sStudentDetails = PopulateStudentDetails();
            moHealthDetailsBL.SaveStudentHealthDetails(hidStudentId.Value.ToInt(), sStudentDetails);
            base.DisplayMessage(S_SAVE_MESSAGE, false, tdMessage);

            GetStudentDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This Event Is Used for Submit student health details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            int iIsPublish;
            if (btnSubmit.Text == S_SUBMIT)
                iIsPublish = Constants.I_ONE;
            else
                iIsPublish = Constants.I_ZERO;

            moHealthDetailsBL.SubmitStudentHealthDetails(hidStudentId.Value.ToInt(), iIsPublish);
            if (iIsPublish == Constants.I_ONE)
                base.DisplayMessage(S_SUBMIT_MESSAGE, false, tdMessage);
            else
                base.DisplayMessage(S_UNSUBMIT_MESSAGE, false, tdMessage);

            GetStudentDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This Event is used to Clear all the controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            ClearControls();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwStudentHealthDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            ProcessGrid("lblComponent", Constants.I_ZERO, true);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwStudentHealthDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                TextBox txtAnswer = e.Item.FindControl("txtAnswer") as TextBox;
                txtAnswer.Attributes.Add("onkeyup", "OnGridKeyUp(this,event);");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method's

    /// <summary>
    /// This method is used to read Query String
    /// </summary>
    private void ReadQueryString()
    {
        if (QueryString["StudentId"] != null)
            hidStudentId.Value = QueryString["StudentId"].ToString();

        if (QueryString["StandardId"] != null)
            hidStandardId.Value = QueryString["StandardId"].ToString();

        if (QueryString["DivisionId"] != null)
            hidDivisionId.Value = QueryString["DivisionId"].ToString();
    }

    /// <summary>
    /// This method is used to get & Fill list view of student details.
    /// </summary>
    private void GetStudentDetails()
    {
        List<StudentHealthDetails> lstStudentHealthDetails = moHealthDetailsBL.GetStudentHealthDetails(hidStudentId.Value.ToInt());

        if (lstStudentHealthDetails.Count > Constants.I_ZERO)
        {
            lblEnrolmentNo.Text = lstStudentHealthDetails[0].EnrolmentNo.ToString();
            lblName.Text = lstStudentHealthDetails[0].StudentName.ToString();
            lblRollNo.Text = Convert.ToString(lstStudentHealthDetails[0].RollNo.ToInt());
            lblClass.Text = lstStudentHealthDetails[0].ClassName.ToString();

            lstvwStudentHealthDetails.DataSource = lstStudentHealthDetails;
            lstvwStudentHealthDetails.DataBind();

            if (lstStudentHealthDetails[0].IsDataSaved == Constants.I_ONE)
                btnSubmit.Enabled = true;
            else
                btnSubmit.Enabled = false;

            if (lstStudentHealthDetails[0].SubmitStatus)
            {
                btnSubmit.Text = S_UNSUBMIT;
                btnSave.Enabled = false;
                btnClear.Enabled = false;
                lstvwStudentHealthDetails.Enabled = false;
            }
            else
            {
                btnSubmit.Text = S_SUBMIT;
                btnSave.Enabled = true;                                
                lstvwStudentHealthDetails.Enabled = true;
            }
        }
    }

    /// <summary>
    /// This method is used to Set Java script attributes to controls.
    /// </summary>
    private void SetJavascriptAttribute()
    {
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        base.ApplyMouseHoverEffect(new List<Button> { btnSave, btnClear,btnBack });
        string sQueryString = "StandardId=" + hidStandardId.Value +
                              "&DivisionId=" + hidDivisionId.Value;
        hidQueryString.Value = CommonUtility.EncryptQuerystring(sQueryString);
        btnBack.Attributes.Add("onclick", "if(!OpenStudentListScreen()) return false;");
    }

    /// <summary>
    /// This method is used to clear all the controls.
    /// </summary>
    private void ClearControls()
    {
        foreach (ListViewItem obj in lstvwStudentHealthDetails.Items)
        {
            TextBox txtAnswer = obj.FindControl("txtAnswer") as TextBox;
            txtAnswer.Text = string.Empty;
        }
    }

    /// <summary>
    /// This method is used to populate student details for save.
    /// </summary>
    /// <returns></returns>
    private string PopulateStudentDetails()
    {
        List<StudentHealthDetails> lstStudentHealthDetails = new List<StudentHealthDetails>();
        foreach (ListViewItem obj in lstvwStudentHealthDetails.Items)
        {
            StudentHealthDetails oStudentHealthDetails = new StudentHealthDetails();
            TextBox txtAnswer = obj.FindControl("txtAnswer") as TextBox;
            oStudentHealthDetails.ParameterId = lstvwStudentHealthDetails.DataKeys[obj.DisplayIndex]["ParameterId"].ToInt();
            oStudentHealthDetails.Answer = txtAnswer.Text.Trim();
            lstStudentHealthDetails.Add(oStudentHealthDetails);
        }
        return base.GenerateXml(lstStudentHealthDetails);
    }

    /// <summary>
    /// Common method to process the ListView rows to set rowspan for repeating items.
    /// </summary>
    /// <param name="sControlId">Id of the control to check repetition.</param>
    /// <param name="iIndex">Index of the cell in the row, to set rowspan.</param>
    private void ProcessGrid(string sControlId, int iIndex, bool bSetClass)
    {
        string sContent = String.Empty;
        int iCount = 0;
        ListViewDataItem oCurrent = null;
        string sClassName = "ClsGridAltRow";

        foreach (ListViewDataItem item in lstvwStudentHealthDetails.Items)
        {
            Label lblLabel = item.FindControl(sControlId) as Label;
            if (lblLabel.Text == sContent)
            {
                iCount++;
                HtmlTableRow oHTMLCurrentRow = item.FindControl("trGridRow") as HtmlTableRow;
                oHTMLCurrentRow.Cells[iIndex].Style["display"] = "none";
                if (bSetClass)
                    oHTMLCurrentRow.Attributes["class"] = sClassName;
                lblLabel.Text = String.Empty;
                continue;
            }
            else
            {
                if (iCount != 0)
                {
                    HtmlTableRow oHTMLCurrentRow = oCurrent.FindControl("trGridRow") as HtmlTableRow;
                    oHTMLCurrentRow.Cells[iIndex].Attributes["rowspan"] = (iCount + 1).ToString();
                    iCount = 0;
                }

                sClassName = (sClassName == "ClsGridRow") ? "ClsGridAltRow" : "ClsGridRow";

                if (bSetClass)
                {
                    HtmlTableRow oHTMLTableRow = item.FindControl("trGridRow") as HtmlTableRow;
                    oHTMLTableRow.Attributes["class"] = sClassName;
                }

                oCurrent = item;
                sContent = lblLabel.Text;
            }
        }

        if (oCurrent != null)
        {
            HtmlTableRow oTableRow = oCurrent.FindControl("trGridRow") as HtmlTableRow;
            if (iCount != 0)
                oTableRow.Cells[iIndex].Attributes["rowspan"] = (iCount + 1).ToString();
            if (bSetClass)
                oTableRow.Attributes["class"] = sClassName;
        }
    }

    #endregion    
}