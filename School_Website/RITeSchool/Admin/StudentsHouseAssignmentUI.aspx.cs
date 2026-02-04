using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using BusinessLogic.Exceptions;
using BusinessLogic;
using Utility;
using House;
using StudentEntities;
public partial class StudentsHouseAssignmentUI : SchoolBase
{
    #region "Constants"   

    #endregion

    #region "Data Members"

    HouseCofigurationBL moHouseCofigurationBL = null;
    List<HouseConfiguration> mlstHouseConfiguration = null;

    #endregion

    #region "Events"

    /// <summary>
    /// This event is used to default values.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moHouseCofigurationBL = new HouseCofigurationBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                FillStandardCombobox();
                SetJavaScriptAttributres();                
            }           
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to fill division combobox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {   
                FillDivisionCombobox();
                ResetListviews();
                SetButtonState(false);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is sued to reset listview and set button state.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ResetListviews();
            SetButtonState(false);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to bind data to listview columns.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstStudentsHouseInformation_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                DropDownList cmbHouse = oCurrentItem.FindControl("cmbHouse") as DropDownList;
                //DropDownList cmbHouseColor = oCurrentItem.FindControl("cmbHouseColors") as DropDownList;
                StudentInfo oStudentInfo = oCurrentItem.DataItem as StudentInfo;
              //  ListSource.FillDropDownList(mlstHouseConfiguration, cmbHouse, "Name", "Id", Constants.S_SELECT);
                cmbHouse.Items.Clear();
                
                ListItem oListItemSelect = new ListItem
                {
                    Text = Constants.S_SELECT,
                    Value = Constants.S_ZERO
                };
                oListItemSelect.Attributes.Add("style", "background-color:white;");

                cmbHouse.Items.Add(oListItemSelect);
                mlstHouseConfiguration.ForEach(
                    house=>
                {
                    ListItem oListItem = new ListItem
                    {
                        Text = house.Name,
                        Value = house.Id.ToString()
                    };
                    oListItem.Attributes.Add("style", "background-color:" + house.Color + ";");
                    if (house.Id == oStudentInfo.HouseId)
                    {
                        oListItem.Selected = true;
                        cmbHouse.BackColor = Color.FromName(house.Color);
                    }
                    cmbHouse.Items.Add(oListItem);   
             
                }
                    );
               // cmbHouse.SelectedValue = oStudentInfo.HouseId.ToString();
               // cmbHouse.BackColor = Color.FromName(oStudentInfo.HouseColor.ToString());
               cmbHouse.Attributes.Add("onclick", "ChangeColor(this)");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

  
    /// <summary>
    /// This event is used to save student details. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Update();
            DisplayMessage(Constants.ItemState.updated, false);
            if (QueryString["Is_Configured"] != Constants.S_YES)
                SaveConfigDetails(Constants.SchoolConfigurations.HouseInformation.ToInt());

            btnShow_Click(btnShow, null);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill Student information.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            FillStudentListview();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstStudentsHouseInformation_DataBound(object sender, EventArgs e)
    {
        //DropDownList cmb = (DropDownList)oCurrentItem.FindControl("cmbHouse");
        //cmb.Attributes.Add("onchange", "ChangeColor(this)");
    }
    /// <summary>
    /// This is event is used to go back to dashboard screen.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBackUp_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
            if (hidHouseConId.Value == Constants.S_ONE)
                oMasterPage.RedirectToNextPage("~/RITeSchool/Common/ControlPanel.aspx");
            if (hidHouseConId.Value == Constants.S_TWO)
                oMasterPage.RedirectToNextPage("~/RITeSchool/Admin/AllStudentsUI.aspx");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is sued to set color.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwHouseCount_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                HouseSummary oHouseSummary = e.Item.DataItem as HouseSummary;
                HtmlTableCell td = e.Item.FindControl("tdColor") as HtmlTableCell;
                if (td != null)
                    td.BgColor = oHouseSummary.HouseColor;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region "Private methods"

    /// <summary>
    ///This methos is used to fill standard combobox.
    /// </summary>
    private void FillStandardCombobox()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtStandardCollection = oStandardCollectionBL.GetAssociatedStandardsForHouse();
        ControlUtility.FillDropDownList(oDtStandardCollection, ref cmbStandard,
                                       Constants.S_STANDARD_ID_FIELD,
                                       Constants.S_STANDARD_NAME_FIELD,
                                       Constants.S_SELECT);
    }
    /// <summary>
    /// This method is used to fill division combobox.
    /// </summary>
    private void FillDivisionCombobox()
    {
        int iStandardId = Convert.ToInt32(cmbStandard.SelectedValue);
        DivisionCollectionBL oDivisionCollectionBL = new DivisionCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDSStandardCollection = oDivisionCollectionBL.GetAllDivisionsForStandard(iStandardId);

        ControlUtility.FillDropDownList(oDSStandardCollection, ref cmbDivision,
                                       Constants.S_DIVISION_ID_FIELD,
                                       Constants.S_DIVISION_NAME_FIELD,
                                        Constants.S_SELECT);
    }
    /// <summary>
    /// This method is used to fill student listview.
    /// </summary>
    private void FillStudentListview()
    {
        lstStudentsHouseInformation.Visible = true;
        int iStandardId = Convert.ToInt32(cmbStandard.SelectedValue);
        int iDivisionId = Convert.ToInt32(cmbDivision.SelectedValue);
        int iConfigured = 0;      
                  
         if (chkCofiguredStudents.Checked == true)
             iConfigured = 1;

        StudentBL oStudentBL = new StudentBL();      
        mlstHouseConfiguration = moHouseCofigurationBL.GetAll(miSchoolId, miAcademicYearId, string.Empty, 0, 0);
        List<StudentInfo> lstStudentDetails = oStudentBL.GetAllStudentForHouseAssignment(miSchoolId, miAcademicYearId, iStandardId, iDivisionId, iConfigured);
        lstStudentsHouseInformation.DataSource = lstStudentDetails;
        lstStudentsHouseInformation.DataBind();
        if (lstStudentDetails.Count > 0)
        {            
            SetHeaderHouseComboBox();       
            lstStudentsHouseInformation.Visible = true;
            SetButtonState(true);

            if (chkCofiguredStudents.Checked)
            {
                var oHouseGroups = lstStudentDetails.GroupBy(sd => sd.HouseId).Select(sd => new { HouseId = sd.Key, RecordCount = sd.Count() }).ToList();
                if (oHouseGroups != null && oHouseGroups.Count > 0)
                {
                    List<HouseSummary> oHouses = oHouseGroups.Join(mlstHouseConfiguration, hg => hg.HouseId, hc => hc.Id, (hg, hc) => new HouseSummary { HouseName = hc.Name, StudentCount = hg.RecordCount, HouseColor = hc.Color }).OrderBy(hg => hg.HouseName).ToList();
                    if (oHouses != null && oHouses.Count > 0)
                    {
                        lstvwHouseCount.DataSource = oHouses;
                        lstvwHouseCount.DataBind();
                    }
                    else
                    {
                        lstvwHouseCount.DataSource = null;
                        lstvwHouseCount.DataBind();
                    }
                }
                else
                {
                    lstvwHouseCount.DataSource = null;
                    lstvwHouseCount.DataBind();
                }
            }
            else
            {
                if (lstStudentDetails.Count > 0)
                {
                    List<HouseSummary> oHouses = new List<HouseSummary>();
                    oHouses.Add(new HouseSummary { HouseName = "Non-Assigned Student Count", StudentCount = lstStudentDetails.Count });
                    lstvwHouseCount.DataSource = oHouses;
                    lstvwHouseCount.DataBind();
                }
                else
                {
                    lstvwHouseCount.DataSource = null;
                    lstvwHouseCount.DataBind();
                }
            }
        }
        else
        {
            //lstStudentsHouseInformation.Visible = false;
            SetButtonState(false);
            lstvwHouseCount.DataSource = null;
            lstvwHouseCount.DataBind();
        }
       
    }

    /// <summary>
    /// This method is used to fill Header House combo box in listview.
    /// </summary>
    private void SetHeaderHouseComboBox()
    {
        HtmlTableRow oHtmlTableRow = (HtmlTableRow)lstStudentsHouseInformation.FindControl("trHeader");
        DropDownList oDropDownList = (DropDownList)oHtmlTableRow.FindControl("cmbAllHouse");        
        ListSource.FillDropDownList(mlstHouseConfiguration, oDropDownList, "Name", "Id", Constants.S_SELECT);

        oDropDownList.Items.Clear();

        ListItem oListItemSelect = new ListItem
        {
            Text = Constants.S_SELECT,
            Value = Constants.S_ZERO
        };
        oListItemSelect.Attributes.Add("style", "background-color:white;");

        oDropDownList.Items.Add(oListItemSelect);

        mlstHouseConfiguration.ForEach(
            house =>
            {
                ListItem oListItem = new ListItem
                {
                    Text = house.Name,
                    Value = house.Id.ToString()
                };
                oListItem.Attributes.Add("style", "background-color:" + house.Color + ";");
                oDropDownList.Items.Add(oListItem);
                oDropDownList.Attributes.Add("onclick", "ChangeColor(this)");
            }
            );
        oDropDownList.Attributes.Add("onclick", "ChangeColor(this)");
    }

    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavaScriptAttributres()
    {
        ApplyMouseHoverEffect(new List<Button> { btnSave,btnShow, btnBack, btnSaveUp, btnBackUp });
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;

        if (QueryString["From"] == null || QueryString["From"].ToString() == "0" || QueryString["From"].ToString() == "")
        {
            btnBackUp.Visible = false;
            btnBack.Visible = false;
        }
        else
            hidHouseConId.Value = QueryString["From"].ToString();

        cmbStandard.Focus();
    }

    /// <summary>
    /// This method is used to populate Student Details with House.
    /// </summary>
    /// <returns></returns>
    private List<StudentInfo> PopulateStudentDetails()
    {
        List<StudentInfo> lstStudentDetail = new List<StudentInfo>();
        StudentInfo oStudentInfo = null;
        foreach (ListViewDataItem oCurrentItem in lstStudentsHouseInformation.Items)
        {
            DropDownList cmbHouse = oCurrentItem.FindControl("cmbHouse") as DropDownList;          
            {
                oStudentInfo = new StudentInfo()
                {
                    HouseId = Convert.ToInt32(cmbHouse.SelectedValue),
                    SchoolwiseStudentId = Convert.ToInt32(lstStudentsHouseInformation.DataKeys[oCurrentItem.DisplayIndex]["SchoolwiseStudentId"])
                };
                lstStudentDetail.Add(oStudentInfo);
            }
        }
        return lstStudentDetail;
    }


    /// <summary>
    /// This method is used to update student details.
    /// </summary>
    private void Update()
    {
       string sXml = base.GenerateXml(PopulateStudentDetails());
        moHouseCofigurationBL.UpdateStudentHouseInformation(sXml);
    }

    /// <summary>
    /// This method is used to set button status.
    /// </summary>
    /// <param name="aFlag"></param>
    private void SetButtonState(bool abFlag)
    {
        btnSave.Visible = abFlag;
        btnSaveUp.Visible = abFlag;
        btnBackUp.Visible = abFlag;
    }
    

    /// <summary>
    /// This method is used to display message.
    /// </summary>
    /// <param name="aoItemState"></param>
    /// <param name="abIsErrorMessage"></param>
    private void DisplayMessage(Constants.ItemState aoItemState, bool abIsErrorMessage)
    {
        string sMessage = "Student details " + aoItemState.ToString() + " successfully!!!";
        DisplayMessage(sMessage, abIsErrorMessage, tdMessage);
    }

    /// <summary>
    /// This method isused to reset listviews.
    /// </summary>
    private void ResetListviews()
    {
        lstStudentsHouseInformation.DataSource = null;
        lstStudentsHouseInformation.DataBind();

        lstvwHouseCount.DataSource = null;
        lstvwHouseCount.DataBind();
    }   

    #endregion

    private class HouseSummary
    {
        public string HouseName { get; set; }
        public int StudentCount { get; set; }
        public string HouseColor { get; set; }
    }    
}