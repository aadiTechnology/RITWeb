/* File NAme - SurveyUserListUI.aspx.cs
 * Creator Name - Sachin
 * Created Date - 09-Nov-2015
 * Description - This class is used to display all users.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.Services;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Kendo.DynamicLinq;
using SchoolEntities;
using Utility;

public partial class SurveyUserListUI : SchoolBase
{
    #region Data Member(s)
    
    private UserSurveyBL moUserSurveyBL; 

    #endregion

    #region Event(s)

    /// <summary>
    /// THis event is used to fill all combo boxes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moUserSurveyBL = new UserSurveyBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                SetJavascriptAttributes();
                FillSurveyDetails();
                FillUserRoles();
                ReadQuerystring();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    } 

    #endregion

    #region Private Method(s)

    /// <summary>
    /// This method is used to read query string.
    /// </summary>
    private void ReadQuerystring()
    {
        if (QueryString["SurveyId"] != null)
        {
            cmbSurvey.SelectedValue = QueryString["SurveyId"].ToString();
            hidSurvey.Value = QueryString["SurveyId"].ToString();
        }

        if (QueryString["UserRoleId"] != null)
        {
            cmbUserRole.SelectedValue = QueryString["UserRoleId"].ToString();
            hidUserRole.Value = QueryString["UserRoleId"].ToString();
        }

        if (QueryString["Filter"] != null)
        {
            txtSearch.Text = QueryString["Filter"].ToString();
            hidFilter.Value = QueryString["Filter"].ToString();
        }
    }

    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        hidSchoolId.Value = miSchoolId.ToString();
        hidAcademicYearId.Value = miAcademicYearId.ToString();
        base.ApplyMouseHoverEffect(new List<Button> { btnShow });
    }

    /// <summary>
    /// This method is used to fill survey details.
    /// </summary>
    private void FillSurveyDetails()
    {
        List<SurveyConfig> lstSurveys = moUserSurveyBL.GetAllSurveys();
        ListSource.FillDropDownList(lstSurveys, cmbSurvey, "SurveyName", "Id", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is sued to fill user role combo box.
    /// </summary>
    private void FillUserRoles()
    {
        DataTable oDT = moUserSurveyBL.GetAllUserRoles();
        ListSource.FillDropDownList(oDT, cmbUserRole, "User_Role_Name", "User_Role_Id", Constants.S_SELECT);
    } 

    #endregion

    #region Public Method(s)

    [WebMethod]
    public static DataSourceResult GetAllUsers(int take, int skip, IEnumerable<Sort> sort, int aiSchoolId, int aiAcademicYearId, int aiSurveyId, int aiUserRoleId, string asFilter)
    {
        int iStartIndex = skip;
        int iEndIndex = skip + take;
        List<SurveyUserDetails> lstUsers = UserSurveyBL.GetAllUsers(aiSchoolId, aiAcademicYearId, aiSurveyId, asFilter, aiUserRoleId, iStartIndex, iEndIndex);
        int iTotalCount = (lstUsers.Count == 0 ? 0 : lstUsers[0].TotalRecordCount);

        var result = new DataSourceResult()
        {
            Data = lstUsers,
            Total = iTotalCount
        };
        return result;
    }

    [WebMethod]
    public static string ReadQuerystring(int aiSurveyId, int aiUserId, string asFilter, int aiUserRoleId)
    {
        return "UserSurveyDetailsUI.aspx?" + CommonUtility.EncryptQuerystring("SurveyId=" + aiSurveyId + "&UserId=" + aiUserId + "&Filter=" + asFilter + "&UserRoleId=" + aiUserRoleId);
    } 

    #endregion

}