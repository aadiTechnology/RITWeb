using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using BusinessLogic;
using BusinessLogic.Exceptions;
using MasterEntities;
using TaskManagementEntities;
using Utility;

/// <summary>
/// This class is used to set workflow role configuration .
/// </summary>
public partial class WorkFlowRoleConfigurationUI : SchoolBase
{
	#region "Data member"

	WorkFlowRoleConfigurationBL moWorkFlowConfigurationBL = null;

	#endregion

	#region "Events"

	/// <summary>
	/// This event is used to set javascript attributes for buttons, set default values to controls.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			moWorkFlowConfigurationBL = new WorkFlowRoleConfigurationBL(miSchoolId, miAcademicYearId);
			if (!IsPostBack)
			{
				FillAssignerRoleComboBox();
				if (cmbAssignTaskBy.Items.Count > 0)
				{
					FillAssigneeRoleListView();
					cmbAssignTaskBy.Focus();
				}
				else
					SetControlVisibility(false);
				SetJavaScriptAttributes();
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	private void SetControlVisibility(bool abFlag)
	{
		trAssignTaskBy.Visible = abFlag;
		btnSave.Visible = abFlag;
		trLbl.Visible = abFlag;
		trListView.Visible = abFlag;
	}

	/// <summary>
	/// This event is used to save work flow configuration details
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
			Save();
			if (!IsConfigured())
				SaveConfigDetails(Constants.SchoolConfigurations.WorkFlowConfiguration.ToInt());
			lblUpdateSucess.Visible = true;
			lblUpdateSucess.Text = "Task assignment saved successfully !!!";

		}
		catch (SqlException ex)
		{
			lblErrorMessage.Text = ex.Message;
			FillAssigneeRoleListView();
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
	protected void lstvwWorkFlowConfiguration_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
				CheckBox chkSelect = e.Item.FindControl("ChkSelect") as CheckBox;
				int iRowId = oCurrentItem.DisplayIndex;

				if (((WorkFlowRoleConfigurationDetail)(oCurrentItem.DataItem)).Is_Deleted == "N")
					chkSelect.Checked = true;
				else
					chkSelect.Checked = false;
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to fill ListView for selected designation.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void cmbAssignTaskBy_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			FillAssigneeRoleListView();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	#endregion

	#region "Private Methods"

	/// <summary>
	/// This event is used to save Work Flow configuration details.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void Save()
	{
		List<WorkFlowRoleConfigurationDetail> lstWorkFlowRoleConfigDetail = PopulateWorkflowRoleConfigList();
		moWorkFlowConfigurationBL.SaveWorkFlowConfigDetails(GetWorkFlowConfigDetailXML(lstWorkFlowRoleConfigDetail), miUserId);
		FillAssigneeRoleListView();

	}

	/// <summary>
	/// This method is used to fill FillAssignTaskTo ComboBox .
	/// </summary>
	private void FillAssignerRoleComboBox()
	{
		List<DesignationMaster> lstWorkFlowConfigurationDetail = moWorkFlowConfigurationBL.GetAllDesignations();
		cmbAssignTaskBy.Items.Clear();
		if (lstWorkFlowConfigurationDetail.Count > 0)
		{
			cmbAssignTaskBy.DataSource = lstWorkFlowConfigurationDetail;
			cmbAssignTaskBy.DataTextField = "Designation";
			cmbAssignTaskBy.DataValueField = "DesignationId";
			cmbAssignTaskBy.DataBind();
		}
		else
		{
			tbldivErr.Visible = true;
			divErr.InnerHtml = "Users are not available. Please add any of following user details:";
		}
	}

	/// <summary>
	/// This method is used to fill Work Flow configuration details in list view.
	/// </summary>
	private void FillAssigneeRoleListView()
	{
		lstvwWorkFlowConfiguration.Items.Clear();
		List<WorkFlowRoleConfigurationDetail> lstWorkFlowConfigurationDetail = moWorkFlowConfigurationBL.GetAllAssigneeList(Convert.ToInt32(cmbAssignTaskBy.SelectedValue));
		lstvwWorkFlowConfiguration.DataSource = lstWorkFlowConfigurationDetail.Where(designation => designation.Designation != cmbAssignTaskBy.SelectedItem.Text);
		lstvwWorkFlowConfiguration.DataBind();
	}

	/// <summary>
	/// This method is used to populate work flow etails.
	/// </summary>
	private List<WorkFlowRoleConfigurationDetail> PopulateWorkflowRoleConfigList()
	{
		WorkFlowRoleConfigurationDetail oWorkFlowConfigurationDetail = null;
		List<WorkFlowRoleConfigurationDetail> lstWorkFlowConfigDetail = new List<WorkFlowRoleConfigurationDetail>();
		int iRowId = 0;
		int iAssignedToDesignationId = 0;

		foreach (ListViewDataItem oDataItem in lstvwWorkFlowConfiguration.Items)
		{
			oWorkFlowConfigurationDetail = new WorkFlowRoleConfigurationDetail();
			iRowId = Convert.ToInt32(oDataItem.DisplayIndex);
			iAssignedToDesignationId = Convert.ToInt32(lstvwWorkFlowConfiguration.DataKeys[iRowId]["AssignedToDesignationId"].ToString());
			CheckBox chkSelect = oDataItem.FindControl("ChkSelect") as CheckBox;

			if (chkSelect.Checked == true)
			{
				oWorkFlowConfigurationDetail.WorkFlowLevelId = Convert.ToInt32(lstvwWorkFlowConfiguration.DataKeys[iRowId]["WorkFlowLevelId"].ToString());
				oWorkFlowConfigurationDetail.AssignedByDesignationId = Convert.ToInt32(cmbAssignTaskBy.SelectedValue);
				oWorkFlowConfigurationDetail.AssignedToDesignationId = iAssignedToDesignationId;
				oWorkFlowConfigurationDetail.SchoolId = miSchoolId;
				oWorkFlowConfigurationDetail.Is_Deleted = Constants.C_NO.ToString();
				oWorkFlowConfigurationDetail.InsertedById = miUserId;
				;
				lstWorkFlowConfigDetail.Add(oWorkFlowConfigurationDetail);
			}

		}
		return lstWorkFlowConfigDetail;
	}

	/// <summary>
	/// This method creates an XML file.
	/// </summary>
	private string GetWorkFlowConfigDetailXML(List<WorkFlowRoleConfigurationDetail> lstWorkFlowConfigurationDetails)
	{
		StringWriter sw = new StringWriter();
		new XmlSerializer(lstWorkFlowConfigurationDetails.GetType()).Serialize(sw, lstWorkFlowConfigurationDetails);
		string sXML = sw.ToString();
		sXML = sXML.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", string.Empty);
		return sXML;
	}

	/// <summary>
	/// This method is used to set javascript attributes to controls.
	/// </summary>
	private void SetJavaScriptAttributes()
	{

		ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel });
		btnSave.Attributes.Add("onclick", "if(!CheckAtListOne()) return false;");
		btnCancel.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Task_Related));
	}

	/// <summary>
	/// This method is used to decrypt query string.
	/// </summary>
	/// <returns></returns>
	private bool IsConfigured()
	{
		return QueryString[Constants.S_IS_CONFIGURED] == Constants.S_YES;
	}

	#endregion
}
