using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class StudentTransportDetailsUI : SchoolBase
{
    # region "CONSTANTS"
    const int I_TBL_VEHICLE_DETAILS_INDEX = 0;
    const int I_TBL_STAFF_DETAILS_INDEX = 1;
    const int I_TBL_TIMING_DETAILS_INDEX = 2;
    const string S_VEHICLE_ROUTE = "VehicleRoute";
    const string S_VEHICLE_NUMBER = "VehicleNumber";
    const string S_VEHICLE_TYPE = "VehicleType";
    const string S_TRANSPORT_SHIFT_NAME = "TransportShiftName";
    const string S_STOP_NAME = "StopName";
    const string S_TRANSPORT_STAFF_NAME = "TransportStaffName";
    const string S_MOBILE_NUMBER = "MobileNo";
    const string S_STAFF_DESIGNATION = "Designation";
    private const string S_FOLDER_PATH = @"../DOWNLOADS/Route Map Picture/";
    
    #endregion "CONSTANTS"
    DataSet moTransportDetails;
    /// <summary>
    /// This event is used to fill Stop details in to ListView.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                SetGPSTrackingUrl();
                SetJavascriptAttributes();
                FillLables();
                FillListView();
                AddLables();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to focus the ListView row.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStudentTransportDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iLastIndex = oCurrentItem.DisplayIndex;
                string sStopName = lstvwStudentTransportDetails.DataKeys[iLastIndex][S_STOP_NAME].ToString();
                var lblPickUpTime = oCurrentItem.FindControl("lblPickUpTime") as Label;
                var lblStopName = oCurrentItem.FindControl("lblStopName") as Label;
                var lblDropTime = oCurrentItem.FindControl("DropTime") as Label;

                if (sStopName == hidStopName.Value)
                {
                    HtmlTableRow oHtmlTableHeaderRow = e.Item.FindControl("trTimingDetails") as HtmlTableRow;
                    if (oHtmlTableHeaderRow != null)
                    {
                        oHtmlTableHeaderRow.Style.Add("background-color", "#FFCCCC");
                        lblStopName.ForeColor = System.Drawing.Color.Maroon;
                        lblStopName.Style.Add("font-weight", "bold");

                        if (rdoTransportTypes.SelectedValue == Constants.S_ONE)
                        {
                            lblPickUpTime.ForeColor = System.Drawing.Color.Maroon;
                            lblPickUpTime.Style.Add("font-weight", "bold");
                        }
                        else
                        {
                            lblDropTime.ForeColor = System.Drawing.Color.Maroon;
                            lblDropTime.Style.Add("font-weight", "bold");
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
    /// This event is used to load the transport details on change of transport type.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rdoTransportTypes_SelectedIndexChanged(object sender, EventArgs e)
    {

        FillLables();
        FillListView();
        AddLables();
    }

    /// <summary>
    /// This method is used to fill ListView.
    /// </summary>
    public void FillListView()
    {
        TravelerTransportDetailsBL oTravelerTransportDetailsBL = new TravelerTransportDetailsBL();
        moTransportDetails = oTravelerTransportDetailsBL.TransportDetails(miUserId, miSchoolId, miAcademicYearId, rdoTransportTypes.SelectedValue.ToInt());
        lstvwStudentTransportDetails.DataSource = moTransportDetails.Tables[I_TBL_TIMING_DETAILS_INDEX].DefaultView;
        if (moTransportDetails.Tables[I_TBL_TIMING_DETAILS_INDEX].Rows.Count > 0)
        {
            lstvwStudentTransportDetails.DataBind();
            ClearFields(true);

            if (moSchool != Constants.SchoolId.SNS)
                divGPSTracking.Visible = true;
            else
                divGPSTracking.Visible = false;
        }
        else
        {
            ClearFields(false);
            divGPSTracking.Visible = false;
        }
    }
   /// <summary>
   /// This method is used to fill Lables.
   /// </summary>
    public void FillLables()
    {
        TravelerTransportDetailsBL oTravelerTransportDetailsBL = new TravelerTransportDetailsBL();
        moTransportDetails = oTravelerTransportDetailsBL.TransportDetails(miUserId, miSchoolId, miAcademicYearId,rdoTransportTypes.SelectedValue.ToInt());
        foreach (DataRow oDataRow in moTransportDetails.Tables[I_TBL_VEHICLE_DETAILS_INDEX].Rows)
        {
            lblVehicleRoute.Text = oDataRow[S_VEHICLE_ROUTE].ToString();
            lblVehicleNumber.Text = oDataRow[S_VEHICLE_NUMBER].ToString();
            lblVehicle.Text = oDataRow[S_VEHICLE_TYPE].ToString();
            lblShift.Text = oDataRow[S_TRANSPORT_SHIFT_NAME].ToString();
            lblVehicleContactNo.Text = oDataRow["VehicleOfficialContactNo"].ToString();

            if (moSchool == Constants.SchoolId.SNS && oDataRow["TrackingURL"] != DBNull.Value && oDataRow["TrackingURL"].ToString() != string.Empty)
            {
                trTracking.Visible = true;
                if (oDataRow["TrackingMessage"].ToString() == string.Empty)
                {  
                    divPickupVehicleURL.Visible = true;
                    divPickupVehicleURL.InnerHtml = "<embed src='" + oDataRow["TrackingURL"].ToString() + "' width='100%' height='500'>";
                    trMapMessage.Visible = false;
                    trMap.Visible = true;
                }
                else
                {
                    lblMapMessage.Text = oDataRow["TrackingMessage"].ToString();
                    trMapMessage.Visible = true;
                    trMap.Visible = false;
                }
            }
            else
            {
                divPickupVehicleURL.Visible = false;
                trTracking.Visible = false;
                trMapMessage.Visible = false;
                trMap.Visible = false;
            }

            string sStopName = oDataRow[S_STOP_NAME].ToString();
            hidStopName.Value = sStopName;
            string sLinkUrl = oDataRow["LinkUrl"].ToString();
            if (sLinkUrl != string.Empty)
            {
                lnkbtnRoute.Visible = true;
                string sUrl = S_FOLDER_PATH + sLinkUrl;
                lnkbtnRoute.Attributes.Add("onclick", "OpenWindow('" + sUrl + "'); return false;");
            }
            else
            {
                lnkbtnRoute.Visible = false;
            }
        }
    }
    
    /// <summary>
    /// This method is used to set JavaScript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        hlnkGPSTrancking.Attributes.Add("onclick", "openGPSTrackingLink('" + hidGPSTrackingUrl.Value + "'); return false;");
    }
  /// <summary>
  /// This method is used  Add the Lables of Staff details dynamically.
  /// </summary>
    public void AddLables()
    {
        TravelerTransportDetailsBL oTravelerTransportDetailsBL = new TravelerTransportDetailsBL();
        moTransportDetails = oTravelerTransportDetailsBL.TransportDetails(miUserId, miSchoolId, miAcademicYearId,rdoTransportTypes.SelectedValue.ToInt());
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        
        tblStudentTransportDetails.Rows.Add(oHtmlTableRow);
        HtmlTableCell oHtmlTableCell = new HtmlTableCell();
        for (int iTransportStaffDetails = 0; iTransportStaffDetails < moTransportDetails.Tables[I_TBL_STAFF_DETAILS_INDEX].Rows.Count; iTransportStaffDetails++)
            {
                if ((iTransportStaffDetails % 1) == 0)
                {
                    oHtmlTableRow = new HtmlTableRow();
                    oHtmlTableCell = new HtmlTableCell();
                    oHtmlTableCell.VAlign = "top";
                    oHtmlTableCell.ColSpan = 4;
                    oHtmlTableRow.Cells.Add(oHtmlTableCell);
                    oHtmlTableRow.Height = "1px";
                    oHtmlTableRow = new HtmlTableRow();
                    tblStudentTransportDetails.Align = "center";
                    tblStudentTransportDetails.Rows.Add(oHtmlTableRow);
                }
                oHtmlTableCell = new HtmlTableCell();
                oHtmlTableCell.Align = "left";
                oHtmlTableCell.Width = "20%";
                oHtmlTableCell.Height = "26px";
                oHtmlTableCell.Attributes.Add("class", "ClsBorderlight paddingL ");
                oHtmlTableCell.Attributes.Add("colspan", "2");
                oHtmlTableRow.Cells.Add(oHtmlTableCell);

                Label oLblStaffName = new Label();
                oLblStaffName.Text = "Staff Name : ";
                oLblStaffName.Style.Add("CssClass", "ClsLblRslt ");
                tblStudentTransportDetails.Align = "center";
                oHtmlTableCell.Controls.Add(oLblStaffName);

                oHtmlTableCell = new HtmlTableCell();
                oHtmlTableCell.Align = "left";
                oHtmlTableCell.Width = "35%";
                oHtmlTableCell.Height = "26px";
                oHtmlTableCell.Attributes.Add("class", "ClsBorderlight paddingL");
                oHtmlTableCell.Attributes.Add("colspan", "2");
                oHtmlTableRow.Cells.Add(oHtmlTableCell);
                Label oLblMobileNumber = new Label();
                oLblMobileNumber.Text = moTransportDetails.Tables[I_TBL_STAFF_DETAILS_INDEX].Rows[iTransportStaffDetails][S_TRANSPORT_STAFF_NAME].ToString() + " (" + moTransportDetails.Tables[I_TBL_STAFF_DETAILS_INDEX].Rows[iTransportStaffDetails][S_STAFF_DESIGNATION].ToString() + ")";
                oLblMobileNumber.Style.Add("CssClass", "ClsLblRslt");
                oHtmlTableCell.Controls.Add(oLblMobileNumber);

                if (moTransportDetails.Tables[I_TBL_VEHICLE_DETAILS_INDEX].Rows[0]["ShowStaffContactDetails"].ToBool())
                {
                    oHtmlTableCell = new HtmlTableCell();
                    oHtmlTableCell.Align = "left";
                    oHtmlTableCell.Width = "20%";
                    oHtmlTableCell.Height = "26px";
                    oHtmlTableCell.Attributes.Add("class", "ClsBorderlight paddingL");
                    oHtmlTableCell.Attributes.Add("colspan", "2");
                    oHtmlTableRow.Cells.Add(oHtmlTableCell);

                    Label oLblStaffName2 = new Label();
                    oLblStaffName2.Text = "Mobile No.: ";
                    oLblStaffName2.Style.Add("CssClass", "ClsLblRslt ");
                    tblStudentTransportDetails.Align = "center";
                    oHtmlTableCell.Controls.Add(oLblStaffName2);

                    oHtmlTableCell = new HtmlTableCell();
                    oHtmlTableCell.Align = "left";
                    oHtmlTableCell.Width = "35%";
                    oHtmlTableCell.Height = "26px";
                    oHtmlTableCell.Attributes.Add("class", "ClsBorderlight paddingL");
                    oHtmlTableCell.Attributes.Add("colspan", "2");
                    oHtmlTableRow.Cells.Add(oHtmlTableCell);

                    Label oLblMobileNumber2 = new Label();
                    oLblMobileNumber2.Text = moTransportDetails.Tables[I_TBL_STAFF_DETAILS_INDEX].Rows[iTransportStaffDetails][S_MOBILE_NUMBER].ToString();
                    oLblMobileNumber2.Style.Add("CssClass", "ClsLblRslt");
                    oHtmlTableCell.Controls.Add(oLblMobileNumber2);
                }
        }


        SetFieldState(moTransportDetails.Tables[I_TBL_VEHICLE_DETAILS_INDEX]);
    }

    private void SetFieldState(DataTable aoDT)
    {
        if (aoDT.Rows[0]["ShowStops"].ToBool())
            trStops.Visible = true;
        else
            trStops.Visible = false;

        if (aoDT.Rows[0]["ShowVehicleOfficialContactNo"].ToBool())
        {
            tdVehicleContactNoHeader.Visible = true;
            tdVehicleContactNo.Visible = true;
        }
        else
        {
            tdVehicleContactNoHeader.Visible = false;
            tdVehicleContactNo.Visible = false;
        }
    }

    private void SetGPSTrackingUrl()
    {
        hidGPSTrackingUrl.Value = Settings.GPSTrackingUrl.ToString();
        if (Settings.GPSTrackingUrl == string.Empty)
            hlnkGPSTrancking.Visible = false;
        else
            hlnkGPSTrancking.Visible = true;
}

    /// <summary>
    /// This method will be used to visible of hide fields.
    /// </summary>
    /// <param name="abFlag"></param>
    private void ClearFields(bool abFlag)
    {
        tblStudentTransportDetails.Visible = abFlag;
        tblLegend.Visible = abFlag;
        tblVehicleDetails.Visible = abFlag;
        tblShift.Visible = abFlag;
        tblStudentTransportDetails.Visible = abFlag;
        trTransportDetails.Visible = abFlag;
        trVehicleDetails.Visible = abFlag;
        trShift.Visible = abFlag;
        trShiftdet.Visible = abFlag;
        tblList.Visible = abFlag;
        tblErrorMessage.Visible = !abFlag;
        lblError.Text = "No records found.";       
    }    
}
