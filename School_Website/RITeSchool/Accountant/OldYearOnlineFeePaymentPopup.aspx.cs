using System;
using System.Web.UI.WebControls;
using BusinessLogic;
using System.Data;
using Utility;
using BusinessLogic.Exceptions;
using System.Reflection;

public partial class OldYearOnlineFeePaymentPopup : SchoolBase
{
    #region Data Member(s)
    
    private StudentFeeDetailsBL moStudentFeeDetailsBL;

    #endregion

    #region Event(s)
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moStudentFeeDetailsBL = new StudentFeeDetailsBL();
            if (!IsPostBack)
            {
                ReadQueryString();
                ValSUm.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
                Session["IsForNextYear"] = null;
                FillAcademicYears();
                FillFeeDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwFeeDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                CheckBox chkSelect = e.Item.FindControl("chkSelect") as CheckBox;
                HyperLink lnkMini = e.Item.FindControl("lnkMini") as HyperLink;
                DataRowView dv = e.Item.DataItem as DataRowView;
                if (dv["IsFirstRecord"].ToBool())
                    chkSelect.Visible = true;

                if (dv["IsPaid"].ToBool())
                {
                    chkSelect.Visible = false;
                    lnkMini.Visible = true;

                    string sQueryString = string.Format("ReceiptNo={0}&AcademicYear={1}&AccountHeaderId={2}&StudentId={3}", dv["ReceiptNumber"].ToString(), hidOldAcademicYearId.Value, 0, hidStudentId.Value);
                    string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
                    lnkMini.NavigateUrl = lnkMini.NavigateUrl + sEncrypt;
                    lnkMini.Attributes.Add("onclick", string.Format("window.open('{0}' , '_blank','scrollbars=yes,resizable=no,top=0,left=0,width=900,height=700').focus(); return false;", lnkMini.NavigateUrl));
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    protected void btnPay_Click(object sender, EventArgs e)
    {
        try
        {
            string sDueDates = String.Empty;

            string sStudFeeId = string.Empty;

            foreach (ListViewItem item in lstvwFeeDetails.Items)
            {
                CheckBox chkSelect = item.FindControl("chkSelect") as CheckBox;
                //Calculate total amount (checked checkbox)
                if (chkSelect.Checked)
                {
                    Label lblPaidDate = item.FindControl("lblPaidDate") as Label;
                    DateTime dtDueDate = lblPaidDate.Text.ToDateTime();
                    sDueDates = sDueDates + "," + dtDueDate;

                    if (miSchoolId == Constants.SchoolId.SNS.ToInt())
                        sStudFeeId = lstvwFeeDetails.DataKeys[item.DisplayIndex]["Schoolwise_Student_Fee_Id"].ToString();
                }
            }

            if (sDueDates.StartsWith(","))
                sDueDates = sDueDates.Substring(1);

            string sQueryString = string.Format("StudentId={0}&DueDates={1}&Remarks={2}&SchoolwiseStudentFeeId={3}&AcadmicYearId={4}&IsOldAcademicYearPayment={5}", hidOldStudentId.Value, sDueDates, string.Empty, sStudFeeId, hidOldAcademicYearId.Value, Constants.S_ONE);
            hidQueryString.Value = CommonUtility.EncryptQuerystring(sQueryString);                      
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void cmbAcademicYrId_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillFeeDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    } 

    #endregion

    #region Method(s)

    private void ReadQueryString()
    {
        if (QueryString["StudentId"] != null)
            hidStudentId.Value = QueryString["StudentId"].ToString();
        ;
    }

    private void FillFeeDetails()
    {
        DataSet dsDebitDetails = moStudentFeeDetailsBL.GetLastYearFeeDetails(miSchoolId, cmbAcademicYrId.SelectedValue.ToInt(), hidStudentId.Value.ToInt());

        hidOldStudentId.Value = dsDebitDetails.Tables[1].Rows[0]["OldStudentId"].ToString();
        hidOldAcademicYearId.Value = dsDebitDetails.Tables[1].Rows[0]["OldAcademicYearId"].ToString();
        lstvwFeeDetails.DataSource = dsDebitDetails.Tables[0];
        lstvwFeeDetails.DataBind();

        Session[Constants.S_SESSION_SELECTED_YEAR] = cmbAcademicYrId.SelectedValue;

        if (dsDebitDetails.Tables[0].Rows.Count == 0)
            btnPay.Visible = false;
        else
            btnPay.Visible = true;
    }

    private void FillAcademicYears()
    {
        DataSet oDSYearsInfo = SchoolWiseAcademicYearMasterBL.GetPendingFeeAcademicYears(miSchoolId, hidStudentId.Value.ToInt(), miAcademicYearId, false, false);
        DataTable oDtYearInfo = oDSYearsInfo.Tables[0];
        if (oDtYearInfo != null && oDtYearInfo.Rows.Count > 0 && oDtYearInfo.Rows[0][0] != DBNull.Value)
        {
            cmbAcademicYrId.Bind(oDtYearInfo, "Academic_Year_Id", "AcademicYear", Constants.S_SELECT);
            if (cmbAcademicYrId.Items.Count == 1)
                cmbAcademicYrId.Enabled = false;
            else
            {
                if (Session[Constants.S_SESSION_SELECTED_YEAR] != null && Session[Constants.S_SESSION_SELECTED_YEAR].ToString() != string.Empty && Session[Constants.S_SESSION_SELECTED_YEAR].ToString() != Constants.S_ZERO &&
                    Session[Constants.S_SESSION_DO_REFRESH_PAGE] != null && Session[Constants.S_SESSION_DO_REFRESH_PAGE].ToString() == Constants.S_ONE)
                {
                    int iAcademicYEar = Session[Constants.S_SESSION_SELECTED_YEAR].ToInt();
                    ListItem oListItem = cmbAcademicYrId.Items.FindByValue(iAcademicYEar.ToString());

                    if (oListItem != null)
                        oListItem.Selected = true;
                    else
                        cmbAcademicYrId.SelectedValue = oDtYearInfo.Rows[0]["Academic_Year_Id"].ToString();
                }
                else
                    cmbAcademicYrId.SelectedValue = oDtYearInfo.Rows[0]["Academic_Year_Id"].ToString();
            }
        }
        else
            cmbAcademicYrId.Items.Add(new ListItem { Text = Constants.S_SELECT, Value = Constants.S_ZERO });

        Session[Constants.S_SESSION_SELECTED_YEAR] = null;
        Session[Constants.S_SESSION_DO_REFRESH_PAGE] = null;
    } 

    #endregion
}