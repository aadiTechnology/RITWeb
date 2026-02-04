/* File Name :- MobileAppDetailsIUI.aspx.cs
 * Created Date :- 31-May-2018
 * Class Description :- This class is used to display monthwise mobile and website user count. 
 * Created By :- Dnyaneshwar Shinde.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using BusinessLogic;
using System.Data;

public partial class MobileAppDetailsIUI : SchoolBase
{
    #region DataMember

    private SchoolBL moSchoolBL;
    
    #endregion

    #region Events

    /// <summary>
    /// This event is used to load all controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moSchoolBL = new SchoolBL();
            GetMobileAppDownloadCount();
            GetMonthwiseLoginCountDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to get the mobile app download count.
    /// </summary>
    private void GetMobileAppDownloadCount()
    {
        lblStudentCount.Text = moSchoolBL.GetMobileUserDetails(miSchoolId);
    }

    /// <summary>
    /// This method is used to get the monthwise login user count details of mobile app ans school website.
    /// </summary>
    private void GetMonthwiseLoginCountDetails()
    {
        DataTable dtLoginDetails = moSchoolBL.GetLoginDetailsForFeatureUsage();
        lstvwLoginDetails.DataSource = dtLoginDetails;
        lstvwLoginDetails.DataBind();
    }

    #endregion
}