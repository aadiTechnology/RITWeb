using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace ASP
{
    [ParseChildren(true)]
    [PersistChildren(false)]
    [ToolboxData("<{0}:Gallery runat=server></{0}:Gallery>")]
    public partial class ComboRpt : System.Web.UI.UserControl
    {
        private int iReportFieldId;

        public delegate void ComboSelectedIndexChangeEventHandler(object sender, EventArgs e);
        public event ComboSelectedIndexChangeEventHandler ComboChangeEvent;

        public int ReportFieldId
        {
            get { return iReportFieldId; }
            set { iReportFieldId = value; }
        }

        public string ComboText
        {
            get { return this.cmbUC.SelectedItem.Text; }
            set { this.cmbUC.Text = value; }
        }

        public bool IsRequired
        {
            get { return this.RFVDDLParamReport.Visible; }
            set { this.RFVDDLParamReport.Visible = value; }

        }

        public bool IsRequiredLabel
        {
            get { return this.lblDDLMandatory.Visible; }
            set { this.lblDDLMandatory.Visible = value; }

        }

        public string ErrorMessage
        {
            get { return this.RFVDDLParamReport.ErrorMessage; }
            set { this.RFVDDLParamReport.ErrorMessage = value; }

        }

        public object DataSource
        {
            get { return this.cmbUC.DataSource; }
            set { this.cmbUC.DataSource = value; }
        }
        public string DataTextField
        {
            get { return this.cmbUC.DataTextField; }
            set { this.cmbUC.DataTextField = value; }
        }

        public string DataValueField
        {
            get { return this.cmbUC.DataValueField; }
            set { this.cmbUC.DataValueField = value; }
        }

        public ListItemCollection Items
        {
            get { return this.cmbUC.Items; }
        }

        public bool AutoPostBack
        {
            get { return this.cmbUC.AutoPostBack; }
            set { this.cmbUC.AutoPostBack = value; }
        }

        public bool Enabled
        {
            get { return this.cmbUC.Enabled; }
            set { this.cmbUC.Enabled = value; }
        }

        public ListItem SelectedItem
        {
            get { return this.cmbUC.SelectedItem; }
        }

        public string SelectedValue
        {
            get { return this.cmbUC.SelectedValue; }
            set { this.cmbUC.SelectedValue = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.cmbUC.SelectedIndexChanged += new EventHandler(cmbUC_SelectedIndexChanged);
        }

        public void cmbUC_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Raise the ok button click event.
            if (this.ComboChangeEvent != null)
            {
                EventArgs ev = new EventArgs();
                this.ComboChangeEvent(this, ev);
            }
        }

        public override void DataBind()
        {
            this.cmbUC.DataBind();

        }

    }
}

