using System;
using System.Collections.Generic;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Kendo.DynamicLinq;
using SchoolEntities;
using Utility;
using System.Web.Services;

public partial class PurchaseOrderApproverUI : SchoolBase
{
    #region Page Events

    /// <summary>
    /// This event is used to set page according to user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SetJavascriptAttributes();
                ReadQueryString();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }   

    /// <summary>
    /// This event is used to Get All Purchase Order details for fill KendoGrid.
    /// </summary>
    /// <param name="aiSchoolId"></param>
    /// <param name="asUserId"></param>
    /// <param name="asStatusId"></param>
    [WebMethod]
    public static DataSourceResult GetAllPODetails(string asSchoolId, string asUserId, string asStatusId)
    {
        PurchaseOrderBL oPurchaseOrderBL = new PurchaseOrderBL();
        List<PODetailsForApprove> lstPODetailsForApprove = oPurchaseOrderBL.GetAllPODetailsForApprove(asSchoolId.ToInt(), asUserId.ToInt(), asStatusId.ToInt());
        var result = new DataSourceResult()
        {
            Data = lstPODetailsForApprove,
            Total = lstPODetailsForApprove.Count
        };

        return result;
    }

    /// <summary>
    /// This event is used to Get query string.
    /// </summary>
    /// <param name="asPoId"></param>
    /// <param name="adtDate"></param>
    /// <param name="asRequesterId"></param>
    /// <param name="asStatusId"></param>
    [WebMethod]
    public static string GetQuerystring(string asPoId, string adtDate, string asRequesterId, string asStatusId)
    {
        return "PurchaseOrderListUI.aspx?" + CommonUtility.EncryptQuerystring("PoId=" + asPoId + "&Date=" + adtDate + "&RequesterId=" + asRequesterId + "&StatusId=" + asStatusId + "&IsFromApproverScreen=Y");
    }

    /// <summary>
    /// This event is used to Approve PO.
    /// </summary>
    /// <param name="asPoId"></param>
    /// <param name="asSchoolId"></param>
    /// <param name="asUserId"></param>    
    [WebMethod]
    public static void ApprovePurchaseOrder(string asPOId, string asSchoolId, string asUserId)
    {
        PurchaseOrderBL oPurchaseOrderBL = new PurchaseOrderBL();
        oPurchaseOrderBL.ApprovePurchaseOrder(asSchoolId.ToInt(), asPOId.ToInt(), asUserId.ToInt());
    }
    #endregion

    #region Private Method's

    /// <summary>
    /// This method is used to set java script attributes to controls..
    /// </summary>
    private void SetJavascriptAttributes()
    {        
        hidSchoolId.Value = miSchoolId.ToString();
        hidAcademicYearId.Value = miAcademicYearId.ToString();
        hidUserId.Value = miUserId.ToString();
        cmbStatus.Attributes.Add("onchange", "SetAttributes()");
    }

    /// <summary>
    /// This method is used to read query string.
    /// </summary>
    private void ReadQueryString()
    {
        if (QueryString["StatusId"] != null)
            cmbStatus.SelectedValue = QueryString["StatusId"];
    }

    #endregion
}