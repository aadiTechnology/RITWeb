/* Class Name - TransportCapacityDetailsUI
 * Created By - Vishakha
 * Created On - 07-July-2023
 * Description - This class is used to display transport capacity details.
 */
using System;
using System.Collections.Generic;
using BusinessLogic.Exceptions;
using BusinessLogic.TransportBL;
using SchoolEntities.Transport;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;


public partial class TransportCapacityDetailsUI : SchoolBase
{
    private TransportCapacityDetailsBL moTransportCapacityDetailsBL;
    
    /// <summary>
    /// This event is used to display transport capacity details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moTransportCapacityDetailsBL = new TransportCapacityDetailsBL(miSchoolId, miAcademicYearId);
            if (!IsPostBack)
            {
                FillTransportCapacityDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwTransportCapacity_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                string sVehicleNumber = Convert.ToString(lstvwTransportCapacity.DataKeys[e.Item.DisplayIndex]["VehicleNumber"].ToString());

                TransportCapacityDetails oTransportCapacityDetails = e.Item.DataItem as TransportCapacityDetails;

                string sTooltip = "Click here to see standardwise count";

                LinkButton lnkPickupCountA = e.Item.FindControl("lnkPickupCountA") as LinkButton;
                if (oTransportCapacityDetails.PickUpCount_A > 0)
                {
                    lnkPickupCountA.Attributes.Add("onclick", "FillStandardwiseCount(this,'" + sVehicleNumber + "',' " + "1" + "', '" + "A" + "'); return false;");
                    lnkPickupCountA.ToolTip = sTooltip;
                }
                else
                    lnkPickupCountA.Attributes.Add("onclick", "CloseDiv();return false;");

                LinkButton lnkPickupCountB = e.Item.FindControl("lnkPickupCountB") as LinkButton;
                if (oTransportCapacityDetails.PickUpCount_B > 0)
                {
                    lnkPickupCountB.Attributes.Add("onclick", "FillStandardwiseCount(this,'" + sVehicleNumber + "',' " + "1" + "', '" + "B" + "'); return false;");
                    lnkPickupCountB.ToolTip = sTooltip;
                }
                else
                    lnkPickupCountB.Attributes.Add("onclick", "CloseDiv();return false;");

                LinkButton lnkPickupCountC = e.Item.FindControl("lnkPickupCountC") as LinkButton;
                if (oTransportCapacityDetails.PickUpCount_C > 0)
                {
                    lnkPickupCountC.Attributes.Add("onclick", "FillStandardwiseCount(this,'" + sVehicleNumber + "',' " + "1" + "', '" + "C" + "'); return false;");
                    lnkPickupCountC.ToolTip = sTooltip;
                }
                else
                    lnkPickupCountC.Attributes.Add("onclick", "CloseDiv();return false;");

                LinkButton lnkDropCountA = e.Item.FindControl("lnkDropCountA") as LinkButton;
                if (oTransportCapacityDetails.DropCount_A > 0)
                {
                    lnkDropCountA.Attributes.Add("onclick", "FillStandardwiseCount(this,'" + sVehicleNumber + "',' " + "2" + "', '" + "A" + "'); return false;");
                    lnkDropCountA.ToolTip = sTooltip;
                }
                else
                    lnkDropCountA.Attributes.Add("onclick", "CloseDiv();return false;");

                LinkButton lnkDropCountB = e.Item.FindControl("lnkDropCountB") as LinkButton;
                if (oTransportCapacityDetails.DropCount_B > 0)
                {
                    lnkDropCountB.Attributes.Add("onclick", "FillStandardwiseCount(this,'" + sVehicleNumber + "', '" + "2" + "', '" + "B" + "'); return false;");
                    lnkDropCountB.ToolTip = sTooltip;
                }
                else
                    lnkDropCountB.Attributes.Add("onclick", "CloseDiv();return false;");

                LinkButton lnkDropCountC = e.Item.FindControl("lnkDropCountC") as LinkButton;
                if (oTransportCapacityDetails.DropCount_C > 0)
                {
                    lnkDropCountC.Attributes.Add("onclick", "FillStandardwiseCount(this,'" + sVehicleNumber + "', '" + "2" + "', '" + "C" + "'); return false;");
                    lnkDropCountC.ToolTip = sTooltip;
                }
                else
                    lnkDropCountC.Attributes.Add("onclick", "CloseDiv();return false;");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to fill transport capacity related listview.
    /// </summary>
    private void FillTransportCapacityDetails()
    {
        List<TransportCapacityDetails> lstTransportCapacity = moTransportCapacityDetailsBL.GetTransportCapacityDetails();
        lstvwTransportCapacity.DataSource = lstTransportCapacity;
        lstvwTransportCapacity.DataBind();

        var jsSerializer = new JavaScriptSerializer();
        hidStd.Value = jsSerializer.Serialize(moTransportCapacityDetailsBL.StandardwiseCount);
    }
}