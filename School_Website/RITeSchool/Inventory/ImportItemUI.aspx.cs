// File Name :- ImportItemUI.aspx.cs
// Purpose   :- This class is used to import item details from excel sheet to database
//              and shows item details on list view.
// Date     :- 27 July 2009
//  Author   :- Amit
//

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class ImportItemUI : SchoolBase
{
    #region " Data Member and Constants"

    private const string S_DEFAULT_SORT_EXP = "ItemName"; 

    #endregion " Data Member and Constants"

    #region " Events "

    /// <summary>
    /// This event is used to set client side attributes, fill list view and set default controls. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
                SetDefaultProperties();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to import inventory item details in the database.  
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnImportItems_Click(object sender, EventArgs e)
    {
        string sServerFilePath = string.Empty;
        try
        {
            string sErrorMessage = string.Empty;
            string sFileName =CommonUtility.GetFileNameForRenaming(fileUploadItems.FileName);
            //string sFolderName = Server.MapPath("~") + "\\RITeSchool\\Uploads\\";
            string sFolderName = base.BasePath + "\\RITeSchool\\Uploads\\";
            sServerFilePath = sFolderName + sFileName;

            fileUploadItems.SaveAs(sServerFilePath);

            string sSourceFileName = fileUploadItems.PostedFile.FileName;
            
            bool bSetAutoCode = chkSetAutoItemCode.Checked;

            ImportItemBL oItemImportBL = new ImportItemBL(sSourceFileName, sServerFilePath);
            oItemImportBL.SchoolId = miSchoolId;
            oItemImportBL.AcademicYearId = miAcademicYearId;
            oItemImportBL.UserId = miUserId;

            sErrorMessage = oItemImportBL.UploadFile(bSetAutoCode);

            ShowUploadMsg(sErrorMessage);
        }
        catch (InvalidItemDataException ex)
        {
            lblUploadErrMsg.Text = ex.Message;
            lblUploadErrMsg.CssClass = "ClsLabel";
            lblUploadErrMsg.Visible = true;
            lblUploadErrMsg.ForeColor = System.Drawing.Color.Red;
            AddSortImage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
        finally
        {
            try
            {
                if (File.Exists(sServerFilePath))
                    File.Delete(sServerFilePath);
            }
            catch (Exception ex)
            {
                ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
            }
        }
    }

    #endregion " Events "

    #region " List View Events "

    /// <summary>
    /// This event is used to select paging list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlPageNos_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwItemDetails);
            lstvwItemDetails.DataSourceID = lstvwDSobj.ID;
            HtmlTableRow oHtmlTableHeaderRow = lstvwItemDetails.FindControl("trHeader") as HtmlTableRow;
            if (oHtmlTableHeaderRow != null)
                CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill footer pager property of list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwItemDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwItemDetails.Items.Count > 0)
                ControlUtility.FillListViewPagerFooter(lstvwItemDetails, DtPgCount);
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to sort items in list view. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwItemDetails_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            SetSortVariables();
            hidSortExpression.Value = e.SortExpression;
            HtmlTableRow oHtmlTableHeaderRow = lstvwItemDetails.FindControl("trHeader") as HtmlTableRow;
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion " List View Events "

    #region " Private Methods "

    /// <summary>
    /// This method is used to set validation header text and set hyperlink attributes on javascript.
    /// </summary>
    private void SetDefaultProperties()
    {
        fileUploadItems.Focus();
        valsumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        hlnkDownloadTemplate.Attributes.Add("onclick", "window.open('../downloads/ItemDetails.xls','_self'); return false;");
        btnImportItems.Attributes["onclick"] = "javascript:DisableButtons(this)";
        btnBack.Attributes["onclick"] = "javascript:DisableButtons(this)";
        ApplyMouseHoverEffect(new List<Button> {btnImportItems, btnBack});
        lstvwItemDetails.DataSourceID = lstvwDSobj.ID;
        hidSortDirection.Value = Constants.S_ASCENDING;
        hidSortExpression.Value = "ItemName";
        HtmlTableRow oHtmlTableHeaderRow = lstvwItemDetails.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }
   
    /// <summary>
    /// This method is used to set sort variables.
    /// </summary>
    private void SetSortVariables()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to set item importing message and fill item detail list view.
    /// </summary>
    /// <param name="asErrorMessage"></param>
    private void ShowUploadMsg(string asErrorMessage)
    {
        if (asErrorMessage.Equals(""))
        {
            lblUploadMsg.CssClass = "ClsHilightTextB";
            lblUploadMsg.Text = "File uploaded successfully !!!";
            lblUploadMsg.Visible = true;
            DataPager pager = lstvwItemDetails.FindControl("DtPgDropDown") as DataPager;
            if (pager != null)
                pager.SetPageProperties(0, pager.PageSize, true);
            lstvwItemDetails.DataSourceID = lstvwDSobj.ID;
            lstvwItemDetails.DataBind();
        }
        else
        {
            lblUploadErrMsg.Text = asErrorMessage;
            lblUploadErrMsg.Visible = true;
        }
        AddSortImage();
    }

    /// <summary>
    /// This method is used to set sorting image in list view header column.
    /// </summary>
    private void AddSortImage()
    {
        if (lstvwItemDetails.SortDirection.ToString() == "Ascending")
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
        if (lstvwItemDetails.SortExpression != string.Empty)
            hidSortExpression.Value = lstvwItemDetails.SortExpression.ToString();
        else
            hidSortExpression.Value = S_DEFAULT_SORT_EXP;
        HtmlTableRow oHtmlTableHeaderRow = lstvwItemDetails.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    #endregion " Private Methods "
}
