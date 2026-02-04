/* File Name - SalaryChangesUI.aspx.cs
 * Created By - Sachin
 * Created Date - 11-Aug-2018
 * Description - This class is used to export salary incremnt details.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using Utility;

public partial class SalaryChangesUI : SchoolBase
{
    #region Data Member(s)

    private SalaryDetailsBL moSalaryDetailsBL;
    private List<SalaryChange> mlstSalaryChanges; 

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to fill staff groups.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moSalaryDetailsBL = new SalaryDetailsBL(miSchoolId, miAcademicYearId);
            if (!IsPostBack)
            {
                FillStaffGroups();
                SetJavascriptAttributes();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill user combo.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStaffGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillUsers();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is sued to export salary details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=SalaryRecord-" + cmbUser.SelectedItem.Text.Replace(" ", "") + ".xls");
            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            HttpContext.Current.Response.Write("<BR><BR><BR>");

            StringBuilder obj = new StringBuilder();

            obj.Append("<Table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:15px; font-family:Calibri; background:white;'>");

            mlstSalaryChanges = moSalaryDetailsBL.GetAllSalaryChanges(cmbUser.SelectedValue.ToInt());

            obj.Append(AddRow((mlstSalaryChanges.Count + 1), cmbUser.SelectedItem.Text, "center"));
            obj.Append(AddRow((mlstSalaryChanges.Count + 1), "Salary Record", "left"));

            obj.Append(AddColumns());

            obj.Append(AddEarnings("Basic", 1));
            obj.Append(AddEarnings("G.P.", 2));
            obj.Append(AddEarnings("D.A.", 3));
            obj.Append(AddEarnings("D.A. Percentage", 4));
            obj.Append(AddEarnings("H.R.A.", 5));
            obj.Append(AddEarnings("H.R.A. Percentage", 6));
            obj.Append(AddEarnings("T.A.", 7));
            
            obj.Append(AddTotalRow());

            obj.Append(AddEarnings("R.A.", 8));

            obj.Append("<TR></TR>");

            int iCount = moSalaryDetailsBL.PaidSalaryDifferences.Select(sd => new { sd.Year, sd.MonthId }).Distinct().Count();
            obj.Append(AddRow((iCount + 1), "Monthwise Salary Difference", "left"));
            obj.Append(AddSalDiffColumns());
            obj.Append(AddSalaryDifference());
            obj.Append(AddSalaryDifferenceTotals());

            obj.Append("<TR></TR>");
            int iPaidCount = moSalaryDetailsBL.SalaryDifferencePaidDetails.Select(sd => new { sd.PaidYear, sd.PaidMonthId }).Distinct().Count();
            obj.Append(AddRow((iPaidCount + 1), "Monthwise Paid Salary Difference", "left"));
            obj.Append(AddPaidSalDiffColumns());
            obj.Append(AddPaidSalaryDifference());
            obj.Append(AddPaidSalaryDifferenceTotals());

            obj.Append("</Table>");
            obj.Append("</font>");

            HttpContext.Current.Response.Write(obj.ToString());
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    } 

    #endregion

    #region Method(s)

    /// <summary>
    /// Tihs method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnExport });
        ValSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
    }

    /// <summary>
    /// This method is used to fill up user combo box.
    /// </summary>
    private void FillUsers()
    {   
        SalaryDetailsBL oSalaryDetailsBL = new SalaryDetailsBL(miSchoolId, miAcademicYearId);
        List<UserBasicDetails> lstUserBasicDetails = oSalaryDetailsBL.GetUsers(cmbStaffGroup.SelectedValue.ToInt());
        ListSource.FillDropDownList(lstUserBasicDetails, cmbUser, "StaffName", "UserId", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill staff groups.
    /// </summary>
    private void FillStaffGroups()
    {
        SalaryDetailsBL oSalaryDetailsBL = new SalaryDetailsBL();
        oSalaryDetailsBL.GetStaffGroupsAndMonths(miSchoolId, miAcademicYearId);
        ListSource.FillDropDownList(oSalaryDetailsBL.SalaryEntityLists.lstStaffGroups, cmbStaffGroup, "StaffGroupsName", "StaffGroupsId", Constants.S_SELECT);
        FillUsers();
    }

    /// <summary>
    /// This method is used to add new row with given caption.
    /// </summary>
    /// <param name="aiColsSpan"></param>
    /// <param name="asText"></param>
    /// <param name="asAlign"></param>
    /// <returns></returns>
    private string AddRow(int aiColsSpan, string asText, string asAlign)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<TR>");
        sb.Append("<TD align='" + asAlign + "' colspan='" + aiColsSpan + "'>");
        sb.Append("<B>" + asText + "</B>");
        sb.Append("</TD>");
        sb.Append("</TR>");
        return sb.ToString();
    }

    /// <summary>
    /// This method is used to add salary difference total row.
    /// </summary>
    /// <returns></returns>
    private string AddSalaryDifferenceTotals()
    {
        StringBuilder sb = new StringBuilder();
        string sStyle = " style='background-color:#C697F8'";
        sb.Append("<TR>");
        sb.Append("<TD " + sStyle + ">");
        sb.Append("<B>Total</B>");
        sb.Append("</TD>");

        moSalaryDetailsBL.PaidSalaryDifferences.Select(sd => new { sd.Year, sd.MonthId, sd.MonthName }).Distinct().OrderBy(sd => sd.Year).ThenBy(sd => sd.MonthId).ToList().ForEach(
            sal =>
            {
                sb.Append("<TD style='background-color:#C697F8;width:75px'>");
                sb.Append("<B>" + moSalaryDetailsBL.PaidSalaryDifferences.Where(s => s.MonthId == sal.MonthId && s.Year == sal.Year).Sum(s => s.Amount) + "</B>");
                sb.Append("</TD>");
            }
            );

        sb.Append("</TR>");
        return sb.ToString();
    }

    /// <summary>
    /// This method is used to add salary difference total row.
    /// </summary>
    /// <returns></returns>
    private string AddPaidSalaryDifferenceTotals()
    {
        StringBuilder sb = new StringBuilder();
        string sStyle = " style='background-color:#C697F8'";
        sb.Append("<TR>");
        sb.Append("<TD " + sStyle + ">");
        sb.Append("<B>Total</B>");
        sb.Append("</TD>");

        moSalaryDetailsBL.SalaryDifferencePaidDetails.Select(sd => new { sd.PaidYear, sd.PaidMonthId, sd.MonthName }).Distinct().OrderBy(sd => sd.PaidYear).ThenBy(sd => sd.PaidMonthId).ToList().ForEach(
            sal =>
            {
                sb.Append("<TD style='background-color:#C697F8;width:75px'>");
                sb.Append("<B>" + moSalaryDetailsBL.SalaryDifferencePaidDetails.Where(s => s.PaidMonthId == sal.PaidMonthId && s.PaidYear == sal.PaidYear).Sum(s => s.Amount) + "</B>");
                sb.Append("</TD>");
            }
            );

        sb.Append("</TR>");
        return sb.ToString();
    }

    /// <summary>
    /// This method is used to add salary difference columns.
    /// </summary>
    /// <returns></returns>
    private string AddSalDiffColumns()
    {
        StringBuilder sb = new StringBuilder();
        string sStyle = " style='background-color:#D6DBDF'";
        sb.Append("<TR>");
        sb.Append("<TD " + sStyle + ">");
        sb.Append("<B>Earnings</B>");
        sb.Append("</TD>");

        moSalaryDetailsBL.PaidSalaryDifferences.Select(sd => new { sd.Year, sd.MonthId, sd.MonthName }).Distinct().OrderBy(sd => sd.Year).ThenBy(sd => sd.MonthId).ToList().ForEach(
            sal =>
            {
                sb.Append("<TD style='background-color:#D6DBDF;width:75px'>");
                sb.Append("<B>" + sal.MonthName + "-" + sal.Year + "</B>");
                sb.Append("</TD>");
            }
            );
        sb.Append("</TR>");
        return sb.ToString();
    }

    /// <summary>
    /// This method is used to add salary difference columns.
    /// </summary>
    /// <returns></returns>
    private string AddPaidSalDiffColumns()
    {
        StringBuilder sb = new StringBuilder();
        string sStyle = " style='background-color:#D6DBDF'";
        sb.Append("<TR>");
        sb.Append("<TD " + sStyle + ">");
        sb.Append("<B>Earnings</B>");
        sb.Append("</TD>");

        moSalaryDetailsBL.SalaryDifferencePaidDetails.Select(sd => new { sd.PaidYear, sd.PaidMonthId, sd.MonthName }).Distinct().OrderBy(sd => sd.PaidYear).ThenBy(sd => sd.PaidMonthId).ToList().ForEach(
            sal =>
            {
                sb.Append("<TD style='background-color:#D6DBDF;width:75px'>");
                sb.Append("<B>" + sal.MonthName + "-" + sal.PaidYear + "</B>");
                sb.Append("</TD>");
            }
            );
        sb.Append("</TR>");
        return sb.ToString();
    }

    /// <summary>
    /// This method is used to add salary difference details.
    /// </summary>
    /// <returns></returns>
    private string AddSalaryDifference()
    {
        StringBuilder sb = new StringBuilder();

        moSalaryDetailsBL.PaidSalaryDifferences.Select(sd => new { sd.EdId, sd.ShortName, sd.OriginalEdId }).Distinct().OrderBy(sd => sd.OriginalEdId).ToList().ForEach(
            sal =>
            {
                sb.Append("<TR>");
                sb.Append("<TD>" + sal.ShortName + "</TD>");
                moSalaryDetailsBL.PaidSalaryDifferences.Select(sd => new { sd.Year, sd.MonthId }).Distinct().OrderBy(sd => sd.Year).ThenBy(sd => sd.MonthId).ToList().ForEach
                    (
                        ed =>
                        {
                            var Amt = moSalaryDetailsBL.PaidSalaryDifferences.Where(s => s.EdId == sal.EdId && s.Year == ed.Year && s.MonthId == ed.MonthId).Select(s => s.Amount).FirstOrDefault();
                            if (Amt != null)
                                sb.Append("<TD>" + Amt + "</TD>");
                            else
                                sb.Append("<TD>0</TD>");
                        }
                    );
                sb.Append("</TR>");
            }
            );
        return sb.ToString();
    }

    /// <summary>
    /// This method is used to add salary difference details.
    /// </summary>
    /// <returns></returns>
    private string AddPaidSalaryDifference()
    {
        StringBuilder sb = new StringBuilder();

        moSalaryDetailsBL.SalaryDifferencePaidDetails.Select(sd => new { sd.EdId, sd.ShortName, sd.OriginalEdId }).Distinct().OrderBy(sd => sd.OriginalEdId).ToList().ForEach(
            sal =>
            {
                sb.Append("<TR>");
                sb.Append("<TD>" + sal.ShortName + "</TD>");
                moSalaryDetailsBL.SalaryDifferencePaidDetails.Select(sd => new { sd.PaidYear, sd.PaidMonthId }).Distinct().OrderBy(sd => sd.PaidYear).ThenBy(sd => sd.PaidMonthId).ToList().ForEach
                    (
                        ed =>
                        {
                            var Amt = moSalaryDetailsBL.SalaryDifferencePaidDetails.Where(s => s.EdId == sal.EdId && s.PaidYear == ed.PaidYear && s.PaidMonthId == ed.PaidMonthId).Select(s => s.Amount).FirstOrDefault();
                            if (Amt != null)
                                sb.Append("<TD>" + Amt + "</TD>");
                            else
                                sb.Append("<TD>0</TD>");
                        }
                    );
                sb.Append("</TR>");
            }
            );
        return sb.ToString();
    }

    /// <summary>
    /// This method is used to add summary row.
    /// </summary>
    /// <returns></returns>
    private string AddTotalRow()
    {
        string sColor = " style='background-color:#C697F8'";
        string sDifferenceColor = " style='background-color:#ECB2A6'";
        StringBuilder sb = new StringBuilder();

        sb.Append("<TR>");
        sb.Append("<TD" + sColor + ">");
        sb.Append("<B>Total</B>");
        sb.Append("</TD>");

        StringBuilder obj = new StringBuilder();

        int iIndex = 0;
        int iLastTotal = 0;
        mlstSalaryChanges.Select(sd => new { sd.Year, sd.MonthId }).OrderBy(sd => sd.Year).ThenBy(sd => sd.MonthId).ToList().ForEach(
            sal =>
            {

                sb.Append("<TD " + sColor + ">");
                int iBasic = mlstSalaryChanges.Where(s => s.Year == sal.Year && s.MonthId == sal.MonthId).Select(s => s.Basic).FirstOrDefault();
                int iGP = mlstSalaryChanges.Where(s => s.Year == sal.Year && s.MonthId == sal.MonthId).Select(s => s.GP).FirstOrDefault();
                int iDA = mlstSalaryChanges.Where(s => s.Year == sal.Year && s.MonthId == sal.MonthId).Select(s => s.DAPercentage).FirstOrDefault();
                iDA = ((iBasic + iGP) * iDA) / 100;
                int iHRA = mlstSalaryChanges.Where(s => s.Year == sal.Year && s.MonthId == sal.MonthId).Select(s => s.HRAPercentage).FirstOrDefault();
                iHRA = ((iBasic + iGP) * iHRA) / 100;
                int iTA = mlstSalaryChanges.Where(s => s.Year == sal.Year && s.MonthId == sal.MonthId).Select(s => s.TA).FirstOrDefault();

                int iTotal = iBasic + iGP + iDA + iHRA + iTA;

                if (iIndex == 0)
                    obj.Append("<TD " + sDifferenceColor + "><B>0</B></TD>");
                else
                    obj.Append("<TD " + sDifferenceColor + "><B>" + (iTotal - iLastTotal) + "</B></TD>");

                iLastTotal = iTotal;

                sb.Append("<B>" + iTotal + "</B>");
                sb.Append("</TD>");

                iIndex++;
            }
            );

        sb.Append("</TR>");

        sb.Append("<TR>");
        sb.Append("<TD " + sDifferenceColor + ">");
        sb.Append("<B>Difference</B>");
        sb.Append("</TD>");
        sb.Append(obj.ToString());
        sb.Append("</TR>");
        return sb.ToString();
    }

    /// <summary>
    /// This method is used to add earning details.
    /// </summary>
    /// <param name="asCaption"></param>
    /// <param name="aiIndex"></param>
    /// <returns></returns>
    private string AddEarnings(string asCaption, int aiIndex)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<TR>");

        if (aiIndex == 4 || aiIndex == 6)
            sb.Append("<TD valign='middle' style='color:navy'>");
        else
            sb.Append("<TD valign='middle'>");

        sb.Append(asCaption);
        sb.Append("</TD>");
        mlstSalaryChanges.OrderBy(sd => sd.Year).ThenBy(sd => sd.MonthId).ToList().ForEach(
            sal =>
            {
                int iAmount = 0;

                if (aiIndex == 4 || aiIndex == 6)
                    sb.Append("<TD style='color:navy'>");
                else
                    sb.Append("<TD>");

                switch (aiIndex)
                {
                    case 1: iAmount = sal.Basic; break;
                    case 2: iAmount = sal.GP; break;
                    case 3: iAmount = ((sal.Basic + sal.GP) * sal.DAPercentage) / 100; break;
                    case 4: iAmount = sal.DAPercentage; break;
                    case 5: iAmount = ((sal.Basic + sal.GP) * sal.HRAPercentage) / 100; break;
                    case 6: iAmount = sal.HRAPercentage; break;
                    case 7: iAmount = sal.TA; break;
                    case 8: iAmount = sal.RA; break;
                }

                sb.Append(iAmount + ((aiIndex == 4 || aiIndex == 6) ? "%" : string.Empty));
                sb.Append("</TD>");
            }
            );
        sb.Append("</TR>");
        return sb.ToString();
    }

    /// <summary>
    /// This method is used to add columns.
    /// </summary>
    /// <returns></returns>
    private string AddColumns()
    {
        StringBuilder sb = new StringBuilder();
        string sStyle = " style='background-color:#D6DBDF'";
        sb.Append("<TR>");
        sb.Append("<TD " + sStyle + ">");
        sb.Append("<B>Earnings</B>");
        sb.Append("</TD>");

        mlstSalaryChanges.OrderBy(sd => sd.Year).ThenBy(sd => sd.MonthId).ToList().ForEach(
            sal =>
            {
                sb.Append("<TD style='background-color:#D6DBDF;width:75px'>");
                sb.Append("<B>" + sal.MonthName + "-" + sal.Year + "</B>");
                sb.Append("</TD>");
            }
            );
        sb.Append("</TR>");
        return sb.ToString();
    }     

    #endregion
}

