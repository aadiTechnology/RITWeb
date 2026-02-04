// File Name  : CardClearanceListUI.aspx.cs
// Created By : Deepak
// Date       : 26 November 2010
//Description :This class is used to cleare the swipe card payments.

using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Xml;
using System.Threading;
using BusinessLogic;
using Utility;
public partial class CardClearanceListUI : System.Web.UI.Page
{
    #region "CONSTANTS"

    const string HIDE_PAGE_NUMBER = "1";
    const int I_COLUMN_INDEX_CLEARANCE_DATETIME = 5;

    #endregion

    int miTotalAmount;

    #region Events

    /// <summary>
    /// This event is used Initialise controls, set javascript attributes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                InitializeControls();
                FillCardTypeCombo();
                SetJavaScriptAttribute();
                btnSave.Style.Add("Visibility", "Hidden");
                btnExport.Style.Add("Visibility", "Hidden");
            }
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
            (ex.Message + Constants.S_TRACE + ex.StackTrace,
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
            Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    /// <summary>
    /// This event is used to set filter of Registration number for displaying grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optRegNo_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            optRegNoChecked();
            InvisibleErrorMessage();
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
            (ex.Message + Constants.S_TRACE + ex.StackTrace,
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
            Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    /// <summary>
    /// This event is used to set filter based on Payment Date for displaying grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optPaymentDate_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            optPaymentDateChecked();
            InvisibleErrorMessage();
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
            (ex.Message + Constants.S_TRACE + ex.StackTrace,
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
            Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    /// <summary>
    /// This event is used to set filter based on Clearance Date for displaying grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optClearanceDate_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            optClearanceDateChecked();
            InvisibleErrorMessage();
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
            (ex.Message + Constants.S_TRACE + ex.StackTrace,
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
            Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }
    
    /// <summary>
/// This method is used to set enable disable filters and to display the data as per selected filters.
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnShow.Text == "Show")
            {
                hidPageNo.Value = "1";
                grdvwCardPayments.PageIndex = 0;                
                grdvwCardPayments.Visible = true;
                lblSuccessMsg.Visible = true;
                FillCardPaymentsGrid();
                btnShow.Text = "Change Input";                

                EnableDisableControlChecked(false);
                EnableDisableControl(false);
               }
            else
            {
                btnShow.Text = "Show";
                grdvwCardPayments.DataSource = null;
                grdvwCardPayments.DataBind();
                grdvwCardPayments.Visible = false;
                trTotalRec.Visible = false;
                lblSuccessMsg.Visible = false;
                lblSuccessMsg.Text = string.Empty;
                tblTotalAmount.Visible = false;
                EnableDisableControlChecked(true);
                EnableDisableControl(true);
                grdvwCardPayments.PageIndex = 0;               
            }
            lblError.Visible = false;
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
            (ex.Message + Constants.S_TRACE + ex.StackTrace,
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
            Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    /// <summary>
    /// This method is used to save cleared swipe card payments.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string sXML = GenerateXML();
            CashClearanceListBL.UpdateCardPaymentsDetails(sXML);
            lblError.Visible = false;
            lblSuccessMsg.Visible = true;
            lblSuccessMsg.Text = "Swipe Card Clearance data updated successfully !!!";
            FillCardPaymentsGrid();
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
            (ex.Message + Constants.S_TRACE + ex.StackTrace,
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
            Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    /// <summary>
    /// This event is used to fill footer drop down list in the grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwCardPayments_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                hidRowCnt.Value = grdvwCardPayments.Rows.Count.ToString();
                GridViewRow PageRow = e.Row;
                DropDownList pageList = (DropDownList)PageRow.Cells[0].FindControl("PageDropDownList");
                pageList.Attributes.Add("onchange", "if(!MessageAboutDate('" + pageList.ClientID + "')){return false;}");
                Label oPageLabel = (Label)PageRow.Cells[0].FindControl("CurrentPageLabel");
                if (pageList != null)
                {
                    // Create the values for the DropDownList control based on 
                    // the  total number of pages required to display the data
                    // source.
                    for (int i = 0; i < grdvwCardPayments.PageCount; i++)
                    {
                        // Create a ListItem object to represent a page.
                        int pageNumber = i + 1;
                        ListItem item = new ListItem(pageNumber.ToString());

                        // If the ListItem object matches the currently selected
                        // page, flag the ListItem object as being selected. Because
                        // the DropDownList control is recreated each time the pager
                        // row gets created, this will persist the selected item in
                        // the DropDownList control.   
                        if (i == grdvwCardPayments.PageIndex)
                            item.Selected = true;

                        // Add the ListItem object to the Items collection of the 
                        // DropDownList.
                        pageList.Items.Add(item);
                    }
                }
                if (oPageLabel != null)
                {
                    // Calculate the current page number.
                    int currentPage = grdvwCardPayments.PageIndex + 1;

                    // Update the Label control with the current page information.
                    oPageLabel.Text = "Page " + currentPage.ToString() +
                      " of " + grdvwCardPayments.PageCount.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
            (ex.Message + Constants.S_TRACE + ex.StackTrace,
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
            Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
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
            GridViewRow oPageRow = grdvwCardPayments.BottomPagerRow;
            DropDownList oPageNumberList = (DropDownList)oPageRow.Cells[0].FindControl("PageDropDownList");
            grdvwCardPayments.PageIndex = oPageNumberList.SelectedIndex;
            grdvwCardPayments.DataSourceID = objDSCardPayment.ID;
            grdvwCardPayments.DataBind();
            hidPageNo.Value = (oPageNumberList.SelectedIndex + 1).ToString();
            lblError.Visible = false;
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
            (ex.Message + Constants.S_TRACE + ex.StackTrace,
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
            Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    /// <summary>
    /// This event is used to set record count.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GrdDSobj_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {
            if (e.ReturnValue.ToString() != "" && e.ReturnValue != null)
            {
                lblStartIndex.Text = Convert.ToString((grdvwCardPayments.PageSize * grdvwCardPayments.PageIndex) + 1);
                lblEndIndex.Text = Convert.ToString((Convert.ToInt32(lblStartIndex.Text) + grdvwCardPayments.PageSize) - 1);
                if (e.ReturnValue.ToString() != "" && e.ReturnValue != null)
                {
                    lblTotal.Text = e.ReturnValue.ToString();
                    if (e.ReturnValue.GetType() != typeof(DataTable))
                    {
                        if (Convert.ToInt32(lblEndIndex.Text) > Convert.ToInt32(lblTotal.Text))
                            lblEndIndex.Text = e.ReturnValue.ToString();
                        if (Convert.ToInt32(lblTotal.Text) <= Constants.I_GRID_PAGE_COUNT)
                            trTotalRec.Visible = false;
                        else
                            trTotalRec.Visible = true;
                    }
                    if (lblTotal.Text != "")
                    {
                        if (Convert.ToInt32(lblTotal.Text) <= Constants.I_GRID_PAGE_COUNT)
                            trTotalRec.Visible = false;
                        else
                            trTotalRec.Visible = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
             (ex.Message + Constants.S_TRACE + ex.StackTrace,
             System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
             Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// This method is used to initialize controls.
    /// </summary>
    private void InitializeControls()
    {
        optRegNo.Checked = true;
        optRegNoChecked();
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        valSave.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        grdvwCardPayments.PageSize = Constants.I_GRID_PAGE_COUNT;
        hidPageNo.Value = HIDE_PAGE_NUMBER;
    }

    /// <summary>
    /// This method is used to set JavaScript attributes.
    /// </summary>
    private void SetJavaScriptAttribute()
    {
        btnShow.Attributes["onmouseover"] = "javascript:fnover('" + btnShow.ClientID + "');";
        btnShow.Attributes["onmouseout"] = "javascript:fnout('" + btnShow.ClientID + "');";
        btnSave.Attributes["onmouseover"] = "javascript:fnover('" + btnSave.ClientID + "');";
        btnSave.Attributes["onmouseout"] = "javascript:fnout('" + btnSave.ClientID + "');";
        optRegNo.Attributes.Add("onclick", "if(!ClearValSum()){return false;}");
        optClearanceDate.Attributes.Add("onclick", "if(!ClearValSum()){return false;}");
        optPaymentDate.Attributes.Add("onclick", "if(!ClearValSum()){return false;}");
    }

    /// <summary>
    /// This method is used to clear texts.
    /// </summary>
    private void ClearTextboxes()
    {
        txtRegNo.Text = string.Empty;
        txtPaymentStartDate.Text = string.Empty;
        txtPaymentEndDate.Text = string.Empty;
        txtClearanceStartDate.Text = string.Empty;
        txtClearanceEndDate.Text = string.Empty;
    }

    /// <summary>
    /// This method is used set controls when PaymentDate radio button checked.
    /// </summary>co
    private void optPaymentDateChecked()
    {
        ClearTextboxes();
        txtRegNo.Enabled = false;
        txtPaymentStartDate.Enabled = true;
        txtPaymentEndDate.Enabled = true;
        txtClearanceStartDate.Enabled = false;
        txtClearanceEndDate.Enabled = false;
        chkIncludeAll.Checked = false;
    }

    /// <summary>
    /// This method is used set controls when ClearanceDate radio button checked.
    /// </summary>
    private void optClearanceDateChecked()
    {
        ClearTextboxes();
        txtRegNo.Enabled = false;
        txtPaymentStartDate.Enabled = false;
        txtPaymentEndDate.Enabled = false;
        txtClearanceStartDate.Enabled = true;
        txtClearanceEndDate.Enabled = true;
        chkIncludeAll.Checked = true;
    }
    /// <summary>
    /// This method is used to hide error message.
    /// </summary>
    private void InvisibleErrorMessage()
    {
        trTotalRec.Visible = false;
        lblError.Visible = false;
    }

    /// <summary>
    /// This method is used set controls when RegNo radio button checked.
    /// </summary>
    private void optRegNoChecked()
    {
        txtRegNo.Focus();
        ClearTextboxes();
        txtRegNo.Enabled = true;
        txtPaymentStartDate.Enabled = false;
        txtPaymentEndDate.Enabled = false;
        txtClearanceStartDate.Enabled = false;
        txtClearanceEndDate.Enabled = false;
        chkIncludeAll.Checked = false;
    }
   
    /// <summary>
    /// This method used to enabled or disabled controls.
    /// </summary>
    private void EnableDisableControl(bool abflag)
    {
        optRegNo.Enabled = abflag;
        optPaymentDate.Enabled = abflag;
        optClearanceDate.Enabled = abflag;
        chkIncludeAll.Enabled = abflag;
    }

    /// <summary>
    /// This method used to enabled or disabled depending on radio button control checked.
    /// </summary>
    /// 
    private void EnableDisableControlChecked(bool abFlag)
    {
        if (optRegNo.Checked)
            txtRegNo.Enabled = abFlag;
        else if (optPaymentDate.Checked)
        {
            txtPaymentStartDate.Enabled = abFlag;
            txtPaymentEndDate.Enabled = abFlag;
        }
        else if (optClearanceDate.Checked)
        {
            txtClearanceStartDate.Enabled = abFlag;
            txtClearanceEndDate.Enabled = abFlag;
        }
        cmbCardType.Enabled = abFlag;
    }

    /// <summary>
    /// This method is used to fill card type combo.
    /// </summary>
    private void FillCardTypeCombo()
    {
        SchoolwiseBankMasterBL oSchoolwiseBankMasterBL = new SchoolwiseBankMasterBL();
        DataTable oDT = oSchoolwiseBankMasterBL.GetSchoolwiseCardTypeList(Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]));
        ControlUtility.FillDropDownList(oDT, ref cmbCardType, "CardTypeId", "CardType", Constants.S_SELECT_ALL);
    }  

    /// <summary>
    /// This method is used to collect paramters and send it to Stored procedure.
    /// </summary>
    /// <returns></returns>
    private string GenerateXML()
    {
        const string S_ELEMENT = "element";
        string sAttribute;
        XmlDocument oDoc = new XmlDocument();
        XmlElement oElement = oDoc.CreateElement("ClearedCardPaymentInfo");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "ClearedCardPaymentInfo", "");
        for (int i = 0; i < grdvwCardPayments.Rows.Count; i++)
        {
            TextBox otxtClearanceDate = (TextBox)grdvwCardPayments.Rows[i].Cells[I_COLUMN_INDEX_CLEARANCE_DATETIME].FindControl("txtclearance");


            XmlNode oXMLNode = oDoc.CreateNode(S_ELEMENT, "ClearedCardPaymentInfo", "");

            sAttribute = "StudentCardPaymentDetailsId";
            XmlAttribute oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = grdvwCardPayments.DataKeys[i]["StudentCardPaymentDetailsId"].ToString();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "ClearanceDate";
            oAttr = oDoc.CreateAttribute(sAttribute);
            if (otxtClearanceDate.Text.Trim() != "")
                oAttr.Value = otxtClearanceDate.Text.Trim();
            else
                oAttr.Value = "";
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "SchoolId";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = (Session[Constants.S_SESSION_SCHOOL_ID]).ToString(); ;
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "AcademicYearId";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = (Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID]).ToString();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "Insert_Date";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = System.DateTime.Now.ToString();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "Inserted_By_Id";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = Session[Constants.S_SESSION_USER_ID].ToString();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "Update_Date";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = System.DateTime.Now.ToString();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "Updated_By_Id";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = Session[Constants.S_SESSION_USER_ID].ToString();
            oXMLNode.Attributes.Append(oAttr);
            oXmlRootNode.AppendChild(oXMLNode);

        }
        oElement.AppendChild(oXmlRootNode);
        return oElement.InnerXml;
    }

    /// <summary>
    /// This method is used to fill swipe card payments grid.
    /// </summary>
    private void FillCardPaymentsGrid()
    {
        grdvwCardPayments.DataSourceID = objDSCardPayment.ID;
        grdvwCardPayments.DataBind();
        CashClearanceListBL oCashClearanceListBL = new CashClearanceListBL();
        miTotalAmount = oCashClearanceListBL.CardPaymentsTotalAmount(Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]), Convert.ToInt32(Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID]), txtRegNo.Text, txtPaymentStartDate.Text, txtPaymentEndDate.Text,
                                                                     txtClearanceStartDate.Text, txtClearanceEndDate.Text, chkIncludeAll.Checked, Convert.ToInt32(cmbCardType.SelectedValue));

        if (miTotalAmount != 0)
        {
            tblTotalAmount.Visible = true;
            lblTotalAmount.Text = miTotalAmount.ToString();
        }
        else
            tblTotalAmount.Visible = false;
    }

    #endregion

    #region Functionality For Export

    /// <summary>
    /// This event is used to export the cheque clearance details in the Excel sheet.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.CardPaymentDetails, GetFilterString());
            oReportDisplay.DisplayReport();
        }
        catch (ThreadAbortException)
        {

        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
            (ex.Message + Constants.S_TRACE + ex.StackTrace,
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
            Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    /// <summary>
    /// This method generates the report filter as per the field selection.
    /// </summary>
    /// <returns></returns>
    private string GetFilterString()
    {
        string sSchoolYearFilter = "";
        int iSchoolID = Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]);
        int iAcadYearID = Convert.ToInt32(Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID]);
        string sViewNameSchoolId = Constants.S_EXPORT_CARDPAYMENTS_USP + ".SchoolId}";
        string sViewNameAcademic_Year_Id = Constants.S_EXPORT_CARDPAYMENTS_USP + ".Academic_Year_Id}";
        string sViewNameRegNo = Constants.S_EXPORT_CARDPAYMENTS_USP + ".RegNo}";
        string sViewNamePaymentStartDate = Constants.S_EXPORT_CARDPAYMENTS_USP + ".PaymentStartDate}";
        string sViewNamePaymentEndDate = Constants.S_EXPORT_CARDPAYMENTS_USP + ".PaymentEndDate}";
        string sViewNameClearanceStartDate = Constants.S_EXPORT_CARDPAYMENTS_USP + ".ClearanceStartDate}";
        string sViewNameClearanceEndDate = Constants.S_EXPORT_CARDPAYMENTS_USP + ".ClearanceEndDate}";
        string sViewNameIncludeAll = Constants.S_EXPORT_CARDPAYMENTS_USP + ".IncludeAll}";
        string sViewNameCardTypeId = Constants.S_EXPORT_CARDPAYMENTS_USP + ".CardTypeId}";      
        
       sSchoolYearFilter= sViewNameSchoolId +"="+iSchoolID + " AND " + sViewNameAcademic_Year_Id + "=" +iAcadYearID;

       if (txtRegNo.Text != null && txtRegNo.Text != "")
           sSchoolYearFilter = sSchoolYearFilter + " AND " + sViewNameRegNo + "=" + txtRegNo.Text.Trim();
        else
           sSchoolYearFilter = sSchoolYearFilter + " AND " + sViewNameRegNo + "= null";


       if (txtPaymentStartDate.Text != null && txtPaymentStartDate.Text != "")
           sSchoolYearFilter = sSchoolYearFilter + " AND " + sViewNamePaymentStartDate + "=" + txtPaymentStartDate.Text.Trim();
       else
           sSchoolYearFilter = sSchoolYearFilter + " AND " + sViewNamePaymentStartDate + "= null";

       if (txtPaymentEndDate.Text != null && txtPaymentEndDate.Text != "")
           sSchoolYearFilter = sSchoolYearFilter + " AND " + sViewNamePaymentEndDate + "=" + txtPaymentEndDate.Text.Trim();
       else
           sSchoolYearFilter = sSchoolYearFilter + " AND " + sViewNamePaymentEndDate + "= null";

       if (txtClearanceStartDate.Text != null && txtClearanceStartDate.Text != "")
           sSchoolYearFilter = sSchoolYearFilter + " AND " + sViewNameClearanceStartDate + "=" + txtClearanceStartDate.Text.Trim();
       else
           sSchoolYearFilter = sSchoolYearFilter + " AND " + sViewNameClearanceStartDate + "= null";

       if (txtClearanceEndDate.Text != null && txtClearanceEndDate.Text != "")
           sSchoolYearFilter = sSchoolYearFilter + " AND " + sViewNameClearanceEndDate + "=" + txtClearanceEndDate.Text.Trim();
       else
           sSchoolYearFilter = sSchoolYearFilter + " AND " + sViewNameClearanceEndDate + "= null";

        sSchoolYearFilter = sSchoolYearFilter + " AND " + sViewNameIncludeAll + "=" + (chkIncludeAll.Checked ? "1" : "0");

        sSchoolYearFilter = sSchoolYearFilter + " AND " + sViewNameCardTypeId + "=" + cmbCardType.SelectedValue;

        return "(" + sSchoolYearFilter + ")@ ";
    }

    #endregion
}
