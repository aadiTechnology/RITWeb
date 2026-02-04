// File Name  : TeacherPhotoUI.aspx.cs
// Created By : DEEPAK
// Created Date : 21/6/2010
//Class Description : This class is used to generate I card for Teachers.

using System;
using System.Data;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class TeacherIdentityCardUI : SchoolBase
{
    #region "Contant"

    protected int I_DB_TABLE_INDEX_HEADER = 2;

    #endregion

    #region "Data Members"

    protected DataSet moIdentityDetails;
    private int miTeacherId = 0;
    private string msTeacherName = "";

    #endregion

    #region "Event"

    /// <summary>
    /// Used to initalise page controls
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            ReadQueryString();
            GetTeachersForIdentityCards();
            if (moIdentityDetails != null)
                GenerateIdentityCardsControls();
        }
        catch (BusinessLogic.Exceptions.NoResultFound)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region "Methods"

    /// <summary>
    /// Used to create the I card for all teachers.
    /// </summary>
    /// <param name="oMainHtmlTable"></param>
    private void GenerateIdentityCardsControls()
    {
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        GridViewContainer.Rows.Add(oHtmlTableRow);

        HtmlTableCell oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.VAlign = "top";
        oHtmlTableRow.Cells.Add(oHtmlTableCell);

        HtmlTable oMainHtmlTable = new HtmlTable();
        oMainHtmlTable.Border = 0;
        oHtmlTableCell.Controls.Add(oMainHtmlTable);
        oHtmlTableRow = new HtmlTableRow();
        oMainHtmlTable.CellSpacing = 1;
        for (int iTeacherCnt = 0; iTeacherCnt < moIdentityDetails.Tables[1].Rows.Count; iTeacherCnt++)
        {
            if ((iTeacherCnt % 2) == 0)
            {
                oHtmlTableRow = new HtmlTableRow();
                oMainHtmlTable.Rows.Add(oHtmlTableRow);
                oHtmlTableCell = new HtmlTableCell();
                oHtmlTableCell.VAlign = "top";
                oHtmlTableCell.InnerHtml = "&nbsp;";
                oHtmlTableCell.ColSpan = 4;
                oHtmlTableRow.Cells.Add(oHtmlTableCell);
                oHtmlTableRow.Height = "28px";

                oHtmlTableRow = new HtmlTableRow();
                oMainHtmlTable.Rows.Add(oHtmlTableRow);
            }

            oHtmlTableCell = new HtmlTableCell();
            oHtmlTableCell.VAlign = "top";
            oHtmlTableCell.InnerHtml = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            oHtmlTableCell.Width = "55px";
            oHtmlTableRow.Cells.Add(oHtmlTableCell);

            oHtmlTableCell = new HtmlTableCell();
            oHtmlTableCell.VAlign = "top";
            oHtmlTableRow.Cells.Add(oHtmlTableCell);

            HtmlTable oIdentityHtmlTable = CreateHdTable();
            oHtmlTableCell.Controls.Add(oIdentityHtmlTable);
            CreateICard(oIdentityHtmlTable, iTeacherCnt);
        }
    }

    /// <summary>
    /// This function outputs response to doc.
    /// </summary>
    private void OutPutResponseToDoc()
    {
        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=FileName.doc");
        Response.Charset = "";
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/vnd.word";

        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);

        GridViewContainer.RenderControl(htmlWrite);
        Response.Write(stringWrite.ToString());
        Response.End();
    }
    /// <summary>
    /// This method is used to create I card for teacher.
    /// </summary>
    /// <param name="oMainHtmlTable"></param>
    /// <param name="iTeacherCnt"></param>

    private void CreateICard(HtmlTable oMainHtmlTable, int iTeacherCnt)
    {
        DataRow oDataRow = moIdentityDetails.Tables[0].Rows[0];
        string sCity = Convert.ToString(oDataRow["City"]);
        string sPincode = Convert.ToString(oDataRow["Pincode"]);
        string sAddress = Convert.ToString(oDataRow["Address"]) + ", " ;
        string sPhoneNumber = sCity + " - " + sPincode+", " +"Tel. : " + Convert.ToString(oDataRow["Phone_Number"]);
        DataRow oDataRowTeacher = moIdentityDetails.Tables[1].Rows[iTeacherCnt];
        string sTeacherName = Convert.ToString(oDataRowTeacher["Salutation_Name"])+" " + Convert.ToString(oDataRowTeacher["First_Name"]) + "  " + Convert.ToString(oDataRowTeacher["Last_Name"]);
        string sPhotoPath = Convert.ToString(oDataRowTeacher["Photo_file_Path"]);
        string sSchoolName = Convert.ToString(oDataRow["School_Name"]).ToUpper();
        string sDesignation = Convert.ToString(oDataRowTeacher["Teacher_Designation_Name"]);

        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        oMainHtmlTable.Rows.Add(oHtmlTableRow);
        HtmlTableCell oHtmlTableCell = new HtmlTableCell();
        Image oImage = new Image();
        oHtmlTableCell.Width = "20%";
        oHtmlTableCell.Height = "20px";
        oImage.ImageUrl = ".." + Convert.ToString(oDataRow["ICardLogo"]);
        oImage.Style.Add("center", "0");
        oImage.Style.Add("height", "55px");
        oImage.Style.Add("width", "90px");
        oHtmlTableCell.Controls.Add(oImage);
        oHtmlTableCell.Attributes.Add("colspan", "3");
        oHtmlTableCell.Attributes.Add("bgcolor", "white");
        oHtmlTableCell.Align = HorizontalAlign.Center.ToString();
        oHtmlTableRow.Cells.Add(oHtmlTableCell);

        oHtmlTableRow = new HtmlTableRow();
        oMainHtmlTable.Rows.Add(oHtmlTableRow);
        CreateHtmlCell(oHtmlTableRow, sSchoolName, "SchoolHeadTeacherID  ", 0, 3);
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Align = HorizontalAlign.Center.ToString();
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].VAlign = VerticalAlign.Bottom.ToString();
        oHtmlTableRow.Height = "7px";

        oHtmlTableRow = new HtmlTableRow();
        oMainHtmlTable.Rows.Add(oHtmlTableRow);
        oHtmlTableCell = new HtmlTableCell();
        Image oTeacherImage = new Image();
        oTeacherImage.Height = Unit.Pixel(77);
        oTeacherImage.Width = Unit.Pixel(64);
        if (sPhotoPath != string.Empty)
            oTeacherImage.ImageUrl = ".." + sPhotoPath.Replace("\\", "/");
        else
            oTeacherImage.ImageUrl = "../images/StudentSml_phBDay.gif";
        oTeacherImage.Style.Add("center", "0");
        oTeacherImage.BorderStyle = BorderStyle.Solid;
        oTeacherImage.BorderWidth = Unit.Pixel(2);
        oHtmlTableCell.Controls.Add(oTeacherImage);
        oHtmlTableCell.VAlign = "top";
        oHtmlTableCell.Style.Add("center", "0");
        oHtmlTableCell.Attributes.Add("colspan", "3");
        oHtmlTableRow.Cells.Add(oHtmlTableCell);
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Align = HorizontalAlign.Center.ToString();
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Style["padding-top"] = "2px";

        oHtmlTableRow = new HtmlTableRow();
        oMainHtmlTable.Rows.Add(oHtmlTableRow);
        CreateHtmlCell(oHtmlTableRow, sTeacherName.ToUpper(), "TeacherNameID", 0, 3);
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Style["padding-top"] = "2px";
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].VAlign = VerticalAlign.Top.ToString();
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Align = HorizontalAlign.Center.ToString();
        
        oHtmlTableRow = new HtmlTableRow();
        oMainHtmlTable.Rows.Add(oHtmlTableRow);
        CreateHtmlCell(oHtmlTableRow, sDesignation, "TeacherDesigNameID  ", 0, 3);
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Align = HorizontalAlign.Center.ToString();
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].VAlign = VerticalAlign.Bottom.ToString();
        oHtmlTableRow.Height = "7px";

        oHtmlTableRow = new HtmlTableRow();
        oMainHtmlTable.Rows.Add(oHtmlTableRow);
        oHtmlTableCell = new HtmlTableCell();
        oImage = new Image();
        oImage.ImageUrl = ".." + Convert.ToString(moIdentityDetails.Tables[0].Rows[0]["SignPath"]);
        oHtmlTableCell.Controls.Add(oImage);
        oHtmlTableCell.Controls.Add(new LiteralControl("<BR/>"));
        Label oLabel = new Label();
        oLabel.Text = "Authorised Sign.";
        oLabel.CssClass = "SchoolAddID";
        oHtmlTableCell.Controls.Add(oLabel);
        oHtmlTableCell.Controls.Add(new LiteralControl("<BR/>"));
        Label oLabel1 = new Label();
        oLabel1.Text = "______________________________________";
        oLabel1.CssClass = "SchoolAddID";
        oHtmlTableCell.Controls.Add(oLabel1);
        oHtmlTableCell.ColSpan = 3;
        oImage.Width = Unit.Pixel(65);
        oImage.Height = Unit.Pixel(23);
        oHtmlTableCell.VAlign = "bottom";
        
        oHtmlTableRow.Cells.Add(oHtmlTableCell);
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Align = HorizontalAlign.Center.ToString();
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Style["padding-bottom"] = "0px";
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Style["padding-top"] = "9px"; 
        
        oHtmlTableRow = new HtmlTableRow();
        oMainHtmlTable.Rows.Add(oHtmlTableRow);
        CreateHtmlCell(oHtmlTableRow, sAddress, "SchoolAddID", 0, 3);
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Style["vertical-align"] = "top";
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Align = HorizontalAlign.Center.ToString();

        oHtmlTableRow = new HtmlTableRow();
        oMainHtmlTable.Rows.Add(oHtmlTableRow);
        CreateHtmlCell(oHtmlTableRow, sPhoneNumber + "<BR/>" + " e-mail: info@ppspune.com", " SchoolAddID ", 0, 3);
        oHtmlTableRow.Height = "10px";
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Align = HorizontalAlign.Center.ToString();
    }

    /// <summary>
    /// This methos is used to set I card size.
    /// 
    /// </summary>
    protected HtmlTable CreateHdTable()
    {
        HtmlTable HeaderHtmlTable = new HtmlTable();
        HeaderHtmlTable.EnableViewState = false;
        HeaderHtmlTable.CellPadding = 0;
        HeaderHtmlTable.CellSpacing = 1;
        HeaderHtmlTable.Attributes.Add("class", "IDtblBorder");
        HeaderHtmlTable.Height = "270px";
        HeaderHtmlTable.Width = "196px";
        HeaderHtmlTable.Border =0;
        HeaderHtmlTable.Attributes.Add("bgcolor ", "white");
        HeaderHtmlTable.Style.Add(HtmlTextWriterStyle.VerticalAlign, HtmlTextWriterStyle.Top.ToString());
        HeaderHtmlTable.Style.Add(HtmlTextWriterStyle.Left, Unit.Pixel(0).ToString());
        return HeaderHtmlTable;
    }

    /// <summary>
    /// This method is used to get cell's for the I card.
    /// </summary>
    protected void CreateHtmlCell(HtmlTableRow oHtmlTableRow, String sInnerText, String sClassName, int iRowSpan, int iColSpan)
    {
        HtmlTableCell oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.InnerHtml = sInnerText;
        oHtmlTableCell.Attributes.Add("rowspan", iRowSpan.ToString());
        oHtmlTableCell.Attributes.Add("colspan", iColSpan.ToString());
        oHtmlTableCell.Attributes.Add("class", sClassName);
        oHtmlTableCell.Attributes.Add("align", "center");
        oHtmlTableCell.Align = HorizontalAlign.Center.ToString();
        oHtmlTableRow.Cells.Add(oHtmlTableCell);
        oHtmlTableCell.Dispose();
    }

    /// <summary>
    /// This method is used to get resultset for the I card.
    /// </summary>
    protected void GetTeachersForIdentityCards()
    {
        SchoolWiseTeacherMasterBL oSchoolWiseTeacherMasterBL = new SchoolWiseTeacherMasterBL();
        moIdentityDetails = oSchoolWiseTeacherMasterBL.getTeacherIdentityCards(miSchoolId,miAcademicYearId,miTeacherId,msTeacherName);
    }

    /// <summary>
    /// This method is used to initalise the member variables.
    /// </summary>
    private void ReadQueryString()
    {
        if (QueryString["TeacherId"] != null)
            miTeacherId = QueryString["TeacherId"].ToInt();
        
		if (QueryString["TeacherName"] != null)
            msTeacherName = QueryString["TeacherName"];
    }

    #endregion

}
