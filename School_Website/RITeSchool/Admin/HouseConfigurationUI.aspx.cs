using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Reflection;
using Utility;
using BusinessLogic;
using BusinessLogic.Exceptions;
using System.Collections.Generic;
using System.Data.SqlClient;
using House;
public partial class HouseConfigurationUI : SchoolBase
{
    #region Data Members

    private HouseCofigurationBL moHouseCofigurationBL;
    const string S_DEFAULT_SORT_EXP = "Name";

    #endregion

    #region Events

    /// <summary>
    /// This event is used to fill existing House listView
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        moHouseCofigurationBL = new HouseCofigurationBL(miSchoolId, miAcademicYearId, miUserId);        
        if (!IsPostBack)
        {
            FillColorCombo();
            FillHouseConfiguration();
            SetJavascriptAttributes();           
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (cmbHouseColor.SelectedIndex != 0)
            {
                if (btnAdd.Text == Constants.ButtonText.Update.ToString())
                {
                    Update();
                    DisplayMessage(Constants.ItemState.updated, false);
                }
                else if (btnAdd.Text == Constants.ButtonText.Save.ToString())
                {
                    Save();
                    DisplayMessage(Constants.ItemState.saved, false);
                }
                FillHouseConfiguration();
                ClearFields();
                if (QueryString["Is_Configured"] != Constants.S_YES)
                    SaveConfigDetails(Constants.SchoolConfigurations.Houses.ToInt());
            }
            else
                DisplayMessage("Color should be selected.", true, tdMessage);
            
        }
        catch (DuplicateEntityException exx)
        {
            DisplayMessage(exx.Message, true, tdMessage);
            SetColor();
        }
        catch (SqlException se)
        {
            DisplayMessage(se.Message, true, tdMessage);
            SetColor();
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
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearFields();        
        SetColor();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, System.EventArgs e)
    {
        MasterPage oMasterPage = (MasterPage)this.Master;
        oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Basic_Configuration)));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwConfigureHouse_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            SetSortVariables();
            SetColor();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set the ListView controls set the serial no for each row of ListView.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwConfigureHouse_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewItem oCurrentItem = (ListViewItem)e.Item;
                Label lblSrNo = (Label)oCurrentItem.FindControl("lblSrNo");
                Label lblColor = (Label)oCurrentItem.FindControl("lblColor");
                lblSrNo.Text = (oCurrentItem.DisplayIndex + 1).ToString();
                ImageButton oimgbtnDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
                oimgbtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");

                var tableRow2 = oCurrentItem.FindControl("tdColor") as HtmlTableCell;
                var tableRow3 = oCurrentItem.FindControl("tdColor1") as HtmlTableCell;
                if (tableRow2 != null)
                    tableRow2.Style.Add(HtmlTextWriterStyle.BackgroundColor, lblColor.Text);
                if (tableRow3 != null)
                    tableRow3.Style.Add(HtmlTextWriterStyle.BackgroundColor, lblColor.Text);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to Edit or Delete Designation Names 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwConfigureHouse_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName != Constants.S_COMMAND_SORT)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iHouseId = Convert.ToInt32(lstvwConfigureHouse.DataKeys[oCurrentItem.DisplayIndex]["Id"]);
                hidHouseConfigurationId.Value = iHouseId.ToString();                
                if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    Delete(iHouseId);
                    DisplayMessage(Constants.ItemState.deleted, false);
                    FillHouseConfiguration();
                    if (lstvwConfigureHouse.Items.Count == 0)
                        DeleteConfigDetails(Constants.SchoolConfigurations.Houses.ToInt());
                    ClearFields();
                }
                else if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    btnAdd.Text = Constants.ButtonText.Update.ToString();
                    HouseConfiguration oHouseConfiguration = moHouseCofigurationBL.Get(iHouseId);
                    {
                        cmbHouseColor.SelectedValue = oHouseConfiguration.Color.ToString();
                        txtHouseName.Text = oHouseConfiguration.Name;
                        txtMotto.Text = oHouseConfiguration.Motto;                     
                       
                    }
                }
                SetColor();
            }
        }
        catch (Exception ex)
        {
            DisplayMessage(ex.Message, true, tdMessage);
        }
    }

  
    #endregion

    #region Private Methods

    /// <summary>
    /// This method is used to fill color combobox;
    /// </summary>
    /// <param name="cmbColors"></param>
    private void FillColorCombo()
    {
        Type tColors = typeof(Color);
        PropertyInfo[] oPropInfoArr = tColors.GetProperties(BindingFlags.Static | BindingFlags.Public);

        ListItem oListItemSelect = new ListItem { Text = Constants.S_SELECT, Value = Constants.S_ZERO };
        oListItemSelect.Attributes.Add("style", "background-color:white;");
        cmbHouseColor.Items.Add(oListItemSelect);

        foreach (PropertyInfo oProperty in oPropInfoArr)
        {
            if (oProperty.DeclaringType.Equals(typeof(Color)))
            {
              ListItem oListItem = new ListItem { Text = oProperty.Name, Value = oProperty.Name };
              oListItem.Attributes.Add("style", "background-color:"+oProperty.Name);
              cmbHouseColor.Items.Add(oListItem);
            }
        }        
    }

    private void SetColor()
    {
        foreach (ListItem oListItem in cmbHouseColor.Items)
        {
            oListItem.Attributes.Add("style", "background-color:" + oListItem.Text);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void FillHouseConfiguration()
    {
        lstvwConfigureHouse.DataSourceID = ObjDSConfigureHouse.ID;
        lstvwConfigureHouse.DataBind();
    }

    /// <summary>
    /// This method is used to save House Details
    /// </summary>
    private void Save()
    {
        HouseConfiguration oHouseConfiguration = Populate();
        moHouseCofigurationBL.Insert(oHouseConfiguration);      
    }

    /// <summary>
    /// This method is used to Update House Details
    /// </summary>
    private void Update()
    {
        HouseConfiguration oHouseConfiguration = Populate();
        if (!string.IsNullOrEmpty(hidHouseConfigurationId.Value))
        {
            oHouseConfiguration.Id = Convert.ToInt32(hidHouseConfigurationId.Value);
            moHouseCofigurationBL.Update(oHouseConfiguration);           
        }
    }

    /// <summary>
    /// This method is used to Delete House Details
    /// </summary>
    /// <param name="iHouseId"></param>
    private int Delete(int aiHouseId)
    {
        return moHouseCofigurationBL.Delete(aiHouseId);
    }

    /// <summary>
    /// This method is used fill entities to add or update from screen.
    /// </summary>
    /// <returns></returns>
    private HouseConfiguration Populate()
    {
        HouseConfiguration oHouseConfiguration = new HouseConfiguration
        {
            Name = txtHouseName.Text.ToString(),
            Color = cmbHouseColor.SelectedItem.ToString(),
            Motto = txtMotto.Text.Trim()
        };
        return oHouseConfiguration;
    }

    /// <summary>
    /// This method is used to display message.
    /// </summary>
    /// <param name="aoItemState"></param>
    /// <param name="abIsErrorMessage"></param>
    private void DisplayMessage(Constants.ItemState aoItemState, bool abIsErrorMessage)
    {
        string sMessage = "House details " + aoItemState.ToString() + " successfully!!!";
        DisplayMessage(sMessage, abIsErrorMessage, tdMessage);
        SetColor();
    }

    /// <summary>
    /// This Method is used to clear form fields.
    /// </summary>
    private void ClearFields()
    {
        txtHouseName.Text = string.Empty;
        txtMotto.Text = string.Empty;        
        cmbHouseColor.SelectedIndex = 0;      
        btnAdd.Text = Constants.ButtonText.Save.ToString();
    }

    /// <summary>
    /// This method is used to set JavaScript attributes
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnCancel, btnAdd, btnBack });
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        btnAdd.Attributes.Add("Onclick", "ClearSuccessfulMessage()");
        cmbHouseColor.Attributes.Add("onclick", "ChangeColor(this)");
        lnkHouseConfig.Attributes.Add("onclick", "OpenPopup(); return false;");
        btnAdd.Text = Constants.ButtonText.Save.ToString();
        SetDefaultValues();
        txtHouseName.Focus();
    }

    /// <summary>
    /// This method is used set sort variables.
    /// </summary>
    private void SetSortVariables()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to set sorting image to list view headers.
    /// </summary>
    private void AddSortImage()
    {
        if (lstvwConfigureHouse.SortDirection.ToString() == "Ascending" || lstvwConfigureHouse.SortDirection.ToString() == string.Empty)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
        if (lstvwConfigureHouse.SortExpression != string.Empty)
            hidSortExpression.Value = lstvwConfigureHouse.SortExpression.ToString();
        else
            hidSortExpression.Value = S_DEFAULT_SORT_EXP;

        HtmlTableRow oHtmlTableHeaderRow = lstvwConfigureHouse.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        AddSortImage();
        hidSortExpression.Value = S_DEFAULT_SORT_EXP;
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        hidSortDirection.Value = SortDirection.Ascending.ToString();
    }
    #endregion
   
}