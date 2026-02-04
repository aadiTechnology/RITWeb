/* File Name - VehiclePassingDetailsUI.aspx.cs
 * Created By - Sachin
 * Created Date - 11-Apr-2020
 * Description - This class is used to handle vehicle passing details.
 * 
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities.Transport;
using Utility;
using System.Web.UI.HtmlControls;

public partial class VehiclePassingDetailsUI : SchoolBase
{
    #region Data Member(s)

    VehicleDetailsBL moVehicleDetailsBL;

    #endregion

    #region Constant(s)

    private const string S_FOLDER_PATH = @"\downloads\TransportModule\VehiclePassingDetails\";

    #endregion

    #region Event(s)
    /// <summary>
    /// This event is used to add sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        try
        {
            if (hidSortExpression.Value == string.Empty)
            {
                hidSortExpression.Value = "ExpiryDate";
                hidSortDirection.Value = Constants.S_ASCENDING;
            }

            AddSortImage(lstvwPassingDetails, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill vehicles and passing details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moVehicleDetailsBL = new VehicleDetailsBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                SetDefaultValues();
                GetVehicleOptionExpiryDate();
                FillVehicleList();
                FillVehiclePassingDetails();
            }

            base.SetDefaultButton(btnShow);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event used set paging for list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwPassingDetails);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save vehicle passing details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            VehiclePassingDetails oVehiclePassingDetails = Populate();
            oVehiclePassingDetails.FilePath = GetFileName();

            moVehicleDetailsBL.SaveVehiclePassingDetails(oVehiclePassingDetails);
            base.DisplayMessage("Vehicle passing details saved successfully!!!", false, tdMessage);
            ClearFields();
            FillVehiclePassingDetails();
            GetVehicleOptionExpiryDate();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set couple of attributes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwPassingDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                Label lblPassingDate = e.Item.FindControl("lblPassingDate") as Label;
                Label lblExpiryDate = e.Item.FindControl("lblExpiryDate") as Label;
                ImageButton imgImage = e.Item.FindControl("imgImage") as ImageButton;
                ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
                HiddenField hidQueryString = e.Item.FindControl("hidQueryString") as HiddenField;
                ImageButton btnEdit = e.Item.FindControl("btnEdit") as ImageButton;

                VehiclePassingDetails oVehiclePassingDetails = e.Item.DataItem as VehiclePassingDetails;
                lblPassingDate.Text = oVehiclePassingDetails.PassingDate.ToString(Constants.S_DATE_FORMAT);
                lblExpiryDate.Text = oVehiclePassingDetails.ExpiryDate.ToString(Constants.S_DATE_FORMAT);

                if (oVehiclePassingDetails.IsLocked)
                {
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                }

                if (oVehiclePassingDetails.IsAttachmentPresent)
                {
                    imgImage.Visible = true;
                    //hidQueryString.Value = CommonUtility.EncryptQuerystring("Type=" + Constants.TransportOptions.Servicing.ToInt() + "&Id=" + oVehiclePassingDetails.Id);
                    hidQueryString.Value = CommonUtility.EncryptQuerystring("TypeId=" + Constants.TransportOptions.Passing.ToInt() + "&DetailsId=" + oVehiclePassingDetails.Id + "&VehicleId=" + oVehiclePassingDetails.VehicleId);
                    imgImage.Attributes.Add("onclick", "OpenImagePopup(" + e.Item.DisplayIndex + "); return false;");
                }
                else
                {
                    imgImage.Visible = false;
                    hidQueryString.Value = string.Empty;
                }

                if (!oVehiclePassingDetails.IsOldRecord && DateTime.Now.Date.AddDays(oVehiclePassingDetails.NotificationDays) >= oVehiclePassingDetails.ExpiryDate)
                {
                    HtmlTableRow tr = e.Item.FindControl("Tr2") as HtmlTableRow;
                    if (tr != null)
                    {
                        tr.Style.Add("color", "Maroon");
                        tr.Style.Add("font-weight", "Bold");
                    }
                }
                else if (oVehiclePassingDetails.IsOldRecord)
                {
                    HtmlTableRow tr = e.Item.FindControl("Tr2") as HtmlTableRow;
                    if (tr != null)
                    {
                        tr.Style.Add("color", "Navy");
                        tr.Style.Add("font-weight", "Bold");
                    }
                }

                btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set paging details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwPassingDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwPassingDetails.Items.Count > Constants.I_ZERO)
                ControlUtility.FillListViewPagerFooter(lstvwPassingDetails, DtPgCount);
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle actions.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwPassingDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int iId = lstvwPassingDetails.DataKeys[e.Item.DisplayIndex]["Id"].ToInt();
                if (e.CommandName.ToUpper() == "UPDATECOMMAND")
                {
                    VehiclePassingDetails oVehiclePassingDetails = moVehicleDetailsBL.GetVehiclePassingDetails(iId);
                    if (oVehiclePassingDetails != null)
                    {
                        cmbVehicleNos.SelectedValue = oVehiclePassingDetails.VehicleId.ToString();
                        txtDate.Text = oVehiclePassingDetails.PassingDate.ToString(Constants.S_DATE_FORMAT);
                        txtExpiryDate.Text = oVehiclePassingDetails.ExpiryDate.ToString(Constants.S_DATE_FORMAT);
                        txtNotificationDays.Text = oVehiclePassingDetails.NotificationDays.ToString();
                        txtNote.Text = oVehiclePassingDetails.Note;
                        hidPassingDetailsId.Value = iId.ToString();

                        hidQueryString.Value = string.Empty;
                        if (oVehiclePassingDetails.IsAttachmentPresent)
                        {
                            imgView.Visible = true;
                            //hidQueryString.Value = CommonUtility.EncryptQuerystring("Type=" + Constants.TransportOptions.Servicing.ToInt() + "&Id=" + oVehiclePassingDetails.Id);
                            hidQueryString.Value = CommonUtility.EncryptQuerystring("TypeId=" + Constants.TransportOptions.Passing.ToInt() + "&DetailsId=" + oVehiclePassingDetails.Id + "&VehicleId=" + oVehiclePassingDetails.VehicleId);

                            imgView.Attributes.Add("onclick", "OpenPopup(); return false;");
                        }
                        else
                            imgView.Visible = false;

                    }
                }
                else if (e.CommandName.ToUpper() == "REMOVECOMMAND")
                {
                    List<string> lstFileNames = moVehicleDetailsBL.DeleteVehiclePassingDetails(iId);

                    if (lstFileNames.Count > 0)
                    {
                        lstFileNames.ForEach(fl =>
                            {
                                if (File.Exists(Server.MapPath("..") + S_FOLDER_PATH + fl))
                                    File.Delete(Server.MapPath("..") + S_FOLDER_PATH + fl);
                            });
                    }
                    base.DisplayMessage("Vehicle passing details deleted successfully!!!", false, tdMessage);

                    if (hidPassingDetailsId.Value == iId.ToString())
                        ClearFields();

                    FillVehiclePassingDetails();
                }
            }
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
            FillVehiclePassingDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to cancel actions.
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
    /// This event is used to handle sorting event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwPassingDetails_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            if (hidSortExpression.Value != e.SortExpression)
                hidSortDirection.Value = Constants.S_DESCENDING;
            base.RevertSortOrder(hidSortDirection);
            hidSortExpression.Value = e.SortExpression;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill vehicle passing details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            FillVehiclePassingDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    
    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to read vehiclewise dates.
    /// </summary>
    private void GetVehicleOptionExpiryDate()
    {
        List<VehicleOptionDate> lstVehicleOptionDate = moVehicleDetailsBL.GetVehicleOptionExpiryDate(Constants.TransportOptions.Passing);
        var jsSerializer = new JavaScriptSerializer();
        hidDates.Value = jsSerializer.Serialize(lstVehicleOptionDate);
    }

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        cmbVehicleNos.Items.Add(new ListItem { Text = "Select", Value = "0" });
        txtDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        txtExpiryDate.Text = DateTime.Now.Date.AddMonths(Settings.VehiclePassingPeriod).ToString(Constants.S_DATE_FORMAT);
        hidPassingPeriod.Value = Settings.VehiclePassingPeriod.ToString();
        txtSearch.Focus();

        if (moUserRole == Constants.UserRoles.Admin)
        {
            btnBack.Visible = true;
            btnBack.PostBackUrl = Constants.S_PAGE_CONTROL_PANEL;
        }
    }

    /// <summary>
    /// This method is used to fill passing details.
    /// </summary>
    private void FillVehiclePassingDetails()
    {
        lstvwPassingDetails.DataSourceID = objdsPassingDetails.ID;
        lstvwPassingDetails.DataBind();
    }

    /// <summary>
    /// This method is used to fill vehicle dropdownlist.
    /// </summary>
    private void FillVehicleList()
    {
        List<VehicleDetails> lstVehicleDetails = moVehicleDetailsBL.GetAllVehicles();
        ListSource.FillDropDownList(lstVehicleDetails, cmbVehicleNos, "VehicleNumber", "VehicleId", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to return file nname.
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
    /// This method is used to clear fields.
    /// </summary>
    private void ClearFields()
    {
        hidPassingDetailsId.Value = Constants.S_ZERO;
        txtNote.Text = string.Empty;
        txtNotificationDays.Text = string.Empty;
        cmbVehicleNos.ClearSelection();
        txtDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        txtExpiryDate.Text = DateTime.Now.Date.AddMonths(Settings.VehiclePassingPeriod).ToString(Constants.S_DATE_FORMAT);
        imgView.Visible = false;
    }

    /// <summary>
    /// This method is used to populate object.
    /// </summary>
    /// <returns></returns>
    private VehiclePassingDetails Populate()
    {
        return new VehiclePassingDetails
        {
            ExpiryDate = txtExpiryDate.Text.ToDateTime(),
            Id = hidPassingDetailsId.Value.ToInt(),
            Note = txtNote.Text.Trim(),
            NotificationDays = txtNotificationDays.Text.ToInt(),
            PassingDate = txtDate.Text.ToDateTime(),
            VehicleId = cmbVehicleNos.SelectedValue.ToInt()
        };
    } 

    #endregion    
}