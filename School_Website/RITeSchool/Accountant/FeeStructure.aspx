<%@ Page Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="FeeStructure.aspx.cs" Inherits="FeeStructure"
    Title="Fee Structure" EnableViewState="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">

    <script language="javascript" type="text/javascript">
        function openFeeStructure() {
            window.open('NewFeeStructure.aspx', '_new', 'scrollbars=yes,resizable=no,menubar=no,status=no,titlebar=no,toolbar=no,top=20,left=100,width=800,height=950');
        }
    </script>

    <table cellspacing="2" cellpadding="0" width="95%" border="0">
        <table class="paddingLR" style="width: 100%" cellspacing="2" cellpadding="0" border="0">
            <tbody>
                <tr>
                    <td align="left">
                        <a href="#New_Student" style="font-size: 12px; font-weight: bold">Fee Structure 2010
                            - 11 (Updated As On 20<sup>th</sup> April 2010) (New Students) </a>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <a href="#Old_Student" style="font-size: 12px; font-weight: bold">Fee Structure 2010
                            - 11 (Updated As On 20<sup>th</sup> April 2010) (Existing Students) </a>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="ClsLabelNrml padding10" valign="top">
                    </td>
                </tr>
                <tr>
                    <td align="left" class="ClsLabelNrml padding10" valign="top">
                    </td>
                </tr>
                <tr>
                    <td align="left" class="HeadTxtB borderBtm" colspan="1" style="height: 25px">
                        <a name="New_Student" class="Lbl10ptB" style="font-size: 14pt">Fee Structure 2010 -
                            11 <span style="color: #c45001">(Updated As On 20<sup>th</sup> April 2010) (New Students)
                            </span></a>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="ClsLabelNrml padding10">
                        <table cellpadding="3" cellspacing="1" class="ColorHeadBgFee" width="100%">
                            <tr>
                                <td align="center" class="TxtDivBG paddingL ClsLabelNrml" style="font-size: small;
                                    font-weight: bold; background-color: LightGrey; width: 22%">
                                    Fee Break-up
                                </td>
                                <td align="center" class="TxtDivBG paddingL ClsLabelNrml" style="font-size: small;
                                    font-weight: bold; background-color: LightGrey; width: 14%">
                                    Type
                                </td>
                                <td align="center" class="TxtDivBG paddingL ClsLabelNrml" style="font-size: small;
                                    font-weight: bold; background-color: LightGrey">
                                    Frequency
                                </td>
                                <td align="center" class="TxtDivBG paddingL ClsLabelNrml" style="font-size: small;
                                    font-weight: bold; background-color: LightGrey">
                                    Payable
                                </td>
                                <td align="center" class="TxtDivBG paddingL ClsLabelNrml" style="font-size: small;
                                    font-weight: bold; background-color: LightGrey">
                                    Amount<br />
                                    (in Rs.)
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    Application Form Fee
                                </td>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    Non-refundable
                                </td>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    One time
                                </td>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    At the time of purchase.
                                </td>
                                <td align="right" class="TxtDivBG paddingL ClsLabelNrml">
                                    200
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    Caution Money Deposit(Interest Free)
                                </td>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    Refundable
                                </td>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    One time
                                </td>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    At the time of admission. Will be refunded at the time of withdrawal from school.
                                </td>
                                <td align="right" class="TxtDivBG paddingL ClsLabelNrml">
                                    10,000
                                </td>
                            </tr>
                            <%--<tr>
                                                    <td align="left" class="TxtDivBG paddingL ClsLabelNrml" style="height: 5%" colspan="5">
                                                    </td>
                                                </tr>--%>
                            <tr>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml" colspan="3">
                                    Qtr 1: <b>At the Time of Admission</b>
                                </td>
                                <td align="right" class="TxtDivBG paddingL ClsLabelNrml">
                                    Total Annual Fees Rs.
                                </td>
                                <td align="right" class="TxtDivBG paddingL ClsLabelNrml" style="font-weight: bold">
                                    29,150
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    Tuition Fee
                                </td>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    Non-refundable
                                </td>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    <b>Quarterly</b>
                                </td>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    At the time of admission & subsequently by the 10<sup>th</sup> of the first month
                                    of every quarter.
                                </td>
                                <td align="right" class="TxtDivBG paddingL ClsLabelNrml">
                                    6,000
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    Term Fee
                                </td>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    Non-refundable
                                </td>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    Twice Annually
                                </td>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    At the time of admission / in May and in December every year.
                                </td>
                                <td align="right" class="TxtDivBG paddingL ClsLabelNrml">
                                    2,000
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    Admission Fee
                                </td>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    Non-refundable
                                </td>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    One time
                                </td>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    At the time of admission.
                                </td>
                                <td align="right" class="TxtDivBG paddingL ClsLabelNrml">
                                    500
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    Computer Charges
                                </td>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    Non-refundable
                                </td>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    Per Annum
                                </td>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    At the time of admission / in May every year.
                                </td>
                                <td align="right" class="TxtDivBG paddingL ClsLabelNrml">
                                    650
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="TxtDivBG paddingL ClsLabelNrml" style="font-weight: bold"
                                    colspan="4">
                                    Total Fees to be paid at the time of admission Rs.
                                </td>
                                <td align="right" class="TxtDivBG paddingL ClsLabelNrml" style="font-weight: bold">
                                    9,150
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    <b>Tennis Coaching Std. III to Std. IX</b>
                                </td>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    <b>Non-refundable</b>
                                </td>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    <b>Quarterly</b>
                                </td>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    At the time of admission & subsequently by the 10<sup>th</sup> of the first month
                                    of every quarter.
                                </td>
                                <td align="right" class="TxtDivBG paddingL ClsLabelNrml">
                                    375 <span style="color: Red; font-size: x-large"><b>*</b> </span>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="ClsLabelNrml padding10" valign="top">
                        Fees have to be paid through <b>Demand Draft / Pay Order only</b>.
                    </td>
                </tr>
                <tr>
                    <td align="left" class="ClsLabelNrml padding10" valign="top">
                        Two separate DD’s have to be issued in favour of:
                    </td>
                </tr>
                <tr>
                    <td align="left" class="ClsLabelNrml padding10" valign="top">
                        1) "<b>Pawar Public Charitable Trust</b>" for <b>Rs. 10,000/-</b> towards Refundable
                        Caution Money Deposit.
                    </td>
                </tr>
                <tr>
                    <td align="left" class="ClsLabelNrml padding10" valign="top">
                        2) "<b>Pawar Public School, Pune</b>" for <b>Rs. 9,150/-</b> towards Admission Fee,
                        Quarterly Tuition Fee, Term Fee and Computer charges.<br />
                        <b><u>Kindly note this fee includes Tuition fees only for the first quarter.</u></b>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="ClsLabelNrml padding10" valign="top">
                        3) <span style="color: Red; font-size: x-large"><b>*</b></span>A separate DD for<b>
                            Rs. 375/- </b>for tennis coaching drawn in favour of "<b> Mahesh Bhupathi Team Tennis
                                Pvt. Ltd.</b>" should be paid along with the quarterly school fees.
                    </td>
                </tr>
                <tr>
                    <td align="left" class="ClsLabelNrml padding10" valign="top">
                        4) Online payment facility for fees now available on the school website.
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="top">
                        <hr style="height: 1px; color: Black" />
                    </td>
                </tr>
                <tr>
                    <td align="left" class="HeadTxtB borderBtm" colspan="1" style="height: 25px">
                        <a name="Old_Student" class="Lbl10ptB" style="font-size: 14pt">Fee Structure 2010 -
                            11 <span style="color: #c45001">(Updated As On 20<sup>th</sup> April 2010) (Existing
                                Students) </span></a>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="ClsLabelNrml padding10">
                        <table cellpadding="3" cellspacing="1" class="ColorHeadBgFee" width="100%">
                            <tr>
                                <td align="center" class="TxtDivBG paddingL ClsLabelNrml" style="font-size: small;
                                    font-weight: bold; background-color: LightGrey; width: 22%">
                                    Fee Break-up
                                </td>
                                <td align="center" class="TxtDivBG paddingL ClsLabelNrml" style="font-size: small;
                                    font-weight: bold; background-color: LightGrey; width: 14%">
                                    Type
                                </td>
                                <td align="center" class="TxtDivBG paddingL ClsLabelNrml" style="font-size: small;
                                    font-weight: bold; background-color: LightGrey">
                                    Frequency
                                </td>
                                <td align="center" class="TxtDivBG paddingL ClsLabelNrml" style="font-size: small;
                                    font-weight: bold; background-color: LightGrey">
                                    Payable
                                </td>
                                <td align="center" class="TxtDivBG paddingL ClsLabelNrml" style="font-size: small;
                                    font-weight: bold; background-color: LightGrey">
                                    Amount<br />
                                    (in Rs.)
                                </td>
                            </tr>
                            <%--<tr>
                                                    <td align="left" class="TxtDivBG paddingL ClsLabelNrml" style="height: 5%" colspan="5">
                                                    </td>
                                                </tr>--%>
                            <tr>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml" colspan="3">
                                    Qtr 1: <b>At the Time of Admission</b>
                                </td>
                                <td align="right" class="TxtDivBG paddingL ClsLabelNrml">
                                    Total Annual Fees Rs.
                                </td>
                                <td align="right" class="TxtDivBG paddingL ClsLabelNrml" style="font-weight: bold">
                                    29,150
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    Tuition Fee
                                </td>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    Non-refundable
                                </td>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    <b>Quarterly</b>
                                </td>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    Between 29<sup>th</sup> April, 2010 to 4<sup>th</sup> May, 2010 & subsequently by
                                    the 10<sup>th</sup> of the first month of every quarter.
                                </td>
                                <td align="right" class="TxtDivBG paddingL ClsLabelNrml">
                                    6,000
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    Term Fee
                                </td>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    Non-refundable
                                </td>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    Twice Annually
                                </td>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    Between 29<sup>th</sup> April, 2010 to 4<sup>th</sup> May, 2010 and in December
                                    every year.
                                </td>
                                <td align="right" class="TxtDivBG paddingL ClsLabelNrml">
                                    2,000
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    Computer Charges
                                </td>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    Non-refundable
                                </td>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    Per Annum
                                </td>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    Between 29<sup>th</sup> April, 2010 to 4<sup>th</sup> May, 2010 / in May every year.
                                </td>
                                <td align="right" class="TxtDivBG paddingL ClsLabelNrml">
                                    650
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="TxtDivBG paddingL ClsLabelNrml" style="font-weight: bold"
                                    colspan="4">
                                    Total Fees to be paid Between 29<sup>th</sup> April, 2010 to 4<sup>th</sup> May,
                                    2010 Rs.
                                </td>
                                <td align="right" class="TxtDivBG paddingL ClsLabelNrml" style="font-weight: bold">
                                    8,650
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="TxtDivBG paddingL ClsLabelNrml" style="font-weight: bold"
                                    colspan="4">
                                    Sr. KG students getting promoted to Std I will have to pay Re-Admission fee
                                </td>
                                <td align="right" class="TxtDivBG paddingL ClsLabelNrml">
                                    500
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="TxtDivBG paddingL ClsLabelNrml" style="font-weight: bold"
                                    colspan="4">
                                    Total Amount (for Sr. KG going to Std. I)
                                </td>
                                <td align="right" class="TxtDivBG paddingL ClsLabelNrml" style="font-weight: bold">
                                    9,150
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    <b>Tennis Coaching Std. III to Std. IX</b>
                                </td>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    <b>Non-refundable</b>
                                </td>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    <b>Quarterly</b>
                                </td>
                                <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                    Between 29<sup>th</sup> April, 2010 to 4<sup>th</sup> May, 2010 & subsequently by
                                    the 10<sup>th</sup> of the first month of every quarter.
                                </td>
                                <td align="right" class="TxtDivBG paddingL ClsLabelNrml">
                                    375 <span style="color: Red; font-size: x-large"><b>*</b> </span>
                                </td>
                            </tr>
                            <%--<tr>
                                                    <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                                        Admission fee
                                                    </td>
                                                    <td align="right" class="TxtDivBG paddingL ClsLabelNrml">
                                                        500.00
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                                        Software Charges
                                                    </td>
                                                    <td align="right" class="TxtDivBG paddingL ClsLabelNrml">
                                                        500.00
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                                        ID Card and School diary
                                                    </td>
                                                    <td align="right" class="TxtDivBG paddingL ClsLabelNrml">
                                                        150.00
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                                        Field Trips and ECA
                                                    </td>
                                                    <td align="right" class="TxtDivBG paddingL ClsLabelNrml">
                                                        1250.00
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" class="TxtDivBG paddingL ClsLabelNrml" style="font-weight: bold">
                                                        Total Rs.
                                                    <td align="right" class="TxtDivBG paddingL ClsLabelNrml" style="font-weight: bold">
                                                        2,400.00
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="TxtDivBG paddingL ClsLabelNrml" style="font-weight: bold">
                                                        1<sup>st</sup> Quarter
                                                    </td>
                                                    <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                                        Term Fee - I
                                                    </td>
                                                    <td align="right" class="TxtDivBG paddingL ClsLabelNrml">
                                                        2,000.00
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                                        Tuition fee for June - August 2009
                                                    </td>
                                                    <td align="right" class="TxtDivBG paddingL ClsLabelNrml">
                                                        6,000.00
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" class="TxtDivBG paddingL ClsLabelNrml" style="font-weight: bold">
                                                        Total Rs.
                                                    <td align="right" class="TxtDivBG paddingL ClsLabelNrml" style="font-weight: bold">
                                                        8,000.00
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="TxtDivBG paddingL ClsLabelNrml" style="font-weight: bold">
                                                        2<sup>nd</sup> Quarter
                                                    </td>
                                                    <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                                        Tuition fee for September - November 2009
                                                    </td>
                                                    <td align="right" class="TxtDivBG paddingL ClsLabelNrml">
                                                        6,000.00
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" class="TxtDivBG paddingL ClsLabelNrml" style="font-weight: bold">
                                                        Total Rs.
                                                    <td align="right" class="TxtDivBG paddingL ClsLabelNrml" style="font-weight: bold">
                                                        6,000.00
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="TxtDivBG paddingL ClsLabelNrml" style="font-weight: bold">
                                                        3<sup>rd</sup> Quarter
                                                    </td>
                                                    <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                                        Term Fee - II
                                                    </td>
                                                    <td align="right" class="TxtDivBG paddingL ClsLabelNrml">
                                                        2,000.00
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                                        Tuition fee for December 2009 - February 2010
                                                    </td>
                                                    <td align="right" class="TxtDivBG paddingL ClsLabelNrml">
                                                        6,000.00
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" class="TxtDivBG paddingL ClsLabelNrml" style="font-weight: bold">
                                                        Total Rs.
                                                    <td align="right" class="TxtDivBG paddingL ClsLabelNrml" style="font-weight: bold">
                                                        8,000.00
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="TxtDivBG paddingL ClsLabelNrml" style="font-weight: bold">
                                                        4<sup>th</sup> Quarter
                                                    </td>
                                                    <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="TxtDivBG paddingL ClsLabelNrml">
                                                        Tuition fee for March - May 2010
                                                    </td>
                                                    <td align="right" class="TxtDivBG paddingL ClsLabelNrml">
                                                        6,000.00
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" class="TxtDivBG paddingL ClsLabelNrml" style="font-weight: bold">
                                                        Total Rs.
                                                    <td align="right" class="TxtDivBG paddingL ClsLabelNrml" style="font-weight: bold">
                                                        6,000.00
                                                    </td>
                                                </tr>--%>
                        </table>
                    </td>
                </tr>
                <%--<tr>
                                        <td align="left" class="ClsLabelNrml paddingL" valign="top">
                                        <a href="javascript:openFeeStructure();" class="HPLFee">Circular for Parents</a>
                                        </td>
                                    </tr>--%>
                <%--<tr>
                                        <td align="left" class="ClsLabelNrml padding10" valign="top" >
                                            For <b>Std. 8<sup>th</sup> Rs. 300/- extra for Asset Exam</b>.
                                        </td>
                                    </tr>--%>
                <tr>
                    <td align="left" class="ClsLabelNrml padding10" valign="top">
                        Fees have to be paid through <b>Demand Draft / Pay Order only</b>.
                    </td>
                </tr>
                <tr>
                    <td align="left" class="ClsLabelNrml padding10" valign="top">
                        Two separate DD’s have to be issued in favour of:
                    </td>
                </tr>
                <tr>
                    <td align="left" class="ClsLabelNrml padding10" valign="top">
                        1) "<b>Pawar Public School, Pune</b>" for <b>Rs. 8,650/-</b> towards 1<sup>st</sup>
                        Quarter Tuition Fee, Term Fee and Computer charges.<br />
                        <%-- <b><u>Kindly note this fee includes Tuition fees only for the first quarter.</u></b>--%>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="ClsLabelNrml padding10" valign="top">
                        2) <span style="color: Red; font-size: x-large"><b>*</b></span>A separate DD for<b>
                            Rs. 375/- </b>for tennis coaching drawn in favour of "<b> Mahesh Bhupathi Team Tennis
                                Pvt. Ltd.</b>" should be paid along with the quarterly school fees.
                    </td>
                </tr>
                <tr>
                    <td align="left" class="ClsLabelNrml padding10" valign="top">
                        3) Online payment facility for fees now available on the school website.
                    </td>
                </tr>
                <tr>
                    <td align="left" class="ClsLabelNrml padding10" valign="top">
                        <b>In case the fees are not paid on due date the school reserves the right to cancel
                            the allotment &amp; offer the same to another child.</b>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="Lbl10ptB borderBtm" colspan="1" style="height: 25px">
                        <span class="Lbl10ptB" style="font-size: 14pt">Cancellation Policy for 2010-11</span><span
                            style="font-size: 12pt"> (NURSERY to Std. IX)</span><br />
                    </td>
                </tr>
                <tr>
                    <td align="left" class="ClsLabelNrml padding10" valign="top">
                        <b><u>No Fees will be refunded except the Caution Money Deposit of Rs. 10,000/-</u></b>
                    </td>
                </tr>
            </tbody>
        </table>
    </table>
</asp:Content>
