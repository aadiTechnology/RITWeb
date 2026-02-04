using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;

public partial class AchievementDetailsUI : SchoolBase
{
    #region Constants

    private const string S_SAVE_STATEMENT = "Achievement details saved successfully !!!";
    private const string S_UPDATE_STATEMENT = "Achievement details updated successfully !!!";
    private const string S_DELETE_MSG = "Achievement details deleted successfully !!!";
    private const string S_FOLDER_LOCATION = "RITeSchool\\DOWNLOADS\\Achievements\\";
    private const string S_FOLDER_PATH = @"../DOWNLOADS/Achievements/";
    private const string S_FILE_SIZE_ERROR = "Size of file is too large.";
    private const int I_FILE_SIZE_LIMIT = 256000;  // File limit is 500 KB 

    #endregion

    #region Data Members

    private StudentAchievementBL moStudentAchievementBL;

    #endregion

    #region Event(s)

    /// <summary>
    /// this method is used to initialize the controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moStudentAchievementBL = new StudentAchievementBL(miSchoolId, miAcademicYearId, miUserId);
            if (!Page.IsPostBack)
            {
                FillAchievementDetailsGrid();
                SetJavascriptAttributes();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save Achievement details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            int iAchievementd = 0;
            if (hidId.Value != string.Empty)
                iAchievementd = Convert.ToInt32(hidId.Value);

            string sPhotoXML = string.Empty;
            AchievementDetails oAchievementDetails = Populate(iAchievementd);
            string sXml = base.GenerateXml(oAchievementDetails);

            string sErrorMsg = UploadImages();
            if (sErrorMsg == string.Empty)
            {
                sPhotoXML = GetUploadFilesXML();
                moStudentAchievementBL.SaveAchievementDetails(sXml, sPhotoXML);
            }
            else
                base.DisplayMessage(sErrorMsg, true, tdMessage);

            FillAchievementDetailsGrid();
            if (iAchievementd == Constants.I_ZERO)
                base.DisplayMessage(S_SAVE_STATEMENT, false, tdMessage);
            else
            {
                base.DisplayMessage(S_UPDATE_STATEMENT, false, tdMessage);
                btnSave.Text = Constants.ButtonText.Save.ToString(); ;
            }
            ClearFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is called while cancelling Save/Update operation.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancelText_Click(object sender, EventArgs e)
    {
        try
        {
            ClearFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// this event is called while row in list view is clicked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwAchievements_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;

            int iAchievementId = Convert.ToInt32(lstvwAchievements.DataKeys[oCurrentItem.DisplayIndex]["Id"]);
            if (e.CommandName == Constants.S_COMMAND_UPDATE)
                SetEditMode(iAchievementId);
            if (e.CommandName == Constants.S_COMMAND_REMOVE)
            {
                Delete(iAchievementId);

                if (hidId.Value == iAchievementId.ToString())
                    ClearFields();

                FillAchievementDetailsGrid();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used while loading rows in listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwAchievements_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                AchievementDetails oAchievementDetails = e.Item.DataItem as AchievementDetails;
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                ImageButton imgBtnDelete = oCurrentItem.FindControl("imgBtnDelete") as ImageButton;
                imgBtnDelete.Attributes.Add("Onclick", "if(!ConfirmDelete()) {return false;}");

                Image imgHomePage = oCurrentItem.FindControl("imgHomePage") as Image;
                if (!oAchievementDetails.IsSelected)
                    imgHomePage.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This event is used to remove image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgbtnDelete1_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            btnView1.Visible = false;
            imgbtnDelete1.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to remove image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgbtnDelete2_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            btnView2.Visible = false;
            imgbtnDelete2.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to remove image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgbtnDelete3_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            btnView3.Visible = false;
            imgbtnDelete3.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to remove image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgbtnDelete4_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            btnView4.Visible = false;
            imgbtnDelete4.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to remove image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgbtnDelete5_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            btnView5.Visible = false;
            imgbtnDelete5.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// this method used to populate achievement deatils
    /// </summary>
    /// <param name="aiAchievementd"></param>
    /// <returns></returns>
    private AchievementDetails Populate(int aiAchievementd)
    {
        var oAchievementDetails = new AchievementDetails
        {
            Id = aiAchievementd,
            AchievementTitle = txtTitle.Text.Trim(),
            Description = txtDescription.Text.Trim(),
            IsSelected = chkDisplayOnHomepage.Checked
        };
        return oAchievementDetails;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private string GetUploadFilesXML()
    {
        ImageGalleryBL oImageGalleryBL = new ImageGalleryBL();
        const string S_ELEMENT = "element";
        XmlDocument oXMLDoc = new XmlDocument();

        // Create a root level element.
        XmlElement oXMLRootElement = oXMLDoc.CreateElement("PhotoGallery");
        XmlNode oXmlRootNode = oXMLDoc.CreateNode(S_ELEMENT, "PhotoGallery", string.Empty);
        XmlNode oXmlNode = oXMLDoc.CreateNode(S_ELEMENT, "PhotoGallery", string.Empty);

        // Create distinct names for the images. Logic - <Sr_No>_<file name>
        // Save the files images\<gallery name> folder.
        for (int iCount = 1; iCount <= 5; iCount++)
        {
            string sFileName = string.Empty;
            FileUpload oUpload = (FileUpload)this.FindControl("ctl00$MainBody$flImage" + iCount);
            sFileName = oUpload.FileName;

            if (!oUpload.FileName.Trim().Equals(string.Empty))
            {
                sFileName = CommonUtility.GetFileNameForRenaming(oUpload.FileName);
                oUpload.SaveAs(Server.MapPath("~") + S_FOLDER_LOCATION + sFileName);
            }

            oXmlNode = oXMLDoc.CreateNode(S_ELEMENT, "PhotoGallery", string.Empty);

            string sAtrrName = "FieldIndex";
            XmlAttribute attr = oXMLDoc.CreateAttribute(sAtrrName);
            attr.Value = iCount.ToString();
            oXmlNode.Attributes.Append(attr);

            sAtrrName = "IsDeleted";
            attr = oXMLDoc.CreateAttribute(sAtrrName);
            attr.Value = sFileName.TrimAll() != string.Empty ? Constants.S_ZERO : IsFileDeleted(iCount);
            oXmlNode.Attributes.Append(attr);

            sAtrrName = "Image_Path";
            attr = oXMLDoc.CreateAttribute(sAtrrName);
            attr.Value = sFileName;
            oXmlNode.Attributes.Append(attr);

            // Add the node to root node.
            oXmlRootNode.AppendChild(oXmlNode);
            //}
        }
        // Add the root node to document element. 
        oXMLRootElement.AppendChild(oXmlRootNode);
        return oXMLRootElement.InnerXml;
    }

    /// <summary>
    /// This method is used to check file delete status.
    /// </summary>
    /// <param name="aiCount"></param>
    /// <returns></returns>
    private string IsFileDeleted(int aiCount)
    {
        string sIsDeleted = Constants.S_ZERO;
        switch (aiCount)
        {
            case 1: if (!btnView1.Visible)
                    sIsDeleted = Constants.S_ONE;
                break;
            case 2: if (!btnView2.Visible)
                    sIsDeleted = Constants.S_ONE;
                break;
            case 3: if (!btnView3.Visible)
                    sIsDeleted = Constants.S_ONE;
                break;
            case 4: if (!btnView4.Visible)
                    sIsDeleted = Constants.S_ONE;
                break;
            case 5: if (!btnView5.Visible)
                    sIsDeleted = Constants.S_ONE;
                break;
        }
        return sIsDeleted;
    }

    /// <summary>
    /// this method is used to set default values to control.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        valSumErrorMsgText.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        new Button[] { btnCancelText, btnSave }.ApplyEffect();
        btnSave.Attributes.Add("onclick", "ClearLabels()");
    }

    /// <summary>
    /// this method is used to fill Achievement ListView.
    /// </summary>
    private void FillAchievementDetailsGrid()
    {
        List<Images> lstImagePath;
        List<AchievementDetails> lstAchievementDetails = moStudentAchievementBL.GetAchievementDetails(miSchoolId, out lstImagePath, 0);

        lstAchievementDetails = lstAchievementDetails.OrderByDescending(ad => ad.Id).ToList();

        lstvwAchievements.DataSource = lstAchievementDetails;
        lstvwAchievements.DataBind();
    }

    /// <summary>
    /// This method is used to set default control fields.
    /// </summary>
    private void ClearFields()
    {
        hidId.Value = string.Empty;
        txtTitle.Text = string.Empty;
        txtDescription.Text = string.Empty;
        chkDisplayOnHomepage.Checked = false;
        btnSave.Text = Constants.ButtonText.Save.ToString();
        btnView1.Visible = false;
        imgbtnDelete1.Visible = false;
        btnView2.Visible = false;
        imgbtnDelete2.Visible = false;
        btnView3.Visible = false;
        imgbtnDelete3.Visible = false;
        btnView4.Visible = false;
        imgbtnDelete4.Visible = false;
        btnView5.Visible = false;
        imgbtnDelete5.Visible = false;
    }

    /// <summary>
    /// This method is used to set values to controls in edit mode.
    /// </summary>
    /// <param name="oCurrentItem"></param>
    /// <param name="aiAchievementId"></param>
    private void SetEditMode(int aiAchievementId)
    {
        List<Images> lstImagePath;
        List<AchievementDetails> lstAchievementDetails = moStudentAchievementBL.GetAchievementDetails(miSchoolId, out lstImagePath, aiAchievementId);

        txtTitle.Text = lstAchievementDetails[0].AchievementTitle;
        txtDescription.Text = lstAchievementDetails[0].Description;
        chkDisplayOnHomepage.Checked = lstAchievementDetails[0].IsSelected;

        var oFirstField = lstImagePath.Where(ip => ip.FieldIndex == 1).FirstOrDefault();
        if (oFirstField != null)
        {
            btnView1.Visible = true;
            imgbtnDelete1.Visible = true;
            btnView1.Attributes.Add("onclick", "OpenPopup('" + S_FOLDER_PATH + oFirstField.ImagePath + "');return false;");
        }

        var oSecondField = lstImagePath.Where(ip => ip.FieldIndex == 2).FirstOrDefault();
        if (oSecondField != null)
        {
            btnView2.Visible = true;
            imgbtnDelete2.Visible = true;
            btnView2.Attributes.Add("onclick", "OpenPopup('" + S_FOLDER_PATH + oSecondField.ImagePath + "');return false;");
        }

        var oThirdField = lstImagePath.Where(ip => ip.FieldIndex == 3).FirstOrDefault();
        if (oThirdField != null)
        {
            btnView3.Visible = true;
            imgbtnDelete3.Visible = true;
            btnView3.Attributes.Add("onclick", "OpenPopup('" + S_FOLDER_PATH + oThirdField.ImagePath + "');return false;");
        }

        var oForthField = lstImagePath.Where(ip => ip.FieldIndex == 4).FirstOrDefault();
        if (oForthField != null)
        {
            btnView4.Visible = true;
            imgbtnDelete4.Visible = true;
            btnView4.Attributes.Add("onclick", "OpenPopup('" + S_FOLDER_PATH + oForthField.ImagePath + "');return false;");
        }

        var oFifthField = lstImagePath.Where(ip => ip.FieldIndex == 5).FirstOrDefault();
        if (oFifthField != null)
        {
            btnView5.Visible = true;
            imgbtnDelete5.Visible = true;
            btnView5.Attributes.Add("onclick", "OpenPopup('" + S_FOLDER_PATH + oFifthField.ImagePath + "');return false;");
        }

        btnSave.Text = Constants.ButtonText.Update.ToString();
        hidId.Value = aiAchievementId.ToString();

    }

    /// <summary>
    /// this method is used to delete achievement details.
    /// </summary>
    /// <param name="aiAchievementId"></param>
    /// <param name="oCurrentItem"></param>
    private void Delete(int aiAchievementId)
    {
        moStudentAchievementBL.DeleteAchievementDetails(aiAchievementId);
        base.DisplayMessage(S_DELETE_MSG, false, tdMessage);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private string UploadImages()
    {
        string sReturnErrorMsg = string.Empty;

        for (int iCount = 1; iCount <= 5; iCount++)
        {
            FileUpload oUpload = (FileUpload)this.FindControl("ctl00$MainBody$flImage" + iCount);
            string sFileName = oUpload.FileName;
            if (sFileName != string.Empty)
            {
                if (oUpload.HasFile)
                {
                    // Check for File size
                    if (oUpload.PostedFile.ContentLength > I_FILE_SIZE_LIMIT)
                        sReturnErrorMsg = S_FILE_SIZE_ERROR;
                }
            }
        }
        return sReturnErrorMsg;
    }

    #endregion
}