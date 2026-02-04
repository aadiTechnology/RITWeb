// File Name   : OnlineAdmissionFeeClearanceListUI.aspx.cs
// Created By  : Vinod
// Date        : -
// Modified By : 
// Date        : 27 Dec 10
// Description : This class is used to display the bank Online Admission fee details in grid and allow
//the user to add and modity clearance date and TSPLTransactionID.

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using SchoolBusinessService;
using AccountsEntities;
using System.ServiceModel;
using System.Web.Script.Serialization;
using System.Web;
using System.Threading;

public partial class OnlineAdmissionFeeClearanceListUI : SchoolBase
{
	#region -- CONSTANT(s) --

	private const int GRID_PAGE_COUNT = 20;
    private List<BankAccount> mlstBanks;

	#endregion -- CONSTANT(s) --

    #region -- PROPERTIES --

    /// <summary>
    /// Returns true if the Accounts module is enabled, false otherwise.
    /// </summary>
    private bool IsAccountsModuleEnabled
    {
        get { return Settings.EnableAccountsModule; }
    }

    #endregion -- PROPERTIES --

    #region -- EVENT HANDLER(s) --

	/// <summary>
	/// This event is used set default properties to controls on the page.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{			
			if (!IsPostBack)
			{
				InitialiseControls();
				FillStandardNameComboBox();
			}
			SetJavaScriptAttribute();
            SerializeFinancialYear();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to fill the grid according to filter.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			if (btnShow.Text == "Show")
			{
				hidPageNo.Value = Constants.I_ONE.ToString();
				grdOnlineAdmissionFeeDetails.PageIndex = Constants.I_ZERO;
				FillOnlineAdmissionFeeGrid();
				btnShow.Text = "Change Input";
				EnableDisableControlChecked(false);
				EnableDisableControl(false);
				FillStandardNameComboBox();                
			}
			else
			{
				btnShow.Text = "Show";
				EnableDisableControlChecked(true);
				EnableDisableControl(true);
				grdOnlineAdmissionFeeDetails.DataSource = null;
				grdOnlineAdmissionFeeDetails.Visible = false;
				trTotalRec.Visible = false;
				FillStandardNameComboBox();
				tblTotalAmount.Visible = false;
			}
			lblError.Visible = false;

		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to fill grid row data. 
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void grdOnlineAdmissionFeeDetails_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		try
		{
			if (e.Row.RowType == DataControlRowType.Pager)
			{
				var oPageList = e.Row.Cells[0].FindControl("PageDropDownList") as DropDownList;
				oPageList.Attributes.Add("onchange", "if(!MessageAboutDate('" + oPageList.ClientID + "')){return false;}");
				var oPageLabel = e.Row.Cells[0].FindControl("CurrentPageLabel") as Label;
				
				for (int i = 0; i < grdOnlineAdmissionFeeDetails.PageCount; i++)
				{
					int iPageumber = i + 1;
					var oListItem = new ListItem(iPageumber.ToString());

					if (i == grdOnlineAdmissionFeeDetails.PageIndex)
						oListItem.Selected = true;
					oPageList.Items.Add(oListItem);
				}
				
				if (oPageLabel != null)
				{
					int iCurrentPageCount = grdOnlineAdmissionFeeDetails.PageIndex + 1;
					oPageLabel.Text = string.Format("Page {0} of {1}", iCurrentPageCount, grdOnlineAdmissionFeeDetails.PageCount);
				}               

				DisplayRowDetails();
			}

            if (IsAccountsModuleEnabled && e.Row.RowType == DataControlRowType.DataRow)
            {
                var ddlDepositBankList = e.Row.FindControl("ddlDepositedBankList") as DropDownList;
                ddlDepositBankList.Bind(GetBankList(), "Id", "Name", Constants.S_SELECT);
                if (grdOnlineAdmissionFeeDetails.DataKeys[e.Row.RowIndex]["DepositeBankId"].ToInt() != 0)
                {
                    //ddlDepositBankList.SelectedItem.Text = grdOnlineAdmissionFeeDetails.DataKeys[e.Row.RowIndex]["DepositedBankName"].ToString();
                    ListItem oItem = ddlDepositBankList.Items.FindByText(grdOnlineAdmissionFeeDetails.DataKeys[e.Row.RowIndex]["DepositedBankName"].ToString());
                    if (oItem != null)
                        oItem.Selected = true;
                }
                else
                {
                    if (miSchoolId == Constants.SchoolId.PPSN.ToInt())
                    {
                        ListItem oItem = ddlDepositBankList.Items.FindByText("AXIS BANK");
                        if (oItem != null)
                            oItem.Selected = true;
                    }
                    else                    
                        ddlDepositBankList.SelectedValue = grdOnlineAdmissionFeeDetails.DataKeys[e.Row.RowIndex]["DepositeBankId"].ToString();
                }
            }
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

    /// <summary>
    /// Serializes the FinancialYearMaster entity object to a hidden field.
    /// </summary>
    private void SerializeFinancialYear()
    {
        if (!IsAccountsModuleEnabled)
            return;

        var oFinancialYear = Session[Constants.S_SESSION_FINANCIAL_YEAR] as FinancialYear;
        if (oFinancialYear != null)
        {
            var jsSerializer = new JavaScriptSerializer();
            hidFinancialYearJSON.Value = jsSerializer.Serialize(oFinancialYear);
        }

        if (Session[Constants.S_SESSION_CAN_EDIT_OLD_FINANCIAL_YEAR] != null)
            hidCanEditOldFinancialYear.Value = Session[Constants.S_SESSION_CAN_EDIT_OLD_FINANCIAL_YEAR].ToString().ToLower();
    }

    /// <summary>
    /// Returns the list of bank accounts configured in the accounts module.
    /// </summary>
    /// <returns>A List of BankAccountDetails entity objects.</returns>
    private List<BankAccount> GetBankList()
    {
        if (mlstBanks == null)
        {
            BankAccountClient oBankClient = null;
            try
            {
                oBankClient = new BankAccountClient();
                oBankClient.Open();
                mlstBanks = oBankClient.GetAllBanksDetails(miSchoolId, miFinancialYearId);
            }
            catch (Exception ex)
            {
                ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), "Accounts Module : There was an error fetching Bank details.");
            }
            finally
            {
                if (oBankClient != null && oBankClient.State != CommunicationState.Faulted)
                    oBankClient.Close();
            }
        }

        return mlstBanks;
    }

	/// <summary>
	/// This event is used to set control enable or disable 
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void optFormNo_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			optFormNoChecked();
			DisableErrorLabel();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to set control enable or disable 
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void optStandardName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			optStandardNameChecked();
			DisableErrorLabel();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to set control enable or disable 
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void optTransactionNumber_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			OptTrasactionIdCheck();
			DisableErrorLabel();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to save value updated in grid and also check is TPSlTransaction ID duplicate or not.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
			string sOnlineAdmissionFeeXML = GenerateXML();

			var oUpdateNetBankingPaymentTransactions = new NetBankingPaymentTransactionsBL();
			oUpdateNetBankingPaymentTransactions.SetOnlineAdmissionFeeDetails(sOnlineAdmissionFeeXML, miSchoolId, miAcademicYearId);

            if (IsAccountsModuleEnabled)
                RecordPayment();

			FillOnlineAdmissionFeeGrid();
			lblError.Visible = false;
			lblSuccessMsg.Visible = true;
			lblSuccessMsg.Text = "Online transaction clearance data updated successfully !!!";
		}
		catch (SqlException sex)
		{
			lblError.ForeColor = System.Drawing.Color.Red;
			lblError.Text = "TPSLTransactionID should not be duplicate for student(s): " + sex.Message;
			lblError.Visible = true;
			lblSuccessMsg.Visible = false;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

    /// <summary>
    /// This event is used to Export Clearance Details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            const int FILTER_BY_TRANSACTION_ID = 1;
            const int FILTER_BY_FORM_NUMBER = 2;
            const int FILTER_BY_STANDARD_NAME = 3;
            bool bChkAll = chkIncludeAll.Checked;

            DataSet oDSOnlineTransaction = new DataSet();
            DataTable oDTOnlineTransaction = new DataTable();

            if (optTransactionNumber.Checked)
            {
                oDSOnlineTransaction = NetBankingPaymentTransactionsBL.FetchOnlineAdmissionFeeDetail(txtTransactionIDNumber.Text.Trim(), FILTER_BY_TRANSACTION_ID, miSchoolId, miAcademicYearId, bChkAll);
                oDTOnlineTransaction = oDSOnlineTransaction.Tables[0];
            }
            else if (optFormNo.Checked)
            {
                oDSOnlineTransaction = NetBankingPaymentTransactionsBL.FetchOnlineAdmissionFeeDetail(txtFormNo.Text.Trim(), FILTER_BY_FORM_NUMBER, miSchoolId, miAcademicYearId, bChkAll);
                oDTOnlineTransaction = oDSOnlineTransaction.Tables[0];
            }
            else if (optStandardName.Checked)
            {
                if (cmbStandardName.SelectedItem.ToString() == "--All--")
                {
                    cmbStandardName.SelectedItem.Text = String.Empty;
                    oDSOnlineTransaction = NetBankingPaymentTransactionsBL.FetchOnlineAdmissionFeeDetail(cmbStandardName.SelectedItem.ToString(), FILTER_BY_STANDARD_NAME, miSchoolId, miAcademicYearId, bChkAll);
                    oDTOnlineTransaction = oDSOnlineTransaction.Tables[0];
                }
                else
                {
                    oDSOnlineTransaction = NetBankingPaymentTransactionsBL.FetchOnlineAdmissionFeeDetail(cmbStandardName.SelectedItem.ToString(), FILTER_BY_STANDARD_NAME, miSchoolId, miAcademicYearId, chkIncludeAll.Checked);
                    oDTOnlineTransaction = oDSOnlineTransaction.Tables[0];
                }
            }
            else if (optPaymentDate.Checked)
            {
                const int FILTER_BY_PAID_DATE = 4;
                oDSOnlineTransaction = NetBankingPaymentTransactionsBL.FetchOnlineAdmissionFeeDetails(txtPaymentStartDate.Text.Trim(), txtPaymentEndDate.Text.Trim(), null, FILTER_BY_PAID_DATE, miSchoolId, miAcademicYearId, bChkAll);
                oDTOnlineTransaction = oDSOnlineTransaction.Tables[0];
            }

            else if (optClearanceDate.Checked)
            {
                const int FILTER_BY_CLEARANCE_DATE = 5;
                oDSOnlineTransaction = NetBankingPaymentTransactionsBL.FetchOnlineAdmissionFeeDetails(txtClearanceStartDate.Text.Trim(), txtClearanceEndDate.Text.Trim(), null, FILTER_BY_CLEARANCE_DATE, miSchoolId, miAcademicYearId, bChkAll);
                oDTOnlineTransaction = oDSOnlineTransaction.Tables[0];
            }
           
           
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Report-StudentAdmission.XLS");
            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            HttpContext.Current.Response.Write("<BR><BR><BR>");

            HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:15px; font-family:Calibri; background:white;'>");
            HttpContext.Current.Response.Write("<TR>");

            AddHeader("Form No.", "text-align:left; font-weight:bold; font-size:17px;");
            AddHeader("Student Name", "text-align:left; font-weight:bold; font-size:17px;");
            AddHeader("Standard Name", "text-align:left; font-weight:bold; font-size:17px;");
            AddHeader("Transaction Id", "text-align:left; font-weight:bold; font-size:17px;");
            AddHeader("Bank Name", "text-align:left; font-weight:bold; font-size:17px;");
            AddHeader("Amount", "text-align:center; font-weight:bold; font-size:17px;");
            AddHeader("Transaction Date", "text-align:center; font-weight:bold; font-size:17px;");
            AddHeader("Clearance Date", "text-align:center; font-weight:bold; font-size:17px;");
            AddHeader("Deposited Bank", "text-align:left; font-weight:bold; font-size:17px;");
           
            HttpContext.Current.Response.Write("</TR>");

            foreach (DataRow row in oDTOnlineTransaction.Rows)
            {
                HttpContext.Current.Response.Write("<TR>");

                AddTableRows(row["Form_Number"].ToString(), "text-align:left");
                AddTableRows(row["StudentName"].ToString(), "text-align:left");
                AddTableRows(row["Standard_Name"].ToString(), "text-align:left");
                AddTableRows(row["TPSLTransactionID"].ToString() + "&nbsp;", "text-align:left");
                AddTableRows(row["RegisterdBankName"].ToString(), "text-align:left");
                AddTableRows(row["Amount"].ToString(), "text-align:center");
                AddTableRows(row["TransactionDateTime"].ToDateTime().ToString(Constants.S_DATE_FORMAT), "text-align:center");

                if (row["ClearanceDate"].ToString() != string.Empty && row["ClearanceDate"].ToString() != null)
                    AddTableRows(row["ClearanceDate"].ToDateTime().ToString(Constants.S_DATE_FORMAT), "text-align:center");
                else
                    AddTableRows(string.Empty, "text-align:center");

                AddTableRows(row["DepositedBankName"].ToString(), "text-align:left");

                HttpContext.Current.Response.Write("</TR>");
            }

            HttpContext.Current.Response.Write("</Table>");
            HttpContext.Current.Response.Write("</font>");
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End(); 
        }
        catch (ThreadAbortException)
        {
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
	protected void grdOnlineAdmissionFeeDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
	{
		try
		{
			grdOnlineAdmissionFeeDetails.PageIndex = e.NewPageIndex;
			FillOnlineAdmissionFeeGrid();
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
	protected void PageDropDownList_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			GridViewRow oPageRow = grdOnlineAdmissionFeeDetails.BottomPagerRow;
			var oPageNumberList = oPageRow.Cells[0].FindControl("PageDropDownList") as DropDownList;
			grdOnlineAdmissionFeeDetails.PageIndex = oPageNumberList.SelectedIndex;
			FillOnlineAdmissionFeeGrid();
			hidPageNo.Value = (oPageNumberList.SelectedIndex + 1).ToString();
			lblError.Visible = false;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	#endregion -- EVENT HANDLER(s) --

	#region -- PRIVATE METHOD(s) --

    /// <summary>
    /// This method is used for Adding the row Header.
    /// </summary>
    private void AddHeader(string asText, string asStyle = "")
    {
        string sStyle = string.Empty;
        if (asStyle != string.Empty)
            sStyle = "style='" + asStyle + "'";
        HttpContext.Current.Response.Write("<Td colspan='" + "' " + sStyle + ">");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(asText);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
    }

    /// <summary>
    /// 	This method is used for Adding the rows in to Table.
    /// </summary>
    private void AddTableRows(string sRowHeader, string asStyle = "")
    {
        string sStyle = string.Empty;
        if (asStyle != string.Empty)
            sStyle = "style='" + asStyle + "'";
        HttpContext.Current.Response.Write("<TD " + sStyle + ">");
        HttpContext.Current.Response.Write(sRowHeader.ToString());
        HttpContext.Current.Response.Write("</TD>");
    }

	/// <summary>
	/// This method used to disabled error labels.
	/// </summary>
	private void DisableErrorLabel()
	{
		trTotalRec.Visible = false;
		lblError.Visible = false;
	}

	/// <summary>
	/// This event is used set default properties to controls on the page.
	/// </summary>
	private void InitialiseControls()
	{
		txtTransactionIDNumber.Focus();
		OptTrasactionIdCheck();
		optTransactionNumber.Checked = true;
		grdOnlineAdmissionFeeDetails.Visible = false;
		grdOnlineAdmissionFeeDetails.PageSize = GRID_PAGE_COUNT;
		valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
		btnSave.Style.Add("Visibility", "Hidden");
        btnExport.Style.Add("Visibility", "Hidden");
		
		SetDefaultButton(btnShow);
	}

	/// <summary>
	/// This method used to set TPSLTransaction ID filter as well as to enabled or disabled 
	/// controls according to that.
	/// </summary>
	private void OptTrasactionIdCheck()
	{
		SetDefaultValuestoControls();
		txtFormNo.Enabled = false;
		cmbStandardName.Enabled = false;
		txtTransactionIDNumber.Enabled = true;
	}

	/// <summary>
	/// This method is used clear the text from the textboxes.
	/// </summary>
	private void SetDefaultValuestoControls()
	{
		txtFormNo.Text = string.Empty;
		txtTransactionIDNumber.Text = string.Empty;
		cmbStandardName.SelectedValue = "0";
	}

	/// <summary>
	/// This method used to set Form No filter as well as to enabled or disabled 
	/// controls according to that.
	/// </summary>
	private void optFormNoChecked()
	{
		SetDefaultValuestoControls();
		txtFormNo.Enabled = true;
		cmbStandardName.Enabled = false;
		txtTransactionIDNumber.Enabled = false;
	}

	/// <summary>
	/// This method used to set Standard Name filter as well as to enabled or disabled 
	/// controls according to that.
	/// </summary>
	private void optStandardNameChecked()
	{
		SetDefaultValuestoControls();
		txtFormNo.Enabled = false;
		cmbStandardName.Enabled = true;
		txtTransactionIDNumber.Enabled = false;
	}

    /// <summary>
	/// This method is used set JavaScript Attribute.
	/// </summary>
	private void SetJavaScriptAttribute()
	{
		ApplyMouseHoverEffect(new List<Button> { btnShow, btnSave, btnExport });
		optTransactionNumber.Attributes.Add("onclick", "if(!ClearValSum()){return false;}");
		optFormNo.Attributes.Add("onclick", "if(!ClearValSum()){return false;}");
		optStandardName.Attributes.Add("onclick", "if(!ClearValSum()){return false;}");
		btnSave.Style.Add("Visibility", "Hidden");
        btnExport.Style.Add("Visibility", "Hidden");
	}

	/// <summary>
	/// This Methos is used fill standard name combobox.
	/// </summary>
	private void FillStandardNameComboBox()
	{
		DataTable oDTStandarName = NetBankingPaymentTransactionsBL.FetchStandardNameDetails(miSchoolId, miAcademicYearId);
		string sCmbValue = cmbStandardName.SelectedValue;
		cmbStandardName.Items.Clear();
		cmbStandardName.Items.Add(new ListItem("--All--", Constants.S_ZERO));
		cmbStandardName.DataSource = oDTStandarName;
		cmbStandardName.DataTextField = "Standard_Name";
		cmbStandardName.DataValueField = "Standard_Id";
		cmbStandardName.DataBind();
		cmbStandardName.SelectedValue = Constants.S_ZERO;
		if (cmbStandardName.Items.FindByValue(sCmbValue) != null)
			cmbStandardName.SelectedValue = sCmbValue;
	}

	/// <summary>
	/// This method used to enabled or disabled radio button controls..
	/// <param name="abFlag"></param>
	/// </summary>
	private void EnableDisableControlChecked(bool abFlag)
	{
		if (optTransactionNumber.Checked)
			txtTransactionIDNumber.Enabled = abFlag;
		else if (optFormNo.Checked)
			txtFormNo.Enabled = abFlag;
		else if (optStandardName.Checked)
			cmbStandardName.Enabled = abFlag;
	}

	/// <summary>
	/// This method used to set the value to the label indicating records from the grid.
	/// </summary>
	private void DisplayRowDetails()
	{
		int iRowCount = ((DataTable)(grdOnlineAdmissionFeeDetails.DataSource)).Rows.Count;

		lblStartIndex.Text = Convert.ToString((grdOnlineAdmissionFeeDetails.PageSize * grdOnlineAdmissionFeeDetails.PageIndex) + 1);
		lblEndIndex.Text = Convert.ToString((lblStartIndex.Text.ToInt() + grdOnlineAdmissionFeeDetails.PageSize) - 1);
		lblTotal.Text = iRowCount.ToString();

		if (lblEndIndex.Text.ToInt() > lblTotal.Text.ToInt())
			lblEndIndex.Text = iRowCount.ToString();

		trTotalRec.Visible = iRowCount.ToString() != "0";

		if (lblTotal.Text != String.Empty)
			trTotalRec.Visible = lblTotal.Text.ToInt() > Constants.I_GRID_PAGE_COUNT;
	}

	/// <summary>
	/// This method used to enabled or disabled controls.
	/// <param name="abFlag"></param>
	/// </summary>
	private void EnableDisableControl(bool abFlag)
	{
		optTransactionNumber.Enabled = abFlag;
		optFormNo.Enabled = abFlag;
		optStandardName.Enabled = abFlag;
		chkIncludeAll.Enabled = abFlag;
	}

	/// <summary>
	/// This method used to fill the grid according to selected filter.
	/// </summary>
	private void FillOnlineAdmissionFeeGrid()
	{
		lblError.Visible = false;
		const int FILTER_BY_TRANSACTION_ID = 1;
		const int FILTER_BY_FORM_NUMBER = 2;
		const int FILTER_BY_STANDARD_NAME = 3;
        const int FILTER_BY_PAID_DATE = 4;
        const int FILTER_BY_CLEARANCE_DATE = 5;
		DataSet oDSOnlineTransaction = null;
		DataTable oDTOnlineTransaction = null;

		bool bChkAll = chkIncludeAll.Checked;

		if (optTransactionNumber.Checked)    
		{
			oDSOnlineTransaction = NetBankingPaymentTransactionsBL.FetchOnlineAdmissionFeeDetail(txtTransactionIDNumber.Text.Trim(), FILTER_BY_TRANSACTION_ID, miSchoolId, miAcademicYearId, bChkAll);
			oDTOnlineTransaction = oDSOnlineTransaction.Tables[0];
		}
		else if (optFormNo.Checked)
		{
			oDSOnlineTransaction = NetBankingPaymentTransactionsBL.FetchOnlineAdmissionFeeDetail(txtFormNo.Text.Trim(), FILTER_BY_FORM_NUMBER, miSchoolId, miAcademicYearId, bChkAll);
			oDTOnlineTransaction = oDSOnlineTransaction.Tables[0];
		}
		else if (optStandardName.Checked)
		{
			if (cmbStandardName.SelectedItem.ToString() == "--All--")
			{
				cmbStandardName.SelectedItem.Text = String.Empty;
				oDSOnlineTransaction = NetBankingPaymentTransactionsBL.FetchOnlineAdmissionFeeDetail(cmbStandardName.SelectedItem.ToString(), FILTER_BY_STANDARD_NAME, miSchoolId, miAcademicYearId, bChkAll);
				oDTOnlineTransaction = oDSOnlineTransaction.Tables[0];
			}
			else
			{
				oDSOnlineTransaction = NetBankingPaymentTransactionsBL.FetchOnlineAdmissionFeeDetail(cmbStandardName.SelectedItem.ToString(), FILTER_BY_STANDARD_NAME, miSchoolId, miAcademicYearId, chkIncludeAll.Checked);
				oDTOnlineTransaction = oDSOnlineTransaction.Tables[0];
			}
		}
        else if (optPaymentDate.Checked)   //add new for date filter payment date
        {
            oDSOnlineTransaction = NetBankingPaymentTransactionsBL.FetchOnlineAdmissionFeeDetails(txtPaymentStartDate.Text.Trim(), txtPaymentEndDate.Text.Trim(), null, FILTER_BY_PAID_DATE, miSchoolId, miAcademicYearId, bChkAll);
            oDTOnlineTransaction = oDSOnlineTransaction.Tables[0];
        }
        else if (optClearanceDate.Checked)  //add new for date filter clearance date
        {
            oDSOnlineTransaction = NetBankingPaymentTransactionsBL.FetchOnlineAdmissionFeeDetails(txtClearanceStartDate.Text.Trim(), txtClearanceEndDate.Text.Trim(), null, FILTER_BY_CLEARANCE_DATE, miSchoolId, miAcademicYearId, bChkAll);
            oDTOnlineTransaction = oDSOnlineTransaction.Tables[0];
        }
		if (oDTOnlineTransaction != null)
		{
			grdOnlineAdmissionFeeDetails.Visible = true;
			grdOnlineAdmissionFeeDetails.DataSource = oDTOnlineTransaction;
			grdOnlineAdmissionFeeDetails.DataBind();
			hidRowCnt.Value = Convert.ToString(grdOnlineAdmissionFeeDetails.Rows.Count);

		}
		if (Convert.ToString(oDSOnlineTransaction.Tables[1].Rows[0][0]) != string.Empty)
		{
			tblTotalAmount.Visible = true;
			int iTotalAmount = oDSOnlineTransaction.Tables[1].Rows[0][0].ToInt();
			lblTotalAmount.Text = iTotalAmount.ToString();
		}
		else
			tblTotalAmount.Visible = false;

		if (oDTOnlineTransaction.Rows.Count == 0)
			trTotalRec.Visible = false;
	}

	/// <summary>
	/// This method is used to generate XML.
	/// </summary>
	/// <returns></returns>
	private string GenerateXML()
	{
		const int I_COLUMN_INDEX_CLEARANCE_DATETIME = 7;
        const int I_COLUMN_INDEX_DEPOSITED_BANKID = 8;
		const int I_COLUMN_INDEX_TSPLTRANSACTIONID = 3;        
		const string S_ELEMENT = "element";

		string sAttribute;
		var oDoc = new XmlDocument();
		XmlElement oElement = oDoc.CreateElement("OnlineAdmissionFeeInfo");
		XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "OnlineAdmissionFeeInfo", String.Empty);
		for (int i = 0; i < grdOnlineAdmissionFeeDetails.Rows.Count; i++)
		{
			var otxtClearanceDate = grdOnlineAdmissionFeeDetails.Rows[i].Cells[I_COLUMN_INDEX_CLEARANCE_DATETIME].FindControl("txtclearance") as TextBox;
			var otxtTSPLTransactionID = grdOnlineAdmissionFeeDetails.Rows[i].Cells[I_COLUMN_INDEX_TSPLTRANSACTIONID].FindControl("txtTSPLTransactionID") as TextBox;
            var ddlDepositedBankList = grdOnlineAdmissionFeeDetails.Rows[i].Cells[I_COLUMN_INDEX_DEPOSITED_BANKID].FindControl("ddlDepositedBankList") as DropDownList;            

			XmlNode oXMLNode = oDoc.CreateNode(S_ELEMENT, "OnlineAdmissionFeeInfo", String.Empty);

			sAttribute = "NetBankingPaymentTransactionID";
			XmlAttribute oAttr = oDoc.CreateAttribute(sAttribute);
			oAttr.Value = grdOnlineAdmissionFeeDetails.DataKeys[i]["NetBankingPaymentTransactionID"].ToString();
			oXMLNode.Attributes.Append(oAttr);

			sAttribute = "TPSLTransactionID";
			oAttr = oDoc.CreateAttribute(sAttribute);
			oAttr.Value = otxtTSPLTransactionID.Text.Trim();
			oXMLNode.Attributes.Append(oAttr);

			sAttribute = "Update_Date";
			oAttr = oDoc.CreateAttribute(sAttribute);
			oAttr.Value = DateTime.Now.ToString();
			oXMLNode.Attributes.Append(oAttr);

			sAttribute = "Updated_By_Id";
			oAttr = oDoc.CreateAttribute(sAttribute);
			oAttr.Value = Session[Constants.S_SESSION_USER_ID].ToString();
			oXMLNode.Attributes.Append(oAttr);

			sAttribute = "ClearanceDate";
			oAttr = oDoc.CreateAttribute(sAttribute);
			oAttr.Value = otxtClearanceDate.Text.Trim() != String.Empty ? otxtClearanceDate.Text.Trim() : DBNull.Value.ToString();
			oXMLNode.Attributes.Append(oAttr);

            sAttribute = "DepositeBankId";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = ddlDepositedBankList.SelectedValue.ToString();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "FormNumber";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = grdOnlineAdmissionFeeDetails.DataKeys[i]["Form_Number"].ToString(); 
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "StudentAdmissionId";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = grdOnlineAdmissionFeeDetails.DataKeys[i]["StudentAdmissionId"].ToString(); 
            oXMLNode.Attributes.Append(oAttr);

			oXmlRootNode.AppendChild(oXMLNode);
		}
		oElement.AppendChild(oXmlRootNode);
		return oElement.InnerXml;
	}

    /// <summary>
    /// Records cleared payment details in the accounts module.
    /// </summary>
    /// <param name="aePaymentMode"></param>
    private void RecordPayment()
    {
        AccountVoucherClient oVoucherClient = null;
        try
        {
            oVoucherClient = new AccountVoucherClient();
            oVoucherClient.Open();
            oVoucherClient.CreateAdmissionFormVoucher(miSchoolId, miAcademicYearId, miFinancialYearId, miUserId, GenerateXML());
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), "Accounts Module : An exception occured while processing online form payments.");
        }
        finally
        {
            if (oVoucherClient.State != CommunicationState.Faulted)
                oVoucherClient.Close();
        }
    }

    /// <summary>
    /// This event is used to set filter based on Payment Date for displaying grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optPaymentDate_CheckedChanged(object sender, EventArgs e)    //add new for date  filter
    {
        try
        {
            optPaymentDateChecked();           
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set filter based on Clearance Date for displaying grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optClearanceDate_CheckedChanged(object sender, EventArgs e)  //new add for date filter
    {
        try
        {
            optClearanceDateChecked();          
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used set controls when learanceDate radio button checked.
    /// </summary>
    private void optClearanceDateChecked()         //add new for date filter
    {  
        txtPaymentStartDate.Enabled = false;
        txtPaymentEndDate.Enabled = false;
        txtClearanceStartDate.Enabled = true;
        txtClearanceEndDate.Enabled = true;
        chkIncludeAll.Checked = true;
        txtTransactionIDNumber.Enabled = false;       
    }

    /// <summary>
    /// This method is used set controls when PaymentDate radio button checked.
    /// </summary>
    private void optPaymentDateChecked()   //add new for date filter
    { 
        txtPaymentStartDate.Enabled = true;
        txtPaymentEndDate.Enabled = true;
        txtClearanceStartDate.Enabled = false;
        txtClearanceEndDate.Enabled = false;
        chkIncludeAll.Checked = false;
        txtTransactionIDNumber.Enabled = false;       
    }

	#endregion -- PRIVATE METHOD(s) --    

    public int FILTER_BY_CLEARANCE_DATE { get; set; }
}
