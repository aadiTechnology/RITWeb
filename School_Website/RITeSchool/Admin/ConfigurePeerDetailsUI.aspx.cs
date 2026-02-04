/* File Name -    ConfigurePeerDetailsUI.aspx.cs
 * Created Date - 15 Oct 2024
 * Created By -   Rutuja
 * Description -  This class is used to save peer student details.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;

public partial class ConfigurePeerDetailsUI : SchoolBase
{
    #region Data Member(s)

    private List<ConfigurePeerDetails> mlstpeer;
    private ConfigurePeerBL moConfigurePeerBL;

    #endregion

    #region Constant(s)

    const string S_TEACHER_DATA = "TEACHER_DATA";

    #endregion

    #region Events

    /// <summary>
    /// This event is used to fill listview, standard, division and peer dropdown.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moConfigurePeerBL = new ConfigurePeerBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                SetDefaultValues();
                CheckUserAccess();
                SetClassTeacherDetails();
                FillStandardCombo();                
                FillPeerListView();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
   
    /// <summary>
    ///  This event is used to fill division dropdown.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int iStandardId = Convert.ToInt32(ddlStandard.SelectedValue);
            if (iStandardId != 0)
                FillDivisions();
            else
            {
                ClearDivisions();
                lstvwConfigurePeerDetails.DataSource = null;
                lstvwConfigurePeerDetails.DataBind();
                btnSave.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill peer details listview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillPeerListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set value for listview dropdown.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwConfigurePeerDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ConfigurePeerDetails oConfigurePeerDetails = e.Item.DataItem as ConfigurePeerDetails;
                int iYearwiseStudentId = lstvwConfigurePeerDetails.DataKeys[e.Item.DisplayIndex]["YearwiseStudentId"].ToInt();

                DropDownList ddlPeer = e.Item.FindControl("ddlPeer") as DropDownList;
                List<ConfigurePeerDetails> lstPeerStudent = mlstpeer.Where(ps => ps.YearwiseStudentId != iYearwiseStudentId).ToList();
                ListSource.FillDropDownList(lstPeerStudent, ddlPeer, "PeerName", "YearwiseStudentId", Constants.S_SELECT);
                ddlPeer.SelectedValue = oConfigurePeerDetails.PeerYrStudentId.ToString();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save peer student details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            SaveConfigureDetails();
            lblUpdateMessage.Text = "Peer Details saved successfully !!!";
            FillPeerListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    #endregion

    #region Methods

    /// <summary>
    /// This method is used to fill peer student listview.
    /// </summary>
    private void FillPeerListView()
    {
        int iStandardId = Convert.ToInt32(ddlStandard.SelectedValue);
        int iDivisionId = Convert.ToInt32(ddlDivision.SelectedValue);        
        mlstpeer = moConfigurePeerBL.GetAll(ddlStandard.SelectedValue.ToInt(), ddlDivision.SelectedValue.ToInt());
        lstvwConfigurePeerDetails.DataSource = mlstpeer;
        lstvwConfigurePeerDetails.DataBind();

        btnSave.Enabled = mlstpeer.Count > 0;
    }

    /// <summary>
    /// This method is used to save details.
    /// </summary>
    private void SaveConfigureDetails()
    {
        List<ConfigurePeerDetails> lstConfigurePeer = new List<ConfigurePeerDetails>();
        lstConfigurePeer = Populate();
        string sXml = base.GenerateXml(lstConfigurePeer);
        moConfigurePeerBL.Save(sXml);
    }

    /// <summary>
    /// This method is used to populate document details.
    /// </summary>
    /// <param name="iId"></param>
    /// <returns></returns>
    private List<ConfigurePeerDetails> Populate()
    {
        List<ConfigurePeerDetails> lstConfigurePeer = new List<ConfigurePeerDetails>();
        {
            foreach (ListViewDataItem item in lstvwConfigurePeerDetails.Items)
            {
                DropDownList ddlPeer = item.FindControl("ddlPeer") as DropDownList;
                int iStudentId = lstvwConfigurePeerDetails.DataKeys[item.DisplayIndex]["YearwiseStudentId"].ToInt();
                int aiId = lstvwConfigurePeerDetails.DataKeys[item.DisplayIndex]["Id"].ToInt();

                if (ddlPeer.SelectedValue.ToString() != Constants.S_ZERO || aiId != 0)
                {
                    ConfigurePeerDetails oConfigurePeerDetails = new ConfigurePeerDetails
                    {
                        Id = aiId,
                        PeerYrStudentId = ddlPeer.SelectedValue.ToInt(),
                        YearwiseStudentId = iStudentId.ToInt()
                    };
                    lstConfigurePeer.Add(oConfigurePeerDetails);
                }
            }
        }
        return lstConfigurePeer;
    }


    /// <summary>
    /// This method is used to fill standard's combo.
    /// </summary>
    private void FillStandardCombo()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtStandardCollection = oStandardCollectionBL.GetAssociatedStandards();

        DataTable oDTStandard = oDtStandardCollection.Clone();
        if (moUserRole == Constants.UserRoles.Admin || hidHasEditAccess.Value == Constants.S_YES)
        {
            oDTStandard = oDtStandardCollection;
        }
        else if (moUserRole == Constants.UserRoles.Teacher)
        {
            DataTable oDT = ViewState[S_TEACHER_DATA] as DataTable;
            List<int> lstStdIds = oDT.AsEnumerable().Select(std => std.Field<int>("Standard_Id")).ToList().Distinct().ToList();
            var oData = (from std in oDtStandardCollection.AsEnumerable()
                         join sid in lstStdIds
                         on std.Field<int>("Standard_Id") equals sid
                         select std);

            if (oData != null && oData.Count() > 0)
                oDTStandard = oData.CopyToDataTable();
        }

        ControlUtility.FillDropDownList(oDTStandard, ref ddlStandard, Constants.S_STANDARD_ID_FIELD, Constants.S_STANDARD_NAME_FIELD, "-- Select --");

        if (oDTStandard.Rows.Count == 1)
        {
            ddlStandard.SelectedIndex = 1;
            ddlStandard_SelectedIndexChanged(ddlStandard, new EventArgs());
        }
        else
        {
            ClearDivisions();
        }
    }

    private void ClearDivisions()
    {
        ddlDivision.Items.Clear();
        ListItem olstDivision = new ListItem();
        olstDivision.Value = Constants.S_ZERO;
        olstDivision.Text = Constants.S_SELECT;
        ddlDivision.Items.Add(olstDivision);
    }

    /// <summary>
    /// This method is used to fill division's combo.    
    /// </summary>
    private void FillDivisions()
    {
        int iStandardId = Convert.ToInt32(ddlStandard.SelectedValue);
        DivisionCollectionBL oDivisionCollectionBL = new DivisionCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDSStandardCollection = oDivisionCollectionBL.GetAllDivisionsForStandard(iStandardId);

        DataTable oDTDivisions = oDSStandardCollection.Clone();
        if (moUserRole == Constants.UserRoles.Admin || hidHasEditAccess.Value == Constants.S_YES)
        {
            oDTDivisions = oDSStandardCollection;
        }
        else if (moUserRole == Constants.UserRoles.Teacher)
        {
            DataTable oDT = ViewState[S_TEACHER_DATA] as DataTable;
            List<int> lstDivIds = oDT.AsEnumerable().Where(std => std.Field<int>("Standard_Id") == iStandardId).Select(std => std.Field<int>("Division_Id")).ToList().Distinct().ToList();
            var oData = (from div in oDSStandardCollection.AsEnumerable()
                         join did in lstDivIds
                        on div.Field<int>("Division_Id") equals did
                         select div);

            if (oData != null && oData.Count() > 0)
                oDTDivisions = oData.CopyToDataTable();
        }

        ControlUtility.FillDropDownList(oDTDivisions, ref ddlDivision, Constants.S_DIVISION_ID_FIELD, Constants.S_DIVISION_NAME_FIELD, Constants.S_SELECT);

        if (oDTDivisions.Rows.Count == 1)
        {
            ddlDivision.SelectedIndex = 1;
            ddlDivision_SelectedIndexChanged(ddlDivision, null);
        }
        else
        {
            lstvwConfigurePeerDetails.DataSource = null;
            lstvwConfigurePeerDetails.DataBind();
            btnSave.Enabled = false;
        }
    }

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
    }

    private void CheckUserAccess()
    {
        hidHasEditAccess.Value = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.ConfigurePeerDetails).ToString();
    }

    private void SetClassTeacherDetails()
    {
        if (moUserRole == Constants.UserRoles.Teacher)
        {
            MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
            DataTable oDataTable = oMasterDataCollectionBL.GetClassTeachers(miSchoolId, miAcademicYearId);

            if (moUserRole == Constants.UserRoles.Admin || hidHasEditAccess.Value == Constants.S_YES)
                ViewState[S_TEACHER_DATA] = oDataTable;
            else
            {
                DataRow[] oDataRow = oDataTable.Select("Teacher_Id=" + Convert.ToString(Session[Constants.S_SESSION_TEACHER_ID]));
                ViewState[S_TEACHER_DATA] = oDataRow.CopyToDataTable();
            }
        }
    }

    #endregion
}