using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SchoolEntities;
using BusinessLogic.Exceptions;
using System.Reflection;
using Utility;
using SchoolEntities.Transport;
using SchoolEntities.StudentFee;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Xml;
/// <summary>
/// This class is used to pay/delete/refund transport fees for selected user.
/// </summary>
public partial class PayTransportChargesPopUp : SchoolBase
{
    #region -- CONSTANT(s) --        
    
    private const string S_PAY_SUCCESS_MESSAGE = "Transport charges paid successfully!!!";
    private const string S_DELETE_SUCCESS_MESSAGE = "Transport charges deleted successfully!!!";
    private const string S_REFUND_SUCCESS_MESSAGE = "Transport charges refunded successfully!!!";

    #endregion -- CONSTANT(s) --

    #region -- DATA MEMBER(s) --
    
    private TransportChargesBL moTransportChargesBL;

    #endregion -- DATA MEMBER(s) --

    #region -- EVENT HANDLER(s) --

    /// <summary>
    /// This is the page load event. We will use it to fill all the default details for selected user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moTransportChargesBL = new TransportChargesBL(miSchoolId, miAcademicYearId, miUserId);

            if (!IsPostBack)
            {
                ReadQuerystring();
                SetDefaultValues();
                SetJavaScriptAttributes();
                DisplayFeeDetails();
                SetMode();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Here we set the attributes to the different controls. Also we set the visibility of controls depend on the date comes on selected row.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTransportFee_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
            int iRowId = oCurrentItem.DisplayIndex;           

            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                CheckBox chkSelect = oCurrentItem.FindControl("chkSelect") as CheckBox;                
                ImageButton imgDelete = oCurrentItem.FindControl("imgDelete") as ImageButton;
                bool bIsRefund = lstvwTransportFee.DataKeys[iRowId]["IsRefund"].ToBool();
                bool bIsLastCredit = lstvwTransportFee.DataKeys[iRowId]["IsLastCredit"].ToBool();
                var oStudentPaidFeeDetails = lstvwTransportFee.DataKeys[iRowId]["oStudentPaidFeeDetails"] as StudentPaidFeeDetails;
                var oStudentPayFeeDetails = lstvwTransportFee.DataKeys[iRowId]["oStudentPayFeeDetails"] as StudentPayFeeDetails;
                
                if (oStudentPaidFeeDetails.DebitOrCredit.Trim() != "Debit")
                {
                    chkSelect.Visible = false;
                    string sRecieptQueryString = String.Format("UserIdId={0}&AcademicYear={1}&ReceiptNo={2}", hidUserId.Value, miAcademicYearId, oStudentPayFeeDetails.ReceiptNumberOutput);                    
                }
                else
                    imgDelete.Visible = false;

                if (bIsLastCredit || bIsRefund)
                {
                    imgDelete.Visible = true;
                    imgDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
                }
                else
                    imgDelete.Visible = false;

                chkSelect.Attributes.Add("onclick", "CheckSelected(this,'" + iRowId + "')");

                SetLateFeeStyle(oCurrentItem);
                SetRefundStyle(oCurrentItem);                
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }    

    /// <summary>
    /// This event is used to set late fee details on change of payment date.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtPaymentDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            FillLateFeeDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set late fee details on change of payment date.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cal_PaymentDate_SelectionChanged(object sender, EventArgs e)
    {
        try
        {
            FillLateFeeDetails();            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle the deletion of a particular payment for selected user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTransportFee_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                string sReceiptNumber = e.CommandArgument.ToString();
                var oCurrentItem = e.Item as ListViewDataItem;                                
                int iRowId = oCurrentItem.DisplayIndex;                                                       
                string sTransportFeeDetailsId = lstvwTransportFee.DataKeys[iRowId]["TransportFeeDetailsId"].ToString();                
                bool bIsRefund = lstvwTransportFee.DataKeys[iRowId]["IsRefund"].ToBool();

                if (bIsRefund)
                    sReceiptNumber = sTransportFeeDetailsId;
                
                if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    moTransportChargesBL.Delete(sReceiptNumber,bIsRefund);
                    ClearFields();                    
                    DisplayFeeDetails();
                    DisplayMessage(S_DELETE_SUCCESS_MESSAGE);
                }
            }            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Here we delesect the select all checkbox after refilling the data.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTransportFee_DataBound(object sender, EventArgs e)
    {
        try
        {
            var chkSelectAll = lstvwTransportFee.FindControl("chkSelectAll") as CheckBox;
            if (!chkSelectAll.IsNull())
                chkSelectAll.Checked = false;
            if (lstvwTransportFee.Items.Count > 0)
            {
                if (hidIsOnlyRefund.Value.ToBool())
                {
                    var lbllstDueDate = lstvwTransportFee.FindControl("lbllstDueDate") as Label;
                    lbllstDueDate.Text = "Paid Date";
                    var thDelete = lstvwTransportFee.FindControl("thDelete") as HtmlTableCell;
                    var thPrint = lstvwTransportFee.FindControl("thPrint") as HtmlTableCell;
                    var thLateFee = lstvwTransportFee.FindControl("thLateFee") as HtmlTableCell;
                    thDelete.Visible = false;
                    thPrint.Visible = false;
                    thLateFee.Visible = false;
                }
            }
            hidRowCount.Value = lstvwTransportFee.Items.Count.ToString();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to pay the transport charges of the selected user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPay_Click(object sender, EventArgs e)
    {
        try
        {
            string sTransportFeeDetailsXML = GetDebitDetails();
            moTransportChargesBL.Insert(sTransportFeeDetailsXML);
            ClearFields();
            DisplayFeeDetails();
            DisplayMessage(S_PAY_SUCCESS_MESSAGE);            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to refund the fee payments done previously for selected users.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRefund_Click(object sender, EventArgs e)
    {
        try
        {
            List<int> lstTransportIds = (from currentItem in lstvwTransportFee.Items
                                        let chkSelect = currentItem.FindControl("chkSelect") as CheckBox
                                        where chkSelect != null && chkSelect.Checked
                                        select lstvwTransportFee.DataKeys[currentItem.DisplayIndex]["TransportFeeDetailsId"].ToInt()
                                            ).ToList();
            string sTransportFeeId = string.Join(", ", lstTransportIds);

            moTransportChargesBL.RefundCharges(sTransportFeeId, txtRefundDate.Text.ToDateTime());
            DisplayMessage(S_REFUND_SUCCESS_MESSAGE);
            DisplayFeeDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion -- EVENT HANDLER(s) --

    #region -- PRIVATE METHOD(s) --

    /// <summary>
    /// This function is used to set the default values on page laod.
    /// </summary>
    private void SetDefaultValues()
    {
        cal_PaymentDate.DateValue = Convert.ToDateTime(DateTime.Today.ToString("dd-MMM-yyyy"));
        PopCalendar1.DateValue = Convert.ToDateTime(DateTime.Today.ToString("dd-MMM-yyyy"));
        hidCurrentDate.Value = DateTime.Today.ToString("dd-MMM-yyyy");
        hidYearStartDate.Value = Convert.ToDateTime(Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE]).ToString("dd-MMM-yyyy");
        hidYearEndDate.Value = Convert.ToDateTime(Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE]).ToString("dd-MMM-yyyy");
    }

    /// <summary>
    /// This method is used to decrypt & read query string.
    /// </summary>
    private void ReadQuerystring()
    {
        if (Request.QueryString.ToString() == Constants.S_EMPTY_STRING)
            return;

        hidQueryString.Value = Request.QueryString.ToString();

        if (!QueryString["UserId"].IsNull())
            hidUserId.Value = QueryString["UserId"];

        if (!QueryString["RegNo"].IsNull())
            hidSearch.Value = QueryString["RegNo"];

        if (!QueryString["Name"].IsNull())
        {
            hidUserName.Value = QueryString["Name"];
            lblStudentHeading.Text = hidUserName.Value;
        }

        if (!QueryString["pIndex"].IsNull())
            hidPageIndex.Value = QueryString["pIndex"];

        if (!QueryString["RoleId"].IsNull())
            hidRole.Value = QueryString["RoleId"];

        if (!QueryString["IsRefund"].IsNull())
            hidIsOnlyRefund.Value = QueryString["IsRefund"];
        else
            hidIsOnlyRefund.Value = "False";
    }

    /// <summary>
    /// This method used to set java script attributes for buttons.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        btnClose.Attributes.Add("onclick", "CloseWindow();");
        ApplyMouseHoverEffect(new List<Button> { btnPay, btnClose, btnPayAndPrint,btnRefund});
    }

    /// <summary>
    /// This is a common function is used to display message on different actions like save/delete.
    /// </summary>
    /// <param name="sStr"></param>
    private void DisplayMessage(string sStr)
    {
        lblUpdateSucess.Visible = true;
        lblUpdateSucess.Text = sStr;
    }

    /// <summary>
    /// This method is used to clear the control after the fees paid/delete/date change.
    /// </summary>
    private void ClearFields()
    {
        cal_PaymentDate.DateValue = Convert.ToDateTime(DateTime.Today.ToString("dd-MMM-yyyy"));
        txtLateFeeAmt.Text = Constants.S_ZERO;
        txtConcessionAmt.Text = Constants.S_ZERO;
        txtActualAmt.Text = string.Empty;
        txtRemarks.Text = string.Empty;
        lblUpdateSucess.Text = string.Empty;
    }

    /// <summary>
    /// This method is used to collect the selected transport id's to pay.
    /// </summary>
    /// <returns></returns>    
    private string GetDebitDetails()
    {
        List<PayTransportCharges> lstPayTransportCharges = new List<PayTransportCharges>();

        List<int> lstTransportIds = (from currentItem in lstvwTransportFee.Items
                                    let chkSelect = currentItem.FindControl("chkSelect") as CheckBox
                                    where chkSelect != null && chkSelect.Checked
                                    select lstvwTransportFee.DataKeys[currentItem.DisplayIndex]["TransportFeeDetailsId"].ToInt()
                                        ).ToList();
        string sTransportFeeId = string.Join(", ", lstTransportIds);

        return GenerateXML(sTransportFeeId);       
    }

    /// <summary>
    /// This function is used to generate XML for selected transport charges details.
    /// </summary>
    /// <param name="asTransportFeeId"></param>
    /// <returns></returns>
    private string GenerateXML(string asTransportFeeId)
    {
        const string S_ELEMENT = "element";
        string sAttribute;
        var oDoc = new XmlDocument();
        XmlElement oElement = oDoc.CreateElement("TransportChargesInfo");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "TransportChargesInfo", String.Empty);

        XmlNode oXMLNode = oDoc.CreateNode(S_ELEMENT, "TransportChargesInfo", String.Empty);
        sAttribute = "TransportFeeIds";
        XmlAttribute oAttr = oDoc.CreateAttribute(sAttribute);
        oAttr.Value = asTransportFeeId;
        oXMLNode.Attributes.Append(oAttr);
        
        sAttribute = "PaymentDate";
        oAttr = oDoc.CreateAttribute(sAttribute);
        oAttr.Value = txtPaymentDate.Text.ToString();
        oXMLNode.Attributes.Append(oAttr);

        sAttribute = "LateFeeAmt";
        oAttr = oDoc.CreateAttribute(sAttribute);
        oAttr.Value = txtLateFeeAmt.Text;
        oXMLNode.Attributes.Append(oAttr);

        sAttribute = "ConcessionAmt";
        oAttr = oDoc.CreateAttribute(sAttribute);
        oAttr.Value = txtConcessionAmt.Text;
        oXMLNode.Attributes.Append(oAttr);

        sAttribute = "Remark";
        oAttr = oDoc.CreateAttribute(sAttribute);
        oAttr.Value = txtRemarks.Text;
        oXMLNode.Attributes.Append(oAttr);

        oXmlRootNode.AppendChild(oXMLNode);        
        oElement.AppendChild(oXmlRootNode);
        return oElement.InnerXml;
    }
    
    /// <summary>
    /// This Method is used to set default remark text.
    /// </summary>
    private void DisplayFeeDetails()
    {
        List<PayTransportCharges> lstPayTransportCharges = moTransportChargesBL.GetAll(hidUserId.Value.ToInt(), txtPaymentDate.Text.ToDateTime(), hidIsOnlyRefund.Value.ToBool());
        lstvwTransportFee.DataSource = lstPayTransportCharges;
        lstvwTransportFee.DataBind();
    }

    /// <summary>
    /// This method is used to refill the listview for selected payment date.
    /// </summary>
    private void FillLateFeeDetails()
    {
        DateTime dtValidDate = System.DateTime.Now;
        if (DateTime.TryParse(txtPaymentDate.Text, out dtValidDate))
            DisplayFeeDetails();
        else
        {
            cal_PaymentDate.DateValue = System.DateTime.Now;
            DisplayFeeDetails();
        }

        txtLateFeeAmt.Text = Constants.S_ZERO;
        txtConcessionAmt.Text = Constants.S_ZERO;
    }

    /// <summary>
    /// This method is used to set the mode i.e. Pay/refund. Controls will be displayed as per the mode.
    /// </summary>
    private void SetMode()
    {
        if (!hidIsOnlyRefund.Value.IsNullOrEmpty() && hidIsOnlyRefund.Value.ToBool())
        {            
            tblFeeDetails.Visible = false;
            btnPay.Visible = false;
            btnPayAndPrint.Visible = false;
            trLegend.Visible = false;
            lblMainTitleHead.InnerText = "Refund Transport Charges";
        }
        else
        {
            tblPendingFeeDetails.Visible = false;
            btnRefund.Visible = false;
        }
    }

    /// <summary>
    /// This method is called to set the style to the late fee charges for the listview.
    /// </summary>
    /// <param name="aoListViewDataItem"></param>
    private void SetLateFeeStyle(ListViewDataItem aoListViewDataItem)
    {
        int iRowId = aoListViewDataItem.DisplayIndex;
        CheckBox chkSelect = aoListViewDataItem.FindControl("chkSelect") as CheckBox;
        var lblDueDate = aoListViewDataItem.FindControl("lblDueDate") as Label;
        bool bIsRefund = lstvwTransportFee.DataKeys[iRowId]["IsRefund"].ToBool();
        var lblAmount = aoListViewDataItem.FindControl("lblAmount") as Label;
        var lblPaybleFor = aoListViewDataItem.FindControl("lblPaybleFor") as Label;
        var lblMonth = aoListViewDataItem.FindControl("lblMonth") as Label;
        var lblLateFee = aoListViewDataItem.FindControl("lblLateFee") as Label;
        var tableRow = aoListViewDataItem.FindControl("trlstvwRow") as HtmlTableRow;

        if (!chkSelect.Checked && chkSelect.Visible && lblDueDate.Text.ToDateTime() < DateTime.Today && !bIsRefund)
        {
            tableRow.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#FEEABA");
            lblDueDate.ForeColor = Color.Red;
            lblAmount.ForeColor = Color.Red;
            lblPaybleFor.ForeColor = Color.Red;
            lblMonth.ForeColor = Color.Red;
            lblLateFee.ForeColor = Color.Red;
        }
    }

    /// <summary>
    /// This method is used to set the refund style details for the listview.
    /// </summary>
    private void SetRefundStyle(ListViewDataItem aoListViewDataItem)
    {
        int iRowId = aoListViewDataItem.DisplayIndex;
        CheckBox chkSelect = aoListViewDataItem.FindControl("chkSelect") as CheckBox;
        bool bIsRefund = lstvwTransportFee.DataKeys[iRowId]["IsRefund"].ToBool();
        var tableRow = aoListViewDataItem.FindControl("trlstvwRow") as HtmlTableRow;

        if (bIsRefund)
        {
            tableRow.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#E6E9C7");
            chkSelect.Visible = false;
        }

        if (hidIsOnlyRefund.Value.ToBool())
        {
            var tdDelete = aoListViewDataItem.FindControl("tdDelete") as HtmlTableCell;
            var tdPrint = aoListViewDataItem.FindControl("tdPrint") as HtmlTableCell;
            var tdLateFee = aoListViewDataItem.FindControl("tdLateFee") as HtmlTableCell;
            tdDelete.Visible = false;
            tdPrint.Visible = false;
            tdLateFee.Visible = false;
            chkSelect.Visible = true;
        }
    }

    #endregion -- PRIVATE METHOD(s) --                
}