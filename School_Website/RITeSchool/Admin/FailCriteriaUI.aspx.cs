// File Name     : FailCriteriaUI.aspx.cs
// Modified By   : Amit 
// Modified Date : 14/09/2009
// Description   : This class is used save fail criteria for class.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class FailCriteriaUI : SchoolBase
{
    #region "Constants"

    const int I_DATA_KEY_STANDARD_ID = 0;
    const int I_DATA_KEY_CONFIG_ID = 1;
    const int I_DATA_KEY_TOT_SUBJ = 2;
    const int I_DATA_KEY_FAIL_CRITERIA_NOT_APP = 3;

    #endregion "Constants"

    #region "Events"

    /// <summary>
    /// This event is used to fill fail criteria grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)    
    {
        try
        {            
            if (!IsPostBack)
            {
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                DesignSettingAccordingLanguage();
                if(CheckPreCondition())
                {
                    FillFailCriteriaDetails();
                    SetFocusOnFirstDataEntryCntl();
                    InitializePage();                 
                }
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                DesignSettingAccordingLanguage();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to move on previous page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgbtnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Exam_Related)));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save fail criteria for different classes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string sIsConfig = string.Empty;            
            Hashtable oHChgStds = GetListOfStdsChanged(grdvwFailCriteria);
            PassFailCriteriaCollectionBL.GetDependanciesForCriteria(oHChgStds, miAcademicYearId);
            string StandardIdXml = GenrateXML(oHChgStds);
            PassFailCriteriaCollectionBL.IsExamsPublishedForStandards(miSchoolId, miAcademicYearId, StandardIdXml);
            string sFailCriteriaDetails = GetXMLStringFormatFromGridRows(grdvwFailCriteria, "PassFailDetails", "PassFailDetail");
            PassFailCriteriaBL oPassFailCriteriaBL = new PassFailCriteriaBL();
            oPassFailCriteriaBL.InsertPassFailCriteria(miSchoolId, miAcademicYearId, miUserId, sFailCriteriaDetails);

            sIsConfig = ReadQuerystring();
            if (sIsConfig != "Y")
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.FailCriteria));

            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Exam_Related)));
        }
        catch (SqlException)
        {
            lblErr.Visible = true;
            lblErr.Text = Resources.LocalizedResources.ErrorInSavingFailCriteriaForDiffClass;
            FillFailCriteriaDetails();
        }

        catch (BusinessLogic.Exceptions.ReferenceExceptions)
        {
            lblErr.Visible = true;
            lblErr.Text = Resources.LocalizedResources.ErrorInSavingFailCriteriaForDiffClass;
            FillFailCriteriaDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill grade name in combo for respective standard.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwFailCriteria_RowDataBound(object sender, GridViewRowEventArgs e)
    
    {
        try
        {
            if (e.Row.RowIndex >= 0)
            {
                int iRowIndex = Convert.ToInt32(e.Row.RowIndex);
                int iStandardId = Convert.ToInt32(grdvwFailCriteria.DataKeys[iRowIndex][I_DATA_KEY_STANDARD_ID].ToString()); ;

                DropDownList cmbGrades1 = (DropDownList)e.Row.Cells[2].FindControl("cmbGrades");
                TextBox txtNoOfSubject = (TextBox)e.Row.Cells[2].FindControl("txtNoOfSubjects");
                MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
                oMasterDataCollectionBL.FillGradeNameComboxForSelectedStandard(miSchoolId, miAcademicYearId, iStandardId, ref cmbGrades1);
                CheckBox chkIsFailCriteriaNotApplicable = e.Row.Cells[1].FindControl("chkIsFailCriteriaNotApp") as CheckBox;
                chkIsFailCriteriaNotApplicable.Attributes.Add("onclick", "SetControlEnability(" + chkIsFailCriteriaNotApplicable.ClientID + ", " + iRowIndex + ");");
                cmbGrades1.SelectedValue = grdvwFailCriteria.DataKeys[iRowIndex][I_DATA_KEY_CONFIG_ID].ToString();
                //IsFailCriteriaNotApplicable
                if (grdvwFailCriteria.DataKeys[iRowIndex][I_DATA_KEY_FAIL_CRITERIA_NOT_APP].ToString() == Constants.C_YES.ToString())
                {
                    chkIsFailCriteriaNotApplicable.Checked = true;
                    cmbGrades1.Enabled = txtNoOfSubject.Enabled = false;

                }
                else if (grdvwFailCriteria.DataKeys[iRowIndex][I_DATA_KEY_FAIL_CRITERIA_NOT_APP].ToString() == Constants.C_NO.ToString() && cmbGrades1.SelectedValue != "0")
                {
                    chkIsFailCriteriaNotApplicable.Checked = false;
                    cmbGrades1.Enabled = txtNoOfSubject.Enabled = true;
                }
                else {
                    chkIsFailCriteriaNotApplicable.Checked = false;
                    cmbGrades1.Enabled = txtNoOfSubject.Enabled = true;
                    txtNoOfSubject.Text = string.Empty;
                }
                if (txtNoOfSubject.Text == "0")
                    txtNoOfSubject.Text = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion "Events"

    #region "Private Methods"

    /// <summary>
    /// This method is used to decrypt encrypted querystring.
    /// </summary>
    private string ReadQuerystring()
    {
        try
        {
	        if (Request.QueryString.ToString() != Constants.S_EMPTY_STRING)
		        return QueryString["Is_Configured"];
        }
        catch (Exception)
        {
            MasterPage oMasterPage = (MasterPage)this.Master; 
            oMasterPage.RedirectToNextPage(Constants.S_PAGE_ERROR);
        }
        
		return String.Empty;
    }

    /// <summary>
    /// This method is used to set focus on first combo in grid. 
    /// </summary>
    private void SetFocusOnFirstDataEntryCntl()
    {
        DropDownList cmbgradeNames = (DropDownList)grdvwFailCriteria.Rows[0].Cells[2].FindControl("cmbGrades");
        cmbgradeNames.Focus();
    }

    /// <summary>
    /// This method is used to check the preconditions of Configured Subjects for Subject Group criteria.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.FailCriteria);

        if (sLinks.Equals(""))
        {
            divErr.Visible = false;
            bReturn = true;
        }
        else
        {
            divErr.InnerHtml = sLinks;
            VisibleOrHideControls();
        }
        return bReturn;
    }

    /// <summary>
    /// This method is used to fill fail criteria grid.
    /// </summary>
    private void FillFailCriteriaDetails()
    {
        DataTable oDTCriteria;
        PassFailCriteriaCollectionBL oPassFailCriteriaCollectionBL = new PassFailCriteriaCollectionBL();
        oDTCriteria = oPassFailCriteriaCollectionBL.GetAllPassFailDetails(miSchoolId, miAcademicYearId);

        grdvwFailCriteria.DataSource = oDTCriteria.DefaultView;
        grdvwFailCriteria.DataBind();
    }

    /// <summary>
    /// This method is used to fill details of fail criteria of standards into hashtable,
    /// which is used to check dependancy criteria of standard with exam result. 
    /// </summary>
    /// <param name="aoGridView"></param>
    /// <returns></returns>
    private Hashtable GetListOfStdsChanged(GridView aoGridView)
    {
        Hashtable oHChgStds = new Hashtable();
        string sGradeId = "";
        string sOriginalGradeId = "";
        string sTotalNumSubj = "";
        string sOrgTotSubj = "";
        int iKey = 0;
        string sValue = "";
        for (int iRowCount = 0; iRowCount <= aoGridView.Rows.Count - 1; iRowCount++)
        {
            DropDownList ddlGrade= (DropDownList)aoGridView.Rows[iRowCount].Cells[2].FindControl("cmbGrades");
            CheckBox chkIsFailCriteriaNotApp = (CheckBox)aoGridView.Rows[iRowCount].Cells[1].FindControl("chkIsFailCriteriaNotApp");
            sGradeId = ddlGrade.SelectedValue;
            sOriginalGradeId = Convert.ToString(aoGridView.DataKeys[iRowCount][I_DATA_KEY_CONFIG_ID]);
            sTotalNumSubj = chkIsFailCriteriaNotApp.Checked ? "0" : ((TextBox)aoGridView.Rows[iRowCount].Cells[3].FindControl("txtNoOfSubjects")).Text;
            sOrgTotSubj = Convert.ToString(aoGridView.DataKeys[iRowCount][I_DATA_KEY_TOT_SUBJ]);
            if (!(sGradeId.Equals("0") && (sOriginalGradeId.Equals(""))) || sOriginalGradeId.Equals("0") || chkIsFailCriteriaNotApp.Checked == true )
            {
                if ((!(sGradeId.Equals(sOriginalGradeId))) || (!(sTotalNumSubj.Equals(sOrgTotSubj))) || (sOriginalGradeId.Equals("0") && ddlGrade.Enabled == true))
                {
                    iKey = Convert.ToInt32(aoGridView.DataKeys[iRowCount][I_DATA_KEY_STANDARD_ID]);
                    sValue = aoGridView.Rows[iRowCount].Cells[0].Text;
                    oHChgStds.Add(iKey, sValue);
                }
            }
        }
        return oHChgStds;
    }

    /// <summary>
    /// This method is used to get XML of fail criteria details which used to save fail criteria. 
    /// </summary>
    /// <param name="aoGridView"></param>
    /// <param name="asRootElementName"></param>
    /// <param name="asElementName"></param>
    /// <returns></returns>
    public string GetXMLStringFormatFromGridRows(GridView aoGridView, string asRootElementName, string asElementName) //, int aiNumOfsubjects
    {
        const string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();
        string sAtrrName;
        XmlAttribute attr;
        // Create a root level element.
        XmlElement root = oDoc.CreateElement(asRootElementName);
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, asRootElementName, "");

        // Loop through all the grid rows.
        for (int iRowCount = 0; iRowCount <= aoGridView.Rows.Count - 1; iRowCount++)
        {
            // Create root xml element.
            XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, asElementName, "");
            GridViewRow oRow = aoGridView.Rows[iRowCount];

            // Looop through all the columns for the row.
            for (int iColCount = 0; iColCount <= oRow.Cells.Count - 1; iColCount++)
            {
                sAtrrName = "Marks_Grades_configuration_Id";
                attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = ((DropDownList)oRow.Cells[2].FindControl("cmbGrades")).Text;
                oXmlNode.Attributes.Append(attr);

                sAtrrName = "School_Id";//this is for schoolId parameter
                attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = Convert.ToString(miSchoolId);
                oXmlNode.Attributes.Append(attr);

                sAtrrName = "Academic_Year_Id";//this is for Academic_Year_Id parameter
                attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = Convert.ToString(miAcademicYearId);
                oXmlNode.Attributes.Append(attr);

                sAtrrName = "Standard_Id";//this is for Standard_Id parameter
                attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = aoGridView.DataKeys[iRowCount][I_DATA_KEY_STANDARD_ID].ToString();
                oXmlNode.Attributes.Append(attr);

                sAtrrName = "Number_Of_Subjects";//this is for Number_Of_Subjects parameter
                attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = ((TextBox)oRow.Cells[3].FindControl("txtNoOfSubjects")).Text;
                oXmlNode.Attributes.Append(attr);

                sAtrrName = "IsFailCriteriaNotApplicable";//this is for IsFailCriteriaNotApplicable parameter
                attr = oDoc.CreateAttribute(sAtrrName);
                bool IsFailNotCriteriaNotApp = ((CheckBox)oRow.Cells[1].FindControl("chkIsFailCriteriaNotApp")).Checked;
                    attr.Value = IsFailNotCriteriaNotApp == true ? "Y" : "N";
                oXmlNode.Attributes.Append(attr);

                //check is grade is selected or not and No. of subjects should be entered,
                //if grade name is selected  and no. of subjects added then only
                // child node is added to root node.
                if (((oXmlNode.Attributes[0].Value != "0" && oXmlNode.Attributes[4].Value != "") && IsFailNotCriteriaNotApp == false)
                    || ((oXmlNode.Attributes[0].Value == "0" && oXmlNode.Attributes[4].Value == "") && IsFailNotCriteriaNotApp == true))
                {
                    // Add the node to root node.
                    oXmlRootNode.AppendChild(oXmlNode);
                }
            }
        }
        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);

        // return the string generated.
        return root.InnerXml;
    }
    
    /// <summary>
    /// This method is used to hide page controls if precondition fails.
    /// </summary>
    private void VisibleOrHideControls()
    {
        btnSave.Visible = false;
        grdvwFailCriteria.Visible = false;
        tdCancel.Align = "Center";
        imgbtnCancel.Text = "Back";
    }

    /// <summary>
    /// This method is used to add java scripts to button attribute.
    /// </summary>
    private void InitializePage()
    {
        valSumError.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        btnSave.Attributes.Add("onclick", "if(!(EnterNoOfSubjects())){return false};");
        imgbtnCancel.Attributes["onclick"] = "javascript:DisableButtons()";
        ApplyMouseHoverEffect(new List<Button> { btnSave, imgbtnCancel });
    }

    private string GenrateXML(Hashtable oHChgStds)
    {
        const string S_ELEMENT = "element";
        string sAttribute;
        XmlDocument oDoc = new XmlDocument();
        XmlElement root = oDoc.CreateElement("StdId");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "StdId", "");

        foreach (DictionaryEntry oDE in oHChgStds)
        {
            XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "StdId", "");
            sAttribute = "StdId";
            XmlAttribute attr = oDoc.CreateAttribute(sAttribute);
            attr.Value = Convert.ToString(oDE.Key);
            oXmlNode.Attributes.Append(attr);

            oXmlRootNode.AppendChild(oXmlNode);
        }
        root.AppendChild(oXmlRootNode);
        return root.InnerXml;
    }

    private void DesignSettingAccordingLanguage()
    {
        hidStandard.Value = Resources.LocalizedResources.Standard;
        hidPleaseFixFollowingErrors.Value = Resources.LocalizedResources.PleaseFixFollowingError;
        hidNumberOfSubjectsShouldBeLessThanTotalNumberOfSubjects.Value = Resources.LocalizedResources.NumberOfSubjectsShouldBeLessThanTotalNumberOfSubjects;
        hidGradeNameShouldBeSelectedForFollowingStandard.Value = Resources.LocalizedResources.GradeNameShouldBeSelectedForFollowingStandard;
        hidNumberOfSubjectsShouldNotBeBlankForFollowingStandards.Value= Resources.LocalizedResources.NumberOfSubjectsShouldNotBeBlankForFollowingStandards;
    }

    #endregion "Private Methods"
}

