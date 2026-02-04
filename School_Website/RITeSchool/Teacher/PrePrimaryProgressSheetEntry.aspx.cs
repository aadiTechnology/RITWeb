/*
* This Class is used to show student progress report 
 * rendered HTMLTable to show this progress report including subject group and test types.
 * Author: Shankar Gurav.
 * Date of creation: 28 Jan 2008
 * Date of modification: 2 Feb 2008
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class PrePrimaryProgressSheetEntry : SchoolBase
{
    #region Class Members
    int miStudentId;
    int miTestId;
    int miClassTacherID;
    int miStdDivId;
    #endregion Class Members

    #region Events

    /// <summary>
    /// Overidded method for page initialization.
    /// </summary>
    /// <param name="e"></param>
    override protected void OnInit(EventArgs e)
    {
        try
        {
            base.OnInit(e);            
            GetQueryString();
            if (!IsPostBack)
            {
                CheckIsThisTestAlreadyPublished();
                IsPrePrimaryConfigured();
            }
            else
                ResultContainer.Visible = false;
            PrePrimaryStudentProgress oPrePrimaryStudentProgressDisplay = new PrePrimaryStudentProgress(GridViewScrollContainer);
            oPrePrimaryStudentProgressDisplay.TestId = miTestId;
            oPrePrimaryStudentProgressDisplay.ReadOnly= Boolean.Parse(hidIsReadOnly.Value);
            oPrePrimaryStudentProgressDisplay.PageMode = Constants.PageMode.Edit;
            oPrePrimaryStudentProgressDisplay.SelectedIndexChanged = new EventHandler(cmbGrade_SelectedIndexChanged);
            oPrePrimaryStudentProgressDisplay.ShowProgressSheet(miStudentId);
            btnResult.Enabled = !oPrePrimaryStudentProgressDisplay.ReadOnly;

        }
        catch (BusinessLogic.Exceptions.MarksNotAvailableForResult Ex)
        {
            pnlErrorMsg.Visible = true;
            lblErrorMsg.Text = Ex.Message;
            btnResult.Visible = false;
            hidResultGenrted.Value = Constants.I_ZERO.ToString();
        }
        catch (BusinessLogic.Exceptions.NoResultFound Ex)
        {
            pnlErrorMsg.Visible = true;
            lblErrorMsg.Text = Ex.Message;
            btnResult.Visible = false;
            hidResultGenrted.Value = Constants.I_ZERO.ToString();
            if (moUserRole == Constants.UserRoles.Admin)
                hlnkConf.Visible = true;
        }
       catch (Exception ex)
        {
            hidResultGenrted.Value = Constants.I_ZERO.ToString();
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to intialize the page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ApplyMouseHoverEffect(new List<Button> { btnBack, btnResult });
        }
        catch (BusinessLogic.Exceptions.ResultNotPublished ex)
        {
            pnlErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to generate result and show into 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnResult_Click(object sender, EventArgs e)
    {
        try
        {
            UpdateStudentMarks(miStudentId);
            RedirectToPreviusPage();
        }
        catch (BusinessLogic.Exceptions.NoResultFound)
        {
            ResultContainer.Visible = false;            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to navigate to back page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            RedirectToPreviusPage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

/// <summary>
    /// This method is used to cmbGrade_SelectedIndexChanged
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
    protected void cmbGrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Request.Params["__EVENTTARGET"].Equals("ctl00$MainBody$ddlDefault_Entry"))
            {
                DropDownList oDropDownList = ((DropDownList)sender);
                if (oDropDownList.SelectedIndex > 0)
                {
                    HtmlTable oHtmlTable = (HtmlTable)GridViewScrollContainer.FindControl("tbl_" + miStudentId.ToString());
                    SetDefaultGrade(oHtmlTable, oDropDownList.SelectedValue);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion Events

    #region Private Methods

    /// <summary>
    /// This method is used to set default value to drop down.
    /// </summary>
    /// <param name="oMainHtmlTable"></param>
    /// <param name="sSelectedValue"></param>
    private void SetDefaultGrade(HtmlTable oMainHtmlTable, string sSelectedValue)
    {
        foreach (HtmlTableRow oHtmlTableRow in oMainHtmlTable.Rows)
        {
            foreach (HtmlTableCell oHtmlTableCell in oHtmlTableRow.Cells)
            {
                foreach (Control oControl in oHtmlTableCell.Controls)
                {
                    if (oControl is HtmlTable)
                    {

                        SetDefaultGrade(((HtmlTable)oControl), sSelectedValue);
                    }
                    else
                    {
                        if (oControl is DropDownList)
                        {
                            if (oControl.ID != "Test_" + miStudentId.ToString() &&
                                oControl.ID != "ddlDefault_Entry")
                            {

                                DropDownList oDropDownList = (DropDownList)oControl;
                                oDropDownList.SelectedValue = sSelectedValue;
                            }
                        }
                    }
                }
            }
        }
    }

/// <summary>
/// This method is used to redirect to previous page
/// </summary>
    private void RedirectToPreviusPage()
    {

        string sUrl = "";
        if (hidFrom.Value.Equals("ExamResult"))
        {
            sUrl = "~/Teacher/ClassTeacherTestMarksUI.aspx?";
        }
        else
        {
            sUrl = "~/Teacher/PrePrimaryStudentProgressList.aspx?";
        }

        MasterPage oMasterPage = (MasterPage)this.Master; oMasterPage.RedirectToNextPage(sUrl + HidBackUrl.Value);
    }

    /// <summary>
    /// This method is used to update students marks
    /// </summary>
    /// <param name="iStudentId"></param>
    private void UpdateStudentMarks(int iStudentId)
    {
        HtmlTable oHtmlTable = (HtmlTable)GridViewScrollContainer.FindControl("tbl_" + iStudentId.ToString());
        XmlDocument oDoc = new XmlDocument();
        XmlElement root = oDoc.CreateElement("StudentTestMarksDetails");
        XmlNode oXmlRootNode = oDoc.CreateNode("element", "StudentTestMarksDetails", "");
        string xmlStr = getMarksUpdateXML(iStudentId, oHtmlTable, oDoc, root, oXmlRootNode).InnerXml;
        string sTestComment = getTestComment(iStudentId, oHtmlTable);
        StudentSubjectMarksBL.UpdatePrePrimaryTestMarks(xmlStr, sTestComment, miStudentId, miTestId, miUserId);
    }

    /// <summary>
    /// This method is used to get Test comment
    /// </summary>
    /// <param name="iStudentId"></param>
    /// <param name="oHtmlTable"></param>
    /// <returns></returns>
    private string getTestComment(int iStudentId, HtmlTable oHtmlTable)
    {
        Control oControl = oHtmlTable.FindControl("Test_" + miStudentId.ToString());
        if (oControl is TextBox)
        {
            TextBox oTextBox = (TextBox)oControl;
            if (oTextBox.Text.Trim().Length > 0)
                return oTextBox.Text.Trim();
        }
        return string.Empty;
    }

    /// <summary>
    /// This function sets the form fields according to the query string values.
    /// </summary>
    private void GetQueryString()
    {
	    if (QueryString.Count <= 0)
		    return;
        HidBackUrl.Value = Request.QueryString.ToString();
		if (QueryString["TeacherId"] != null)
		    miClassTacherID = QueryString["TeacherId"].ToInt();
	    if (QueryString["StudentId"] != null)
		    miStudentId = QueryString["StudentId"].ToInt();
		if (QueryString["StandardDivisionId"] != null)
			miStdDivId = QueryString["StandardDivisionId"].ToInt();
	    if (QueryString["TestId"] != null)
		    miTestId = QueryString["TestId"].ToInt();
	    if (QueryString["IsReadOnly"] != null)
		    hidIsReadOnly.Value = QueryString["IsReadOnly"];
	    if (QueryString["IsPublish"] != null
	        && QueryString["IsPublish"] == "Y")
		    pnlSubmitStatus.Visible = true;                
	    if (QueryString["From"] != null)
		    hidFrom.Value = QueryString["From"];
    }

    /// <summary>
    /// This method is used to check that if this test is already published.
    /// </summary>
    private void CheckIsThisTestAlreadyPublished()
    {
        SchoolWiseStanderedDivisionTestMasterBL oSWStdDivTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL(miSchoolId, miAcademicYearId, miStdDivId, miTestId);
        if (oSWStdDivTestMasterBL.StanderedDivisionTest_Id != 0)
            hidIsTestPublished.Value = Constants.C_YES.ToString();
        btnResult.Attributes.Add("onclick", "if(!ConfirmAction()){return false;}");
    }

    /// <summary>
    /// This Method is used Primary Configured
    /// </summary>
    private void IsPrePrimaryConfigured()
    {
        PrePrimaryProgressSheetConfigBL oPrePrimaryProgressSheetConfigBL = new PrePrimaryProgressSheetConfigBL();
        if (!oPrePrimaryProgressSheetConfigBL.IsPrePrimaryConfigured(miStdDivId, miSchoolId, miAcademicYearId))
            throw new BusinessLogic.Exceptions.NoResultFound("Preprimary progress report configuration is not yet done.");
    }

    #endregion Private Methods

    #region XML functions

    /// <summary>
    /// This method is used Gete marks Update XML
    /// </summary>
    /// <param name="aiStudentId"></param>
    /// <param name="oHtmlTable"></param>
    /// <param name="oDoc"></param>
    /// <param name="root"></param>
    /// <param name="oXmlRootNode"></param>
    /// <returns></returns>
    private XmlNode getMarksUpdateXML(int aiStudentId, HtmlTable oHtmlTable, XmlDocument oDoc, XmlElement root, XmlNode oXmlRootNode)
    {
        XmlNode oXmlNode = null;
        foreach (HtmlTableRow oHtmlTableRow in oHtmlTable.Rows)
        {
            foreach (HtmlTableCell oHtmlTableCell in oHtmlTableRow.Cells)
            {
                foreach (Control oControl in oHtmlTableCell.Controls)
                {
                    if (oControl is HtmlTable)
                    {
                        getMarksUpdateXML(aiStudentId, ((HtmlTable)oControl), oDoc, root, oXmlRootNode);
                    }
                    else
                    {
                        if (oControl.ID != "Test_" + miStudentId.ToString())                            
                        {
                            if (oControl is TextBox)
                            {
                                if (((TextBox)oControl).Text.Trim() != string.Empty)
                                {
                                    String sCntrlId = oControl.ID;
                                    Char cSplit = Convert.ToChar("_");
                                    String[] sIds = sCntrlId.Split(cSplit);
                                    oXmlNode = GetNodeTestMarks(aiStudentId, (TextBox)oControl, sIds, ref oDoc);
                                    oXmlRootNode.AppendChild(oXmlNode);
                                }
                            }
                            else if (oControl is DropDownList)
                            {
                                if (oControl.ID != "ddlDefault_Entry")
                                {
                                    if (((DropDownList)oControl).SelectedIndex > 0)
                                    {
                                        String sCntrlId = oControl.ID;
                                        Char cSplit = Convert.ToChar("_");
                                        String[] sIds = sCntrlId.Split(cSplit);
                                        oXmlNode = GetNodeTestGrade(aiStudentId, (DropDownList)oControl, sIds, ref oDoc);
                                        oXmlRootNode.AppendChild(oXmlNode);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);

        // return the string generated.
        return root;
    }

    /// <summary>
    /// This method is used to GetNodeTestGrade
    /// </summary>
    /// <param name="aiStudentId"></param>
    /// <param name="oddlGrade"></param>
    /// <param name="sIds"></param>
    /// <param name="aoDoc"></param>
    /// <returns></returns>
    private XmlNode GetNodeTestGrade(int aiStudentId, DropDownList oddlGrade, String[] sIds, ref XmlDocument aoDoc)
    {
        XmlNode oXmlNode = GetNodeForMarksAssigned(ref aoDoc, aiStudentId, sIds);

        string sAtrrName = "Value";
        XmlAttribute attr = aoDoc.CreateAttribute(sAtrrName);
        if (sIds[1].Equals("Entry"))
            attr.Value = oddlGrade.SelectedValue;
        else
            attr.Value = null;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Description";
        attr = aoDoc.CreateAttribute(sAtrrName);

        if (sIds[1].Equals("Desc"))
            attr.Value = oddlGrade.SelectedValue;
        else
            attr.Value = null;

        attr.Value = null;
        oXmlNode.Attributes.Append(attr);

        return oXmlNode;
    }

    /// <summary>
    /// This method is used to GetNodeTestMarks
    /// </summary>
    /// <param name="aiStudentId"></param>
    /// <param name="oTxtMarks"></param>
    /// <param name="sIds"></param>
    /// <param name="aoDoc"></param>
    /// <returns></returns>
    private XmlNode GetNodeTestMarks(int aiStudentId, TextBox oTxtMarks, String[] sIds, ref XmlDocument aoDoc)
    {
        XmlNode oXmlNode = GetNodeForMarksAssigned(ref aoDoc, aiStudentId, sIds);

        string sAtrrName = "Value";
        XmlAttribute attr = aoDoc.CreateAttribute(sAtrrName);
        if (sIds[1].Equals("Entry"))
            attr.Value = oTxtMarks.Text;
        else
            attr.Value = null;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Description";
        attr = aoDoc.CreateAttribute(sAtrrName);
        if (sIds[1].Equals("Desc") && oTxtMarks.Text.Trim().Length > 0)
            attr.Value = oTxtMarks.Text;
        else
            attr.Value = null;
        oXmlNode.Attributes.Append(attr);

        return oXmlNode;
    }

/// <summary>
    ///  This method is used to GetNodeForMarksAssigned
/// </summary>
/// <param name="aoDoc"></param>
/// <param name="aiStudentId"></param>
/// <param name="sIds"></param>
/// <returns></returns>
    private XmlNode GetNodeForMarksAssigned(ref XmlDocument aoDoc, int aiStudentId, String[] sIds)
    {
        string stxtType = sIds[1];
        int iHeaderId = Convert.ToInt32(sIds[2]);


        const string S_ELEMENT = "element";
        XmlNode oXmlNode = aoDoc.CreateNode(S_ELEMENT, "StudentTestMarksDetail", "");

        string sAtrrName = "School_Id"; //oRow.Cells[iColCount]
        XmlAttribute attr = aoDoc.CreateAttribute(sAtrrName);
        attr.Value = miSchoolId.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Academic_Year_Id";
        attr = aoDoc.CreateAttribute(sAtrrName);
        attr.Value = miAcademicYearId.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "student_Id";
        attr = aoDoc.CreateAttribute(sAtrrName);
        attr.Value = aiStudentId.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Test_Id";
        attr = aoDoc.CreateAttribute(sAtrrName);
        attr.Value = miTestId.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Heading_Id";
        attr = aoDoc.CreateAttribute(sAtrrName);
        attr.Value = iHeaderId.ToString();
        oXmlNode.Attributes.Append(attr);

        return oXmlNode;
    }

    #endregion XML functions

}