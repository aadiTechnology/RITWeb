/* File Name :- Home.aspx.cs
 * Modified By :- Sachin
 * Modified Date :- 18-Sept-2009
 * Purpose :- Code Review.
 * Class Description :- This class is used to authenticate user details.
*/
using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using BusinessLogic;
using Utility;
using System.Collections.Generic;
using BusinessLogic.Exceptions;
using System.Reflection;

public partial class Home : SchoolBase
{
    #region Events

    /// <summary>
    /// This event is used to set default values and javascript attributes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultValues();
            if (!IsPostBack)
            {
                valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
                this.Page.Title = Constants.S_TITLE_FOR_PAGE;
                Session.Clear();
                Session.Abandon();                
            }
            SetJavascriptAttributes();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
   
    /// <summary>
    /// This event is used to authenticate user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
    {
        try
        {
            string sLogin = Login1.UserName.Trim();
            string sPassword = Login1.Password.Trim();
            int iSchoolId = Convert.ToInt32(cmbSchools.SelectedValue);
            string sIPAddress = Request.UserHostAddress.ToString();

            UserAuthentication oUserAuthentication = new UserAuthentication(iSchoolId, sLogin, sPassword, sIPAddress);

            if (oUserAuthentication.ValidUser && !oUserAuthentication.Locked)
            {
                if (!oUserAuthentication.TermAccepted)
                {
                    string sQuerystring = "login=true&sLogin=" + sLogin + "&sPassword=" + sPassword + "&iSchoolId=" + iSchoolId;
                    string sEncrypt = Utility.CommonUtility.EncryptQuerystring(sQuerystring);
                  Server.Transfer("~/TermsOfUse.aspx?" + sEncrypt, false);
                }
                else
                {
                    oUserAuthentication.UpdateSession();
                    Response.Redirect("Common/ControlPanel.aspx", false);
                }
            }
            else if (oUserAuthentication.Locked)
                Login1.FailureText = "Your account is locked. Please contact school administrator.";
            else
                Login1.FailureText = "You are not authenticated user.";
        }
        catch (AcademicYearException ex)
        {
            Login1.FailureText = ex.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }
  
    #endregion

    #region Members

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        TextBox otxtPassword = (TextBox)Login1.Controls[0].FindControl("Password");
        otxtPassword.Attributes.Add("onfocus", "SelectText();");
        otxtPassword.Attributes.Add("value", "111111");
        ApplyMouseHoverEffect(new List<Button> { ((Button)this.FindControl("Login1$LoginButton")) }); 
    }

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        Login1.UserName = "144";
        cmbSchools.Focus();
        form1.DefaultButton = this.FindControl("Login1$LoginButton").UniqueID;
        trSchoolName.Visible = false;
		cmbSchools.Items.Add(new ListItem(ConfigurationManager.AppSettings["SchoolID"], ConfigurationManager.AppSettings["SchoolID"]));
        cmbSchools.SelectedIndex = 1;
        Login1.FailureText = "";
    }
  
    /// <summary>
    /// This method is used to fill school combobox.
    /// </summary>
    private void FillSchoolCombo()
    {
        DataTable oDTSchool = MasterDataCollectionBL.GetAllSchools();
        ControlUtility.FillDropDownList(oDTSchool,
                                        ref cmbSchools,
                                        "School_id",
                                        "school_Name",
                                        "-- Select school --");
        Session["S_SCHOOL_NAMES"] = oDTSchool;
    }

    #endregion
}
