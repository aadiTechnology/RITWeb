using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using SchoolEntities;
using Utility;
using System.Web.UI.HtmlControls;

public partial class HomeAdditionalAttachmentPopUp : SchoolBase
{
    #region Data Member(s)

    private HomeWorkBL moHomeworkBL;
    private const string S_Homework_DOCUMENT_FOLDER_LOCATION = "\\DOWNLOADS\\Homework\\";    

    #endregion

    #region Event(s)

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moHomeworkBL = new HomeWorkBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                hidHomeworkId.Value = Convert.ToString(QueryString["HomeworkId"]);
                FillDocumentsLisView();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set attributes on listview child controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwDocuments_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                Homework oHomework = oCurrentItem.DataItem as Homework;
                Label lblFileName = oCurrentItem.FindControl("lblFileName") as Label;
                ImageButton oimgbtnDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
                oimgbtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete(" + (oHomework.HasLinkedHomework?1:0) + ")) {return false;}");
               
                List<string> lstExtensions = new List<string>{".JPG",".JPEG",".BMP",".PNG"};
                string sExtension = oHomework.AttachmentsName.Substring(oHomework.AttachmentsName.LastIndexOf("."));
                ImageButton imgFile = e.Item.FindControl("imgFile") as ImageButton;
                LinkButton lnkFile = e.Item.FindControl("lnkFile") as LinkButton;
                if (lstExtensions.Contains(sExtension))
                {
                    imgFile.Visible = true;
                    lnkFile.Visible = false;
                    imgFile.ImageUrl = "../DOWNLOADS/Homework/" + oHomework.AttachmentsName;
                    imgFile.Attributes.Add("onclick", "OpenFile('" + imgFile.ImageUrl + "'); return false;");
                }
                else
                {
                    imgFile.Visible = false;
                    lnkFile.Visible = true;

                    string sAttachmentName = oHomework.AttachmentsName;
                    if (sAttachmentName.IndexOf("$") > 0)
                        sAttachmentName = sAttachmentName.Substring(0, sAttachmentName.IndexOf("$")) + sAttachmentName.Substring(sAttachmentName.LastIndexOf("."));
                    lnkFile.Text = sAttachmentName;

                    lnkFile.Attributes.Add("onclick", "OpenFile('" + "../DOWNLOADS/Homework/" + oHomework.AttachmentsName + "'); return false;");
                }

                if (moUserRole == Constants.UserRoles.Student)
                {
                    HtmlTableCell oHtmlTableRow = e.Item.FindControl("tdDelete") as HtmlTableCell;
                    if (oHtmlTableRow != null)
                        oHtmlTableRow.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to remove documet.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwDocuments_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    int iId = Convert.ToInt32(lstvwDocuments.DataKeys[e.Item.DisplayIndex]["Id"]);
                    string sFileNameToDelete = moHomeworkBL.DeleteDocument(iId, hidDeleteFromAll.Value);
                    DisplayMessage("deleted");

                    if (sFileNameToDelete != string.Empty)
                    {
                        string sServerFilePath = Server.MapPath("..") + S_Homework_DOCUMENT_FOLDER_LOCATION + sFileNameToDelete;

                        if (File.Exists(sServerFilePath))
                            File.Delete(sServerFilePath);
                    }

                    FillDocumentsLisView();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    } 

    #endregion


    #region Method(s)

    /// <summary>
    /// This method is used to fill documents in listview.
    /// </summary>
    private void FillDocumentsLisView()
    {
        int iHomeworkId = Convert.ToInt32(QueryString["HomeworkId"]);
        List<Homework> lstDocuments = moHomeworkBL.GetDocuments(iHomeworkId, miAcademicYearId);
        lstvwDocuments.DataSource = lstDocuments;
        lstvwDocuments.DataBind();

        if (moUserRole == Constants.UserRoles.Student)
        {
            HtmlTableCell thDelete = lstvwDocuments.FindControl("thDelete") as HtmlTableCell;
            if (thDelete != null)
                thDelete.Visible = false;
        }
    }

    /// <summary>
    /// This method is used to display message.
    /// </summary>
    /// <param name="asMessage"></param>
    private void DisplayMessage(string asMessage)
    {
        string sMessage = "Document " + asMessage + " successfully !!!";
        base.DisplayMessage(sMessage, false, tdMessage);
    } 

    #endregion
}