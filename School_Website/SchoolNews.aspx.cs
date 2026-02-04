using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SchoolEntities;
using System.Configuration;
using System.Web.UI.HtmlControls;
using Utility;

public partial class SchoolNews : SchoolBase
{
    #region "Data Member"

    SchoolNewsBL moSchoolNewsBL;
        
    #endregion

    /// <summary>
    /// This event is used to display the news listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moSchoolNewsBL = new SchoolNewsBL();
            if (!IsPostBack)
            {
                FillSelectedSchoolNews();                                
            }
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to fill details into table of new details.
    /// </summary>
    /// <param name=""></param>
    /// <param name=""></param>
    private void FillSelectedSchoolNews()
    {
        List<NewsDetails> lstNewsDetails = moSchoolNewsBL.GetSelectedNews(Convert.ToInt32(ConfigurationManager.AppSettings["SchoolID"]));

        foreach (NewsDetails oNewsDetails in lstNewsDetails)
        {

            HtmlTableRow trDate = new HtmlTableRow();
            AddCell(trDate, oNewsDetails.NewsDate.ToString(), "ClsProgressGridTestHeader1", "Left", 0, "font-weight:bold;border-style:solid;border-width:1px;border-color:skyblue;width:100%;");
            tblParameter.Rows.Add(trDate);

            HtmlTableRow trHeading = new HtmlTableRow();
            AddCell(trHeading, oNewsDetails.NewsHeading, "ClsProgressGridTestHeader1", "Left", 0, "font-weight:bold;border-style:solid;border-width:1px;border-color:skyblue;width:100%;");
            tblParameter.Rows.Add(trHeading);

            HtmlTableRow trDescription = new HtmlTableRow();
            AddCell(trDescription, "<br />" + oNewsDetails.NewsContent, "ClsProgressGridTestBody1", "Left", 2, "border-style:solid;border-width:1px;border-color:skyblue;width:100%;color:navy");
            tblParameter.Rows.Add(trDescription);

            HtmlTableRow trNewLine = new HtmlTableRow();
            AddCell(trNewLine, "<br />", "", "Left", 2);
            tblParameter.Rows.Add(trNewLine);
        }
    }

}