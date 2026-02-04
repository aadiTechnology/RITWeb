<%@ Page Language="C#" MasterPageFile="~/PPSMaster.master" AutoEventWireup="true"
    CodeFile="eSchoolLogin.aspx.cs" Inherits="eSchoolLogin" %>
<%@ Register Src="~/UserControls/NoticeDivUC.ascx" TagName="ucNoticeDivUC" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="js/jquery.easing.1.3.js"></script>
	<script type="text/javascript" src="js/jquery.shuffleLetters.js"></script>
    <script type="text/javascript">

        var events;
        var eventColors = ['royalblue', 'coral', 'darkgreen'];
        var eventInterval = 6000;

        function processEvents() {
            if (!events)
                events = eval($('#<%= this.hidEventData.ClientID %>').get(0).value);

            if (!events || events.length <= 0)
                return;

            var event = events[0];
            processEvent(event, 0, 0);
        }

        function processEvent(event, index, colorIndex) {
            $('#upcomingEvents #eventDate').text('');
            $('#upcomingEvents #eventDescription').text('');

            if (!event || index + 1 > events.length)
                return;

            var nextEvent, nextIndex;


            // When there is just one event to be displayed.
            if (events.length == 1) {
                nextEvent = event;
                nextIndex = index;
            }
            // When the current index is the last event in the queue.
            else if (index + 1 == events.length) {
                nextEvent = events[0];
                nextIndex = 0;
            }
            // When the current index is not the first or last, ie it is inbetween.
            else {
                nextEvent = events[index + 1];
                nextIndex = index + 1;
            }

            if (colorIndex > 2)
                colorIndex = 0;

            $('#upcomingEvents .body').css('color', eventColors[colorIndex]);

            $('#upcomingEvents #eventDate')
				.shuffleLetters({
				    text: event.date,
				    step: 1,
				    fps: event.date.length * 1.5,
				    callback: function () {
				        $('#upcomingEvents #eventDescription')
							.shuffleLetters({
							    text: event.name,
							    step: 1,
							    fps: event.name.length,
							    callback: function () {
							        window.setTimeout(function () {
							            processEvent(nextEvent, nextIndex, colorIndex + 1);
							        }, eventInterval);
							    }
							});
				    }
				});


        }
        $(document).ready(function () {
            if ($('#<%= this.defaultUpcomingEvents.ClientID %>').length == 0) {
                $('#upcomingEvents').show();
                window.setTimeout(processEvents, 750);
            }
        });
	</script>
	<style type="text/css" >		

		.box {
			width: 280px;
			height: 184px;
			background: transparent url(images/Box_bg.png) no-repeat;
			text-align: center;
			font-family: Verdana;
		}

		.box .content {
			padding: 15px 15px 20px;
			width: 250px;
		}

		.box .header {
			height: 32px;
			text-decoration: underline;
			background: transparent url(images/calendar.png) no-repeat right center;
			font-weight: bold;
			font-size: 13pt;
			color: #3399FF;
		}

		.box .body {
			height: 115px;
			width: 250px;
			font-weight: bold;
			font-size: 13pt;
		}
	</style>
    <table border="0" cellpadding="0" cellspacing="1" style="width: 95%" align="center"
        class="bordergray">
        <tr id="Tr2" runat="Server" align="center">
            <td class="bordergray tdFeatures" colspan="3">
                <asp:Label runat="server" Style="cursor: progress; border-top-width: thin; vertical-align: bottom;
                    border-top-color: blue;">
                    <a id="features" runat="server" href="#" class="navUG">Click here to view features available for Parents in RITeSchool
                    </a></asp:Label>
                    <div style="float:right; width:auto" align="right">
                    <label style="padding-right:10px; font-size:12px; font-family:Verdana;" class="TextHeadB">Join Us:</label> 
                    <span style="padding-right:10px; float:right; width:130px"><script type="IN/Share" data-counter="right" data-url="http://www.riteschool.com"></script></span>
                    <div style="padding-right:0px; float:right; width:60px"><a href="https://twitter.com/share" class="twitter-share-button" target="_blank" data-url="http://www.riteschool.com" data-via="RegulusIT" rel="me">Tweet</a> </div>
                    <div style="padding-right:0px; float:right; width:70px"><div class="g-plusone" data-size="medium" data-href="http://www.riteschool.com"></div></div>
                    <span style="padding-right:10px; float:right; width:70px"><fb:like href="http://www.facebook.com/pages/RITeSchool/264274746954829" layout="button_count" show_faces="false" action="like" colorscheme="light"></fb:like></span>
                    </div>
            </td>
        </tr>
        <tr id="trLogin" runat="Server">
            <td visible ="false" id="defaultupcomingevent" align="center" class="bordergray GreenBGPatch" runat="server">
                <div id="defaultUpcomingEvents" runat="server">
				</div>
			    <div id="upcomingEvents" style ="width:280px; height:200px; display: none;" class="box" >
					<div class="content" >
						<table cellpadding="0" cellspacing="0" border="0">
							<tr>
								<td class="header" valign="middle">
									Upcoming Event(s)
								</td>
							</tr>
							<tr>
								<td class="body" valign="middle">
									<div id="eventDate"></div>
									<div id="eventDescription"></div>
								</td>
							</tr>
						</table>
					</div>
				</div>      
            </td>
            <td align="Right" valign="middle" width="70%" class="bordergray HomePGTopImg">
                <div style="float: left">
                    <a href="http://www.riteschool.com" target="_blank">
                        <img alt="www.RITeSchool.com" src="images/spacer.gif" border="0" style="height: 200px;
                            width: 260px" /></a>
                </div>
            </td>
             <td align="center" style =" width:50%;" class="bordergray GreenBGPatch">
                <div runat="Server" id="Div1">
                    <asp:Login ID="Login1" runat="server" OnAuthenticate="Login1_Authenticate" RememberMeSet="True">
                        <TitleTextStyle CssClass="LblUsrNameHead" />
                        <LayoutTemplate>
                            <table cellpadding="0" width="100%" border="0">
                                <tbody>
                                    <tr>
                                        <td style="padding-right: 5px" align="left">
                                            <asp:Label ID="UserNameLabel" runat="server" CssClass="TxtBSml" Width="120px" AssociatedControlID="UserName"
                                                EnableViewState="False">User Name:</asp:Label><span style="color: red"> </span>
                                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ValidationGroup="Login1"
                                                CssClass="ErrMsg" ToolTip="User Name should not be blank." SetFocusOnError="true"
                                                ErrorMessage="User Name should not be blank." ControlToValidate="UserName" Display="None"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:TextBox ID="UserName" runat="server" CssClass="TxtBoxLogin"></asp:TextBox>
                                            <span style="color: #ff0000">*</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-right: 5px" align="left">
                                            <asp:Label ID="PasswordLabel" runat="server" CssClass="TxtBSml" AssociatedControlID="Password"
                                                EnableViewState="False">Password:</asp:Label>
                                            <span style="color: #ff0000"></span>
                                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ValidationGroup="Login1"
                                                CssClass="ErrMsg" ToolTip="Password is required." SetFocusOnError="true" ErrorMessage="Password should not be blank."
                                                ControlToValidate="Password" Display="None"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-right: 5px" align="left">
                                            <asp:TextBox ID="Password" runat="server" CssClass="TxtBoxLogin" TextMode="Password"></asp:TextBox>
                                            <span style="color: #ff0000">*</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ErrMsg" align="center">
                                            <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Button ID="LoginButton" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                                CommandName="Login" CssClass="ClsButton" Text="Log In" ValidationGroup="Login1" />
                                            <asp:LinkButton ID="hlinkForgotPassword" runat="server" CssClass="navForgotPwd" OnClientClick="if(!OpenForgotPassword()) return false;"
                                                Target="_blank">Forgot Password?</asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="bottom">
                                            <table class="borderblue" cellpadding="0" border="0">
                                                <tbody>
                                                    <tr>
                                                        <td class="Rlink" align="left">
                                                            Powered by:
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding-left: 5px" align="center">
                                                            <a href="http://www.regulusit.net" target="_blank" border="0">
                                                                <asp:Image ID="Image1" runat="server" ImageUrl="~/images/RIT_LogoAnimated.gif" AlternateText="http://www.regulusit.net">
                                                                </asp:Image>
                                                            </a>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </LayoutTemplate>
                    </asp:Login>
                </div>
            </td>
        </tr>        
    </table>

    <table>
		<tr>
			<td>
				<asp:Panel ID="pnlListFilters" runat="server">
					<uc1:ucNoticeDivUC ID="NoticeDivUC" runat="server" DisplayLocation="H" />
				</asp:Panel>
			</td>
		</tr>
	</table>

    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ErrMsg" ShowMessageBox="true"
        ShowSummary="false" ValidationGroup="Login1" />
    <asp:HiddenField ID="hidScreenWidth" runat="server" />
    <asp:HiddenField runat="server" ID="hidRedirect" />
    <asp:HiddenField ID="hidEventData" runat="server" />     
    <div style="height: 50px; text-align: center">
    </div>
    <script src="js/MobileRedirection.js" type="text/javascript"></script>
    <script language="javascript">
        _hidScreenWidth = "<%=this.hidScreenWidth.ClientID%>";
        //This function is used to open user guid popup.
        function OpenWindow(url) {
            window.open(url + 'User_Guide.aspx', '_blank', 'scrollbars=yes,statusbar=no,resizable=no,top=5,left=30,width=850,height=680');            
            return false;
        }
        //This function is used to open forget password popup.
        function OpenForgotPassword() {
            var _sClientUserName = "<%=this.Login1.UserName %>";
            window.open("ForgotPassword.aspx", '_new', 'fullscreen=no,scrollbars=yes,resizable=no,top=200,left=200,width=540,height=430');
            return false;
        }
        //This function is used to open feedback form.
        function OpenFeedback() {
            window.open('ParentsFeedback.aspx', '_blank', 'scrollbars=yes,statusbar=no,resizable=no,top=5,left=30,width=850,height=680');
            return false;
        }
        function SetWidth() {
            if (document.getElementById(_hidScreenWidth) != null) {
                var hidScreenWidth = document.getElementById(_hidScreenWidth)
                hidScreenWidth.value = "" + window.screen.width;
            }
        }
        SetWidth();

    </script>

</asp:Content>
