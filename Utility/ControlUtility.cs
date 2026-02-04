/* Class Name   :   ControlUtility
 * Purpose      :   This class contains all the functions related to controls present on the pages.
 *                  General functionalities related to treeview filling, combobox filling etc. are available here.
 * Date Creation:   27 Feb 2007
*/

using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

namespace Utility
{
    public class ControlUtility
    {
        #region Public Static Methods

        /// <summary>
        /// This method is used fill the datapager dropdown list in the list view.
        /// Pager control name should be same as defined here.
        /// e.g. DtPgDropDown is the datapager name which contains the drop down list.
        /// Same for drop down list in the pager control as well as label
        /// </summary>
        public static void FillPageNoCombo(ListView oListView, DataPager oPgCntDataPager)
        {
            DataPager oDataPager = oListView.FindControl("DtPgDropDown") as DataPager;
            HtmlTable otblDataPager = oListView.FindControl("tblDataPager") as HtmlTable;
            otblDataPager.Visible = false;
            oPgCntDataPager.Visible = false;
            int iCurrentPage = (oDataPager.StartRowIndex / oDataPager.PageSize) + 1;
            int iTotalPages = oDataPager.TotalRowCount / oDataPager.PageSize;
            if (iTotalPages * oDataPager.PageSize < oDataPager.TotalRowCount)
                iTotalPages += 1;

            if (iTotalPages > 1)
            {
                otblDataPager.Visible = true;
                oPgCntDataPager.Visible = true;
                //Populate the DropDownList if needed
                DropDownList ddlCount = (oDataPager.Controls[0].FindControl("ddlCnt")) as DropDownList;

                if (ddlCount.Items.Count == 0)
                {
                    //Add a list item for each page
                    for (int iddlCount = 1; iddlCount <= iTotalPages; iddlCount++)
                        ddlCount.Items.Add(iddlCount.ToString());


                    //Set the DDL to the appropriate page value
                    ddlCount.Items.FindByValue(iCurrentPage.ToString()).Selected = true;
                    Label oLabel = (oDataPager.Controls[0].FindControl("CurrentPageLabel")) as Label;
                    oLabel.Font.Bold = true;
                    oLabel.Text = "Page " + iCurrentPage + " of " + iTotalPages;
                }
            }
        }


        /// <summary>
        /// This method is used fill the datapager dropdown list in the list view.
        /// Pager control name should be same as defined here.
        /// e.g. DtPgDropDown is the datapager name which contains the drop down list.
        /// Same for drop down list in the pager control as well as label
        /// </summary>
        public static void FillListViewPagerFooter(ListView oListView, DataPager oPgCntDataPager)
        {
            DataPager oDataPager = oListView.FindControl("DtPgDropDown") as DataPager;
            HtmlTableRow otrDataPager = oListView.FindControl("trDataPager") as HtmlTableRow;
            otrDataPager.Visible = false;
            oPgCntDataPager.Visible = false;
            int icurrentPage = (oDataPager.StartRowIndex / oDataPager.PageSize) + 1;
            int itotalPages = oDataPager.TotalRowCount / oDataPager.PageSize;
            if (itotalPages * oDataPager.PageSize < oDataPager.TotalRowCount)
                itotalPages += 1;

            if (itotalPages > 1)
            {
                otrDataPager.Visible = true;
                oPgCntDataPager.Visible = true;
                //Populate the DropDownList if needed
                DropDownList ddlCnt = (oDataPager.Controls[0].FindControl("ddlCnt")) as DropDownList;

                if (ddlCnt.Items.Count == 0)
                {
                    //Add a list item for each page
                    for (int i = 1; i <= itotalPages; i++)
                        ddlCnt.Items.Add(i.ToString());

                    //Set the DDL to the appropriate page value
                    ddlCnt.Items.FindByValue(icurrentPage.ToString()).Selected = true;

                    Label lblCurrentPageLabel = (oDataPager.Controls[0].FindControl("CurrentPageLabel")) as Label;
                    lblCurrentPageLabel.Font.Bold = true;
                    lblCurrentPageLabel.Text = "Page " + icurrentPage + " of " + itotalPages;
                }
            }
        }

        /// <summary>
        /// This method is used fill the datapager dropdown list in the list view.
        /// Pager control name should be same as defined here.
        /// e.g. DtPgDropDown is the datapager name which contains the drop down list.
        /// Same for drop down list in the pager control as well as label
        /// </summary>
        public static void FillListViewPagerFooterWithCulture(ListView oListView, DataPager oPgCntDataPager, string sPageNoText, string sOfText, string sOutOfTExt)
        {
            DataPager oDataPager = oListView.FindControl("DtPgDropDown") as DataPager;
            HtmlTableRow otrDataPager = oListView.FindControl("trDataPager") as HtmlTableRow;
            otrDataPager.Visible = false;
            oPgCntDataPager.Visible = false;
            int icurrentPage = (oDataPager.StartRowIndex / oDataPager.PageSize) + 1;
            int itotalPages = oDataPager.TotalRowCount / oDataPager.PageSize;
            if (itotalPages * oDataPager.PageSize < oDataPager.TotalRowCount)
                itotalPages += 1;

            if (itotalPages > 1)
            {
                otrDataPager.Visible = true;
                oPgCntDataPager.Visible = true;
                //Populate the DropDownList if needed
                DropDownList ddlCnt = (oDataPager.Controls[0].FindControl("ddlCnt")) as DropDownList;
                ddlCnt.Items.Clear();
                if (ddlCnt.Items.Count == 0)
                {
                    //Add a list item for each page
                    for (int i = 1; i <= itotalPages; i++)
                        ddlCnt.Items.Add(i.ToString());

                    //Set the DDL to the appropriate page value
                    ddlCnt.Items.FindByValue(icurrentPage.ToString()).Selected = true;

                    Label lblCurrentPageLabel = (oDataPager.Controls[0].FindControl("CurrentPageLabel")) as Label;
                    lblCurrentPageLabel.Font.Bold = true;
                    lblCurrentPageLabel.Text = sPageNoText + " " + icurrentPage + " " + sOfText + " " + itotalPages + " " + sOutOfTExt;
                }
            }
        }

        /// <summary>
        /// This method is used to set list view according selected page from the pager dropdownlist.
        /// Pager control name should be same as defined here.
        /// e.g. DtPgDropDown is the datapager name which contains the drop down list.
        /// Same for drop down list in the pager control as well as label
        /// </summary>
        public static void SetDataPagerAccordingToPageNo(ListView oListView)
        {
            DataPager oDtPgDropDown = oListView.FindControl("DtPgDropDown") as DataPager;
            DropDownList oddlCnt = (oDtPgDropDown.Controls[0].FindControl("ddlCnt")) as DropDownList;
            int iRowIndex = (Convert.ToInt32(oddlCnt.SelectedValue) - 1) * oDtPgDropDown.PageSize;

            oDtPgDropDown.SetPageProperties(iRowIndex, oDtPgDropDown.PageSize, true);

            int icurrentPage = (oDtPgDropDown.StartRowIndex / oDtPgDropDown.PageSize) + 1;
            int itotalPages = oDtPgDropDown.TotalRowCount / oDtPgDropDown.PageSize;

            Label lblCurrentPageLabel = (oDtPgDropDown.Controls[0].FindControl("CurrentPageLabel")) as Label;
            lblCurrentPageLabel.Text = "Page " + icurrentPage + " of " + itotalPages;
        }


        /// <summary>
        /// This method is used to set list view according selected page and culture from the pager dropdownlist.
        /// Pager control name should be same as defined here.
        /// e.g. DtPgDropDown is the datapager name which contains the drop down list.
        /// Same for drop down list in the pager control as well as label
        /// </summary>
        public static void SetDataPagerAccordingToPageNoAndCulture(ListView oListView, string sPageNoText, string sOfText, string sOutOfTExt)
        {
            DataPager oDtPgDropDown = oListView.FindControl("DtPgDropDown") as DataPager;
            DropDownList oddlCnt = (oDtPgDropDown.Controls[0].FindControl("ddlCnt")) as DropDownList;
            int iRowIndex = (Convert.ToInt32(oddlCnt.SelectedValue) - 1) * oDtPgDropDown.PageSize;

            oDtPgDropDown.SetPageProperties(iRowIndex, oDtPgDropDown.PageSize, true);

            int icurrentPage = (oDtPgDropDown.StartRowIndex / oDtPgDropDown.PageSize) + 1;
            int itotalPages = oDtPgDropDown.TotalRowCount / oDtPgDropDown.PageSize;

            Label lblCurrentPageLabel = (oDtPgDropDown.Controls[0].FindControl("CurrentPageLabel")) as Label;
            lblCurrentPageLabel.Text = sPageNoText + " " + icurrentPage + " " + sOfText + " " + itotalPages + " " + sOutOfTExt;
        }

        public static void FillDropDownList(System.Data.DataTable aoDataTable, ref  DropDownList aoDropDownList, string asValueMember, string asDisplayMember, string asTopElement)
        {
            // This method accepts parameters as aoDataReader, aoDropDownList, asValueMember, asDisplayMember, abSort.
            // It fills the dropdown list with the specified data given.

            // Check if top element is passed, then add it to the list.
            aoDropDownList.AppendDataBoundItems = true;
            aoDropDownList.Items.Clear();

            if (asTopElement != "")
                aoDropDownList.Items.Add(new ListItem(asTopElement, "0"));
            aoDropDownList.DataTextField = asDisplayMember;
            aoDropDownList.DataValueField = asValueMember;

            aoDropDownList.DataSource = aoDataTable;
            aoDropDownList.DataBind();
            
        }
        public static void FillDropDownList(System.Data.DataRow[] aoDataRows, ref  DropDownList aoDropDownList, string asValueMember, string asDisplayMember, string asTopElement)
        {
            // This method accepts parameters as aoDataReader, aoDropDownList, asValueMember, asDisplayMember, abSort.
            // It fills the dropdown list with the specified data given.

            // Check if top element is passed, then add it to the list.
            aoDropDownList.Items.Clear();

            if (asTopElement != "")
                aoDropDownList.Items.Add(new ListItem(asTopElement, "0"));

            if (aoDataRows.Length > 0)
            {
                for (int iCount = 0; iCount <= aoDataRows.Length - 1; iCount++)
                    aoDropDownList.Items.Add(new ListItem(aoDataRows[iCount][asDisplayMember].ToString(), aoDataRows[iCount][asValueMember].ToString()));
            }
        }
        public static void FillCheckBoxList(System.Data.DataTable aoDataTable, ref CheckBoxList aoCheckBoxList, string asValueMember, string asDisplayMember, bool abDefaultCheck)
        {
            // This method accepts parameters as aoDataReader, aoListBox, asValueMember, asDisplayMember, abSort.
            // It fills the dropdown list with the specified data given.

            // Check if top element is passed, then add it to the list.
            aoCheckBoxList.Items.Clear();
           
            for (int iCount = 0; iCount <= aoDataTable.Rows.Count - 1; iCount++)
            {
                aoCheckBoxList.Items.Add(new ListItem(aoDataTable.Rows[iCount][asDisplayMember].ToString(), aoDataTable.Rows[iCount][asValueMember].ToString()));
                aoCheckBoxList.Items[iCount].Selected = abDefaultCheck;
            }
            
        }

        public static string GetWebRequestResult(string sPostString, string asURL)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] ArrMessage = encoding.GetBytes(sPostString);

            // Web request to call the service is created.
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            HttpWebRequest oRequest = (HttpWebRequest)WebRequest.Create(asURL);
            oRequest.Method = "POST";
            oRequest.ContentType = "application/x-www-form-urlencoded";
            oRequest.ContentLength = ArrMessage.Length;
            Stream oRequestStream = oRequest.GetRequestStream();
            oRequestStream.Write(ArrMessage, 0, ArrMessage.Length);
            WebResponse oWebResponse = oRequest.GetResponse();
            Stream oResponseMessage = oWebResponse.GetResponseStream();
            using (StreamReader oStreamReader = new StreamReader(oResponseMessage))
            {
                return oStreamReader.ReadToEnd();
            }
        }

        #endregion
    }


    public class FileUtility
    {
        #region Constant(s)

        private const string S_FILE_SIZE_ERROR_MESSAGE = " File size exceeds for row(s) :";
        private const string S_FILE_TYPE_ERROR_MESSAGE = " Invalid file type for row(s) : ";
        private const string S_FILE_SIZE = "FS";
        private const string S_FILE_TYPE = "FT";

        #endregion

        #region Data Member(s)

        private Dictionary<int, string> moErrorMessages = new Dictionary<int, string>();

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to set error message.
        /// </summary>
        public void SetErrorMessage()
        {
            if (moErrorMessages.Count > 0)
            {
                string sMessage = string.Empty;
                StringBuilder oFileISizeExceedNos = new StringBuilder();
                StringBuilder oFileITypeNos = new StringBuilder();

                foreach (KeyValuePair<int, string> kvp in moErrorMessages)
                {
                    if (kvp.Value == S_FILE_SIZE)
                        oFileISizeExceedNos.Append(", " + kvp.Key);
                    if (kvp.Value == S_FILE_TYPE)
                        oFileITypeNos.Append(", " + kvp.Key);
                }

                if (oFileITypeNos.Length > 0)
                    sMessage = S_FILE_TYPE_ERROR_MESSAGE + oFileITypeNos.ToString().Substring(1);

                if (oFileISizeExceedNos.Length > 0)
                {
                    if (sMessage != string.Empty)
                        sMessage = sMessage + "<BR />" + S_FILE_SIZE_ERROR_MESSAGE + oFileISizeExceedNos.ToString().Substring(1);
                    else
                        sMessage = S_FILE_SIZE_ERROR_MESSAGE + oFileISizeExceedNos.ToString().Substring(1);
                }
                moErrorMessages.Clear();
                if (sMessage != string.Empty)
                    throw new ApplicationException(sMessage);
            }
        }

        /// <summary>
        /// This method is used to validate file.
        /// </summary>
        /// <param name="asServerFilePath"></param>
        /// <param name="aiRowIndex"></param>
        /// <returns>Error message</returns>
        public string ValidateFile(string asServerFilePath, int aiRowIndex, List<string> alstExtensions, int aiFileSizeLimit)
        {
            string sReturnErrorMsg = string.Empty;
            FileInfo oFile = new FileInfo(asServerFilePath);
            string sFileName = oFile.Name;
            if (oFile.Length > aiFileSizeLimit)
            {
                moErrorMessages.Add(aiRowIndex, S_FILE_SIZE);
                sReturnErrorMsg = S_FILE_SIZE;
            }

            if (alstExtensions.FindAll(ext => sFileName.ToUpper().EndsWith(ext)).Count == 0)
            {
                moErrorMessages.Add(aiRowIndex, S_FILE_TYPE);
                sReturnErrorMsg = S_FILE_TYPE;
            }

            oFile = null;
            return sReturnErrorMsg;
        }

        #endregion
    }
}
