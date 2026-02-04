/* -----------------------------------------------------------------------
 *	Author		: Vishal B. Shah
 *	Date		: 9-Mar-2012
 *	Purpose		: Allows configuring templates for cheque printing.
 * -----------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.ServiceModel;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using AccountsEntities;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SchoolBusinessService;
using SchoolEntities;
using Utility;

/// <summary>
/// Allows configuring templates for cheque printing, for existing banks in the accounts module.
/// </summary>
public partial class ChequeTemplateConfigurationUI : SchoolBase
{

	#region -- CONSTANT(s) --

	private const string S_SAVE_MESSAGE = "Cheque template configuration saved successfully!!!";
	private const string S_SAVE_ERROR_MESSAGE = "Failed to save cheque template configuration.";
	private const string S_UPDATE_MESSAGE = "Cheque template configuration updated successfully!!!";
	private const string S_UPDATE_ERROR_MESSAGE = "Failed to update cheque template configuration.";
	private const string S_DELETE_MESSAGE = "Cheque template configuration deleted successfully!!!";
	private const string S_DELETE_ERROR_MESSAGE = "Failed to delete cheque template configuration.";

	private const string S_DELETE_ROW = "DELETE_ROW";

	#endregion -- CONSTANT(s) --

	#region -- MEMBER(s) --

    private BankAccountClient moBankAccountClient;

	#endregion -- MEMBER(s) --

	#region -- EVENT HANDLER(s) --
	
	/// <summary>
	/// Hnadles the loading of the page.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{			
			if (!IsPostBack)
			{
				InitBankService();
				FillBankList();
				DisplayConfigurations();
				Initialize();
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
		finally
		{
			CloseBankService();
		}
	}

	/// <summary>
	/// Rebinds the grid with the selected item.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void ddlBankList_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			InitBankService();
			DisplayConfigurations();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
		finally
		{
			CloseBankService();
		}
	}

	/// <summary>
	/// Saves the configuration to db.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnSave_Click(object sender, EventArgs e)
	{
		bool bIsUpdate = !hidConfigId.Value.IsNullOrEmpty() && hidConfigId.Value != Constants.S_ZERO;
		try
		{
			InitBankService();
			ChequeConfiguration oChqConfig = GetChequeConfiguration();
			
			string sResult = moBankAccountClient.SaveChqConfiguration(oChqConfig);
			if (sResult.IsNullOrEmpty())
			{
				SetMessage(bIsUpdate ? S_UPDATE_MESSAGE : S_SAVE_MESSAGE, false);
				DisplayConfigurations();
			}
			else
			{
				SetControlsVisibility(true);
				SetMessage(sResult, true);
			}
		}
		catch (Exception ex)
		{
			SetMessage(bIsUpdate ? S_UPDATE_ERROR_MESSAGE : S_SAVE_ERROR_MESSAGE, true);
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
		finally
		{
			CloseBankService();
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwChqConfigurations_ItemCommand(object sender, ListViewCommandEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				if (e.CommandName == S_DELETE_ROW)
				{
					InitBankService();
					var oChqConfig = new ChequeConfiguration
					                 	{
					                 		Id			 = e.CommandArgument.ToInt(),
											SchoolId	 = miSchoolId,
											InsertedById = miUserId
					                 	};

					if (moBankAccountClient.DeleteChqConfiguration(oChqConfig))
					{
						SetMessage(S_DELETE_MESSAGE, false);
						DisplayConfigurations();
					}
					else
						SetMessage(S_DELETE_ERROR_MESSAGE, true);
				}
			}
		}
		catch (Exception ex)
		{
			SetMessage(S_DELETE_ERROR_MESSAGE, true);
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
		finally
		{
			CloseBankService();
		}
	}

	/// <summary>
	/// Sets the onclick attributes for Edit & Copy buttons.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstvwChqConfigurations_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				var oCurrentItem = e.Item as ListViewDataItem;
				int iConfigId = lstvwChqConfigurations.DataKeys[oCurrentItem.DisplayIndex]["Id"].ToInt();
				string sName = lstvwChqConfigurations.DataKeys[oCurrentItem.DisplayIndex]["Name"].ToString();
				
				var imgBtn = oCurrentItem.FindControl("imgbtnEdit") as ImageButton;
				imgBtn.Attributes["onclick"] = String.Format("EditConfig({0}, '{1}'); return false;", iConfigId, sName.Replace("'", @"\'"));
				
				imgBtn = oCurrentItem.FindControl("imgbtnCopy") as ImageButton;
				imgBtn.Attributes["onclick"] = String.Format("CopyConfig({0}); return false;", iConfigId);
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	#endregion -- EVENT HANDLER(s) --

	#region -- PRIVATE METHOD(s) --

	/// <summary>
	/// Initializes the Accounts Bank Service client object.
	/// </summary>
	private void InitBankService()
	{
        moBankAccountClient = new BankAccountClient();
		moBankAccountClient.Open();
	}

	/// <summary>
	/// Disposes off the Accounts Bank Service client object.
	/// </summary>
	private void CloseBankService()
	{
		if (moBankAccountClient != null && moBankAccountClient.State != CommunicationState.Faulted)
			moBankAccountClient.Close();
	}

	/// <summary>
	/// Sets properties for controls on the Page.
	/// </summary>
	private void Initialize()
	{
		valSummary.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
		ApplyMouseHoverEffect(new List<Button> { btnAdd, btnCancel, btnClose, btnSave });
	}

	/// <summary>
	/// Populates the Banks Dropdownlist with values.
	/// </summary>
	private void FillBankList()
	{
		List<Bank> lstBanks = moBankAccountClient.GetAllBanksForChequeConfiguration(miSchoolId, miFinancialYearId);
		ddlBankList.Bind(lstBanks, "Id", "Name");
	}

	/// <summary>
	/// 
	/// </summary>
	private void DisplayConfigurations()
	{
		int iBankId = ddlBankList.SelectedValue.ToInt();
		List<ChequeConfiguration> lstChqConfigurations = moBankAccountClient.GetChequeConfigurationsForBank(miSchoolId, iBankId);
		lstvwChqConfigurations.DataSource = lstChqConfigurations;
		lstvwChqConfigurations.DataBind();

		SerializeConfigurations(lstChqConfigurations);
		SetControlsVisibility(false);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="alstChqConfigurations"></param>
	private void SerializeConfigurations(List<ChequeConfiguration> alstChqConfigurations)
	{
		var obj = new Dictionary<string, object>();
		alstChqConfigurations.ForEach(cfg => obj.Add(cfg.Id.ToString(), GetJSONObject(cfg.ConfigXML)));

		var jsSerializer = new JavaScriptSerializer();
		hidConfigJSON.Value = String.Format("[{0}]", jsSerializer.Serialize(obj));
	}

	/// <summary>
	/// Returns a new JSON object
	/// </summary>
	/// <param name="asConfigXML"></param>
	/// <returns></returns>
	private object GetJSONObject(string asConfigXML)
	{
		var oXmlDocument = new XmlDocument();
		oXmlDocument.LoadXml(asConfigXML);
		JObject jObject = JObject.Parse(JsonConvert.SerializeXmlNode(oXmlDocument, Newtonsoft.Json.Formatting.None, true));
		
		if (!jObject.IsNull())
			return new
		       		{
		       			Date = new
		       		       		{
									Show = jObject["Date"]["@Show"].ToString(),
		       		       			Top  = jObject["Date"]["@Top"].ToString(),
									Left = jObject["Date"]["@Left"].ToString()
		       		       		},
						Payee = new
		       		       		{
									Show  = jObject["Payee"]["@Show"].ToString(),
		       		       			Top   = jObject["Payee"]["@Top"].ToString(),
									Left  = jObject["Payee"]["@Left"].ToString(),
									Width = jObject["Payee"]["@Width"].ToString()
		       		       		},
						Amount = new
		       		       		{
									Show = jObject["Amount"]["@Show"].ToString(),
		       		       			Top  = jObject["Amount"]["@Top"].ToString(),
									Left = jObject["Amount"]["@Left"].ToString()
		       		       		},
						AmountInWords = new
		       		       				{
											Show		= jObject["AmountInWords"]["@Show"].ToString(),
		       		       					Top			= jObject["AmountInWords"]["@Top"].ToString(),
											Left		= jObject["AmountInWords"]["@Left"].ToString(),
											Width		= jObject["AmountInWords"]["@Width"].ToString(),
											Height		= jObject["AmountInWords"]["@Height"].ToString(),
											Indent		= jObject["AmountInWords"]["@Indent"].ToString(),
											LineSpacing = jObject["AmountInWords"]["@LineSpacing"].ToString()
		       		       				},
						Company = new
		       		       		{
									Show  = jObject["Company"]["@Show"].ToString(),
		       		       			Top	  = jObject["Company"]["@Top"].ToString(),
									Left  = jObject["Company"]["@Left"].ToString(),
									Width = jObject["Company"]["@Width"].ToString(),
									Name  = jObject["Company"]["@Name"].ToString()
		       		       		},
						Signatory1 = new
		       		       		{
									Show  = jObject["Signatory1"]["@Show"].ToString(),
		       		       			Top	  = jObject["Signatory1"]["@Top"].ToString(),
									Left  = jObject["Signatory1"]["@Left"].ToString(),
									Width = jObject["Signatory1"]["@Width"].ToString(),
									Name  = jObject["Signatory1"]["@Name"].ToString()
		       		       		},
						Signatory2 = new
		       		       		{
									Show  = jObject["Signatory2"]["@Show"].ToString(),
		       		       			Top	  = jObject["Signatory2"]["@Top"].ToString(),
									Left  = jObject["Signatory2"]["@Left"].ToString(),
									Width = jObject["Signatory2"]["@Width"].ToString(),
									Name  = jObject["Signatory2"]["@Name"].ToString()
		       		       		}
		       		};
	
		return null;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="abShowEditor"></param>
	private void SetControlsVisibility(bool abShowEditor)
	{
		configList.Style["display"]		= !abShowEditor ? "" : "none";
		inputContainer.Style["display"] =  abShowEditor ? "" : "none";
		canvas.Style["display"]			=  abShowEditor ? "" : "none";
		btnSave.Style["display"]		=  abShowEditor ? "" : "none";
		btnCancel.Style["display"]		=  abShowEditor ? "" : "none";
		btnAdd.Style["display"]			= !abShowEditor ? "" : "none";
		if (!abShowEditor)
			hidConfigId.Value = String.Empty;
	}

	/// <summary>
	/// Returns a ChequeConfiguration entity using values on the Page.
	/// </summary>
	/// <returns></returns>
	private ChequeConfiguration GetChequeConfiguration()
	{
		return new ChequeConfiguration
		       	{
		       		Id			 = hidConfigId.Value.IsNullOrEmpty() ? 0 : hidConfigId.Value.ToInt(),
					Name		 = txtTemplateName.Text.Trim(),
					Bank		 = new Bank { Id = ddlBankList.SelectedValue.ToInt() },
					ConfigXML	 = GetConfigXML(),
					SchoolId	 = miSchoolId,
					InsertedById = miUserId
		       	};
	}

	/// <summary>
	/// Creates an XML of element dimensions specified on the Page.
	/// </summary>
	/// <returns></returns>
	private string GetConfigXML()
	{
		var oXElement = new XElement("ChequeConfiguration",
									 new XElement("Date",
												  new XAttribute("Show", chkDate.Checked),
												  new XAttribute("Top" , txtDateTop.Text.Trim()),
												  new XAttribute("Left", txtDateLeft.Text.Trim())),
									 new XElement("Payee",
												  new XAttribute("Show" , chkPayee.Checked),
												  new XAttribute("Top"  , txtPayeeTop.Text.Trim()),
												  new XAttribute("Left" , txtPayeeLeft.Text.Trim()),
												  new XAttribute("Width", txtPayeeWidth.Text.Trim())),
									 new XElement("Amount",
												  new XAttribute("Show", chkAmount.Checked),
												  new XAttribute("Top" , txtAmountTop.Text.Trim()),
												  new XAttribute("Left", txtAmountLeft.Text.Trim())),
									 new XElement("AmountInWords",
												  new XAttribute("Show"		  , chkAmountInWords.Checked),
												  new XAttribute("Top"		  , txtAmountInWordsTop.Text.Trim()),
												  new XAttribute("Left"		  , txtAmountInWordsLeft.Text.Trim()),
												  new XAttribute("Width"	  , txtAmountInWordsWidth.Text.Trim()),
												  new XAttribute("Height"	  , txtAmountInWordsHeight.Text.Trim()),
												  new XAttribute("Indent"	  , txtAmountInWordsIndent.Text.Trim()),
												  new XAttribute("LineSpacing", txtAmountInWordsLineHeight.Text.Trim())),
									 new XElement("Company",
												  new XAttribute("Show" , chkCompanyName.Checked),
												  new XAttribute("Top"	, txtCompanyTop.Text.Trim()),
												  new XAttribute("Left"	, txtCompanyLeft.Text.Trim()),
												  new XAttribute("Width", txtCompanyWidth.Text.Trim()),
												  new XAttribute("Name"	, txtCompanyName.Text.Trim())),
									 new XElement("Signatory1",
												  new XAttribute("Show" , chkSignatory1.Checked),
												  new XAttribute("Top"	, txtSignatory1Top.Text.Trim()),
												  new XAttribute("Left"	, txtSignatory1Left.Text.Trim()),
												  new XAttribute("Width", txtSignatory1Width.Text.Trim()),
												  new XAttribute("Name"	, txtSignatory1Name.Text.Trim())),
									 new XElement("Signatory2",
												  new XAttribute("Show" , chkSignatory2.Checked),
												  new XAttribute("Top"	, txtSignatory2Top.Text.Trim()),
												  new XAttribute("Left"	, txtSignatory2Left.Text.Trim()),
												  new XAttribute("Width", txtSignatory2Width.Text.Trim()),
												  new XAttribute("Name"	, txtSignatory2Name.Text.Trim()))
									 );
		
		return oXElement.ToString();
	}

	/// <summary>
	/// Sets a message to be displayed on the Page.
	/// </summary>
	/// <param name="asMessage"></param>
	/// <param name="abIsError"></param>
	private void SetMessage(string asMessage, bool abIsError)
	{
		lblMessage.Visible = true;
		lblMessage.Text = asMessage;
		if (abIsError)
		{
			lblMessage.Font.Bold = false;
			lblMessage.ForeColor = Color.Red;
		}
		else
		{
			lblMessage.Font.Bold = true;
			lblMessage.ForeColor = Color.Blue;
		}
	}

	#endregion -- PRIVATE METHOD(s) --

}