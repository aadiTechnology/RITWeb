<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImageGallery.aspx.cs" MasterPageFile="~/RITeSchool/MasterPages/BlankMaster.master"
    Inherits="ImageGallery" %>

<asp:Content ID="headContent" runat="server" ContentPlaceHolderID="headContentPlaceHolder">
    <script src="../Scripts/jquery-1.7.min.js" type="text/javascript"></script>
    <link href="../Styles/cycle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #main
        {
            text-align: center;
            width: 700px;
            margin: 0 auto;
        }
        #slideshowHolder, #nav
        {
            margin: 20px auto;
        }
        #nav a
        {
            padding: 4px 6px;
            margin: 3px;
            border: 1px solid #ccc;
            text-align: center;
            text-decoration: none;
            background-color: #ddd;
            width: 20px;
            float: left;
        }
        #nav a.activeSlide
        {
            color: #c00;
        }
        #nav a:focus
        {
            outline: none;
        }
        #comment
        {
            clear: both;
            margin: 0;
            padding-top: 1.7em;
        }
    </style>
    <script src="../Scripts/jquery.cycle.all.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(function () {
       
            $('#comment').html($('#<%=slideshowHolder.ClientID%> img:first').attr("alt"));
            var slidetimeout = 500;
            if (document.getElementById("<%=this.optSlow.ClientID %>") != null && document.getElementById("<%=this.optSlow.ClientID %>").checked == true)
                slidetimeout = 7000;
            if (document.getElementById("<%=this.optMediam.ClientID %>") != null && document.getElementById("<%=this.optMediam.ClientID %>").checked == true)
                slidetimeout = 3000;
            if (document.getElementById("<%=this.optFast.ClientID %>") != null && document.getElementById("<%=this.optFast.ClientID %>").checked == true)
                slidetimeout = 1200;

            $('.pics').cycle({
                before: function () {
                    $('#comment').html(this.alt);
                },
                fx: 'all',
                timeout: slidetimeout,
                pager: '#nav',
                autoPlay: true
            });
        });
    </script>
</asp:Content>
<asp:Content ID="bodyContent" runat="server" ContentPlaceHolderID="bodyContentPlaceHolder">
    <div id="Divradio" runat="server" style="height: 40px;">
    <div style="height: 5px;"></div>
        <table     style="width:420px; height:25px; border-spacing:0">
            <tr style="padding:10px 0px 10px 15px;"  >
                <td   style="width: 410px;background-color:#c0c0c0;padding:10px 0px 10px 15px;">
                    <asp:Label ID="Label2" runat="server" Text="Slide Show Speed :" Font-Size="Larger" ></asp:Label>
                </td>
                          <td style="background-color:#c0c0c0;padding:10px 0px 10px 0px;" >
                                <asp:RadioButton ID="optSlow" runat="server" CssClass="ClsLabel" Font-Bold="False"
                                    GroupName="UserType" Text="Slow" AutoPostBack="true" />
                         </td>
                           <td style="background-color:#c0c0c0;padding:10px 0px 10px 0px;">
                                <asp:RadioButton ID="optMediam" runat="server" CssClass="ClsLabel" Font-Bold="False"
                                    GroupName="UserType" Text="Medium" AutoPostBack="true" />
                            
                          </td>
                           <td style="background-color:#c0c0c0;padding: 10px 0px 10px 0px;">
                                <asp:RadioButton ID="optFast" runat="server" CssClass="ClsLabel" Font-Bold="False"
                                    GroupName="UserType" Text="Fast" AutoPostBack="true" />
                            </td>
           
        </tr> </table>
    </div>
    <div id="main">
        <h3>
            <asp:Label ID="lblGalname" runat="server" />
        </h3>
        <div id='slideshowHolder' runat="server" class="pics">
        </div>        
        <p id="comment" style="height: 20px; width: 682px; padding:0; margin-top:10px; overflow: visible; text-align:left">
        </p>
        <div id="nav">
        </div>
    </div>
</asp:Content>
