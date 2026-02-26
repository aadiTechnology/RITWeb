    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SchoolEntities;
using System.Web.UI.HtmlControls;
using Utility;
using BusinessLogic.Exceptions;
using System.Reflection;
using System.Web.Script.Serialization;

public partial class ObservationGradeAssignmentUI : SchoolBase
{
    #region Constant(s)
    
    const string S_PARAMETERS = "Parameters"; 

    #endregion

    #region Data Member(s)
    
    private ObservationDetailsBL moObservationDetailsBL;
    private List<StudentBasicDetails> mlstStudents; 

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
            moObservationDetailsBL = new ObservationDetailsBL(miSchoolId, miAcademicYearId, miUserId);
            ReadQueryString();
            if (Page.Request.Params.Get("__EVENTTARGET") != null)
            {
                if (btnSave.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")) ||
                    btnSubmit.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")))
                {
                    FillObservations();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill up observation details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {   
                FillObservations();
                SetJavascriptAttributes();
                SetPostbackURL();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is sued to save observation details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Save();
            base.DisplayMessage("Observation details saved successfully !!!", false, tdMessage);
            FillObservations();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is sued to submit observation details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            moObservationDetailsBL.Submit(hidTestId.Value.ToInt(), hidSubjectId.Value.ToInt(), hidStdDivId.Value.ToInt(), 1);
            base.DisplayMessage("Observation details submitted successfully !!!", false, tdMessage);
            FillObservations();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This method is sued to Unsubmit observation details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUnSubmit_Click(object sender, EventArgs e)
    {
        try
        {            
            moObservationDetailsBL.Submit(hidTestId.Value.ToInt(), hidSubjectId.Value.ToInt(), hidStdDivId.Value.ToInt(), 0);
            base.DisplayMessage("Observation details Unsubmitted successfully !!!", false, tdMessage);
            FillObservations();
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
        if (IsPostBack)
        {
            hidTestId.Value = Convert.ToString(Request.Params[hidTestId.ClientID.Replace("_", "$")]);
            hidSubjectId.Value = Convert.ToString(Request.Params[hidSubjectId.ClientID.Replace("_", "$")]);
            hidStdDivId.Value = Convert.ToString(Request.Params[hidStdDivId.ClientID.Replace("_", "$")]);
            hidFilterStdDivId.Value = Convert.ToString(Request.Params[hidFilterStdDivId.ClientID.Replace("_", "$")]);
        }
        else
        {
            hidTestId.Value = QueryString["TestId"];
            hidStdDivId.Value = QueryString["StandardDivisionId"];
            hidSubjectId.Value = QueryString["SubjectId"];
            hidTeacherId.Value = QueryString["TeacherId"];
            hidIsClassTeacher.Value = QueryString["IsClassTeacher"];
            hidFilterStdDivId.Value = QueryString["FilteredStdDivId"];            
        }
    }

    /// <summary>
    /// This method is used to set post back URL.
    /// </summary>
    private void SetPostbackURL()
    {
        string sQueryString = CommonUtility.EncryptQuerystring("TeacherId=" + hidTeacherId.Value + "&TestId=" + hidTestId.Value + "&IsClassTeacher=" + hidIsClassTeacher.Value + "&FilteredStdDivId=" + hidFilterStdDivId.Value);
        btnBack.PostBackUrl = "~/RITeSchool/Teacher/AssignGradesUI.aspx?" + sQueryString;
        btnNoteBack.PostBackUrl = "~/RITeSchool/Teacher/AssignGradesUI.aspx?" + sQueryString;
    }

    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        valsum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        base.ApplyMouseHoverEffect(new List<Button> { btnSave, btnBack, btnSubmit });
        //hidApplyGradeParamFilter.Value = (miSchoolId == Constants.SchoolId.SNS.ToInt()) ? "Y" : "N";
    }

    /// <summary>
    /// this method is used to fill observation details.
    /// </summary>
    private void FillObservations()
    {
        mlstStudents = moObservationDetailsBL.GetObservationDetails(hidTestId.Value.ToInt(), hidStdDivId.Value.ToInt(), hidSubjectId.Value.ToInt());

        if (mlstStudents.Count > 0)
        {
            if (moObservationDetailsBL.Parameters.Count == 0)
            {
                tblNote.Visible = true;
                tblData.Visible = false;
            }
            else
            {
                tblNote.Visible = false;
                tblData.Visible = true;
                tblParameters.Rows.Clear();
                SetLegends();
                FillSkills();
                FillHeaders();
                FillHeaderControls();
                FillStudents();
                SetButtonState();

                ViewState[S_PARAMETERS] = moObservationDetailsBL.Parameters;

                var jsSerializer = new JavaScriptSerializer();
                hidRemarks.Value = jsSerializer.Serialize(moObservationDetailsBL.Remarks);
            }
        }
        else
        {
            spnNote.InnerText = "This Optional Subject is not marked for any student.";
            tblNote.Visible = true;
            tblData.Visible = false;
        }
    }

    /// <summary>
    /// This method is sued to set button state.
    /// </summary>
    private void SetButtonState()
    {
        if (moObservationDetailsBL.IsSubmitted == true)
        {
            btnSave.Enabled = false;
            btnSubmit.Enabled = false;

            btnUnSubmit.Visible = true;
            btnUnSubmit.Enabled = true;            
        }
        else
        {
            btnSave.Enabled = true;
            if (moObservationDetailsBL.Observations.Count > 0)
                btnSubmit.Enabled = true;
           
            btnUnSubmit.Visible = true;
            btnUnSubmit.Enabled = false;            
        }

        if (hidIsClassTeacher.Value == Constants.S_YES)
        {
            btnSubmit.Visible = false;
            btnUnSubmit.Visible = false;
            btnSave.Enabled = true;
        }

        if (moObservationDetailsBL.IsPublished == true)
        {
            btnSave.Enabled = false;
            btnSubmit.Enabled = false;
            btnUnSubmit.Enabled = false;
        }
    }

    /// <summary>
    /// This method is used to display skills.
    /// </summary>
    private void FillSkills()
    {
        HtmlTableRow trHeader = new HtmlTableRow();
        this.AddTableCell(trHeader, string.Empty, "ClsProgressGridTestHeader", "right");
        this.AddTableCell(trHeader, string.Empty, "ClsProgressGridTestHeader", "left", 2, "width:200px");

        moObservationDetailsBL.Skills.OrderBy(skl => skl.SortOrder).ToList().ForEach
            (
             skill =>
             {
                 int iCount = moObservationDetailsBL.Parameters.Where(prm => prm.SkillId == skill.Id).Count();
                 this.AddTableCell(trHeader, skill.Name, "ClsProgressGridTestHeader", "center", iCount);
             }

        );

        tblParameters.Rows.Add(trHeader);
    }

    /// <summary>
    /// This method is used to display header controls.
    /// </summary>
    private void FillHeaderControls()
    {
        HtmlTableRow trRow = new HtmlTableRow();
        this.AddTableCell(trRow, string.Empty, "ClsProgressGridTestHeader", "right");
        this.AddTableCell(trRow, string.Empty, "ClsProgressGridTestHeader", "left", 2, "width:200px");

        moObservationDetailsBL.Skills.OrderBy(skl => skl.SortOrder).ToList().ForEach
            (
             skill =>
             {
                 moObservationDetailsBL.Parameters.Where(prm => prm.SkillId == skill.Id).OrderBy(prm => prm.SortOrder).ToList().ForEach
                     (
                         parameter =>
                         {
                             Control ctrl = new Control();

                             if (parameter.ControlTypeId == 1 || parameter.ControlTypeId == 2 || parameter.ControlTypeId == 3)
                             {
                                 DropDownList ddl = new DropDownList();
                                 ddl.ID = "cmb_" + parameter.Id;
                                 ddl.CssClass = "smlCombo";
                                 ListSource.FillDropDownList(moObservationDetailsBL.Grades, ddl, "ShortName", "Id", Constants.S_SELECT);
                                 ddl.Attributes.Add("onchange", "SelectAll(this)");

                                 if (moObservationDetailsBL.IsSubmitted)
                                 {
                                     if (hidIsClassTeacher.Value == Constants.S_YES)
                                         ddl.Enabled = true;
                                     else
                                         ddl.Enabled = false;
                                 }
                                 else
                                     ddl.Enabled = true;

                                 ctrl.Controls.Add(ddl);
                             }
                             if (parameter.ControlTypeId == 2 || parameter.ControlTypeId == 3)
                             {
                                 TextBox txtRemark = new TextBox();
                                 txtRemark.ID = "txtHeaderRemark_" + parameter.Id;
                                 txtRemark.CssClass = "LrgTxtBox";
                                 txtRemark.TextMode = TextBoxMode.MultiLine;
                                 txtRemark.Height = Unit.Pixel(50);
                                 txtRemark.Attributes.Add("onchange", "ChangeAllRemark(this," + parameter.Id + ");");

                                 if (moObservationDetailsBL.IsSubmitted)
                                 {
                                     if (hidIsClassTeacher.Value == Constants.S_YES)
                                         txtRemark.Enabled = true;
                                     else
                                         txtRemark.Enabled = false;
                                 }
                                 else
                                     txtRemark.Enabled = true;

                                 ctrl.Controls.Add(txtRemark);
                             }

                             this.AddTableCell(trRow, string.Empty, "ClsProgressGridTestHeader", "center", 1, "width:50px", ctrl);
                         }

                     );
             }

        );
        tblParameters.Rows.Add(trRow);
    }
        
    /// <summary>
    /// This method is used to display legends.
    /// </summary>
    private void SetLegends()
    {
        lblClass.Text = moObservationDetailsBL.ClassName;
        lblExam.Text = moObservationDetailsBL.TestName;
        lblSubject.Text = moObservationDetailsBL.SubjectName;
    }

    /// <summary>
    /// This method is used to display student list.
    /// </summary>
    private void FillStudents()
    {
        mlstStudents.OrderBy(std => std.RollNo).ToList().ForEach
            (
            student =>
            {
                HtmlTableRow trRow = new HtmlTableRow();
                trRow.ID = "tr_" + student.YearwiseStudentId;
                this.AddTableCell(trRow, student.RollNo.ToString(), "ClsMarksCell", "right", 1, "width:100px");
                this.AddTableCell(trRow, student.StudentName, "ClsMarksCell", "left", 2, "width:200px;white-space:nowrap");

                moObservationDetailsBL.Skills.OrderBy(skl => skl.SortOrder).ToList().ForEach
                    (
                     skill =>
                     {
                         moObservationDetailsBL.Parameters.Where(prm => prm.SkillId == skill.Id).OrderBy(prm => prm.SortOrder).ToList().ForEach
                             (
                                 parameter =>
                                 {
                                     Control ctrl = new Control();
                                     var oGrade = moObservationDetailsBL.Observations.Where(obs => obs.StudentId == student.YearwiseStudentId && obs.ParameterId == parameter.Id).FirstOrDefault();
                                     
                                     if (parameter.ControlTypeId == 1 || parameter.ControlTypeId == 2 ||parameter.ControlTypeId == 3)
                                     {
                                         DropDownList ddl = new DropDownList();
                                         ddl.ID = "cmb_" + student.YearwiseStudentId + "_" + parameter.Id;
                                         ddl.CssClass = "smlCombo";
                                         ListSource.FillDropDownList(moObservationDetailsBL.Grades, ddl, "ShortName", "Id", Constants.S_SELECT);
                                         ddl.Attributes.Add("onchange", "SetColor(this)");
                                         ddl.Style.Add("margin-top", "5px");
                                         ddl.ToolTip = student.StudentName + " [" + parameter.Parameter + "]";

                                         if (oGrade != null)
                                             ddl.SelectedValue = oGrade.GradeId.ToString();

                                         if (moObservationDetailsBL.IsSubmitted)
                                         {
                                             if (hidIsClassTeacher.Value == Constants.S_YES)
                                                 ddl.Enabled = true;
                                             else
                                                 ddl.Enabled = false;
                                         }
                                         else
                                             ddl.Enabled = true;

                                         ctrl.Controls.Add(ddl);
                                     }

                                     if (parameter.ControlTypeId == 2 || parameter.ControlTypeId == 3)
                                     {
                                         TextBox txtRemark = new TextBox();
                                         txtRemark.ID = "txtRemark_" + student.YearwiseStudentId + "_" + parameter.Id;
                                         txtRemark.CssClass = "LrgTxtBox";
                                         txtRemark.TextMode = TextBoxMode.MultiLine;
                                         txtRemark.Height = Unit.Pixel(50);
                                         txtRemark.Style.Add("margin-bottom", "5px");
                                         txtRemark.ToolTip = student.StudentName + " [" + parameter.Parameter + "]";
                                             
                                         Button btnPlus = new Button();
                                         btnPlus.ID = "btnPlus_" + student.YearwiseStudentId + "_" + parameter.Id;
                                         btnPlus.Text = "+";
                                         btnPlus.CssClass = "smlBtn";
                                         btnPlus.Style.Add("margin-left", "5px");
                                         //btnPlus.Attributes.Add("onclick", "return FillRemarksDynamic(this," + skill.Id + "," +  parameter.Id + ",'" + txtRemark.ClientID + "');");

                                         btnPlus.Attributes.Add("onclick", "FillRemarks(this," + skill.Id + ",'" + txtRemark.ClientID + "'); return false;");

                                         if (oGrade != null)
                                             txtRemark.Text = oGrade.Remark;

                                         if (moObservationDetailsBL.IsSubmitted)
                                         {
                                             if (hidIsClassTeacher.Value == Constants.S_YES)
                                             {
                                                 txtRemark.Enabled = true;
                                                 btnPlus.Enabled = true;
                                             }
                                             else
                                             {
                                                 txtRemark.Enabled = false;
                                                 btnPlus.Enabled = false;
                                             }
                                         }
                                         else
                                         {
                                             txtRemark.Enabled = true;
                                             btnPlus.Enabled = true;
                                         }

                                         ctrl.Controls.Add(btnPlus);

                                         ctrl.Controls.Add(txtRemark);
                                     }

                                     this.AddTableCell(trRow, string.Empty, "ClsMarksCell", "center", 1, "width:250px", ctrl);
                                 }

                             );
                     }

                );

                tblParameters.Rows.Add(trRow);
            }

        );
    }

    /// <summary>
    /// This method is used to display header.
    /// </summary>
    private void FillHeaders()
    {
        HtmlTableRow trHeader = new HtmlTableRow();
        this.AddTableCell(trHeader, "Roll No.", "ClsProgressGridTestHeader", "right", 1, "white-space:nowrap;width:100px");
        this.AddTableCell(trHeader, "Student Name", "ClsProgressGridTestHeader", "left", 2, "width:200px");

        moObservationDetailsBL.Skills.OrderBy(skl => skl.SortOrder).ToList().ForEach
            (
             skill =>
             {
                 moObservationDetailsBL.Parameters.Where(prm => prm.SkillId == skill.Id).OrderBy(prm => prm.SortOrder).ToList().ForEach
                     (
                         parameter =>
                         {
                             this.AddTableCell(trHeader, parameter.Parameter, "ClsProgressGridTestHeader", "center", 1, "width:50px");
                         }

                     );
             }

        );

        tblParameters.Rows.Add(trHeader);
    }

    /// <summary>
    /// This method is used to add cell into given row.
    /// </summary>
    /// <param name="aoHtmlTableRow"></param>
    /// <param name="asCaption"></param>
    /// <param name="asClass"></param>
    /// <param name="asAlign"></param>
    /// <param name="aiColSpan"></param>
    /// <param name="asStyles"></param>
    /// <param name="aoControl"></param>
    private void AddTableCell(HtmlTableRow aoHtmlTableRow, string asCaption, string asClass, string asAlign = "Center", int aiColSpan = 1, string asStyles = "", Control aoControl = null)
    {
        string[] stl;
        HtmlTableCell oHtmlTableCell = new HtmlTableCell { InnerHtml = asCaption, Align = asAlign, ColSpan = aiColSpan };
        oHtmlTableCell.Attributes.Add("class", asClass);
        if (aoControl != null)
            oHtmlTableCell.Controls.Add(aoControl);

        oHtmlTableCell.Style.Add("Padding-Left", "5pt");

        if (asStyles != string.Empty)
        {
            string[] sArrStyles = asStyles.Split(';');
            sArrStyles.ToList().ForEach
                (
                    style =>
                    {
                        if (style.Trim() != string.Empty)
                        {
                            stl = style.Split(':');
                            if (stl[0] != string.Empty && stl[1] != string.Empty)
                                oHtmlTableCell.Style.Add(stl[0], stl[1]);
                            stl = null;
                        }
                    });
        }

        aoHtmlTableRow.Cells.Add(oHtmlTableCell);
    }

    /// <summary>
    /// This method is used to save grades.
    /// </summary>
    private void Save()
    {
        List<ObservationDetails> lstObservations = new List<ObservationDetails>();
        List<ObservationParameter> lstParameters;
        if (ViewState[S_PARAMETERS] != null)
            lstParameters = ViewState[S_PARAMETERS] as List<ObservationParameter>;
        else
        {
            moObservationDetailsBL.GetObservationDetails(hidTestId.Value.ToInt(), hidStdDivId.Value.ToInt(), hidSubjectId.Value.ToInt());
            lstParameters = moObservationDetailsBL.Parameters;
        }

        foreach (HtmlTableRow tr in tblParameters.Rows)
        {
            if (!string.IsNullOrEmpty(tr.ID))
            {
                int iStudentId = tr.ID.Split('_')[1].ToInt();
                lstParameters.ForEach
                    (
                        parameter =>
                        {
                            string sRemark = "";
                            TextBox txtRemark = tr.FindControl("txtRemark_" + iStudentId + "_" + parameter.Id) as TextBox;
                            if (txtRemark != null)
                                sRemark = txtRemark.Text.Trim();

                            DropDownList ddl = tr.FindControl("cmb_" + iStudentId + "_" + parameter.Id) as DropDownList;
                            if (ddl != null)
                            {
                                lstObservations.Add
                                    (
                                        new ObservationDetails
                                        {
                                            StudentId = iStudentId,
                                            ParameterId = parameter.Id,
                                            GradeId = ddl.SelectedValue.ToInt(),
                                            Remark = sRemark
                                        }
                                    );
                            }
                        }

                    );
            }
        }

        moObservationDetailsBL.Save(hidTestId.Value.ToInt(), hidSubjectId.Value.ToInt(), hidStdDivId.Value.ToInt(), base.GenerateXml(lstObservations));
    } 

    #endregion
}