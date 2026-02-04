using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;
using XseedReportEntities;

public partial class AssignDescriptiveIndicatorMarksUI : SchoolBase
{
    #region Constant(s)

    private const string S_SAVE_MESSAGE = "Marks saved successfully !!!";
    
	#endregion

    #region Data Member(s)
    
    private DescriptiveIndicatorBL moDescriptiveIndicatorBL; 

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to set base class details.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnInit(EventArgs e)
    {
        try
        {
            base.OnInit(e);
            moDescriptiveIndicatorBL = new DescriptiveIndicatorBL(miSchoolId, miAcademicYearId, miUserId);

            if (Page.Request.Params.Get("__EVENTTARGET") != null)
            {
                if (btnSave.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")))
                    FillDescriptiveIndicators();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to fill terms, sections and descriptive indicators.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moDescriptiveIndicatorBL = new DescriptiveIndicatorBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                SetJavascriptAttributes();
                FillTerms();
                ReadQueryString();
                FillSections();
                FillDescriptiveIndicators();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill descriptive indicators as per selected term.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbTerm_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillDescriptiveIndicators();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill descriptive indicators as per selected section / skill.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillDescriptiveIndicators();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save marks and remarks.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            List<StudentwiseDescriptiveObservation> lstObservations = new List<StudentwiseDescriptiveObservation>();
            List<StudentwiseDescriptiveMark> lstMarks = new List<StudentwiseDescriptiveMark>();
          
            foreach (HtmlTableRow tr in tblMain.Rows)
            {
                if (tr.ID != null && tr.ID != string.Empty)
                {
                    string[] iarrSkillId = tr.ID.Split('_');
                    int iSkillId = iarrSkillId[1].ToInt();
                    int iParameterId = iarrSkillId[2].ToInt();

                    if (!lstObservations.Any(sk => sk.SkillId == iSkillId))
                    {
                        StudentwiseDescriptiveObservation oObservaion = new StudentwiseDescriptiveObservation();
                        oObservaion.SkillId = iSkillId;

                        TextBox txtObservation = tr.FindControl("txtObservation_" + iSkillId) as TextBox;
                        if (txtObservation != null)
                            oObservaion.Observation = txtObservation.Text.Trim();

                        lstObservations.Add(oObservaion);
                    }

                    TextBox txtMarks = tr.FindControl("txtMark_" + iSkillId + "_" + iParameterId) as TextBox;
                    DropDownList ddlmrks = tr.FindControl("ddlMarks_" + iSkillId + "_" + iParameterId) as DropDownList;

                    if (txtMarks != null || ddlmrks != null)
                    {
                        StudentwiseDescriptiveMark oMark = new StudentwiseDescriptiveMark();
                        oMark.SkillId = iSkillId;
                        oMark.ParameterId = iParameterId;
                        if (txtMarks != null)
                        {
                            if (txtMarks.Text == string.Empty)
                                oMark.Mark = 0;
                            else
                                oMark.Mark = txtMarks.Text.ToDecimal();
                        }

                        //if (SchoolBase.Settings.DescriptiveIndicatorMarkType == "G")
                        //{
                             if(ddlmrks != null)                            
                                  oMark.AssignedGradeId = ddlmrks.SelectedValue.ToInt();
                              else
                                oMark.AssignedGradeId = 0;
                        //}
                            lstMarks.Add(oMark);
                        
                    }
                }
            }

            moDescriptiveIndicatorBL.Save(hidYearwiseStudentId.Value.ToInt(), cmbTerm.SelectedValue.ToInt(), base.GenerateXml(lstObservations), base.GenerateXml(lstMarks));

            if (Settings.DescriptiveIndicatorMarkType == "M")
                lblMessage.Text = "Marks saved successfully !!!";
            else
                lblMessage.Text = "Grades saved successfully !!!";

            FillDescriptiveIndicators();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to return back to student list page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            string sQueryString = CommonUtility.EncryptQuerystring("TermId=" + cmbTerm.SelectedValue + "&StdDivId=" + hidStdDivId.Value);
            MasterPage oMaster = this.Master as MasterPage;
            oMaster.RedirectToNextPage("DescriptiveIndicatorsUI.aspx?" + sQueryString);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to read query string.
    /// </summary>
    private void ReadQueryString()
    {
        hidStandardId.Value = QueryString["StandardId"].ToString();
        hidYearwiseStudentId.Value = QueryString["StudentId"].ToString();
        cmbTerm.SelectedValue = QueryString["TermId"].ToString();
        hidStdDivId.Value = QueryString["StdDivId"].ToString();
        cmbTerm_SelectedIndexChanged(cmbTerm, null);
    }

    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        valsum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;

        if (Settings.DescriptiveIndicatorMarkType == "M")
        {
            custValMarks.Enabled = true;
            custValGrade.Enabled = false;
        }
        else
        {
            custValMarks.Enabled = false;
            custValGrade.Enabled = true;
        }
    }

    /// <summary>
    /// This method is used to fill sections.
    /// </summary>
    private void FillSections()
    {
        List<DescriptiveSkill> lstSkills = moDescriptiveIndicatorBL.GetAllSections(hidStandardId.Value.ToInt());
        lstSkills = lstSkills.Where(sk => sk.ParentSkillId == 0).ToList();
        ListSource.FillDropDownList(lstSkills, cmbSection, "Skill", "Id", string.Empty);
    }

    /// <summary>
    /// This methd is used to fill term combobox.
    /// </summary>
    private void FillTerms()
    {
        DataTable oDataTable = StudentwiseRemarkMasterBL.GetTestwiseTerm(miSchoolId);
        ControlUtility.FillDropDownList(oDataTable, ref cmbTerm, "Value_Member", "Display_Member", string.Empty);
    }

    /// <summary>
    /// This method is used to fill descriptive indicators.
    /// </summary>
    private void FillDescriptiveIndicators()
    {
        tblMain.Rows.Clear();

        int iStudentId = 0;
        int iSectionId = 0;
        int iTermId = 0;

        if (!IsPostBack)
        {
            iStudentId = hidYearwiseStudentId.Value.ToInt();
            if (cmbSection.SelectedValue != "")
                iSectionId = cmbSection.SelectedValue.ToInt();
            if(cmbTerm.SelectedValue != "")
                iTermId = cmbTerm.SelectedValue.ToInt();
        }
        else
        {
            iStudentId = Convert.ToInt32(Request.Params[hidYearwiseStudentId.ClientID.Replace("_", "$")]);
            iSectionId = Convert.ToInt32(Request.Params[cmbSection.ClientID.Replace("_", "$")]);
            iTermId = Convert.ToInt32(Request.Params[cmbTerm.ClientID.Replace("_", "$")]);
        }

        DescriptiveIndicator oDescriptiveIndicator = moDescriptiveIndicatorBL.GetAll(iStudentId, iSectionId, iTermId);
        SetStudentDetails(oDescriptiveIndicator.StudentDetails);
        SetSkills(oDescriptiveIndicator);

        if (oDescriptiveIndicator.StudentDetails.IsPublished)
            btnSave.Enabled = false;
    }

    /// <summary>
    /// This method is used to display skills and parameters.
    /// </summary>
    /// <param name="aoDescriptiveIndicator"></param>
    private void SetSkills(DescriptiveIndicator aoDescriptiveIndicator)
    {
        hidMaxValue.Value = aoDescriptiveIndicator.DescriptiveSkills[0].OutOfMark.ToString();

        List<int> lstSkillIds = new List<int>();
        aoDescriptiveIndicator.DescriptiveSkills.ForEach(
            skill =>
            {
                HtmlTableRow tr = new HtmlTableRow();
                tr.ID = "tr_" + skill.Id + "_0";

                AddCell(tr, skill.Skill, "ProgressReportHeader", "Center", 4);
                tblMain.Rows.Add(tr);
                
                HtmlTableRow trParameters = new HtmlTableRow();
                AddCell(trParameters, "Sr. No.", "ProgressReportRow", "right", 1, "width:60px;padding-right:5px");
                AddCell(trParameters, "Descriptors", "ProgressReportRow", "Left", 1);

                if(Settings.DescriptiveIndicatorMarkType =="M")
                    AddCell(trParameters, "Mark/5", "ProgressReportRow", "center", 1, "width:80px");
                else
                    AddCell(trParameters, "Grade", "ProgressReportRow", "center", 1, "width:80px");

                AddCell(trParameters, "Observation", "ProgressReportRow", "center", 1, "width:40%");
                tblMain.Rows.Add(trParameters);



                int iParamIndex = 1;

                var oObservation = aoDescriptiveIndicator.StudentwiseDescriptiveObservations.Where(obs => obs.SkillId == skill.Id).FirstOrDefault();

                TextBox txtObservation = new TextBox();
                txtObservation.ID = "txtObservation_" + skill.Id;
                txtObservation.TextMode = TextBoxMode.MultiLine;
                txtObservation.Width = Unit.Percentage(90);


                if (aoDescriptiveIndicator.StudentDetails.IsPublished)
                    txtObservation.Enabled = false;

                if (oObservation != null)
                    txtObservation.Text = oObservation.Observation;

                int iCnt = aoDescriptiveIndicator.DescriptiveParameters.Count(dp => dp.SkillId == skill.Id);
                txtObservation.Height = Unit.Pixel(iCnt * 25);

                aoDescriptiveIndicator.DescriptiveParameters.Where(dp => dp.SkillId == skill.Id).ToList().ForEach
                    (
                        param =>
                        {
                            HtmlTableRow trParam = new HtmlTableRow();
                            trParam.ID = "tr_" + skill.Id + "_" + param.Id;

                            string sParameter = UpdateParameter(param.Parameter, aoDescriptiveIndicator.StudentDetails.Gender);

                            AddCell(trParam, iParamIndex.ToString(), "ProgressReportParameter", "right", 1, "padding-right:5px");
                            AddCell(trParam, sParameter, "ProgressReportParameter", "Left", 1);

                            TextBox txtParameter = new TextBox();
                            txtParameter.ID = "txtMark_" + skill.Id + "_" + param.Id;
                            txtParameter.Width = Unit.Percentage(100);
                            txtParameter.Style.Add("text-align", "right");
                            txtParameter.Style.Add("Padding-right", "5px");
                            txtParameter.MaxLength = 4;
                            txtParameter.Attributes.Add("onkeypress", "return blockNonNumbers (this, event, true, false);");
                            txtParameter.Attributes.Add("onblur", "extractNumber(this,1,false)");
                            txtParameter.Attributes.Add("onkeyup", "extractNumber(this,1,false)");
                            txtParameter.Attributes.Add("onpaste", "event.returnValue=false");
                            txtParameter.Attributes.Add("ondrop", "event.returnValue=false");


                            DropDownList ddlMarks = new DropDownList();
                            ddlMarks.ID = "ddlMarks_" + skill.Id + "_" + param.Id;
                            ddlMarks.Width = Unit.Pixel(150);


                            DataTable oDataTable = moDescriptiveIndicatorBL.GetAllGradeDetails();
                            ListSource.FillDropDownList(oDataTable, ddlMarks, "Name", "Id", Constants.S_SELECT);
                          
                            if (aoDescriptiveIndicator.StudentDetails.IsPublished)
                                txtParameter.Enabled = false;


                            if (oObservation != null)
                            {
                                var oMarks = aoDescriptiveIndicator.StudentwiseDescriptiveMarks.Where(mk => mk.ObservationId == oObservation.Id && mk.ParameterId == param.Id).FirstOrDefault();
                                if (oMarks != null)
                                    txtParameter.Text = oMarks.Mark.ToString();
                            }
                            if (SchoolBase.Settings.DescriptiveIndicatorMarkType == "G")
                            {
                                if (oObservation != null)
                                {
                                    var ddlmrk = aoDescriptiveIndicator.StudentwiseDescriptiveMarks.Where(mk => mk.ObservationId == oObservation.Id && mk.ParameterId == param.Id).FirstOrDefault();
                                    if (ddlmrk != null)
                                        ddlMarks.SelectedValue = ddlmrk.AssignedGradeId.ToString();
                                }
                            }

                          
                            if (SchoolBase.Settings.DescriptiveIndicatorMarkType == "G")
                            {
                                AddCell(trParam, "", "ProgressReportParameter", "center", 1, "padding-left:2px", ddlMarks);
                            }
                            else
                            {
                                AddCell(trParam, "", "ProgressReportParameter", "center", 1, "padding-left:2px", txtParameter);
                            }
                            if (iParamIndex == 1)
                            {
                                HtmlTableCell oHtmlTableCell = new HtmlTableCell { ColSpan = 1, RowSpan = iCnt, Align = "center" };
                                oHtmlTableCell.Attributes.Add("class", "ProgressReportParameter");
                                oHtmlTableCell.Controls.Add(txtObservation);
                                trParam.Cells.Add(oHtmlTableCell);
                            }

                            tblMain.Rows.Add(trParam);

                            iParamIndex++;
                        }
                    );

                if (aoDescriptiveIndicator.DescriptiveParameters.Any(dp => dp.SkillId == skill.Id))
                    lstSkillIds.Add(skill.Id);

                iParamIndex = 1;

                if (Settings.DescriptiveIndicatorMarkType == "M")
                {
                    HtmlTableRow trTotal = new HtmlTableRow();
                    Label lblTotal = new Label();
                    var dcTotalMarks = (from dp in aoDescriptiveIndicator.DescriptiveParameters
                                        join sd in aoDescriptiveIndicator.StudentwiseDescriptiveMarks
                                        on dp.Id equals sd.ParameterId
                                        where dp.SkillId == skill.Id
                                        select sd.Mark
                                       ).Sum();
                    lblTotal.Text = dcTotalMarks.ToString();

                    AddCell(trTotal, "", "ProgressReportParameter", "right", 1, "padding-left:2px");
                    AddCell(trTotal, "Total", "ProgressReportParameter", "left", 1, "padding-left:2px;font-weight:bold");
                    AddCell(trTotal, "", "ProgressReportParameter", "right", 1, "padding-right:5px;font-weight:bold", lblTotal);
                    AddCell(trTotal, "", "ProgressReportParameter", "right", 1, "padding-left:2px");
                    tblMain.Rows.Add(trTotal);
                }

                HtmlTableRow trFooter = new HtmlTableRow();
                AddCell(trFooter, string.Empty, string.Empty, "Center", 4, "height:10px;");
                tblMain.Rows.Add(trFooter);
            }
            );

        hidSkillIds.Value = string.Join(",", lstSkillIds);
    }

    /// <summary>
    /// This method is used to update parameters.
    /// </summary>
    /// <param name="asParameter"></param>
    /// <param name="acGender"></param>
    /// <returns></returns>
    private string UpdateParameter(string asParameter, char acGender)
    {
        switch (acGender)
        {
            case 'M': asParameter = asParameter.Replace("He/She", "He").Replace("He / She", "He").Replace("his/her", "his").Replace("his / her", "his"); break;
            case 'F': asParameter = asParameter.Replace("He/She", "She").Replace("He / She", "She").Replace("his/her", "her").Replace("his / her", "her"); break;
        }
        return asParameter;
    }

    /// <summary>
    /// This method is used to set student details.
    /// </summary>
    /// <param name="aoStudent"></param>
    private void SetStudentDetails(Student aoStudent)
    {
        lblRollNo.Text = aoStudent.RollNo.ToString();
        lblName.Text = aoStudent.Name;
        lblClass.Text = aoStudent.ClassName;
    } 

    #endregion
}