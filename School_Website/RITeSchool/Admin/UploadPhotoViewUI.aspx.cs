/* File Name :- UploadPhotoViewUI.aspx.cs
* Created By :- Sachin
* Created Date :- 25-March-2009
* Class Description :- This class is used to update/delete photo's from photo gallery.
*/

using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Ionic.Zip;
using Utility;
using System.Xml;
using System.Collections.Generic;

public partial class PhotoGalleryUI : SchoolBase
{
	#region Constants

	private const int I_DELETE_COLUMN_INDEX = 4;

	private const string S_EDIT_COMMAND = "EDIT_ROW";
	private const string S_DELETE_COMMAND = "DELETE_ROW";

	private const string S_UPDATE_MESSAGE = "Photo comment updated successfully!!!";
	private const string S_UPDATE_ERROR_MESSAGE = "Failed to update photo comment.";
	private const string S_DELETE_MESSAGE = "Photo deleted successfully!!!";
	private const string S_DELETE_ERROR_MESSAGE = "Failed to delete photo.";
	private const string S_EDIT_ERROR_MESSAGE = "There was an error editing photo.";

	#endregion

	#region Events

	/// <summary>
	/// This event is used to set client script attributes and fill photo's of selected gallery into gridview.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{            
			if (!IsPostBack)
			{
				ReadQuerystring();
				FillPhotoGallery();
				DisableControls(true);
				SetJavascriptAttributes();
				btnPhotoUpdate.Focus();
			}

			var oForm = this.Master.FindControl("form1") as HtmlForm;
			oForm.DefaultButton = btnPhotoUpdate.UniqueID;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to delete selected photo.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void grdPhotos_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		string sCommand = e.CommandName.ToUpper();
		
		try
		{
			int iRowIndex = e.CommandArgument.ToInt();
			switch (sCommand)
			{
				case S_EDIT_COMMAND:
					EditComment(iRowIndex);
					break;
				case S_DELETE_COMMAND:
					{
						DeletePhoto(iRowIndex);

						string sGalleryName = lblGalleryName.Text;

						// Check if the Gallery contains any images
						// If it DOES NOT, then delete it's Zip archive & XML file stored on the server
						var oImageGalleryBL = new ImageGalleryBL
							{
								SchoolId = miSchoolId
							};
						int iGalleryImageCount = oImageGalleryBL.GetPhotoCount(sGalleryName);
						if (iGalleryImageCount == 0)
						{
							string sGalleryZipFilePath = Server.MapPath("..") + "\\DOWNLOADS\\" + sGalleryName + ".zip";
							if (File.Exists(sGalleryZipFilePath))
								File.Delete(sGalleryZipFilePath);
							string sGalleryXMLFilePath = Server.MapPath("..") + "\\Gallery\\" + sGalleryName + ".xml";
							if (File.Exists(sGalleryXMLFilePath))
								File.Delete(sGalleryXMLFilePath);
						}
							// If it DOES, then update the Zip archive & XML file with the new data
						else
						{
							// Recreate Gallery XML to reflect changes made to the Gallery.
							CreateXMLOfGallery(sGalleryName);

							// Recreate the Zip archive to reflect changes made to the Gallery.
							CreateGalleryArchive(sGalleryName);
						}
					}
					break;
			}
		}
		catch (Exception ex)
		{
			string sMessage = sCommand == S_EDIT_COMMAND ? S_EDIT_ERROR_MESSAGE : S_DELETE_ERROR_MESSAGE;
			SetMessage(sMessage, true);
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to add delete button attribute.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void grdPhotos_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		try
		{
			if (e.Row.RowIndex >= Constants.I_ZERO)
			{
				var oPhotoDelete = e.Row.Cells[I_DELETE_COLUMN_INDEX].Controls[Constants.I_ZERO] as ImageButton;
				oPhotoDelete.Attributes.Add("onclick", "if(!ConfirmPhotoDelete()) {return false;}");
				var oImg = e.Row.Cells[1].Controls[Constants.I_ZERO] as Image;
				oImg.ImageUrl = "..\\" + oImg.ImageUrl;
				oImg.Height = 120;
				oImg.Width = 160;
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to update comment.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnPhotoUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			ImageGalleryBL oImageGalleryBL = InitializeGalleryBL();
			oImageGalleryBL.GalleryId = hidGalleryId.Value.ToInt();
			oImageGalleryBL.UpdateComment();
			DisableControls(true);
			FillPhotoGallery();
			CreateXMLOfGallery(lblGalleryName.Text);
			txtComment.Text = string.Empty;
			btnPhotoUpdate.Focus();

			SetMessage(S_UPDATE_MESSAGE, false);
		}
		catch (Exception ex)
		{
			SetMessage(S_UPDATE_ERROR_MESSAGE, true);
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	#endregion

	#region Methods

	/// <summary>
	/// This method is used to set javascript attributes.
	/// </summary>
	private void SetJavascriptAttributes()
	{
		btnClose.Attributes["onClick"] = "refreshParent()";
        ApplyMouseHoverEffect(new List<Button> { btnPhotoUpdate, btnClose });
	}

	/// <summary>
	/// This method is used to fill photo gallery.
	/// </summary>
	private void FillPhotoGallery()
	{
		DataTable oDataTable = ImageGalleryCollectionBL.FetchPhotosForGallery(lblGalleryName.Text, miSchoolId, miAcademicYearId);
		grdPhotos.Columns[0].Visible = true;
		grdPhotos.DataSource = oDataTable;
		grdPhotos.DataBind();
		grdPhotos.Columns[0].Visible = false;
	}

	/// <summary>
	/// This method is used to disable controls.
	/// </summary>
	/// <param name="abFlag"></param>
	private void DisableControls(bool abFlag)
	{
		btnPhotoUpdate.Enabled = !abFlag;
		txtComment.ReadOnly = abFlag;
	}

	/// <summary>
	/// This method is used to delete photo.
	/// </summary>
	/// <param name="iRowIndex"></param>
	private void DeletePhoto(int iRowIndex)
	{
		// Get image ID from grid and delete the image from database.
		int iPhotoId = grdPhotos.DataKeys[iRowIndex][0].ToString().ToInt();
		ImageGalleryBL.DeletePhoto(iPhotoId);
		DisableControls(true);
		txtComment.Text = string.Empty;
		// Delete the file from physical location as well.
		File.Delete(Server.MapPath("..") + "\\" + grdPhotos.Rows[iRowIndex].Cells[0].Text);
		FillPhotoGallery();

		SetMessage(S_DELETE_MESSAGE, false);
	}

	/// <summary>
	/// This method is used to edit comment.
	/// </summary>
	/// <param name="iRowIndex"></param>
	private void EditComment(int iRowIndex)
	{
		const int I_EDIT_COLUMN_INDEX = 2;
		txtComment.Text = HttpUtility.HtmlDecode(grdPhotos.Rows[iRowIndex].Cells[I_EDIT_COLUMN_INDEX].Text.Trim());
		hidGalleryId.Value = grdPhotos.DataKeys[iRowIndex][0].ToString();
		DisableControls(false);
		txtComment.Focus();
	}

	/// <summary>
	/// This method is used to decrypt the given query string.
	/// </summary>
	private void ReadQuerystring()
	{
		if (QueryString.Count > 0 && QueryString["ImageGalleryName"] != null)
			lblGalleryName.Text = QueryString["ImageGalleryName"];
	}

	/// <summary>
	/// This method is used to initialize gallery.
	/// </summary>
	/// <returns></returns>
	private ImageGalleryBL InitializeGalleryBL()
	{
		var oImageGalleryBL = new ImageGalleryBL
			{
				Comment = txtComment.Text.Trim(),
				SchoolId = miSchoolId,
				AcademicYrId =miAcademicYearId
			};
		return oImageGalleryBL;
	}

	/// <summary>
	/// This method is used to create XML of given photo gallery.
	/// </summary>
	/// <param name="asGalleryName"></param>
	private void CreateXMLOfGallery(string asGalleryName)
	{
		const int I_IMAGE_WIDTH = 800;
		const int I_IMAGE_HEIGHT = 500;
		const string S_ELEMENT = "element";
		// Get all images uploaded for the selected gallery.
		DataTable oDTImages = ImageGalleryCollectionBL.FetchPhotosForGallery(asGalleryName, miSchoolId, miAcademicYearId);
		if (oDTImages.Rows.Count <= 0)
			return;
		
		// Create XML file for these images.
		var oDoc = new XmlDocument();

		// Create a root level element.
		XmlElement root = oDoc.CreateElement("gallery");
		XmlNode oXmlBaseNode = GetBaseXMLDocument(ref oDoc, ref root, asGalleryName);
		XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "images", string.Empty);

		string sAtrrName = "id";
		XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
		attr.Value = "images";
		oXmlRootNode.Attributes.Append(attr);

		foreach (DataRow oRow in oDTImages.Rows)
		{
			XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "image", string.Empty);
			string sPath = oRow["Image_Path"].ToString();
			sPath = sPath.Substring(sPath.LastIndexOf("\\") + 1);

			sAtrrName = "path";
			attr = oDoc.CreateAttribute(sAtrrName);
			attr.Value = sPath;
			oXmlNode.Attributes.Append(attr);

			sAtrrName = "width";
			attr = oDoc.CreateAttribute(sAtrrName);
			attr.Value = I_IMAGE_WIDTH.ToString();
			oXmlNode.Attributes.Append(attr);

			sAtrrName = "height";
			attr = oDoc.CreateAttribute(sAtrrName);
			attr.Value = I_IMAGE_HEIGHT.ToString();
			oXmlNode.Attributes.Append(attr);

			sAtrrName = "thumbpath";
			attr = oDoc.CreateAttribute(sAtrrName);
			attr.Value = sPath;
			oXmlNode.Attributes.Append(attr);

			sAtrrName = "comment";
			attr = oDoc.CreateAttribute(sAtrrName);
			attr.Value = oRow["Comment"].ToString();
			oXmlNode.Attributes.Append(attr);

			oXmlRootNode.AppendChild(oXmlNode);
		}

		oXmlBaseNode.AppendChild(oXmlRootNode);

		// Add the root node to document element. 
		oDoc.AppendChild(oXmlBaseNode);

		// Save the XML file in folder.
		if (File.Exists(Server.MapPath("..") + "\\Gallery\\" + asGalleryName + ".xml"))
			File.Delete(Server.MapPath("..") + "\\Gallery\\" + asGalleryName + ".xml");

		// Remove all special charachers from file name.
		oDoc.Save(Server.MapPath("..") + "\\Gallery\\" + asGalleryName + ".xml");
	}

	/// <summary>
	/// This method is used to generate base XML Document.
	/// </summary>
	/// <param name="oDoc"></param>
	/// <param name="root"></param>
	/// <param name="asGalleryName"></param>
	/// <returns></returns>
	private XmlNode GetBaseXMLDocument(ref XmlDocument oDoc, ref XmlElement root, string asGalleryName)
	{
		const string S_ELEMENT = "element";
		XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "gallery", string.Empty);

		string sAtrrName = "base";
		XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
		attr.Value = string.Empty;
		oXmlRootNode.Attributes.Append(attr);

		sAtrrName = "background";
		attr = oDoc.CreateAttribute(sAtrrName);
		attr.Value = "#ffffff";
		oXmlRootNode.Attributes.Append(attr);

		sAtrrName = "banner";
		attr = oDoc.CreateAttribute(sAtrrName);
		attr.Value = "#ffffff";
		oXmlRootNode.Attributes.Append(attr);

		sAtrrName = "text";
		attr = oDoc.CreateAttribute(sAtrrName);
		attr.Value = "#cc3366";
		oXmlRootNode.Attributes.Append(attr);

		sAtrrName = "link";
		attr = oDoc.CreateAttribute(sAtrrName);
		attr.Value = "#1313A2";
		oXmlRootNode.Attributes.Append(attr);

		sAtrrName = "alink";
		attr = oDoc.CreateAttribute(sAtrrName);
		attr.Value = "#8F6F6F";
		oXmlRootNode.Attributes.Append(attr);

		sAtrrName = "vlink";
		attr = oDoc.CreateAttribute(sAtrrName);
		attr.Value = "#550080";
		oXmlRootNode.Attributes.Append(attr);

		sAtrrName = "date";
		attr = oDoc.CreateAttribute(sAtrrName);
		attr.Value = "Gallery Name :  " + asGalleryName;
		oXmlRootNode.Attributes.Append(attr);

		// Next element "banner".
		XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "banner", string.Empty);

		sAtrrName = "font";
		attr = oDoc.CreateAttribute(sAtrrName);
		attr.Value = "Verdana";
		oXmlNode.Attributes.Append(attr);

		sAtrrName = "fontsize";
		attr = oDoc.CreateAttribute(sAtrrName);
		attr.Value = "5";
		oXmlNode.Attributes.Append(attr);

		sAtrrName = "color";
		attr = oDoc.CreateAttribute(sAtrrName);
		attr.Value = "#F0F0F0";
		oXmlNode.Attributes.Append(attr);

		oXmlRootNode.AppendChild(oXmlNode);

		// Next element "thumbnail".
		oXmlNode = oDoc.CreateNode(S_ELEMENT, "thumbnail", string.Empty);

		sAtrrName = "base";
		attr = oDoc.CreateAttribute(sAtrrName);
		attr.Value = "/RITeSchool/images/gallery/";
		oXmlNode.Attributes.Append(attr);

		sAtrrName = "font";
		attr = oDoc.CreateAttribute(sAtrrName);
		attr.Value = "Verdana";
		oXmlNode.Attributes.Append(attr);

		sAtrrName = "fontsize";
		attr = oDoc.CreateAttribute(sAtrrName);
		attr.Value = "4";
		oXmlNode.Attributes.Append(attr);

		sAtrrName = "color";
		attr = oDoc.CreateAttribute(sAtrrName);
		attr.Value = "#F0F0F0";
		oXmlNode.Attributes.Append(attr);

		sAtrrName = "border";
		attr = oDoc.CreateAttribute(sAtrrName);
		attr.Value = "0";
		oXmlNode.Attributes.Append(attr);

		sAtrrName = "rows";
		attr = oDoc.CreateAttribute(sAtrrName);
		attr.Value = "0";
		oXmlNode.Attributes.Append(attr);

		sAtrrName = "col";
		attr = oDoc.CreateAttribute(sAtrrName);
		attr.Value = "0";
		oXmlNode.Attributes.Append(attr);

		oXmlRootNode.AppendChild(oXmlNode);

		// Next element "large".
		oXmlNode = oDoc.CreateNode(S_ELEMENT, "large", string.Empty);

		sAtrrName = "base";
		attr = oDoc.CreateAttribute(sAtrrName);
		attr.Value = "../images/gallery/";
		oXmlNode.Attributes.Append(attr);

		sAtrrName = "font";
		attr = oDoc.CreateAttribute(sAtrrName);
		attr.Value = "Verdana";
		oXmlNode.Attributes.Append(attr);

		sAtrrName = "fontsize";
		attr = oDoc.CreateAttribute(sAtrrName);
		attr.Value = "4";
		oXmlNode.Attributes.Append(attr);

		sAtrrName = "color";
		attr = oDoc.CreateAttribute(sAtrrName);
		attr.Value = "#F0F0F0";
		oXmlNode.Attributes.Append(attr);

		sAtrrName = "border";
		attr = oDoc.CreateAttribute(sAtrrName);
		attr.Value = "0";
		oXmlNode.Attributes.Append(attr);

		oXmlRootNode.AppendChild(oXmlNode);

		return oXmlRootNode;
	}

	/// <summary>
	/// Creates a Zip archive of a Photo Gallery
	/// </summary>
	/// <param name="asGalleryName">Name of the Photo Gallery</param>
	private void CreateGalleryArchive(string asGalleryName)
	{
		try
		{
			DataTable oDataTable = ImageGalleryBL.GetImages(miSchoolId, asGalleryName);
			if (oDataTable != null && oDataTable.Rows.Count > 0 && oDataTable.Rows[0][0] != DBNull.Value)
			{
				string sFileName;
				int iIndex;
				int iCount = 0;
				string sDestination = Server.MapPath("..") + "\\DOWNLOADS\\" + asGalleryName + ".zip";
				if (File.Exists(sDestination))
					File.Delete(sDestination);
				using (var zip = new ZipFile(sDestination))
				{
					iCount = oDataTable.Rows.Count;
					for (iIndex = 0; iIndex < iCount; iIndex++)
					{
						sFileName = Server.MapPath("..") + "\\" + oDataTable.Rows[iIndex][0];
						zip.AddFile(sFileName, asGalleryName);
					}
					zip.Save();
				}
			}
		}
		catch (Exception ex)
		{
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
	}

	/// <summary>
	///		Sets the message to be shown on the page.
	/// </summary>
	/// <param name="asMessage"></param>
	/// <param name="abIsError"></param>
	private void SetMessage(string asMessage, bool abIsError)
	{
		lblErrorMessage.Visible = lblUpateMessage.Visible = false;
		(abIsError ? lblErrorMessage : lblUpateMessage).Text = asMessage;
		(abIsError ? lblErrorMessage : lblUpateMessage).Visible = true;
	}

	#endregion
}
