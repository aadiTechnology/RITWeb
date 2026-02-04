// File Name  : PurchaseOrderListUI.aspx.cs
// Created By : Milind
// Date       : 14/7/2009
// Description : This class is used to display the list of the purchase order.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using CrystalDecisions.Shared;

public partial class PurchaseOrderListUI : SchoolBase
{
    #region " Constants "

    const string S_DEFUALT_SORT_EXPR = "Insert_Date";

    #endregion " Constants "

    #region Events

    #region Page Events

    /// <summary>
    /// This event is used to fill list view lstvwPOList.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                ReadQueryString();
                lstvwPOList.DataSourceID = lstDSobj.ID;
                SetButtonVisibility();
                ApplyMouseHoverEffect(new List<Button> {btnAdd});
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region ListView Events

    /// <summary>
    /// This event is used to fill the datapager combobox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwPOList_DataBound(object sender, EventArgs e)
    {
        try
        {
            DtPgCount.Visible = true;
            if (lstvwPOList.Items.Count > 0)
            {
                ControlUtility.FillListViewPagerFooter(lstvwPOList, DtPgCount);
                AddSortImage();
            }
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill the list view according to the selected pageindex in the combo box. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCnt_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
             ControlUtility.SetDataPagerAccordingToPageNo(lstvwPOList);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to delete PO details from database.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwPOList_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            int iPurchaseOrderID = lstvwPOList.DataKeys[e.Item.DisplayIndex]["PurchaseOrderID"].ToInt();
            if (e.CommandName == "Remove")
            {
                int iPOID = Convert.ToInt32(((ImageButton)(e.CommandSource)).CommandArgument);
                PurchaseOrderBL oPurchaseOrderBL = new PurchaseOrderBL();
                oPurchaseOrderBL.DeletePurchaseOrderDetails(iPOID, miSchoolId, miUserId);

                lstvwPOList.DataSourceID = lstDSobj.ID;
                lstvwPOList.DataBind();
            }
            else if (e.CommandName == "EXPORT")
            {
                string sRecordSelectionFormula = "(usp_GetAllPODetailsForExoprt.SchoolId}=" + miSchoolId + " AND  usp_GetAllPODetailsForExoprt.POId} =" + iPurchaseOrderID + " AND usp_GetAllPODetailsForExoprt.UserId}=" + miUserId + ")" + "@ ";

                ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.PurchaseOrder, sRecordSelectionFormula, ExportFormatType.PortableDocFormat);
                oReportDisplay.DisplayReport();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set the javascripts attributs to the list view imagebutton.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwPOList_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {

                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                ImageButton oimgbtnDeletePO = (ImageButton)e.Item.FindControl("imgbtnDeletePO");
                LinkButton lbtnExport = e.Item.FindControl("lbtnExport") as LinkButton;
                int iIsFinalApproved = lstvwPOList.DataKeys[iRowId]["IsFinalApproved"].ToInt();
                if (iIsFinalApproved == Constants.I_ZERO)
                {
                    lbtnExport.Enabled = false;
                    lbtnExport.Text = "-";
                }
                else
                {
                    lbtnExport.Enabled = true;
                    lbtnExport.Text = "Export";
                }

                if (hidPOId.Value != Constants.S_ZERO && hidIsFromApproverScreen.Value == "Y")
                    oimgbtnDeletePO.Enabled = false;
                else
                    oimgbtnDeletePO.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");

                ImageButton oimgbtnViewPO = (ImageButton)e.Item.FindControl("imgbtnViewPO");

                string sQueryString = "POID=" + lstvwPOList.DataKeys[iRowId]["PurchaseOrderID"].ToString();
                sQueryString += "&CanModify=" + lstvwPOList.DataKeys[iRowId]["Editable"].ToString() + "&StatusId=" + hidStatusId.Value + "&IsFromApproverScreen=" + hidIsFromApproverScreen.Value;

                oimgbtnViewPO.Attributes.Add("onclick", "window.open('../Inventory/PurchaseOrderDetailsUI.aspx?" + CommonUtility.EncryptQuerystring(sQueryString.ToString())
                                                             + " ' , '_self');return false;");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set sorting image to the the column header.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwPOList_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            SetSortVariables();
            hidSortExpression.Value = e.SortExpression;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #endregion

    #region Private Method

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
    /// This method is used to set sorting image in list view column header.
    /// </summary>
    private void AddSortImage()
    {
        if (lstvwPOList.SortDirection.ToString() == "Ascending" || lstvwPOList.SortDirection.ToString() == string.Empty)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
        if (lstvwPOList.SortExpression != string.Empty)
            hidSortExpression.Value = lstvwPOList.SortExpression.ToString();
        else
            hidSortExpression.Value = S_DEFUALT_SORT_EXPR;
        HtmlTableRow oHtmlTableHeaderRow = lstvwPOList.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    /// <summary>
    /// This method is used to read the query string.
    /// </summary>
    private void ReadQueryString()
    {
        if (QueryString["PoId"] != null)
            hidPOId.Value = QueryString["PoId"];

        if (QueryString["RequesterId"] != null)
            hidUserId.Value = QueryString["RequesterId"];

        if (QueryString["StatusId"] != null)
            hidStatusId.Value = QueryString["StatusId"];

        if (QueryString["IsFromApproverScreen"] != null)
            hidIsFromApproverScreen.Value = QueryString["IsFromApproverScreen"];

        if (hidIsFromApproverScreen.Value == Constants.S_NO)
            hidPOId.Value = Constants.S_ZERO;
    }

    /// <summary>
    /// This method is used to set the buttons visibility as per condition.
    /// </summary>
    private void SetButtonVisibility()
    {
        if (hidPOId.Value != Constants.S_ZERO && hidIsFromApproverScreen.Value == Constants.S_YES)
        {
            btnAdd.Visible = false;            
            btnBack.Visible = true;

            string sQueryString = "StatusId=" + hidStatusId.Value;
            hidQueryString.Value = CommonUtility.EncryptQuerystring(sQueryString);
            
        }
    }

    #endregion

}
