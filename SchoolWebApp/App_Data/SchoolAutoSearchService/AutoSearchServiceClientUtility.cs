using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SchoolAutoSearchService.Client;
using BusinessLogic.Exceptions;
using System.Reflection;
using System.ServiceModel;

/// <summary>
/// Summary description for SchoolAutoSearchServiceClientUtility
/// </summary>
public class AutoSearchServiceClientUtility
{
    public AutoSearchServiceClientUtility()
	{	
	}

    public static void RefreshStudentCache(string asFilter)
    {
        AutoSearchServiceClient oAutoSearchServiceClient = new AutoSearchServiceClient();
        try
        {
            oAutoSearchServiceClient.Open();
            oAutoSearchServiceClient.RefreshStudentCache(asFilter);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
        finally
        {
            if (oAutoSearchServiceClient.State != CommunicationState.Faulted)
                oAutoSearchServiceClient.Close();
        }
    }
}