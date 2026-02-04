/* File Name :- SchoolwiseAcademicYearPopup.aspx.cs
 * Modified By :- Sachin
 * Modified  Date :- 25-Sept-2009
 * Purpose :- Code Review.
 * Class Description :- This class is used to edit academic year.
*/

using System;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Xml.Serialization;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Xml;
using BusinessLogic;
using Utility;
using CrystalDecisions.Shared;
using StandardwiseAcademicYear;
using BusinessLogic.Exceptions;
using System.Reflection;
using System.Globalization;

public partial class SchoolwiseAcademicYearPopup :SchoolBase
{
    #region Constants

    const string S_WARNIG_MESSAGE = "You are viewing closed academic year.";
    const string S_NO_RECORD_MESSAGE = "No student out of academic year.";
    const string S_DEFAULT_DATETIME = "1/1/1900 12:00:00 AM";

    #endregion

    #region Data Member

    SchoolWiseAcademicYearMasterBL moSchoolWiseAcademicYearMasterBL;

    #endregion

    #region Events

    /// <summary>
    /// This event is used to descrypt querystring and set valuesto  controls.
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
                //Open in edit mode to update schoolwise academic year.
                if (Request.QueryString.Count != Constants.I_ZERO)
                {
                    SetValuesToControls();
                }
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                RefreshValue();
                SetDefaultValues();
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValue();
            }

            lblNorecord.Text = string.Empty;            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to add or update schoolwise academic year.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        CheckIsSelectedYearClosed();
        moSchoolWiseAcademicYearMasterBL = new SchoolWiseAcademicYearMasterBL();
        try
        {
            string Message = "";
            moSchoolWiseAcademicYearMasterBL = new SchoolWiseAcademicYearMasterBL();
            string sStandardwiseAcademicYearXML = GenerateXML();

            moSchoolWiseAcademicYearMasterBL.CheckOverlappingofStandardwiseAcademicYear(sStandardwiseAcademicYearXML, out Message);
            if (!string.IsNullOrEmpty(Message))
                lblErrorMsg.Text = Resources.LocalizedResources.ValDateOverlapping + Message.Substring(1, Message.Length - 1);
            else
            {
                moSchoolWiseAcademicYearMasterBL.SaveStandardwiseAcademicYear(sStandardwiseAcademicYearXML);

                int iAcademicYearId = Convert.ToInt32(hidAcademicYearId.Value);
                moSchoolWiseAcademicYearMasterBL = new SchoolWiseAcademicYearMasterBL(miSchoolId, iAcademicYearId);
                List<StandardwiseAcademicYearEntity> olstStandardwiseAcademicYear = SchoolWiseAcademicYearMasterBL.GetStandardwiseAcademicYear(miSchoolId, iAcademicYearId);

                DateTime a = olstStandardwiseAcademicYear.Max(o => o.EndDate);
                DateTime b = olstStandardwiseAcademicYear.Min(o => o.StartDate);

                moSchoolWiseAcademicYearMasterBL = SetAcademicYearMasterBL(b, a);
                if (hidActionFlag.Value.Equals(Constants.ViewMode.Edit.ToString()))
                {
                    if (chkIsCurrentYear.Checked == true)
                    {
                        moSchoolWiseAcademicYearMasterBL.Is_NewlyCreated = Constants.C_NO.ToString();
                        moSchoolWiseAcademicYearMasterBL.UpdateSchoolWiseAcademicYearMaster();
                        Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE] = b;
                        Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE] = a;
                    }
                    else if (chkIsClosedYear.Checked == true)
                        moSchoolWiseAcademicYearMasterBL.UpdateSchoolWiseAcademicYearMaster();
                    else
                        moSchoolWiseAcademicYearMasterBL.UpdateSchoolWiseAcademicYearMaster();

                    UpdateDefaultNoticeDate(b, a);
                }
                else
                    moSchoolWiseAcademicYearMasterBL.InsertSchoolWiseAcademicYearMaster();
                Response.Write("<Script language='Javascript'>window.opener.location.reload(true); window.close();window.opener.focus(); </Script>");
            }
        }
        catch (BusinessLogic.Exceptions.NonWorkingDay ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
        }
        catch (BusinessLogic.HolidaysMasterBL.PerdefinedStartAndEndDate ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to get all the student list those are out of academic year date.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPreviewStudentList_Click(object sender, EventArgs e)
    {
        try
        {   
            moSchoolWiseAcademicYearMasterBL = new SchoolWiseAcademicYearMasterBL();
            string sStandardwiseAcademicYearXML = CommonUtility.GetXMLForList<StandardwiseAcademicYearEntity>(PopulateAcademicYearInfo());

            if (!SchoolWiseAcademicYearMasterBL.IsReportEmpty(sStandardwiseAcademicYearXML, miSchoolId, Convert.ToInt32(hidAcademicYearId.Value)))
            {
                lblNorecord.Visible = true;
                lblNorecord.Text = Resources.LocalizedResources.NoStudentOutOfAcademicYear;
            }
            else            
                DisplayReport();
        }
        catch (System.Threading.ThreadAbortException)
        { }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// This checks wherther selected year is closed or not.
    /// </summary>
    private void CheckIsSelectedYearClosed()
    {
        int iChangedAcademicYear = Convert.ToInt32(hidAcademicYearId.Value);
        int iStartYear = ((DateTime)Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE]).Year;
        int iEndYear = ((DateTime)Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE]).Year;

        string sYearValue = Convert.ToString(iStartYear) + '-' + Convert.ToString(iEndYear);
        if (chkIsClosedYear.Checked == true && (miAcademicYearId == iChangedAcademicYear))
            Session[Constants.S_SESSION_ACADEMIC_YEAR_STATUS] = Resources.LocalizedResources.YouAreViewingClosedAcademicYear + "(" + sYearValue + ")";
        else
            Session[Constants.S_SESSION_ACADEMIC_YEAR_STATUS] = string.Empty;
    }

    /// <summary>
    /// This Method is used to update default notice board date on change of academic year date.
    /// </summary>
    private void UpdateDefaultNoticeDate(DateTime StartDate, DateTime EndDate)
    {
        NoticeBoardBL oNoticeBoardBL = new NoticeBoardBL();
        oNoticeBoardBL.SchoolId = miSchoolId;
        oNoticeBoardBL.AcademicYearId = miAcademicYearId;
        oNoticeBoardBL.UpdatedById = miUserId;
        oNoticeBoardBL.UpdatedDate = Convert.ToDateTime(System.DateTime.Today);
        oNoticeBoardBL.StartDate = StartDate;
        oNoticeBoardBL.EndDate = EndDate;
        oNoticeBoardBL.UpdateDefaultNoticeDates();
    }

    /// <summary>
    /// This method is used to insert schoolwise academic year data.
    /// </summary>
    private SchoolWiseAcademicYearMasterBL SetAcademicYearMasterBL(DateTime StartDate, DateTime EndDate)
    {
      
        moSchoolWiseAcademicYearMasterBL = new SchoolWiseAcademicYearMasterBL();

        moSchoolWiseAcademicYearMasterBL.StartDate = StartDate;
        moSchoolWiseAcademicYearMasterBL.EndDate = EndDate;

        moSchoolWiseAcademicYearMasterBL.SchoolId = miSchoolId;
        moSchoolWiseAcademicYearMasterBL.InsertedByid = miUserId;
        moSchoolWiseAcademicYearMasterBL.UpdatedById = miUserId;
        if (chkIsClosedYear.Checked == true)
            moSchoolWiseAcademicYearMasterBL.IsCloseYear = Constants.C_YES.ToString();
        else
            moSchoolWiseAcademicYearMasterBL.IsCloseYear = Constants.C_NO.ToString();

        if (hidAcademicYearId.Value != string.Empty)
            moSchoolWiseAcademicYearMasterBL.SchoolWiseAcademicYearId = Convert.ToInt32(hidAcademicYearId.Value);
        if (chkIsCurrentYear.Checked == true)
        {
            moSchoolWiseAcademicYearMasterBL.UpdateIsCurrentFlag(miSchoolId);
            moSchoolWiseAcademicYearMasterBL.IsCurrentYear = Constants.C_YES.ToString();
        }
        else
            moSchoolWiseAcademicYearMasterBL.IsCurrentYear = Constants.C_NO.ToString();

        moSchoolWiseAcademicYearMasterBL.Is_NewlyCreated = moSchoolWiseAcademicYearMasterBL.IsNewlyCreated(miSchoolId, hidAcademicYearId.Value.ToInt());
        return moSchoolWiseAcademicYearMasterBL;
    }

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        lblErrorMsg.Text = string.Empty;
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        chkIsClosedYear.Attributes.Add("Onclick", "DisableCurrentYearIfCloseYearChecked()");
        chkIsCurrentYear.Attributes.Add("Onclick", "DisableCloseYearIfCurrentYearChecked();");
        btnSave.Attributes["onclick"] = "if(!(ConfirmAction())){return false;}";
        btnBack.Attributes.Add("onclick", "if(!(closewindow())){return false;}");
        btnPreviewStudentList.Attributes["onclick"] = "ClearMessage()";
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnBack, btnPreviewStudentList });
    }

    /// <summary>
    /// This method is used to set values to respective control.
    /// </summary>
    private void SetValuesToControls()
    {
        if (QueryString.Count > 0)
        {
            if (QueryString["AcademicYearId"] != null)
            {
                hidAcademicYearId.Value = QueryString["AcademicYearId"];
                hidActionFlag.Value = Constants.ViewMode.Edit.ToString();
                lblAddAcademicYear.Text = Resources.LocalizedResources.EditAcademicYear;
            }
        }
        int iAcademicYearId = Convert.ToInt32(hidAcademicYearId.Value);
        moSchoolWiseAcademicYearMasterBL = new SchoolWiseAcademicYearMasterBL(miSchoolId, iAcademicYearId);
        List<StandardwiseAcademicYearEntity> olstStandardwiseAcademicYear = SchoolWiseAcademicYearMasterBL.GetStandardwiseAcademicYear(miSchoolId, iAcademicYearId);
        grdvwStandard.DataSource = olstStandardwiseAcademicYear;
        grdvwStandard.DataBind();


        DateTime a = olstStandardwiseAcademicYear.Max(o => o.EndDate);
        DateTime b = olstStandardwiseAcademicYear.Min(o => o.StartDate);

        Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE] = b;
        Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE] = a;

        chkIsClosedYear.Enabled = true;

        if (moSchoolWiseAcademicYearMasterBL.IsCurrentYear.Equals(Constants.C_YES.ToString()))
        {
            chkIsCurrentYear.Checked = true;
            chkIsCurrentYear.Enabled = false;
            chkIsClosedYear.Enabled = false;
        }
        else if (moSchoolWiseAcademicYearMasterBL.IsCloseYear.Equals(Constants.C_YES.ToString()))
            chkIsClosedYear.Checked = true;
        else if (moSchoolWiseAcademicYearMasterBL.Is_NewlyCreated.Equals(Constants.C_YES.ToString()) &&
            moSchoolWiseAcademicYearMasterBL.Is_FinalYear_Generated.Equals(Constants.C_NO.ToString()))
        {
            chkIsNewYear.Checked = true;
            chkIsNewYear.Enabled = false;
            chkIsCurrentYear.Enabled = false;
            chkIsClosedYear.Enabled = false;
        }
        else if (moSchoolWiseAcademicYearMasterBL.Is_NewlyCreated.Equals(Constants.C_YES.ToString()))
        {
            chkIsNewYear.Checked = true;
            chkIsCurrentYear.Enabled = true;
            chkIsClosedYear.Enabled = false;
        }

        if (moSchoolWiseAcademicYearMasterBL.IsCloseYear.Equals(Constants.C_NO.ToString()))
            chkIsClosedYear.Checked = false;
        else
            chkIsClosedYear.Checked = true;        
    }

    /// <summary>
    /// This method is used to fill list of sstandardwise acadamic year details.
    /// </summary>
    /// <returns></returns>
    private List<StandardwiseAcademicYearEntity> PopulateAcademicYearInfo()
    {
        StandardwiseAcademicYearEntity oStandardwiseAcademicYearEntity = null;
        List<StandardwiseAcademicYearEntity> lstAcademicYearInfo = new List<StandardwiseAcademicYearEntity>();

        for (int iRowNo = 0; iRowNo < grdvwStandard.Rows.Count; iRowNo++)
        {
            GridViewRow oCurrentItem = (GridViewRow)grdvwStandard.Rows[iRowNo];
            int iRowId = Convert.ToInt32(oCurrentItem.DataItemIndex);

            TextBox txtStartDt = oCurrentItem.FindControl("txtStartDate") as TextBox;
            TextBox txtEndDt = oCurrentItem.FindControl("txtEndDate") as TextBox;
            int iStandardId = Convert.ToInt32(grdvwStandard.DataKeys[iRowId]["StandardId"]);

            if (!string.IsNullOrEmpty(txtStartDt.Text) || !string.IsNullOrEmpty(txtEndDt.Text))
            {
                oStandardwiseAcademicYearEntity = new StandardwiseAcademicYearEntity();
                oStandardwiseAcademicYearEntity.StandardId = iStandardId;

                oStandardwiseAcademicYearEntity.StartDate = !string.IsNullOrEmpty(txtStartDt.Text) ? Convert.ToDateTime(txtStartDt.Text) : oStandardwiseAcademicYearEntity.StartDate = Convert.ToDateTime(S_DEFAULT_DATETIME);
                oStandardwiseAcademicYearEntity.EndDate = !string.IsNullOrEmpty(txtEndDt.Text) ? Convert.ToDateTime(txtEndDt.Text) : oStandardwiseAcademicYearEntity.EndDate = Convert.ToDateTime(S_DEFAULT_DATETIME);

                lstAcademicYearInfo.Add(oStandardwiseAcademicYearEntity);
            }
        }
        return lstAcademicYearInfo;
    }

    /// <summary>
    /// This method is used to display report.
    /// </summary>
    private void DisplayReport()
    {
        ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.OutofAcademicYearStudentList, GetFilterString(), ExportFormatType.PortableDocFormat);
        oReportDisplay.DisplayReport();
    }

    /// <summary>
    /// This method is used to get the filter string.
    /// </summary>
    /// <returns></returns>
    private string GetFilterString()
    {
        string sStandardwiseAcademicYearXML = CommonUtility.GetXMLForList<StandardwiseAcademicYearEntity>(PopulateAcademicYearInfo());
        return "(usp_GetOutofAcademicYearStudentList.SchoolId}=" + miSchoolId + " AND  usp_GetOutofAcademicYearStudentList.AcademicYearId} =" + Convert.ToInt32(hidAcademicYearId.Value) + " AND  usp_GetOutofAcademicYearStudentList.AcademicYearXML} ='" + sStandardwiseAcademicYearXML + "') @";       
    }

    /// <summary>
    /// This method is used to generate XML.
    /// </summary>
    /// <returns></returns>
    private string GenerateXML()
    {
        const int I_COLUMN_INDEX_START_DATETIME = 1;
        const int I_COLUMN_INDEX_END_DATETIME = 2;
        const int I_COLUMN_INDEX_REOPNING_DATETIME = 3;
        const string S_ELEMENT = "element";

        string sAttribute;
        XmlDocument oDoc = new XmlDocument();
        XmlElement oElement = oDoc.CreateElement("StandardwiseAcademicYear");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "StandardwiseAcademicYear", "");
        for (int i = 0; i < grdvwStandard.Rows.Count; i++)
        {
            TextBox otxtStartDate = (TextBox)grdvwStandard.Rows[i].Cells[I_COLUMN_INDEX_START_DATETIME].FindControl("txtStartDate");
            TextBox otxtEndDate = (TextBox)grdvwStandard.Rows[i].Cells[I_COLUMN_INDEX_END_DATETIME].FindControl("txtEndDate");
            TextBox otxtReopningDate = (TextBox)grdvwStandard.Rows[i].Cells[I_COLUMN_INDEX_REOPNING_DATETIME].FindControl("txtReopeningDate");

            XmlNode oXMLNode = oDoc.CreateNode(S_ELEMENT, "StandardwiseAcademicYear", "");

            sAttribute = "StandardwiseAcademicYearId";
            XmlAttribute oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = grdvwStandard.DataKeys[i]["StandardwiseAcademicYearId"].ToString();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "StandardId";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = grdvwStandard.DataKeys[i]["StandardId"].ToString();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "StartDate";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = otxtStartDate.Text.Trim();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "EndDate";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = otxtEndDate.Text.Trim();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "ReopningDate";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = otxtReopningDate.Text.Trim();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "SchoolId";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = Convert.ToString(miSchoolId); ;
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "AcademicYearId";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = hidAcademicYearId.Value.ToString();
            oXMLNode.Attributes.Append(oAttr);

            sAttribute = "InsertedById";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = miUserId.ToString();
            oXMLNode.Attributes.Append(oAttr);

            oXmlRootNode.AppendChild(oXMLNode);
        }
        oElement.AppendChild(oXmlRootNode);
        return oElement.InnerXml;
    }
    /// <summary>
    /// This is used to refresh the values.
    /// </summary>
    private void RefreshValue()
    {
        hidValAttendenceDeleted.Value = Resources.LocalizedResources.ValAttendenceDeleted;
        hidStartDateShouldNotBeBlankForRowNumber.Value = Resources.LocalizedResources.StartDateShouldNotBeBlankForRowNumber;
        hidStartDateShouldBeLessThanEndDateForRowNumber.Value = Resources.LocalizedResources.StartDateShouldBeLessThanEndDateForRowNumber;
        hidSchoolReopeningDateShouldNotBeBlankForRowNumber.Value = Resources.LocalizedResources.SchoolReopeningDateShouldNotBeBlankForRowNumber;
        hidEndDateShouldNotBeBlankForRowNumber.Value = Resources.LocalizedResources.EndDateShouldNotBeBlankForRowNumber;
    }
    #endregion

}
