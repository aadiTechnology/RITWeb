using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using System.Reflection;
using BusinessLogic;
using SchoolEntities;
using SchoolEntities.Transport;
using Utility;
using System.Data;
using BusinessLogic.TransportBL;
using System.Configuration;

public partial class RouteStopAssignmentPopup : SchoolBase
{
    #region Data Member(s)

    private RouteStopAssignmentBL moRouteStopAssignmentBL; 

    #endregion

    bool ShowJourney
    {
        get
        {
            return moSchool == Constants.SchoolId.SNS;
        }
    }

    #region Event(s)
    
    /// <summary>
    /// This event is used to fill all the transport details on page load.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moRouteStopAssignmentBL = new RouteStopAssignmentBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                ReadQuerystring();
                RenameLabel();
                SetJavascriptAttributes();                
                FillRoutes();
                SetDefaultView();
                FillTransportDetails();
                ShowHideControl();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle route change event. On changing route, we will fill all stops and shift.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbRoute_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillStopShift(cmbRoute.SelectedValue.ToInt(), cmbStop, cmbShift, 1);            
            ShowHideControl();
            FillVehicles(cmbRoute.SelectedValue.ToInt(), cmbStop.SelectedValue.ToInt(), cmbShift.SelectedValue.ToInt(), cmbVehicle);           
            ResetFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle stop change event. On changing stop, we will fill vehicles.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStop_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillVehicles(cmbRoute.SelectedValue.ToInt(), cmbStop.SelectedValue.ToInt(), cmbShift.SelectedValue.ToInt(), cmbVehicle);
            ResetFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle drop route change event. On changing route, we will fill all stops and shift.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbDropRoute_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillStopShift(cmbDropRoute.SelectedValue.ToInt(), cmbDropStop, cmbDropShift, 2);
            ShowHideControl();
            FillVehicles(cmbDropRoute.SelectedValue.ToInt(), cmbDropStop.SelectedValue.ToInt(), cmbDropShift.SelectedValue.ToInt(), cmbDropVehile);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle drop stop change event. On changing stop, we will fill vehicles.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbDropStop_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillVehicles(cmbDropRoute.SelectedValue.ToInt(), cmbDropStop.SelectedValue.ToInt(), cmbDropShift.SelectedValue.ToInt(), cmbDropVehile);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle shift change event. On changing shift, we will fill vehicles.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbDropShift_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillVehicles(cmbDropRoute.SelectedValue.ToInt(), cmbDropStop.SelectedValue.ToInt(), cmbDropShift.SelectedValue.ToInt(), cmbDropVehile);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle shift change event. On changing shift, we will fill vehicles.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbShift_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillVehicles(cmbRoute.SelectedValue.ToInt(), cmbStop.SelectedValue.ToInt(), cmbShift.SelectedValue.ToInt(), cmbVehicle);
            ResetFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }   

    /// <summary>
    /// This event is used to handle the checkbox check change event. Depending on which we deside the associated on non associated users.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkIncludeAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            cmbDropRoute.SelectedValue = cmbRoute.SelectedValue;
            FillStopShift(cmbDropRoute.SelectedValue.ToInt(), cmbDropStop, cmbDropShift, 2);
            cmbDropStop.SelectedValue = cmbStop.SelectedValue;
            cmbDropShift.SelectedValue = cmbShift.SelectedValue;
            FillVehicles(cmbDropRoute.SelectedValue.ToInt(), cmbDropStop.SelectedValue.ToInt(), cmbDropShift.SelectedValue.ToInt(), cmbDropVehile);
            cmbDropVehile.SelectedValue = cmbVehicle.SelectedValue;

            if (chkIncludeAll.Checked)
            {
                trDropRoute.Disabled = true;
                trDropStop.Disabled = true;
                trDropShift.Disabled = true;
            }
            else
            {
                trDropRoute.Disabled = false;
                trDropStop.Disabled = false;
                trDropShift.Disabled = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle the save button event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string sTravelerTransportXML = GetTravelersTransportXML();
            moRouteStopAssignmentBL.Insert(sTravelerTransportXML, txtPaymentDate.Text.ToDateTime(),txtEndDate.Text, hidUserId.Value.ToInt());

            if (ConfigurationManager.AppSettings["TransportExternalDBName"] != null && ConfigurationManager.AppSettings["TransportExternalDBName"].ToString() != string.Empty)
            {
                string sDBName = ConfigurationManager.AppSettings["reportdatabasename"].ToString();
                string sTransportDBName = ConfigurationManager.AppSettings["TransportExternalDBName"].ToString();
                TransferTransportDetailsBL oTransferTransportDetailsBL = new TransferTransportDetailsBL(miSchoolId, sDBName, sTransportDBName);
                oTransferTransportDetailsBL.UpdateJourneyDetails();
            }

            ScriptManager.RegisterStartupScript(btnSave, this.GetType(), "CloseWindow", "CloseWindow();", true);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Thie event is used to handel delete button event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            ClearAllFields();
            btnSave_Click(sender, e);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method(s)

    /// <summary>
    /// This method is used to clear all the controls for delete transport assignment.
    /// </summary>
    private void ClearAllFields()
    {
        cmbRoute.ClearSelection();
        cmbStop.ClearSelection();
        cmbShift.ClearSelection();
        cmbVehicle.ClearSelection();
        cmbDropRoute.ClearSelection();
        cmbDropStop.ClearSelection();
        cmbDropShift.ClearSelection();
        cmbDropVehile.ClearSelection();        
    }

    /// <summary>
    /// This method is used to reset the fields on changing of route,stop,shifts.
    /// </summary>
    private void ResetFields()
    {
        if (cmbRoute.SelectedValue != Constants.S_ZERO && cmbStop.SelectedValue != Constants.S_ZERO && cmbShift.SelectedValue != Constants.S_ZERO)
            chkIncludeAll.Enabled = true;
        else
        {
            chkIncludeAll.Enabled = false;
            chkIncludeAll.Checked = false;
            
            trDropRoute.Disabled = false;
            trDropStop.Disabled = false;
            trDropShift.Disabled = false;
            if (chkIncludeAll.Checked)
            {
                cmbDropRoute.ClearSelection();
                cmbDropStop.ClearSelection();
                cmbDropShift.ClearSelection();
            }
        }
    }

    private void ShowHideControl()
    {
        if (cmbRoute.SelectedValue != Constants.S_ZERO)
        {
            StopMandatory.Visible = true;
            ShiftMandatory.Visible = true;
            VehicleMandatory.Visible = true;
        }
        else
        {
            StopMandatory.Visible = false;
            ShiftMandatory.Visible = false;
            VehicleMandatory.Visible = false;
        }

        if (cmbDropRoute.SelectedValue != Constants.S_ZERO)
        {
            StopMandatory1.Visible = true;
            ShiftMandatory1.Visible = true;
            VehicleMandatory1.Visible = true;
        }
        else
        {
            StopMandatory1.Visible = false;
            ShiftMandatory1.Visible = false;
            VehicleMandatory1.Visible = false;
        }

        if (cmbRoute.SelectedValue != Constants.S_ZERO && cmbDropRoute.SelectedValue != Constants.S_ZERO)
            btnDelete.Enabled = true;
        else
            btnDelete.Enabled = false;
    }

    /// <summary>
    /// This method is used to decrypt query string.
    /// </summary>
    private void ReadQuerystring()
    {
        if (Request.QueryString.ToString() == Constants.S_EMPTY_STRING)
            return;

        hidQueryString.Value = Request.QueryString.ToString();

        if (!QueryString["UserId"].IsNull())
            hidUserId.Value = QueryString["UserId"];

        if (!QueryString["UserName"].IsNull())
        {
            hidUserName.Value = QueryString["UserName"];
            lblName.Text = hidUserName.Value;
        }
    }

    /// <summary>
    /// This function is used to initialize controls to their default values.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnClose, btnSave });
        base.SetDefaultButton(btnSave);        
        btnClose.Attributes.Add("onclick", "CloseWindow()");
        btnSave.Text = Resources.LocalizedResources.Save;
        hidAcYearStartDate.Value = (Convert.ToDateTime(Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE])).ToString("dd-MMM-yyyy");
        hidAcYearEndDate.Value = (Convert.ToDateTime(Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE])).ToString("dd-MMM-yyyy");
        btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");
    }

    /// <summary>
    /// This function is used to set the default view.
    /// </summary>
    private void SetDefaultView()
    {
        FillStopShift(cmbRoute.SelectedValue.ToInt(), cmbStop, cmbShift, 1);
        FillVehicles(cmbRoute.SelectedValue.ToInt(), cmbStop.SelectedValue.ToInt(), cmbShift.SelectedValue.ToInt(), cmbVehicle);
        FillStopShift(cmbDropRoute.SelectedValue.ToInt(), cmbDropStop, cmbDropShift, 2);
        FillVehicles(cmbDropRoute.SelectedValue.ToInt(), cmbDropStop.SelectedValue.ToInt(), cmbDropShift.SelectedValue.ToInt(), cmbDropVehile);            
    }

    /// <summary>
    /// This method is used to fill user role, route,shift and stop combos of filter.
    /// </summary>
    private void FillRoutes()
    {
        RouteDetailsBL oRouteDetailsBL = new RouteDetailsBL(miSchoolId, miAcademicYearId, miUserId);
        List<TransportRoutes> lstTransportRoute = oRouteDetailsBL.GetAllRoutes();
        ListSource.FillDropDownList(lstTransportRoute, cmbRoute, "RouteName", "RouteId", Constants.S_SELECT);
        ListSource.FillDropDownList(lstTransportRoute, cmbDropRoute, "RouteName", "RouteId", Constants.S_SELECT);               
    }

    /// <summary>
    /// This method is used fill all the stops as well as shifts for selected route.
    /// </summary>
    /// <param name="aoCmbRoute"></param>
    /// <param name="aoCmbStop"></param>
    /// <param name="aoCmbShift"></param>
    private void FillStopShift(int aiRouteId, DropDownList aoCmbStop, DropDownList aoCmbShift, int aiJourneyTypeId)
    {
        List<StopDetails> lstStopDetails = moRouteStopAssignmentBL.GetStopShiftDetails(aiRouteId);
        List<ShiftDetails> lstShiftDetails = moRouteStopAssignmentBL.ShiftDetail;

        lstShiftDetails = lstShiftDetails.Where(sh => sh.JourneyTypeId == 0 || sh.JourneyTypeId == aiJourneyTypeId).ToList();

        ListSource.FillDropDownList(lstStopDetails, aoCmbStop, "StopName", "StopId", Constants.S_SELECT);
        ListSource.FillDropDownList(lstShiftDetails, aoCmbShift, "ShiftName", "ShiftId", Constants.S_SELECT);        
    }

    /// <summary>
    /// This method is used to fill all the vehicles available for selected route,stop,shift.
    /// </summary>
    /// <param name="aoCmbRoute"></param>
    /// <param name="aoCmbStop"></param>
    /// <param name="aoCmbShift"></param>
    /// <param name="aoCmbVehicles"></param>
    private void FillVehicles(int aiRouteId, int aiStopId, int aiShiftId,DropDownList aoCmbVehicles)
    {
        VehicleDetailsBL oVehicleDetailsBL = new VehicleDetailsBL(miSchoolId, miAcademicYearId, miUserId);
        List<VehicleDetails> lstVehicleDetail = oVehicleDetailsBL.GetVehicles(aiRouteId, aiStopId, aiShiftId);
        ListSource.FillDropDownList(lstVehicleDetail, aoCmbVehicles, "VehicleNumber", "VehicleId", Constants.S_SELECT);        
    }

    /// <summary>
    /// This method is used to load the users transport assignment.
    /// </summary>
    private void FillTransportDetails()
    {
        TravelerTransportDetailsBL oTravelerTransportDetailsBL = new TravelerTransportDetailsBL(miSchoolId, miAcademicYearId, miUserId);
        List<TransportDetails> lstTransportDetail = oTravelerTransportDetailsBL.GetAll(hidUserId.Value.ToInt());
        if (lstTransportDetail.Count > 0)
        {
            // Here we load both pick up and drop details seperately as controls for both are different.
            TransportDetails oTransportDetails = null;
            if (lstTransportDetail.Exists(a => a.TransportTypeId == Constants.TransportType.Pickup.ToInt()))
            {
                oTransportDetails = lstTransportDetail.Where(a => a.TransportTypeId == Constants.TransportType.Pickup.ToInt()).FirstOrDefault();
                cmbRoute.SelectedValue = oTransportDetails.RouteId.ToString();
                FillStopShift(cmbRoute.SelectedValue.ToInt(), cmbStop, cmbShift, 1);
                cmbStop.SelectedValue = oTransportDetails.StopId.ToString();
                cmbShift.SelectedValue = oTransportDetails.ShiftId.ToString();
                FillVehicles(cmbRoute.SelectedValue.ToInt(), cmbStop.SelectedValue.ToInt(), cmbShift.SelectedValue.ToInt(), cmbVehicle);            
                cmbVehicle.SelectedValue = oTransportDetails.VehicleId.ToString();

                if (oTransportDetails.EffectiveFromDate != DateTime.MinValue)
                    txtPaymentDate.Text = oTransportDetails.EffectiveFromDate.ToString(Constants.S_DATE_FORMAT);

                if (oTransportDetails.EffectiveToDate != DateTime.MinValue)
                    txtEndDate.Text = oTransportDetails.EffectiveToDate.ToString(Constants.S_DATE_FORMAT);
            }

            if (lstTransportDetail.Exists(a => a.TransportTypeId == Constants.TransportType.Drop.ToInt()))
            {
                oTransportDetails = lstTransportDetail.Where(a => a.TransportTypeId == Constants.TransportType.Drop.ToInt()).FirstOrDefault();
                cmbDropRoute.SelectedValue = oTransportDetails.RouteId.ToString();
                FillStopShift(cmbDropRoute.SelectedValue.ToInt(), cmbDropStop, cmbDropShift, 2);
                cmbDropStop.SelectedValue = oTransportDetails.StopId.ToString();
                cmbDropShift.SelectedValue = oTransportDetails.ShiftId.ToString();
                FillVehicles(cmbDropRoute.SelectedValue.ToInt(), cmbDropStop.SelectedValue.ToInt(), cmbDropShift.SelectedValue.ToInt(), cmbDropVehile);            
                cmbDropVehile.SelectedValue = oTransportDetails.VehicleId.ToString();
            }
        }
    }


    /// <summary>
    /// This method is used to generate Xml for traveler's transport details.
    /// </summary>
    /// <returns></returns>
    private string GetTravelersTransportXML()
    {
        List<TransportDetails> lstTransportDetail = new List<TransportDetails>();

        // Here we collect pick details to generate XML.
        if (cmbRoute.SelectedValue != Constants.S_ZERO)
        {
            TransportDetails oPickupTransportDetails = new TransportDetails
            {
                TransportTypeId = Constants.TransportType.Pickup.ToInt(),
                RouteId = cmbRoute.SelectedValue.ToInt(),
                StopId = cmbStop.SelectedValue.ToInt(),
                ShiftId = cmbShift.SelectedValue.ToInt(),
                VehicleId = cmbVehicle.SelectedValue.ToInt()
            };

            lstTransportDetail.Add(oPickupTransportDetails);
        }

        // Here we collect drop details to generate XML.
        if (cmbDropRoute.SelectedValue != Constants.S_ZERO)
        {
            TransportDetails oDropTransportDetails = new TransportDetails
            {
                TransportTypeId = Constants.TransportType.Drop.ToInt(),
                RouteId = cmbDropRoute.SelectedValue.ToInt(),
                StopId = cmbDropStop.SelectedValue.ToInt(),
                ShiftId = cmbDropShift.SelectedValue.ToInt(),
                VehicleId = cmbDropVehile.SelectedValue.ToInt()
            };

            lstTransportDetail.Add(oDropTransportDetails);
        }

        return CommonUtility.GenerateXml(lstTransportDetail);
    }

    private void RenameLabel()
    {
        if (miSchoolId == Constants.SchoolId.SNS.ToInt())
        {
            lblPickupShift.Text = "Journey";
            lblDropShift.Text = "Journey";
            cstvalPickupShift.ErrorMessage = "Pickup Journey should be selected.";
            cstvalDropShift.ErrorMessage = "Drop Journey should be selected.";
        }
    }


    #endregion           
}