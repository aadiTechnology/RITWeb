<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EmployeeBasicDetails.ascx.cs"
    Inherits="EmployeeBasicDetailsUC" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<%@ Register Assembly="EventCalendar" Namespace="ExtendedControls" TagPrefix="Calender" %>
<%@ Register Assembly="eWorld.UI.Compatibility, Version=2.0.6.2393, Culture=neutral, PublicKeyToken=24d65337282035f2"
    Namespace="eWorld.UI.Compatibility" TagPrefix="cc1" %>
<cc1:CollapsablePanel ID="colpnlAdditionalInfo" runat="server" TitleText="Additional Information"
    TitleStyle-CssClass="CollapsTitle" AllowSliding="true" ExpandImageUrl="~/RITeSchool/images/node_open.gif"
    CollapseImageUrl="~/RITeSchool/images/node_close.gif" CollapserAlign="Left" TitleStyle-Height="25px"
    Collapsed="True" SlideSpeed="25" CollapsedTitleStyle-CssClass="CollapsedTitle">
    <table style="width: 100%">
        <tr>
            <td align="left" colspan="4" class="ClsBtmBorderGray">
                <span class="ClsLblLgnd" style="width: 200px; font: Bold">
                    <asp:Label ID="Label90" runat="server" EnableViewState="False" Text="Other Details">
                    </asp:Label></span>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="4">
            </td>
        </tr>
        <tr>
            <td style="height: 5px;" colspan="4">
                <table width="100%">
                    <tr>
                        <td align="left">
                            <span class="ClsLabel">
                                <asp:Label ID="Label91" runat="server" EnableViewState="False" Text="Gender"></asp:Label>
                                <span class="colonPadding">:</span></span>
                        </td>
                        <td align="left">
                            <%--  <asp:TextBox ID="txtGender" CssClass="MidTxtBox" runat="server"  style="width:220px;"></asp:TextBox>--%>
                            <asp:RadioButton ID="rdoMale" Text="Male" runat="server" GroupName="rdoGroupSex"
                                CssClass="ClsLabel" Checked="True"></asp:RadioButton>
                            <asp:RadioButton ID="rdoFemale" Text="Female" runat="server" GroupName="rdoGroupSex"
                                CssClass="ClsLabel clsLabel"></asp:RadioButton>
                        </td>
                        <td align="left" style="width: 15%;">
                            <span class="ClsLabel">
                                <asp:Label ID="Label93" runat="server" EnableViewState="False" Text="Reference"></asp:Label>
                                <span class="colonPadding">:</span></span>
                        </td>
                        <td align="left" style="width: 23%;">
                            <asp:TextBox ID="txtReference" CssClass="MidTxtBox" runat="server" Style="width: 220px;"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 15%;">
                            <span class="ClsLabel">
                                <asp:Label ID="Label94" runat="server" EnableViewState="False" Text="Marital Status"></asp:Label>
                                <span class="colonPadding">:</span></span>
                        </td>
                        <td align="left" style="width: 23%;">
                            <%--    <asp:TextBox ID="txtMaritalStatus" CssClass="MidTxtBox" runat="server" style="width:220px;" ></asp:TextBox>--%>
                            <asp:RadioButton ID="rdomarried" Text="Married" runat="server" GroupName="rdomarried"
                                CssClass="ClsLabel" Checked="True"></asp:RadioButton>
                            <asp:RadioButton ID="rdounmarried" Text="Unmarried" runat="server" GroupName="rdomarried"
                                CssClass="ClsLabel clsLabel"></asp:RadioButton>
                        </td>
                        <td align="left" style="width: 15%;">
                            <span class="ClsLabel">
                                <asp:Label ID="Label96" runat="server" EnableViewState="False" Text="Salary Scale"></asp:Label>
                                <span class="colonPadding">:</span></span>
                        </td>
                        <td align="left" style="width: 23%;">
                            <asp:TextBox ID="txtSalaryScale" CssClass="MidTxtBox" runat="server" Style="width: 220px;"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                    </tr>
                    <tr>
                        <td align="left" style="width: 15%;">
                            <span class="ClsLabel">
                                <asp:Label ID="Label97" runat="server" EnableViewState="False" Text="Whatsapp No."></asp:Label>
                                <span class="colonPadding">:</span></span>
                        </td>
                        <td align="left" style="width: 23%;">
                            <asp:TextBox ID="txtWhatsappNo" CssClass="MidTxtBox" runat="server" Style="width: 220px;"
                                MaxLength="10" onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                ondrop="event.returnValue=false"></asp:TextBox>
                        </td>
                        <td align="left" style="width: 15%;">
                            <span class="ClsLabel">
                                <asp:Label ID="Label98" runat="server" EnableViewState="False" Text="GPF Account No."></asp:Label>
                                <span class="colonPadding">:</span></span>
                        </td>
                        <td align="left" style="width: 23%;">
                            <asp:TextBox ID="txtGPFACNo" CssClass="MidTxtBox" runat="server" Style="width: 220px;"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="left" colspan="4" class="ClsBtmBorderGray">
                <span class="ClsLblLgnd" style="width: 200px; font: Bold">
                    <asp:Label ID="Label82" runat="server" EnableViewState="False" Text="Contact Details">
                    </asp:Label></span>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="4">
            </td>
        </tr>
        <tr>
            <td style="height: 5px;" colspan="4">
                <table width="100%">
                    <tr>
                        <td align="left">
                            <span class="ClsLabel">
                                <asp:Label ID="Label83" runat="server" EnableViewState="False" Text="Primary Email"></asp:Label>
                                <span class="colonPadding">:</span></span>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtPrimaryEmail" CssClass="MidTxtBox" runat="server" Style="width: 220px;"></asp:TextBox>
                            <asp:CustomValidator ID="cstValEmail" runat="server" ControlToValidate="txtPrimaryEmail"
                                ValidationGroup="Save" ClientValidationFunction="EmailValidation1" Display="None"
                                ValidateEmptyText="True" Visible="true"></asp:CustomValidator>
                        </td>
                        <%-- <td style="width: 132px">
                                                    <span class="ClsLabel"><asp:Label ID="Label84" runat="server" EnableViewState="False" Text="Mobile Number"></asp:Label>
                                                         <span class="colonPadding"> :</span></span>
                                                    </td>
                                                     <td align="left" style="width: 13%;">
                                                     <asp:TextBox ID="txtMobileNo" CssClass="MidTxtBox" runat="server" 
                                                       MaxLength="10"
                                                        onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                         onkeypress="return blockNonNumbers (this, event, false, false);"
                                                          onpaste="event.returnValue=false" ondrop="event.returnValue=false"         />
                                                                </td>--%>
                        <td align="left" style="width: 15%;">
                            <span class="ClsLabel">
                                <asp:Label ID="Label88" runat="server" EnableViewState="False" Text="Company Cont. No."></asp:Label>
                                <span class="colonPadding">:</span></span>
                        </td>
                        <td align="left" style="width: 23%;">
                            <asp:TextBox ID="txtCompanyContNo" CssClass="MidTxtBox" runat="server" Style="width: 220px;"
                                onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                ondrop="event.returnValue=false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 15%;">
                            <span class="ClsLabel">
                                <asp:Label ID="Label85" runat="server" EnableViewState="False" Text="Company Email"></asp:Label>
                                <span class="colonPadding">:</span></span>
                        </td>
                        <td align="left" style="width: 23%;">
                            <asp:TextBox ID="txtCompanyEmail" CssClass="MidTxtBox" runat="server" Style="width: 220px;"></asp:TextBox>
                            <asp:CustomValidator ID="cstCmpnyEmail" runat="server" ControlToValidate="txtCompanyEmail"
                                ValidationGroup="Save" ClientValidationFunction="CompanyEmailValidation" Display="None"
                                ValidateEmptyText="True"></asp:CustomValidator>
                        </td>
                        <td align="left" style="width: 15%;">
                            <span class="ClsLabel">
                                <asp:Label ID="Label86" runat="server" EnableViewState="False" Text="Permanent Cont. No."></asp:Label>
                                <span class="colonPadding">:</span></span>
                        </td>
                        <td align="left" style="width: 23%;">
                            <asp:TextBox ID="txtPermanentContNo" CssClass="MidTxtBox" runat="server" Style="width: 220px;"
                                onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                ondrop="event.returnValue=false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 15%;">
                            <span class="ClsLabel">
                                <asp:Label ID="Label87" runat="server" EnableViewState="False" Text="Extension No."></asp:Label>
                                <span class="colonPadding">:</span></span>
                        </td>
                        <td align="left" style="width: 23%;">
                            <asp:TextBox ID="txtExtensionNo" CssClass="MidTxtBox" runat="server" Style="width: 220px;"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="left" colspan="4" class="ClsBtmBorderGray">
                <span class="ClsLblLgnd" style="width: 200px; font: Bold">
                    <asp:Label ID="Label51" runat="server" EnableViewState="False" Text="Detail Of Family Members">
                    </asp:Label></span>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="4">
            </td>
        </tr>
        <tr>
            <td style="height: 5px;" colspan="4">
                <table width="100%">
                    <tr>
                        <td align="left">
                            <span class="ClsLabel">
                                <asp:Label ID="Label52" runat="server" EnableViewState="False" Text="Name"></asp:Label>
                                <span class="colonPadding">:</span></span>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtName" CssClass="MidTxtBox" runat="server" Style="width: 370px;"></asp:TextBox>
                        </td>
                        <td style="width: 132px">
                            <span class="ClsLabel">
                                <asp:Label ID="Label53" runat="server" EnableViewState="False" Text="Age"></asp:Label>
                                <span class="colonPadding">:</span></span>
                        </td>
                        <td align="left" style="width: 13%;">
                            <asp:TextBox ID="txtAge" CssClass="MidTxtBox" runat="server" MaxLength="100" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 15%;">
                            <span class="ClsLabel">
                                <asp:Label ID="Label54" runat="server" EnableViewState="False" Text="Relation"></asp:Label>
                                <span class="colonPadding">:</span></span>
                        </td>
                        <td align="left" style="width: 23%;">
                            <asp:TextBox ID="txtRelation" CssClass="MidTxtBox" runat="server" Style="width: 220px;"></asp:TextBox>
                        </td>
                        <td align="left" style="width: 15%;">
                            <span class="ClsLabel">
                                <asp:Label ID="Label55" runat="server" EnableViewState="False" Text="Occupation"></asp:Label>
                                <span class="colonPadding">:</span></span>
                        </td>
                        <td align="left" style="width: 23%;">
                            <asp:TextBox ID="txtOccupation" CssClass="MidTxtBox" runat="server" Style="width: 220px;"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="4">
            </td>
        </tr>
        <%--  <tr>
                                                <td align="left" colspan="4" class="ClsBtmBorderGray">                                                   
                                                        <span class="ClsLblLgnd" style="width:200px;font:Bold" >
                                                        <asp:Label ID="Label56" runat="server" EnableViewState="False" Text="Previous Employment">
                                                    </asp:Label></span>
                                                </td>
                                            </tr>
                                              <tr>
                                                <td align="center" colspan="4">
                                                </td>
                                            </tr>
                                            <tr>
                                            <td style="height: 5px;" colspan="4">
                                            <table width ="100%">
                                            <tr>
                                          
                                                    <td style="width: 132px">
                                                    <span class="ClsLabel"><asp:Label ID="Label58" runat="server" EnableViewState="False" Text="Designation"></asp:Label>
                                                         <span class="colonPadding"> :</span></span>
                                                    </td>
                                                     <td align="left" style="width: 13%;">
                                                     <asp:TextBox ID="txtDesignation" CssClass="MidTxtBox" runat="server" MaxLength="100" style="width:220px;"
                                                                />
                                                                </td>
                                                                 <td align="left" style="width: 15%;">
                                                        <span class="ClsLabel"><asp:Label ID="Label59" runat="server" EnableViewState="False" Text="Last Salary"></asp:Label>
                                                         <span class="colonPadding"> :</span></span>
                                                         </td>
                                                         <td align="left" style="width: 23%;">
                                                            <asp:TextBox ID="txtLastSalary" CssClass="MidTxtBox" runat="server" style="width:220px;" ></asp:TextBox>
                                                              
                                                         </td>
                                            </tr>
                                            <tr>
                                             
                                                               
                                                  <td align="left" style="width: 15%;">
                                                    <span class="ClsLabel"><asp:Label ID="Label60" runat="server" EnableViewState="False" Text="Duration"></asp:Label>
                                                         <span class="colonPadding"> :</span></span>
                                                  </td>
                                                  <td align="left" style="width: 23%;">
                                                            <asp:TextBox ID="txtDuration" CssClass="MidTxtBox" runat="server" style="width:220px;" ></asp:TextBox>
                                                              
                                                         </td>
                                                           <td align="left" style="width: 15%;">
                                                    <span class="ClsLabel"><asp:Label ID="Label61" runat="server" EnableViewState="False" Text="Job Description"></asp:Label>
                                                         <span class="colonPadding"> :</span></span>
                                                  </td>
                                                  <td align="left" style="width: 23%;">
                                                            <asp:TextBox ID="txtJobDescription" CssClass="MidTxtBox" runat="server" style="width:220px;" ></asp:TextBox>
                                                              
                                                         </td>
                                                         </tr>
                                                         <tr>
                                                        
                                                          <td align="left" style="width: 15%;">
                                                            <span class="ClsLabel"><asp:Label ID="Label62" runat="server" EnableViewState="False" Text="Reason For Leaving"></asp:Label>
                                                         <span class="colonPadding"> :</span></span>
                                                          </td>
                                                           <td align="left" style="width: 23%;">
                                                            <asp:TextBox ID="txtReasonForLeaving" CssClass="MidTxtBox" runat="server" style="width:220px;" ></asp:TextBox>
                                                           </td>

                                            </tr>
                                            
                                            </table>
                                            </td>
                                            </tr>--%>
        <tr>
            <td align="center" colspan="4">
            </td>
        </tr>
        <tr>
            <td align="left" colspan="4" class="ClsBtmBorderGray">
                <span class="ClsLblLgnd" style="width: 200px; font: Bold">
                    <asp:Label ID="Label63" runat="server" EnableViewState="False" Text="Statutory Details">
                    </asp:Label></span>
            </td>
        </tr>
        <tr>
            <td style="height: 5px;" colspan="4">
                <table width="100%">
                    <tr>
                        <td align="left">
                            <span class="ClsLabel">
                                <asp:Label ID="Label64" runat="server" EnableViewState="False" Text="EPF Number"></asp:Label>
                                <span class="colonPadding">:</span></span>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtEPFNumber" CssClass="MidTxtBox" runat="server" Style="width: 220px;"></asp:TextBox>
                        </td>
                        <td style="width: 132px">
                            <span class="ClsLabel">
                                <asp:Label ID="Label65" runat="server" EnableViewState="False" Text="Is VPF Deduction"></asp:Label>
                                <span class="colonPadding">:</span></span>
                        </td>
                        <td align="left" style="width: 13%;">
                            <%-- <asp:TextBox ID="txtIsVPFDeduction" CssClass="MidTxtBox" runat="server" MaxLength="100" style="width:220px;"
                                                                />--%>
                            <asp:RadioButton ID="rdoVPFDeduction1" Text="True" runat="server" GroupName="rdoGroupDeduction"
                                CssClass="ClsLabel" Checked="True"></asp:RadioButton>
                            <asp:RadioButton ID="rdoVPFDeduction2" Text="False" runat="server" GroupName="rdoGroupDeduction"
                                CssClass="ClsLabel clsLabel"></asp:RadioButton>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 15%;">
                            <span class="ClsLabel">
                                <asp:Label ID="Label66" runat="server" EnableViewState="False" Text="VPF Contribution ID"></asp:Label>
                                <span class="colonPadding">:</span></span>
                        </td>
                        <td align="left" style="width: 23%;">
                            <asp:TextBox ID="txtVPFContributionID" CssClass="MidTxtBox" runat="server" Style="width: 220px;"></asp:TextBox>
                        </td>
                        <td align="left" style="width: 15%;">
                            <span class="ClsLabel">
                                <asp:Label ID="Label67" runat="server" EnableViewState="False" Text="VPF Percentage"></asp:Label>
                                <span class="colonPadding">:</span></span>
                        </td>
                        <td align="left" style="width: 23%;">
                            <asp:TextBox ID="txtVPFPercentage" CssClass="MidTxtBox" runat="server" Style="width: 220px;"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 15%;">
                            <span class="ClsLabel">
                                <asp:Label ID="Label68" runat="server" EnableViewState="False" Text="VPF Contribution Effective Form "></asp:Label>
                                <span class="colonPadding">:</span></span>
                        </td>
                        <td align="left" style="width: 23%;">
                            <asp:TextBox ID="txtVPFContrEffectiveForm" CssClass="MidTxtBox" runat="server" Style="width: 190px;"></asp:TextBox>
                            <rjs:PopCalendar ID="PopCalendar3" runat="server" Culture="en-US" Control="txtVPFContrEffectiveForm"
                                Enabled="true" ValidationGroup="valGrpExpDetails" ShowErrorMessage="false" Format="dd MMM yyyy"
                                ShowWeekend="True" />
                        </td>
                        <td align="left" style="width: 15%;">
                            <span class="ClsLabel">
                                <asp:Label ID="Label69" runat="server" EnableViewState="False" Text="VPF Amount"></asp:Label>
                                <span class="colonPadding">:</span></span>
                        </td>
                        <td align="left" style="width: 23%;">
                            <asp:TextBox ID="txtVPFAmount" CssClass="MidTxtBox" runat="server" Style="width: 220px;"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 15%;">
                            <span class="ClsLabel">
                                <asp:Label ID="Label70" runat="server" EnableViewState="False" Text="Bank Name "></asp:Label>
                                <span class="colonPadding">:</span></span>
                        </td>
                        <td align="left" style="width: 23%;">
                            <%--  <asp:TextBox ID="txtBankName" CssClass="MidTxtBox" runat="server" style="width:220px;" ></asp:TextBox>--%>
                            <asp:DropDownList ID="cmbBank" runat="server" CssClass="LrgCombo">
                            </asp:DropDownList>
                        </td>
                        <td align="left" style="width: 15%;">
                            <span class="ClsLabel">
                                <asp:Label ID="Label71" runat="server" EnableViewState="False" Text="Branch"></asp:Label>
                                <span class="colonPadding">:</span></span>
                        </td>
                        <td align="left" style="width: 23%;">
                            <asp:TextBox ID="txtBranch" CssClass="MidTxtBox" runat="server" Style="width: 220px;"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 15%;">
                            <span class="ClsLabel">
                                <asp:Label ID="Label72" runat="server" EnableViewState="False" Text="Account Number"></asp:Label>
                                <span class="colonPadding">:</span></span>
                        </td>
                        <td align="left" style="width: 23%;">
                            <asp:TextBox ID="txtAccNumber" CssClass="MidTxtBox" runat="server" Style="width: 220px;"></asp:TextBox>
                        </td>
                        <td align="left" style="width: 15%;">
                            <span class="ClsLabel">
                                <asp:Label ID="Label73" runat="server" EnableViewState="False" Text="Increment Date"></asp:Label>
                                <span class="colonPadding">:</span></span>
                        </td>
                        <td align="left" style="width: 23%;">
                            <asp:TextBox ID="txtIncrementDate" CssClass="MidTxtBox" runat="server" Style="width: 190px;"></asp:TextBox>
                            <rjs:PopCalendar ID="PopCalendar1" runat="server" Culture="en-US" Control="txtIncrementDate"
                                Enabled="true" ValidationGroup="valGrpExpDetails" ShowErrorMessage="false" Format="dd MMM yyyy"
                                ShowWeekend="True" />
                        </td>
                    </tr>
                    <tr>
                        <%--<td align="left" style="width: 15%;">
                                                    <span class="ClsLabel"><asp:Label ID="Label74" runat="server" EnableViewState="False" Text="PAN Number"></asp:Label>
                                                         <span class="colonPadding"> :</span></span>
                                                  </td>
                                                  <td align="left" style="width: 23%;">
                                                            <asp:TextBox ID="txtPan" CssClass="MidTxtBox" runat="server" style="width:220px;" ></asp:TextBox>
                                                              
                                                         </td>--%>
                        <td align="left" style="width: 15%;">
                            <span class="ClsLabel">
                                <asp:Label ID="Label81" runat="server" EnableViewState="False" Text="EPF Join Date"></asp:Label>
                                <span class="colonPadding">:</span></span>
                        </td>
                        <td align="left" style="width: 23%;">
                            <asp:TextBox ID="txtEPFJoinDate" CssClass="MidTxtBox" runat="server" Style="width: 190px;"></asp:TextBox>
                            <rjs:PopCalendar ID="PopCalendar2" runat="server" Culture="en-US" Control="txtEPFJoinDate"
                                Enabled="true" ValidationGroup="valGrpExpDetails" ShowErrorMessage="false" Format="dd MMM yyyy"
                                ShowWeekend="True" />
                        </td>
                        <td align="left" style="width: 15%;">
                            <span class="ClsLabel">
                                <asp:Label ID="Label75" runat="server" EnableViewState="False" Text="UAN"></asp:Label>
                                <span class="colonPadding">:</span></span>
                        </td>
                        <td align="left" style="width: 23%;">
                            <asp:TextBox ID="txtUAN" CssClass="MidTxtBox" runat="server" Style="width: 220px;"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 15%;">
                            <span class="ClsLabel">
                                <asp:Label ID="Label76" runat="server" EnableViewState="False" Text="Income Tax Status ID"></asp:Label>
                                <span class="colonPadding">:</span></span>
                        </td>
                        <td align="left" style="width: 23%;">
                            <asp:TextBox ID="txtIncomeTaxStatusID" CssClass="MidTxtBox" runat="server" Style="width: 220px;"></asp:TextBox>
                        </td>
                        <td align="left" style="width: 15%;">
                            <span class="ClsLabel">
                                <asp:Label ID="Label77" runat="server" EnableViewState="False" Text="Payroll ID"></asp:Label>
                                <span class="colonPadding">:</span></span>
                        </td>
                        <td align="left" style="width: 23%;">
                            <asp:TextBox ID="txtPAyrollId" CssClass="MidTxtBox" runat="server" Style="width: 220px;"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 15%;">
                            <span class="ClsLabel">
                                <asp:Label ID="Label78" runat="server" EnableViewState="False" Text="Basic Pay"></asp:Label>
                                <span class="colonPadding">:</span></span>
                        </td>
                        <td align="left" style="width: 23%;">
                            <asp:TextBox ID="txtBasicPay" CssClass="MidTxtBox" runat="server" Style="width: 220px;"></asp:TextBox>
                        </td>
                        <td align="left" style="width: 15%;">
                            <span class="ClsLabel">
                                <asp:Label ID="Label79" runat="server" EnableViewState="False" Text="Payroll Group ID"></asp:Label>
                                <span class="colonPadding">:</span></span>
                        </td>
                        <td align="left" style="width: 23%;">
                            <asp:TextBox ID="txtPayrollGroupId" CssClass="MidTxtBox" runat="server" Style="width: 220px;"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 15%;">
                            <span class="ClsLabel">
                                <asp:Label ID="Label80" runat="server" EnableViewState="False" Text="Pay Scale"></asp:Label>
                                <span class="colonPadding">:</span></span>
                        </td>
                        <td align="left" style="width: 23%;">
                            <asp:TextBox ID="txtPayScale" CssClass="MidTxtBox" runat="server" Style="width: 220px;"></asp:TextBox>
                        </td>
                        <%--  <td align="left" style="width: 15%;">
                                                            <span class="ClsLabel"><asp:Label ID="Label81" runat="server" EnableViewState="False" Text="EPF Join Date"></asp:Label>
                                                         <span class="colonPadding"> :</span></span>
                                                          </td>
                                                           <td align="left" style="width: 23%;">
                                                            <asp:TextBox ID="txtEPFJoinDate" CssClass="MidTxtBox" runat="server" style="width:190px;" ></asp:TextBox>
                                                               <rjs:PopCalendar ID="PopCalendar2" runat="server" Culture="en-US" Control="txtEPFJoinDate" 
                                                        Enabled="true" ValidationGroup="valGrpExpDetails" ShowErrorMessage="false" Format="dd MMM yyyy"
                                                        ShowWeekend="True"  />
                                                           </td>--%>
                    </tr>
                </table>
            </td>
        </tr>
        <asp:HiddenField ID="hidSchoolId" runat="Server" ViewStateMode="Enabled" Value="0" />
        <asp:HiddenField ID="hidOWSSchoolId" runat="Server" ViewStateMode="Enabled" Value="0" />
    </table>
</cc1:CollapsablePanel>
<script language="javascript" type="text/javascript">

    _clientcstValidateEmail = '<%=this.cstValEmail.ClientID %>';
    _clienttxtEmailId1 = '<%= this.txtPrimaryEmail.ClientID %>';
    _ClientcstCompanyEmail = '<%=this.cstCmpnyEmail.ClientID %>';
    _ClienttxtCompanyEmail = '<%=this.txtCompanyEmail.ClientID %>';
    _clienthidSchoolId = "<%=this.hidSchoolId.ClientID %>";
    _clienthidOWSSchoolId = "<%=this.hidOWSSchoolId.ClientID %>";
    function EmailValidation1(oSrc, args) {

        var sEmail = document.getElementById(_clienttxtEmailId1).value
        var sSchoolId = document.getElementById(_clienthidSchoolId).value
        var sOWSSchoolID = document.getElementById(_clienthidOWSSchoolId).value
        sEmail = stripLeadingTrailingBlanks(sEmail)
        if (sSchoolId = sOWSSchoolID) {
            if (isEmpty(sEmail)) {
                document.getElementById(_clientcstValidateEmail).errormessage = "Email Address should not be blank."
                args.IsValid = false
                return true
            }
            else {
                if (!isEmail(sEmail)) {
                    document.getElementById(_clientcstValidateEmail).errormessage = "Email Address should be in valid format(For Example :\" john.smith@yahoo.com \")."
                    args.IsValid = false
                    return true
                }
            }
        }
        else {
            if (!isEmpty(sEmail)) {
                if (!isEmail(sEmail)) {
                    document.getElementById(_clientcstValidateEmail).errormessage = "Email Address should be in valid format(For Example :\" john.smith@yahoo.com \")."
                    args.IsValid = false
                    return true
                }
            }
        }
        args.IsValid = true
        return false
    }

    function CompanyEmailValidation(oSrc, args) {
        var sEmail = document.getElementById(_ClienttxtCompanyEmail).value
        var sSchoolId = document.getElementById(_clienthidSchoolId).value
        var sOWSSchoolID = document.getElementById(_clienthidOWSSchoolId).value
        sEmail = stripLeadingTrailingBlanks(sEmail)
        if (sSchoolId = sOWSSchoolID) {
            if (isEmpty(sEmail)) {
                document.getElementById(_ClientcstCompanyEmail).errormessage = "Email Address should not be blank."
                args.IsValid = false
                return true
            }
            else {
                if (!isEmail(sEmail)) {
                    document.getElementById(_ClientcstCompanyEmail).errormessage = "Email Address should be in valid format(For Example :\" john.smith@yahoo.com \")."
                    args.IsValid = false
                    return true
                }
            }
        }
        else {
            if (!isEmpty(sEmail)) {
                if (!isEmail(sEmail)) {
                    document.getElementById(_ClientcstCompanyEmail).errormessage = "Email Address should be in valid format(For Example :\" john.smith@yahoo.com \")."
                    args.IsValid = false
                    return true
                }
            }
        }
        args.IsValid = true
        return false
    }

</script>
