/* Class Name - TransportCapacityDetailsUI
 * Created By - Vishakha
 * Created On - 07-July-2023
 * Description - This class is used to display transport capacity details.
 */
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using BusinessLogic.TransportBL;
using SchoolEntities.Transport;
using Utility;

public partial class TransportNotificationDetailsUI : SchoolBase
{
    #region Data Member(s)

    private TransportNotificationDetailsBL moTransportNotificationDetailsBL;

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to display value of Route, Vehicle no, Jpurney drop down.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moTransportNotificationDetailsBL = new TransportNotificationDetailsBL(miSchoolId, miAcademicYearId);
            if (!IsPostBack)
            {
                SetDefaultValues();
                FillRoute();
                FillJourney();
                FillTypes();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwNotificationDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                NotificationDetailsForScreen oNotificationDetailsForScreen = e.Item.DataItem as NotificationDetailsForScreen;
                Label lblDate = e.Item.FindControl("lblDate") as Label;
                lblDate.Text = oNotificationDetailsForScreen.CreateDate.ToString(Constants.S_DATE_FORMAT + " " + "hh:mm tt");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill Route drop down.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlRoute_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillJourney();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill notification related listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillTransportNotificationDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
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
    /// This event is used to load current notification data.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLoadData_Click(object sender, EventArgs e)
    {
        try
        {
            TransportNotificationBL oTransportNotificationBL = new TransportNotificationBL();
            string DBName = ConfigurationManager.AppSettings["ReportDataBaseName"].ToString();
            oTransportNotificationBL.CopyTransportNotification(miSchoolId, DBName);
            lblMessage.Text = "Current Notification Details are loaded successfully!!!";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to fill notification listview.
    /// </summary>
    private void FillTransportNotificationDetails()
    {
        List<NotificationDetailsForScreen> lstTransportNotificationDetails = moTransportNotificationDetailsBL.GetTransportNotificationDetails(txtSearch.Text.Trim(), txtStartDate.Text.ToDateTime(), txtEndDate.Text.ToDateTime(),ddlTypes.SelectedValue.ToInt(), txtVehicleNo.Text.Trim(), ddlRoute.SelectedValue.ToInt(), ddlJourney.SelectedValue.ToInt());
        lstvwNotificationDetails.DataSource = lstTransportNotificationDetails;
        lstvwNotificationDetails.DataBind();

        lblTotalCount.Text = "Total Records : " + lstTransportNotificationDetails.Count;
    }

    /// <summary>
    /// This method is used to fill Route dropdown.
    /// </summary>
    private void FillRoute()
    {
        List<Route> lstRoute = moTransportNotificationDetailsBL.GetRoute();
        ListSource.FillDropDownList(lstRoute, ddlRoute, "RouteName", "RouteId", Constants.S_ALL);
    }

    /// <summary>
    /// This method is used to fill journey dropdown.
    /// </summary>
    private void FillJourney()
    {
        List<JourneyDetails> lstJourney = moTransportNotificationDetailsBL.GetJourney(ddlRoute.SelectedValue.ToInt());
        ListSource.FillDropDownList(lstJourney, ddlJourney, "TransportShiftName", "TransportShiftId", Constants.S_ALL);
    }

    private void FillTypes()
    {
        ddlTypes.Items.Add(new ListItem { Text = "-- All --", Value = "0" });
        ddlTypes.Items.Add(new ListItem { Text = "On board", Value = "1" });
        ddlTypes.Items.Add(new ListItem { Text = "Off board", Value = "2" });
        ddlTypes.Items.Add(new ListItem { Text = "Geofencing", Value = "3" });
    }

    /// <summary>
    /// This method is used to show validation summary.
    /// </summary>
    private void SetDefaultValues()
    {
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        txtStartDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        txtEndDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        base.SetDefaultButton(btnSearch);
    }

    private void ClearFields()
    {
        ddlRoute.ClearSelection();
        ddlJourney.ClearSelection();
        ddlTypes.ClearSelection();
        txtVehicleNo.Text = string.Empty;
        txtSearch.Text = string.Empty;
        txtStartDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        txtEndDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);

        lstvwNotificationDetails.DataSource = null;
        lstvwNotificationDetails.DataBind();
        lblTotalCount.Text = "Total Records : 0";
    }

    #endregion    
}