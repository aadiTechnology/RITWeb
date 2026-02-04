/*   Author		 : Vishal Shah
 *   Date		 : 10 Sept 2011
 *	 Description : This class is used to display and edit/update Configuration details for Leaving Certificate Report.
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;

public partial class LeavingCertificateConfigUI : SchoolBase
{

    #region -- MEMBER(s) --

    private const string S_SAVE_MESSAGE = "Configuration saved successfully !!!";
    private const string S_ERROR_MESSAGE = "There was an error updating the Configuration.";
    private List<ListItem> oValues = new List<ListItem>();

    
    #endregion -- MEMBER(s) --


    #region -- EVENT(s) --

    /// <summary>
    /// This event is handled to Fill the LCDetails grid and set Javascript attributes for the buttons.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                FillLeavingCertificateGrid();
                RefreshValue();
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValue();
            }
            SetJavaScriptAttributes();
        }
        catch (Exception ex)
        {
            AddExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set control visiblity as per the Configuration details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwLCDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;

                // Set class for Alternate Row
                if (oCurrentItem.DisplayIndex % 2 == 1)
                {
                    HtmlTableRow oHTMLCurrentRow = oCurrentItem.FindControl("trGridRow") as HtmlTableRow;
                    if (oHTMLCurrentRow != null)
                        oHTMLCurrentRow.Attributes.Add("class", "ClsGridAltRow");
                }

                // Select checkbox for enabled items & show mandatory star accordingly
                CheckBox oCheckBox = oCurrentItem.FindControl("chkSelect") as CheckBox;
                if (oCheckBox != null)
                {
                    if (Convert.ToInt32(Constants.S_DEFAUL_SCHOOL_ID) == Convert.ToInt32(lstvwLCDetails.DataKeys[oCurrentItem.DisplayIndex]["SchoolId"]))
                    {
                        TextBox otxtBox = oCurrentItem.FindControl("txtLCDetailsName") as TextBox;
                        HtmlControl mdtStar = oCurrentItem.FindControl("mdtStar") as HtmlControl;
                        HtmlControl mdtStarSortOrder = oCurrentItem.FindControl("mdtStarSortOrder") as HtmlControl;
                        otxtBox.Enabled = false;
                        mdtStar.Style.Add("visibility", "hidden");
                        mdtStarSortOrder.Style.Add("visibility", "hidden");
                    }
                    else
                    {
                        oCheckBox.Checked = true;
                    }
                }

                DropDownList oDropDownList = oCurrentItem.FindControl("ddlSortOrder") as DropDownList;
                if (oDropDownList != null)
                {
                    oDropDownList.DataSource = oValues;
                    oDropDownList.DataBind();
                    if (oCheckBox.Checked)
                    {
                        oDropDownList.SelectedValue = (oCurrentItem.DataItem as LeavingCertificateConfig).SortOrder;
                    }
                    else
                    {
                        oDropDownList.SelectedValue = Constants.S_ZERO;
                        oDropDownList.Enabled = false;
                    }
                   
                }

                TextBox txtDefaultValue = e.Item.FindControl("txtDefaultValue") as TextBox;
                LeavingCertificateConfig oLeavingCertificateConfig = oCurrentItem.DataItem as LeavingCertificateConfig;
                txtDefaultValue.Visible = oLeavingCertificateConfig.IsDefaultValueApplicable;
            }
        }
        catch (Exception ex)
        {
            AddExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to Save/Edit/Update Configuration details to the database.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            List<LeavingCertificateConfig> olstLeavingCertificateConfig = new List<LeavingCertificateConfig>();

            foreach (ListViewDataItem item in lstvwLCDetails.Items)
            {
                CheckBox oCheckBox = item.FindControl("chkSelect") as CheckBox;
                TextBox oTextBox = item.FindControl("txtLCDetailsName") as TextBox;
                TextBox txtDefaultValue = item.FindControl("txtDefaultValue") as TextBox;
                DropDownList oDropDownList=item.FindControl("ddlSortOrder") as DropDownList;
                string sSortOrder= oDropDownList.SelectedValue.ToString();
                if (oCheckBox.Checked)
                {
                    olstLeavingCertificateConfig.Add(new LeavingCertificateConfig()
                    {
                        Name = oTextBox.Text.Trim(),
                        OriginalId = Convert.ToInt32(lstvwLCDetails.DataKeys[item.DisplayIndex]["OriginalId"]),
                        SortOrder=sSortOrder,
                        DefaultValue = txtDefaultValue.Text.Trim()
                    });
                }
            }

            LeavingCertificateConfigBL oLCConfigBL = new LeavingCertificateConfigBL(miSchoolId,
                                                                                    miUserId);
            oLCConfigBL.SaveLeavingCertificateConfig(olstLeavingCertificateConfig);

            FillLeavingCertificateGrid();

            lblUpdateMessage.Text = Resources.LocalizedResources.ConfigurationSavedSuccessfully;
            lblUpdateMessage.Visible = true;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = Resources.LocalizedResources.ThereWasAnErrorUpdatingTheConfiguration;
            lblErrorMessage.Visible = true;
            AddExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion -- EVENT(s) --


    #region -- PRIVATE METHOD(s) --

    /// <summary>
    /// This function sets default values and applies the hover effect for buttons on the page.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        valSummary.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        ApplyMouseHoverEffect(new List<Button> { btnSave });
    }

    /// <summary>
    /// This function fills the LCDetails table with values from the database.
    /// </summary>
    private void FillLeavingCertificateGrid()
    {
        int iSchoolId = miSchoolId;
        List<LeavingCertificateConfig> olstLeavingCertificateConfig = LeavingCertificateConfigBL.GetLeavingCertificateConfigList(iSchoolId);
        if (olstLeavingCertificateConfig.Count>0)
        {
            int iCount=olstLeavingCertificateConfig.Count;
            oValues.Add(new ListItem { Text = Constants.S_SELECT, Value = Constants.S_ZERO});
            for (int iItems = 1; iItems <= iCount; iItems++)
                oValues.Add(new ListItem { Text=iItems.ToString(),Value=iItems.ToString()});
        }
        lstvwLCDetails.DataSource = olstLeavingCertificateConfig;
        lstvwLCDetails.DataBind();
    }

    /// <summary>
    /// This method is used to log an exception to the error log table in the database.
    /// </summary>
    /// <param name="ex"></param>
    /// <param name="currentMethod"></param>
    private void AddExceptionToErrorLog(Exception ex, MethodBase currentMethod)
    {
        int iUserid = miUserId;
        ExceptionHandler.WriteExceptionToErrorLog(String.Format("{0}. Trace: {1}", ex.Message, ex.StackTrace),
                                                  String.Format("{0}.{1}", currentMethod.DeclaringType.FullName, currentMethod.Name),
                                                  iUserid);
      
    }

     /// <summary>
    /// This method used to value based on Culture
    /// </summary>
    private void RefreshValue()
    {
        hidSortOrderRepeatedForTheRowNo.Value = Resources.LocalizedResources.SortOrderRepeatedForTheRowNo;
        hidSortOrderIsMissingForTheRowNo.Value = Resources.LocalizedResources.SortOrderIsMissingForTheRowNo;
    }
    #endregion -- PRIVATE METHOD(s) --

}
