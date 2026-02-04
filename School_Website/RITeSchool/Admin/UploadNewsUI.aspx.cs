using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;
/// <summary>
/// This class is used to insert, update and delete the NEWS.
/// </summary>
public partial class UploadNewsUI : SchoolBase
{
    #region "Constants"

    private const string S_DEFAULT_SORT_EXP = "StartDate";
    private const string S_SAVE_STATEMENT = "News saved successfully !!!";
    private const string S_UPDATE_STATEMENT = "News updated successfully !!!";
    private const string S_FOLDER_PATH = @"../DOWNLOADS/School News/";
    private const string S_FOLDER_LOCATION = "RITeSchool\\DOWNLOADS\\School News\\";
    private const string S_COMMAND_UPDATE_NOTICE = "UpdateNews";
    private const string S_FILE_NOT_FOUND = "File does not Exists.";
    private const string S_COMMAND_DELETE_NOTICE = "DeleteNews";   
    private const string S_ADD_UPDATE = "AddUpdate";
    private const string S_FILE_SIZE_ERROR = "Size of file is too large.";
    private const int I_FILE_SIZE_LIMIT = 524288;  // File limit is 500 KB
    private const string S_DUPLICATE_LINK_NAME = "News name already exists.";
    private const string S_DELETE_MSG = "News deleted successfully !!!";
    private const string S_TEXT_SAVE = "Save";
    private const string S_TEXT_UPDATE = "Update";
    private const string S_FILE_EXIST = "File already exists.";
    private const string S_SAVE_SELECTED_NOTICE = "Selected new(s) saved successfully !!!";
    private const string S_BLANK_MSG = "News content should not be blank.";
    private const string S_BLANK_FILE_MSG = "File to be uploaded should be selected.";

    #endregion "Constants"

    #region Member(s)

    SchoolNewsBL moSchoolNewsBL;

    #endregion

    #region Properties

    public int IsTextType
    {
        get
        {
            if (optText.Checked)
                return Constants.I_ONE;
            else
                return Constants.I_ZERO;
        }

    }
    #endregion

    #region Event(s)

    /// <summary>
    /// This class is used to initialize news controls and binding data to listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {   
            InitializeMemberVariables();
            moSchoolNewsBL = new SchoolNewsBL(miSchoolId, miUserId);
           
            if (!IsPostBack)
            {
                base.SetDocType();                
                SetJavaScriptAttributes();
                FillNewsDetailGridView();
            }
            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This class is used to update and delete the news.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwNewsDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = oCurrentItem.DisplayIndex;
                int iNewsId = Convert.ToInt32(lstvwNewsDetails.DataKeys[iRowId]["NewsId"]);
                hidRowNo.Value = (oCurrentItem.DisplayIndex + 1).ToString();
                hidNewsId.Value = iNewsId.ToString();
                NewsDetails oNewsDetails = moSchoolNewsBL.Get(iNewsId);

                if (e.CommandName == S_COMMAND_UPDATE_NOTICE)
                {
                    
                    if (oNewsDetails != null)
                    {
                        txtNewsHeading.Text = oNewsDetails.NewsHeading;
                        txtNewsDate.Text = oNewsDetails.NewsDate.ToDateTime().ToString("dd-MMM-yyyy");
                        txtSortOrder.Text = oNewsDetails.SortOrder.ToString();
                        hidFileName.Value = oNewsDetails.FileName.ToString();
                        if (!string.IsNullOrEmpty(oNewsDetails.NewsContent))
                        {
                            FCKNoticeContent.Text = HttpUtility.HtmlDecode(oNewsDetails.NewsContent);
                        }
                        else
                            FCKNoticeContent.Text = HttpUtility.HtmlDecode("<p><BR><p>");
                    }
                    btnSaveText.Text = S_TEXT_UPDATE;
                }
                else if (e.CommandName == S_COMMAND_DELETE_NOTICE)
                {
                  
                    moSchoolNewsBL.Delete(iNewsId);
                    string sServerPath = Server.MapPath("~");
                    
                    if (sServerPath.Substring(sServerPath.Length - 1) != "\\")
                        sServerPath = sServerPath + "\\";
                    
                    // Check for File size
                    if (!string.IsNullOrEmpty(oNewsDetails.FileName.ToString()))
                    {
                        if (File.Exists(sServerPath + S_FOLDER_LOCATION + oNewsDetails.FileName.ToString()))
                            File.Delete(sServerPath + S_FOLDER_LOCATION + oNewsDetails.FileName.ToString());
                    }
                    
                    FillNewsDetailGridView();
                    ResetFields();
                    lblUpdateSucess.Text = S_DELETE_MSG;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This class is used to save news details into database.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveText_Click(object sender, EventArgs e)
    {
        try
        {
            FCKNoticeContent.ReadOnly = false;
           
            string sPlainText = StripHTML(FCKNoticeContent.Text);
            sPlainText = sPlainText.Replace("\r\r", string.Empty).Trim();

            if (IsTextType == Constants.I_ONE)
            { 
                if (!string.IsNullOrEmpty(sPlainText.Trim()))
                {
                    string sFileName = string.Empty;
                    NewsDetails oNewsDetails;
                    oNewsDetails = Populate(sFileName);
                    moSchoolNewsBL.Save(oNewsDetails);
                    FillNewsDetailGridView();
                    if (btnSaveText.Text == S_TEXT_SAVE)
                        lblUpdateSucess.Text = S_SAVE_STATEMENT;
                    else
                    {
                        lblUpdateSucess.Text = S_UPDATE_STATEMENT;
                        btnSaveText.Text = S_TEXT_SAVE;
                    }

                    ResetFields();
                }
                else
                    lblErrorMsg.Text = S_BLANK_MSG;
            }
            else 
            {
             if(!ValidateNewsName())
             {
              lblErrorMsg.Text = S_DUPLICATE_LINK_NAME;
             }
             else
             {
                 string sErrorMsg = SaveNewsDetails();
                if (string.IsNullOrEmpty(sErrorMsg))
                {
                    
                    ResetFields();
                    FillNewsDetailGridView();                   
                }
                else
                    lblErrorMsg.Text = sErrorMsg;

             }
            }
        }
        catch(SqlException sqlex)
        {
            lblErrorMsg.Text = sqlex.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This class is used to save the selected news to disaply on home page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveSelected_Click(object sender, EventArgs e)
    {

        try
        {
            List<NewsDetails> lstNewId = new List<NewsDetails>();
        for(int iCnt = 0; iCnt < lstvwNewsDetails.Items.Count; iCnt++)
        {
        NewsDetails oNewsDetails = new NewsDetails();
            CheckBox chkSelect = lstvwNewsDetails.Items[iCnt].FindControl("chkSelect") as CheckBox;
            oNewsDetails.IsSelected = chkSelect.Checked;
            oNewsDetails.NewsId = Convert.ToInt32(lstvwNewsDetails.DataKeys[iCnt]["NewsId"]);
            oNewsDetails.InertedById = miUserId;
            lstNewId.Add(oNewsDetails);

        }
          string sXml = CommonUtility.GenerateXml(lstNewId);
          moSchoolNewsBL.SaveSelectedNews(sXml);
          lblUpdateSucess.Text = S_SAVE_SELECTED_NOTICE;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
        
    }

    /// <summary>
    ///This class is used to clear the news control. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancelText_Click(object sender, EventArgs e)
    {
        try
        {
            ResetFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This class is used to select option Text
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optText_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            ResetFields();
            SetVisibility(false);
            FillNewsDetailGridView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This class is used to select option File News
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optLink_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            ResetFields();
            SetVisibility(true);
            FillNewsDetailGridView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This class is used to set bind values to variable.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwNewsDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            LinkButton lnkFileName = e.Item.FindControl("hlnkFileName") as LinkButton;
            if (lnkFileName != null)
            {
                string sServerPath = Server.MapPath("~");

                if (sServerPath.Substring(sServerPath.Length - 1) != "\\")
                    sServerPath = sServerPath + "\\";

                string sNewFileName = S_FOLDER_PATH + lnkFileName.Text.ToString();

                lnkFileName.Attributes.Add("onclick", "OpenWindow('" + sNewFileName + "'); return false;");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }

    }

    #endregion

    #region Private Method(s)           

      /// <summary>
    /// This Method checks for duplicate notice names 
    /// </summary>
    private bool ValidateNewsName()
    {
        bool bIsValid = false;
        int iExistingNewsId;
        string sNewsHeading;
        sNewsHeading = txtNewsHeading.Text;

        iExistingNewsId = SchoolNewsBL.GetIDByName(miSchoolId, sNewsHeading);
          if (!string.IsNullOrEmpty(hidNewsId.Value))
            {   // Update Operation for link notice
                int iNewsId = Convert.ToInt32(hidNewsId.Value);
                if ((iNewsId == iExistingNewsId) || (iExistingNewsId == Constants.I_ZERO))
                    bIsValid = true;
            }
          else if (iExistingNewsId == Constants.I_ZERO)    // ADD operation
                bIsValid = true;
       
       return bIsValid;
    }

     /// <summary>
    /// This method is used to check file size and then check correct file to specified location
    /// </summary>
    private string UploadNoticeFile()
    {
        if (fileUploadItems.FileName != string.Empty)
        {
            string sReturnErrorMsg = string.Empty;
            string sOldFileName = hidFileName.Value;
            string sServerPath = Server.MapPath("~");
            if (sServerPath.Substring(sServerPath.Length - 1) != "\\")
                sServerPath = sServerPath + "\\";
            bool bHasFile = fileUploadItems.HasFile;
            string sNewFileName = sServerPath + S_FOLDER_LOCATION + fileUploadItems.FileName.ToString();
            if (bHasFile)
            {
                // Check for File size
                if (fileUploadItems.PostedFile.ContentLength > I_FILE_SIZE_LIMIT)
                    sReturnErrorMsg = S_FILE_SIZE_ERROR;
                else
                {
                    if (sOldFileName != string.Empty)
                    {
                        if (File.Exists(sServerPath + S_FOLDER_LOCATION + sOldFileName))
                            File.Delete(sServerPath + S_FOLDER_LOCATION + sOldFileName);
                        fileUploadItems.SaveAs(sNewFileName);
                    }
                    else
                    {
                        if (File.Exists(sNewFileName))
                            sReturnErrorMsg = S_FILE_EXIST;
                        else
                            fileUploadItems.SaveAs(sNewFileName);
                    }
                }

            }
            else
            {
                sReturnErrorMsg = S_FILE_NOT_FOUND;
                throw new FileNotFoundException();
            }
            return sReturnErrorMsg;
        }
        return string.Empty;
    }

      /// <summary>
    /// This method is used Update notice details.
    /// </summary>
    private string SaveNewsDetails()
    {
        string sErrorMsg = string.Empty;
        int iNewsId = 0;
        string sFileName = fileUploadItems.FileName;

        if (hidNewsId.Value != string.Empty)
            iNewsId = Convert.ToInt32(hidNewsId.Value);
        if (sFileName == string.Empty && string.IsNullOrEmpty(hidFileName.Value))
        {
            sErrorMsg = S_BLANK_FILE_MSG;
        }
        else if (!string.IsNullOrEmpty(hidFileName.Value))
        {
            sFileName = hidFileName.Value;
        }
        else
        {
            sErrorMsg = UploadNoticeFile();
        }

        if (sErrorMsg == string.Empty)
        {
            NewsDetails oNewsDetails = Populate(sFileName);
            moSchoolNewsBL.Save(oNewsDetails);
            FillNewsDetailGridView();
            if (btnSaveText.Text == S_TEXT_SAVE)
                lblUpdateSucess.Text = S_SAVE_STATEMENT;
            else
            {
                lblUpdateSucess.Text = S_UPDATE_STATEMENT;
                btnSaveText.Text = S_TEXT_SAVE;
            }

            ResetFields();
        }
        return sErrorMsg;
    }


    /// <summary>
    /// This method is used to set default control fields.
    /// </summary>
    private void ResetFields()
    {
        FCKNoticeContent.Text = string.Empty;
        txtNewsDate.Text = string.Empty;
        txtNewsHeading.Text = string.Empty;
        txtSortOrder.Text = string.Empty;
        hidNewsId.Value = Constants.S_ZERO;        
        txtNewsHeading.Focus();
        btnSaveText.Text = S_TEXT_SAVE;
        hidFileName.Value = string.Empty;
    }

    /// <summary>
    /// This method is used fill gridview of news details.
    /// </summary>
    private void FillNewsDetailGridView()
    {
        List<NewsDetails> lstNewsDetails = moSchoolNewsBL.GetAll(IsTextType);
        lstvwNewsDetails.DataSource = lstNewsDetails;
        lstvwNewsDetails.DataBind();
        if (lstNewsDetails.Count == Constants.I_ZERO)
            btnSaveSelected.Visible = false;
        else
        {
            btnSaveSelected.Visible = true;
            ControlVisiblility();
        }
        trSave.Visible = trNote.Visible = lstvwNewsDetails.Items.Count > Constants.I_ZERO;
    }

    /// <summary>
    /// This method is used to control visibility for file upload control.
    /// </summary>
    private void ControlVisiblility()
    {
        if (IsTextType == Constants.I_ONE)
        {
            lstvwNewsDetails.FindControl("thFileName").Visible = false;
            foreach (ListViewItem item in lstvwNewsDetails.Items)
            {
                item.FindControl("tdFileName").Visible = false;
            }
        }
        else
        {
            lstvwNewsDetails.FindControl("thFileName").Visible = true;
            foreach (ListViewItem item in lstvwNewsDetails.Items)
            {
                item.FindControl("tdFileName").Visible = true;
            }
        }
    }

    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        valSumErrorMsgText.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        new Button[] { btnSaveText, btnSaveSelected, btnCancelText }.ApplyEffect();
        //btnSaveSelected.Attributes.Add("onclick", "if(!SelectedCount(0)){return false;}");
        txtNewsHeading.Focus();
    }
   
    /// <summary>
    ///This class is used to populate the object of NewsDetails class.
    /// </summary>
    /// <returns></returns>
    private NewsDetails Populate(string asFileName)
    {
        NewsDetails oNewsDetails = new NewsDetails
        {
            NewsId = hidNewsId.Value.ToInt(),
           NewsHeading = txtNewsHeading.Text,
           NewsContent = HttpUtility.HtmlEncode(FCKNoticeContent.Text),
            NewsDate  = Convert.ToDateTime(txtNewsDate.Text).ToString(),
            SortOrder = Convert.ToInt32(txtSortOrder.Text),
            FileName = asFileName,
            IsText = IsTextType
        };
        return oNewsDetails;
    }

     /// <summary>
    /// This method is used to convert HTML to plai text.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    private string StripHTML(string source)
    {
        string result;

        // Remove HTML Development formatting
        // Replace line breaks with space
        // because browsers inserts space
        result = source.Replace("\r", " ");

        // Replace line breaks with space
        // because browsers inserts space
        result = result.Replace("\n", " ");

        // Remove step-formatting
        result = result.Replace("\t", string.Empty);

        // Remove repeating spaces because browsers ignore them
        result = System.Text.RegularExpressions.Regex.Replace(result,
                                                              @"( )+", " ");

        // Remove the header (prepare first by clearing attributes)
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<( )*head([^>])*>", "<head>",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"(<( )*(/)( )*head( )*>)", "</head>",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 "(<head>).*(</head>)", string.Empty,
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // remove all scripts (prepare first by clearing attributes)
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<( )*script([^>])*>", "<script>",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"(<( )*(/)( )*script( )*>)", "</script>",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        //result = System.Text.RegularExpressions.Regex.Replace(result,
        //         @"(<script>)([^(<script>\.</script>)])*(</script>)",
        //         string.Empty,
        //         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"(<script>).*(</script>)", string.Empty,
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // remove all styles (prepare first by clearing attributes)
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<( )*style([^>])*>", "<style>",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"(<( )*(/)( )*style( )*>)", "</style>",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 "(<style>).*(</style>)", string.Empty,
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // insert tabs in spaces of <td> tags
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<( )*td([^>])*>", "\t",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // insert line breaks in places of <BR> and <LI> tags
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<( )*br( )*>", "\r",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<( )*li( )*>", "\r",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // insert line paragraphs (double line breaks) in place
        // if <P>, <DIV> and <TR> tags
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<( )*div([^>])*>", "\r\r",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<( )*tr([^>])*>", "\r\r",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<( )*p([^>])*>", "\r\r",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // Remove remaining tags like <a>, links, images,
        // comments etc - anything that's enclosed inside < >
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<[^>]*>", string.Empty,
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // replace special characters:
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @" ", " ",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&bull;", " * ",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&lsaquo;", "<",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&rsaquo;", ">",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&trade;", "(tm)",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&frasl;", "/",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&lt;", "<",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&gt;", ">",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&copy;", "(c)",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&reg;", "(r)",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // Remove all others. More can be added, see
        // http://hotwired.lycos.com/webmonkey/reference/special_characters/
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&(.{2,6});", string.Empty,
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);


        // make line breaking consistent
        result = result.Replace("\n", "\r");

        // Remove extra line breaks and tabs:
        // replace over 2 breaks with 2 and over 4 tabs with 4.
        // Prepare first to remove any whitespaces in between
        // the escaped characters and remove redundant tabs in between line breaks
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 "(\r)( )+(\r)", "\r\r",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 "(\t)( )+(\t)", "\t\t",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 "(\t)( )+(\r)", "\t\r",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 "(\r)( )+(\t)", "\r\t",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // Remove redundant tabs
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 "(\r)(\t)+(\r)", "\r\r",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // Remove multiple tabs following a line break with just one tab
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 "(\r)(\t)+", "\r\t",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // Initial replacement target string for line breaks
        string breaks = "\r\r\r";

        // Initial replacement target string for tabs
        string tabs = "\t\t\t\t\t";
        for (int index = 0; index < result.Length; index++)
        {
            result = result.Replace(breaks, "\r\r");
            result = result.Replace(tabs, "\t\t\t\t");
            breaks = breaks + "\r";
            tabs = tabs + "\t";
        }

        // That's it.
        return result;
    }

    /// <summary>
    /// this method is used to show controls and listview according to radio button selected.
    /// </summary>
    /// <param name="abFlag"></param>
    private void SetVisibility(bool abFlag)
    {
        trfileuploadcontrol.Visible = abFlag;
        trfileuploadnote.Visible = abFlag;
        trfckeditor.Visible = !abFlag;
    }

    #endregion

}
