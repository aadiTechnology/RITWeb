using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using DocumentEntity;
using Utility;
using XseedReportEntities;
using System.Linq;

public partial class StudentDocumentUI :SchoolBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillTeachersComboBox();            
        }        
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                SaveDocumentDetails();
                lblMessage.Text = "Document details saved successfully !!!";
                FillDocumentDetails();
            }
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
            
        }
    }
    protected void cmbClassTeacher_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            XseedProgressReportBL oXseedProgressReportBL = new XseedProgressReportBL();
            List<YearwiseStudentMaster> lstYearwiseStudentMaster = new List<YearwiseStudentMaster>();
            if (Convert.ToInt32(cmbClassTeacher.SelectedValue) != 0)
            {
                int iStandardDivisionId = Convert.ToInt32(cmbClassTeacher.SelectedValue);
                lstYearwiseStudentMaster = oXseedProgressReportBL.GetStudents(miSchoolId, miAcademicYearId, iStandardDivisionId, 0);

                if (lstYearwiseStudentMaster.Count > 0)
                {
                    ListSource.FillDropDownList(lstYearwiseStudentMaster, cmbStudents, "StudentName", "YearwiseStudentId", Constants.S_SELECT);                    
                }
                else
                {
                    cmbStudents.Items.Clear();
                    cmbStudents.Items.Insert(0, new ListItem(Constants.S_SELECT, Constants.S_ZERO));
                    ResetData();
                }
            }
            else
            {
                cmbStudents.Items.Clear();
                cmbStudents.Items.Insert(0, new ListItem(Constants.S_SELECT, Constants.S_ZERO));
                ResetData();
            }
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }
    protected void cmbStudents_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillDocumentDetails();
    }

    private void FillDocumentDetails()
    {
        StudentBL oStudentBL = new StudentBL(cmbStudents.SelectedValue.ToInt());
        StandardwiseDocumentMasterBL oStandardwiseDocumentMasterBL = new StandardwiseDocumentMasterBL(miSchoolId, miAcademicYearId);
        if (Convert.ToInt32(cmbStudents.SelectedValue) != 0)
        {
            List<StudentDocument> lstStudentDocument = oStandardwiseDocumentMasterBL.GetAdmissionDocumentDetails(oStudentBL.StandardId, oStudentBL.StudentId);
            lstvwConfiguredDocument.DataSource = lstStudentDocument;
            lstvwConfiguredDocument.DataBind();
            btnSave.Visible = lstStudentDocument.Count > 0;
        }
        else
        {
            ResetData();
        }
    }

    private void ResetData()
    {
        lstvwConfiguredDocument.DataSource = null;
        lstvwConfiguredDocument.DataBind();
        btnSave.Visible = false;
    }

    protected void lstvwConfiguredDocument_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
            int iRowId = oCurrentItem.DisplayIndex;
            
            CheckBox oChkIsSubmitted = oCurrentItem.FindControl("ChkSelect") as CheckBox;
            CheckBox oChkIsApplicable = oCurrentItem.FindControl("chkIsApplicable") as CheckBox;
            oChkIsApplicable.Attributes["onclick"] = "javascript:SetIsApplicableSatus(this, " + iRowId + " );";
            oChkIsSubmitted.Attributes["onclick"] = "javascript:SetIsSubmittedSatus(this, " + iRowId + " );";
            oChkIsSubmitted.Checked = Convert.ToBoolean(lstvwConfiguredDocument.DataKeys[iRowId]["IsSubmitted"]);
            oChkIsApplicable.Checked = Convert.ToBoolean(lstvwConfiguredDocument.DataKeys[iRowId]["IsApplicable"]);
            LinkButton oLinkButton = e.Item.FindControl("lnkAttachment") as LinkButton;
            string sQueryString = string.Empty;

            HiddenField oHiddenField = e.Item.FindControl("hidIsDocMandatory") as HiddenField;
            oHiddenField.Value = Convert.ToString(lstvwConfiguredDocument.DataKeys[iRowId]["IsSubmissionMandatory"]);

            if (miSchoolId == Constants.SchoolId.SNS.ToInt() && oHiddenField.Value == "True")
            {
                HtmlTableCell tdMandatoryDoc = e.Item.FindControl("tdDocumentName") as HtmlTableCell;
                HtmlTableCell tdlnkAttachment = e.Item.FindControl("tdlnkAttachment") as HtmlTableCell;
                HtmlTableCell tdSelect = e.Item.FindControl("tdSelect") as HtmlTableCell;
                HtmlTableCell tdIsApplicable = e.Item.FindControl("tdIsApplicable") as HtmlTableCell;

                if (tdMandatoryDoc != null && tdlnkAttachment != null && tdSelect != null && tdIsApplicable != null)
                {
                    LegendTable.Visible = true;
                    tdMandatoryDoc.BgColor = "#ffffcc";
                    tdlnkAttachment.BgColor = "#ffffcc";
                    tdSelect.BgColor = "#ffffcc";
                    tdIsApplicable.BgColor = "#ffffcc";
                }
            }
            
            StudentBL oStudentBL = new StudentBL(cmbStudents.SelectedValue.ToInt());
            int iStandardwiseDocumentId = Convert.ToInt32(lstvwConfiguredDocument.DataKeys[e.Item.DisplayIndex]["StandardwiseDocumentId"]);
            sQueryString = "UserId=" + oStudentBL.UserId +
                           "&DocumentId=" + iStandardwiseDocumentId +
                           "&DocumentTypeId=" + Constants.DocumentTypes.StudentDocuments.ToInt();
             
             sQueryString = CommonUtility.EncryptQuerystring(sQueryString);
             oLinkButton.Attributes.Add("onclick", "OpenPopup('" + sQueryString + "'); return false;");
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    private void FillTeachersComboBox()
    {
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        List<ClassTeacherDetails> lstTeacher = MasterDataCollectionBL.GetClassTeacher(miSchoolId, miAcademicYearId);

        string sHasFullAccess = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.UploadStudentDocument).ToString();

        if (moUserRole == Constants.UserRoles.Admin || (moUserRole == Constants.UserRoles.Supervisor && sHasFullAccess == Constants.S_YES))
            ListSource.FillDropDownList(lstTeacher, cmbClassTeacher, "TeacherName", "StandardDivisionId", Constants.S_SELECT);
        else if (moUserRole == Constants.UserRoles.Teacher && sHasFullAccess == Constants.S_YES)
        {
            ListSource.FillDropDownList(lstTeacher, cmbClassTeacher, "TeacherName", "StandardDivisionId", Constants.S_SELECT);
            ClassTeacherDetails oClassTeacher = lstTeacher.Where(ct => ct.TeacherId == Session[Constants.S_SESSION_TEACHER_ID].ToInt()).FirstOrDefault();
            if (oClassTeacher != null)
            {
                ListItem oListItem = cmbClassTeacher.Items.FindByValue(oClassTeacher.StandardDivisionId.ToString());
                if (oListItem != null)
                {
                    oListItem.Selected = true;
                    cmbClassTeacher_SelectedIndexChanged(cmbClassTeacher, new EventArgs());
                }
            }
        }
        else if (moUserRole == Constants.UserRoles.Teacher && sHasFullAccess != Constants.S_YES)
        {
            List<ClassTeacherDetails> lstClassTeacher = lstTeacher.Where(ct => ct.TeacherId == Session[Constants.S_SESSION_TEACHER_ID].ToInt()).ToList();
            ListSource.FillDropDownList(lstClassTeacher, cmbClassTeacher, "TeacherName", "StandardDivisionId", Constants.S_SELECT);
            if (lstClassTeacher.Count == 1)
            {
                cmbClassTeacher.SelectedIndex = 1;
                cmbClassTeacher_SelectedIndexChanged(cmbClassTeacher, new EventArgs());
                cmbClassTeacher.Enabled = false;
            }
        }
    }

    private void SaveDocumentDetails()
    {        
        StandardwiseDocumentMasterBL oStandardwiseDocumentMasterBL = new StandardwiseDocumentMasterBL(miSchoolId);
        oStandardwiseDocumentMasterBL.SaveSubmittedDocuments(GenerateXml(PopulateDocumentDetails()), cmbStudents.SelectedValue.ToInt(), miUserId);
    }

    private List<StudentDocument> PopulateDocumentDetails()
    {
        List<StudentDocument> lstDocumentInfo = new List<StudentDocument>();
        StudentDocument oStudentDocument = null;

        for (int iRowCount = 0; iRowCount < lstvwConfiguredDocument.Items.Count; iRowCount++)
        {
            oStudentDocument = new StudentDocument();
            ListViewDataItem oCurrentItem = (ListViewDataItem)lstvwConfiguredDocument.Items[iRowCount];
            int iRowId = oCurrentItem.DisplayIndex;
            int iStudentDocumentId = Convert.ToInt32(lstvwConfiguredDocument.DataKeys[iRowId]["StudentDocumentId"]);
            int iStandardwiseDocumentId = Convert.ToInt32(lstvwConfiguredDocument.DataKeys[iRowId]["StandardwiseDocumentId"]);
            CheckBox oChkIsSubmitted = oCurrentItem.FindControl("ChkSelect") as CheckBox;
            CheckBox oChkIsApplicable = oCurrentItem.FindControl("chkIsApplicable") as CheckBox;
            
            oStudentDocument.StudentDocumentId = iStudentDocumentId;
            oStudentDocument.StandardwiseDocumentId = iStandardwiseDocumentId;
            oStudentDocument.IsSubmitted = oChkIsSubmitted.Checked;
            oStudentDocument.IsApplicable = oChkIsApplicable.Checked;
            lstDocumentInfo.Add(oStudentDocument);

        }
        return lstDocumentInfo;
    }
}





