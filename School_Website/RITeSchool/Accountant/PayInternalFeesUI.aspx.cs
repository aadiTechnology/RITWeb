// File Name  : PayInternalFeesUI.aspx.cs
// Created By : Deepak
// Date       : 07/11/2009
//Description :This class is used to show internal fees details,pay internal fees and print reciept for internal fees.  

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using System.Resources;

public partial class PayInternalFeesUI : SchoolBase
{
    private ResourceManager oResourceManager = new ResourceManager(typeof(Resources.LocalizedResources));
	#region -- CONSTANT(s) --

	private const string S_DEFAULT_SORT_EXP = "Enrolment_Number";

	#endregion -- CONSTANT(s) --

	#region -- EVENT HANDLER(s) --

	/// <summary>
	/// This event is used set controls visibility,set default values and decrypt query string.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{			
			if (!IsPostBack)
			{
                hidShow.Value = "Show";
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                DesignSettingAccordinglanguage();
				SetDefaultValues();
				SetJavaScriptAttributes();				
				ReadQuerystring();
			}
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
			SetDefaultButton(btnShow);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}  

	/// <summary>
	/// This event is used to set list view footer.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwStudent_DataBound(object sender, EventArgs e)
	{
		try
		{
			if (lstvwStudent.Items.Count > 0)
			{
				//ControlUtility.FillListViewPagerFooter(lstvwStudent, DtPgCount);
                ControlUtility.FillListViewPagerFooterWithCulture(lstvwStudent, DtPgCount, Resources.LocalizedResources.PageNo, Resources.LocalizedResources.Of, Resources.LocalizedResources.OutOflst);
				//if (optFeeNotPaid.Checked)
				//	btnSendSms.Visible = true;
			}
			else
			{
				btnSendSms.Visible = false;
				DtPgCount.Visible = false;
			}			
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to enable disable listview controls and set javascript to listview buttons.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwStudent_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
                var oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = oCurrentItem.DisplayIndex;    
                var oimgbtnEdit = e.Item.FindControl("imgBtnEdit") as ImageButton;                
                var ohlnkCustomReceipt = e.Item.FindControl("hlnkCustomReceipt") as HyperLink;
                var oDtPgr = lstvwStudent.FindControl("DtPgDropDown") as DataPager;
                int iPageIndex = (oDtPgr.StartRowIndex / oDtPgr.PageSize) + 1;
                int iStudentId = lstvwStudent.DataKeys[iRowId]["SchoolWise_Student_Id"].ToInt();                
                string sStudentName = Convert.ToString(lstvwStudent.DataKeys[iRowId]["StudentName"]);
                int iAmount = lstvwStudent.DataKeys[iRowId]["TotalAmount"].ToInt();
                int iPendingAmount = lstvwStudent.DataKeys[iRowId]["PendingAmount"].ToInt();
                if (iAmount == iPendingAmount)
                    ohlnkCustomReceipt.Visible = false;

                string sQueryString = String.Format("StudentId={0}&StudentName={1}&Amount={2}&RegNo={3}&pIndex={4}",
                                                    iStudentId,
                                                    //iNextAcademicYearId,
                                                    sStudentName,
                                                    iAmount,
                                                    txtRegNo.Text,                                                    
                                                    iPageIndex
                                                    //iInternalFeeDetailsId
                                                    );

                oimgbtnEdit.Attributes.Add("onclick", "if(!OpenPopup( 'PayInternalFeePopup.aspx?" + CommonUtility.EncryptQuerystring(sQueryString) + "' )) return false;");                
                ohlnkCustomReceipt.Attributes.Add("onclick", "if(!OpenPopup( 'CustomizeInternalRecieptPopUp.aspx?" + CommonUtility.EncryptQuerystring(sQueryString) + "' )) return false;");                
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

    
	protected void btnSendSms_Click(object sender, EventArgs e)
	{
		try
		{
            //const string S_PAGE = "InternalFee";

            //string sQueryStr = string.Format("From={0}&RegNo={1}&FromDate={2}&ToDate={3}&IncludePaid={4}&PayForNextYear={5}&IsRegNoFilter={6}&FeeTypeID={7}",
            //                                  S_PAGE, 
            //                                  txtRegNo.Text, 
            //                                  txtFromDate.Text, 
            //                                  txtToDate.Text, 
            //                                  optFeePaid.Checked, 
            //                                  chkPayForNextYear.Checked, 
            //                                  optRegNo.Checked, 
            //                                  ddlInternalFeeType.SelectedValue);

            //string sQueryString = CommonUtility.EncryptQuerystring(sQueryStr);
            //var oMasterPage = this.Master as MasterPage;
            //oMasterPage.RedirectToNextPage("~/Common/SMSUI.aspx?" + sQueryString);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event used set paging for listview.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			ControlUtility.SetDataPagerAccordingToPageNo(lstvwStudent);			
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event used show students fee details.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
            if (hidShow.Value == "Show")
			{
                ToggleListView(true);                
				FillStudentList();								
				btnShow.Text = Resources.LocalizedResources.ChangeFilter;
                hidShow.Value = "Change Filter";
                txtRegNo.Enabled = false;
			}
			else
			{
                ToggleListView(false);								
				btnShow.Text = Resources.LocalizedResources.Show;
                hidShow.Value = "Show";
				btnSendSms.Visible = false;
                txtRegNo.Enabled = true;
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}	

	#endregion -- EVENT HANDLER(s) --

	#region -- PRIVATE METHOD(s) --	

	/// <summary>
	/// This method is used make list view visible or hide it.
	/// </summary>
	/// <param name="abAction"></param>
	private void ToggleListView(bool abAction)
	{
		lstvwStudent.DataSourceID = null;
		lstvwStudent.Visible = abAction;
		trTotalRec.Visible = abAction;
	}

	/// <summary>
	/// This method sets registration no. and fee not paid option buttons checked by default.
	/// </summary>
	private void SetDefaultValues()
	{
		txtRegNo.Focus();
        btnSendSms.Visible = false;
        hidbaseUrl.Value = Request.Url.GetLeftPart(UriPartial.Authority);
        txtRegNo.Attributes.Add("onkeypress", string.Format("return clickButton(event,'{0}')", btnShow.ClientID));
		if (Settings.IsMiniSite)
			btnSendSms.Visible = false;
	}

	
	/// <summary>
	/// This event is used to fill student list view.
	/// </summary>
	private void FillStudentList()
	{
		lstvwStudent.DataSourceID = objDSStudentList.ID;
		lstvwStudent.DataBind();
	}

	private void AddImageToHeader(HtmlTableRow aoHtmlTableRow, string asSortExpression, string asSortDirection)
	{
		if (asSortExpression.Trim().Equals(String.Empty))
			return;

		// Create the sorting image based on the sort direction.
		var sortImage = new Image();
		sortImage.ID = "sortImage";
		
		switch (asSortDirection)
		{
			case "asc":
				sortImage.ImageUrl = "~/RITeSchool/images/up.gif";
				sortImage.AlternateText = "Ascending Order";
				break;
			case "desc":
				sortImage.ImageUrl = "~/RITeSchool/images/down.gif";
				sortImage.AlternateText = "Descending Order";
				break;
		}
		
		// Iterate through the Columns collection to determine the index
		// of the column being sorted.
		foreach (HtmlTableCell oHtmlTableCell in aoHtmlTableRow.Cells)
		{
			asSortExpression = asSortExpression.Replace(" ", String.Empty).Replace("asc", String.Empty).Replace("desc", String.Empty);

			// Iterate through the cells collection to determine the index
			// of the cell being sorted.
			foreach (Control oControl in oHtmlTableCell.Controls)
			{
				var oLinkButton = oControl as LinkButton;
				if (oLinkButton != null && oLinkButton.CommandArgument == asSortExpression)
				{
					var oImage = oHtmlTableCell.FindControl("sortImage") as Image;
					if (oImage == null)
					{
						// Add the image to the appropriate header cell.
						if (sortImage.ImageUrl != String.Empty)
						{
							oHtmlTableCell.Controls.Add(sortImage);
							break;
						}
					}

				}
			}
		}
	}

	/// <summary>
	/// This method is used to decrypt query string.
	/// </summary>
	private void ReadQuerystring()
	{
        if (Request.QueryString.ToString() != Constants.S_EMPTY_STRING)
        {
            if (!QueryString["RegNo"].IsNull())
                txtRegNo.Text = QueryString["RegNo"];

            if (!QueryString["FeeTypeID"].IsNull())
                hidFeeTypeID.Value = QueryString["FeeTypeID"];

            if (!QueryString["pIndex"].IsNull())
                hidPageIndex.Value = QueryString["pIndex"];
            FillStudentList();
            btnShow.Text = Resources.LocalizedResources.ChangeFilter;
            hidShow.Value = "Change Filter";
            txtRegNo.Enabled = false;
            SetDataPagerValue();
        }
        else
        {
            txtRegNo.Enabled = true;
        }
	}

	/// <summary>
	/// This method used to set java script attributes for buttons.
	/// </summary>
	private void SetJavaScriptAttributes()
	{
		ApplyMouseHoverEffect(new List<Button> { btnShow, btnSendSms });
	}

	/// <summary>
	/// This method is used to set data pager value after popup closed.
	/// </summary>
	private void SetDataPagerValue()
	{
		if (hidPageIndex.Value.ToInt() <= 0)
			return;
		
		var oDtPager = lstvwStudent.FindControl("DtPgDropDown") as DataPager;
		
		// If the records displayed on the page are less than the page size, we need not show the pager controls.
		if (oDtPager == null || (oDtPager.TotalRowCount <= oDtPager.PageSize))
			return;
		
		var ddlCnt = (oDtPager.Controls[0].FindControl("ddlCnt")) as DropDownList;
		ddlCnt.SelectedValue = hidPageIndex.Value;
		cmbPageCnt_SelectedIndexChanged(ddlCnt, null);
	}
    /// <summary>
    /// This method es used to set design according to the language selected.
    /// </summary>
    private void DesignSettingAccordinglanguage()
    {
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        btnShow.Text = oResourceManager.GetString(hidShow.Value.Replace(" ", string.Empty));
        hidAreYouSureYouWantToDeleteThisRecord.Value = Resources.LocalizedResources.AreYouSureYouWantToDeleteThisRecords;
        if (lstvwStudent.Items.Count > 0)
         ControlUtility.FillListViewPagerFooterWithCulture(lstvwStudent, DtPgCount, Resources.LocalizedResources.PageNo, Resources.LocalizedResources.Of, Resources.LocalizedResources.OutOflst);
    }

	#endregion -- PRIVATE METHOD(s) --
}