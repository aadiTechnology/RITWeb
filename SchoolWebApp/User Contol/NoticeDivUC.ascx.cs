/* File Name = NoticeDivUC.aspx.cs
 * Created Date - 1 February 2012
 * Created by - Poonam
 * Class Description - This class is defined to manage User Control of Notices which are to be displayed on Home Page and Cotrol Pannel.*/

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;
using System.Web;

namespace SchoolWebApp
{

    public partial class NoticeDivUC : UserControl
    {
        #region "Const"

        private string S_TEXT_PATH = "../DOWNLOADS/School Notices/";
        private string S_TEXT_IMAGE_PATH = "../images/GridHeaderBG.gif";
        private string msServerPath = "RITeSchool/DOWNLOADS/School Notices/";
        private string msImagePath = "RITeSchool/images/GridHeaderBG.gif";
        private string S_CONTROL_PANEL = "ControlPanel.aspx";
        #endregion

        #region "Members"

        private string msDisolayLocation;
        public event EventHandler GetEvents;
        private List<Event> lstEventDetails = new List<Event>();
        #endregion "Members"

        #region "Properties"

        public string DisplayLocation
        {
            get { return msDisolayLocation; }
            set { msDisolayLocation = value; }
        }

        #endregion "Properties"

        #region "Events"

        /// <summary>
        /// This event is used to set default control fields and java script attributes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    string sFolderPath = Request.Url.ToString();
                    if (sFolderPath.Contains(S_CONTROL_PANEL))
                    {
                        msServerPath = S_TEXT_PATH;
                        msImagePath = S_TEXT_IMAGE_PATH;
                    }

                    hidSchoolId.Value = Resources.SchoolSettings.SchoolID;
                    FillNoticeListview();
                }

                SetJavaScriptAttribute();
            }
            catch (Exception ex)
            {
                string sErrMsg = string.Empty;
                if (Request.UrlReferrer != null)
                    sErrMsg = Request.UrlReferrer.AbsoluteUri;

                sErrMsg = sErrMsg + " : " + ex.Message + Constants.S_TRACE + ex.StackTrace;
                ExceptionHandler.WriteExceptionToErrorLog(ex.Message + Constants.S_TRACE + ex.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name, Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
            }
        }

        /// <summary>
        /// This event is used to set Parameter values for retriving correct data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ObjDSNotices_selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            try
            {
                e.InputParameters["asDisplayLocation"] = msDisolayLocation;
                e.InputParameters["aiSchoolId"] = Resources.SchoolSettings.SchoolID;
            }
            catch (Exception ex)
            {
                ExceptionHandler.WriteExceptionToErrorLog(ex.Message + Constants.S_TRACE + ex.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
                Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
            }
        }

        /// <summary>
        /// This event is used to set width of div.  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstvwNotices_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                HyperLink oHyperLink = e.Item.FindControl("hlnkNoticeH") as HyperLink;
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iLinkLength = oHyperLink.Text.Length;
                if (iLinkLength > 30)
                {
                    hidWidth.Value = "550px";
                    hidInnerWidth.Value = "520px";
                }

                if (e.Item.DisplayIndex > 0)
                {
                    hidHeight.Value = Convert.ToString((190 + (e.Item.DisplayIndex * 30)) + "px");
                    hidInnerHeight.Value = Convert.ToString((130 + (e.Item.DisplayIndex * 30)) + "px");
                }

                divInner.Style.Add("background-image", msImagePath);
                InnerDivHeader.Style.Add("background-image", msImagePath);
                oHyperLink.ForeColor = System.Drawing.Color.Blue;
                if (lstvwNotices.DataKeys[oCurrentItem.DisplayIndex]["FileName"] != "" && lstvwNotices.DataKeys[oCurrentItem.DisplayIndex]["FileName"] != null)
                {
                    oHyperLink.NavigateUrl = msServerPath + lstvwNotices.DataKeys[oCurrentItem.DisplayIndex]["FileName"].ToString();
                    oHyperLink.Attributes.Add("onclick", "window.open('" + oHyperLink.NavigateUrl
                                       + "' , '_blank','scrollbars=yes,resizable=no,top=0,left=0,width=800,height=600'); return false;");
                }
                else
                {
                    string sContent = HttpUtility.HtmlDecode(lstvwNotices.DataKeys[oCurrentItem.DisplayIndex]["NoticeContent"].ToString());

                    string sNoticeName = ((NoticeDetails)oCurrentItem.DataItem).NoticeName;
                    sNoticeName = sNoticeName.Replace("'", "\\'");
                    sContent = sContent.Replace("\n", "\\n");
                    sContent = sContent.Replace("'", "\"");
                    oHyperLink.Attributes.Add("onclick", "ShowNoticePopup('" + sContent + "','" + sNoticeName + "');");
                    oHyperLink.Style.Add("color", "blue");

                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
            }
        }

        /// <summary>
        /// This event is used to hide popoup if there is no record.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstvwNotices_DataBound(object sender, EventArgs e)
        {
            try
            {
                if (lstvwNotices.Items.Count <= Constants.I_ZERO)
                {
                    divSchoolNoticesLink.Visible = false;
                    hidSchoolNoticesLinkPopUp.Value = Constants.S_YES;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.WriteExceptionToErrorLog(ex.Message + Constants.S_TRACE + ex.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
                 Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
            }
        }

        #endregion "Events"

        #region "Private methods"

        /// <summary>
        /// This method is used to set java script attribute.
        /// </summary>
        private void SetJavaScriptAttribute()
        {
            new Button[] { btnCancelNotice }.ApplyEffect();
            btnCancelNotice.Attributes.Add("onclick", "HidePopupSchoolNoticesLink();return false;");
        }

        /// <summary>
        /// This method is used to fill list view.
        /// </summary>
        private void FillNoticeListview()
        {
            NoticeDetailsBL oNoticeDetailsBL = new NoticeDetailsBL();
            List<NoticeDetails> lstNotice = oNoticeDetailsBL.GetNotices(Convert.ToInt32(Resources.SchoolSettings.SchoolID), DisplayLocation, false, string.Empty, string.Empty, Constants.I_GRID_PAGE_COUNT, Constants.I_ZERO);

            lstvwNotices.DataSource = lstNotice;
            lstvwNotices.DataBind();
            if (GetEvents != null)
                GetEvents(oNoticeDetailsBL.lstEventDetails, new EventArgs());
        }

        #endregion
    }
}