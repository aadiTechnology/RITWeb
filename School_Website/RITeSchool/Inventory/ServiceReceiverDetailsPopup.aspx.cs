using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;
using SchoolEntities;
using BusinessLogic;
using BusinessLogic.Exceptions;
using System.Collections;

public partial class RITeSchool_Transport_TransportDetails : SchoolBase
{
    #region "Constants"

    private const string S_DELETE_MSG = "Service Receiver Details Deleted successfully !!!";
    private const string S_TEXT_UPDATE = "Update";
    private const string S_COMMAND_DELETE = "RemoveCommand";
    private const string S_TEXT_SAVE = "Save";
    private const string S_COMMAND_UPDATE = "UpdateReceiverDetails"; 

    #endregion "Constants"

    #region "Events"

    /// <summary>
    /// This event is used to set default control fields and java script attributes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    ServiceReceiverDetailsBL oServiceReceiverDetailsBL;
    
    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        try
        {
            //if (hidSortExpression.Value == string.Empty)
            //{
            //    hidSortExpression.Value = "Name";
            //    hidSortDirection.Value = Constants.S_DESCENDING;
            //}

            if (hidSortExpression.Value != string.Empty)
                AddSortImage(lstvwTransportDetails, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        oServiceReceiverDetailsBL = new ServiceReceiverDetailsBL(miSchoolId, miAcademicYearId,miUserId);
        if (!IsPostBack)
        {
            SetJavaScriptAttributes();
            FillExistingNameListview();
            SetDefaultValues();
        }
    }

    /// <summary>
    /// This event is used to get Last ReadingTo entry of the service receiver details.
    /// </summary> 
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        ControlUtility.SetDataPagerAccordingToPageNo(lstvwTransportDetails);
    }

    /// <summary>
    /// This method is used to save service receiver details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (this.IsValid)
        {
            ServiceReceiverDetails oServiceReceiverDetails = new ServiceReceiverDetails
            {
                Address = txtaddress.Text.Trim(),
                City = txtcity.Text.Trim(),
                GSTIN = txtGSTIN.Text.Trim(),
                Id = hidId.Value.ToInt(),
                MobileNo = txtmobileno.Text,
                Name = txtName.Text.Trim(),
                Pincode = txtpincode.Text
            };

            oServiceReceiverDetailsBL.Save(oServiceReceiverDetails);
            if (hidId.Value == string.Empty || hidId.Value == "0")
                lblUpdateSucess.Text = "Service receiver details saved successfully !!!";
            else
                lblUpdateSucess.Text = "Service receiver details updated successfully !!!";

            ClearFields();
            FillExistingNameListview();
        }
    }

    /// <summary>
    /// This event is used to cancel service receiver details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void btnCancel_Click(object sender, EventArgs e)
    {
         try
         {
             ClearFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is called while row in list view is clicked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
  
    protected void lstvwTransportDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = oCurrentItem.DisplayIndex;
                int iId = Convert.ToInt32(lstvwTransportDetails.DataKeys[iRowId]["Id"]);

                if (e.CommandName == "UpdateCommand")
                {
                    ServiceReceiverDetails oServiceReceiverDetails = oServiceReceiverDetailsBL.Get(iId);
                    txtName.Text = oServiceReceiverDetails.Name.ToString();
                    txtaddress.Text = oServiceReceiverDetails.Address.ToString();
                    txtcity.Text = oServiceReceiverDetails.City.ToString();
                    txtpincode.Text = oServiceReceiverDetails.Pincode.ToString();
                    txtmobileno.Text = oServiceReceiverDetails.MobileNo.ToString();
                    txtGSTIN.Text = oServiceReceiverDetails.GSTIN.ToString();
                    hidId.Value = iId.ToString();
                    btnSave.Text = Constants.ButtonText.Update.ToString();
                }
                else if (e.CommandName == S_COMMAND_DELETE)
                {
                    Delete(iId);
                    ClearFields();
                    FillExistingNameListview();
                }
            }
        }
        catch (ReferenceExceptions ex)
        {
            lblUpdateSucess.Text = ex.Message;
            lblUpdateSucess.ForeColor = System.Drawing.Color.Red;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }


    /// <summary>
    /// This event is used while loading rows in listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// 
    protected void lstvwTransportDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
            ImageButton btnDelete = oCurrentItem.FindControl("btnDelete") as ImageButton;
            btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");
        }
    }

    /// <summary>
    /// This event is called once while loading listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void lstvwTransportDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwTransportDetails.Items.Count > 0)
            {
                ControlUtility.FillListViewPagerFooter(lstvwTransportDetails, DtPgCount);
            }
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {

            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    
    protected void lstvwTransport_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            SetSortVariable();
        }
        catch (Exception ex)
        {
            
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
          FillExistingNameListview();
    }
    
    protected void Name_Validate(object sender, ServerValidateEventArgs e)
    {
       bool bIsValid = oServiceReceiverDetailsBL.Validate(1, txtName.Text.Trim(), hidId.Value.ToInt());
       e.IsValid = !bIsValid;
    }

    protected void GSTIN_Validate(object sender, ServerValidateEventArgs e)
    {
        txtGSTIN.Text = txtGSTIN.Text.Trim().Trim();

        if (txtGSTIN.Text != string.Empty)
        {
            bool bIsValid = oServiceReceiverDetailsBL.Validate(2, txtGSTIN.Text.Trim(), hidId.Value.ToInt());
            e.IsValid = !bIsValid;
        }
        else
            e.IsValid = true;
    }

    #endregion "Events"


    #region "Private Methods"

    /// <summary>
    /// This method is used to delete default control fields.
    /// </summary>

    private void Delete(int iId)
    {
        oServiceReceiverDetailsBL.Delete(iId);
        lblUpdateSucess.Text = S_DELETE_MSG;
    }

    /// <summary>
    /// This method is used to fill listview.
    /// </summary>

    private void FillExistingNameListview()
    {
        lstvwTransportDetails.DataSourceID = objdsServiceReceiverDetails.ID;
        lstvwTransportDetails.DataBind();
    }

    private void SetJavaScriptAttributes()
    {
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        btnSave.Attributes.Add("onclick", "ResetMessage()");
        base.SetDefaultButton(btnSearch);
        txtNameSearch.Focus();
    }

    /// <summary>
    /// This method is used to clear default control fields.
    /// </summary>

    private void ClearFields()
    {
        txtName.Text = string.Empty;
        txtaddress.Text = string.Empty;
        txtcity.Text = string.Empty;
        txtpincode.Text = string.Empty;
        txtmobileno.Text = string.Empty;
        txtGSTIN.Text = string.Empty;
        hidId.Value = Constants.S_ZERO;
        //btnSave.Text = Resources.LocalizedResources.Save;
        btnSave.Text = S_TEXT_SAVE;        
    }

    private void SetSortVariable()
    {
        if (hidSortDirection.Value == string.Empty || hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }


    private void SetDefaultValues()
    {
        btnClose.Attributes.Add("onclick", "hidepopup(); return false;");
    }

    #endregion "Private Methods"
    
}
