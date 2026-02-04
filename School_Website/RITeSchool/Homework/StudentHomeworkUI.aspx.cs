using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using BusinessLogic;
using Utility;
using SchoolEntities;
using System.Web.UI.HtmlControls;

/// <summary>
/// This class is used to display homework of all subjects of logged in student.
/// </summary>
public partial class StudentHomeworkUI : SchoolBase
{
    #region Constants

    private const string S_Homework_FOLDER_LOCATION = "RITeSchool\\DOWNLOADS\\Homework\\"; 
    
    #endregion
	
    #region "Events"

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

	/// <summary>
	/// This event is used to apply hover effect to button, set value to date control and fill homework list.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{            
			if (!IsPostBack)
			{
				SetDefaultValues();
				FillHomeworkList();
                FillHomeworkLogsDetails();
                SetHomeworkLogFields();
            }
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to fill homework listview if date is changed
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void calAssignedDtSearch_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			FillHomeworkList();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to set date time format for homework assigned date and complete by date.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwHomeworkStudent_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
				Label lblCompleteDt = oCurrentItem.FindControl("lblCompleteDt") as Label;
				lblCompleteDt.Text = ((Homework)oCurrentItem.DataItem).CompleteByDate.ToString(Constants.S_STANDARD_DATE_FORMAT);
				LinkButton lnkTitle = oCurrentItem.FindControl("lnkTitle") as LinkButton;
				string sQueryString = CommonUtility.EncryptQuerystring("HomeworkId=" + ((Homework)oCurrentItem.DataItem).Id);
				lnkTitle.Attributes.Add("onclick", "window.open('" + lnkTitle.PostBackUrl + sQueryString + "' , '_blank','scrollbars=yes,resizable=yes,top=0,left=0,width=800,height=600'); return false;");
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}
    /// <summary>
    /// This method use to set status of exam whether assign by or completed by
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void drdwnHomeWorkStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            calAssignedDtSearch_SelectionChanged(sender, e);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is sued to search homework log.
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
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set homework log date format and attributes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
                string sPath = Constants.S_HOMEWORK_FOLDER_LOCATION+"DailyLog/" + oHomeworkDailyLog.AttachmentsName;
                lnkAttachment.NavigateUrl = Constants.S_HOMEWORK_FOLDER_LOCATION + "DailyLog/" + oHomeworkDailyLog.AttachmentsName;
                lnkAttachment.Attributes.Add("Onclick", "OpenFile('" + sPath + "'); return false;");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set pager.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
    /// This event is used to pager state.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

	#endregion

	#region "Private Methods"

	/// <summary>
	/// This method is used to fill homework listview for students.
	/// </summary>
	private void FillHomeworkList()
	{
		HomeWorkBL moHomeworkBL = new HomeWorkBL(miSchoolId, miAcademicYearId, miUserId);
        List<Homework> lstHomework = moHomeworkBL.GetListForStudent(Session[Constants.S_SESSION_STUDENT_STANDERED_DIVISION_ID].ToInt(), txtSearchDt.Text, drdwnHomeWorkStatus.SelectedValue);
		lstvwHomeworkStudent.DataSource = lstHomework;
		lstvwHomeworkStudent.DataBind();
	}

	/// <summary>
	/// This method is used to apply mouse over effect, default date to search.
	/// </summary>
	private void SetDefaultValues()
	{
		txtSearchDt.Focus();
		txtSearchDt.Text = QueryString.Count <= Constants.I_ZERO || QueryString["Date"] == null ? DateTime.Now.ToString(Constants.S_DATE_FORMAT) : QueryString["Date"];
	}

    /// <summary>
    /// This method is used to fill homework log details. 
    /// </summary>
    private void FillHomeworkLogsDetails()
    {
        if (Settings.AllowHomewirkDailyLog)
        {
            lstvwHomeworklogs.DataSourceID = objdsHomeworks.ID;
            lstvwHomeworklogs.DataBind();
        }
    }

    private void SetHomeworkLogFields()
    {
        bool bAllowHomewirkDailyLog = Settings.AllowHomewirkDailyLog;
        trHR.Visible = bAllowHomewirkDailyLog;
        trLogHeader.Visible = bAllowHomewirkDailyLog;
        trLogFilter.Visible = bAllowHomewirkDailyLog;
        trLogData.Visible = bAllowHomewirkDailyLog;
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

	#endregion
}
