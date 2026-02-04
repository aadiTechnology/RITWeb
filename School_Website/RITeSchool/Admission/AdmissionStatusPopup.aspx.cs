/*
 * File Name - AdmissionStatusPopup.aspx.cs
 * Created By - Sachin
 * Created Date - 2 Jan 215
 * Class Descriptin - This class is sued to add and display admission status and comments.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;

public partial class AdmissionStatusPopup : SchoolBase
{
    #region Data Member(s)

    private StudentAdmissionsBL moStudentAdmissionsBL;

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to set base class details.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnInit(EventArgs e)
    {
        try
        {
            base.OnInit(e);
            moStudentAdmissionsBL = new StudentAdmissionsBL(miSchoolId, miAcademicYearId, miUserId);

            if (Page.Request.Params.Get("__EVENTTARGET") != null)
            {
                if (btnSave.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")) ||
                    btnCancel.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")))
                    DisplayPreviousComments();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill status combo box and fill display previous comments.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SetJavascriptAttributes();
                FillStatusCombo();
                DisplayPreviousComments();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    //protected void imgBtnEdit_Click(object sender, ImageClickEventArgs e)
    //{
    //    try
    //    {
    //        int iLastCommentId = hidLastCommentId.Value.ToInt();
    //        AdmissionStatusDetails oAdmissionStatusDetails = moStudentAdmissionsBL.GetStatusComment(iLastCommentId);
    //        if (oAdmissionStatusDetails != null)
    //        {
    //            cmbStatus.SelectedValue = oAdmissionStatusDetails.StatusId.ToString();
    //            txtComment.Text = oAdmissionStatusDetails.Comment;
    //            txtFollowupDate.Text = oAdmissionStatusDetails.FollowUpDate.ToString(Constants.S_DATE_FORMAT);
    //            hidAdmissionStatusDetailsId.Value = oAdmissionStatusDetails.Id.ToString();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
    //    }
    //}

    //protected void imgBtnDelete_Click(object sender, ImageClickEventArgs e)
    //{
    //    try
    //    {
    //        int iLastCommentId = hidLastCommentId.Value.ToInt();
    //        moStudentAdmissionsBL.DeleteAdmissionStatusComment(iLastCommentId);
    //        if (hidAdmissionStatusDetailsId.Value == hidLastCommentId.Value)
    //            ClearFields();
    //        FillComments();
    //    }
    //    catch (Exception ex)
    //    {
    //        ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
    //    }
    //}

    /// <summary>
    /// This event is used to cancel current operation and clear controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
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

    /// <summary>
    /// This event is used to save admission status details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            AdmissionStatusDetails oAdmissionStatusDetails = new AdmissionStatusDetails
            {
                Id = Convert.ToInt32(hidAdmissionStatusDetailsId.Value),
                StudentAdmissionId = Convert.ToInt32(hidStudentAdmissionId.Value),
                Comment = txtComment.Text.Trim(),
                Date = Convert.ToDateTime(txtDate.Text),
                FollowUpDate = Convert.ToDateTime(txtFollowupDate.Text),
                StatusId = Convert.ToInt32(cmbStatus.SelectedValue)
            };

            moStudentAdmissionsBL.SaveAdmissionStatusDetails(oAdmissionStatusDetails);
            lblMessage.Text = "Admission status details saved successfully!!!";
            ClearFields();
            DisplayPreviousComments();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to fill status combo.
    /// </summary>
    private void FillStatusCombo()
    {
        List<AdmissionStatus> lstStatuses = moStudentAdmissionsBL.GetAllAdmissionStatuses();
        lstStatuses = lstStatuses.Where(st => st.Id != Constants.AdmissionStatus.Open.ToInt()).ToList();
        ListSource.FillDropDownList(lstStatuses, cmbStatus, "Name", "Id", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to display previous comments.
    /// </summary>
    private void DisplayPreviousComments()
    {
        AdmissionDetails oAdmissionDetails;

        if (hidStudentAdmissionId.Value.Trim() == string.Empty)
            hidStudentAdmissionId.Value = QueryString["StudentAdmissionId"].ToString();

        int iStudentAdmissionId = Convert.ToInt32(hidStudentAdmissionId.Value);
        List<AdmissionStatusDetails> lstDetails = moStudentAdmissionsBL.GetAllComments(iStudentAdmissionId, out oAdmissionDetails).OrderByDescending(st => st.Id).ToList();

        lblStudentName.Text = oAdmissionDetails.StudentName;
        lblFormNo.Text = oAdmissionDetails.FormNumber;
        lblCurrentStatus.Text = oAdmissionDetails.CurrentStatus;
        hidLastCommentId.Value = Constants.S_ZERO;

        AdmissionStatusDetails oAdmissionStatusDetails = lstDetails.OrderByDescending(st => st.Id).FirstOrDefault();
        if (oAdmissionStatusDetails != null)
            hidLastCommentId.Value = oAdmissionStatusDetails.Id.ToString();

        bool bIsAlternetRow = false;

        lstDetails.ForEach
            (
                status =>
                {
                    string sHeaderClassName = "ClsProgressGridTestHeader";
                    string sCellClassName = "ClsMarksCell";
                    if (bIsAlternetRow)
                    {
                        sHeaderClassName = "ClsReceiverHeader";
                        sCellClassName = "ClsReceiverCell";
                        bIsAlternetRow = false;
                    }
                    else
                        bIsAlternetRow = true;

                    HtmlTableRow trSubHeader = new HtmlTableRow();
                    base.AddCell(trSubHeader, "Date : " + status.Date.ToString(Constants.S_DATE_FORMAT + " hh:mm tt"), sHeaderClassName, "left", 1, "width:50%");
                    base.AddCell(trSubHeader, "Updated By : " + status.UpdatedBy, sHeaderClassName, "left");

                    tblComments.Rows.Add(trSubHeader);

                    HtmlTableRow trContent = new HtmlTableRow();
                    base.AddCell(trContent, status.Comment, sCellClassName, "left", 2);
                    tblComments.Rows.Add(trContent);

                    AddEmptyRow();
                }

            );

        if (lstDetails.Count == 0)
        {
            lblPreviousComments.Visible = false;
            //imgBtnEdit.Visible = false;
            //imgBtnDelete.Visible = false;
        }
        else
        {
            //imgBtnEdit.Visible = true;
            //imgBtnDelete.Visible = true;
            lblPreviousComments.Visible = true;
        }
    }

    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        hidStudentAdmissionId.Value = QueryString["StudentAdmissionId"].ToString();
        base.ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel, btnClose });
        txtDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        btnSave.Attributes.Add("onclick", "ClearMessage()");
        cmbStatus.Focus();
    }

    /// <summary>
    /// This method is used to add empty row.
    /// </summary>
    private void AddEmptyRow()
    {
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        AddCell(oHtmlTableRow, "<BR />", "ClsMarksCell", "Left", 3, "background-color:white");
        tblComments.Rows.Add(oHtmlTableRow);
    }

    /// <summary>
    /// This method is used to clear fields.
    /// </summary>
    private void ClearFields()
    {
        cmbStatus.ClearSelection();
        txtComment.Text = string.Empty;
        txtFollowupDate.Text = string.Empty;
        hidAdmissionStatusDetailsId.Value = Constants.S_ZERO;
    }

    #endregion
}