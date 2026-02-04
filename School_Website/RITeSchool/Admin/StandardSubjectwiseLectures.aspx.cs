// File Name  : StandardSubjectwiseLectures.aspx.cs
// Created By : Anugandha
// Date       : 29/02/2008
//Description :This class is used to assign number
//             of lectures of a particular subject to each standard.

using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using BusinessLogic.Exceptions;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using BusinessLogic;
using Utility;

public partial class StdSubWiseLectures : SchoolBase
{
    #region Constants

    const string S_COL_SUB_ID = "Subject_Id";
    const string S_COL_STDSUB_ID = "Division_Subject_Id";

    const Int32 I_STD_COL_INDEX = 2;
    const int I_MAX_LECT_PER_STD_SUB = 1;
    const int I_FIRST_CONTROL_INDEX = 1;
    const int I_TBL_STD = 0;
    const int I_TBL_SUBJECT = 1;
    const int I_TBL_STDSUBJECT = 2;
    const int I_TBL_LECTURES = 3;
    #endregion

    #region members

    private DataSet moDsLectures;

    #endregion

    #region Events

    /// <summary>
    /// This event is used to set focus on first data entry control of grid.
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
                if (grdStandards.Rows.Count > 0)
                {
                    grdStandards.Columns[0].HeaderText = "";
                }
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
    /// This event is used to intialize page and fill grid controls.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnInit(EventArgs e)
    {
        try
        {
            base.OnInit(e);
            InitializeMemberVariables();
            if (!IsPostBack)
            {
                btnCancel.Attributes["onclick"] = "javascript:DisableButtons()";
                btnCancel.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Timetable_Related));            
                ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel });
            }
            FillStandardsGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }        
    }

    /// <summary>
    /// This event is used to save all transactions.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
			string sErrorMsg=CheckUpdatedCount();
			if(!string.IsNullOrEmpty(sErrorMsg ))
			{
                lblErr.Text = sErrorMsg.Replace("Maximum no. of lectures for following", Resources.LocalizedResources.RefValTimeTable1).Replace("cannot be reduced as Timetable already contains greater number of lectures", Resources.LocalizedResources.RefValTimeTable);
	        }
			else
			{
            const Int32 I_NUMERIC_BOX_CONTROL_INDEX = 1;            
            SubjectCollectionBL oSubjectCollectionBL = new SubjectCollectionBL(miSchoolId, miAcademicYearId);
            Collection<StandardMasterBL> oStandardCollection = new Collection<StandardMasterBL>();
            Hashtable oHash = new Hashtable();
            object[] objStdSubjects = new object[2];
            object[] objLectures = new object[1];
            //This loop is for each standard.
            for (int iRowCount = 0; iRowCount < grdStandards.Rows.Count; iRowCount++)
            {
                StandardMasterBL oStandardMasterBL = new StandardMasterBL();
                Collection<LecturesPerStandardSubjectWeekBL> oLectCountCollection = new Collection<LecturesPerStandardSubjectWeekBL>();
                int iStandardId = Convert.ToInt32(grdStandards.Rows[iRowCount].Cells[I_STD_COL_INDEX].Text);
                LecturesPerStandardSubjectWeekBL oLectPerStdSubWeekBL = new LecturesPerStandardSubjectWeekBL();
                DataTable oDtAllSubjects = moDsLectures.Tables[I_TBL_SUBJECT];

                //This loop is for each subject of a particular standard.
                for (int iCellIndex = 2; iCellIndex <= oDtAllSubjects.Rows.Count+1 ; iCellIndex++)
                {
                    int iSubjectId = Convert.ToInt32(oDtAllSubjects.Rows[iCellIndex - 2][S_COL_SUB_ID].ToString());
                    objStdSubjects[0] = iSubjectId;
                    objStdSubjects[1] = iStandardId;
                    DataRow oDRStandardSubjectId = moDsLectures.Tables[I_TBL_STDSUBJECT].Rows.Find(objStdSubjects);
                    //If grid's cell contains textbox then go for adding or modifying values.
                    if (grdStandards.Rows[iRowCount].Cells[iCellIndex+1].Controls.Count > 0 && oDRStandardSubjectId != null)
                    {
                        int iStandardSubjectId = Convert.ToInt32(oDRStandardSubjectId[S_COL_STDSUB_ID]);
                        objLectures[0] = iStandardSubjectId;
                        DataRow oDrLectures = moDsLectures.Tables[I_TBL_LECTURES].Rows.Find(objLectures);
                        TextBox otxtLecCount = (TextBox)(grdStandards.Rows[iRowCount].Cells[iCellIndex+1].Controls[I_NUMERIC_BOX_CONTROL_INDEX]);

                        //Used to add new records.
                        if (oDrLectures == null)
                        {
                            oLectPerStdSubWeekBL = PopulateFields(Convert.ToInt32(otxtLecCount.Text), iStandardSubjectId);
                            oLectPerStdSubWeekBL.ConfigurationAction = Constants.Action.Insert;
                            oLectCountCollection.Add(oLectPerStdSubWeekBL);
                        }

                        //Used to modify records.
                        else if (oDrLectures[1].ToString() != otxtLecCount.Text)
                        {
                            int iLectCountId = Convert.ToInt32(oDrLectures["Lectures_Per_Standard_Subject_Week_Id"].ToString());
                            oLectPerStdSubWeekBL = PopulateFields(Convert.ToInt32(otxtLecCount.Text), iStandardSubjectId, iLectCountId);
                            oLectPerStdSubWeekBL.ConfigurationAction = Constants.Action.Update;
                            oLectCountCollection.Add(oLectPerStdSubWeekBL);
                        }
                    }
                }
                //Check count of collection object.
                if (oLectCountCollection.Count > 0)
                {
                    oStandardMasterBL.LectureCountCollection = oLectCountCollection;
                    oStandardCollection.Add(oStandardMasterBL);
                }
            }
            if (oStandardCollection.Count > 0)
            {
                StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId);
                oStandardCollectionBL.UpdateLectureCount(oStandardCollection, oHash, miAcademicYearId);
            }

            string sIsConfig = ReadQuerystring();
            if (sIsConfig != "Y")
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.WeeklyMaxLecturePerStandardSubject));
            
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Timetable_Related)));
			}
        }
        catch (BusinessLogic.Exceptions.ReferenceExceptions ex)
        {
            lblErr.Text = ex.Message;
            FillStandardsGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Grid Event

    /// <summary>
    /// This event is used to apply Css class to Header and other rows. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStandards_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            e.Row.Cells[0].CssClass = "locked";
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.Cells[0].CssClass = "Llocked";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// This method is used to decrypt encrypted querystring.
    /// </summary>
    private string ReadQuerystring()
    {
        try
        {
			if (QueryString["Is_Configured"] != null)
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
    /// This method checks the preconditons of Configured Subjects for Subject Group criteria.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.WeeklyMaxLecturePerStandardSubject);
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
    /// This method is used to visible and hide controls.
    /// </summary>
    private void VisibleOrHideControls()
    {
        pnlLegend.Visible = false;
        grdStandards.Visible = false;
        btnSave.Visible = false;
        btnCancel.Text = "Back";
        tdGrid.Visible = false;
    }

    /// <summary>
    /// This method is used to fill grid with standard names.
    /// </summary>
    private void FillStandardsGrid()
    {
        if (CheckPreCondition())
        {
            LecturesPerStandardSubjectWeekCollectionBL objLectures = new LecturesPerStandardSubjectWeekCollectionBL();
            moDsLectures = objLectures.GetStdSubjectLectures(miSchoolId, miAcademicYearId);
            SetPrimaryKey();
            grdStandards.Columns[2].Visible = true;
            grdStandards.DataSource = moDsLectures.Tables[I_TBL_STD];
            grdStandards.DataBind();
            grdStandards.Columns[2].Visible =false;
            AddSubjectColumns();
            btnSave.Attributes.Add("onclick", "if(!(validatetextbox(" + hidColumnCount.Value + "))){return false;}");
        }
    }

    /// <summary>
    /// This method is used to generate columns of subjects of grid dynamically
    /// which is attached to grid one by one and show textbox 
    /// </summary>
    private void AddSubjectColumns()
    {
        int iCellIndex = 0;        
        int iSubjectIndex;
        DataTable oDSAllSubjects = moDsLectures.Tables[I_TBL_SUBJECT];

        //This method is used to set header to grid.
        AddHeaderToGrid();

        //This loop is for each standard.
        for (int iRowIndex = 0; iRowIndex < grdStandards.Rows.Count; iRowIndex++)
        {
            //This loop is for each subject of a particular standard.
            for (iSubjectIndex = 0; iSubjectIndex < oDSAllSubjects.Rows.Count; iSubjectIndex++)
            {
                iCellIndex = AddControlToGrid(iRowIndex, iSubjectIndex);
            }
        }
        hidColumnCount.Value = iCellIndex.ToString();
    }

    /// <summary>
    /// This method is used to set fields of LecturesPerStandardSubjectWeekBL class.
    /// </summary>
    /// <param name="aiLectureCount"></param>
    /// <param name="aiStandardSubjectId"></param>
    /// <returns>LecturesPerStandardSubjectWeekBL</returns>
    private LecturesPerStandardSubjectWeekBL PopulateFields(int aiLectureCount, int aiStandardSubjectId)
    {
        LecturesPerStandardSubjectWeekBL oLectPerStdSubWeekBL = new LecturesPerStandardSubjectWeekBL();        
        oLectPerStdSubWeekBL.School_Id = miSchoolId;
        oLectPerStdSubWeekBL.Academic_Year_Id = miAcademicYearId;
        oLectPerStdSubWeekBL.Inserted_By_Id = miUserId;
        oLectPerStdSubWeekBL.Max_Lectures_Per_Standard_Subject = aiLectureCount;
        oLectPerStdSubWeekBL.Is_Deleted = aiLectureCount==0 ? Constants.S_YES : Constants.S_NO;
        oLectPerStdSubWeekBL.Standard_Subject_Id = aiStandardSubjectId;
        return oLectPerStdSubWeekBL;
    }

    /// <summary>
    /// This method is used to set fields of LecturesPerStandardSubjectWeekBL class in edit mode.
    /// </summary>
    /// <param name="aiLectureCount"></param>
    /// <param name="aiStandardSubjectId"></param>
    /// <param name="aiLecCountId"></param>
    /// <returns>LecturesPerStandardSubjectWeekBL</returns>
    private LecturesPerStandardSubjectWeekBL PopulateFields(int aiLectureCount, int aiStandardSubjectId, int aiLecCountId)
    {
        LecturesPerStandardSubjectWeekBL oLectPerStdSubWeekBL = new LecturesPerStandardSubjectWeekBL();        
        oLectPerStdSubWeekBL.School_Id = miSchoolId;
        oLectPerStdSubWeekBL.Academic_Year_Id = miAcademicYearId;
        oLectPerStdSubWeekBL.Updated_By_Id = miUserId;
        oLectPerStdSubWeekBL.Max_Lectures_Per_Standard_Subject = aiLectureCount;
        oLectPerStdSubWeekBL.Is_Deleted = aiLectureCount==0 ? Constants.S_YES : Constants.S_NO;
        oLectPerStdSubWeekBL.Standard_Subject_Id = aiStandardSubjectId;
        oLectPerStdSubWeekBL.Lectures_Per_Standard_Subject_Week_Id = aiLecCountId;
        return oLectPerStdSubWeekBL;
    }

    /// <summary>
    /// This method is used to create numeric box as well to set its properties.
    /// </summary>
    /// <param name="otxtLecture"></param>
    /// <param name="iRowIndex"></param>
    /// <param name="iSubjectIndex"></param>
    private void SetNumericBoxProperties(TextBox otxtLecture, int iRowIndex, int iSubjectIndex) //Numeric
    {
        otxtLecture.MaxLength = 2;
        otxtLecture.Width = 40;
        otxtLecture.ID = "txt_0" + (iRowIndex + 3) + "_" + iSubjectIndex;
    }

    /// <summary>
    /// This method is used to set header to grid
    /// </summary>
    /// <param name="aDSAllSubjects"></param>
    private void AddHeaderToGrid()
    {
        
        int iSubjectIndex;
        DataTable aDtAllSubjects = moDsLectures.Tables[I_TBL_SUBJECT];
        int k = 0;
        int headerCellNo = 0;
        for (iSubjectIndex = 0; iSubjectIndex < aDtAllSubjects.Rows.Count; iSubjectIndex++)
        {
            Label olbl = new Label();
            DataControlFieldHeaderCell oTHeader = new DataControlFieldHeaderCell(null);
            oTHeader.CssClass = "locked";
            oTHeader.Text= olbl.Text = aDtAllSubjects.Rows[iSubjectIndex][3].ToString();
            oTHeader.HorizontalAlign = HorizontalAlign.Center;
            oTHeader.Wrap = false;
            oTHeader.Style.Add(HtmlTextWriterStyle.PaddingLeft, "5");
            oTHeader.Style.Add(HtmlTextWriterStyle.PaddingRight, "5");
            oTHeader.Style.Add(HtmlTextWriterStyle.MarginRight, "5");
            k = grdStandards.HeaderRow.Cells.Add(oTHeader);
            grdStandards.HeaderRow.Cells[k].Controls.Add(olbl);
            TextBox oChkHeader = new TextBox();
            SetTextBoxProperties(oChkHeader);
            grdStandards.HeaderRow.Cells[k].Controls.Add(oChkHeader);
            oChkHeader.Attributes.Add("onchange", "SetToAllRows(this, " + headerCellNo + ")");
            headerCellNo = headerCellNo + 1;
       }
    }
   
    /// <summary>
    /// This method is used to add numeric box to grid's cell.
    /// </summary>
    /// <param name="aiRowIndex"></param>
    /// <param name="aiSubjectIndex"></param>
    /// <param name="aoDSAllSubjects"></param>
    private Int32 AddControlToGrid(int aiRowIndex, int aiSubjectIndex)
    {
        
        int iCellIndex;
        int iStandardId = Convert.ToInt32(grdStandards.Rows[aiRowIndex].Cells[I_STD_COL_INDEX].Text);
        LecturesPerStandardSubjectWeekBL oLectPerStdSubWeekBL = new LecturesPerStandardSubjectWeekBL();
        LecturesPerStandardSubjectWeekCollectionBL olectureCollectionBL = new LecturesPerStandardSubjectWeekCollectionBL();
        HiddenField hidStandardName = GetHiddenFieldWithId(aiRowIndex, aiSubjectIndex, moDsLectures.Tables[I_TBL_SUBJECT]);
        TableCell osTableCell = new TableCell();

        TextBox otxtLecture = new TextBox();
        otxtLecture.Attributes.Add("onblur", "extractNumber(this,0,false);");
        otxtLecture.Attributes.Add("onkeyup", "extractNumber(this,0,false);");
        otxtLecture.Attributes.Add("onkeypress", "return blockNonNumbers(this, event, false, false);");
        otxtLecture.Attributes.Add("onpaste", "event.returnValue=false;");
        otxtLecture.Attributes.Add("ondrop", "event.returnValue=false;");
        otxtLecture.CssClass = "TxtBoxNOL";
        otxtLecture.Style.Add(HtmlTextWriterStyle.TextAlign, "center");

        //This method is used to set numeric box properties as well to create id.
        SetNumericBoxProperties(otxtLecture, aiRowIndex, aiSubjectIndex);

        osTableCell.Text = moDsLectures.Tables[I_TBL_SUBJECT].Rows[aiSubjectIndex][S_COL_SUB_ID].ToString();
        osTableCell.HorizontalAlign = HorizontalAlign.Center;

        iCellIndex = grdStandards.Rows[aiRowIndex].Cells.Add(osTableCell);
        osTableCell.Attributes.Add("title", Resources.LocalizedResources.Standard+" " + grdStandards.Rows[aiRowIndex].Cells[1].Text + " [" + grdStandards.HeaderRow.Cells[iCellIndex].Text + "]");
        int iSubjectId = Convert.ToInt32(grdStandards.Rows[aiRowIndex].Cells[iCellIndex].Text);

        object[] objStdSubjectCols = new object[2];
        objStdSubjectCols[0] = iSubjectId;
        objStdSubjectCols[1] = iStandardId;
        DataRow oDTStandardSubjectId = moDsLectures.Tables[I_TBL_STDSUBJECT].Rows.Find(objStdSubjectCols);
        grdStandards.Rows[aiRowIndex].Cells[iCellIndex].Controls.Add(hidStandardName);

        otxtLecture.Text = Constants.S_EMPTY_STRING;
        //This loop is used to set last updated value to control.
        if (oDTStandardSubjectId != null)
        {
            object[] objLectures = new object[1];
            objLectures[0] = oDTStandardSubjectId["Division_Subject_Id"];
            DataRow oDtLectures = moDsLectures.Tables[I_TBL_LECTURES].Rows.Find(objLectures);
            if (oDtLectures != null)
            {
                otxtLecture.Text = oDtLectures["Max_Lectures_Per_Standard_Subject"].ToString();
            }
            grdStandards.Rows[aiRowIndex].Cells[iCellIndex].Controls.Add(otxtLecture);
        }
        else
        {
            osTableCell.Text = Constants.S_EMPTY_STRING;
            grdStandards.Rows[aiRowIndex].Cells[iCellIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "#eaeaea");
                    
        }
        return iCellIndex;
    }

    /// <summary>
    /// This method is used to create hiddenfield id and to set value.
    /// </summary>
    /// <param name="aiRowIndex"></param>
    /// <param name="aiSubjectIndex"></param>
    /// <param name="aoDSAllSubjects"></param>
    private HiddenField GetHiddenFieldWithId(int aiRowIndex, int aiSubjectIndex, DataTable aDSAllSubjects)
    {
        HiddenField hidStandardName;
        hidStandardName = new HiddenField();
        hidStandardName.Value = grdStandards.Rows[aiRowIndex].Cells[1].Text + " ( " + aDSAllSubjects.Rows[aiSubjectIndex][3].ToString() + ")";
        hidStandardName.ID = "hid_0" + (aiRowIndex + 3) + "_" + aiSubjectIndex;
        return hidStandardName;
    }
    /// <summary>
    /// Set the primary key of the table.
    /// </summary>
    private void SetPrimaryKey()
    {
        DataColumn[] oDtCols = new DataColumn[2];
        oDtCols[0] = moDsLectures.Tables[I_TBL_STDSUBJECT].Columns[S_COL_SUB_ID];
        oDtCols[1] = moDsLectures.Tables[I_TBL_STDSUBJECT].Columns["standard_Id"];
        moDsLectures.Tables[I_TBL_STDSUBJECT].PrimaryKey = oDtCols;
        oDtCols = new DataColumn[1];
        oDtCols[0] = moDsLectures.Tables[I_TBL_LECTURES].Columns["Division_Subject_Id"];
        moDsLectures.Tables[I_TBL_LECTURES].PrimaryKey = oDtCols;
    }

	/// <summary>
	/// This method is used to check if lecture count is changed to lower than configured in time table.
	/// </summary>
	/// <returns></returns>
	private string CheckUpdatedCount()
	{
		object[] objStdSubjects = new object[2];
		object[] objLectures = new object[1];
		const Int32 I_NUMERIC_BOX_CONTROL_INDEX = 1;
		List<LecturesPerStandardSubjectWeekBL> lstDetails = new List<LecturesPerStandardSubjectWeekBL>();
        for (int iRowCount = 0; iRowCount < grdStandards.Rows.Count; iRowCount++)
		{
			StandardMasterBL oStandardMasterBL = new StandardMasterBL();
			Collection<LecturesPerStandardSubjectWeekBL> oLectCountCollection = new Collection<LecturesPerStandardSubjectWeekBL>();
			int iStandardId = Convert.ToInt32(grdStandards.Rows[iRowCount].Cells[I_STD_COL_INDEX].Text);
			LecturesPerStandardSubjectWeekBL oLectPerStdSubWeekBL = new LecturesPerStandardSubjectWeekBL();
			DataTable oDtAllSubjects = moDsLectures.Tables[I_TBL_SUBJECT];

			//This loop is for each subject of a particular standard.
			for (int iCellIndex = 2; iCellIndex <= oDtAllSubjects.Rows.Count + 1; iCellIndex++)
			{
				int iSubjectId = Convert.ToInt32(oDtAllSubjects.Rows[iCellIndex - 2][S_COL_SUB_ID].ToString());
				objStdSubjects[0] = iSubjectId;
				objStdSubjects[1] = iStandardId;
				DataRow oDRStandardSubjectId = moDsLectures.Tables[I_TBL_STDSUBJECT].Rows.Find(objStdSubjects);
				//If grid's cell contains textbox then go for adding or modifying values.
				if (grdStandards.Rows[iRowCount].Cells[iCellIndex+1].Controls.Count > 0 && oDRStandardSubjectId != null)
				{
					int iStandardSubjectId = Convert.ToInt32(oDRStandardSubjectId[S_COL_STDSUB_ID]);
					objLectures[0] = iStandardSubjectId;
					DataRow oDrLectures = moDsLectures.Tables[I_TBL_LECTURES].Rows.Find(objLectures);

					TextBox otxtLecCount = (TextBox)(grdStandards.Rows[iRowCount].Cells[iCellIndex+1].Controls[I_NUMERIC_BOX_CONTROL_INDEX]);
					int iLectCountId = 0;
					if (oDrLectures != null)
						iLectCountId = Convert.ToInt32(oDrLectures["Lectures_Per_Standard_Subject_Week_Id"].ToString());
                    
					oLectPerStdSubWeekBL = PopulateFields(Convert.ToInt32(otxtLecCount.Text), iStandardSubjectId, iLectCountId);
					lstDetails.Add(oLectPerStdSubWeekBL);
				}
			}
		}

		string sXml= CommonUtility.GenerateXml(lstDetails);		
		return LecturesPerStandardSubjectWeekBL.CheckValidUpdatedCount(miSchoolId,miAcademicYearId, sXml);
	}

    /// <summary>
    /// This Method used to change value of messgae according to culture
    /// </summary>
    private void RefreshValues()
    {
        hidValNoOfLectures.Value = Resources.LocalizedResources.ValNoOfLectures;
        hidPleaseFixFollowingError.Value = Resources.LocalizedResources.PleaseFixFollowingError;
        hidValLectureZero.Value = Resources.LocalizedResources.ValLectureZero;
        hidForStandard.Value = Resources.LocalizedResources.Standard;
        hidSubject.Value = Resources.LocalizedResources.Subject;
        FillStandardsGrid();
    }

    /// <summary>
    /// This is a common function used to set properties for all textboxes in grid.
    /// </summary>
    /// <param name="txtFeeType"></param>
    private void SetTextBoxProperties(TextBox txtFeeType)
    {
        txtFeeType.MaxLength = 2;
        txtFeeType.TextMode = TextBoxMode.SingleLine;
        txtFeeType.CssClass = "SmlTxtBox";
        txtFeeType.Width = Unit.Pixel(36);
        txtFeeType.Height = Unit.Pixel(19);
        txtFeeType.Attributes.Add("onkeyup", "extractNumber(this, 1 ,false);");
        txtFeeType.Attributes.Add("onkeypress", "return blockNonNumbers(this, event, false, false);");
        txtFeeType.Attributes.Add("onpaste", "event.returnValue=false;");
        txtFeeType.Attributes.Add("ondrop", "event.returnValue=false;");
        txtFeeType.Attributes.Add("onblur", "extractNumber(this,2,false);");
    }
    #endregion

}
