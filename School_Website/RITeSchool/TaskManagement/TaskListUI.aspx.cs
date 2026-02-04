using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using MasterEntities;
using TaskManagementEntities;
using Utility;

/// <summary>
/// This class is used to display list of tasks assigned to user or assigned by user.
/// </summary>
public partial class TaskListUI : SchoolBase
{
	#region "Constants"

	const string S_TASK_FILTERS = "TaskFilters";
	const string S_DEFAULT_SORT_EXP = "StartDate";
	const string S_TASK_ASSIGNED_TO = "AssignedTo";
	const string S_TASK_ASSIGNED_BY = "AssignedBy";
	const string S_DESIGNATION = "Designation";
	const string S_RESOURCE = "User";
	const string S_TASK_TYPE = "TaskType";
	const string S_TASK_STATUS = "TaskStatus";
	const string S_FROM_DATE = "FromDate";
	const string S_TO_DATE = "ToDate";
	const string S_TO_TIME = "ToTime";
	const string S_FROM_TIME = "FromTime";
	const string S_TASK_TYPE_ID = "TaskTypeId";
	const string S_TASK_STATUS_ID = "TaskStatusId";

	TaskListBL moTaskListBL = null;

	#endregion "Constants"

	#region "Event"

	/// <summary>
	/// This event is used to set javascript attributes for buttons, set default values to controls.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{			
			if (!IsPostBack)
			{
				ReadQuerystring();
				InitialiseHiddenVariables();
				FillStatusTaskTypeAndRoleCombobox();
				SetFilters();
				FillTaskListView();
				AddSortImage();
			}
			SetJavaScriptAttributes();
			SetDefaultProperties();
			optAssignedTo.Focus();

		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to edit or delete records in the listview
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwTaskList_ItemCommand(object sender, ListViewCommandEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				moTaskListBL = new TaskListBL(miSchoolId, miAcademicYearId);
				ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
				int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
				if (e.CommandName == Constants.S_COMMAND_REMOVE)
				{
					int iTaskId = Convert.ToInt32(lstvwTaskList.DataKeys[iRowId]["TaskId"]);
					int iTaskStatusId = Convert.ToInt32(lstvwTaskList.DataKeys[iRowId][S_TASK_STATUS_ID]);
					int iTaskTypeId = Convert.ToInt32(lstvwTaskList.DataKeys[iRowId][S_TASK_TYPE_ID]);
					int iAssignerToUserId = Convert.ToInt32(lstvwTaskList.DataKeys[iRowId]["AssignedToUserId"]);
					int iTaskDetailsId = Convert.ToInt32(lstvwTaskList.DataKeys[iRowId]["TaskDetailsId"]);
					moTaskListBL.DeleteTaskDetails(iTaskId, iTaskStatusId, iAssignerToUserId, iTaskTypeId, iTaskDetailsId);
					FillTaskListView();
				}
			}
			if (e.CommandName == Constants.S_COMMAND_SORT)
				hidSortExpression.Value = e.CommandArgument.ToString();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to search particuar record
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnSearch_Click(object sender, EventArgs e)
	{
		try
		{
			FillTaskListView();
			AddSortImage();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
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

		FillTaskListView();
	}

	/// <summary>
	/// This method is used to set sorting image to list view headers.
	/// </summary>
	private void AddSortImage()
	{

		if (string.IsNullOrEmpty(hidSortExpression.Value))
			hidSortExpression.Value = S_DEFAULT_SORT_EXP;
		HtmlTableRow oHtmlTableHeaderRow = lstvwTaskList.FindControl("trHeader") as HtmlTableRow;
		if (oHtmlTableHeaderRow != null)
			CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
	}

	/// <summary>
	/// This event is used to fill user drop down list.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void cmbDesignation_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			FillResourceComboBox();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to set values to listview columns.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwTaskList_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
				Label lblNumber = oCurrentItem.FindControl("lblNo") as Label;
				int iRowId = oCurrentItem.DisplayIndex;
				ImageButton imgBtnDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
				ImageButton imgButnEdit = e.Item.FindControl("imgBtnEdit") as ImageButton;
				int iTaskStatusId = Convert.ToInt32(lstvwTaskList.DataKeys[iRowId][S_TASK_STATUS_ID]);
				int iTaskTypeId = Convert.ToInt32(lstvwTaskList.DataKeys[iRowId][S_TASK_TYPE_ID]);

				DateTime dtCurrentDate = System.DateTime.Now;
				if (((UserTaskList)(oCurrentItem.DataItem)).StartDate.Date > dtCurrentDate.Date)
					imgBtnDelete.Visible = false;

				if (iTaskStatusId == Convert.ToInt32(Constants.TaskStatus.TASK_COMPLETED))
				{
					imgBtnDelete.Visible = false;
					imgButnEdit.Visible = false;
				}
				if (optAssignedBy.Checked == true)
					imgBtnDelete.Visible = false;

				lblNumber.Text = (iRowId + Constants.I_ONE).ToString();

				Label lblStartDateTime = oCurrentItem.FindControl("lblStartDateTime") as Label;
				lblStartDateTime.Text += " " + lstvwTaskList.DataKeys[iRowId]["StartTime"].ToString();

				Label lblEndDateTime = oCurrentItem.FindControl("lblEndDateTime") as Label;
				lblEndDateTime.Text += " " + lstvwTaskList.DataKeys[iRowId]["EndTime"].ToString();

				string sFlag = optAssignedTo.Checked ? Constants.I_ONE.ToString() : Constants.I_TWO.ToString();
				imgBtnDelete.Attributes.Add("onclick", "if(!ConfirmRemove()) {return false;}");
				string sQueryString = "TaskDetailId=" + lstvwTaskList.DataKeys[iRowId]["TaskDetailsId"].ToString() +
							   "&TaskId=" + lstvwTaskList.DataKeys[iRowId]["TaskId"].ToString() +
							   "&TaskAssignerUserId=" + lstvwTaskList.DataKeys[iRowId]["TaskAssignerUserId"].ToString() +
							   "&TaskStatusId=" + lstvwTaskList.DataKeys[iRowId][S_TASK_STATUS_ID].ToString() +
							   "&Flag=" + sFlag +
							   "&TaskTypeId=" + lstvwTaskList.DataKeys[iRowId][S_TASK_TYPE_ID].ToString();

				sQueryString = CommonUtility.EncryptQuerystring(sQueryString);
				ImageButton imgBtnEdit = oCurrentItem.FindControl("imgBtnEdit") as ImageButton;
				if (imgBtnEdit != null)
					imgBtnEdit.Attributes.Add("onclick", "if(!OpenAddNewTaskPopup('" + sQueryString + "')) return false;");

                if (optAssignedBy.Checked)
                {                   
                     HtmlTableCell oTdDelete = e.Item.FindControl("tdDelete") as HtmlTableCell;
                     if (oTdDelete != null)
                        oTdDelete.Visible = false;
                }
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to fill all dropdown lists and listview according to radio button.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void optAssignedTo_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			ClearControls();
			FillRoleCombobox();
			FillResourceComboBox();
			FillTaskListView();
			AddSortImage();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	///  This event is used to fill all dropdown lists and listview according to radio button.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void optAssignedBy_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			ClearControls();
			FillRoleCombobox();
			FillResourceComboBox();
			FillTaskListView();
			AddSortImage();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used for sorting listview columns.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwTaskList_Sorting(object sender, ListViewSortEventArgs e)
	{
		try
		{
			hidSortExpression.Value = (e.SortExpression != string.Empty) ? e.SortExpression
												: S_DEFAULT_SORT_EXP;
			SetSortVariables();
			AddSortImage();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	#endregion

	#region "Private Method"
	/// <summary>
	/// this method is used to set values of control to zero.
	/// </summary>
	private void ClearControls()
	{
		txtFromDate.Text = string.Empty;
		txtFromTime.Text = string.Empty;
		txtToDate.Text = string.Empty;
		txtToTime.Text = string.Empty;
		cmbDesignation.ClearSelection();
		cmbStatus.ClearSelection();
		cmbTaskType.ClearSelection();
		cmbUser.ClearSelection();
	}
	/// <summary>
	/// This method is used to set values to filter.
	/// </summary>
	private void SetFilters()
	{
		if (Request.QueryString.ToString() != Constants.S_EMPTY_STRING)
		{
			Dictionary<string, string> dictTaskListFilters = null;
			if (Session[S_TASK_FILTERS] != null)
			{
				dictTaskListFilters = Session[S_TASK_FILTERS] as Dictionary<string, string>;
				if (dictTaskListFilters[S_TASK_ASSIGNED_TO] == "True")
				{
					optAssignedTo.Checked = true;
					optAssignedBy.Checked = false;
				}
				else
				{
					optAssignedTo.Checked = false;
					optAssignedBy.Checked = true;
				}
				cmbDesignation.SelectedValue = dictTaskListFilters[S_DESIGNATION];
				cmbUser.SelectedValue = dictTaskListFilters[S_RESOURCE];
				cmbTaskType.SelectedValue = dictTaskListFilters[S_TASK_TYPE];
				cmbStatus.SelectedValue = dictTaskListFilters[S_TASK_STATUS];
				txtFromDate.Text = dictTaskListFilters[S_FROM_DATE];
				txtFromTime.Text = dictTaskListFilters[S_FROM_TIME];
				txtToDate.Text = dictTaskListFilters[S_TO_DATE];
				txtToTime.Text = dictTaskListFilters[S_TO_TIME];

				Session.Remove(S_TASK_FILTERS);
			}
		}
	}
	/// <summary>
	/// This method is used to fill Listview.
	/// </summary>
	private void FillTaskListView()
	{
		moTaskListBL = new TaskListBL(miSchoolId, miAcademicYearId);
		FillFilterDictionary();
		List<UserTaskList> lstTaskList = moTaskListBL.GetListViewDetails(GenerateFilterXML(), hidSortExpression.Value, hidSortDirection.Value);
		lstvwTaskList.DataSource = lstTaskList;
		lstvwTaskList.DataBind();

        HtmlTableRow trHeader = lstvwTaskList.FindControl("trHeader") as HtmlTableRow;
        HtmlTableCell tcDeleteHead = trHeader.FindControl("thDelete") as HtmlTableCell;

        if (optAssignedBy.Checked)
        {
            if (trHeader != null)
            {
                if (tcDeleteHead != null)
                    tcDeleteHead.Visible = false;
            }
        }
        else
        {
            if (trHeader != null)
            {
                if (tcDeleteHead != null)
                    tcDeleteHead.Visible = true;
            }
        }

	}
	/// <summary>
	/// This method is used to fill dictonary with filters.
	/// </summary>
	private void FillFilterDictionary()
	{
		Dictionary<string, string> dictValue = new Dictionary<string, string>();
		dictValue.Add(S_TASK_ASSIGNED_TO, optAssignedTo.Checked.ToString());
		dictValue.Add(S_TASK_ASSIGNED_BY, optAssignedBy.Checked.ToString());
		dictValue.Add(S_DESIGNATION, cmbDesignation.SelectedValue);
		dictValue.Add(S_RESOURCE, cmbUser.SelectedValue);
		dictValue.Add(S_TASK_TYPE, cmbTaskType.SelectedValue);
		dictValue.Add(S_TASK_STATUS, cmbStatus.SelectedValue);
		dictValue.Add(S_FROM_DATE, txtFromDate.Text);
		dictValue.Add(S_FROM_TIME, txtFromTime.Text);
		dictValue.Add(S_TO_DATE, txtToDate.Text);
		dictValue.Add(S_TO_TIME, txtToTime.Text);

		Session[S_TASK_FILTERS] = dictValue;
	}
	/// <summary>
	/// This method is used to generate xml of filters.
	/// </summary>
	/// <returns></returns>
	private string GenerateFilterXML()
	{
		const string S_ELEMENT = "element";
		XmlDocument oDoc = new XmlDocument();
		XmlElement oRoot = oDoc.CreateElement("Tasks");
		XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "Tasks", "");

		XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "Tasks", "");

		XmlAttribute oAttr = oDoc.CreateAttribute("Flag");
		oAttr.Value = optAssignedTo.Checked ? Constants.I_ONE.ToString() : Constants.I_ZERO.ToString();
		oXmlNode.Attributes.Append(oAttr);

		if (cmbDesignation.SelectedValue != Constants.I_ZERO.ToString())
		{
			oAttr = oDoc.CreateAttribute("DesignationId");
			oAttr.Value = cmbDesignation.SelectedValue;
			oXmlNode.Attributes.Append(oAttr);
		}
		if (cmbUser.SelectedValue != Constants.I_ZERO.ToString())
		{
			oAttr = oDoc.CreateAttribute("UserId");
			oAttr.Value = cmbUser.SelectedValue;
			oXmlNode.Attributes.Append(oAttr);
		}
		if (cmbTaskType.SelectedValue != Constants.I_ZERO.ToString())
		{
			oAttr = oDoc.CreateAttribute(S_TASK_TYPE_ID);
			oAttr.Value = cmbTaskType.SelectedValue;
			oXmlNode.Attributes.Append(oAttr);
		}
		if (cmbStatus.SelectedValue != Constants.I_ZERO.ToString())
		{
			oAttr = oDoc.CreateAttribute(S_TASK_STATUS_ID);
			oAttr.Value = cmbStatus.SelectedValue;
			oXmlNode.Attributes.Append(oAttr);
		}
		txtFromDate.Text = txtFromDate.Text.Trim();
		txtFromTime.Text = txtFromTime.Text.Trim();
		if ((!string.IsNullOrEmpty(txtFromDate.Text)) && (!string.IsNullOrEmpty(txtFromTime.Text)))
		{
			oAttr = oDoc.CreateAttribute(S_FROM_DATE);
			oAttr.Value = txtFromDate.Text + " " + txtFromTime.Text.Trim();
			oXmlNode.Attributes.Append(oAttr);
		}
		else if (!string.IsNullOrEmpty(txtFromTime.Text))
		{
			oAttr = oDoc.CreateAttribute(S_FROM_DATE);
			oAttr.Value = (DateTime.Now.ToShortDateString()).ToString() + " " + txtFromTime.Text.Trim();
			oXmlNode.Attributes.Append(oAttr);
		}
		else if (!string.IsNullOrEmpty(txtFromDate.Text))
		{
			oAttr = oDoc.CreateAttribute(S_FROM_DATE);
			oAttr.Value = txtFromDate.Text + " " + "00:01AM";
			oXmlNode.Attributes.Append(oAttr);
		}
		txtToDate.Text = txtToDate.Text.Trim();
		txtToTime.Text = txtToTime.Text.Trim();

		if (!string.IsNullOrEmpty(txtToDate.Text) && (!string.IsNullOrEmpty(txtFromTime.Text)))
		{
			oAttr = oDoc.CreateAttribute(S_TO_DATE);
			oAttr.Value = txtToDate.Text + " " + txtToTime.Text.Trim();
			oXmlNode.Attributes.Append(oAttr);
		}
		else if (!string.IsNullOrEmpty(txtToTime.Text))
		{
			oAttr = oDoc.CreateAttribute(S_TO_DATE);
			oAttr.Value = (DateTime.Now.ToShortDateString()).ToString() + " " + txtToTime.Text.Trim();
			oXmlNode.Attributes.Append(oAttr);
		}
		else if (!string.IsNullOrEmpty(txtToDate.Text))
		{
			oAttr = oDoc.CreateAttribute(S_TO_DATE);
			oAttr.Value = txtToDate.Text + " " + "11:59PM";
			oXmlNode.Attributes.Append(oAttr);
		}
		oAttr = oDoc.CreateAttribute("OwnerUserId");
		oAttr.Value = miUserId.ToString();
		oXmlNode.Attributes.Append(oAttr);

		oXmlRootNode.AppendChild(oXmlNode);
		oRoot.AppendChild(oXmlRootNode);
		return oRoot.InnerXml;
	}
	/// <summary>
	/// This method is used to Fill User drop down list.
	/// </summary>
	public void FillResourceComboBox()
	{
		int iFlag = Constants.I_ONE;
		if (optAssignedBy.Checked == true)
			iFlag = Constants.I_TWO;
		moTaskListBL = new TaskListBL(miSchoolId, miAcademicYearId);
		ListSource.FillDropDownList(moTaskListBL.GetDesignationwiseResourceList(Convert.ToInt32(cmbDesignation.SelectedValue), iFlag), cmbUser, "Display_Member", "Value_Member", Constants.S_ALL);
	}
	/// <summary>
	/// This method is used to fill status dropdown list. 
	/// </summary>
	private void FillStatusTaskTypeAndRoleCombobox()
	{
		int iFlag = Constants.I_ONE;
		if (optAssignedBy.Checked == true)
			iFlag = Constants.I_TWO;
		moTaskListBL = new TaskListBL(miSchoolId, miAcademicYearId);
		moTaskListBL.GetTaskTypeStatusAndDesignation(moUserRole.ToInt(), miUserId, iFlag);
		moTaskListBL.FillTaskTypeStatusAndDesignationComboboxes(cmbTaskType, cmbStatus, cmbDesignation);
	}
	/// <summary>
	/// This method is used to fill designation dropdown list. 
	/// </summary>
	private void FillRoleCombobox()
	{
		int iFlag = Constants.I_ONE;
		if (optAssignedBy.Checked == true)
			iFlag = Constants.I_TWO;
		moTaskListBL = new TaskListBL(miSchoolId, miAcademicYearId);
		List<DesignationMaster> lstRoles = MasterDataCollectionBL.GetDesignationsDetails(miUserId, moUserRole.ToInt(), iFlag, miSchoolId, miAcademicYearId);
		ListSource.FillDropDownList(lstRoles, cmbDesignation, "Designation", "DesignationId", Constants.S_ALL);
	}
	/// <summary>
	/// This method is used to set java script attributes
	/// </summary>
	private void SetJavaScriptAttributes()
	{
		new Button[] { btnSearch, btnAddNewTask }.ApplyEffect();

		string sQueryString = "TaskDetailId=" + "0" +
							  "&TaskId=" + "0" +
							  "&TaskAssignerUserId=" + "0" +
							  "&TaskStatusId=" + "0" +
							  "&Flag=" + "1" +
							  "&TaskTypeId=" + "0";
		sQueryString = CommonUtility.EncryptQuerystring(sQueryString);
		btnAddNewTask.Attributes.Add("onclick", "if(!OpenAddNewTaskPopup('" + sQueryString + "')) return false;");
	}
	/// <summary>
	/// This method is used to read query string.
	/// </summary>
	/// <returns></returns>
	private void ReadQuerystring()
	{
		if (Request.QueryString.ToString() == Constants.S_EMPTY_STRING)
			return;

		hidTaskDetailId.Value = QueryString["TaskDetailId"];
		hidTaskId.Value = QueryString["TaskId"];
		hidTaskAssignerUserId.Value = QueryString["TaskAssignerUserId"];
		hidStatusId.Value = QueryString[S_TASK_STATUS_ID];
		hidTaskTypeId.Value = QueryString[S_TASK_TYPE_ID];
	}
	/// <summary>
	/// This method is used to initialise hidden valriable.
	/// </summary>
	private void InitialiseHiddenVariables()
	{
		hidSortExpression.Value = S_DEFAULT_SORT_EXP;
		hidSortDirection.Value = Constants.S_ASCENDING;
	}

	/// <summary>
	/// This method is used to set default values to controls.
	/// </summary> 
	private void SetDefaultProperties()
	{
		//Set default button property.
		HtmlForm oForm = (HtmlForm)this.Master.FindControl("Form1");
		oForm.DefaultButton = btnSearch.UniqueID;
	}
	#endregion

}
