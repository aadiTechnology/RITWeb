/* File Name :- AssemblyListUI.aspx.cs
 * Created Date :- 15-Feb-2016
 * Class Description :- This class is used to display list of Assembly Details. 
 * Created By :- Dnyaneshwar Shinde.
 */

using System;
using System.Collections.Generic;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Kendo.DynamicLinq;
using SchoolEntities;
using Utility;
using System.Data.SqlClient;
using System.Web.Services;
using System.Web.UI.WebControls;

public partial class AssemblyListUI : SchoolBase
{
    #region Events

    /// <summary>
    /// This Event is used to set Javascript Attributes & values to hidden fields.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SetJavascriptAttributes();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This Event is used to Get Assembly Details List.
    /// </summary>
    /// <param name="aiSchoolId"></param>
    /// <param name="aiAcademivYearId></param>
    [WebMethod]
    public static DataSourceResult GetAllAssemblyList(int aiSchoolId, int aiAcademicYearId)
    {
        AssemblyDetailsBL oAssemblyDetailsBL = new AssemblyDetailsBL(aiSchoolId, aiAcademicYearId, 0);
        List<AssemblyDetails> lstAssemblyDetails = oAssemblyDetailsBL.GetAllAssemblyDetailsList();

        var Result = new DataSourceResult()
        {
            Data = lstAssemblyDetails,
            Total = lstAssemblyDetails.Count
        };
        return Result;
    }

    /// <summary>
    /// This event is used to delete Assembly Details.
    /// </summary>
    /// <param name="aiAssemblyId"></param>
    /// <param name="aiSchoolId"></param>
    /// <param name="aiAcademicYearId"></param>
    /// <param name="aiUserId"></param>
    [WebMethod]
    public static void DeleteAssembly(int aiAssemblyId, int aiSchoolId, int aiAcademicYearId, int aiUserId)
    {
        AssemblyDetailsBL oAssemblyDetailsBL = new AssemblyDetailsBL(aiSchoolId, aiAcademicYearId, aiUserId);
        oAssemblyDetailsBL.DeleteAssemblyDetails(aiAssemblyId);
    }

    /// <summary>
    /// This event is used to set Query String.
    /// </summary>
    /// <param name="aiAssemblyId"></param>
    /// <param name="aiSchoolId"></param>
    /// <param name="aiAcademicYearId"></param>
    /// <param name="aiUserId"></param>
    /// <param name="dtDate"></param>
    [WebMethod]
    public static string GetQuerystring(int aiAssemblyId, string adtDate)
    {
        return "AssemblyDetailsUI.aspx?" + CommonUtility.EncryptQuerystring("AssemblyId=" + aiAssemblyId + "&Date=" + adtDate);
    }

    #endregion

    #region Private Method(s)

    /// <summary>
    /// This method is used to set javascript attributes to controls.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnAdd });
        btnAdd.Attributes.Add("onclick", "if(!OpenAssembly()) return false;");
        hidSchoolId.Value = miSchoolId.ToString();
        hidAcademicYearId.Value = miAcademicYearId.ToString();
        hidUserId.Value = miUserId.ToString();
    }

    #endregion
}