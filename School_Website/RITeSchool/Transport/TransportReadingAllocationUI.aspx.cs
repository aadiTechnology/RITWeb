using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SchoolEntities.Transport;
using System.Data.SqlClient;
using BusinessLogic.TransportBL;
using BusinessLogic;
using Utility;
using BusinessLogic.Exceptions;
using System.Reflection;
using System.Data;
using System.Web.UI.HtmlControls;
public partial class RITeSchool_Transport_TransportReadingAllocationUI : SchoolBase
{
    #region "Constants"

    private const string S_COMMAND_UPDATE_VEHICLE_DETAILS = "UpdateCommand";
    private const string S_COMMAND_DELETE_VEHICLE_DETAILS = "DeleteCommand";
    private const string S_TEXT_UPDATE = "Update";
    private const string S_SAVE_STATEMENT = "Vehicle Reading Allocation Details Saved Successfully !!!";
    private const string S_UPDATE_STATEMENT = "Vehicle Reading Allocation Details Updated successfully !!!";
    private const string S_DELETE_MSG = "Vehicle Reading Allocation Details Deleted successfully !!!";
    private const string S_TEXT_SAVE = "Save";
    private const string S_VIEW = "View";
    private const string S_DEFAULT_SORT_EXP = "ReadingDate";

    #endregion "Constants"


    #region "Events"

    /// <summary>
    /// This event is used to set default control fields and java script attributes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SetJavascriptAttributes();
            SetDefaultValues();
            GetAllVehicleNumber();
            FillVehicleAllocationDetails();
        }
    }

    /// <summary>
    /// This event is used to get Last ReadingTo entry of the selected Vehicle as ReadingFrom value.
    /// </summary> 
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlVehicleNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        VehicleReadingAllocationBL oVehicleReadingAllocationBL = new VehicleReadingAllocationBL();
        DataSet dsReadingAllocationDetails = oVehicleReadingAllocationBL.GetAllVehicleReadingAllocationDetails(miSchoolId, miAcademicYearId, "", "", "", Constants.S_ZERO);

        Decimal lastReadingTo = 0;

        if (dsReadingAllocationDetails.Tables[0].AsEnumerable().Any(ra => ra.Field<Int32>("VehicleId") == ddlVehicleNumber.SelectedValue.ToInt()))
            lastReadingTo = dsReadingAllocationDetails.Tables[0].AsEnumerable().Where(ra => ra.Field<Int32>("VehicleId") == ddlVehicleNumber.SelectedValue.ToInt()).Select(ra => ra.Field<Decimal>("ReadingTo")).Max();

        txtReadingFrom.Text = lastReadingTo != 0 ? lastReadingTo.ToString() : "";
    }

    /// <summary>
    /// This method is used to save transport reading allocation details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        int iVehicleReadingAllocationId = 0;

        if (hidVehicleReadingAllocationId.Value != string.Empty)
            iVehicleReadingAllocationId = Convert.ToInt32(hidVehicleReadingAllocationId.Value);

        TransportReadingAllocationDetails oTransportReadingAllocationDetails = Populate(iVehicleReadingAllocationId);
        if (oTransportReadingAllocationDetails != null)
        {
            string sXml = CommonUtility.GenerateXml(oTransportReadingAllocationDetails);

            VehicleReadingAllocationBL.SaveTransportReadingAllocationDetails(sXml);
            GetAllVehicleNumber();
            FillVehicleAllocationDetails();
            if (iVehicleReadingAllocationId == Constants.I_ZERO)
                lblUpdateSucess.Text = S_SAVE_STATEMENT;
            else
            {
                lblUpdateSucess.Text = S_UPDATE_STATEMENT;
                btnSave.Text = S_TEXT_SAVE;
            }

            ClearFields();
        }

        else
        {
            AddSortImage();
            lblErrorMsg.Visible = true;
        }
    }

    /// <summary>
    /// This event is called while cancelling Save/Update operation.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            GetAllVehicleNumber();
            FillVehicleAllocationDetails();
            ClearFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///  this event is called while row in list view is clicked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwVehicleReadingAllocationDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
            int iRowId = oCurrentItem.DisplayIndex;
            int iVehicleReadingAllocationId = Convert.ToInt32(lstvwVehicleReadingAllocationDetails.DataKeys[iRowId]["VehicleReadingAllocationId"]);

            if (e.CommandName == S_COMMAND_UPDATE_VEHICLE_DETAILS)
            {
                SetEditModeForVehicleDetails(oCurrentItem, iVehicleReadingAllocationId);
            }
            if (e.CommandName == S_COMMAND_DELETE_VEHICLE_DETAILS)
            {
                DeleteVehicleAllocationDetailsDetails(iVehicleReadingAllocationId, oCurrentItem);
                GetAllVehicleNumber();
                FillVehicleAllocationDetails();
            }
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
    protected void lstvwVehicleReadingAllocationDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
            if (oCurrentItem != null)
                SetVisibilityOfColumns(oCurrentItem);

            DataRowView oDataRowView = (DataRowView)oCurrentItem.DataItem;
            int iReadingFrom = Convert.ToInt32(oDataRowView["ReadingFrom"]);
            int iReadingTo = Convert.ToInt32(oDataRowView["ReadingTo"]);
            int iLitters = Convert.ToInt32(oDataRowView["Litters"]);

            Label lblAverage = e.Item.FindControl("lblAverage") as Label;
            lblAverage.Text = Convert.ToString(((iReadingTo - iReadingFrom) / iLitters).ToDecimal());
        }
    }

    /// <summary>
    /// This event is used to sort the listview of Vehicle Reading Allocation Details. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwVehicleReadingAllocationDetails_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            if (hidSortDirection.Value == Constants.S_DESCENDING)
                hidSortDirection.Value = Constants.S_ASCENDING;
            else
                hidSortDirection.Value = Constants.S_DESCENDING;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is called once while loading listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwVehicleReadingAllocationDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwVehicleReadingAllocationDetails.Items.Count > 0)
                ControlUtility.FillListViewPagerFooter(lstvwVehicleReadingAllocationDetails, DtPgCount);
            else
                DtPgCount.Visible = false;
            if (Page.IsPostBack)
            {
                AddSortImage();
            }

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to view pagewise vehicle reading allocation details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwVehicleReadingAllocationDetails);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }


    protected internal void txtLitters_TextChanged(object sender, EventArgs e)
    {
        if (txtLitters.Text != "" && txtPerLitterCost.Text != "")
            txtTotalCost.Text = (Convert.ToDecimal(txtPerLitterCost.Text) * Convert.ToDecimal(txtLitters.Text)).ToString();
    }


    protected void txtPerLitterCost_TextChanged(object sender, EventArgs e)
    {
        if (txtLitters.Text != "" && txtPerLitterCost.Text != "")
            txtTotalCost.Text = (Convert.ToDecimal(txtLitters.Text) * Convert.ToDecimal(txtPerLitterCost.Text)).ToString();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (ChkIncludeAllDates.Checked)
            {
                hidIncludeAllDates.Value = Constants.S_ONE;
            }
            else
            {
                hidIncludeAllDates.Value = Constants.S_ZERO;
            }
            FillVehicleAllocationDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to enable / disable fields.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ChkIncludeAllDates_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (ChkIncludeAllDates.Checked)
            {
                txtSearchReadingDate.Enabled = false;
                cal_ReadingDate.Enabled = false;
            }
            else
            {
                txtSearchReadingDate.Enabled = true;
                cal_ReadingDate.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion "Events"

    #region "Private Methods"

    /// <summary>
    /// this method is use to populate vehicle reading allocation details.
    /// </summary>
    /// <param name="iVehicleReadingAllocationId"></param>
    /// <returns></returns>

    private TransportReadingAllocationDetails Populate(int iVehicleReadingAllocationId)
    {
        TransportReadingAllocationDetails oTransportReadingAllocationDetails = new TransportReadingAllocationDetails
        {
            VehicleId = ddlVehicleNumber.SelectedValue.ToInt(),
            VehicleReadingAllocationId = iVehicleReadingAllocationId,
            ReadingFrom = Convert.ToDouble(txtReadingFrom.Text),
            ReadingTo = Convert.ToDouble(txtReadingTo.Text),
            ReceiptNumber = txtReceiptNumber.Text,
            ReadingDate = Convert.ToDateTime(txtReadingDate.Text),
            Litters = txtLitters.Text != string.Empty ? Convert.ToDecimal(txtLitters.Text) : 0,
            PerLitterCost = txtPerLitterCost.Text != string.Empty ? Convert.ToDecimal(txtPerLitterCost.Text) : 0,
            TotalCost = Convert.ToDecimal(txtLitters.Text) * Convert.ToDecimal(txtPerLitterCost.Text),
            FuelStationName = txtFuelStationName.Text,
            SchoolId = miSchoolId,
            AcademicYearId = miAcademicYearId,
            InsertedById = miUserId,

        };

        return oTransportReadingAllocationDetails;
    }

    /// <summary>
    /// This method is used to set default control fields.
    /// </summary>

    private void ClearFields()
    {
        hidVehicleReadingAllocationId.Value = string.Empty;
        txtReadingFrom.Text = string.Empty;
        txtReceiptNumber.Text = string.Empty;
        txtReceiptNumber.Text = string.Empty;
        txtReadingTo.Text = string.Empty;
        txtPerLitterCost.Text = string.Empty;
        txtLitters.Text = string.Empty;
        txtReadingDate.Text = DateTime.Today.ToString(Utility.Constants.S_DATE_FORMAT);
        txtTotalCost.Text = string.Empty;
        ddlVehicleNumber.SelectedValue = Constants.S_ZERO;
        txtFuelStationName.Text = string.Empty;
        btnSave.Text = Constants.ButtonText.Save.ToString();
    }

    /// <summary>
    /// this method is used to delete perticular records from listview.
    /// </summary>
    /// <param name="aiNoticeId"></param>

    private void DeleteVehicleAllocationDetailsDetails(int iVehicleReadingAllocationId, ListViewDataItem oCurrentItem)
    {
        VehicleReadingAllocationBL.DeleteVehicleAllocationDetails(iVehicleReadingAllocationId, miSchoolId, miAcademicYearId, miUserId);
        lblUpdateSucess.Text = S_DELETE_MSG;
    }

    /// <summary>
    /// This method is used to set values to controls in edit mode.
    /// </summary>
    /// <param name="iNoticeId"></param>
    private void SetEditModeForVehicleDetails(ListViewDataItem oCurrentItem, int iVehicleReadingAllocationId)
    {
        btnSave.Text = S_TEXT_UPDATE;

        Label oLblReadingDate = oCurrentItem.FindControl("lblReadingDate") as Label;
        Label oLblReadingFrom = oCurrentItem.FindControl("lblReadingFrom") as Label;
        Label oLblReceiptNumber = oCurrentItem.FindControl("lblReceiptNumber") as Label;
        Label oLblLitters = oCurrentItem.FindControl("lblLitters") as Label;
        Label oLblPerLittersCost = oCurrentItem.FindControl("lblPerLitterCost") as Label;
        Label oLblTotalCost = oCurrentItem.FindControl("lblTotalCost") as Label;
        Label oLblFuelStationName = oCurrentItem.FindControl("lblFuelStationName") as Label;
        Label oLblReadingTo = oCurrentItem.FindControl("lblReadingTo") as Label;

        int iRowID = oCurrentItem.DisplayIndex;
        int iVehicleID = Convert.ToInt32(lstvwVehicleReadingAllocationDetails.DataKeys[iRowID]["VehicleId"]);
        hidVehicleReadingAllocationId.Value = iVehicleReadingAllocationId.ToString();

        txtReadingDate.Text = oLblReadingDate.Text;
        txtReadingFrom.Text = oLblReadingFrom.Text;
        txtReadingTo.Text = oLblReadingTo.Text;
        txtReceiptNumber.Text = oLblReceiptNumber.Text;
        txtLitters.Text = oLblLitters.Text;
        txtPerLitterCost.Text = oLblPerLittersCost.Text;
        txtTotalCost.Text = oLblTotalCost.Text;
        ddlVehicleNumber.SelectedValue = iVehicleID.ToString();
        txtFuelStationName.Text = oLblFuelStationName.Text;
    }

    /// <summary>
    /// this method is uesd to load all vehicle numbers and set default to vehicle number dropdownlist.
    /// </summary>

    private void GetAllVehicleNumber()
    {
        List<TransportReadingAllocationDetails> lstTransportReadingAllocationDetails = new List<TransportReadingAllocationDetails>();
        VehicleReadingAllocationBL moVehicleReadingAllocationBL = new VehicleReadingAllocationBL();
        lstTransportReadingAllocationDetails = moVehicleReadingAllocationBL.GetAllVehicleNumbers(miAcademicYearId);

        if (lstTransportReadingAllocationDetails.Count > 0)
        {
            ListSource.FillDropDownList(lstTransportReadingAllocationDetails, ddlVehicleNumber, "VehicleNumber", "VehicleId", Constants.S_SELECT);
        }
    }

    /// <summary>
    /// this method is used to set default values to control.
    /// </summary>

    private void SetDefaultValues()
    {
        txtReadingDate.Text = DateTime.Today.ToString(Utility.Constants.S_DATE_FORMAT);
        HtmlTableRow oHtmlTableHeaderRow = lstvwVehicleReadingAllocationDetails.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    /// <summary>
    /// This methos is used to set java script attributes.
    /// </summary>

    private void SetJavascriptAttributes()
    {
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        new Button[] { btnCancel, btnSave }.ApplyEffect();

        if (moUserRole == Constants.UserRoles.Admin)
        {
            btnBack.Visible = true;
            btnBack.PostBackUrl = Constants.S_PAGE_CONTROL_PANEL;
        }
    }

    /// <summary>
    /// this method is used to fill ListView.
    /// </summary>

    private void FillVehicleAllocationDetails()
    {
        lstvwVehicleReadingAllocationDetails.DataSourceID = ObjDSVehicleReadingAllocationDetails.ID;
    }

    /// <summary>
    /// This method is used to hide or show filename column in listview.
    /// </summary>
    /// <param name="oCurrentItem"></param>
    private void SetVisibilityOfColumns(ListViewDataItem oCurrentItem)
    {
        ImageButton imgBtnDelete = oCurrentItem.FindControl("imgBtnDelete") as ImageButton;
        imgBtnDelete.Attributes.Add("Onclick", "if(!ConfirmDelete()) {return false;}");
    }

    private void AddSortImage()
    {
        if (lstvwVehicleReadingAllocationDetails.SortDirection.ToString() == "Ascending" || lstvwVehicleReadingAllocationDetails.SortDirection.ToString() == string.Empty)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
        if (lstvwVehicleReadingAllocationDetails.SortExpression != string.Empty)
            hidSortExpression.Value = lstvwVehicleReadingAllocationDetails.SortExpression.ToString();
        HtmlTableRow oHtmlTableHeaderRow = lstvwVehicleReadingAllocationDetails.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    #endregion "Private Methods"
}