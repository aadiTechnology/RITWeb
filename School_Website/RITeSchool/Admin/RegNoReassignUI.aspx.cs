// File Name     : ReassignRegNoUI.aspx.cs
// Modified By   : Amit 
// Modified Date : 07/09/2009
// Description   : This class is used to reassign registration number of student.

using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using Utility;
using System.Linq;
using System.Collections.Generic;
using StudentEntities;
using System.Text;
using SchoolAutoSearchService.Service;

public partial class ReassignRegNoUI : SchoolBase
{

    #region " Constants "

    const string S_BUTTON_SEARCH_TEXT = "Show";
    const string S_BUTTON_CHANGE_TEXT = "Change Input";
    const int I_COLUMN_INDEX_NEW_REG_NO = 3;

    const string S_PREPRIMARY_PREFIX_ERROR = "For pre-primary standards registration number should start with PP followed by numbers for roll number(s) : ";
    const string S_PRIMARY_PREFIX_ERROR = "For primary standards registration number should end with ";
    const string S_EMPTY_REG_NUMBER = "Registration number should not be blank for roll number(s) : ";
    const int I_BLANK_REG_COUNT_FILTER = 0;
    const int I_BLANK_REG_COUNT_SCHOOL = 1;

    #endregion " Constants "

    #region " Events "

    /// <summary>
    /// This event is used to show grid containing student registation number detail. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                RefreshValue();
                FillOperators();
                GetPrefixes();
                GetAllRegNoPostfixes();
                ReadQueryString();
                FillStandardCombobox();
                FillDivisionCombobox();
                ShowHideControls(true);
                ShowHideBlankRegCount(true);
                InitializePageControls();
                SetBlankRegCount();
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
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
    /// This event is used to view student registration records.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnShow.Text != Resources.LocalizedResources.ChangeInput)
            {
                btnShow.Text = Resources.LocalizedResources.ChangeInput;
                ShowHideBlankRegCount(true);
                ShowHideControls(true);
                grdvwRegNo.DataSourceID = GrdDSobj.ID;
                grdvwRegNo.DataBind();
                if (grdvwRegNo.Rows.Count > 0)
                    btnUpdate.Visible = true;
                else
                {
                    btnUpdate.Visible = false;
                    trTopButtons.Visible = false;
                }
                SetBlankRegCount();
            }
            else
            {
                btnShow.Text = Resources.LocalizedResources.Search;
                ShowHideControls(false);
                ShowHideBlankRegCount(false);
                if (optExact.Checked)
                {
                    cmbOperation.Enabled = true;
                    cmbPrefix.Enabled = true;
                }
                ddlStandard.Focus();

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
    /// This event used to fill divisions in combo as per selected standard.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hidStandardId.Value = ddlStandard.SelectedValue;
            hidDivisionId.Value = "0";
            FillDivisionCombobox();
            ShowHideControlsToViewAll();
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
    /// This event is used to enable/ disable filter criteria.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hidDivisionId.Value = ddlDivision.SelectedValue;
            ShowHideControlsToViewAll();
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
    /// This event is used to move to previous page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            string sEncrypt = GetQueryString();
            string sURL = Constants.S_PAGE_ALL_STUDENTS_LIST + "?" + sEncrypt;
            PopupMaster oMasterPage = (PopupMaster)this.Master;
            oMasterPage.RedirectToNextPage(sURL);
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
    /// This event is used to update registration number.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        // This saves changes to database
        try
        {
            int iUserId = Convert.ToInt32(Convert.ToString(Session[Constants.S_SESSION_USER_ID]));
            int iStandardId = Convert.ToInt32(ddlStandard.SelectedValue);
            int iDivisionId = Convert.ToInt32(ddlDivision.SelectedValue);
            string sRegNumber = txtRegNumber.Text;
            string sXmlStudentsRegNos = GenerateStudentsrRegNosXML();

            SchoolUserBL oSchoolUserBL = new SchoolUserBL();
            DataTable oDTBlankRegNo = oSchoolUserBL.UpdateStudentRegNoAndLoginPassword(miSchoolId, miAcademicYearId, iUserId, iStandardId, iDivisionId, sRegNumber, sXmlStudentsRegNos);
            grdvwRegNo.DataSourceID = GrdDSobj.ID;
            grdvwRegNo.DataBind();
            if (grdvwRegNo.Rows.Count == 0)
            {
                btnUpdate.Visible = false;
                trTopButtons.Visible = false;
            }

            lblBlankRegCountFilter.Text = oDTBlankRegNo.Rows[0][I_BLANK_REG_COUNT_FILTER].ToString();
            lblBlankRegCount.Text = oDTBlankRegNo.Rows[0][I_BLANK_REG_COUNT_SCHOOL].ToString();
            tdMessage.Align = "center";
            lblError.Visible = true;
            lblError.ForeColor = System.Drawing.Color.Blue;
            lblError.Text = "<b>" + Resources.LocalizedResources.MsgRegNoUpdated + "</b>";
            if (lblBlankRegCount.Text != "0")
                lblError.Text = lblError.Text + "<br><b>" + Resources.LocalizedResources.InSchoolStill + lblBlankRegCount.Text + Resources.LocalizedResources.MsgStudentBlankRegNo + "</b>";
            btnShow.Focus();
            RefreshStudentCache();
        }
        catch (BusinessLogic.Exceptions.DuplicateRegisterNumberExceptions ex)
        {
            tdMessage.Align = "left";
            lblError.Text = ex.Message;
            lblError.Visible = true;
            lblError.CssClass = "ClsLabel";
            lblError.ForeColor = System.Drawing.Color.Red;
            btnShow.Focus();
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
    /// This event is used to give red colour to the students who leaves school.  
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwRegNo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= 0)
            {
                if (grdvwRegNo.DataKeys[e.Row.RowIndex]["SchoolLeft_Date"] != DBNull.Value)
                    e.Row.ForeColor = System.Drawing.Color.Red;

                //if (e.Row.RowIndex >= 1)
                //{
                //    TextBox txtNewRegNo = ((TextBox)grdvwRegNo.Rows[e.Row.RowIndex-1].Cells[3].FindControl("txtNewRegNo"));
                //    if (txtNewRegNo != null && txtNewRegNo.Text.Trim() != string.Empty && moSchool == Constants.SchoolId.PPSN)
                //        txtNewRegNo.Enabled = false;
                //}
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

    protected void optMain_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (optMain.Checked)
                SetControlsForLikeCriteria();

        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
            (ex.Message + Constants.S_TRACE + ex.StackTrace,
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
            Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    protected void optExact_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (optExact.Checked)
                SetControlsForExactMatchCriteria();
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
            (ex.Message + Constants.S_TRACE + ex.StackTrace,
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
            Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    protected void grdvwRegNo_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (grdvwRegNo.Rows.Count > 0)
            {
                TextBox txtNewRegNo = ((TextBox)grdvwRegNo.Rows[grdvwRegNo.Rows.Count - 1].Cells[3].FindControl("txtNewRegNo"));
                if (txtNewRegNo != null && txtNewRegNo.Text.Trim() != string.Empty && moSchool == Constants.SchoolId.PPSN)
                    txtNewRegNo.Enabled = false;
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

    #endregion " Events "

    #region " Private Methods "

    /// <summary>
    /// This method is used to fill the operators in the dropdownlist.
    /// </summary>
    private void FillOperators()
    {
        List<Operator> olstOperators = StudentBL.GetOperators();
        ListSource.FillDropDownList(olstOperators, cmbOperation, "Text", "Value", string.Empty);
    }

    /// <summary>
    /// This method is used to get the list of prefixes.
    /// </summary>
    /// <param name="aiSchoolId"></param>
    /// <returns></returns>
    private void GetPrefixes()
    {
        List<string> olstPrefixes = StudentBL.GetPrefixes(miSchoolId, miAcademicYearId);
        cmbPrefix.Items.Add(new ListItem(Constants.S_ALL, Constants.S_ALL));
        if (olstPrefixes.Count > Constants.I_ZERO)
            olstPrefixes.ForEach(pfx => cmbPrefix.Items.Add(new ListItem(pfx, pfx)));
    }

    private void GetAllRegNoPostfixes()
    {       
        List<string> olstPostfixes = StudentBL.GetAllRegNoPostfixes(miSchoolId, miAcademicYearId);
        if (olstPostfixes.Count > Constants.I_ZERO)
            olstPostfixes.ForEach(pfx => cmbPrefix.Items.Add(new ListItem(pfx, pfx)));
    }

    /// <summary>
    /// This method is used to fill combobox with all standards available in current school.
    /// </summary>
    private void FillStandardCombobox()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtStandardCollection = oStandardCollectionBL.GetAssociatedStandards();
        ControlUtility.FillDropDownList(oDtStandardCollection, ref ddlStandard,
                                       Constants.S_STANDARD_ID_FIELD,
                                       Constants.S_STANDARD_NAME_FIELD,
                                       Constants.S_SELECT_ALL);
        ddlStandard.SelectedValue = hidStandardId.Value;
    }

    /// <summary>
    /// This method is used to fills combobox with Divisions with selected Standard.
    /// </summary>
    /// <param name="aiStandardId"></param>
    private void FillDivisionCombobox()
    {
        const string S_STDDIV_ID_FLD = "division_Id";        
        DivisionCollectionBL oDiv = new DivisionCollectionBL(miSchoolId, miAcademicYearId);
        DataTable dtDivision = null;

        if (hidStandardId.Value != string.Empty && hidStandardId.Value != "0")
            dtDivision = oDiv.GetAllDivisionsForStandard(Convert.ToInt32(hidStandardId.Value));
        else
            hidDivisionId.Value = "0";
        //This method is used to fill current division's combo.
        ControlUtility.FillDropDownList(dtDivision, ref ddlDivision,
                                       S_STDDIV_ID_FLD,
                                       Constants.S_DIVISION_NAME_FIELD,
                                       Constants.S_SELECT_ALL);

        if (hidStandardId.Value != "0")
            ddlDivision.SelectedValue = hidDivisionId.Value;
    }

    /// <summary>
    /// This method used to count number of student having blank registration number.
    /// </summary>
    private void SetBlankRegCount()
    {
         int iStandardId = Convert.ToInt32(hidStandardId.Value);
        int iDivisionId = Convert.ToInt32(hidDivisionId.Value);
        string sName = string.Empty;
        if (optExact.Checked && !chkIsStudBlankRegNo.Checked)
            sName = txtReg.Text.Trim(); 
        else if(optMain.Checked && !chkIsStudBlankRegNo.Checked)
            sName = txtRegNumber.Text.Trim();
        DataTable oDTCount = StudentBL.GetBlankRegNoCount(miSchoolId, miAcademicYearId, iStandardId, iDivisionId, sName);
        lblBlankRegCountFilter.Text = oDTCount.Rows[0][I_BLANK_REG_COUNT_FILTER].ToString();
        lblBlankRegCount.Text = oDTCount.Rows[0][I_BLANK_REG_COUNT_SCHOOL].ToString();
    }

    /// <summary>
    /// This method is used to show or hide controls.
    /// </summary>
    /// <param name="abFlag"></param>
    private void ShowHideControls(bool abFlag)
    {
        //txtRegNumber.Enabled = !abFlag;
        ddlStandard.Enabled = !abFlag;
        ddlDivision.Enabled = !abFlag;
        chkIsStudBlankRegNo.Enabled = !abFlag;
        grdvwRegNo.Visible = abFlag;
        lblError.Visible = false;
        btnUpdate.Visible = abFlag;
        trTopButtons.Visible = abFlag;
        optExact.Enabled = !abFlag;
        optMain.Enabled = !abFlag;

        if (optMain.Checked)
            txtRegNumber.Enabled = !abFlag;
        else if (optExact.Checked)
            txtReg.Enabled = !abFlag;
    }

    /// <summary>
    /// This methos is used to show or hide filtered count of student having blank registration number.
    /// </summary>
    /// <param name="abFlag"></param>
    private void ShowHideBlankRegCount(bool abFlag)
    {
        tdTitleRegCountFilter.Visible = abFlag;
        tdBlankRegCountFilter.Visible = abFlag;
    }

    /// <summary>
    /// This method is used initialises variables.
    /// </summary>
    private void InitializePageControls()
    {
        btnShow.Attributes["onmouseover"] = "javascript:fnover('" + btnShow.ClientID + "',this);";
        btnShow.Attributes["onmouseout"] = "javascript:fnout('" + btnShow.ClientID + "',this);";

        btnBack.Attributes["onmouseover"] = "javascript:fnover('" + btnBack.ClientID + "',this);";
        btnBack.Attributes["onmouseout"] = "javascript:fnout('" + btnBack.ClientID + "',this);";

        btnUpdate.Attributes["onmouseover"] = "javascript:fnover('" + btnUpdate.ClientID + "',this);";
        btnUpdate.Attributes["onmouseout"] = "javascript:fnout('" + btnUpdate.ClientID + "',this);";

        btnTopUpdate.Attributes["onmouseover"] = "javascript:fnover('" + btnTopUpdate.ClientID + "',this);";
        btnTopUpdate.Attributes["onmouseout"] = "javascript:fnout('" + btnTopUpdate.ClientID + "',this);";

        btnTopClose.Attributes["onmouseover"] = "javascript:fnover('" + btnTopClose.ClientID + "',this);";
        btnTopClose.Attributes["onmouseout"] = "javascript:fnout('" + btnTopClose.ClientID + "',this);";


        btnUpdate.Attributes.Add("onClick", "if(!ValidateRegNosInGrid('txtNewRegNo')) return false;");
        btnTopUpdate.Attributes.Add("onClick", "if(!ValidateRegNosInGrid('txtNewRegNo')) return false;");
        btnBack.Text = Resources.LocalizedResources.Close;
        btnShow.Text = Resources.LocalizedResources.ChangeInput;

        if (hidStandardId.Value == "0" || hidDivisionId.Value == "0")
            chkIsStudBlankRegNo.Checked = true;
        else
            chkIsStudBlankRegNo.Checked = false;
        if (grdvwRegNo.Rows.Count > 0)
            btnUpdate.Visible = true;
        else
        {
            btnUpdate.Visible = false;
            trTopButtons.Visible = false;
        }
    }

    /// <summary>
    /// This method is used to check validation in grid as per standard for primary and preprimary.
    /// </summary>
    /// <param name="aiRowId"></param>
    /// <param name="asErrorRegNos"></param>
    /// <param name="asErrorPPRegNos"></param>
    private void ValidateRegNo(int aiRowId, ref string asErrorRegNos, ref string asErrorPPRegNos, ref string asErrorPRegNos, ref string sEmptyRegNos)
    {
       
            {
                string sPreFix = grdvwRegNo.DataKeys[aiRowId]["Reg_No_Prefix"].ToString();
                //for registration number 
                int iCount = 0;
                List<string> RegNoPostFix = StudentBL.GetAllRegNoPostfixes(miSchoolId, miAcademicYearId);
                string[] sRegNoPostFix = RegNoPostFix.ToArray();
                StringBuilder oStringBuilder = new StringBuilder();
                for (int iPostfixvalue = 0; iPostfixvalue < RegNoPostFix.Count; iPostfixvalue++)
                {
                    oStringBuilder.Append(',' + sRegNoPostFix[iPostfixvalue]);
                }
                string sRollNo = grdvwRegNo.Rows[aiRowId].Cells[0].Text;
                long iRegNo;
                TextBox txtNewRegNo = ((TextBox)grdvwRegNo.Rows[aiRowId].Cells[3].FindControl("txtNewRegNo"));
                string sNewRegNo = txtNewRegNo.Text.Trim();
                lblError.Text = string.Empty;

                if (sNewRegNo != string.Empty)
                {
                    if (sPreFix != string.Empty)
                    {

                        int iLenPreFix = sPreFix.Length;
                        if (sNewRegNo.Length > sPreFix.Length
                            && sPreFix == sNewRegNo.Substring(Constants.I_ZERO, iLenPreFix))
                        {
                            if (!long.TryParse(sNewRegNo.Substring(iLenPreFix), out iRegNo))
                                asErrorPPRegNos = asErrorPPRegNos + ", " + sRollNo;
                        }
                        else
                            asErrorPPRegNos = asErrorPPRegNos + ", " + sRollNo;
                    }
                    else if (sRegNoPostFix.Length > 0 && sPreFix == string.Empty)
                    {
                        int iLinePostFix = RegNoPostFix[0].Length;

                        string sRegistratioNumberNew = sNewRegNo.Substring(0, sNewRegNo.Trim().Length - iLinePostFix);
                        if (!long.TryParse(sRegistratioNumberNew, out iRegNo))
                            asErrorRegNos = asErrorRegNos + oStringBuilder + '.' + "For roll number(s): " + sRollNo;

                        if (sNewRegNo.Length > iLinePostFix)
                        {
                            for (int iPostFicINdex = 1; iPostFicINdex < sRegNoPostFix.Length; iPostFicINdex++)
                            {
                                int iValidRegNo = sNewRegNo.IndexOf(sRegNoPostFix[iPostFicINdex]);
                                if (iValidRegNo > 0)
                                {
                                    iCount += 1;
                                    break;
                                }
                            }
                        }

                        if (iCount <= 0)
                        {
                            if (asErrorPRegNos == string.Empty)
                                asErrorPRegNos = asErrorPRegNos + oStringBuilder + '.' + "For roll number(s): " + sRollNo;
                            else
                                asErrorPRegNos = asErrorPRegNos + ',' + sRollNo;
                        }

                    }


                    else if (!long.TryParse(sNewRegNo, out iRegNo))
                        asErrorRegNos = asErrorRegNos + ", " + sRollNo;
                }
                else
                {
                    if (Settings.AllowEmptyRegNo == "false")
                    {
                        sEmptyRegNos = sEmptyRegNos + "," + sRollNo;
                    }
                }
            }
        }
        
       
    

    /// <summary>
    /// This method is used to set querystring according to selected standard and division.
    /// </summary>
    private string GetQueryString()
    {
        string sQueryString = string.Empty;

        sQueryString += "StandardId=" + ddlStandard.SelectedValue;
        sQueryString += "&DivisionId=" + ddlDivision.SelectedValue;
        sQueryString += "&NameOrRegNo=" + txtRegNumber.Text.Trim();
        sQueryString += "&RegNo=" + txtReg.Text.Trim();
        sQueryString += "&abIsExactMatch=" + optExact.Checked.ToString();
        if (optExact.Checked)
        {
            hidPrefix.Value = cmbPrefix.SelectedValue;
            hidOperator.Value = cmbOperation.SelectedValue;
        }
        sQueryString += "&asOperator="+hidOperator.Value;
        sQueryString += "&asPrefix="+hidPrefix.Value;        
        sQueryString += "&asPostfix=" + hidPostfix.Value;

        string sEncrypt = Utility.CommonUtility.EncryptQuerystring(sQueryString.ToString());
        return sEncrypt;
    }

    /// <summary>
    /// This method is used to read querystring.
    /// </summary>
    private void ReadQueryString()
    {
        HttpRequest oHttprequest;
        string sQueryString = string.Empty;
        string sEventDateDecrypt = Server.UrlDecode(Request.QueryString.ToString());
        if (!sEventDateDecrypt.Equals(string.Empty))
            sQueryString = CommonUtility.DecryptQuerystring(sEventDateDecrypt);
        oHttprequest = new HttpRequest(Page.Request.FilePath.ToString(),
                                        Page.Request.Url.ToString(),
                                        sQueryString);

        hidDivisionId.Value = oHttprequest.QueryString.Get("DivisionId");
        hidStandardId.Value = oHttprequest.QueryString.Get("StandardId");

        if (Convert.ToBoolean(oHttprequest.QueryString.Get("abIsExactMatch")))
        {
            optExact.Checked = true;
            SetControlsForExactMatchCriteria();
            txtReg.Text = oHttprequest.QueryString.Get("RegNo");

            hidOperator.Value = oHttprequest.QueryString.Get("asOperator");
            hidPrefix.Value = oHttprequest.QueryString.Get("asPrefix");
            hidPostfix.Value = oHttprequest.QueryString.Get("asPostfix");
            cmbPrefix.SelectedValue = hidPrefix.Value.ToString();           
            cmbOperation.SelectedValue = hidOperator.Value.ToString();
            if (!IsPostBack)
            {
                cmbOperation.Enabled = false;
                cmbPrefix.Enabled = false;
            }
            
        }
        else
        {
            optMain.Checked = true;
            SetControlsForLikeCriteria();
            txtRegNumber.Text = oHttprequest.QueryString.Get("NameOrRegNo");
        }
    }

    /// <summary>
    /// Generate XML for the RollNos order.
    /// And it also check registration number validation according to preprimary and primary.
    /// i.e. For Preprimary PP prefix and primary registration number is number.
    /// </summary>
    /// <returns></returns>
    private string GenerateStudentsrRegNosXML()
    {
        const string S_ELEMENT = "element";
        string sErrorRegNos = string.Empty;
        string sErrorPPRegNos = string.Empty;
        string sErrorPRegNos = string.Empty;
        string sEmptyRegNos = string.Empty;       
        XmlDocument oDoc = new XmlDocument();

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("StudentsRegNosCollection");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "StudentsRegNosCollection", "");
        // Loop through all the grid rows.
        for (int iRowCount = 0; iRowCount < grdvwRegNo.Rows.Count; iRowCount++)
        {
            ValidateRegNo(iRowCount, ref sErrorRegNos, ref sErrorPPRegNos, ref sErrorPRegNos, ref sEmptyRegNos);
            TextBox oTextBox = grdvwRegNo.Rows[iRowCount].Cells[I_COLUMN_INDEX_NEW_REG_NO].FindControl("txtNewRegNo") as TextBox;
            // Create root xml element.
            XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "StudentsRegNos", "");

            string sAtrrName = "YearWise_Student_Id";
            XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = grdvwRegNo.DataKeys[iRowCount]["Student_Id"].ToString();
            oXmlNode.Attributes.Append(attr);

            sAtrrName = "OldRegNo";
            attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = grdvwRegNo.DataKeys[iRowCount]["Enrolment_Number"].ToString();
            oXmlNode.Attributes.Append(attr);

            sAtrrName = "NewRegNo";
            attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = oTextBox.Text;
            oXmlNode.Attributes.Append(attr);

            // Add the node to root node.
            oXmlRootNode.AppendChild(oXmlNode);
        }
        if (sErrorRegNos != string.Empty || sErrorPPRegNos != string.Empty || sErrorPRegNos != string.Empty || sEmptyRegNos != string.Empty)
        {
            string sMsg = string.Empty;
            if (sErrorPPRegNos != string.Empty)
            {
                sErrorPPRegNos = sErrorPPRegNos.Substring(2, sErrorPPRegNos.Length - 2);
                sMsg = sMsg + Resources.LocalizedResources.MsgPrePrimaryPrefixError + " " + sErrorPPRegNos + " <br>";
            }
            if (sErrorRegNos != string.Empty)
            {
                sErrorRegNos = sErrorRegNos.Substring(2, sErrorRegNos.Length - 2);
                sMsg = sMsg + Resources.LocalizedResources.MsgPrimaryPrefixError + " " + sErrorRegNos + " <br>";
            }
            if (sErrorPRegNos != string.Empty)
            {
                sErrorPRegNos = sErrorPRegNos.Substring(0, sErrorPRegNos.Length);
                sMsg = sMsg + Resources.LocalizedResources.MsgPrimaryPrefixError + " " + sErrorPRegNos + " <br>";
                
            }

            
                if (sEmptyRegNos != string.Empty)
                {
                    sEmptyRegNos = sEmptyRegNos.Substring(1, sEmptyRegNos.Length - 1);
                    sMsg = sMsg + Resources.LocalizedResources.MsgEmptyRegNo + " " + sEmptyRegNos + " <br>";
                }
                throw new BusinessLogic.Exceptions.DuplicateRegisterNumberExceptions(sMsg);
           
            
            
        }
        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);

        // return the string generated.
        return root.InnerXml;
    }

    /// <summary>
    /// This event is used to enable/disable controls when no standard or division selected.
    /// </summary>
    private void ShowHideControlsToViewAll()
    {
        bool bFlag;
        if (hidDivisionId.Value == "0")
            bFlag = true;
        else
            bFlag = false;

        chkIsStudBlankRegNo.Checked = bFlag;
        chkIsStudBlankRegNo.Enabled = true;
    }

    private void SetControlsForLikeCriteria()
    {
        txtRegNumber.Text = "";
        txtReg.Text = "";
        txtReg.Enabled = false;
        cmbOperation.ClearSelection();
        cmbOperation.Enabled = false;
        cmbPrefix.ClearSelection();
        cmbPrefix.Enabled = false;
        txtRegNumber.Enabled = true;
        txtRegNumber.Focus();
    }

    private void SetControlsForExactMatchCriteria()
    {
        txtRegNumber.Text = "";
        txtReg.Text = "";
        cmbOperation.Enabled = true;
        cmbPrefix.Enabled = true;
        txtRegNumber.Enabled = false;
        txtReg.Enabled = true;
        txtReg.Focus();
    }

    private void RefreshValue()
    {
        hidValRegNoZeroForRollNo.Value = Resources.LocalizedResources.ValRegNoZeroForRollNo;
        hidValDuplicateRegNoForRollNo.Value = Resources.LocalizedResources.ValDuplicateRegNoForRollNo;
        HidEmptyRegNo.Value = Settings.AllowEmptyRegNo;
         
     }

    /// <summary>
    /// This method is used to refresh student cache.
    /// </summary>
    private void RefreshStudentCache()
    {
        AutoSearchService oAutoSearchService = new AutoSearchService();
        oAutoSearchService.RefreshStudentCache(miSchoolId, miAcademicYearId, new List<int>(), Constants.Action.Insert);
    }

    #endregion " Private Methods "
}
