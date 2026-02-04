using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SchoolEntities.Inventory;
using BusinessLogic;

public partial class RITeSchool_Inventory_ItemImageUI : SchoolBase
{
    private const string S_FOLDER_PATH = @"../DOWNLOADS/Inventory Items/";

    protected void Page_Load(object sender, EventArgs e)
    {
        int iItemId = Convert.ToInt32(Request.QueryString["ItemId"]);
        ItemsMasterBL oItemsMasterBL = new ItemsMasterBL();
        List<ItemImageDetails> lstItemImage = oItemsMasterBL.GetImagesUrl(iItemId);
        if (lstItemImage != null && lstItemImage.Count > 0)
        {
            var result = (from Item in lstItemImage where Item.ControlId == 1 select Item).FirstOrDefault();
            if (result != null)
            {
                imgItem1.Visible = true;                
                string sNewFileName = S_FOLDER_PATH + result.ImageUrl;
                imgItem1.ImageUrl = sNewFileName;               
                imgItem1.Attributes.Add("onclick", "window.open('" + sNewFileName + "', '_new', 'scrollbars=yes,resizable=no,menubar=no,status=no,titlebar=no,toolbar=no,top=20,left=100,width=1000,height=700'); return false;");
                
            }
            else
            {
                imgItem1.Visible = false;                
            }
            var result1 = (from Item in lstItemImage where Item.ControlId == 2 select Item).FirstOrDefault();
            if (result1 != null)
            {
                imgItem2.Visible = true;                
                string sNewFileName = S_FOLDER_PATH + result1.ImageUrl;
                imgItem2.ImageUrl = sNewFileName;
                imgItem2.Attributes.Add("onclick", "window.open('" + sNewFileName + "', '_new', 'scrollbars=yes,resizable=no,menubar=no,status=no,titlebar=no,toolbar=no,top=20,left=100,width=1000,height=700'); return false;");
            }
            else
            {
                imgItem2.Visible = false;                
            }

            var result2 = (from Item in lstItemImage where Item.ControlId == 3 select Item).FirstOrDefault();
            if (result2 != null)
            {
                imgItem3.Visible = true;                
                string sNewFileName = S_FOLDER_PATH + result2.ImageUrl;
                imgItem3.ImageUrl = sNewFileName;
                imgItem3.Attributes.Add("onclick", "window.open('" + sNewFileName + "', '_new', 'scrollbars=yes,resizable=no,menubar=no,status=no,titlebar=no,toolbar=no,top=20,left=100,width=1000,height=700'); return false;");
            }
            else
            {
                imgItem3.Visible = false;                
            }
        }
    }
}