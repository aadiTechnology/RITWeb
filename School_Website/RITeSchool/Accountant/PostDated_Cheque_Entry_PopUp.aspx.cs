// File Name   : BankDetailsPopup.aspx.cs
// Created By  : -
// Date        : -
// Modified By : Milind
// Date        : 11 Sept 09
// Description : This class is used to add the post dated cheque entry for the selected student.  

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class PostDated_Cheque_Entry_PopUp : SchoolBase
{
	#region -- CONSTANT(s) --

	private const string S_BUTTON_SEARCH_CPTION = "Show";
	private const string S_BUTTON_CHANGE_CPTION = "Change student";
	private const string S_CMD_NAME_DELETE_CHEQUE = "Delete_Cheque";
	private const string S_CMD_NAME_EDIT_CHEQUE = "Edit_Cheque";
	private string S_CMD_NAME_UPDATE = Resources.LocalizedResources.Update;
	private string S_CMD_NAME_SAVE = Resources.LocalizedResources.Save;

	private const int I_COLUMN_INDEX_DELETE = 6;
	private const int I_COLUMN_INDEX_EDIT = 5;
	private const int I_COLUMN_INDEX_CHEQUE_DATE = 1;

	#endregion -- CONSTANT(s) --

	#region -- EVENT HANLDER(s) --

	/// <summary>
	/// This event is used to set default values.
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
				valChequeData.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
				SetSortingFieldDefaultValues();
				ReadQueryString();
				FillBankCombo();
                
                RefreshValue();
				btnCancel.Attributes["onclick"] = "HideValSummary()";
				ApplyMouseHoverEffect(new List<Button> { btnCancel, btnSave, btnClose });
			}
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValue();
            }
			SetAcademicYearDates();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to save postdated cheque details.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
			string S_ERR_MSG = Resources.LocalizedResources.ValCheckNameExists;
			StudentPostDatedChequesBL oStudentChequeDetails = PopulateChequeDetails();
			int iStudentId = oStudentChequeDetails.Student_Id;
			string sChequeNo = oStudentChequeDetails.Cheque_Number;

			oStudentChequeDetails.PostDated_Cheque_Id = hidPostdatedChequeId.Value.ToInt();
			if (!oStudentChequeDetails.IsChequeNoDuplicate(sChequeNo, iStudentId))
			{
				if (hidMode.Value == "New")
				{
					oStudentChequeDetails.InsertStudentPostDatedCheques();
					hidPostdatedChequeId.Value = "0";
				}
				else
				{
					oStudentChequeDetails.Updated_By_Id = miUserId;
					oStudentChequeDetails.UpdateStudentPostDatedCheques();
					hidMode.Value = "New";
					hidPostdatedChequeId.Value = "0";
				}
				ClearAllControls();
				FillStudentChequeGrid();
				btnSave.Text = Resources.LocalizedResources.Save;
			}
			else
			{
				lblErrMsg.Visible = true;
				lblErrMsg.Text = S_ERR_MSG;
				txtChequeNumber.Focus();
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This method is used to reset all controls and to cancel current transaction.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnCancel_Click(object sender, EventArgs e)
	{
		try
		{
			ClearAllControls();
			hidMode.Value = "New";
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to to close pop up window.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnClose_Click(object sender, EventArgs e)
	{
		try
		{
			SetQueryString();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to set edit or delete buttons depending on fee paid status.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void grdPostDatedCheque_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		try
		{
			int iRowIndex = e.Row.RowIndex;
			if (iRowIndex >= 0)
			{
				string sIsChequeBounce = grdPostDatedCheque.DataKeys[iRowIndex]["Is_Cheque_Bounce"].ToString();
				string sStatus = grdPostDatedCheque.DataKeys[iRowIndex][Constants.I_ONE].ToString();
				var imgEdit = e.Row.Cells[I_COLUMN_INDEX_EDIT].Controls[Constants.I_ZERO] as ImageButton;
				var imgDelete = e.Row.Cells[I_COLUMN_INDEX_DELETE].Controls[Constants.I_ZERO] as ImageButton;
				imgDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
				
				if (sStatus == "Paid" || sIsChequeBounce == Constants.C_YES.ToString())
				{
					imgEdit.Visible = false;
					imgDelete.Visible = false;
				}
				
				if (sIsChequeBounce == Constants.C_YES.ToString())
				{
					e.Row.ForeColor = System.Drawing.Color.Red;
					e.Row.ToolTip = Resources.LocalizedResources.ChequeIsBounced;
				}
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to modify or to update records.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void grdPostDatedCheque_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		try
		{
			if (e.CommandName.ToUpper() != "SORT")
			{
				int iRowindex = e.CommandArgument.ToInt();
				int iPostDatedChequeId = grdPostDatedCheque.DataKeys[iRowindex].Value.ToInt();
				switch (e.CommandName)
				{
					case S_CMD_NAME_EDIT_CHEQUE:
						SetChequeDetails(iPostDatedChequeId);
						FillStudentChequeGrid();
						ShowHideControls(true);
						break;
					case S_CMD_NAME_DELETE_CHEQUE:
						StudentPostDatedChequesBL.DeleteChequeDetails(iPostDatedChequeId);
						ClearAllControls();
						hidMode.Value = "New";
						FillStudentChequeGrid();
						break;
				}
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used for sorting. 
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void grdPostDatedCheque_Sorting(object sender, GridViewSortEventArgs e)
	{
		try
		{
			hidSortExpression.Value = e.SortExpression;
			hidSortDirection.Value = hidSortDirection.Value == Constants.S_DESCENDING ? Constants.S_ASCENDING : Constants.S_DESCENDING;
			FillStudentChequeGrid();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to set sorting iamge.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void grdPostDatedCheque_RowCreated(object sender, GridViewRowEventArgs e)
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
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	#endregion -- EVENT HANLDER(s) --

	#region -- PRIVATE METHOD(s) --

	/// <summary>
	/// This method is used to set hidden field to default value for grid sorting.
	/// </summary>
	private void SetSortingFieldDefaultValues()
	{
		hidSortExpression.Value = grdPostDatedCheque.Columns[Constants.I_ONE].SortExpression;
		hidSortDirection.Value = Constants.S_ASCENDING;
	}

	/// <summary>
	/// This method initialises hidden fields with the start and end date of selected academic year.
	/// </summary>
	private void SetAcademicYearDates()
	{
		hidYearStartDate.Value = Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE].ToString();
		hidYearEndDate.Value = Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE].ToString();
		hidServerDate.Value = Convert.ToString(DateTime.Today);
	}

	/// <summary>
	/// This method is used to show or hide controls.
	/// </summary>
	/// <param name="bFlag"></param>
	private void ShowHideControls(bool bFlag)
	{
		trChequeEntry.Visible = bFlag;
		trGrdCheque.Visible = bFlag;
	}

	/// <summary>
	///This method is used to fill postdated cheque details grid.
	/// </summary>
	private void FillStudentChequeGrid()
	{
		int iStudentId = hidStudentId.Value.ToInt();
		var oChequeDetails = new StudentPostDatedChequesBL();
		DataSet dsChequeDetails = oChequeDetails.GetStudentChequeDetails(iStudentId);

		dsChequeDetails.Tables[Constants.I_ZERO].DefaultView.Sort = hidSortExpression.Value + " " + hidSortDirection.Value;
		SetGridViewDateColumnProperties();
		grdPostDatedCheque.DataSource = dsChequeDetails.Tables[Constants.I_ZERO].DefaultView;

		grdPostDatedCheque.DataBind();
	}

	/// <summary>
	/// This method is used to reset all controls.
	/// </summary>
	private void ClearAllControls()
	{
		txtChequeNumber.Text = String.Empty;
		txtChequeDate.Text = String.Empty;
		txtRemarks.Text = String.Empty;
		txtChequeAmt.Text = String.Empty;
		lblErrMsg.Visible = false;
		lblErrMsg.Text = String.Empty;
		btnSave.Text = Resources.LocalizedResources.Save;
		ddlBankName.SelectedIndex = 0;
	}

	/// <summary>
	/// This method is used to populate StudentPostDatedChequesBL and returns object of same.
	/// </summary>
	/// <returns></returns>
	private StudentPostDatedChequesBL PopulateChequeDetails()
	{
		return new StudentPostDatedChequesBL
				{
					SchoolId		= miSchoolId,
					AcademicYrId	= miAcademicYearId,
					Bank_Id			= ddlBankName.SelectedValue.ToInt(),
					Cheque_Amount	= txtChequeAmt.Text.ToInt(),
					Cheque_Date		= cal_ChequeDate.DateValue,
					Cheque_Number	= txtChequeNumber.Text,
					Inserted_By_id	= miUserId,
					Remarks			= txtRemarks.Text,
					Student_Id		= hidStudentId.Value.ToInt()

				};
	}

	/// <summary>
	/// This method is used to set values to conrtrols in edit mode.
	/// </summary>
	/// <param name="aiChequeId"></param>
	private void SetChequeDetails(int aiChequeId)
	{
		var oPostdatedChequeBL = new StudentPostDatedChequesBL(aiChequeId);
		txtChequeNumber.Text = oPostdatedChequeBL.Cheque_Number;
		txtRemarks.Text = oPostdatedChequeBL.Remarks;
		cal_ChequeDate.DateValue = oPostdatedChequeBL.Cheque_Date.ToDateTime();
		txtChequeAmt.Text = oPostdatedChequeBL.Cheque_Amount.ToString();
		hidMode.Value = "Edit";
		hidPostdatedChequeId.Value = aiChequeId.ToString();
		btnSave.Text = Resources.LocalizedResources.Update;
		ddlBankName.SelectedValue = oPostdatedChequeBL.Bank_Id.ToString();
	}

	/// <summary>
	/// This function sets the date format for date column property 
	/// </summary>
	/// 
	private void SetGridViewDateColumnProperties()
	{
		var oReceivedDate = grdPostDatedCheque.Columns[I_COLUMN_INDEX_CHEQUE_DATE] as BoundField;
		oReceivedDate.HtmlEncode = false;
		oReceivedDate.DataFormatString = Constants.S_STANDARD_GRID_DATE_FORMAT;
	}

	/// <summary>
	/// This method is used to read querystring.
	/// </summary>
	private void ReadQueryString()
	{
		if (!QueryString["StudentId"].IsNull())
		{
			hidStudentId.Value = QueryString["StudentId"];
			FillStudentChequeGrid();
			ClearAllControls();
			ShowHideControls(true);
		}
	}

	/// <summary>
	/// This method is used to create query string and redirect to base screen.
	/// </summary>
	private void SetQueryString()
	{
		string sQueryString = "StudentId=" + hidStudentId.Value;
		string sEncryptQueryString = CommonUtility.EncryptQuerystring(sQueryString);
		sQueryString = "'?" + sEncryptQueryString + "'";
		Response.Write("<Script language='Javascript'>window.opener.location=window.opener.location.pathname+" + sQueryString + ";window.opener.focus(); ");
		Response.Write("window.close();");
		Response.Write("</script>");
	}

	/// <summary>
	/// This method is used to fill combobox with bank list.
	/// </summary>
	private void FillBankCombo()
	{
		var oSchoolwiseBankMasterBL = new SchoolwiseBankMasterBL();
		DataTable dtBankList = oSchoolwiseBankMasterBL.GetSchoolwiseBankList(miSchoolId);
		ddlBankName.Bind(dtBankList, "Schoolwise_Bank_Id", "Bank_Name", Constants.S_SELECT);
	}
 
    /// <summary>
    /// This method used to value based on Culture
    /// </summary>
    private void RefreshValue()
    {
        hidChequeAmountShouldNotBeBlank.Value = Resources.LocalizedResources.ChequeAmountShouldNotBeBlank;
        hidChequeAmountShouldBeGreaterThanZero.Value = Resources.LocalizedResources.ChequeAmountShouldBeGreaterThanZero;
        hidChequeNumberShouldNotBeBlank.Value = Resources.LocalizedResources.ChequeNumberShouldNotBeBlank;
        hidBankNameShouldBeSelected.Value = Resources.LocalizedResources.BankNameShouldBeSelected;
        hidChequeDateShouldNotBeBlank.Value = Resources.LocalizedResources.ChequeDateShouldNotBeBlank;
        hidValChequeDate.Value = Resources.LocalizedResources.ValChequeDate;
        hidAnd.Value = Resources.LocalizedResources.And;
        hidValDeleteChequeDetails.Value = Resources.LocalizedResources.ValDeleteChequeDetails;
        btnSave.Text = Resources.LocalizedResources.Save;

    }
	#endregion -- PRIVATE METHOD(s) --
}
