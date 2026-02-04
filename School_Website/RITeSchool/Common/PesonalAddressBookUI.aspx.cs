using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Xml;
using BusinessLogic;
using Utility;
using BusinessLogic.Exceptions;
using System.Reflection;

public partial class RITeSchool_Common_PesonalAddressBookUI : SchoolBase
{
    #region Constants

    const string S_SELECT_AT_LEAST_ONE_USER = "No user is selected. Are you sure you want to continue?";
    private const string S_CONST_EDITADDRESS = "EditAddress";
    private const string S_CONST_DELETEADDRESS = "DeleteAddress";

    #endregion

    #region Events

    /// <summary>
    /// This method is used to initialize page by handling page load event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                InitializePage();
                SetControlsForIndividual();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to handle data bound event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwAddressBook_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwAddressBook.Items.Count > 0)
            {
                HtmlTableRow oHtmlTableRowMain = (HtmlTableRow)lstvwAddressBook.FindControl("trMainHeader");
                CheckBox oCheckBoxMain = (CheckBox)oHtmlTableRowMain.FindControl("chkSelect");
                oCheckBoxMain.Checked = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to handle data bound event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwGroup_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwGroup.Items.Count > 0)
            {
                HtmlTableRow oHtmlTableRowGroup = (HtmlTableRow)lstvwGroup.FindControl("trGroupHeader");
                CheckBox oCheckBoxGroup = (CheckBox)oHtmlTableRowGroup.FindControl("chkSelect");
                oCheckBoxGroup.Checked = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to handle data bound event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwGroupDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwGroupDetails.Items.Count > 0)
            {
                HtmlTableRow oHtmlTableRow = (HtmlTableRow)lstvwGroupDetails.FindControl("trDetailsHeader");
                CheckBox oCheckBox = (CheckBox)oHtmlTableRow.FindControl("chkSelect");
                oCheckBox.Checked = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to get item command event and do operation according to command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwAddressBook_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == S_CONST_EDITADDRESS)
            {
                btnAdd.CommandName = "Update";
                btnAdd.Text = btnAdd.CommandName;
                btnAdd.CommandArgument = ((ImageButton)(e.CommandSource)).CommandArgument;

                Label olblName = e.Item.FindControl("lblName") as Label;
                txtUserName.Text = olblName.Text;

                Label olblMobileNo = e.Item.FindControl("lblMobileNo") as Label;
                txtUserMobileNo.Text = olblMobileNo.Text;
            }
            else if (e.CommandName == S_CONST_DELETEADDRESS)
            {
                int iPersonalAddressBookId = Convert.ToInt32(((ImageButton)(e.CommandSource)).CommandArgument);
                PersonalAddressBookBL oPersonalAddressBookBL = new PersonalAddressBookBL();
                oPersonalAddressBookBL.PersonalAddressBookId = iPersonalAddressBookId;
                oPersonalAddressBookBL.Updated_By_Id = miUserId;
                oPersonalAddressBookBL.Update_Date = DateTime.Now;
                oPersonalAddressBookBL.Is_Deleted = true;
                oPersonalAddressBookBL.DeletePersonalAddressBook();
                ClearControls();
            }
            lstvwAddressBook.DataSourceID = ObjDSPersonalAddBook.ID;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to handle item commands 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwGroup_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == S_CONST_EDITADDRESS)
            {
                btnAdd.CommandName = "Update";
                btnAdd.Text = btnAdd.CommandName;
                btnAdd.CommandArgument = ((ImageButton)(e.CommandSource)).CommandArgument;

                Label olblName = e.Item.FindControl("lblName") as Label;
                txtUserName.Text = olblName.Text;

                hidGroupID.Value = btnAdd.CommandArgument.ToString();
                lstvwGroupDetails.DataSourceID = ObjDSGroupDetails.ID;
                lstvwGroupDetails.DataBind();
            }
            else if (e.CommandName == S_CONST_DELETEADDRESS)
            {
                int iPersonalAddressBookGroupId = Convert.ToInt32(((ImageButton)(e.CommandSource)).CommandArgument);                
                PersonalAddressBookBL oPersonalAddressBookBL = new PersonalAddressBookBL();
                oPersonalAddressBookBL.DeletePersonalAddressBookGroup(iPersonalAddressBookGroupId, miUserId);

                hidGroupID.Value = "0";
                lstvwGroupDetails.DataSourceID = ObjDSGroupDetails.ID;
                lstvwGroupDetails.DataBind();
                ClearControls();
                lstvwGroup.DataSourceID = ObjDSGroup.ID;
                lstvwGroup.DataBind();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to handle checked changed event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optGroup_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (optGroup.Checked)
                SetControlsForGroup();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to get item command event and do operation according to command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optIndividual_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (optIndividual.Checked)
                SetControlsForIndividual();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to handle add event and add new mobile address.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (optIndividual.Checked)
            {
                PersonalAddressBookBL oPersonalAddressBookBL = GetPersonalAddressBookBL();
                String sAlreadyExistsErr = oPersonalAddressBookBL.CheckIfAlreadyExists();
                if (sAlreadyExistsErr == string.Empty)
                {
                    if (btnAdd.CommandName == "Add")
                        oPersonalAddressBookBL.InsertPersonalAddressBook();
                    else if (btnAdd.CommandName == "Update")
                        oPersonalAddressBookBL.UpdatePersonalAddressBook();
                    lstvwAddressBook.DataSourceID = ObjDSPersonalAddBook.ID;
                    ClearControls();
                }
                else
                {
                    lblError.Text = sAlreadyExistsErr;
                    lblError.Visible = true;
                }
            }
            else
            {
                PersonalAddressBookBL oPersonalAddressBookBL = new PersonalAddressBookBL();
                int iPersonalBookGroupId = Convert.ToInt32(hidGroupID.Value);                
                String sAlreadyExistsErr = oPersonalAddressBookBL.CheckIfGroupAlreadyExists(iPersonalBookGroupId, txtUserName.Text.Trim(), miUserId);
                if (sAlreadyExistsErr == string.Empty)
                {
                    string sGroupDetailXML = GetGroupDetailXML();
                    string sGroupName = txtUserName.Text.Trim();
                    if (btnAdd.CommandName == "Add")
                        oPersonalAddressBookBL.InsertPersonalAddressBookGroup(sGroupName, sGroupDetailXML, miUserId);
                    else if (btnAdd.CommandName == "Update")
                    {
                        int iGroupID = Convert.ToInt32(hidGroupID.Value);
                        oPersonalAddressBookBL.UpdatePersonalAddressBookGroup(iGroupID, sGroupName, sGroupDetailXML, miUserId);
                    }
                    lstvwGroup.DataSourceID = ObjDSGroup.ID;
                    lstvwGroupDetails.DataSourceID = ObjDSGroupDetails.ID;
                    ClearControls();
                    hidGroupID.Value = "0";
                }
                else
                {
                    lblError.Text = sAlreadyExistsErr;
                    lblError.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to handle cancel event and clear mobile address input controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ClearControls();
            if (optGroup.Checked)
            {
                hidGroupID.Value = "0";
                lstvwGroupDetails.DataSourceID = ObjDSGroupDetails.ID;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to handle OK button event and generate list of selected mobile number.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgBtnOk_Click(object sender, EventArgs e)
    {
        try
        {
            GenerateMobileNumberList();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// This method is used to initialize page 
    /// </summary>
    private void InitializePage()
    {
        ApplyMouseHoverEffect(new List<Button> { btnAdd, btnCancel, btnCloseBottom, btnCloseUp, imgBtnOKBottom, imgBtnOKUp });
        valSumAddressBook.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        imgBtnOKUp.Attributes.Add("Onclick", String.Format("if(!(ConfirmAction('false','{0}'))){{return false;}}", S_SELECT_AT_LEAST_ONE_USER));
        imgBtnOKBottom.Attributes.Add("Onclick", String.Format("if(!(ConfirmAction('false','{0}'))){{return false;}}", S_SELECT_AT_LEAST_ONE_USER));       
        optIndividual.Checked = true;
    }

    /// <summary>
    /// This method get personal address book object.
    /// </summary>
    /// <returns></returns>
    private PersonalAddressBookBL GetPersonalAddressBookBL()
    {
        PersonalAddressBookBL oPersonalAddressBookBL = new PersonalAddressBookBL();
        oPersonalAddressBookBL.PersonalAddressBookId = Convert.ToInt32(btnAdd.CommandArgument);
        oPersonalAddressBookBL.Name = txtUserName.Text.Trim();
        oPersonalAddressBookBL.Mobile_No = txtUserMobileNo.Text.Trim();
        oPersonalAddressBookBL.User_Id = miUserId;
        oPersonalAddressBookBL.Inserted_By_id = miUserId;
        oPersonalAddressBookBL.Insert_Date = DateTime.Now;
        oPersonalAddressBookBL.Updated_By_Id = miUserId;
        oPersonalAddressBookBL.Update_Date = DateTime.Now;
        return oPersonalAddressBookBL;
    }

    /// <summary>
    /// This method is used to clear controls
    /// </summary>
    private void ClearControls()
    {
        btnAdd.CommandArgument = "0";
        btnAdd.CommandName = "Add";
        btnAdd.Text = btnAdd.CommandName;
        txtUserName.Text = string.Empty;
        txtUserMobileNo.Text = string.Empty;
    }

    /// <summary>
    /// This method is used to generate list of users for mobile numbers selected.
    /// </summary>
    private void GenerateMobileNumberList()
    {
        StringBuilder osbMobileNumbers = new StringBuilder();
        String sMobileNos = hidMobileNos.Value.Trim();
        string[] sArrMobileNUmbers = sMobileNos.Split(',');
        if (optIndividual.Checked)
        {
            foreach (ListViewItem oListViewItem in lstvwAddressBook.Items)
            {
                CheckBox ochkSelect = oListViewItem.FindControl("chkSelect") as CheckBox;
                if (ochkSelect.Checked)
                {
                    Label olblMobileNo = oListViewItem.FindControl("lblMobileNo") as Label;
                    ///Check if that mobile number is already contained in selcted mobile number list.
                    ///and if not contain then add it with comma saparated list.
                    if (!Array.Exists<string>(sArrMobileNUmbers, delegate(String sMobilNo)
                    {
                        return olblMobileNo.Text.Trim() == sMobilNo;
                    }) && !osbMobileNumbers.ToString().Contains(olblMobileNo.Text.Trim()))
                    {
                        osbMobileNumbers.Append(olblMobileNo.Text.Trim());
                        osbMobileNumbers.Append(", ");
                    }
                }
            }
        }
        else
        {
            string sGroupIds = "";
            foreach (ListViewDataItem oListViewItem in lstvwGroup.Items)
            {
                CheckBox ochkSelect = oListViewItem.FindControl("chkSelect") as CheckBox;
                int iRowId = Convert.ToInt32(oListViewItem.DataItemIndex);
                if (ochkSelect.Checked)
                    sGroupIds += (lstvwGroup.DataKeys[iRowId]["PersonalAddressBookGroupId"]).ToString() + ",";
            }
            if (sGroupIds != "")
            {
                sGroupIds = sGroupIds.Substring(0, sGroupIds.LastIndexOf(','));
                PersonalAddressBookBL oPersonalAddressBookBL = new PersonalAddressBookBL();
                DataTable oDataTable = new DataTable();
                oDataTable = oPersonalAddressBookBL.GetDetailsOfGroups(sGroupIds);

                if (oDataTable.Rows.Count > 0)
                {
                    for (int iCount = 0; iCount < oDataTable.Rows.Count; iCount++)
                    {
                        if (!Array.Exists<string>(sArrMobileNUmbers, delegate(String sMobilNo)
                        {
                            return oDataTable.Rows[iCount]["Mobile_No"].ToString() == sMobilNo;
                        }) && !osbMobileNumbers.ToString().Contains(oDataTable.Rows[iCount]["Mobile_No"].ToString()))
                        {
                            osbMobileNumbers.Append(oDataTable.Rows[iCount]["Mobile_No"].ToString());
                            osbMobileNumbers.Append(", ");
                        }
                    }
                }
            }
        }

        if (osbMobileNumbers.Length > 2)
            osbMobileNumbers.Remove(osbMobileNumbers.Length - 2, 2);

        if (sMobileNos.EndsWith(","))
            sMobileNos = sMobileNos.Remove(sMobileNos.Length - 1, 1);

        if (sMobileNos.Length >= 10)
            sMobileNos = String.Format(sMobileNos + ", {0}", osbMobileNumbers);
        else
            sMobileNos = osbMobileNumbers.ToString();
        ///Call parents calling page's javascript function and pass the selected mobile number list.
        Response.Write(String.Format("<Script  type='text/javascript'>window.opener.setManualNumbers('{0}');</Script>", sMobileNos));
        Response.Write("<Script type='text/javascript'>window.close();</Script>");
    }

    /// <summary>
    /// Sets controls when group radiobutton selected
    /// </summary>
    private void SetControlsForGroup()
    {
        lstvwGroup.DataSourceID = ObjDSGroup.ID;
        lstvwGroup.DataBind();
        trMobileNumber.Visible = false;
        lstvwGroupDetails.Visible = true;
        lstvwAddressBook.Visible = false;
        lstvwGroup.Visible = true;
        lstvwGroupDetails.DataSourceID = ObjDSGroupDetails.ID;
        lstvwGroupDetails.DataBind();
        lblTitle.Text = "Add or Update Phone Book Contact Group";
        txtUserMobileNo.Text = string.Empty;
        txtUserName.Text = string.Empty;
        btnAdd.Text = "Add";
        btnAdd.CommandName = "Add";
    }

    /// <summary>
    /// Sets controls when radiobutton individual selected
    /// </summary>
    private void SetControlsForIndividual()
    {
        trMobileNumber.Visible = true;
        lstvwAddressBook.Visible = true;
        lstvwGroup.Visible = false;
        lstvwGroupDetails.Visible = false;
        lstvwAddressBook.DataSourceID = ObjDSPersonalAddBook.ID;
        lstvwAddressBook.DataBind();
        lblTitle.Text = "Add or Update Phone Book Contact";
        txtUserMobileNo.Text = string.Empty;
        txtUserName.Text = string.Empty;
        btnAdd.Text = "Add";
        btnAdd.CommandName = "Add";
    }

    /// <summary>
    /// Gets xml of details of selected group
    /// </summary>
    /// <returns></returns>
    private string GetGroupDetailXML()
    {
        const string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("PersonalAddressBook");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "PersonalAddressBook", "");

        // Loop through all the list view items.
        foreach (ListViewDataItem oListViewDataItem in lstvwGroupDetails.Items)
        {
            int iRowId = Convert.ToInt32(oListViewDataItem.DataItemIndex);
            CheckBox oCheckBox = (CheckBox)oListViewDataItem.FindControl("chkSelect");

            if (oCheckBox.Checked)
            {
                // Create root xml element.
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "PersonalAddressBook", "");
                string sPersonalAddressBookId = (lstvwGroupDetails.DataKeys[iRowId]["PersonalAddressBookId"]).ToString();

                string sAtrrName = "PersonalAddressBookId";
                XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = sPersonalAddressBookId;
                oXmlNode.Attributes.Append(attr);
                oXmlRootNode.AppendChild(oXmlNode);
            }
        }
        root.AppendChild(oXmlRootNode);
        return root.InnerXml;
    }
    #endregion    
}
