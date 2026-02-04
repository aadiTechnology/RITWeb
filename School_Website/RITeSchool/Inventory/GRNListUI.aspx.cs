// File Name  : GRNListUI.aspx.cs
// Created By : Amit 
// Date       : 18/07/2009
//Description : This class is used to list GRNs and edit/delete GRN (Goods Received Note).

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;


public partial class GRNListUI : SchoolBase
{
    #region " Constants "

    const string S_COMMAND_REMOVE = "Remove";
    const string S_DEFAULT_SORT_EXP = "GRNCode";

    #endregion " Constants "

    #region " Events "

    /// <summary>
    /// This event is used to fill list view with all created GRNs.
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
    /// This event is used to delete GRN.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwGRNList_ItemCommand (object sender, ListViewCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == S_COMMAND_REMOVE)
            {
                int iGRNID = Convert.ToInt32(((ImageButton)(e.CommandSource)).CommandArgument);
                GRNDetailsBL oGRNDetailsBL = new GRNDetailsBL();
                oGRNDetailsBL.DeleteGRNDetails(iGRNID, miSchoolId, miUserId);
                lstvwGRNList.DataSourceID = ObjDSGRNList.ID;
                AddSortImage();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill footer propery of list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwGRNList_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwGRNList.Items.Count > 0)
                ControlUtility.FillListViewPagerFooter(lstvwGRNList, DtPgCount);
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to add attribute properties to list view item control
    /// and to generate query string.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwGRNList_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                DataRowView oDataRowView = oCurrentItem.DataItem as DataRowView;
                ImageButton oimgbtnDeletePO = e.Item.FindControl("imgbtnDeleteGRN") as ImageButton;
                oimgbtnDeletePO.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");

                ImageButton oimgbtnViewPO = e.Item.FindControl("imgbtnViewGRN") as ImageButton;
                string sQueryString = "GRNID=" + oDataRowView["GRNID"].ToString();
                oimgbtnViewPO.Attributes.Add("onclick", "window.open('../Inventory/GRNDetailsUI.aspx?" + CommonUtility.EncryptQuerystring(sQueryString.ToString())+ " ' , '_self');return false;");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    /// <summary>
    /// This event is used to sort list view items.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwGRNList_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            SetSortVariables();
            hidSortExpression.Value = e.SortExpression;
            HtmlTableRow oHtmlTableHeaderRow = lstvwGRNList.FindControl("trHeader") as HtmlTableRow;
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    /// <summary>
    /// This event is used to view page wise GRN list.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCnt_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwGRNList);
            AddSortImage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion  " Events "

    #region " Private Methods "

    /// <summary>
    /// This method is used set default properties of controls.
    /// </summary>
    private void SetDefaultProperties()
    {
        lstvwGRNList.DataSourceID = ObjDSGRNList.ID;
        AddSortImage();
        ApplyMouseHoverEffect(new List<Button> {btnAdd});
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
    /// This method is used to set sorting image in list view header column.
    /// </summary>
    private void AddSortImage()
    {
        if (lstvwGRNList.SortDirection.ToString() == "Ascending" || lstvwGRNList.SortDirection.ToString() == string.Empty)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
        if (lstvwGRNList.SortExpression != string.Empty)
            hidSortExpression.Value = lstvwGRNList.SortExpression.ToString();
        else
            hidSortExpression.Value = S_DEFAULT_SORT_EXP;
        HtmlTableRow oHtmlTableHeaderRow = lstvwGRNList.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    #endregion " Private Methods " 
}
