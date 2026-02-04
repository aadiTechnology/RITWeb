<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using BarCode39Lib;

public class Handler : IHttpHandler
{   
    public void ProcessRequest(HttpContext context)
    {
        string sCode = context.Request.QueryString["id"].ToString();

        BarCode39 oBarCode39 = new BarCode39(sCode);
        Bitmap imgCode = oBarCode39.GenerateBarCode();    
      
        MemoryStream ms = new MemoryStream();
        imgCode.Save(ms, ImageFormat.Png);
        
        context.Response.ContentType = "image/bmp";
        context.Response.BinaryWrite(ms.GetBuffer());
        
     
        ms.Dispose();
        imgCode.Dispose();
        context.Response.Flush();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}