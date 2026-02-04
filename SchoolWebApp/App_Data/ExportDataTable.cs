using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for ExportDataTable
/// </summary>
public class ExportDataTable : SchoolBase
{
    protected void ExportToExcel(string strFileName, DataTable oDatatable)
    {
        DataGrid dg = new DataGrid();
        dg.DataSource = oDatatable;
        dg.DataBind();
        Response.ClearContent();
        Response.AddHeader("content-disposition", "attachment; filename=" + strFileName);
        Response.ContentType = "application/excel";
        System.IO.StringWriter sw = new System.IO.StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        dg.RenderControl(htw);
        Response.Write(sw.ToString());
        Response.End();
        dg = null;
        dg.Dispose();
    }
}
