using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator;
using ControlEntities;
using Utility;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace BusinessLogic
{
    public class AdditionalFieldsBL
    {
        public int FillAdditionalFields(HtmlTable atblAdditionalInformation,int aiScreenId)
        {
            int iCellCount = 0;
            HtmlTableRow oHtmlTableRow = new HtmlTableRow();
            AdditionalFieldsDC oAdditionalFieldsDC = new AdditionalFieldsDC();
            List<AdditionalFields> lstAdditionalFields = oAdditionalFieldsDC.GetAdditionalFields(aiScreenId);
            lstAdditionalFields.ForEach
            (
                AdditionalFields =>
                {
                    if (!string.IsNullOrEmpty(AdditionalFields.DisplayText))
                    {
                        HtmlTableCell oHtmlTableCell = new HtmlTableCell();
                        oHtmlTableCell.Width = Unit.Percentage(Convert.ToDouble(25)).ToString();
                        oHtmlTableCell.Attributes.Add("Class", "ClsBorderlight");
                        Label olbl = new Label() { CssClass = "ClsLabel" };
                        olbl.Text = AdditionalFields.DisplayText;
                        oHtmlTableCell.Controls.Add(olbl);
                        oHtmlTableRow.Controls.Add(oHtmlTableCell);
                        iCellCount++;
                    }
                    if (AdditionalFields.Control == "TextBox")
                    {
                        HtmlTableCell oHtmlTableCell = new HtmlTableCell();
                        oHtmlTableCell.Width = Unit.Percentage(Convert.ToDouble(25)).ToString();
                        oHtmlTableCell.Attributes.Add("Class", "ClsBorderlight");
                        TextBox oTextBox = new TextBox() { CssClass = "MidTxtBox", ID = AdditionalFields.AdditionalFieldId };
                        oTextBox.Attributes.Add("onblur","extractNumber(this,2,false);");
                        oTextBox.Attributes.Add("onkeyup","extractNumber(this,2,false);");
                        oTextBox.Attributes.Add("onkeypress", "return blockNonNumbersAndDecimalOnFirstPlace (this, event, true, false);");
                        oTextBox.Attributes.Add("onpaste","event.returnValue=false");
                        oTextBox.Attributes.Add("ondrop","event.returnValue=false");
                        oTextBox.MaxLength = AdditionalFields.MaxLength;
                        oHtmlTableCell.Controls.Add(oTextBox);
                        oHtmlTableRow.Controls.Add(oHtmlTableCell);
                        iCellCount++;
                    }
                    if (iCellCount == Constants.I_FOUR)
                    {
                        iCellCount = 0;
                        atblAdditionalInformation.Controls.Add(oHtmlTableRow);
                        oHtmlTableRow = new HtmlTableRow();
                    }
                }
            );
            return lstAdditionalFields.Count;
        }
    }
}