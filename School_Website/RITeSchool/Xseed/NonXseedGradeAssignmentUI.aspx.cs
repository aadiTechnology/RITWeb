//----------------------------------------------------------------------------------------------------------
// Class Name       :- NonXseedGradeAssignmentUI
// Purpose          :- This class is used to display all the students 
//                     and assign grades and observations for all students.
// Date Of creation :- 6/05/2011
// Author Name      :- Shobha Patil.
//----------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using XseedReportEntities;

public partial class NonXseedGradeAssignmentUI : SchoolBase
{

    #region "CONSTANTS"

    const string S_XSEED_GRADES="XseedGrades";

    #endregion

    #region "DATA MEMBERS"

    StudentXseedGradeAssignmentBL moStudentXseedGradeAssignmentBL;

    #endregion

    #region "EVENTS"

    /// <summary>
    /// This event is used to initialise the controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            moStudentXseedGradeAssignmentBL = new StudentXseedGradeAssignmentBL();
            if (!IsPostBack)
            {
                ReadQueryString();
                SetJavaScriptAttributes();
                FillGradeAssignmentListView();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to transfer control to Previous page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(SetQueryString(Convert.ToInt32(hidAssessmentId.Value)));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used fill the top header grade combobox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStudentGradeDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                StudentXseedGradeDetails oStudentXseedGradeDetails = (StudentXseedGradeDetails)oCurrentItem.DataItem;
                int iRowId = oCurrentItem.DisplayIndex;
                DropDownList cmbGrades = e.Item.FindControl("cmbGrades") as DropDownList;
                TextBox txtObservetions = e.Item.FindControl("txtObservations") as TextBox;
                if (ViewState[S_XSEED_GRADES] != null)
                {
                    List<GradeMaster> lstGradeMaster = ViewState[S_XSEED_GRADES] as List<GradeMaster>;
                    cmbGrades.Items.Add(new ListItem(Constants.S_SELECT, "0"));
                    lstGradeMaster.ForEach(Grades => cmbGrades.Items.Add(new ListItem(Grades.GradeName, Grades.GradeId.ToString())));
                }
                if (oStudentXseedGradeDetails.GradeId != 0)
                {
                    cmbGrades.SelectedValue = oStudentXseedGradeDetails.GradeId.ToString();
                    txtObservetions.Enabled = true;
                }
                else
                    txtObservetions.Enabled = false;

                cmbGrades.Attributes.Add("onclick", "SelectAllControls(this," + iRowId + ")");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save the assigned grade and observation for the students.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            List<NonXseedSubjectGrades> lstNonXseedSubjectGrades = PopulateNonXseedSubjectGrades();
            moStudentXseedGradeAssignmentBL.GradeSubmitStatus = PopulateGradeSubmitStatusBL();
            moStudentXseedGradeAssignmentBL.Save(GetSubjectSectionXML(lstNonXseedSubjectGrades));
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(SetQueryString(Convert.ToInt32(hidAssessmentId.Value)));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    private string GetSubjectSectionXML(List<NonXseedSubjectGrades> lstNonXseedSubjectGrades)
    {
        StringWriter sw = new StringWriter();
        new XmlSerializer(lstNonXseedSubjectGrades.GetType()).Serialize(sw, lstNonXseedSubjectGrades);
        string sXML = sw.ToString();
        sXML = sXML.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", string.Empty);
        return sXML;
    }

    #endregion

    #region "PRIVATE METHODS"

    /// <summary>
    /// This event is used fill the top header grade combobox.
    /// </summary>
    private void FillHeaderGradesCombobox()
     {
        HtmlTableRow oHtmlTableRow = (HtmlTableRow)lstvwStudentGradeDetails.FindControl("trHeader");
        DropDownList cmbAllGrades = (DropDownList)oHtmlTableRow.FindControl("cmbAllGrades");
        cmbAllGrades.Focus();
        if (ViewState[S_XSEED_GRADES] != null)
        {
            List<GradeMaster> lstGradeMaster = ViewState[S_XSEED_GRADES] as List<GradeMaster>;
            cmbAllGrades.Items.Add(new ListItem(Constants.S_SELECT, "0"));
            lstGradeMaster.ForEach(Grades => cmbAllGrades.Items.Add(new ListItem(Grades.GradeName, Grades.GradeId.ToString())));
            cmbAllGrades.Attributes.Add("onchange", "SelectAll(this)");
        }
    }

    /// <summary>
    /// This method is used to set the default attributes.
    /// </summary>
    private void SetJavaScriptAttributes()
    {

        valSumNonXseedGrades.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        ApplyMouseHoverEffect(new List<Button> {btnBack, btnSave});
    }

     /// <summary>
    /// This method is used to populate StudentXseedGradeAssignmentBL objects.
    /// </summary>
    /// <returns></returns>
    private GradeSubmitStatus PopulateGradeSubmitStatusBL()
    {
        GradeSubmitStatus oGradeSubmitStatus = new GradeSubmitStatus
        {
            AcademicYearId = miAcademicYearId,
            SchoolId = miSchoolId,
            StandardDivisionId = Convert.ToInt32(hidStandardDivisionId.Value),
            AssessmentId = Convert.ToInt32(hidAssessmentId.Value),
            SubjectId = Convert.ToInt32(hidSubjectId.Value),
            InsertedById = miUserId,
        };
        return oGradeSubmitStatus;
    }

    /// <summary>
    /// This method is used to fill the student listview.
    /// </summary>
    private void FillGradeAssignmentListView()
    {

        moStudentXseedGradeAssignmentBL.GradeSubmitStatus = PopulateGradeSubmitStatusBL();
        moStudentXseedGradeAssignmentBL.GetStudentsForStdDiv();
        lblClass.Text = moStudentXseedGradeAssignmentBL.ClassName;
        LblAssessment.Text = moStudentXseedGradeAssignmentBL.AssessmentDetails.Name;
        lblDataSubjectName.Text = moStudentXseedGradeAssignmentBL.SubjectName;
        List<GradeMaster> lstGradeMaster = moStudentXseedGradeAssignmentBL.GradeDetailsList;
        ViewState[S_XSEED_GRADES] = lstGradeMaster;
        hidIsAbsent.Value = lstGradeMaster.Where(ls => ls.ConsideredAsAbsent).Select(l => l.GradeId).FirstOrDefault().ToString();
        hidIsExempted.Value = lstGradeMaster.Where(ls => ls.ConsideredAsExempted).Select(l => l.GradeId).FirstOrDefault().ToString();
        int iAssessmentId = Convert.ToInt32(hidAssessmentId.Value);
        int iSubjectId = Convert.ToInt32(hidSubjectId.Value);
        List<StudentXseedGradeDetails> lstStudentXseedGradeDetails = StudentXseedGradeAssignmentBL.GetAllStudents(miSchoolId,miAcademicYearId, Convert.ToInt32(hidStandardDivisionId.Value),iAssessmentId,iSubjectId);
        lstvwStudentGradeDetails.DataSource = lstStudentXseedGradeDetails;
        lstvwStudentGradeDetails.DataBind();
        FillHeaderGradesCombobox();
        MakeListReadOnly();
    }

    /// <summary>
    /// This method is used to check whether the grades for the selected subjects are submitted and make the ListView readonly.
    /// </summary>
    private void MakeListReadOnly()
    {
        if (hidIsReadOnly.Value == "3" || hidIsReadOnly.Value=="Y")
        {
            lstvwStudentGradeDetails.Enabled = false;
            btnSave.Visible = false;
            pnlSubmitStatus.Visible = true;
            if(hidFrom.Value == "0")
                lblSubmitMessage.Visible = true;
            else
                lblSubmitMessage.Text = "Results for this assessment has been published. You need to unpublish the assessment to update the grades.";
        }
        else
        {
            lstvwStudentGradeDetails.Enabled = true;
            btnSave.Visible = true;
            lblSubmitMessage.Visible = false;
        }
    }

    /// <summary>
    /// This method is used to read query string.
    /// </summary>
    private void ReadQueryString()
    {
        hidStandardDivisionId.Value = QueryString["StandardDivisionId"];
        hidSubjectId.Value = QueryString["SubjectId"];
        hidAssessmentId.Value = QueryString["AssessmentId"];
        hidTeacherId.Value = QueryString["TeacherId"];
        hidIsReadOnly.Value = QueryString["IsReadOnly"];
        hidFrom.Value = QueryString["From"] ?? Constants.S_ZERO;
    }
   
    /// <summary>
    /// This method is used to set query string.
    /// </summary>
    /// <param name="aiAssessmentId"></param>
    /// <returns></returns>
    private string SetQueryString(int aiAssessmentId)
    {
        string sQuerystring = string.Empty;
        sQuerystring = "AssessmentId=" + aiAssessmentId.ToString();
        sQuerystring = sQuerystring + "&TeacherId=" + hidTeacherId.Value;
        string sUrl = "~/Xseed/AssignXseedGradesUI.aspx?";
        if (hidFrom.Value != "0")
            sUrl = "~/Xseed/ClassTeacherXseedGradesUI.aspx?";
        return sUrl + CommonUtility.EncryptQuerystring(sQuerystring);
    }
    /// <summary>
    /// This method is used to populate Non-Xseed Subject grade details.
    /// </summary>
    /// <returns></returns>
    private List<NonXseedSubjectGrades> PopulateNonXseedSubjectGrades()
    {
        List<NonXseedSubjectGrades> lstNonXseedSubjectGardes = new List<NonXseedSubjectGrades>();
        NonXseedSubjectGrades oNonXseedSubjectGardes = null;
        foreach (ListViewDataItem oCurrentItem in lstvwStudentGradeDetails.Items)
        {
            int iRowId = oCurrentItem.DisplayIndex;
            DropDownList cmbGrades = oCurrentItem.FindControl("cmbGrades") as DropDownList;
            TextBox txtObservations = oCurrentItem.FindControl("txtObservations") as TextBox;
            if (!cmbGrades.SelectedValue.Equals("0"))
            {
                oNonXseedSubjectGardes = new NonXseedSubjectGrades();
                oNonXseedSubjectGardes.YearwiseStudentId = Convert.ToInt32(lstvwStudentGradeDetails.DataKeys[iRowId]["YaerwiseStudentId"]);
                oNonXseedSubjectGardes.GradeId = Convert.ToInt32(cmbGrades.SelectedValue);
                oNonXseedSubjectGardes.Observation = txtObservations.Text;
                lstNonXseedSubjectGardes.Add(oNonXseedSubjectGardes);
            }
        }
        return lstNonXseedSubjectGardes;
    }
   
    #endregion
}
