/* File Name :- StudentTransportHistoryUI.aspx.cs
 * Created Date :- 16-Apr-2020
 * Class Description :- This class is used to display Transport history details of user.
 * Created By :- Dnyaneshwar Shinde
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using SchoolEntities.Transport;
using BusinessLogic;
using Utility;

public partial class StudentTransportHistoryUI : SchoolBase
{
    #region Event(s)
    
    /// <summary>
    /// This event is used to load basic controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ReadQueryString();
                FillTransportHistory();
                SetQueryString();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to bound the details to liast view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStudentHistory_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                Label lblLeftDate = e.Item.FindControl("lblLeftDate") as Label;
                Label lblEffectiveFromDate = e.Item.FindControl("lblEffectiveFromDate") as Label;
                Label lblEffectiveToDate = e.Item.FindControl("lblEffectiveToDate") as Label;

                TransportHistoryDetails oTransportHistoryDetails = e.Item.DataItem as TransportHistoryDetails;

                if (oTransportHistoryDetails.LeftDate != null && oTransportHistoryDetails.LeftDate.ToString(Constants.S_DATE_FORMAT) != Constants.S_DEFAULT_DATE_6)
                    lblLeftDate.Text = oTransportHistoryDetails.LeftDate.ToDateTime().ToString(Constants.S_DATE_FORMAT);
                else
                    lblLeftDate.Text = "-";

                if (oTransportHistoryDetails.EffectiveFromDate != null)
                    lblEffectiveFromDate.Text = oTransportHistoryDetails.EffectiveFromDate.ToDateTime().ToString(Constants.S_DATE_FORMAT);
                else
                    lblEffectiveToDate.Text = "-";

                if (oTransportHistoryDetails.EffectiveToDate != Convert.ToDateTime("01/01/0001"))
                    lblEffectiveToDate.Text = oTransportHistoryDetails.EffectiveToDate.ToDateTime().ToString(Constants.S_DATE_FORMAT);
                else
                    lblEffectiveToDate.Text = "-";
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Event(s)

    /// <summary>
    /// This method is used to read the query string.
    /// </summary>
    private void ReadQueryString()
    {
        if (QueryString["UserId"] != null)
            hidUserId.Value = QueryString["UserId"];

        if (QueryString["StdId"] != null)
            hidStandardId.Value = QueryString["StdId"];

        if (QueryString["DivId"] != null)
            hidDivisionId.Value = QueryString["DivId"];

        if (QueryString["SearchText"] != null)
            hidSearchText.Value = QueryString["SearchText"];

        if (QueryString["UserRoleId"] != null)
            hidUserRoleId.Value = QueryString["UserRoleId"];

        if (QueryString["RouteId"] != null)
            hidRouteId.Value = QueryString["RouteId"];

        if (QueryString["StopId"] != null)
            hidStopId.Value = QueryString["StopId"];

        if (QueryString["ShiftId"] != null)
            hidShiftId.Value = QueryString["ShiftId"];

        if (QueryString["IncludeNotAssociated"] != null)
            hidIncludeNotAssociated.Value = QueryString["IncludeNotAssociated"];
    }

    /// <summary>
    /// This methos is used to Fill the user Transport history details.
    /// </summary>
    private void FillTransportHistory()
    { 
        VehicleDetailsBL oVehicleDetailsBL = new VehicleDetailsBL(miSchoolId,miAcademicYearId,miUserId);
        List<TransportHistoryDetails> lstTransportHistoryDetails = oVehicleDetailsBL.GetUserTransportHistory(Convert.ToInt32(hidUserId.Value));

        if (lstTransportHistoryDetails.Count > Constants.I_ZERO)
            lblUserName.Text = lstTransportHistoryDetails[0].UserName;
        else
            tdUserName.Visible = false;

        lstvwStudentHistory.DataSource = lstTransportHistoryDetails;
        lstvwStudentHistory.DataBind();
    }

    /// <summary>
    /// This method is used to set the query string.
    /// </summary>
    private void SetQueryString()
    {
        bool bIncludeNotAssociated = false;
        if( hidIncludeNotAssociated.Value == Constants.S_ZERO)
            bIncludeNotAssociated = true;

        string sQueryStr = string.Format("UserId={0}&StdId={1}&DivId={2}&SearchText={3}&UserRoleId={4}&IncludeNotAssociated={5}&RouteId={6}&StopId={7}&ShiftId={8}", hidUserId.Value, hidStandardId.Value, hidDivisionId.Value, hidSearchText.Value, hidUserRoleId.Value, bIncludeNotAssociated, hidRouteId.Value, hidStopId.Value, hidShiftId.Value);
        btnBack.PostBackUrl = "../Transport/TravelerTransportDetailsUI.aspx?" + CommonUtility.EncryptQuerystring(sQueryStr);
    }

    #endregion
}