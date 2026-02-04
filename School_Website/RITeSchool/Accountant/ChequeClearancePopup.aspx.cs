// File Name  : ChequeClearancePopup.aspx.cs
// Created By : Pallavi
// Date       : 8/12/2008
// Modified By : Milind
// Date        : 10 Sept 09   

using System;
using System.Web;
using System.Web.UI;
using BusinessLogic;
using Utility;

/// <summary>
/// This Class is used to add and edit cheque clearance date.
/// </summary>

public partial class ChequeClearancePopup : System.Web.UI.Page
{
    #region Event

    /// <summary>
    /// Event to set default values to controls.
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
                DecryptQueryString();
                if (txtClearanceDate.Text.Trim() != "")
                    btnRemove.Enabled = true;
                else
                    btnRemove.Enabled = false;
            }

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
    /// This event is used to save,update data & transfer control to HolidaysManagementConfiguration page on Sucess.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            StudentPostDatedChequesBL oStudentPostDatedChequesBL = GetStudentPostDatedChequesObject();
            oStudentPostDatedChequesBL.Cheque_Passed_Date = Convert.ToDateTime(txtClearanceDate.Text);
            oStudentPostDatedChequesBL.SetChequeClearance();
            CloseForm();
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
    /// This event is used to 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRemove_Click(object sender, EventArgs e)
    {
        try
        {
            StudentPostDatedChequesBL oStudentPostDatedChequesBL = GetStudentPostDatedChequesObject();
            oStudentPostDatedChequesBL.DeleteChequeClearance();
            CloseForm();
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
    /// This event is used to close the pop up.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            CloseForm();
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

    #region Private Method

    /// <summary>
    /// This method initialises variables.
    /// </summary>
    private void Initialise()
    {
        txtClearanceDate.Focus();
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        btnSave.Attributes.Add("onclick", "if(!ClearErrorLabel()){return false;}");
        hidServerDate.Value = Convert.ToString(DateTime.Today);
    }

    /// <summary>
    /// This method initialises variables.
    /// </summary>
    private void SetClientScriptAttributes()
    {
        btnSave.Attributes["onmouseover"] = "javascript:fnover('" + btnSave.ClientID + "');";
        btnSave.Attributes["onmouseout"] = "javascript:fnout('" + btnSave.ClientID + "');";
        btnClose.Attributes["onmouseover"] = "javascript:fnover('" + btnClose.ClientID + "');";
        btnClose.Attributes["onmouseout"] = "javascript:fnout('" + btnClose.ClientID + "');";
        btnRemove.Attributes["onmouseover"] = "javascript:fnover('" + btnRemove.ClientID + "');";
        btnRemove.Attributes["onmouseout"] = "javascript:fnout('" + btnRemove.ClientID + "');";
        btnRemove.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
    }

    /// <summary>
    /// This method is used to set fields of StudentPostDatedChequesBL.
    /// </summary>
    private StudentPostDatedChequesBL GetStudentPostDatedChequesObject()
    {
        StudentPostDatedChequesBL oStudentPostDatedChequesBL = new StudentPostDatedChequesBL();
        oStudentPostDatedChequesBL.PostDated_Cheque_Id = Convert.ToInt32(hidPDCId.Value);        
        oStudentPostDatedChequesBL.Updated_By_Id = Convert.ToInt32(Session[Constants.S_SESSION_USER_ID].ToString());
        return oStudentPostDatedChequesBL;
    }

    /// <summary>
    /// This method is used to decrypt query string.
    /// </summary>
    private void DecryptQueryString()
    {
        HttpRequest moHttprequest;
        string sQueryString = "";
        string sEventDateDecrypt = Server.UrlDecode(Request.QueryString.ToString());
        if (!sEventDateDecrypt.Equals(""))
            sQueryString = Utility.CommonUtility.DecryptQuerystring(sEventDateDecrypt);
        moHttprequest = new HttpRequest(Page.Request.FilePath.ToString(),
                                        Page.Request.Url.ToString(),
                                        sQueryString);
        //if (moHttprequest.QueryString.Get("StudentId") != null)
        //{
        hidPDCId.Value = moHttprequest.QueryString.Get("ChequeID").ToString();
        if (!moHttprequest.QueryString.Get("cDate").ToString().Equals(""))
        {
            //txtClearanceDate.Text = Convert.ToDateTime(moHttprequest.QueryString.Get("cDate").ToString()).ToShortDateString();
            calClearanceDate.DateValue = Convert.ToDateTime(moHttprequest.QueryString.Get("cDate").ToString());
        }

        DateTime dtDate = Convert.ToDateTime(moHttprequest.QueryString.Get("ChequeDate").ToString());

        string sdate = dtDate.ToString(Constants.S_STANDARD_DATE_FORMAT);
        lblChequeNumber.Text = moHttprequest.QueryString.Get("Number").ToString();
        lblChequeDate.Text = sdate.Replace(' ', '-');
        lblBankName.Text = moHttprequest.QueryString.Get("BankName").ToString();
        hidCategoryName.Value = moHttprequest.QueryString.Get("CategoryName").ToString();
        hidCategoryValue.Value  = moHttprequest.QueryString.Get("CategoryValue").ToString();
        hidIncludeChequeFlag.Value = moHttprequest.QueryString.Get("InculdeChequeFlag").ToString();
        hidPageIndex.Value = moHttprequest.QueryString.Get("PageIndex").ToString();
        lblRegNo.Text = moHttprequest.QueryString.Get("EnrolmentNo").ToString();
        lblStudentClass.Text = moHttprequest.QueryString.Get("StandardName").ToString() + " " + moHttprequest.QueryString.Get("DivisionName").ToString();
        //lblStudentName.Text = moHttprequest.QueryString.Get("FirstName").ToString() + " " + moHttprequest.QueryString.Get("MiddleName").ToString() + " " + moHttprequest.QueryString.Get("LastName").ToString();
        lblStudentName.Text = moHttprequest.QueryString.Get("StudentName").ToString();
    }

    /// <summary>
    /// This method is used to close the pop up.
    /// </summary>
    private void CloseForm()
    {
        //string sQueryString = "StudentId=" + hidStudentId.Value;
        //string sEncryptQueryString = Utility.CommonUtility.EncryptQuerystring(sQueryString);
        //sQueryString = "'?" + sEncryptQueryString + "'";
        //Response.Write("<Script language='Javascript'>window.opener.location=window.opener.location.pathname+" + sQueryString + ";window.opener.focus(); ");

        string sQueryString = "CategoryName=" + hidCategoryName.Value + "&CategoryValue=" + hidCategoryValue.Value + "&InculdeChequeFlag=" + hidIncludeChequeFlag.Value + "&PageIndex=" + hidPageIndex.Value;
        string sEncryptQueryString = Utility.CommonUtility.EncryptQuerystring(sQueryString);
        sQueryString = "'?" + sEncryptQueryString + "'";

        Response.Write("<Script language='Javascript'>window.opener.location=window.opener.location.pathname+" + sQueryString  + ";window.close();window.opener.focus();</Script>");
    }

    #endregion
   
}

