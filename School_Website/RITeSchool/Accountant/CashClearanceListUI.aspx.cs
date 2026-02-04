using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;
using BusinessLogic;
using DataCommunicator;
using Utility;
using System.Text;
using System.Threading;

public partial class CashClearanceListUI : System.Web.UI.Page
{
    #region "CONSTANTS"

    const string HIDE_PAGE_NUMBER = "1";

    #endregion

    #region Events
    /// <summary>
    /// This event is used Initialise controls.
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
                SetClientScriptAttribute();
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
    /// This event is used to fill  grid according to filter and cleare paid cash amount.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnShow.Text == "Show")
            {
                hidPageNo.Value = HIDE_PAGE_NUMBER;
                grdvwClearedCash.PageIndex = Constants.I_ZERO;
                FillClearedCashPaymentGrid();
                btnShow.Text = "Change Input";
                EnableDisableControlChecked(false);
                EnableDisableControls(false);
            }
            else
            {
                btnShow.Text = "Show";
                EnableDisableControlChecked(true);
                EnableDisableControls(true);
                grdvwClearedCash.DataSource = null;
                grdvwClearedCash.DataBind();
                grdvwClearedCash.Visible = false;
                trTotalRec.Visible = false;
                tblTotalAmount.Visible = false;
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
            GridViewRow oPageRow = grdvwClearedCash.BottomPagerRow;
            DropDownList oPageNumberList = (DropDownList)oPageRow.Cells[0].FindControl("PageDropDownList");
            grdvwClearedCash.PageIndex = oPageNumberList.SelectedIndex;
            FillClearedCashPaymentGrid();
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
    /// This event is used to save Cash payments which are cleared.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string sXML = GenerateXML();
            CashClearanceListBL oUpdateStudentClearedCashPayment = new CashClearanceListBL();
            oUpdateStudentClearedCashPayment.UpdateCashClearanceDate(sXML);
            lblError.Visible = false;
            lblSuccessMsg.Visible = true;
            lblSuccessMsg.Text = "Cash Clearance data updated successfully !!!";
            FillClearedCashPaymentGrid();
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
    /// This event is used to fill footer dropdown list in the grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwClearedCash_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                GridViewRow PageRow = e.Row;
                DropDownList oPageList = (DropDownList)PageRow.Cells[0].FindControl("PageDropDownList");
                oPageList.Attributes.Add("onchange", "if(!MessageAboutDate('" + oPageList.ClientID + "')){return false;}");
                Label oPageLabel = (Label)PageRow.Cells[0].FindControl("CurrentPageLabel");
                if (oPageList != null)
                {
                    for (int i = 0; i < grdvwClearedCash.PageCount; i++)
                    {
                        int iPageumber = i + 1;
                        ListItem oListItem = new ListItem(iPageumber.ToString());
                        if (i == grdvwClearedCash.PageIndex)
                            oListItem.Selected = true;
                        oPageList.Items.Add(oListItem);
                    }
                }
                if (oPageLabel != null)
                {
                    int iCurrentPageCount = grdvwClearedCash.PageIndex + 1;
                    oPageLabel.Text = "Page " + iCurrentPageCount.ToString() + " " +
                        "of" + " " + grdvwClearedCash.PageCount.ToString();
                }
                DisplayRowDetails();
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
    /// This event is used to set grid according to selected page in the footer drop down list of grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwClearedCash_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdvwClearedCash.PageIndex = e.NewPageIndex;
            FillClearedCashPaymentGrid();
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
    /// This method is used to fill GridView.
    /// </summary>
    private void FillClearedCashPaymentGrid()
    {
        const int FILTER_BY_STUDENT_NAME_REG_NO = 1;
        const int FILTER_BY_PAID_DATE = 2;
        const int FILTER_BY_CLEARANCE_DATE = 3;
        DataTable odtClearedCash = null;
        int iSchoolId = Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]);
        int iAcademicYearId = Convert.ToInt32(Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID]);
        int iTotalAmount = 0;
        if (optRegNo.Checked == true)
            odtClearedCash = CashClearanceListBL.FetchClearedCashDetails(txtRegNo.Text.Trim(), string.Empty, string.Empty, chkIncludeAll.Checked, FILTER_BY_STUDENT_NAME_REG_NO, iSchoolId, iAcademicYearId,out iTotalAmount);
        else if (optPaymentDate.Checked == true)
            odtClearedCash = CashClearanceListBL.FetchClearedCashDetails(null, txtPaymentStartDate.Text.Trim(), txtPaymentEndDate.Text.Trim(), chkIncludeAll.Checked, FILTER_BY_PAID_DATE, iSchoolId, iAcademicYearId, out iTotalAmount);
        else if (optClearanceDate.Checked == true)
            odtClearedCash = CashClearanceListBL.FetchClearedCashDetails(null, txtClearanceStartDate.Text.Trim(), txtClearanceEndDate.Text.Trim(), chkIncludeAll.Checked, FILTER_BY_CLEARANCE_DATE, iSchoolId, iAcademicYearId, out iTotalAmount);

        if (odtClearedCash != null)
        {
            grdvwClearedCash.Visible = true;
            grdvwClearedCash.DataSource = odtClearedCash.DefaultView;
            grdvwClearedCash.DataBind();
            hidRowCnt.Value = Convert.ToString(grdvwClearedCash.Rows.Count);
            tblTotalAmount.Visible = true;
            lblTotalAmount.Text = iTotalAmount.ToString();
        }
        if (odtClearedCash.Rows.Count == 0)
        {
            trTotalRec.Visible = false;
            tblTotalAmount.Visible = false;
        }
    }
    /// <summary>
    /// This method used to set the value to the label indicating records from the grid.
    /// </summary>
    private void DisplayRowDetails()
    {
        int iRowCount = ((DataView)(grdvwClearedCash.DataSource)).Count;
        lblStartIndex.Text = Convert.ToString((grdvwClearedCash.PageSize * grdvwClearedCash.PageIndex) + 1);
        lblEndIndex.Text = Convert.ToString((Convert.ToInt32(lblStartIndex.Text) + grdvwClearedCash.PageSize) - 1);
        lblTotal.Text = iRowCount.ToString();
        if (Convert.ToInt32(lblEndIndex.Text) > Convert.ToInt32(lblTotal.Text))
            lblEndIndex.Text = iRowCount.ToString();
        if (iRowCount.ToString() == "0")
            trTotalRec.Visible = false;
        else
            trTotalRec.Visible = true;
        if (lblTotal.Text != "")
        {
            if (Convert.ToInt32(lblTotal.Text) <= Constants.I_GRID_PAGE_COUNT)
                trTotalRec.Visible = false;
            else
                trTotalRec.Visible = true;
        }
    }
    private void InvisibleErrorMessage()
    {
        trTotalRec.Visible = false;
        lblError.Visible = false;
    }
    /// <summary>
    /// This method used to enabled or disabled controls.
    /// </summary>
    private void EnableDisableControls(bool abflag)
    {
        optRegNo.Enabled = abflag;
        optPaymentDate.Enabled = abflag;
        optClearanceDate.Enabled = abflag;
        chkIncludeAll.Enabled = abflag;
    }
    /// <summary>
    /// This method used to enabled or disabled radio button controls.
    /// </summary>
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
    }
    /// <summary>
    /// This method is used to clear texts.
    /// </summary>
    private void  ClearTextboxes()
    {
        txtRegNo.Text = string.Empty;
        txtPaymentStartDate.Text = string.Empty;
        txtPaymentEndDate.Text = string.Empty;
        txtClearanceStartDate.Text = string.Empty;
        txtClearanceEndDate.Text = string.Empty;
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
    /// This method is used set controls when PaymentDate radio button checked.
    /// </summary>
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
    /// This method is used set controls when learanceDate radio button checked.
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
    /// This method is used to Initialize controls.
    /// </summary>
    private void InitializeControls()
    {
       
        optRegNo.Checked = true;
        optRegNoChecked();
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        valSave.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        grdvwClearedCash.PageSize = Constants.I_GRID_PAGE_COUNT;
        hidPageNo.Value = HIDE_PAGE_NUMBER;
    }

    /// <summary>
    /// This method is used to collect paramters and send it to Stored procedure.
    /// </summary>
    /// <returns></returns>
    private string GenerateXML()
    {
        const int I_COLUMN_INDEX_CLEARANCE_DATETIME = 5;
        const string S_ELEMENT = "element";
        string sAttribute;
        XmlDocument oDoc = new XmlDocument();
        XmlElement oElement = oDoc.CreateElement("ClearedCashInfo");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "ClearedCashInfo", "");
        for (int i = 0; i < grdvwClearedCash.Rows.Count; i++)
        {
            TextBox otxtClearanceDate = (TextBox)grdvwClearedCash.Rows[i].Cells[I_COLUMN_INDEX_CLEARANCE_DATETIME].FindControl("txtclearance");
        
                XmlNode oXMLNode = oDoc.CreateNode(S_ELEMENT, "ClearedCashInfo", "");

                sAttribute = "Receipt_Number";
                XmlAttribute oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = grdvwClearedCash.DataKeys[i]["Receipt_Number"].ToString();
                oXMLNode.Attributes.Append(oAttr);

                sAttribute = "ClearanceDate";
                oAttr = oDoc.CreateAttribute(sAttribute);
                if (otxtClearanceDate.Text.Trim() != "")
                    oAttr.Value = otxtClearanceDate.Text.Trim();
                else
                    oAttr.Value = DBNull.Value.ToString();
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
    /// This method is used to set JavaScript attributes.
    /// </summary>
    private void SetClientScriptAttribute()
    {
        btnShow.Attributes["onmouseover"] = "javascript:fnover('" + btnShow.ClientID + "');";
        btnShow.Attributes["onmouseout"] = "javascript:fnout('" + btnShow.ClientID + "');";
        btnSave.Attributes["onmouseover"] = "javascript:fnover('" + btnSave.ClientID + "');";
        btnSave.Attributes["onmouseout"] = "javascript:fnout('" + btnSave.ClientID + "');";
        optRegNo.Attributes.Add("onclick", "if(!ClearValSum()){return false;}");
        optClearanceDate.Attributes.Add("onclick", "if(!ClearValSum()){return false;}");
        optPaymentDate.Attributes.Add("onclick", "if(!ClearValSum()){return false;}");
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
            ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.ExportCashPayment, GetFilterString());
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
        string sViewNameSchID = Constants.S_EXPORTUSP_CLEAREDCASHPAYMENT_USP + ".SchoolId}";
        string sViewNameAcdYearId = Constants.S_EXPORTUSP_CLEAREDCASHPAYMENT_USP + ".Academic_Year_Id}";
        string sViewNameRegNo = Constants.S_EXPORTUSP_CLEAREDCASHPAYMENT_USP + ".RegNo}";
        string sViewNamePaymentStartDate = Constants.S_EXPORTUSP_CLEAREDCASHPAYMENT_USP + ".PaymentStartDate}";
        string sViewNamePaymentEndDate = Constants.S_EXPORTUSP_CLEAREDCASHPAYMENT_USP + ".PaymentEndDate}";
        string sViewNameClearanceStartDate = Constants.S_EXPORTUSP_CLEAREDCASHPAYMENT_USP + ".ClearanceStartDate}";
        string sViewNameClearanceEndDate = Constants.S_EXPORTUSP_CLEAREDCASHPAYMENT_USP + ".ClearanceEndDate}";
        string sViewNameIncldeCheck = Constants.S_EXPORTUSP_CLEAREDCASHPAYMENT_USP + ".abIncldeCheck}";

        if (optPaymentDate.Checked)
        {
            sSchoolYearFilter = "(" + sViewNameSchID + "=" + iSchoolID + " AND " +
                                                           sViewNameAcdYearId + "=" + iAcadYearID +
                                                            " AND " +
                                                            sViewNamePaymentStartDate + "=" + (txtPaymentStartDate.Text.Trim() == string.Empty ? "null" : txtPaymentStartDate.Text.Trim()) + " AND " +
                                      sViewNamePaymentEndDate + "=" + (txtPaymentEndDate.Text.Trim() == string.Empty ? "null" : txtPaymentEndDate.Text.Trim()) + " AND " +
                                                                            sViewNameRegNo + " =null AND " +
                                      sViewNameIncldeCheck + "=" + chkIncludeAll.Checked.ToString()  + ")";

        }
        else if (optRegNo.Checked)
        {
            sSchoolYearFilter = "(" + sViewNameSchID + "=" + iSchoolID + " AND " +
                                                           sViewNameAcdYearId + "=" + iAcadYearID +
                                                            " AND " +
                                      sViewNameRegNo + "=" + (txtRegNo.Text.Trim() == string.Empty ? "null" : txtRegNo.Text.Trim()) + " AND " +
                                      sViewNamePaymentStartDate + "= null AND " +
                                      sViewNamePaymentEndDate + "= null AND " +
                                      sViewNameClearanceStartDate + "= null AND " +                                     
                                      sViewNameClearanceEndDate + "= null AND " +
                                      sViewNameIncldeCheck + "=" + chkIncludeAll.Checked.ToString() + ")";
        }
        else if (optClearanceDate.Checked)
        {
            sSchoolYearFilter = "(" + sViewNameSchID + "=" + iSchoolID + " AND " +
                                                          sViewNameAcdYearId + "=" + iAcadYearID +
                                                           " AND " +
                                     sViewNameRegNo + "=" + (txtRegNo.Text.Trim() == string.Empty ? "null" : txtRegNo.Text.Trim()) + " AND " +
                                     sViewNamePaymentStartDate + "= null AND " +
                                     sViewNamePaymentEndDate + "= null AND " +
                                     sViewNameClearanceStartDate + "=" + (txtClearanceStartDate.Text.Trim() == string.Empty ? "null" : txtClearanceStartDate.Text.Trim()) + " AND " +
                                     sViewNameClearanceEndDate + "=" + (txtClearanceEndDate.Text.Trim() == string.Empty ? "null" : txtClearanceEndDate.Text.Trim()) + " AND " +
                                     sViewNameIncldeCheck + "=" + chkIncludeAll.Checked.ToString() + ")";
        }

        return sSchoolYearFilter + "@ ";
    }

    #endregion
}
