// -----------------------------------------------------------------------
/* File Name - BlockProgressReportUI.aspx.cs
 * Created Date - 22-Oct-2011
 * Created by - Lakshman Shinde
 * Class Description - This class is used for Block student progress report.
 */
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using ProgressReportEntities;
using Utility;
using System.Data;

public partial class BlockProgressReportUI : SchoolBase
{

    # region Constants

	const string S_BTN_BLOCK="Block";
	const string S_BTN_UNBLOCK = "Unblock";
    const string S_REASON_UPDATE = "Selected student(s) Reason updated successfully !!!";
	
    #endregion

	#region Data Members

	private ProgressReportBL moProgressReportBL;

	#endregion


	# region Property(s)

	private ProgressReportBL ProgressReportBL
	{
		get
		{
	   	 	if (moProgressReportBL == null)
				moProgressReportBL = new ProgressReportBL(miSchoolId,miAcademicYearId,miUserId);
			return moProgressReportBL;
		}
		set { moProgressReportBL = value; }
	}

	#endregion

	#region Events
	/// <summary>
	/// This Page load event is used to load controls like Teachers and students combo and student list view.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			if (!IsPostBack)
			{
				SetViewAsPerAccess();
				SetDefaultValues();
				FillTeachersComboBox();
				FillStudentsListView();
				FillStudentsComboBox();
				SetJavascriptAttributes();
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This teacher selected index change comb box event used to fill bind students listview as per teacher  selection in combox
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void cmbTeachers_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
            DtPgCount.Visible = true;
			txtSearch.Text = string.Empty;
			hidStdDivId.Value = cmbTeachers.SelectedValue;
			FillStudentsComboBox();
            if (lstvwBlockedUnblockedStudent.Items.Count > 0)
            {
                ((DataPager)lstvwBlockedUnblockedStudent.FindControl("DtPgDropDown")).SetPageProperties(Constants.I_ZERO, 20, false);
                FillStudentsListView();
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
	/// This Radio button changed event used to  bind blocked or  unblocked students to listview as per radio button selection
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void optBlocked_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			txtSearch.Text = string.Empty;
			if (optUnblocked.Checked)
			{
				btnBlockUnblock.Text = S_BTN_BLOCK;
				btnUpdate.Visible = false;
				HidOptUnblocked.Value = optUnblocked.Checked.ToString();
			}
			else
			{
				btnBlockUnblock.Text = S_BTN_UNBLOCK;
				btnUpdate.Visible = true;
			}
			HidOptUnblocked.Value = optUnblocked.Checked.ToString();
			FillStudentsListView();
			FillStudentsComboBox();
			
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This is page changed com box event is used to bind students  listview as per page 
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			DropDownList ocmbPageCount = ((DataPager)lstvwBlockedUnblockedStudent.FindControl("DtPgDropDown")).Controls[0].FindControl("ddlCnt") as DropDownList;
			hidPageNo.Value = ocmbPageCount.SelectedValue;
			ocmbPageCount.Attributes.Add("onchange", "if(!DatalossAlert('" + ocmbPageCount.ClientID + "')){return false;}");
			ControlUtility.SetDataPagerAccordingToPageNo(lstvwBlockedUnblockedStudent);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This method is used to block and unblock The  progerss report Students
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnBlockUnblock_Click(object sender, EventArgs e)
	{
		try
		{
			ProgressReportBL.SaveBlockStudentDetails(CommonUtility.GetXMLForList(PopulateStudentsStatusDetails()), optUnblocked.Checked);
			FillStudentsListView();
			DisplayMessage(optUnblocked.Checked ? "blocked" : "unblocked");
			FillStudentsComboBox();
			hidAlert.Value = Constants.S_ZERO;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This is Used to update Reason (insertind New Entry)
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (btnBlockUnblock.Text == S_BTN_UNBLOCK)
			{
				ProgressReportBL.SaveBlockStudentDetails(CommonUtility.GetXMLForList(PopulateStudentsStatusDetails()), true);
				FillStudentsListView();
				lblUpdateSucess.Text=S_REASON_UPDATE;
				hidAlert.Value = Constants.S_ZERO;
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This is button search event is used to get search student
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnSearch_Click(object sender, EventArgs e)
	{
		try
		{
           if(lstvwBlockedUnblockedStudent.Items.Count > 0)
		        ((DataPager) lstvwBlockedUnblockedStudent.FindControl("DtPgDropDown")).SetPageProperties(Constants.I_ZERO, 20, false);
			FillStudentsListView();

		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	#region Listview Event
	/// <summary>
	/// This listview item databound event used to add "On change" event text box which is used in  listview
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwBlockedUnblockedStudent_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
				TextBox otxtReasonforBlock = oCurrentItem.FindControl("txtReason") as TextBox;
				otxtReasonforBlock.Attributes.Add("onChange", "HidvalueChangeforAlert()");
				DropDownList ocmbPageCount = oCurrentItem.FindControl("ddlCnt") as DropDownList;
				if(ocmbPageCount!=null)
				ocmbPageCount.Attributes.Add("onchange", "if(!DatalossAlert('" + cmbStudent.ClientID + "')){return false;}");
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This listview Databound event add "Onchange" event to page dropdown
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwBlockedUnblockedStudent_DataBound(object sender, EventArgs e)
	{
		try
		{
			 DtPgCount.Visible = true;
			if (lstvwBlockedUnblockedStudent.Items.Count > 0)
			{
				DropDownList ocmbPageCount = ((DataPager)lstvwBlockedUnblockedStudent.FindControl("DtPgDropDown")).Controls[0].FindControl("ddlCnt") as DropDownList;
				ocmbPageCount.Attributes.Add("onchange", "if(!DatalossAlert('" + ocmbPageCount.ClientID + "')){return false;}");
			    ControlUtility.FillListViewPagerFooter(lstvwBlockedUnblockedStudent, DtPgCount);
				btnUpdate.Enabled = true;
				btnBlockUnblock.Enabled = true;
			}
			else
			{
				DtPgCount.Visible = false;
				btnUpdate.Enabled = false;
				btnBlockUnblock.Enabled=false;
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This listviev sorting event to get SortExpression 
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwBlockedUnblockedStudent_Sorting(object sender, ListViewSortEventArgs e)
	{
		try
		{
			hidSortExpression.Value = e.SortExpression;
			RevertSortOrder(hidSortDirection);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This listview prerender event  is used to set default Image to sortable column
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwBlockedUnblockedStudent_PreRender(object sender, EventArgs e)
	{
		try
		{
			hidSortExpression.Value = hidSortExpression.Value.IsNullOrEmpty() ? "RollNo" : hidSortExpression.Value;
			hidSortDirection.Value = hidSortDirection.Value.IsNullOrEmpty() ? "asc" : hidSortDirection.Value;
			AddSortImage(lstvwBlockedUnblockedStudent, hidSortExpression.Value, hidSortDirection.Value);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	#endregion

	# region ObjectDatasource Event
	/// <summary>
	/// This method is used set created object 
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void objDSStudentList_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
	{
		try
		{
			e.ObjectInstance = ProgressReportBL;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This method is used to disposing the object
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void objDSStudentList_ObjectDisposing(object sender, ObjectDataSourceDisposingEventArgs e)
	{
		try
		{
			e.Cancel = true;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This student comb box selected index change event bind that particular student to listview
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void cmbStudent_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			HidStudent.Value = cmbStudent.SelectedValue;
			FillStudentsListView();
			txtSearch.Text = string.Empty;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}


	#endregion

	# endregion

	#region Private method
	/// <summary>
	/// This Method is Used to fill Teacher Comb Box
	/// </summary>
	private void FillTeachersComboBox()
	{
		MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
		DataTable oDtClassTeachers= oMasterDataCollectionBL.GetAllClassTeachers(miSchoolId, miAcademicYearId);
		if (moUserRole == Constants.UserRoles.Teacher && CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.BlockProgressReport) == Constants.C_NO)
		{
			tdteacher.Visible = true;
			tdteacherdropdown.Visible = true;
			DataTable oDtClasses = oDtClassTeachers.Select("Teacher_Id=" + Session[Constants.S_SESSION_TEACHER_ID]).CopyToDataTable();
			ListSource.FillDropDownList(oDtClasses, cmbTeachers, Constants.S_TEACHER_NAME_FIELD, Constants.S_STANDARD_DIVISION_ID_FIELD, string.Empty);
			hidStdDivId.Value = cmbTeachers.SelectedValue;
		}
		else
			ListSource.FillDropDownList(oDtClassTeachers, cmbTeachers, Constants.S_TEACHER_NAME_FIELD, Constants.S_STANDARD_DIVISION_ID_FIELD, Constants.S_SELECT);
	}

	/// <summary>
	/// This used to fill Student Listview
	/// </summary>
	private void FillStudentsListView()
	{
		lstvwBlockedUnblockedStudent.DataSourceID = objDSStudentList.ID;
		lstvwBlockedUnblockedStudent.DataBind();
		if (lstvwBlockedUnblockedStudent.Items.Count > 0)
		{
			DropDownList ocmbPageCount =((DataPager) lstvwBlockedUnblockedStudent.FindControl("DtPgDropDown")).Controls[0].FindControl("ddlCnt") as DropDownList;
			ocmbPageCount.Attributes.Add("onchange", "if(!DatalossAlert('" + ocmbPageCount.ClientID + "')){return false;}");
			
		}
	}

	/// <summary>
	/// This Method is used to fill Student Comb box.
	/// </summary>
	private void FillStudentsComboBox()
	{
		if (cmbTeachers.SelectedValue != "0")
		{
			List<BlockStudentsProgressReportDetails> lstBlockStudentsProgressReportDetails = ProgressReportBL.GetAllBlockedUnBlockedStudents(hidStdDivId.Value.ToInt(), optBlocked.Checked, Constants.I_ZERO, txtSearch.Text, string.Empty, 100, 0);
			cmbStudent.Bind(lstBlockStudentsProgressReportDetails, "YearwiseStudentId", "StudentName", Constants.S_SELECT_ALL);
		}
		else
		{
			cmbStudent.Items.Clear();
			cmbStudent.Items.Add(new ListItem(Constants.S_SELECT_ALL, Constants.S_ZERO));
		}
	}

	/// <summary>
	/// This Method is used to Populate Student Status
	/// </summary>
	/// <returns></returns>
	private List<BlockStudentsProgressReportDetails> PopulateStudentsStatusDetails()
	{
		List<BlockStudentsProgressReportDetails> lstBlockStudentsProgressReportDetails = new List<BlockStudentsProgressReportDetails>();
		
		foreach (ListViewDataItem oCurrentItem in lstvwBlockedUnblockedStudent.Items)
         {
			 CheckBox chkSelect = oCurrentItem.FindControl("chkSelect") as CheckBox;
			 if (chkSelect.Checked)
			  {
			    BlockStudentsProgressReportDetails oBlockStudentsProgressReportDetails = new BlockStudentsProgressReportDetails();
				TextBox otxtReasonforBlock = oCurrentItem.FindControl("txtReason") as TextBox;
				oBlockStudentsProgressReportDetails.YearwiseStudentId = lstvwBlockedUnblockedStudent.DataKeys[oCurrentItem.DisplayIndex]["YearwiseStudentId"].ToInt();
				oBlockStudentsProgressReportDetails.Reason = otxtReasonforBlock.Text;
				lstBlockStudentsProgressReportDetails.Add(oBlockStudentsProgressReportDetails);
			  }
		 }
		 return lstBlockStudentsProgressReportDetails;
	}

	/// <summary>
	/// This method used set javascript
	/// </summary>
	private void SetJavascriptAttributes()
	{
		ApplyMouseHoverEffect(new List<Button> { btnBlockUnblock, btnUpdate, btnSearch });
		cmbTeachers.Attributes.Add("onchange", "if(!DatalossAlert()){return false;}");
		cmbStudent.Attributes.Add("onchange", "if(!DatalossAlert()){return false;}");
		btnSearch.Attributes.Add("onclick", "if(!DatalossAlert()){return false;}");
		optBlocked.Attributes.Add("onclick", "if(!DatalossAlert()){return false;}");
		optUnblocked.Attributes.Add("onclick", "if(!DatalossAlert()){return false;}");
		btnUpdate.Attributes.Add("onclick", "if(!CheckAtLeastOneStudentIsSelected()){return false;}");
		btnBlockUnblock.Attributes.Add("onclick", "if(!CheckAtLeastOneStudentIsSelected()){return false;}");
		valSummary.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
	}

	/// <summary>
	/// This Method is used to set default values
	/// </summary>
	private  void SetDefaultValues()
	{
		cmbStudent.Items.Add(new ListItem(Constants.S_SELECT_ALL, Constants.S_ZERO));
		optUnblocked.Checked = true;
		btnBlockUnblock.Text = S_BTN_BLOCK;
		btnUpdate.Visible = false;		
		hidStdDivId.Value = Constants.S_ZERO;
		HidStudent.Value = cmbStudent.SelectedValue;
	}

	/// <summary>
	/// This method is used check the access
	/// </summary>
	private void SetViewAsPerAccess()
	{
		if (moUserRole == Constants.UserRoles.Teacher && CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.BlockProgressReport) == Constants.C_NO)
		{
			tdteacher.Visible = false;
			tdteacherdropdown.Visible = false;
			hidStdDivId.Value = cmbTeachers.SelectedValue;
		}
	}

	/// <summary>
	/// This method is used to display message
	/// </summary>
	/// <param name="asMessage"></param>
	private void DisplayMessage(string asMessage)
	{
		lblUpdateSucess.Text = "Selected student(s) " + asMessage + " successfully !!!";
	}
	
	#endregion
}