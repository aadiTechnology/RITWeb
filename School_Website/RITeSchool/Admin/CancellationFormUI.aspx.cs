using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using System.Web.Script.Serialization;
using Utility;
using SchoolEntities;
using System.Reflection;

public partial class CancellationFormUI : SchoolBase
{
    #region Constant(s)

    private const string S_TEXT_SAVE = "Save";
    private const string S_TEXT_UPDATE = "Update";
    private const string S_SAVE_MSG = "Cancellation Form details saved successfully !!!";
    private const string S_UPDATE_MSG = "Cancellation Form details updated successfully !!!";
    private const string S_DELETE_MSG = "Cancellation Form details deleted successfully !!!";
    private const string S_COMMAND_DELETE = "DeleteCancellationFormDetails";
    private const string S_COMMAND_UPDATE = "UpdateCancellationFormDetails";
    private const string S_COMMAND_SELECT = "SelectDetails";
    private const string S_APPLY_MSG = "Cancellation Form fee applied successfully !!!";

    #endregion

    #region Data Member(s)

    private CancellationFormBL moCancellationFormBL;

    #endregion

    #region Event(s)
    /// <summary>
    /// This event is used to add sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        try
        {
            if (hidSortExpression.Value == string.Empty)
            {
                hidSortExpression.Value = "Roll_No";
                hidSortDirection.Value = Constants.S_DESCENDING;
            }

            AddSortImage(lstvwStudents, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This event is used to display details at page load.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moCancellationFormBL = new CancellationFormBL(miSchoolId, miUserId, miAcademicYearId);
            if (!IsPostBack)
            {
                SetDefaultValues();
                FillStudentDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to select stuent details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwSearchStudentDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = oCurrentItem.DisplayIndex;
                int iSchoolwiseStudentId = Convert.ToInt32(lstvwSearchStudentDetails.DataKeys[iRowId]["SchoolWiseStudentId"]);
                hidSchoolwiseStudentId.Value = iSchoolwiseStudentId.ToString();

                int aiId = Convert.ToInt32(lstvwSearchStudentDetails.DataKeys[iRowId]["Id"]);
                hidId.Value = aiId.ToString();

                if (e.CommandName == S_COMMAND_SELECT)
                {
                    CancellationForm oCancellationForm = moCancellationFormBL.GetControlDetails(iSchoolwiseStudentId, aiId);
                    txtReason.Text = oCancellationForm.Reason;
                    txtRefundcheque.Text = oCancellationForm.RefundChequeInFavourOf;
                    txtCell.Text = oCancellationForm.Cell;
                    lblStudentName1.Text = oCancellationForm.StudentName;
                    hidId.Value = oCancellationForm.Id.ToString();
                    hidSchoolwiseStudentId.Value = oCancellationForm.SchoolWiseStudentId.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill page footer.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwSearchStudentDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwSearchStudentDetails.Items.Count > 0)
            {
                ControlUtility.FillListViewPagerFooter(lstvwSearchStudentDetails, DtPgCount);
            }
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to sort variables.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwSearchStudentDetails_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            SetSortVariables();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to select page no.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwSearchStudentDetails);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used for edit, delete functionality.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStudents_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = oCurrentItem.DisplayIndex;
                
                int aiSchoolwiseStudentId = Convert.ToInt32(lstvwStudents.DataKeys[iRowId]["SchoolwiseStudentId"].ToInt());
                hidSchoolwiseStudentId.Value = aiSchoolwiseStudentId.ToString();

                int iId = Convert.ToInt32(lstvwStudents.DataKeys[iRowId]["Id"]);
                hidId.Value = iId.ToString();

                if (e.CommandName == S_COMMAND_UPDATE)
                    SetControlsForEditMode(iId, aiSchoolwiseStudentId);
                
                else if (e.CommandName == S_COMMAND_DELETE)
                {
                    Delete(iId);
                    ResetFields();
                    FillStudentDetails();
                }
                else if (e.CommandName == "APPLY_FEE")
                {
                    int iStudentId = Convert.ToInt32(lstvwStudents.DataKeys[iRowId]["StudentId"].ToInt());
                    moCancellationFormBL.ApplyConcessionFormFee(iStudentId);
                    lblUpdate.Text = S_APPLY_MSG;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill student details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStudents_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                CancellationForm oCancellationForm = e.Item.DataItem as CancellationForm;
                ImageButton imgbtnDelete = e.Item.FindControl("imgbtnDelete") as ImageButton;

                imgbtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");

                int iId = Convert.ToInt32(lstvwStudents.DataKeys[e.Item.DisplayIndex]["Id"].ToString());
                int iStandardId = Convert.ToInt32(lstvwStudents.DataKeys[e.Item.DisplayIndex]["StandardId"].ToString());
                int iDivisionId = Convert.ToInt32(lstvwStudents.DataKeys[e.Item.DisplayIndex]["DivisionId"].ToString());
                int iStudentId = Convert.ToInt32(lstvwStudents.DataKeys[e.Item.DisplayIndex]["StudentId"].ToString());
                int iSubmittedBy = Convert.ToInt32(lstvwStudents.DataKeys[e.Item.DisplayIndex]["SubmittedBy"].ToString());
                string sQueryString = CommonUtility.EncryptQuerystring("CancFormId=" + iId + "&Standard_Id=" + iStandardId + "&Division_Id=" + iDivisionId + "&Student_Id=" + iStudentId + "&SubmittedBy=" + iSubmittedBy);

                HiddenField hidData1 = e.Item.FindControl("hidData1") as HiddenField;
                hidData1.Value = sQueryString;
                
                LinkButton lnkReport = e.Item.FindControl("lnkReport") as LinkButton;
                lnkReport.Attributes.Add("onclick", "OpenReport(" + e.Item.DisplayIndex + "); return false;");

             }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill pager footer.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStudents_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwStudents.Items.Count > 0)
            {
                ControlUtility.FillListViewPagerFooter(lstvwStudents, DtPgCountStudents);
            }
            else
                DtPgCountStudents.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to sort variables.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStudents_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            SetSortVariables();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to select page no.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwStudents);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Save();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to cancel details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ResetFields();
            FillStudentDetails();
            btnSave.Text = S_TEXT_SAVE;
            lblUpdate.Text = "";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
   
    /// <summary>
    /// This event is used to search student.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillSearchStudentDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to populate student details.
    /// </summary>
    /// <param name="iId"></param>
    /// <param name="aiSchoolwiseStudentId"></param>
    /// <returns></returns>
    private CancellationForm PopulateStudentDetails(int iId, int aiSchoolwiseStudentId)
    {
        CancellationForm oCancellationForm = new CancellationForm
        {
            Reason = txtReason.Text,
            RefundChequeInFavourOf = txtRefundcheque.Text,
            Cell = txtCell.Text,
            Id = iId,
            SchoolWiseStudentId = aiSchoolwiseStudentId
        };
        return oCancellationForm;
    }

    /// <summary>
    /// This method is used to save details.
    /// </summary>
    private void Save()
    {
        int iId = 0;
        if (hidId.Value != string.Empty)
        {
            iId = Convert.ToInt32(hidId.Value);
        }

        int aiSchoolwiseStudentId = Convert.ToInt32(hidSchoolwiseStudentId.Value);
        
        CancellationForm oCancellationForm = PopulateStudentDetails(iId, aiSchoolwiseStudentId);
        moCancellationFormBL.Save(oCancellationForm);
        if (btnSave.Text == S_TEXT_SAVE)
            lblUpdate.Text = S_SAVE_MSG;
        else
        {
            lblUpdate.Text = S_UPDATE_MSG;
            btnSave.Text = S_TEXT_SAVE;
        }
        hidId.Value = "0";

        FillStudentDetails();
        ResetFields();
    }

    /// <summary>
    /// This method is used to edit details.
    /// </summary>
    /// <param name="aiId"></param>
    /// <param name="aiSchoolwiseStudentId"></param>
    private void SetControlsForEditMode(int aiId, int aiSchoolwiseStudentId)
    {
        btnSave.Text = S_TEXT_UPDATE;
        hidId.Value = aiId.ToString();

        CancellationForm oCancellationForm = moCancellationFormBL.Get(aiId, aiSchoolwiseStudentId);
        txtReason.Text = oCancellationForm.Reason;
        txtRefundcheque.Text = oCancellationForm.RefundChequeInFavourOf;
        txtCell.Text = oCancellationForm.Cell;
        lblStudentName1.Text = oCancellationForm.StudentName;
     }

    /// <summary>
    /// This method is used to set sort variables.
    /// </summary>
    private void SetSortVariables()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to delete details.
    /// </summary>
    /// <param name="aiId"></param>
    private void Delete(int aiId)
    {
        moCancellationFormBL.Delete(aiId);
        lblUpdate.Text = S_DELETE_MSG;
    }
    
    /// <summary>
    /// This method is used to Search student listview.
    /// </summary>
    private void FillSearchStudentDetails()
    {
        lstvwSearchStudentDetails.DataSourceID = objdsSearchStudentDetails.ID;
        lstvwSearchStudentDetails.DataBind();
    }

    /// <summary>
    /// This method is used to fill student details listview.
    /// </summary>
    private void FillStudentDetails()
    {
        lstvwStudents.DataSourceID = ObjectDataSourceStudent.ID;
        lstvwStudents.DataBind();
    }

    /// <summary>
    /// This method is used to reset fields.
    /// </summary>
    private void ResetFields()
    {
        txtReason.Text = string.Empty;
        txtRefundcheque.Text = string.Empty;
        txtCell.Text = string.Empty;
        hidId.Value = "0";
        lblStudentName1.Text = string.Empty;
    }

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        ValSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
    }

    #endregion
}