/*File Name - SMSStatusPopup.aspx.cs
 * Created By - Sachin
 * Created Date - 21-Oct-2015
 * Description - This class is used to display sent SMS status.
 */

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using Kendo.DynamicLinq;
using Newtonsoft.Json.Linq;
using SchoolEntities.Admin;
using Utility;

public partial class SMSStatusPopup : SchoolBase
{
    /// <summary>
    /// This event is used o set java script status.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                GenerateJavascriptAttributes();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used t set java script attributes.
    /// </summary>
    private void GenerateJavascriptAttributes()
    {
        txtSMSDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        btnSearch.Attributes.Add("onclick", "LoadSmsStatus();");
        base.ApplyMouseHoverEffect(new List<Button> { btnSearch, btnClose });
    }

    //private static List<SMSDetails> FillSMSStatusDetails(DateTime dtSMSSentDate, out int aiTotalCount, int aiTake, int aiSkip, string asFilter)
    //{
    //    BSSentItemsSoapClient oClient = new BSSentItemsSoapClient();
    //    string sSendSMS = ConfigurationManager.AppSettings["SendSMS"];

    //    List<SMSDetails> lstSMSDetails = new List<SMSDetails>();

    //    aiTotalCount = 0;

    //    //if (sSendSMS == Constants.S_YES)
    //    {
    //        string sSMSSenderUName = ConfigurationManager.AppSettings["SMSSenderUName"];
    //        string sSMSSenderUPwd = ConfigurationManager.AppSettings["SMSSenderUPwd"];

    //        XElement element = oClient.BSSentItems("thesteppingstonepune@regulusit.net", "ss2oo*", dtSMSSentDate);
    //        //XElement element = oClient.BSSentItems(sSMSSenderUName, sSMSSenderUPwd, DateTime.Now.Date);

    //        var lstItems = from item in element.Descendants("SentItem")
    //                       where item.Element("SMSText").ToString().ToLower().Contains(asFilter) ||
    //                       item.Element("DeliveryStatus").ToString().ToLower().Contains(asFilter) ||
    //                       item.Element("MobileNos").ToString().ToLower().Contains(asFilter)
    //                       orderby ((DateTime)item.Element("SMS_Time")) descending
    //                       select item;

    //        asFilter = asFilter.ToLower();

    //        if (lstItems.Count() > 0)
    //        {
    //            aiTotalCount = lstItems.Count();

    //            var lstFilteredItems = lstItems.Skip(aiSkip).Take(aiTake);

    //            foreach (XElement item in lstFilteredItems)
    //            {
    //                SMSDetails oSMSDetails = new SMSDetails
    //                {
    //                    SMSTime = ((DateTime)item.Element("SMS_Time")).ToString("hh:mm:ss tt"),
    //                    SMSText = (string)item.Element("SMSText"),
    //                    DeliveryStatus = (string)item.Element("DeliveryStatus"),
    //                    MobileNos = (string)item.Element("MobileNos"),
    //                    TotalSMS = (int)item.Element("TotalSMS")
    //                };
    //                lstSMSDetails.Add(oSMSDetails);
    //            }

    //            lstSMSDetails.Where(sms => sms.SMSText.Contains("Password") && sms.SMSText.Contains("User Name")).ToList().ForEach(
    //                    sd =>
    //                    {
    //                        int iIndex = sd.SMSText.IndexOf("Password=");
    //                        int iLastINdex = sd.SMSText.IndexOf("\"",iIndex+10);
    //                        StringBuilder obj = new StringBuilder();
    //                        obj.Append(sd.SMSText);
    //                        obj.Remove(iIndex + 10, iLastINdex - iIndex - 10);
    //                        obj.Insert(iIndex + 10,"*", iLastINdex - iIndex - 10);
    //                        sd.SMSText = obj.ToString();
    //                    }
    //                );

    //            lstSMSDetails = lstSMSDetails.OrderBy(sm => sm.SMSTime.ToDateTime()).ToList();
    //        }
    //    }
    //    return lstSMSDetails;
    //}

    /// <summary>
    /// This method is used to return sms status details.
    /// </summary>
    /// <param name="dtSMSSentDate"></param>
    /// <param name="aiTotalCount"></param>
    /// <param name="aiTake"></param>
    /// <param name="aiSkip"></param>
    /// <param name="asFilter"></param>
    /// <returns></returns>
    private static List<SMSDetails> FillSMSStatusDetails(DateTime dtSMSSentDate, out int aiTotalCount, int aiTake, int aiSkip, string asFilter)
    {
        string sSendSMS = ConfigurationManager.AppSettings["SendSMS"];
        List<SMSDetails> lstSMSDetails = new List<SMSDetails>();
        aiTotalCount = 0;

        if (sSendSMS == Constants.S_YES)
        {
            if (Settings.SMSProviderForWebsite.ToLower() == Constants.SMSProviders.BusinessSMS.ToString().ToLower())
            {
                string sSMSSenderUName = ConfigurationManager.AppSettings["SMSSenderUName"];
                string sSMSSenderUPwd = ConfigurationManager.AppSettings["SMSSenderUPwd"];

                List<SMSDetails> lstAllSMSDetails = new List<SMSDetails>();
                List<SMSDetails> lstToFilterSMSDetails = new List<SMSDetails>();

                string sToken = string.Empty;                
                HttpWebRequest oRequestForToken = (HttpWebRequest)WebRequest.Create("https://messaging.charteredinfo.com/AuthTokenV1/AuthToken?UserId=" + sSMSSenderUName + "&Password=" + sSMSSenderUPwd);

                oRequestForToken.Method = "GET";
                WebResponse oWebResponseForToken = oRequestForToken.GetResponse();
                Stream oResponseMessageForToken = oWebResponseForToken.GetResponseStream();
                using (StreamReader oStreamReaderForToken = new StreamReader(oResponseMessageForToken))
                {
                    string sTokenData = oStreamReaderForToken.ReadToEnd();
                    dynamic data = JObject.Parse(sTokenData);
                    sToken = data.TxnOutcome;
                }

                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;                
                HttpWebRequest oRequest = (HttpWebRequest)WebRequest.Create("https://messaging.charteredinfo.com/TxnLogV1/SentItems?ForDate=" + dtSMSSentDate.ToString("dd-MM-yyyy") + "&StartLogID=0");
                oRequest.Headers.Add("Authorization", "Bearer " + sToken);
                oRequest.Method = "GET";
                WebResponse oWebResponse = oRequest.GetResponse();
                Stream oResponseMessage = oWebResponse.GetResponseStream();
                using (StreamReader oStreamReader = new StreamReader(oResponseMessage))
                {
                    string sData = oStreamReader.ReadToEnd();
                    dynamic jData = JObject.Parse(sData);
                    var serializer = new JavaScriptSerializer();
                    lstToFilterSMSDetails = serializer.Deserialize<List<SMSDetails>>(jData.TxnOutcome.Value);                    
                }

                asFilter = asFilter.ToLower();
                lstAllSMSDetails = lstToFilterSMSDetails.Where(smsTxt => smsTxt.SMSText.ToLower().Contains(asFilter) || smsTxt.DeliveryStatus.ToLower().Contains(asFilter) || smsTxt.MobileNos.ToLower().Contains(asFilter)).ToList();

                //BSSentItemsSoapClient oClient = new BSSentItemsSoapClient();

                //XmlElement element = oClient.BSSentItem(sSMSSenderUName, sSMSSenderUPwd, dtSMSSentDate);
                
                //string sResult = "<SMSResult>" + element.InnerXml + "</SMSResult>";
                //XmlDocument doc = new XmlDocument();
                //doc.LoadXml(sResult);

                //foreach (XmlNode node in doc.SelectNodes("/SMSResult/SentItem"))
                //{
                //    XmlDocument newDoc = new XmlDocument();
                //    string sXML = "<Result>"+node.InnerXml+"</Result>";
                //    newDoc.LoadXml(sXML);

                //    string sSMSTime = newDoc.SelectSingleNode("/Result/SMS_Time").InnerText;
                //    string sDeliveryStatus = newDoc.SelectSingleNode("/Result/DeliveryStatus").InnerText;
                //    string sMobileNos = newDoc.SelectSingleNode("/Result/MobileNos").InnerText;
                //    string sSMSText = newDoc.SelectSingleNode("/Result/SMSText").InnerText;
                //    int iTotalSMS = newDoc.SelectSingleNode("/Result/TotalSMS").InnerText.ToInt();

                //    if (sSMSTime.ToLower().Contains(asFilter) || sDeliveryStatus.ToLower().Contains(asFilter) || sMobileNos.ToLower().Contains(asFilter))
                //    {
                //        SMSDetails oSMSDetails = new SMSDetails
                //        {
                //            SMSTime = (Convert.ToDateTime(sSMSTime)).ToString("hh:mm:ss tt"),
                //            SMSText = sSMSText,
                //            DeliveryStatus = sDeliveryStatus,
                //            MobileNos = sMobileNos,
                //            TotalSMS = iTotalSMS
                //        };
                //        lstAllSMSDetails.Add(oSMSDetails);
                //    }
                //}

                //foreach (var el in element)
                //{
                //    string sSMSTime = element.ChildNodes[iElementIndex].ChildNodes[1].InnerText;
                //    string sDeliveryStatus = element.ChildNodes[iElementIndex].ChildNodes[5].InnerText;
                //    string sMobileNos = element.ChildNodes[iElementIndex].ChildNodes[2].InnerText;

                //    if (sSMSTime.ToLower().Contains(asFilter) || sDeliveryStatus.ToLower().Contains(asFilter) || sMobileNos.ToLower().Contains(asFilter))
                //    {
                //        SMSDetails oSMSDetails = new SMSDetails
                //            {
                //                SMSTime = (Convert.ToDateTime(sSMSTime)).ToString("hh:mm:ss tt"),
                //                SMSText = (string)element.ChildNodes[iElementIndex].ChildNodes[3].InnerText,
                //                DeliveryStatus = sDeliveryStatus,
                //                MobileNos = sMobileNos,
                //                TotalSMS = Convert.ToInt32(element.ChildNodes[iElementIndex].ChildNodes[4].InnerText)
                //            };
                //        lstAllSMSDetails.Add(oSMSDetails);
                //    }

                //    iElementIndex++;
                //}

                aiTotalCount = lstAllSMSDetails.Count();

                lstAllSMSDetails = lstAllSMSDetails.OrderByDescending(sm => sm.SMSTime.ToDateTime()).ToList();

                lstSMSDetails = lstAllSMSDetails.Skip(aiSkip).Take(aiTake).ToList();

                lstSMSDetails.Where(sms => sms.SMSText.Contains("Password") && sms.SMSText.Contains("User Name")).ToList().ForEach(
                        sd =>
                        {
                            int iIndex = sd.SMSText.IndexOf("Password=");
                            int iLastINdex = sd.SMSText.IndexOf(" ", iIndex + 10);
                            StringBuilder obj = new StringBuilder();
                            obj.Append(sd.SMSText);
                            obj.Remove(iIndex + 9, iLastINdex - iIndex - 9);
                            obj.Insert(iIndex + 9, "*", iLastINdex - iIndex - 9);
                            sd.SMSText = obj.ToString();
                        }
                    );

                lstSMSDetails.Where(sms => sms.SMSText.Contains("Salary for the month ") && sms.SMSText.Contains("deposited")).ToList().ForEach(
                        sd =>
                        {
                            int iIndex = sd.SMSText.IndexOf("Rs.");
                            int iLastIndex = sd.SMSText.IndexOf("has been deposited");
                            if (iLastIndex == -1)
                                iLastIndex = sd.SMSText.IndexOf("will be deposited");

                            StringBuilder obj = new StringBuilder();
                            obj.Append(sd.SMSText);
                            obj.Remove(iIndex + 3, (iLastIndex - iIndex - 3));
                            obj.Insert(iIndex + 3, "*", (iLastIndex - iIndex - 3));
                            sd.SMSText = obj.ToString();
                        }
                    );

                lstSMSDetails.Where(sms => sms.SMSText.Contains("Salary Details - Basic") && sms.SMSText.Contains("Gross Salary")).ToList().ForEach(
                        sd =>
                        {
                            int iIndex = sd.SMSText.IndexOf("Salary Details - ");
                            int iLastINdex = sd.SMSText.Length;
                            StringBuilder obj = new StringBuilder();
                            obj.Append(sd.SMSText);
                            obj.Remove(iIndex + 24, (iLastINdex - iIndex - 24));
                            obj.Insert(iIndex + 24, "*", (iLastINdex - iIndex - 24));
                            sd.SMSText = obj.ToString();
                        }
                    );

                lstSMSDetails.Where(sms => sms.SMSText.Contains("Salary for the month ") && sms.SMSText.Contains("Attendance")).ToList().ForEach(
                        sd =>
                        {
                            int iIndex = sd.SMSText.IndexOf("Attendance");
                            int iLastINdex = sd.SMSText.Length;
                            StringBuilder obj = new StringBuilder();
                            obj.Append(sd.SMSText);
                            obj.Remove(iIndex + 12, (iLastINdex - iIndex - 12));
                            obj.Insert(iIndex + 12, "*", (iLastINdex - iIndex - 12));
                            sd.SMSText = obj.ToString();
                        }
                    );
            }
        }
        return lstSMSDetails;
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
    public static DataSourceResult GetSMSStatusDetails(int take, int skip, IEnumerable<Sort> sort, string asSMSSentDate, string asFilter)
    {
        int iTotalCount;
        List<SMSDetails> lstDetails = FillSMSStatusDetails(asSMSSentDate.ToDateTime(), out iTotalCount, take, skip, asFilter);

        var result = new DataSourceResult()
        {
            Data = lstDetails,
            Total = iTotalCount
        };

        return result;
    }
}