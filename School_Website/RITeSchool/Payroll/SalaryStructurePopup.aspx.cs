/* File Name -  SalaryStructurePopup.aspx.cs
 * Created By - Sachin
 * Created Date - 14 Feb 2014
 * Description - This class is used to display salary structure of user.
 */
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using Utility;

public partial class SalaryStructurePopup : SchoolBase
{
    #region Data Member(s)
    
    SalaryDetailsBL moSalaryDetailsBL; 

    #endregion

    #region Event(s)

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moSalaryDetailsBL = new SalaryDetailsBL(miSchoolId, miAcademicYearId);
            if (!IsPostBack)
            {
                FillSalaryStructure();
                lblUserName.Text = QueryString["UserName"].ToString();
                imgPhoto.Src = Constants.S_IMAGE_GENERATOR_PATH + "Value=" + QueryString["UserId"].ToString();
                ApplyMouseHoverEffect(new List<Button> { btnClose });
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwEarningsDeductions_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                Label lblShortName = e.Item.FindControl("lblShortName") as Label;
                Label lblAmount = e.Item.FindControl("lblAmount") as Label;
                UsersEarningsDeduction oUsersEarningsDeduction = e.Item.DataItem as UsersEarningsDeduction;
                lblShortName.Text = oUsersEarningsDeduction.EarningsDeductionsId < 0 ? lblShortName.Text : (oUsersEarningsDeduction.IsEarning ? "(+) " : "(-) ") + lblShortName.Text;

                if (oUsersEarningsDeduction.ShortName == string.Empty)
                {
                    lblShortName.Text = string.Empty;
                    lblAmount.Text = string.Empty;
                }

                if (oUsersEarningsDeduction.EarningsDeductionsId < 0)
                {
                    lblShortName.Font.Bold = true;
                    lblAmount.Font.Bold = true;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    } 

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to fill up salary structure list view.
    /// </summary>
    private void FillSalaryStructure()
    {
        int iUserId = Convert.ToInt32(QueryString["UserId"]);
        List<UsersEarningsDeduction> UsersEDs = moSalaryDetailsBL.GetSalaryStructureOfUser(iUserId);
        lstvwEarningsDeductions.DataSource = UsersEDs;
        lstvwEarningsDeductions.DataBind();
    } 

    #endregion
}