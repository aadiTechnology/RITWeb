using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Services;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Kendo.DynamicLinq;
using PayrollEntities;
using Utility;

public partial class SalaryPaymentPopup : SchoolBase
{
    #region Event(s)
    
    /// <summary>
    /// This event is sued to set mouse hover effect to buttons.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                base.ApplyMouseHoverEffect(new List<Button> { btnSave, btnClose });
                valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
                txtNumber.Focus();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    } 

    #endregion

    #region Public Method(s)

    /// <summary>
    /// This method will be used to return payment details.
    /// </summary>
    /// <param name="aiSchoolId"></param>
    /// <returns></returns>
    [WebMethod]
    public static DataSourceResult GetAllPaymentDetails(int aiSchoolId)
    {
        SalaryDetailsBL oSalaryDetailsBL = new SalaryDetailsBL(aiSchoolId, 0);
        List<SalaryPaymentDetails> lstPaymentDetails = oSalaryDetailsBL.GetAllPaymentDetails();

        var result = new DataSourceResult()
        {
            Data = lstPaymentDetails,
            Total = lstPaymentDetails.Count
        };

        return result;
    }

    /// <summary>
    /// This method is used to save payment details.
    /// </summary>
    /// <param name="asDate"></param>
    [WebMethod]
    public static void SavePaymentDetails(int abIsOnlineTransaction, string asTransactionNumber, int aiSchoolId, int aiMonthId, int aiYear)
    {
        SalaryDetailsBL oSalaryDetailsBL = new SalaryDetailsBL(aiSchoolId, 0);
        oSalaryDetailsBL.SavePaymentDetails(Convert.ToBoolean(abIsOnlineTransaction), asTransactionNumber, aiSchoolId, aiMonthId, aiYear);
    } 

    #endregion
}