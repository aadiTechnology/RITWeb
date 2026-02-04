using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using BusinessLogic.Exceptions;
using Utility;
using BusinessLogic;
using System.Reflection;
public partial class OnlineExamDetailsUI : SchoolBase
{
    private int miExam;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {                
                FillTestCombobox();
                FillListview();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    private void FillTestCombobox()
    {
        int iStudentId = Session[Constants.S_SESSION_STUDENT_ID].ToInt();
        OnlineExamConfigurationBL oOnlineExamConfigurationBL = new OnlineExamConfigurationBL(miSchoolId, miAcademicYearId, miUserId);
        DataTable oDsAllTests = oOnlineExamConfigurationBL.GetAllTestsForStudent(iStudentId);
        ControlUtility.FillDropDownList(oDsAllTests, ref cmbExam, "Id", "Name", Constants.S_SELECT); 
       
        if(QueryString["ExamId"] != null && QueryString["ExamId"].ToString() != string.Empty)
        {
            cmbExam.SelectedValue = QueryString["ExamId"].ToString();
            cmbExam_SelectedIndexChanged(cmbExam,null);
        }
    }

    protected void cmbExam_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillListview();
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }
    private void FillListview()  //fill listview
    {
        int iStudentid = Session[Constants.S_SESSION_STUDENT_ID].ToInt();
        DataTable oDt = OnlineExamWiseQueConfigBL.GetAllSubjectsForExam(miSchoolId, miAcademicYearId, cmbExam.SelectedValue.ToInt(), iStudentid);
        lstvwExam.DataSource = oDt;
        lstvwExam.DataBind();

        DateTime dtt = oDt.AsEnumerable().Where(dt => dt.Field<DateTime>("StartDateAndTime").Date == DateTime.Now.Date && dt.Field<DateTime>("StartDateAndTime")> DateTime.Now).OrderBy(dt => dt.Field<DateTime>("StartDateAndTime")).Select(dt => dt.Field<DateTime>("StartDateAndTime")).FirstOrDefault();
        DateTime edt = oDt.AsEnumerable().Where(dt => dt.Field<DateTime>("EndDateAndTime").Date == DateTime.Now.Date && dt.Field<DateTime>("EndDateAndTime") > DateTime.Now).OrderBy(dt => dt.Field<DateTime>("EndDateAndTime")).Select(dt => dt.Field<DateTime>("EndDateAndTime")).FirstOrDefault();

        if (edt < dtt || dtt == DateTime.MinValue)
            dtt = edt;

        if (dtt != null && dtt != DateTime.MinValue)
        {
            var diff = dtt.Subtract(DateTime.Now);
            Timer1.Interval = diff.TotalMilliseconds.ToInt();
            Timer1.Enabled = true;
        }
        else
            Timer1.Enabled = false;
    }

    protected void lstvwExam_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oLstVwItem = (ListViewDataItem)e.Item;
                int iRowId = Convert.ToInt32(oLstVwItem.DisplayIndex);
                LinkButton oLinkButton = e.Item.FindControl("lnkDetails") as LinkButton;
                int iItemID = Convert.ToInt32(lstvwExam.DataKeys[iRowId]["ExamID"]);
                int StandardDivisionId = Convert.ToInt32(lstvwExam.DataKeys[iRowId]["StandardDivisionId"]);
                string StartTime = Convert.ToString(lstvwExam.DataKeys[iRowId]["StartTime"]);
                string EndTime = Convert.ToString(lstvwExam.DataKeys[iRowId]["EndTime"]);
                int SubjectId = Convert.ToInt32(lstvwExam.DataKeys[iRowId]["SubjectId"]);
                int IsSubmited = Convert.ToInt32(lstvwExam.DataKeys[iRowId]["IsSubmited"]);
                DateTime d = DateTime.Now;

                DataRowView dv = e.Item.DataItem as DataRowView;
                DateTime dtStartDate = dv["StartDateAndTime"].ToDateTime();
                DateTime dtEndDateAndTime = dv["EndDateAndTime"].ToDateTime();

                if (dtStartDate <= DateTime.Now && DateTime.Now <= dtEndDateAndTime)
                {
                    oLinkButton.Enabled = true;
                    oLinkButton.Text = "Exam";
                    string sQueryString = "ExamId=" + iItemID + "&StandardDivisionId=" + StandardDivisionId + "&SubjectId=" + SubjectId + "&StartTime=" + StartTime + "&EndTime=" + EndTime;
                    oLinkButton.Attributes.Add("onclick", "window.open('../OnlineExam/StudentOnlineExamUI.aspx?" + CommonUtility.EncryptQuerystring(sQueryString) + "', '_Self');return false;");
                }
                else
                {
                    oLinkButton.Text = "-";
                    oLinkButton.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }  
    
    protected void lstvwExam_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        FillListview();
    }
}