<%@ WebHandler Language="C#" Class="HandleExport" %>

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using SchoolEntities;
using Utility;
using System.Linq;

public class HandleExport : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        if (context.Request.QueryString.Count == 0)
            return;

        string sDecryptQuerystring = CommonUtility.DecryptQuerystring(context.Request.QueryString.ToString().Replace("%2f", "/").Replace("%20", "+").Replace("%3d", "="));

        var oQueryParams = sDecryptQuerystring.Split('&').Select(x => x.Split('=')).ToDictionary(x => x[0], x => x[1]);

        if (oQueryParams.Count > 0 && oQueryParams["ReportId"].ToString() == ExportReportNames.LeaveReportExport.ToInt().ToString())
        {
            string sStartDate = oQueryParams["StartDate"];
            string sEndDate = oQueryParams["EndDate"];
            string sReportId = oQueryParams["ReportId"];
            string sStaffGroupsId = oQueryParams["StaffGroupsId"];
            string sUserId = oQueryParams["UserId"];
            string sSchoolId = oQueryParams["SchoolId"];
            string sAcademicYearId = oQueryParams["AcademicYearId"];
            string sLoginUserId = oQueryParams["LoginUserId"];
            string sExportFormatType = oQueryParams["ExportFormatType"];

            ExcelRequest requestData = new ExcelRequest();
            requestData.aiSchoolId = sSchoolId.ToInt();
            requestData.aiAcademicYearId = sAcademicYearId.ToInt();
            requestData.aiLoginUserId = sLoginUserId.ToInt();
            requestData.aiReportId = sReportId.ToInt();
            requestData.aiExportFormatType = sExportFormatType.ToInt(); //5 For Excel

            requestData.aoParameterPairs = new ParameterPair[]
                {
                    new ParameterPair {Name = "School_Id", Value = sSchoolId},
                    new ParameterPair {Name = "Academic_Year_Id", Value = sAcademicYearId},
                    new ParameterPair {Name = "StartDate", Value = sStartDate},
                    new ParameterPair {Name = "EndDate", Value = sEndDate},
                    new ParameterPair {Name = "StaffGroupsId",Value = sStaffGroupsId},
                    new ParameterPair {Name = "UserId",Value = sUserId}
                };

            ExportToExcel(context, requestData, sSchoolId.ToInt());
        }
        else
            return;
    }

    private static void ExportToExcel(HttpContext context, ExcelRequest requestData, int aiSchoolId)
    {
        try
        {
            BusinessLogic.SchoolBL oSchoolBL = new BusinessLogic.SchoolBL(aiSchoolId);
            string sURL = oSchoolBL.GetSchoolSettingByName(aiSchoolId, "NewUIAPIURL");

            if (!string.IsNullOrEmpty(sURL))
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string jsonRequest = serializer.Serialize(requestData);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sURL);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                request.Timeout = 120000;

                byte[] requestBytes = Encoding.UTF8.GetBytes(jsonRequest);
                request.ContentLength = requestBytes.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(requestBytes, 0, requestBytes.Length);
                }

                string responseJson = "";

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            responseJson = reader.ReadToEnd();
                        }
                    }
                }

                ExcelResponse apiResponse = serializer.Deserialize<ExcelResponse>(responseJson);

                if (apiResponse == null)
                    throw new Exception("Invalid response received from API.");

                if (!apiResponse.Success)
                    throw new Exception("API Error: " + apiResponse.ErrorMessage);

                if (string.IsNullOrEmpty(apiResponse.FileContent))
                    throw new Exception("API returned empty FileContent.");

                byte[] fileBytes;

                try
                {
                    fileBytes = Convert.FromBase64String(apiResponse.FileContent);
                }
                catch (FormatException)
                {
                    throw new Exception("FileContent returned by API is not valid Base64.");
                }

                if (fileBytes == null || fileBytes.Length == 0)
                    throw new Exception("Excel file contains no data.");

                string fileName = apiResponse.FileName;
                if (string.IsNullOrEmpty(fileName))
                    fileName = "Report.xlsx";

                string contentType = apiResponse.ContentType;
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                context.Response.Clear();
                context.Response.ClearHeaders();
                context.Response.ClearContent();
                context.Response.Buffer = true;
                context.Response.ContentType = contentType;
                context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
                context.Response.AddHeader("Content-Length", fileBytes.Length.ToString());
                context.Response.OutputStream.Write(fileBytes, 0, fileBytes.Length);
                context.Response.Flush();
                context.Response.SuppressContent = true;
                context.ApplicationInstance.CompleteRequest();
            }
        }
        catch (Exception ex)
        {
            context.Response.Clear();
            context.Response.ContentType = "text/plain";
            context.Response.Write("Excel download failed: " + ex.ToString());
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}

public class ExcelRequest
{
    public int aiSchoolId { get; set; }
    public int aiAcademicYearId { get; set; }
    public int aiLoginUserId { get; set; }
    public int aiReportId { get; set; }
    public ParameterPair[] aoParameterPairs { get; set; }
    public int aiExportFormatType { get; set; }
}

public class ExcelResponse
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
    public string FileContent { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
}

public enum ExportReportNames
{
    LeaveReportExport = 332
}