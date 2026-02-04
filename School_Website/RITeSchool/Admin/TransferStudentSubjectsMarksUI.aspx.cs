/* File Name - TransferStudentSubjectsMarksUI.aspx.cs
 * Created Date - 6-Feb-2012
 * Created by - Vipul
 * Class Description - This class is used transfering student marks fom one optional subject to another.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using StudentMarksEntities;
using Utility;

public partial class TransferStudentSubjectsMarksUI :SchoolBase
{
    #region "Constants"

    private const string S_MSG_SUCCESSFULL = "Subject(s) transferred successfully!!!";
    private const string S_CLASS_TEACHER_TEST_MARK_URL = "~/Teacher/ClassTeacherTestMarksUI.aspx";
    private const string S_OPTIONAL_SUBJECTS = "OptionalSubjects";
    private const string S_SUBJECT_GROUP = "S.G.";

    #endregion "Constants"

    #region "Data Members"

    private int miTeacherId;
    private List<OptionalSubject> mlstOptionalSubjects;

    #endregion "Data Members"

    #region "Events"

    /// <summary>
    /// Overidded method for page initialization.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnInit(EventArgs e)
    {
        try
        {
            //calling base class method
            InitializeMemberVariables();
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to intialize page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (ViewState[S_OPTIONAL_SUBJECTS] != null)
                mlstOptionalSubjects = (List<OptionalSubject>)ViewState[S_OPTIONAL_SUBJECTS];
            if (!IsPostBack)
            {
                if (CheckPreCondition())
                {
                    FillClassCombo();
                    Initialize();
                    SetJavaScriptAttributes();
                }
            }
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to transfer students marks.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnTransfer_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrorMsg.Text = string.Empty;
            lblUpdateSucess.Text = string.Empty;
            if (ValidateOptionalSubjectTreeView())
            {
                string sStudentSubjectMarksXml = GetTransferSubjectMarksXml();
                if (!string.IsNullOrEmpty(sStudentSubjectMarksXml))
                {
                    StudentSubjectMarksBL oStudentSubjectMarksBL = new StudentSubjectMarksBL(miSchoolId, miAcademicYearId);
                    DataTable oDT = oStudentSubjectMarksBL.Transfer(sStudentSubjectMarksXml, miUserId);

                    if (oDT.Rows.Count > 0 && oDT.Rows[0][0] != DBNull.Value)
                    {
                        lblErrorMsg.Text = oDT.Rows[0][0].ToString();
                    }
                    else
                    {
                        lstvwStudentMarks.DataSourceID = ObjDSStudentMarksTransfer.ID;
                        lstvwStudentMarks.DataBind();
                        lblUpdateSucess.Text = S_MSG_SUCCESSFULL;
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "ClearTreeview", "ClearTreeview()", true);
                    }
                }
            }
        }        
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to go back to the previous url.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)Master;
            oMasterPage.RedirectToNextPage(S_CLASS_TEACHER_TEST_MARK_URL);
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to view pagewise students.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrorMsg.Text = string.Empty;
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwStudentMarks);
            DataPager oDataPager = lstvwStudentMarks.FindControl("DtPgDropDown") as DataPager;
            DropDownList oDdlCnt = oDataPager.Controls[0].FindControl("ddlCnt") as DropDownList;
            hidPageNo.Value = (oDdlCnt.SelectedIndex + 1).ToString();
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill list view footer and set confirm messge.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStudentMarks_DataBound(object sender, EventArgs e)
    {
        try
        {            
            if (lstvwStudentMarks.Items.Count > Constants.I_ZERO)
            {
                if (mlstOptionalSubjects.Count > Constants.I_ONE || hidStandardDivisionId.Value == Constants.S_ZERO)
                {
                    SetControlState(true);
                    lblUpdateSucess.Text = string.Empty;
                    ControlUtility.FillListViewPagerFooter(lstvwStudentMarks, DtPgCount);

                    SetConfirmationMessage();
                    if (lstvwStudentMarks.Items.Count == DtPgCount.TotalRowCount)
                        DtPgCount.Visible = false;
                }
                else
                {
                    DtPgCount.Visible = false;
                    cmbTeachers.Attributes.Remove("onchange");
                }
            }
            else
            {
                SetControlState(false);
                DtPgCount.Visible = false;
                cmbTeachers.Attributes.Remove("onchange");
            }
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to search studnts as per given filters.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (cmbTeachers.SelectedValue != Constants.S_ZERO)
                FillStudentList();
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill students as per selected class.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbTeachers_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbTeachers.SelectedValue != Constants.S_ZERO)
            {
                DtPgCount.SetPageProperties(Constants.I_ZERO, Constants.I_GRID_PAGE_COUNT, false);
                hidTeacherId.Value = cmbTeachers.SelectedValue;
                FillOptionalSubjectTreeView();
                FillStudentList();
            }
            else
            {
                lstvwStudentMarks.DataSource = null;
                lstvwStudentMarks.DataBind();
                trPagerStudentMarksTransfer.Visible = false;
                trStudentInfo.Visible = false;
                btnTransfer.Enabled = false;
                tblNotes.Visible = false;
                btnSearch.Enabled = false;
            }
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }
    #endregion "Events"

    #region "Private Methods"

    /// <summary>
    /// This method is used to validate tree veiw.
    /// </summary>
    /// <returns></returns>
    private bool ValidateOptionalSubjectTreeView()
    {
        foreach (TreeNode oNode in trvwOptionalSubject.Nodes)
        {
            if (oNode.ChildNodes.Count > 0)
                ValidateOptionalSubjectGroup(oNode, true);
        }

        if (!lblErrorMsg.Text.IsNullOrEmpty())
            lblErrorMsg.Text += "</ul>";

        return lblErrorMsg.Text.IsNullOrEmpty();
    }

    /// <summary>
    /// This is used to validate optional subject groups (chlid nodes).
    /// </summary>
    /// <param name="aoNode"></param>
    /// <param name="abFireValidation"></param>
    /// <returns></returns>
    private int ValidateOptionalSubjectGroup(TreeNode aoNode, bool abFireValidation)
    {
        int iSubjectCount = 0;
        var lstChildSubbjects = (from oOptionalSubject in mlstOptionalSubjects
                   where oOptionalSubject.ParentOptionalSubjectId == aoNode.Value.ToInt()
                   group oOptionalSubject by new { oOptionalSubject.ParentOptionalSubjectId, oOptionalSubject.OptionalSubjectName, oOptionalSubject.NoOfSubjects }
                       into grp
                       select new
                       {
                           grp.Key.ParentOptionalSubjectId,
                           grp.Key.OptionalSubjectName,
                           grp.Key.NoOfSubjects
                       }).ToList();

        lstChildSubbjects.ForEach(
                    oSubject =>
                    {
                        foreach (TreeNode oChildNode in aoNode.ChildNodes)
                        {
                            if (oChildNode.ChildNodes.Count > 0 && !oChildNode.Value.Contains(S_SUBJECT_GROUP))
                                iSubjectCount += ValidateOptionalSubjectGroup(oChildNode, false);
                            else
                            {
                                if (oChildNode.Checked)
                                    iSubjectCount++;
                            }
                        }

                        if (abFireValidation)
                        {
                            if (iSubjectCount > oSubject.NoOfSubjects)
                            {
                                if (lblErrorMsg.Text.IsNullOrEmpty())
                                    lblErrorMsg.Text = Constants.S_VALIDATION_SUMMARY_HEADER + "<br /><ul>";
                                lblErrorMsg.Text += "<li>At most " + oSubject.NoOfSubjects + " subject(s) can be selected for optional subject " + oSubject.OptionalSubjectName + ".</li>";
                            }
                            else if (iSubjectCount < oSubject.NoOfSubjects)
                            {
                                if (lblErrorMsg.Text.IsNullOrEmpty())
                                    lblErrorMsg.Text = Constants.S_VALIDATION_SUMMARY_HEADER + "<br /><ul>";
                                lblErrorMsg.Text += "<li>At least " + oSubject.NoOfSubjects + " subject(s) should be selected for optional subject " + oSubject.OptionalSubjectName + ".</li>";
                            }
                        }
                    }

                );

        return iSubjectCount;
    }

    /// <summary>
    /// This method is used to get list of subjects which are selected.
    /// </summary>
    /// <param name="aoSubject"></param>
    /// <param name="aoNode"></param>
    /// <returns></returns>
    private List<int> GetSelectedSubjectsList(List<int> aoSubject, TreeNode aoNode)
    {
        foreach (TreeNode oNode in aoNode.ChildNodes)
        {
            if (oNode.ChildNodes.Count > 0)
                aoSubject = GetSelectedSubjectsList(aoSubject, oNode);
            else if (oNode.Checked || (aoNode.Value.Contains(S_SUBJECT_GROUP) && aoNode.Checked))
                aoSubject.Add(oNode.Value.ToInt());
        }

        return aoSubject;
    }

    /// <summary>
    /// This is methods is used to fill optional subject tree view.
    /// </summary>
    private void FillOptionalSubjectTreeView()
    {
        ClasswiseOptionalSubjectBL oClasswiseOptionalSubjectBL = new ClasswiseOptionalSubjectBL(miSchoolId, miAcademicYearId, cmbTeachers.SelectedValue.ToInt());
        mlstOptionalSubjects = oClasswiseOptionalSubjectBL.GetForClass();
        ViewState[S_OPTIONAL_SUBJECTS] = mlstOptionalSubjects;

        trvwOptionalSubject.Nodes.Clear();

        (from oOptionalSubject in mlstOptionalSubjects
         group oOptionalSubject by new { oOptionalSubject.ParentOptionalSubjectId, oOptionalSubject.OptionalSubjectName, oOptionalSubject.NoOfSubjects }
             into grp
             select new
             {
                 grp.Key.ParentOptionalSubjectId,
                 grp.Key.OptionalSubjectName,
                 grp.Key.NoOfSubjects
             }).ToList()
        .ForEach(
            oOptionalSubject =>
            {
                if (mlstOptionalSubjects.Where(oSubject => oSubject.ChildOptionalSubjectId == oOptionalSubject.ParentOptionalSubjectId).ToList().Count == 0)
                {
                    string sOptionalSubjectGroupName = oOptionalSubject.OptionalSubjectName + " (Select any " + oOptionalSubject.NoOfSubjects + ")";
                    TreeNode otrndOptionalSubjectGroup = new TreeNode(sOptionalSubjectGroupName, oOptionalSubject.ParentOptionalSubjectId.ToString())
                                                             {SelectAction = TreeNodeSelectAction.None};
                    otrndOptionalSubjectGroup = AddSubjectToOptionalSubjectGroup(oOptionalSubject.ParentOptionalSubjectId, otrndOptionalSubjectGroup, mlstOptionalSubjects);
                    trvwOptionalSubject.Nodes.Add(otrndOptionalSubjectGroup);
                }
            }
        );

        trvwOptionalSubject.ExpandAll();
    }

    /// <summary>
    /// This method is used to add optional subject group to tree view.
    /// </summary>
    /// <param name="aiOptionalSubjectGroup"></param>
    /// <param name="aotrndOptionalSubjectGroup"></param>
    /// <param name="aoOptionalSubjectDetails"></param>
    /// <returns></returns>
    private TreeNode AddSubjectToOptionalSubjectGroup(int aiOptionalSubjectGroup, TreeNode aotrndOptionalSubjectGroup, List<OptionalSubject> aoOptionalSubjectDetails)
    {
        aoOptionalSubjectDetails.Where(oOptionalSubject => oOptionalSubject.ParentOptionalSubjectId == aiOptionalSubjectGroup && oOptionalSubject.OptionalSubjectsId != 0).ToList().ForEach(
            oSubject =>
            {
                string sSubjectId = oSubject.ChildOptionalSubjectId != 0 ? oSubject.ChildOptionalSubjectId.ToString() : oSubject.SubjectGroupId != 0 ? S_SUBJECT_GROUP + oSubject.SubjectGroupId.ToString() : oSubject.SubjectId.ToString();
                string sSubjectName = string.Empty;

                if (oSubject.ChildOptionalSubjectId != 0)
                {
                    // If optional subject group has child optional subject group adding its name to tree view.
                    var lstSubjects = (from oSubjects in aoOptionalSubjectDetails.Where(oOptionalSubject => oOptionalSubject.ParentOptionalSubjectId == oSubject.ChildOptionalSubjectId).ToList()
                                       group oSubjects by new { oSubjects.OptionalSubjectName, oSubjects.NoOfSubjects }
                                           into grp
                                           select new
                                           {
                                               grp.Key.OptionalSubjectName,
                                               grp.Key.NoOfSubjects,
                                           }).ToList();
                    sSubjectName = lstSubjects[0].OptionalSubjectName + " (Select any " + lstSubjects[0].NoOfSubjects + ")";
                }
                else sSubjectName = oSubject.SubjectName;

                TreeNode otrndSubject = new TreeNode(sSubjectName, sSubjectId)
                                            {
                                                SelectAction = TreeNodeSelectAction.None,
                                                ShowCheckBox = oSubject.OptionalSubjectsId != 0
                                            };

                if (oSubject.SubjectGroupId != 0)
                {
                    // If optional subject group has child subject group then adding sub-subjects in tree view.
                    List<OptionalSubject> oOptionalSubject = aoOptionalSubjectDetails.Where(oSubjectGroup => oSubjectGroup.SubjectGroupId == oSubject.SubjectGroupId && oSubjectGroup.OptionalSubjectsId == 0).ToList();
                    otrndSubject = AddSubjectToSubjectGroup(oSubject.SubjectGroupId, otrndSubject, oOptionalSubject);
                }

                if (oSubject.ChildOptionalSubjectId != 0)
                {
                    // If optional subject group has child optional subject group then to add optional subject optional subject group details.
                    otrndSubject.ShowCheckBox = false;
                    AddSubjectToOptionalSubjectGroup(oSubject.ChildOptionalSubjectId, otrndSubject, aoOptionalSubjectDetails);
                }

                aotrndOptionalSubjectGroup.ChildNodes.Add(otrndSubject);
            }
        );

        return aotrndOptionalSubjectGroup;
    }

    /// <summary>
    /// This method is used to add subjects to subject group node in tree view.
    /// </summary>
    /// <param name="aiSubjectGroupId"></param>
    /// <param name="aotrndSubjectGroup"></param>
    /// <param name="alstOptionalSubject"></param>
    /// <returns></returns>
    private TreeNode AddSubjectToSubjectGroup(int aiSubjectGroupId, TreeNode aotrndSubjectGroup, List<OptionalSubject> alstOptionalSubject)
    {
        alstOptionalSubject.Where(oOptionalSubject => oOptionalSubject.SubjectGroupId == aiSubjectGroupId && oOptionalSubject.OptionalSubjectsId == 0).ToList().ForEach(
            oSubject =>
            {
                TreeNode otrndSubject = new TreeNode(oSubject.SubjectName, oSubject.SubjectId.ToString())
                                            {SelectAction = TreeNodeSelectAction.None};
                aotrndSubjectGroup.ChildNodes.Add(otrndSubject);
            }
        );

        return aotrndSubjectGroup;
    }

    /// <summary>
    /// This method is used to initialize all controls.
    /// </summary>
    private void Initialize()
    {
	    if (moUserRole == Constants.UserRoles.Teacher && !bool.Parse(hidUserHasFullAccess.Value))
		    ClassTeacherView();
	    else
		    OtherUsersView();
    }

    /// <summary>
    /// This method is used to set contols for class teacher.
    /// </summary>
    private void ClassTeacherView()
    {
        ReadQueryString();
        tdTeacherlbl.Visible = false;
        tdTeachercmb.Visible = false;
        TeacherStandardDetailsCollectionBL oTeacherStandardDetailsCollectionBL = new TeacherStandardDetailsCollectionBL(miSchoolId, miAcademicYearId);
        hidStandardDivisionId.Value = oTeacherStandardDetailsCollectionBL.GetStdDivIdOfClassTeacher(miTeacherId).ToString();
    }

    /// <summary>
    /// This method is used to initialize controls for other users.
    /// </summary>
    private void OtherUsersView()
    {
        hidUserHasFullAccess.Value = "true";
        FillClassCombo();
    }

    /// <summary>
    /// This method is used to get values from querry string.
    /// </summary>
    private void ReadQueryString()
    {
        miTeacherId = QueryString["TeacherId"].ToInt();
    }

    /// <summary>
    /// This method is used to get xml of student marks transfer details. 
    /// </summary>
    /// <returns></returns>
    private string GetTransferSubjectMarksXml()
    {
        List<TransferSubjectMarksInfo> oTransferMarksDetails = new List<TransferSubjectMarksInfo>();
        List<int> lstSubject = new List<int>();

        //Get selected nodes.
        foreach (TreeNode oNode in trvwOptionalSubject.Nodes)
            lstSubject = GetSelectedSubjectsList(lstSubject, oNode);

        //Getting selected students.
        (from oCurrentItem in lstvwStudentMarks.Items
                 let chkSelect = (CheckBox)oCurrentItem.FindControl("ChkSelectAll")
                 where chkSelect.Checked
                 select new TransferSubjectMarksInfo()
                 {
                     StandardDivisionId = Convert.ToInt32(lstvwStudentMarks.DataKeys[oCurrentItem.DisplayIndex]["Standard_Division_Id"]),
                     StudentId = Convert.ToInt32(lstvwStudentMarks.DataKeys[oCurrentItem.DisplayIndex]["YearwiseStudentId"])
                 }).ToList().ForEach(
                    oStudentSubject =>
                    {
                        //For each selected student and subject creating object.
                        lstSubject.ForEach(
                            oSubject =>
                            {
                                int iSubjectGroupId = mlstOptionalSubjects.Where(OptionalSubject => OptionalSubject.SubjectId == oSubject).ToList()[0].SubjectGroupId;
                                oTransferMarksDetails.Add(new TransferSubjectMarksInfo()
                                {
                                    StudentId = oStudentSubject.StudentId,
                                    StandardDivisionId = oStudentSubject.StandardDivisionId,
                                    SubjectId = oSubject,
                                    SubjectGroupId = iSubjectGroupId,
                                });
                            });
                    }
                );

        return oTransferMarksDetails.Count > Constants.I_ZERO ? CommonUtility.GenerateXml(oTransferMarksDetails) : string.Empty;
    }

    /// <summary>
    /// This method is used to set confirmation messaege on change of page.
    /// </summary>
    private void SetConfirmationMessage()
    {
        DataPager oDataPager = lstvwStudentMarks.FindControl("DtPgDropDown") as DataPager;
        DropDownList ddlCnt = oDataPager.Controls[0].FindControl("ddlCnt") as DropDownList;
        ddlCnt.Attributes.Add("onchange", "if(!ConfirmMessage('" + ddlCnt.ClientID + "')){return false;}");
        cmbTeachers.Attributes.Add("onchange", "if(!ConfirmMessage('" + null + "')){return false;}");
    }

    /// <summary>
    /// This method is used to fill standard's combo.
    /// </summary>
    private void FillClassCombo()
    {
        TeacherStandardDetailsBL oTeacherStandardDetailsBL = new TeacherStandardDetailsBL();
        hidUserHasFullAccess.Value = CommonUtility.IsUserHasScreenAccess(Constants.SchoolConfigurations.ExamResults).ToString();
        int iTeacherId = (!bool.Parse(hidUserHasFullAccess.Value)) ? miTeacherId : Constants.I_ZERO;
        ListSource.FillDropDownList(
                   oTeacherStandardDetailsBL.GetClassTeachersForOptionalSubjectClasses(
                                             miAcademicYearId,
                                             miSchoolId,
                                             iTeacherId),
                   cmbTeachers,
                   "TeacherName",
                   "StandardDivisionId",
                   Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to set client side attributes.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        trStudentInfo.Visible = false;
        btnSearch.Enabled = false;
        btnTransfer.Enabled = false;
        btnSearchBack.Visible = false;
        tblNotes.Visible = false;
        new[] { btnBack, btnTransfer, btnSearch }.ApplyEffect();
        btnTransfer.Attributes.Add("onclick", "if(!ValidateSubjects()) {return false;}");
    }

    /// <summary>
    /// This method is used to fill student list.
    /// </summary>
    private void FillStudentList()
    {
        lblErrorMsg.Text = string.Empty;
        tblPagerStudentMarksTransfer.Visible = true;
        hidStandardDivisionId.Value = cmbTeachers.SelectedValue;
        lstvwStudentMarks.DataSourceID = ObjDSStudentMarksTransfer.ID;
        lstvwStudentMarks.DataBind();
    }

    /// <summary>
    /// This function checks the preconditons of Optional Subject Configuration.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.TransferOptionalSubjectMarks);

        if (sLinks.Equals(string.Empty))
        {
            divErr.Visible = false;
            bReturn = true;
        }
        else
        {
            divErr.InnerHtml = sLinks;
            btnTransfer.Visible = btnBack.Visible = false;
            trTeacherControl.Visible = btnSearch.Visible = false;
            trStudentInfo.Visible = false;
            tblNotes.Visible = false;
        }

        return bReturn;
    }

    /// <summary>
    /// This method is used to set control state as per passed value.
    /// </summary>
    /// 
    /// <param name="abValue"></param>
    private void SetControlState(bool abValue)
    {
        trStudentInfo.Visible = abValue || cmbTeachers.SelectedValue != Constants.S_ZERO;
        tdOptionalSubject.Visible = abValue;
        btnSearch.Enabled = abValue || cmbTeachers.SelectedValue != Constants.S_ZERO;
        btnTransfer.Enabled = abValue;
        tblNotes.Visible = abValue;
    }
    #endregion "Private Methods"
}