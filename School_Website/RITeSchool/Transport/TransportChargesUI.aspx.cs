using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;
using BusinessLogic.Exceptions;
using System.Reflection;
using BookEntities;
using BusinessLogic;

public partial class TransportChargesUI : SchoolBase
{
    #region Event(s)

    #region -- CONSTANT(s) --

    private const string S_SHOW = "Show";
    private const string S_CHANGE_INPUT = "Change Filter";    

    #endregion -- CONSTANT(s) --

    /// <summary>
    /// This event is used to load the basic details like user roles and setting default properties.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                hidShow.Value = S_SHOW;
                SetDefaultValues();
                FillUserRoles();
                ReadQueryString();
                if (cmbRole.SelectedValue != Constants.S_ZERO)
                    FillUsers();
                SetDefaultButton(btnShow);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }   

    /// <summary>
    /// This event is used to fill the listview pager and footer.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwUser_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwUser.Items.Count > 0)
                ControlUtility.FillListViewPagerFooter(lstvwUser, DtPgCount);
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle the databound event. It will bind the pay and refund link details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwUser_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = oCurrentItem.DisplayIndex;
                var imgBtnEdit = oCurrentItem.FindControl("imgBtnEdit") as ImageButton;
                var imgBtnRefund = oCurrentItem.FindControl("imgBtnRefund") as ImageButton;                
                var oDtPgr = lstvwUser.FindControl("DtPgDropDown") as DataPager;

                int iPageIndex = (oDtPgr.StartRowIndex / oDtPgr.PageSize) + 1;
                int iUserId = lstvwUser.DataKeys[iRowId]["UserId"].ToInt();
                string sName = Convert.ToString(lstvwUser.DataKeys[iRowId]["Name"]);

                string sQueryString = String.Format("UserId={0}&Name={1}&RegNo={2}&pIndex={3}&RoleId={4}",
                                                    iUserId,
                                                    sName,
                                                    txtName.Text,
                                                    iPageIndex,
                                                    cmbRole.SelectedValue.ToInt()
                                                    );

                string sRefundQueryString = String.Format(sQueryString + "&IsRefund={0}", true);
                imgBtnEdit.Attributes.Add("onclick", "if(!OpenPopup( '../Transport/PayTransportChargesPopUp.aspx?" + CommonUtility.EncryptQuerystring(sQueryString) + "' )) return false;");
                imgBtnRefund.Attributes.Add("onclick", "if(!OpenPopup( '../Transport/PayTransportChargesPopUp.aspx?" + CommonUtility.EncryptQuerystring(sRefundQueryString) + "' )) return false;");                
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event i sused to show the transport fee details for selected criteria.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (hidShow.Value == S_SHOW)
            {
                ToggleListView(true);
                FillUsers();
                btnShow.Text = Resources.LocalizedResources.ChangeFilter;
                hidShow.Value = S_CHANGE_INPUT;
                txtName.Enabled = false;
                cmbRole.Enabled = false;
                btnShow.Focus();
                btnShow.TabIndex = 1;
            }
            else
            {
                ToggleListView(false);
                btnShow.Text = Resources.LocalizedResources.Show;
                hidShow.Value = S_SHOW;
                if(cmbRole.SelectedValue != Constants.S_ZERO)
                    txtName.Enabled = true;
                cmbRole.Enabled = true;
                cmbRole.TabIndex = 1;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }    

    /// <summary>
    /// This event used set paging for listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwUser);            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set the control properties after changing role.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbRole.SelectedItem.Value != Constants.S_ZERO)
                txtName.Enabled = true;
            else
            {
                txtName.Enabled = false;
                txtName.Text = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion
    
    #region Public Method(s)

    /// <summary>
    /// This method is used to read query string on page load.
    /// </summary>
    private void ReadQueryString()
    {
        if (Request.QueryString.ToString() == Constants.S_EMPTY_STRING)
            return;

        if (!QueryString["pIndex"].IsNull())
            hidPageIndex.Value = QueryString["pIndex"].ToString();

        if (!QueryString["RoleId"].IsNull())
        {
            cmbRole.SelectedValue = QueryString["RoleId"].ToString();
            cmbRole.Enabled = false;
            hidRoleId.Value = QueryString["RoleId"].ToString();
        }

        if (!QueryString["RegNo"].IsNull())
            txtName.Text = QueryString["RegNo"].ToString();

        hidShow.Value = S_CHANGE_INPUT;
        btnShow.Text = Resources.LocalizedResources.ChangeFilter;        
    }

    /// <summary>
    /// This method is used make list view visible or hide it.
    /// </summary>
    /// <param name="abAction"></param>
    private void ToggleListView(bool abAction)
    {
        lstvwUser.DataSourceID = null;
        lstvwUser.Visible = abAction;
        trTotalRec.Visible = abAction;
    }

    /// <summary>
    /// This method sets registration no. and fee not paid option buttons checked by default.
    /// </summary>
    private void SetDefaultValues()
    {
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        cmbRole.Focus();        
        txtName.Attributes.Add("onkeypress", string.Format("return clickButton(event,'{0}')", btnShow.ClientID));
        ApplyMouseHoverEffect(new List<Button> { btnShow });
    }

    /// <summary>
    /// This method is used to fill all the transport user roles into user role dropdown list.
    /// </summary>
    private void FillUserRoles()
    {
        List<UserRoles> lstUserRole = MasterDataCollectionBL.GetAllRoles();
        lstUserRole = lstUserRole.Where(a => a.User_Role_Id != Constants.UserRoles.TransportStaff.ToInt() && a.User_Role_Id != Constants.UserRoles.Parent.ToInt()).ToList();
        ListSource.FillDropDownList(lstUserRole, cmbRole, "User_Role_Name", "User_Role_Id", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill the searched user details for selected role.
    /// </summary>
    private void FillUsers()
    {       
        lstvwUser.DataSourceID = objDSUserList.ID;
        lstvwUser.DataBind();
    }

    #endregion 
}