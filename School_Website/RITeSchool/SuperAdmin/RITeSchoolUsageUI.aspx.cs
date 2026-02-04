/* File Name - RITeSchoolUsageUI.aspx.cs
 * Created By - Sachin
 * Created Date - 20 May 2014
 * Description - This class is used to display RIT usage verification dates and usage details.
 */
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Services;
using BusinessLogic;
using Kendo.DynamicLinq;
using SchoolEntities;
using Utility;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;

public partial class RITeSchoolUsageUI : SchoolBase
{
    #region Event(s)

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if(!IsPostBack)
                base.ApplyMouseHoverEffect(new List<Button> { btnBack, btnSendMail });
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    } 

    #endregion

    /// <summary>
    /// This method is used to return execution dates.
    /// </summary>
    /// <param name="take"></param>
    /// <param name="skip"></param>
    /// <param name="sort"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [WebMethod]
    public static DataSourceResult GetAllExecutionDates(int take, int skip, IEnumerable<Sort> sort, Filter filter)
    {
        List<ExecutionDate> lstDates = RiteSchoolUsageBL.GetAllDates();

        if(sort != null)
        {
            if (sort.FirstOrDefault().Dir == "asc")
                lstDates = lstDates.OrderBy(dt => dt.Date).ToList();
            else
                lstDates = lstDates.OrderByDescending(dt => dt.Date).ToList();
        }
        else
            lstDates = lstDates.OrderByDescending(dt => dt.Date).ToList();
        
        List<ExecutionDate> lstFilteredDates = lstDates.Skip(skip).Take(take).ToList();

        var result = new DataSourceResult()
        {
            Data = lstFilteredDates,
            Total = lstDates.Count            
        };

        return result;
    }

    /// <summary>
    /// TYhis method is used to return all usage details.
    /// </summary>
    /// <param name="take"></param>
    /// <param name="skip"></param>
    /// <param name="sort"></param>
    /// <param name="filter"></param>
    /// <param name="asDate"></param>
    /// <returns></returns>
    [WebMethod]
    public static DataSourceResult GetAllUsageDetails(int take, int skip, IEnumerable<Sort> sort, Filter filter, string asDate)
    {
        int iStartINdex = skip + 1;
        int iEndIndex = skip + take;
        string sSortExpression = "QueryName";
        if (sort != null && sort.Count() > 0)
            sSortExpression = sort.FirstOrDefault().Field + " " + sort.FirstOrDefault().Dir;
        List<UsageDetails> lstUsage = RiteSchoolUsageBL.GetRitUsageDetails(iStartINdex, iEndIndex, sSortExpression, asDate);

        var result = new DataSourceResult()
        {
            Data = lstUsage,
            Total = (lstUsage.Count > 0? lstUsage[0].TotalRows : 0)
        };

        return result;
    }

    /// <summary>
    /// This method is used to send RIT usage mail.
    /// </summary>
    /// <param name="asDate"></param>
    [WebMethod]
    public static void SendRITUsageMail(string asDate)
    {
        DateTime dtExecutionDate = Convert.ToDateTime(asDate);
        string sFromEmailAddress = ConfigurationManager.AppSettings["FromMailAddress"];
        string sToEmailAddress = ConfigurationManager.AppSettings["EmailAddress"];
        string sMailBody = GetEmailBody(asDate);
        CommonUtility.SendE_Mail(sToEmailAddress, sFromEmailAddress, "RITeSchool Features Usage - " + dtExecutionDate.ToString("MMM") + " " + dtExecutionDate.ToString("yyyy"), sMailBody);
    }


    [WebMethod]
    public static void GenerateReport()
    {
        RiteSchoolUsageBL.GenerateReport();
    }

    /// <summary>
    /// This method is used to return mail body.
    /// </summary>
    /// <returns></returns>
    private static string GetEmailBody(string asDate)
    {
        List<UsageDetails> lstUsages = RiteSchoolUsageBL.GetRitUsageDetails(0, 9999, string.Empty, asDate);
        
        StringBuilder oStringBuilder = new StringBuilder();
        oStringBuilder.Append("<table cellpadding='1' cellspacing='1' " +
                      "frame='border' style='border: thin solid #00003E; background-color: #CEDBFF; font-family: Arial, Helvetica, sans-serif; font-size: small;width:600px;'> " +
                      "<tr><td align='left' colspan='3'><span style='font-style: italic'>" +
                       "Dear Sir / Ma’am,<BR /><BR />Following is the status of RITeSchool Features usage. For each feature, we have categorized status and any action required from your side. " +
                       "If you need any clarification or assistance while executing the activity, do get in touch with us." + "<BR /><BR />" +
                      "</span></td></tr>" +
                      "<tr style='border-spacing: 1px;'>" +
                        "<td align='left' style='padding-left:5px;border: thin solid #00003E;font-weight: bold;font-size: small;'>Feature</td>" +
                        "<td align='center' style='border-style: solid solid solid none; border-width: thin; border-color: #00003E;font-weight: bold;font-size: small;'>Status</td>" +
                        "<td align='left' style='border-style: solid solid solid none; border-width: thin; border-color: #00003E; padding-left:5px;font-weight: bold;font-size: small;'>Action Required from School</td>" +
                      "</tr>");

        lstUsages.ForEach
            (
                usg =>
                {
                    string sColor = string.Empty;
                    if (usg.Legend == "Ok")
                        sColor = "background-color: green";
                    else if (usg.Legend == "Little")
                        sColor = "background-color: red";
                    else if (usg.Legend == "Partial")
                        sColor = "background-color: Yellow";
                    else if (usg.Legend == "Almost")
                        sColor = "background-color: #AAFFAA";

                    oStringBuilder.Append("<tr>");
                    oStringBuilder.Append("<td align='left' style='border-style: none solid solid solid; border-width: thin; border-color: #00003E; padding-left:5px;'>" + usg.QueryName + "</td>");
                    oStringBuilder.Append("<td align='center' style='border-style: none solid solid none; border-width: thin; border-color: #00003E;font-weight: bold;color: #FFFFFF;" + sColor + "'>" + usg.Legend + "</td>");
                    oStringBuilder.Append("<td align='left' style='border-style: none solid solid none; border-width: thin; border-color: #00003E; padding-left:5px; background-color: #FFFFFF;'></td>");
                    oStringBuilder.Append("</tr>");
                }
            );

        oStringBuilder.Append("</table>");
        oStringBuilder.Append("<BR /><div style='float:left;font-family: Arial, Helvetica, sans-serif; font-size: small;'>Thanks.</div>");
        return oStringBuilder.ToString();
    }
}