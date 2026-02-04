/* Class Name :- StudentSiblingDetailsUI.aspx.cs
 * Created By :- Shobha
 * Created Date :- 19-Nov-2010
 * Description :- This class is used to save sibling details of student.
 * Modified :- 05 July 2012
 * By :- Rohini
 * Desc. :- Dispalying added sibling for new student before saving.
*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using StudentEntities;
using Utility;

public partial class StudentSiblingDetailsUI : SchoolBase
{
    #region "Constants"

    private const string S_COMMAND_REMOVE = "REMOVE";
    private const string S_TEXT_SUBMIT = "Submit";
    private const string S_STATUS_MSG = "No Sibling Details are added.";

    #endregion

    #region Data Members

    StudentSiblingDetailsBL moStudentSiblingDetailsBL;

    #endregion

    #region "Events"

    /// <summary>
    /// This event is used to initialise the controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            moStudentSiblingDetailsBL = new StudentSiblingDetailsBL();
            if (!IsPostBack)
            {
                ReadQueryString();
                InitializeForm();
                SetJavascriptAttributes();
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                    RefreshValue();
                }
            }

            txtStudentName.Focus();
            lblUpdateSucess.Visible = false;
            SetDefaultButton(btnSearch);
           
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set the confirmation message before deleting the records.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwSiblingDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = oCurrentItem.DisplayIndex;
                DataRowView oDataRowView = oCurrentItem.DataItem as DataRowView;
                ImageButton oImgBtnDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
                oImgBtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
                if (((StudentEntities.StudentInfo)oCurrentItem.DataItem).IsLeftStudent.ToString() != Constants.I_ZERO.ToString())
                {
                    HtmlTable oHtmlTable = lstvwSiblingDetails.FindControl("tblStopInfo") as HtmlTable;
                    HtmlTableRow oHtmlTableRow = e.Item.FindControl("Tr2") as HtmlTableRow;
                    oHtmlTableRow.Style.Add("background-color", "Gainsboro");
                    oHtmlTableRow.Style.Add(HtmlTextWriterStyle.Color, "red");
                }

                ImageButton imgBtnDelete = oCurrentItem.FindControl("imgBtnDelete") as ImageButton;
                if (((StudentEntities.StudentInfo)oCurrentItem.DataItem).StudentSiblingId.ToString() == Constants.I_ZERO.ToString())
                    imgBtnDelete.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());            
        }
    }

    /// <summary>
    /// This event is used to set delete the added sibling records.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwSiblingDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == S_COMMAND_REMOVE)
            {
                if (hidMode.Value != "Temp")
                {
                    ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                    int iListIndex = oCurrentItem.DisplayIndex;
                    int iYearwiseSiblingStudentId = Convert.ToInt32(lstvwSiblingDetails.DataKeys[iListIndex]["YearwiseStudentId"]);
                    int iSiblingStudentId = Convert.ToInt32(lstvwSiblingDetails.DataKeys[iListIndex]["StudentSiblingId"]);
                    DeleteSiblingDetails(iYearwiseSiblingStudentId, miSchoolId, miAcademicYearId, iSiblingStudentId);
                    FillSiblingDetails();
                    FillStudentListView();
                }
                else
                {
                    ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                    string sStudentId = lstvwSiblingDetails.DataKeys[oCurrentItem.DisplayIndex]["YearwiseStudentId"].ToString();
                    if (hidSiblingStudentId.Value.Contains(sStudentId))
                        hidSiblingStudentId.Value = hidSiblingStudentId.Value.Replace(sStudentId, string.Empty).Trim();
                    FillTemporarySiblingsList();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());           
        }
    }

    /// <summary>
    /// This event is used to search the students to get siblings with Name or Register number filter.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            DtPgCount.SetPageProperties(0, Constants.I_GRID_PAGE_COUNT, false);
            FillStudentListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());            
        }
    }

    /// <summary>
    /// This event is used to fill list view pager and footer.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStudentList_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwStudentList.Items.Count > 0)
            {
                FillPageNoCombo(lstvwStudentList, DtPgCount);
                btnSave.Visible = true;
                DataPager oDataPager = lstvwStudentList.FindControl("DtPgDropDown") as DataPager;
                int iCurrentPage = (oDataPager.StartRowIndex / oDataPager.PageSize) + 1;
                hidPageNo.Value = iCurrentPage.ToString();
            }
            else
            {
                DtPgCount.Visible = false;
                btnSave.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());            
        }
    }

    protected void lstvwStudentList_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
            CheckBox chkSiblingName = oCurrentItem.FindControl("ChkSelect") as CheckBox;

            string[] SiblingStudents = hidSiblingStudentId.Value.Split(',');
            if (SiblingStudents.Length > 0 && SiblingStudents.Contains((lstvwStudentList.DataKeys[oCurrentItem.DisplayIndex]["YearwiseStudentId"]).ToString()))
                chkSiblingName.Checked = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to set Datpager.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwStudentList);
            FillStudentListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save the student sibling details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnSave.Text != Resources.LocalizedResources.Submit)
            {
                string sStudentSiblingsXML = GenerateStudentSiblingsXML();
                moStudentSiblingDetailsBL.StudentInfoEntity = PopulateStudentSiblingDetailsBL();
                moStudentSiblingDetailsBL.SaveStudentSiblingDetails(sStudentSiblingsXML);
                lblUpdateSucess.Visible = true;
                lblUpdateSucess.Text = Resources.LocalizedResources.MsgSiblingDetailsSavedSuccessfully;
                FillSiblingDetails();
                FillStudentListView();
            }
            else
            {
                string sYearwiseStudentList = string.Empty;

                foreach (ListViewDataItem lstDataItem in lstvwStudentList.Items)
                {
                    CheckBox chkSiblingName = lstDataItem.FindControl("ChkSelect") as CheckBox;
                    int iRowCount = lstDataItem.DisplayIndex;
                    if (chkSiblingName.Checked)
                    {
                        if (sYearwiseStudentList == string.Empty)
                            sYearwiseStudentList = lstvwStudentList.DataKeys[iRowCount]["YearwiseStudentId"].ToString();
                        else
                            sYearwiseStudentList += "," + lstvwStudentList.DataKeys[iRowCount]["YearwiseStudentId"].ToString();

                        if (hidDummyName.Value.IsNullOrEmpty())
                            hidDummyName.Value = lblStudentName.Text;
                        else
                            hidDummyName.Value += "," + lblStudentName.Text;
                    }
                }

                if (!string.IsNullOrEmpty(hidSiblingStudentId.Value))
                    if (!sYearwiseStudentList.Contains(hidSiblingStudentId.Value))
                    {
                        sYearwiseStudentList += "," + hidSiblingStudentId.Value;
                        hidSiblingStudentId.Value = sYearwiseStudentList;
                    }

                string sSiblingNames = moStudentSiblingDetailsBL.GetSiblingNames(miSchoolId, miAcademicYearId, sYearwiseStudentList);
                if (sSiblingNames.IsNullOrEmpty())
                    sSiblingNames = hidDummyName.Value;
                ScriptManager.RegisterStartupScript(btnSave, this.GetType(), "CloseWin", "CloseWindow1('" + sSiblingNames.ToString().Replace("'","%*") + "');", true); 
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());           
        }
    }

    #endregion

    #region "Private Methods"

    /// <summary>
    /// This method is used to read query string.
    /// </summary>
    private void ReadQueryString()
    {
        hidStandardId.Value = QueryString["StandardId"];
        hidDivisionId.Value = QueryString["DivisionId"];
        hidYearWiseStudentId.Value = QueryString["StudentId"];
        hidSiblingStudentId.Value = QueryString["SiblingStudentId"] ?? String.Empty;
    }

    /// <summary>
    /// This method is used to set the javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        lblStudentName.Focus();
        ApplyMouseHoverEffect(new List<Button>{ btnClose, btnSave, btnSearch });
        btnSave.Attributes.Add("onclick", "if(!ConfirmForStudent())return false;");
    }

    /// <summary>
    /// This method is used to initialise the form.
    /// </summary>
    private void InitializeForm()
    {
        btnSave.Visible = false;
        if (hidYearWiseStudentId.Value != Constants.I_ZERO.ToString())
        {
            moStudentSiblingDetailsBL = new StudentSiblingDetailsBL(Convert.ToInt32(hidYearWiseStudentId.Value), miSchoolId, miAcademicYearId);
            StudentInfo oStudentInfo = moStudentSiblingDetailsBL.StudentInfoEntity;
            lblStudentName.Text = oStudentInfo.StudentName;
            hidClassName.Value = oStudentInfo.ClassName;
            hidStudentId.Value = Convert.ToString(oStudentInfo.SchoolwiseStudentId);
            FillSiblingDetails();
        }
        else
        {
            if (!string.IsNullOrEmpty(hidSiblingStudentId.Value))
            {
                trStudentName.Visible = false;
                FillTemporarySiblingsList();
                btnSave.Text = Resources.LocalizedResources.Submit;
            }
            else
            {
                trStudentName.Visible = false;
                lblNorecord.Visible = true;
                lblNorecord.Text = Resources.LocalizedResources.MsgNoSiblingDetailsAdded;
                btnSave.Text = Resources.LocalizedResources.Submit;
            }
        }
        if (moUserRole != Constants.UserRoles.Admin)
            hidUserHasFullAccess.Value = CommonUtility.IsUserHasScreenAccess(Constants.SchoolConfigurations.Student).ToString();
    }

    /// <summary>
    /// This method is used to show sibling details while adding new students.
    /// </summary>
    private void FillTemporarySiblingsList()
    {
        List<StudentInfo> lstStudentInfo = new List<StudentInfo>();
        hidSiblingStudentId.Value = hidSiblingStudentId.Value.TrimEnd(',').TrimStart(',').Trim();
        string[] iSiblingIds = hidSiblingStudentId.Value.Split(',');
        for (int iSiblingCnt = 0; iSiblingCnt < iSiblingIds.Length; iSiblingCnt++)
        {
            int iSiblingId = iSiblingIds[iSiblingCnt] != string.Empty ? Convert.ToInt32(iSiblingIds[iSiblingCnt]) : 0;
            StudentSiblingDetailsBL oStudentSiblingDetailsBL = new StudentSiblingDetailsBL(iSiblingId, miSchoolId, miAcademicYearId);
            if (oStudentSiblingDetailsBL.StudentInfoEntity != null)
            {
                oStudentSiblingDetailsBL.StudentInfoEntity.StudentSiblingId = 0;
                oStudentSiblingDetailsBL.StudentInfoEntity.IsLeftStudent = 0;
                lstStudentInfo.Add(oStudentSiblingDetailsBL.StudentInfoEntity);
            }
        }

        lstvwSiblingDetails.DataSource = lstStudentInfo;
        lstvwSiblingDetails.DataBind();

        hidMode.Value = "Temp";
        for (int iSiblingCnt = 0; iSiblingCnt < lstvwSiblingDetails.Items.Count; iSiblingCnt++)
        {
            ImageButton imgBtnDelete = lstvwSiblingDetails.Items[iSiblingCnt].FindControl("imgBtnDelete") as ImageButton;
                imgBtnDelete.Visible = true;
        }

        SetVisibilityOfControls();
    }

    /// <summary>
    /// This method is used to show status message.
    /// </summary>
    private void SetVisibilityOfControls()
    {
        if (lstvwSiblingDetails.Items.Count <= 0)
        {
            lblNorecord.Visible = true;
            lblNorecord.Text = Resources.LocalizedResources.MsgNoSiblingDetailsAdded;
            hidSiblingRowCount.Value = "0";
        }
        else
        {
            lblNorecord.Visible = false;
            hidSiblingRowCount.Value = lstvwSiblingDetails.Items.Count.ToString();
        }
    }

    /// <summary>
    /// This method is used to fill the ListBox with already added sibling details.
    /// </summary>
    private void FillSiblingDetails()
    {
        lstvwSiblingDetails.DataSourceID = lstvwSiblingDetailsDSobj.ID;
        lstvwSiblingDetails.DataBind();
        SetVisibilityOfControls();
    }

    /// <summary>
    /// This method is used to fill stdent ListBox based on search creteria.
    /// </summary>
    private void FillStudentListView()
    {
        lstvwStudentList.DataSourceID = lstvwStudentDSobj.ID;
        lstvwStudentList.DataBind();
    }

    /// <summary>
    /// This method is used to set the page number combo.
    /// </summary>
    /// <param name="oListView"></param>
    /// <param name="oPgCntDataPager"></param>
    public static void FillPageNoCombo(ListView oListView, DataPager oPgCntDataPager)
    {
        DataPager oDataPager = oListView.FindControl("DtPgDropDown") as DataPager;
        HtmlTableRow otblDataPager = oListView.FindControl("trDataPager") as HtmlTableRow;
        otblDataPager.Visible = false;
        oPgCntDataPager.Visible = false;
        int iCurrentPage = (oDataPager.StartRowIndex / oDataPager.PageSize) + 1;
        int iTotalPages = oDataPager.TotalRowCount / oDataPager.PageSize;
        if (iTotalPages * oDataPager.PageSize < oDataPager.TotalRowCount)
            iTotalPages += 1;
        if (iTotalPages > 1)
        {
            otblDataPager.Visible = true;
            oPgCntDataPager.Visible = true;
            // Populate the DropDownList if needed
            DropDownList ddlCount = (oDataPager.Controls[0].FindControl("ddlCnt")) as DropDownList;
            ddlCount.Attributes.Add("onchange", "if(!MessageAboutUpload('" + ddlCount.ClientID + "')){return false;}");
            if (ddlCount.Items.Count == 0)
            {
                // Add a list item for each page
                for (int iddlCount = 1; iddlCount <= iTotalPages; iddlCount++)
                    ddlCount.Items.Add(iddlCount.ToString());
                // Set the DDL to the appropriate page value
                ddlCount.Items.FindByValue(iCurrentPage.ToString()).Selected = true;
                Label oLabel = (oDataPager.Controls[0].FindControl("CurrentPageLabel")) as Label;
                oLabel.Font.Bold = true;
                oLabel.Text = Resources.LocalizedResources.PageNo + " " + iCurrentPage + " " + Resources.LocalizedResources.Of + " "+ iTotalPages + " " + Resources.LocalizedResources.OutOflst;
            }
        }
    }

    /// <summary>
    /// This method is used to populate StudentSiblingDetailsBL objects.
    /// </summary>
    /// <param name="iRowId"></param>
    /// <returns></returns>
    private StudentInfo PopulateStudentSiblingDetailsBL()
    {
        StudentInfo oStudentInfo = new StudentInfo
        {
            YearwiseStudentId = Convert.ToInt32(hidYearWiseStudentId.Value),
            AcademicYearId = miAcademicYearId,
            SchoolId = miSchoolId,
            InsertedById = miUserId,
            UpdatedById = miUserId
        };
        return oStudentInfo;
    }

    /// <summary>
    /// This method is used to delete sibling details.
    /// </summary>
    /// <param name="iYearwiseSiblingStudentId"></param>
    /// <param name="aiSchoolId"> </param>
    /// <param name="aiAcademicYearId"> </param>
    /// <param name="iSiblingStudentId"> </param>
    private void DeleteSiblingDetails(int iYearwiseSiblingStudentId, int aiSchoolId, int aiAcademicYearId, int iSiblingStudentId)
    {
        moStudentSiblingDetailsBL.DeleteStudentSiblingDetails(iYearwiseSiblingStudentId, aiSchoolId, aiAcademicYearId, iSiblingStudentId);
    }

    private void SetQueryString()
    {
        StringBuilder sQueryString = new StringBuilder();
        string sNewMode = "N";
        sQueryString.AppendFormat("StandardId={0}", hidStandardId.Value.ToInt());
        sQueryString.AppendFormat("&DivisionId={0}", hidDivisionId.Value.ToInt());
        sQueryString.AppendFormat("&StudentId={0}", hidStudentId.Value.ToInt());
        sQueryString.AppendFormat("&standardName={0}", Convert.ToString(string.Empty));
        sQueryString.AppendFormat("&DivisionName={0}", Convert.ToString(string.Empty));
        sQueryString.AppendFormat("&NewMode={0}", Convert.ToString(sNewMode));
        sQueryString.AppendFormat("&ClassName={0}", Convert.ToString(hidClassName.Value));
        string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString.ToString());
        HidBackUrl.Value = sEncrypt;
    }

    private string GenerateStudentSiblingsXML()
    {
        const string S_ELEMENT = "element";
        string sAttribute;
        XmlDocument oDoc = new XmlDocument();
        XmlElement oElement = oDoc.CreateElement("SiblingDetails");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "SiblingDetails", "");
        int iYearwiseStudentId = Convert.ToInt32(hidYearWiseStudentId.Value);
        for (int iRowCount = 0; iRowCount < lstvwStudentList.Items.Count; iRowCount++)
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)lstvwStudentList.Items[iRowCount];
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
            CheckBox chkSiblingName = oCurrentItem.FindControl("ChkSelect") as CheckBox;

            XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "SiblingDetails", "");
            if (chkSiblingName.Checked)
            {
                sAttribute = "Yearwise_Student_Id";
                XmlAttribute oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = iYearwiseStudentId.ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "YearwiseSiblingStudentId";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = lstvwStudentList.DataKeys[iRowId]["YearwiseStudentId"].ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "Insert_Date";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = System.DateTime.Now.ToString(Constants.S_DATE_FORMAT_MARATHI);
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "Update_Date";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = System.DateTime.Now.ToString(Constants.S_DATE_FORMAT_MARATHI);
                oXmlNode.Attributes.Append(oAttr);

                oXmlRootNode.AppendChild(oXmlNode);
            }
            oElement.AppendChild(oXmlRootNode);
            // return the string generated.
        }
        return oElement.InnerXml;
    }

    private void RefreshValue()
    {
        hidAlertSelectedStudentFromPageGetLost.Value = Resources.LocalizedResources.AlertSelectedStudentFromPageGetLost;
        hidAlertDeleterecord.Value = Resources.LocalizedResources.AlertDeleterecord;
        hidAtLeastOneStudentSelectedForSibling.Value = Resources.LocalizedResources.AtLeastOneStudentSelectedForSibling;
        hidAlertMultipleSiblingSelected.Value = Resources.LocalizedResources.AlertMultipleSiblingSelected;

    }

    #endregion "Private Methods"
}
