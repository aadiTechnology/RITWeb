using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using System.IO;
using SchoolEntities;
using System.Data;

public partial class StudentAchievementPopup : SchoolBase
{
    #region Constants

    private const string S_DELETE_MESSAGE = "Student Achievement details deleted successfully !!!";
    private const string S_UPDATE_MESSAGE = "Student Achievement details updated successfully !!!";
    private const string S_SAVE_MESSAGE = "Student Achievement details saved successfully !!!";
    private const string S_UPDATE_TEXT = "Update";
    private const string S_SAVE_TEXT = "Save";
    private const string S_UPLOAD_ACHIEVEMENT_PATH = "\\DOWNLOADS\\StudentAchievement\\";
    private const int I_FILE_SIZE_LIMIT = 1048576; // nearly 1 mb
    private const string S_FILE_SIZE_ERROR_MESSAGE = "File size should not be greater than 1 MB.";

    #endregion

    #region DataMember

    private StudentAchievementBL moStudentAchievementBL;

    #endregion


    #region Events


    /// <summary>
    /// This event is used to set the page load events.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moStudentAchievementBL = new StudentAchievementBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                ReadQueryString();
                FillNoteCategories();
                SetJavascriptAttributes();
                GetStudentDetails();
                FillStudentAchievementDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to Save the students achievement details.
    /// </summary>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        bool sFilePath = ValidateFile();
        if (sFilePath == false)
        {
            DisplayMessage(S_FILE_SIZE_ERROR_MESSAGE, true, tdMessage);
        }
        else
        {
            StudentAchievement oStudentAchievement = Populate();
            moStudentAchievementBL.Save(oStudentAchievement);
            FillStudentAchievementDetails();
            if (oStudentAchievement.AchievementId == 0)
                base.DisplayMessage(S_SAVE_MESSAGE, false, tdMessage);
            else
                base.DisplayMessage(S_UPDATE_MESSAGE, false, tdMessage);
            ClearFields();
        }
    }

    /// <summary>
    /// This event is used to fill the data in listview.
    /// </summary>
    protected void lstvwStudentAchievement_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            int iStudentId = Convert.ToInt32(hidAchievementStudentId.Value);
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int iAchievementId = Convert.ToInt32(lstvwStudentAchievement.DataKeys[e.Item.DisplayIndex]["AchievementId"]);
                if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    btnSave.Text = S_UPDATE_TEXT;
                    StudentAchievement oStudentAchievement = moStudentAchievementBL.Get(iAchievementId, iStudentId);
                    hidAchievementId.Value = oStudentAchievement.AchievementId.ToString();
                    txtAchievementDate.Text = oStudentAchievement.AchievementDate.ToString(Constants.S_DATE_FORMAT);
                    txtDescription.Text = oStudentAchievement.Description.ToString();
                    hidAttachment.Value = oStudentAchievement.Attachment;
                }
                else if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    moStudentAchievementBL.Delete(iAchievementId, iStudentId);
                    FillStudentAchievementDetails();
                    base.DisplayMessage(S_DELETE_MESSAGE, false, tdMessage);
                    if (Convert.ToInt32(hidAchievementId.Value) == iAchievementId)
                    {
                        ClearFields();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to bound the data in listview.
    /// </summary>
    protected void lstvwStudentAchievement_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ImageButton imgbtnDelete = e.Item.FindControl("imgbtnDelete") as ImageButton;
                imgbtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");

                StudentAchievement oStudentAchievement = e.Item.DataItem as StudentAchievement;
                Label lblAchievementDate = e.Item.FindControl("lblAchievementDate") as Label;
                lblAchievementDate.Text = oStudentAchievement.AchievementDate.ToString(Constants.S_DATE_FORMAT);

                ImageButton imgbtnAttach = e.Item.FindControl("imgbtnAttach") as ImageButton;
                if (oStudentAchievement.Attachment != string.Empty)
                {
                    imgbtnAttach.Visible = true;
                    imgbtnAttach.Attributes.Add("onclick", "OpenFile('" + oStudentAchievement.Attachment + "')");
                }
                else
                    imgbtnAttach.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwStudentAchievement_SelectedIndexChanged(object sender, EventArgs e) { }

    /// <summary>
    /// This event is used to Deleting the items in listview.
    /// </summary>
    protected void lstvwStudentAchievement_ItemDeleting(object sender, ListViewDeleteEventArgs e) { }

    /// <summary>
    /// This event is used to Editing the items in listview.
    /// </summary>
    protected void lstvwStudentAchievement_ItemEditing(object sender, ListViewEditEventArgs e) { }

    /// <summary>
    /// This event is used to Cancel the save achievement process & clear fields.
    /// </summary>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearFields();
    }

    protected void cmbNoteCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillStudentAchievementDetails();
            ClearFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method

    /// <summary>
    /// This method is used to populate the data before save.
    /// </summary>
    private StudentAchievement Populate()
    {
        StudentAchievement oStudentAchievement = new StudentAchievement();
        oStudentAchievement.AchievementId = Convert.ToInt32(hidAchievementId.Value);
        oStudentAchievement.StudentId = Convert.ToInt32(hidAchievementStudentId.Value);
        oStudentAchievement.AchievementDate = Convert.ToDateTime(txtAchievementDate.Text);
        oStudentAchievement.Description = txtDescription.Text.Trim();
        oStudentAchievement.NoteCategoryId = cmbNoteCategory.SelectedValue.ToInt();
        if (FileUploadAchievement.HasFile)
        {
            oStudentAchievement.Attachment = SaveFileOnServer(FileUploadAchievement.FileName);
        }
        else
        {
            if (oStudentAchievement.AchievementId == 0)
                oStudentAchievement.Attachment = string.Empty;
            else
                oStudentAchievement.Attachment = hidAttachment.Value;
        }
        return oStudentAchievement;
    }

    /// <summary>
    /// This method is used to get name of file from server.
    /// </summary>
    private string SaveFileOnServer(string asFileName)
    {
        string sFolderName = Server.MapPath("..") + S_UPLOAD_ACHIEVEMENT_PATH;
        string sServerFilePath = sFolderName + asFileName;
        string sFileName = asFileName;

        if (File.Exists(sServerFilePath))
        {
            sFileName = CommonUtility.GetFileNameForRenaming(asFileName);
            sServerFilePath = sFolderName + sFileName;
        }
        FileUploadAchievement.SaveAs(sServerFilePath);
        return sFileName;
    }

    /// <summary>
    /// This method is used to check file size.
    /// </summary>
    private bool ValidateFile()
    {
        bool bIsValid = true;
        if (FileUploadAchievement.HasFile)
        {
            if (FileUploadAchievement.FileContent.Length > I_FILE_SIZE_LIMIT)
            {
                bIsValid = false;
            }
        }
        return bIsValid;
    }

    /// <summary>
    /// This method is used to fill lstvwStudentAchievementDetails.
    /// </summary>
    private void FillStudentAchievementDetails()
    {
        int iSchoolwiseStudentId = Convert.ToInt32(hidAchievementStudentId.Value);
        List<StudentAchievement> lstStudentAchievement = moStudentAchievementBL.GetAll(iSchoolwiseStudentId,cmbNoteCategory.SelectedValue.ToInt());
        lstvwStudentAchievement.DataSource = lstStudentAchievement;
        lstvwStudentAchievement.DataBind();
    }

    /// <summary>
    /// This method is used to set javascript attributes to controls.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        txtAchievementDate.Text = DateTime.Now.ToString(Constants.S_DATE_FORMAT);
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel, btnClose });
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
    }

    /// <summary>
    /// This method is used to read the query string.
    /// </summary>
    private void ReadQueryString()
    {
        hidAchievementStudentId.Value = QueryString["SchoolWiseStudentId"];
    }

    /// <summary>
    /// This method is used to clear the fields.
    /// </summary>
    private void ClearFields()
    {
        btnSave.Text = S_SAVE_TEXT;
        hidAchievementId.Value = Constants.S_ZERO;
        txtAchievementDate.Text = DateTime.Now.ToString(Constants.S_DATE_FORMAT);
        txtDescription.Text = string.Empty;
     }

    /// <summary>
    /// This method is used to get the student Name & Registration number to display.
    /// </summary>
    private void GetStudentDetails()
    {
        int iStudentId = Convert.ToInt32(hidAchievementStudentId.Value);
        StudentAchievement oStudentAchievement = moStudentAchievementBL.GetStudentDetails(iStudentId);
        lblStudentName.Text = Convert.ToString(oStudentAchievement.StudentName);
        lblRegistration.Text = Convert.ToString(oStudentAchievement.RegistrationNo);
    }

    private void FillNoteCategories()
    {
        DataTable dtNoteCategory = moStudentAchievementBL.GetNoteCategories();
        cmbNoteCategory.Bind(dtNoteCategory, "Id", "NoteCategory", string.Empty);
    }

    #endregion    
}