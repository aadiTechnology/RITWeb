// File Name   : AddStudentDetails.aspx.cs
// Created By  : Madhuri S.
// Date        : 04/01/2017
// Description : This form is used to Add user's details.
// Modified By : -
// Date        : -

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SuperAdminEntities;
using System.Linq;
using Utility;
using System.Configuration;
using PushNotificationService;

///<Summary>
///This class is used to change user's password which is already exist.
///</Summary>
public partial class AddStudentDetails : SchoolBase
{
    private const string S_FOLDER_LOCATION = "RITeSchool\\DOWNLOADS\\Aadhar Cards\\";
    private const string S_FOLDER_PATH = @"../DOWNLOADS/Aadhar Cards/";
    private const string S_FOLDER_LOCATION1 = "RITeSchool\\DOWNLOADS\\Admission\\BirthCertificates\\";
    private const string S_FOLDER_PATH1 = @"../DOWNLOADS/Admission/BirthCertificates/";
    private const int I_FILE_SIZE_LIMIT = 1048576;
    private const string S_FILE_SIZE_ERROR = "File size should not be greater than 1 MB.";
    
    #region Events

    ///<Summary>
    ///This event is used to set default properties to page controls..
    ///</Summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
              HideControl();
              SetClientSideScriptAttributes();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    ///<Summary>
    ///This event is used to update user's password.
    ///</Summary>
    protected void imgBtnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
                string aadharFile, birthFile;
                CheckIsFileFileUploaded(out aadharFile, out birthFile);
                SchoolUserBL moSchoolUserBL = PopulateStudentBL(aadharFile, birthFile);
               
                moSchoolUserBL.UpdateStudentAadharNumber(moUserRole.ToInt());
                lblUpdateSucess.Visible = true;
                lblUpdateSucess.Text = "Aadhar Card details is submitted successfully!!!";
                lblUpdateSucess.ForeColor = System.Drawing.Color.Blue;
                lblUpdateSucess.Font.Bold = true;

            SetClientSideScriptAttributes();
        }
        catch (DuplicateUserException ex)
        {
            lblErrorMsg.Text = ex.Message;
            lblErrorMsg.Visible = true;            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

 

    #endregion Events 

    #region Private Methods


    private void HideControl()
     {
         if (miSchoolId == Constants.SchoolId.SNS.ToInt())
         {
             trBirthCertificate.Visible = true;
         }
         else
         {
             trBirthCertificate.Visible = false;
         }
     }

    /// <summary>
    /// This event is used to set java script properties to page controls.
    /// </summary>
    private void SetClientSideScriptAttributes()
    {
        txtLogin.Enabled = false;
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        imgBtnSubmit.Attributes.Add("onclick", "ResetErrLabel()");       

        SchoolUserBL moSchoolUserBL = new SchoolUserBL(miUserId, miSchoolId);

        txtLogin.Text = moSchoolUserBL.StudentName;
        txtAadharNumber.Text = moSchoolUserBL.AadharCardNo;
        txtNameOnAadharCard.Text = moSchoolUserBL.StudentNameOnAadharCard;
        txtMothertongue.Text = moSchoolUserBL.MotherTongue;
        txtEmail.Text = moSchoolUserBL.Email;
        
        ListItem oBloodGroup = ddlBloodGroup.Items.FindByValue(moSchoolUserBL.BloodGroup);
        if (oBloodGroup != null)
            oBloodGroup.Selected = true;

        if (moSchoolUserBL.AadharCard_Photo_Copy_Path != null && moSchoolUserBL.AadharCard_Photo_Copy_Path != string.Empty)
        {
            btnView.Visible = true;
            string sNewFileName = S_FOLDER_PATH + moSchoolUserBL.AadharCard_Photo_Copy_Path;
            hidAadharImage.Value = moSchoolUserBL.AadharCard_Photo_Copy_Path;
            btnView.Attributes.Add("onclick", " window.open('" + sNewFileName + "', '', 'popup_window', 'height=150, width=100, resizable=No'); return false;");
        }

        if (moSchoolUserBL.BirthCertificateScanCopyFileName != null && moSchoolUserBL.BirthCertificateScanCopyFileName != string.Empty)
        {
            btnViewBirthCert.Visible = true;
            string sNewFileName1 = S_FOLDER_PATH1 + moSchoolUserBL.BirthCertificateScanCopyFileName;
            hidBirthCertificate.Value = moSchoolUserBL.BirthCertificateScanCopyFileName;
            btnViewBirthCert.Attributes.Add("onclick", " window.open('" + sNewFileName1 + "', '', 'popup_window', 'height=150, width=100, resizable=No'); return false;");
        }
    }

    /// <summary>
    /// This method populates properties of SchoolUserBL and return its object.
    /// </summary>
    /// <returns>EventDescriptionBL</returns>
    /// 
    private SchoolUserBL PopulateStudentBL(string aadharFile, string birthFile)
    {
        SchoolUserBL oSchoolUserBL = new SchoolUserBL();
        oSchoolUserBL.AadharCard_Photo_Copy_Path = aadharFile;
        oSchoolUserBL.BirthCertificateScanCopyFileName = birthFile;
        oSchoolUserBL.AadharCardNo = txtAadharNumber.Text;
        oSchoolUserBL.StudentNameOnAadharCard = txtNameOnAadharCard.Text.Trim();
        oSchoolUserBL.MotherTongue = txtMothertongue.Text;
        oSchoolUserBL.Email = txtEmail.Text;
        oSchoolUserBL.UserId = miUserId;
        oSchoolUserBL.SchoolId = miSchoolId;
        oSchoolUserBL.BloodGroup = ddlBloodGroup.SelectedItem.Text;
        return oSchoolUserBL;
    }


  
    ///// <summary>
    ///// This method is used to check Is file Uploaded or not.
    ///// </summary>
    //private string CheckIsFileFileUploaded(out string asFileName)
    //{
    //    asFileName = string.Empty;
    //    if (fuAadharNumber.FileName != string.Empty)
    //    {
    //        string sServerPath = Server.MapPath("~");
    //        if (sServerPath.Substring(sServerPath.Length - 1) != "\\")
    //            sServerPath = sServerPath + "\\";
    //        string sLinkName = CommonUtility.GetFileNameForRenaming(fuAadharNumber.FileName.ToString());
    //        if (fuAadharNumber.HasFile)
    //        {
    //            string sFileName = fuAadharNumber.PostedFile.FileName;
    //            string sLinkPath = sServerPath + S_FOLDER_LOCATION + sLinkName;
    //            fuAadharNumber.SaveAs(sLinkPath);
    //            asFileName = sLinkName;
    //        }
    //    }
    //    if (asFileName == string.Empty)
    //        asFileName = hidAadharImage.Value;
    //    return string.Empty;
    //}

    private string CheckIsFileFileUploaded(out string aadharFileName, out string birthFileName)
    {
        aadharFileName = string.Empty;
        birthFileName = string.Empty;

        string sServerPath = Server.MapPath("~");
        if (sServerPath.Substring(sServerPath.Length - 1) != "\\")
            sServerPath = sServerPath + "\\";

        // Aadhar Upload
        if (fuAadharNumber.FileName != string.Empty && fuAadharNumber.HasFile)
        {
            string sAadharLinkName = CommonUtility.GetFileNameForRenaming(fuAadharNumber.FileName.ToString());
            string sAadharLinkPath = sServerPath + S_FOLDER_LOCATION + sAadharLinkName;
            fuAadharNumber.SaveAs(sAadharLinkPath);
            aadharFileName = sAadharLinkName;
        }
        else
        {
            aadharFileName = hidAadharImage.Value;
        }

        // Birth Certificate Upload
        if (fuBirthCertificate.FileName != string.Empty && fuBirthCertificate.HasFile)
        {
            string sBirthLinkName = CommonUtility.GetFileNameForRenaming(fuBirthCertificate.FileName.ToString());
            string sBirthLinkPath = sServerPath + S_FOLDER_LOCATION1 + sBirthLinkName;
            fuBirthCertificate.SaveAs(sBirthLinkPath);
            birthFileName = sBirthLinkName;
        }
        else
        {
            birthFileName = hidBirthCertificate.Value;
        }

        return string.Empty;
    }
   
    #endregion

}