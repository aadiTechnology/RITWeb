using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using AccountsEntities;
using BusinessLogic.Exceptions;
using SchoolBusinessService;
using Utility;
using BusinessLogic;
using DataCommunicator;
using System.Data;

public partial class NextFinancialYearUI : SchoolBase
{
	#region "Members"
	
	private AccountsBaseClient moAccountsBaseClient;
	private FinancialYearBL moFinancialYearBL=new FinancialYearBL();

	#endregion

	#region

	/// <summary>
	/// This method is used to initialize the controls.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
		try
		{			
			if (Settings.EnableAccountsModule)
				InitializeGroupServiceObj();

            if (!IsPostBack)
            {
                ApplyMouseHoverEffect(new List<Button>() { btnCancel, btnCreate, btnOk });
                CheckFinancialYear();
            }			
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
		}
    }

	/// <summary>
	/// This method is used to create financial year.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnCreate_Click(object sender, EventArgs e)
	{
		try
		{
            if (moFinancialYearBL.CreateFinancialYear(miSchoolId, Session[Constants.S_SESSION_USER_ID].ToInt(), chkMarkAsCurrent.Checked))
            {
                trSuccess.Visible = true;
                trControls.Visible = false;
            }
            if (Settings.EnableAccountsModule)
                moAccountsBaseClient.RebuildCache();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
		}
	}

	#endregion

	#region "Private Method"

	/// <summary>
	/// Initializes the Group service object.
	/// </summary>
	private void InitializeGroupServiceObj()
	{
		moAccountsBaseClient = new AccountsBaseClient();
		moAccountsBaseClient.Open();
	}

	/// <summary>
	/// This method is used to check whether next year exists or not.
	/// </summary>
	/// <returns></returns>
	private void CheckFinancialYear()
	{
		List<FinancialYear> oLstFinancialYears = moFinancialYearBL.GetAllFinancialYears(miSchoolId);
		FinancialYear oCurrentYear = oLstFinancialYears.FirstOrDefault(fy => fy.SchoolId == miSchoolId && fy.IsCurrent);		
        FinancialYear oNewYear = oLstFinancialYears.FirstOrDefault(newFy => newFy.SchoolId == miSchoolId && newFy.StartDate.Year == oCurrentYear.EndDate.Year);

        if (oNewYear == null)
        {
            trNew.Visible = true;
            trExist.Visible = false;
            spnNewYear.InnerText = oCurrentYear.EndDate.Year + " - " + oCurrentYear.EndDate.AddYears(1).Year;
        }
        else
        {
            trNew.Visible = false; ;
            trExist.Visible = true;
            spnYear.InnerText = oNewYear.StartDate.Year + " - " + oNewYear.EndDate.Year;
        }			
	}
    
	#endregion
}