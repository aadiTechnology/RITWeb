/* File Name = UpdateMenuFilesUI.aspx.cs
 * Created Date - 12 July 2011
 * Created by - Vipul
 * Class Description - This class is defined to manage Menu Files.*/

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;

public partial class UpdateMenuFilesUI : SchoolBase
{

	#region -- CONSTANT(s) --

	private const string S_DEFAULT_SORT_EXP = "InsertDate";
	private const string S_SAVE_MESSAGE = "File saved successfully!!!";
	private const string S_UPDATE_MESSAGE = "File updated successfully !!!";
	private const string S_DELETE_MESSAGE = "File deleted successfully!!!";
	private const string S_COMMON_ERROR = "There was an error deleting the file.";
	private const string S_UPDATE_FILE = "UpdateFile";
	private const string S_DELETE_FILE = "DeleteFile";
	private const string S_SORT_ROW = "SortRow";
	private const string S_NEW = "New";
	private const string S_EDIT = "Edit";
	private const string S_SAVE = "Save";
	private const string S_UPDATE = "Update";

	#endregion -- CONSTANT(s) --

	#region -- EVENT HANDLER(s) --

	/// <summary>
	/// This event is used to set default control fields and java script attributes.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{			
			if (!IsPostBack)
			{
				FillMenuList();
				SetDefaultValuesToControls();
				SetJavaScriptAttributes();
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to update menu file.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
			SaveMenuFile(hidMode.Value == S_NEW);
			SetDefaultValuesToControls();

			if (!IsConfigured())
				SaveConfigDetails(Constants.SchoolConfigurations.MenuFiles.ToInt());


			RebindList();

            MasterPage oMaster = (MasterPage)this.Master;
            oMaster.FillMenuControl();
		}
		catch (FileNotFoundException oEx)
		{
			lblErrorMsg.Text = oEx.Message;
			RebindList();
            MasterPage oMaster = (MasterPage)this.Master;
            oMaster.FillMenuControl();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to clear all controls.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnCancel_Click(object sender, EventArgs e)
	{
		try
		{
			SetDefaultValuesToControls();
			RebindList();
            MasterPage oMaster = (MasterPage)this.Master;
            oMaster.FillMenuControl();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

    /// <summary>
	/// This event is used to load top menu bar.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
	{
		try
		{
			SetDefaultValuesToControls();
			RebindList();
            MasterPage oMaster = (MasterPage)this.Master;
            oMaster.FillMenuControl();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}
    
	/// <summary>
	/// This event is used to view pagewise menu files.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void cmbPageCnt_SelectedIndexChanged(Object sender, EventArgs e)
	{
		try
		{
			ControlUtility.SetDataPagerAccordingToPageNo(lstvwMenuFilesDetails);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to fill file extensions.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwMenuFilesDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				var oCurrentItem = e.Item as ListViewDataItem;

                MenuFile oMenuFile = oCurrentItem.DataItem as MenuFile;
                if (oMenuFile.IsURL)
                    (oCurrentItem.FindControl("lblExtension") as Label).Text = "URL";
                else
                {
                    string sFilePath = Convert.ToString(lstvwMenuFilesDetails.DataKeys[oCurrentItem.DisplayIndex]["Path"]);
                    (oCurrentItem.FindControl("lblExtension") as Label).Text = sFilePath.Substring(sFilePath.LastIndexOf(".") + 1, sFilePath.Length - sFilePath.LastIndexOf(".") - 1).ToLower();
                }

                Label lblAddedOn = oCurrentItem.FindControl("lblAddedOn") as Label;
                if (lblAddedOn != null)
                {
                    lblAddedOn.Text = string.Empty;
                    if (!string.IsNullOrEmpty(oMenuFile.InsertDate))
                    {
                        DateTime dtAddedOn;
                        if (DateTime.TryParse(oMenuFile.InsertDate, out dtAddedOn))
                            lblAddedOn.Text = dtAddedOn.ToString(Constants.S_DATE_FORMAT);
                    }
                }
            }
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to load file menu details.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwMenuFilesDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
	{
		try
		{
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int iMenuFileDetailsId = lstvwMenuFilesDetails.DataKeys[e.Item.DisplayIndex]["Id"].ToInt();
                  switch (e.CommandName)
                {
                    case S_UPDATE_FILE:
                        hidMode.Value = S_EDIT;
                        btnSave.Text = S_UPDATE;
                        spanUploadFile.Visible = false;
                        LoadMenuFilesDetails(iMenuFileDetailsId);
                        RebindList();
                      break;
                    case S_DELETE_FILE:
                        DeleteMenuFile(iMenuFileDetailsId);

                        var lstvwMenuFiles = sender as ListView;
                        if (lstvwMenuFiles.Items.Count == 1)
                            DeleteConfigDetails(Constants.SchoolConfigurations.MenuFiles.ToInt());

                        SetMessage(S_DELETE_MESSAGE, false);
                        SetDefaultValuesToControls();
                        RebindList();
                        break;
                 }     
            }
            // This case is to handle a sort command. We have set a custom sort command - 'SORT_ROW' so we can handle sorting ourselves.
            // In such a scenario, the ItemType property is actually EmptyItem, hence we cannot handle this in the previous block.
            else if (e.Item.ItemType == ListViewItemType.EmptyItem && e.CommandSource is LinkButton && e.CommandName == S_SORT_ROW)
            {
                if (hidSortExpression.Value != e.CommandArgument.ToString())
                    hidSortDirection.Value = Constants.S_DESCENDING;
                SetSortVariables();
                hidSortExpression.Value = e.CommandArgument.ToString();
                lstvwMenuFilesDetails.DataSourceID = ObjDSMenuFilesDetails.ID;
            }
		}
		catch (FileNotFoundException fex)
		{
			SetMessage(fex.Message, true);
		}
		
        catch (Exception ex)
		{
			SetMessage(S_COMMON_ERROR, true);
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to fill footer property and add sort image.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwMenuFilesDetails_DataBound(object sender, EventArgs e)
	{
		try
		{
			if (lstvwMenuFilesDetails.Items.Count > 0)
			{
				lstvwMenuFilesDetails.Items.Clear();
				ControlUtility.FillListViewPagerFooter(lstvwMenuFilesDetails, DtPgCount);
				AddSortImage();

			}
			else
			{
				DtPgCount.Visible = false;
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	///		
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void ddlMenus_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
  
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}
    /// <summary>
    /// This event is used for the Search facility For Parent Menu,Sub Menu And Link Name.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            lstvwMenuFilesDetails.DataSourceID = ObjDSMenuFilesDetails.ID;
            DataPager dtPager = lstvwMenuFilesDetails.FindControl("DtPgDropDown") as DataPager;
            if (dtPager != null)
            {
                DropDownList ddlCnt = (dtPager.Controls[0].FindControl("ddlCnt")) as DropDownList;
                if (ddlCnt != null)
                {
                    ddlCnt.SelectedValue = Constants.S_ONE;
                    cmbPageCnt_SelectedIndexChanged(ddlCnt, e);
                    
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    /// <summary>
    /// This event is used to search the menu name for fill the menu combobox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnTopSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillMenuList();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
   
	#endregion -- EVENT HANDLER(s) --

	#region -- PRIVATE METHOD(s) --

	/// <summary>
	/// Popuplates the Menu dropdownlist.
	/// </summary>
	private void FillMenuList()
	{
		var oConfigureCollectionMenuBL = new ConfigureCollectionMenuBL();
		DataTable oDataTable = oConfigureCollectionMenuBL.FetchAllInternalMenus(miSchoolId,txtTopSearch.Text.Trim(), true);
		if (oDataTable != null && oDataTable.Columns.Contains("InsertDate"))
		{
			oDataTable.DefaultView.Sort = "InsertDate DESC";
			ddlMenus.Bind(oDataTable.DefaultView, "ConfigureMenuId", "MenuName", Constants.S_SELECT);
		}
		else
			ddlMenus.Bind(oDataTable, "ConfigureMenuId", "MenuName", Constants.S_SELECT);
	}

	/// <summary>
	/// This methos is used to set java script attributes.
	/// </summary>
	private void SetJavaScriptAttributes()
	{
		valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
		btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Basic_Configuration));
		ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel, btnBack });
        optFilePath.Checked = true;
        optFilePath.Attributes.Add("onclick", "ShowFileURL()");
        optURL.Attributes.Add("onclick", "ShowFileURL()");
        
	}

	/// <summary>
	/// This method is used to check whether screen is configured or not.
	/// </summary>
	/// <returns></returns>
	private bool IsConfigured()
	{
		return QueryString[Constants.S_IS_CONFIGURED] != null && QueryString[Constants.S_IS_CONFIGURED] == Constants.S_YES;
	}

	/// <summary>
	///		Creates a new menu file or updates and existing one.
	/// </summary>
	/// <param name="abIsNewFile"></param>
	private void SaveMenuFile(bool abIsNewFile)
	{

        var oMenuFile = new MenuFile
            {
                Id = hidMenuFileId.Value.IsNullOrEmpty() ? Constants.I_ZERO : hidMenuFileId.Value.ToInt(),
                Name = txtLinkName.Text.Trim(),
                Menu = new SchoolEntities.Menu { Id = ddlMenus.SelectedValue.ToInt() },
                SchoolId = miSchoolId,
                InsertedById = miUserId,                
                IsURL = optURL.Checked,
        
            };         
            MenuFileBL.SaveMenuFile(oMenuFile, fileUploadItems.PostedFile, abIsNewFile, hidFilePath.Value,txtFileURL.Text.Trim());
		    SetMessage(abIsNewFile ? S_SAVE_MESSAGE : S_UPDATE_MESSAGE, false);
	}

	/// <summary>
	/// This method is used to update menu file.
	/// </summary>
	private void UpdateMenuFile()
	{
        var oMenuFileBL = new MenuFileBL();
		string sOldFilePath = hidFilePath.Value;
		string sServerPath = Server.MapPath("~");
      if (sServerPath.Substring(sServerPath.Length - 2) != "\\")
			sServerPath = sServerPath + "\\";
		File.SetAttributes(sServerPath + sOldFilePath, FileAttributes.Archive);
		bool bHasFile = fileUploadItems.HasFile;
        
       string sNewFilePath = hidFilePath.Value.Substring(0, hidFilePath.Value.LastIndexOf(".")) + fileUploadItems.FileName.Substring(fileUploadItems.FileName.LastIndexOf("."));
		if (bHasFile)
		{
			if (File.Exists(sServerPath + sOldFilePath))
				File.Delete(sServerPath + sOldFilePath);
			fileUploadItems.SaveAs(sServerPath + sNewFilePath);
			MenuFileBL.UpdateFileDetails(miSchoolId, sOldFilePath, sNewFilePath,txtFileURL.Text.Trim());     
		}    
		else
			throw new FileNotFoundException();

        SetMessage(S_UPDATE_MESSAGE, false);
    
	}

	/// <summary>
	///		Deletes the specified menu file.
	/// </summary>
	/// <param name="aiMenuFileId"></param>
	private void DeleteMenuFile(int aiMenuFileId)
	{
		MenuFileBL.DeleteMenuFile(aiMenuFileId);
	}

	/// <summary>
	///		Sets the message informing the user about the staus of the action.
	/// </summary>
	/// <param name="asMessage">The message to be displayed.</param>
	/// <param name="abIsError">Bool indicating if it's an error message.</param>
	private void SetMessage(string asMessage, bool abIsError)
	{
		if (abIsError)
			lblErrorMsg.Text = asMessage;
		else
			lblUpdateSucess.Text = asMessage;

		lblErrorMsg.Visible = abIsError;
		lblUpdateSucess.Visible = !abIsError;
	}

	/// <summary>
	/// This method is used to set default control fields.
	/// </summary>
	private void SetDefaultValuesToControls()
	{
		txtLinkName.Text = string.Empty;
        txtFileURL.Text = string.Empty;
		hidFileName.Value = string.Empty;
		hidFilePath.Value = string.Empty;
		hidNewFileName.Value = string.Empty;
		hidSortExpression.Value = S_DEFAULT_SORT_EXP;
		hidSortDirection.Value = Constants.S_DESCENDING;
		ddlMenus.Enabled = true;
		ddlMenus.ClearSelection();
		hidMode.Value = S_NEW;
		btnSave.Text = S_SAVE;
        optFilePath.Checked = true;
        optURL.Checked = false;
		ddlMenus.Focus();
	}

	/// <summary>
	/// This method is used to set sorting image to list view headers.
	/// </summary>
	private void AddSortImage()
	{
		var oHtmlTableHeaderRow = lstvwMenuFilesDetails.FindControl("trHeader") as HtmlTableRow;
		if (oHtmlTableHeaderRow != null)
			CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
	}

	/// <summary>
	/// This method is used set sort variables.
	/// </summary>
	private void SetSortVariables()
	{
		hidSortDirection.Value = hidSortDirection.Value == Constants.S_DESCENDING ? Constants.S_ASCENDING : Constants.S_DESCENDING;
	}

	/// <summary>
	/// This method is used to load menu file details.
	/// </summary>
	/// <param name="aiMenuFileDetailsId"></param>
	private void LoadMenuFilesDetails(int aiMenuFileDetailsId)
    {
        var oMenuFileBL = new MenuFileBL(aiMenuFileDetailsId);
        ddlMenus.SelectedValue = oMenuFileBL.MenuFileDetails.Menu.Id.ToString();
        ddlMenus.Enabled = false;
        string sFilePath = oMenuFileBL.MenuFileDetails.Path;
        hidFileName.Value = sFilePath.Substring((sFilePath.LastIndexOf("\\") + 1), (sFilePath.Length - sFilePath.LastIndexOf("\\")) - 1);
        hidFileType.Value = hidFileName.Value.Substring(hidFileName.Value.LastIndexOf(".") + 1, hidFileName.Value.Length - hidFileName.Value.LastIndexOf(".") - 1);
        hidFilePath.Value = sFilePath;
        txtLinkName.Text = oMenuFileBL.MenuFileDetails.Name;
        hidMenuFileId.Value = aiMenuFileDetailsId.ToString();
        if (oMenuFileBL.MenuFileDetails.IsURL)
        {
            txtFileURL.Text = sFilePath;
            optURL.Checked = true;
            optFilePath.Checked = false;
        }
        else
        {
            optFilePath.Checked = true;
            optURL.Checked = false;
        }
    }

	/// <summary>
	///		Rebinds the File Menu List.
	/// </summary>
	private void RebindList()
	{
		lstvwMenuFilesDetails.Items.Clear();
		lstvwMenuFilesDetails.DataSourceID = ObjDSMenuFilesDetails.ID;
	}

	#endregion -- PRIVATE METHOD(s) --
}
