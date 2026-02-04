/*
 * File Name - TaskManagementPopup.aspx.xs
 * Creadted By - Vinod
 * Created Date - 9-Jun-2011
 * Description - This class is used to Add/Edit New Task.
*/

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using TaskManagementEntities;
using Utility;

/// <summary>
/// This class is used to setpartial leaves.
/// </summary>
public partial class TaskManagementPopup : SchoolBase
{
    #region Constants

    public const string S_DEFAULT_DATE_2 = "01/01/1900 12:00:00 AM";
    public const string S_DEFAULT_DATE_3 = "1/1/1900 12:00:00 AM";
    public const string S_DEFAULT_DATE_4 = "01-Jan-1900";

    #endregion

    #region Members

    //This variable is used while assining task to self.
    public static int miIncludeMe = 0;

    #endregion

    #region Events

    /// <summary>
    /// This event is used to fill User task listview and set query string.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                miIncludeMe = Constants.I_ZERO;
                ReadQuerystring();
                SetJavascriptAttributes();
                FillComboBoxes();
                SetAsscessLevel();
                txtTaskName.Focus(); 
                SetDefaultVisibility(false);                
            }
            SetCancelBtnAttribute();

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to bind data to listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwUserDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iRowId = oCurrentItem.DisplayIndex;
                CheckBox chkSelect = (CheckBox)oCurrentItem.FindControl("chkSelect");

                if (((TaskManagementEntities.DesignationwiseUserTaskDetails)(oCurrentItem.DataItem)).IsSelected.ToString() == Constants.I_ONE.ToString())
                    chkSelect.Checked = true;
                else
                    chkSelect.Checked = false;
                //Those  task get completed, can not editable.
                if (((TaskManagementEntities.DesignationwiseUserTaskDetails)(oCurrentItem.DataItem)).TaskStatusId.ToString() == Constants.I_FOUR.ToString())
                    chkSelect.Enabled = false;
                else
                    chkSelect.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set enability of controls after listview filled.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwUserDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwUserDetails.Items.Count == 0)
                SetDefaultVisibility(false);
            else
                SetDefaultVisibility(true);

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///  This event is used show Assigned Task details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwUserDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "DETAIL")
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                int iUserId = Convert.ToInt32(lstvwUserDetails.DataKeys[iRowId]["UserId"]);
                int iDesignationId = Convert.ToInt32(lstvwUserDetails.DataKeys[iRowId]["DesignationId"]);
                HtmlTableRow oHtmlTableRow = e.Item.FindControl("trUserTaskDetails") as HtmlTableRow;
                HtmlTableRow oHtmlTableCell = e.Item.FindControl("tdUserTaskDetails") as HtmlTableRow;
                ListView olstvwUserTaskDetails = e.Item.FindControl("lstvwUserTaskDetails") as ListView;
                oHtmlTableRow.Visible = true;
                TaskManagementBL oTaskManagementBL = new TaskManagementBL(miSchoolId, miAcademicYearId);
                olstvwUserTaskDetails.DataSource = oTaskManagementBL.GetUserTaskDetails(iUserId, iDesignationId);
                olstvwUserTaskDetails.DataBind();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill listview as per designation selection.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbDesignation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbDesignation.SelectedIndex != Constants.I_ZERO)
            {
                SetDefaultVisibility(true);
                FillUserListView();
                HtmlTableRow oHtmlTableRow = (HtmlTableRow)lstvwUserDetails.FindControl("trHeader");
                CheckBox chkSelectAll;
                if (oHtmlTableRow != null)
                {
                    chkSelectAll = oHtmlTableRow.FindControl("ChkSelectAll") as CheckBox;
                    chkSelectAll.Checked = false;
                }
            }
            else
                SetDefaultVisibility(false);

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to close the users task details listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnCancelTask_Click(object sender, EventArgs e)
    {
        try
        {
            Button oButton = sender as Button;
            oButton.Parent.Parent.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to contol enability as per the task type.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optDailyTask_OnCheckedChanged(object sender, EventArgs e)
    {
        Button oButton = ((Button)wizard_TaskDetails.WizardSteps[0].FindControl("StartNavigationTemplateContainerID").FindControl("StartNextButton"));
        if (oButton != null)
        {
            if (hidTaskId.Value == Constants.I_ZERO.ToString())
            {
                cmbStatus.Enabled = false;
                txtComment.Enabled = false;
                cmbDesignation.Enabled = true;
            }
            else
            {
                cmbStatus.Enabled = true;
                txtComment.Enabled = true;
            }
        }
        Button oButtonNext = (Button)wizard_TaskDetails.WizardSteps[1].FindControl("StepNavigationTemplateContainerID").FindControl("StepNextButton");
        if (oButtonNext != null)
            oButtonNext.Attributes.Add("onclick", "if(!CheckAtLeastOne()){return false;}");
    }

    /// <summary>
    /// This event is used to contol enability as per the task type.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optNormalTask_OnCheckedChanged(object sender, EventArgs e)
    {
        Button oButton = ((Button)wizard_TaskDetails.WizardSteps[0].FindControl("StartNavigationTemplateContainerID").FindControl("StartNextButton"));
        if (oButton != null)
        {
            if (hidTaskId.Value == Constants.I_ZERO.ToString())
            {
                cmbStatus.Enabled = false;
                txtComment.Enabled = false;
                cmbDesignation.Enabled = true;
            }
            else
            {
                cmbStatus.Enabled = true;
                txtComment.Enabled = true;
            }
        }
        Button oButtonNext = (Button)wizard_TaskDetails.WizardSteps[1].FindControl("StepNavigationTemplateContainerID").FindControl("StepNextButton");
        if (oButtonNext != null)
            oButtonNext.Attributes.Add("onclick", "if(!CheckAtLeastOne()){return false;}");
    }

    /// <summary>
    /// This event is used to contol enability as per the task type.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optGeneralTask_OnCheckedChanged(object sender, EventArgs e)
    {
        Button oButton = ((Button)wizard_TaskDetails.WizardSteps[0].FindControl("StartNavigationTemplateContainerID").FindControl("StartNextButton"));
        if (oButton != null)
        {
            //Add Mode
            if (hidTaskId.Value == Constants.I_ZERO.ToString())
            {
                cmbDesignation.Enabled = false;
                cmbStatus.Enabled = false;
                txtComment.Enabled = false;
                divUserListView.Visible = false;
            }
            else
            {
                cmbDesignation.Enabled = false;
                divUserListView.Visible = false;
                cmbStatus.Enabled = true;
                txtComment.Enabled = true;
            }
        }
        Button oButtonNext = (Button)wizard_TaskDetails.WizardSteps[1].FindControl("StepNavigationTemplateContainerID").FindControl("StepNextButton");
        if (oButtonNext != null)
            oButtonNext.Attributes.Add("onclick", "");
    }

    #region Inner List View(Task List View) events

    protected void lstvwUserTaskDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = oCurrentItem.DisplayIndex;
                Label lblStartDateTime = oCurrentItem.FindControl("lblStartDateTime") as Label;
                Label lblEndDateTime = oCurrentItem.FindControl("lblEndDateTime") as Label;
                lblStartDateTime.Text = ((TaskManagementEntities.UserAssignedTaskDetails)(oCurrentItem.DataItem)).StartDate.ToString("dd-MMM-yyyy")
                            + " " + ((TaskManagementEntities.UserAssignedTaskDetails)(oCurrentItem.DataItem)).StartTime;
                lblEndDateTime.Text = ((TaskManagementEntities.UserAssignedTaskDetails)(oCurrentItem.DataItem)).EndDate.ToString("dd-MMM-yyyy")
                            + " " + ((TaskManagementEntities.UserAssignedTaskDetails)(oCurrentItem.DataItem)).EndTime;
            }
        }

        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #endregion

    #region Wizard's Event

    /// <summary>
    /// This event is used to set Default control on Wizard.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void wizard_TaskDetails_ActiveStepChanged(object sender, EventArgs e)
    {
        try
        {
            if (wizard_TaskDetails.ActiveStep == WizardStep1)
                SetNewTaskDetailsAttributes();
            if (wizard_TaskDetails.ActiveStep == WizardStep2)
                SetUserDetailsAttributes();
            if (wizard_TaskDetails.ActiveStep == WizardStep3)
                SetStatusDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set next button click and also check duplication of Task Title.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void wizard_TaskDetails_NextButtonClick(object sender, WizardNavigationEventArgs e)
    {
        try
        {
            Button oButton = (Button)wizard_TaskDetails.WizardSteps[1].FindControl("StepNavigationTemplateContainerID").FindControl("StepNextButton");
            if (hidTaskId.Value == Constants.I_ZERO.ToString())
            {
                if (optDailyTask.Checked || optNormalTask.Checked)
                {
                    if (oButton != null)
                        oButton.Attributes.Add("onclick", "if(!CheckAtLeastOne()){return false;}");
                }
                if (optGeneralTask.Checked)
                {
                    cmbDesignation.SelectedValue = Constants.I_ZERO.ToString();
                    cmbDesignation.Enabled = false;
                    if (oButton != null)
                        oButton.Attributes.Add("onclick", "");
                }
                cmbStatus.Enabled = false;
                txtComment.Enabled = false;
                cmbStatus.SelectedValue = Constants.I_ONE.ToString();
            }
            else
            {
                if (optGeneralTask.Checked)
                {
                    cmbDesignation.SelectedValue = Constants.I_ZERO.ToString();
                    divUserListView.Visible = false;
                    cmbDesignation.Enabled = false;
                    cmbStatus.Enabled = true;
                    txtComment.Enabled = true;
                    if (oButton != null)
                        oButton.Attributes.Add("onclick", "");
                }
                else
                {
                    cmbDesignation.Enabled = true;
                    if (oButton != null)
                        oButton.Attributes.Add("onclick", "if(!CheckAtLeastOne()){return false;}");
                }
                //When Self task is assigned.
                if (hidTaskId.Value != Constants.I_ZERO.ToString())
                    if (!chkIncludeMe.Checked)
                        chkIncludeMe.Checked = (miIncludeMe == Constants.I_ONE) ? true : false;
                miIncludeMe = 0;
            }
        }
        catch (BusinessLogic.Exceptions.DuplicateEntityException Ex)
        {
            lblMessage.Visible = true;
            lblMessage.Text = Ex.ErrorMessage;
            wizard_TaskDetails.ActiveStepIndex = Constants.I_ZERO;
            e.Cancel = true;
            txtTaskName.Focus();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save Task details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void wizard_TaskDetails_FinishButtonClick(object sender, WizardNavigationEventArgs e)
    {
        try
        {
            TaskManagementBL oTaskManagementBL = new TaskManagementBL();
            oTaskManagementBL.SaveUserTaskDetails(GenerateXML());
            CloseWindow();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Wizard Methods

    /// <summary>
    /// This method is used to set attributes on task details screen.
    /// </summary>
    private void SetNewTaskDetailsAttributes()
    {
        if (wizard_TaskDetails.WizardSteps[0].FindControl("StartNavigationTemplateContainerID") != null)
        {
            Button oButton = ((Button)wizard_TaskDetails.WizardSteps[0].FindControl("StartNavigationTemplateContainerID").FindControl("StartNextButton"));
            if (oButton != null)
            {
                ApplyMouseHoverEffect(new List<Button>{oButton});
            }
            Button CancelButton = ((Button)wizard_TaskDetails.WizardSteps[0].FindControl("StartNavigationTemplateContainerID").FindControl("CancelButton"));
            if (CancelButton != null)
            {
                string sQueryString = "TaskDetailId=" + hidTaskDetailId.Value +
                               "&TaskId=" + hidTaskId.Value +
                               "&TaskAssignerUserId=" + hidTaskAssignerUserId.Value +
                               "&TaskStatusId=" + hidStatusId.Value +
                               "&TaskTypeId=" + hidTaskTypeId.Value;

                sQueryString = CommonUtility.EncryptQuerystring(sQueryString);
                sQueryString = "?" + sQueryString + "";
                CancelButton.Attributes.Add("onclick", "window.close();");
                ApplyMouseHoverEffect(new List<Button> { CancelButton });

            }
        }
    }

    /// <summary>
    /// This method is used to set attributes on task details screen.
    /// </summary>
    private void SetUserDetailsAttributes()
    {
        if (wizard_TaskDetails.WizardSteps[1].FindControl("StepNavigationTemplateContainerID") != null)
        {
            Button oButton = (Button)wizard_TaskDetails.WizardSteps[1].FindControl("StepNavigationTemplateContainerID").FindControl("StepNextButton");
            if (oButton != null)
            {
                ApplyMouseHoverEffect(new List<Button> { oButton });
            }
            Button CancelButton = (Button)wizard_TaskDetails.WizardSteps[1].FindControl("StepNavigationTemplateContainerID").FindControl("CancelButton");
            if (oButton != null)
            {
                CancelButton.Attributes.Add("onclick", "window.close();");
                ApplyMouseHoverEffect(new List<Button> { CancelButton });

            }
            Button oBtnFinish = (Button)wizard_TaskDetails.WizardSteps[1].FindControl("StepNavigationTemplateContainerID").FindControl("StepPreviousButton");
            if (oBtnFinish != null)
            {
                ApplyMouseHoverEffect(new List<Button> { oBtnFinish });
            }
        }
    }

    /// <summary>
    /// This method is used to set attributes on Status details screen.
    /// </summary>
    private void SetStatusDetails()
    {
        cmbStatus.Focus();        
        Button oBtnFinish = (Button)wizard_TaskDetails.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishButton");

        if (oBtnFinish != null)
        {
            oBtnFinish.Attributes.Add("onclick", "if(!CheckAtLeastOneStatus()){return false;}");
            ApplyMouseHoverEffect(new List<Button> { oBtnFinish });
        }
        Button oButton = (Button)wizard_TaskDetails.FindControl("FinishNavigationTemplateContainerID").FindControl("CancelButton");
        if (oButton != null)
        {
            string sQueryString = "TaskDetailId=" + hidTaskDetailId.Value +
                             "&TaskId=" + hidTaskId.Value +
                             "&TaskAssignerUserId=" + hidTaskAssignerUserId.Value +
                             "&TaskStatusId=" + hidStatusId.Value +
                             "&TaskTypeId=" + hidTaskTypeId.Value;

            sQueryString = CommonUtility.EncryptQuerystring(sQueryString);
            sQueryString = "?" + sQueryString + "";
            oButton.Attributes.Add("onclick", "window.close();");
           ApplyMouseHoverEffect(new List<Button> { oButton });
        }

        oButton = (Button)wizard_TaskDetails.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishPreviousButton");
        if (oButton != null)
        {
            ApplyMouseHoverEffect(new List<Button> { oButton });
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// This method is used to close the current popup.
    /// </summary>
    private void CloseWindow()
    {
        string sQueryString = "TaskDetailId=" + hidTaskDetailId.Value +
                              "&TaskId=" + hidTaskId.Value +
                              "&TaskAssignerUserId=" + hidTaskAssignerUserId.Value +
                              "&TaskStatusId=" + hidStatusId.Value +
                              "&TaskTypeId=" + hidTaskTypeId.Value;

        sQueryString = CommonUtility.EncryptQuerystring(sQueryString);
        sQueryString = "?" + sQueryString + "";
        Button oBtnFinish = (Button)wizard_TaskDetails.WizardSteps[1].FindControl("FinishNavigationTemplateContainerID").FindControl("FinishButton");
        ScriptManager.RegisterStartupScript(oBtnFinish, this.GetType(), "CloseWin", "CloseWindow('" + sQueryString + "');", true);
    }

    /// <summary>
    /// Thois method is used to det default visibility.
    /// </summary>
    /// <param name="abFlag"></param>
    private void SetDefaultVisibility(bool abFlag)
    {
        if (!IsPostBack)
        {
            if (cmbDesignation.SelectedIndex == Constants.I_ZERO)
            {
                divUserListView.Visible = false;
                trNoRecordMsg.Visible = false;
            }
        }
        else
        {
            divUserListView.Visible = abFlag;
            trNoRecordMsg.Visible = !abFlag;
        }
    }

    /// <summary>
    /// This method is used to generate xml of User Task details.
    /// </summary>
    /// <returns></returns>
    private string GenerateXML()
    {
        const string S_ELEMENT = "element";
        string sAttribute;
        int iTaskId = Constants.I_ZERO;
        XmlDocument oXmlDocument = new XmlDocument();

        // Create a root level element.
        XmlElement rootElement = oXmlDocument.CreateElement("UserAssignedTaskDetails");
        XmlNode oXmlRootNode = oXmlDocument.CreateNode(S_ELEMENT, "UserAssignedTaskDetails", "");
        string sUserId = string.Empty;
        // Loop through all the grid rows.
        for (int iRowCount = Constants.I_ZERO; iRowCount < lstvwUserDetails.Items.Count; iRowCount++)
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)lstvwUserDetails.Items[iRowCount];

            CheckBox chkSelect = oCurrentItem.FindControl("ChkSelect") as CheckBox;
            if (chkSelect.Checked)
            {
                if (sUserId == string.Empty)
                    sUserId = Convert.ToString(lstvwUserDetails.DataKeys[iRowCount]["UserId"]);
                else
                    sUserId += ", " + Convert.ToString(lstvwUserDetails.DataKeys[iRowCount]["UserId"]);
            }
        }
        if (chkIncludeMe.Checked)
        {
            if (sUserId == string.Empty)
                sUserId = Convert.ToString(miUserId);
            else
                sUserId += ", " + Convert.ToString(miUserId);
        }
        else
        {
            if (sUserId == string.Empty)
                sUserId = Convert.ToString(miUserId);           
        }
        XmlNode oXmlNode = oXmlDocument.CreateNode(S_ELEMENT, "UserTaskDetails", "");

        sAttribute = "TaskDetailId";
        XmlAttribute oXmlAttribute = oXmlDocument.CreateAttribute(sAttribute);
        oXmlAttribute.Value = hidTaskDetailId.Value;
        oXmlNode.Attributes.Append(oXmlAttribute);

        sAttribute = "TaskDetails";
        oXmlAttribute = oXmlDocument.CreateAttribute(sAttribute);
        oXmlAttribute.Value = txtTaskDetails.Text;
        oXmlNode.Attributes.Append(oXmlAttribute);

        sAttribute = "TaskId";
        oXmlAttribute = oXmlDocument.CreateAttribute(sAttribute);
        oXmlAttribute.Value = hidTaskId.Value;
        oXmlNode.Attributes.Append(oXmlAttribute);

        sAttribute = "TaskName";
        oXmlAttribute = oXmlDocument.CreateAttribute(sAttribute);
        oXmlAttribute.Value = txtTaskName.Text;
        oXmlNode.Attributes.Append(oXmlAttribute);

        sAttribute = "UserId";
        oXmlAttribute = oXmlDocument.CreateAttribute(sAttribute);
        oXmlAttribute.Value = sUserId.ToString();
        oXmlNode.Attributes.Append(oXmlAttribute);

        sAttribute = "StartDate";
        oXmlAttribute = oXmlDocument.CreateAttribute(sAttribute);
        oXmlAttribute.Value = txtStartDate.Text;
        oXmlNode.Attributes.Append(oXmlAttribute);

        sAttribute = "StartTime";
        oXmlAttribute = oXmlDocument.CreateAttribute(sAttribute);
        oXmlAttribute.Value = txtStartTime.Text;
        oXmlNode.Attributes.Append(oXmlAttribute);

        sAttribute = "EndDate";
        oXmlAttribute = oXmlDocument.CreateAttribute(sAttribute);
        oXmlAttribute.Value = txtEndDate.Text;
        oXmlNode.Attributes.Append(oXmlAttribute);

        sAttribute = "EndTime";
        oXmlAttribute = oXmlDocument.CreateAttribute(sAttribute);
        oXmlAttribute.Value = txtEndTime.Text;
        oXmlNode.Attributes.Append(oXmlAttribute);

        sAttribute = "BufferDate";
        oXmlAttribute = oXmlDocument.CreateAttribute(sAttribute);
        oXmlAttribute.Value = txtBufferDate.Text;
        oXmlNode.Attributes.Append(oXmlAttribute);

        sAttribute = "BufferTime";
        oXmlAttribute = oXmlDocument.CreateAttribute(sAttribute);
        oXmlAttribute.Value = txtBufferTime.Text;
        oXmlNode.Attributes.Append(oXmlAttribute);

        sAttribute = "TaskTypeId";
        oXmlAttribute = oXmlDocument.CreateAttribute(sAttribute);
        if (optDailyTask.Checked)
            iTaskId = Constants.I_ONE;
        else if (optGeneralTask.Checked)
            iTaskId = Constants.I_THREE;
        else
            iTaskId = Constants.I_TWO;
        oXmlAttribute.Value = iTaskId.ToString();
        oXmlNode.Attributes.Append(oXmlAttribute);

        sAttribute = "TaskStatusId";
        oXmlAttribute = oXmlDocument.CreateAttribute(sAttribute);
        if (cmbStatus.Enabled == true && cmbStatus.SelectedValue != Constants.I_ZERO.ToString())
            oXmlAttribute.Value = cmbStatus.SelectedValue;
        else
            oXmlAttribute.Value = Constants.I_ONE.ToString();
        oXmlNode.Attributes.Append(oXmlAttribute);

        sAttribute = "Comment";
        oXmlAttribute = oXmlDocument.CreateAttribute(sAttribute);
        oXmlAttribute.Value = txtComment.Text;
        oXmlNode.Attributes.Append(oXmlAttribute);

        sAttribute = "SchoolId";
        oXmlAttribute = oXmlDocument.CreateAttribute(sAttribute);
        oXmlAttribute.Value = miSchoolId.ToString();
        oXmlNode.Attributes.Append(oXmlAttribute);

        sAttribute = "AcademicYearId";
        oXmlAttribute = oXmlDocument.CreateAttribute(sAttribute);
        oXmlAttribute.Value = miAcademicYearId.ToString();
        oXmlNode.Attributes.Append(oXmlAttribute);

        sAttribute = "InsertedById";
        oXmlAttribute = oXmlDocument.CreateAttribute(sAttribute);
        oXmlAttribute.Value = miUserId.ToString();
        oXmlNode.Attributes.Append(oXmlAttribute);

        sAttribute = "AssignerUserId";
        oXmlAttribute = oXmlDocument.CreateAttribute(sAttribute);
        oXmlAttribute.Value = hidTaskAssignerUserId.Value;
        oXmlNode.Attributes.Append(oXmlAttribute);

        sAttribute = "AssignedTypeFlag";
        oXmlAttribute = oXmlDocument.CreateAttribute(sAttribute);
        oXmlAttribute.Value = hidFlag.Value;
        oXmlNode.Attributes.Append(oXmlAttribute);

        // Add the node to root node.
        oXmlRootNode.AppendChild(oXmlNode);

        rootElement.AppendChild(oXmlRootNode);
        return rootElement.InnerXml;

    }

    /// <summary>
    /// This method is used to fill comboboxes.
    /// </summary>
    public void FillComboBoxes()
    {
        FillDesignationCombobox();
        FillStatusCombobox();
    }

    /// <summary>
    /// This method is used to fill designation combobox.
    /// </summary>
    private void FillDesignationCombobox()
    {
        TaskManagementBL oTaskManagementBL = new TaskManagementBL(miSchoolId, miAcademicYearId);
        int iUserId = Constants.I_ZERO; int iUserRoleId = Constants.I_ZERO;
        if (hidTaskId.Value != Constants.I_ZERO.ToString())
        {
            if (Convert.ToInt32(hidTaskAssignerUserId.Value) ==miUserId)
            {
                iUserId = miUserId;
                iUserRoleId = moUserRole.ToInt();
            }
            else
            {
                iUserId = Convert.ToInt32(hidTaskAssignerUserId.Value);
                iUserRoleId = Constants.I_ZERO;
            }
        }
        else
        {
            iUserId = miUserId;
            iUserRoleId = moUserRole.ToInt();
        }

        ControlUtility.FillDropDownList(oTaskManagementBL.GetDesignationsDetails(iUserId, iUserRoleId, Constants.I_ONE), ref cmbDesignation, "Value_Member", "Display_Member", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill Task Status combobox.
    /// </summary>
    private void FillStatusCombobox()
    {
        TaskManagementBL oTaskManagementBL = new TaskManagementBL(miSchoolId, miAcademicYearId);
        ControlUtility.FillDropDownList(oTaskManagementBL.GetTaskStatusDetails(Convert.ToInt32(hidTaskId.Value), Convert.ToInt32(hidTaskTypeId.Value), Convert.ToInt32(hidTaskDetailId.Value), Convert.ToInt32(hidTaskAssignerUserId.Value), miUserId, Convert.ToInt32(hidFlag.Value)), ref cmbStatus, "TaskStatusId", "StatusName", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        valSumTaskDetails.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        valsumUserDetails.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        valsumStatusDetails.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        optGeneralTask.Visible = false;
    }

    /// <summary>
    /// This method is used to fill user list view.
    /// </summary>
    private void FillUserListView()
    {
        divUserListView.Visible = true;
        TaskManagementBL oTaskManagementBL = new TaskManagementBL(miSchoolId, miAcademicYearId);
        List<DesignationwiseUserTaskDetails> lstUserAssignedTaskDetail = oTaskManagementBL.GetDesignationwiseUserList(Convert.ToInt32(cmbDesignation.SelectedValue), Convert.ToInt32(hidTaskDetailId.Value), Convert.ToInt32(hidTaskId.Value), Convert.ToInt32(hidTaskTypeId.Value),miAcademicYearId,Convert.ToInt32(hidFlag.Value));
        lstvwUserDetails.DataSource = lstUserAssignedTaskDetail;
        lstvwUserDetails.DataBind();
        hidRowCount.Value = lstvwUserDetails.Items.Count.ToString();
        if(lstUserAssignedTaskDetail.Count!=0)
        if (lstUserAssignedTaskDetail[0].IsLoggedUser.ToString() == Constants.I_ONE.ToString())
            miIncludeMe = Constants.I_ONE;
    }

    /// <summary>
    /// This method is used to read query string.
    /// </summary>
    /// <returns></returns>
    private void ReadQuerystring()
    {
	    if (QueryString.Count <= 0)
		    return;
	    
		if (QueryString["TaskDetailId"] != null)
		    hidTaskDetailId.Value = QueryString["TaskDetailId"];
	    if (QueryString["TaskId"] != null)
		    hidTaskId.Value = QueryString["TaskId"];
	    if (QueryString["TaskAssignerUserId"] != null)
		    hidTaskAssignerUserId.Value = QueryString["TaskAssignerUserId"];
	    if (!IsPostBack && QueryString["TaskStatusId"] != null)
		    hidStatusId.Value = QueryString["TaskStatusId"];
	    if (QueryString["TaskTypeId"] != null)
		    hidTaskTypeId.Value = QueryString["TaskTypeId"];
	    if (QueryString["Flag"] != null)
		    hidFlag.Value = QueryString["Flag"];
    }

    /// <summary>
    /// This method is used to set cancel button attribute.
    /// </summary>
    private void SetCancelBtnAttribute()
    {
        if (wizard_TaskDetails.WizardSteps[0].FindControl("StartNavigationTemplateContainerID") != null)
        {
            Button CancelButton = ((Button)wizard_TaskDetails.WizardSteps[0].FindControl("StartNavigationTemplateContainerID").FindControl("CancelButton"));
            if (CancelButton != null)
            {
                string sQueryString = "TaskDetailId=" + hidTaskDetailId.Value +
                               "&TaskId=" + hidTaskId.Value +
                               "&TaskAssignerUserId=" + hidTaskAssignerUserId.Value +
                               "&TaskStatusId=" + hidStatusId.Value +
                               "&TaskTypeId=" + hidTaskTypeId.Value;

                sQueryString = CommonUtility.EncryptQuerystring(sQueryString);
                sQueryString = "?" + sQueryString + "";
                CancelButton.Attributes.Add("onclick", "window.close();");
            }
        }
    }

    /// <summary>
    /// This method is used to set access leve of steps.
    /// </summary>
    private void SetAsscessLevel()
    {
        //Assigned task (AssignedBy Others)
        if ((hidTaskId.Value != Constants.I_ZERO.ToString() && hidTaskAssignerUserId.Value !=miUserId .ToString()) || (hidTaskId.Value != Constants.I_ZERO.ToString() && hidFlag.Value == Constants.I_TWO.ToString()))
        {
            wizard_TaskDetails.ActiveStepIndex = 2;
            Button oBtnFinish = (Button)wizard_TaskDetails.WizardSteps[1].FindControl("FinishNavigationTemplateContainerID").FindControl("FinishPreviousButton");
            if (oBtnFinish != null)
                oBtnFinish.Enabled = false;
            cmbStatus.Enabled = true;
            txtComment.Enabled = true;
            FillAllControls();
            SetEnability();
        }
        //Assigner Task (AssignedTo Others)
        else if (hidTaskId.Value != Constants.I_ZERO.ToString() && hidTaskAssignerUserId.Value == miUserId.ToString())
        {
            wizard_TaskDetails.ActiveStepIndex = Constants.I_ZERO;
            FillAllControls();
            SetEnability();
        }
        else
            wizard_TaskDetails.ActiveStepIndex = Constants.I_ZERO;
    }

    /// <summary>
    /// This method is used to set enability of controls.
    /// </summary>
    private void SetEnability()
    {
        if (hidTaskId.Value != Constants.I_ZERO.ToString())
        {
            //DAILY TASK
            if (hidTaskTypeId.Value == Constants.I_ONE.ToString())
            {
                optDailyTask.Enabled = true;
                optNormalTask.Enabled = false;
                optGeneralTask.Enabled = false;
            }
            //NORMAL TASK
            if (hidTaskTypeId.Value == Constants.I_TWO.ToString())
            {
                optDailyTask.Enabled = false;
                optNormalTask.Enabled = true;
                optGeneralTask.Enabled = false;
            }
            //GENERAL TASK
            if (hidTaskTypeId.Value == Constants.I_THREE.ToString())
            {
                optDailyTask.Enabled = false;
                optNormalTask.Enabled = false;
                optGeneralTask.Enabled = true;
            }
        }
    }

    /// <summary>
    /// This method is used to fill all cotrols.
    /// </summary>
    private void FillAllControls()
    {
        TaskManagementBL oTaskManagementBL = new TaskManagementBL(miSchoolId, miAcademicYearId);
        oTaskManagementBL.GetTaskDetails(Convert.ToInt32(hidTaskId.Value), Convert.ToInt32(hidTaskDetailId.Value), Convert.ToInt32(hidTaskAssignerUserId.Value), Convert.ToInt32(hidTaskTypeId.Value), miUserId);

        txtTaskName.Text = oTaskManagementBL.lstUserAssignedTaskDetails[0].TaskName;
        txtTaskDetails.Text = oTaskManagementBL.lstUserAssignedTaskDetails[0].TaskDetails;
        txtStartDate.Text = Convert.ToDateTime(oTaskManagementBL.lstUserAssignedTaskDetails[0].StartDate).ToString("dd-MMM-yyyy");
        txtStartTime.Text = oTaskManagementBL.lstUserAssignedTaskDetails[0].StartTime;
        txtEndDate.Text = Convert.ToDateTime(oTaskManagementBL.lstUserAssignedTaskDetails[0].EndDate).ToString("dd-MMM-yyyy");
        txtEndTime.Text = oTaskManagementBL.lstUserAssignedTaskDetails[0].EndTime;
        if (Convert.ToDateTime(oTaskManagementBL.lstUserAssignedTaskDetails[0].BufferDate).ToString("dd-MMM-yyyy") == S_DEFAULT_DATE_2 ||
            Convert.ToDateTime(oTaskManagementBL.lstUserAssignedTaskDetails[0].BufferDate).ToString("dd-MMM-yyyy") == S_DEFAULT_DATE_3 ||
            Convert.ToDateTime(oTaskManagementBL.lstUserAssignedTaskDetails[0].BufferDate).ToString("dd-MMM-yyyy") == S_DEFAULT_DATE_4)
            txtBufferDate.Text = string.Empty;
        else
            txtBufferDate.Text = Convert.ToDateTime(oTaskManagementBL.lstUserAssignedTaskDetails[0].BufferDate).ToString("dd-MMM-yyyy");
        txtBufferTime.Text = oTaskManagementBL.lstUserAssignedTaskDetails[0].BufferTime;
        if (oTaskManagementBL.lstUserAssignedTaskDetails[0].TaskTypeId == Constants.I_ONE)
        {
            optDailyTask.Checked = true;
            optGeneralTask.Checked = false;
            optNormalTask.Checked = false;
        }
        else if (oTaskManagementBL.lstUserAssignedTaskDetails[0].TaskTypeId == Constants.I_TWO)
        {
            optDailyTask.Checked = false;
            optGeneralTask.Checked = false;
            optNormalTask.Checked = true;
        }
        else if (oTaskManagementBL.lstUserAssignedTaskDetails[0].TaskTypeId == Constants.I_THREE)
        {
            optDailyTask.Checked = false;
            optGeneralTask.Checked = true;
            optNormalTask.Checked = false;
        }

        cmbDesignation.SelectedValue = oTaskManagementBL.lstUserAssignedTaskDetails[0].DesignationId.ToString();
        if (cmbDesignation.SelectedValue != Constants.I_ZERO.ToString())
            FillUserListView();
        chkIncludeMe.Checked = (cmbDesignation.SelectedValue == Constants.I_ZERO.ToString() && oTaskManagementBL.lstUserAssignedTaskDetails[0].IsLoggedUser == 1) ? true : false;
        cmbStatus.SelectedValue = oTaskManagementBL.lstUserAssignedTaskDetails[0].TaskStatusId.ToString();
        txtCommentDetails.Text = oTaskManagementBL.lstUserAssignedTaskDetails[0].Comments.Replace("\\n", "\n");

        if (Convert.ToDateTime(oTaskManagementBL.lstUserAssignedTaskDetails[0].TaskCompletedDate).ToString("dd-MMM-yyyy") != S_DEFAULT_DATE_2 ||
            Convert.ToDateTime(oTaskManagementBL.lstUserAssignedTaskDetails[0].TaskCompletedDate).ToString("dd-MMM-yyyy") != S_DEFAULT_DATE_3 ||
            Convert.ToDateTime(oTaskManagementBL.lstUserAssignedTaskDetails[0].TaskCompletedDate).ToString("dd-MMM-yyyy") != S_DEFAULT_DATE_4)
        {
            hidTaskCompletedDate.Value = Convert.ToDateTime(oTaskManagementBL.lstUserAssignedTaskDetails[0].TaskCompletedDate).ToString("dd-MMM-yyyy");
            hidTaskCompletedStTime.Value = oTaskManagementBL.lstUserAssignedTaskDetails[0].TaskCompletedStTime.ToString();
            hidTaskCompletedEndTime.Value = oTaskManagementBL.lstUserAssignedTaskDetails[0].TaskCompletedEndTime.ToString();
        }
        else
        {
            hidTaskCompletedDate.Value = string.Empty;
            hidTaskCompletedStTime.Value = string.Empty;
            hidTaskCompletedEndTime.Value = string.Empty;
        }

        if (Convert.ToInt32(oTaskManagementBL.lstUserAssignedTaskDetails[0].TaskCompletedCount).ToString() != Constants.I_ZERO.ToString())
            hidTaskCompletedCount.Value = oTaskManagementBL.lstUserAssignedTaskDetails[0].TaskCompletedCount.ToString();
        else
            hidTaskCompletedCount.Value = Constants.I_ZERO.ToString();

        hidStartDate.Value = Convert.ToDateTime(oTaskManagementBL.lstUserAssignedTaskDetails[0].StartDate).ToString("dd-MMM-yyyy");
        hidEndDate.Value = Convert.ToDateTime(oTaskManagementBL.lstUserAssignedTaskDetails[0].EndDate).ToString("dd-MMM-yyyy");

        hidStTime.Value = oTaskManagementBL.lstUserAssignedTaskDetails[0].StartTime.ToString();
        hidEndTime.Value = oTaskManagementBL.lstUserAssignedTaskDetails[0].EndTime.ToString();

    }

    #endregion

}