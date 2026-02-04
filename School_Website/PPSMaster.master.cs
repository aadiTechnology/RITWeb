/* File Name :- OxfordMaster.master.cs
 * Modified By :- Sachin
 * Modified Date :- 17-Sept-2009
 * Purpose :- Code Review.
 * Class Description :- This class is used to display site updation date and external event link.
*/
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Reflection;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class PPSMaster : System.Web.UI.MasterPage
{
    #region Event

    /// <summary>
    /// This event is used to display site updation date.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //hlnkLoginPage.HRef = Resources.SchoolSettings.ResourceManager.GetString("SubDomainLoginUrl");
            DirectoryInfo oDirectoryInfo = new DirectoryInfo(Server.MapPath(".").ToString() + "\\bin");
            if (oDirectoryInfo.GetFiles().Length > 0)
            {
                DateTime oDtLastUpdatetime = oDirectoryInfo.GetFiles()[0].LastWriteTime;
                foreach (FileInfo oFileInfo in oDirectoryInfo.GetFiles())
                {
                    DateTime oDateTime = oFileInfo.LastWriteTime;
                    if (oDtLastUpdatetime.Date < oDateTime.Date)
                        oDtLastUpdatetime = oDateTime;
                }

                GetMaxEndDateforNotice();
                lblLastUpdateDate.Text = String.Format("Site Update Date: {0}", oDtLastUpdatetime.ToString("dd MMM yyyy hh:mm tt"));
            }
            //if (!IsPostBack)
            //    GetExternalEvents();
            hidServerDate.Value = Convert.ToString(DateTime.Now.Date.Year);
            
            // Enable Online Admission link based on the current date & time.
            // The value for it will change every year.
            //DateTime dtOpenDate = DateTime.Parse("3-Dec-2011 18:00:00");
            //if(DateTime.Now >= dtOpenDate)
            //{
            //    hlnkOnlineAdmission.Visible = true;
            //    imgNew.Visible = true;
            //}
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex,MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to redirect towards login page on click of login link.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnRITLogin_Login(object sender, EventArgs e)
    {
        try
        {
			Response.Redirect(SchoolBase.Settings.SubDomainLoginUrl, false);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    } 

    #endregion

    #region Method

    /// <summary>
    /// This method is used to display link of latest active external event.
    /// </summary>
    private void GetExternalEvents()
    {
        //DataTable oDataTable = ConfigureMenuBL.FetchMenuContentDetails();
        //if (oDataTable != null && oDataTable.Rows.Count > 0 && oDataTable.Rows[0][0] != DBNull.Value)
        //{
        //    hlnkExternalEvents.Visible = true;
        //    tdNew.Visible = true;
        //    hlnkExternalEvents.Text = oDataTable.Rows[0]["ConfigureMenuName"].ToString();
        //}
        //else
        //{
        //    hlnkExternalEvents.Visible = false;
        //    tdNew.Visible = false;
        //}
        //tdNew.Visible = true;
    }

    /// <summary>
    /// This method is used to show or hide new image for notice. 
    /// </summary>
    private void GetMaxEndDateforNotice()
    {
        DateTime dtMaxEndDate = NoticeDetailsBL.GetMaxEndDate();
        imgNewNotice.Visible = dtMaxEndDate >= DateTime.Today;
    }
    #endregion
}

