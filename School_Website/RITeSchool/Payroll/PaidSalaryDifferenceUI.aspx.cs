/* File Name :- PaidSalaryDifferenceUI.aspx.cs
 * Created By - Sachin
 * Created Date :- 26-July-2010
 * Class Description :- This class is used to display paid salary difference.
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class PaidSalaryDifferenceUI : SchoolBase
{
    #region Data members

    int miMonthId = 0;
    int miYear = 0;
    string msMonthName = string.Empty; 

    #endregion

    #region Data Members

    /// <summary>
    /// This event is used to decrypt querystring, set screen width and fillpaid salary difference grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                ReadQueryString();
                //SetControlWidth();
                SetJavascriptAttributes();
                FillPaidSalaryDifferenceGrid();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to format gridview columns.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdSalaryDifference_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            TableCellCollection cells = e.Row.Cells;
            int iCellIndex = 0;
            foreach (TableCell cell in cells)
            {
                cell.HorizontalAlign = HorizontalAlign.Right;
                if (iCellIndex < 2)
                    cell.HorizontalAlign = HorizontalAlign.Left;
                iCellIndex++;

                cell.Style.Add("padding-left", "5px");
                cell.Style.Add("padding-right", "5px");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    } 

    #endregion

    #region Methods

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {   
        ApplyMouseHoverEffect(new List<Button> { btnBack });
        btnBack.Attributes["onclick"] = "ClosePopup()";
    }

    /// <summary>
    /// This method is used to fill paid salary difference grid.
    /// </summary>
    private void FillPaidSalaryDifferenceGrid()
    {
        SalaryDifferenceBL oSalaryDifferenceBL = new SalaryDifferenceBL(miSchoolId, miAcademicYearId, miUserId);
        oSalaryDifferenceBL.GetPaidSalaryDifferenceDetails(miMonthId,miYear);
        var PaidSalaryDifferences = oSalaryDifferenceBL.SalaryDifferences;

        var columnCollection = from column in PaidSalaryDifferences.AsEnumerable()
                               orderby column.Year ascending, column.MonthId ascending
                               select new { ColumnName = Convert.ToString(String.Format("{0:MMMM}", Convert.ToDateTime("2010-" + column.MonthId + "-02")) + " - " + column.Year) };

        if (columnCollection.Count() > 0)
            columnCollection = columnCollection.Distinct();

        DataTable oDataTable = null;
        if (columnCollection.Count() > 0)
        {
            oDataTable = new DataTable();
            oDataTable.Columns.Add("UserId");
            oDataTable.Columns.Add("Name");
            oDataTable.Columns.Add("Designation");

            foreach (var column in columnCollection)
                oDataTable.Columns.Add(column.ColumnName);

            var userCollection = PaidSalaryDifferences.Select(user => new{ UserId = Convert.ToInt32(user.UserId), Name = user.Name, Designation = user.Designation });

            if (userCollection.Count() > 0)
            {   
                var SalaryDifferenceCollection = PaidSalaryDifferences
                                                 .Select(column =>
                                                                     new
                                                                     {
                                                                         UserId = Convert.ToInt32(column.UserId),
                                                                         ColumnName = Convert.ToString(String.Format("{0:MMMM}", Convert.ToDateTime("2010-" + column.MonthId + "-02")) + " - " + column.Year),
                                                                         Amount = Convert.ToInt32(Math.Round(column.Amount))
                                                                     }
                                                 );

                var Users = userCollection.Distinct();
                int iRowIndex = 0;
                foreach (var user in Users)
                {
                    oDataTable.Rows.Add();
                    oDataTable.Rows[iRowIndex]["UserId"] = user.UserId;
                    oDataTable.Rows[iRowIndex]["Name"] = user.Name;
                    oDataTable.Rows[iRowIndex]["Designation"] = user.Designation;

                    var monthwiseAmount = from column in columnCollection
                                          join salary in SalaryDifferenceCollection
                                          on column.ColumnName equals salary.ColumnName
                                          where Convert.ToInt32(salary.UserId) == Convert.ToInt32(user.UserId)
                                          select new
                                          {
                                              ColumnName = column.ColumnName,
                                              Amount = salary.Amount
                                          };

                    foreach (var amount in monthwiseAmount)
                        oDataTable.Rows[iRowIndex][amount.ColumnName] = amount.Amount;

                    iRowIndex++;
                }
            }
        }

        oDataTable.Columns.RemoveAt(0);
        grdSalaryDifference.DataSource = oDataTable;
        grdSalaryDifference.DataBind();
    }

    /// <summary>
    /// This method is used to decrypt querystring.
    /// </summary>
    private void ReadQueryString()
    {
        miMonthId = QueryString["MonthId"].ToInt();
        miYear = QueryString["Year"].ToInt();
        msMonthName = QueryString["MonthName"];

        lblHeader.Text = string.Empty;
        lblHeader.Text = "Salary Difference Paid in Month(s): " + msMonthName;
    }

    /// <summary>
    /// This method is used to set control width.
    /// </summary>
    private void SetControlWidth()
    {
        if (Session[Constants.S_SESSION_SCREEN_WIDTH] != null)
        {
            int iWidth = Convert.ToInt32(Session[Constants.S_SESSION_SCREEN_WIDTH]);
            iWidth = iWidth / 100 * 80;
            divContainer.Style.Add("width", iWidth.ToString() + "px !important");
            lblNoRecordMessage.Width = Unit.Pixel(iWidth);
        }
        else
        {
            divContainer.Style.Add("width", Convert.ToString(1024) + "px !important");
            lblNoRecordMessage.Width = Unit.Pixel(1024);
        }
    } 

    #endregion
}
