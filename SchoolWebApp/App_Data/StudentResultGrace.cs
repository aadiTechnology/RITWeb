/*
* This Class is used to show student Result report 
 * rendered HTMLTable to show this Result.
 * Author: Shankar Gurav.
 * Date of creation: 5 March 2008
 * Date of modification: 5 March 2008
  
 * Modified Date - 11-Feb-2013
 * Modified by - Vipul
 * Modification Description - Code review changes - Use of entity classes and LINQ.
 */
using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using ProgressReportEntities;
using Utility;

/// <summary>
/// Summary description for StudentResult
/// </summary>
public class StudentResultGrace : StudentResult
{

    #region Constant
    //Database tables indexces constants
   
    private string S_SAPARATOR = "_";

    #endregion Constant

    #region Data Member

    Panel GridViewScrollContainer;
    private StringBuilder sSubjectList = new StringBuilder();

    public String SubjectList
    {
        get
        {
            return sSubjectList.Remove(sSubjectList.Length - 2, 2).ToString();
        }
    }
    #endregion

    #region Custructor
        
    public StudentResultGrace(Panel oPanel)
    {
        GridViewScrollContainer = oPanel;
        SetpanelMember(GridViewScrollContainer);
        menumResultType = enumResultType.Annual;
    }

    #endregion Custructor

    #region Overrided method
    
    /// <summary>
    /// this method is used to take a row header for annual result
    /// Overided for the grace row.
    /// </summary>
    /// <returns></returns>
    protected override string[] GetRowHeaders()
    {
        String[] sRowHeader = new String[3];
        sRowHeader[0] = "Marks";
        sRowHeader[1] = "Grace";
        sRowHeader[2] = "Subject Grade";
        return sRowHeader;
    }

    /// <summary>
    /// This method is used to set marks to subject cell for a given test Id.
    /// </summary>
    /// <param name="aiTestId"></param>
    /// <param name="oHTSubjectEntry"></param>
    /// <param name="aiRowIndex"></param>
    protected override void FillSubjectExamMarks(int aiTestId, DictionaryEntry oHTSubjectEntry, int aiRowIndex)
    {
        SubjectDetailsForProgressReport oSubjectDetailsForProgressReport = (SubjectDetailsForProgressReport)oHTSubjectEntry.Value;
        var oSubjectDetails = moStudentProgressReport.SubjectDetails.Cast<FinalResultSubjectDetails>().Where(frsd => frsd.SubjectId == oSubjectDetailsForProgressReport.SubjectId).ToList();
        if (oSubjectDetails.Count > 0 && oSubjectDetails[0].MarksScored != Constants.I_ZERO)
        {
            //If subject has grade then dont append total marks(i.e 12/100)                     
            HtmlTableCell oHtmlTableCell = tblProgress.Rows[aiRowIndex].Cells[oHTSubjectEntry.Key.ToInt()];
            oHtmlTableCell.Align = "center";
            oHtmlTableCell.Attributes.Remove("class");
            oHtmlTableCell.Attributes.Add("class", S_CSS_PRINT_PREFIX + S_CSS_CLSMARKSCELL);
            if(menmPagemode  == Constants.PageMode.Print)
                oHtmlTableCell.InnerHtml = "<B>" + Convert.ToString(oSubjectDetails[0].MarksScored) + "</B>" + "/" + oSubjectDetails[0].SubjectTotalMarks;
            else
                oHtmlTableCell.InnerHtml = "<B>" + Convert.ToString(oSubjectDetails[0].MarksScored) + "</B>" + " / " + oSubjectDetails[0].SubjectTotalMarks;

            SetValuesToCell(tblProgress.Rows[aiRowIndex + 1].Cells[oHTSubjectEntry.Key.ToInt()], oHTSubjectEntry, aiRowIndex, oSubjectDetails[0]);

            oHtmlTableCell = tblProgress.Rows[aiRowIndex + 2].Cells[oHTSubjectEntry.Key.ToInt()];
            oHtmlTableCell.Align = "center";
            oHtmlTableCell.Attributes.Remove("class");
            oHtmlTableCell.Attributes.Add("class", S_CSS_PRINT_PREFIX + S_CSS_CLSMARKSCELL);
            oHtmlTableCell.InnerHtml = oSubjectDetails[0].Grade;
        }
        else
            SetNotApplicableCellValues(tblProgress.Rows[aiRowIndex], oHTSubjectEntry.Key.ToInt(), null);
    }

    /// <summary>
    /// This method is used to set grade to subject cell for a given test Id.
    /// </summary>
    /// <param name="aiTestId"></param>
    /// <param name="oHTSubjectEntry"></param>
    /// <param name="aiRowIndex"></param>
    protected override void FillSubjectExamGrade(int aiTestId, DictionaryEntry oHTSubjectEntry, int aiRowIndex)
    {
        SubjectDetailsForProgressReport oSubjectDetailsForProgressReport = (SubjectDetailsForProgressReport)oHTSubjectEntry.Value;
        var oSubjectsDetails = moStudentProgressReport.SubjectDetails.Cast<FinalResultSubjectDetails>().Where(frsd => frsd.SubjectId == oSubjectDetailsForProgressReport.SubjectId).ToList();
        HtmlTableCell oHtmlTableCell = tblProgress.Rows[aiRowIndex].Cells[oHTSubjectEntry.Key.ToInt()];
        if (oSubjectsDetails.Count > 0)
        {
            oHtmlTableCell.Align = "center";
            oHtmlTableCell.Attributes.Remove("class");
            oHtmlTableCell.Attributes.Add("class", S_CSS_PRINT_PREFIX + S_CSS_CLSMARKSCELL);

            // If subject has grade then dont append total marks(i.e 12/100)                                 
            oHtmlTableCell.InnerHtml = "<B>" + oSubjectsDetails[0].Grade + "</B>";

        }
        else
            SetNotApplicableCellValues(tblProgress.Rows[aiRowIndex], oHTSubjectEntry.Key.ToInt(), null);
    }

    /// <summary>
    /// This method is used to set exam group total of a subject cell for a given test Id.
    /// </summary>
    /// <param name="aiTestId"></param>
    /// <param name="oHTSubjectEntry"></param>
    /// <param name="aiRowIndex"></param>
    protected override void FillSubjectExamGroupTotal(int aiTestId, DictionaryEntry oHTSubjectEntry, int aiRowIndex)
    {
        SubjectDetailsForProgressReport oSubjectDetailsForProgressReport = (SubjectDetailsForProgressReport)oHTSubjectEntry.Value;
        var oParentSubjectDetails = moStudentProgressReport.SubjectDetails.Where(sd => sd.ParentSubjectId == oSubjectDetailsForProgressReport.ParentSubjectId).OrderByDescending(sd => sd.Id).ToList();
        
        if (oParentSubjectDetails.Count > 0 && !oParentSubjectDetails[0].SubjectName.IsNullOrEmpty())
        {
            // Take a group total of a subject.
            var oParentSubjectTotalDetails = moStudentProgressReport.SubjectDetails.Cast<FinalResultSubjectDetails>().Where(frsd => frsd.SubjectId == oSubjectDetailsForProgressReport.ParentSubjectId).ToList();
            if (oParentSubjectTotalDetails.Count > 0)
            {
                HtmlTableCell oHtmlTableCell = tblProgress.Rows[aiRowIndex].Cells[oHTSubjectEntry.Key.ToInt()];
                oHtmlTableCell.Align = "center";
                oHtmlTableCell.Attributes.Remove("class");
                oHtmlTableCell.Attributes.Add("class", S_CSS_PRINT_PREFIX + S_CSS_CLSMARKSCELL);
                oHtmlTableCell.InnerHtml = "<B>" + oParentSubjectTotalDetails[0].MarksScored + "</B>" + " / " + oParentSubjectTotalDetails[0].SubjectTotalMarks;
                oHtmlTableCell.Attributes["class"] = oHtmlTableCell.Attributes["class"];
                oHtmlTableCell = tblProgress.Rows[aiRowIndex + 2].Cells[oHTSubjectEntry.Key.ToInt()];
                oHtmlTableCell.InnerHtml = "<B>" + oParentSubjectTotalDetails[0].Grade;
                oHtmlTableCell.Attributes["class"] = oHtmlTableCell.Attributes["class"];
            }
            else
                FillSubjectExamMarks(aiTestId, oHTSubjectEntry, aiRowIndex);
        }
        else
            FillSubjectExamMarks(aiTestId, oHTSubjectEntry, aiRowIndex);
    }

    /// <summary>
    /// This method is used to add controls and set values to cell.
    /// </summary>
    /// <param name="oHtmlTableCell"></param>
    /// <param name="oHTSubjectEntry"></param>
    /// <param name="aiRowIndex"></param>
    /// <param name="aoSubject"></param>
    private void SetValuesToCell(HtmlTableCell oHtmlTableCell, DictionaryEntry oHTSubjectEntry, int aiRowIndex, FinalResultSubjectDetails aoSubject)
    {
        SubjectDetailsForProgressReport oSubjectDetailsForProgressReport = (SubjectDetailsForProgressReport)oHTSubjectEntry.Value;

        Label olblMarksTotal = new Label();
        
        TextBox oTextBox = new TextBox();
        oTextBox.Text = aoSubject.GraceMarks.ToString();
        oTextBox.ID = CreateIDForCntrl("txt", aiRowIndex, oHTSubjectEntry.Key.ToInt()
                                        , aoSubject, oSubjectDetailsForProgressReport.SubjectCellType);

        oTextBox.MaxLength = 3;
        oTextBox.Width = Unit.Pixel(30);
        oTextBox.CssClass = "ExSmlTxtBoxP";
        oTextBox.Attributes.Add("onkeyup", "javascript:extractNumber(this,0,false);");
        oTextBox.Attributes.Add("onblur", "javascript:Validate(this,'ctl00_MainBody'," + aoSubject.MarksScored + "," + aoSubject.SubjectTotalMarks + "," + aoSubject.SubjectMaxGrace + "," + aoSubject.StandardMaxGrace + ");extractNumber(this,0,false);");
        oTextBox.Attributes.Add("onkeypress", "return blockNonNumbers(this, event, false, false);");
        oTextBox.Attributes.Add("onfocus", "javascript:SetValue(this);");
        oHtmlTableCell.Controls.Add(oTextBox);
        
        oHtmlTableCell.Controls.Add(olblMarksTotal);
    }

    /// <summary>
    /// This method is used to create Id for a textbox added to cell
    /// </summary>
    /// <param name="aiRowIndex"></param>
    /// <param name="asPrefix"></param>
    /// <param name="aiCellIndex"></param>
    /// <param name="aoSubject"></param>
    /// <param name="aiEnumColType"></param>
    /// <returns></returns>
    private String CreateIDForCntrl(String asPrefix, int aiRowIndex, int aiCellIndex, Subject aoSubject, Constants.ReportCellType aiEnumColType)
    {
        sSubjectList.Append(asPrefix + aoSubject.SubjectName + S_SAPARATOR + aoSubject.SubjectId);
        sSubjectList.Append("||");
        return asPrefix + aoSubject.SubjectName + S_SAPARATOR + aoSubject.SubjectId;
    }


    /// <summary>
    /// This method is used to set Student Progress dataSet.
    /// </summary>
    /// <param name="aiStudentId"></param>
    protected override void SetStudentProgressDataSet(int aiStudentId)
    {
        StudentSubjectMarksBL oStudentSubjectMarksBL = new StudentSubjectMarksBL();
        moStudentProgressReport = oStudentSubjectMarksBL.GetStudentGraceResult(miSchoolId, miAcademicYearId, aiStudentId);
    }

    /// <summary>
    /// This method is used to show grace note.
    /// </summary>
    protected override void ShowGraceNote()
    {
        HtmlTable HeaderHtmlTable = new HtmlTable();
        HeaderHtmlTable.EnableViewState = false;
        HeaderHtmlTable.CellPadding = 3;
        HeaderHtmlTable.CellSpacing = 1;
        HeaderHtmlTable.Border = 0;
        if (menmPagemode != Constants.PageMode.Print)
            HeaderHtmlTable.Align = "left";
        HeaderHtmlTable.BgColor = "Black";
        HeaderHtmlTable.Style.Add(HtmlTextWriterStyle.VerticalAlign, HtmlTextWriterStyle.Top.ToString());
        HeaderHtmlTable.Style.Add(HtmlTextWriterStyle.Left, Unit.Pixel(0).ToString());
        GridViewScrollContainer.Controls.Add(HeaderHtmlTable);

        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        CreateHtmlCell(oHtmlTableRow, moStudentProgressReport.GraceMarksMessage, "Lbl10ptB ConfigHeadBG", 1, 1);
        HeaderHtmlTable.Rows.Add(oHtmlTableRow);
    }

    /// <summary>
    /// This method is used to set grace mark not if student is promoted
    /// </summary>
    /// <param name="oHtmlTableRow"></param>
    protected override void SetGraceMark(HtmlTableRow oHtmlTableRow)
    {

    }

    #region UpdateMarks

    /// <summary>
    /// This method is used to update students marks
    /// </summary>
    /// <param name="iStudentId"></param>
    public void UpdateStudentMarks(int aiStudentId)
    {
        HtmlTable oHtmlTable = (HtmlTable)GridViewScrollContainer.FindControl("tbl_" + menumResultType + aiStudentId.ToString());
        string xmlStr = getMarksUpdateXML(aiStudentId, oHtmlTable);
        StudentSubjectMarksBL.UpdateAnnualResultGraceMarks(aiStudentId, miUserId, xmlStr);
        GridViewScrollContainer.Controls.Clear();
    }

    #region XML functions

    private string getMarksUpdateXML(int aiStudentId, HtmlTable oHtmlTable)
    {

        const string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();
        XmlElement root = oDoc.CreateElement("AnnualResultGrace");
        XmlNode oXmlNode = null;
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "AnnualResultGrace", string.Empty);
        foreach (HtmlTableRow oHtmlTableRow in oHtmlTable.Rows)
        {
            foreach (HtmlTableCell oHtmlTableCell in oHtmlTableRow.Cells)
            {
                foreach (Control oControl in oHtmlTableCell.Controls)
                {
                    if (oControl is TextBox)
                    {
                        String sCntrlId = oControl.ID;
                        Char cSplit = Convert.ToChar(S_SAPARATOR);
                        String[] sIds = sCntrlId.Split(cSplit);
                        oXmlNode = GetNodeMarks(aiStudentId, (TextBox)oControl, sIds, ref oDoc);
                        oXmlRootNode.AppendChild(oXmlNode);
                    }
                }
            }
        }
        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);

        // return the string generated.
        return root.InnerXml;
    }

    private XmlNode GetNodeTestGrade(int aiStudentId, DropDownList oddlGrade, String[] sIds, ref XmlDocument aoDoc)
    {
        return aoDoc.CreateNode("element", "AnnualStudentMarksDetail", string.Empty);       
    }

    private XmlNode GetNodeMarks(int aiStudentId, TextBox oTxtMarks, String[] sIds, ref XmlDocument aoDoc)
    {
        string sSubjectName = Convert.ToString(sIds[0]);
        int iSubjectId = sIds[1].ToInt();        

        const string S_ELEMENT = "element";
        XmlNode oXmlNode = aoDoc.CreateNode(S_ELEMENT, "AnnualStudentMarksDetail", string.Empty);

        string sAtrrName = "StudentId";
        XmlAttribute attr = aoDoc.CreateAttribute(sAtrrName);
        attr.Value = aiStudentId.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "SubjectId";
        attr = aoDoc.CreateAttribute(sAtrrName);
        attr.Value = iSubjectId.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "GraceMarks";
        attr = aoDoc.CreateAttribute(sAtrrName);
        attr.Value = oTxtMarks.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        return oXmlNode;
    }

    #endregion XML functions

    #endregion UpdateMarks

    #endregion Overrided method

}
