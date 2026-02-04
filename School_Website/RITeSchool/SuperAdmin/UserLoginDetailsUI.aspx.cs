// File Name  : UserLoginDetailsUI.aspx.cs
// Created By : Yogesh
// Date       : 16/Oct/2015
// Description :This class is used to export login details in excel sheet.

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;
using System.Threading;

public partial class UserLoginDetailsUI : ExportDataTable
{

    #region Constant(s)

    private const string S_NO_RECORD_FOUND = "No Record(s) Found.";
    const string S_SCREENS_URL = "ScreensUI.aspx";
    static string msFromUrl = string.Empty;
    #endregion

    #region Event(s)

    protected override void OnPreInit(EventArgs e)
    {
        try
        {
            base.OnPreInit(e);

            if (!IsPostBack)
                msFromUrl = GetFromPageUrl();

            string sFromPage = string.Empty;

            if (Request.QueryString.ToString() != string.Empty)
            {
                if (QueryString["FromPage"] != null)
                    sFromPage = QueryString["FromPage"];
            }

            if (msFromUrl.Equals(S_SCREENS_URL) || sFromPage == S_SCREENS_URL)
                this.Page.MasterPageFile = "~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master";
            else
                this.Page.MasterPageFile = "../MasterPages/MasterPage.master";

            if (sFromPage == S_SCREENS_URL)
                msFromUrl = sFromPage;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }


    /// <summary>
    /// This method is used to ger referrence page URL.
    /// </summary>
    /// <returns></returns>
    private string GetFromPageUrl()
    {
        string sSourcePageUrl = string.Empty;
        if (Request.UrlReferrer != null)
        {
            sSourcePageUrl = Request.UrlReferrer.AbsolutePath;
            sSourcePageUrl = sSourcePageUrl.Substring(sSourcePageUrl.LastIndexOf("/") + 1);
        }
        return sSourcePageUrl;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                FillUserRoleCombo();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    /// <summary>
    /// This event will fired while export button will clicked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            SchoolUserBL oSchoolUserBL = new SchoolUserBL();
            List<UserLoginDetails> lstUserLoginDetails = oSchoolUserBL.GetLoginDetails(miSchoolId, miAcademicYearId, ddlUserRole.SelectedValue.ToInt());
            if (lstUserLoginDetails.Count == 0)
            {
                lblErrorMsg.Visible = true;
                lblErrorMsg.Text = S_NO_RECORD_FOUND;
            }
            else
            {
                DataTable Odt = new DataTable();
                Odt = ConvertToDatatable(lstUserLoginDetails);
                ExportToExcel("UserLoginDetails.xls", Odt);
                lblErrorMsg.Visible = true;
                lblErrorMsg.Text = string.Empty;
            }
        }
        catch (ThreadAbortException)
        {
        }   
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method(s)

    /// <summary>
    /// This method is used to fill user role combo.
    /// </summary>
    private void FillUserRoleCombo()
    {
        // Fill the user role's combobox with all the user roles available in the system.
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        DataTable oDSStateCollection = oMasterDataCollectionBL.GetAllUserRoles();
        ControlUtility.FillDropDownList(oDSStateCollection.Select("User_Role_Id <> " + Constants.UserRoles.Parent.ToInt() + " AND User_Role_Id <> " + Constants.UserRoles.ExAdmin.ToInt()+" AND User_Role_Id <> " + Constants.UserRoles.TransportStaff.ToInt()), ref ddlUserRole, Constants.S_USER_ROLE_ID_FIELD, Constants.S_USER_ROLE_NAME_FIELD, Constants.S_SELECT_ALL);
    }

    /// <summary>
    /// This method is used to convert list into datatable.
    /// </summary>
    /// <param name="lstUserLoginDetails"></param>
    /// <returns></returns>
    private DataTable ConvertToDatatable(List<UserLoginDetails> lstUserLoginDetails)
    {
        DataTable dt = new DataTable();
        
        if (ddlUserRole.SelectedValue.ToInt() == Constants.UserRoles.Student.ToInt() || ddlUserRole.SelectedValue.ToInt() == Constants.I_ZERO)
        {
            string[] Studentcolumns = {"<b>Class Name<b/>","<b>Name<b/>","<b>Mobile Number1<b/>","<b>Mobile Number2<b/>","<b>User Name<b/>","<b>Password<b/>"};
            dt.AddColumns(Studentcolumns);
            foreach (var LoginDetails in lstUserLoginDetails)
            {
                dt.Rows.Add(LoginDetails.ClassName, LoginDetails.UserName, LoginDetails.MobileNumber1, LoginDetails.MobileNumber2, LoginDetails.UserLogin, LoginDetails.Password);
            }
        }
        else
        {
            string [] Staffcolumns = {"<b>Name<b/>","<b>Mobile Number1<b/>","<b>Mobile Number2<b/>","<b>User Name<b/>","<b>Password<b/>"};
            dt.AddColumns(Staffcolumns);
            foreach (var LoginDetails in lstUserLoginDetails)
            {
                dt.Rows.Add(LoginDetails.UserName, LoginDetails.MobileNumber1, LoginDetails.MobileNumber2, LoginDetails.UserLogin, LoginDetails.Password);
            }
        
        }
        return dt;
    }

    #endregion
   
}