using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Data;
using BusinessLogic;
using Utility;
using System.IO;
using SchoolEntities;
using System.Data.SqlClient;
using DataCommunicator;

public partial class PlayVideoPopup : SchoolBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                VideoGalleryBL moVideoGalleryBL = new VideoGalleryBL();
                int iVideoDetailsId = QueryString["VideoDetailsId"].ToInt();
                srcMedia.Attributes["src"] = moVideoGalleryBL.ReadData(iVideoDetailsId);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
}