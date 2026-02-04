using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using Utility;
using System.Reflection;
using System.Collections.Generic;
using BusinessLogic.Exceptions;

/// <summary>
/// This class displays the Category details.
/// Only Admin users have access to the page.
/// 1. User 1st Enter the category name in textbox.
/// 2. And then Update or Delete existing category.
/// </summary>

public partial class CategoryUI : SchoolBase
{
    #region " Date Member and Constant "

    DataTable moDTCategory;
    const int I_PRINTABLE = 1;
    const int I_NON_PRINTABLE = 0;

    #endregion 

    #region " Events "

    /// <summary>
    /// This event is used to fill tree view and category combo box and also set default property on page load.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {			
            if (!IsPostBack)
            {
                SetDefaultControl();                
                FillTreeView();
                FillCategoryComboBox();
                tvwCategory.CollapseAll();                
            }
            SetClientScriptAttributes();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used for Add or Update Category detains in database.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (hidCategoryId.Value == string.Empty)
                AddBookCategory();                
            else
                UpdateCategory();
            FillTreeView();
            FillCategoryComboBox();
            ClearAllControl();
            btnDelete.Enabled = false;
        }
        catch (BusinessLogic.Exceptions.DuplicateEntityException )
        {
            lblErrorMessage.Visible = true;
            lblErrorMessage.Text = "Category Name already exists."; ;
        }
        catch (BusinessLogic.Exceptions.ReferenceExceptions)
        {
            lblErrorMessage.Visible = true;
            lblErrorMessage.Text = "Category cannot be added. Since it is already assigned to book.";
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set Printable or Non printable media type and fill respective combo box. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optPrintable_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            GetCategoryDetails();
            FillCategoryComboBox();
            txtCategory.Text = "";
            tvwCategory.CollapseAll();
            hidCategoryId.Value = string.Empty;
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to delete category from database side.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            const int I_PRINTABLE_NODE = 0;
            const int I_NON_PRINTABLE_NODE = 1;
            DeleteCategory();
            FillTreeView();
            FillCategoryComboBox();
            ClearAllControl();
            if (tvwCategory.Nodes[I_PRINTABLE_NODE].ChildNodes.Count == 0
                && tvwCategory.Nodes[I_NON_PRINTABLE_NODE].ChildNodes.Count == 0)
				DeleteConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.CategoryManagement));
            lblMessage.Visible = true;           
            lblMessage.Text = "Category deleted successfully!!!";
        }
        catch (BusinessLogic.Exceptions.ReferenceExceptions ex)
        {
            lblErrorMessage.Visible = true;
            lblErrorMessage.Text = ex.Message;
            FillTreeView();
            FillCategoryComboBox();
            ClearAllControl();
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to reset all category related controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (tvwCategory.SelectedNode != null)
                tvwCategory.SelectedNode.Selected = false;
            ClearAllControl();
            GetCategoryDetails();
            FillCategoryComboBox();
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
      
    /// <summary>
    /// this event is used to set the back page URL.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param> 
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Library_Related)));
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set category detail for edit and delete purpose.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void tvwCategory_SelectedNodeChanged(object sender, EventArgs e)
    {
        try
        {
            txtCategory.Text = tvwCategory.SelectedNode.Text;
            hidCategoryName.Value = tvwCategory.SelectedNode.Text;
            int iCategoryId = Convert.ToInt32(tvwCategory.SelectedNode.Value);
            CategoryBL oCategoryBL = new CategoryBL(iCategoryId);
            if (oCategoryBL.IsPrintable == 1)
                SetPrintableRadioBtn(true);
            else if (oCategoryBL.IsPrintable == 0)
                SetPrintableRadioBtn(false);
            GetCategoryDetails();
            FillCategoryComboBox();
            if (oCategoryBL.CategoryLevel == 0)
            {
                hidCategoryId.Value = Convert.ToString(tvwCategory.SelectedNode.Value);
                cmbMainCategory.SelectedValue = hidCategoryId.Value;
                hidIsSubCategory.Value = "false";
                lblCategory.Text = "Category Name :";
            }
            if (oCategoryBL.CategoryLevel > 0)
            {
                hidCategoryId.Value = oCategoryBL.ParentId.ToString();                
                hidSubCategoryId.Value = oCategoryBL.CategoryId.ToString();
                cmbMainCategory.SelectedValue = oCategoryBL.ParentId.ToString();
                hidIsSubCategory.Value = "true";
                lblCategory.Text = "Sub Category Name :";
            }
            btnDelete.Enabled = true;
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region " Private Methods "

    /// <summary>
    /// This method is used to set printable/non-printable radio button as per requirement.
    /// </summary>
    /// <param name="abFlag"></param>
    private void SetPrintableRadioBtn(bool abFlag)
    {
        optPrintable.Checked = abFlag;
        optNonPrintable.Checked = !abFlag;
    }

    /// <summary>
    /// This method is used to set validation header summary, media type and reset category id hidden field.
    /// </summary>
    private void SetDefaultControl()
    {   
        SetPrintableRadioBtn(true);
        cmbMainCategory.Focus();        
        hidCategoryId.Value = string.Empty;
        valsumMainCategory.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        valsumSubCategory.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;        
    }

    /// <summary>
    /// This event is used for delete category and check dependancy.
    /// </summary>
    private void DeleteCategory()
    {
        CategoryBL oCategoryBL =  InitializeCategoryBL();

        if (hidIsSubCategory.Value == "false" )
        {
            if (hidCategoryId.Value != "")
            {
                int iCategoryID = Convert.ToInt32(hidCategoryId.Value);
                CategoryBL.GetDependanciesForCategory(iCategoryID, hidCategoryName.Value, miAcademicYearId);
                CategoryBL.GetDependanciesForSubCategory(iCategoryID, hidCategoryName.Value, miAcademicYearId);
                oCategoryBL.CategoryId = iCategoryID;
                oCategoryBL.DeleteCategory();
            }
        }
        else
        {
            if (hidSubCategoryId.Value != "")
            {
                int iSubCategoryID = Convert.ToInt32(hidSubCategoryId.Value);
                CategoryBL.GetDependanciesForCategory(iSubCategoryID, hidCategoryName.Value, miAcademicYearId);
                CategoryBL.GetDependanciesForSubCategory(iSubCategoryID, hidCategoryName.Value, miAcademicYearId);
                oCategoryBL.CategoryId = iSubCategoryID;
                oCategoryBL.DeleteCategory();
            }
        }
        if (txtCategory.Text == hidCategoryName.Value)
        {
            txtCategory.Text = "";
            hidIsSubCategory.Value = "true";
        }        
    }

    /// <summary>
    /// This method is used to populate category BL objects.
    /// </summary>
    /// <returns></returns>
    private CategoryBL PopulateCategoryBL()
    {
        CategoryBL oCategoryBL = new CategoryBL();

        oCategoryBL.CategoryName = txtCategory.Text;
        if (hidCategoryId.Value != "")
            oCategoryBL.CategoryId = Convert.ToInt32(hidCategoryId.Value);

        if (optPrintable.Checked)
            oCategoryBL.IsPrintable = 1;
        else
            oCategoryBL.IsPrintable = 0;

        if (cmbMainCategory.SelectedIndex != 0)
            oCategoryBL.ParentId = Convert.ToInt32(cmbMainCategory.SelectedValue);
        
        return oCategoryBL;
    }

    /// <summary>
    /// This method initialises session variables like shcool id and academic year id 
    /// and set default control value.
    /// </summary>
    private CategoryBL InitializeCategoryBL()
    {
        CategoryBL oCategoryBL = PopulateCategoryBL();
        oCategoryBL.SchoolId = miSchoolId;
        oCategoryBL.UpdatedById = miUserId;
        oCategoryBL.InsertedById = miUserId;
        oCategoryBL.UserId = miUserId;
        oCategoryBL.UpdatedDate = DateTime.Now;
        
        return oCategoryBL;
    }

    /// <summary>
    /// This method is used to add book category in data base side.
    /// </summary>
    private void AddBookCategory()
    {
        CategoryBL oCategoryBL = InitializeCategoryBL();
        if (cmbMainCategory.SelectedIndex == 0)
        {
            oCategoryBL.IsDuplicateCategory();
            ShowMessage("Category name added");
        }
        else
        {
            int iCategoryID = Convert.ToInt32(cmbMainCategory.SelectedValue);
            oCategoryBL.CategoryId = iCategoryID;
            oCategoryBL.IsDuplicateSubCategory();
            CategoryBL.GetDependanciesForCategory(iCategoryID, cmbMainCategory.SelectedItem.Text, miAcademicYearId);            
            ShowMessage("Sub category name added");
        }
        oCategoryBL.AddCategory();

        if (QueryString["Is_Configured"] == null || QueryString["Is_Configured"] == Constants.S_NO)
			SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.CategoryManagement));
        
    }
    
    /// <summary>
    /// This method is used for updating category details in database side.
    /// </summary>
    private void UpdateCategory()
    {
        CategoryBL oCategoryBL =  InitializeCategoryBL();
        if (hidIsSubCategory.Value == "false")
        {   
            oCategoryBL.CategoryId = Convert.ToInt32(hidCategoryId.Value);
            oCategoryBL.IsDuplicateCategory();
            oCategoryBL.UpdateCategory();
            ShowMessage("Category name updated");
        }
        else
        {
            oCategoryBL.CategoryId = Convert.ToInt32(cmbMainCategory.SelectedValue);
            oCategoryBL.SubCategoryName = txtCategory.Text;
            oCategoryBL.SubCategoryId = Convert.ToInt32(hidSubCategoryId.Value);
            oCategoryBL.IsDuplicateSubCategory();
            oCategoryBL.UpdateSubCategory();
            ShowMessage("Sub category name updated");
        }        
    }

    /// <summary>
    /// This method is used to show successfull message.
    /// </summary>
    private void ShowMessage(string asMessage)
    {        
        lblMessage.Visible = true;
        lblMessage.Text = asMessage + " successfully !!!";
    }

    /// <summary>
    /// This method is used to set javascript attribute on page load event.
    /// </summary>
    private void SetClientScriptAttributes()
    {
        lblErrorMessage.Visible = false;
        lblMessage.Visible = false;
        btnDelete.Enabled = false;
        optPrintable.Attributes.Add("onclick", "if(!ClearValSum()){return false;}");
        optNonPrintable.Attributes.Add("onclick", "if(!ClearValSum()){return false;}");
        tvwCategory.Attributes.Add("onclick", "if(!ClearValSum()){return false;}");
        cmbMainCategory.Attributes.Add("onchange", "if(!ResetCategoryName(this)){return false;}");
        btnNew.Attributes.Add("onclick", "if(!ClearValSum()){return false;}");
        btnSave.Attributes.Add("onclick", "if(!ClearText()){return false;}");
        btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");

		ApplyMouseHoverEffect(new List<Button>(){ btnBack, btnSave, btnDelete, btnNew});
    }

    /// <summary>
    /// This method is used to fill tree view node.
    /// </summary>
    /// <param name="aiCompanyId"></param>
    private void FillCategoryNodes(TreeNode oParentNode, int aiIsPrintable, int aiParentId)
    {
        pnlTree.Visible = true;

        DataRow[] oDataRow = moDTCategory.Select(" Is_Printable =" + aiIsPrintable + " AND Parent_Id=" + aiParentId);
        for (int iRowCount = 0; iRowCount < oDataRow.Length; iRowCount++)
        {
            TreeNode otrParentNode = new TreeNode();
            string sCategoryName = oDataRow[iRowCount]["Category_Name"].ToString();
            otrParentNode.Text = sCategoryName;
            string sMainCategoryId = oDataRow[iRowCount]["Category_Id"].ToString();
            otrParentNode.Value = sMainCategoryId;           
            oParentNode.ChildNodes.Add(otrParentNode);
            DataRow[] oDRChildNode = moDTCategory.Select("Parent_Id=" + Convert.ToInt32(sMainCategoryId));
            if (oDRChildNode.Length > 0)
                FillCategoryNodes(otrParentNode, aiIsPrintable, Convert.ToInt32(sMainCategoryId));
        }
    }

    /// <summary>
    /// This method is used to fill media type.
    /// </summary>
    private void FillTreeView()
    {
        GetCategoryDetails();
        tvwCategory.Nodes.Clear();
        FillPrintableMedia();
        FillNonPrintableMedia();
        SetPrintableRadioBtn(true);
    }

    /// <summary>
    /// This method is used to get category details and set data table.
    /// </summary>
    private void GetCategoryDetails()
    {
        CategoryBL oCategoryBL = new CategoryBL();
        oCategoryBL.SchoolId = miSchoolId;
        moDTCategory = oCategoryBL.RetriveMainCategoryList();
    }

    /// <summary>
    /// This method is used to fill  Printable media type.
    /// </summary>
    private void FillPrintableMedia()
    {
        TreeNode oPrintableNode = new TreeNode();
        oPrintableNode.Text = "Printable";
        tvwCategory.Nodes.Add(oPrintableNode);
        oPrintableNode.SelectAction = TreeNodeSelectAction.None;
        int iParentNode = 0;
        FillCategoryNodes(oPrintableNode, I_PRINTABLE, iParentNode);
    }

    /// <summary>
    /// This method is used to fill  Non Printable media type.
    /// </summary>
    private void FillNonPrintableMedia()
    {
        TreeNode oNonPrintableNode = new TreeNode();
        oNonPrintableNode.Text = "NonPrintable";
        tvwCategory.Nodes.Add(oNonPrintableNode);
        oNonPrintableNode.SelectAction = TreeNodeSelectAction.None;
        int iParentNode = 0;
        FillCategoryNodes(oNonPrintableNode, I_NON_PRINTABLE, iParentNode);
    }

    /// <summary>
    /// This method is used to fill category combo box as per media type ie Printable or non-Printable.
    /// </summary>
    private void FillCategoryComboBox()
    {
        DataRow[] oDRCategory;
        if (optPrintable.Checked)
            oDRCategory = moDTCategory.Select("Is_Printable =" + I_PRINTABLE);
        else
            oDRCategory = moDTCategory.Select("Is_Printable =" + I_NON_PRINTABLE);
        
        ControlUtility.FillDropDownList(oDRCategory, ref cmbMainCategory,
                                     "Category_Id",
                                     "Category_Name",
                                     "Consider as Parent");
    }
	
    /// <summary>
    /// This method is used to reset default category controls.
    /// </summary>
    private void ClearAllControl()
    {
        txtCategory.Text = string.Empty;
        hidCategoryName.Value = string.Empty;
        hidIsSubCategory.Value = "false";
        hidCategoryId.Value = string.Empty;
        hidSubCategoryId.Value = string.Empty;
        SetPrintableRadioBtn(true);
        cmbMainCategory.SelectedIndex = 0;
    }
    #endregion " End Private Methods "

   
}
