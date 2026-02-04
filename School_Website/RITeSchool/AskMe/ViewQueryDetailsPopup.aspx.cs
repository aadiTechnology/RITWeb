using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using SchoolEntities;
using Utility;

public partial class ViewQueryDetailsPopup : SchoolBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillCommunications();
            base.ApplyMouseHoverEffect(new List<Button> { btnClose });
        }
    }

    private void FillCommunications()
    {
        int iQuestionId = Convert.ToInt32(QueryString["QuestionId"]);
        AskMeQuestionMaster oAskMeQuestionMaster = AskMeQuestionMasterBL.GetQuestionDetails(miSchoolId, miAcademicYearId, 0, iQuestionId,miUserId);
        List<AskMeQuestionDetails> lstQuestionDetails = AskMeQuestionMasterBL.GetAllQuestionCommunications(miSchoolId, miAcademicYearId, iQuestionId, "Date", "Desc", 0, 9999, miUserId, Convert.ToBoolean(QueryString["IsPublishedView"].ToInt()));
        
        HtmlTableRow trTitle = new HtmlTableRow();
        this.AddTableCell(trTitle, "Query : " + oAskMeQuestionMaster.Title, "ClsProgressGridTestHeader","Left", 2);
        tblCommunications.Rows.Add(trTitle);

        HtmlTableRow trStartDate = new HtmlTableRow();
        this.AddTableCell(trStartDate, "Start Date : " + oAskMeQuestionMaster.CommunicationStartDate.ToString(Constants.S_DATE_FORMAT), "ClsProgressGridTestHeader", "Left", 2);
        tblCommunications.Rows.Add(trStartDate);

        AddEmptyRow();

        lstQuestionDetails.ForEach
            (
                question =>
                {   
                    string sName = "Sender : " + question.SenderName;
                    if (QueryString["IsPublishedView"] == Constants.S_ONE)
                        sName = "Sender";
                    string sHeaderClassName = "ClsProgressGridTestHeader";
                    string sCellClassName = "ClsMarksCell";
                    if (oAskMeQuestionMaster.StudentUserId != question.SenderUserId)
                    {
                        if (QueryString["IsPublishedView"] == Constants.S_ONE)
                            sName = "Receiver";
                        else
                            sName = "Receiver : " + question.SenderName;
                        sHeaderClassName = "ClsReceiverHeader";
                        sCellClassName = "ClsReceiverCell";
                    }

                    HtmlTableRow oHtmlTableRow = new HtmlTableRow();
                    this.AddTableCell(oHtmlTableRow, sName, sCellClassName, "Left", 2, "font-weight:bold;border-style:solid;border-width:1px;border-color:skyblue;color:Navy");
                    tblCommunications.Rows.Add(oHtmlTableRow);

                    oHtmlTableRow = new HtmlTableRow();
                    this.AddTableCell(oHtmlTableRow, "Date : " + question.Date.ToString(Constants.S_DATE_FORMAT + " hh:mm tt"), sHeaderClassName, "Left", 1, "width:50%;font-weight:bold");

                    LinkButton oLinkButton = new LinkButton();
                    if (!string.IsNullOrEmpty(question.AttachedFileName))
                    {   
                        oLinkButton.Text = "Download Attachment";
                        oLinkButton.OnClientClick = "OpenFile('" + question.AttachedFileName + "'); return false;";
                    }
                    this.AddTableCell(oHtmlTableRow, string.Empty, sHeaderClassName, "Left", 1, "font-weight:bold", oLinkButton);

                    tblCommunications.Rows.Add(oHtmlTableRow);

                    oHtmlTableRow = new HtmlTableRow();
                    this.AddTableCell(oHtmlTableRow, question.Comment, sCellClassName, "Left", 2, "border-style:solid;border-width:1px;border-color:skyblue");
                    tblCommunications.Rows.Add(oHtmlTableRow);

                    AddEmptyRow();
                }
            );
    }

    private void AddEmptyRow()
    {
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        this.AddTableCell(oHtmlTableRow, "<BR />", "ClsMarksCell", "Left", 2, "background-color:white");
        tblCommunications.Rows.Add(oHtmlTableRow);
    }

    /// <summary>
    /// This method is used to add cell into given row.
    /// </summary>
    /// <param name="aoHtmlTableRow"></param>
    /// <param name="asCaption"></param>
    /// <param name="asClass"></param>
    /// <param name="asAlign"></param>
    /// <param name="aiColSpan"></param>
    /// <param name="asStyles"></param>
    /// <param name="aoControl"></param>
    private void AddTableCell(HtmlTableRow aoHtmlTableRow, string asCaption, string asClass, string asAlign = "Center", int aiColSpan = 1, string asStyles = "", Control aoControl = null)
    {
        string[] stl;
        HtmlTableCell oHtmlTableCell = new HtmlTableCell { InnerHtml = asCaption, Align = asAlign, ColSpan = aiColSpan };
        oHtmlTableCell.Attributes.Add("class", asClass);
        if (aoControl != null)
            oHtmlTableCell.Controls.Add(aoControl);

        oHtmlTableCell.Style.Add("Padding-Left", "5pt");

        if (asStyles != string.Empty)
        {
            string[] sArrStyles = asStyles.Split(';');
            sArrStyles.ToList().ForEach
                (
                    style =>
                    {
                        if (style.Trim() != string.Empty)
                        {
                            stl = style.Split(':');
                            if (stl[0] != string.Empty && stl[1] != string.Empty)
                                oHtmlTableCell.Style.Add(stl[0], stl[1]);
                            stl = null;
                        }
                    });
        }

        aoHtmlTableRow.Cells.Add(oHtmlTableCell);
    }
}