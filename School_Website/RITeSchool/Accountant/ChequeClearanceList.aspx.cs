// File Name   : ChequeClearanceList.aspx.cs
// Created By  : Ketan
// Date        : 29/11/2007
// Modified by : Milind
// Date        : 11 Sept 2009

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using Utility;

/// <summary>
/// This Class is used to add and edit holiday management configuration.
/// </summary>
public partial class ChequeClearanceList : System.Web.UI.Page
{
    #region constants
    const int I_COLUMN_INDEX_CLEARANCE_DATETIME = 7;
    const int I_COLUMN_INDEX_CHEQUE_NUMBER = 2;
    #endregion

    #region Event

    /// <summary>
    /// This event is used to decrypt query string and set default properties to controls on the page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Initialise();
                btnShow.Text = "Show";
                btnExport.Style.Add("Visibility", "Hidden");
                btnSave.Style.Add("Visibility", "Hidden");
            }

            HtmlForm oform = (HtmlForm)this.Master.FindControl("form1");
            oform.DefaultButton = btnShow.UniqueID;
            SetClientScriptAttributes();
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
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                GridViewRow pagerRow = e.Row;

                // Retrieve the DropDownList and Label controls from the row.
                DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");
                pageList.Attributes.Add("onchange", "if(!MessageAboutDate('" + pageList.ClientID + "')){return false;}");
                Label pageLabel = (Label)pagerRow.Cells[0].FindControl("CurrentPageLabel");

                if (pageList != null)
                {

                    // Create the values for the DropDownList control based on 
                    // the  total number of pages required to display the data
                    // source.
                    for (int i = 0; i < grdCheques.PageCount; i++)
                    {
                        // Create a ListItem object to represent a page.
                        int pageNumber = i + 1;
                        ListItem item = new ListItem(pageNumber.ToString());
                        if (i == grdCheques.PageIndex)
                            item.Selected = true;

                        // Add the ListItem object to the Items collection of the 
                        // DropDownList.
                        pageList.Items.Add(item);
                    }
                }
                if (pageLabel != null)
                {
                    // Calculate the current page number.
                    int currentPage = grdCheques.PageIndex + 1;

                    // Update the Label control with the current page information.
                    pageLabel.Text = "Page " + currentPage.ToString() +
                      " of " + grdCheques.PageCount.ToString();
                    DisplayRowDetails();
                }
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int iRowIndex = ((GridViewRow)e.Row).RowIndex;
                int iPostDatedChequeId = Convert.ToInt32(grdCheques.DataKeys[iRowIndex]["PostDated_Cheque_Id"]);
                if (iPostDatedChequeId == 0)
                    e.Row.BackColor = System.Drawing.Color.LightBlue;
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
    /// This event is used to set grid according to selected page in the footer drop down list of grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdCheques_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdCheques.PageIndex = e.NewPageIndex;
            FillChequesGrid();
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
    /// This event is used to fill the grid according to filter.
    /// And also it is used for toggle the status of filter controls.
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
                grdCheques.PageIndex = Constants.I_ZERO;
                FillChequesGrid();
                if (grdCheques.Rows.Count > 0)
                    btnExport.Visible = true;
                EnableDisableControlsChecked(false);
                EnableDisableControls(false);

                btnShow.Text = "Change Input";
            }
            else
            {
                btnShow.Text = "Show";
                EnableDisableControlsChecked(true);
                EnableDisableControls(true);
                btnExport.Visible = false;
                grdCheques.DataSource = null;
                grdCheques.DataBind();
                grdCheques.Visible = false;
                tblLegend.Visible = false;
                trTotalRec.Visible = false;
                tblTotalAmount.Visible = false;
                lblSuccessMsg.Visible = false;
                lblSuccessMsg.Text = string.Empty;
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
    /// This event is used to set grid according to selected page in the footer drop down list of grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void PageDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            // Retrieve the pager row.
            GridViewRow pagerRow = grdCheques.BottomPagerRow;

            // Retrieve the PageDropDownList DropDownList from the bottom pager row.
            DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");

            // Set the PageIndex property to display that page selected by the user.
            grdCheques.PageIndex = pageList.SelectedIndex;
            FillChequesGrid();
            hidPageNo.Value = (pageList.SelectedIndex + 1).ToString();
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
    /// This event is used to set filter of cheque number for displaying grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optChequeNumber_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            OptCheckedNumberChecked();
            trTotalRec.Visible = false;
            grdCheques.Visible = false;
            tblLegend.Visible = false;
            lblError.Visible = false;
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
    /// This event is used to set filter of Registration number for displaying grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optRegNo_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            OptRegNoChecked();
            trTotalRec.Visible = false;
            grdCheques.Visible = false;
            tblLegend.Visible = false;
            lblError.Visible = false;
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
    /// This event is used to set filter of start date and end date for displaying grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optDate_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            OptDateChecked();
            trTotalRec.Visible = false;
            grdCheques.Visible = false;
            tblLegend.Visible = false;
            lblError.Visible = false;
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
    /// This event is used to set filter of clearance date and end date for displaying grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optClearanceDate_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            optClearanceDateChecked();
            trTotalRec.Visible = false;
            grdCheques.Visible = false;
            tblLegend.Visible = false;
            lblError.Visible = false;
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
    /// This event is used to save the cleared cheque details database.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (chkCautionMoney.Checked == false)
            {
                string sXML = GenerateXML();
                StudentPostDatedChequesBL oUpdateStudentPostDatedCheques = new StudentPostDatedChequesBL();
                DataTable oDTMessage = StudentPostDatedChequesBL.IsDuplicateChequeNo(sXML, false);
                if (oDTMessage == null || !(oDTMessage.Rows.Count > 0) || oDTMessage.Rows[0][0] == null)
                {
                    oUpdateStudentPostDatedCheques.SetChequeClearanceDate(sXML, false);
                    lblError.Visible = false;
                    lblSuccessMsg.Visible = true;
                    lblSuccessMsg.Text = "Cheque Clearance data updated successfully !!!";
                    FillChequesGrid();
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
                DataTable oDTMessage = StudentPostDatedChequesBL.IsDuplicateChequeNo(sXML, false);
                if (oDTMessage == null || !(oDTMessage.Rows.Count > 0) || oDTMessage.Rows[0][0] == null)
                {
                    oUpdateStudentPostDatedCheques.SetCautionClearanceDate(sXML);
                    lblError.Visible = false;
                    lblSuccessMsg.Visible = true;
                    lblSuccessMsg.Text = "Cheque Clearance data updated successfully !!!";
                    FillChequesGrid();
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
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
            (ex.Message + Constants.S_TRACE + ex.StackTrace,
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
            Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    /// <summary>
    ///  This XML is used to set the parameters to clear cheque detais.
    /// </summary>
    /// <returns></returns>
    private string GenerateXML()
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

    #endregion

    #region Private Method

    /// <summary>
    /// This method is used to display payment date and clearance date in proper formate.
    /// </summary>
    private void SetGridViewDateColumnProperties()
    {
        const Int32 I_COLUMN_INDEX_PAYMENT_DATE = 7;
        BoundField oPaymentDate = (BoundField)grdCheques.Columns[I_COLUMN_INDEX_PAYMENT_DATE];
        oPaymentDate.HtmlEncode = false;
        oPaymentDate.DataFormatString = Constants.S_STANDARD_GRID_DATE_FORMAT;
    }

    /// <summary>
    /// This method initialises variables.
    /// </summary>
    private void Initialise()
    {
        optChequeNumber.Focus();
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        valSave.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        grdCheques.PageSize = Constants.I_GRID_PAGE_COUNT;

        OptCheckedNumberChecked();

        optChequeNumber.Attributes.Add("onclick", "if(!ClearValSum()){return false;}");
        optRegNo.Attributes.Add("onclick", "if(!ClearValSum()){return false;}");
        optDate.Attributes.Add("onclick", "if(!ClearValSum()){return false;}");

        hidPageNo.Value = "1";
    }

    /// <summary>
    /// This method is used clear the text from the textboxes.
    /// </summary>
    private void ClearTexts()
    {
        txtChequeNumber.Text = "";
        txtRegNo.Text = "";
        txtFromDate.Text = "";
        txtToDate.Text = "";
        txtClearanceEndDate.Text = "";
        txtClearanceStartDate.Text = "";
    }

    /// <summary>
    /// This method used to set javascrpit attributes to the controls.
    /// </summary>
    private void SetClientScriptAttributes()
    {
        btnShow.Attributes["onmouseover"] = "javascript:fnover('" + btnShow.ClientID + "');";
        btnShow.Attributes["onmouseout"] = "javascript:fnout('" + btnShow.ClientID + "');";
        btnExport.Attributes["onmouseover"] = "javascript:fnover('" + btnExport.ClientID + "');";
        btnExport.Attributes["onmouseout"] = "javascript:fnout('" + btnExport.ClientID + "');";
        btnSave.Attributes["onmouseover"] = "javascript:fnover('" + btnSave.ClientID + "');";
        btnSave.Attributes["onmouseout"] = "javascript:fnout('" + btnSave.ClientID + "');";

        txtChequeNumber.Attributes.Add("onkeypress", "return clickButton(event)");
        txtFromDate.Attributes.Add("onkeypress", "return clickButton(event)");
        txtRegNo.Attributes.Add("onkeypress", "return clickButton(event)");
        txtToDate.Attributes.Add("onkeypress", "return clickButton(event)");
        hidServerDate.Value = System.DateTime.Today.ToString();
    }

    /// <summary>
    /// This method used to fill the grid according to selected filter.
    /// </summary>
    private void FillChequesGrid()
    {
        lblError.Visible = false;
        int iSchoolId = Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]);
        int iAcaYearId = Convert.ToInt32(Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID]);
        DataTable oDTCheques = null;
        string sRegNo = txtRegNo.Text.Trim();
        string sChequeNo = txtChequeNumber.Text.Trim();
        int TotalAmount = 0;
        if (!sRegNo.Equals(""))
            oDTCheques = StudenChequesCollectionBL.FetchChequesDetails(sRegNo, iSchoolId, iAcaYearId, chkIncludeAll.Checked, chkCautionMoney.Checked, false, out TotalAmount, false);
        else if (!sChequeNo.Equals(""))
            oDTCheques = StudenChequesCollectionBL.FetchChequesDetails(sChequeNo, iSchoolId, iAcaYearId, chkIncludeAll.Checked, chkCautionMoney.Checked, true, out TotalAmount, false);
        else if (!txtFromDate.Text.Equals("") && !txtToDate.Text.Equals(""))
        {
            DateTime odtStartDate = Convert.ToDateTime(txtFromDate.Text);
            DateTime odtToDate = Convert.ToDateTime(txtToDate.Text);
            oDTCheques = StudenChequesCollectionBL.FetchChequesDetails(odtStartDate, odtToDate, iSchoolId, iAcaYearId, chkIncludeAll.Checked, chkCautionMoney.Checked, true, out TotalAmount, false);
        }
        else if (!txtClearanceStartDate.Text.Equals("") && !txtClearanceEndDate.Text.Equals(""))
        {
            DateTime odtStartDate = Convert.ToDateTime(txtClearanceStartDate.Text.Trim());
            DateTime odtEndDate = Convert.ToDateTime(txtClearanceEndDate.Text.Trim());
            oDTCheques = StudenChequesCollectionBL.FetchChequesDetails(odtStartDate, odtEndDate, iSchoolId, iAcaYearId, chkIncludeAll.Checked, chkCautionMoney.Checked, false, out TotalAmount, false);
        }
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
    /// This method used to set the value to the label indicating records from the grid.
    /// </summary>
    private void DisplayRowDetails()
    {
        int iRowCount = ((DataView)(grdCheques.DataSource)).Count;

        lblStartIndex.Text = Convert.ToString((grdCheques.PageSize * grdCheques.PageIndex) + 1);
        lblEndIndex.Text = Convert.ToString((Convert.ToInt32(lblStartIndex.Text) + grdCheques.PageSize) - 1);
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

    /// <summary>
    /// This method used to set cheque number filter as well as to enabled or disabled 
    /// controls according to that.
    /// </summary>
    private void OptCheckedNumberChecked()
    {
        ClearTexts();
        txtChequeNumber.Enabled = true;
        txtFromDate.Enabled = false;
        txtToDate.Enabled = false;
        cFromDate.Enabled = false;
        cToDate.Enabled = false;
        txtRegNo.Enabled = false;
        txtClearanceStartDate.Enabled = false;
        txtClearanceEndDate.Enabled = false;
        calClearanceEndDate.Enabled = false;
        calClearanceStartDate.Enabled = false;

        lblChequeNumberMandMark.Visible = true;
        lblRegNoMandMark.Visible = false;
        lblFromDateMandMark.Visible = false;
        lblToDateMandMark.Visible = false;
        lblClearanceEndDate.Visible = false;
        lblClearanceStartDate.Visible = false;
        chkIncludeAll.Checked = false;
        tblLegend.Visible = false;
    }

    /// <summary>
    /// This method used to set Registration number filter as well as to enabled or disabled 
    /// controls according to that.
    /// </summary>
    private void OptRegNoChecked()
    {
        ClearTexts();
        txtChequeNumber.Enabled = false;
        txtFromDate.Enabled = false;
        txtToDate.Enabled = false;
        cFromDate.Enabled = false;
        cToDate.Enabled = false;
        txtRegNo.Enabled = true;
        txtClearanceStartDate.Enabled = false;
        txtClearanceEndDate.Enabled = false;
        calClearanceEndDate.Enabled = false;
        calClearanceStartDate.Enabled = false;

        lblChequeNumberMandMark.Visible = false;
        lblRegNoMandMark.Visible = true;
        lblFromDateMandMark.Visible = false;
        lblToDateMandMark.Visible = false;
        lblClearanceEndDate.Visible = false;
        lblClearanceStartDate.Visible = false;
        chkIncludeAll.Checked = false;
    }

    /// <summary>
    /// This method used to set cheque start and end date filter as well as to enabled or disabled 
    /// controls according to that.
    /// </summary>
    private void OptDateChecked()
    {
        ClearTexts();
        txtChequeNumber.Enabled = false;
        txtFromDate.Enabled = true;
        txtToDate.Enabled = true;
        cFromDate.Enabled = true;
        cToDate.Enabled = true;
        txtRegNo.Enabled = false;
        txtClearanceStartDate.Enabled = false;
        txtClearanceEndDate.Enabled = false;
        calClearanceEndDate.Enabled = false;
        calClearanceStartDate.Enabled = false;

        lblChequeNumberMandMark.Visible = false;
        lblRegNoMandMark.Visible = false;
        lblFromDateMandMark.Visible = true;
        lblToDateMandMark.Visible = true;
        lblClearanceEndDate.Visible = false;
        lblClearanceStartDate.Visible = false;
        chkIncludeAll.Checked = false;
    }

    /// <summary>
    /// This method used to set Clearance start and end date filter as well as to enabled or disabled 
    /// controls according to that.
    /// </summary>
    private void optClearanceDateChecked()
    {
        ClearTexts();
        txtChequeNumber.Enabled = false;
        txtFromDate.Enabled = false;
        txtToDate.Enabled = false;
        cFromDate.Enabled = false;
        cToDate.Enabled = false;
        txtRegNo.Enabled = false;
        txtClearanceStartDate.Enabled = true;
        txtClearanceEndDate.Enabled = true;
        calClearanceEndDate.Enabled = true;
        calClearanceStartDate.Enabled = true;

        lblChequeNumberMandMark.Visible = false;
        lblRegNoMandMark.Visible = false;
        lblFromDateMandMark.Visible = false;
        lblToDateMandMark.Visible = false;
        lblClearanceEndDate.Visible = true;
        lblClearanceStartDate.Visible = true;
        chkIncludeAll.Checked = true;
    }

    /// <summary>
    /// This method used to enabled or disabled controls.
    /// </summary>
    private void EnableDisableControls(bool abFlag)
    {
        optRegNo.Enabled = abFlag;
        optDate.Enabled = abFlag;
        optChequeNumber.Enabled = abFlag;
        optClearanceDate.Enabled = abFlag;
        chkIncludeAll.Enabled = abFlag;
        chkCautionMoney.Enabled = abFlag;
    }

    /// <summary>
    /// This method used to enabled or disabled controls.
    /// </summary>
    private void EnableDisableControlsChecked(bool abFlag)
    {
        if (optRegNo.Checked)
            txtRegNo.Enabled = abFlag;
        if (optDate.Checked)
        {
            txtFromDate.Enabled = abFlag;
            txtToDate.Enabled = abFlag;
        }
        if (optChequeNumber.Checked)
            txtChequeNumber.Enabled = abFlag;
        if (optClearanceDate.Checked)
        {
            txtClearanceEndDate.Enabled = abFlag;
            txtClearanceStartDate.Enabled = abFlag;
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
            ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.ChequeClearanceDetails, GetFilterString());
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
                                          sViewNameStartDate + "=" + System.DateTime.Now + " AND " +
                                          sViewNameEndDate + "=" + System.DateTime.Now + "  AND " +
                                          sViewNameIsChequeClear + "=" + chkIncludeAll.Checked.ToString() + "AND " +
                                          sViewNameIsChequeClearanceDate + "=" + optClearanceDate.Checked.ToString() + "AND " +
                                          sViewNameIsCautionClear + "=" + chkCautionMoney.Checked.ToString()+ ")";
                                          
            }
            else if (optDate.Checked)
            {
                sSchoolYearFilter = "(" + sViewNameSchID + "=" + iSchoolID + " AND " +
                                                               sViewNameAcdYearId + "=" + iAcadYearID +
                                                                " AND " +
                                          sViewNameStartDate + "=" + txtFromDate.Text.Trim() + " AND " +
                                          sViewNameEndDate + "=" + txtToDate.Text.Trim() + " AND " +
                                          sViewNameChequeNo + "=  AND " +
                                          sViewNameRegNo + "= AND " +
                                          sViewNameIsChequeClear + "=" + chkIncludeAll.Checked.ToString() + "AND " +
                                           sViewNameIsChequeClearanceDate + "=" + optClearanceDate.Checked.ToString() + "AND " +
                                          sViewNameIsCautionClear + "=" + chkCautionMoney.Checked.ToString() + ")";

            }
            else if (optRegNo.Checked)
            {
                sSchoolYearFilter = "(" + sViewNameSchID + "=" + iSchoolID + " AND " +
                                                               sViewNameAcdYearId + "=" + iAcadYearID +
                                                                " AND " +
                                          sViewNameRegNo + "=" + txtRegNo.Text.Trim() + " AND " +
                                          sViewNameStartDate + "=" + System.DateTime.Now + " AND " +
                                          sViewNameEndDate + "=" + System.DateTime.Now + " AND " +
                                          sViewNameChequeNo + "= AND " +
                                          sViewNameIsChequeClear + "=" + chkIncludeAll.Checked.ToString() + "AND " +
                                          sViewNameIsChequeClearanceDate + "=" + optClearanceDate.Checked.ToString() + "AND " +
                                          sViewNameIsCautionClear + "=" + chkCautionMoney.Checked.ToString() + ")";
            }
            else if (optClearanceDate.Checked)
            {
                sSchoolYearFilter = "(" + sViewNameSchID + "=" + iSchoolID + " AND " +
                                                               sViewNameAcdYearId + "=" + iAcadYearID +
                                                                " AND " +
                                          sViewNameStartDate + "=" + txtClearanceStartDate.Text.Trim() + " AND " +
                                          sViewNameEndDate + "=" + txtClearanceEndDate.Text.Trim() + " AND " +
                                          sViewNameChequeNo + "=  AND " +
                                          sViewNameRegNo + "= AND " +
                                          sViewNameIsChequeClear + "=" + chkIncludeAll.Checked.ToString() + "AND " +
                                          sViewNameIsChequeClearanceDate + "=" + optClearanceDate.Checked.ToString() + "AND " +
                                          sViewNameIsCautionClear + "=" + chkCautionMoney.Checked.ToString() + ")";
            }
        
        return sSchoolYearFilter + "@ ";
    }

    #endregion

}




