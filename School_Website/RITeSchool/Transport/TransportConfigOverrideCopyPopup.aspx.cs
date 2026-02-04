/*File Name - TransportConfigOverrideCopyPopup.aspx.cs
 * Created By - Sachin
 * Created Date - 26-Oct-2023
 * Description - This class isused to copy transport config overrides.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using System.Configuration;
using BusinessLogic.TransportBL;

public partial class TransportConfigOverrideCopyPopup : SchoolBase
{
    #region Event(s)

    /// <summary>
    /// This event is used to fill vehicle listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                FillVehicleRouteShifts();
                SetDefaultValues();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to copy override configuration.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCopy_Click(object sender, EventArgs e)
    {
        try
        {
            List<VehicleJourney> lstVehicleIds = (from item in lstVehicles.Items
                                                  let chkSelect = item.FindControl("chkSelect") as CheckBox
                                                  where chkSelect.Checked
                                                  let iVehicleId = lstVehicles.DataKeys[item.DisplayIndex]["VehicleId"].ToInt()
                                                  let iJourneyId = lstVehicles.DataKeys[item.DisplayIndex]["TransportShiftId"].ToInt()
                                                  let iRouteId = lstVehicles.DataKeys[item.DisplayIndex]["RouteId"].ToInt()
                                                  select new VehicleJourney { RouteId = iRouteId, VehicleId = iVehicleId, JourneyId = iJourneyId }).ToList();

            string sVehicleIds = this.GenerateXml(lstVehicleIds);

            RouteShiftTimingDetailsBL oRouteShiftTimingDetailsBL = new RouteShiftTimingDetailsBL();
            oRouteShiftTimingDetailsBL.CopyConfig(miSchoolId, miAcademicYearId, miUserId, QueryString["Id"].ToInt(), sVehicleIds, txtDisplayName.Text.Trim());

            if (ConfigurationManager.AppSettings["TransportExternalDBName"] != null && ConfigurationManager.AppSettings["TransportExternalDBName"].ToString() != string.Empty)
            {
                string sDBName = ConfigurationManager.AppSettings["reportdatabasename"].ToString();
                string sTransportDBName = ConfigurationManager.AppSettings["TransportExternalDBName"].ToString();
                TransferTransportDetailsBL oTransferTransportDetailsBL = new TransferTransportDetailsBL(miSchoolId, sDBName, sTransportDBName);
                oTransferTransportDetailsBL.UpdateJourneyOverrideDetails();
            }

            Response.Write("<script>window.opener.location.reload();window.close();window.opener.focus();</script>");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to hide selection option and set legends.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstVehicles_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                bool bIsAlreadyExist = lstVehicles.DataKeys[e.Item.DisplayIndex]["IsAlreadyExist"].ToBool();
                if (bIsAlreadyExist)
                {
                    CheckBox chkSelect = e.Item.FindControl("chkSelect") as CheckBox;
                    chkSelect.Visible = false;

                    Label lblVehicleNo = e.Item.FindControl("lblVehicleNo") as Label;
                    lblVehicleNo.ForeColor = System.Drawing.Color.Navy;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    } 

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to set default value.
    /// </summary>
    private void SetDefaultValues()
    {
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
    }

    /// <summary>
    /// This method is sued to fill vehicle-route-shifts.
    /// </summary>
    private void FillVehicleRouteShifts()
    {
        RouteShiftTimingDetailsBL oRouteShiftTimingDetailsBL = new RouteShiftTimingDetailsBL();
        DataTable dt = oRouteShiftTimingDetailsBL.GetAllVehicles(miSchoolId, miAcademicYearId, QueryString["Id"].ToInt());
        lstVehicles.DataSource = dt;
        lstVehicles.DataBind();
    }

    /// <summary>
    ///		Converts the given object into XML form.
    /// </summary>
    /// <param name="alstGenerateXML"></param>
    /// <returns></returns>
    public string GenerateXml(Object alstGenerateXML)
    {
        var oStrwrtr = new StringWriter();
        new XmlSerializer(alstGenerateXML.GetType()).Serialize(oStrwrtr, alstGenerateXML);
        string sXml = oStrwrtr.ToString();
        return sXml.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", string.Empty);
    } 

    #endregion

    public class VehicleJourney
    {
        public int RouteId { get; set; }
        public int VehicleId { get; set; }
        public int JourneyId { get; set; }
    }
}