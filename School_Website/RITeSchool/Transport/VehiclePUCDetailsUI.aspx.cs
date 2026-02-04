/* File Name :- TransportPUCDetailsUI.aspx.cs
 * Created Date :- 09-Apr-2020
 * Class Description :- This class is used to configure Vehicle PUC details. 
 * Created By :- Dnyaneshwar Shinde
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using BusinessLogic.TransportBL;
using System.Data;
using Utility;
using SchoolEntities.Transport;
using System.Reflection;
using System.IO;
using BusinessLogic;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;

public partial class VehiclePUCDetailsUI : SchoolBase
{
    #region Constant's

    private const string S_DELETE_MESSAGE = "Vehicle PUC details deleted successfully !!!";
    private const string S_SAVE_MESSAGE = "Vehicle PUC details saved successfully !!!";
    private const string S_UPDATE_MESSAGE = "Vehicle PUC details updated successfully !!!";
    private const string S_FOLDER_PATH = @"\downloads\TransportModule\VehiclePUCDetails\";
    private const string S_SORT_ROW = "SortRow";
    private const string S_UPDATE_TEXT = "Update";
    private const string S_SAVE_TEXT = "Save";

    #endregion

    #region DataMember

    private VehiclePUCDetailsBL moVehiclePUCDetailsBL;

    #endregion

    #region Events

    /// <summary>
    /// Thos event is used to add sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>    
    protected void page_PreRenderComplete(object sender, EventArgs e)
    {
        try
        {
            if (hidSortExpression.Value == string.Empty || hidSortDirection.Value == string.Empty)
            {
                hidSortExpression.Value = "ExpiryDate";
                hidSortDirection.Value = Constants.S_ASCENDING;
            }

            base.AddSortImage(lstvwVehiclePUCDetails, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to load the controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moVehiclePUCDetailsBL = new VehiclePUCDetailsBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                GetVehicleOptionExpiryDate();
                FillVehicalCombo();
                SetJavascriptAttributes();
                FillVehiclePUCDetails();
            }
            base.SetDefaultButton(btnSearch);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to Save the Vehicle details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {   
            TransportPUCDetails oTransportPUCDetails = Populate();
            oTransportPUCDetails.DocumnetPhoto = GetFileName();
            moVehiclePUCDetailsBL.Save(oTransportPUCDetails);
            FillVehiclePUCDetails();
            ClearFields();
            GetVehicleOptionExpiryDate();

            if (oTransportPUCDetails.VehiclePUCId == Constants.I_ZERO)
                base.DisplayMessage(S_SAVE_MESSAGE, false, tdMessage);
            else
                base.DisplayMessage(S_UPDATE_MESSAGE, false, tdMessage);
        }
        catch (SqlException ex)
        {
            base.DisplayMessage(ex.Message, true, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }        
    }

    /// <summary>
    /// This event is used to Clear all the fields.
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
    /// This event is used to display listview record according to value in page combo box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwVehiclePUCDetails);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to List view Item Data Bound.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwVehiclePUCDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                Label lblTestDate = e.Item.FindControl("lblTestDate") as Label;
                Label lblExpiryDate = e.Item.FindControl("lblExpiryDate") as Label;
                ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
                ImageButton btnView = e.Item.FindControl("btnView") as ImageButton;
                ImageButton btnEdit = e.Item.FindControl("btnEdit") as ImageButton;

                TransportPUCDetails oTransportPUCDetails = e.Item.DataItem as TransportPUCDetails;
                if (oTransportPUCDetails.TestDate != null && oTransportPUCDetails.TestDate.ToString(Constants.S_DATE_FORMAT) != Constants.S_DEFAULT_DATE_6)
                    lblTestDate.Text = oTransportPUCDetails.TestDate.ToString(Constants.S_DATE_FORMAT);

                if (oTransportPUCDetails.ExpiryDate != null && oTransportPUCDetails.ExpiryDate.ToString(Constants.S_DATE_FORMAT) != Constants.S_DEFAULT_DATE_6)
                    lblExpiryDate.Text = oTransportPUCDetails.ExpiryDate.ToString(Constants.S_DATE_FORMAT);

                if (oTransportPUCDetails.IsFileExists)
                {
                    string sQueryString = "TypeId=" + Constants.TransportOptions.PUC.ToInt() + "&DetailsId=" + oTransportPUCDetails.VehiclePUCId + "&VehicleId=" + oTransportPUCDetails.VehicalId;
                    btnView.Attributes.Add("onclick", "OpenPhotoPopup('" + CommonUtility.EncryptQuerystring(sQueryString) + "'); return false;");
                }
                else
                    btnView.Visible = false;

                if (!oTransportPUCDetails.IsOldRecord && DateTime.Now.Date.AddDays(oTransportPUCDetails.NoticicationDays) >= oTransportPUCDetails.ExpiryDate)
                {
                    HtmlTableRow tr = e.Item.FindControl("Tr2") as HtmlTableRow;
                    if (tr != null)
                    {
                        tr.Style.Add("color", "Maroon");
                        tr.Style.Add("font-weight", "Bold");
                    }
                }
                else if (oTransportPUCDetails.IsOldRecord)
                {
                    HtmlTableRow tr = e.Item.FindControl("Tr2") as HtmlTableRow;
                    if (tr != null)
                    {
                        tr.Style.Add("color", "Navy");
                        tr.Style.Add("font-weight", "Bold");
                    }
                }

                if (oTransportPUCDetails.IsLocked)
                {
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                }   
                else
                    btnEdit.Visible = true;

                btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This enent is used to Data Bound.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwVehiclePUCDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwVehiclePUCDetails.Items.Count > Constants.I_ZERO)
                ControlUtility.FillListViewPagerFooter(lstvwVehiclePUCDetails, DtPgCount);
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }    

    /// <summary>
    /// This event is used to sorting.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwVehiclePUCDetails_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            if (hidSortExpression.Value != e.SortExpression)
                hidSortDirection.Value = Constants.S_DESCENDING;
            base.RevertSortOrder(hidSortDirection);
            hidSortExpression.Value = e.SortExpression;
            FillVehiclePUCDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }    

    /// <summary>
    /// This event is used to list view command.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwVehiclePUCDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int iVehiclePUCId = Convert.ToInt32(lstvwVehiclePUCDetails.DataKeys[e.Item.DisplayIndex]["VehiclePUCId"]);
                bool bIsFileExists = Convert.ToBoolean(lstvwVehiclePUCDetails.DataKeys[e.Item.DisplayIndex]["IsFileExists"]);
                int iVehicleId = Convert.ToInt32(lstvwVehiclePUCDetails.DataKeys[e.Item.DisplayIndex]["VehicalId"]);
                if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    btnSave.Text = S_UPDATE_TEXT;
                    TransportPUCDetails oTransportPUCDetails = moVehiclePUCDetailsBL.Get(iVehiclePUCId);

                    hidPUCId.Value = oTransportPUCDetails.VehiclePUCId.ToString();
                    cmbVehical.SelectedValue = oTransportPUCDetails.VehicalId.ToString();
                    txtSerialNo.Text = oTransportPUCDetails.SerialNumber;
                    txtTestDate.Text = oTransportPUCDetails.TestDate.ToDateTime().ToString(Constants.S_DATE_FORMAT);
                    txtExpiryDate.Text = oTransportPUCDetails.ExpiryDate.ToDateTime().ToString(Constants.S_DATE_FORMAT);
                    txtNotificationDays.Text = oTransportPUCDetails.NoticicationDays.ToString();
                    txtNote.Text = oTransportPUCDetails.PUCNote;

                    if (bIsFileExists)
                    {
                        btnView.Visible = true;
                        string sQueryString = "TypeId=3&DetailsId=" + iVehiclePUCId + "&VehicleId=" + iVehicleId;
                        btnView.Attributes.Add("onclick", "OpenPhotoPopup('" + CommonUtility.EncryptQuerystring(sQueryString) + "'); return false;");
                    }
                    else
                        btnView.Visible = false;
                }
                else if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {                    
                    List<string> lstFileNames = moVehiclePUCDetailsBL.Delete(iVehiclePUCId);
                    if (lstFileNames.Count > 0)
                    {
                        lstFileNames.ForEach(fl =>
                        {
                            if (File.Exists(Server.MapPath("..") + S_FOLDER_PATH + fl))
                                File.Delete(Server.MapPath("..") + S_FOLDER_PATH + fl);
                        });
                    }

                    FillVehiclePUCDetails();
                    base.DisplayMessage(S_DELETE_MESSAGE, false, tdMessage);

                    if (hidPUCId.Value.ToInt() == iVehiclePUCId)
                        ClearFields();
                }
            }
            else if (e.Item.ItemType == ListViewItemType.EmptyItem && e.CommandSource is LinkButton && e.CommandName == S_SORT_ROW)
            {
                base.RevertSortOrder(hidSortDirection);
                hidSortExpression.Value = e.CommandArgument.ToString();
                FillVehiclePUCDetails();                
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to search button click.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillVehiclePUCDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void chkShowOldRecord_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FillVehiclePUCDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method's

    /// <summary>
    /// This method is used to fill Vehicle combobox.
    /// </summary>
    private void FillVehicalCombo()
    {
        DataTable dtVehicle = moVehiclePUCDetailsBL.GetVehicalDetailsForComboBox();
        cmbVehical.Bind(dtVehicle, "VehicleId", "VehicleNumber", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to set Java script attributes to controls.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel});
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        hidNotificationDays.Value = Settings.VehiclePUCPeriod.ToString();
        txtTestDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        txtExpiryDate.Text = DateTime.Now.Date.AddMonths(Settings.VehiclePUCPeriod).ToString(Constants.S_DATE_FORMAT);
        chkShowOldRecord.Checked = false;

        if (moUserRole == Constants.UserRoles.Admin)
        {
            btnBack.Visible = true;
            btnBack.PostBackUrl = Constants.S_PAGE_CONTROL_PANEL;
        }

        txtSearch.Focus();
    }

    /// <summary>
    /// This mehos is used to populate the control values for save.
    /// </summary>
    /// <returns></returns>
    private TransportPUCDetails Populate()
    {
        TransportPUCDetails oTransportPUCDetails = new TransportPUCDetails();

        if (hidPUCId.Value != Constants.S_ZERO)
            oTransportPUCDetails.VehiclePUCId = hidPUCId.Value.ToInt();
        else
            oTransportPUCDetails.VehiclePUCId = Constants.I_ZERO;

        oTransportPUCDetails.VehicalId = cmbVehical.SelectedValue.ToInt();
        oTransportPUCDetails.SerialNumber = txtSerialNo.Text.TrimAll();
        oTransportPUCDetails.TestDate = txtTestDate.Text.ToDateTime();
        oTransportPUCDetails.ExpiryDate = txtExpiryDate.Text.ToDateTime();
        oTransportPUCDetails.NoticicationDays = txtNotificationDays.Text.ToInt();        
        oTransportPUCDetails.PUCNote = txtNote.Text.TrimAll();

        return oTransportPUCDetails;
    }

    /// <summary>
    /// This method is ued to clear all the controls.
    /// </summary>
    private void ClearFields()
    {
        cmbVehical.ClearSelection();
        txtSerialNo.Text = string.Empty;
        txtNotificationDays.Text = string.Empty;
        txtNote.Text = string.Empty;
        btnSave.Text = S_SAVE_TEXT;
        btnView.Visible = false;
        hidPUCId.Value = Constants.S_ZERO;
        txtTestDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        txtExpiryDate.Text = DateTime.Now.Date.AddMonths(Settings.VehiclePUCPeriod).ToString(Constants.S_DATE_FORMAT); 
    }

    /// <summary>
    /// This method is used to fill Vehicle details Liast view.
    /// </summary>
    private void FillVehiclePUCDetails()
    {
        lstvwVehiclePUCDetails.DataSourceID = lstvwDSobj.ID;
        lstvwVehiclePUCDetails.DataBind();
    }

    private string GetFileName()
    {
        string sFileName = string.Empty;
        HttpFileCollection oCollection = Request.Files;
        List<string> lstFileNames = new List<string>();
        for (int iCount = 0; iCount < oCollection.Count; iCount++)
        {
            HttpPostedFile aoAttachment = oCollection[iCount];

            sFileName = aoAttachment.FileName;

            if (!aoAttachment.FileName.Trim().Equals(string.Empty))
            {
                if (File.Exists(Server.MapPath("..") + S_FOLDER_PATH + sFileName))
                    sFileName = CommonUtility.GetFileNameForRenaming(sFileName);

                aoAttachment.SaveAs(Server.MapPath("..") + S_FOLDER_PATH + sFileName);
                lstFileNames.Add(sFileName);
            }
        }

        if (lstFileNames.Count > 0)
            sFileName = base.GenerateXml(lstFileNames);

        return sFileName;
    }

    /// <summary>
    /// This method is used to get Vehicle Servicing expiry Date details.
    /// </summary>
    private void GetVehicleOptionExpiryDate()
    {
        VehicleDetailsBL oVehicleDetailsBL = new VehicleDetailsBL(miSchoolId, miAcademicYearId, miUserId);
        List<VehicleOptionDate> lstVehicleOptionDate = oVehicleDetailsBL.GetVehicleOptionExpiryDate(Constants.TransportOptions.PUC);        

        var jsSerializer = new JavaScriptSerializer();
        hidDates.Value = jsSerializer.Serialize(lstVehicleOptionDate);
    }

    #endregion    
}