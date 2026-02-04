using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SchoolEntities;
using BusinessLogic.Exceptions;
using System.Reflection;
using Utility;
using System.Data;

public partial class AssemblyQuestionConfigurationUI : SchoolBase
{
    #region Constant(s)

    private const string S_SAVE_MESSAGE = "Assembly question configuration details saved successfully !!!";
    private const string S_UPDATE_MESSAGE = "Assembly question configuration details updated successfully !!!";  

    #endregion

    #region Event(s)

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                FillAssemblyGroupDetails();
                FillAssemblyParentQuestion();

            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
           string sValue = Populate();
           if (sValue == Constants.S_ONE)
               base.DisplayMessage(S_SAVE_MESSAGE, false, tdMessage);
           else
               base.DisplayMessage(S_UPDATE_MESSAGE, true, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method(s)

    private void FillAssemblyGroupDetails()
    {
        AssemblyDetailsBL oAssemblyDetailsBL = new AssemblyDetailsBL(miSchoolId, miAcademicYearId, miUserId);
        DataTable oDT = oAssemblyDetailsBL.GetAllAssemblyQuestionsForConfiguration();
        ControlUtility.FillDropDownList(oDT, ref cmbGroups, "Id", "Name", Constants.S_SELECT);
    }

    private void FillAssemblyParentQuestion()
    {
        AssemblyDetailsBL oAssemblyDetailsBL = new AssemblyDetailsBL(miSchoolId, miAcademicYearId, miUserId);
        DataTable oDT = oAssemblyDetailsBL.GetAllAssemblyParentQuestions();
        ControlUtility.FillDropDownList(oDT, ref cmbParentQuestion, "ParentQueId", "ParentQueName", Constants.S_SELECT);
    }

    private string Populate()
    {
        string sVal = string.Empty;
        AssemblyDetailsBL oAssemblyDetailsBL = new AssemblyDetailsBL();

        return sVal;
    }

    #endregion
}