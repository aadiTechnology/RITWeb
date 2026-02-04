using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using Utility;

namespace SchoolWebApp
{
    public partial class SwipeCardClearanceListUI : PaymentClearanceUC
    {
        #region "Constants and Data Members"

        const int I_COLUMN_INDEX_CLEARANCE_DATETIME = 5;
        const string HIDE_PAGE_NUMBER = "1";

        int miTotalAmount;

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

        /// <summary>
        /// This event is used initialise controls and set client side attributes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                InitializeControls();
                SetClientScriptAttribute(AddControls());
                HideButtons(btnSave, btnExport);
            }
        }

        /// <summary>
        /// This event is used to fill  grid according to filter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// This event is used to set grid according to selected page in the footer drop down list of grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PageDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                hidPageNo.Value = SetPageForGrid(grdvwCardPayments);
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
        /// This event is used to save payments which are cleared.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveCardPayments();
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
        protected void grdvwCardPayments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                SetDataPagerOfGrid(sender, e, grdvwCardPayments);
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

        /// <summary>
        /// This event is used to set grid according to selected page in the footer drop down list of grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdvwCardPayments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                SetNewPageIndex(sender, e, grdvwCardPayments);
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
        /// This method is used to fill Swipe card type  combo box.
        /// </summary>
        private void FillCardTypeCombo()
        {
            SchoolwiseBankMasterBL oSchoolwiseBankMasterBL = new SchoolwiseBankMasterBL();
            DataTable oDT = oSchoolwiseBankMasterBL.GetSchoolwiseCardTypeList(Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]));
            ControlUtility.FillDropDownList(oDT, ref cmbCardType, "CardTypeId", "CardType", Constants.S_SELECT_ALL);
        }

        ///<summary>
        ///This method is used to fill GridView.
        ///</summary>
        public override void FillPaymnetsGrid()
        {

            CashClearanceListBL oCashClearanceListBL = new CashClearanceListBL();
            DataTable oDt = oCashClearanceListBL.GetCardPaymentList(Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]), Convert.ToInt32(Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID]), ClearanceListFilters.StudentNameOrRegNo,
                                                                    ClearanceListFilters.PaymentStartDate.ToString(), ClearanceListFilters.PaymentEndDate.ToString(),
                                                                    ClearanceListFilters.ClearanceStartDate.ToString(), ClearanceListFilters.ClearanceEndDate.ToString(),
                                                                    ClearanceListFilters.IncludeAll, Convert.ToInt32(cmbCardType.SelectedValue == "" ? "0" : cmbCardType.SelectedValue));
            grdvwCardPayments.Visible = true;
            grdvwCardPayments.DataSource = oDt.DefaultView;
            grdvwCardPayments.DataBind();
            hidRowCnt.Value = Convert.ToString(grdvwCardPayments.Rows.Count);

            miTotalAmount = oCashClearanceListBL.CardPaymentsTotalAmount(Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]), Convert.ToInt32(Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID]), ClearanceListFilters.StudentNameOrRegNo, ClearanceListFilters.PaymentStartDate.ToString(), ClearanceListFilters.PaymentEndDate.ToString(),
                                                                         ClearanceListFilters.ClearanceStartDate.ToString(), ClearanceListFilters.ClearanceEndDate.ToString(), ClearanceListFilters.IncludeAll, Convert.ToInt32(cmbCardType.SelectedValue == "" ? "0" : cmbCardType.SelectedValue));

            if (miTotalAmount != 0)
            {
                tblTotalAmount.Visible = true;
                lblTotalAmount.Text = miTotalAmount.ToString();
            }
            else
                tblTotalAmount.Visible = false;
        }

        /// <summary>
        /// this method is used hide error message.
        /// </summary>
        private void InvisibleErrorMessage()
        {
            trTotalRec.Visible = false;
            lblError.Visible = false;
        }

        /// <summary>
        /// This method is used to Initialize controls.
        /// </summary>
        private void InitializeControls()
        {
            valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
            valSave.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
            grdvwCardPayments.PageSize = Constants.I_GRID_PAGE_COUNT;
            hidPageNo.Value = HIDE_PAGE_NUMBER;
            FillCardTypeCombo();
        }

        /// <summary>
        /// This method is used to collect paramters and send it to Stored procedure.
        /// </summary>
        /// <returns></returns>
        private string GenerateCardPaymentXML()
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

        public override List<Control> AddControls()
        {
            List<Control> olstControls = new List<Control>();
            olstControls.Add(btnShow);
            olstControls.Add(btnSave);
            olstControls.Add(btnBack);
            olstControls.Add(btnExport);
            olstControls.Add(ClearanceListFilters.RegNoRadioButton);
            olstControls.Add(ClearanceListFilters.PaymentDateRadioButton);
            olstControls.Add(ClearanceListFilters.ClearanceDateRadioButton);
            return olstControls;
        }

        /// <summary>
        /// This method is used to save card payments.
        /// </summary>
        private void SaveCardPayments()
        {
            string sXML = GenerateCardPaymentXML();
            CashClearanceListBL.UpdateCardPaymentsDetails(sXML);
            lblError.Visible = false;
            lblSuccessMsg.Visible = true;
            lblSuccessMsg.Text = "Swipe Card Clearance data updated successfully !!!";
            FillPaymnetsGrid();
        }

        #endregion

        #region "Public"

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
            if (btnShow.Text == "Show")
            {
                hidPageNo.Value = HIDE_PAGE_NUMBER;
                grdvwCardPayments.PageIndex = Constants.I_ZERO;
                FillPaymnetsGrid();
                btnShow.Text = "Change Input";
                ClearanceListFilters.EnableDisableControlChecked(false);
                ClearanceListFilters.EnableDisableControls(false);
            }
            else
            {
                btnShow.Text = "Show";
                ClearanceListFilters.EnableDisableControlChecked(true);
                ClearanceListFilters.EnableDisableControls(true);
                grdvwCardPayments.DataSource = null;
                grdvwCardPayments.DataBind();
                grdvwCardPayments.Visible = false;
                trTotalRec.Visible = false;
                tblTotalAmount.Visible = false;
                lblError.Visible = false;
            }
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
                ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.CardPaymentDetails, GetCardClearanceFilterString());
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
        private string GetCardClearanceFilterString()
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

            sSchoolYearFilter = sViewNameSchoolId + "=" + iSchoolID + " AND " + sViewNameAcademic_Year_Id + "=" + iAcadYearID;

            if (ClearanceListFilters.StudentNameOrRegNo != null && ClearanceListFilters.StudentNameOrRegNo != "")
                sSchoolYearFilter = sSchoolYearFilter + " AND " + sViewNameRegNo + "=" + ClearanceListFilters.StudentNameOrRegNo.Trim();
            else
                sSchoolYearFilter = sSchoolYearFilter + " AND " + sViewNameRegNo + "= null";


            if (ClearanceListFilters.PaymentStartDate != "")
                sSchoolYearFilter = sSchoolYearFilter + " AND " + sViewNamePaymentStartDate + "=" + ClearanceListFilters.PaymentStartDate.ToString().Trim();
            else
                sSchoolYearFilter = sSchoolYearFilter + " AND " + sViewNamePaymentStartDate + "= null";

            if (ClearanceListFilters.PaymentEndDate != "")
                sSchoolYearFilter = sSchoolYearFilter + " AND " + sViewNamePaymentEndDate + "=" + ClearanceListFilters.PaymentEndDate.ToString().Trim();
            else
                sSchoolYearFilter = sSchoolYearFilter + " AND " + sViewNamePaymentEndDate + "= null";

            if (ClearanceListFilters.ClearanceStartDate != "")
                sSchoolYearFilter = sSchoolYearFilter + " AND " + sViewNameClearanceStartDate + "=" + ClearanceListFilters.ClearanceStartDate.ToString().Trim();
            else
                sSchoolYearFilter = sSchoolYearFilter + " AND " + sViewNameClearanceStartDate + "= null";

            if (ClearanceListFilters.ClearanceEndDate != "")
                sSchoolYearFilter = sSchoolYearFilter + " AND " + sViewNameClearanceEndDate + "=" + ClearanceListFilters.ClearanceEndDate.ToString().Trim();
            else
                sSchoolYearFilter = sSchoolYearFilter + " AND " + sViewNameClearanceEndDate + "= null";

            sSchoolYearFilter = sSchoolYearFilter + " AND " + sViewNameIncludeAll + "=" + (ClearanceListFilters.IncludeAll ? "1" : "0");

            sSchoolYearFilter = sSchoolYearFilter + " AND " + sViewNameCardTypeId + "=" + cmbCardType.SelectedValue;

            return "(" + sSchoolYearFilter + ")@ ";
        }

        #endregion

    }
}