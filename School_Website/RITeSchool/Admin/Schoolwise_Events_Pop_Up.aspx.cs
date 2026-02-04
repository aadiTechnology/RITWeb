/* File Name :- Schoolwise_Events_Pop_Up.aspx.cs
 * Modified By :- Sachin
 * Modified Date :- 21-Sept-2009
 * Purpose :- Code Review.
 * Class Description :- This class is used to manipulate event details.
*/
using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Linq;
using System.Reflection;
using BusinessLogic.Exceptions;
using System.Web.UI.WebControls;
using BusinessLogic;
using Utility;
using System.Collections.Generic;
using System.Globalization;
using MasterEntities;

public partial class AddEventPopup : SchoolBase
{
    #region Constants

    private const string S_FOLDER_LOCATION = "RITeSchool\\DOWNLOADS\\Event Planner\\";
    private const string S_FOLDER_PATH = @"../DOWNLOADS/Event Planner/";
    private const string S_FILE_NOT_FOUND = "File does not exists.";
    private const int I_FILE_SIZE_LIMIT = 1048576;
    private const string S_FILE_SIZE_ERROR = "Size of file is too large.";

    #endregion

    #region Data Members

    DateTime mdtEventDate;
    int miStandardId;
    int miDivisionId;
    List<StandardDivisionMaster> molstStandardDivisionMaster = new List<StandardDivisionMaster>();

    #endregion

    #region Events

    /// <summary>
    /// This event is used to fill list of events.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            GetQueryString();    
            if (!IsPostBack)
            {
                txtEventDesc.Focus();
                SetAcademicYearDates();
                FillEventListBox();
                FillStandardChkLstBox();
                InitializeControls();
                SetJavascriptAttributes();
                RefreshValues();
            }            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
   
    /// <summary>
    /// This method is used to add or edit events.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {   
            ArrayList arrLstStandard = new ArrayList();
            arrLstStandard = GetStandardArrLst();
            
            string sLinkName;
            string sFileUploadErr = CheckIsFileFileUploaded(out sLinkName);

            string sLinkFamilyName;
            string sFileUploadError = CheckIsFamilyPhotoUploaded(out sLinkFamilyName);


            if (sFileUploadErr == string.Empty || sFileUploadError == string.Empty)
            {
                SchoolEventBL oEventDescriptionBL = PopulateEventBL(sLinkName);

                if (hidIsNewRecord.Value != "true")
                    oEventDescriptionBL.Event_Id = Convert.ToInt32(hidEventID.Value);
                if (!oEventDescriptionBL.IsEventNameDuplicate())
                {
                    if (hidIsNewRecord.Value == "true")
                        oEventDescriptionBL.InsertEventDescription(arrLstStandard);
                    else
                        oEventDescriptionBL.UpdateEventDescription(arrLstStandard);
                    SetQueryString();
                }
                else
                    lblErrorMsg.Text = Resources.LocalizedResources.EventNameAlreadyExists;
            }
            else
                lblErrorMsg.Text = sFileUploadErr;
                lblErrorMsg.Text = sFileUploadError;
           
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }    

    /// <summary>
    /// This event fills particular event data.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstEvents_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hidIsNewRecord.Value = "false";
            BtnDelete.Visible = true;
            hidEventID.Value = lstEvents.SelectedValue;
            FillEventData();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to add new event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnNew_Click(object sender, EventArgs e)
    {
        try
        {
            ResetControls();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to delete particular eventt.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            SchoolEventBL oEventDescriptionBL = new SchoolEventBL();
            oEventDescriptionBL.Event_Id = Convert.ToInt32(hidEventID.Value);
            oEventDescriptionBL.DeleteSchoolwiseEventDescription();
            FillEventListBox();
            if (lstEvents.Items.Count > 0)
            {
                hidEventID.Value = lstEvents.SelectedValue;
                FillEventData();
            }
            else
                ResetControls();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to delete event Image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgbtnDelete_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            SchoolEventBL oEventDescriptionBL = new SchoolEventBL(miSchoolId,miUserId,miAcademicYearId);
            oEventDescriptionBL.Event_Id = Convert.ToInt32(hidEventID.Value);
            oEventDescriptionBL.DeleteEventImage();
            hidEventImage.Value = null;
            FillEventData();
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
            StandardDivisionMaster oStandardDivisionMaster = oCurrentItem.DataItem as StandardDivisionMaster;
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                CheckBox chkStandard = oCurrentItem.FindControl("chkStandard") as CheckBox;
                CheckBoxList chkStandardDivLst = oCurrentItem.FindControl("chkStandardDivLst") as CheckBoxList;
                int iStandardId = lstvwStandardDivisions.DataKeys[iRowId]["StandardId"].ToInt();

                var oDivision = molstStandardDivisionMaster.Where(sd => sd.StandardId == iStandardId).Select(sd => new { DivisionName = sd.DivisionName, Id = sd.StandardDivisionId });
                chkStandardDivLst.DataSource = oDivision;
                chkStandardDivLst.DataTextField = "DivisionName";
                chkStandardDivLst.DataValueField = "Id";
                chkStandardDivLst.DataBind();
                chkStandard.Attributes.Add("onclick", "CheckAll(this,'" + iRowId + "')");
                chkStandardDivLst.Attributes.Add("onclick", "CheckAllCheck('" + iRowId + "')");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method

    /// <summary>
    /// This method is used to check Is file Uploaded or not.
    /// </summary>
    private string CheckIsFileFileUploaded(out string asFileName)
    {
        asFileName = string.Empty;
        if (FilUpImg.FileName != string.Empty)
        {
            string sReturnErrorMsg = string.Empty;
            string sServerPath = Server.MapPath("~");
            if (sServerPath.Substring(sServerPath.Length - 1) != "\\")
                sServerPath = sServerPath + "\\";
            string sLinkName = CommonUtility.GetFileNameForRenaming(FilUpImg.FileName.ToString());
            if (FilUpImg.HasFile)
            {
                string sFileName = FilUpImg.PostedFile.FileName;
                string sFileExtention = System.IO.Path.GetExtension(sFileName);
                string sFileMimeType = FilUpImg.PostedFile.ContentType;
                int iFileLengthinKb = FilUpImg.PostedFile.ContentLength / I_FILE_SIZE_LIMIT;

                string[] matchExtension = { ".jpg", ".png", ".bmp", ".jpeg", ".pdf", ".JPG", ".PNG", ".BMP", ".JPEG", ".PDF" };
                string[] matchMimeType = { "image/jpg", "image/png", "image/bmp", "image/jpeg", "application/pdf", "image/JPG", "image/PNG", "image/BMP", "image/JPEG", "application/PDF" };

                if (matchExtension.Contains(sFileExtention) && matchMimeType.Contains(sFileMimeType))
                {
                    if (FilUpImg.PostedFile.ContentLength <= I_FILE_SIZE_LIMIT)
                    {
                        string sLinkPath = sServerPath + S_FOLDER_LOCATION + sLinkName;
                        FilUpImg.SaveAs(sLinkPath);
                        asFileName = sLinkName;
                    }
                    else
                        sReturnErrorMsg = S_FILE_SIZE_ERROR;
                }
                else
                    sReturnErrorMsg = "File type should be between .pdf, .jpg, .jpeg, .png and .bmp.";
            }            
            return sReturnErrorMsg;
        }
        if (asFileName == string.Empty)
            asFileName = hidEventImage.Value;
        return string.Empty;
    }

    /// <summary>
    /// This method is used to check Is file Uploaded or not.
    /// </summary>
    private string CheckIsFamilyPhotoUploaded(out string asFamilyFileName)
    {
        asFamilyFileName = string.Empty;
        if (FilUpImg.FileName != string.Empty)
        {
            string sReturnErrorMsg = string.Empty;
            string sServerPath = Server.MapPath("~");
            if (sServerPath.Substring(sServerPath.Length - 1) != "\\")
                sServerPath = sServerPath + "\\";
            string sLinkFamilyName = CommonUtility.GetFamilyFileNameForRenaming(FilUpImg.FileName.ToString());
            if (FilUpImg.HasFile)
            {
                string sFileName = FilUpImg.PostedFile.FileName;
                string sFileExtention = System.IO.Path.GetExtension(sFileName);
                string sFileMimeType = FilUpImg.PostedFile.ContentType;
                int iFileLengthinKb = FilUpImg.PostedFile.ContentLength / I_FILE_SIZE_LIMIT;

                string[] matchExtension = { ".jpg", ".png", ".bmp", ".jpeg", ".pdf", ".JPG", ".PNG", ".BMP", ".JPEG", ".PDF" };
                string[] matchMimeType = { "image/jpg", "image/png", "image/bmp", "image/jpeg", "application/pdf", "image/JPG", "image/PNG", "image/BMP", "image/JPEG", "application/PDF" };

                if (matchExtension.Contains(sFileExtention) && matchMimeType.Contains(sFileMimeType))
                {
                    if (FilUpImg.PostedFile.ContentLength <= I_FILE_SIZE_LIMIT)
                    {
                        string sLinkFamilyPath = sServerPath + S_FOLDER_LOCATION + sLinkFamilyName;
                        FilUpImg.SaveAs(sLinkFamilyPath);
                        asFamilyFileName = sLinkFamilyName;
                    }
                    else
                        sReturnErrorMsg = S_FILE_SIZE_ERROR;
                }
                else
                    sReturnErrorMsg = "File type should be between .pdf, .jpg, .jpeg, .png and .bmp.";
            }
            return sReturnErrorMsg;
        }
        if (asFamilyFileName == string.Empty)
            asFamilyFileName = hidEventImage.Value;
        return string.Empty;
    }


    /// <summary>
    /// This method is used to create query string and redirect to base screen.
    /// </summary>
    private void SetQueryString()
    {
        string sQueryString = "EventDate=" + mdtEventDate + "&Standard_Id=" + miStandardId + "&DivisionId=" +miDivisionId;
        string sEncryptQueryString = Utility.CommonUtility.EncryptQuerystring(sQueryString);
        sQueryString = "'?" + sEncryptQueryString + "'";
        Response.Write("<Script language='Javascript'>window.opener.location=window.opener.location.pathname+" + sQueryString + ";window.opener.focus(); ");
        Response.Write("window.close();");
        Response.Write("</script>");
    }
    
    /// <summary>
    /// This method is used to set today's date to start date.
    /// </summary>
    private void SetDefaultValues()
    {
        cStartDate.DateValue = mdtEventDate;
        csEndDate.DateValue = mdtEventDate;
        txtEventDesc.Text = "";
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        btnsave.Attributes.Add("onclick", "DisableButtons()");
        chkAll.Attributes.Add("onclick", "CheckOrUncheckAllCheckBox()");
        btncancel.Attributes.Add("onclick", "if(!(closewindow(" + miStandardId + "))){return false};");
        BtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
        calStartDate.Attributes.Add("onChange", "CheckDate();");
        BtnNew.Attributes["onclick"] = "javascript:ValidateControls()";        
        ApplyMouseHoverEffect(new List<Button> { BtnDelete, BtnNew, btnsave, btncancel });
    }

    /// <summary>
    /// This method is used to reset all the controls.
    /// </summary>
    private void ResetControls()
    {
        hidIsNewRecord.Value = "true";
        BtnDelete.Visible = false;
        SetDefaultValues();
        
        foreach (ListViewDataItem Item in lstvwStandardDivisions.Items)
        {
            CheckBoxList chkStandardDivLst = Item.FindControl("chkStandardDivLst") as CheckBoxList;
            CheckBox chkStandard = Item.FindControl("chkStandard") as CheckBox;

            for (int iStandardIndex = 0; iStandardIndex < chkStandardDivLst.Items.Count; iStandardIndex++)            
                chkStandardDivLst.Items[iStandardIndex].Selected = false;     
                           
            chkStandard.Checked = false;
        }

        txtEventDesc.Focus();
        chkDisplayOnHomepage.Checked = false;
        txtEvevtDescription.Text = string.Empty;
        imgbtnDelete.Visible = false;
        btnView.Visible = false;
    }
    
    /// <summary>
    /// This method is used to fill particular event data.
    /// </summary>
    private void FillEventData()
    {
        int iEventId = Convert.ToInt32(hidEventID.Value);
        SchoolEventBL oEventDescriptionBL = new SchoolEventBL(iEventId);
        DataTable odtStandardList = SchoolEventBL.GetAssociatedStdLst(iEventId);       
		chkDisplayOnHomepage.Checked=oEventDescriptionBL.Display_On_Homepage==1;       

        string[] sArrStandards = new string[odtStandardList.Rows.Count];

        for (int i = 0; i < odtStandardList.Rows.Count; i++)
        {
            sArrStandards[i] = odtStandardList.Rows[i]["StandardDivisionId"].ToString();
        }

            foreach (ListViewDataItem Item in lstvwStandardDivisions.Items)
            {
                CheckBoxList chkStandardDivLst = Item.FindControl("chkStandardDivLst") as CheckBoxList;
                CheckBox chkStandard = Item.FindControl("chkStandard") as CheckBox;
                int iTotal = 0;
                for (int iStandardIndex = 0; iStandardIndex < chkStandardDivLst.Items.Count; iStandardIndex++)
                {
                    string sStandardId = chkStandardDivLst.Items[iStandardIndex].Value.ToString();                    
                    if (sArrStandards.Contains(sStandardId))
                    {
                        chkStandardDivLst.Items.FindByValue(sStandardId).Selected = true;
                        iTotal++;
                    }
                    else
                        chkStandardDivLst.Items.FindByValue(sStandardId).Selected = false;
                }

                if (iTotal == chkStandardDivLst.Items.Count)
                    chkStandard.Checked = true;
                else
                    chkStandard.Checked = false;
            }

        txtEventDesc.Text = oEventDescriptionBL.Event_Description;
        cStartDate.DateValue = oEventDescriptionBL.Event_Start_Date;
        csEndDate.DateValue = oEventDescriptionBL.Event_End_Date;
        txtEvevtDescription.Text = oEventDescriptionBL.Event_Comments;

        if (oEventDescriptionBL.Event_Photo != null)
        {
            btnView.Visible = true;
            imgbtnDelete.Visible = true;

            hidEventImage.Value = oEventDescriptionBL.Event_Photo;

            string sNewFileName = S_FOLDER_PATH + oEventDescriptionBL.Event_Photo;
            btnView.Attributes.Add("onclick", "OpenWindow('" + sNewFileName + "'); return false;");
        }
        else
        {
            btnView.Visible = false;
            imgbtnDelete.Visible = false;
        }
    }

    /// <summary>
    /// This method is used to initialize controls.
    /// </summary>
    private void InitializeControls()
    {        
        cStartDate.DateValue = mdtEventDate;
        csEndDate.DateValue = mdtEventDate;        
        if(lstEvents.Items.Count > 0)
        {
            hidEventID.Value = lstEvents.SelectedValue;
            FillEventData();
        }
    }

    /// <summary>
    /// This method initialises hidden fields with the start and end date of selected academic year.
    /// </summary>
    private void SetAcademicYearDates()
    {
        hidYearStartDate.Value = Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE].ToDateTime().ToString(new CultureInfo("en"));
        hidYearEndDate.Value = Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE].ToDateTime().ToString(new CultureInfo("en"));
    }   

    /// <summary>
    /// This method is used to read querystring.
    /// </summary>
    private void GetQueryString()
    {
        if (QueryString["EventDate"] != null)
            mdtEventDate = Convert.ToDateTime(QueryString["EventDate"].ToDateTime().ToString(Constants.S_DATE_FORMAT_MARATHI, new CultureInfo("en")));
        
		if (QueryString["Standard_Id"] != null)
            miStandardId = QueryString["Standard_Id"].ToInt();

        if (QueryString["DivisionId"] != null)
            miDivisionId = QueryString["DivisionId"].ToInt();
    }

    /// <summary>
    /// This method is used fill event list.
    /// </summary>
    private void FillEventListBox()
    {
        SchoolEventBL oEventDescriptionBL = new SchoolEventBL();        
        int iStandardId = miStandardId;
        DataTable oDTEventList = oEventDescriptionBL.GetEventDescription(mdtEventDate, miSchoolId,miAcademicYearId,iStandardId, miDivisionId);

        List<int> eventDetailIds = (from event1 in oDTEventList.AsEnumerable()
                                    join event2 in oDTEventList.AsEnumerable()
                                    on event1["Event_Id"] equals event2["Event_Id"]
                                    where Convert.ToInt32(event1["StandardDivisionId"]) != 0
                                    && Convert.ToInt32(event2["StandardDivisionId"]) == 0
                                    select Convert.ToInt32(event2["Schoolwise_Event_Detail_Id"])).Distinct().ToList();

        DataTable oDTEvents;
        if (eventDetailIds.Count == 0)
            oDTEvents = oDTEventList;
        else
        {
            oDTEvents =
            ((from event1 in oDTEventList.AsEnumerable()              
              select event1)
                                         .Except
                                         (
                                          from event1 in oDTEventList.AsEnumerable()
                                          join ids in eventDetailIds
                                          on Convert.ToInt32(event1["Schoolwise_Event_Detail_Id"]) equals ids
                                          where Convert.ToInt32(event1["Standard_Id"]) == 0
                                          select event1
                                         )).CopyToDataTable();

        }
        lstEvents.Items.Clear();
        if (oDTEvents.Rows.Count > Constants.I_ZERO)
        {
            lstEvents.DataSource = oDTEvents.DefaultView;
            lstEvents.DataBind();
            lstEvents.SelectedIndex = Constants.I_ZERO;
            chkDisplayOnHomepage.Checked = Convert.ToBoolean(oDTEvents.Rows[0]["Display_On_Homepage"]);
            hidIsNewRecord.Value = "false";
            BtnDelete.Visible = true;
        }        
    }   

    /// <summary>
    /// This method populates properties of EventDescriptionBL and return its object.
    /// </summary>
    /// <returns>EventDescriptionBL</returns>
    private SchoolEventBL PopulateEventBL(string sFileNAme)
    {
        SchoolEventBL oEventDescriptionBL = new SchoolEventBL();        
        oEventDescriptionBL.Event_Description = txtEventDesc.Text.Trim();
        oEventDescriptionBL.Event_Start_Date =Convert.ToDateTime(calStartDate.Text);
        oEventDescriptionBL.Event_End_Date = Convert.ToDateTime(calEndDate.Text);
        oEventDescriptionBL.School_Id = miSchoolId;
        oEventDescriptionBL.Schoolwise_Academic_Year_Id = miAcademicYearId;
        oEventDescriptionBL.Inserted_By_id = miUserId;
        oEventDescriptionBL.Updated_By_Id = miUserId;
        oEventDescriptionBL.Display_On_Homepage = Convert.ToInt32(chkDisplayOnHomepage.Checked);
        oEventDescriptionBL.Event_Photo = sFileNAme;
        oEventDescriptionBL.Event_Comments = txtEvevtDescription.Text.Trim();
        return oEventDescriptionBL;
    }

    /// <summary>
    /// This method is used to fill standard check box list.
    /// </summary>
    private void FillStandardChkLstBox()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        molstStandardDivisionMaster = oStandardCollectionBL.GetAllClasses();

        var oStandards = molstStandardDivisionMaster.Select(sd => new { StandardName = sd.StandardName, StandardId = sd.StandardId }).Distinct();
        lstvwStandardDivisions.DataSource = oStandards;
        lstvwStandardDivisions.DataBind();         
    }

    /// <summary>
    /// This method is used to get arraylist of standards.
    /// </summary>
    /// <returns></returns>
    private ArrayList GetStandardArrLst()
    {
        ArrayList arrAssociatedStdLst = new ArrayList();

        foreach (ListViewDataItem Item in lstvwStandardDivisions.Items)
        {
            CheckBoxList chkStandardDivLst = Item.FindControl("chkStandardDivLst") as CheckBoxList;
            for (int iCount = 0; iCount < chkStandardDivLst.Items.Count; iCount++)
            {
                if (chkStandardDivLst.Items[iCount].Selected)
                    arrAssociatedStdLst.Add(chkStandardDivLst.Items[iCount].Value);
            }
        }
        return arrAssociatedStdLst;
    }

    /// <summary>
    /// This Method used to change value of messgae according to culture
    /// </summary>
    private void RefreshValues()
    {
        hidValEventStartDate.Value = Resources.LocalizedResources.ValEventStartDate;
        hidValEventEndDate.Value = Resources.LocalizedResources.ValEventEndDate;
        hidAreYouSureYouWantDeleteEvent.Value = Resources.LocalizedResources.AreYouSureYouWantDeleteEvent;
        hidValEventDisplayOnHomePage.Value = Resources.LocalizedResources.ValEventDisplayOnHomePage;
        hidValEventLength.Value = Resources.LocalizedResources.ValEventLength;
        hidEventDescriptionShouldNotBeBlank.Value = Resources.LocalizedResources.EventDescriptionShouldNotBeBlank;
        ValErrMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;       
    }

    #endregion
}
