/* File Name = TeacherExternalLectureAssignmentUI.aspx.cs
 * Created Date - 
 * Modified Date  - 23 June 2011
 * Created by - Vipul
 * Class Description - This class is defined to manage external lecture details.*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml.Serialization;
using System.Reflection;
using BusinessLogic.Exceptions;
using Utility;
using BusinessLogic;
using ExternalLectures;
using WeekDayNameDetails;

public partial class TeacherExternalLectureAssignmentUI : SchoolBase
{
    #region "Data Members"

    List<WeekDays> mlstWeekDays;
    List<StandardDivisions> mlstStandardDivisions;
    List<StayBackLectureDetails> mlstStayBackLectureDetails;

    #endregion "Data Members"

    #region "Constant"

    private const string S_STAYBACK ="Stayback";
    private const string S_ASSEMBLY = "Assembly";
    private const string S_MPT = "MPT";
    private const string S_WEEKLYTEST = "WeeklyTest";
   
    #endregion "Contant"

    #region "Event"

    /// <summary>
    /// This event is used to fill standard divisionwise teacher's external details and stay back lectures .
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            InitializeMemberVariables();
            if (!IsPostBack)
            {
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                SetDefaultFields();
                SetExternalLecturesHiddenFields();
                FillTeacherDetails();
                SetJavaScriptAttributres();
                FillStayBackDetailsListView();
                FillAssemblyDetailsListView();
                FillMPTDetailsListView();
                FillWeeklyTestListView();
                
                RefreshValues();
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValues();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save external lecture details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        ExternalLecturesBL oExternalLecturesBL = new ExternalLecturesBL();
        oExternalLecturesBL.SaveTeacherExternalLectureDetails(GenerateXml(GetExternalSubjectDetails()));
        DeleteAdditionalLecture();
        lblUpdateSucess.Text = Resources.LocalizedResources.MsgExternalLectureSuccess;
        if (!IsConfigured())
            SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.ExternalLectureConfiguration));
    }

    /// <summary>
    /// This event is used to search teacher.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        lstvwExtenalLectureDetails.DataSourceID = ObjDSTeacherDetails.ID;
        lstvwExtenalLectureDetails.DataBind();
    }

    /// <summary>
    /// This event is used to view page wise external lecture details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNoAndCulture(lstvwExtenalLectureDetails, Resources.LocalizedResources.PageNo, Resources.LocalizedResources.Of, Resources.LocalizedResources.OutOflst);
            DataPager oDataPager = lstvwExtenalLectureDetails.FindControl("DtPgDropDown") as DataPager;
            DropDownList ddlCnt = (oDataPager.Controls[0].FindControl("ddlCnt")) as DropDownList;
            hidPageNo.Value = (ddlCnt.SelectedIndex + 1).ToString();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set external lecture check boxes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwExtenalLectureDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                HtmlTableCell oCellIsAssemblyApplicable = oCurrentItem.FindControl("tdAssemblyApplicable") as HtmlTableCell;
                oCellIsAssemblyApplicable.Visible = (hidAssemblyApplicable.Value == Constants.C_YES.ToString()) ? true : false;

                HtmlTableCell oCellMPTApplicable = oCurrentItem.FindControl("tdMPTApplicable") as HtmlTableCell;
                oCellMPTApplicable.Visible = (hidMPTApplicable.Value == Constants.C_YES.ToString()) ? true : false;

                HtmlTableCell oCellIsStayBackApplicable = oCurrentItem.FindControl("tdStayBackApplicable") as HtmlTableCell;
                oCellIsStayBackApplicable.Visible = (hidStayBackApplicable.Value == Constants.C_YES.ToString()) ? true : false;

                ((CheckBox)oCurrentItem.FindControl("chkAssembly")).Checked = Convert.ToBoolean(lstvwExtenalLectureDetails.DataKeys[oCurrentItem.DisplayIndex]["IsAssembly"]);
                ((CheckBox)oCurrentItem.FindControl("chkMPT")).Checked = Convert.ToBoolean(lstvwExtenalLectureDetails.DataKeys[oCurrentItem.DisplayIndex]["IsMPT"]);
                ((CheckBox)oCurrentItem.FindControl("chkStayback")).Checked = Convert.ToBoolean(lstvwExtenalLectureDetails.DataKeys[oCurrentItem.DisplayIndex]["IsStayBack"]);
                ((CheckBox)oCurrentItem.FindControl("chkWeeklyTest")).Checked = Convert.ToBoolean(lstvwExtenalLectureDetails.DataKeys[oCurrentItem.DisplayIndex]["WeeklyTestApplicable"]);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill pager footer details and set list view's header properties.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwExtenalLectureDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwExtenalLectureDetails.Items.Count > 0)
            {
                tblSearch.Visible = true;
                ControlUtility.FillListViewPagerFooterWithCulture(lstvwExtenalLectureDetails, DtPgCount, Resources.LocalizedResources.PageNo, Resources.LocalizedResources.Of, Resources.LocalizedResources.OutOflst);
                HtmlTableRow otrDataPager = lstvwExtenalLectureDetails.FindControl("trDataPager") as HtmlTableRow;
                trPagerTeacherDetails.Visible = (otrDataPager.Visible == true) ? true : false;
                SetConfirmationMessage();
                HtmlTableCell oCellIsAssemblyApplicable = lstvwExtenalLectureDetails.FindControl("thAssemblyApplicable") as HtmlTableCell;
                oCellIsAssemblyApplicable.Visible = (hidAssemblyApplicable.Value == Constants.C_YES.ToString()) ? true : false;

                HtmlTableCell oCellMPTApplicable = lstvwExtenalLectureDetails.FindControl("thMPTApplicable") as HtmlTableCell;
                oCellMPTApplicable.Visible = (hidMPTApplicable.Value == Constants.C_YES.ToString()) ? true : false;

                HtmlTableCell oCellIsStayBackApplicable = lstvwExtenalLectureDetails.FindControl("thStayBackApplicable") as HtmlTableCell;
                oCellIsStayBackApplicable.Visible = (hidStayBackApplicable.Value == Constants.C_YES.ToString()) ? true : false;
                btnSave.Visible = true;
            }
            else
            {
                trPagerTeacherDetails.Visible = false;
                btnSave.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill standard divisionwise week days cells for MPT.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwMPTLecture_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                AddLinkToAssignMPTLectures(oCurrentItem);
                HideVisibleListViewCellsForMPT(oCurrentItem);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    ///This event is used to fill standard divisionwise week days cells for assembly lecturs.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwAssemblyLectures_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                AddLinkToAssignAssemblyLectures(oCurrentItem);
                HideVisibleListViewCellsForAssembly(oCurrentItem);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill standard divisionwise week days cells for stayback lecturs.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStaybackLectures_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                AddLinkToAssignStayBackLectures(oCurrentItem);
                HideVisibleListViewCellsForStayback(oCurrentItem);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display week day name on Assembly listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwAssemblyLectures_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwAssemblyLectures.Items.Count > 0)
            {
                ExternalLecturesBL oExternalLecturesBL = new ExternalLecturesBL();
                List<WeekdaysName> lstWeekdaysName = oExternalLecturesBL.GetWeedDaysName(miSchoolId, miAcademicYearId);
                HtmlTableRow oHtmlTableHeaderRow = lstvwAssemblyLectures.FindControl("trHeader") as HtmlTableRow;
                Label[] oLabel = new Label[10];
                oLabel[1] = oHtmlTableHeaderRow.FindControl("lblMon") as Label;
                oLabel[2] = oHtmlTableHeaderRow.FindControl("lblTue") as Label;
                oLabel[3] = oHtmlTableHeaderRow.FindControl("lblWed") as Label;
                oLabel[4] = oHtmlTableHeaderRow.FindControl("lblThu") as Label;
                oLabel[5] = oHtmlTableHeaderRow.FindControl("lblFri") as Label;
                oLabel[6] = oHtmlTableHeaderRow.FindControl("lblSat") as Label;
                oLabel[7] = oHtmlTableHeaderRow.FindControl("lblSun") as Label;
                for (int iIndex = 0; iIndex < lstWeekdaysName.Count; iIndex++)
                {
                    oLabel[lstWeekdaysName[iIndex].Id].Text = lstWeekdaysName[iIndex].WeekDayName;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display week day name on MPT listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwMPTLecture_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwMPTLecture.Items.Count > 0)
            {
                ExternalLecturesBL oExternalLecturesBL = new ExternalLecturesBL();
                List<WeekdaysName> lstWeekdaysName = oExternalLecturesBL.GetWeedDaysName(miSchoolId, miAcademicYearId);
                HtmlTableRow oHtmlTableHeaderRow = lstvwMPTLecture.FindControl("trHeader") as HtmlTableRow;
                Label[] oLabel = new Label[10];
                oLabel[1] = oHtmlTableHeaderRow.FindControl("lblMon") as Label;
                oLabel[2] = oHtmlTableHeaderRow.FindControl("lblTue") as Label;
                oLabel[3] = oHtmlTableHeaderRow.FindControl("lblWed") as Label;
                oLabel[4] = oHtmlTableHeaderRow.FindControl("lblThu") as Label;
                oLabel[5] = oHtmlTableHeaderRow.FindControl("lblFri") as Label;
                oLabel[6] = oHtmlTableHeaderRow.FindControl("lblSat") as Label;
                oLabel[7] = oHtmlTableHeaderRow.FindControl("lblSun") as Label;
                for (int iIndex = 0; iIndex < lstWeekdaysName.Count; iIndex++)
                {
                    oLabel[lstWeekdaysName[iIndex].Id].Text = lstWeekdaysName[iIndex].WeekDayName;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display week day name on Stayback listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStaybackLectures_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwStaybackLectures.Items.Count > 0)
            {
                ExternalLecturesBL oExternalLecturesBL = new ExternalLecturesBL();
                List<WeekdaysName> lstWeekdaysName = oExternalLecturesBL.GetWeedDaysName(miSchoolId, miAcademicYearId);
                HtmlTableRow oHtmlTableHeaderRow = lstvwStaybackLectures.FindControl("trHeader") as HtmlTableRow;
                Label[] oLabel = new Label[10];
                oLabel[1] = oHtmlTableHeaderRow.FindControl("lblMon") as Label;
                oLabel[2] = oHtmlTableHeaderRow.FindControl("lblTue") as Label;
                oLabel[3] = oHtmlTableHeaderRow.FindControl("lblWed") as Label;
                oLabel[4] = oHtmlTableHeaderRow.FindControl("lblThu") as Label;
                oLabel[5] = oHtmlTableHeaderRow.FindControl("lblFri") as Label;
                oLabel[6] = oHtmlTableHeaderRow.FindControl("lblSat") as Label;
                oLabel[7] = oHtmlTableHeaderRow.FindControl("lblSun") as Label;
                for (int iIndex = 0; iIndex < lstWeekdaysName.Count; iIndex++)
                {
                    oLabel[lstWeekdaysName[iIndex].Id].Text = lstWeekdaysName[iIndex].WeekDayName;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwWeeklyTest_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                AddLinkToAssignWeeklyTestLectures(oCurrentItem);
                HideVisibleListViewCellsForMPT(oCurrentItem);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwWeeklyTest_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwStaybackLectures.Items.Count > 0)
            {
                ExternalLecturesBL oExternalLecturesBL = new ExternalLecturesBL();
                List<WeekdaysName> lstWeekdaysName = oExternalLecturesBL.GetWeedDaysName(miSchoolId, miAcademicYearId);
                HtmlTableRow oHtmlTableHeaderRow = lstvwWeeklyTest.FindControl("trHeader") as HtmlTableRow;
                Label[] oLabel = new Label[10];
                oLabel[1] = oHtmlTableHeaderRow.FindControl("lblMon") as Label;
                oLabel[2] = oHtmlTableHeaderRow.FindControl("lblTue") as Label;
                oLabel[3] = oHtmlTableHeaderRow.FindControl("lblWed") as Label;
                oLabel[4] = oHtmlTableHeaderRow.FindControl("lblThu") as Label;
                oLabel[5] = oHtmlTableHeaderRow.FindControl("lblFri") as Label;
                oLabel[6] = oHtmlTableHeaderRow.FindControl("lblSat") as Label;
                oLabel[7] = oHtmlTableHeaderRow.FindControl("lblSun") as Label;
                for (int iIndex = 0; iIndex < lstWeekdaysName.Count; iIndex++)
                {
                    oLabel[lstWeekdaysName[iIndex].Id].Text = lstWeekdaysName[iIndex].WeekDayName;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion "Event"

    #region " Private Methods "

    /// <summary>
    /// This method is used to set java script attributes and post back url.
    /// </summary>
    private void SetJavaScriptAttributres()
    {
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Timetable_Related));
        ApplyMouseHoverEffect(new List<Button> { btnSearch, btnSave, btnBack });
        SetDefaultButton(btnSearch);
    }

    /// <summary>
    /// This method is used to set default control properties.
    /// </summary>
    private void SetDefaultFields()
    {
        colpnlExternalLectures.Collapsed = false;
        colpnlStayBackLectures.Collapsed = false;
        hidIsConfigured.Value = IsConfigured() ? Constants.C_YES.ToString() : Constants.C_NO.ToString();
    }

    /// <summary>
    /// This method is used to fill teacher details.
    /// </summary>
    private void FillTeacherDetails()
    {
        if (hidAssemblyApplicable.Value == Constants.C_YES.ToString()
                || hidMPTApplicable.Value == Constants.C_YES.ToString()
                    || hidStayBackApplicable.Value == Constants.C_YES.ToString())
        {
            lstvwExtenalLectureDetails.DataSourceID = ObjDSTeacherDetails.ID;
            lstvwExtenalLectureDetails.DataBind();
        }
        else
        {
            btnSave.Visible = false;
            divExternalLectureAssignment.Visible = false;
            divStayBackLectures.Visible = false;
            trExternalLecturesNotAplicable.Visible = true;
        }
    }

    /// <summary>
    /// This method is used to set hidden filds for external lectures.
    /// </summary>
    private void SetExternalLecturesHiddenFields()
    {
        hidAssemblyApplicable.Value = Settings.IsAssemblyApplicable ? Constants.S_YES : Constants.S_NO;
        hidMPTApplicable.Value = Settings.IsMPTApplicable ? Constants.S_YES : Constants.S_NO;
        hidStayBackApplicable.Value = Settings.IsStaybackApplicable ? Constants.S_YES : Constants.S_NO;
    }

    /// <summary>
    /// This method is used to set confirmation messaege on change of page.
    /// </summary>
    private void SetConfirmationMessage()
    {
        DataPager oDataPager = lstvwExtenalLectureDetails.FindControl("DtPgDropDown") as DataPager;
        DropDownList ddlCnt = (oDataPager.Controls[0].FindControl("ddlCnt")) as DropDownList;
        ddlCnt.Attributes.Add("onchange", "if(!ConfirmMsg('" + ddlCnt.ClientID + "')){return false;}");
    }

    /// <summary>
    /// This method is used to get detailed lsit of external subject.
    /// </summary>
    /// <returns></returns>
    private List<TeacherExternalLecturesDetails> GetExternalSubjectDetails()
    {
        List<TeacherExternalLecturesDetails> lstTeacherExternalLecturesDetails = new List<TeacherExternalLecturesDetails>();
        foreach (ListViewDataItem oCurrentItem in lstvwExtenalLectureDetails.Items)
        {
            TeacherExternalLecturesDetails oTeacherDetails = new TeacherExternalLecturesDetails()
            {
                IsAssembly = ((CheckBox)oCurrentItem.FindControl("chkAssembly")).Checked,
                IsMPT = ((CheckBox)oCurrentItem.FindControl("chkMPT")).Checked,
                IsStayBack = ((CheckBox)oCurrentItem.FindControl("chkStayback")).Checked,
                WeeklyTestApplicable = ((CheckBox)oCurrentItem.FindControl("chkWeeklyTest")).Checked,
                TeacherId = Convert.ToInt32(lstvwExtenalLectureDetails.DataKeys[oCurrentItem.DisplayIndex]["TeacherId"]),
            };
            lstTeacherExternalLecturesDetails.Add(oTeacherDetails);
        }
        return lstTeacherExternalLecturesDetails;
    }

    /// <summary>
    /// This method is used to decrypt querystring.
    /// </summary>
    /// <returns></returns>
    private bool IsConfigured()
    {
        return QueryString[Constants.S_IS_CONFIGURED] != null && QueryString[Constants.S_IS_CONFIGURED] == Constants.S_YES;
    }

    /// <summary>
    /// This method is used to fill list view for stay back details.
    /// </summary>
    private void FillStayBackDetailsListView()
    {
        if (CheckPreCondition())
        {
            if (hidStayBackApplicable.Value == Constants.S_YES)
            {
                SetStayBackNotApplicableMsg(true);
                ExternalLecturesBL oExternalLecturesBL = new ExternalLecturesBL();
                oExternalLecturesBL.GetStayBackLectureDetails(miSchoolId, miAcademicYearId, S_STAYBACK);
                mlstWeekDays = oExternalLecturesBL.lstWeekDays;
                mlstStandardDivisions = oExternalLecturesBL.lstStandardDivisions;
                mlstStayBackLectureDetails = oExternalLecturesBL.lstStayBackLectureDetails;
                lstvwStaybackLectures.DataSource = mlstStandardDivisions;
                lstvwStaybackLectures.DataBind();
                ShowWeekDayHeadersForStayback();
            }
            else
                SetStayBackNotApplicableMsg(false);
        }
    }
    /// <summary>
    ///  This method is used to fill list view for assembly details.
    /// </summary>
    private void FillAssemblyDetailsListView()
    {
        if (CheckPreCondition())
        {
            if (hidAssemblyApplicable.Value == Constants.S_YES)
            {
                SetStayBackNotApplicableMsg(true);
                ExternalLecturesBL oExternalLecturesBL = new ExternalLecturesBL();
                oExternalLecturesBL.GetStayBackLectureDetails(miSchoolId, miAcademicYearId, S_ASSEMBLY);
                mlstWeekDays = oExternalLecturesBL.lstWeekDays;
                mlstStandardDivisions = oExternalLecturesBL.lstStandardDivisions;
                mlstStayBackLectureDetails = oExternalLecturesBL.lstStayBackLectureDetails;
                lstvwAssemblyLectures.DataSource = mlstStandardDivisions;
                lstvwAssemblyLectures.DataBind();
                ShowWeekDayHeadersForAssembly();
            }
            else
                SetStayBackNotApplicableMsg(false);
        }
    }
    /// <summary>
    ///  This method is used to fill list view for MPT details.
    /// </summary>
    private void FillMPTDetailsListView()
    {
        if (CheckPreCondition())
        {
            if (hidMPTApplicable.Value == Constants.S_YES)
            {
                SetStayBackNotApplicableMsg(true);
                ExternalLecturesBL oExternalLecturesBL = new ExternalLecturesBL();
                oExternalLecturesBL.GetStayBackLectureDetails(miSchoolId, miAcademicYearId, S_MPT);
                mlstWeekDays = oExternalLecturesBL.lstWeekDays;
                mlstStandardDivisions = oExternalLecturesBL.lstStandardDivisions;
                mlstStayBackLectureDetails = oExternalLecturesBL.lstStayBackLectureDetails;
                lstvwMPTLecture.DataSource = mlstStandardDivisions;
                lstvwMPTLecture.DataBind();
                ShowWeekDayHeadersForMPT();
            }
            else
                SetStayBackNotApplicableMsg(false);
        }
    }

    private void FillWeeklyTestListView()
    {
        if (Settings.IsWeeklyTestApplicable)
        {
            SetStayBackNotApplicableMsg(true);
            ExternalLecturesBL oExternalLecturesBL = new ExternalLecturesBL();
            oExternalLecturesBL.GetStayBackLectureDetails(miSchoolId, miAcademicYearId, S_WEEKLYTEST);
            mlstWeekDays = oExternalLecturesBL.lstWeekDays;
            mlstStandardDivisions = oExternalLecturesBL.lstStandardDivisions;
            mlstStayBackLectureDetails = oExternalLecturesBL.lstStayBackLectureDetails;
            lstvwWeeklyTest.DataSource = mlstStandardDivisions;
            lstvwWeeklyTest.DataBind();
        }
        else
            trWeeklyTestApplicable.Visible = false;
    }



    /// <summary>
    /// This method is used to set message that stay back not applicable.
    /// </summary>
    /// <param name="abFlag"></param>
    private void SetStayBackNotApplicableMsg(bool abFlag)
    {
        trLegends.Visible = abFlag;
        trlstvwStaybackLectures.Visible = abFlag;
        trStayBackNotAplicable.Visible = !abFlag;
    }

    /// <summary>
    /// This method is used to check dependencies.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.ExternalLectureConfiguration);

        if (sLinks.Equals(string.Empty))
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
    /// This method is used to set visible or hide properties of controls.
    /// </summary>
    private void VisibleOrHideControls()
    {
        trLegends.Visible = false;
        trStayBackNotAplicable.Visible = false;
        trlstvwStaybackLectures.Visible = false;
    }

    /// <summary>
    /// This method is used to show week day headers of stay back details list view.
    /// </summary>
    private void ShowWeekDayHeadersForStayback()
    {
        ListViewDataItem oCurrentItem = lstvwStaybackLectures.Items[0];
        HideVisibleListViewCellsForStayback(oCurrentItem);
    }

    private void ShowWeekDayHeadersForMPT()
    {
        ListViewDataItem oCurrentItem = lstvwMPTLecture.Items[0];
        HideVisibleListViewCellsForMPT(oCurrentItem);
    }

    private void ShowWeekDayHeadersForAssembly()
    {
        ListViewDataItem oCurrentItem = lstvwAssemblyLectures.Items[0];
        HideVisibleListViewCellsForAssembly(oCurrentItem);
    }

    /// <summary>
    /// This method is used to hide or visible list view cells for stayback.
    /// </summary>
    /// <param name="oCurrentItem"></param>
    /// <param name="sWeekDay"></param>
    private void HideVisibleListViewCellsForStayback(ListViewDataItem oCurrentItem)
    {
        (from WeekDay in mlstWeekDays
         select WeekDay.WeekDayId).Where(DayId => DayId == -999).ToList()
        .ForEach
        (DayId =>
        {
            (from WeekDay in mlstWeekDays
             where WeekDay.WeekDayId == DayId
             select WeekDay.WeekDay).ToList().ForEach
            (Day =>
            {
                HtmlTableCell oCell = ((oCurrentItem.DataItem != null) ? oCurrentItem.FindControl("td" + Day) : lstvwStaybackLectures.FindControl("th" + Day)) as HtmlTableCell;
                oCell.Visible = false;
            }
            );
        }
        );
    }

    /// <summary>
    /// This method is used to hide or visible list view cells for MPT.
    /// </summary>
    /// <param name="oCurrentItem"></param>
    /// <param name="sWeekDay"></param>
    private void HideVisibleListViewCellsForMPT(ListViewDataItem oCurrentItem)
    {
        (from WeekDay in mlstWeekDays
         select WeekDay.WeekDayId).Where(DayId => DayId == -999).ToList()
        .ForEach
        (DayId =>
        {
            (from WeekDay in mlstWeekDays
             where WeekDay.WeekDayId == DayId
             select WeekDay.WeekDay).ToList().ForEach
            (Day =>
            {
                HtmlTableCell oCell = ((oCurrentItem.DataItem != null) ? oCurrentItem.FindControl("td" + Day) : lstvwMPTLecture.FindControl("th" + Day)) as HtmlTableCell;
                oCell.Visible = false;
            }
            );
        }
        );
    }
    /// <summary>
    /// This method is used to hide or visible list view cells for assembly.
    /// </summary>
    /// <param name="oCurrentItem"></param>
    /// <param name="sWeekDay"></param>
    private void HideVisibleListViewCellsForAssembly(ListViewDataItem oCurrentItem)
    {
        (from WeekDay in mlstWeekDays
         select WeekDay.WeekDayId).Where(DayId => DayId == -999).ToList()
        .ForEach
        (DayId =>
        {
            (from WeekDay in mlstWeekDays
             where WeekDay.WeekDayId == DayId
             select WeekDay.WeekDay).ToList().ForEach
            (Day =>
            {
                HtmlTableCell oCell = ((oCurrentItem.DataItem != null) ? oCurrentItem.FindControl("td" + Day) : lstvwAssemblyLectures.FindControl("th" + Day)) as HtmlTableCell;
                oCell.Visible = false;
            }
            );
        }
        );
    }

    /// <summary>
    /// This method is used to add link to assign stay back lectures.
    /// </summary>
    /// <param name="oCurrentItem"></param>
    private void AddLinkToAssignStayBackLectures(ListViewDataItem oCurrentItem)
    {
        (from WeekDay in mlstWeekDays
         select WeekDay.WeekDay).ToList()
        .ForEach
        (WeekDay =>
        {
            Label olblAssignStayBackLectures = new Label();
            StandardDivisions oclsEmployeeSpecialCodes = oCurrentItem.DataItem as StandardDivisions;
            olblAssignStayBackLectures.Text = GetAssingedLectures(oclsEmployeeSpecialCodes.StandardwiseDivisionId, WeekDay);
            olblAssignStayBackLectures.ForeColor = (olblAssignStayBackLectures.Text.Contains("Assign") || olblAssignStayBackLectures.Text.Contains("नियुक्त करा")) ? System.Drawing.Color.White : System.Drawing.Color.Black;
            olblAssignStayBackLectures.Font.Bold = false;
            olblAssignStayBackLectures.Font.Size = 9;
            olblAssignStayBackLectures.Font.Name = "Arial";
            olblAssignStayBackLectures.ToolTip = Resources.LocalizedResources.Class + " " + oclsEmployeeSpecialCodes.StandardDivision + " [" + WeekDay + "]";
            olblAssignStayBackLectures.Style.Add(HtmlTextWriterStyle.TextDecoration, "Underline");
            olblAssignStayBackLectures.Style.Add(HtmlTextWriterStyle.Cursor, "Hand");
            olblAssignStayBackLectures.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");
            olblAssignStayBackLectures.Attributes.Add("onclick", "window.open('StayBackLectureAssignmentPopUpUI.aspx?" + CommonUtility.EncryptQuerystring(GetQueryString(oCurrentItem, WeekDay)) +
                                         "' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=700,height=450'); return false;");
            HtmlTableCell oCell = oCurrentItem.FindControl("td" + WeekDay) as HtmlTableCell;
            oCell.Controls.Add(olblAssignStayBackLectures);
            oCell.BgColor = (olblAssignStayBackLectures.Text.Contains("Assign") || olblAssignStayBackLectures.Text.Contains("नियुक्त करा")) ? "#5dad8e" : "#eaeaea";
        }
        );
    }
    /// <summary>
    ///  This method is used to add link to assign assembly lectures.
    /// </summary>
    /// <param name="oCurrentItem"></param>
    private void AddLinkToAssignAssemblyLectures(ListViewDataItem oCurrentItem)
    {
        (from WeekDay in mlstWeekDays
         select WeekDay.WeekDay).ToList()
        .ForEach
        (WeekDay =>
        {
            Label olblAssignStayBackLectures = new Label();
            StandardDivisions oclsEmployeeSpecialCodes = oCurrentItem.DataItem as StandardDivisions;
            olblAssignStayBackLectures.Text = GetAssingedLecturesForAssembly(oclsEmployeeSpecialCodes.StandardwiseDivisionId, WeekDay);
            olblAssignStayBackLectures.ForeColor = (olblAssignStayBackLectures.Text.Contains("Assign")) ? System.Drawing.Color.White : System.Drawing.Color.Black;
            olblAssignStayBackLectures.Font.Bold = false;
            olblAssignStayBackLectures.Font.Size = 9;
            olblAssignStayBackLectures.Font.Name = "Arial";
            olblAssignStayBackLectures.ToolTip = "Class " + oclsEmployeeSpecialCodes.StandardDivision + " [" + WeekDay + "]";
            olblAssignStayBackLectures.Style.Add(HtmlTextWriterStyle.TextDecoration, "Underline");
            olblAssignStayBackLectures.Style.Add(HtmlTextWriterStyle.Cursor, "Hand");
            olblAssignStayBackLectures.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");
            olblAssignStayBackLectures.Attributes.Add("onclick", "window.open('StayBackLectureAssignmentPopUpUI.aspx?" + CommonUtility.EncryptQuerystring(GetQueryStringForAssembly(oCurrentItem, WeekDay)) +
                                         "' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=700,height=450'); return false;");
            HtmlTableCell oCell = oCurrentItem.FindControl("td" + WeekDay) as HtmlTableCell;
            oCell.Controls.Add(olblAssignStayBackLectures);
            oCell.BgColor = (olblAssignStayBackLectures.Text.Contains("Assign")) ? "#5dad8e" : "#eaeaea";
        }
        );
    }
    /// <summary>
    ///  This method is used to add link to assign MPT lectures.
    /// </summary>
    /// <param name="oCurrentItem"></param>
    private void AddLinkToAssignMPTLectures(ListViewDataItem oCurrentItem)
    {
        (from WeekDay in mlstWeekDays
         select WeekDay.WeekDay).ToList()
        .ForEach
        (WeekDay =>
        {
            Label olblAssignStayBackLectures = new Label();
            StandardDivisions oclsEmployeeSpecialCodes = oCurrentItem.DataItem as StandardDivisions;
            olblAssignStayBackLectures.Text = GetAssingedLecturesForMPT(oclsEmployeeSpecialCodes.StandardwiseDivisionId, WeekDay);
            olblAssignStayBackLectures.ForeColor = (olblAssignStayBackLectures.Text.Contains("Assign")) ? System.Drawing.Color.White : System.Drawing.Color.Black;
            olblAssignStayBackLectures.Font.Bold = false;
            olblAssignStayBackLectures.Font.Size = 9;
            olblAssignStayBackLectures.Font.Name = "Arial";
            olblAssignStayBackLectures.ToolTip = "Class " + oclsEmployeeSpecialCodes.StandardDivision + " [" + WeekDay + "]";
            olblAssignStayBackLectures.Style.Add(HtmlTextWriterStyle.TextDecoration, "Underline");
            olblAssignStayBackLectures.Style.Add(HtmlTextWriterStyle.Cursor, "Hand");
            olblAssignStayBackLectures.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");
            olblAssignStayBackLectures.Attributes.Add("onclick", "window.open('StayBackLectureAssignmentPopUpUI.aspx?" + CommonUtility.EncryptQuerystring(GetQueryStringForMPT(oCurrentItem, WeekDay)) +
                                         "' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=700,height=450'); return false;");
            HtmlTableCell oCell = oCurrentItem.FindControl("td" + WeekDay) as HtmlTableCell;
            oCell.Controls.Add(olblAssignStayBackLectures);
            oCell.BgColor = (olblAssignStayBackLectures.Text.Contains("Assign")) ? "#5dad8e" : "#eaeaea";
        }
        );
    }

    /// <summary>
    ///  This method is used to add link to assign Weekly test.
    /// </summary>
    /// <param name="oCurrentItem"></param>
    private void AddLinkToAssignWeeklyTestLectures(ListViewDataItem oCurrentItem)
    {
        (from WeekDay in mlstWeekDays
         select WeekDay.WeekDay).ToList()
        .ForEach
        (WeekDay =>
        {
            Label olblAssignStayBackLectures = new Label();
            StandardDivisions oclsEmployeeSpecialCodes = oCurrentItem.DataItem as StandardDivisions;
            olblAssignStayBackLectures.Text = GetAssingedLecturesForWeeklyTimeTable(oclsEmployeeSpecialCodes.StandardwiseDivisionId, WeekDay);
            olblAssignStayBackLectures.ForeColor = (olblAssignStayBackLectures.Text.Contains("Assign")) ? System.Drawing.Color.White : System.Drawing.Color.Black;
            olblAssignStayBackLectures.Font.Bold = false;
            olblAssignStayBackLectures.Font.Size = 9;
            olblAssignStayBackLectures.Font.Name = "Arial";
            olblAssignStayBackLectures.ToolTip = "Class " + oclsEmployeeSpecialCodes.StandardDivision + " [" + WeekDay + "]";
            olblAssignStayBackLectures.Style.Add(HtmlTextWriterStyle.TextDecoration, "Underline");
            olblAssignStayBackLectures.Style.Add(HtmlTextWriterStyle.Cursor, "Hand");
            olblAssignStayBackLectures.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");
            olblAssignStayBackLectures.Attributes.Add("onclick", "window.open('StayBackLectureAssignmentPopUpUI.aspx?" + CommonUtility.EncryptQuerystring(GetQueryStringForWeeklyTest(oCurrentItem, WeekDay)) +
                                         "' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=700,height=450'); return false;");
            HtmlTableCell oCell = oCurrentItem.FindControl("td" + WeekDay) as HtmlTableCell;
            oCell.Controls.Add(olblAssignStayBackLectures);
            oCell.BgColor = (olblAssignStayBackLectures.Text.Contains("Assign")) ? "#5dad8e" : "#eaeaea";
        }
        );
    }


    /// <summary>
    /// This method is used to get query string for stayback. 
    /// </summary>
    /// <param name="oCurrentItem"></param>
    /// <param name="asWeekDays"></param>
    /// <returns></returns>
    private string GetQueryString(ListViewDataItem oCurrentItem, string asWeekDays)
    {
        int iWeekDayId = (from WeekDay in mlstWeekDays
                          where WeekDay.WeekDay == asWeekDays
                          select WeekDay.WeekDayId).First();
        string sQueryString = string.Empty;
        sQueryString = "StandardDivisionId=" + Convert.ToInt32(lstvwStaybackLectures.DataKeys[oCurrentItem.DisplayIndex]["StandardwiseDivisionId"]) +
                       "&WeekDayId=" + iWeekDayId +
                       "&Is_Configured=" + hidIsConfigured.Value +
                       "&ExternalLecture=" + S_STAYBACK;
        return sQueryString;
    }

    /// <summary>
    /// This method use get query string for Assembly lecture
    /// </summary>
    /// <param name="oCurrentItem"></param>
    /// <param name="asWeekDays"></param>
    /// <returns></returns>
    private string GetQueryStringForAssembly(ListViewDataItem oCurrentItem, string asWeekDays)
    {
        int iWeekDayId = (from WeekDay in mlstWeekDays
                          where WeekDay.WeekDay == asWeekDays
                          select WeekDay.WeekDayId).First();
        string sQueryString = string.Empty;
        sQueryString = "StandardDivisionId=" + Convert.ToInt32(lstvwAssemblyLectures.DataKeys[oCurrentItem.DisplayIndex]["StandardwiseDivisionId"]) +
                       "&WeekDayId=" + iWeekDayId +
                       "&Is_Configured=" + hidIsConfigured.Value +
                       "&ExternalLecture=" + S_ASSEMBLY;
        return sQueryString;
    }

    /// <summary>
    /// This method use get query string for MPT lecture
    /// </summary>
    /// <param name="oCurrentItem"></param>
    /// <param name="asWeekDays"></param>
    /// <returns></returns>

    private string GetQueryStringForMPT(ListViewDataItem oCurrentItem, string asWeekDays)
    {
        int iWeekDayId = (from WeekDay in mlstWeekDays
                          where WeekDay.WeekDay == asWeekDays
                          select WeekDay.WeekDayId).First();
        string sQueryString = string.Empty;
        sQueryString = "StandardDivisionId=" + Convert.ToInt32(lstvwMPTLecture.DataKeys[oCurrentItem.DisplayIndex]["StandardwiseDivisionId"]) +
                       "&WeekDayId=" + iWeekDayId +
                       "&Is_Configured=" + hidIsConfigured.Value +
                       "&ExternalLecture=" + S_MPT;
        return sQueryString;
    }

   private string GetQueryStringForWeeklyTest(ListViewDataItem oCurrentItem, string asWeekDays)
    {
        int iWeekDayId = (from WeekDay in mlstWeekDays
                          where WeekDay.WeekDay == asWeekDays
                          select WeekDay.WeekDayId).First();
        string sQueryString = string.Empty;
        sQueryString = "StandardDivisionId=" + Convert.ToInt32(lstvwMPTLecture.DataKeys[oCurrentItem.DisplayIndex]["StandardwiseDivisionId"]) +
                       "&WeekDayId=" + iWeekDayId +
                       "&Is_Configured=" + hidIsConfigured.Value +
                       "&ExternalLecture=" + S_WEEKLYTEST;
        return sQueryString;
    }



    /// <summary>
    /// This method is used to get stay back lectures assigned to standard division.
    /// </summary>
    /// <param name="aiStandardwiseDivisionId"></param>
    /// <param name="asWeekDay"></param>
    /// <returns></returns>
    private string GetAssingedLectures(int aiStandardwiseDivisionId, string asWeekDay)
    {
        string sText = string.Empty;
        //Gets Week Day id for passed week day name(asWeekDay) from mlstWeekDays.
        int iWeekDay = (from WeekDay in mlstWeekDays
                        where WeekDay.WeekDay == asWeekDay
                        select WeekDay.WeekDayId).First();

        //Gets lectures for week day id(iWeekDay) from mlstStayBackLectureDetails.
        List<int> lstLectureNo = (from StayBackLecture in mlstStayBackLectureDetails
                                  where StayBackLecture.StandardwiseDivisionId == aiStandardwiseDivisionId
                                  && StayBackLecture.WeekDayId == iWeekDay
                                  select StayBackLecture.LectureNo).ToList();

        if (lstLectureNo.Count > 0)
        {
            //Gets  Week Day id for standardwise division id(aiStandardwiseDivisionId) from mlstStayBackLectureDetails.
            int iWeekDayId = (from StayBackLecture in mlstStayBackLectureDetails
                              where StayBackLecture.StandardwiseDivisionId == aiStandardwiseDivisionId
                              && StayBackLecture.WeekDayId == iWeekDay
                              select StayBackLecture.WeekDayId).First();

            (from WeekDay in mlstWeekDays
             where WeekDay.WeekDayId == iWeekDayId
             select WeekDay.WeekDay).ToList()
            .ForEach
            (WeekDay =>
            {
                if (WeekDay == asWeekDay)
                {
                    foreach (int olstLectureNo in lstLectureNo)
                    {
                        if (olstLectureNo != Constants.I_ZERO)
                            sText += ", " + olstLectureNo.ToString();
                    }
                }
            }
            );
            if (sText != string.Empty)
                return sText = Resources.LocalizedResources.LectNo + ": " + sText.Substring(1, sText.Length - 1);
        }
        sText = Resources.LocalizedResources.MsgAssignStayBackLectures;
        return sText;
    }
    /// <summary>
    /// This method is used to get assembly lectures assigned to standard division.
    /// </summary>
    /// <param name="aiStandardwiseDivisionId"></param>
    /// <param name="asWeekDay"></param>
    /// <returns></returns>
    private string GetAssingedLecturesForAssembly(int aiStandardwiseDivisionId, string asWeekDay)
    {
        string sText = string.Empty;
        //Gets Week Day id for passed week day name(asWeekDay) from mlstWeekDays.
        int iWeekDay = (from WeekDay in mlstWeekDays
                        where WeekDay.WeekDay == asWeekDay
                        select WeekDay.WeekDayId).First();

        //Gets lectures for week day id(iWeekDay) from mlstStayBackLectureDetails.
        List<int> lstLectureNo = (from StayBackLecture in mlstStayBackLectureDetails
                                  where StayBackLecture.StandardwiseDivisionId == aiStandardwiseDivisionId
                                  && StayBackLecture.WeekDayId == iWeekDay
                                  select StayBackLecture.LectureNo).ToList();

        if (lstLectureNo.Count > 0)
        {
            //Gets  Week Day id for standardwise division id(aiStandardwiseDivisionId) from mlstStayBackLectureDetails.
            int iWeekDayId = (from StayBackLecture in mlstStayBackLectureDetails
                              where StayBackLecture.StandardwiseDivisionId == aiStandardwiseDivisionId
                              && StayBackLecture.WeekDayId == iWeekDay
                              select StayBackLecture.WeekDayId).First();

            (from WeekDay in mlstWeekDays
             where WeekDay.WeekDayId == iWeekDayId
             select WeekDay.WeekDay).ToList()
            .ForEach
            (WeekDay =>
            {
                if (WeekDay == asWeekDay)
                {
                    foreach (int olstLectureNo in lstLectureNo)
                    {
                        if (olstLectureNo != Constants.I_ZERO)
                            sText += ", " + olstLectureNo.ToString();
                    }
                }
            }
            );
            if (sText != string.Empty)
                return sText = "Lect. No.: " + sText.Substring(1, sText.Length - 1);
        }
        sText = "Assign Assembly Lectures";
        return sText;
    }
    /// <summary>
    /// This method is used to get MPT lectures assigned to standard division.
    /// </summary>
    /// <param name="aiStandardwiseDivisionId"></param>
    /// <param name="asWeekDay"></param>
    /// <returns></returns>
    private string GetAssingedLecturesForMPT(int aiStandardwiseDivisionId, string asWeekDay)
    {
        string sText = string.Empty;
        //Gets Week Day id for passed week day name(asWeekDay) from mlstWeekDays.
        int iWeekDay = (from WeekDay in mlstWeekDays
                        where WeekDay.WeekDay == asWeekDay
                        select WeekDay.WeekDayId).First();

        //Gets lectures for week day id(iWeekDay) from mlstStayBackLectureDetails.
        List<int> lstLectureNo = (from StayBackLecture in mlstStayBackLectureDetails
                                  where StayBackLecture.StandardwiseDivisionId == aiStandardwiseDivisionId
                                  && StayBackLecture.WeekDayId == iWeekDay
                                  select StayBackLecture.LectureNo).ToList();

        if (lstLectureNo.Count > 0)
        {
            //Gets  Week Day id for standardwise division id(aiStandardwiseDivisionId) from mlstStayBackLectureDetails.
            int iWeekDayId = (from StayBackLecture in mlstStayBackLectureDetails
                              where StayBackLecture.StandardwiseDivisionId == aiStandardwiseDivisionId
                              && StayBackLecture.WeekDayId == iWeekDay
                              select StayBackLecture.WeekDayId).First();

            (from WeekDay in mlstWeekDays
             where WeekDay.WeekDayId == iWeekDayId
             select WeekDay.WeekDay).ToList()
            .ForEach
            (WeekDay =>
            {
                if (WeekDay == asWeekDay)
                {
                    foreach (int olstLectureNo in lstLectureNo)
                    {
                        if (olstLectureNo != Constants.I_ZERO)
                            sText += ", " + olstLectureNo.ToString();
                    }
                }
            }
            );
            if (sText != string.Empty)
                return sText = "Lect. No.: " + sText.Substring(1, sText.Length - 1);
        }
        sText = "Assign M.P.T. Lectures";
        return sText;
    }

    /// <summary>
    /// This method is used to get MPT lectures assigned to standard division.
    /// </summary>
    /// <param name="aiStandardwiseDivisionId"></param>
    /// <param name="asWeekDay"></param>
    /// <returns></returns>
    private string GetAssingedLecturesForWeeklyTimeTable(int aiStandardwiseDivisionId, string asWeekDay)
    {
        string sText = string.Empty;
        //Gets Week Day id for passed week day name(asWeekDay) from mlstWeekDays.
        int iWeekDay = (from WeekDay in mlstWeekDays
                        where WeekDay.WeekDay == asWeekDay
                        select WeekDay.WeekDayId).First();

        //Gets lectures for week day id(iWeekDay) from mlstStayBackLectureDetails.
        List<int> lstLectureNo = (from StayBackLecture in mlstStayBackLectureDetails
                                  where StayBackLecture.StandardwiseDivisionId == aiStandardwiseDivisionId
                                  && StayBackLecture.WeekDayId == iWeekDay
                                  select StayBackLecture.LectureNo).ToList();

        if (lstLectureNo.Count > 0)
        {
            //Gets  Week Day id for standardwise division id(aiStandardwiseDivisionId) from mlstStayBackLectureDetails.
            int iWeekDayId = (from StayBackLecture in mlstStayBackLectureDetails
                              where StayBackLecture.StandardwiseDivisionId == aiStandardwiseDivisionId
                              && StayBackLecture.WeekDayId == iWeekDay
                              select StayBackLecture.WeekDayId).First();

            (from WeekDay in mlstWeekDays
             where WeekDay.WeekDayId == iWeekDayId
             select WeekDay.WeekDay).ToList()
            .ForEach
            (WeekDay =>
            {
                if (WeekDay == asWeekDay)
                {
                    foreach (int olstLectureNo in lstLectureNo)
                    {
                        if (olstLectureNo != Constants.I_ZERO)
                            sText += ", " + olstLectureNo.ToString();
                    }
                }
            }
            );
            if (sText != string.Empty)
                return sText = "Lect. No.: " + sText.Substring(1, sText.Length - 1);
        }
        sText = "Assign Weekly Test";
        return sText;
    }

    /// <summary>
    /// This method is used to delete additional lecture when extra lecture is added at the same time.
    /// </summary>
    private void DeleteAdditionalLecture()
    {
        string sAssemblyWeekday = Settings.AssemblyWeekday;
        int iAssemblyLectNo = Settings.AssemblyLectNo;
        string sMPTWeekday = Settings.MPTWeekday;
        int iMPTLectNo = Settings.MPTLectNo;
        bool bIsStayBack = false;

        bool sAssemblyApplicable = Settings.IsAssemblyApplicable;
        bool sMPTApplicable = Settings.IsMPTApplicable;
        bool sStaybackApplicable = Settings.IsStaybackApplicable;


        string sAssemblyDay = string.Empty;
        int iTempAssemblyLectNo = 0;
        string sTempMPTWeekday = string.Empty;
        int iTempMPTLectNo = 0;

        for (int iRowCnt = 0; iRowCnt < lstvwExtenalLectureDetails.Items.Count; iRowCnt++)
        {
            int iTeacherId = lstvwExtenalLectureDetails.DataKeys[iRowCnt]["TeacherId"].ToInt();
            CheckBox chkMPT = lstvwExtenalLectureDetails.Items[iRowCnt].FindControl("chkMPT") as CheckBox;
            CheckBox chkAssembly = lstvwExtenalLectureDetails.Items[iRowCnt].FindControl("chkAssembly") as CheckBox;
            CheckBox chkStayback = lstvwExtenalLectureDetails.Items[iRowCnt].FindControl("chkStayback") as CheckBox;
            if (chkAssembly.Checked && sAssemblyApplicable)
            {
                sAssemblyDay = sAssemblyWeekday;
                iTempAssemblyLectNo = iAssemblyLectNo;
            }
            else
            {
                sAssemblyDay = string.Empty;
                iTempAssemblyLectNo = 0;
            }
            if (chkMPT.Checked && sMPTApplicable)
            {
                sTempMPTWeekday = sMPTWeekday;
                iTempMPTLectNo = iMPTLectNo;
            }
            else
            {
                sTempMPTWeekday = string.Empty;
                iTempMPTLectNo = 0;
            }

            if (chkStayback.Checked && sStaybackApplicable)
                bIsStayBack = true;
            else
                bIsStayBack = false;

            if (iTempAssemblyLectNo != 0 || iTempMPTLectNo != 0 || bIsStayBack)
                SchoolTimeTableMasterBL.DeleteAdditionalLecture(iTeacherId, iTempMPTLectNo, iTempAssemblyLectNo, sAssemblyDay, sTempMPTWeekday, bIsStayBack, miSchoolId, miAcademicYearId);
        }
    }

    /// <summary>
    /// This Method used to change value of messgae according to culture
    /// </summary>
    private void RefreshValues()
    {
        hidWarringExternalLecture.Value = Resources.LocalizedResources.WarringExternalLecture;
        FillStayBackDetailsListView();
        FillMPTDetailsListView();
        FillAssemblyDetailsListView();
        if (lstvwExtenalLectureDetails.Items.Count > 0)
            ControlUtility.FillListViewPagerFooterWithCulture(lstvwExtenalLectureDetails, DtPgCount, Resources.LocalizedResources.PageNo, Resources.LocalizedResources.Of, Resources.LocalizedResources.OutOflst);
    }

    #endregion " Private Methods "    
}