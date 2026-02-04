using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections;
using BusinessLogic;
using Utility;
using System.Configuration;

public partial class StudentIdentityCards : Page
{

    protected DataSet moIdentityDetails;
    protected int I_DB_TABLE_INDEX_HEADER = 2;
    protected int mistandardId = 0;
    protected int miDivisionId = 0;
    private int miStudentId = 0;
    private string msStudentName = string.Empty;
    private string msStudentReg = string.Empty;
    private ArrayList oArrayList = new ArrayList();

    /// <summary>
    /// Used to initalise page controls
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            InialiseMemberVariables();
            GetStudentsForIdentityCards();
            if (moIdentityDetails != null)
                GenerateIdentityCardsControls();
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
            (ex.Message + Constants.S_TRACE + ex.StackTrace,
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
            Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    /// <summary>
    /// USed to create the identity card.
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
        //oMainHtmlTable.Rows.Add(oHtmlTableRow);
        oMainHtmlTable.CellSpacing = 1;
        for (int iStudentCnt = 0; iStudentCnt < moIdentityDetails.Tables[1].Rows.Count; iStudentCnt++)
        {
            if ((iStudentCnt % 2) == 0)
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
            CreateSchoolHeading(oIdentityHtmlTable, iStudentCnt);
            CreateStudentsDetails(oIdentityHtmlTable, iStudentCnt);

            if (((iStudentCnt % 10) == 9) || moIdentityDetails.Tables[1].Rows.Count - 1 == iStudentCnt)
            {
                for (int icnt = 0; icnt < oArrayList.Count; icnt = icnt + 2)
                {
                    oHtmlTableRow = new HtmlTableRow();
                    oMainHtmlTable.Rows.Add(oHtmlTableRow);


                    oHtmlTableCell = new HtmlTableCell();
                    oHtmlTableCell.VAlign = "top";
                    if (icnt == 0)
                        oHtmlTableCell.InnerHtml = "<P CLASS='breakhere'>";
                    else
                        oHtmlTableCell.InnerHtml = "&nbsp;";
                    oHtmlTableCell.ColSpan = 4;
                    oHtmlTableRow.Cells.Add(oHtmlTableCell);
                    oHtmlTableRow.Height = "28px";

                    if (icnt == 0)
                    {
                        oHtmlTableRow = new HtmlTableRow();
                        oMainHtmlTable.Rows.Add(oHtmlTableRow);

                        oHtmlTableCell = new HtmlTableCell();
                        oHtmlTableCell.VAlign = "top";
                        oHtmlTableCell.InnerHtml = "&nbsp;";
                        oHtmlTableCell.ColSpan = 4;
                        oHtmlTableRow.Cells.Add(oHtmlTableCell);
                    }
                    oHtmlTableRow = new HtmlTableRow();
                    oMainHtmlTable.Rows.Add(oHtmlTableRow);

                    oHtmlTableCell = new HtmlTableCell();
                    oHtmlTableCell.VAlign = "top";
                    oHtmlTableCell.InnerHtml = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                    oHtmlTableRow.Cells.Add(oHtmlTableCell);

                    oHtmlTableCell = new HtmlTableCell();
                    oHtmlTableCell.VAlign = "top";
                    oHtmlTableRow.Cells.Add(oHtmlTableCell);
                    HtmlTable oIdentityTable;
                    if (icnt < oArrayList.Count - 1)
                        oIdentityTable = (HtmlTable)oArrayList[icnt + 1];
                    else
                        oIdentityTable = CreateHdTable();
                    oHtmlTableCell.Controls.Add(oIdentityTable);

                    oHtmlTableCell = new HtmlTableCell();
                    oHtmlTableCell.VAlign = "top";
                    oHtmlTableCell.InnerHtml = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                    oHtmlTableRow.Cells.Add(oHtmlTableCell);

                    oHtmlTableCell = new HtmlTableCell();
                    oHtmlTableCell.VAlign = "top";
                    oHtmlTableRow.Cells.Add(oHtmlTableCell);

                    oIdentityTable = (HtmlTable)oArrayList[icnt];
                    oHtmlTableCell.Controls.Add(oIdentityTable);

                    oHtmlTableCell = new HtmlTableCell();
                    oHtmlTableCell.VAlign = "top";
                    oHtmlTableCell.InnerHtml = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                    oHtmlTableRow.Height = "28px";
                    oHtmlTableRow.Cells.Add(oHtmlTableCell);
                }
                oArrayList.Clear();

                oHtmlTableRow = new HtmlTableRow();
                oMainHtmlTable.Rows.Add(oHtmlTableRow);

                oHtmlTableCell = new HtmlTableCell();
                oHtmlTableCell.VAlign = "top";
                oHtmlTableCell.InnerHtml = "<P CLASS='breakhere'>";
                oHtmlTableCell.ColSpan = 4;
                oHtmlTableRow.Cells.Add(oHtmlTableCell);
            }
        }
        //OutPutResponseToDoc();       
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
    /// USed to create the students Details for the identity card.
    /// </summary>
    /// <param name="oMainHtmlTable"></param>
    private void CreateStudentsDetails(HtmlTable oMainHtmlTable, int iStudentCnt)
    {
        HtmlTable oMainAddressHtmlTable = CreateHdTable();
        oArrayList.Add(oMainAddressHtmlTable);

        const string CSS_LBLSMLVB = "LblSmlVB";
        const string CSS_IDDETAILS = "IdDetails";
        DataRow oDataRow = moIdentityDetails.Tables[1].Rows[iStudentCnt];

        string sStudentName = Convert.ToString(oDataRow["First_Name"]) + " " + Convert.ToString(oDataRow["Last_Name"]); ;
        string sPhotoPath = Convert.ToString(oDataRow["Photo_file_Path"]);
        string sStandard = Convert.ToString(oDataRow["Standard_Name"]);
        string sDivision = Convert.ToString(oDataRow["Division_Name"]);
        string sGrNumber = Convert.ToString(oDataRow["Enrolment_Number"]);
        string sCity = Convert.ToString(oDataRow["City"]);
        string sPincode = Convert.ToString(oDataRow["Pincode"]);
        string sAddress = Convert.ToString(oDataRow["Address"]) + ", " + sCity;

        //string sBloodGroup = string.Empty;
        //if (oDataRow["Blood_Group"] != DBNull.Value)
        //    sBloodGroup = Convert.ToString(oDataRow["Blood_Group"]);
        string sDOB = Convert.ToDateTime(oDataRow["DOB"]).ToString("dd-MMM-yyyy");


        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        oHtmlTableRow.Height = "110px";
        oMainHtmlTable.Rows.Add(oHtmlTableRow);
        HtmlTableCell oHtmlTableCell = new HtmlTableCell();
        oHtmlTableRow.Cells.Add(oHtmlTableCell);
        oHtmlTableCell.Attributes.Add("colspan", "2");

        oMainHtmlTable = new HtmlTable();
        oMainHtmlTable.Height = "100%";
        oMainHtmlTable.Width = "100%";
        oMainHtmlTable.Border = 0;
        oHtmlTableCell.Controls.Add(oMainHtmlTable);

        oHtmlTableRow = new HtmlTableRow();
        oMainHtmlTable.Rows.Add(oHtmlTableRow);
        CreateHtmlCell(oHtmlTableRow, "Name :", CSS_LBLSMLVB, 1, 1);
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Height = "5px";
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].VAlign = "top";
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Style["padding-top"] = "2px";
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Width = "45px";
        CreateHtmlCell(oHtmlTableRow, sStudentName.ToUpper(), "StudentNameID", 1, 4);
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Style["padding-top"] = "2px";
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].VAlign = "top";

        oHtmlTableRow = new HtmlTableRow();
        oMainHtmlTable.Rows.Add(oHtmlTableRow);
        CreateHtmlCell(oHtmlTableRow, "Std.&nbsp;&nbsp;&nbsp; :", CSS_LBLSMLVB, 1, 1);
        CreateHtmlCell(oHtmlTableRow, sStandard + "<span class='LblSmlVB'>&nbsp;&nbsp;&nbsp;Div.&nbsp;:&nbsp; </span>" + sDivision, CSS_IDDETAILS, 1, 3);

        oHtmlTableCell = new HtmlTableCell();
        Image oImage = new Image();
        oImage.Height = Unit.Pixel(77);
        oImage.Width = Unit.Pixel(64);
        oHtmlTableCell.Height = "68px";
        oHtmlTableCell.Width = "64px";
        if (sPhotoPath != string.Empty)
            oImage.ImageUrl = ".." + sPhotoPath.Replace("\\", "/");
        else
            oImage.ImageUrl = "../images/StudentSml_phBDay.gif";
        oImage.Style.Add("left", "0");
        oImage.BorderStyle = BorderStyle.Solid;
        oImage.BorderWidth = Unit.Pixel(2);
        oHtmlTableCell.Controls.Add(oImage);
        oHtmlTableCell.VAlign = "top";
        oHtmlTableCell.Align = "right";
        oHtmlTableCell.Attributes.Add("rowspan", "3");
        oHtmlTableRow.Cells.Add(oHtmlTableCell);


        oHtmlTableRow = new HtmlTableRow();
        oMainAddressHtmlTable.Rows.Add(oHtmlTableRow);
        CreateHtmlCell(oHtmlTableRow, "<span class='LblSmlVB'>Address&nbsp;&nbsp;:</span>&nbsp;", CSS_IDDETAILS, 4, 1);
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].VAlign = "top";
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Style.Add(HtmlTextWriterStyle.PaddingTop, "15px");
        CreateHtmlCell(oHtmlTableRow, sAddress, CSS_IDDETAILS, 4, 3);
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].VAlign = "top";
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Style.Add(HtmlTextWriterStyle.PaddingTop, "15px");
        //oArrayList.Add(oHtmlTableCell);

        oHtmlTableRow = new HtmlTableRow();
        oMainAddressHtmlTable.Rows.Add(oHtmlTableRow);
        CreateHtmlCell(oHtmlTableRow, string.Empty, CSS_IDDETAILS, 1, 1);

        oHtmlTableRow = new HtmlTableRow();
        oMainAddressHtmlTable.Rows.Add(oHtmlTableRow);
        CreateHtmlCell(oHtmlTableRow, string.Empty, CSS_IDDETAILS, 1, 1);

        oHtmlTableRow = new HtmlTableRow();
        oMainAddressHtmlTable.Rows.Add(oHtmlTableRow);
        CreateHtmlCell(oHtmlTableRow, string.Empty, CSS_IDDETAILS, 1, 1);

        oHtmlTableRow = new HtmlTableRow();
        oMainAddressHtmlTable.Rows.Add(oHtmlTableRow);
        CreateHtmlCell(oHtmlTableRow, "Emergency Ph. No. :  <span class='IdDetails'>" + Convert.ToString(oDataRow["Mobile_Number"]) + "</span>", CSS_LBLSMLVB, 1, 4);
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].VAlign = "top";

        if (oDataRow["Residence_Phone_Number"] != DBNull.Value && Convert.ToString(oDataRow["Residence_Phone_Number"]).Trim() != string.Empty)
        {
            oHtmlTableRow = new HtmlTableRow();
            oMainAddressHtmlTable.Rows.Add(oHtmlTableRow);
            CreateHtmlCell(oHtmlTableRow, "Ph. No. :", CSS_LBLSMLVB, 1, 1);
            oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].VAlign = "top";
            CreateHtmlCell(oHtmlTableRow, Convert.ToString(oDataRow["Residence_Phone_Number"]), CSS_IDDETAILS, 1, 4);
            oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].VAlign = "top";
        }
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Style.Add(HtmlTextWriterStyle.PaddingBottom, "10px");

        oHtmlTableRow = new HtmlTableRow();
        oMainHtmlTable.Rows.Add(oHtmlTableRow);
        CreateHtmlCell(oHtmlTableRow, "D.O.B. :", CSS_LBLSMLVB, 1, 1);
        CreateHtmlCell(oHtmlTableRow, sDOB, CSS_IDDETAILS, 1, 3);


        //oHtmlTableRow = new HtmlTableRow();
        //oMainHtmlTable.Rows.Add(oHtmlTableRow);
        //CreateHtmlCell(oHtmlTableRow, "B.Grp. : ", CSS_LBLSMLVB, 1, 1);
        //oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].VAlign = "top";
        //CreateHtmlCell(oHtmlTableRow, "<span class='IdDetails'>" + sBloodGroup + "</span>", CSS_LBLSMLVB, 1, 2);
        //oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].VAlign = "top";
        
        oHtmlTableRow = new HtmlTableRow();
        oMainHtmlTable.Rows.Add(oHtmlTableRow);
        CreateHtmlCell(oHtmlTableRow, "&nbsp;", CSS_LBLSMLVB, 1, 1);
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].VAlign = "top";
        CreateHtmlCell(oHtmlTableRow, "&nbsp; ", CSS_LBLSMLVB, 1, 2);
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].VAlign = "top";


        
        oHtmlTableCell = new HtmlTableCell();
        oImage = new Image();
        //oImage.ImageUrl = "../images/IdentityCardImages/Signature.png";
        oImage.ImageUrl = ".." + Convert.ToString(moIdentityDetails.Tables[0].Rows[0]["SignPath"]);
        oImage.Style.Add("left", "0");
        oHtmlTableCell.Controls.Add(oImage);
        oHtmlTableCell.Controls.Add(new LiteralControl("<BR/>"));
        Label oLabel = new Label();
        oLabel.Text = "Principal's Sign.";
        oLabel.CssClass = "SchoolAddID";
        oHtmlTableCell.Controls.Add(oLabel);
        oHtmlTableCell.ColSpan = 1;
        oImage.Width = Unit.Pixel(50);
        oImage.Height = Unit.Pixel(17);
        oHtmlTableCell.VAlign = "bottom";
        oHtmlTableRow.Cells.Add(oHtmlTableCell);
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Align = HorizontalAlign.Center.ToString();
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Style["padding-bottom"] = "7px";

    }

    /// <summary>
    /// USed to create the school headings for the identity card.
    /// </summary>
    /// <param name="oMainHtmlTable"></param>
    private void CreateSchoolHeading(HtmlTable oMainHtmlTable, int iStudentCnt)
    {
        DataRow oDataRow = moIdentityDetails.Tables[0].Rows[0];
        string sCity = Convert.ToString(oDataRow["City"]);
        string sPincode = Convert.ToString(oDataRow["Pincode"]);
        string sAddress = Convert.ToString(oDataRow["Address"]) + ", " + sCity + " - " + sPincode;
        string sPhoneNumber = "Tel. : " + Convert.ToString(oDataRow["Phone_Number"]);

        string sSchoolName = Convert.ToString(oDataRow["School_Name"]).ToUpper();

        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        oMainHtmlTable.Rows.Add(oHtmlTableRow);

        HtmlTableCell oHtmlTableCell = new HtmlTableCell();
        Image oImage = new Image();
        oHtmlTableCell.Width = "20%";
        oHtmlTableCell.Height = "20px";
        //oImage.ImageUrl = "../images/IdentityCardImages/School_logo.png";
        oImage.ImageUrl = ".." + Convert.ToString(oDataRow["ICardLogo"]);
        oImage.Style.Add("right", "0");
        oImage.Style.Add("height", "30px");
        oImage.Style.Add("width", "50px");
        oHtmlTableCell.Controls.Add(oImage);
        oHtmlTableCell.Attributes.Add("rowspan", "3");
        oHtmlTableCell.Attributes.Add("bgcolor", "white");
        oHtmlTableCell.Align = HorizontalAlign.Right.ToString();
        oHtmlTableRow.Cells.Add(oHtmlTableCell);

        CreateHtmlCell(oHtmlTableRow, sSchoolName, "SchoolHeadID  ", 1, 1);
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Align = HorizontalAlign.Center.ToString();
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].VAlign = VerticalAlign.Bottom.ToString();
        oHtmlTableRow.Height = "7px";
        oHtmlTableRow = new HtmlTableRow();
        oMainHtmlTable.Rows.Add(oHtmlTableRow);
        CreateHtmlCell(oHtmlTableRow, sAddress, "SchoolAddID", 1, 1);
        oHtmlTableRow.Height = "10px";
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Align = HorizontalAlign.Center.ToString();
        oHtmlTableRow = new HtmlTableRow();
        oMainHtmlTable.Rows.Add(oHtmlTableRow);
        //CreateHtmlCell(oHtmlTableRow, sPhoneNumber + " e-mail: info@ppspune.com", " SchoolAddID ", 1, 1);
		CreateHtmlCell(oHtmlTableRow, sPhoneNumber + " e-mail: " + ConfigurationManager.AppSettings["EmailAddress"], " SchoolAddID ", 1, 1);
        oHtmlTableRow.Height = "10px";
        oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1].Align = HorizontalAlign.Center.ToString();
    }

    /// <summary>
    /// This methos is used to create not applicable ledgend.
    /// </summary>
    protected HtmlTable CreateHdTable()
    {
        HtmlTable HeaderHtmlTable = new HtmlTable();
        HeaderHtmlTable.EnableViewState = false;
        HeaderHtmlTable.CellPadding = 0;
        HeaderHtmlTable.CellSpacing = 1;

        HeaderHtmlTable.Attributes.Add("class", "IDtblBorder");
        HeaderHtmlTable.Height = "162px";
        HeaderHtmlTable.Width = "270px";
        HeaderHtmlTable.Border = 0;
        //HeaderHtmlTable.Border = 1;
        HeaderHtmlTable.Attributes.Add("bgcolor ", "white");
        HeaderHtmlTable.Style.Add(HtmlTextWriterStyle.VerticalAlign, HtmlTextWriterStyle.Top.ToString());
        HeaderHtmlTable.Style.Add(HtmlTextWriterStyle.Left, Unit.Pixel(0).ToString());
        return HeaderHtmlTable;
    }

    /// <summary>
    /// This method is used to create cell
    /// </summary>
    /// <param name="sInnerText"></param>
    /// <param name="sClassName"></param>
    /// <param name="iRowSpan"></param>
    /// <param name="iColSpan"></param>
    protected void CreateHtmlCell(HtmlTableRow oHtmlTableRow, String sInnerText, String sClassName, int iRowSpan, int iColSpan)
    {
        HtmlTableCell oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.InnerHtml = sInnerText;
        oHtmlTableCell.Attributes.Add("rowspan", iRowSpan.ToString());
        oHtmlTableCell.Attributes.Add("colspan", iColSpan.ToString());
        oHtmlTableCell.Attributes.Add("class", sClassName);
        oHtmlTableCell.Attributes.Add("align", "left");
        oHtmlTableCell.Attributes.Add("style", "padding-left : 2px");
        oHtmlTableCell.Align = HorizontalAlign.Left.ToString();
        oHtmlTableRow.Cells.Add(oHtmlTableCell);
        oHtmlTableCell.Dispose();
    }

    /// <summary>
    /// This method is used to get resultset for the progress sheet
    /// </summary>
    protected void GetStudentsForIdentityCards()
    {
        int iAcademicYrID = Convert.ToInt32(Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID]);
        int iSchoolID = Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]);
        StudentBL oStudentBL = new StudentBL();
        moIdentityDetails = oStudentBL.getStudentIdentityCards(iSchoolID, iAcademicYrID, mistandardId, miDivisionId, miStudentId,msStudentName,msStudentReg);
    }

    /// <summary>
    /// This method is used to initalise the member variables.
    /// </summary>
    private void InialiseMemberVariables()
    {
        HttpRequest oHttpRequest = DecryptQuerystring();
        if (oHttpRequest.QueryString["iStandardId"] != null)
            mistandardId = Convert.ToInt32(oHttpRequest.QueryString["iStandardId"]);
        if (oHttpRequest.QueryString["iDivisionId"] != null)
            miDivisionId = Convert.ToInt32(oHttpRequest.QueryString["iDivisionId"]);
        if (oHttpRequest.QueryString["iStudentId"] != null)
            miStudentId = Convert.ToInt32(oHttpRequest.QueryString["iStudentId"]);
        if (miStudentId == 0)
        {
            if (oHttpRequest.QueryString["sStudentName"] != null)
                msStudentName = Convert.ToString(oHttpRequest.QueryString["sStudentName"]);
            if (oHttpRequest.QueryString["sStudentReg"] != null)
                msStudentReg = Convert.ToString(oHttpRequest.QueryString["sStudentReg"]);
        }
    }

    /// <summary>
    /// This method is used to decrypt encrypted querystring.
    /// </summary>
    public HttpRequest DecryptQuerystring()
    {
        string sQueryString = "";
        HttpRequest oHttpRequest = null;
        if (Request.QueryString.ToString() != Constants.S_EMPTY_STRING)
        {
            string sTestDecrypt = Server.UrlDecode(Request.QueryString.ToString());

            sQueryString = Utility.CommonUtility.DecryptQuerystring(sTestDecrypt);
            oHttpRequest = new HttpRequest(Page.Request.FilePath.ToString(),
                                            Page.Request.Url.ToString(),
                                            sQueryString);

        }
        return oHttpRequest;
    }

}
