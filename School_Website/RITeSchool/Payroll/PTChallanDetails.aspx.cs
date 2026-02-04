/*
 * File Name - PTChallanDetails.aspx.cs
 * Created Date - 
 * Created By -
 * Description - This class is used to configure P.T. chalan details.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using Utility;

public partial class PTChallanDetails : SchoolBase
{
    #region Constant

    private const string S_DEFAULT_SORT_EXP = "Year";
    private const string S_SAVE_MESSAGE = "P.T. Details Saved Successfully!!";
    private const string S_CHALLAN_EXIST_MESSAGE = "Professional Tax Challan Details for this month already exists.";
    private const string S_CHEQUENO_EXIST_MESSAGE = "This Cheque No. for Professional Tax Challan  Details already exists.";
    private const string S_CINNO_EXIST_MESSAGE = "This CIN No. for Professional Tax Challan  Details already exists.";
    private const string S_NEW_MODE = "NEW";
    private const string S_EDIT_MODE = "EDIT";

    #endregion

    #region Date Member(s)

    private MonthwiseProfessionalTaxDetailsBL moMonthwiseProfessionalTaxDetailsBL; 

    #endregion

    #region Event(s)

    /// <summary>
    /// Page load event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moMonthwiseProfessionalTaxDetailsBL = new MonthwiseProfessionalTaxDetailsBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                if (CheckPrcondition())
                {
                    SetDefaultValues();
                    SetJavaScriptAttributes();
                    FillBankNameMonthYearCombo();
                    DisableControls(false);
                    divErr.Visible = false;
                }
                else
                {
                    DisableControls(true);
                    divErr.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///  This event is used to bound data to list view
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwPTChallanDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                DataRowView oDataRowView = oCurrentItem.DataItem as DataRowView;
                int iListIndex = oCurrentItem.DisplayIndex;
                int iMonthId = Convert.ToInt32(lstvwPTChallanDetails.DataKeys[iListIndex]["MonthId"]);
                int iYear = Convert.ToInt32(lstvwPTChallanDetails.DataKeys[iListIndex]["Year"]);
                
                //ImageButton oEdit = (ImageButton)oCurrentItem.FindControl("imgBtnEdit");
                //oEdit.Visible = false;
                //if ((oDataRowView["ChequeNo"].ToString().TrimAll() == string.Empty && oDataRowView["CINNO"].ToString().TrimAll() == string.Empty) || !moMonthwiseProfessionalTaxDetailsBL.IsSalaryPaid(iMonthId, iYear))
                //{
                //    oEdit.Visible = true;
                //}
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill footer property of existing Staff Members list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwPTChallanDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwPTChallanDetails.Items.Count > 0)
            {
                ControlUtility.FillListViewPagerFooter(lstvwPTChallanDetails, DtPgCount);
                AddSortImage();
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
    /// This event is used to edit and update the staff details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwPTChallanDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == Constants.S_COMMAND_UPDATE)
            {
                ListViewDataItem ocurrentItem = e.Item as ListViewDataItem;
                int iListIndex = ocurrentItem.DisplayIndex;
                int iMonthwiseProfessionalTaxDetailsId = Convert.ToInt32(lstvwPTChallanDetails.DataKeys[iListIndex]["MonthwiseProfessionalTaxDetailsId"]);
                FillControlsForUpdate(iMonthwiseProfessionalTaxDetailsId);
                FillPTChallanDetailsList();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to sort the list view of staff members items by Name,Designation and Mobile No..
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwPTChallanDetails_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            SetSortVariables();
            hidSortExpression.Value = e.SortExpression;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Save Challan details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            SaveDetails();
            if (QueryString["Is_Configured"] != null && QueryString["Is_Configured"] != Constants.S_YES)
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.PTChallanDetails));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Cancel to Clear Fields
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ClearFields();
            FillPTChallanDetailsList();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to change list view page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwPTChallanDetails);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    } 

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to set values for controls.
    /// </summary>
    /// <param name="iMonthwiseProfessionalTaxDetailsId"></param>
    /// <param name="iSchoolID"></param>
    private void FillControlsForUpdate(int aiMonthwiseProfessionalTaxDetailsId)
    {
        MonthwiseProfessionalTaxDetails oMonthwiseProfessionalTaxDetails = moMonthwiseProfessionalTaxDetailsBL.Get(aiMonthwiseProfessionalTaxDetailsId);        
        txtChequeNo.Text = Convert.ToString(oMonthwiseProfessionalTaxDetails.ChequeNo).Trim();
        txtCINNo.Text = oMonthwiseProfessionalTaxDetails.CINNo;
        txtPTRegCertificateNo.Text = Convert.ToString(oMonthwiseProfessionalTaxDetails.PTRegCertificateNo).Trim();
        ddlBankName.SelectedValue = Convert.ToString(oMonthwiseProfessionalTaxDetails.BankId);
        ddlMonth.SelectedValue = Convert.ToString(oMonthwiseProfessionalTaxDetails.MonthId);
        ddlYear.SelectedValue = Convert.ToString(oMonthwiseProfessionalTaxDetails.Year);
        hidMonthwiseProfessionalTaxDetailsId.Value = oMonthwiseProfessionalTaxDetails.MonthwiseProfessionalTaxDetailsId.ToString();
        hidMode.Value = S_EDIT_MODE;
        hidPTRegCertificateId.Value = oMonthwiseProfessionalTaxDetails.PTRegCertificateId.ToString();
    }

    /// <summary>
    /// Fill challan details
    /// </summary>
    private void FillPTChallanDetailsList()
    {
        lstvwPTChallanDetails.DataSourceID = ObjDSPTChallanDetails.ID;
        lstvwPTChallanDetails.DataBind();
    }

    /// <summary>
    /// This method is used to disable controls according to condition.
    /// </summary>
    /// <param name="bAction"></param>
    private void DisableControls(bool abAction)
    {
        tblPTDetails.Visible = !abAction;
        tbllstPTChallanDetails.Visible = !abAction;
    }

    /// <summary>
    /// Checks Precondition
    /// </summary>
    /// <returns></returns>
    private bool CheckPrcondition()
    {
        return moMonthwiseProfessionalTaxDetailsBL.CheckPrecondition();
    }

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        hidSortExpression.Value = S_DEFAULT_SORT_EXP;
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        hidSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        btnSave.Attributes["onclick"] = "ResetUpdateLbl()";
        ApplyMouseHoverEffect(new List<Button> { btnSave, BtnCancel, btnBack });
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Payroll_Related));
    }

    /// <summary>
    /// This method is used to fill bank, month and year combo box.
    /// </summary>
    private void FillBankNameMonthYearCombo()
    {
        const int I_STAFF_GROUPS = 0;
        const int I_MONTHS = 1;
        const int I_YEAR = 2;
        const int I_CURRENT_MONTH = 4;
        const int I_MONTH = 12;

        DataSet oDSBasicDetails = moMonthwiseProfessionalTaxDetailsBL.GetBankNameMonthYear();
        ControlUtility.FillDropDownList(oDSBasicDetails.Tables[I_STAFF_GROUPS], ref ddlYear, "Value_Member", "Display_Member", Constants.S_SELECT);
        ControlUtility.FillDropDownList(oDSBasicDetails.Tables[I_MONTHS], ref ddlBankName, "Schoolwise_Bank_Id", "Bank_Name", Constants.S_SELECT);
        ControlUtility.FillDropDownList(oDSBasicDetails.Tables[I_YEAR], ref ddlMonth, "MonthID", "Month", Constants.S_SELECT);
        txtPTRegCertificateNo.Text = Convert.ToString(oDSBasicDetails.Tables[3].Rows[0][0]);

        if (oDSBasicDetails.Tables[I_CURRENT_MONTH].Rows.Count > 0)
        {
            int iPublishMonth = Convert.ToInt32(oDSBasicDetails.Tables[I_CURRENT_MONTH].Rows[0][0].ToString());
            bool iIsPublish = Convert.ToBoolean(oDSBasicDetails.Tables[I_CURRENT_MONTH].Rows[0][2].ToString());
            if (iIsPublish == true)
            {
                if (iPublishMonth == I_MONTH)
                {
                    hidCurrentMonth.Value = Constants.I_ONE.ToString();
                    hidCurrentYear.Value = (Convert.ToInt32(oDSBasicDetails.Tables[I_CURRENT_MONTH].Rows[0][1]) + 1).ToString();
                }
                else
                {
                    hidCurrentMonth.Value = (iPublishMonth + 1).ToString();
                    hidCurrentYear.Value = oDSBasicDetails.Tables[I_CURRENT_MONTH].Rows[0][1].ToString();
                }
            }
            else
            {
                hidCurrentMonth.Value = oDSBasicDetails.Tables[I_CURRENT_MONTH].Rows[0][0].ToString();
                hidCurrentYear.Value = oDSBasicDetails.Tables[I_CURRENT_MONTH].Rows[0][1].ToString();
            }
            ddlMonth.SelectedValue = hidCurrentMonth.Value;
            ddlYear.SelectedValue = hidCurrentYear.Value;
        }
        else
        {
            ddlMonth.SelectedValue = DateTime.Now.Month.ToString();
            ddlYear.SelectedValue = DateTime.Now.Year.ToString();
            hidCurrentMonth.Value = DateTime.Now.Month.ToString();
            hidCurrentYear.Value = DateTime.Now.Year.ToString();
        }
    }

    /// <summary>
    ///  Set Sort Variables
    /// </summary>
    private void SetSortVariables()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to set sorting image to list view headers.
    /// </summary>
    private void AddSortImage()
    {
        if (lstvwPTChallanDetails.SortExpression != string.Empty)
            hidSortExpression.Value = lstvwPTChallanDetails.SortExpression.ToString();
        else
            hidSortExpression.Value = S_DEFAULT_SORT_EXP;

        HtmlTableRow oHtmlTableHeaderRow = lstvwPTChallanDetails.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    /// <summary>
    /// Save Challan details
    /// </summary>
    private void SaveDetails()
    {
        moMonthwiseProfessionalTaxDetailsBL.MonthwiseProfessionalTaxDetails = PopulateBL();       
        if (hidMode.Value != S_EDIT_MODE)
        {
            if (!moMonthwiseProfessionalTaxDetailsBL.IsCINNoDuplicate())
            {
                if (!moMonthwiseProfessionalTaxDetailsBL.IsDuplicate())
                {
                    if (moMonthwiseProfessionalTaxDetailsBL.Insert())
                    {
                        lblUpdateSucess.Visible = true;
                        lblUpdateSucess.Text = S_SAVE_MESSAGE;
                        FillPTChallanDetailsList();
                        ClearFields();
                        hidMode.Value = S_NEW_MODE;
                    }
                    else
                        lblErrorMsg.Text = S_CHALLAN_EXIST_MESSAGE;
                }
                else
                    lblErrorMsg.Text = S_CHEQUENO_EXIST_MESSAGE;
            }
            else
                lblErrorMsg.Text = S_CINNO_EXIST_MESSAGE;
        }
        else
        {
            moMonthwiseProfessionalTaxDetailsBL.MonthwiseProfessionalTaxDetails.MonthwiseProfessionalTaxDetailsId = Convert.ToInt32(hidMonthwiseProfessionalTaxDetailsId.Value);
            if (!moMonthwiseProfessionalTaxDetailsBL.IsCINNoDuplicate())
            {
                if (!moMonthwiseProfessionalTaxDetailsBL.IsDuplicate())
                {
                    if (moMonthwiseProfessionalTaxDetailsBL.Update())
                    {
                        lblUpdateSucess.Visible = true;
                        lblUpdateSucess.Text = S_SAVE_MESSAGE;
                        FillPTChallanDetailsList();
                        ClearFields();
                        hidMode.Value = S_NEW_MODE;
                    }
                    else
                        lblErrorMsg.Text = S_CHALLAN_EXIST_MESSAGE;
                }
                else
                    lblErrorMsg.Text = S_CHEQUENO_EXIST_MESSAGE;
            }
            else
                lblErrorMsg.Text = S_CINNO_EXIST_MESSAGE;
        }

    }

    /// <summary>
    /// Clears the Fields
    /// </summary>
    private void ClearFields()
    {
        ddlBankName.ClearSelection();
        txtChequeNo.Text = string.Empty;
        txtCINNo.Text = string.Empty;
        ddlYear.SelectedValue = hidCurrentYear.Value;
        ddlMonth.SelectedValue = hidCurrentMonth.Value;
        hidMode.Value = S_NEW_MODE;
    }

    /// <summary>
    /// This method is used to populate P.T. object.
    /// </summary>
    /// <returns></returns>
    private MonthwiseProfessionalTaxDetails PopulateBL()
    {
        MonthwiseProfessionalTaxDetails oMonthwiseProfessionalTaxDetails = new MonthwiseProfessionalTaxDetails
        {        
            BankId = Convert.ToInt32(ddlBankName.SelectedValue),
            ChequeNo = Convert.ToString(txtChequeNo.Text.Trim()),
            CINNo=txtCINNo.Text,
            PTRegCertificateId = Convert.ToInt32(hidPTRegCertificateId.Value == string.Empty ? Constants.S_ZERO : hidPTRegCertificateId.Value),
            PTRegCertificateNo = Convert.ToString(txtPTRegCertificateNo.Text.Trim()),
            Year = Convert.ToInt32(ddlYear.SelectedValue),
            MonthId = Convert.ToInt32(ddlMonth.SelectedValue),
            InsertedById = miUserId
            
    
        };
        return oMonthwiseProfessionalTaxDetails;
    }

    #endregion
  
}
