using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using BusinessLogic;
using SchoolEntities;
using Utility;
using System.Xml;

public partial class AddCareerOpenings : SchoolBase
{
    private JobDetailsBL moJobDetailsBL;

    private const string S_DELETE_MSG = "Job deleted successfully !!!";
    private const string S_TEXT_SAVE = "Save";
    private const string S_TEXT_UPDATE = "Update";
    private const string S_SAVE_SELECTED_JOB = "Selected job(s) saved successfully !!!";
    private const string S_SAVE_STATEMENT = "Job details are saved successfully !!!";
    private const string S_UPDATE_STATEMENT = "Job details are updated successfully !!!";
    private const string S_COMMAND_DELETE_JOB = "DeleteCareerDetails";
    private const string S_COMMAND_UPDATE_JOB = "UpdateCareerDetails";

    /// <summary>
    /// This class is used to initialize job controls and binding data to listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            InitializeMemberVariables();
            moJobDetailsBL = new JobDetailsBL(miSchoolId, miUserId);
            if (!IsPostBack)
            {
                SetJavaScriptAttributes();
                FillJobDetailGridView();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to update and delete the jobs.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwCareerDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var oCurrentItem = e.Item as ListViewDataItem;
                var iRowId = oCurrentItem.DisplayIndex;
                var iJobId = Convert.ToInt32(lstvwCareerDetails.DataKeys[iRowId]["JobId"]);
                hidRowNo.Value = (oCurrentItem.DisplayIndex + 1).ToString();
                hidJobId.Value = iJobId.ToString();
                var oJobDetails = moJobDetailsBL.Get(iJobId);

                if (e.CommandName == S_COMMAND_UPDATE_JOB)
                {
                    if (oJobDetails != null)
                    {
                        txtJobTitle.Text = oJobDetails.JobTitle;
                        txtQualification.Text = oJobDetails.Qualification;
                        txtSortorder.Text = oJobDetails.SortOrder.ToString();
                        txtDescription.Text = oJobDetails.Description;
                        txtExperience.Text = oJobDetails.Experience.ToString();
                    }
                    btnSaveText.Text = S_TEXT_UPDATE;
                }
                else
                {
                    if (e.CommandName == S_COMMAND_DELETE_JOB)
                    {
                        moJobDetailsBL.Delete(iJobId);
                        FillJobDetailGridView();
                        ResetFields();
                        lblUpdateSucess.Text = S_DELETE_MSG;
                        lblUpdateSucess.Visible = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to save job details into database.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveText_Click(object sender, EventArgs e)
    {
        try
        {
                JobDetails oJobDetails;
                oJobDetails = Populate();
                moJobDetailsBL.Save(oJobDetails);
                FillJobDetailGridView();
                if (btnSaveText.Text == S_TEXT_SAVE)
                {
                    lblUpdateSucess.Text = S_SAVE_STATEMENT;
                    lblUpdateSucess.Visible = true;
                }
                else
                {
                    lblUpdateSucess.Text = S_UPDATE_STATEMENT;
                    btnSaveText.Text = S_TEXT_SAVE;
                    lblUpdateSucess.Visible = true;
                }

                ResetFields();
         }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to cancel the process.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancelText_Click(object sender, EventArgs e)
    {
        try
        {
            ResetFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to save the selected jobs to disaply on Career page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveSelected_Click(object sender, EventArgs e)
    {
        try
        {
            var lstJobId = new List<JobDetails>();
            for (var iCnt = 0; iCnt < lstvwCareerDetails.Items.Count; iCnt++)
            {
                var oJobDetails = new JobDetails();
                var chkSelect = lstvwCareerDetails.Items[iCnt].FindControl("chkSelect") as CheckBox;
                oJobDetails.IsSelected = chkSelect.Checked;
                oJobDetails.JobId = Convert.ToInt32(lstvwCareerDetails.DataKeys[iCnt]["JobId"]);
                oJobDetails.InsertedById = miUserId;
                lstJobId.Add(oJobDetails);
            }

            var sXml = CommonUtility.GenerateXml(lstJobId);
            moJobDetailsBL.SaveSelectedJob(sXml);
            lblUpdateSucess.Text = S_SAVE_SELECTED_JOB;
            lblUpdateSucess.Visible = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to fill Job details gridview.
    /// </summary>
    private void FillJobDetailGridView()
    {
        List<JobDetails> lstJobDetails = moJobDetailsBL.GetAll();
        if (lstJobDetails.Count == Constants.I_ZERO)
        {
            btnSaveSelected.Visible = false;
            lstvwCareerDetails.DataSource = lstJobDetails;
            lstvwCareerDetails.DataBind();
        }
        else
        {
            lstvwCareerDetails.DataSource = lstJobDetails;
            lstvwCareerDetails.DataBind();
            btnSaveSelected.Visible = true;
        }
        trSave.Visible = lstvwCareerDetails.Items.Count > Constants.I_ZERO;
    }

    /// <summary>
    /// This class is used to populate the object of JobDetails class.
    /// </summary>
    /// <returns></returns>
    private JobDetails Populate()
    {
        var oNewsDetails = new JobDetails
        {
            JobId = hidJobId.Value.ToInt(),
            JobTitle = txtJobTitle.Text,
            Qualification = txtQualification.Text,
            Description = txtDescription.Text,
            SortOrder = Convert.ToInt32(txtSortorder.Text),
            Experience = Convert.ToInt32(txtExperience.Text)
        };
        return oNewsDetails;
    }

    /// <summary>
    /// This method is used to set default control fields.
    /// </summary>
    private void ResetFields()
    {
        txtJobTitle.Text = string.Empty;
        txtQualification.Text = string.Empty;
        txtDescription.Text = string.Empty;
        txtSortorder.Text = string.Empty;
        txtExperience.Text = string.Empty;
        hidJobId.Value = Constants.S_ZERO;
        txtJobTitle.Focus();
        btnSaveText.Text = S_TEXT_SAVE;
    }

    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        valSumErrorMsgText.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        new Button[] { btnSaveText, btnSaveSelected, btnCancelText }.ApplyEffect();

    }
}