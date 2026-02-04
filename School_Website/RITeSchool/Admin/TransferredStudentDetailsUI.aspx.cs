using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;
using System.Web.UI.HtmlControls;

public partial class TransferredStudentDetailsUI : SchoolBase
{
    #region Constants

    const string S_BRANCHES = "Branches";
    const string S_CLASSES = "Classes";

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to fill branches and student list.
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
                FillSchoolBranchDetails();
                LoadAvailableClasses();
                FillstudentList();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to search student.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillstudentList();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to activate student in system.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            StudentBL oStudentBL = new StudentBL();
            string Ids = GetSelectedStudentIds();
            oStudentBL.SaveTransferredStudentDetails(Ids, miSchoolId, miAcademicYearId, miUserId);
            lblUpdateMessage.Text = "Student(s) are activated successfully!!!";
            FillstudentList();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill division dropdownlist.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwBranch_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            DataRowView dv = e.Item.DataItem as DataRowView;
            var FromBranchId = dv["FromBranchId"].ToInt();

            List<SchoolBranchDetails> lstSchoolBranchDetails = new List<SchoolBranchDetails>();
            if (ViewState[S_BRANCHES] != null)
            {
                lstSchoolBranchDetails = ViewState[S_BRANCHES] as List<SchoolBranchDetails>;
                var sBranch = lstSchoolBranchDetails.Where(ss => ss.SchoolId == FromBranchId).FirstOrDefault();
                if (sBranch != null)
                {
                    Label lblBranchName = e.Item.FindControl("lblBranchName") as Label;
                    lblBranchName.Text = sBranch.SchoolName;
                }
            }

            if (optFrom.Checked)
            {
                CheckBox chkSelect = e.Item.FindControl("chkSelect") as CheckBox;    
                chkSelect.Attributes.Add("onclick", "SetField('" + e.Item.DisplayIndex + "')");

                if (ViewState[S_CLASSES] != null)
                {
                    DataTable dt = ViewState[S_CLASSES] as DataTable;
                    DataTable newDT = dt.Select("Standard_Name='" + dv["Standard_Name"].ToString() + "'").CopyToDataTable();
                    DropDownList ddlTargetDivision = e.Item.FindControl("ddlTargetDivision") as DropDownList;
                    ListSource.FillDropDownList(newDT, ddlTargetDivision, "Division_Name", "schoolwise_standard_Division_Id", Constants.S_SELECT);

                    if (dv["TargetStdDivId"] != null && dv["TargetStdDivId"].ToString() != Constants.S_ZERO)
                    {
                        ddlTargetDivision.SelectedValue = dv["TargetStdDivId"].ToString();
                        chkSelect.Checked = true;
                        chkSelect.Enabled = false;
                        ddlTargetDivision.Enabled = false;
                    }
                }
            }
            else
            {
                HtmlTableCell tdSelect = e.Item.FindControl("tdSelect") as HtmlTableCell;
                if (tdSelect != null)
                    tdSelect.Visible = false;

                HtmlTableCell tdTargetDivision = e.Item.FindControl("tdTargetDivision") as HtmlTableCell;
                if (tdTargetDivision != null)
                    tdTargetDivision.Visible = false;
            }
        }
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to set javascript atributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        btnSearch.Attributes.Add("onclick", "ResetMessage()");
        BtnAdd.Attributes.Add("onclick", "ResetMessage()");
        optFrom.Checked = true;

        optFrom.Attributes.Add("onclick", "HideFields('0')");
        optTo.Attributes.Add("onclick", "HideFields('1')");
    }

    /// <summary>
    /// This method is used to load available classes.
    /// </summary>
    private void LoadAvailableClasses()
    {
        StandardDivisionCollectionBL oStandardDivisionCollectionBL = new StandardDivisionCollectionBL(miSchoolId, miAcademicYearId);
        DataTable dt = oStandardDivisionCollectionBL.GetAssociatedStandardsDivisions();
        ViewState[S_CLASSES] = dt;
    }

    /// <summary>
    /// This method is used to fill branches.
    /// </summary>
    private void FillSchoolBranchDetails()
    {
        StudentBL oStudentBL = new StudentBL();
        List<SchoolBranchDetails> lstSchoolBranchDetails = oStudentBL.GetSchoolBranchDetails(miSchoolId);
        ListSource.FillDropDownList(lstSchoolBranchDetails, ddlBranch, "SchoolName", "SchoolId", Constants.S_SELECT_ALL);
        ViewState[S_BRANCHES] = lstSchoolBranchDetails;
    }

    /// <summary>
    /// This method is used to fill student list.
    /// </summary>
    private void FillstudentList()
    {
        StudentBL oStudentBL = new StudentBL();
        DataTable oDTCurrentStudents = oStudentBL.GetStudentListToActiveTransfer(ddlBranch.SelectedValue.ToInt(), txtSearch.Text.Trim(), chkOnlyNonActivated.Checked, optFrom.Checked);
        lstvwBranch.DataSource = oDTCurrentStudents;
        lstvwBranch.DataBind();

        if (oDTCurrentStudents.Rows.Count > 0)
        {
            HtmlTableRow trHeader = lstvwBranch.FindControl("trHeader") as HtmlTableRow;
            if (trHeader != null)
            {
                HtmlTableCell thChkSelectAll = trHeader.FindControl("thChkSelectAll") as HtmlTableCell;
                if (thChkSelectAll != null)
                {
                    if (optTo.Checked)
                        thChkSelectAll.Visible = false;
                    else
                        thChkSelectAll.Visible = true;
                }

                HtmlTableCell thTargetClass = trHeader.FindControl("thTargetClass") as HtmlTableCell;
                if (thTargetClass != null)
                {
                    if (optTo.Checked)
                        thTargetClass.Visible = false;
                    else
                        thTargetClass.Visible = true;
                }
            }

            if (optFrom.Checked)
                BtnAdd.Visible = true;
            else
                BtnAdd.Visible = false;
        }
        else
        {
            BtnAdd.Visible = false;
        }
    }

    /// <summary>
    /// This method is used to get selected ids.
    /// </summary>
    /// <returns></returns>
    private string GetSelectedStudentIds()
    {
        List<StudentData> lstStudentData = new List<StudentData>();
        foreach (ListViewDataItem Item in lstvwBranch.Items)
        {
            CheckBox chkstudent = Item.FindControl("chkSelect") as CheckBox;
            DropDownList ddlTargetDivision = Item.FindControl("ddlTargetDivision") as DropDownList;
            int iStudentId = lstvwBranch.DataKeys[Item.DisplayIndex]["Student_Id"].ToInt();
            if (chkstudent.Enabled && chkstudent.Checked)
                lstStudentData.Add(new StudentData { StudentId = iStudentId, StdDivId = ddlTargetDivision.SelectedValue.ToInt() });
        }

        string sIds = base.GenerateXml(lstStudentData);

        return sIds;
    }

    /// <summary>
    /// This method is used to generate xml.
    /// </summary>
    /// <param name="alstGenerateXML"></param>
    /// <returns></returns>
    public string GenerateStudentDataXml(Object alstGenerateXML)
    {
        var oStrwrtr = new StringWriter();
        new XmlSerializer(alstGenerateXML.GetType()).Serialize(oStrwrtr, alstGenerateXML);
        string sXml = oStrwrtr.ToString();
        return sXml.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", string.Empty);
    }

    #endregion

    public class StudentData
    {
        public int StudentId { get; set; }
        public int StdDivId { get; set; }
    }
}