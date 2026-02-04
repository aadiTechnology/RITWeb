/*
 * File Name - PartialLeavePopup.aspx.cs
 * Creadted By - Sachin
 * Created Date - 31-May-2011
 * Description - This class is used to setpartial leaves.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using Utility;

/// <summary>
/// This class is used to setpartial leaves.
/// </summary>
public partial class PartialLeavePopup : SchoolBase
{
    #region Data Members

    DatewiseStaffLeavesBL moDatewiseStaffLeavesBL;
    Dictionary<int, int> moLeaveDictionary = new Dictionary<int, int>(); 

    #endregion

    #region Events

    /// <summary>
    /// This event is used to fill partial leave listview and set query string.
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
                FillPartialLeaveListview();
                SetJavascriptAttributes();
                SetQueryString();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set partial leaves.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSet_Click(object sender, EventArgs e)
    {
        try
        {
            string sPartialLeaves = Set();
            string sQueryString = CommonUtility.EncryptQuerystring(SetQueryString() + "&PartialLeave=" + sPartialLeaves);

            PopupMasterSml oMasterPage = this.Master as PopupMasterSml;
            oMasterPage.RedirectToNextPage("DatewiseStaffLeavesPopup.aspx?" + sQueryString);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill leave combobox of listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwPartialLEaves_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                PartialLeaveDetails PartialLeaveDetail = (PartialLeaveDetails)oCurrentItem.DataItem;
                int ExistingLeaveId = Convert.ToInt32(lstvwPartialLEaves.DataKeys[oCurrentItem.DisplayIndex]["ExistingLeaveId"]);
                DropDownList cmbLeaves = oCurrentItem.FindControl("cmbLeave") as DropDownList;
                List<ConfiguredLeaves> ConfiguredLeavesList = moDatewiseStaffLeavesBL.ConfiguredLeavesList.Where(leave => leave.LeaveId != ExistingLeaveId).ToList();
                ConfiguredLeavesList.Insert(0, new ConfiguredLeaves { ShortName = "-- Select --", LeaveId = 0 });
                cmbLeaves.DataSource = ConfiguredLeavesList;
                cmbLeaves.DataTextField = "ShortName";
                cmbLeaves.DataValueField = "LeaveId";
                cmbLeaves.DataBind();

                if (moLeaveDictionary.Count > 0 && moLeaveDictionary.ContainsKey(PartialLeaveDetail.LeaveDate.Day))
                    cmbLeaves.SelectedValue = moLeaveDictionary[PartialLeaveDetail.LeaveDate.Day].ToString();

                Label leaveDate = (Label)oCurrentItem.FindControl("lblLeaveDate");
                leaveDate.Text = PartialLeaveDetail.LeaveDate.ToString("dd-MMM-yyyy");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    } 

    #endregion

    #region Methods

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        lblUserName.Text = moDatewiseStaffLeavesBL.SalaryCommonUtility.Name + " (" + moDatewiseStaffLeavesBL.SalaryCommonUtility.StaffGroupsName + ")";
        ApplyMouseHoverEffect(new List<Button> { btnSet, btnCancel });
    }

    /// <summary>
    /// This method is used to fill partial leaves into listview.
    /// </summary>
    private void FillPartialLeaveListview()
    {
        moDatewiseStaffLeavesBL = new DatewiseStaffLeavesBL();
        moDatewiseStaffLeavesBL.DatewiseStaffLeaves = new PayrollEntities.DatewiseStaffLeave
            {
                SchoolId = miSchoolId,
                AcademicYearId = miAcademicYearId,
                MonthId = Convert.ToInt32(hidMonthId.Value),
                Year = Convert.ToInt32(hidYear.Value),
                UserId = Convert.ToInt32(hidUserId.Value)
            };
        moDatewiseStaffLeavesBL.GetUsersPartialLeaveDetails();

        lstvwPartialLEaves.DataSource = moDatewiseStaffLeavesBL.PartialLeaveDetailsList;
        lstvwPartialLEaves.DataBind();
        btnSet.Visible = lstvwPartialLEaves.Items.Count > 0;
    }

    /// <summary>
    /// This method is used to decrypt query string.
    /// </summary>
    private void ReadQuerystring()
    {
	    if (Request.QueryString.ToString() == string.Empty)
		    return;
	    
		hidMonthId.Value = QueryString["MonthId"];
	    hidYear.Value = QueryString["Year"];
	    hidUserId.Value = QueryString["UserId"];
	    hidStaffGroupId.Value = QueryString["StaffGroupId"];
	    SetPartialLeaves(QueryString["PartialLeaves"]);
	    if (QueryString["Filter"] != null)
		    hidFilter.Value = QueryString["Filter"];
    }
    
    /// <summary>
    /// This method is used to fill partial leave dictionary.
    /// </summary>
    /// <param name="asPartialLeaves"></param>
    private void SetPartialLeaves(string asPartialLeaves)
    {
        if (!string.IsNullOrEmpty(asPartialLeaves))
        {
            string[] leaveDays;
            asPartialLeaves.Split('$').ToList().ForEach(
                leave =>
                {
                    leaveDays = leave.Split(',');
                    moLeaveDictionary.Add(Convert.ToInt32(leaveDays[0]), Convert.ToInt32(leaveDays[1]));
                });
        }
    }

    /// <summary>
    /// This method is used to set querystring.
    /// </summary>
    private string SetQueryString()
    {
        string sQueryString = "MonthId=" + hidMonthId.Value +
                              "&Year=" + hidYear.Value +
                              "&StaffGroupId=" + hidStaffGroupId.Value +
                              "&UserId=" + hidUserId.Value +
                              "&Filter=" + hidFilter.Value;
        btnCancel.PostBackUrl = "DatewiseStaffLeavesPopup.aspx?" + CommonUtility.EncryptQuerystring(sQueryString);
        return sQueryString;
    }

    /// <summary>
    /// This method is used to get partial leaves.
    /// </summary>
    /// <returns></returns>
    private string Set()
    {
        StringBuilder sLeaves = new StringBuilder();
        DropDownList cmbLeave;
        Label lblLeaveDate;
        foreach (ListViewDataItem oListViewDataItem in lstvwPartialLEaves.Items)
        {
            cmbLeave = (DropDownList)oListViewDataItem.FindControl("cmbLeave");
            lblLeaveDate = (Label)oListViewDataItem.FindControl("lblLeaveDate");
            sLeaves.Append("$" + Convert.ToDateTime(lblLeaveDate.Text).Day + "," + cmbLeave.SelectedValue);

        }
        if (sLeaves.Length > 0)
            return sLeaves.ToString().Substring(1);
        return string.Empty;
    } 

    #endregion
}