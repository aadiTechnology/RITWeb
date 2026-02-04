using System;
using System.Data;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class PhotoForStudent : SchoolBase
{

	#region -- MEMBER(s) --

	private const string S_PHOTO = "Photo";

	#endregion -- MEMBER(s) --

	#region -- EVENT HANDLER(s) --

	/// <summary>
	///		Handles the page load event.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{			
			ReadQueryString();
			if (!IsPostBack)
			{
				DataSet oDataSet = new DataSet();
				StudentBL oStudentBL = new StudentBL();
				oDataSet = oStudentBL.GetStudentPhoto(miSchoolId, miAcademicYearId, hidStandardId.Value, hidDivisionId.Value, hidName.Value, hidRegNo.Value, hidIsExactMatch.Value.ToInt(), hidOperator.Value, hidPrefix.Value);

				if (hidRegNo.Value == string.Empty)
				{
					ViewState[S_PHOTO] = oDataSet.Tables[0];
					lstVwMain.DataSource = oDataSet.Tables[1];
					lstVwMain.DataBind();                    
				}
				else
				{
					LstVwExactPhoto.DataSource = oDataSet.Tables[0];
					LstVwExactPhoto.DataBind();
				}
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	///		
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lstVwMain_ItemDataBound(object sender, ListViewItemEventArgs e)
	{
		try
		{
			Label lblStdDiv = e.Item.FindControl("lblClass") as Label;
			DataTable oDataTable = new DataTable();
			oDataTable = ViewState[S_PHOTO] as DataTable;

			EnumerableRowCollection<DataRow> query = from order in oDataTable.AsEnumerable() where order.Field<string>("Standard_Division_Name") == lblStdDiv.Text select order;
			DataView view = query.AsDataView();

			HtmlTableRow oHtmlTableRow = e.Item.FindControl("trPhoto") as HtmlTableRow;
			HtmlTableCell oHtmlTableCell = oHtmlTableRow.FindControl("tdPhoto") as HtmlTableCell;
			ListView ogroupListView = oHtmlTableCell.FindControl("groupListView") as ListView;
            
			ogroupListView.DataSource = view;
			ogroupListView.DataBind();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

    /// <summary>
    /// This is for outer listview Item data bound.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LstVwExactPhoto_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
            string sUserId = LstVwExactPhoto.DataKeys[oCurrentItem.DataItemIndex]["UserId"].ToString();
            HtmlImage imgStudent = oCurrentItem.FindControl("imgStudent1") as HtmlImage;
                
            if (imgStudent != null)
                imgStudent.Src = Constants.S_IMAGE_GENERATOR_PATH + "Value="  + sUserId;                  
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This is for inner listview Item data bound.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void groupListView_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
            HiddenField hidUserId = oCurrentItem.FindControl("hidUserId") as HiddenField;
            HtmlImage imgStudent = oCurrentItem.FindControl("imgStudent1") as HtmlImage;
            if (imgStudent != null)
                imgStudent.Src = Constants.S_IMAGE_GENERATOR_PATH + "Value="  + hidUserId.Value;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }    

	#endregion -- EVENT HANDLER(s) --

	#region -- PRIVATE METHOD(s) --

	/// <summary>
	///		This method is used to read querystring.
	/// </summary>
	private void ReadQueryString()
	{
		hidDivisionId.Value = QueryString["DivisionId"];
		hidStandardId.Value = QueryString["StandardId"];
		hidName.Value = QueryString["NameOrRegNo"];
		hidRegNo.Value = QueryString["sStudentReg"];
		hidIsExactMatch.Value = QueryString["IsExactMatch"];
		if (hidIsExactMatch.Value.ToInt() == 1)
		{
			if (QueryString["Operator"] != null)
				hidOperator.Value = QueryString["Operator"];

			if (QueryString["Prefix"] != null)
				hidPrefix.Value = QueryString["Prefix"];
		}
	}

	#endregion -- PRIVATE METHOD(s) --

    
}
