using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using BusinessLogic;
using Utility;

public partial class LateFeeSetting : SchoolBase
{
    #region Constants

    private const string S_LNK_UPDATE_CONFIGURATION = "Configured";
    private const string S_LNK_S_NOT_CONFIGURE = "Not Configured";
    private const string S_IMG_FOR_STANDARD_FEE_TYPE = "~/RITeSchool/images/GridHead_Std_FeeType.gif";

    #endregion

    #region DataMembers
    const Int32 I_ORIGINAL_STANDARD_ID_COLUMN_NUMBER = 2;
    const Int32 I_STANDARD_ID_COLUMN_NUMBER = 1;
    const Int32 I_STANDARD_NAME_COLUMN_NUMBER = 3;
    const Int32 I_START_COUNT = 4;
    private string IsConfig;
    private string msQuerystring;
    #endregion

    #region event handlers

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                grdStandards.Columns[0].HeaderImageUrl = Resources.LocalizedResources.ImageStandardFeeType;
                grdStandards.Columns[0].HeaderText = "";
                RefreshValue();
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValue();
            }

            bool bIsUseSubmitBehavior = CommonUtility.CheckCancelOrBackClickEvent(this.Page);
            if (bIsUseSubmitBehavior == true)
            {
                if (CheckPreCondition())
                {
                    FillStandardGrid();
                }
            }
            ApplyMouseHoverEffect(new List<Button> { btnCancel });
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
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

    #endregion

    #region private methods

    private void GenerateColumnsOfGrid()
    {
        //add columns of divisions to the grid header row 
        AddStandardColumnsToHeaderRow();
        AddColumnstoOtherRows();
    }

    /// <summary>
    /// This method is used to add columns (of Divisions) to the grid
    /// </summary>
    /// <param name="aoDSAllDivisions"> dataset containing all divisions in school
    /// </param>
    private void AddStandardColumnsToHeaderRow()
    {
        TableCell oTableCell1 = new TableCell();
        oTableCell1.HorizontalAlign = HorizontalAlign.Center;

        oTableCell1.Width = System.Web.UI.WebControls.Unit.Point(900);
        oTableCell1.Wrap = false;
        oTableCell1.Text = Resources.LocalizedResources.LateFeeConfiguration;

        grdStandards.HeaderRow.Cells.Add(oTableCell1);
    }

    private void AddColumnstoOtherRows()
    {
        int iRowCount = grdStandards.Rows.Count;
        int iCount = 1;
        int iCellIndex = 0;
        DataSet oDs = (DataSet)grdStandards.DataSource;
        DataTable oDtStd = oDs.Tables[0];
        DataTable oDtConfg = oDs.Tables[1];        
        //This loop is for generating new table cells for respective fee types and standard.
        for (int iRowIndex = 0; iRowIndex < iRowCount; iRowIndex++)
        {
            int iStdID = Convert.ToInt32(grdStandards.DataKeys[iRowIndex][1].ToString());            
            int iStandardID = Convert.ToInt32(grdStandards.DataKeys[iRowIndex][1].ToString());
            DataRow[] oDr = oDtConfg.Select("Standard_Id = " + iStandardID);
            DataRow[] oDrStd = oDtStd.Select("Standard_Id = " + iStandardID);

            //loop through columns
            for (int iColIndex = 0; iColIndex < iCount; iColIndex++)
            {
                TableCell oTableCell = new TableCell();
                oTableCell.Width = System.Web.UI.WebControls.Unit.Point(900);
                oTableCell.Wrap = false;
                oTableCell.Text = oDtStd.Rows[iColIndex]["Standard_Id"].ToString();
                iCellIndex = grdStandards.Rows[iRowIndex].Cells.Add(oTableCell);
                int iFeeTypeId = Convert.ToInt32(oDrStd[0]["feeTypeId"]);

                grdStandards.Rows[iRowIndex].Cells[iCellIndex].HorizontalAlign = HorizontalAlign.Center;
                {
                    ReadQuerystring();
                    Label oLbl;
                    if (iFeeTypeId == 0)
                    {
                        grdStandards.Rows[iRowIndex].Cells[iCellIndex].CssClass = "ClsNotAssignDark";
                        grdStandards.Rows[iRowIndex].Cells[iCellIndex].Text = Resources.LocalizedResources.NA;
                        grdStandards.Rows[iRowIndex].Cells[iCellIndex].Font.Bold = true;
                    }
                    else if (oDr.Length > 0)
                    {
                        string sId = oDr[0]["SchoolWise_Standard_LateFee_DueDates_Id"].ToString();
                        msQuerystring = "StandardId=" + iStandardID +
                                        "&LateFeeId= " + sId                            
                                          + "&ViewMode=" + Constants.ViewMode.Edit.ToString()
                                          + "&Is_Configured=" + IsConfig;
                        oLbl = AddLinkToUpdateSettings();
                        grdStandards.Rows[iRowIndex].Cells[iCellIndex].Controls.Add(oLbl);
                        grdStandards.Rows[iRowIndex].Cells[iCellIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "#aae2cd");
                    }
                    else
                    {
                        msQuerystring = "StandardId=" + iStandardID +
                                        "&LateFeeId= 0"
                                          + "&ViewMode=" + Constants.ViewMode.Edit.ToString()
                                          + "&Is_Configured=" + IsConfig; ;
                        oLbl = AddLinkForFeesConfiguration();
                        grdStandards.Rows[iRowIndex].Cells[iCellIndex].Controls.Add(oLbl);
                        grdStandards.Rows[iRowIndex].Cells[iCellIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "#5dad8e");
                    }
                }
            }
        }
    }

    /// <summary>
    /// This method is used to decrypt encrypted querystring.
    /// </summary>
    private void ReadQuerystring()
    {
        try
        {
	        if (Request.QueryString.ToString() != Constants.S_EMPTY_STRING)
		        IsConfig = QueryString["Is_Configured"];
        }
        catch (Exception)
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
			oMasterPage.RedirectToNextPage(Constants.S_PAGE_ERROR);
        }
    }

    /// <summary>
    /// This method is used to provide link to remove assignment of teacher
    /// or add new teacher.
    /// </summary>
    private Label AddLinkToUpdateSettings()
    {
        Label oLbl = new Label();
        string sEncrypt = Utility.CommonUtility.EncryptQuerystring(msQuerystring);
        string sURL = "../Admin/LateFeeSettingDetails.aspx" + "?" + sEncrypt;
        oLbl.Text = Resources.LocalizedResources.Configured;
        oLbl.ForeColor = System.Drawing.Color.Black;
        oLbl.Font.Bold = true;
        oLbl.Style.Add(HtmlTextWriterStyle.Cursor, "Hand");
        oLbl.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");
        oLbl.Style.Add(HtmlTextWriterStyle.TextDecoration, "underline");
        oLbl.Attributes.Add("onclick", "window.open('" + sURL
                                   + "' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=1150,height=650'); return false;");
        return oLbl;
    }

    /// <summary>
    /// This method is used to add hyperlink to the table cell where we have to
    /// assign teacher.
    /// </summary>
    private Label AddLinkForFeesConfiguration()
    {

        Label oLbl = new Label();
        string sEncrypt = Utility.CommonUtility.EncryptQuerystring(msQuerystring);
        string sURL = "../Admin/LateFeeSettingDetails.aspx" + "?" + sEncrypt;
        oLbl.Text = Resources.LocalizedResources.NotConfigured1;
        oLbl.ForeColor = System.Drawing.Color.White;
        oLbl.Style.Add(HtmlTextWriterStyle.TextDecoration, "underline");
        oLbl.Font.Bold = true;
        oLbl.Style.Add(HtmlTextWriterStyle.Cursor, "Hand");
        oLbl.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");
        oLbl.Attributes.Add("onclick", "window.open('" + sURL
                                   + "' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=1050,height=650'); return false;");
        return oLbl;
    }

    /// <summary>
    /// This function checks the preconditons of Configured Subjects for Subject Group criteria.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.LateFeeSettings);

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
    /// This method is used to visible or hide controls on page load as per configuration is 
    /// done or not.
    /// </summary>
    private void VisibleOrHideControls()
    {
        divGridView.Visible = false;
        LegendTable.Visible = false;
        trerr.Visible = false;
    }

    /// <summary>
    /// This function is to fill grid with Associated standards.
    /// </summary>
    private void FillStandardGrid()
    {
        DataSet oDs = SchoolWiseStandardLateFeeDueDatesMasterCollectionBL.GetStdLateFeesConfiguration(miSchoolId, miAcademicYearId);
        grdStandards.DataSource = oDs;
        grdStandards.DataBind();
        GenerateColumnsOfGrid();
    }
    /// <summary>
    /// This method used to value based on Culture
    /// </summary>
    private void RefreshValue()
    {
        grdStandards.Columns[0].HeaderImageUrl = Resources.LocalizedResources.ImageStandardFeeType;
    }
    #endregion
}
