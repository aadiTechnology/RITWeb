using System;
using System.Data;
using System.Threading;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using Utility;

namespace SchoolWebApp
{
    public partial class ChequeClearanceListUI : PaymentClearanceUC
    {
        #region "Constants"

        const int I_COLUMN_INDEX_CLEARANCE_DATETIME = 7;
        const int I_COLUMN_INDEX_CHEQUE_NUMBER = 2;

        const string HIDE_PAGE_NUMBER = "1";


        #endregion

        #region "Properties"

        public RadioButton RadioButtonChequeNo
        {
            get { return optChequeNumber; }
        }

        public Button ShowButton
        {
            get { return btnShow; }
        }

        #endregion

        #region Events

        /// <summary>
        /// This method is used to initialize the controls and set the handler to the event from the filters users control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    InitializeControls();
                    SetClientScriptAttribute(AddControls());
                    HideButtons(btnSave, btnExport);
                }
                ClearanceListFilters.OnClearanceFiltersChanged += new EventHandler(ClearanceListFilters_ClearanceFiltersChanged);
                tblLegend.Visible = false;
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
        /// This event is used uncheck the cheque number radio button when one of the radio buttons from the filters user control is checked. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ClearanceListFilters_ClearanceFiltersChanged(object sender, EventArgs e)
        {
            try
            {
                optChequeNumber.Checked = false;
                txtChequeNumber.Text = "";
                txtChequeNumber.Enabled = false;
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
                hidPageNo.Value = SetPageForGrid(grdCheques);
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
                SaveChequePayments();
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
        /// This event is used to set the query string and to fill footer drop down list in the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdCheques_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                SetDataPagerOfGrid(sender, e, grdCheques);
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
        /// This event is used to set filter of cheque number.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void optChequeNumber_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                SetChequeNumberRadioButton();
            }
            catch (Exception Ex)
            {
                BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
                (Ex.Message + Constants.S_TRACE + Ex.StackTrace,
                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
                Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
            }
        }

        /// <summary>
        /// This event is used to set grid according to selected page in the footer drop down list of grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdCheques_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                SetNewPageIndex(sender, e, grdCheques);
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
        /// This method used to fill the grid according to selected filter.
        /// </summary>
        public override void FillPaymnetsGrid()
        {
            lblError.Visible = false;
            int iSchoolId = Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]);
            int iAcaYearId = Convert.ToInt32(Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID]);
            DataTable oDTCheques = null;
            string sRegNo = this.ClearanceListFilters.StudentNameOrRegNo.Trim();
            string sChequeNo = txtChequeNumber.Text.Trim();
            int TotalAmount = 0;

            if (!sRegNo.Equals(""))
                oDTCheques = StudenChequesCollectionBL.FetchChequesDetails(sRegNo, iSchoolId, iAcaYearId, this.ClearanceListFilters.IncludeAll, chkCautionMoney.Checked, false, out TotalAmount);
            else if (!sChequeNo.Equals(""))
                oDTCheques = StudenChequesCollectionBL.FetchChequesDetails(sChequeNo, iSchoolId, iAcaYearId, this.ClearanceListFilters.IncludeAll, chkCautionMoney.Checked, true, out TotalAmount);
            else if (!(ClearanceListFilters.PaymentStartDate == "") || !(ClearanceListFilters.PaymentEndDate == ""))
            {
                DateTime odtStartDate = ClearanceListFilters.PaymentStartDate == "" ? DateTime.MinValue : Convert.ToDateTime(ClearanceListFilters.PaymentStartDate);
                DateTime odtToDate = ClearanceListFilters.PaymentEndDate == "" ? DateTime.MinValue : Convert.ToDateTime(ClearanceListFilters.PaymentEndDate);
                oDTCheques = StudenChequesCollectionBL.FetchChequesDetails(odtStartDate, odtToDate, iSchoolId, iAcaYearId, this.ClearanceListFilters.IncludeAll, chkCautionMoney.Checked, true, out TotalAmount);
            }
            else if (!ClearanceListFilters.ClearanceStartDate.Equals("") || !ClearanceListFilters.ClearanceEndDate.Equals(""))
            {
                DateTime odtStartDate = ClearanceListFilters.ClearanceStartDate.Equals("") ? DateTime.MinValue : Convert.ToDateTime(ClearanceListFilters.ClearanceStartDate);
                DateTime odtEndDate = ClearanceListFilters.ClearanceEndDate.Equals("") ? DateTime.MinValue : Convert.ToDateTime(ClearanceListFilters.ClearanceEndDate);
                oDTCheques = StudenChequesCollectionBL.FetchChequesDetails(odtStartDate, odtEndDate, iSchoolId, iAcaYearId, this.ClearanceListFilters.IncludeAll, chkCautionMoney.Checked, false, out TotalAmount);
            }
            else
                oDTCheques = StudenChequesCollectionBL.FetchChequesDetails(iSchoolId, iAcaYearId, this.ClearanceListFilters.IncludeAll, chkCautionMoney.Checked, out TotalAmount);

            if (oDTCheques != null)
            {
                grdCheques.Visible = true;
                if (chkCautionMoney.Checked == true)
                    tblLegend.Visible = true;
                else
                    tblLegend.Visible = false;
                SetGridViewDateColumnProperties();
                grdCheques.DataSource = oDTCheques.DefaultView;
                grdCheques.DataBind();
                hidRowCnt.Value = Convert.ToString(grdCheques.Rows.Count);
                tblTotalAmount.Visible = true;
                lblTotalAmount.Text = TotalAmount.ToString();
            }
            if (oDTCheques.Rows.Count == 0)
            {
                trTotalRec.Visible = false;
                tblLegend.Visible = false;
                tblTotalAmount.Visible = false;
            }
        }

        /// <summary>
        /// This method is used to display payment date in proper formate.
        /// </summary>
        private void SetGridViewDateColumnProperties()
        {
            const Int32 I_COLUMN_INDEX_PAYMENT_DATE = 7;
            BoundField oPaymentDate = (BoundField)grdCheques.Columns[I_COLUMN_INDEX_PAYMENT_DATE];
            oPaymentDate.HtmlEncode = false;
            oPaymentDate.DataFormatString = Constants.S_STANDARD_GRID_DATE_FORMAT;
        }

        /// <summary>
        /// This method is used to hide the error message.
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
            grdCheques.PageSize = Constants.I_GRID_PAGE_COUNT;
            hidPageNo.Value = HIDE_PAGE_NUMBER;
            hidServerDate.Value = System.DateTime.Today.ToString();
            optChequeNumber.Checked = true;
            optChequeNumber_CheckedChanged(null, null);

        }

        public override List<Control> AddControls()
        {
            List<Control> olstControls = new List<Control>();
            olstControls.Add(btnShow);
            olstControls.Add(btnSave);
            olstControls.Add(btnBack);
            olstControls.Add(btnExport);
            olstControls.Add(optChequeNumber);
            olstControls.Add(ClearanceListFilters.RegNoRadioButton);
            olstControls.Add(ClearanceListFilters.PaymentDateRadioButton);
            olstControls.Add(ClearanceListFilters.ClearanceDateRadioButton);
            return olstControls;
        }

        /// <summary>
        ///  This XML is used to set the parameters to clear cheque detais.
        /// </summary>
        /// <returns></returns>
        private string GenerateChequePaymentXML()
        {
            const string S_ELEMENT = "element";
            string sAttribute;
            XmlDocument oDoc = new XmlDocument();
            // Create a root level element.
            XmlElement oRoot = oDoc.CreateElement("StudentInfo");
            XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "StudentInfo", "");
            // Loop through all the grid rows.

            for (int i = 0; i < grdCheques.Rows.Count; i++)
            {
                TextBox otxtClearanceDate = (TextBox)grdCheques.Rows[i].Cells[I_COLUMN_INDEX_CLEARANCE_DATETIME].FindControl("txtclearance");
                TextBox otxtChequeNo = (TextBox)grdCheques.Rows[i].Cells[I_COLUMN_INDEX_CHEQUE_NUMBER].FindControl("txtChequeNo");
                TextBox otxtChequeDate = (TextBox)grdCheques.Rows[i].Cells[I_COLUMN_INDEX_CLEARANCE_DATETIME].FindControl("txtChequeDate");

                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "StudentInfo", "");

                sAttribute = "BankId";
                XmlAttribute oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = grdCheques.DataKeys[i]["Bank_Id"].ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "ChequeNo";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = otxtChequeNo.Text.Trim();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "Student_Id";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = grdCheques.DataKeys[i]["Student_Id"].ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "PostDated_Cheque_Id";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = grdCheques.DataKeys[i]["PostDated_Cheque_Id"].ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "Update_Date";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = System.DateTime.Now.ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "Updated_By_Id";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = Session[Constants.S_SESSION_USER_ID].ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "Cheque_Date";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = otxtChequeDate.Text.Trim();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "Cheque_Passed_Date";
                oAttr = oDoc.CreateAttribute(sAttribute);
                if (otxtClearanceDate.Text.Trim() != "")
                    oAttr.Value = otxtClearanceDate.Text.Trim();
                else
                    oAttr.Value = DBNull.Value.ToString();
                oXmlNode.Attributes.Append(oAttr);

                // Add the node to root node.
                oXmlRootNode.AppendChild(oXmlNode);

            }

            // Add the root node to document element.         
            oRoot.AppendChild(oXmlRootNode);
            // return the string generated.
            return oRoot.InnerXml;
        }

        /// <summary>
        ///  This XML is used to set the parameters to clear cheque and caution money details.
        /// </summary>
        /// <returns></returns>
        private string GenerateCautionMoneyXML()
        {
            const string S_ELEMENT = "element";
            string sAttribute;
            XmlDocument oDoc = new XmlDocument();
            XmlElement oRoot = oDoc.CreateElement("StudentInfo");
            XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "StudentInfo", "");
            for (int i = 0; i < grdCheques.Rows.Count; i++)
            {
                TextBox otxtClearanceDate = (TextBox)grdCheques.Rows[i].Cells[I_COLUMN_INDEX_CLEARANCE_DATETIME].FindControl("txtclearance");
                TextBox otxtChequeNo = (TextBox)grdCheques.Rows[i].Cells[I_COLUMN_INDEX_CHEQUE_NUMBER].FindControl("txtChequeNo");
                TextBox otxtChequeDate = (TextBox)grdCheques.Rows[i].Cells[I_COLUMN_INDEX_CLEARANCE_DATETIME].FindControl("txtChequeDate");

                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "StudentInfo", "");

                sAttribute = "BankId";
                XmlAttribute oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = grdCheques.DataKeys[i]["Bank_Id"].ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "ChequeNo";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = otxtChequeNo.Text.Trim();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "Student_Id";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = grdCheques.DataKeys[i]["Student_Id"].ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "PostDated_Cheque_Id";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = grdCheques.DataKeys[i]["PostDated_Cheque_Id"].ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "Payment_Cheque_Id";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = grdCheques.DataKeys[i]["Payment_Cheque_Id"].ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "Insert_Date";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = System.DateTime.Now.ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "Inserted_By_Id";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = Session[Constants.S_SESSION_USER_ID].ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "Update_Date";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = System.DateTime.Now.ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "Updated_By_Id";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = Session[Constants.S_SESSION_USER_ID].ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "Cheque_Date";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = otxtChequeDate.Text.Trim();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "ClearanceDate";
                oAttr = oDoc.CreateAttribute(sAttribute);
                if (otxtClearanceDate.Text.Trim() != "")
                    oAttr.Value = otxtClearanceDate.Text.Trim();
                else
                    oAttr.Value = DBNull.Value.ToString();
                oXmlNode.Attributes.Append(oAttr);

                oXmlRootNode.AppendChild(oXmlNode);
            }
            oRoot.AppendChild(oXmlRootNode);
            // return the string generated.
            return oRoot.InnerXml;
        }

        /// <summary>
        /// This method is used save cleared cheque payments. 
        /// </summary>
        private void SaveChequePayments()
        {
            if (chkCautionMoney.Checked == false)
            {
                string sXML = GenerateChequePaymentXML();
                StudentPostDatedChequesBL oUpdateStudentPostDatedCheques = new StudentPostDatedChequesBL();
                DataTable oDTMessage = StudentPostDatedChequesBL.IsDuplicateChequeNo(sXML);
                if (oDTMessage == null || !(oDTMessage.Rows.Count > 0) || oDTMessage.Rows[0][0] == null)
                {
                    oUpdateStudentPostDatedCheques.SetChequeClearanceDate(sXML);
                    lblError.Visible = false;
                    lblSuccessMsg.Visible = true;
                    lblSuccessMsg.Text = "Cheque Clearance data updated successfully !!!";
                    FillPaymnetsGrid();
                }
                else
                {
                    string sMessage = string.Empty;
                    for (int i = 0; i < oDTMessage.Rows.Count; i++)
                    {
                        sMessage = sMessage + ", " + Convert.ToString(oDTMessage.Rows[i]["Name"]);
                    }
                    lblError.Visible = true;
                    lblSuccessMsg.Visible = false;
                    lblError.Text = "Cheque Number already exists for student(s) " + sMessage.Substring(1, sMessage.Length - 1) + ".";
                }
            }
            else
            {
                string sXML = GenerateCautionMoneyXML();
                StudentPostDatedChequesBL oUpdateStudentPostDatedCheques = new StudentPostDatedChequesBL();
                DataTable oDTMessage = StudentPostDatedChequesBL.IsDuplicateChequeNo(sXML);
                if (oDTMessage == null || !(oDTMessage.Rows.Count > 0) || oDTMessage.Rows[0][0] == null)
                {
                    oUpdateStudentPostDatedCheques.SetCautionClearanceDate(sXML);
                    lblError.Visible = false;
                    lblSuccessMsg.Visible = true;
                    lblSuccessMsg.Text = "Cheque Clearance data updated successfully !!!";
                    FillPaymnetsGrid();
                }
                else
                {
                    string sMessage = string.Empty;
                    for (int i = 0; i < oDTMessage.Rows.Count; i++)
                    {
                        sMessage = sMessage + ", " + Convert.ToString(oDTMessage.Rows[i]["Name"]);
                    }
                    lblError.Visible = true;
                    lblSuccessMsg.Visible = false;
                    lblError.Text = "Cheque Number already exists for student(s) " + sMessage.Substring(1, sMessage.Length - 1) + ".";
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method exposes the functionality of showing the grid depending on the caption of the show button.
        /// </summary>
        public override void ShowPaymnetsGrid()
        {
            if (btnShow.Text == "Show")
            {
                FillPaymnetsGrid();
                btnShow.Text = "Change Input";
                optChequeNumber.Enabled = false;
                txtChequeNumber.Enabled = false;
                chkCautionMoney.Enabled = false;
                this.ClearanceListFilters.EnableDisableControlChecked(false);
                this.ClearanceListFilters.EnableDisableControls(false);
            }
            else
            {
                btnShow.Text = "Show";
                optChequeNumber.Enabled = true;
                txtChequeNumber.Enabled = true;
                chkCautionMoney.Enabled = true;
                this.ClearanceListFilters.EnableDisableControlChecked(true);
                this.ClearanceListFilters.EnableDisableControls(true);
                grdCheques.DataSource = null;
                grdCheques.DataBind();
                grdCheques.Visible = false;
                tblLegend.Visible = false;
                trTotalRec.Visible = false;
                tblTotalAmount.Visible = false;
                lblError.Visible = false;

            }
        }

        /// <summary>
        /// This method exposes the functionality to check the Radio button of cheque number.
        /// </summary>
        public void SetChequeNumberRadioButton()
        {
            txtChequeNumber.Enabled = true;
            InvisibleErrorMessage();
            ClearanceListFilters.RegNoRadioButton.Checked = !optChequeNumber.Checked;
            ClearanceListFilters.PaymentDateRadioButton.Checked = !optChequeNumber.Checked;
            ClearanceListFilters.ClearanceDateRadioButton.Checked = !optChequeNumber.Checked;
            ClearanceListFilters.DisableAll();
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
                ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.ChequeClearanceDetails, GetFilterChequePaymentString());
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
        private string GetFilterChequePaymentString()
        {
            string sSchoolYearFilter = "";
            int iSchoolID = Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]);
            int iAcadYearID = Convert.ToInt32(Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID]);
            string sViewNameSchID = Constants.S_EXPORT_CHEQUECLEARANCE_USP + ".iSchoolId}";
            string sViewNameAcdYearId = Constants.S_EXPORT_CHEQUECLEARANCE_USP + ".iAcademicYrId}";
            string sViewNameIsChequeClear = Constants.S_EXPORT_CHEQUECLEARANCE_USP + ".IsChequeClear}";
            string sViewNameChequeNo = Constants.S_EXPORT_CHEQUECLEARANCE_USP + ".ChequeNo}";
            string sViewNameStartDate = Constants.S_EXPORT_CHEQUECLEARANCE_USP + ".StartDate}";
            string sViewNameEndDate = Constants.S_EXPORT_CHEQUECLEARANCE_USP + ".EndDate}";
            string sViewNameRegNo = Constants.S_EXPORT_CHEQUECLEARANCE_USP + ".RegNo}";
            string sViewNameIsChequeClearanceDate = Constants.S_EXPORT_CHEQUECLEARANCE_USP + ".IsChequeClearanceDate}";
            string sViewNameIsCautionClear = Constants.S_EXPORT_CHEQUECLEARANCE_USP + ".IsCautionClearanceDate}";
            if (optChequeNumber.Checked)
            {
                sSchoolYearFilter = "(" + sViewNameSchID + "=" + iSchoolID + " AND " +
                                                               sViewNameAcdYearId + "=" + iAcadYearID +
                                                                " AND " +
                                          sViewNameChequeNo + "=" + txtChequeNumber.Text.Trim() + " AND " +
                                          sViewNameRegNo + "= AND " +
                                          sViewNameStartDate + "= null " + " AND " +
                                          sViewNameEndDate + "= null" + "  AND " +
                                          sViewNameIsChequeClear + "=" + this.ClearanceListFilters.IncludeAll.ToString() + "AND " +
                                          sViewNameIsChequeClearanceDate + "=" + this.ClearanceListFilters.ClearanceDateChecked.ToString() + "AND " +
                                          sViewNameIsCautionClear + "=" + chkCautionMoney.Checked.ToString() + ")";

            }
            else if (this.ClearanceListFilters.PaymentDateChecked)
            {
                sSchoolYearFilter = "(" + sViewNameSchID + "=" + iSchoolID + " AND " +
                                                               sViewNameAcdYearId + "=" + iAcadYearID +
                                                                " AND " +
                                          sViewNameStartDate + "=" + (this.ClearanceListFilters.PaymentStartDate.ToString() == string.Empty ? "null" : this.ClearanceListFilters.PaymentStartDate.ToString().Trim()) + " AND " +
                                          sViewNameEndDate + "=" + (this.ClearanceListFilters.PaymentEndDate.ToString().Trim() == string.Empty ? "null" : this.ClearanceListFilters.PaymentEndDate.ToString().Trim()) + " AND " +
                                          sViewNameChequeNo + "=  AND " +
                                          sViewNameRegNo + "= AND " +
                                          sViewNameIsChequeClear + "=" + this.ClearanceListFilters.IncludeAll.ToString() + "AND " +
                                           sViewNameIsChequeClearanceDate + "=" + this.ClearanceListFilters.ClearanceDateChecked.ToString() + "AND " +
                                          sViewNameIsCautionClear + "=" + chkCautionMoney.Checked.ToString() + ")";

            }
            else if (this.ClearanceListFilters.RegNoChecked)
            {
                sSchoolYearFilter = "(" + sViewNameSchID + "=" + iSchoolID + " AND " +
                                                               sViewNameAcdYearId + "=" + iAcadYearID +
                                                                " AND " +
                                          sViewNameRegNo + "=" + this.ClearanceListFilters.StudentNameOrRegNo.Trim() + " AND " +
                                          sViewNameStartDate + "= null" + " AND " +
                                          sViewNameEndDate + "= null" + " AND " +
                                          sViewNameChequeNo + "= AND " +
                                          sViewNameIsChequeClear + "=" + this.ClearanceListFilters.IncludeAll.ToString() + "AND " +
                                          sViewNameIsChequeClearanceDate + "=" + this.ClearanceListFilters.ClearanceDateChecked.ToString() + "AND " +
                                          sViewNameIsCautionClear + "=" + chkCautionMoney.Checked.ToString() + ")";
            }
            else if (this.ClearanceListFilters.ClearanceDateChecked)
            {
                sSchoolYearFilter = "(" + sViewNameSchID + "=" + iSchoolID + " AND " +
                                                               sViewNameAcdYearId + "=" + iAcadYearID +
                                                                " AND " +
                                          sViewNameStartDate + "=" + (this.ClearanceListFilters.ClearanceStartDate.ToString().Trim() == string.Empty ? "null" : this.ClearanceListFilters.ClearanceStartDate.ToString().Trim()) + " AND " +
                                          sViewNameEndDate + "=" + (this.ClearanceListFilters.ClearanceEndDate.ToString().Trim() == string.Empty ? "null" : this.ClearanceListFilters.ClearanceEndDate.ToString().Trim()) + " AND " +
                                          sViewNameChequeNo + "=  AND " +
                                          sViewNameRegNo + "= AND " +
                                          sViewNameIsChequeClear + "=" + this.ClearanceListFilters.IncludeAll.ToString() + "AND " +
                                          sViewNameIsChequeClearanceDate + "=" + this.ClearanceListFilters.ClearanceDateChecked.ToString() + "AND " +
                                          sViewNameIsCautionClear + "=" + chkCautionMoney.Checked.ToString() + ")";
            }
            else if (this.ClearanceListFilters.StudentNameOrRegNo.Trim() == "" && this.ClearanceListFilters.PaymentStartDate.ToString() == "" && this.ClearanceListFilters.PaymentEndDate.ToString() == ""
                       && this.ClearanceListFilters.ClearanceStartDate.ToString().Trim() == "" && this.ClearanceListFilters.ClearanceEndDate.ToString().Trim() == "" && txtChequeNumber.Text == "")
            {
                sSchoolYearFilter = "(" + sViewNameSchID + "=" + iSchoolID + " AND " +
                                                               sViewNameAcdYearId + "=" + iAcadYearID +
                                                                " AND " +
                                          sViewNameRegNo + "= AND " +
                                          sViewNameStartDate + "= null" + " AND " +
                                          sViewNameEndDate + "= null" + " AND " +
                                          sViewNameChequeNo + "= AND " +
                                          sViewNameIsChequeClear + "=" + this.ClearanceListFilters.IncludeAll.ToString() + "AND " +
                                          sViewNameIsChequeClearanceDate + "=" + this.ClearanceListFilters.ClearanceDateChecked.ToString() + "AND " +
                                          sViewNameIsCautionClear + "=" + chkCautionMoney.Checked.ToString() + ")";
            }

            return sSchoolYearFilter + "@ ";
        }

        #endregion

    }
}