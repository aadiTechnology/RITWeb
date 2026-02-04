using System;
using System.IO;
using System.Reflection;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using System.Threading;

public partial class DownloadFileUI : SchoolBase
{
    #region Data Member(s)

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (miSchoolId != 0)
            {
                if (QueryString["FileTypeId"] != null && QueryString["FileTypeId"].ToString() != string.Empty)
                {
                    int iFileTypeId = QueryString["FileTypeId"].ToInt();
                    int iAttachmentId = QueryString["AttachmentId"].ToInt();
                    if ((Constants.DownloadFileType)iFileTypeId == Constants.DownloadFileType.MessageCenterAttachment)
                    {
                        DownloadFileBL oDownloadFileBL = new DownloadFileBL(miSchoolId, miAcademicYearId, miUserId);
                        string sAttachmentFileName = oDownloadFileBL.GetFilePathAndName(iFileTypeId, iAttachmentId);

                        if (sAttachmentFileName != string.Empty)
                        {
                            int iIndex = sAttachmentFileName.LastIndexOf("/");
                            string sFileName = sAttachmentFileName.Substring(iIndex + 1);
                            string sActualFileName = sFileName;

                            if (sFileName.Contains("$") && sFileName.Length >= 15)
                            {
                                sActualFileName = sFileName.Substring(sFileName.LastIndexOf("$"), 15);
                                sActualFileName = sFileName.Replace(sActualFileName, string.Empty);
                            }

                            string filePath = Server.MapPath(sAttachmentFileName);

                            if (File.Exists(filePath))
                            {
                                Response.Clear();
                                Response.ContentType = GetMimeType(sFileName);
                                Response.AddHeader("Content-Disposition", "attachment; filename=" + sActualFileName);
                                Response.WriteFile(filePath);
                                Response.End();
                            }
                            else
                            {
                                Response.StatusCode = 404;
                                Response.End();
                            }
                        }
                        else
                        {
                            Response.StatusCode = 403;
                            Response.End();
                            return;
                        }
                    }
                    else
                    {
                        Response.StatusCode = 403;
                        Response.End();
                        return;
                    }
                }
                else
                {
                    Response.StatusCode = 403;
                    Response.End();
                    return;
                }
            }
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
            Response.StatusCode = 403;
            Response.End();
            return;
        }        
    }

    #endregion

    #region Method(s)

    private string GetMimeType(string fileName)
    {
        string ext = Path.GetExtension(fileName).ToLowerInvariant();

        switch (ext)
        {
            case ".txt": return "text/plain";
            case ".pdf": return "application/pdf";
            case ".doc": return "application/msword";
            case ".docx": return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            case ".xls": return "application/vnd.ms-excel";
            case ".xlsx": return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            case ".jpg":
            case ".jpeg": return "image/jpeg";
            case ".png": return "image/png";
            case ".gif": return "image/gif";
            case ".zip": return "application/zip";
            case ".rar": return "application/x-rar-compressed";
            default: return "application/octet-stream"; // generic binary
        }
    }

    #endregion
}