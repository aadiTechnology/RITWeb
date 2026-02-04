// File Name  : RequisitionListUI.aspx.cs
// Created By : Milind
// Date       : 26/6/2009
//Description : This class is used to show list of requisition according to the status.

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using SchoolEntities.Inventory;

public partial class RequisitionListUI : SchoolBase
{

    #region Constants

    const string S_DB_COLUMN_STATUS_ID = "StatusID";
    const string S_DB_COLUMN_STATUS_NAME = "StatusName";
    const string S_COMMAND_REMOVE = "Remove";
    const string S_COMMAND_CANCEL = "CANCEL_COMMAND";
    const string S_MODE_EDIT = "Edit";
    const string S_MODE_VIEW = "View";
    const string S_CANCEL_MESSAGE = "Requisition canceled successfully !!!";
    const string S_DEL_REQ_SUB = "Requisition deleted";
    const string S_DEL_REQ_MSG = "Requisition (%Code%) is deleted by %DeletedName%.";
    const string S_CAN_REQ_MSG = "Requisition (%Code%) is Canceled by %CanceledByName%.";
    const string S_DEFUALT_SORT_EXPR = "Created_Date";

    #endregion

    #region Events

    

    #region Page Events

    /// <summary>
    /// This event is used to fill the status combo box and list view according to selected value in the combo box.
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
                FillStatusCombobox();
                hidSortDirection.Value = Constants.S_ASCENDING;
                lstvwRequisition.DataSourceID = lstDSobj.ID;
                ApplyMouseHoverEffect(new List<Button> { btnAdd});
                HidereqNameColumn();

            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill requisition in list view as per status of requisition.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lstvwRequisition.DataSourceID = lstDSobj.ID;
            lstvwRequisition.DataBind();
            HidereqNameColumn();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    #endregion

    #region ListView Events

    /// <summary>
    /// This event is used to fill the drop down list in the datapager according to pagesize.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwRequisition_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwRequisition.Items.Count > 0)
            {

                ControlUtility.FillListViewPagerFooter(lstvwRequisition, DtPgCount);
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
    /// This event is used to add the attributes to the image button(Edit) in the list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwRequisition_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ImageButton imgbtnEdit = e.Item.FindControl("imgbtnEditReq") as ImageButton;
                ImageButton imgbtnView = e.Item.FindControl("imgbtnViewReq") as ImageButton;
                ImageButton imgbtnDelete = e.Item.FindControl("imgbtnDeleteReq") as ImageButton;
                LinkButton imgbtnCancel = e.Item.FindControl("imgbtnCancelReq") as LinkButton;
                
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);

                string sQueryStringEdit = "&Mode=" + S_MODE_EDIT;
                string sQueryStringView = "&Mode=" + S_MODE_VIEW;

                string sQueryString = "RequisitionID=" + lstvwRequisition.DataKeys[iRowId]["RequisitionID"].ToString();
                sQueryString += "&StatusID=" + ddlStatus.SelectedValue;
                sQueryString += "&NextDesignationId=" + lstvwRequisition.DataKeys[iRowId]["NextDesignationId"].ToString();
                sQueryString += "&CreatorName=" + lstvwRequisition.DataKeys[iRowId]["CreaterName"].ToString();
                sQueryString += "&RequisitionCode=" + lstvwRequisition.DataKeys[iRowId]["RequisitionCode"].ToString();
                sQueryString += "&IsFinalApproval=" + Convert.ToBoolean(lstvwRequisition.DataKeys[iRowId]["IsFinalApproval"]);
                sQueryString += "&CreatorID=" + lstvwRequisition.DataKeys[iRowId]["CreatedId"].ToString();

                imgbtnEdit.Attributes.Add("onclick", "window.open('../Inventory/AddRequisitionUI.aspx?" + CommonUtility.EncryptQuerystring(sQueryString.ToString() + sQueryStringEdit.ToString()) + " ' , '_self');return false;");

                imgbtnView.Attributes.Add("onclick", "window.open('../Inventory/AddRequisitionUI.aspx?" + CommonUtility.EncryptQuerystring(sQueryString.ToString() + sQueryStringView.ToString())
                                                             + " ' , '_self');return false;");
                imgbtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");

                int iUserid = Convert.ToInt32(lstvwRequisition.DataKeys[iRowId]["CreatedId"]);

                if ((lstvwRequisition.DataKeys[iRowId]["StatusID"].ToInt() == Constants.I_THREE || lstvwRequisition.DataKeys[iRowId]["StatusID"].ToInt() == Constants.I_EIGHT) && miUserId != iUserid)
                    imgbtnCancel.Visible = true;
                else
                    imgbtnCancel.Visible = false;
				
				HtmlTableCell oReqname = (HtmlTableCell)e.Item.FindControl("tdReqName");
				if(oReqname != null)
				{
					if (miSchoolId == Constants.SchoolId.PPSN.ToInt())
						oReqname.Visible = false;
					else
						oReqname.Visible = true;
				}
                Label lblExpiryDate = e.Item.FindControl("lblExpiryDate") as Label;
                if (lstvwRequisition.DataKeys[iRowId]["ExpiryDate"] == DBNull.Value)
                    lblExpiryDate.Text = "-";
              }
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
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwRequisition);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to delete the list view items.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwRequisition_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                if (e.CommandName == S_COMMAND_REMOVE)
                {
                    int iRequisitionID = Convert.ToInt32(((ImageButton)(e.CommandSource)).CommandArgument);

                    RequisitionBL oRequisitionBL = new RequisitionBL();
                    DataTable oDTUserId = oRequisitionBL.DeleteRequisition(iRequisitionID, miSchoolId);

                    //If NextDesignationId is not null that time send the message of 
                    //delete requisition to the next authority also.
                    if (lstvwRequisition.DataKeys[iRowId]["NextDesignationId"] != null)
                    {
                        if (oDTUserId.Rows.Count > 0)
                        {
                            string sUserID = Constants.S_EMPTY_STRING;
                            for (int iCount = 0; iCount < oDTUserId.Rows.Count; iCount++)
                                sUserID += oDTUserId.Rows[iCount]["User_Id"].ToString() + ";";
                            if (lstvwRequisition.DataKeys[iRowId]["CreatedId"].ToString() != miUserId.ToString())
                                sUserID += lstvwRequisition.DataKeys[iRowId]["CreatedId"].ToString();
                            else if (sUserID.Contains(";"))
                                sUserID = sUserID.Substring(0, sUserID.IndexOf(";"));
                            string sMessageBody = S_DEL_REQ_MSG;
                            sMessageBody = sMessageBody.Replace("%Code%", lstvwRequisition.DataKeys[iRowId]["RequisitionCode"].ToString());
                            sMessageBody = sMessageBody.Replace("%DeletedName%", Convert.ToString(Session[Constants.S_SESSION_USER_FULLNAME]));
                            SendMessageAboutAction(sUserID, S_DEL_REQ_SUB, sMessageBody);
                        }
                    }
                    lstvwRequisition.DataSourceID = lstDSobj.ID;
                    HidereqNameColumn();
                  }
                else if (e.CommandName == S_COMMAND_CANCEL)
                {
                    Label oLabelRequisition = e.Item.FindControl("lblName") as Label;
                    Label oLabelStatus = e.Item.FindControl("lblStatus") as Label;
                    Label oLabelCreater = e.Item.FindControl("lblCreaterName") as Label;
                    LinkButton imgbtnCancel = e.Item.FindControl("imgbtnCancelReq") as LinkButton;

                    hidRequisitionId.Value = lstvwRequisition.DataKeys[iRowId]["RequisitionID"].ToString();
                    hidCode.Value = lstvwRequisition.DataKeys[iRowId]["RequisitionCode"].ToString();
                    hidRequistion.Value = oLabelRequisition.Text;
                    hidStatus.Value = oLabelStatus.Text;
                    hidRequester.Value = oLabelCreater.Text;
                    hidCreatedId.Value = lstvwRequisition.DataKeys[iRowId]["CreatedId"].ToString();

                    ScriptManager.RegisterStartupScript(lstvwRequisition, this.GetType(), "OpenReasonWindow", "OpenReasonWindow();", true);
                    lstvwRequisition.DataSourceID = lstDSobj.ID;
                    HidereqNameColumn();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to add sort image on the header of sorting column according to the sort direction. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwRequisition_Sorting(object sender, ListViewSortEventArgs e)
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
    /// This event is used to save revert details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            RevertRequisition();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #endregion

    #region Private methods

    /// <summary>
    /// This method is used to fill status combo.
    /// </summary>
    private void FillStatusCombobox()
    {
        RequisitionBL oRequisitionBL = new RequisitionBL();
        DataTable oDTStatus = oRequisitionBL.GetStatusList();

        //var rows = oDTStatus.Select("StatusID = 6");
        //foreach (var row in rows)
        //    row.Delete();

        ControlUtility.FillDropDownList(oDTStatus, ref ddlStatus, S_DB_COLUMN_STATUS_ID, S_DB_COLUMN_STATUS_NAME, Constants.S_EMPTY_STRING);

        ddlStatus.SelectedValue = hidStatusID.Value;
    }

    /// <summary>
    /// This method is used to set sort variables.
    /// </summary>
    private void SetSortVariables()
    {
        hidSortDirection.Value = hidSortDirection.Value == Constants.S_DESCENDING ? Constants.S_ASCENDING : Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to decrypt the encrypted querystring.
    /// And read that querystring.
    /// </summary>
    private void ReadQueryString()
    {
        if (QueryString["StatusID"] != null)
            hidStatusID.Value = QueryString["StatusID"];
    }

    /// <summary>
    /// This method is used to send the internal message about the action taken on the requisiiton.
    /// </summary>
    private void SendMessageAboutAction(string asUserId, string asMsgSubject, string asMsgBody)
    {
        Message oMessage = new Message();
        oMessage.sMessageBody = asMsgBody + " ";
        oMessage.sMessageSubject = asMsgSubject;
        oMessage.SetMessageReceivers(asUserId, miUserId);
        oMessage.InsertMessageDetails(miUserId, moUserRole.ToInt(), miAcademicYearId);
    }

    /// <summary>
    /// This method is used to set sorting image in list view column header.
    /// </summary>
    private void AddSortImage()
    {
        if (lstvwRequisition.SortDirection.ToString() == "Ascending" || lstvwRequisition.SortDirection.ToString() == string.Empty)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
        if (lstvwRequisition.SortExpression != string.Empty)
            hidSortExpression.Value = lstvwRequisition.SortExpression.ToString();
        else
        {
            hidSortExpression.Value = S_DEFUALT_SORT_EXPR;
            hidSortDirection.Value = Constants.S_DESCENDING;
        }
        HtmlTableRow oHtmlTableHeaderRow = lstvwRequisition.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }
    
    /// <summary>
    /// This method is used to revert requisition approved by self only. 
    /// </summary>
    private void RevertRequisition()
    {
        RequisitionDetailsBL oRequisitionDetailsBL = new RequisitionDetailsBL();
        int iRequisitionId = Convert.ToInt32(hidRequisitionId.Value);
        string sReasonText = txtReason.Text.TrimAll();
        int iCanceledById =  oRequisitionDetailsBL.RevertRequisitionDetails(iRequisitionId, sReasonText, miUserId, miSchoolId);
        
            if(iCanceledById != hidCreatedId.Value.ToInt())
            {
                string sUserID = Constants.S_EMPTY_STRING;
                sUserID += iCanceledById.ToString() + ";";
                
                if (hidCreatedId.Value != miUserId.ToString())
                    sUserID += hidCreatedId.Value;
                else if (sUserID.Contains(";"))
                    sUserID = sUserID.Substring(0, sUserID.IndexOf(";"));
                
                string sMessageBody = S_CAN_REQ_MSG;
                sMessageBody = sMessageBody.Replace("%Code%", hidCode.Value.ToString());
                sMessageBody = sMessageBody.Replace("%CanceledByName%", Convert.ToString(Session[Constants.S_SESSION_USER_FULLNAME]));
                SendMessageAboutAction(sUserID, S_CAN_REQ_MSG, sMessageBody);
            }

        ClearControles();
        lstvwRequisition.DataSourceID = lstDSobj.ID;
        lblUpdateSucess.Text = "Requisition canceled successfully !!!";
        HidereqNameColumn();
    }

    /// <summary>
    /// This method is used to clear controles.
    /// </summary>
    private void ClearControles()
    {
        hidRequisitionId.Value = string.Empty;
        hidCode.Value = string.Empty;
        hidRequistion.Value = string.Empty;
        hidStatus.Value = string.Empty;
        hidRequester.Value = string.Empty;
        hidCreatedId.Value = string.Empty;
        txtReason.Text = string.Empty;
    }

    /// <summary>
    /// This method is used to hide Requisition heading in listview.
    /// </summary>
    private void HidereqNameColumn()
    {
        HtmlTableCell oRequisition = lstvwRequisition.FindControl("thRequisition") as HtmlTableCell;
		if(oRequisition != null)
		{
			if (miSchoolId == Constants.SchoolId.PPSN.ToInt())
				oRequisition.Visible = false;
			else
				oRequisition.Visible = true;
		}
    }
    
    #endregion

    
}
