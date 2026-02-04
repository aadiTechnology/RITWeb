//// File Name  : PANAttachmentUI.aspx.cs
//// Created By : Yogesh
//// Date       : 02/09/2014
//// Description :This class is used to attach PAN copy. 
////   

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.Services;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Kendo.DynamicLinq;
using SchoolEntities.Admin;
using Utility;

public partial class PANAttachmentUI : SchoolBase
{
    #region Constant(s)

    private const string S_UPLOAD_FILE_PATH_FOR_PAN = "..//DOWNLOADS//PAN Attachment//";
    private const string S_UPLOAD_FILE_PATH_FOR_AADHAR = "..//DOWNLOADS//Aadhar Cards//";

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to fill up user role and category combo box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
                this.SetJavascriptAttributes();
                this.FillUserRoleCombo(true);
                FillClasses();
                this.chkshowalldetails.Checked = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill up user role role combo box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            this.FillUserRoleCombo((cmbCategory.SelectedValue == Constants.S_ONE));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Public Method(s)

    /// <summary>
    /// This method is used to get all pan attachment details.
    /// </summary>
    /// <param name="take"></param>
    /// <param name="skip"></param>
    /// <param name="sort"></param>
    /// <param name="aiUserRoleId"></param>
    /// <param name="aiSchoolId"></param>
    /// <param name="aiAcademicYearId"></param>
    /// <param name="asNameFilter"></param>
    /// <param name="abShowAllDetails"></param>
    /// <returns></returns>
    [WebMethod]
    public static DataSourceResult GetAllPanAttachmentDetails(int take, int skip, IEnumerable<Sort> sort, int aiUserRoleId, int aiSchoolId, int aiAcademicYearId, string asNameFilter, int abShowAllDetails, int aiCategoryId, int aiStdDivId, bool asIncludeLeftStudents)
    {
        int iStartINdex = skip + 1;
        int iEndIndex = iStartINdex + take;
        string sSortDirection = "ASC";
        string sSortExpression = "Name";

        if (sort != null && sort.Count() > 0)
        {
            sSortDirection = sort.FirstOrDefault().Dir;
            sSortExpression = sort.FirstOrDefault().Field;
        }

        List<PANAttachmentDetails> lstPANAttachmentDetails = PanAttachmentBL.GetAllPanAttachmentDetails(aiUserRoleId, aiSchoolId, aiAcademicYearId, asNameFilter, Convert.ToInt32(abShowAllDetails), sSortDirection, iStartINdex, iEndIndex, aiCategoryId, aiStdDivId, sSortExpression, asIncludeLeftStudents);
        lstPANAttachmentDetails.Where(pan => pan.PanAttachment.Trim() != string.Empty).ToList()
        .ForEach(att =>
        {
            att.PanAttachment = (aiCategoryId == 1 ? S_UPLOAD_FILE_PATH_FOR_PAN : S_UPLOAD_FILE_PATH_FOR_AADHAR) + att.PanAttachment;
        });

        int iRecordCount = PanAttachmentBL.GetCountAllPanAttachmentDetails(aiUserRoleId, aiSchoolId, aiAcademicYearId, asNameFilter, Convert.ToInt32(abShowAllDetails), aiCategoryId, aiStdDivId, asIncludeLeftStudents);

        var result = new DataSourceResult()
        {
            Data = lstPANAttachmentDetails,
            Total = iRecordCount
        };
        return result;
    }

    /// <summary>
    /// This method is used to return query string.
    /// </summary>
    /// <param name="aiQuestionId"></param>
    /// <param name="aiQuestionDetailsId"></param>
    /// <param name="aiIsReply"></param>
    /// <returns></returns>
    [WebMethod]
    public static string GetQueryString(int aiUserId, int aiCategoryId)
    {
        return CommonUtility.EncryptQuerystring("UserId=" + aiUserId + "&DocumentId=0&IsSubmitted=0&DocumentTypeId=" + (aiCategoryId == 1 ? Constants.DocumentTypes.PAN.ToInt() : Constants.DocumentTypes.AadharCard.ToInt()));
    }

    #endregion

    #region Private Method(s)

    /// <summary>
    /// This method is used to fill class combo box.
    /// </summary>
    private void FillClasses()
    {
        StandardDivisionCollectionBL obj = new StandardDivisionCollectionBL(miSchoolId, miAcademicYearId);
        DataTable dt = obj.GetAssociatedStandardsDivisions();
        DataView dv = dt.DefaultView;
        dv.Sort = "Original_Standard_Id, Original_Division_Id";
        dt = dv.ToTable();
        ControlUtility.FillDropDownList(dt, ref cmbClass, "Schoolwise_Standard_Division_Id", "StandardDivision", Constants.S_SELECT_ALL);
    }

    /// <summary>
    /// This method is used to fill user role combo.
    /// </summary>
    private void FillUserRoleCombo(bool abIsPanCard)
    {
        // Fill the user role's combobox with all the user roles available in the system.
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        DataTable oDSDesignatiuon = oMasterDataCollectionBL.GetAllUserRoles();
        if (abIsPanCard)
            ControlUtility.FillDropDownList(oDSDesignatiuon.Select("User_Role_Id <> " + Convert.ToInt32(Constants.UserRoles.Student) + " AND User_Role_Id <> " + Convert.ToInt32(Constants.UserRoles.Parent) + " AND User_Role_Id <> " + Convert.ToInt32(Constants.UserRoles.TransportStaff) + " AND User_Role_Id <> " + Convert.ToInt32(Constants.UserRoles.ExAdmin)), ref cmbUserRole, Constants.S_USER_ROLE_ID_FIELD, Constants.S_USER_ROLE_NAME_FIELD, Constants.S_SELECT);
        else
            ControlUtility.FillDropDownList(oDSDesignatiuon.Select("User_Role_Id <> " + Convert.ToInt32(Constants.UserRoles.Parent) + " AND User_Role_Id <> " + Convert.ToInt32(Constants.UserRoles.TransportStaff) + " AND User_Role_Id <> " + Convert.ToInt32(Constants.UserRoles.ExAdmin)), ref cmbUserRole, Constants.S_USER_ROLE_ID_FIELD, Constants.S_USER_ROLE_NAME_FIELD, Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to set JavaScript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnSearch });
    }

    #endregion
}
