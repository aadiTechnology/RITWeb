using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using MasterEntities;
using SchoolEntities;
using Utility;
using System.IO;
using System.Collections;
using StudentEntities;
using PushNotificationService;
using System.Text;
using System.Web;

/// <summary>
/// This class is used to assign homework, view homework assigned by other subjects or selected or logged in teacher.
/// </summary>
public partial class AssignHomeworkUI : SchoolBase
{
	#region Constants

	private const string S_SUCCESS_MSG = "Homework published successfully!!!";
	private const string S_UNPUBLISH_MSG = "Homework unpublished successfully!!!";
	private const string S_SAVE_MSG = "Homework saved successfully!!!";
	private const string S_UPDATE_MSG = "Homework updated successfully!!!";	
	private const string S_DELETE_MSG = "Homework deleted successfully!!!";
	public const string S_COMMAND_PUBLISH = "PUBLISH";
    private const string S_Homework_FOLDER_LOCATION = "\\DOWNLOADS\\Homework\\";
    private const int I_FILE_SIZE_LIMIT = 5242880; // nearly 5 mb

	#endregion

	#region "Events"

	private HomeWorkBL moHomeworkBL = null;
    
	/// <summary>
	/// This event is used to initialize the controls with default values. In this event is used fill subject combobox, read query string, and display homework list. Also it is used to 
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
            Page.Culture = "en";
            moHomeworkBL = new HomeWorkBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
			{
                HideControl();
                InitializeControls();
				ReadQueryString();
                FillDivisionCheckBoxList();
				FillSubjectsComboBox();
				txtAssignedDt.Text = DateTime.Now.ToString(Constants.S_DATE_FORMAT);
				txtTitle.Text = cmbSubject.SelectedItem.Text + " : " + DateTime.Now.ToString(Constants.S_DATE_FORMAT);
				txtSearchDt.Text = hidDate.Value != string.Empty ? hidDate.Value : DateTime.Now.ToString(Constants.S_DATE_FORMAT);
				FillHomeworksList();
                CheckSMSStatus();
				cmbSubject.Focus();
			}
          
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}
 
	/// <summary>
	/// This event is used to save homework details.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
            //if (fileUpload.PostedFile.ContentLength > Constants.I_MAX_FILE_SIZE_LIMIT)
            //{
            //    lblError.Visible = true;
            //    lblSuccess.Visible = false;
            //    return;
            //}

            //string sFileName = SaveAttachments();
            string sFileName = string.Empty;
            SaveHomeworkDetails(sFileName);
			
			FillHomeworksList();
            CheckSMSStatus();
			Clear();
		}
		catch (SqlException ex)
		{
			lblSuccess.Visible = false;
			lblError.Visible = true;
			lblError.Text = ex.Message;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}
	
	/// <summary>
	/// This event is used fill listviews according selected subject.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void cmbSubject_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			lblSuccess.Visible = false;
			txtTitle.Text = cmbSubject.SelectedItem.Text + " : " + DateTime.Now.ToString(Constants.S_DATE_FORMAT);
			hidSubjectId.Value = cmbSubject.SelectedValue;
			FillHomeworksList();
			cmbSubject.Attributes.Remove("disabled");
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to fill listviews according to selected date.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void calAssignedDtSearch_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
            CheckSMSStatus();
			lblSuccess.Visible = false;
			FillHomeworksList();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to bind event to image button.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwHomeworkTeacher_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
				Label lblAssignedDt = oCurrentItem.FindControl("lblAssignedDt") as Label;
				Label lblCompleteDt = oCurrentItem.FindControl("lblCompleteDt") as Label;
				Button btnPublish = oCurrentItem.FindControl("btnPublish") as Button;
				LinkButton lnkTitle = oCurrentItem.FindControl("lnkTitle") as LinkButton;
                ImageButton imgBtnEdit = oCurrentItem.FindControl("imgBtnEdit") as ImageButton;
				ImageButton imgBtnDelete = oCurrentItem.FindControl("imgBtnDelete") as ImageButton;
                ImageButton imgBtn = oCurrentItem.FindControl("imgView") as ImageButton;///////////
                bool bHasLinkedHomework = Convert.ToBoolean(lstvwHomeworkTeacher.DataKeys[oCurrentItem.DisplayIndex]["HasLinkedHomework"]);

                int iFlag = Convert.ToInt32(lstvwHomeworkTeacher.DataKeys[oCurrentItem.DisplayIndex]["flag"].ToString());
                if(iFlag!=0)
                    imgBtn.Visible = true;
                else
                    imgBtn.Visible = false;

                imgBtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete(" + (bHasLinkedHomework ? 1 : 0) + ")){return false;}");
				string sQueryString = CommonUtility.EncryptQuerystring("HomeworkId=" + ((Homework)oCurrentItem.DataItem).Id);
				lnkTitle.Attributes.Add("onclick", "window.open('" + lnkTitle.PostBackUrl + sQueryString + "' , '_blank','scrollbars=yes,resizable=yes,top=0,left=0,width=800,height=600'); return false;");

                imgBtn.Attributes.Add("onclick", "window.open('../Homework/HomeAdditionalAttachmentPopUp.aspx?" + sQueryString + "' , '_blank','scrollbars=yes,resizable=yes,top=0,left=0,width=800,height=600'); return false;");

				ApplyMouseHoverEffect(new List<Button>() { btnPublish });

				if (((Homework)oCurrentItem.DataItem).IsPublished)
				{
					btnPublish.Text = "Unpublish";
					imgBtnDelete.Visible = imgBtnEdit.Visible = false;
					btnPublish.Attributes.Add("onclick", "ShowPopup(" + ((Homework)oCurrentItem.DataItem).Id + ");return false;");
				}
                else
                    btnPublish.Attributes.Add("onclick", "if(!ConfirmPublish('" + ((Homework)oCurrentItem.DataItem).AssignedDate.ToString(Constants.S_STANDARD_DATE_FORMAT) + "')){return false;}");
                    
				HyperLink lnkAttachment = oCurrentItem.FindControl("lnkAttachment") as HyperLink;
                
                if (lnkAttachment.Text.IndexOf("$") > 0)
                    lnkAttachment.Text = lnkAttachment.Text.Substring(0, lnkAttachment.Text.IndexOf("$")) + lnkAttachment.Text.Substring(lnkAttachment.Text.LastIndexOf("."));

                lnkAttachment.NavigateUrl = Constants.S_HOMEWORK_FOLDER_LOCATION + ((Homework)oCurrentItem.DataItem).AttachmentPath;
                lnkAttachment.Attributes.Add("onclick", "window.open('" + lnkAttachment.NavigateUrl + "' , '_blank','scrollbars=yes,resizable=yes,top=0,left=0,width=800,height=600'); return false;");
				lblAssignedDt.Text = ((Homework)oCurrentItem.DataItem).AssignedDate.ToString(Constants.S_STANDARD_DATE_FORMAT);
				lblCompleteDt.Text = ((Homework)oCurrentItem.DataItem).CompleteByDate.ToString(Constants.S_STANDARD_DATE_FORMAT);
              
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This method is used to publish, delete homework details.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwHomeworkTeacher_ItemCommand(object sender, ListViewCommandEventArgs e)
	{
		try
		{
			ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
			lblSuccess.Text = string.Empty;
			int iHomeworkId = Convert.ToInt32(lstvwHomeworkTeacher.DataKeys[oCurrentItem.DisplayIndex]["Id"].ToString());
            switch (e.CommandName)
			{
                case Constants.S_COMMAND_REMOVE: moHomeworkBL.Delete(iHomeworkId, hidDeleteFromAll.Value);
					lblSuccess.Text = S_DELETE_MSG;
					FillHomeworksList();
                    btnDownload.Visible = false;
                    imgBtnDelete.Visible = false;
                    lblSuccess.Visible = true;
                    lblError.Visible = false;
                    lblSuccess.Text = S_DELETE_MSG;
					Clear();
					break;
				case Constants.S_COMMAND_UPDATE: hidMode.Value = Constants.S_EDIT_MODE;
					EditHomework(iHomeworkId);
                    SetAttachment();
					break;
                case S_COMMAND_PUBLISH: 
                    moHomeworkBL.Publish(iHomeworkId.ToString(), (hidSendSMS.Value == Constants.S_YES));

                    if (Settings.EnableHomeworkModule && Settings.EnableHomeworkModuleForStudentLogin && Settings.SendHomeworkSMSToParents && hidSendSMS.Value == Constants.S_YES)
                    {
                        SendSMS(iHomeworkId);
                        hidSendSMS.Value = Constants.S_NO;
                    }

					lblError.Visible = false;
					lblSuccess.Visible = true;
					hidMode.Value = Constants.S_NEW_MODE;
					Clear();
					lblSuccess.Text = S_SUCCESS_MSG;
                    SendPushNotification(iHomeworkId.ToString(), oCurrentItem);
					FillHomeworksList();
					break;

			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

    /// <summary>
	/// This event is used to unpublish homework details.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnUnpublish_Click(object sender, EventArgs e)
	{
		try
		{
            moHomeworkBL.UnPublish(hidId.Value, txtUnpublishReason.Text.Trim());
			lblError.Visible = false;
			lblSuccess.Visible = true;
			lblSuccess.Text = S_UNPUBLISH_MSG;
			FillHomeworksList();
			txtUnpublishReason.Text = string.Empty;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}


	/// <summary>
	/// This event is used to set date time format for homework assigned date and complete by date.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwOtherSubjectHomework_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{   
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
				Label lblCompleteDt = oCurrentItem.FindControl("lblCompleteDt") as Label;
				lblCompleteDt.Text = ((Homework)oCurrentItem.DataItem).CompleteByDate.ToString(Constants.S_STANDARD_DATE_FORMAT);                
				LinkButton lnkTitle = oCurrentItem.FindControl("lnkTitle") as LinkButton;
				string sQueryString = CommonUtility.EncryptQuerystring("HomeworkId=" + ((Homework)oCurrentItem.DataItem).Id);
				lnkTitle.Attributes.Add("onclick", "window.open('" + lnkTitle.PostBackUrl + sQueryString + "' , '_blank','scrollbars=yes,resizable=yes,top=0,left=0,width=800,height=600'); return false;");
				HtmlTableRow oHtmlTableRow = oCurrentItem.FindControl("trItem") as HtmlTableRow;
                HiddenField hidHomeworkId = oCurrentItem.FindControl("hidHomeworkId") as HiddenField;

                Label lblSrNo = oCurrentItem.FindControl("lblSrNo") as Label;
                lblSrNo.Text = Convert.ToString(oCurrentItem.DisplayIndex + 1);

				if (moUserRole != Constants.UserRoles.Student)
				{
					Image imgBtbPublished = oCurrentItem.FindControl("imgBtbPublished") as Image;
					Image imgBtbNotPublished = oCurrentItem.FindControl("imgBtbNotPublished") as Image;
                    if (!((Homework)oCurrentItem.DataItem).IsPublished)                    
                        imgBtbPublished.Visible = false;                                            
                    else                    
                        imgBtbNotPublished.Visible = false;                                                            
				}
				else
				{
					HtmlTableCell oHtmlTableCell = oHtmlTableRow.FindControl("tdPublish") as HtmlTableCell;
					oHtmlTableCell.Visible = false;
				}
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}
    
    /// <summary>
    /// This event use to select homework status from drop down box
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void drdwnHomeWorkStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            calAssignedDtSearch_SelectionChanged(sender, e);
            txtHomeworkTitle.Text = string.Empty;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to clear the control.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            txtDetails.Text = string.Empty;
            txtTitle.Text = string.Empty;
            txtCompleteByDt.Text = string.Empty;
            txtAssignedDt.Text = DateTime.Now.ToString(Constants.S_DATE_FORMAT);
            hidMode.Value = "New";
            hidId.Value = Constants.S_ZERO;
            hidFileName.Value = string.Empty;
            btnDownload.Visible = false;
            imgBtnDelete.Visible = false;
            lblSuccess.Text = string.Empty;
            valSumErrorMsg.HeaderText = string.Empty;
            cmbSubject.Attributes.Remove("disabled");
            lblError.Visible = false;
            txtHomeworkTitle.Text = string.Empty;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to publish all unpublished homework.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPublishAll_Click(object sender, EventArgs e)
    {
        try
        {
            string sHomeworkIds = PopulateHomeworkDetails();

            moHomeworkBL.Publish(sHomeworkIds, (hidSendSMS.Value == Constants.S_YES));

            if (Settings.EnableHomeworkModule && Settings.EnableHomeworkModuleForStudentLogin && Settings.SendHomeworkSMSToParents && hidSendSMS.Value == Constants.S_YES)
            {

                string[] HomeworkIds = sHomeworkIds.Split(',');

                int iHomeworkId = HomeworkIds[0].ToInt();

                SendSMS(iHomeworkId.ToInt());
                SendPushNotification(iHomeworkId.ToString(), lstvwOtherSubjectHomework);
                
                hidSendSMS.Value = Constants.S_NO;
            }

            lblError.Visible = false;
            lblSuccess.Visible = true;
            hidMode.Value = Constants.S_NEW_MODE;
            Clear();
            lblSuccess.Text = S_SUCCESS_MSG;            
            FillHomeworksList();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

	#region "Private Method"

    /// <summary>
    /// This method is used to populate values for publish homework.
    /// </summary>
    private string PopulateHomeworkDetails()
    {
        StringBuilder sHomeworIds = new StringBuilder();
        for (int iRowNo = 0; iRowNo < lstvwOtherSubjectHomework.Items.Count; iRowNo++)
        {
            CheckBox chkPublish = lstvwOtherSubjectHomework.Items[iRowNo].FindControl("chkPublish") as CheckBox;
            if (chkPublish.Checked)
            {
                int iHomeworkId = Convert.ToInt32(lstvwOtherSubjectHomework.DataKeys[iRowNo]["Id"]);
                sHomeworIds.Append(iHomeworkId + ",");  
            }
        }
        if (!sHomeworIds.IsNull())
            sHomeworIds = sHomeworIds.Remove(sHomeworIds.Length - 1, 1);

        return sHomeworIds.ToString();
    }

	/// <summary>
	/// This method is used to initialize control values.
	/// </summary>
	private void InitializeControls()
	{
		valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
		ApplyMouseHoverEffect(new List<Button> { btnBack, btnSave, btnUnpublish, btnClosePopup, btnCancel, btnUnpublishAll, btnPublishAll});
		btnBack.PostBackUrl = "~/RITeSchool/Homework/HomeworkUI.aspx?" + Request.QueryString.ToString();
        btnDownload.Visible = false;
        imgBtnDelete.Visible = false;
        Clear();
        btnPublishAll.Attributes.Add("onclick", "if(!ConfirmPublishAllHomework()){return false;}");
        btnUnpublishAll.Attributes.Add("onclick", "GetHomeworIdsAndShowPopup();return false;");
	}
    
    /// <summary>
    /// This method use to delete or view attachement
    /// </summary>
    private void SetAttachment()
    {
        if (!string.IsNullOrEmpty(hidFileName.Value))
        {
            btnDownload.Visible = true;
            btnDownload.Text = hidFileName.Value;
            string sServerPath = Server.MapPath("..");
            if (sServerPath.Substring(sServerPath.Length - 1) != "\\")
                sServerPath = sServerPath + "\\";
            string sDestination = sServerPath + "DOWNLOADS\\Homework\\" + hidFileName.Value;
            if (File.Exists(sDestination))
                btnDownload.Attributes.Add("onclick", "window.open('"+Constants.S_HOMEWORK_FOLDER_LOCATION +""+hidFileName.Value + "','_blank'); return false;");
            imgBtnDelete.Visible = true;

        }
        else
        {
            btnDownload.Visible = false;
            imgBtnDelete.Visible = false;
        }
    }
	/// <summary>
	/// This method is used to fill subject comobox.
	/// </summary>
	private void FillSubjectsComboBox()
	{
		// get all class subjects
        TeacherSubjectAssignmentBL oTeacherSubjectAssignmentBL = new TeacherSubjectAssignmentBL();
        DataTable oDtTeacherSubjects = oTeacherSubjectAssignmentBL.GetTeacherSubjectDetails(miSchoolId, miAcademicYearId);
        if (hidListViewType.Value.ToInt() == Constants.I_ZERO)
        {
            var query = from Record in oDtTeacherSubjects.AsEnumerable() where Record.Field<int>("Teacher_Id") == hidTeacherId.Value.ToInt() && Record.Field<int>("Standard_Division_Id") == hidStdDivId.Value.ToInt() select Record;
            DataTable oDtFilterData = hidTeacherId.Value != Constants.S_ZERO ? query.CopyToDataTable() : null;
            ControlUtility.FillDropDownList(oDtFilterData.DefaultView.ToTable(true, Constants.S_SUBJECT_ID_FIELD, Constants.S_SUBJECT_NAME_FIELD), ref cmbSubject, Constants.S_SUBJECT_ID_FIELD, Constants.S_SUBJECT_NAME_FIELD, Constants.S_ALL);
        }
        else
        {
            int iStdDivId = 0;
            DataRow[] drArray = oDtTeacherSubjects.Select("Is_ClassTeacher = 'Y' AND Teacher_Id=" + hidTeacherId.Value);
            if (drArray.Length > 0)
                iStdDivId = drArray[0]["Standard_Division_Id"].ToInt();
            else
                iStdDivId = hidStdDivId.Value.ToInt();
            var query = from Record in oDtTeacherSubjects.AsEnumerable() where Record.Field<int>("Teacher_Id") != hidTeacherId.Value.ToInt() && Record.Field<int>("Standard_Division_Id") == iStdDivId select Record;
            DataTable oDtFilterData = hidStdDivId.Value != Constants.S_ZERO ? query.CopyToDataTable() : null;
            ControlUtility.FillDropDownList(oDtFilterData.DefaultView.ToTable(true, Constants.S_SUBJECT_ID_FIELD, Constants.S_SUBJECT_NAME_FIELD), ref cmbSubject, Constants.S_SUBJECT_ID_FIELD, Constants.S_SUBJECT_NAME_FIELD, string.Empty);
            }
        }
	

	/// <summary>
	/// This function sets the form fields according to the query string values.
	/// </summary>
	private void ReadQueryString()
	{
		if (QueryString.Count <= 0)
			return;

		if (QueryString["SubjectId"] != null)
		{
			cmbSubject.SelectedValue = QueryString["SubjectId"];      
			hidSubjectId.Value = QueryString["SubjectId"];
		}

		if (QueryString["TeacherId"] != null)
			hidTeacherId.Value = QueryString["TeacherId"];

		if (QueryString["StdDivId"] != null)
			hidStdDivId.Value = QueryString["StdDivId"];

        if (QueryString["ListViewType"] != null)
            hidListViewType.Value = QueryString["ListViewType"];

		if (QueryString["Class"] != null)
			lblClass.Text = hidClassName.Value = QueryString["Class"];

		if (QueryString["Teacher"] != null)
			lblTeacher.Text = hidTeacherName.Value = QueryString["Teacher"];

		if (QueryString["Date"] != null)
		{
			hidDate.Value = QueryString["Date"];
			txtSearchDt.Text = QueryString["Date"];
		}
	}

	/// <summary>
	/// This method is used fill listview of homework assigned by selected date.
	/// </summary>
	private void FillHomeworksList()
	{
        List<Homework> lstHomework = moHomeworkBL.GetListForTeacher(hidStdDivId.Value.ToInt(), txtSearchDt.Text, drdwnHomeWorkStatus.SelectedValue,txtHomeworkTitle.Text.Trim());
		List<Homework> lstOtherSubjectHomework = lstHomework.Where(homework => homework.Subject.SubjectId != hidSubjectId.Value.ToInt()).ToList();
		List<Homework> lstSelectedSubjectHomework = lstHomework.Where(homework => homework.Subject.SubjectId == hidSubjectId.Value.ToInt()).ToList();

		lstvwHomeworkTeacher.DataSource = lstSelectedSubjectHomework;
		lstvwHomeworkTeacher.DataBind();
		lstvwOtherSubjectHomework.DataSource = lstOtherSubjectHomework;
		lstvwOtherSubjectHomework.DataBind();

        if (lstOtherSubjectHomework.Any(sa => sa.IsPublished))
            btnUnpublishAll.Visible = true;

        if (lstOtherSubjectHomework.Any(sa => !sa.IsPublished))
            btnPublishAll.Visible = true;
	}

	/// <summary>
	/// This method is used to clear controls value
	/// </summary>
	private void Clear()
	{
		txtDetails.Text = string.Empty;
		txtCompleteByDt.Text = string.Empty;
		hidMode.Value = Constants.S_NEW_MODE;
		hidId.Value = Constants.S_ZERO;
		cmbSubject.Attributes.Remove("disabled");
        hidFileName.Value = string.Empty;
        txtHomeworkTitle.Text = string.Empty;
        ChkDivisionList.ClearSelection();
	}

	/// <summary>
	/// This method is sued to get homework details and set values to controls.
	/// </summary>
	/// <param name="oHomeWorkBL"></param>
	/// <param name="iHomeworkId"></param>
	private void EditHomework(int aiHomeworkId)
	{
		Homework oHomework = moHomeworkBL.Get(aiHomeworkId);
		txtTitle.Text = oHomework.Title;
		txtDetails.Text = oHomework.Details;
		txtAssignedDt.Text = oHomework.AssignedDate.ToString(Constants.S_DATE_FORMAT);
		txtCompleteByDt.Text = oHomework.CompleteByDate.ToString(Constants.S_DATE_FORMAT);
		hidId.Value = aiHomeworkId.ToString();
        hidFileName.Value = oHomework.AttachmentPath;
        cmbSubject.Attributes.Add("disabled", "true");

        foreach (ListItem oItem in ChkDivisionList.Items)
        {
            if (oHomework.LinkedDivisions.Contains(oItem.Value.ToInt()))
                oItem.Selected = true;
            else
                oItem.Selected = false;
        }
	}

	/// <summary>
	/// This method is used to save homework details.
	/// </summary>
    private void SaveHomeworkDetails(string asFileName)
    {
       List<string> lstFileName = SaveFileToServer();
       string sFileList = base.GenerateXml(lstFileName);

		SubjectMaster oSubject = new SubjectMaster { SubjectId = cmbSubject.SelectedValue.ToInt(), SchoolId = miSchoolId, AcademicYearId = miAcademicYearId };
		Homework oHomeworkDetails = new Homework
		{
			Id = hidId.Value == string.Empty ? 0 : hidId.Value.ToInt(),
			Details = txtDetails.Text.Trim(),
			Title = txtTitle.Text.Trim(),
			AssignedDate = txtAssignedDt.Text.ToDateTime(),
			CompleteByDate = txtCompleteByDt.Text.ToDateTime(),
			InsertedById = Session[Constants.S_SESSION_USER_ID].ToInt(),
			Subject = oSubject,
			StandardDivisionId = hidStdDivId.Value.ToInt(),
            //AttachmentPath = fileUpload.HasFile ? asFileName : hidFileName.Value,            
            AttachmentPath = hidFileName.Value != string.Empty ? hidFileName.Value:string.Empty,
            DivisionIds = GetSelectedDivisions(),
		};
        moHomeworkBL.Save(oHomeworkDetails, sFileList);
        if (hidMode.Value == Constants.S_NEW_MODE)
			lblSuccess.Text = S_SAVE_MSG;
		else
			lblSuccess.Text = S_UPDATE_MSG;
		txtSearchDt.Text = txtAssignedDt.Text;

		lblError.Visible = false;
		lblSuccess.Visible = true;
		cmbSubject.Enabled = true;
        btnDownload.Visible = false;
        imgBtnDelete.Visible = false;
	}

    private string GetSelectedDivisions()
    {
        StringBuilder sb = new StringBuilder();
        foreach (ListItem oItem in ChkDivisionList.Items)
        {
            if (oItem.Selected == true)
                sb.Append("," + oItem.Value);
        }

        if (sb.Length > 0)
            return sb.ToString().Substring(1);
        else
            return string.Empty;
    }

    /// <summary>
    /// This method is used to validate file size new added.
    /// </summary>
    /// <param name="asFileName"></param>
    /// <returns></returns>
    private List<string> SaveFileToServer()
    {
        List<string> lstFiles = new List<string>();
        HttpFileCollection oCollection = Request.Files;
        for (int iCount = 0; iCount < oCollection.Count; iCount++)
        {
            string sFolderName = Server.MapPath("..") + S_Homework_FOLDER_LOCATION;
            HttpPostedFile aoAttachment = oCollection[iCount];
            string sFileName = aoAttachment.FileName;

            if (sFileName.Trim() != string.Empty)
            {
                sFileName = sFileName.Insert(sFileName.LastIndexOf("."), DateTime.Now.ToString("$yyyyMMddHHmmss")).Replace(" ", "_");

                string sServerFilePath = sFolderName + sFileName;
                aoAttachment.SaveAs(sServerFilePath);
                lstFiles.Add(sFileName);
            }
        }

        return lstFiles;
    }
   
	/// <summary>
	/// This method is used to save attachment file on server.
	/// </summary>
    private string SaveAttachments()
    {
        string sFileName = string.Empty;
        string sServerPath = Server.MapPath("~");
        if (sServerPath.Substring(sServerPath.Length - 1) != "\\")
            sServerPath = sServerPath + "\\";
        if (fileUpload.HasFile)
        {
            sFileName = fileUpload.FileName;
            string sNewFileName = sServerPath + "RITeSchool\\DOWNLOADS\\Homework\\" + sFileName;

            if (File.Exists(sNewFileName))
            {
                sFileName = sFileName.Insert(sFileName.LastIndexOf("."), DateTime.Now.ToString("$yyyyMMddHHmmss")).Replace(" ", "_");
                sNewFileName = sServerPath + "RITeSchool\\DOWNLOADS\\Homework\\" + sFileName;
            }

            fileUpload.SaveAs(sNewFileName);
        }

        return sFileName;
    }

    /// <summary>
    /// This method is used to check whether homework sms is already sent.
    /// </summary>
    private void CheckSMSStatus()
    {
        if (Settings.EnableHomeworkModule && Settings.EnableHomeworkModuleForStudentLogin && Settings.SendHomeworkSMSToParents)
        {
            bool bStatus = moHomeworkBL.IsHomeworkSMSSent(hidStdDivId.Value.ToInt(), txtSearchDt.Text.ToDateTime());
            if (bStatus)
                hisSMSStatus.Value = Constants.S_YES;
            else
            {
                hisSMSStatus.Value = Constants.S_NO;
                string sTemplateType = Constants.SMSTemplate.HomeworkAssignmentSMS.ToString();
                DataTable oDt = SmsTemplateBL.GetTemplate(sTemplateType, miSchoolId);
                string sText = oDt.Rows[0]["SmsTemplateText"].ToString();
                hidSMSText.Value = sText.Replace("%CLASS%", lblClass.Text).Replace("%DAY%", txtSearchDt.Text);
            }
        }
        else
            hisSMSStatus.Value = Constants.S_YES;
    }

    /// <summary>
    /// This method is used to send homework SMS.
    /// </summary>
    /// <param name="aiHomeworkId"></param>
    private void SendSMS(int aiHomeworkId)
    {
        string sTemplateType = Constants.SMSTemplate.HomeworkAssignmentSMS.ToString();
        DataTable oDt = SmsTemplateBL.GetTemplate(sTemplateType, miSchoolId);
        string sText = oDt.Rows[0]["SmsTemplateText"].ToString();
        string sSubject = oDt.Rows[0]["SmsTemplateName"].ToString();
       
        string  sName = lblClass.Text;

        sText = sText.Replace("%CLASS%", sName).Replace("%DAY%", txtSearchDt.Text);

        string sTemplateRegistrationId = string.Empty;
        if (oDt.Rows[0]["TemplateRegistrationId"] != DBNull.Value)
            sTemplateRegistrationId = oDt.Rows[0]["TemplateRegistrationId"].ToString();

        SchoolBL oSchoolBL = new SchoolBL(miSchoolId);
        Hashtable oHTUsersMobileNo = new Hashtable();
        string sDisplayText = string.Empty;
        string sMobileNumber = string.Empty;
        string sMobileNumber2 = string.Empty;

        StudentBL oStudentBL = new StudentBL();
        List<StudentInfo> lstStudents = oStudentBL.GetStudentDetails(miSchoolId, miAcademicYearId, aiHomeworkId);

        foreach (StudentInfo oStudent in lstStudents)
        {
            sMobileNumber = oStudent.MobileNo1;
            sMobileNumber2 = oStudent.MobileNo2;

            if (sMobileNumber != string.Empty)
                oHTUsersMobileNo[oStudent.UserId] = sMobileNumber;
            if (sMobileNumber2 != string.Empty && sMobileNumber2 != Constants.S_ZERO)
            {
                oHTUsersMobileNo[oStudent.UserId + "sm;"] = sMobileNumber2;
            }
        }

        if (lstStudents.Count > 0)
        {
            sDisplayText = lstStudents[0].ClassName;

            SMS oSMS = new SMS();
            oSMS.InsertedByID = -9999;
            oSMS.Sender = oSchoolBL.SMSSenderName;

            oSMS.SenderRoleID = Convert.ToInt32(Constants.UserRoles.Admin);
            oSMS.SenderID = oSchoolBL.AdminId;

            if (Settings.HomeworkSmsScheduleTime.Trim() != string.Empty)
            {
                DateTime dtScheduleTime = Settings.HomeworkSmsScheduleTime.ToDateTime();
                DateTime dtCurrentDate = DateTime.Now.AddMinutes(15);
                if (dtCurrentDate < dtScheduleTime)
                {
                    oSMS.IsScheduled = true;
                    oSMS.ScheduledDate = dtScheduleTime;
                }
            }

            oSMS.TemplateRegistrationId = sTemplateRegistrationId;
            oSMS.School_Name = oSchoolBL.SchoolName + "::" + sSubject;
            oSMS.SMSText = sText;
            oSMS.AcademicYearID = miAcademicYearId;
            oSMS.SchoolID = miSchoolId;
            oSMS.DisplayText = sDisplayText;
            oSMS.To = oHTUsersMobileNo;

            oSMS.Send();
        }
        oHTUsersMobileNo.Clear();
    }

    /// <summary>
    /// This method is used to send notification to the parent 
    /// </summary>
    /// <param name="iStudentId"></param>
    /// <param name="dAmount"></param>
    public override void SendPushNotification(string aiHomeworkId, object aoCurrentItem)
    {
        PushNotificationClient pushNotificationClient = null;
        try
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)aoCurrentItem;

            string sSubject = (oCurrentItem.FindControl("lblSubject") as Label).Text;
            // string sTitle = (oCurrentItem.FindControl("lnkTitle") as LinkButton).Text;
            string sAssignedDate = (oCurrentItem.FindControl("lblAssignedDt") as Label).Text;
            string sCompletedDate = (oCurrentItem.FindControl("lblCompleteDt") as Label).Text;

            StudentBL oStudentBL = new StudentBL();
            List<StudentInfo> lstStudents = oStudentBL.GetStudentDetails(miSchoolId, miAcademicYearId, Convert.ToInt32(aiHomeworkId));

            int[] intArrayUserId = lstStudents.Select(s => s.UserId).ToArray();
            string sStandardDivision = lstStudents.Select(s => s.ClassName).FirstOrDefault();


            pushNotificationClient = new PushNotificationClient();
            Dictionary<string, string> dictionaryNotificationParameter = new Dictionary<string, string>();
            dictionaryNotificationParameter.Add(Constants.S_NOTIFICATION_PARAMETER_CLASSNAME, sStandardDivision);
            dictionaryNotificationParameter.Add(Constants.S_NOTIFICATION_PARAMETER_SUBJECT, sSubject);
            dictionaryNotificationParameter.Add(Constants.S_NOTIFICATION_PARAMETER_STARTDATE, sAssignedDate);
            dictionaryNotificationParameter.Add(Constants.S_NOTIFICATION_PARAMETER_ENDDATE, sCompletedDate);
            pushNotificationClient.SendNotification(NotificationMessageHeadings.HomeworkAssigned, this.miSchoolId.ToString(), intArrayUserId, dictionaryNotificationParameter);
            pushNotificationClient.Close();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
        finally
        {
            if (pushNotificationClient.State != System.ServiceModel.CommunicationState.Faulted)
                pushNotificationClient.Close();
        }
    }
    
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillHomeworksList();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    private void HideControl()
    {
        //if (moSchool == Constants.SchoolId.SNS && moUserRole == Constants.UserRoles.Teacher)
        //{
        //    trFirstAttachment.Visible = false;
        //    trFirstAttachmentSupportFiles.Visible = false;
        //}
        //else
        //{
        //    trFirstAttachment.Visible = true;
        //    trFirstAttachmentSupportFiles.Visible = true;
        //}

        trFirstAttachment.Visible = false;
        trFirstAttachmentSupportFiles.Visible = false;
    }

    private void FillDivisionCheckBoxList()
    {
        DivisionMasterBL oDivisionMasterBL = new DivisionMasterBL();
        DataTable oDt = oDivisionMasterBL.GetDivisionsForHomeWork(hidStdDivId.Value.ToInt(), miSchoolId, miAcademicYearId, miUserId, hidSubjectId.Value.ToInt());

        if (oDt.IsNonEmpty())
        {
            trDivisions.Visible = true;
            ListSource.FillCheckBoxList(oDt, ChkDivisionList, "Division_Name", "Division_Id");
        }
        else
            trDivisions.Visible = false;
    }
      
	#endregion    
}