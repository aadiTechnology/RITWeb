/* Class Name - UserAttendanceInBusDetailsUI
 * Created By - Vishakha
 * Created On - 29-Jun-2023
 * Description - This class is used to display user attendance details.
 */
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using BusinessLogic.TransportBL;
using SchoolEntities.Transport;
using System.Configuration;
using System.Linq;

public partial class UserAttendanceInBusDetailsUI : SchoolBase
{
    #region Data Member(s)

    private UserAttendanceInBusBL moUserAttendanceBusInBL;

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to fill vehicle no. journey dropdown.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moUserAttendanceBusInBL = new UserAttendanceInBusBL(miSchoolId, miAcademicYearId);
            if (!IsPostBack)
            {
                SetDefaultValues();
                FillVehicleNumber();
                FillJourney();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to sorting activity.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwUserAttendanceDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = oCurrentItem.DisplayIndex;
                int iVehicleId = Convert.ToInt32(lstvwUserAttendanceDetails.DataKeys[iRowId]["VehicleId"]);

            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill user attendance details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwUserAttendanceDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                UserAttendanceInBus oUserAttendanceInBus = e.Item.DataItem as UserAttendanceInBus;
                LinkButton LnkBtnLocation = e.Item.FindControl("LnkBtnLocation") as LinkButton;

                if (oUserAttendanceInBus.LocationURL != string.Empty && oUserAttendanceInBus.LocationURL.TrimAll() != "-")
                    LnkBtnLocation.Attributes.Add("onclick", "if(OpenLocation('" + oUserAttendanceInBus.LocationURL + "')) return false;");
                else
                    LnkBtnLocation.Visible = false;

                string sComment = "", sColor = "", sText="";
                        
                if (oUserAttendanceInBus.IsVehicleChanged)
                {
                    sComment = "Vehicle changed : " + oUserAttendanceInBus.VehicleNo;
                    sColor = "Maroon";
                    sText = "N/A";
                }
                else if (oUserAttendanceInBus.IsJourneyChanged)
                {
                    sComment = "Journey changed.";
                    sColor = "Navy";
                }

                if (oUserAttendanceInBus.IsVehicleChanged || oUserAttendanceInBus.IsJourneyChanged)
                {
                    SetColor(e.Item, "lblSrno", sComment, sColor);
                    SetColor(e.Item, "lblStudentName", sComment, sColor);
                    SetColor(e.Item, "lblStdName", sComment, sColor);
                    SetColor(e.Item, "lblRouteName", sComment, sColor);
                    SetColor(e.Item, "lblTime1", sComment, sColor);
                    SetColor(e.Item, "lblOnBoard", sComment, sColor, sText);
                    SetColor(e.Item, "lblGeo", sComment, sColor, sText);
                    SetColor(e.Item, "lblOffBoard", sComment, sColor, sText);
                }
            } 
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is usd to fill pager footer.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwUserAttendanceDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            //if (lstvwUserAttendanceDetails.Items.Count > 0)
            //    ControlUtility.FillListViewPagerFooter(lstvwUserAttendanceDetails, DtPgCount);
            //else
            //    DtPgCount.Visible = false;

            trLegend.Visible = lstvwUserAttendanceDetails.Items.Count > 0;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to select page no.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwUserAttendanceDetails);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill Journey dropdown.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlVehicleNumber_SelectedIndexChanged(object sender, EventArgs e)
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
    /// This event is used to show user attendance details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            FillUserAttendanceDetails();
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

    protected void btnLoadAttendance_Click(object sender, EventArgs e)
    {
        try
        {
            if (ConfigurationManager.AppSettings["TransportExternalDBName"] != null && ConfigurationManager.AppSettings["TransportExternalDBName"].ToString() != string.Empty)
            {
                string sDBName = ConfigurationManager.AppSettings["reportdatabasename"].ToString();
                string sTransportDBName = ConfigurationManager.AppSettings["TransportExternalDBName"].ToString();
                TransferTransportDetailsBL oTransferTransportDetailsBL = new TransferTransportDetailsBL(miSchoolId, sDBName, sTransportDBName);
                oTransferTransportDetailsBL.UpdateBusAttendanceDetails();
                lblMessage.Text = "Attendance Details are loaded successfully!!!";
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to fill user attendance related listview.
    /// </summary>
    private void FillUserAttendanceDetails()
    {      
        //lstvwUserAttendanceDetails.DataSourceID = objUserAttendance.ID;
        //lstvwUserAttendanceDetails.DataBind();

       List<UserAttendanceInBus> lstUserAttendanceInBus = moUserAttendanceBusInBL.GetAll(miSchoolId, miAcademicYearId, ddlVehicleNumber.SelectedValue.ToInt(),ddlJourney.SelectedValue.ToInt(), txtDate.Text.ToDateTime(), string.Empty,string.Empty,200,0);
       lstvwUserAttendanceDetails.DataSource = lstUserAttendanceInBus;
       lstvwUserAttendanceDetails.DataBind();
    }

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        txtDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);

        if (moSchool == Constants.SchoolId.SNS)
            trBusAttendance.Visible = true;
    }

    /// <summary>
    /// This method is used to fill vehicle dropdown.
    /// </summary>
    private void FillVehicleNumber()
    {
        List<Vehicle> lstVehicle = moUserAttendanceBusInBL.GetVehicleNumber();
        lstVehicle = lstVehicle.OrderBy(vd => vd.Display_Member).ToList();
        ListSource.FillDropDownList(lstVehicle, ddlVehicleNumber,"Display_Member", "Value_Member", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill journey dropdown.
    /// </summary>
    private void FillJourney()
    {
        List<Journey> lstJourney = moUserAttendanceBusInBL.GetJourney(ddlVehicleNumber.SelectedValue.ToInt());
        ListSource.FillDropDownList(lstJourney, ddlJourney, "Display_Member", "Value_Member", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to set color.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="asColumnName"></param>
    /// <param name="asToolTop"></param>
    private void SetColor(ListViewItem obj, string asColumnName, string asToolTop, string asColor, string asText="")
    {
        Label lblColName = obj.FindControl(asColumnName) as Label;
        lblColName.Style.Add("color", asColor);
        lblColName.Style.Add("font-weight", "bold");
        lblColName.ToolTip = asToolTop;
        if (asText != string.Empty)
            lblColName.Text = asText;
    }

    #endregion        
} 