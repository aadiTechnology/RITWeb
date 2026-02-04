/* File Name :- ImageGallery.aspx.cs
 * Modified By :- Sachin
 * Modified Date :- 23-Seept-2009
 * Purpose :- Code review.
 * Class Description :- This class is used to display slideshow of photo gallery.
*/

using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using BusinessLogic.Exceptions;

public partial class ImageGallery : SchoolBase
{

	#region -- MEMBER(s) --

	protected string msGalleryPath = string.Empty;

	#endregion -- MEMBER(s) --

	#region -- EVENT HANDLER(s) --

	/// <summary>
	///		Handles the page load event.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
        {
            if (!IsPostBack)
            {
                optMediam.Checked = true;
                ReadQueryString();
                BuildSlideShow(msGalleryPath);
            }
           
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
       
	}
    
	#endregion -- EVENT HANDLER(s) --

	#region -- PRIVATE METHOD(s) --

	/// <summary>
	///		This method is used to decrypt querystring and get gallery path.
	/// </summary>
	private void ReadQueryString()
	{
		if (QueryString.Count <= 0)
			return;

        if (QueryString["xmlpath"] != null)
            msGalleryPath = QueryString["xmlpath"];       
	}

	/// <summary>
	///		Builds the slideshow content on the page from the given xml file path.
	/// </summary>
	/// <param name="xmlPath"></param>
	private void BuildSlideShow(string xmlPath)
	{
		try
		{
            if (string.IsNullOrEmpty(xmlPath))
                throw new FileNotFoundException();

			XElement xele = XElement.Load(Server.MapPath(xmlPath));

			lblGalname.Text = xele.Attribute("date").Value;

			XElement xlarge = xele.XPathSelectElement("//thumbnail");
			XElement xImages = xele.XPathSelectElement("//images");

			var strImages = new StringBuilder();

            int iImageCount = 0;
            foreach (XElement xImage in xImages.Elements())
            {
                strImages.AppendFormat("<img src='{0}{1}' alt=\"{2}\" />",
                                        xlarge.Attribute("base").Value,
                                        xImage.Attribute("path").Value,
                                        xImage.Attribute("comment") != null ? xImage.Attribute("comment").Value : string.Empty);
                iImageCount++;
            }

			slideshowHolder.InnerHtml = strImages.ToString();
            if (iImageCount == 1)
                Divradio.Visible = false;
		}
		catch (FileNotFoundException)
		{  
            Divradio.Visible=false;
			lblGalname.Text = "Slide show not available.";
		}
	}

	#endregion -- PRIVATE METHOD(s) --

   
}
