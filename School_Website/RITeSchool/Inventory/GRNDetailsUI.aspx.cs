// File Name   : GRNDetailsUI.aspx.cs
// Created By  : Amit 
// Date        : 14/07/2009
// Description : This class is used to create GRN (Goods Received Note).

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class GRNDetailsUI : SchoolBase
{
    #region " Constants "

    const string S_LISTVIEW_DATASOURCE = "GRNDataSource";

    const string S_DB_COLUMN_ITEM_ID = "ItemID";
    const string S_DB_COLUMN_PO_ID = "POID";
    const string S_DB_COLUMN_ITEM_GRN_QUANTITY = "ItemGRNQty";
    const string S_DB_COLUMN_ITEM_QUANTITY = "ItemOriginalQty";
    const string S_DB_COLUMN_ITEM_REJECTED_QUANTITY = "ItemRejectedQty";
    const string S_DB_COLUMN_ITEM_DIFF = "ItemQtyDiff";
    const string S_DB_COLUMN_ITEM_NAME = "ItemName";
    const string S_DB_COLUMN_ITEM_CODE = "ItemCode";
    const string S_DB_COLUMN_PO_CODE = "POCode";
    const string S_ITEM_UNIT = "ItemUnit";
    const string S_DB_COLUMN_ITEM_PO_ORG_QUANTITY = "ItemOrgQty";
    const string S_ITEM_ADDED = "Added in GRN";
    const string S_COMMAND_ADD = "Add";
    const string S_COMMAND_REMOVE = "Remove";
    const string S_COMMAND_DETAILS = "Details";
    const string S_DEFAULT_SORT_EXP_ITEM = "ItemName";
    const string S_DEFAULT_SORT_EXP_PO = "POName";
    const string S_UNITS = "Unit";
    const string S_UNIT_NAME = "UOMName";
    const string S_PIECE_COUNT = "PieceCount";
    const int I_GRN_NAME_LENGTH = 40;

    #endregion " Constants "

    #region " Structure "

    private struct GRNItemsDetailsStruct
    {
        public int miItemID;
        public int miPOID;
        public double mdGRNQty;
        public double mdOriginalQty;
        public double mdRejectedQty;
        public string msItemCode;
        public string msItemName;
        public string msPOCode;
        public string msItemUnit;
        public double mdPOOrgQty;
        public string msUnits;
        public string msUMOName;
        public int iPieceCount;
    }

    #endregion " Structure "

    #region " Events "

    /// <summary>
    /// This event is used to fill listview by items in purchase order in add GRN mode
    /// Or to fill GRN listview in view GRN mode.
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
                SetClientScriptAttributes();
                InitializeForm();
            }
            btnSave.Attributes.Add("onclick", "if(!AllConfirmDelete(" + lstvwGRN.Items.Count + ")){return false;}");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill Itemwise listview with items in purchase order. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optItemWise_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            //If the radio button item wise is checked then
            //All other controls related to po wise get visible false.
            //List view fills with items present in purchase order.
            //Also hide the Add All button also and first page of list view get selected.
            if (optItemWise.Checked)
            {
                ShowHideControls(true);
                lstvwItemWiseDetails.DataSourceID = objDSPODetails.ID;
                if (lstvwItemWiseDetails.Items.Count > 0)
                {
                    DataPager oDataPager = lstvwItemWiseDetails.FindControl("DtPgDropDown") as DataPager;
                    oDataPager.SetPageProperties(0, oDataPager.PageSize, true);
                }
            }
            else
                optPOWise.Checked = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill POWise listview with purchase order.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>   
    protected void optPOWise_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            //If the radio button po wise is checked then
            //All other controls related to item wise get visible false.
            //List view fills with purchase order.
            //Also hide the Add All button also and first page of list view get selected.
            if (optPOWise.Checked)
            {
                ShowHideControls(false);
                lstvwPOWiseDetails.DataSourceID = objDSPODetails.ID;
            }
            else
                optItemWise.Checked = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save GRN.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            int iGRNId = Convert.ToInt32(hidGRNId.Value);

            if (lstvwGRN.Items.Count > 0)
            {
                FillGRNItemListView();
                string sXmlGRNPOItems = GenerateGRNPOItemXML();
                string sXmlGRNItems = GenerateGRNItemXML();
                string sGRNDesc = txtDescription.Text.Trim();

                GRNDetailsBL oGRNDetailsBL = new GRNDetailsBL();
                oGRNDetailsBL.InsertGRNDetails(miSchoolId, miUserId, hidGRNName.Value, sGRNDesc, sXmlGRNPOItems, sXmlGRNItems, iGRNId, hidIsModify.Value);
            }
            else
            {
                GRNDetailsBL oGRNDetailsBL = new GRNDetailsBL();
                oGRNDetailsBL.DeleteGRNDetails(iGRNId, miSchoolId, miUserId);
            }
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage("~/RITeSchool/Inventory/GRNListUI.aspx");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    /// <summary>
    /// This event is used to reset GRN in edit mode.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            optItemWise.Checked = false;
            optPOWise.Checked = false;
            btnModify.Visible = true;
            SetFormInViewMode();
            DtPOPgCount.Visible = false;

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    /// <summary>
    /// This event is used to modify GRN.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnModify_Click(object sender, EventArgs e)
    {
        try
        {
            SetNewGRNMode();
            ShowHideControlAtAddMode(true);
            hidIsModify.Value = "Y";
            btnModify.Visible = false;
            FillGRNItemListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to add all items in GRN.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddAll_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (ListViewDataItem oLstvwPOItems in lstvwPOItems.Items)
            {
                int iRowId = Convert.ToInt32(oLstvwPOItems.DataItemIndex);

                TextBox otxtAcceptedQty = oLstvwPOItems.FindControl("txtAcceptedQty") as TextBox;
                HiddenField ohidActualPOQty = oLstvwPOItems.FindControl("hidActualPOQty") as HiddenField;
                Label olblRejectedQty = oLstvwPOItems.FindControl("lblRejectedQty") as Label;
                LinkButton olnkbtnRemove = oLstvwPOItems.FindControl("lnkbtnRemove") as LinkButton;
                Label olblItemCode = oLstvwPOItems.FindControl("lblItemCode") as Label;
                Label olblItemName = oLstvwPOItems.FindControl("lblItemName") as Label;
                Label olblPOCode = oLstvwPOItems.FindControl("lblPOCode") as Label;
                DropDownList cmbUnits = oLstvwPOItems.FindControl("cmbUnits") as DropDownList;
                string sUOMName = lstvwPOItems.DataKeys[oLstvwPOItems.DisplayIndex]["Unit"].ToString();

                //otxtAcceptedQty.Attributes.Add("onkeyup",
                //                               "CalculateRejectedQuantity(this,3,false,'" + otxtAcceptedQty.ClientID +
                //                               "', '" + ohidActualPOQty.ClientID + "','" + olblRejectedQty.ClientID + "','" + cmbUnits.ClientID + "'," + iPieceCount +
                //                               ")");

                //otxtAcceptedQty.Attributes.Add("onkeyup",
                //                          "SetValueToHiddenField('" + otxtAcceptedQty.ClientID + "', '" +
                //                          ohidActualPOQty.ClientID + "', '" + cmbUnits.ClientID + "','" + olblItemName.ClientID + "' ,'" +
                //                          olblPOCode.ClientID + "')");




                if (otxtAcceptedQty.Text != "" && otxtAcceptedQty.Text != "0")
                {
                    GRNItemsDetailsStruct oGRNItemsDetailsStruct = new GRNItemsDetailsStruct();
                    oGRNItemsDetailsStruct.miItemID = Convert.ToInt32(lstvwPOItems.DataKeys[iRowId]["ItemID"]);
                    oGRNItemsDetailsStruct.miPOID = Convert.ToInt32(lstvwPOItems.DataKeys[iRowId]["PurchaseOrderID"]);
                    oGRNItemsDetailsStruct.mdOriginalQty = Convert.ToDouble(ohidActualPOQty.Value);
                    if (cmbUnits.SelectedIndex == Constants.I_ZERO)
                    {
                        int iPiece = hidPieceCount.Value.ToInt();
                        string sTextValue = otxtAcceptedQty.Text.ToString();
                        oGRNItemsDetailsStruct.mdGRNQty = sTextValue.ToInt() * iPiece;
                    }
                    else
                        oGRNItemsDetailsStruct.mdGRNQty = Convert.ToDouble(otxtAcceptedQty.Text);
                    oGRNItemsDetailsStruct.mdRejectedQty = Convert.ToDouble(oGRNItemsDetailsStruct.mdOriginalQty - oGRNItemsDetailsStruct.mdGRNQty);
                    oGRNItemsDetailsStruct.msItemName = olblItemName.Text.Trim();
                    oGRNItemsDetailsStruct.msItemCode = olblItemCode.Text.Trim();
                    oGRNItemsDetailsStruct.msPOCode = olblPOCode.Text.Trim();
                    oGRNItemsDetailsStruct.msUnits = cmbUnits.SelectedIndex.ToString();
                    oGRNItemsDetailsStruct.miPOID = Convert.ToInt32(lstvwPOItems.DataKeys[iRowId]["PurchaseOrderID"]);
                    oGRNItemsDetailsStruct.msItemUnit = sUOMName;
                    oGRNItemsDetailsStruct.iPieceCount = Convert.ToInt32(hidPieceCount.Value);

                    AddItemsQtyToDataTable(oGRNItemsDetailsStruct);
                    olnkbtnRemove.Visible = true;
                    if (oGRNItemsDetailsStruct.mdRejectedQty == Constants.I_ZERO)
                        olblRejectedQty.Text = Constants.S_ZERO;
                    else
                        olblRejectedQty.Text = oGRNItemsDetailsStruct.mdRejectedQty.ToString();
                }
                //olblRejectedQty.Text = Constants.S_ZERO;
                //olblRejectedQty.Text = Convert.ToDouble(ohidActualPOQty.Value).ToString();
                otxtAcceptedQty.Enabled = false;
                cmbUnits.Enabled = false;
            }
            FillGRNItemListView();

            if (lstvwItemWiseDetails.Visible == true)
                AddSortImage(lstvwItemWiseDetails, S_DEFAULT_SORT_EXP_ITEM);
            else
                AddSortImage(lstvwPOItems, S_DEFAULT_SORT_EXP_PO);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion " Events "

    #region " List View Events "

    #region " ItemWIse ListView Events "

    /// <summary>
    /// This event is used to bound controls to list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwItemWiseDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwItemWiseDetails.Items.Count > 0)
            {
                ControlUtility.FillListViewPagerFooter(lstvwItemWiseDetails, DtPgCount);
                AddSortImage(lstvwItemWiseDetails, S_DEFAULT_SORT_EXP_ITEM);
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
    /// This event is use add/remove items from GRN. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwItemWiseDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {

            int iGRNId = Convert.ToInt32(hidGRNId.Value);

            if (e.CommandName == S_COMMAND_ADD)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                int iItemID = Convert.ToInt32(lstvwItemWiseDetails.DataKeys[iRowId]["ItemID"]);
                string sGRNCreateMode = "ItemWise";
                int iPoID = 0;
                PurchaseOrderBL oPurchaseOrderBL = new PurchaseOrderBL();
                DataTable oDTPoForItem = oPurchaseOrderBL.GetPOsForItem(miSchoolId, sGRNCreateMode, iItemID, iPoID, iGRNId);

                trlstvwPOItems.Visible = true;
                lstvwPOItems.DataSource = oDTPoForItem;
                lstvwPOItems.DataBind();
                //string sUOMName = lstvwItemWiseDetails.DataKeys[e.Item.DisplayIndex]["UOMUnit"].ToString();
                AddSortImage(lstvwItemWiseDetails, S_DEFAULT_SORT_EXP_ITEM);

                btnAddAll.Visible = true;
                btnAddAll.Attributes.Add("onclick", "if(!AddAllReqItems(" + lstvwPOItems.Items.Count +"," + 0 + ")){return false;}");
            }
            else if (e.CommandName == S_COMMAND_REMOVE)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                int iItemID = Convert.ToInt32(lstvwItemWiseDetails.DataKeys[iRowId]["ItemID"]);
                LinkButton olnkbtnRemove = e.Item.FindControl("lnkbtnRemove") as LinkButton;
                DataTable oDTItemsDetails = ViewState[S_LISTVIEW_DATASOURCE] as DataTable;
                Label olblItemQty = e.Item.FindControl("lblItemQty") as Label;
                Label olblPOCount = e.Item.FindControl("lblPOCount") as Label;
                string sUnit = Convert.ToString(lstvwItemWiseDetails.DataKeys[iRowId]["UOMUnit"]);
                double dPOCount = Convert.ToDouble(olblPOCount.Text);
                double dQty = Convert.ToDouble(olblItemQty.Text.Replace(sUnit, "").Trim());
                int iRowCount = oDTItemsDetails.Rows.Count;

                for (int iCount = iRowCount - 1; iCount >= 0; iCount--)
                {
                    if (Convert.ToInt32(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_ITEM_ID]) == iItemID && Convert.ToInt32(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_PO_ID]) != 0)
                    {
                        dQty += Convert.ToDouble(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_ITEM_GRN_QUANTITY]);
                        DataRow oDTRow = oDTItemsDetails.Rows[iCount];
                        oDTRow.Delete();
                        oDTItemsDetails.AcceptChanges();
                        ViewState[S_LISTVIEW_DATASOURCE] = oDTItemsDetails;
                    }
                }
                olblItemQty.Text = dQty + "  " + sUnit;
                olblPOCount.Text = Convert.ToString(dPOCount);
                olnkbtnRemove.Visible = false;
                FillGRNItemListView();
                AddSortImage(lstvwItemWiseDetails, S_DEFAULT_SORT_EXP_ITEM);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    /// <summary>
    /// This event is used bound item data to Item wise listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwItemWiseDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                DataRowView oDataRowView = oCurrentItem.DataItem as DataRowView;
                string sUnit = Convert.ToString(oDataRowView["UOMUnit"]);
                Label olblQty = e.Item.FindControl("lblItemQty") as Label;

                int iPieceCount = lstvwItemWiseDetails.DataKeys[e.Item.DisplayIndex]["PieceCount"].ToInt();

                if (ViewState[S_LISTVIEW_DATASOURCE] != null)
                {
                    int iItemID = Convert.ToInt32(oDataRowView["ItemID"]);

                    DataTable oDTItemsDetails;
                    oDTItemsDetails = ViewState[S_LISTVIEW_DATASOURCE] as DataTable;
                    double dGRNItemQty = 0;
                    int iPOCount = 0;
                    double dItemQty = Convert.ToDouble(oDataRowView["ItemQty"]);
                    int iOriginalPOCount = Convert.ToInt32(oDataRowView["POCount"]);

                    for (int iCount = 0; iCount < oDTItemsDetails.Rows.Count; iCount++)
                    {
                        if (Convert.ToInt32(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_ITEM_ID]) == iItemID && Convert.ToInt32(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_PO_ID]) != 0)
                        {
                            dGRNItemQty += Convert.ToDouble(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_ITEM_GRN_QUANTITY]);
                        }
                    }
                    olblQty.Text = (dItemQty - dGRNItemQty).ToString();
                    Label olblPOCnt = e.Item.FindControl("lblPOCount") as Label;
                    olblPOCnt.Text = (iOriginalPOCount - iPOCount).ToString();
                    //If item is present in this GRN
                    //Then quantity of that item in that GRN is greater than zero.
                    //That time 'Remove from GRN' linkbutton visible otherwise it hides.
                    if (dGRNItemQty != 0)
                    {
                        LinkButton olnkbtnRemove = e.Item.FindControl("lnkbtnRemove") as LinkButton;
                        olnkbtnRemove.Visible = true;
                    }
                }
                double hItemQty = Convert.ToDouble(olblQty.Text.ToString());

                string sUOMQuery = GetItemQuantity(hItemQty, iPieceCount, sUnit);

                olblQty.Text = sUOMQuery;

            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    /// <summary>
    /// This event is used sort items in Itemwise list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwItemWiseDetails_Sorting(object sender, ListViewSortEventArgs e)
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

    /// <summary>
    /// This event is used to show page wise items in ItemWise list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwItemWiseDetails);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion " ItemWIse List View Events "

    #region " POWise List View Events "

    /// <summary>
    /// This event is used set total view item count and list view pager. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwPOWiseDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwPOWiseDetails.Items.Count > 0)
            {
                ControlUtility.FillListViewPagerFooter(lstvwPOWiseDetails, DtPOPgCount);
                AddSortImage(lstvwPOWiseDetails, S_DEFAULT_SORT_EXP_PO);
            }
            else
                DtPOPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is to sort items in PoWise listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwPOWiseDetails_Sorting(object sender, ListViewSortEventArgs e)
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

    /// <summary>
    /// Thisevent is used to add/remove items in GRN from purchase order.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwPOWiseDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {

            int iGRNId = Convert.ToInt32(hidGRNId.Value);

            if (e.CommandName == S_COMMAND_ADD)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                int iPOID = Convert.ToInt32(lstvwPOWiseDetails.DataKeys[iRowId]["PurchaseOrderID"]);
                string sGRNCreateMode = "POWise";
                int iItemID = 0;
                PurchaseOrderBL oPurchaseOrderBL = new PurchaseOrderBL();
                DataTable oDTPoForItem = oPurchaseOrderBL.GetPOsForItem(miSchoolId, sGRNCreateMode, iItemID, iPOID, iGRNId);

                trlstvwPOItems.Visible = true;
                lstvwPOItems.DataSource = oDTPoForItem;
                lstvwPOItems.DataBind();
                AddSortImage(lstvwPOWiseDetails, S_DEFAULT_SORT_EXP_PO);

                btnAddAll.Visible = true;
                btnAddAll.Attributes.Add("onclick", "if(!AddAllReqItems(" + lstvwPOItems.Items.Count + ")){return false;}");
            }
            else if (e.CommandName == S_COMMAND_REMOVE)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                int iPOID = Convert.ToInt32(lstvwPOWiseDetails.DataKeys[iRowId]["PurchaseOrderID"]);
                LinkButton olnkbtnRemove = e.Item.FindControl("lnkbtnRemove") as LinkButton;
                DataTable oDTItemsDetails = ViewState[S_LISTVIEW_DATASOURCE] as DataTable;
                int iRowCount = oDTItemsDetails.Rows.Count;
                for (int iCount = iRowCount - 1; iCount >= 0; iCount--)
                {
                    if (Convert.ToInt32(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_PO_ID]) == iPOID && Convert.ToInt32(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_PO_ID]) != 0)
                    {
                        DataRow oDTRow = oDTItemsDetails.Rows[iCount];
                        oDTRow.Delete();
                        oDTItemsDetails.AcceptChanges();
                        ViewState[S_LISTVIEW_DATASOURCE] = oDTItemsDetails;
                    }
                }
                olnkbtnRemove.Visible = false;
                FillGRNItemListView();
                AddSortImage(lstvwPOWiseDetails, S_DEFAULT_SORT_EXP_PO);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to bind item in POWise list view which shows purchase order. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwPOWiseDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                DataRowView oDataRowView = oCurrentItem.DataItem as DataRowView;

                if (ViewState[S_LISTVIEW_DATASOURCE] != null)
                {
                    int iPOID = Convert.ToInt32(oDataRowView["PurchaseOrderID"]);

                    DataTable oDTItemsDetails;
                    oDTItemsDetails = ViewState[S_LISTVIEW_DATASOURCE] as DataTable;

                    for (int iCount = 0; iCount < oDTItemsDetails.Rows.Count; iCount++)
                    {
                        if (Convert.ToInt32(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_PO_ID]) == iPOID)
                        {
                            LinkButton olnkbtnRemove = e.Item.FindControl("lnkbtnRemove") as LinkButton;
                            olnkbtnRemove.Visible = true;
                            break;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to view purchase order in paging and set list view footer property.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlPOCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwPOWiseDetails);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion " POWise List  View Events  "

    #region " POItems List View Events "

    /// <summary>
    /// This event used to add/remove items in purchase order.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwPOItems_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);

            int iPOID = Convert.ToInt32(lstvwPOItems.DataKeys[iRowId]["PurchaseOrderID"]);
            int iPieceCount = Convert.ToInt32(lstvwPOItems.DataKeys[iRowId]["PieceCount"]);
            int iItemID = Convert.ToInt32(lstvwPOItems.DataKeys[iRowId]["ItemID"]);
            string sUnit = Convert.ToString(lstvwPOItems.DataKeys[iRowId]["Unit"]);
            TextBox otxtAcceptedQty = lstvwPOItems.Items[iRowId].FindControl("txtAcceptedQty") as TextBox;
            HiddenField ohidActualPOQty = lstvwPOItems.Items[iRowId].FindControl("hidActualPOQty") as HiddenField;
            Label olblRejectedQty = e.Item.FindControl("lblRejectedQty") as Label;
            LinkButton olnkbtnRemove = e.Item.FindControl("lnkbtnRemove") as LinkButton;
            Label olblItemCode = e.Item.FindControl("lblItemCode") as Label;
            Label olblItemName = e.Item.FindControl("lblItemName") as Label;
            Label olblPOCode = e.Item.FindControl("lblPOCode") as Label;
            DropDownList cmbUnits = e.Item.FindControl("cmbUnits") as DropDownList;
            double dQty = otxtAcceptedQty.Text.ToDecimal().ToInt();
            string sName = GetItemQuantity(dQty, iPieceCount, sUnit);

            GRNItemsDetailsStruct oGRNItemsDetailsStruct = new GRNItemsDetailsStruct();

            oGRNItemsDetailsStruct.miItemID = iItemID;
            oGRNItemsDetailsStruct.miPOID = iPOID;
            oGRNItemsDetailsStruct.mdOriginalQty = Convert.ToDouble(ohidActualPOQty.Value);
            if (otxtAcceptedQty.Text == string.Empty)
                oGRNItemsDetailsStruct.mdGRNQty = 0;
            else if (cmbUnits.SelectedIndex == Constants.I_ZERO)
            {
                string sTextValue = Convert.ToString(otxtAcceptedQty.Text.ToString());
                int iValue = sTextValue.ToInt() * iPieceCount;
                oGRNItemsDetailsStruct.mdGRNQty = iValue;
            }
            else
                oGRNItemsDetailsStruct.mdGRNQty = otxtAcceptedQty.Text.Trim().ToDouble();
            oGRNItemsDetailsStruct.msItemName = olblItemName.Text.Trim();
            oGRNItemsDetailsStruct.msItemCode = olblItemCode.Text.Trim();
            oGRNItemsDetailsStruct.msPOCode = olblPOCode.Text.Trim();
            oGRNItemsDetailsStruct.msItemUnit = sUnit;
            oGRNItemsDetailsStruct.msUnits = cmbUnits.SelectedIndex.ToString();
            oGRNItemsDetailsStruct.msUMOName = sName;
            oGRNItemsDetailsStruct.iPieceCount = iPieceCount;

            //When command is "Add" that time add the items details to GRN and View state table.
            //And show link button (Remove From GRN).
            if (e.CommandName == S_COMMAND_ADD)
            {
                AddItemsQtyToDataTable(oGRNItemsDetailsStruct);
                olnkbtnRemove.Visible = true;
                otxtAcceptedQty.Enabled = false;
                cmbUnits.Enabled = false;
                olblRejectedQty.Text = (oGRNItemsDetailsStruct.mdOriginalQty - oGRNItemsDetailsStruct.mdGRNQty).ToString();
                btnSave.Text = "Save";
            }
            //When command is "Remove" that time remove the items details from GRN and View state table.
            //And hide link button (Remove From GRN).
            else if (e.CommandName == S_COMMAND_REMOVE)
            {
                int iCount;
                DataTable oDTItemsDetails;
                oDTItemsDetails = ViewState[S_LISTVIEW_DATASOURCE] as DataTable;

                for (iCount = 0; iCount < oDTItemsDetails.Rows.Count; iCount++)
                {
                    if (Convert.ToInt32(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_ITEM_ID]) == oGRNItemsDetailsStruct.miItemID && Convert.ToInt32(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_PO_ID]) == oGRNItemsDetailsStruct.miPOID)
                        break;
                }
                olblRejectedQty.Text = oGRNItemsDetailsStruct.mdOriginalQty.ToString();
                DataRow oDTRow = oDTItemsDetails.Rows[iCount];
                oDTRow.Delete();
                oDTItemsDetails.AcceptChanges();
                ViewState[S_LISTVIEW_DATASOURCE] = oDTItemsDetails;
                otxtAcceptedQty.Text = "0";
                olnkbtnRemove.Visible = false;
            }
            FillGRNItemListView();
            if (lstvwItemWiseDetails.Visible == true)
                AddSortImage(lstvwItemWiseDetails, S_DEFAULT_SORT_EXP_ITEM);
            else
                AddSortImage(lstvwPOItems, S_DEFAULT_SORT_EXP_PO);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Thisevent is used bind items detail to list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwPOItems_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                DataRowView oDataRowView = oCurrentItem.DataItem as DataRowView;
                double dItemQty = 0;

                hidUOMName.Value = lstvwPOItems.DataKeys[e.Item.DisplayIndex]["Unit"].ToString();
                string sUOM = hidUOMName.Value.ToString();
                hidPieceCount.Value = lstvwPOItems.DataKeys[e.Item.DisplayIndex]["PieceCount"].ToString();
                int iPieceCount = Convert.ToInt32(hidPieceCount.Value);
                DropDownList cmbUnits = e.Item.FindControl("cmbUnits") as DropDownList;
                TextBox txtItemPOQty = e.Item.FindControl("txtAcceptedQty") as TextBox;
                cmbUnits.Items.Clear();
                cmbUnits.Items.Add(new ListItem { Text = sUOM, Value = Constants.S_ZERO });
                cmbUnits.Items.Add(new ListItem { Text = Constants.S_UNITS, Value = Constants.S_ONE });
                int iText = txtItemPOQty.Text.ToDecimal().ToInt();

                if (cmbUnits.SelectedIndex == Constants.I_ZERO)
                {
                    if (iPieceCount > Constants.I_ONE)
                    {
                        if (iText % iPieceCount == 0)
                        {
                            txtItemPOQty.Text = Convert.ToString(iText.ToInt() / iPieceCount.ToInt());
                        }
                        else
                        {
                            cmbUnits.SelectedIndex = Constants.I_ONE;
                            cmbUnits.Enabled = false;
                        }
                    }
                    else
                    {
                        cmbUnits.SelectedIndex = Constants.I_ONE;
                        cmbUnits.Enabled = false;
                    }
                }


                TextBox otxtAcceptedQty = e.Item.FindControl("txtAcceptedQty") as TextBox;
                if (ViewState[S_LISTVIEW_DATASOURCE] != null)
                {
                    int iPOID = Convert.ToInt32(oDataRowView["PurchaseOrderID"]);
                    int iItemID = Convert.ToInt32(oDataRowView["ItemID"]);

                    DataTable oDTItemsDetails = ViewState[S_LISTVIEW_DATASOURCE] as DataTable;

                    //To fill the item quantity of item in the particular purchase order from the Viewstate.
                    for (int iCount = 0; iCount < oDTItemsDetails.Rows.Count; iCount++)
                    {
                        int iPOItemID = Convert.ToInt32(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_ITEM_ID]);
                        int iGRNPOID = Convert.ToInt32(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_PO_ID]);

                        if (iPOItemID == iItemID && iGRNPOID == iPOID)
                        {
                            dItemQty = Convert.ToDouble(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_ITEM_GRN_QUANTITY]);
                            otxtAcceptedQty.Text = dItemQty.ToString();
                        }
                    }
                }
                HiddenField ohidActualPOQty = e.Item.FindControl("hidActualPOQty") as HiddenField;
                string STest = ohidActualPOQty.Value;
                Label olblRejectedQty = e.Item.FindControl("lblRejectedQty") as Label;
                ImageButton oimgbtnAdd = e.Item.FindControl("imgbtnAdd") as ImageButton;
                Label olblItemName = e.Item.FindControl("lblItemName") as Label;
                Label olblPOCode = e.Item.FindControl("lblPOCode") as Label;
                Label olblPOQty = e.Item.FindControl("lblPOQty") as Label;
                double lblPoQty = olblPOQty.Text.ToDecimal().ToInt();
                string sName = GetItemQuantity(lblPoQty, iPieceCount, sUOM);
                olblPOQty.Text = sName;
                olblRejectedQty.Text = Constants.S_ZERO;

                oimgbtnAdd.Attributes.Add("Onclick",
                                          "SetValueToHiddenField('" + otxtAcceptedQty.ClientID + "', '" +
                                          ohidActualPOQty.ClientID + "', '" + cmbUnits.ClientID + "','" + olblItemName.ClientID + "' ,'" +
                                          olblPOCode.ClientID + "')");
               
                otxtAcceptedQty.Attributes.Add("onkeyup",
                                               "CalculateRejectedQuantity(this,3,false,'" + otxtAcceptedQty.ClientID +
                                               "', '" + ohidActualPOQty.ClientID + "','" + olblRejectedQty.ClientID + "','" + cmbUnits.ClientID + "'," + iPieceCount +
                                               ")");
                cmbUnits.Attributes.Add("Onchange",
                                               "CalculateRejectedQuantity(this,3,false,'" + otxtAcceptedQty.ClientID +
                                               "', '" + ohidActualPOQty.ClientID + "','" + olblRejectedQty.ClientID + "','" + cmbUnits.ClientID + "'," + iPieceCount +
                                               ")");

                //Item is present in GRN i.e. if item quantity is greater than zero that time only
                //show the  link button (Remove From GRN).
                if (dItemQty != 0)
                {
                    LinkButton olnkbtnRemove = e.Item.FindControl("lnkbtnRemove") as LinkButton;
                    olnkbtnRemove.Visible = true;
                }

                if (cmbUnits.SelectedIndex == Constants.I_ONE)
                {
                    olblRejectedQty.Text =
                        (Convert.ToDouble(ohidActualPOQty.Value) - Convert.ToDouble(otxtAcceptedQty.Text)).ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion " Events List View POItems"

    #region " GRN List View Events "

    /// <summary>
    /// This event is used to show GRN items in with purchase order and to remove item from GRN.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwGRN_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
            int iItemID = Convert.ToInt32(lstvwGRN.DataKeys[iRowId]["ItemID"]);
            hidUOMName.Value = Convert.ToString(lstvwGRN.DataKeys[iRowId]["ItemUnit"]);
            hidPieceCount.Value = Convert.ToString(lstvwGRN.DataKeys[iRowId]["PieceCount"]);


            DataTable oDTItemsForGRN = ViewState[S_LISTVIEW_DATASOURCE] as DataTable;

            if (e.CommandName == S_COMMAND_DETAILS)
            {
                if (oDTItemsForGRN.Rows.Count > 0)
                {
                    EnumerableRowCollection<DataRow> query = from order in oDTItemsForGRN.AsEnumerable()
                                                             where order.Field<Int32>(S_DB_COLUMN_ITEM_ID) == iItemID
                                                             select order;

                    DataView view = query.AsDataView();

                    HtmlTableRow oHtmlTableRow = e.Item.FindControl("trtxtQty") as HtmlTableRow;
                    HtmlTableCell oHtmlTableCell = oHtmlTableRow.FindControl("tdtxtQty") as HtmlTableCell;
                    ListView olstVwItemDetails = oHtmlTableCell.FindControl("lstVwItemDetails") as ListView;
                    olstVwItemDetails.DataSource = view;
                    olstVwItemDetails.DataBind();
                    if (hidIsModify.Value == "N" && hidGRNId.Value != "0")
                        oHtmlTableCell.ColSpan = 3;
                    else
                        oHtmlTableCell.ColSpan = 4;
                    oHtmlTableRow.Visible = true;
                }
            }
            else if (e.CommandName == S_COMMAND_REMOVE)
            {
                int iRowCount = oDTItemsForGRN.Rows.Count;
                for (int iCount = iRowCount - 1; iCount >= 0; iCount--)
                {
                    if (Convert.ToInt32(oDTItemsForGRN.Rows[iCount][S_DB_COLUMN_ITEM_ID]) == iItemID)
                    {
                        DataRow oDTRow = oDTItemsForGRN.Rows[iCount];
                        oDTRow.Delete();
                        oDTItemsForGRN.AcceptChanges();
                    }
                }
                ViewState[S_LISTVIEW_DATASOURCE] = oDTItemsForGRN;

                // This part hides PO item detals list view.
                lstvwPOItems.DataSource = null;
                lstvwPOItems.DataBind();
                trlstvwPOItems.Visible = false;
                btnAddAll.Visible = false;

                FillGRNItemListView();
                SetQuantityForItem(iItemID);
            }
            if (lstvwItemWiseDetails.Visible == true)
                AddSortImage(lstvwItemWiseDetails, S_DEFAULT_SORT_EXP_ITEM);
            else
                AddSortImage(lstvwPOItems, S_DEFAULT_SORT_EXP_PO);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is to bind items to lstvwGRN list view to show items in GRN.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwGRN_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ImageButton oimgbtnDeleteItem = e.Item.FindControl("imgbtnDeleteItem") as ImageButton;
                Button btnHideDetails = e.Item.FindControl("btnHideDetails") as Button;
                ApplyMouseHoverEffect(new List<Button> { btnHideDetails });
                HtmlTableRow oHtmlTableHeaderRow = lstvwGRN.FindControl("trHeader") as HtmlTableRow;
                HtmlTableCell oHtmlTableCell = oHtmlTableHeaderRow.FindControl("thDelete") as HtmlTableCell;
                HtmlTableRow oHtmlTableRowItem = e.Item.FindControl("trItem") as HtmlTableRow;
                HtmlTableCell oHtmlTableCellItem = oHtmlTableRowItem.FindControl("tdimgbtnDeleteItem") as HtmlTableCell;

                if (hidIsModify.Value == "N" && hidGRNId.Value != "0")
                {
                    oHtmlTableCell.Visible = false;
                    oimgbtnDeleteItem.Visible = false;
                    if (oHtmlTableRowItem != null)
                        oHtmlTableCellItem.Visible = false;
                }
                else
                {
                    oimgbtnDeleteItem.Visible = true;
                    oHtmlTableCell.Visible = true;
                    if (oHtmlTableRowItem != null)
                        oHtmlTableCellItem.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to hide details of items in GRN.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnHideDetails_Click(object sender, EventArgs e)
    {
        try
        {
            Button oButton = sender as Button;
            oButton.Parent.Parent.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion " GRN List View Events "

    #region " Item Details List View Events "

    /// <summary>
    /// This event is used modify count of item in GRN or to remove item from GRN.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstVwItemDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);

            ListView olstVwItemDetails = sender as ListView;

            int iPOID = Convert.ToInt32(olstVwItemDetails.DataKeys[iRowId]["POID"]);
            int iItemID = Convert.ToInt32(olstVwItemDetails.DataKeys[iRowId]["ItemID"]);
            double dQuantity = Convert.ToDouble(olstVwItemDetails.DataKeys[iRowId]["ItemOriginalQty"]);
            TextBox otxtQty = e.Item.FindControl("txtQty") as TextBox;
            HiddenField ohidActualQty = olstVwItemDetails.Items[iRowId].FindControl("hidActualQty") as HiddenField;
            Label olblItemCode = e.Item.FindControl("lblItemCode") as Label;
            Label olblItemName = e.Item.FindControl("lblItemName") as Label;
            Label olblPOCode = e.Item.FindControl("lblPOCode") as Label;
            //Label olblUnit = e.Item.FindControl("lblUnit") as Label;
            DropDownList cmbUnits = e.Item.FindControl("cmbUnits") as DropDownList;
            string sName = olstVwItemDetails.DataKeys[iRowId]["ItemUnit"].ToString();

            GRNItemsDetailsStruct oGRNItemsDetailsStruct = new GRNItemsDetailsStruct();
            if (hidIsModify.Value == "Y")
            {
                ohidActualQty.Value = "0";
                oGRNItemsDetailsStruct.mdRejectedQty = dQuantity;
            }
            oGRNItemsDetailsStruct.miItemID = iItemID;
            oGRNItemsDetailsStruct.miPOID = iPOID;
            oGRNItemsDetailsStruct.mdOriginalQty = Convert.ToDouble(ohidActualQty.Value);
            if (otxtQty.Text.Trim() != ".")
                oGRNItemsDetailsStruct.mdGRNQty = Convert.ToDouble(otxtQty.Text);
            oGRNItemsDetailsStruct.msItemName = olblItemName.Text.Trim();
            oGRNItemsDetailsStruct.msItemCode = olblItemCode.Text.Trim();
            oGRNItemsDetailsStruct.msPOCode = olblPOCode.Text.Trim();
            oGRNItemsDetailsStruct.msItemUnit = sName;
            oGRNItemsDetailsStruct.msUnits = cmbUnits.SelectedIndex.ToString();
            if (e.CommandName == "Modify")
                AddItemsQtyToDataTable(oGRNItemsDetailsStruct);
            else if (e.CommandName == S_COMMAND_REMOVE)
            {
                int iCount;
                DataTable oDTItemsDetails = (DataTable)ViewState[S_LISTVIEW_DATASOURCE];

                for (iCount = 0; iCount < oDTItemsDetails.Rows.Count; iCount++)
                {
                    if (Convert.ToInt32(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_ITEM_ID]) == oGRNItemsDetailsStruct.miItemID && Convert.ToInt32(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_PO_ID]) == oGRNItemsDetailsStruct.miPOID)
                        break;
                }

                DataRow oDTRow = oDTItemsDetails.Rows[iCount];
                oDTRow.Delete();
                oDTItemsDetails.AcceptChanges();
                ViewState[S_LISTVIEW_DATASOURCE] = oDTItemsDetails;
            }

            DataTable oDTItemsForGRN = (DataTable)ViewState[S_LISTVIEW_DATASOURCE];
            if (oDTItemsForGRN.Rows.Count > 0)
            {
                EnumerableRowCollection<DataRow> query = from order in oDTItemsForGRN.AsEnumerable()
                                                         where order.Field<Int32>(S_DB_COLUMN_ITEM_ID) == iItemID
                                                         select order;

                DataView view = query.AsDataView();
                olstVwItemDetails.DataSource = view;
                olstVwItemDetails.DataBind();

                if (view.Count > 0)
                    olstVwItemDetails.Parent.Parent.Visible = true;
                else
                {
                    olstVwItemDetails.Parent.Parent.Visible = false;
                }
            }
            else
                FillGRNItemListView();
            // This part hides PO item detals list view.
            lstvwPOItems.DataSource = null;
            lstvwPOItems.DataBind();
            trlstvwPOItems.Visible = false;
            btnAddAll.Visible = false;

            SetQuantityForItem(iItemID);
            if (lstvwItemWiseDetails.Visible == true)
                AddSortImage(lstvwItemWiseDetails, S_DEFAULT_SORT_EXP_ITEM);
            else
                AddSortImage(lstvwPOItems, S_DEFAULT_SORT_EXP_PO);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is to bind item detail to lstVwItemDetails list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstVwItemDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListView oListView = sender as ListView;
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                DataRowView oDataRowView = oCurrentItem.DataItem as DataRowView;

                DropDownList cmbUnits = e.Item.FindControl("cmbUnits") as DropDownList;
                string sUOM = oListView.DataKeys[e.Item.DisplayIndex]["ItemUnit"].ToString();
                cmbUnits.Items.Clear();
                cmbUnits.Items.Add(new ListItem { Text = sUOM, Value = Constants.S_ZERO });
                cmbUnits.Items.Add(new ListItem { Text = Constants.S_UNITS, Value = Constants.S_ONE });

                int iPieceCount = Convert.ToInt32(hidPieceCount.Value);
                TextBox txtCurrenItem = oCurrentItem.FindControl("txtQty") as TextBox;

                if (cmbUnits.SelectedIndex == Constants.I_ZERO)
                {
                    if (iPieceCount != Constants.I_ONE)
                    {
                        int iTextValue = txtCurrenItem.Text.ToDecimal().ToInt();
                        if (iTextValue % iPieceCount == Constants.I_ZERO)
                        {
                            txtCurrenItem.Text = Convert.ToString(txtCurrenItem.Text.ToDecimal() / iPieceCount);
                        }
                        else
                        {
                            cmbUnits.SelectedIndex = Constants.I_ONE;
                            cmbUnits.Enabled = false;
                        }
                    }
                    else
                    {
                        cmbUnits.SelectedIndex = Constants.I_ONE;
                        cmbUnits.Enabled = false;
                    }
                }

                TextBox otxtQty = e.Item.FindControl("txtQty") as TextBox;
                LinkButton olnkbtnUpdate = e.Item.FindControl("lnkbtnUpdate") as LinkButton;
                LinkButton olnkbtnRemove = e.Item.FindControl("lnkbtnRemove") as LinkButton;
                Label olblName = e.Item.FindControl("lblItemName") as Label;
                Label olblPOCode = e.Item.FindControl("lblPOCode") as Label;
                HiddenField ohidActualQty = e.Item.FindControl("hidActualQty") as HiddenField;

                //olnkbtnUpdate.Attributes.Add("Onclick", "SetValueToHiddenField('" + otxtQty.ClientID + "', '" + ohidActualQty.ClientID + "', '" + olblName.ClientID + "' ,'" + olblPOCode.ClientID + "')");

                if (hidIsModify.Value == "N" && hidGRNId.Value != "0")
                {
                    olnkbtnUpdate.Enabled = false;
                    olnkbtnRemove.Enabled = false;
                    lblMandatory.Visible = false;
                    lblDespMendMark.Visible = false;
                    txtDescription.ReadOnly = true;
                }
                else
                {
                    olnkbtnUpdate.Enabled = true;
                    olnkbtnRemove.Enabled = true;
                }

                olnkbtnUpdate.Attributes.Add("Onclick",
                                          "SetValueToHiddenField('" + otxtQty.ClientID + "', '" +
                                          ohidActualQty.ClientID + "', '" + cmbUnits.ClientID + "','" + olblName.ClientID + "' ,'" +
                                          olblPOCode.ClientID + "')");
                btnSave.Attributes.Add("Onclick",
                                          "SetValueToHiddenField('" + otxtQty.ClientID + "', '" +
                                          ohidActualQty.ClientID + "', '" + cmbUnits.ClientID + "','" + olblName.ClientID + "' ,'" +
                                          olblPOCode.ClientID + "')");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion " Item Details List View Events "

    #endregion " List View Events "

    #region " Private Methods "

    /// <summary>
    /// This method is used bind java script effect to button.
    /// </summary>
    private void SetClientScriptAttributes()
    {
        valsumGRN.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        ApplyMouseHoverEffect(new List<Button> { btnCancel, btnSave, btnAddAll, btnBack, btnModify });
        
    }

    /// <summary>
    /// This method is used to initialize form according to add/view mode of GRN.
    /// </summary>
    private void InitializeForm()
    {
        //If form in add new GRN mode then only value of hidGRNId = 0.
        //Otherwise the value of the hidGRNId is the existing GRN that select to view.
        if (hidGRNId.Value != null && hidGRNId.Value != "0")
            SetFormInViewMode();
        else
        {
            SetNewGRNMode();
            hidIsModify.Value = "N";
            btnModify.Visible = false;
            btnCancel.Visible = false;
        }
    }

    /// <summary>
    /// This method is used to set properties at add GRN mode.
    /// </summary>
    private void SetNewGRNMode()
    {
        SetDefaultProperties();
        lstvwItemWiseDetails.DataSourceID = objDSPODetails.ID;
    }

    /// <summary>
    /// This method is used to set page properties in view mode.
    /// </summary>
    private void SetFormInViewMode()
    {
        hidIsModify.Value = "N";
        int iGRNId = Convert.ToInt32(hidGRNId.Value);

        GRNDetailsBL oGRNDetailsBL = new GRNDetailsBL();
        //This dataset contain all informattion of GRN.
        DataSet oDSGRNItemsDetails = oGRNDetailsBL.GetGRNItemsDetails(iGRNId, miSchoolId);

        //This Datatable[0] contains all item details in GRN
        DataTable oDTGRNItems = oDSGRNItemsDetails.Tables[0];

        //This Datatable[1] contains GRN description.
        txtDescription.Text = oDSGRNItemsDetails.Tables[1].Rows[0][0].ToString();

        ViewState[S_LISTVIEW_DATASOURCE] = null;

        for (int iCount = 0; iCount < oDTGRNItems.Rows.Count; iCount++)
        {
            GRNItemsDetailsStruct oGRNItemsDetailsStruct = new GRNItemsDetailsStruct();
            oGRNItemsDetailsStruct.miItemID = Convert.ToInt32(oDTGRNItems.Rows[iCount]["ItemID"]);
            oGRNItemsDetailsStruct.miPOID = Convert.ToInt32(oDTGRNItems.Rows[iCount]["PurchaseOrderID"]);
            oGRNItemsDetailsStruct.msItemCode = Convert.ToString(oDTGRNItems.Rows[iCount]["ItemCode"]);
            oGRNItemsDetailsStruct.msItemName = Convert.ToString(oDTGRNItems.Rows[iCount]["ItemName"]);
            oGRNItemsDetailsStruct.mdOriginalQty = Convert.ToDouble(oDTGRNItems.Rows[iCount]["ItemQty"]);
            oGRNItemsDetailsStruct.mdGRNQty = Convert.ToDouble(oDTGRNItems.Rows[iCount]["ItemQty"]);
            oGRNItemsDetailsStruct.mdRejectedQty = Convert.ToDouble(oDTGRNItems.Rows[iCount]["ItemQty"]);
            oGRNItemsDetailsStruct.msPOCode = Convert.ToString(oDTGRNItems.Rows[iCount]["PurchaseOrderCode"]);
            oGRNItemsDetailsStruct.msItemUnit = Convert.ToString(oDTGRNItems.Rows[iCount]["UOMUnit"]);
            oGRNItemsDetailsStruct.mdPOOrgQty = Convert.ToDouble(oDTGRNItems.Rows[iCount]["ItemOrgQty"]);
            oGRNItemsDetailsStruct.msUMOName = Convert.ToString(oDTGRNItems.Rows[iCount]["UOMName"]);
            oGRNItemsDetailsStruct.iPieceCount = Convert.ToInt32(oDTGRNItems.Rows[iCount]["PieceCount"]);
            AddItemsQtyToDataTable(oGRNItemsDetailsStruct);
        }
        FillGRNItemListView();

        ShowHideControlAtAddMode(false);
        trPOWiseDetails.Visible = false;
        trlstvwPOItems.Visible = false;
        btnAddAll.Visible = false;
    }

    /// <summary>
    /// This method is used show controls at new GRN creation mode.
    /// </summary>
    /// <param name="bFlag"></param>
    private void ShowHideControlAtAddMode(bool abFlag)
    {
        trViewOptionButton.Visible = abFlag;
        trItemWiseDetails.Visible = abFlag;
        DtPgCount.Visible = abFlag;
        btnCancel.Visible = abFlag;
        btnSave.Visible = abFlag;
        lblMandatory.Visible = abFlag;
        lblDespMendMark.Visible = abFlag;
        txtDescription.ReadOnly = !abFlag;
    }

    private string GetItemQuantity(double dcQuantity, int aiUOMUnitCount, string asUnitName)
    {
        string sQuantity = string.Empty;
        if (aiUOMUnitCount > 1)
        {
            if (dcQuantity % aiUOMUnitCount == 0)
                sQuantity = Math.Round(dcQuantity / aiUOMUnitCount, 2) + " " + asUnitName + " / " + dcQuantity + " " + Constants.S_UNITS;
            else
                sQuantity = dcQuantity + " " + Constants.S_UNITS;
        }
        else
            sQuantity = dcQuantity + " " + asUnitName;
        return sQuantity;
    }

    /// <summary>
    /// This method is show/hide controls add mode. 
    /// </summary>
    /// <param name="abFlag"></param>
    private void ShowHideControls(bool abFlag)
    {
        trItemWiseDetails.Visible = abFlag;
        DtPgCount.Visible = abFlag;
        optItemWise.Checked = abFlag;
        trPOWiseDetails.Visible = !abFlag;
        DtPOPgCount.Visible = !abFlag;
        optPOWise.Checked = !abFlag;
        trlstvwPOItems.Visible = false;
        btnAddAll.Visible = false;
    }

    /// <summary>
    /// This method is used set default properties for controls.
    /// </summary>
    private void SetDefaultProperties()
    {
        optItemWise.Checked = true;
        optPOWise.Checked = false;
        btnAddAll.Visible = false;

    }

    /// <summary>
    /// This method is used set soting direction.
    /// </summary>
    private void SetSortVariables()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to set new updated quantity when item added in GRN.
    /// </summary>
    /// <param name="aiItemID"></param>
    private void SetQuantityForItem(int aiItemID)
    {
        DataTable oDTItemsForGRN = ViewState[S_LISTVIEW_DATASOURCE] as DataTable;
        int iCount = 0;
        if (oDTItemsForGRN.Rows.Count > 0)
        {
            foreach (ListViewDataItem oListViewDataItem in lstvwGRN.Items)
            {
                int iRowIndex = Convert.ToInt32(oListViewDataItem.DataItemIndex);
                Label oItemQty = oListViewDataItem.FindControl("lblQty") as Label;

                if (Convert.ToDouble(lstvwGRN.DataKeys[iRowIndex]["ItemID"]) == aiItemID)
                {
                    var query = from GRNItem in oDTItemsForGRN.AsEnumerable()
                                where GRNItem.Field<Int32>("ItemID") == aiItemID
                                group GRNItem by GRNItem.Field<Int32>("ItemID")
                                    into GRNItemGroup
                                    select new
                                    {
                                        ItemId = GRNItemGroup.Key,
                                        Qty = GRNItemGroup.Sum(GRNItem => GRNItem.Field<double>(S_DB_COLUMN_ITEM_GRN_QUANTITY)),
                                    };

                    foreach (var order in query)
                    {
                        iCount = 1;
                        oItemQty.Text = (order.Qty).ToString();
                    }
                    if (iCount == 0)
                        oListViewDataItem.Visible = false;
                    break;
                }
            }
        }
    }

    /// <summary>
    /// This event is used to read query string and to set default properties at view mode.
    /// </summary>
    private void ReadQueryString()
    {
        if (QueryString["GRNID"] != null)
            hidGRNId.Value = QueryString["GRNID"];
    }

    /// <summary>
    /// This method is used to add sort image in list view column header.
    /// </summary>
    /// <param name="aoListView"></param>
    /// <param name="asSortExpression"></param>
    private void AddSortImage(ListView aoListView, string asSortExpression)
    {
        if (aoListView.SortDirection.ToString() == "Ascending" || aoListView.SortDirection.ToString() == string.Empty)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
        if (aoListView.SortExpression != string.Empty)
            hidSortExpression.Value = aoListView.SortExpression.ToString();
        else
            hidSortExpression.Value = asSortExpression;
        HtmlTableRow oHtmlTableHeaderRow = aoListView.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    /// <summary>
    /// This method is used to add item row in viewstate table. 
    /// </summary>
    private void AddItemsQtyToDataTable(GRNItemsDetailsStruct oGRNItemsDetailsStruct)
    {
        bool bIsNew = true;
        int iCount;
        DataTable oDTItemsDetails;
        if (ViewState[S_LISTVIEW_DATASOURCE] == null)
            oDTItemsDetails = CreateItemsQtyTable();
        else
            oDTItemsDetails = ViewState[S_LISTVIEW_DATASOURCE] as DataTable;

        for (iCount = 0; iCount < oDTItemsDetails.Rows.Count; iCount++)
        {
            if (Convert.ToInt32(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_ITEM_ID]) == oGRNItemsDetailsStruct.miItemID && Convert.ToInt32(oDTItemsDetails.Rows[iCount][S_DB_COLUMN_PO_ID]) == oGRNItemsDetailsStruct.miPOID)
            {
                bIsNew = false;
                break;
            }
        }

        // Once a table has been created,create DataRow.
        if (bIsNew)
            oDTItemsDetails.Rows.Add(AddPOItemsQtyToDataRow(oDTItemsDetails.NewRow(), oGRNItemsDetailsStruct));
        else
        {
            DataRow oDTRow = oDTItemsDetails.Rows[iCount];
            oDTRow.BeginEdit();
            oDTRow[2] = (double)oGRNItemsDetailsStruct.mdGRNQty;
            if (oGRNItemsDetailsStruct.mdOriginalQty != 0)
            {
                oDTRow[4] = (oGRNItemsDetailsStruct.mdOriginalQty - oGRNItemsDetailsStruct.mdGRNQty);
                oDTRow[5] = (oGRNItemsDetailsStruct.mdOriginalQty - oGRNItemsDetailsStruct.mdGRNQty);
            }
            else
            {
                oDTRow[4] = (oGRNItemsDetailsStruct.mdRejectedQty - oGRNItemsDetailsStruct.mdGRNQty);
                oDTRow[5] = (oGRNItemsDetailsStruct.mdRejectedQty - oGRNItemsDetailsStruct.mdGRNQty);
            }
            oDTItemsDetails.AcceptChanges();
            oDTItemsDetails.Rows[iCount].EndEdit();
        }
        ViewState[S_LISTVIEW_DATASOURCE] = oDTItemsDetails;
    }

    /// <summary>
    /// This method is used to create table.
    /// </summary>
    /// <returns></returns>
    private DataTable CreateItemsQtyTable()
    {
        // Create a new DataTable for GRN items details. 
        DataTable oDTItemsDetails = new DataTable();

        // Add columns to the Item table.        
        AddDataColumnToItemQtyTable("System.Int32", S_DB_COLUMN_ITEM_ID, ref oDTItemsDetails, false);
        AddDataColumnToItemQtyTable("System.Int32", S_DB_COLUMN_PO_ID, ref oDTItemsDetails, false);
        AddDataColumnToItemQtyTable("System.Double", S_DB_COLUMN_ITEM_GRN_QUANTITY, ref oDTItemsDetails, false);
        AddDataColumnToItemQtyTable("System.Double", S_DB_COLUMN_ITEM_QUANTITY, ref oDTItemsDetails, false);
        AddDataColumnToItemQtyTable("System.Double", S_DB_COLUMN_ITEM_DIFF, ref oDTItemsDetails, false);
        AddDataColumnToItemQtyTable("System.Double", S_DB_COLUMN_ITEM_REJECTED_QUANTITY, ref oDTItemsDetails, false);
        AddDataColumnToItemQtyTable("System.String", S_DB_COLUMN_ITEM_NAME, ref oDTItemsDetails, false);
        AddDataColumnToItemQtyTable("System.String", S_DB_COLUMN_ITEM_CODE, ref oDTItemsDetails, false);
        AddDataColumnToItemQtyTable("System.String", S_DB_COLUMN_PO_CODE, ref oDTItemsDetails, false);
        AddDataColumnToItemQtyTable("System.String", S_ITEM_UNIT, ref oDTItemsDetails, false);
        AddDataColumnToItemQtyTable("System.String", S_UNITS, ref oDTItemsDetails, false);
        AddDataColumnToItemQtyTable("System.Double", S_DB_COLUMN_ITEM_PO_ORG_QUANTITY, ref oDTItemsDetails, false);
        AddDataColumnToItemQtyTable("System.String", S_UNIT_NAME, ref oDTItemsDetails, false);
        AddDataColumnToItemQtyTable("System.Int32", S_PIECE_COUNT, ref oDTItemsDetails, false);

        return oDTItemsDetails;
    }

    /// <summary>
    /// This method is used add column to table.
    /// </summary>
    /// <param name="asDataType"></param>
    /// <param name="asColumnName"></param>
    /// <param name="aoDataTable"></param>
    /// <param name="abIsPrimaryKey"></param>
    private void AddDataColumnToItemQtyTable(string asDataType, string asColumnName, ref DataTable aoDataTable,
                                                              bool abIsPrimaryKey)
    {
        DataColumn oDataColumn = new DataColumn();
        oDataColumn.DataType = Type.GetType(asDataType);
        oDataColumn.ColumnName = asColumnName;
        aoDataTable.Columns.Add(oDataColumn);

        if (abIsPrimaryKey)
        {
            // Create an array for DataColumn objects.
            DataColumn[] keys = new DataColumn[1];
            keys[0] = oDataColumn;
            aoDataTable.PrimaryKey = keys;
        }
    }
    /// <summary>
    /// This methods is used to fill GRN list view.
    /// </summary>
    private void FillGRNItemListView()
    {
        if (ViewState[S_LISTVIEW_DATASOURCE] != null)
        {
            DataTable oDTItemsForGRN = (DataTable)ViewState[S_LISTVIEW_DATASOURCE];

            //In this LINQ we take sum of all quantities for same item from different purchase order.
            //And Then bind this LINQ to the lstvwGRN List View.
            var query = from GRNItem in oDTItemsForGRN.AsEnumerable()
                        group GRNItem by new { Id = GRNItem.Field<Int32>("ItemID"), Code = GRNItem.Field<string>(S_DB_COLUMN_ITEM_CODE), Name = GRNItem.Field<string>(S_DB_COLUMN_ITEM_NAME), Unit = GRNItem.Field<string>(S_ITEM_UNIT), UOMName = GRNItem.Field<string>(S_UNIT_NAME), ItemUnit = GRNItem.Field<string>(S_ITEM_UNIT), PieceCount = GRNItem.Field<Int32>(S_PIECE_COUNT) }
                            into GRNItemGroup
                            select new
                            {
                                ItemId = GRNItemGroup.Key.Id,
                                ItemCode = GRNItemGroup.Key.Code,
                                ItemName = GRNItemGroup.Key.Name,
                                ItemUnit = GRNItemGroup.Key.ItemUnit,
                                ItemPOQty = GRNItemGroup.Sum(GRNItem => GRNItem.Field<double>(S_DB_COLUMN_ITEM_GRN_QUANTITY)),
                                ItemQty = GetItemQuantity(GRNItemGroup.Sum(GRNItem => GRNItem.Field<double>(S_DB_COLUMN_ITEM_GRN_QUANTITY)), GRNItemGroup.Key.PieceCount, GRNItemGroup.Key.Unit),
                                ItemRejectedQty = GRNItemGroup.Sum(GRNItem => GRNItem.Field<double>(S_DB_COLUMN_ITEM_REJECTED_QUANTITY)),
                                ItemQtyDiff = GRNItemGroup.Sum(GRNItem => GRNItem.Field<double>(S_DB_COLUMN_ITEM_DIFF)),
                                ItemUnitName = GRNItemGroup.Key.UOMName,
                                PieceCount = GRNItemGroup.Key.PieceCount,
                            };


            lstvwGRN.DataSource = query;
            lstvwGRN.DataBind();
            hidGRNItemCount.Value = lstvwGRN.Items.Count.ToString();
            SetSaveButton(oDTItemsForGRN);
        }
    }

    /// <summary>
    /// This method is used to show the save button according to list view lstvwGRN
    /// and view state datatable.
    /// </summary>
    /// <param name="oDTItemsForGRN"></param>
    private void SetSaveButton(DataTable oDTItemsForGRN)
    {
        if (oDTItemsForGRN.Rows.Count > 0)
        {
            lstvwGRN.Visible = true;
            trDesc.Visible = true;
            btnSave.Visible = true;
            btnSave.Text = "Save";
        }
        else
        {
            lstvwGRN.Visible = false;
            if (hidGRNId.Value != "0")
                btnSave.Text = "Delete";
            else
            {
                trDesc.Visible = false;
                btnSave.Visible = false;
            }
        }
    }
    /// <summary>
    /// This method to add items to data row.
    /// </summary>
    /// <param name="oDR"></param>
    /// <returns></returns>
    private DataRow AddPOItemsQtyToDataRow(DataRow oDR, GRNItemsDetailsStruct oGRNItemsDetailsStruct)
    {
        DataRow oDRItem;

        oDRItem = oDR;
        // Then add the new row to the collection.
        oDRItem[S_DB_COLUMN_ITEM_ID] = oGRNItemsDetailsStruct.miItemID;
        oDRItem[S_DB_COLUMN_PO_ID] = oGRNItemsDetailsStruct.miPOID;
        oDRItem[S_DB_COLUMN_ITEM_GRN_QUANTITY] = oGRNItemsDetailsStruct.mdGRNQty;
        oDRItem[S_DB_COLUMN_ITEM_QUANTITY] = oGRNItemsDetailsStruct.mdOriginalQty;
        if (oGRNItemsDetailsStruct.mdOriginalQty != 0)
            oDRItem[S_DB_COLUMN_ITEM_DIFF] = oGRNItemsDetailsStruct.mdOriginalQty - oGRNItemsDetailsStruct.mdGRNQty;
        else
            oDRItem[S_DB_COLUMN_ITEM_DIFF] = 0;
        oDRItem[S_DB_COLUMN_ITEM_REJECTED_QUANTITY] = oGRNItemsDetailsStruct.mdOriginalQty - oGRNItemsDetailsStruct.mdGRNQty;
        oDRItem[S_DB_COLUMN_ITEM_NAME] = oGRNItemsDetailsStruct.msItemName;
        oDRItem[S_DB_COLUMN_ITEM_CODE] = oGRNItemsDetailsStruct.msItemCode;
        oDRItem[S_DB_COLUMN_PO_CODE] = oGRNItemsDetailsStruct.msPOCode;
        oDRItem[S_ITEM_UNIT] = oGRNItemsDetailsStruct.msItemUnit;
        oDRItem[S_DB_COLUMN_ITEM_PO_ORG_QUANTITY] = oGRNItemsDetailsStruct.mdPOOrgQty;
        oDRItem[S_UNITS] = oGRNItemsDetailsStruct.msUnits;
        oDRItem[S_UNIT_NAME] = oGRNItemsDetailsStruct.msUMOName;
        oDRItem[S_PIECE_COUNT] = oGRNItemsDetailsStruct.iPieceCount;

        return oDRItem;
    }

    /// <summary>
    /// This method is used to create XML for items in GRN.
    /// </summary>
    /// <returns></returns>
    private string GenerateGRNPOItemXML()
    {
        DataTable oDTPOReqItems = (DataTable)ViewState[S_LISTVIEW_DATASOURCE];
        const string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("GRNPOItems");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "GRNPOItems", "");


        for (int iCount = 0; iCount < oDTPOReqItems.Rows.Count; iCount++)
        {
            if (Convert.ToInt32(oDTPOReqItems.Rows[iCount][S_DB_COLUMN_PO_ID]) != 0)
            {
                // Create root xml element.
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "GRNPOItems", "");

                string sAtrrName = "POId";
                XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = oDTPOReqItems.Rows[iCount][S_DB_COLUMN_PO_ID].ToString();

                oXmlNode.Attributes.Append(attr);


                sAtrrName = "ItemID";
                attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = oDTPOReqItems.Rows[iCount][S_DB_COLUMN_ITEM_ID].ToString();

                oXmlNode.Attributes.Append(attr);

                sAtrrName = "ItemQty";
                attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = oDTPOReqItems.Rows[iCount][S_DB_COLUMN_ITEM_GRN_QUANTITY].ToString();

                oXmlNode.Attributes.Append(attr);

                sAtrrName = "Units";
                attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = oDTPOReqItems.Rows[iCount][S_UNITS].ToString();

                oXmlNode.Attributes.Append(attr);

                // Add the node to root node.
                oXmlRootNode.AppendChild(oXmlNode);
            }
        }
        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);

        // return the string generated.
        return root.InnerXml;
    }

    /// <summary>
    /// This method is used to create XML for items according to purchase order.
    /// </summary>
    /// <returns></returns>
    private string GenerateGRNItemXML()
    {
        string sGRNName = string.Empty;
        const string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("GRNItemsDetails");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "GRNItemsDetails", "");

        // Loop through all the list view rows.
        foreach (ListViewDataItem oListViewDataItem in lstvwGRN.Items)
        {
            // Create root xml element.
            XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "GRNItemsDetails", "");

            int iRowId = Convert.ToInt32(oListViewDataItem.DataItemIndex);
            HiddenField oItemQty = oListViewDataItem.FindControl("hidItemQty") as HiddenField;
            //Label oItemQty = (Label)oListViewDataItem.FindControl("lblQty");
            Label olblItemName = (Label)oListViewDataItem.FindControl("lblName");


            string sAtrrName = "ItemID";
            XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = (lstvwGRN.DataKeys[iRowId]["ItemID"]).ToString();
            oXmlNode.Attributes.Append(attr);

            sAtrrName = "ReceivedItemQty";
            attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = oItemQty.Value;
            oXmlNode.Attributes.Append(attr);

            sAtrrName = "RejectedItemQty";
            attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = (lstvwGRN.DataKeys[iRowId]["ItemRejectedQty"]).ToString();
            oXmlNode.Attributes.Append(attr);

            sAtrrName = "ItemQtyDiff";
            attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = (lstvwGRN.DataKeys[iRowId]["ItemQtyDiff"]).ToString();
            oXmlNode.Attributes.Append(attr);

            // Add the node to root node.
            oXmlRootNode.AppendChild(oXmlNode);

            sGRNName += olblItemName.Text + ",";
        }
        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);

        sGRNName = sGRNName.Remove(sGRNName.LastIndexOf(","));
        if (sGRNName.Length > I_GRN_NAME_LENGTH)
            sGRNName = sGRNName.Substring(Constants.I_ZERO, I_GRN_NAME_LENGTH);
        hidGRNName.Value = sGRNName;

        // return the string generated.
        return root.InnerXml;
    }

    #endregion " Private Methods "
}