using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic.Exceptions;
using BusinessLogic;
using Utility;

public partial class LecturesPerStandardWeekday : SchoolBase
{
    #region Constants
    const Int32 I_MAX_LECT_PER_WEEKDAY = 2;
    const Int32 I_MAX_LECT_FOR_TEACHER = 3;
    const Int32 I_MAX_LECT_PER_WEEK = 1;
    #endregion

    #region Standards
    const Int32 I_STANDARD_ID_COLUMN_NUMBER = 0;
    const Int32 I_START_COUNT = 4;
    #endregion

    #region Datamembers
    
    private string IsConfig;
    #endregion

    #region event handlers

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Initialise();
            bool bIsUseSubmitBehavior = CommonUtility.CheckCancelOrBackClickEvent(this.Page);
            if (!IsPostBack)
            {
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                btnCancel.Attributes["onclick"] = "javascript:DisableButtons()";                
                ApplyMouseHoverEffect(new List<Button> { btnCancel, btnSave });
                if (CheckPreCondition())
                {
                    FillStandardGrid();
                    setFocusOnFirstDataEntryContl();
                    btnCancel.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Timetable_Related));
                }
                RefreshValues();    
            }
            else
            {
                FillStandardGrid();
                if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                    RefreshValues();
                }
            }

        }            
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    /// <summary>
    /// This method cancels all transaction and 
    /// navigate back to SchoolConfigurationControlPanel page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Timetable_Related)));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Event is fired when user clicks on save button.
    /// This handles saves the desired
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Hashtable oHashWeekdayLimitIds = new Hashtable();
            Hashtable oHashWeeklyLimitIds = new Hashtable();
            LecturesPerStandardBL oLecturesPerStandardBL = new LecturesPerStandardBL();
            string sStandardLectures = GetStandardLecturesXML(ref oHashWeekdayLimitIds);
            string sMaxStandardLecture = GetMaxStandardLectureXML(ref oHashWeeklyLimitIds);

            oLecturesPerStandardBL = PopulateMaxLectPerWeek(sStandardLectures, sMaxStandardLecture);

            oLecturesPerStandardBL.ManageStandardLectures(oHashWeeklyLimitIds, oHashWeekdayLimitIds);

            ReadQuerystring();
            if (IsConfig != "Y")
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.MaxLecturePerStandard));

            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Timetable_Related)));
        }
        catch (BusinessLogic.Exceptions.ReferenceExceptions ex)
        {
            lblErr.Text = CommonUtility.ModifyExceptionMessage(ex.Message, "Maximum no. of lectures for Standard",Resources.LocalizedResources.MaximumNoOfLectures,"cannot be reduced as Timetable is already configured.", Resources.LocalizedResources.TimetableAlreadyConfigured);
            lblErr.Text = CommonUtility.ModifyExceptionMessage(lblErr.Text, "No. of lectures per weekday for Standard", Resources.LocalizedResources.NoOfLecturesPerWeekDay, "cannot be reduced since Timetable is already configured.", Resources.LocalizedResources.TimetableAlreadyConfigured1);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region private methods

    /// <summary>   
    /// 
    /// </summary>
    private void Initialise()
    {
        InitializeMemberVariables();
        hidSchoolId.Value = Session["I_SCHOOL_ID"].ToString();        
        lblError.Text = "";
    }

    /// <summary>
    /// This method is used to generete columns of grid.
    /// </summary>
    private void GenerateColumns()
    {

        int iSchoolId = Convert.ToInt32(hidSchoolId.Value);
        WeekDaysMasterBL oWeekDaysMasterBL = new WeekDaysMasterBL();

        DataTable oDSAllWeekdays = oWeekDaysMasterBL.GetConfiguredWeekDays(iSchoolId, miAcademicYearId);

        LecturesPerStandardBL oLecturesPerStandardBL = new LecturesPerStandardBL();
        DataSet oDSGetRecords = LecturesPerStandardBL.GetAllStandardLectures(iSchoolId, miAcademicYearId);

        hidStayBackApplicable.Value = Settings.IsStaybackApplicable ? Constants.S_YES : Constants.S_NO;

        int iWeekdayCount = oDSAllWeekdays.Rows.Count;
        int iStandardCount = grdStandardWeekDay.Rows.Count;
        int k = 0;
        int iCount = 0;
        int iweekdayId = 0;
        Hashtable oHash = new Hashtable();
        for (int iHcnt = 0; iHcnt < grdStandardWeekDay.Rows.Count; iHcnt++)
        {
            oHash.Add(Convert.ToInt32(grdStandardWeekDay.Rows[iHcnt].Cells[I_STANDARD_ID_COLUMN_NUMBER].Text), false);
        }

        //Add Divisions to header rows
        for (int i = 0; i < iWeekdayCount; i++)
        {
            TableCell oTHeader = new TableCell();
            oTHeader.HorizontalAlign = HorizontalAlign.Center;
            oTHeader.Wrap = false;
            oTHeader.Style.Add(HtmlTextWriterStyle.PaddingLeft, "3");
            oTHeader.Style.Add(HtmlTextWriterStyle.PaddingRight, "3");
            oTHeader.Text = oDSAllWeekdays.Rows[i]["WeekDay_name"].ToString();
            k = grdStandardWeekDay.HeaderRow.Cells.Add(oTHeader);
        }

        // Header cell for week.
        TableCell oTHeaderWeek = new TableCell();
        oTHeaderWeek.HorizontalAlign = HorizontalAlign.Center;
        oTHeaderWeek.Wrap = false;
        oTHeaderWeek.Style.Add(HtmlTextWriterStyle.PaddingLeft, "3");
        oTHeaderWeek.Style.Add(HtmlTextWriterStyle.PaddingRight, "3");
        oTHeaderWeek.Text = Resources.LocalizedResources.MsgMaxLecturePerWeek;
        k = grdStandardWeekDay.HeaderRow.Cells.Add(oTHeaderWeek);

        //Add rows contains standard name.
        HiddenField hidStandard;
        HiddenField hidWeekday;
        int iRowId = 0;
        for (int iStandardIndex = 0; iStandardIndex < iStandardCount; iStandardIndex++)
        {
            int iStandardId = Convert.ToInt32(grdStandardWeekDay.Rows[iStandardIndex].Cells[I_STANDARD_ID_COLUMN_NUMBER].Text);
            //Add columns contains weekdays.
            for (int iWeekdayIndex = 0; iWeekdayIndex < iWeekdayCount; iWeekdayIndex++, iRowId++)
            {
                iweekdayId = Convert.ToInt32(oDSAllWeekdays.Rows[iWeekdayIndex]["WeekDays_id"].ToString());
                //Set properties of hiddenfield which contains standard-weekday name.
                hidStandard = new HiddenField();
                hidStandard.Value = grdStandardWeekDay.Rows[iStandardIndex].Cells[1].Text;
                hidStandard.ID = "hids_0" + (iStandardIndex + 2) + "_" + iWeekdayIndex;

                hidWeekday = new HiddenField();
                hidWeekday.Value = oDSAllWeekdays.Rows[iWeekdayIndex]["WeekDay_name"].ToString();
                hidWeekday.ID = "hidw_0" + (iStandardIndex + 2) + "_" + iWeekdayIndex;

                TableCell oT = new TableCell();

                oT.Width = 200;
                oT.HorizontalAlign = HorizontalAlign.Center;
                oT.Wrap = false;
                oT.Style.Add(HtmlTextWriterStyle.PaddingLeft, "3");
                oT.Style.Add(HtmlTextWriterStyle.PaddingRight, "3");
                oT.Text = oDSAllWeekdays.Rows[iWeekdayIndex]["WeekDays_id"].ToString();
                oT.Attributes.Add("title", Resources.LocalizedResources.Standard + " - " + grdStandardWeekDay.Rows[iStandardIndex].Cells[1].Text + " [" + oDSAllWeekdays.Rows[iWeekdayIndex]["WeekDay_name"].ToString() + "]");
                k = grdStandardWeekDay.Rows[iStandardIndex].Cells.Add(oT);

                Label olbl1 = new Label();

                TextBox oTxt1 = new TextBox();

                oTxt1.Attributes.Add("onblur", "extractNumber(this,0,false);");

                oTxt1.Attributes.Add("onkeyup", "extractNumber(this,0,false);");

                oTxt1.Attributes.Add("onkeypress", "return blockNonNumbers(this, event, false, false);");
                oTxt1.Attributes.Add("onpaste", "event.returnValue=false;");
                oTxt1.Attributes.Add("ondrop", "event.returnValue=false;");

                oTxt1.ID = "txt_0" + (iStandardIndex + 2) + "_" + iWeekdayIndex;
                oTxt1.CssClass = "TxtBoxNOL";
                oTxt1.Style.Add(HtmlTextWriterStyle.PaddingLeft, "3");
                oTxt1.MaxLength = 2;

                HiddenField oHidWeekDayLimitId = new HiddenField();
                oHidWeekDayLimitId.Value = "0";

                HiddenField oHidWeekDayMaxLectures = new HiddenField();
                oHidWeekDayMaxLectures.Value = "0";
                grdStandardWeekDay.Rows[iStandardIndex].Cells[k].Controls.Add(olbl1);
                grdStandardWeekDay.Rows[iStandardIndex].Cells[k].Controls.Add(oTxt1);
                grdStandardWeekDay.Rows[iStandardIndex].Cells[k].Controls.Add(hidStandard);
                grdStandardWeekDay.Rows[iStandardIndex].Cells[k].Controls.Add(hidWeekday);
                grdStandardWeekDay.Rows[iStandardIndex].Cells[k].Controls.Add(oHidWeekDayLimitId);
                grdStandardWeekDay.Rows[iStandardIndex].Cells[k].Controls.Add(oHidWeekDayMaxLectures);

                if (hidStayBackApplicable.Value == Constants.C_YES.ToString())
                {
                    HiddenField oHidMaxLecturesPerStandard = new HiddenField();
                    oHidMaxLecturesPerStandard.ID = "hidMaxLect_0" + (iStandardIndex + 2) + "_" + iWeekdayIndex;
                    oHidMaxLecturesPerStandard.Value = oDSGetRecords.Tables[0].Rows[iRowId]["MaxLectureNo"].ToString();
                    grdStandardWeekDay.Rows[iStandardIndex].Cells[k].Controls.Add(oHidMaxLecturesPerStandard);
                }

                //Set last saved values.                
                if (oDSGetRecords.Tables[0].Rows.Count > 0)
                {
                    DataRow[] oDRLecture = oDSGetRecords.Tables[0].Select("Standard_Id=" + iStandardId + " AND " + "Weekday_Id=" + iweekdayId);
                    if (oDRLecture.Length > 0)
                    {
                        oTxt1.Text = oDRLecture[0]["Max_lectures_per_standard"].ToString();
                        oHidWeekDayLimitId.Value = oDRLecture[0]["Lectures_Per_Standard_Weekdays_Id"].ToString();
                        oHidWeekDayMaxLectures.Value = oTxt1.Text;
                        oHash[iStandardId] = true;
                    }
                    else
                    {
                        oTxt1.Text = "0";
                        oHidWeekDayLimitId.Value = "0";
                        oHidWeekDayMaxLectures.Value = "0";
                    }
                }
            }

            TableCell oTWeek = new TableCell();
            oTWeek.HorizontalAlign = HorizontalAlign.Center;
            oTWeek.Attributes.Add("title", Resources.LocalizedResources.MaxLectures + " : " + grdStandardWeekDay.Rows[iStandardIndex].Cells[1].Text);

            k = grdStandardWeekDay.Rows[iStandardIndex].Cells.Add(oTWeek);

            // Add last column for week.
            TextBox oTxt3 = new TextBox();

            oTxt3.Attributes.Add("onblur", "extractNumber(this,0,false);");
            oTxt3.Attributes.Add("onkeyup", "extractNumber(this,0,false);");
            oTxt3.Attributes.Add("onkeypress", "return blockNonNumbers(this, event, false, false);");
            oTxt3.Attributes.Add("onpaste", "event.returnValue=false;");
            oTxt3.Attributes.Add("ondrop", "event.returnValue=false;");
            oTxt3.ID = "txt2_0" + (iStandardIndex + 2) + "_" + (iWeekdayCount - 1);
            oTxt3.CssClass = "TxtBoxMaxLPWeek";
            oTxt3.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
            oTxt3.MaxLength = 2;
            grdStandardWeekDay.Rows[iStandardIndex].Cells[k].Controls.Add(oTxt3);

            HiddenField oHidWeeklyMax = new HiddenField();
            oHidWeeklyMax.Value = "0";
            grdStandardWeekDay.Rows[iStandardIndex].Cells[k].Controls.Add(oHidWeeklyMax);

            HiddenField oHidId = new HiddenField();
            oHidId.Value = "0";
            grdStandardWeekDay.Rows[iStandardIndex].Cells[k].Controls.Add(oHidId);
            //Set last saved values.
            if (oDSGetRecords.Tables[1].Rows.Count > 0)
            {
                DataRow[] oDRLecture = oDSGetRecords.Tables[0].Select("Standard_Id=" + iStandardId + " AND " + "Weekday_Id=" + iweekdayId);
                if (oDRLecture.Length > 0 || Convert.ToBoolean(oHash[iStandardId]))
                {
                    DataRow[] oDRMaxLectPerWeek = oDSGetRecords.Tables[1].Select("Standard_Id=" + iStandardId);
                    oTxt3.Text = oDRMaxLectPerWeek[Constants.I_ZERO][1].ToString(); ;
                    oHidId.Value = oDSGetRecords.Tables[1].Rows[iCount]["Lectures_Per_Standard_Week_Id"].ToString();
                    iCount++;
                }
                else
                {
                    oTxt3.Text = "0";
                    oHidId.Value = "0";
                }
                oHidWeeklyMax.Value = oTxt3.Text;
            }
        }

        hidRowCount.Value = k.ToString();
        hidColumnCount.Value = iWeekdayCount.ToString();
    }
    /// <summary>
    /// This function checks the preconditons of Configured Subjects for Subject Group criteria.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.MaxLecturePerStandard);
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
    /// This method is used to visible or hide controls on page load as per configuration is 
    /// done or not.
    /// </summary>
    private void VisibleOrHideControls()
    {
        tblLegend.Visible = false;
        btnSave.Visible = false;
        divGridView.Visible = false;
        btnCancel.Text = "Back";
    }

    /// <summary>
    /// This method is used to bind datasource to grid.
    /// </summary>
    private void FillStandardGrid()
    {
        grdColsVisible(true);
        int iSchoolId = Convert.ToInt32(hidSchoolId.Value);

        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(iSchoolId, miAcademicYearId);
        DataTable oDSAllStandards = oStandardCollectionBL.GetAssociatedStandards();
        grdStandardWeekDay.DataSource = oDSAllStandards;
        grdStandardWeekDay.DataBind();
        grdColsVisible(false);
        GenerateColumns();
        btnSave.Attributes.Add("onclick", "if(!(validatetextbox(" + hidColumnCount.Value + ",this))){return false ;}");
    }

    /// <summary>
    /// This method is used to visible or hide grid's column.
    /// </summary>
    /// <param name="abAction"></param>
    private void grdColsVisible(bool abAction)
    {
        // This method hides the Groupid column from Gridview grdStandardWeekDay.
        grdStandardWeekDay.Columns[I_STANDARD_ID_COLUMN_NUMBER].Visible = abAction;
    }

    /// <summary>
    /// This method is used to set fields of LecturesPerStandardBL.
    /// </summary>
    /// <param name="sStandardLectures"></param>
    /// <param name="sMaxStandardLecture"></param>
    /// <returns>LecturesPerStandardBL</returns>
    private LecturesPerStandardBL PopulateMaxLectPerWeek(string sStandardLectures, string sMaxStandardLecture)
    {
        LecturesPerStandardBL oLecturesPerStandardBL = new LecturesPerStandardBL();
        oLecturesPerStandardBL.School_Id = miSchoolId;
        oLecturesPerStandardBL.Academic_Year_ID = miAcademicYearId;
        oLecturesPerStandardBL.Inserted_By_Id = miUserId;
        oLecturesPerStandardBL.MaxLecturesPerStandardWeekDay = sStandardLectures;
        oLecturesPerStandardBL.MaxLecturesPerWeek = sMaxStandardLecture;
        return oLecturesPerStandardBL;
    }

    /// <summary>
    /// This methode is used to set focus on first data entry control of page
    /// </summary>
    private void setFocusOnFirstDataEntryContl()
    {
        TextBox txtCell = (TextBox)(grdStandardWeekDay.Rows[0].Cells[2].Controls[1]);
        txtCell.Focus();
    }

    /// <summary>
    /// This method creates XML which contains maximum lectures per standard-weekday
    /// and Max_teacher_lectures_per_standard.
    /// </summary>
    /// <returns>string</returns>
    private string GetStandardLecturesXML(ref Hashtable oHash)
    {
        WeekDaysMasterBL oWeekDaysMasterBL = new WeekDaysMasterBL();
        DataTable oDSAllWeekdays = oWeekDaysMasterBL.GetConfiguredWeekDays(miSchoolId, miAcademicYearId);

        int iWeekdayCount = oDSAllWeekdays.Rows.Count;
        int iStandardCount = grdStandardWeekDay.Rows.Count;
        string sMessage = string.Empty;
        string sStandardMessage = string.Empty;
        string sWeekDaysMessage = string.Empty;
        bool bStandard = true;

        XmlDocument oDoc = new XmlDocument();
        const string S_ELEMENT = "element";

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("StandardLectures");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "StandardLectures", "");

        for (int iStandardIndex = 0; iStandardIndex < iStandardCount; iStandardIndex++)
        {
            int iStandardId = Convert.ToInt32(grdStandardWeekDay.Rows[iStandardIndex].Cells[I_STANDARD_ID_COLUMN_NUMBER].Text);
            bStandard = true;
            sStandardMessage = sMessage;
            sWeekDaysMessage = string.Empty;

            for (int iWeekdayIndex = 0; iWeekdayIndex < iWeekdayCount; iWeekdayIndex++)
            {
                int iweekdayId = Convert.ToInt32(oDSAllWeekdays.Rows[iWeekdayIndex]["WeekDays_id"].ToString());
                TextBox otxt1 = (TextBox)(grdStandardWeekDay.Rows[iStandardIndex].Cells[iWeekdayIndex + 2].Controls[1]);
                HiddenField oHidName = (HiddenField)(grdStandardWeekDay.Rows[iStandardIndex].Cells[iWeekdayIndex + 2].Controls[2]);
                HiddenField oHidId = (HiddenField)(grdStandardWeekDay.Rows[iStandardIndex].Cells[iWeekdayIndex + 2].Controls[3]);
                HiddenField oHidMaxLectures = (HiddenField)(grdStandardWeekDay.Rows[iStandardIndex].Cells[iWeekdayIndex + 2].Controls[5]);
                HiddenField oHidLecturesID = (HiddenField)(grdStandardWeekDay.Rows[iStandardIndex].Cells[iWeekdayIndex + 2].Controls[4]);
                HiddenField oHidMaxAssignedStayBackLectures = (HiddenField)(grdStandardWeekDay.Rows[iStandardIndex].Cells[iWeekdayIndex + 2].Controls[6]);

                if (Convert.ToInt32(oHidMaxAssignedStayBackLectures.Value) <= Convert.ToInt32(otxt1.Text))
                {
                    if (!oHidMaxLectures.Value.Equals(otxt1.Text) && !oHidMaxLectures.Value.Equals("0"))
                    {
                        if (Convert.ToInt32(otxt1.Text) < Convert.ToInt32(oHidMaxLectures.Value))
                        {
                            oHash.Add(oHidLecturesID.Value, oHidName.Value);
                        }
                    }

                    XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "StandardLecturesDetails", "");

                    string sAtrrName = "Standard_Id";
                    XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = Convert.ToString(iStandardId);
                    oXmlNode.Attributes.Append(attr);

                    string sAtrrName1 = "WeekDay_Id";
                    XmlAttribute attr1 = oDoc.CreateAttribute(sAtrrName1);
                    attr1.Value = Convert.ToString(iweekdayId);
                    oXmlNode.Attributes.Append(attr1);

                    string sAtrrName2 = "Max_lectures_per_standard";
                    XmlAttribute attr2 = oDoc.CreateAttribute(sAtrrName2);
                    attr2.Value = otxt1.Text;
                    oXmlNode.Attributes.Append(attr2);

                    oXmlRootNode.AppendChild(oXmlNode);
                }
                else
                {
                    if (bStandard)
                    {
                        sStandardMessage += ", " + oHidName.Value + " (";
                        bStandard = false;
                    }
                    DataRow[] odrWeekday= (DataRow[])(oDSAllWeekdays.Select("WeekDays_id = " + iweekdayId));
                    sWeekDaysMessage += ", " + odrWeekday[0]["WeekDay_name"].ToString();
                }
            }
            if (!bStandard)
                sMessage = ((iStandardIndex == 0) ? sStandardMessage.Substring(Constants.I_ONE) : sStandardMessage) + sWeekDaysMessage.Substring(Constants.I_ONE) + " )";

            // Add the root node to document element. 
            root.AppendChild(oXmlRootNode);
        }
        if (!string.IsNullOrEmpty(sMessage))
        {
            sMessage = Resources.LocalizedResources.ValMsgNoOfLectures + ": " + sMessage.Substring(2);
            throw new BusinessLogic.Exceptions.ReferenceExceptions(sMessage);
        }
        // return the string generated.
        return root.InnerXml;
    }

    /// <summary>
    /// This method is used to decrypt encrypted querystring.
    /// </summary>
    private void ReadQuerystring()
    {
        try
        {
	        if (Request.QueryString.ToString() != Constants.S_EMPTY_STRING)
		        IsConfig = QueryString["Is_Configured"];
        }
        catch (Exception)
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
			oMasterPage.RedirectToNextPage(Constants.S_PAGE_ERROR);
        }
    }

    /// <summary>
    /// This method creates XML which contains maximum lectures per standard in week.
    /// </summary>
    /// <returns>string</returns>
    //private string GetMaxStandardLectureXML(ref ArrayList oArr)
    private string GetMaxStandardLectureXML(ref Hashtable oHash)
    {
        WeekDaysMasterBL oWeekDaysMasterBL = new WeekDaysMasterBL();
        DataTable oDSAllWeekdays = oWeekDaysMasterBL.GetConfiguredWeekDays(miSchoolId, miAcademicYearId);

        XmlDocument oDoc = new XmlDocument();
        const string S_ELEMENT = "element";

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("StandardLecturesWeek");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "StandardLecturesWeek", "");

        int iStandardCount = grdStandardWeekDay.Rows.Count;
        int iWeekdayCount = oDSAllWeekdays.Rows.Count;

        for (int iStandardIndex = 0; iStandardIndex < iStandardCount; iStandardIndex++)
        {
            int iStandardId = Convert.ToInt32(grdStandardWeekDay.Rows[iStandardIndex].Cells[I_STANDARD_ID_COLUMN_NUMBER].Text);

            XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "StandardLecturesWeekDetail", "");

            TextBox otxt3 = (TextBox)(grdStandardWeekDay.Rows[iStandardIndex].Cells[iWeekdayCount + 2].Controls[0]);
            HiddenField oHidLectures = (HiddenField)(grdStandardWeekDay.Rows[iStandardIndex].Cells[iWeekdayCount + 2].Controls[1]);
            HiddenField oHidId = (HiddenField)(grdStandardWeekDay.Rows[iStandardIndex].Cells[iWeekdayCount + 2].Controls[2]);
            if (!oHidLectures.Value.Equals(otxt3.Text) && !oHidLectures.Value.Equals("0"))
            {
                if (Convert.ToInt32(otxt3.Text) < Convert.ToInt32(oHidLectures.Value))
                {
                    oHash.Add(oHidId.Value, grdStandardWeekDay.Rows[iStandardIndex].Cells[1].Text);
                }
            }

            string sAtrrName1 = "Standard_Id";
            XmlAttribute attr1 = oDoc.CreateAttribute(sAtrrName1);
            attr1.Value = Convert.ToString(iStandardId);
            oXmlNode.Attributes.Append(attr1);

            string sAtrrName2 = "Max_No_of_lectures_per_standard_In_Week";
            XmlAttribute attr2 = oDoc.CreateAttribute(sAtrrName2);
            attr2.Value = otxt3.Text;
            oXmlNode.Attributes.Append(attr2);

            // Add the node to root node.
            oXmlRootNode.AppendChild(oXmlNode);

        }

        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);

        // return the string generated.
        return root.InnerXml;
    }

    
    /// <summary>
    /// This Method used to change value of messgae according to culture
    /// </summary>
    private void RefreshValues()
    {
        hidValMaximumLecturesCondition.Value=Resources.LocalizedResources.ValMaximumLecturesCondition;
        hidValMaximumLecturesBlank.Value=Resources.LocalizedResources.ValMaximumLecturesBlank;
        hidPleaseFixFollowingError.Value=Resources.LocalizedResources.PleaseFixFollowingError;
        hidValMaximumLectures.Value=Resources.LocalizedResources.ValMaximumLectures;
        hidForStandard.Value = Resources.LocalizedResources.Standard;
        hidWeekday.Value = Resources.LocalizedResources.Weekday;
    }

    #endregion
}
