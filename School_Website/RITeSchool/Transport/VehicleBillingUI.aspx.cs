/* File Name - VehicleBillingUI.aspx.cs
 * Created Date - 17 Apr 2020
 * Created By - Sachin
 * Description - This class is used to export behilce billing details.
 */
using System;
using System.Collections.Generic;
using BusinessLogic;
using CrystalDecisions.Shared;
using SchoolEntities.Transport;
using Utility;

public partial class VehicleBillingUI : SchoolBase
{
    #region Data MEmber(s)
    
    private VehicleDetailsBL moVehicleDetailsBL; 

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to fillvehicle dropdownlist.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        moVehicleDetailsBL = new VehicleDetailsBL(miSchoolId, miAcademicYearId, miUserId);
        if (!IsPostBack)
        {
            txtStartDate.Text = DateTime.Now.Date.AddDays((DateTime.Now.Date.Day * -1) + 1).ToString(Constants.S_DATE_FORMAT);
            txtEndDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);

            if (moUserRole == Constants.UserRoles.Admin)
            {
                btnBack.Visible = true;
                btnBack.PostBackUrl = Constants.S_PAGE_CONTROL_PANEL;
            }

            FillVehicleList();
        }
    }

    /// <summary>
    /// This event is used to export vehicle billing details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShow_Click(object sender, EventArgs e)
    {
        ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.VehicleBillingDetails, GetFilterString(), ExportFormatType.PortableDocFormat);
        oReportDisplay.DisplayReport();
    } 

    #endregion

    #region Method(s)
    
    /// <summary>
    /// This method is used to fill vehicle details.
    /// </summary>
    private void FillVehicleList()
    {
        List<VehicleDetails> lstVehicleDetails = moVehicleDetailsBL.GetAllVehicles();
        ListSource.FillDropDownList(lstVehicleDetails, cmbVehicles, "VehicleNumber", "VehicleId", Constants.S_ALL);
    }

    /// <summary>
    /// This method is used to return filter string.
    /// </summary>
    /// <returns></returns>
    private string GetFilterString()
    {
        return "([Transport].[usp_GetVehicleBillingDetails].School_Id}=" + miSchoolId + " AND [Transport].[usp_GetVehicleBillingDetails].Academic_Year_Id}=" + miAcademicYearId + " AND [Transport].[usp_GetVehicleBillingDetails].StartDate}=" + txtStartDate.Text + " AND [Transport].[usp_GetVehicleBillingDetails].EndDate}=" + txtEndDate.Text + " AND [Transport].[usp_GetVehicleBillingDetails].VehicleId}=" + cmbVehicles.SelectedValue + ") @";
    } 

    #endregion
}