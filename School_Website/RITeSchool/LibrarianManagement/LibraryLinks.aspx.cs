using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using Utility;
public partial class LibraryLinks : SchoolBase
{
    protected void Page_Load(object sender, EventArgs e)
    {


        try
        {

            if (!IsPostBack)
            {
                hlnkStudentLibrary.NavigateUrl = Settings.ExternalLibrarySite;
                hlnkStudentLibrary.Target = "_blank";

                hlnkExternalLibrary.NavigateUrl = "https://drive.google.com/folderview?id=1fTRvhiriVwY-dP4m2uxQDi0AE_XD8vXK";
                hlnkExternalLibrary.Target = "_blank";
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }

    }
}