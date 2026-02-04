// File Name   : StudentCautionMoney.aspx.cs
// Created By  : Ketan
// Date        : 29/11/2007
// Modified By : Milind
// Date        : 11 Sept 09   

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using Utility;
using System.Globalization;
using CrystalDecisions.Shared;
using BusinessLogic;
using SchoolBusinessService;
using System.ServiceModel;
using AccountsEntities;
using PayrollReportingUserEntities;
using System.Linq;

/// <summary>
/// This Class is used to add or edit student caution money details .
/// </summary>
public partial class StudentCautionMoney : SchoolBase
{

	#region -- CONSTANT(s) --

	private const int I_COLUMN_INDEX_DOB = 3;
	private const int I_COLUMN_INDEX_AMOUNT = 5;
	private const int I_COLUMN_INDEX_PAID_DATE = 6;
	private const int I_COLUMN_INDEX_RETURN_DATE = 7;
	private const int I_COLUMN_INDEX_PAY = 8;
	private const int I_COLUMN_INDEX_RETURN = 9;
    private const int I_COLUMN_INDEX_DELETE = 11;

	#endregion -- CONSTANT(s) --

    #region -- CONSTANT(s) --

    private bool IsAccountsModuleEnabled
    {
        get { return Settings.EnableAccountsModule; }
    }

    #endregion

    #region -- EVENT HANDLER(s) --

    /// <summary>
	/// This method is used to decrypt query string and fill grid.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{			
			if (!IsPostBack)
			{
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                RefreshValue();
				Initialise();
				ReadQueryString();
				SetSortingFieldDefaultValues();
			}
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValue();
                if (btnShow.Text == Resources.LocalizedResources.Show)
                    btnShow.Text = Resources.LocalizedResources.Show;
                else
                    btnShow.Text = Resources.LocalizedResources.ChangeFilter;
                FillStudentsGrid();

            }
			SetClientScriptAttributes();


			hlnkBankDetails.Attributes.Add("onclick", string.Format("window.open('{0}', '_new','scrollbars=yes,resizable=no,top=0,left=0,width=700,height=650'); return false;", hlnkBankDetails.NavigateUrl));
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This method is used to set grid according to selected page in footer drop down list.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void grdStudents_PageIndexChanging(object sender, GridViewPageEventArgs e)
	{
		try
		{
			grdStudents.PageIndex = e.NewPageIndex;
			FillStudentsGrid();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This method is used to set query string for opening the pop up.
	/// AND also to fill the footer drop down list of the grid and set label according to that.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void grdStudents_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		try
		{
			if (e.Row.RowIndex >= 0)
			{
				string sCautionMode = string.Empty;
				string sStudentId = grdStudents.DataKeys[e.Row.RowIndex][Constants.I_ZERO].ToString();
                hidStudentId.Value = sStudentId;
				string sPaymentChequeId = grdStudents.DataKeys[e.Row.RowIndex]["Payment_Cheque_Id"].ToString();
				string sReturnChequeId = grdStudents.DataKeys[e.Row.RowIndex]["Return_Cheque_Id"].ToString();
				string sPaidByStudentId = grdStudents.DataKeys[e.Row.RowIndex]["Paid_By_Student"].ToString();
				string sReturnedBySchoolId = grdStudents.DataKeys[e.Row.RowIndex]["Returned_By_School"].ToString();
				string sdtSchoolLeftDate = grdStudents.DataKeys[e.Row.RowIndex]["SchoolLeft_Date"].ToString();
                bool bIsRTEStudent = grdStudents.DataKeys[e.Row.RowIndex]["Is_RTE_Student"].ToBool();
				string sdtAdmissionDate = grdStudents.DataKeys[e.Row.RowIndex]["Admission_Date"].ToDateTime().ToString("dd-MMM-yyyy", new CultureInfo("en"));
				string sdtPaidDate = grdStudents.DataKeys[e.Row.RowIndex]["Payment_Date"].ToString();
                string sElectronicPaymentId = grdStudents.DataKeys[e.Row.RowIndex]["ElectronicPaymentId"].ToString();
                string iNetBankingPaymentTransactionID = Convert.ToString(grdStudents.DataKeys[e.Row.RowIndex]["NetBankingPaymentTransactionID"].ToInt());
                hidPaidDate.Value = sdtPaidDate;
				string sAmount = e.Row.Cells[I_COLUMN_INDEX_AMOUNT].Text;
				string sUploadURL = string.Empty;
				if (optCMNotPaid.Checked)
					sCautionMode = "CMNotPaid";
				else if (optCMPaid.Checked)
					sCautionMode = "CMPaid";
				else if (optCMReturned.Checked)
					sCautionMode = "CMReturned";

				var sQueryString = new StringBuilder();
				sQueryString.Append(sPaidByStudentId == "False" ? "Mode=AddPaid" : "Mode=EditPaid");

                sQueryString.AppendFormat("&StudentId={0}&Payment_Cheque_Id={1}&Amount={2}&CautionMode={3}{4}&PageIndex={5}&AdmissionDate={6}&PostBackUrl={7}&ElectronicPaymentId={8}",
										   sStudentId,
										   sPaymentChequeId,
										   sAmount,
										   sCautionMode,
										   hidQueryString.Value,
										   grdStudents.PageIndex,
										   sdtAdmissionDate,
										   sUploadURL,
                                           sElectronicPaymentId);

				string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString.ToString());
				var oPayBtn = e.Row.Cells[I_COLUMN_INDEX_PAY].Controls[Constants.I_ZERO] as ImageButton;
				oPayBtn.Attributes["onclick"] = string.Format("window.open('CautionMoneyChequePopUp.aspx?{0}','_blank','scrollbars=yes,statusbar=no,resizable=no,width=850,height=530'); return false;", sEncrypt);

				sQueryString = new StringBuilder();
				sQueryString.Append(sReturnedBySchoolId == "False" ? "Mode=AddReturn" : "Mode=EditReturn");

                sQueryString.AppendFormat("&StudentId={0}&Payment_Cheque_Id={1}&Return_Cheque_Id={2}&Amount={3}&CautionMode={4}{5}&PageIndex={6}&AdmissionDate={7}&PostBackUrl={8}&ReturnElectronicPaymentId={9}",
										   sStudentId,
										   sPaymentChequeId,
										   sReturnChequeId,
										   sAmount,
										   sCautionMode,
										   hidQueryString.Value,
										   grdStudents.PageIndex,
										   sdtAdmissionDate,
										   sUploadURL,
                                           sElectronicPaymentId);

				sEncrypt = CommonUtility.EncryptQuerystring(sQueryString.ToString());
				var oReturnBtn = e.Row.Cells[I_COLUMN_INDEX_RETURN].Controls[Constants.I_ZERO] as ImageButton;
				oReturnBtn.Attributes["onclick"] = string.Format("window.open('CautionMoneyChequePopUp.aspx?{0}','_blank','scrollbars=yes,statusbar=no,resizable=no,width=650,height=530'); return false;", sEncrypt);

                var btnDelete = e.Row.Cells[I_COLUMN_INDEX_DELETE].Controls[Constants.I_ONE] as ImageButton;
                if (optCMNotPaid.Checked || iNetBankingPaymentTransactionID != Constants.S_ZERO)
                    btnDelete.Enabled = false;
                else
                {  
                    btnDelete.Visible = true;
                    btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");

                    if (moSchool == Constants.SchoolId.SNS)
                    {
                        ReportingUserConfigurationBL oReportingUserConfigurationBL = new ReportingUserConfigurationBL(miSchoolId, miAcademicYearId, miUserId);
                        List<ReportingUserConfiguration> lstUsers = oReportingUserConfigurationBL.GetAll();
                        if (moUserRole != Constants.UserRoles.Admin && (!lstUsers.FindAll(ru => ru.ReportingPrameterId == Constants.ReportingParameters.AllowUserToDeleteFee.ToInt() && ru.UserId == miUserId).Any()))
                            btnDelete.Visible = false;
                    }
                }


				if (optCMNotPaid.Checked)
					DisablePayReturnImageBtn(oReturnBtn);
				else if (optCMPaid.Checked)
				{
                    if (sReturnedBySchoolId == "True")
                        DisablePayReturnImageBtn(oPayBtn);
                    DisablePayReturnImageBtn((sdtSchoolLeftDate.Equals(string.Empty) && bIsRTEStudent == false) ? oReturnBtn : oPayBtn);  
                    if (e.Row.Cells[7].Text != '-'.ToString())
                        DisablePayReturnImageBtn(oReturnBtn);
				}
				else if (optCMReturned.Checked)
				{
                    DisablePayReturnImageBtn(oPayBtn);
                    if (sdtSchoolLeftDate.Equals(string.Empty) && bIsRTEStudent == false)
                        DisablePayReturnImageBtn(oReturnBtn);                   
				}

                 var oHyperLinkField = e.Row.Cells[I_COLUMN_INDEX_RETURN + 1].FindControl("lnkReciept") as LinkButton;
                if (miSchoolId != Constants.SchoolId.SNS.ToInt())
                {                   
                    if (!sdtPaidDate.IsNullOrEmpty())
                    {
                        string sQueryStr = string.Format("StudentId={0}&IsReturnMode={1}", sStudentId, (optCMReturned.Checked ? 1 : 0));
                        sQueryStr = CommonUtility.EncryptQuerystring(sQueryStr);

                        oHyperLinkField.Visible = true;
                        oHyperLinkField.Attributes.Add("onclick", string.Format("window.open('CautionMoneyReciept.aspx?{0}','_blank','scrollbars=yes,resizable=no,top=0,left=0,width=800,height=470'); return false;", sQueryStr));

                        e.Row.ForeColor = System.Drawing.Color.FromArgb(170, 170, 170);
                    }
                    else
                        oHyperLinkField.Visible = false;
                }

                if(optCMNotPaid.Checked)
                    oHyperLinkField.Visible = false;

                if (optCMPaid.Checked && iNetBankingPaymentTransactionID != Constants.S_ZERO)
                    oPayBtn.Enabled = false;
			}
			if (e.Row.RowType == DataControlRowType.Pager)
			{
				GridViewRow pagerRow = e.Row;

				// Retrieve the DropDownList and Label controls from the row.
				var pageList = pagerRow.Cells[0].FindControl("PageDropDownList") as DropDownList;
				var pageLabel = pagerRow.Cells[0].FindControl("CurrentPageLabel") as Label;

				if (pageList != null)
				{
					// Create the values for the DropDownList control based on 
					// the  total number of pages required to display the data
					// source.
					for (int i = 0; i < grdStudents.PageCount; i++)
					{
						// Create a ListItem object to represent a page.
						int pageNumber = i + 1;
						var item = new ListItem(pageNumber.ToString());

						if (i == grdStudents.PageIndex)
							item.Selected = true;

						// Add the ListItem object to the Items collection of the DropDownList.
						pageList.Items.Add(item);
					}
				}
				if (pageLabel != null)
				{
					// Calculate the current page number.
					int currentPage = grdStudents.PageIndex + 1;

					// Update the Label control with the current page information.
                    pageLabel.Text = string.Format(Resources.LocalizedResources.Page + " " + "{0} " + Resources.LocalizedResources.of1
                        + " " + "{1}", currentPage, grdStudents.PageCount);


				}
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

    /// <summary>
    /// This event is used for Display Caution Money Fee Receipt details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStudents_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {             
            int iStudentId = grdStudents.DataKeys[e.CommandArgument.ToInt()]["Schoolwise_Student_Id"].ToInt();
            string sReceiptNo = grdStudents.DataKeys[e.CommandArgument.ToInt()]["ReceiptNumber"].ToString();
            if (e.CommandName == "RECEIPT")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton lnkReceipt = (LinkButton)row.FindControl("lnkReciept");
                string sPaymentDate = grdStudents.DataKeys[e.CommandArgument.ToInt()]["Payment_Date"].ToString();  

                if (!sPaymentDate.IsNullOrEmpty())
                {
                    lnkReceipt.Visible = true;
                    if (miSchoolId == Constants.SchoolId.SNS.ToInt())
                    {
                        DisplayCautionMoneyReport(iStudentId);
                    }                    
                }
                else
                    lnkReceipt.Visible = false;
            }
            else if (e.CommandName == "REMOVE")
            {
                DeleteCautionMoneyDetails(iStudentId);

                if (IsAccountsModuleEnabled)
                {
                    var oVoucherClient = new AccountVoucherClient();
                    try
                    {
                        oVoucherClient.Open();
                        oVoucherClient.DeleteFeeVoucher(miSchoolId, miAcademicYearId, miFinancialYearId, iStudentId, sReceiptNo, string.Empty, miUserId, false);
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), String.Format("Accounts Module : An exception occured while deleting a fee payment. StudentId : {0}. ReceiptNo : {1}", iStudentId, sReceiptNo));
                    }
                    finally
                    {
                        if (oVoucherClient.State != CommunicationState.Faulted)
                            oVoucherClient.Close();
                    }
                }

                FillStudentsGrid();
            }
        }
        catch (ThreadAbortException)
        { }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    

	/// <summary>
	/// This event is used to sort the grid according to sort expression.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void grdStudents_Sorting(object sender, GridViewSortEventArgs e)
	{
		try
		{
			hidSortExpression.Value = e.SortExpression;
			hidSortDirection.Value = hidSortDirection.Value == Constants.S_DESCENDING ? Constants.S_ASCENDING : Constants.S_DESCENDING;

			FillStudentsGrid();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	///  This event is used to fill the grid according to filter.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			grdStudents.PageIndex = 0;
			FillGridAndSetShowBtnText();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to set grid according to selected page in the footer drop down list of grid.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void PageDropDownList_SelectedIndexChanged(Object sender, EventArgs e)
	{
		try
		{
			// Retrieve the pager row.
			GridViewRow pagerRow = grdStudents.BottomPagerRow;

			// Retrieve the PageDropDownList DropDownList from the bottom pager row.
			var pageList = pagerRow.Cells[0].FindControl("PageDropDownList") as DropDownList;

			// Set the PageIndex property to display that page selected by the user.
			grdStudents.PageIndex = pageList.SelectedIndex;
			FillStudentsGrid();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to set sorting image on the header of sortexpression.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void grdStudents_RowCreated(object sender, GridViewRowEventArgs e)
	{
		try
		{
			var sGridviewName = sender as GridView;

			if (e.Row.RowType == DataControlRowType.Header)
			{
				// Call the GetSortColumnIndex helper method to determine
				// the index of the column being sorted.
				int sortColumnIndex = CommonUtility.GetSortColumnIndex(sGridviewName, hidSortExpression.Value);

				if (sortColumnIndex != -1)
				{
					// Call the AddSortImage helper method to add
					// a sort direction image to the appropriate
					// column header. 
					CommonUtility.AddSortImage(sortColumnIndex, e.Row, hidSortDirection.Value);
				}

				var oFormNumber = grdStudents.Columns[1] as BoundField;
                if (miSchoolId == Constants.SchoolId.PPSH.ToInt())
					oFormNumber.Visible = true;                    
                else                
                    oFormNumber.Visible = false;                
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to handle the selected event of ObjectDataSource.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void GrdDSobj_Selected(object sender, ObjectDataSourceStatusEventArgs e)
	{
		try
		{
			if (e.ReturnValue != null && e.ReturnValue.ToString() != string.Empty)
			{
				lblStartIndex.Text = Convert.ToString((grdStudents.PageSize * grdStudents.PageIndex) + 1);
				lblEndIndex.Text = Convert.ToString((lblStartIndex.Text.ToInt() + grdStudents.PageSize) - 1);              
                    
				if (e.ReturnValue != null && e.ReturnValue.ToString() != string.Empty)
				{
					lblTotal.Text = e.ReturnValue.ToString();
					if (e.ReturnValue.GetType() != typeof(DataTable))
					{
						if (lblEndIndex.Text.ToInt() > lblTotal.Text.ToInt())
							lblEndIndex.Text = e.ReturnValue.ToString();
						trTotalRec.Visible = e.ReturnValue.ToString() != "0" && grdStudents.PageCount != 0;
					}
					if (lblTotal.Text != string.Empty)
						trTotalRec.Visible = lblTotal.Text.ToInt() > Constants.I_GRID_PAGE_COUNT;
				}
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to set filter of cheque number for displaying grid.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void optChequeNumber_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			ClearTexts();
			SetControlsForChequeNo();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to set filter of cheque number for displaying grid.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void optRegNo_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			ClearTexts();
			SetControlForRegNo();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to set filter of cheque number for displaying grid.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void optDate_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			ClearTexts();
			SetControlsForDate();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

    /// <summary>
    /// This evnt is used to set field state on change of return dates.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optReturnDate_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            ClearTexts();
            SetControlsForReturnDate();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }


    /// <summary>
    /// This evnt is used to set field stae on change of return status.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optCMReturned_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            SetReturnDateState();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This evnt is used to set field stae on change of return status.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optCMPaid_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            SetReturnDateState();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This evnt is used to set field stae on change of return status.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optCMNotPaid_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            SetReturnDateState();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

	#endregion -- EVENT HANDLER(s) --

	#region -- PRIVATE METHOD(s) --

	/// <summary>
	/// This method initialises variables.
	/// </summary>
	private void Initialise()
	{
        grdStudents.EmptyDataText = hidNoRecordFound.Value;
		grdStudents.PageSize = Constants.I_GRID_PAGE_COUNT;
		trTotalRec.Visible = false;
		optChequeNumber.Checked = true;
		trTotalRec.Visible = false;
		txtRegNo.Enabled = false;
		txtToDate.Enabled = false;
		txtFromDate.Enabled = false;
		cFromDate.Enabled = false;
		cToDate.Enabled = false;
		txtChequeNumber.Enabled = true;
		txtFromDate.Text = string.Empty;
		txtToDate.Text = string.Empty;
	}

	/// <summary>
	///		This method is used to set javascripts attributes to controls.
	/// </summary>
	private void SetClientScriptAttributes()
	{
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
		ApplyMouseHoverEffect(new List<Button> { btnShow, btnExport });

		SetDefaultButton(btnShow);

		optCMNotPaid.Attributes.Add("onkeypress", string.Format("return clickButton(event,'{0}')", btnShow.ClientID));
		optCMPaid.Attributes.Add("onkeypress", string.Format("return clickButton(event,'{0}')", btnShow.ClientID));
		optCMReturned.Attributes.Add("onkeypress", string.Format("return clickButton(event,'{0}')", btnShow.ClientID));
	}

	/// <summary>
	/// This method is used to set hidden field to default value for grid sorting.
	/// </summary>
	private void SetSortingFieldDefaultValues()
	{
		hidSortExpression.Value = grdStudents.Columns[0].SortExpression;
		hidSortDirection.Value = Constants.S_ASCENDING;
	}

	/// <summary>
	/// This method is used to disable pay or image button.
	/// </summary>
	/// <param name="oImageBtn"></param>
	private void DisablePayReturnImageBtn(ImageButton oImageBtn)
	{
		oImageBtn.ImageUrl = "~/RITeSchool/images/IconGrid_EditDis.gif";
		oImageBtn.Attributes.Add("onclick", "if(!NoAction()) {return false;}");        
	}

	/// <summary>
	/// This method is used to fill grid if show button text is Show input.
	/// </summary>
	private void FillGridAndSetShowBtnText()
	{
		if (btnShow.Text == Resources.LocalizedResources.Show)
		{
			SetQueryStringForSearch();
			FillStudentsGrid();
            btnShow.Text = Resources.LocalizedResources.ChangeFilter;
			EnableDisableControls(false);
			EnableDisableControlsChecked(false);
		}
		else
		{
            btnShow.Text = Resources.LocalizedResources.Show;
			grdStudents.DataSourceID = null;
			EnableDisableControls(true);
			EnableDisableControlsChecked(true);
			trTotalRec.Visible = false;
		}
	}

	/// <summary>
	/// This method is used to enable or disable option button as per button text.
	/// </summary>
	/// <param name="bFlag"></param>
	private void EnableDisableControls(bool bFlag)
	{
		optCMNotPaid.Enabled = bFlag;
		optCMPaid.Enabled = bFlag;
		optCMReturned.Enabled = bFlag;
		if (grdStudents.Rows.Count > Constants.I_GRID_PAGE_COUNT)
			trTotalRec.Visible = !bFlag;
		grdStudents.Visible = !bFlag;
		optChequeNumber.Enabled = bFlag;
		optRegNo.Enabled = bFlag;
		optDate.Enabled = bFlag;
		cFromDate.Enabled = bFlag;
		cToDate.Enabled = bFlag;
        calReturn1.Enabled = bFlag;
        calReturn2.Enabled = bFlag;
	}

	/// <summary>
	/// This Method is used to Enable or disable controls depending upon checked criteria.
	/// </summary>
	/// <param name="abFlag"> </param>
	private void EnableDisableControlsChecked(bool abFlag)
	{
		if (optRegNo.Checked)
			txtRegNo.Enabled = abFlag;
		if (optDate.Checked)
		{
			txtFromDate.Enabled = abFlag;
			txtToDate.Enabled = abFlag;
		}
		if (optChequeNumber.Checked)
			txtChequeNumber.Enabled = abFlag;

        if (optReturnDate.Checked)
        {
            txtReturnStartDate.Enabled = abFlag;
            txtReturnEndDate.Enabled = abFlag;
        }
	}

	/// <summary>
	/// This method is used to read query string.
	/// </summary>
	private void ReadQueryString()
	{
		string sEventDateDecrypt = Server.UrlDecode(Request.QueryString.ToString());
		if (!sEventDateDecrypt.IsNullOrEmpty())
		{
			hidCautionMode.Value = QueryString["CautionMode"];
			if (!QueryString["StudentRegNo"].IsNullOrEmpty())
			{
				SetControlForRegNo();
				txtRegNo.Text = QueryString["StudentRegNo"] != "-9999" ? QueryString["StudentRegNo"] : String.Empty;
				optRegNo.Checked = true;
			}
			else if (!QueryString["ChequeNo"].IsNullOrEmpty())
			{
				SetControlsForChequeNo();
				txtChequeNumber.Text = QueryString["ChequeNo"] != "-9999" ? QueryString["ChequeNo"] : String.Empty;
				optChequeNumber.Checked = true;
			}
			else if (!QueryString["ToDate"].IsNull() && !QueryString["FromDate"].IsNullOrEmpty())
			{
				SetControlsForDate();
				if (QueryString["FromDate"] != "-9999")
				{
					txtFromDate.Text = QueryString["FromDate"].ToDateTime().ToString(Constants.S_STANDARD_DATE_FORMAT);
					txtToDate.Text = QueryString["ToDate"].ToDateTime().ToString(Constants.S_STANDARD_DATE_FORMAT);
					cFromDate.DateValue = QueryString["FromDate"].ToDateTime();
					cToDate.DateValue = QueryString["ToDate"].ToDateTime();
				}
				else
				{
					txtFromDate.Text = String.Empty;
					txtToDate.Text = String.Empty;
				}
				optDate.Checked = true;
			}

			switch (hidCautionMode.Value)
			{
				case "CMNotPaid":
					optCMNotPaid.Checked = true;
					break;
				case "CMPaid":
					optCMPaid.Checked = true;
					break;
				case "CMReturned":
					optCMReturned.Checked = true;
					break;
			}

			FillGridAndSetShowBtnText();
		}
		else
		{
			btnExport.Style.Add("Visibility", "Hidden");
			optCMNotPaid.Checked = true;
            SetControlsForChequeNo();
            optReturnDate.Enabled = false;
		}
	}

	/// <summary>
	/// This method is used to fill student grid.
	/// </summary>
	private void FillStudentsGrid()
	{
		grdStudents.DataSourceID = GrdDSobj.ID;
		grdStudents.DataBind();
	}

	/// <summary>
	/// This method is used clear the text from the textboxes.
	/// </summary>
	private void ClearTexts()
	{
		txtChequeNumber.Text = string.Empty;
		txtRegNo.Text = string.Empty;
		txtFromDate.Text = string.Empty;
		txtToDate.Text = string.Empty;
        txtReturnStartDate.Text = string.Empty;
        txtReturnEndDate.Text = string.Empty;
	}

	/// <summary>
	/// Sets controls for Search by reg no.
	/// </summary>
	private void SetControlForRegNo()
	{
		txtRegNo.Enabled = true;
		txtToDate.Enabled = false;
		txtFromDate.Enabled = false;
		cFromDate.Enabled = false;
		cToDate.Enabled = false;
		txtChequeNumber.Enabled = false;
		trTotalRec.Visible = false;
		txtFromDate.Text = string.Empty;
		txtToDate.Text = string.Empty;
        txtReturnStartDate.Text = string.Empty;
        txtReturnEndDate.Text = string.Empty;
        EnableReturnDateFields(false);
	}

	/// <summary>
	/// Sets controls for search by cheque no.
	/// </summary>
	private void SetControlsForChequeNo()
	{
		trTotalRec.Visible = false;
		txtRegNo.Enabled = false;
		txtToDate.Enabled = false;
		txtFromDate.Enabled = false;
		cFromDate.Enabled = false;
		cToDate.Enabled = false;
		txtChequeNumber.Enabled = true;
		txtFromDate.Text = string.Empty;
		txtToDate.Text = string.Empty;
        txtReturnStartDate.Text = string.Empty;
        txtReturnEndDate.Text = string.Empty;
        EnableReturnDateFields(false);
	}

	/// <summary>
	/// Sets controls for search using date.
	/// </summary>
	private void SetControlsForDate()
	{
		txtRegNo.Enabled = false;
		txtToDate.Enabled = true;
		txtFromDate.Enabled = true;
		cFromDate.Enabled = true;
		cToDate.Enabled = true;
		txtChequeNumber.Enabled = false;
		trTotalRec.Visible = false;
		txtFromDate.Text = string.Empty;
		txtToDate.Text = string.Empty;
        txtReturnStartDate.Text = string.Empty;
        txtReturnEndDate.Text = string.Empty;
        EnableReturnDateFields(false);
	}

    /// <summary>
    /// Sets controls for search using date.
    /// </summary>
    private void SetControlsForReturnDate()
    {
        txtRegNo.Enabled = false;
        txtToDate.Enabled = false;
        txtFromDate.Enabled = false;
        cFromDate.Enabled = false;
        cToDate.Enabled = false;
        txtChequeNumber.Enabled = false;
        trTotalRec.Visible = false;
        txtFromDate.Text = string.Empty;
        txtToDate.Text = string.Empty;

        txtReturnStartDate.Text = string.Empty;
        txtReturnEndDate.Text = string.Empty;

        EnableReturnDateFields(true);
    }

    /// <summary>
    /// This method is used to enable return date related fields.
    /// </summary>
    /// <param name="abAction"></param>
    private void EnableReturnDateFields(bool abAction)
    {
        if (optCMReturned.Checked)
        {
            txtReturnStartDate.Enabled = abAction;
            txtReturnEndDate.Enabled = abAction;
            calReturn1.Enabled = abAction;
            calReturn2.Enabled = abAction;
        }
        else
        {
            txtReturnStartDate.Enabled = false;
            txtReturnEndDate.Enabled = false;
            calReturn1.Enabled = false;
            calReturn2.Enabled = false;
            optReturnDate.Checked = false;
        }
    }

	/// <summary>
	/// Sets the query string for search.
	/// </summary>
	private void SetQueryStringForSearch()
	{
		if (optDate.Checked)
			hidQueryString.Value = string.Format("&ToDate={0}&FromDate={1}", txtToDate.Text.Trim() != string.Empty ? txtToDate.Text.Trim() : "-9999", txtFromDate.Text.Trim() != string.Empty ? txtFromDate.Text.Trim() : "-9999");
		else if (optRegNo.Checked)
			hidQueryString.Value = string.Format("&StudentRegNo={0}", txtRegNo.Text.Trim() != string.Empty ? txtRegNo.Text.Trim() : "-9999");
		else
			hidQueryString.Value = string.Format("&ChequeNo={0}", txtChequeNumber.Text.Trim() != string.Empty ? txtChequeNumber.Text.Trim() : "-9999");
	}

    /// <summary>
    /// This Method is used to display receipt of caution money for SNS school.
    /// </summary>
    private void DisplayCautionMoneyReport(int aiStudentId)
    {   
        ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentCautionMoneySNS, GetCautionMoneyFilterString(aiStudentId), ExportFormatType.PortableDocFormat);
        oReportDisplay.DisplayReport();
    }

    /// <summary>
    /// This Method is used to get filters for display report.
    /// </summary>
    private string GetCautionMoneyFilterString(int aiStudentId)
    {
        string sRecordSelectionFormula = string.Empty;

        int iStudentId = hidStudentId.Value.ToInt();
        sRecordSelectionFormula = "(usp_GetCautionMoneyRecieptForSNS.SchoolId }=" + miSchoolId + "AND usp_GetCautionMoneyRecieptForSNS.StudentId }=" + aiStudentId + ") @";
        return sRecordSelectionFormula;
    }

    /// <summary>
    /// This method is used to delete the student caution money details.
    /// </summary>
    /// <param name="iStudentId"></param>
    private void DeleteCautionMoneyDetails(int iStudentId)
    {
        StudentCautionMoneyDetailsCollectionBL oStudentCautionMoneyDetailsCollectionBL = new StudentCautionMoneyDetailsCollectionBL();
        bool bIsReturn = false;
        if (optCMReturned.Checked)
            bIsReturn = true;
        oStudentCautionMoneyDetailsCollectionBL.DeleteCautionMoneyDetails(iStudentId, miSchoolId, miUserId, bIsReturn);
    }

	#endregion -- PRIVATE METHOD(s) --

	#region -- EXPORT FUNCTIONALITY --

	/// <summary>
	/// This event is used to export the caution money details in the Excel sheet.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnExport_Click(object sender, EventArgs e)
	{
		try
		{
			var oReportDisplay = new ReportDisplay(Constants.ExportReports.CautionMoneyDetails, GetFilterString());
			oReportDisplay.DisplayReport();
		}
		catch (ThreadAbortException)
		{ }
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This method generates the report filter as per the field selection.
	/// </summary>
	/// <returns></returns>
	private string GetFilterString()
	{
        return string.Format("({0}.School_Id}}={1} AND {0}.RegNumber}}={2} AND {0}.ChequeNo}}={3} AND {0}.StarDate}}={4} AND {0}.EndDate }}={5}  AND {0}.IncludePaid}}={6} AND {0}.IncludeReturned}}={7} AND {0}.ReturnStartDate}} = {8} AND {0}.ReturnEndDate}} = {9} )" + "@ ",
							  Constants.S_EXPORT_CAUTIONMONEY_USP,
							  miSchoolId,
							  txtRegNo.Text.Trim(),
							  txtChequeNumber.Text.Trim(),
							  txtFromDate.Text.Trim() == string.Empty ? DateTime.Now.ToString() : txtFromDate.Text.Trim(),
							  txtToDate.Text.Trim() == string.Empty ? DateTime.Now.AddDays(-1).ToString() : txtToDate.Text.Trim(),
							  optCMPaid.Checked.ToInt(),
							  optCMReturned.Checked.ToInt(),
                              txtReturnStartDate.Text.Trim() == string.Empty? DateTime.MinValue.ToString():txtReturnStartDate.Text.Trim(),
                              txtReturnEndDate.Text.Trim() == string.Empty ? DateTime.MinValue.ToString() : txtReturnEndDate.Text.Trim());
	}

    /// <summary>
    /// This method used to value based on Culture
    /// </summary>
    private void RefreshValue()
    {
        hidValStartDateBlank.Value = Resources.LocalizedResources.ValStartDateBlank;
        hidValEndDateBlank.Value = Resources.LocalizedResources.ValEndDateBlank;
        hidEndDateShouldBeGreaterThanStartDate.Value = Resources.LocalizedResources.EndDateShouldBeGreaterThanStartDate;
        HidLabel.Value =   Resources.LocalizedResources.Page;
        hidNoRecordFound.Value= Resources.LocalizedResources.NoRecordsFound;
        grdStudents.EmptyDataText = Resources.LocalizedResources.NoRecordsFound;
    }

    /// <summary>
    /// This method is used to set return date state.
    /// </summary>
    private void SetReturnDateState()
    {
        if (optCMReturned.Checked)
            optReturnDate.Enabled = true;
        else
        {
            optReturnDate.Enabled = false;

            if (optReturnDate.Checked)
            {
                optChequeNumber.Checked = true;
                txtReturnStartDate.Text = string.Empty;
                txtReturnEndDate.Text = string.Empty;
                EnableReturnDateFields(false);
                SetControlsForChequeNo();
            }
        }
    }

	#endregion -- EXPORT FUNCTIONALITY --    
}

