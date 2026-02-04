// File Name  : StandardwiseFeeConfigurationUI.aspx.cs
// Created By : Anugandha
// Date       : 07/02/2008
//Description :This class is used to view total fee amount assigned to standards
//             as well to check fee configuration is done or not. 
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Reflection;
using BusinessLogic.Exceptions;
using BusinessLogic;
using Utility;
using FeeEntities;
using System.IO;

public partial class StdwiseFeeConfigUI : SchoolBase
{
    #region Constant

    const int I_FEETYPES_TABLE_INDEX = 1;
    private const string S_FOLDER_LOCATION = "RITeSchool\\DOWNLOADS\\Fee Structure\\";
    private const string S_FOLDER_PATH = @"../DOWNLOADS/Fee Structure/";
    private const string S_FILE_NOT_FOUND = "File does not exists.";
    private const int I_FILE_SIZE_LIMIT = 1048576;  // File limit is 1 MB
    private const string S_FILE_SIZE_ERROR = "Size of file is too large.";

    #endregion

    #region Data member

    private string msQuerystring;
    private string IsConfig;


    
    private FeeStructureLinkBL moFeeStructureLinkBL;

    #endregion

    #region Events

    /// <summary>
    /// This method is used to fill standard-FeeType grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            moFeeStructureLinkBL = new FeeStructureLinkBL(miSchoolId, miUserId, miAcademicYearId);
            ChangeFeeStrutcureLinkStatus();
            lnkbtnFeeStructureLink.Attributes.Add("onclick", "ShowFeeStructurePopup(); return false;");
            if (!IsPostBack)
            {
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                DesignSettingAccordingLanguage();
            }

            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                DesignSettingAccordingLanguage();
            }
            const string S_IMG_FOR_STD_FEE_TYPE = "~/RITeSchool/images/GridHead_Std_FeeType.gif";
            grdFeeTypes.Columns[0].HeaderImageUrl = S_IMG_FOR_STD_FEE_TYPE;
            grdFeeTypes.Columns[0].HeaderText = "";
            FillStdFeeTypesInGrid();

            ApplyMouseHoverEffect(new List<Button> { btnBack, btnSave });
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save and upload fee structure link.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string sLinkName;
            string sFileUploadErr = UploadNoticeFile(out sLinkName);
            if (string.IsNullOrEmpty(sFileUploadErr))
            {
                moFeeStructureLinkBL.Save(sLinkName);
                lblSuccess.Text = "File Uploaded Successfully!!!";
                ChangeFeeStrutcureLinkStatus();
            }
            else
            {
                lblError.Text = sFileUploadErr;
            }

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to update Fee structure link status.
    /// </summary>
    private void ChangeFeeStrutcureLinkStatus()
    {
        FeeStructureLinkBL oFeeStructureLinkBL = new FeeStructureLinkBL(miSchoolId, miUserId, miAcademicYearId);
        bool bShowFeeStructureOfNextYear = SchoolBase.Settings.ShowFeeStructureOfNextYear;

        Dictionary<string, string> dirFeeLinkFileNames = oFeeStructureLinkBL.Get(miSchoolId, miAcademicYearId, miUserId, bShowFeeStructureOfNextYear);
        if (dirFeeLinkFileNames.ContainsKey("CurrentYearFeeStructureUrl") == false)
        {
            btnView.Visible = false;
            btnDelete.Visible = false;
        }
        else
        {
            string sNewFileName = S_FOLDER_PATH + dirFeeLinkFileNames["CurrentYearFeeStructureUrl"];
            btnView.Attributes.Add("onclick", "OpenWindow('" + sNewFileName + "'); return false;");
            btnView.Visible = true;
            btnDelete.Visible = true;
        }
    }

    /// <summary>
    /// This method is used to navigate page to control panel.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Fee_Related)));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to delete current year fee structure.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            moFeeStructureLinkBL.Delete();
            lblSuccess.Text = "Fee Structure Deleted Successfully!!!";
            ChangeFeeStrutcureLinkStatus();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Methods

   

    /// <summary>
    /// This function checks the preconditons of Configured Subjects for Subject Group criteria.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.StandardwiseFeeConfiguration);
        if (sLinks.Equals(""))
        {
            divErr.Visible = false;
            bReturn = true;
        }
        else
        {
            divErr.InnerHtml = sLinks;
            VisibleOrHideControls();
        }
        return bReturn;
    }

    /// <summary>
    /// This method is used to fill standardwise FeeTypes in grid 
    /// </summary>
    private void FillStdFeeTypesInGrid()
    {
        if (CheckPreCondition())
        {
            FillStandardsGrid();
            AddFeeTypesColumns();
        }
    }

    /// <summary>
    /// This method is used to visible or hide controls depends on configuration is done or not.
    /// </summary>
    private void VisibleOrHideControls()
    {
        divGridView.Visible = false;
        LegendTable.Visible = false;
    }

    /// <summary>
    /// This method is used to fill grid with standard names.
    /// </summary>
    private void FillStandardsGrid()
    {
        SchoolwiseStandardFeeConfigurationMasterCollectionBL obj = new SchoolwiseStandardFeeConfigurationMasterCollectionBL(miSchoolId, miAcademicYearId);
        DataSet oDs = obj.GetStdFeeConfigurationDetails();
        grdFeeTypes.DataSource = oDs;
        grdFeeTypes.DataBind();
    }

    /// <summary>
    /// This method is used to generate columns of Fee Types dynamically added to the 
    /// grid.
    /// </summary>
    private void AddFeeTypesColumns()
    {
        const Int32 I_STD_ID_COL_NO = 0;
        int iFeeTypeIndex;

        DataSet oDs = (DataSet)grdFeeTypes.DataSource;
        DataTable oDtFeeTypes = oDs.Tables[I_FEETYPES_TABLE_INDEX];

        //This loop is for generating new table cells for respective fee types and standard.
        for (int iRowIndex = 0; iRowIndex < grdFeeTypes.Rows.Count; iRowIndex++)
        {
            int iStandardId = Convert.ToInt32(grdFeeTypes.DataKeys[iRowIndex][I_STD_ID_COL_NO].ToString());
            for (iFeeTypeIndex = 0; iFeeTypeIndex < oDtFeeTypes.Rows.Count; iFeeTypeIndex++)
            {
                if (iRowIndex == 0)
                    AddFeeTypesHeader(iFeeTypeIndex);//This method is used to set header to grid. 

                string sFeeType = oDs.Tables[1].Rows[iFeeTypeIndex]["Fee_Type"].ToString();
                string sStdName = oDs.Tables[0].Rows[iRowIndex]["Standard_Name"].ToString();
                //This method is used to set fee configuration details(i.e.links as per configuration)
                AddStdWiseFeeConfig(iRowIndex, iFeeTypeIndex, iStandardId, sFeeType, sStdName);
            }
        }
    }

    /// <summary>
    /// This method is used to add hyperlink to the table cell.
    /// </summary>
    private Label AddLinkToConfigure()
    {
        Label oLbl = new Label();
        oLbl.Text = Resources.LocalizedResources.NotConfigured;

        oLbl.ForeColor = System.Drawing.Color.White;
        oLbl.Font.Bold = true;
        oLbl.Style.Add(HtmlTextWriterStyle.Cursor, "Hand");
        oLbl.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");
        oLbl.Style.Add(HtmlTextWriterStyle.TextDecoration, "underline");
        oLbl.Style.Add(HtmlTextWriterStyle.WhiteSpace, "nowrap");
        string sEncrypt = Utility.CommonUtility.EncryptQuerystring(msQuerystring);
        string sURL = "../Admin/StandardwiseFeeConfigurationDetails.aspx?" + sEncrypt;
        oLbl.Attributes.Add("onclick", "window.open('" + sURL
                                  + "' , '_self','scrollbars=yes,resizable=no,top=0,left=0,width=900,height=610'); return false;");

        return oLbl;
    }

    /// <summary>
    /// This method is used to provide link
    /// </summary>
    private Label AddLinkToUpdateConfiguration(string asTotalFees)
    {
        Label oLbl = new Label();
        oLbl.Text = Resources.LocalizedResources.TotalFeeRs + asTotalFees;

        oLbl.ForeColor = System.Drawing.Color.Black;
        oLbl.Font.Bold = true;
        oLbl.Style.Add(HtmlTextWriterStyle.Cursor, "Hand");
        oLbl.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");
        oLbl.Style.Add(HtmlTextWriterStyle.TextDecoration, "underline");
        oLbl.Style.Add(HtmlTextWriterStyle.WhiteSpace, "nowrap");
        string sEncrypt = Utility.CommonUtility.EncryptQuerystring(msQuerystring);
        string sURL = "../Admin/StandardwiseFeeConfigurationDetails.aspx?" + sEncrypt;
        oLbl.Attributes.Add("onclick", "window.open('" + sURL
                                  + "' , '_self','scrollbars=yes,resizable=no,top=0,left=0,width=900,height=610'); return false;");
        return oLbl;
    }

    /// <summary>
    /// This method is used to add fee type header to grid.
    /// </summary>
    private void AddFeeTypesHeader(int aiFeeTypeIndex)
    {
        int iHeaderCellNo = 0;

        DataSet oDs = (DataSet)grdFeeTypes.DataSource;
        DataTable oDtFeeTypes = oDs.Tables[I_FEETYPES_TABLE_INDEX];

        TableCell oTHeader = new TableCell();
        oTHeader.Text = oDtFeeTypes.Rows[aiFeeTypeIndex]["Fee_Type"].ToString();
        oTHeader.HorizontalAlign = HorizontalAlign.Center;
        iHeaderCellNo = grdFeeTypes.HeaderRow.Cells.Add(oTHeader);
    }

    /// <summary>
    /// This method is used to add fee configuration details.
    /// </summary>
    /// <param name="aiRowIndex"></param>
    /// <param name="aiFeeTypeIndex"></param>
    /// <param name="aiStdId"></param>
    private void AddStdWiseFeeConfig(int aiRowIndex, int aiFeeTypeIndex, int aiStdId, string asFeeType, string asStd)
    {
        #region Constants

        const int I_STDFEE_TABLE_INDEX = 2;
        const int I_CONFIG_TABLE_INDEX = 3;

        #endregion

        #region DataTables

        DataSet oDs = (DataSet)grdFeeTypes.DataSource;
        DataTable oDtConfig = oDs.Tables[I_CONFIG_TABLE_INDEX];
        DataTable oDtFeeTypes = oDs.Tables[I_FEETYPES_TABLE_INDEX];
        DataTable oDtStdFeeTypes = oDs.Tables[I_STDFEE_TABLE_INDEX];

        #endregion

        int iCellIndex;

        TableCell oTableCell = SetTableCellProperties(oDtFeeTypes, aiFeeTypeIndex);

        oTableCell.Attributes.Add("title", Resources.LocalizedResources.Std + " : " + asStd + " [" + asFeeType + "]");
        iCellIndex = grdFeeTypes.Rows[aiRowIndex].Cells.Add(oTableCell);

        grdFeeTypes.Rows[aiRowIndex].Cells[iCellIndex].HorizontalAlign = HorizontalAlign.Center;
        int iFeeTypeId = Convert.ToInt32(grdFeeTypes.Rows[aiRowIndex].Cells[iCellIndex].Text);
        Label oLbl;
        DataRow[] oStandardFeeConfigDataRow = oDtConfig.Select("Standard_Id=" + aiStdId + " AND " + "Fee_Type_Id=" + iFeeTypeId);
        DataRow[] oStandardFeeDataRow = oDtStdFeeTypes.Select("Standard_Id=" + aiStdId + " AND " + "Fee_Type_Id=" + iFeeTypeId);

        ReadQuerystring();
        //Check that fee type configuration is done or not.
        if (oStandardFeeConfigDataRow.Length > 0)
        {
            int iStandardwiseFeeConfigId = Convert.ToInt32(oStandardFeeConfigDataRow[0]["Schoolwise_Standard_Fee_Configuration_Id"].ToString());

            //This method is used to create querystring when fee confifuration is done.
            AddFeeConfigQueryString(aiStdId, iFeeTypeId, iStandardwiseFeeConfigId, asFeeType, asStd);

            //This method is used to create label with proper link.
            oLbl = AddLinkToUpdateConfiguration(oStandardFeeConfigDataRow[0]["Total_Fees"].ToString());

            //This method is used to add label to grid cell as well to set backcolor.            
            AddControlToGrid(aiRowIndex, iCellIndex, oLbl, 'Y');

        }
        else if (oStandardFeeDataRow.Length > 0)
        {
            //This method is used to create querystring when fee confifuration is not done.            
            AddNotFeeConfigQueryString(aiStdId, iFeeTypeId, asFeeType, asStd);

            //This method is used to create label with proper link(i.e."Not Configured")
            oLbl = AddLinkToConfigure();

            //This method is used to add label to grid cell as well to set backcolor.
            AddControlToGrid(aiRowIndex, iCellIndex, oLbl, 'N');
        }
        else
        {
            oTableCell.Text = Constants.S_EMPTY_STRING;
            grdFeeTypes.Rows[aiRowIndex].Cells[iCellIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "#eaeaea");
        }
    }

    /// <summary>
    /// This method is used to decrypt encrypted querystring.
    /// </summary>
    private void ReadQuerystring()
    {
        try
        {
            IsConfig = QueryString["Is_Configured"];
        }
        catch (Exception)
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Constants.S_PAGE_ERROR);
        }
    }

    /// <summary>
    /// This method is used to create querystring.
    /// </summary>
    /// <param name="aiStdId"></param>
    /// <param name="aiFeeTypeId"></param>
    /// <param name="aiStdFeeConfigId"></param>
    private void AddFeeConfigQueryString(int aiStdId, int aiFeeTypeId, int aiStdFeeConfigId, string asFeeType, string asStd)
    {
        msQuerystring = "Standard_Id=" + aiStdId
                                                 + "&Fee_Type_Id=" + aiFeeTypeId
                                                 + "&Schoolwise_Standard_Fee_Configuration_Id=" + aiStdFeeConfigId
                                                 + "&ViewMode=" + Constants.ViewMode.Edit.ToString()
                                                 + "&FeeType=" + asFeeType
                                                 + "&Std=" + asStd
                                                 + "&Is_Configured=" + IsConfig;
    }

    /// <summary>
    /// This method is used to create querystring when fee configuration is not done.
    /// </summary>
    /// <param name="aiStdId"></param>
    /// <param name="aiFeeId"></param>
    private void AddNotFeeConfigQueryString(int aiStdId, int aiFeeId, string asFeeType, string asStd)
    {
        msQuerystring = "Standard_Id=" + aiStdId
                                + "&Fee_Type_Id=" + aiFeeId
                                + "&ViewMode=" + Constants.ViewMode.New.ToString()
                                + "&FeeType=" + asFeeType
                                + "&Std=" + asStd
                                + "&Is_Configured=" + IsConfig;
    }

    /// <summary>
    /// This method is used to add control to grid.
    /// </summary>
    /// <param name="aiRowindex"></param>
    /// <param name="aiCellindex"></param>
    /// <param name="oLbl"></param>
    /// <param name="acIsConfig"></param>
    private void AddControlToGrid(int aiRowindex, int aiCellindex, Label oLbl, Char acIsConfig)
    {
        grdFeeTypes.Rows[aiRowindex].Cells[aiCellindex].Controls.Add(oLbl);
        if (acIsConfig == 'Y')
           //grdFeeTypes.Rows[aiRowindex].Cells[aiCellindex].BackColor = System.Drawing.Color.FromArgb(172, 193, 111); //253, 252, 178  
            grdFeeTypes.Rows[aiRowindex].Cells[aiCellindex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "#aae2cd");
        else
          //grdFeeTypes.Rows[aiRowindex].Cells[aiCellindex].BackColor = System.Drawing.Color.FromArgb(88, 104, 43);
            grdFeeTypes.Rows[aiRowindex].Cells[aiCellindex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "#5dad8e");

        grdFeeTypes.Rows[aiRowindex].Cells[aiCellindex].Style[HtmlTextWriterStyle.Padding] = "5px 7px";
    }

    /// <summary>
    /// This method is used to set properties to tablecell and return it.
    /// </summary>
    /// <param name="aDtFeeTypes"></param>
    /// <param name="aiFeeTypeIndex"></param>
    /// <returns>TableCell</returns>
    private TableCell SetTableCellProperties(DataTable aDtFeeTypes, int aiFeeTypeIndex)
    {
        const string S_COL_FEE_TYPE_ID = "Fee_Type_Id";

        TableCell oTableCell = new TableCell();
        oTableCell.Width = System.Web.UI.WebControls.Unit.Point(900);
        oTableCell.Wrap = false;
        oTableCell.Text = aDtFeeTypes.Rows[aiFeeTypeIndex][S_COL_FEE_TYPE_ID].ToString();

        return oTableCell;
    }



    /// <summary>
    /// This method is used to set design according to the selected language.
    /// </summary>
    private void DesignSettingAccordingLanguage()
    {
        valSumErrMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
    }

    /// <summary>
    /// This method is used to check file size and then check correct file to specified location
    /// </summary>
    private string UploadNoticeFile(out string asFileName)
    {
        asFileName = string.Empty;
        if (fileUploadItems.FileName != string.Empty)
        {
            string sReturnErrorMsg = string.Empty;
            string sServerPath = Server.MapPath("~");
            if (sServerPath.Substring(sServerPath.Length - 1) != "\\")
                sServerPath = sServerPath + "\\";
            string sLinkName = CommonUtility.GetFileNameForRenaming(fileUploadItems.FileName.ToString());

            if (fileUploadItems.HasFile )
            {
                if (fileUploadItems.PostedFile.ContentLength <= I_FILE_SIZE_LIMIT)
                {
                    string sLinkPath = sServerPath + S_FOLDER_LOCATION + sLinkName;
                    fileUploadItems.SaveAs(sLinkPath);
                    asFileName = sLinkName;
                }
                else
                {
                    sReturnErrorMsg = S_FILE_SIZE_ERROR;
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

    #endregion

}
