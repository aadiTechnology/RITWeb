using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using System.Reflection;
using Utility;

public partial class MarkLeftForTransportPopup : SchoolBase
{
    #region Event(s)

    /// <summary>
    /// This method is used to set default values to controls.
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
                SetJavascriptAttribute();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save button click.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            TravelerTransportDetailsBL oTravelerTransportDetailsBL = new TravelerTransportDetailsBL(miSchoolId, miAcademicYearId, miUserId);
            oTravelerTransportDetailsBL.LeftTransportService(hidUserId.Value.ToInt(), txtTransportLeftDate.Text.ToDateTime(), txtLeftReason.Text.TrimAll());

            bool bIncludeNotAssociated = false;
            if (hidIncludeNotAssociated.Value == Constants.S_ZERO)
                bIncludeNotAssociated = true;
            string sQueryStr = string.Format("UserId={0}&StdId={1}&DivId={2}&SearchText={3}&UserRoleId={4}&IncludeNotAssociated={5}&RouteId={6}&StopId={7}&ShiftId={8}", hidUserId.Value, hidStandardId.Value, hidDivisionId.Value, hidSearchText.Value, hidUserRoleId.Value, bIncludeNotAssociated, hidRouteId.Value, hidStopId.Value, hidShiftId.Value);
            sQueryStr = CommonUtility.EncryptQuerystring(sQueryStr);
            sQueryStr = string.Format("'?{0}'", sQueryStr);
            Response.Write(string.Format("<Script language='Javascript'>window.opener.location=window.opener.location.pathname+{0};window.close();window.opener.focus(); </Script>", sQueryStr));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method(s)

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

        if (QueryString["UserName"] != null)
            lblUserName.Text = QueryString["UserName"].ToString();
    }

    /// <summary>
    /// This method is used to set java script attributes to controls.
    /// </summary>
    private void SetJavascriptAttribute()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnSave, btnClose });
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        btnClose.Attributes.Add("onclick", "window.close();");
        txtTransportLeftDate.Text = DateTime.Now.ToString(Constants.S_DATE_FORMAT);
    }

    #endregion
}