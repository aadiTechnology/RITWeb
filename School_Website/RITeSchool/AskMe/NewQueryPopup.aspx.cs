using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollReportingUserEntities;
using SchoolEntities;
using Utility;

public partial class NewQueryPopup : SchoolBase
{
    #region Constant(s)
    
    private const string S_ASK_ME = "ASKME"; 

    #endregion

    #region Data Member(s)
    
    private AskMeQuestionMasterBL oAskMeQuestionMasterBL; 

    #endregion

    #region Event(s)
    
    /// <summary>
    /// This event is used to fill up categories and set field values.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            oAskMeQuestionMasterBL = new AskMeQuestionMasterBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                SetJavaScriptAttributes();                
                FillCategories();
                SetFieldState();
                InitlizeFields();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save communication.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {   
            Save();
            ScriptManager.RegisterStartupScript(btnSave, this.GetType(), "CloseWindow", "CloseWindow();", true);
        }
        catch (SqlException ex)
        {
            lblMessage.Text = ex.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }


    #endregion

    #region Data Member(s)

    /// <summary>
    /// This method is used to save query details.
    /// </summary>
    private void Save()
    {
        AskMeQuestionMaster oAskMeQuestionMaster = Populate();
        oAskMeQuestionMasterBL.SaveCommunicationDetails(oAskMeQuestionMaster);
    }
    
    /// <summary>
    /// This method is sued to populate AskMeQuestionMaster object.
    /// </summary>
    /// <returns></returns>
    private AskMeQuestionMaster Populate()
    {
        AskMeQuestionMaster oAskMeQuestionMaster = new AskMeQuestionMaster();
        oAskMeQuestionMaster.Id = Convert.ToInt32(hidQuestionId.Value);        
        oAskMeQuestionMaster.UserRoleId = hidUserRoleId.Value.ToInt();
        oAskMeQuestionMaster.OwnerUserId = hidOwnerUserId.Value.ToInt();

        oAskMeQuestionMaster.Title = txtTitle.Text.Trim();
        oAskMeQuestionMaster.LastUpdatedDate = DateTime.Now;
        oAskMeQuestionMaster.AskMeQuestionDetails = new AskMeQuestionDetails
        {
            AttachedFileName = SaveFileToServer(),
            Comment = txtDescription.Text.Trim(),
            Date = DateTime.Now,
            HasReadMessage = false,
            Id = hidQuestionDetailsId.Value.ToInt(),
            SenderUserId = hidSenderUserId.Value.ToString() == Constants.S_ZERO ? miUserId : hidSenderUserId.Value.ToInt()
        };

        StringBuilder oStringBuilder = new StringBuilder();
        for (int iItemCount = 0; iItemCount < chkCategoryLst.Items.Count; iItemCount++)
        {
            if (chkCategoryLst.Items[iItemCount].Selected)
                oStringBuilder.Append("," + chkCategoryLst.Items[iItemCount].Value);
        }

        oAskMeQuestionMaster.AssociatedCategories = string.Empty;
        if (oStringBuilder.Length > 0)
            oAskMeQuestionMaster.AssociatedCategories = oStringBuilder.ToString().Substring(1);
        
        return oAskMeQuestionMaster;
    }

    /// <summary>
    /// This method is used to save file on server.
    /// </summary>
    /// <returns></returns>
    private string SaveFileToServer()
    {
        string sFileName = flAttachment.FileName;
        if (sFileName.Trim() != string.Empty)
        {
            string sRenamedFileName = sFileName;
            string sFolderName = Server.MapPath("..") + "\\DOWNLOADS\\" + S_ASK_ME;
            string sServerFilePath = sFolderName + "\\" + sFileName;

            if (File.Exists(sServerFilePath))
                sFileName = CommonUtility.GetFileNameForRenaming(sFileName);

            sServerFilePath = sFolderName + "\\" + sRenamedFileName;
            flAttachment.SaveAs(sServerFilePath);
            return sFileName;
        }
        else
            return hidAttachedFileName.Value;
    }

    /// <summary>
    /// This method is used to set java script functions.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnSave, btnClose});
        hidQuestionDetailsId.Value = QueryString["QuestionDetailsId"].ToString();
        hidQuestionId.Value = QueryString["QuestionId"].ToString();
        txtDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
    }

    /// <summary>
    /// This method is used to set field state.
    /// </summary>
    private void SetFieldState()
    {
        ReportingUserConfigurationBL oReportingUserConfigurationBL = new ReportingUserConfigurationBL(miSchoolId, miAcademicYearId, miUserId);
        List<ReportingUserConfiguration> lstUsers = oReportingUserConfigurationBL.GetAll();
        
        if (lstUsers.FindAll(ru => ru.ReportingPrameterId == Constants.ReportingParameters.Moderator.ToInt() && ru.UserId == miUserId).Any())        
        {            
            hidIsModerator.Value = Constants.S_ONE;
        }
    }

    /// <summary>
    /// This method is used to initialize fields.
    /// </summary>
    private void InitlizeFields()
    {
        int iQuestionDetailsId = Convert.ToInt32(hidQuestionDetailsId.Value);
        int iQuestionId = Convert.ToInt32(hidQuestionId.Value);
        AskMeQuestionMaster oAskMeQuestionMaster = AskMeQuestionMasterBL.GetQuestionDetails(miSchoolId, miAcademicYearId, iQuestionDetailsId, iQuestionId, miUserId);
        if (oAskMeQuestionMaster != null && oAskMeQuestionMaster.AskMeQuestionDetails != null)
        {
            txtDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
            txtDescription.Text = oAskMeQuestionMaster.AskMeQuestionDetails.Comment;
            txtTitle.Text = oAskMeQuestionMaster.Title;
           
            hidUserRoleId.Value = oAskMeQuestionMaster.UserRoleId.ToString();
            hidOwnerUserId.Value = oAskMeQuestionMaster.OwnerUserId.ToString();
            hidSenderUserId.Value = oAskMeQuestionMaster.AskMeQuestionDetails.SenderUserId.ToString();
            hidAttachedFileName.Value = oAskMeQuestionMaster.AskMeQuestionDetails.AttachedFileName;

            if (oAskMeQuestionMaster.StudentUserId == miUserId)
                txtTitle.ReadOnly = false;

            if (QueryString["IsReply"] == Constants.S_ONE)
            {
                txtTitle.ReadOnly = true;
                txtDescription.Text = string.Empty;
                hidQuestionDetailsId.Value = Constants.S_ZERO;
                hidSenderUserId.Value = Constants.S_ZERO;
                hidAttachedFileName.Value = string.Empty;
            }

            hidIsCommunicationStarted.Value = (oAskMeQuestionMaster.IsCommunicationStarted || oAskMeQuestionMaster.OwnerUserId == miUserId) ? Constants.S_ONE : Constants.S_ZERO;

            if (oAskMeQuestionMaster.IsCommunicationStarted)
                txtTitle.ReadOnly = true;

            if (!string.IsNullOrEmpty(oAskMeQuestionMaster.AssociatedCategories))
            {
                List<string> lstCategoryids = new List<string>();
                lstCategoryids = oAskMeQuestionMaster.AssociatedCategories.Split(',').ToList();
                for (int iItemCount = 0; iItemCount < chkCategoryLst.Items.Count; iItemCount++)
                {
                    if (lstCategoryids.Contains(chkCategoryLst.Items[iItemCount].Value))
                        chkCategoryLst.Items[iItemCount].Selected = true;                        
                }
            }

            if (oAskMeQuestionMaster.IsCategoryEnabled == true)
                chkCategoryLst.Enabled = true;
            else
                chkCategoryLst.Enabled = false;
        }
    }

    /// <summary>
    /// this method is used to fill category check list box.
    /// </summary>
    private void FillCategories()
    {
        List<AskMeCategory> lstCategories = oAskMeQuestionMasterBL.GetAllCategories();
        ListSource.FillCheckBoxList(lstCategories, chkCategoryLst, "Name", "Id");
    }

    #endregion   
}