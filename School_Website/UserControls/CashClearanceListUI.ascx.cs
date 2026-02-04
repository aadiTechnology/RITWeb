using System;
using System.Data;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Collections.Generic;
using BusinessLogic;
using Utility;



public partial class CashClearanceListUI : PaymentClearanceUC
{
    #region "Constants"

    const string HIDE_PAGE_NUMBER = "1";

    #endregion

    #region "Properties"

    public RadioButton RegNoRadioButton
    {
        get { return ClearanceListFilters.RegNoRadioButton; }
    }

    public Button ShowButton
    {
        get { return btnShow; }
    }

    #endregion

    #region Events

    ///<summary>
    ///This event is used initialise controls and set client side attributes.
    ///</summary>
    ///<param name="sender"></param>
    ///<param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                IntializeControls();
                SetClientScriptAttribute(AddControls());
                HideButtons(btnSave, btnExport);
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

    ///<summary>
    /// This event is used to fill  grid according to filter and cleare paid cash amount.
    ///</summary>
    ///<param name="sender"></param>
    ///<param name="e"></param>
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            ShowPaymnetsGrid();
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
            (ex.Message + Constants.S_TRACE + ex.StackTrace,
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
            Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    ///<summary>
    /// This event is used to set grid according to selected page in the footer drop down list of grid.
    ///</summary>
    ///<param name="sender"></param>
    ///<param name="e"></param>
    protected void PageDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hidPageNo.Value = SetPageForGrid(grdvwClearedCash);
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

    ///<summary>
    ///This event is used to save Cash payments which are cleared.
    ///</summary>
    ///<param name="sender"></param>
    ///<param name="e"></param>
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
            FillPaymnetsGrid();
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
            (ex.Message + Constants.S_TRACE + ex.StackTrace,
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
            Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    ///<summary>
    ///This event is used to fill footer dropdown list in the grid.
    ///</summary>
    ///<param name="sender"></param>
    ///<param name="e"></param>
    protected void grdvwClearedCash_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            SetDataPagerOfGrid(sender, e, grdvwClearedCash);
            lblStartIndex.Text = StartIndex;
            lblEndIndex.Text = EndIndex;
            lblTotal.Text = Total;
            trTotalRec.Visible = ShowTotalRecords;
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
            (ex.Message + Constants.S_TRACE + ex.StackTrace,
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
            Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    ///<summary>
    ///This event is used to set grid according to selected page in the footer drop down list of grid.
    ///</summary>
    ///<param name="sender"></param>
    ///<param name="e"></param>
    protected void grdvwClearedCash_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            SetNewPageIndex(sender, e, grdvwClearedCash);
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

    ///<summary>
    ///This method is used to fill GridView.
    ///</summary>
    public override void FillPaymnetsGrid()
    {
        const int FILTER_BY_STUDENT_NAME_REG_NO = 1;
        const int FILTER_BY_PAID_DATE = 2;
        const int FILTER_BY_CLEARANCE_DATE = 3;
        DataTable odtClearedCash = null;
        int iSchoolId = Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]);
        int iAcademicYearId = Convert.ToInt32(Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID]);
        int iTotalAmount = 0;
        if (this.ClearanceListFilters.RegNoChecked == true)
            odtClearedCash = CashClearanceListBL.FetchClearedCashDetails(this.ClearanceListFilters.StudentNameOrRegNo.Trim(), string.Empty, string.Empty, this.ClearanceListFilters.IncludeAll, FILTER_BY_STUDENT_NAME_REG_NO, iSchoolId, iAcademicYearId, out iTotalAmount);
        else if (this.ClearanceListFilters.PaymentDateChecked == true)
            odtClearedCash = CashClearanceListBL.FetchClearedCashDetails(null, this.ClearanceListFilters.PaymentStartDate.ToString().Trim(), this.ClearanceListFilters.PaymentEndDate.ToString().Trim(), this.ClearanceListFilters.IncludeAll, FILTER_BY_PAID_DATE, iSchoolId, iAcademicYearId, out iTotalAmount);
        else if (this.ClearanceListFilters.ClearanceDateChecked == true)
            odtClearedCash = CashClearanceListBL.FetchClearedCashDetails(null, this.ClearanceListFilters.ClearanceStartDate.ToString().Trim(), this.ClearanceListFilters.ClearanceEndDate.ToString().Trim(), this.ClearanceListFilters.IncludeAll, FILTER_BY_CLEARANCE_DATE, iSchoolId, iAcademicYearId, out iTotalAmount);
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
            grdvwClearedCash.Visible = false;
        }
    }

    /// <summary>
    /// this method is used hide error message.
    /// </summary>
    private void InvisibleErrorMessage()
    {
        trTotalRec.Visible = false;
        lblError.Visible = false;
    }

   

    ///<summary>
    ///<summary>
    ///This method is used to collect paramters and send it to Stored procedure.
    ///</summary>
    ///<returns></returns>
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
    /// This method is used to initialize controls.
    /// </summary>
    private void IntializeControls()
    {
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        valSave.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        grdvwClearedCash.PageSize = Constants.I_GRID_PAGE_COUNT;
        hidPageNo.Value = HIDE_PAGE_NUMBER;
    }

    public override List<Control> AddControls()
    {
        List<Control> olstControls = new List<Control>();
        olstControls.Add(btnShow);
        olstControls.Add(btnSave);
        olstControls.Add(btnExport);
        olstControls.Add(RegNoRadioButton);
        olstControls.Add(ClearanceListFilters.PaymentDateRadioButton);
        olstControls.Add(ClearanceListFilters.ClearanceDateRadioButton);
        return olstControls;
    }

    #endregion

    #region "Public Methods"

    /// <summary>
    /// This method exposes the functionality to check the Radio button of Reg.No. from the user control of filters.
    /// </summary>
    public void optRegNoChecked()
    {
        RegNoRadioButton.Checked = true;
        ClearanceListFilters.PaymentDateRadioButton.Checked = false;
        ClearanceListFilters.ClearanceDateRadioButton.Checked = false;
        ClearanceListFilters.optRegNoChecked();
    }

    /// <summary>
    /// This method exposes the functionality of showing the grid depending on the caption of the show button.
    /// </summary>
    public override void ShowPaymnetsGrid()
    {
        lblError.Visible = false;
        lblSuccessMsg.Visible = false;
        lblSuccessMsg.Text = "";

        if (btnShow.Text == "Show")
        {
            hidPageNo.Value = HIDE_PAGE_NUMBER;
            grdvwClearedCash.PageIndex = Constants.I_ZERO;
            FillPaymnetsGrid();
            btnShow.Text = "Change Input";
            this.ClearanceListFilters.EnableDisableControlChecked(false);
            this.ClearanceListFilters.EnableDisableControls(false);
        }
        else
        {
            btnShow.Text = "Show";
            this.ClearanceListFilters.EnableDisableControlChecked(true);
            this.ClearanceListFilters.EnableDisableControls(true);
            grdvwClearedCash.DataSource = null;
            grdvwClearedCash.DataBind();
            grdvwClearedCash.Visible = false;
            trTotalRec.Visible = false;
            tblTotalAmount.Visible = false;
        }
    }

    #endregion

    #region Functionality For Export

    ///<summary>
    ///This event is used to export the cheque clearance details in the Excel sheet.
    ///</summary>
    ///<param name="sender"></param>
    ///<param name="e"></param>
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

    ///<summary>
    ///This method generates the report filter as per the field selection.
    ///</summary>
    ///<returns></returns>
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

        if (this.ClearanceListFilters.PaymentDateChecked)
        {
            sSchoolYearFilter = "(" + sViewNameSchID + "=" + iSchoolID + " AND " +
                                                           sViewNameAcdYearId + "=" + iAcadYearID +
                                                            " AND " +
                                                            sViewNamePaymentStartDate + "=" + (this.ClearanceListFilters.PaymentStartDate.ToString().Trim() == string.Empty ? "null" : this.ClearanceListFilters.PaymentStartDate.ToString().Trim()) + " AND " +
                                      sViewNamePaymentEndDate + "=" + (this.ClearanceListFilters.PaymentEndDate.ToString().Trim() == string.Empty ? "null" : this.ClearanceListFilters.PaymentEndDate.ToString().Trim()) + " AND " +
                                                                            sViewNameRegNo + " =null AND " +
                                      sViewNameIncldeCheck + "=" + this.ClearanceListFilters.IncludeAll.ToString() + ")";

        }
        else if (this.ClearanceListFilters.RegNoChecked)
        {
            sSchoolYearFilter = "(" + sViewNameSchID + "=" + iSchoolID + " AND " +
                                                           sViewNameAcdYearId + "=" + iAcadYearID +
                                                            " AND " +
                                      sViewNameRegNo + "=" + (this.ClearanceListFilters.StudentNameOrRegNo.Trim() == string.Empty ? "null" : this.ClearanceListFilters.StudentNameOrRegNo.Trim()) + " AND " +
                                      sViewNamePaymentStartDate + "= null AND " +
                                      sViewNamePaymentEndDate + "= null AND " +
                                      sViewNameClearanceStartDate + "= null AND " +
                                      sViewNameClearanceEndDate + "= null AND " +
                                      sViewNameIncldeCheck + "=" + this.ClearanceListFilters.IncludeAll.ToString() + ")";
        }
        else if (this.ClearanceListFilters.ClearanceDateChecked)
        {
            sSchoolYearFilter = "(" + sViewNameSchID + "=" + iSchoolID + " AND " +
                                                          sViewNameAcdYearId + "=" + iAcadYearID +
                                                           " AND " +
                                     sViewNameRegNo + "=" + (this.ClearanceListFilters.StudentNameOrRegNo.Trim() == string.Empty ? "null" : this.ClearanceListFilters.StudentNameOrRegNo.Trim()) + " AND " +
                                     sViewNamePaymentStartDate + "= null AND " +
                                     sViewNamePaymentEndDate + "= null AND " +
                                     sViewNameClearanceStartDate + "=" + (this.ClearanceListFilters.ClearanceStartDate.ToString().Trim() == string.Empty ? "null" : this.ClearanceListFilters.ClearanceStartDate.ToString().Trim()) + " AND " +
                                     sViewNameClearanceEndDate + "=" + (this.ClearanceListFilters.ClearanceEndDate.ToString().Trim() == string.Empty ? "null" : this.ClearanceListFilters.ClearanceEndDate.ToString().Trim()) + " AND " +
                                     sViewNameIncldeCheck + "=" + this.ClearanceListFilters.IncludeAll.ToString() + ")";
        }

        return sSchoolYearFilter + "@ ";
    }

    #endregion

}

