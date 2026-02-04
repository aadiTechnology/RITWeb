using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BookEntities;
using BusinessLogic.Exceptions;
using Utility;

public partial class BookBarcodePopup : SchoolBase
{

    #region -- MEMBER(s) --

    private List<BookDetails> molstBookDetails = new List<BookDetails>();

    #endregion -- MEMBER(s) --

    #region -- EVENT HANDLER(s) --

   /// <summary>
   /// This event is used to generate barcodes
   /// </summary>
   /// <param name="sender"></param>
   /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
		try
		{			
			molstBookDetails = (List<BookDetails>)(Session["BookDetailsList"]);
			DisplayBarcodes();

            if (QueryString["Is_Configured"] != null && QueryString["Is_Configured"] != Constants.S_YES)
				SaveConfigDetails(Constants.SchoolConfigurations.GenerateBarcode.ToInt());
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
    }

    #endregion -- EVENT HANDLER(s) --

    #region -- PRIVATE METHOD(s) --

    /// <summary>
    /// This method is usd to display the barcodes.
    /// </summary>
    private void DisplayBarcodes()
    {
        List<BookDetails> olstBookDetails = molstBookDetails;

        GridViewContainer.Border = 0;
        GridViewContainer.Attributes.Add("class", "ClsBorderlight");

        var oHtmlTableRow = new HtmlTableRow();
        if (miSchoolId == Constants.SchoolId.SNS.ToInt())
        {
            oHtmlTableRow.Attributes.Add("style", "text-align : center;");
        }
        int iCellCount = 0;
        olstBookDetails
			.ForEach(book => {
                        string sCode = string.Empty;
                        if(miSchoolId == Constants.SchoolId.SNS.ToInt())
                            sCode = book.Book_No.ToString();
                        else
                            sCode = Convert.ToChar(Constants.BarcodeChar.Book) + book.Book_Detail_Id.ToString() + Convert.ToChar(Constants.BarcodeChar.Separator) + Session[Constants.S_SESSION_SCHOOL_ID];

						var lblBookNo = new Label();
                        var lblSchoolName = new Label();
                        if (miSchoolId != Constants.SchoolId.SNS.ToInt())
                        {
                            lblBookNo.Text = "<Br />&nbsp;Accession No.: " + book.Book_No;
                            lblBookNo.Attributes.CssStyle.Add("font-size", "8pt");
                            lblSchoolName.Text = string.Empty;
                        }
                        else
                        {
                            lblBookNo.Text = "<Br /> <B>" + book.Book_No + "</B>";
                            lblBookNo.Attributes.Add("style", "font-size : 9pt;");
                            lblSchoolName.Text = "<B>SHANTINIKETAN </B> <Br />";
                            lblSchoolName.Attributes.Add("style", "font-size : 9pt;");
                        }
                
						var oaHtmlTableCell = new HtmlTableCell();
						var img = new Image();                                                

						img.ImageUrl = "Handler.ashx?id=" + sCode;
                
				       oaHtmlTableCell.Attributes.Add("style", "padding-right : 25px");
                     

                      

                        if (miSchoolId == Constants.SchoolId.SNS.ToInt())
                            oaHtmlTableCell.Controls.Add(lblSchoolName);

						oaHtmlTableCell.Controls.Add(img);
						oaHtmlTableCell.Controls.Add(lblBookNo);

						oHtmlTableRow.Cells.Add(oaHtmlTableCell);
						iCellCount++;
                        if (miSchoolId == Constants.SchoolId.SNS.ToInt())
                        {
                            if (iCellCount == 4)
                            {
                                GridViewContainer.Rows.Add(oHtmlTableRow);
                                oHtmlTableRow = new HtmlTableRow();                                
                                oHtmlTableRow.Attributes.Add("style", "text-align : center;");                                
                                iCellCount = 0;
                            }
                        }
                        else
                        {
                            if (iCellCount == 4)
                            {
                                GridViewContainer.Rows.Add(oHtmlTableRow);
                                oHtmlTableRow = new HtmlTableRow();
                                iCellCount = 0;
                            }
                        }
					});

        GridViewContainer.Rows.Add(oHtmlTableRow);
    }

    #endregion -- PRIVATE METHOD(s) --

}