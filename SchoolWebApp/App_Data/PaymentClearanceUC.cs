using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Utility;


/// <summary>
/// Summary description for PaymentClearanceUC
/// </summary>
public class PaymentClearanceUC : System.Web.UI.UserControl
{

    public PaymentClearanceUC()
    { }

    public string StartIndex
    { get; set; }

    public string EndIndex
    { get; set; }

    public string Total
    { get; set; }

    public bool ShowTotalRecords
    { get; set; }

    private void DisplayRowDetails(GridView oGrid)
    {
        int iRowCount = ((DataView)(oGrid.DataSource)).Count;
        StartIndex = Convert.ToString((oGrid.PageSize * oGrid.PageIndex) + 1);
        EndIndex = Convert.ToString((Convert.ToInt32(EndIndex) + oGrid.PageSize) - 1);
        Total = iRowCount.ToString();
        if (Convert.ToInt32(EndIndex) > Convert.ToInt32(Total))
            EndIndex = iRowCount.ToString();
        if (iRowCount.ToString() == "0")
            ShowTotalRecords = false;
        else
            ShowTotalRecords = true;
        if (Total != "")
        {
            if (Convert.ToInt32(Total) <= Constants.I_GRID_PAGE_COUNT)
                ShowTotalRecords = false;
            else
                ShowTotalRecords = true;
        }
    }
    
    protected void SetClientScriptAttribute(List<Control> olstControls)
    {
        foreach (Control control in olstControls)
        {
            if (control.GetType() == typeof(Button))
            {
                ((Button)control).Attributes["onmouseover"] = "javascript:fnover('" + ((Button)control).ClientID + "');";
                ((Button)control).Attributes["onmouseout"] = "javascript:fnout('" + ((Button)control).ClientID + "');";
            }
            else if (control.GetType() == typeof(RadioButton))
                ((RadioButton)control).Attributes.Add("onclick", "if(!ClearValSum()){return false;}");
        }
    }

    protected void SetNewPageIndex(object sender, GridViewPageEventArgs e, GridView oGrid)
    {
        oGrid.PageIndex = e.NewPageIndex;
        FillPaymnetsGrid();
    }

    protected void HideButtons(Button btnSave, Button btnExport)
    {
        btnSave.Style.Add("Visibility", "Hidden");
        btnExport.Style.Add("Visibility", "Hidden");
    }

    protected string SetPageForGrid(GridView oGrid)
    {
        GridViewRow oPageRow = oGrid.BottomPagerRow;
        DropDownList oPageNumberList = (DropDownList)oPageRow.Cells[0].FindControl("PageDropDownList");
        oGrid.PageIndex = oPageNumberList.SelectedIndex;
        FillPaymnetsGrid();
        return (oPageNumberList.SelectedIndex + 1).ToString();
    }

    protected void SetDataPagerOfGrid(object sender, GridViewRowEventArgs e, GridView oGrid)
    {
        if (e.Row.RowType == DataControlRowType.Pager)
        {
            GridViewRow pagerRow = e.Row;

            // Retrieve the DropDownList and Label controls from the row.
            DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");
            pageList.Attributes.Add("onchange", "if(!MessageAboutDate('" + pageList.ClientID + "')){return false;}");
            Label pageLabel = (Label)pagerRow.Cells[0].FindControl("CurrentPageLabel");

            if (pageList != null)
            {

                // Create the values for the DropDownList control based on 
                // the  total number of pages required to display the data
                // source.
                for (int i = 0; i < oGrid.PageCount; i++)
                {
                    // Create a ListItem object to represent a page.
                    int pageNumber = i + 1;
                    ListItem item = new ListItem(pageNumber.ToString());
                    if (i == oGrid.PageIndex)
                        item.Selected = true;

                    // Add the ListItem object to the Items collection of the 
                    // DropDownList.
                    pageList.Items.Add(item);
                }
            }
            if (pageLabel != null)
            {
                // Calculate the current page number.
                int currentPage = oGrid.PageIndex + 1;

                // Update the Label control with the current page information.
                pageLabel.Text = "Page " + currentPage.ToString() +
                  " of " + oGrid.PageCount.ToString();
                DisplayRowDetails(oGrid);
            }
        }
        if (oGrid.ID == "grdCheques")
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int iRowIndex = ((GridViewRow)e.Row).RowIndex;
                int iPostDatedChequeId = Convert.ToInt32(oGrid.DataKeys[iRowIndex]["PostDated_Cheque_Id"]);
                if (iPostDatedChequeId == 0)
                    e.Row.BackColor = System.Drawing.Color.LightBlue;
                DisplayRowDetails(oGrid);
            }
        }
    }

    public virtual List<Control> AddControls()
    {
        List<Control> olstControls = new List<Control>();
        return olstControls;
    }   

    public virtual void FillPaymnetsGrid()
    {
    }

    public virtual void ShowPaymnetsGrid()
    {
    }    
}

