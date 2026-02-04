/*File Name - FormNo16ReportUI.aspx.cs
* Created Date - 21 Feb 2013
* Created By - Sachin
* Description - This class is used to open income tax details report.
* 
* Modified Date - 23 Nov 2013
* Modified By - Sachin
* Modification Reason - This class is modified for adding a new methods to calculate income tax amount and pass it to caller page.
*/

using System;
using System.Reflection;
using System.Threading;
using BusinessLogic;
using BusinessLogic.Exceptions;
using CrystalDecisions.Shared;
using Utility;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public partial class FormNo16ReportUI : SchoolBase
{
    #region Event(s)

    /// <summary>
    /// This event is used to calculate income tax amount / open income tax stateent report.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {   
                if (QueryString["ShowReport"] == Constants.S_YES)
                    DisplayReport();
                else
                    CalculateIncomeTaxAmount();
            }
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    } 

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to display report.
    /// </summary>
    private void DisplayReport()
    {
        ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.FormNo16Report, GetFilterString(), ExportFormatType.PortableDocFormat);
        oReportDisplay.DisplayReport();
    }

    /// <summary>
    /// This method is used to return filter string.
    /// </summary>
    /// <returns></returns>
    private string GetFilterString()
    {
        int iSelectedUserId = Convert.ToInt32(QueryString["UserId"]);
        string sRecordSelectionFormula = "(usp_GetIncomeTaxDetailsForReort.School_Id}=" + miSchoolId + " AND  usp_GetIncomeTaxDetailsForReort.Academic_Year_Id} =" + miAcademicYearId + " AND usp_GetIncomeTaxDetailsForReort.FinancialYearId}=" + miFinancialYearId +
       " AND  usp_GetIncomeTaxDetailsForReort.StaffGroupsId} = null AND  usp_GetIncomeTaxDetailsForReort.HasFullAccess} = 1 AND usp_GetIncomeTaxDetailsForReort.UserId}=" + iSelectedUserId + ")" + "@ ";
        return sRecordSelectionFormula;
    }

    /// <summary>
    /// This method is used to calculate income tax amount.
    /// </summary>
    private void CalculateIncomeTaxAmount()
    {   
        string sUserIds = Convert.ToString(QueryString["UserId"]);
        
        IncomeTaxDetailsBL oIncomeTaxDetailsBL = new IncomeTaxDetailsBL(miSchoolId, miFinancialYearId, miUserId, miAcademicYearId);
        hidITAmount.Value = oIncomeTaxDetailsBL.GetIncomeTaxAmount(sUserIds);
           
        if (hidITAmount.Value.StartsWith(","))
            hidITAmount.Value = hidITAmount.Value.Substring(1);
        hisIsForSingle.Value = QueryString["IsForSingle"].ToString();
    } 

    #endregion
}