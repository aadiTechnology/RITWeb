/* Creater : Shankar Gurav
 * Created date: 2 July 2008
 * Last updated date: 3 July 2008
 * Purpose : This page class is gives the new academic generation wizard to the user.
 */

using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using BusinessLogic;
using Utility;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using BusinessLogic.Exceptions;
using System.Reflection;
using System.Configuration;
using System.Resources;

public partial class Admin_StartNextAcademic : SchoolBase
{
    #region Members

    ResourceManager oResourceManager = new ResourceManager(typeof(Resources.LocalizedResources));

    #endregion

    #region Events

    const int itblMidDetailsIndex = 2;
    string S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR = "N";

	protected override void OnPreInit(EventArgs e)
	{
		try
		{
			base.OnPreInit(e);
			if (Settings.IsMiniSite)
				Page.MasterPageFile = "~/RITeSchool/MasterPages/MasterPage.master";
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

    /// <summary>
    /// This method is used to hadle page load event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {           
            CheckPrecondition();
            SetJavascriptAttributes();            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to hadle wizard load event and handle the page load and initalize the wizard
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void wizNextAcaGen_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ShowDefaultAcdemicDates();
            }
            else
            {
                if (wizNextAcaGen.ActiveStep == AcaGenStep2)
                {
                    Button oButton = (Button)wizNextAcaGen.WizardSteps[2].FindControl("FinishNavigationTemplateContainerID").FindControl("FinishButton");
                    oButton.Attributes.Add("Onclick", "if(!(ConfirmAction('" + grdConfiguration.AllowPaging + "','" + Resources.LocalizedResources.PleaseSelectAtLeastOneConfigurationToCopy + "'))){return false;}");

                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
        }
    }

    /// <summary>
    /// This method is used to do transaction at final stage.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void wizNextAcaGen_FinishButtonClick(object sender, WizardNavigationEventArgs e)
    {
        try
        {
            TranserferDataToNewYear();
            wizNextAcaGen.ActiveStepIndex = 5;
            using (var swFile = new StreamWriter(Server.MapPath(@"~\Cache.txt"), true))
            {
                swFile.WriteLine("\n" + DateTime.Now);
                swFile.Flush();
                swFile.Close();
            }
			
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
            wizNextAcaGen.ActiveStepIndex = 0;
        }
    }
    /// <summary>
    /// This event is used to set javascript attributes to wizards step buttons.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void wizNextAcaGen_ActiveStepChanged(object sender, EventArgs e)
    {
        try
        {
            if (wizNextAcaGen.ActiveStep == AcaGenStep0 && wizNextAcaGen.FindControl("StartNavigationTemplateContainerID") != null)
                SetJavascriptAttributes();
            else if (wizNextAcaGen.ActiveStep != AcaGenStep0)
            {
                Button oBtnPrevious = (Button)wizNextAcaGen.FindControl("StepNavigationTemplateContainerID").FindControl("StepPreviousButton");
                ApplyMouseHoverEffect(new List<Button> { oBtnPrevious });
                Button oCancleButton = (Button)wizNextAcaGen.FindControl("StepNavigationTemplateContainerID").FindControl("CancelButton");
                if (oCancleButton != null)
                {
                    ApplyMouseHoverEffect(new List<Button> { oCancleButton });
                }

                Button oNextButton = (Button)wizNextAcaGen.FindControl("StepNavigationTemplateContainerID").FindControl("StepNextButton");
                if (oNextButton != null)
                {
                    ApplyMouseHoverEffect(new List<Button> { oNextButton });
                }

                Button oBtnFinish = (Button)wizNextAcaGen.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishButton");
                //oBtnFinish.Attributes.Add("onclick", "javascript:disableControls();");
                ApplyMouseHoverEffect(new List<Button> { oBtnFinish });
                //oBtnFinish.Attributes.Add("onclick", "if(!DisplayConfirmation()) return false;");

                Button oFinishCancelButton = (Button)wizNextAcaGen.FindControl("FinishNavigationTemplateContainerID").FindControl("CancelButton");
                if (oFinishCancelButton != null)
                {
                    ApplyMouseHoverEffect(new List<Button> { oFinishCancelButton });
                }

                Button oFinishPreviousButton = (Button)wizNextAcaGen.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishPreviousButton");
                if (oFinishPreviousButton != null)
                {
                    ApplyMouseHoverEffect(new List<Button> { oFinishPreviousButton });
                }

                if (rdoFinalAcademic.Checked && hidIsMidCreated.Value == Constants.S_ZERO)
                {
                    wizNextAcaGen.ActiveStepIndex = 0;
                    lblMidAcademicYr.Visible = true;
                    lblMidAcademicYr.Text = Resources.LocalizedResources.PleaseGenerateAMidAcademicYearBeforeProceedingToAFinalAcademicYearGeneration;
                    Button oBtnNext = (Button)wizNextAcaGen.WizardSteps[0].FindControl("StartNavigationTemplateContainerID").FindControl("FinishNextButton");
                    oBtnNext.Enabled = true;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to handal next button navigation.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void wizNextAcaGen_NextButtonClick(object sender, WizardNavigationEventArgs e)
    {
        try
        {
            if (wizNextAcaGen.ActiveStep == AcaGenStep0)
            {
                ShowNextConfiguredAcademicYear();                
                ShowConfigurationList();                
            }
            else if (wizNextAcaGen.ActiveStep == AcaGenStep1)
            {
                if (rdoFinalAcademic.Checked)
                {
                    wizNextAcaGen.ActiveStepIndex = 3;
                    AcaGenStep3.StepType = WizardStepType.Step;
                }
            }
            else if (wizNextAcaGen.ActiveStep == AcaGenStep2)
            {
                if (cStartDate.DateValue <= Convert.ToDateTime(Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE]))
                {
                    throw new ApplicationException(Resources.LocalizedResources.AcademicYearMustBeGreaterThanCurrentAcademicYear);
                }
                SchoolWiseAcademicYearMasterBL oSchoolWiseAcademicYearMasterBL = SetAcademicYearMasterBL();
                oSchoolWiseAcademicYearMasterBL.IsAcademicYrStartAndEndDtPredefined();
                AcaGenStep2.StepType = WizardStepType.Step;
                if (rdoMidAcademic.Checked)
                    AcaGenStep3.StepType = WizardStepType.Finish;
            }
            else if (wizNextAcaGen.ActiveStep == AcaGenStep3)
            {
                if (!Settings.EnableTransportModule)
                {
                    lblTransportData.Visible = false;
                    chkTransportData.Visible = false;
                }
            }
        }
        catch (BusinessLogic.Exceptions.NonWorkingDay ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
            e.Cancel = true;
        }
        catch (BusinessLogic.HolidaysMasterBL.PerdefinedStartAndEndDate ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = oResourceManager.GetString(ex.Message.Replace(" ", string.Empty));
            e.Cancel = true;
        }
        catch (ApplicationException ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
            e.Cancel = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
            e.Cancel = true;
        }
    }

    /// <summary>
    /// This button is used to navigate to contol page on cancel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void wizNextAcaGen_CancelButtonClick(object sender, EventArgs e)
    {
		try
		{
			if (Settings.IsMiniSite)
			{
				MasterPage oAdminMasterPage = (MasterPage)this.Master;
				wizNextAcaGen.CancelDestinationPageUrl = Constants.S_PAGE_CONTROL_PANEL;
				oAdminMasterPage.Response.Redirect(Constants.S_PAGE_CONTROL_PANEL);
			}
			else
			{
				SuperAdminMasterPage oSuperAdminMasterPage = (SuperAdminMasterPage)this.Master;
				oSuperAdminMasterPage.RedirectToNextPage("~/SuperAdmin/ScreensUI.aspx");
			}
		}
		catch (ThreadAbortException)
		{ }
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
    }

    /// <summary>
    /// This method is used to handal prev button navigation.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void wizNextAcaGen_PreviousButtonClick(object sender, WizardNavigationEventArgs e)
    {
        try
        {
            Button oBtnNext = (Button)wizNextAcaGen.WizardSteps[0].FindControl("StartNavigationTemplateContainerID").FindControl("FinishNextButton");
            oBtnNext.Enabled = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void rdoMidAcademic_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            Button oBtnNext = (Button)wizNextAcaGen.WizardSteps[0].FindControl("StartNavigationTemplateContainerID").FindControl("FinishNextButton");
            if (rdoFinalAcademic.Checked)
            {
                tblChkList.Visible = true;
                int icount = FillConfigurationCheckBoxList();
                oBtnNext.Enabled = false;
                chkListConfiguration.Attributes.Add("onclick", "NextEnable(" + icount + " , '" + oBtnNext.ClientID + "')");
            }
            else
            {
                tblChkList.Visible = false;
                oBtnNext.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    #endregion Events

    #region Private methods

    /// <summary>
    /// this method is used to check precondition that is all result is generated or not.
    /// </summary>
    private void CheckPrecondition()
    {
        try
        {
            if ((Session[Constants.S_SESSION_ACADEMIC_YEAR_IS_NEWLYCREATED] != null) &&
           (Convert.ToChar(Session[Constants.S_SESSION_ACADEMIC_YEAR_IS_NEWLYCREATED]) == Constants.C_YES))
            {
                string sMessage = Convert.ToString(miAcademicYearId);
                throw new ApplicationException(sMessage + Resources.LocalizedResources.ToCreateNewAcademicYearPleaseSelectCurrentAcademicFromDashboard);
            }
        }
        catch (Exception ex)
        {
            lblErrorMsg.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
            lblErrorMsg.Style.Add(HtmlTextWriterStyle.BorderStyle, "None");
            lblErrorMsg.CssClass = "LblNoRecord";
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
            wizNextAcaGen.Visible = false;
            btnBack.Visible = true;
        }
    }

    /// <summary>
    /// This method is used to show all configuration list which r going to copied to next academic year.
    /// </summary>
    private void ShowConfigurationList()
    {
        int iAcademicYearId = 0;
        if (rdoMidAcademic.Checked)
            iAcademicYearId = miAcademicYearId;
        else
            iAcademicYearId = Convert.ToInt32(hidNextAcademiYearId.Value);
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        DataSet oDSConfigurationDetails = oMasterDataCollectionBL.GetAllConfigurationsForAcademicData(miSchoolId, iAcademicYearId, rdoMidAcademic.Checked);
        grdConfiguration.DataSource = oDSConfigurationDetails.Tables[0].DefaultView;
        grdConfiguration.DataBind();
        AssignDependancy();
    }

    /// <summary>
    /// This function is used to show all configuration list which r need to configured before Final Academic year generation.
    /// </summary>

    private int FillConfigurationCheckBoxList()
    {
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        DataSet oDSConfigurationDetails = oMasterDataCollectionBL.GetConfigurationsForFinalAcademicYearGeneration();
        chkListConfiguration.DataSource = oDSConfigurationDetails;
        chkListConfiguration.DataTextField = "Configure_Name";
        chkListConfiguration.DataValueField = "Configure_Id";
        chkListConfiguration.DataBind();
        int iCount = chkListConfiguration.Items.Count;
        return iCount;
    }


    /// <summary>
    /// This function is used to assign dependancy for check box
    /// </summary>
    private void AssignDependancy()
    {
        foreach (GridViewRow oGridRow in grdConfiguration.Rows)
        {
            string sDependantConfIds = grdConfiguration.DataKeys[oGridRow.RowIndex].Values[1].ToString();
            Control oCntrl = oGridRow.FindControl("chkSelectConf");
            char[] arrCh = { '_' };
            StringBuilder sDependantChkIds = new StringBuilder(string.Empty);
            //Take the "-" saparated dependant ids and check that is  any checkbox present with this id and 
            //prepare the id list to check on client click event.
            string[] sDepConfIds = sDependantConfIds.Split(arrCh);
            if (oCntrl != null)
            {
                CheckBox oCheckBox = (CheckBox)oCntrl;
                LockIfCompulsary(oCheckBox, grdConfiguration.DataKeys[oGridRow.RowIndex].Values[0].ToString(), oGridRow.RowIndex);
                foreach (GridViewRow oGridViewRow in grdConfiguration.Rows)
                {
                    string str = grdConfiguration.DataKeys[oGridViewRow.RowIndex].Value.ToString();
                    for (int i = 0; i < sDepConfIds.Length; i++)
                    {
                        if (str == sDepConfIds[i])
                        {
                            oCntrl = oGridViewRow.FindControl("chkSelectConf");
                            Control oHidCntrl = oGridViewRow.FindControl("hidRefCount");
                            if (oCntrl != null)
                            {
                                //add the client id of checkbox to event string argument
                                CheckBox odependCheckBox = (CheckBox)oCntrl;
                                sDependantChkIds.Append(odependCheckBox.ClientID.Replace(grdConfiguration.ClientID, string.Empty) + "|");
                            }
                        }
                    }
                }
                oCheckBox.Attributes.Add("onclick", "Check(this,'" + sDependantChkIds.ToString() + "')");
            }
        }
    }

    /// <summary>
    /// This mathod is used to lock copulsory config ids.
    /// </summary>
    /// <param name="oCheckBox"></param>
    /// <param name="ConfId"></param>
    private void LockIfCompulsary(CheckBox oCheckBox, string ConfId, int iRowId)
    {
        ///Thease are the seed config data and considered as default and need to lock
        string[] sLockedConfigArrays = { Convert.ToInt32(Constants.SchoolConfigurations.Standard).ToString()
                                           , Convert.ToInt32(Constants.SchoolConfigurations.Division).ToString()
                                           , Convert.ToInt32(Constants.SchoolConfigurations.StandardwiseDivision).ToString()
                                           , Convert.ToInt32(Constants.SchoolConfigurations.Subjects).ToString()
                                           , Convert.ToInt32(Constants.SchoolConfigurations.SubjectGrade).ToString()
                                           , Convert.ToInt32(Constants.SchoolConfigurations.SubjectGrade).ToString()
                                           , Convert.ToInt32(Constants.SchoolConfigurations.StandardwiseSubjects).ToString()
                                           , Convert.ToInt32(Constants.SchoolConfigurations.DivisionwiseSubjects).ToString()
                                           , Convert.ToInt32(Constants.SchoolConfigurations.LateFeeSettings).ToString()};

        for (int i = 0; i < sLockedConfigArrays.Length; i++)
        {
            if (rdoFinalAcademic.Checked)
            {
                if (ConfId == sLockedConfigArrays[i])
                {
                    oCheckBox.Checked = true;
                    oCheckBox.Enabled = false;
                    ((HtmlInputHidden)grdConfiguration.Rows[iRowId].FindControl("hidRefCount")).Value = "1000";
                }
            }
        }
    }

    /// <summary>
    /// This method is used to get next configured year from database and display to the user.
    /// </summary>
    private void ShowNextConfiguredAcademicYear()
    {

        SchoolWiseAcademicYearMasterBL oSchoolWiseAcademicYearMasterBL = new SchoolWiseAcademicYearMasterBL();
		S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR = Settings.ShowAdmissionForCurrentYear ? Constants.S_YES : Constants.S_NO;
        DataSet oDSNextAcdemic = oSchoolWiseAcademicYearMasterBL.GetNextConfiguredAcademicYear(miSchoolId, S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR);
        if (oDSNextAcdemic.Tables[itblMidDetailsIndex].Rows[0]["IsMidCreated"] != DBNull.Value)
            hidIsMidCreated.Value = oDSNextAcdemic.Tables[2].Rows[0]["IsMidCreated"].ToString();
        if (oDSNextAcdemic != null && oDSNextAcdemic.Tables[0].Rows.Count > 0)
        {
            if (oDSNextAcdemic.Tables[0].Rows.Count > 0)
            {
                
                //If new year alredy defined
                if (oDSNextAcdemic.Tables[0].Rows[0]["Start_date"] != DBNull.Value)
                    hidNextYearEndDt.Value = oDSNextAcdemic.Tables[0].Rows[0]["Start_date"].ToString();
                if (oDSNextAcdemic.Tables[0].Rows[0]["End_Date"] != DBNull.Value)
                    hidNextYearStartDt.Value = oDSNextAcdemic.Tables[0].Rows[0]["End_Date"].ToString();
                if (oDSNextAcdemic.Tables[0].Rows[0]["Academic_Year_Id"] != DBNull.Value)
                    hidNextAcademiYearId.Value = oDSNextAcdemic.Tables[0].Rows[0]["Academic_Year_Id"].ToString();
                else
                    hidNextAcademiYearId.Value = "0";
                if (oDSNextAcdemic.Tables[0].Rows[0]["Academic_date"] != DBNull.Value)
                    lblNextAcademicYearVal.Text = oDSNextAcdemic.Tables[0].Rows[0]["Academic_date"].ToString();
                if (oDSNextAcdemic.Tables[0].Rows[0]["Is_FinalYear_Generated"] != DBNull.Value)
                    hidFinalYearGenerated.Value = oDSNextAcdemic.Tables[0].Rows[0]["Is_FinalYear_Generated"].ToString();
                Button oBtnNext = (Button)wizNextAcaGen.FindControl("StepNavigationTemplateContainerID").FindControl("StepNextButton");
                chkDeleteYear.Attributes.Add("onclick", "Enable(this,'" + oBtnNext.ClientID + "')");
            }
            if (oDSNextAcdemic.Tables[1].Rows.Count > 0)
            {
                if (rdoFinalAcademic.Checked && hidFinalYearGenerated.Value == Constants.C_NO.ToString())
                {
                    grdAlreadyConfigured.DataSource = oDSNextAcdemic.Tables[1];
                    grdAlreadyConfigured.DataBind();
                    trgrdAlreadyConfigured.Visible = true;
                    trChkDeleteYear.Visible = false;
                    lblNote.Text = Resources.LocalizedResources.TheExistingNewMidYearConfiguredIs;
                    lblAcademicYearNote.Text =  Resources.LocalizedResources.ThefollowingConfigurationIsAlreadyConfiguredForThisMidYear;
                    chkDeleteYear.Text = Resources.LocalizedResources.DeleteExistingNewAcademicYear;
                }
                else if (rdoFinalAcademic.Checked && hidFinalYearGenerated.Value == Constants.C_YES.ToString())
                {
                    lblNote.Text = Resources.LocalizedResources.TheExistingNewFinalYearConfiguredIs;
                    lblAcademicYearNote.Text = Resources.LocalizedResources.DeleteTheExistingOneBeforeCreatingAnotherNewFinalAcademicYear;
                    chkDeleteYear.Text = Resources.LocalizedResources.DeleteExistingNewFinalAcademicYear;
                }
                else
                {
                    trgrdAlreadyConfigured.Visible = false;
                    trChkDeleteYear.Visible = true;
                    lblNote.Text = Resources.LocalizedResources.TheExistingNewAcademicYearIs;
                    lblAcademicYearNote.Text = Resources.LocalizedResources.DeleteTheExistingOneBeforeCcreatingAnotherNewAcademicYear;
                    chkDeleteYear.Text = Resources.LocalizedResources.DeleteExistingNewAcademicYear;
                }
            }
        }
        else
        {
                wizNextAcaGen.ActiveStepIndex = 2;
                hidNextAcademiYearId.Value = "0";
        }
       
        
    }

    /// <summary>
    /// This method is used to show the acdemic year default dates 
    /// </summary>
    private void ShowDefaultAcdemicDates()
    {
        DateTime dtCurrEndDate = Convert.ToDateTime(Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE]);
        cStartDate.DateValue = dtCurrEndDate.AddDays(1);
        cEndDate.DateValue = dtCurrEndDate.AddYears(1);
    }

    /// <summary>
    /// This method is used to transfer the data to next newly created year.
    /// </summary>
    private void TranserferDataToNewYear()
    {
        Boolean bGenerateRollNos = false;
        Boolean bGenerateRegNos = false;
        Boolean bGenerateDebitEntries = false;
        Boolean bGenerateTransportData = false;
        DataTable oDataTable = new DataTable("ConfiIds");
        // Add three column objects to the table.
        DataColumn idColumn = new DataColumn();
        idColumn.DataType = System.Type.GetType("System.Int32");
        idColumn.ColumnName = "id";
        idColumn.AutoIncrement = false;
        oDataTable.Columns.Add(idColumn);
        StringBuilder sConfIds = new StringBuilder(string.Empty);
        //add Ids to the datatable by eterating gridview checkboxes if they are checked
        foreach (GridViewRow oGridViewRow in grdConfiguration.Rows)
        {
            if (oGridViewRow.RowType != DataControlRowType.Header)
            {
                Control oCntrl = oGridViewRow.FindControl("chkSelectConf");
                if (oCntrl != null)
                {
                    CheckBox oCheckBox = (CheckBox)oCntrl;
                    if (oCheckBox.Checked)
                        oDataTable.Rows.Add(grdConfiguration.DataKeys[oGridViewRow.RowIndex].Values[0]);
                }
            }
        }
        //convert table to xml and pass to function
        StringWriter oStringWriter = new StringWriter();
        oDataTable.WriteXml(oStringWriter);
        if (chkGenRollNos.Checked && rdoFinalAcademic.Checked)
            bGenerateRollNos = true;
        if (chkGenDebitEntries.Checked && rdoFinalAcademic.Checked)
            bGenerateDebitEntries = true;
        if (chkTransportData.Checked && rdoFinalAcademic.Checked)
            bGenerateTransportData = true;
        SchoolWiseAcademicYearMasterBL oSchoolWiseAcademicYearMasterBL = new SchoolWiseAcademicYearMasterBL(miSchoolId, miAcademicYearId);
        oSchoolWiseAcademicYearMasterBL.GenerateNextYearData(oStringWriter.ToString(), cStartDate.DateValue, cEndDate.DateValue, bGenerateRollNos, bGenerateRegNos, bGenerateDebitEntries,bGenerateTransportData, rdoMidAcademic.Checked);
    }

    /// <summary>
    /// This method is used to insert schoolwise academic year data.
    /// </summary>
    private Int32 AddNewAcademic()
    {
        Int32 iNewAcademicYearId = 0;
        try
        {
            SchoolWiseAcademicYearMasterBL oSchoolWiseAcademicYearMasterBL = new SchoolWiseAcademicYearMasterBL();
            oSchoolWiseAcademicYearMasterBL = SetAcademicYearMasterBL();
            iNewAcademicYearId = oSchoolWiseAcademicYearMasterBL.InsertSchoolWiseAcademicYearMaster();
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
        return iNewAcademicYearId;
    }

    /// <summary>
    /// This method is used to insert schoolwise academic year data.
    /// </summary>
    private SchoolWiseAcademicYearMasterBL SetAcademicYearMasterBL()
    {
        SchoolWiseAcademicYearMasterBL oSchoolWiseAcademicYearMasterBL = new SchoolWiseAcademicYearMasterBL();
        oSchoolWiseAcademicYearMasterBL.SchoolWiseAcademicYearId = Convert.ToInt32(hidNextAcademiYearId.Value);
        oSchoolWiseAcademicYearMasterBL.StartDate = cStartDate.DateValue;
        oSchoolWiseAcademicYearMasterBL.EndDate = cEndDate.DateValue;
        oSchoolWiseAcademicYearMasterBL.SchoolId = miSchoolId;
        oSchoolWiseAcademicYearMasterBL.IsCloseYear = Constants.C_NO.ToString();
        oSchoolWiseAcademicYearMasterBL.IsCurrentYear = Constants.C_NO.ToString();
        oSchoolWiseAcademicYearMasterBL.Is_NewlyCreated = Constants.C_YES.ToString();
        return oSchoolWiseAcademicYearMasterBL;
    }


    private void SetJavascriptAttributes()
    {
        Button oButton = (Button)wizNextAcaGen.FindControl("StartNavigationTemplateContainerID").FindControl("CancelButton");

        if (oButton != null)
        {
            ApplyMouseHoverEffect(new List<Button> { oButton });
        }
        oButton = (Button)wizNextAcaGen.FindControl("StartNavigationTemplateContainerID").FindControl("FinishNextButton");
        if (oButton != null)
        {
            ApplyMouseHoverEffect(new List<Button> { oButton });
        }
        ApplyMouseHoverEffect(new List<Button> { btnBack, btnOK });

        valSumErrorMsg.HeaderText =Resources.LocalizedResources.PleaseFixFollowingError;
    }
    #endregion Private methods
}
