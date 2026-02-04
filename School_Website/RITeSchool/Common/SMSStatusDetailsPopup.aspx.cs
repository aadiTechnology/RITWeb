using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web.Services;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using Kendo.DynamicLinq;
using SchoolEntities.Admin;
using Utility;

public partial class SMSStatusDetailsPopup : SchoolBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                hidSMSShootId.Value = QueryString["SMSShootId"];
                base.ApplyMouseHoverEffect(new List<Button> { btnSearch, btnClose });
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to return sms status.
    /// </summary>
    /// <param name="take"></param>
    /// <param name="skip"></param>
    /// <param name="sort"></param>
    /// <param name="asSMSSentDate"></param>
    /// <param name="asFilter"></param>
    /// <returns></returns>
    [WebMethod]
    public static DataSourceResult GetSMSStatusDetails(int take, int skip, IEnumerable<Sort> sort, string asFilter, string asSMSShootId)
    {
        int iTotalCount;
        List<SMSDetails> lstDetails = FillSMSStatusDetails(out iTotalCount, take, skip, asFilter, asSMSShootId);

        var result = new DataSourceResult()
        {
            Data = lstDetails,
            Total = iTotalCount
        };

        return result;
    }

    /// <summary>
    /// This method is used to return sms status details.
    /// </summary>
    /// <param name="dtSMSSentDate"></param>
    /// <param name="aiTotalCount"></param>
    /// <param name="aiTake"></param>
    /// <param name="aiSkip"></param>
    /// <param name="asFilter"></param>
    /// <returns></returns>
    private static List<SMSDetails> FillSMSStatusDetails(out int aiTotalCount, int aiTake, int aiSkip, string asFilter, string asSMSShootId)
    {
        string sSendSMS = ConfigurationManager.AppSettings["SendSMS"];
        List<SMSDetails> lstSMSDetails = new List<SMSDetails>();
        aiTotalCount = 0;

        if (sSendSMS == Constants.S_YES)
        {
            asFilter = asFilter.ToLower();

            List<SMSDetails> lstAllSMSDetails = new List<SMSDetails>();
            WebRequest request = WebRequest.Create(ConfigurationManager.AppSettings["SMSSenderIPForSoftSMS"].ToString() + "/miscapi/" + ConfigurationManager.AppSettings["SMSSenderUPwdForSoftSMS"].ToString() + "/getDLR/" + asSMSShootId);
            request.Credentials = CredentialCache.DefaultCredentials;
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();

            string responseFromServer = string.Empty;
            using (StreamReader reader = new StreamReader(dataStream))
            {   
                responseFromServer = reader.ReadToEnd();
                reader.Close();
            }
            response.Close();
            
            responseFromServer =responseFromServer.Replace("{","").Replace("}","").Replace("\"","");
            string[] sArrDetails = responseFromServer.Split(',');

            foreach (string sStatus in sArrDetails)
            {
                string[] sArrStatus = sStatus.Split(':');
                SMSDetails oSMSDetails = new SMSDetails();
                oSMSDetails.MobileNos = sArrStatus[0];
                
                if(sArrStatus[1] == "1004")
                    oSMSDetails.DeliveryStatus = "Submitted";
                else if(sArrStatus[1] == "Delivered")
                    oSMSDetails.DeliveryStatus = "Delivered";
                else if(sArrStatus[1] == "33")
                    oSMSDetails.DeliveryStatus = "Failed";
                else
                    oSMSDetails.DeliveryStatus = "Unknown";

                if (oSMSDetails.MobileNos.Contains(asFilter) || oSMSDetails.DeliveryStatus.ToLower().Contains(asFilter.ToLower()))
                lstAllSMSDetails.Add(oSMSDetails);
            }

            aiTotalCount = lstAllSMSDetails.Count();

            lstAllSMSDetails = lstAllSMSDetails.OrderByDescending(sm => sm.SMSTime.ToDateTime()).ToList();

            lstSMSDetails = lstAllSMSDetails.Skip(aiSkip).Take(aiTake).ToList();
        }
        return lstSMSDetails;
    }
}