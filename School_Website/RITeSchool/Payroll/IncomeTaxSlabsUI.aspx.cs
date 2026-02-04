// File Name - IncomeTaxSlabsUI.aspx.cs
// Creator - Sunny
// Created Date - 
// Description - This class is used to configure incomer tax slabs.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using Utility;
using System.Data.SqlClient;

public partial class IncomeTaxSlabsUI : SchoolBase
{		
	#region Data Member(s)

	private IncomeTaxSlabsBL moIncomeTaxSlabsBL;	
	private IncomeTaxDetailsBL moIncomeTaxDetailsBL;
	private bool mbIsPublished;

	#endregion

	#region Event(s)

	/// <summary>
	/// This event is used to fill income tax slab details in list view.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
    {
		try
		{				
				moIncomeTaxSlabsBL = new IncomeTaxSlabsBL(miSchoolId,miFinancialYearId,miAcademicYearId,miUserId);
				moIncomeTaxDetailsBL = new IncomeTaxDetailsBL(miSchoolId,miFinancialYearId,miUserId,miAcademicYearId);
				hidIsConfigured.Value = QueryString["Is_Configured"];
				cmbCategory.Focus();				
				if(!IsPostBack)
				{
				  SetJavaScriptAttributes();				  
				  FillCategory();
				  FillSlabs();
				}
				btnSave.Attributes.Add("Onclick","ClearSuccessfulMessage()");

		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
    }		

	/// <summary>
	/// This method is  used to save/update income tax slab details.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnSave_Click(object sender, EventArgs e)
	{
        try
        {
            IncomeTaxSlab oIncomeTaxSlab = Populate();
            moIncomeTaxSlabsBL.Save(oIncomeTaxSlab);
            if (btnSave.Text == Constants.ButtonText.Update.ToString())
                DisplayMessage(Constants.ItemState.updated, false);
            else
            {
                DisplayMessage(Constants.ItemState.saved, false);
                if (hidIsConfigured.Value == Constants.S_NO)
                {
                    SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.IncomeTaxSlabs));
                    hidIsConfigured.Value = Constants.S_YES;
                }
            }
            FillSlabs();
            ResetFields();
        }
        catch (SqlException se)
        {
            DisplayMessage(se.Message, true, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

	}	
	/// <summary>
	/// This event is used to set maximum To amount for the selected category.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{		   
		   txtFromAmount.Text = moIncomeTaxSlabsBL.GetMaxToAmount(cmbCategory.SelectedValue.ToInt()).ToString();
           txtToAmount.Focus();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to disable save,edit and delete button when income tax is published.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwSlabs_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				if (mbIsPublished)
				{
					ImageButton oimgbtnDelete = e.Item.FindControl("btnDelete") as ImageButton;
					ImageButton oimgbtnEdit = e.Item.FindControl("btnEdit") as ImageButton;
					oimgbtnDelete.Enabled = oimgbtnEdit.Enabled = false;					
				}
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}
	/// <summary>
	/// This event is used to edit/delete configuration.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwSlabs_ItemCommand(object sender, ListViewCommandEventArgs e)
	{
		try
		{
			int iIncomeTaxRangeId = Convert.ToInt32(lstvwSlabs.DataKeys[e.Item.DisplayIndex]["Id"]);
			if (e.CommandName == Constants.S_COMMAND_UPDATE)
			{				
				hidIncomeTaxRangeId.Value = iIncomeTaxRangeId.ToString();
				IncomeTaxSlab oIncomeTaxSlab = moIncomeTaxSlabsBL.Get(iIncomeTaxRangeId);
				if (oIncomeTaxSlab != null)
				{
					cmbCategory.SelectedValue = oIncomeTaxSlab.Category.Id.ToString();
					txtToAmount.Text = oIncomeTaxSlab.ToAmount.ToString();
					txtFromAmount.Text = oIncomeTaxSlab.FromAmount.ToString();
					txtPercentage.Text = oIncomeTaxSlab.Percentage.ToString();
				}
				btnSave.Text = Constants.ButtonText.Update.ToString();
			}
			else if (e.CommandName == Constants.S_COMMAND_REMOVE)
			{		
				moIncomeTaxSlabsBL.Delete(iIncomeTaxRangeId);
				DisplayMessage(Constants.ItemState.deleted, false);
				FillSlabs();				
				if (hidIsConfigured.Value == Constants.S_YES && lstvwSlabs.Items.Count == 0)
				{
					DeleteConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.IncomeTaxSlabs));
					hidIsConfigured.Value = Constants.S_NO;
				}
				ResetFields();
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to go back to payroll dashboard.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnBack_Click(object sender, EventArgs e)
	{
		try
		{
			MasterPage oMasterPage = (MasterPage)this.Master;
			oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Payroll_Related)));
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to reset fields.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnCancel_Click(object sender, EventArgs e)
	{
		try
		{	
			ResetFields();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	#endregion

	#region Method(s)

	/// <summary>
	/// This method is used to populate object.
	/// </summary>
	/// <returns></returns>
		private IncomeTaxSlab Populate()
		{
			IncomeTaxSlab oIncomeTaxSlab = new IncomeTaxSlab
			{	
				Id = hidIncomeTaxRangeId.Value.ToInt(),			
				Category = new ITSlabCategory { Id = cmbCategory.SelectedValue.ToInt() },
				FromAmount = Convert.ToInt32(txtFromAmount.Text),
				ToAmount = Convert.ToInt32(txtToAmount.Text),
				Percentage = Convert.ToDouble(txtPercentage.Text),
			};
			return oIncomeTaxSlab;
		}

		/// <summary>
		/// This method is used to fill category combo box.
		/// </summary>
		private void FillCategory()
		{
			List<ITSlabCategory> lstITSlabCategories = moIncomeTaxSlabsBL.GetAllCategories();
			ListSource.FillDropDownList(lstITSlabCategories, cmbCategory, "Name", "Id", Constants.S_SELECT);
		}

		/// <summary>
		/// This method is used to fill listview of IT slabs.
		/// </summary>
		private void FillSlabs()
		{
			mbIsPublished = moIncomeTaxDetailsBL.CheckIsPublished();
			if (mbIsPublished)
				btnSave.Enabled = false;
			List<IncomeTaxSlab> lstIncomeTaxSlab = moIncomeTaxSlabsBL.GetAll();
			lstvwSlabs.DataSource = lstIncomeTaxSlab;
			lstvwSlabs.DataBind();
		}

		private void SetJavaScriptAttributes()
		{
			ApplyMouseHoverEffect(new List<Button>{btnCancel, btnBack, btnSave});
			valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;				
		}

		/// <summary>
		/// This method is used to reset fields.
		/// </summary>
		private void ResetFields()
		{
		    hidIncomeTaxRangeId.Value = Constants.S_ZERO;
			cmbCategory.ClearSelection();
			cmbCategory.Focus();		
			txtToAmount.Text = string.Empty;
			txtPercentage.Text = string.Empty;
			btnSave.Text = Constants.ButtonText.Save.ToString();
			txtFromAmount.Text = string.Empty;
		}

		/// <summary>
		/// This method is used to display message.
		/// </summary>
		/// <param name="aoItemState"></param>
		/// <param name="abIsErrorMessage"></param>
		private void DisplayMessage(Constants.ItemState aoItemState, bool abIsErrorMessage)
		{
			string sMessage = "Income tax slab has been " + aoItemState.ToString() + " successfully !!!";
			DisplayMessage(sMessage, abIsErrorMessage, tdMessage);
		}

	#endregion	
}