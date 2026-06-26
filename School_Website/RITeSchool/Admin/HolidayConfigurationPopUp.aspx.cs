// File Name    : HolidayConfigurationPopup.aspx.cs
// Created By   : Ketan
// Created Date : 29/11/2007
// Description  : This class is used save holiday details.  

using System;
using System.Web;
using System.Web.UI;
using System.Reflection;
using System.Collections.Generic;
using BusinessLogic.Exceptions;
using BusinessLogic;
using Utility;
using System.Data;
using System.Collections;
using SchoolEntities.Admin;
using System.Web.UI.WebControls;
using System.Linq;
using System.Text;
using System.Resources;

/// <summary>
/// This class is used to add and edit holiday management configuration.
/// </summary>
public partial class HolidayConfigurationPopup : SchoolBase
{
    #region Event

    List<ClasswiseAttendanceStatus> molstClasswiseAttendanceStatus = new List<ClasswiseAttendanceStatus>();

    ResourceManager oResourceManager = new ResourceManager(typeof(Resources.LocalizedResources));
    /// <summary>
    /// This event is used to decrypt query string and initialise page controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                if (CheckPreCondition())//check for week day configuration.
                {
                    
                    FillStandardChkLstBox();
                    SetHolidayInformationAccordingToMode();
                }
                txtStartDate.Focus();
                InitializePage();
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();

                }
                RefreshValue();
            }
            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save,update data 
    /// & transfer control to HolidaysManagementConfiguration page on Sucess
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
       
        Int32 iInsertOrUpdateFlag = 0;
        lblErrorMsg.Text = string.Empty;
        lblTotaldays.Text = hidTotalDays.Value;
        try
        {
            HolidaysMasterBL oHolidaysMasterBL = SetAllFieldToHolidayMaster();
            oHolidaysMasterBL.IsHolidayNameDuplicate();
            if (!chkConfirmOverLapping.Checked)
            {
                oHolidaysMasterBL.IsHolidayStartAndEndDatePredefined();
            }

              // To insert  data into Holidays_Master.            
                if (hidActionFlag.Value == Convert.ToString(Constants.I_ZERO))
                {
                    oHolidaysMasterBL.InsertHolidaysMaster();
                    iInsertOrUpdateFlag = 1;
                }
                else  //To Update data into Holidays_Master
                {
                    oHolidaysMasterBL.UpdateHolidaysMaster();
                    iInsertOrUpdateFlag = 1;
                }

                if (iInsertOrUpdateFlag == 1)
                {
                    if (hidIsConfig.Value != "Y")
                        SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.HolidaysManagement));
                    Response.Write("<Script language='Javascript'>window.opener.location.reload(true); window.close();window.opener.focus(); </Script>");
                }
                cEndDate.DateValue = Convert.ToDateTime(hidEndDate.Value);
            
           }
            catch (BusinessLogic.HolidaysMasterBL.DuplicateHolidayName ex)
        {
            ShowErrorMessage(oResourceManager.GetString(ex.Message.Replace(" ", string.Empty)));            
            cEndDate.DateValue = Convert.ToDateTime(hidEndDate.Value);
            lblHeader.Text=hidHolidayName.Value;
        }
        catch (BusinessLogic.HolidaysMasterBL.PerdefinedStartAndEndDate ex)
        {
            ShowErrorMessage(oResourceManager.GetString(ex.Message.Replace(" ", string.Empty)));
            cEndDate.DateValue = Convert.ToDateTime(hidEndDate.Value);
            lblHeader.Text=hidHolidayName.Value;
        }
        catch (BusinessLogic.HolidaysMasterBL.NonWorkingDay ex)
        {
            ShowErrorMessage(ex.Message);
            cEndDate.DateValue = Convert.ToDateTime(hidEndDate.Value);
            lblHeader.Text=hidHolidayName.Value;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }   

    protected void lstvwStandardDivisions_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
            int iRowId = oCurrentItem.DisplayIndex;            
            ClasswiseAttendanceStatus oClasswiseAttendanceStatus=oCurrentItem.DataItem as ClasswiseAttendanceStatus;  
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                CheckBox chkStandard = oCurrentItem.FindControl("chkStandard") as CheckBox;
                CheckBoxList chkStandardDivLst = oCurrentItem.FindControl("chkStandardDivLst") as CheckBoxList;
                int iStandardId = lstvwStandardDivisions.DataKeys[iRowId]["StandardId"].ToInt();

                var oDivision = molstClasswiseAttendanceStatus.Where(sd => sd.StandardId == iStandardId).Select(sd => new { DivisionName = sd.DivisionName, Id = sd.SchoolWiseStandardDivisionId });
                chkStandardDivLst.DataSource = oDivision;
                chkStandardDivLst.DataTextField = "DivisionName";
                chkStandardDivLst.DataValueField = "Id";
                chkStandardDivLst.DataBind();                
                chkStandard.Attributes.Add("onclick", "CheckAll(this,'" + iRowId+ "')");
                chkStandardDivLst.Attributes.Add("onclick", "CheckAllCheck('"+chkStandard+"','" + iRowId + "')"); 
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method

    private string GetSelectedStandardDivList()
    {
        StringBuilder oStandards = new StringBuilder();

        foreach (ListViewDataItem Item in lstvwStandardDivisions.Items)
        {
            CheckBoxList chkStandardDivLst = Item.FindControl("chkStandardDivLst") as CheckBoxList;
            for (int iCount = 0; iCount < chkStandardDivLst.Items.Count; iCount++)
            {
                if (chkStandardDivLst.Items[iCount].Selected)
                    oStandards.Append(chkStandardDivLst.Items[iCount].Value + ",");

            }
        }
        return oStandards.ToString();
    }

    /// <summary>
    /// This method is used to fill standard check box list.
    /// </summary>
    private void FillStandardChkLstBox()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtStandardCollection = oStandardCollectionBL.GetAssociatedStandards();
        AttendanceDetailsBL oAttendanceDetailsBL = new AttendanceDetailsBL();
        molstClasswiseAttendanceStatus = oAttendanceDetailsBL.Get(miSchoolId, miAcademicYearId, DateTime.Today.ToString("MM/dd/yyyy"));
        var oStandards = molstClasswiseAttendanceStatus.Select(sd => new { StandardName = sd.StandardName, StandardId = sd.StandardId}).Distinct();
        lstvwStandardDivisions.DataSource = oStandards;
        lstvwStandardDivisions.DataBind();        
    }
    
    /// <summary>
    /// This method is used to initialise page variables.
    /// </summary>
    private void InitializePage()
    {
        SetAcademicYearDates();
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        btnSave.Attributes.Add("onclick", "ClearErrorLabel()");
        btnCancel.Attributes.Add("onclick", "if(!(closewindow())){return false};");
        ApplyMouseHoverEffect(new List<System.Web.UI.WebControls.Button> { btnCancel, btnSave });        
    }

    /// <summary>
    /// This method used to initialises hidden fields with the start and end date of selected academic year.
    /// </summary>
    private void SetAcademicYearDates()
    {
        hidYearStartDate.Value = Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE].ToString();
        hidYearEndDate.Value = Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE].ToString();
    }

    /// <summary>
    /// This method is used to set all fields of HolidayMaster.
    /// </summary>
    private HolidaysMasterBL SetAllFieldToHolidayMaster()
    {
        string sStandardDiv = GetSelectedStandardDivList();
        HolidaysMasterBL oHolidaysMasterBL = new HolidaysMasterBL();
        oHolidaysMasterBL.SchoolId = miSchoolId;
        oHolidaysMasterBL.AcademicYearId = miAcademicYearId;
        oHolidaysMasterBL.HolidayId = Convert.ToInt32(hidHolidayId.Value);
        oHolidaysMasterBL.HolidayStartDate = Convert.ToDateTime(cStartDate.DateValue);
        oHolidaysMasterBL.HolidayEndDate = Convert.ToDateTime(hidEndDate.Value);
        oHolidaysMasterBL.AssoiciatedStandards = sStandardDiv;
        oHolidaysMasterBL.HolidayName = txtNameofHoliday.Text;
        oHolidaysMasterBL.Remarks = txtRemarks.Text;
        oHolidaysMasterBL.AllowOverLapping = chkConfirmOverLapping.Checked;
        oHolidaysMasterBL.InsertedById = miUserId;
        return oHolidaysMasterBL;
    }

    /// <summary>
    /// This method is used to show error message in catch block.
    /// </summary>
    /// <param name="asMessage"></param>
    private void ShowErrorMessage(string asMessage)
    {
        lblErrorMsg.Visible = true;
        lblErrorMsg.Text = asMessage;
    }

    /// <summary>
    /// This method is used to decrypt querystring.
    /// </summary>
    private void ReadQuerystring()
    {
	    if (Request.QueryString.ToString() == Constants.S_EMPTY_STRING)
		    return;
	    
		if (QueryString["HolidayId"] != null)
		    hidHolidayId.Value = QueryString["HolidayId"];
	    
		hidIsConfig.Value = QueryString["Is_Configured"];
    }

    /// <summary>
    /// This method  retrives the data for selected holiday.
    /// And sets the form fields accordingly.
    /// </summary>
    private void FillHolidayData()
    {
        Int32 iHolidayID = Convert.ToInt32(hidHolidayId.Value);
       
        HolidaysMasterBL oHolidaysMasterBL = new HolidaysMasterBL(iHolidayID,miSchoolId,miAcademicYearId);
        cStartDate.DateValue = oHolidaysMasterBL.HolidayStartDate;
        cEndDate.DateValue = oHolidaysMasterBL.HolidayEndDate;
        txtNameofHoliday.Text = oHolidaysMasterBL.HolidayName;
        TimeSpan oT = oHolidaysMasterBL.HolidayEndDate.Subtract(oHolidaysMasterBL.HolidayStartDate);
        lblTotaldays.Text = (oT.Days + 1).ToString();
        txtRemarks.Text = oHolidaysMasterBL.Remarks;
       chkConfirmOverLapping.Checked = oHolidaysMasterBL.AllowOverLapping;      
        string sStandards=string.Empty;
       

        if (!oHolidaysMasterBL.AssoiciatedStandards.IsNullOrEmpty())
        {
            sStandards = oHolidaysMasterBL.AssoiciatedStandards.ToString();
            string[] sArrStandards = sStandards.Split(',');            
            
            foreach (ListViewDataItem Item in lstvwStandardDivisions.Items)
            {
                CheckBoxList chkStandardDivLst = Item.FindControl("chkStandardDivLst") as CheckBoxList;
                CheckBox chkStandard = Item.FindControl("chkStandard") as CheckBox;
                int iTotal = 0;
                for (int iStandardIndex = 0; iStandardIndex < sArrStandards.Length; iStandardIndex++)
                {
                    string sStandardId = sArrStandards[iStandardIndex].ToString();
                    if (chkStandardDivLst.Items.FindByValue(sStandardId)!=null)
                        chkStandardDivLst.Items.FindByValue(sStandardId).Selected = true;                    
                }

                for (int iStandards = 0; iStandards < chkStandardDivLst.Items.Count; iStandards++)
                {
                    if (chkStandardDivLst.Items[iStandards].Selected)
                    {
                        iTotal++;
                        if (iTotal == chkStandardDivLst.Items.Count)
                            chkStandard.Checked = true;
                    }
                }
            }            
        }       
       
    }

    /// <summary>
    /// This method checks the preconditons for Holiday configuration Pop up UI.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.HolidaysManagement);
        if (sLinks.Equals(""))
        {
            divErr.Visible = false;
            bReturn = true;
        }
        else
        {
            divErr.InnerHtml = sLinks;
            //pnlFields.Visible = false;
        }

        return bReturn;
    }

    /// <summary>
    /// This method decides the display mode (add or edit).
    /// and sets the form fields accordingly.
    /// </summary>
    private void SetHolidayInformationAccordingToMode()
    {
        ReadQuerystring();
        if (hidHolidayId.Value !="")
        {
            FillHolidayData();
            hidActionFlag.Value = Convert.ToString(Constants.I_ONE);
            lblHeader.Text = Resources.LocalizedResources.EditHoliday;
            hidHolidayName.Value = Resources.LocalizedResources.EditHoliday;
        }
        else
        {
            lblHeader.Text = Resources.LocalizedResources.AddHoliday;
            hidHolidayName.Value = Resources.LocalizedResources.AddHoliday;
            
            SetNewModeHolidayInformation();
        }
    }

    /// <summary>
    /// This method sets the form fields for new mode.
    /// It sets 
    /// 1. default start and end date (as current date).
    /// 2. default value for total days = 1.
    /// 3. and hidden variable for mode to zero.
    /// </summary>
    private void SetNewModeHolidayInformation()
    {
        cStartDate.DateValue = DateTime.Now;
        cEndDate.DateValue = DateTime.Now;
        lblTotaldays.Text = "1";
        hidHolidayId.Value = Convert.ToString(Constants.I_ZERO);
        hidActionFlag.Value = Convert.ToString(Constants.I_ZERO);
        CheckAll();
    }

    private void CheckAll()
    {
        chkAll.Checked = true;        
        foreach (ListViewDataItem Item in lstvwStandardDivisions.Items)
        {
            
            CheckBoxList chkStandardDivLst = Item.FindControl("chkStandardDivLst") as CheckBoxList;
            CheckBox chkStandard = Item.FindControl("chkStandard") as CheckBox;
            chkStandard.Checked = true;
            for (int iStandardIndex = 0; iStandardIndex < chkStandardDivLst.Items.Count; iStandardIndex++)                
                chkStandardDivLst.Items[iStandardIndex].Selected = true;            
        }
    }

    private void RefreshValue()
    {
        hidValStartEndDate.Value = Resources.LocalizedResources.ValStartEndDate;
        hidvalHolidayStartDate.Value = Resources.LocalizedResources.valHolidayStartDate;
        hidvalHolidayEndDate.Value = Resources.LocalizedResources.valHolidayEndDate;
        hidand.Value = Resources.LocalizedResources.And;
        hidbetween.Value = Resources.LocalizedResources.between;
        hidHolidayBetween.Value = Resources.LocalizedResources.HolidayBetween;
    }

    #endregion
}

