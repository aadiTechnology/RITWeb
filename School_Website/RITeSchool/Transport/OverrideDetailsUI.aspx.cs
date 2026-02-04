/* File Name - OverrideDetailsUI.aspx.cs
 * Created By - Sachin
 * Created Date - 29-Aug-2023
 * Description - This class is used to show/save override details.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.TransportBL;
using SchoolEntities;
using SchoolEntities.Transport;
using Utility;
using BusinessLogic.Exceptions;
using System.Configuration;

public partial class OverrideDetailsUI : SchoolBase
{
    #region Data Member(s)
    
    private TransportOverrideDetailsBL moTransportOverrideDetailsBL; 

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to fill all fields as well as to show details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moTransportOverrideDetailsBL = new TransportOverrideDetailsBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                FillRoutes();
                SetDefaultValues();
                SetOverrideDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to validate fields.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Date_Validate(object sender, ServerValidateEventArgs e)
    {
        try
        {
            int iId = 0;
            if (QueryString["Id"] != null && QueryString["Id"].ToString() != string.Empty)
                iId = QueryString["Id"].ToInt();

            string sMessage = moTransportOverrideDetailsBL.Validate(cmbSourceRoute.SelectedValue.ToInt(), cmbSourceVehicle.SelectedValue.ToInt(), cmbSourceJourney.SelectedValue.ToInt(), txtStartDate.Text.ToDateTime(), txtEndDate.Text.ToDateTime(), iId, txtName.Text.Trim());
            if (sMessage == string.Empty)
                e.IsValid = true;
            else
            {
                ((CustomValidator)sender).ErrorMessage = sMessage;
                e.IsValid = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill source vehicles.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbSourceRoute_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillVehicles(cmbSourceRoute.SelectedValue.ToInt(), cmbSourceVehicle);
            cmbSourceVehicle_SelectedIndexChanged(cmbSourceVehicle, null);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill target vehicles.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbTargetRoute_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillVehicles(cmbTargetRoute.SelectedValue.ToInt(), cmbTargetVehicle);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill source jounrey.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbSourceVehicle_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillJourney(cmbSourceVehicle.SelectedValue.ToInt(), cmbSourceJourney);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill target journey.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbTargetVehicle_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillJourney(cmbTargetVehicle.SelectedValue.ToInt(), cmbTargetJourney);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill student listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbSourceJourney_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillStudentList();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save override details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                TransportOverrideDetails obj = Populate();
                moTransportOverrideDetailsBL.Save(obj);
                UpdateJourneyOverrides();

                MasterPage oMaster = this.Master as MasterPage;
                oMaster.RedirectToNextPage("TransportOverrideDetailsUI.aspx");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    } 

    #endregion

    #region Method(s)

    private void UpdateJourneyOverrides()
    {
        if (ConfigurationManager.AppSettings["TransportExternalDBName"] != null && ConfigurationManager.AppSettings["TransportExternalDBName"].ToString() != string.Empty)
        {
            string sDBName = ConfigurationManager.AppSettings["reportdatabasename"].ToString();
            string sTransportDBName = ConfigurationManager.AppSettings["TransportExternalDBName"].ToString();
            TransferTransportDetailsBL oTransferTransportDetailsBL = new TransferTransportDetailsBL(miSchoolId, sDBName, sTransportDBName);
            oTransferTransportDetailsBL.UpdateJourneyOverrideDetails();
        }
    }

    /// <summary>
    /// This method is used to fill student list.
    /// </summary>
    private void FillStudentList()
    {
        List<Student> lstStudents = moTransportOverrideDetailsBL.GetStudentList(cmbSourceRoute.SelectedValue.ToInt(), cmbSourceVehicle.SelectedValue.ToInt(), cmbSourceJourney.SelectedValue.ToInt());
        lstvwStudentList.DataSource = lstStudents;
        lstvwStudentList.DataBind();
    }

    /// <summary>
    /// This method is used to populate override object.
    /// </summary>
    /// <returns></returns>
    private TransportOverrideDetails Populate()
    {
        TransportOverrideDetails oOverride = new TransportOverrideDetails();

        oOverride.Id = QueryString["Id"].ToInt();

        oOverride.Name = txtName.Text.Trim();
        oOverride.StartDate = txtStartDate.Text.ToDateTime();
        oOverride.EndDate = txtEndDate.Text.ToDateTime();

        oOverride.SourceJourneyId = cmbSourceJourney.SelectedValue.ToInt();
        oOverride.SourceRouteId = cmbSourceRoute.SelectedValue.ToInt();
        oOverride.SourceVehicleId = cmbSourceVehicle.SelectedValue.ToInt();

        oOverride.TargetJourneyId = cmbTargetJourney.SelectedValue.ToInt();
        oOverride.TargetRouteId = cmbTargetRoute.SelectedValue.ToInt();
        oOverride.TargetVehicleId = cmbTargetVehicle.SelectedValue.ToInt();
        oOverride.UserIds = string.Empty;

        bool bIsUnChecked = false;
        StringBuilder obj = new StringBuilder();
        foreach (ListViewItem oItem in lstvwStudentList.Items)
        {
            CheckBox chkSelect = oItem.FindControl("chkSelect") as CheckBox;
            int iUserId = lstvwStudentList.DataKeys[oItem.DisplayIndex]["UserId"].ToInt();
            if (chkSelect.Checked)
                obj.Append("," + iUserId);
            else
                bIsUnChecked = true;

            if (obj.Length > 0)
                oOverride.UserIds = obj.ToString().Substring(1);
        }

        if (!bIsUnChecked)
            oOverride.UserIds = string.Empty;

        if (oOverride.UserIds != string.Empty && bIsUnChecked)
            oOverride.CategoryId = 3;
        else if (!bIsUnChecked && cmbSourceJourney.SelectedValue != Constants.S_ZERO)
            oOverride.CategoryId = 2;
        else if (cmbSourceJourney.SelectedValue == Constants.S_ZERO && cmbTargetJourney.SelectedValue == Constants.S_ZERO)
            oOverride.CategoryId = 1;

        return oOverride;
    }

    /// <summary>
    /// This method is used to show override details.
    /// </summary>
    private void SetOverrideDetails()
    {
        if (QueryString["Id"] != null && QueryString["Id"].ToString() != string.Empty)
        {
            TransportOverrideDetails oOverride = moTransportOverrideDetailsBL.Get(QueryString["Id"].ToInt());
            txtName.Text = oOverride.Name;
            txtStartDate.Text = oOverride.StartDate.ToString(Constants.S_DATE_FORMAT);
            txtEndDate.Text = oOverride.EndDate.ToString(Constants.S_DATE_FORMAT);

            cmbTargetRoute.SelectedValue = oOverride.TargetRouteId.ToString();
            cmbTargetRoute_SelectedIndexChanged(cmbTargetRoute, null);

            cmbTargetVehicle.SelectedValue = oOverride.TargetVehicleId.ToString();
            cmbTargetVehicle_SelectedIndexChanged(cmbTargetVehicle, null);

            cmbTargetJourney.SelectedValue = oOverride.TargetJourneyId.ToString();

            cmbSourceRoute.SelectedValue = oOverride.SourceRouteId.ToString();
            cmbSourceRoute_SelectedIndexChanged(cmbSourceRoute, null);

            cmbSourceVehicle.SelectedValue = oOverride.SourceVehicleId.ToString();
            cmbSourceVehicle_SelectedIndexChanged(cmbSourceVehicle, null);

            cmbSourceJourney.SelectedValue = oOverride.SourceJourneyId.ToString();
            cmbSourceJourney_SelectedIndexChanged(cmbSourceJourney, null);

            if (oOverride.UserIds != string.Empty)
            {
                List<int> lstUserIds = oOverride.UserIds.Split(',').ToList().Select(id => id.ToInt()).ToList();

                foreach (ListViewItem oItem in lstvwStudentList.Items)
                {
                    CheckBox chkSelect = oItem.FindControl("chkSelect") as CheckBox;
                    int iUserId = lstvwStudentList.DataKeys[oItem.DisplayIndex]["UserId"].ToInt();
                    if (lstUserIds.Contains(iUserId))
                        chkSelect.Checked = true;
                }
            }
            else if (oOverride.CategoryId == 2)
            {
                foreach (ListViewItem oItem in lstvwStudentList.Items)
                {
                    CheckBox chkSelect = oItem.FindControl("chkSelect") as CheckBox;
                    chkSelect.Checked = true;
                }

                HtmlTableCell th = lstvwStudentList.FindControl("thSelect") as HtmlTableCell;
                if (th != null)
                {
                    CheckBox chkSelectAll = th.FindControl("chkSelectAll") as CheckBox;
                    chkSelectAll.Checked = true;
                }
            }

            MasterPage oMaster = this.Master as MasterPage;
            oMaster.SetCurrentNodeText("Edit Override Details", moUserRole.ToInt(), miSchoolId);
        }
    }

    /// <summary>
    /// This method is used to set default details.
    /// </summary>
    private void SetDefaultValues()
    {
        cmbSourceJourney.Items.Add(new ListItem { Text = Constants.S_ALL, Value = Constants.S_ZERO });
        cmbSourceVehicle.Items.Add(new ListItem { Text = Constants.S_SELECT, Value = Constants.S_ZERO });
        cmbTargetJourney.Items.Add(new ListItem { Text = Constants.S_ALL, Value = Constants.S_ZERO });
        cmbTargetVehicle.Items.Add(new ListItem { Text = Constants.S_SELECT, Value = Constants.S_ZERO });
        txtStartDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        txtEndDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);

        lstvwStudentList.DataSource = null;
        lstvwStudentList.DataBind();

        btnCancel.PostBackUrl = "TransportOverrideDetailsUI.aspx";
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
    }

    /// <summary>
    /// This method is used to fill routes.
    /// </summary>
    private void FillRoutes()
    {
        RouteDetailsBL oRouteDetailsBL = new RouteDetailsBL(miSchoolId, miAcademicYearId, miUserId);
        List<TransportRoutes> lstTransportRoute = oRouteDetailsBL.GetAllRoutes();
        ListSource.FillDropDownList(lstTransportRoute, cmbSourceRoute, "RouteName", "RouteId", Constants.S_SELECT);
        ListSource.FillDropDownList(lstTransportRoute, cmbTargetRoute, "RouteName", "RouteId", Constants.S_SELECT);
    }

    //This method is used to fill vehicles.
    private void FillVehicles(int aiRouteId, DropDownList aoCmbVehicles)
    {
        VehicleDetailsBL oVehicleDetailsBL = new VehicleDetailsBL(miSchoolId, miAcademicYearId, miUserId);
        List<VehicleDetails> lstVehicleDetail = oVehicleDetailsBL.GetVehicles(aiRouteId);
        ListSource.FillDropDownList(lstVehicleDetail, aoCmbVehicles, "VehicleNumber", "VehicleId", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill journey.
    /// </summary>
    /// <param name="aiVehicleId"></param>
    /// <param name="aoDropDownList"></param>
    private void FillJourney(int aiVehicleId, DropDownList aoDropDownList)
    {
        UserAttendanceInBusBL oUserAttendanceBusInBL = new UserAttendanceInBusBL(miSchoolId, miAcademicYearId);
        List<Journey> lstJourney = oUserAttendanceBusInBL.GetJourney(aiVehicleId);
        ListSource.FillDropDownList(lstJourney, aoDropDownList, "Display_Member", "Value_Member", Constants.S_ALL);

        if (aoDropDownList == cmbSourceJourney)
        {
            lstvwStudentList.DataSource = null;
            lstvwStudentList.DataBind();
        }
    } 

    #endregion
}