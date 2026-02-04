using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using Utility;
using FeeEntities;
using BusinessLogic.Exceptions;
using System.Reflection;

public partial class CustomizeInternalRecieptPopUp : SchoolBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                ReadQuerystring();
                FillPayableList();
                SetJavaScriptAttributes();
                cal_PaymentDate.DateValue = DateTime.Today;
                hidServerDate.Value = Convert.ToString(DateTime.Today);
                RefreshValues();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnPrint_OnClick(object sender, EventArgs e)
    {
        try
        {
            string sInternalFeeDetailsIds = string.Empty;
            foreach (ListItem oInternalFee in chklstPayableFor.Items)
                sInternalFeeDetailsIds += oInternalFee.Selected ? oInternalFee.Value + "," : string.Empty;
            InternalFeeDetailsBL oInternalFeeDetailsBL = new InternalFeeDetailsBL()
            {
                AcademicYearId = miAcademicYearId,
                SchoolId = miSchoolId,
                PaidDate = txtDate.Text.ToDateTime(),
                Remark = txtRemark.Text,
                Schoolwise_Student_Id = hidStudentId.Value.ToInt(),
                InternalFeeDetailsIds = sInternalFeeDetailsIds,
                ShowConsolidatedPartialPayments = chkConslidatedPayableFor.Checked,
                InsertedById = miUserId
            };

            int iDuplicateInternalFeeDetailsId = oInternalFeeDetailsBL.InsertDuplicateInternalFeeReceiptDetails().Rows[0][0].ToInt();
            string sQueryString = string.Format("StudentId={0}&InternalFeeDetailsId={1}&AcademicYear={2}&RegNo={3}&FromDate={4}&ToDate={5}&IncludePaid={6}&PayForNextYear={7}&IsRegNoFilter={8}&StandardID={9}&DivisionID={10}&FeeTypeID={11}&pIndex={12}&DuplicateInternalFeeDetailsId={13}",
                                                     hidStudentId.Value,
                                                     Constants.I_ZERO,
                                                     hidNextAcademicYearId.Value,
                                                     hidRegNo.Value,
                                                     hidFromDate.Value,
                                                     hidToDate.Value,
                                                     hidIncludePaid.Value,
                                                     hidPayForNextYear.Value,
                                                     hidIsRegNoFilter.Value,
                                                     hidStandardID.Value,
                                                     hidDivisionID.Value,
                                                     hidFeeTypeID.Value,
                                                     hidPageIndex.Value,
                                                     iDuplicateInternalFeeDetailsId);
            sQueryString = CommonUtility.EncryptQuerystring(sQueryString);
            Response.Write("<Script language='javascript'>window.open('../Accountant/InternalFeePaymentReceipt.aspx?" + sQueryString + "','_self','left=0, top=0, height=450, width=670, resizable= no, scrollbars= yes')</Script>");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnCancel_OnClick(object sender, EventArgs e)
    {
        try
        {
            Response.Write("<Script language='Javascript'>window.close();</Script>");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    private void FillPayableList()
    {
        List<InternalFeeDetails> olstInternalFeeDetails = InternalFeeDetailsBL.GetInternalFeeDetailsForPayment(miSchoolId, miAcademicYearId, hidStudentId.Value.ToInt());
        List<InternalFeeDetails> olstSelectedInternalFeeDetails = olstInternalFeeDetails.Where(InternalFee => InternalFee.IsSelected).ToList<InternalFeeDetails>();
        InternalFeeDetails oInternalFeeDetails = olstSelectedInternalFeeDetails[0];
        chkSelectAll.Checked = olstInternalFeeDetails.Count == olstSelectedInternalFeeDetails.Count;
        int iAmount = 0;
        olstSelectedInternalFeeDetails.ForEach(
                oInternalFee =>
                {
                    iAmount += oInternalFee.Amount;
                }
            );
        lblPaybleAmount.Text = iAmount.ToString();

        olstInternalFeeDetails.ForEach(
            oInternalFee =>
            {
                ListItem oListItem = new ListItem(oInternalFee.PayableForDisplayText, oInternalFee.InternalFeeDetailsId.ToString());
                oListItem.Selected = oInternalFee.IsSelected;
                chklstPayableFor.Items.Add(oListItem);
                oListItem.Attributes.Add("onclick", "SetTotal(this.checked, " + oInternalFee.Amount + ")");
            }
        );
    }

    private void SetJavaScriptAttributes()
    {
        txtDate.Focus();
        ApplyMouseHoverEffect(new List<Button> { btnCancel, btnPrint });
    }

    /// <summary>
    /// This method is used to decrypt query string.
    /// </summary>
    private void ReadQuerystring()
    {
        if (Request.QueryString.ToString() == Constants.S_EMPTY_STRING)
            return;

        if (!QueryString["StudentId"].IsNull())
            hidStudentId.Value = QueryString["StudentId"];

        if (!QueryString["NextAcademicYearId"].IsNull())
            hidNextAcademicYearId.Value = QueryString["NextAcademicYearId"];

        if (!QueryString["StudentName"].IsNull())
        {
            hidStudentName.Value = QueryString["StudentName"];
            lblStudentHeading.Text = hidStudentName.Value;
        }

        if (!QueryString["RegNo"].IsNull())
            hidRegNo.Value = QueryString["RegNo"];

        if (!QueryString["FromDate"].IsNull())
            hidFromDate.Value = QueryString["FromDate"];

        if (!QueryString["ToDate"].IsNull())
            hidToDate.Value = QueryString["ToDate"];

        if (!QueryString["IncludePaid"].IsNull())
            hidIncludePaid.Value = QueryString["IncludePaid"];

        if (!QueryString["PayForNextYear"].IsNull())
            hidPayForNextYear.Value = QueryString["PayForNextYear"];

        if (!QueryString["IsRegNoFilter"].IsNull())
            hidIsRegNoFilter.Value = QueryString["IsRegNoFilter"];

        if (!QueryString["StandardID"].IsNull())
            hidStandardID.Value = QueryString["StandardID"];

        if (!QueryString["DivisionID"].IsNull())
            hidDivisionID.Value = QueryString["DivisionID"];

        if (!QueryString["FeeTypeID"].IsNull())
            hidFeeTypeID.Value = QueryString["FeeTypeID"];

        if (!QueryString["InternalFeeDetailsId"].IsNull())
            hidInternalFeeDetailsId.Value = QueryString["InternalFeeDetailsId"];

        if (!QueryString["pIndex"].IsNull())
            hidPageIndex.Value = QueryString["pIndex"];
    }

    /// <summary>
    /// This Method used to change value of messgae according to culture
    /// </summary>
    private void RefreshValues()
    {
        hidPaymentDateShouldNotFutureDate.Value = Resources.LocalizedResources.PaymentDateShouldNotFutureDate;
        hidAtLeastPayableForShouldBeSelected.Value = Resources.LocalizedResources.AtLeastPayableForShouldBeSelected;
    }
}