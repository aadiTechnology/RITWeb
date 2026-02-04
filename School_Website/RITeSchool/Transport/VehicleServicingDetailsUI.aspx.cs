/* File Name :- VehicleServicingDetailsUI.aspx.cs
 * Created Date :- 11-Apr-2020
 * Class Description :- This class is used to configure Vehicle Servicing details. 
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
using System.IO;
using System.Reflection;
using BusinessLogic;
using System.Web.Script.Serialization;
using System.Web.UI.HtmlControls;

public partial class VehicleServicingDetailsUI : SchoolBase
{
    #region Constant's

    private const string S_DELETE_MESSAGE = "Vehicle Servicing details deleted successfully !!!";
    private const string S_SAVE_MESSAGE = "Vehicle Servicing details saved successfully !!!";
    private const string S_UPDATE_MESSAGE = "Vehicle Servicing details updated successfully !!!";
    private const string S_FOLDER_PATH = @"\downloads\TransportModule\VehicleServicingDetails\";
    private const string S_SORT_ROW = "SortRow";
    private const string S_UPDATE_TEXT = "Update";
    private const string S_SAVE_TEXT = "Save";

    #endregion

    #region DataMember

    private VehicleServicingDetailsBL moVehicleServicingDetailsBL;

    #endregion

    #region Event(s)

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
                hidSortExpression.Value = "NextServicingDate";
                hidSortDirection.Value = Constants.S_ASCENDING;
            }

            base.AddSortImage(lstvwVehicleServicingDetails, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to load the default controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moVehicleServicingDetailsBL = new VehicleServicingDetailsBL(miSchoolId, miAcademicYearId);
            if (!IsPostBack)
            {
                GetVehicleOptionExpiryDate();
                FillVehicalCombo();
                FillVehicleServicingDetails();
                SetJavascriptAttributes();
            }
            base.SetDefaultButton(btnSearch);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This Event is used to save button click.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {   
            VehicleServicingDetails oVehicleServicingDetails = Populate();
            oVehicleServicingDetails.DocumnetPhoto = GetFileName();
            moVehicleServicingDetailsBL.Save(oVehicleServicingDetails);
            FillVehicleServicingDetails();
            ClearFields();
            GetVehicleOptionExpiryDate();

            if (oVehicleServicingDetails.VehicleServicingId == Constants.I_ZERO)
                base.DisplayMessage(S_SAVE_MESSAGE, false, tdMessage);
            else
                base.DisplayMessage(S_UPDATE_MESSAGE, false, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to cancel button click event.
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
    /// This event is used to Search button click event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillVehicleServicingDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void chkShowOldRecord_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FillVehicleServicingDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to list view Item command.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwVehicleServicingDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
             {
                 int iVehicleServicingId = Convert.ToInt32(lstvwVehicleServicingDetails.DataKeys[e.Item.DisplayIndex]["VehicleServicingId"]);
                bool bIsFileExists = Convert.ToBoolean(lstvwVehicleServicingDetails.DataKeys[e.Item.DisplayIndex]["IsFileExists"]);
                int iVehicalId = Convert.ToInt32(lstvwVehicleServicingDetails.DataKeys[e.Item.DisplayIndex]["VehicalId"]);
                if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    btnSave.Text = S_UPDATE_TEXT;
                    VehicleServicingDetails oVehicleServicingDetails = moVehicleServicingDetailsBL.Get(iVehicleServicingId);

                    hidServicingId.Value = oVehicleServicingDetails.VehicleServicingId.ToString();
                    cmbVehical.SelectedValue = oVehicleServicingDetails.VehicalId.ToString();                    
                    txtServicingDate.Text = oVehicleServicingDetails.ServicingDate.ToDateTime().ToString(Constants.S_DATE_FORMAT);
                    txtNextServicingDate.Text = oVehicleServicingDetails.NextServicingDate.ToDateTime().ToString(Constants.S_DATE_FORMAT);
                    txtNotificationDays.Text = oVehicleServicingDetails.NotificationDays.ToString();
                    txtNote.Text = oVehicleServicingDetails.ServicingNote;

                    if (bIsFileExists)
                    {
                        btnView.Visible = true;
                        string sQueryString = "TypeId=1&DetailsId=" + iVehicleServicingId + "&VehicleId=" + iVehicalId;
                        btnView.Attributes.Add("onclick", "OpenPhotoPopup('" + CommonUtility.EncryptQuerystring(sQueryString) + "'); return false;");
                    }
                    else
                        btnView.Visible = false;
                }
                else if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {   
                    List<string> lstFileNames = moVehicleServicingDetailsBL.Delete(iVehicleServicingId);
                    if (lstFileNames.Count > 0)
                    {
                        lstFileNames.ForEach(fl =>
                        {
                            if (File.Exists(Server.MapPath("..") + S_FOLDER_PATH + fl))
                                File.Delete(Server.MapPath("..") + S_FOLDER_PATH + fl);
                        });
                    }

                    FillVehicleServicingDetails();
                    base.DisplayMessage(S_DELETE_MESSAGE, false, tdMessage);

                    if (hidServicingId.Value.ToInt() == iVehicleServicingId)
                        ClearFields();
                }
            }
            else if (e.Item.ItemType == ListViewItemType.EmptyItem && e.CommandSource is LinkButton && e.CommandName == S_SORT_ROW)
            {
                base.RevertSortOrder(hidSortDirection);
                hidSortExpression.Value = e.CommandArgument.ToString();
                FillVehicleServicingDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to bind the data in list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwVehicleServicingDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                Label lblServicingDate = e.Item.FindControl("lblServicingDate") as Label;
                Label lblNextServicingDate = e.Item.FindControl("lblNextServicingDate") as Label;
                
                ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
                ImageButton btnView = e.Item.FindControl("btnView") as ImageButton;
                ImageButton btnEdit = e.Item.FindControl("btnEdit") as ImageButton;
                VehicleServicingDetails oVehicleServicingDetails = e.Item.DataItem as VehicleServicingDetails;

                if (!oVehicleServicingDetails.IsOldRecord && DateTime.Now.Date.AddDays(oVehicleServicingDetails.NotificationDays) >= oVehicleServicingDetails.NextServicingDate)
                {
                    HtmlTableRow tr = e.Item.FindControl("Tr2") as HtmlTableRow;
                    if (tr != null)
                    {
                        tr.Style.Add("color", "Maroon");
                        tr.Style.Add("font-weight", "Bold");
                    }
                }
                else if (oVehicleServicingDetails.IsOldRecord)
                {
                    HtmlTableRow tr = e.Item.FindControl("Tr2") as HtmlTableRow;
                    if (tr != null)
                    {
                        tr.Style.Add("color", "Navy");
                        tr.Style.Add("font-weight", "Bold");
                    }
                }

                if (oVehicleServicingDetails.ServicingDate != null && oVehicleServicingDetails.ServicingDate.ToString(Constants.S_DATE_FORMAT) != Constants.S_DEFAULT_DATE_6)
                    lblServicingDate.Text = oVehicleServicingDetails.ServicingDate.ToString(Constants.S_DATE_FORMAT);

                if (oVehicleServicingDetails.NextServicingDate != null && oVehicleServicingDetails.NextServicingDate.ToString(Constants.S_DATE_FORMAT) != Constants.S_DEFAULT_DATE_6)
                    lblNextServicingDate.Text = oVehicleServicingDetails.NextServicingDate.ToString(Constants.S_DATE_FORMAT);

                if (oVehicleServicingDetails.IsFileExists)
                {
                    string sQueryString = "TypeId=" + Constants.TransportOptions.Servicing.ToInt() + "&DetailsId=" + oVehicleServicingDetails.VehicleServicingId + "&VehicleId=" + oVehicleServicingDetails.VehicalId;
                    btnView.Attributes.Add("onclick", "OpenPhotoPopup('" + CommonUtility.EncryptQuerystring(sQueryString) + "'); return false;");
                }
                else
                    btnView.Visible = false;

                if (oVehicleServicingDetails.IsLocked)
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
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to list view DataBound.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwVehicleServicingDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwVehicleServicingDetails.Items.Count > Constants.I_ZERO)
                ControlUtility.FillListViewPagerFooter(lstvwVehicleServicingDetails, DtPgCount);
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This Event is used to listview sorting.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwVehicleServicingDetails_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            if (hidSortExpression.Value != e.SortExpression)
                hidSortDirection.Value = Constants.S_DESCENDING;
            base.RevertSortOrder(hidSortDirection);
            hidSortExpression.Value = e.SortExpression;
            FillVehicleServicingDetails();
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
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwVehicleServicingDetails);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method(s)

    /// <summary>
    /// This method is used to fill Vehicle combobox.
    /// </summary>
    private void FillVehicalCombo()
    {
        VehiclePUCDetailsBL oVehiclePUCDetailsBL = new VehiclePUCDetailsBL(miSchoolId, miAcademicYearId, miUserId);
        DataTable dtVehicle = oVehiclePUCDetailsBL.GetVehicalDetailsForComboBox();
        cmbVehical.Bind(dtVehicle, "VehicleId", "VehicleNumber", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to set Java script attributes to controls.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel });
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        hidNotificationDays.Value = Settings.VehicleServicingPeriod.ToString();
        txtServicingDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        txtNextServicingDate.Text = DateTime.Now.Date.AddMonths(Settings.VehicleServicingPeriod).ToString(Constants.S_DATE_FORMAT);

        if (moUserRole == Constants.UserRoles.Admin)
        {
            btnBack.Visible = true;
            btnBack.PostBackUrl = Constants.S_PAGE_CONTROL_PANEL;
        }
    }

    /// <summary>
    /// This mehos is used to populate the control values for save.
    /// </summary>
    /// <returns></returns>
    private VehicleServicingDetails Populate()
    {
        VehicleServicingDetails oVehicleServicingDetails = new VehicleServicingDetails();

        if (hidServicingId.Value != Constants.S_ZERO)
            oVehicleServicingDetails.VehicleServicingId = hidServicingId.Value.ToInt();
        else
            oVehicleServicingDetails.VehicleServicingId = Constants.I_ZERO;

        oVehicleServicingDetails.VehicalId = cmbVehical.SelectedValue.ToInt();
        oVehicleServicingDetails.ServicingDate = txtServicingDate.Text.ToDateTime();
        oVehicleServicingDetails.NextServicingDate = txtNextServicingDate.Text.ToDateTime();
        oVehicleServicingDetails.NotificationDays = txtNotificationDays.Text.ToInt();
        oVehicleServicingDetails.ServicingNote = txtNote.Text.TrimAll();


        return oVehicleServicingDetails;
    }

    /// <summary>
    /// This method is used to get the file name.
    /// </summary>
    /// <returns></returns>
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
    /// This method is ued to clear all the controls.
    /// </summary>
    private void ClearFields()
    {
        cmbVehical.ClearSelection();
        txtNotificationDays.Text = string.Empty;
        txtNote.Text = string.Empty;
        btnSave.Text = S_SAVE_TEXT;
        btnView.Visible = false;
        hidServicingId.Value = Constants.S_ZERO;
        txtServicingDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        txtNextServicingDate.Text = DateTime.Now.Date.AddMonths(Settings.VehicleServicingPeriod).ToString(Constants.S_DATE_FORMAT); 
    }

    /// <summary>
    /// This method is used to fill Vehicle details Liast view.
    /// </summary>
    private void FillVehicleServicingDetails()
    {
        lstvwVehicleServicingDetails.DataSourceID = lstvwDSobj.ID;
        lstvwVehicleServicingDetails.DataBind();
    }

    /// <summary>
    /// This method is used to get Vehicle Servicing expiry Date details.
    /// </summary>
    private void GetVehicleOptionExpiryDate()
    {
        VehicleDetailsBL oVehicleDetailsBL = new VehicleDetailsBL(miSchoolId, miAcademicYearId, miUserId);
        List<VehicleOptionDate> lstVehicleOptionDate = oVehicleDetailsBL.GetVehicleOptionExpiryDate(Constants.TransportOptions.Servicing);

        var jsSerializer = new JavaScriptSerializer();
        hidDates.Value = jsSerializer.Serialize(lstVehicleOptionDate);
    }

    #endregion
}