/*File Name - StudentCountDetailsUI.aspx.cs
 * Created By - Sachin
 * Created Date - 13-Sept-2016
 * Description - This class is used to export student count details in required format.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;
using System.Threading;

public partial class StudentCountDetailsUI : SchoolBase
{
    #region Constant(s)
    
    private const string S_NEW_ADMISSION_COLOR = "background-color:#CEAEFF";
    private const string S_LEFT_STUDENT_COLOR = "background-color:#FFCBB3";
    private const string S_TOTAL_COLOR = "background-color:#B0FFDF";
    private const string S_GRAY_COLOR = "background-color:skyblue"; 

    #endregion

    #region Data Member(s)
    
    private StudentBL moStudentBL; 

    #endregion

    #region Enum

    private enum Section
    {
        StartingStudentCount = 1,
        RepeatingStudentCount = 2,
        Other = 3
    } 

    #endregion

    #region Event(s)
    
    /// <summary>
    /// This event is used to fill academic year combo box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moStudentBL = new StudentBL(miSchoolId, 0);
            if (!IsPostBack)
            {
                FillAcademicYears();
                SetJavascriptAttributes();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to export student count details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            List<StandardwiseStudentCount> lstStandardwiseStudentCount = moStudentBL.GetStandardwiseStudentCountDetails(miSchoolId, cmbAcademicYear.SelectedValue.ToInt());


            //lstStandardwiseStudentCount = lstStandardwiseStudentCount.Where(dt => dt.Date.Month == 6 && dt.Date.Year == 2016).ToList();

            StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, cmbAcademicYear.SelectedValue.ToInt());

            DataTable lstStandards = oStandardCollectionBL.GetAllStandards();

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Report-" + cmbAcademicYear.SelectedItem.Text + ".xls");
            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            HttpContext.Current.Response.Write("<BR><BR><BR>");

            HttpContext.Current.Response.Write("<Table>");
            HttpContext.Current.Response.Write("<TR>");

            int iSectionNo = 0;
            DateTime dtStartDate = DateTime.MaxValue;

            if (lstStandardwiseStudentCount.Any(st => st.IsStartingCount))
            {
                dtStartDate = lstStandardwiseStudentCount.Where(st => st.IsStartingCount).FirstOrDefault().Date;
                SetDateView(lstStandardwiseStudentCount, lstStandards, dtStartDate, Section.StartingStudentCount, iSectionNo);
                iSectionNo++;
            }

            DateTime dtRepeatingStartDate = DateTime.MaxValue;

            if (lstStandardwiseStudentCount.Any(st => st.IsStudentRepeatingClass))
            {
                dtRepeatingStartDate = lstStandardwiseStudentCount.Where(st => st.IsStudentRepeatingClass).FirstOrDefault().Date;
                SetDateView(lstStandardwiseStudentCount, lstStandards, dtRepeatingStartDate, Section.RepeatingStudentCount, iSectionNo);
                iSectionNo++;
            }

            lstStandardwiseStudentCount.Where(st => !st.IsStartingCount && !st.IsStudentRepeatingClass).OrderBy(st => st.Date).Select(st => st.Date).Distinct().ToList().ForEach
            (
                dt =>
                {
                    SetDateView(lstStandardwiseStudentCount, lstStandards, dt, Section.Other, iSectionNo);
                    iSectionNo++;
                }

            );

            SetDateView(lstStandardwiseStudentCount, lstStandards, DateTime.MinValue, Section.Other, iSectionNo);


            HttpContext.Current.Response.Write("</TR>");
            HttpContext.Current.Response.Write("</Table>");

            HttpContext.Current.Response.Write("</font>");
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    } 

    #endregion

    #region Method(s)
    
    /// <summary>
    /// This method is used to set date view.
    /// </summary>
    /// <param name="lstStandardwiseStudentCount"></param>
    /// <param name="lstStandards"></param>
    /// <param name="dt"></param>
    /// <param name="aoSection"></param>
    /// <param name="aiSectionNo"></param>
    private void SetDateView(List<StandardwiseStudentCount> lstStandardwiseStudentCount, DataTable lstStandards, DateTime dt, Section aoSection, int aiSectionNo)
    {
        HttpContext.Current.Response.Write("<TD>");
        string sHeader = "WITHDRAWAL / ADMISSION FOR " + cmbAcademicYear.SelectedItem.Text;

        if (dt.Date != DateTime.MinValue)
        {
            if (aoSection == Section.RepeatingStudentCount)
                sHeader = "Students Repeating the same class";
            else
                sHeader = lstStandardwiseStudentCount.Where(st => st.Date == dt.Date).FirstOrDefault().Header;
        }
        
        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:15px; font-family:Calibri; background:white;'>");

        HttpContext.Current.Response.Write("<TR style='background-color: " + (aiSectionNo % 2 == 0 ? "#BFFFFF" : "Silver") + "'>");
        if (aoSection == Section.Other)
        {
            if (dt == DateTime.MinValue)
                AddHeader(sHeader, 21, "text-align:center");
            else
                AddHeader(sHeader, 15, "text-align:center");
        }
        else if (aoSection == Section.StartingStudentCount)
            AddHeader(sHeader, 6, "text-align:center");
        else
            AddHeader(sHeader, 5, "text-align:center");

        HttpContext.Current.Response.Write("</TR>");

        HttpContext.Current.Response.Write("<TR>");

        if (aoSection == Section.StartingStudentCount)
            AddHeader(string.Empty, 1, "text-align:center");

        if (aoSection == Section.Other)
        {
            if (dt != DateTime.MinValue)
            {
                AddHeader("New Admission", 5, "text-align:center;" + S_NEW_ADMISSION_COLOR);
                AddHeader("Withdrawal", 5, "text-align:center;" + S_LEFT_STUDENT_COLOR);
                AddHeader("Total", 5, "text-align:center;" + S_TOTAL_COLOR);
            }
            else
            {
                var dt2 = lstStandardwiseStudentCount.Where(sd => sd.Date.Month == 4).FirstOrDefault();
                AddHeader("New Admission", 7, "text-align:center;" + S_NEW_ADMISSION_COLOR);
                AddHeader("Withdrawal", 7, "text-align:center;" + S_LEFT_STUDENT_COLOR);

                if (dt2 != null)
                    AddHeader("Total Count as on " + dt2.Date.ToString("dd MMM yyyy"), 7, "text-align:center;" + S_TOTAL_COLOR);
                else
                {
                    SchoolWiseAcademicYearMasterBL obj = new SchoolWiseAcademicYearMasterBL(miSchoolId, cmbAcademicYear.SelectedValue.ToInt());
                    AddHeader("Total Count as on" + new DateTime(obj.EndDate.Year,4,30).ToString("dd MMM yyyy"), 7, "text-align:center;" + S_TOTAL_COLOR);
                }
            }
        }
        else if (aoSection == Section.StartingStudentCount)
            AddHeader("Student Count", 5, "text-align:center;" + S_TOTAL_COLOR);
        else
            AddHeader("Add", 5, "text-align:center;" + S_GRAY_COLOR);

        HttpContext.Current.Response.Write("</TR>");

        AddHeaderRow(aoSection, dt);

        lstStandards.AsEnumerable().Where(st => st.Field<int>("School_Id") != -9999).OrderBy(std => std.Field<int>("Original_Standard_Id")).ToList().ForEach
        (
            std =>
            {
                SetStandardView(lstStandardwiseStudentCount, dt, std.Field<int>("Standard_Id"), std.Field<string>("Standard_Name"), string.Empty, aoSection);
            }

        );

        SetStandardView(lstStandardwiseStudentCount, dt, 0, "Total Students", "font-weight:bold", aoSection);

        HttpContext.Current.Response.Write("</Table>");
        HttpContext.Current.Response.Write("</TD>");
    }

    /// <summary>
    /// This method is used to header row.
    /// </summary>
    /// <param name="aoSection"></param>
    private void AddHeaderRow(Section aoSection, DateTime adtDate)
    {
        HttpContext.Current.Response.Write("<TR>");

        if (aoSection == Section.StartingStudentCount)
            AddHeader("Standard", 1, "width:100px;text-align:left");

        string sColor = GetColor(aoSection, true);

        AddHeader("Girl", 1, "width:100px;text-align:center;" + sColor);
        AddHeader("Boy", 1, "width:50px;text-align:center;" + sColor);
        AddHeader("RTE Girl", 1, "width:50px;text-align:center;" + sColor);
        AddHeader("RTE Boy", 1, "width:50px;text-align:center;" + sColor);

        if (adtDate == DateTime.MinValue)
        {
            AddHeader("Total of Girls", 1, "width:50px;text-align:center;" + sColor);
            AddHeader("Total of Boys", 1, "width:50px;text-align:center;" + sColor);
        }

        if (aoSection == Section.Other)
        {
            AddHeader("Total New Admissions", 1, "text-align:center;" + S_NEW_ADMISSION_COLOR);
            AddHeader("Girls", 1, "width:50px;text-align:center;" + S_LEFT_STUDENT_COLOR);
            AddHeader("Boys", 1, "width:50px;text-align:center;" + S_LEFT_STUDENT_COLOR);
            AddHeader("RTE Girls", 1, "width:50px;text-align:center;" + S_LEFT_STUDENT_COLOR);
            AddHeader("RTE Boys", 1, "width:50px;text-align:center;" + S_LEFT_STUDENT_COLOR);

            if (adtDate == DateTime.MinValue)
            {
                AddHeader("Total of Girls", 1, "width:50px;text-align:center;" + S_LEFT_STUDENT_COLOR);
                AddHeader("Total of Boys", 1, "width:50px;text-align:center;" + S_LEFT_STUDENT_COLOR);
            }

            AddHeader("Total Withdrawals", 1, "text-align:center;" + S_LEFT_STUDENT_COLOR);

            AddHeader("Total Girls", 1, "text-align:center;" + S_TOTAL_COLOR);
            AddHeader("Total Boys", 1, "text-align:center;" + S_TOTAL_COLOR);
            AddHeader("Total RTE Girls", 1, "text-align:center;" + S_TOTAL_COLOR);
            AddHeader("Total RTE Boys", 1, "text-align:center;" + S_TOTAL_COLOR);

            if (adtDate == DateTime.MinValue)
            {
                AddHeader("Total of Girls", 1, "width:50px;text-align:center;" + S_TOTAL_COLOR);
                AddHeader("Total of Boys", 1, "width:50px;text-align:center;" + S_TOTAL_COLOR);
            }
            
            AddHeader("Total Students", 1, "text-align:center;" + S_TOTAL_COLOR);
        }
        else if (aoSection == Section.StartingStudentCount)
            AddHeader("Total", 1, "text-align:center;" + sColor);
        else
            AddHeader("Total Repeaters", 1, "text-align:center;" + sColor);

        HttpContext.Current.Response.Write("</TR>");
    }

    /// <summary>
    /// This method is used to set standard view.
    /// </summary>
    /// <param name="lstStandardwiseStudentCount"></param>
    /// <param name="dt"></param>
    /// <param name="aiStandardId"></param>
    /// <param name="asStandardName"></param>
    /// <param name="asStyle"></param>
    /// <param name="aoSection"></param>
    private void SetStandardView(List<StandardwiseStudentCount> lstStandardwiseStudentCount, DateTime dt, int aiStandardId, string asStandardName, string asStyle, Section aoSection)
    {
        string sStyle = string.Empty;
        if (asStyle != string.Empty)
            sStyle = "style='" + asStyle + "'";

        HttpContext.Current.Response.Write("<TR " + sStyle + ">");
        List<StandardwiseStudentCount> oStudentCount = lstStandardwiseStudentCount.Where(sd => (sd.StandardId == aiStandardId || aiStandardId == 0) && (sd.Date.Date == dt.Date.Date || (dt.Date.Date == DateTime.MinValue && !sd.IsStartingCount))).ToList();
        List<StandardwiseStudentCount> oTotalStudentCount = lstStandardwiseStudentCount.Where(sd => (sd.StandardId == aiStandardId || aiStandardId == 0) && (sd.Date.Date <= dt.Date.Date || (dt.Date.Date == DateTime.MinValue && !sd.IsStartingCount))).ToList();

        //List<StandardwiseStudentCount> oStudentCount = lstStandardwiseStudentCount.Where(sd => (sd.StandardId == aiStandardId || aiStandardId == 0) && (sd.Date.Date == dt.Date.Date )).ToList();
        //List<StandardwiseStudentCount> oTotalStudentCount = lstStandardwiseStudentCount.Where(sd => (sd.StandardId == aiStandardId || aiStandardId == 0) && (sd.Date.Date == dt.Date.Date )).ToList();

        if (aoSection == Section.StartingStudentCount)
        {
            HttpContext.Current.Response.Write("<Td style='font-weight:bold'>");
            HttpContext.Current.Response.Write(asStandardName);
            HttpContext.Current.Response.Write("</Td>");
        }

        AddNewStudent(oStudentCount, true, aoSection, dt);
        if (aoSection == Section.Other)
        {
            AddNewStudent(oStudentCount, false, Section.Other, dt);

            List<StandardwiseStudentCount> lstCounts = new List<StandardwiseStudentCount>();
            if (dt.Date == DateTime.MinValue)
            {
                var dt2 = lstStandardwiseStudentCount.Where(sd => sd.Date.Month == 4).FirstOrDefault();
                if (dt2 != null)
                    lstCounts = lstStandardwiseStudentCount.Where(sd => (sd.StandardId == aiStandardId || aiStandardId == 0) && sd.Date <= dt2.Date).ToList();
            }
            else
                lstCounts = oTotalStudentCount;

            AddTotalColumn(lstCounts, 'F', false);
            AddTotalColumn(lstCounts, 'M', false);
            AddTotalColumn(lstCounts, 'F', true);
            AddTotalColumn(lstCounts, 'M', true);

            if (dt == DateTime.MinValue)
            {
                AddRow(lstCounts.Where(st => st.IsNewStudent && st.Sex == 'F').Sum(ss => ss.StudentCount) - lstCounts.Where(st => !st.IsNewStudent && st.Sex == 'F').Sum(ss => ss.StudentCount), S_TOTAL_COLOR);
                AddRow(lstCounts.Where(st => st.IsNewStudent && st.Sex == 'M').Sum(ss => ss.StudentCount) - lstCounts.Where(st => !st.IsNewStudent && st.Sex == 'M').Sum(ss => ss.StudentCount), S_TOTAL_COLOR);
            }

            AddRow(lstCounts.Where(st => st.IsNewStudent).Sum(ss => ss.StudentCount) - lstCounts.Where(st => !st.IsNewStudent).Sum(ss => ss.StudentCount), S_TOTAL_COLOR);
        }

        HttpContext.Current.Response.Write("</TR>");
    }

    /// <summary>
    /// This method is used to add total column.
    /// </summary>
    /// <param name="alstStudentCount"></param>
    /// <param name="acSex"></param>
    /// <param name="abIsRteStudent"></param>
    private void AddTotalColumn(List<StandardwiseStudentCount> alstStudentCount, char acSex, bool abIsRteStudent)
    {
        var iNewGirlCount = alstStudentCount.Where(sd => sd.Sex == acSex && sd.IsNewStudent == true && sd.IsRteStudent == abIsRteStudent).Sum(st => st.StudentCount);
        var iLeftGirlCount = alstStudentCount.Where(sd => sd.Sex == acSex && sd.IsNewStudent == false && sd.IsRteStudent == abIsRteStudent).Sum(st => st.StudentCount);

        AddRow(iNewGirlCount - iLeftGirlCount, S_TOTAL_COLOR);
    }

    /// <summary>
    /// This method is used to add new student.
    /// </summary>
    /// <param name="oStudentCount"></param>
    /// <param name="abIsNewStudent"></param>
    /// <param name="aoSection"></param>
    private void AddNewStudent(List<StandardwiseStudentCount> oStudentCount, bool abIsNewStudent, Section aoSection, DateTime adtDate)
    {
        AddCell(oStudentCount, 'F', abIsNewStudent, false, aoSection);
        AddCell(oStudentCount, 'M', abIsNewStudent, false, aoSection);
        AddCell(oStudentCount, 'F', abIsNewStudent, true, aoSection);
        AddCell(oStudentCount, 'M', abIsNewStudent, true, aoSection);

        if (adtDate == DateTime.MinValue)
        {
            AddRow(oStudentCount.Where(st => st.IsNewStudent == abIsNewStudent && st.Sex  == 'F').Sum(st => st.StudentCount), GetColor(aoSection, abIsNewStudent));
            AddRow(oStudentCount.Where(st => st.IsNewStudent == abIsNewStudent && st.Sex == 'M').Sum(st => st.StudentCount), GetColor(aoSection, abIsNewStudent));
        }

        AddRow(oStudentCount.Where(st => st.IsNewStudent == abIsNewStudent).Sum(st => st.StudentCount), GetColor(aoSection, abIsNewStudent));
    }

    /// <summary>
    /// This method is sued to return color.
    /// </summary>
    /// <param name="aoSection"></param>
    /// <param name="abIsNewStudent"></param>
    /// <returns></returns>
    private string GetColor(Section aoSection, bool abIsNewStudent)
    {
        string sColor = string.Empty;

        if (aoSection == Section.StartingStudentCount)
            sColor = S_TOTAL_COLOR;
        else if (aoSection == Section.RepeatingStudentCount)
            sColor = S_GRAY_COLOR;
        else if (abIsNewStudent)
            sColor = S_NEW_ADMISSION_COLOR;
        else
            sColor = S_LEFT_STUDENT_COLOR;
        return sColor;
    }

    /// <summary>
    /// This method is used to add cell.
    /// </summary>
    /// <param name="alstStudentCount"></param>
    /// <param name="acSex"></param>
    /// <param name="abIsNewStudent"></param>
    /// <param name="abIsRteStudent"></param>
    /// <param name="aoSection"></param>
    private void AddCell(List<StandardwiseStudentCount> alstStudentCount, char acSex, bool abIsNewStudent, bool abIsRteStudent, Section aoSection)
    {
        int iNewGirlCount = alstStudentCount.Where(sd => sd.Sex == acSex && sd.IsNewStudent == abIsNewStudent && sd.IsRteStudent == abIsRteStudent).Sum(st => st.StudentCount);
        AddRow(iNewGirlCount, GetColor(aoSection, abIsNewStudent));
    }

    /// <summary>
    /// This method is used to add row.
    /// </summary>
    /// <param name="aiCount"></param>
    /// <param name="asStyle"></param>
    private static void AddRow(int aiCount, string asStyle = "")
    {
        string sStyle = string.Empty;
        if (asStyle != string.Empty)
            sStyle = "style='" + asStyle + "'";

        HttpContext.Current.Response.Write("<Td " + sStyle + ">");
        HttpContext.Current.Response.Write(aiCount.ToString());
        HttpContext.Current.Response.Write("</Td>");
    }

    /// <summary>
    /// This method is used to add header.
    /// </summary>
    /// <param name="asText"></param>
    /// <param name="aiColSpan"></param>
    /// <param name="asStyle"></param>
    private static void AddHeader(string asText, int aiColSpan = 1, string asStyle = "")
    {
        string sStyle = string.Empty;
        if (asStyle != string.Empty)
            sStyle = "style='" + asStyle + "'";
        HttpContext.Current.Response.Write("<Td colspan='" + aiColSpan + "' " + sStyle + ">");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(asText);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
    }
    
    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        base.ApplyMouseHoverEffect(new List<Button> { btnShow });
    }

    /// <summary>
    /// This method is used to fill academic years.
    /// </summary>
    private void FillAcademicYears()
    {
        SchoolWiseAcademicYearMasterBL oSchoolWiseAcademicYearMasterBL = new SchoolWiseAcademicYearMasterBL();
        DataTable oDataTable = oSchoolWiseAcademicYearMasterBL.GetAllAcademicYearsForSchool(miSchoolId);
        ListSource.FillDropDownList(oDataTable, cmbAcademicYear, "YearValue", "Academic_Year_Id", Constants.S_SELECT);
    } 

    #endregion
}