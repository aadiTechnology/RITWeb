using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;
using System.IO;
using PushNotificationService;


public partial class HomeworkDailyLogsUI : SchoolBase
{
    #region Constants

    private const string S_SAVE_MSG = " Log saved successfully !!!";
    private const string S_UPDATE_MSG = "Log updated successfully!!!";
    private const string S_DELETE_MSG = "Log deleted successfully !!!";
    private const string S_PUBLISH_MSG = "Log published successfully !!!";
    private const string S_UNPUBLISH_MSG = "Log Unpublished successfully !!!";
    private const int I_FILE_SIZE_LIMIT = 5242880; // nearly 5 mb
    private const string S_FILE_SIZE_ERROR_MESSAGE = "File size should not be greater than 5 MB.";
    private const string S_Homework_FOLDER_LOCATION = "RITeSchool\\DOWNLOADS\\Homework\\DailyLog\\";

    #endregion

    #region Data Member(s)

    private HomeworkDailyLogBL moHomeworkDailyLogBL = null;

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
                hidSortExpression.Value = "Date";
                hidSortDirection.Value = Constants.S_DESCENDING;
            }

            AddSortImage(lstvwHomeworklogs, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moHomeworkDailyLogBL = new HomeworkDailyLogBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                InitializeControls();
                FillHomeworkLogsDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ClearFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (this.IsValid)
            {
                SaveHomeworkDetails();

                if (btnSave.Text == Resources.LocalizedResources.Save)
                    lblMessage.Text = S_SAVE_MSG;
                else
                    lblMessage.Text = S_UPDATE_MSG;

                ClearFields();
                FillHomeworkLogsDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwHomeworklogs_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwHomeworklogs.Items.Count > Constants.I_ZERO)
                ControlUtility.FillListViewPagerFooter(lstvwHomeworklogs, DtPgCount);
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwHomeworklogs_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                Button btnPublish = oCurrentItem.FindControl("btnPublish") as Button;
                int iHomeWorkDailyId = Convert.ToInt32(lstvwHomeworklogs.DataKeys[e.Item.DisplayIndex]["Id"]);
                
                switch (e.CommandName)
                {
                    case Constants.S_COMMAND_UPDATE:
                        hidId.Value = iHomeWorkDailyId.ToString();

                        HomeworkDailyLog oHomework = moHomeworkDailyLogBL.Get(iHomeWorkDailyId);
                        btnSave.Text = Resources.LocalizedResources.Update;
                        txtDate.Text = oHomework.Date.ToString(Constants.S_DATE_FORMAT, new CultureInfo("en"));

                        string sPath = Constants.S_HOMEWORK_FOLDER_LOCATION + oHomework.AttachmentsName;
                        HyperLink lnkAttachment = oCurrentItem.FindControl("lnkAttachment") as HyperLink;
                        lnkAttachment.Attributes.Add("Onclick", "OpenFile('" + sPath + "'); return false;");

                        hidFileUpload.Value = oHomework.AttachmentsName;
                        break;
                    case Constants.S_COMMAND_PUBLISH:
                        if (btnPublish.Text == "Publish")
                        {
                            string sUserIds = moHomeworkDailyLogBL.Publish(iHomeWorkDailyId.ToString(), true);
                            lblMessage.Text = S_PUBLISH_MSG;

                            string sDate = (e.Item.FindControl("lblCompleteDt") as Label).Text;
                            SendNotification(sUserIds, sDate);
                        }
                        else
                        {
                            moHomeworkDailyLogBL.Publish(iHomeWorkDailyId.ToString(), false);
                            lblMessage.Text = S_UNPUBLISH_MSG;
                        }
                        FillHomeworkLogsDetails();
                        break;
                    case Constants.S_COMMAND_REMOVE:
                        {
                            moHomeworkDailyLogBL.Delete(iHomeWorkDailyId);
                            lblMessage.Text = S_DELETE_MSG;
                            FillHomeworkLogsDetails();
                            ClearFields();
                        }
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwHomeworklogs_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            SetSortVariables();
            hidSortExpression.Value = e.SortExpression;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwHomeworklogs_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                HomeworkDailyLog oHomeworkDailyLog = oCurrentItem.DataItem as HomeworkDailyLog;
                Label lbldate = oCurrentItem.FindControl("lblCompleteDt") as Label;
                lbldate.Text = oHomeworkDailyLog.Date.ToString(Constants.S_DATE_FORMAT);

                HyperLink lnkAttachment = oCurrentItem.FindControl("lnkAttachment") as HyperLink;
                string sPath = Constants.S_HOMEWORK_FOLDER_LOCATION +"DailyLog/" + oHomeworkDailyLog.AttachmentsName;
                lnkAttachment.NavigateUrl = Constants.S_HOMEWORK_FOLDER_LOCATION + oHomeworkDailyLog.AttachmentsName;
                lnkAttachment.Attributes.Add("Onclick", "OpenFile('" + sPath + "'); return false;");

                ImageButton btnDelete = oCurrentItem.FindControl("imgBtnDelete") as ImageButton;
                ImageButton btnEdit = oCurrentItem.FindControl("imgBtnEdit") as ImageButton;

                Button btnPublish = oCurrentItem.FindControl("btnPublish") as Button;
                if (((HomeworkDailyLog)oCurrentItem.DataItem).IsPublished)
                {
                    btnPublish.Text = "Unpublish";
                    btnDelete.Visible = btnEdit.Visible = false;
                }
                else
                {
                    btnPublish.Text = "Publish";
                    btnDelete.Visible = btnEdit.Visible = true;
                }
            }

        }

        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwHomeworklogs);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to show record in listivew as per filter.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillHomeworkLogsDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void Validate_Date(object obj, ServerValidateEventArgs e)
    {
        try
        {
            bool bIsValid = moHomeworkDailyLogBL.ValidateHomeworkDailyLog(miSchoolId, miAcademicYearId, txtDate.Text, hidStdDivId.Value.ToInt(), hidId.Value.ToInt());
            e.IsValid = bIsValid;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void btnCancel_Click1(object sender, EventArgs e)
    {
        try
        {
            ClearFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to initialize control values.
    /// </summary>
    private void InitializeControls()
    {
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        ApplyMouseHoverEffect(new List<Button> { imgbtnBack, btnSave, btnCancel });
        lblClassName.Text = QueryString["ClassName"].ToString();
        hidStdDivId.Value = QueryString["StdDivId"].ToString();
        hidServerDate.Value = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
    }

    private void FillHomeworkLogsDetails()
    {
        lstvwHomeworklogs.DataSourceID = objdsHomeworks.ID;
        lstvwHomeworklogs.DataBind();
    }

    private void ClearFields()
    {
        txtDate.Text = string.Empty;
        hidId.Value = Constants.S_ZERO;
        btnSave.Text = Resources.LocalizedResources.Save;
        hidFileUpload.Value = string.Empty;
    }

    /// <summary>
    /// This method is used to save homework details.
    /// </summary>
    private void SaveHomeworkDetails()
    {
        string sLinkName = CheckIsFileUploaded();
        
        HomeworkDailyLog oHomeworkDailyLog = new HomeworkDailyLog
        {
            Id = hidId.Value == string.Empty ? 0 : hidId.Value.ToInt(),
            Date = txtDate.Text.ToDateTime(),
        };

        moHomeworkDailyLogBL.Save(oHomeworkDailyLog, sLinkName, hidStdDivId.Value.ToInt());

    }

    /// This method is used to check Is file Uploaded or not.
    /// </summary>
    private string CheckIsFileUploaded()
    {
        string sFileName = string.Empty;
        if (flDocument.FileName != string.Empty)
        {
            string sServerPath = Server.MapPath("~");
            if (sServerPath.Substring(sServerPath.Length - 1) != "\\")
                sServerPath = sServerPath + "\\";

            string sLinkName = flDocument.FileName;

            if (File.Exists(sServerPath + S_Homework_FOLDER_LOCATION + sLinkName))
                sLinkName = CommonUtility.GetFileNameForRenaming(flDocument.FileName.ToString());

            string sLinkPath = sServerPath + S_Homework_FOLDER_LOCATION + sLinkName;

            if (flDocument.HasFile)
            {
                flDocument.SaveAs(sLinkPath);
                sFileName = sLinkName;
            }
        }
        else
            sFileName = hidFileUpload.Value;

        return sFileName;
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
    /// Add entry to notification table so service send notifiction to users selected for message.
    /// </summary>
    /// <param name="sUserIds"></param>
    /// <param name="sSubjectName"></param>
    public void SendNotification(string sUserIds, string asDate)
    {
        if (sUserIds != string.Empty)
        {
            PushNotificationClient pushNotificationClient = null;
            try
            {
                pushNotificationClient = new PushNotificationClient();
                string[] strArrayUserid = sUserIds.Split(',');
                int[] intArrayUserId = Array.ConvertAll(strArrayUserid, userId => int.Parse(userId));

                pushNotificationClient = new PushNotificationClient();
                Dictionary<string, string> dictionaryNotificationParameter = new Dictionary<string, string>();
                dictionaryNotificationParameter.Add(Constants.S_NOTIFICATION_PARAMETER_CLASSNAME, lblClassName.Text);
                dictionaryNotificationParameter.Add(Constants.S_NOTIFICATION_PARAMETER_DATE, asDate);                
                pushNotificationClient.SendNotification(NotificationMessageHeadings.DailyLogAssigned, this.miSchoolId.ToString(), intArrayUserId, dictionaryNotificationParameter);
                pushNotificationClient.Close();
            } 
            catch (Exception ex)
            {
                ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
            }
            finally
            {
                if (pushNotificationClient.State != System.ServiceModel.CommunicationState.Faulted)
                    pushNotificationClient.Close();
            }
        }
    }

    #endregion
    
}