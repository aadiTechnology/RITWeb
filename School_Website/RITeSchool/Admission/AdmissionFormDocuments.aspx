<%@ Page Title="Admission process" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/OnlineAdmission.master"
    AutoEventWireup="true" CodeFile="AdmissionFormDocuments.aspx.cs" Inherits="AdmissionFormDocuments"
    ErrorPage="~/RITeSchool/Admission/Error.aspx" %>

<%@ Register TagPrefix="Wizard" TagName="AdmissionSteps" Src="~/UserControls/AdmissionWizardStepsUC.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div>
        <table>
            <tr>
                <td>
                    <Wizard:AdmissionSteps ID="SubmissionWizardSteps" runat="server" ActiveSteps="2">
                    </Wizard:AdmissionSteps>
                </td>
            </tr>
        </table>
    </div>
    <div style="width: 97%" align="center">
        <div id="nifty" align="center">
            <b class="rtop"><b class="r1"></b><b class="r2"></b><b class="r3"></b><b class="r4">
            </b></b>
            <table align="center" id="tblSupportingDocuments" runat="server" class="paddingLR"
                cellspacing="1" cellpadding="1" border="0" width="100%">
                <tbody>
                    <tr id="trInst" runat="server">
                        <td class="HeadTxtBWOPadding borderBtm" align="left" colspan="4">
                            Admission Form - Instructions
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="TxtNormal" style="width: 135px">
                            <asp:Image ID="Image1" runat="server" Height="1px" ImageUrl="~/images/spacer.gif"
                                Width="150px" />
                        </td>
                        <td align="left">
                        </td>
                        <td align="left" class="TxtNormal">
                            <asp:Image ID="Image3" runat="server" Height="1px" ImageUrl="~/images/spacer.gif"
                                Width="120px" />
                        </td>
                        <td align="left">
                        </td>
                    </tr>
                    <tr id="trHeading" runat="server" visible="false">
                        <td class="HeadTxtBWOPadding borderBtm" style="height: 25px" align="left" colspan="4">
                            List of Supporting Documents:
                        </td>
                    </tr>
                    <tr id="trDSKHeading" runat="server" visible="false">
                        <td class="HeadTxtBWOPadding borderBtm" style="height: 25px" align="left" colspan="4">
                            List of Supporting Documents to be Submitted along with the Form :
                        </td>
                    </tr>
                    <tr id="trDocumnetHeader" runat="server">
                        <td colspan="4" align="left" class="TextNormalB">
                            Please bring the following documents along with you when coming for final admission.
                        </td>
                    </tr>
                    <tr id="trDocumentHeaderDPIS" runat="server" visible="false">
                        <td colspan="4" align="left" class="TextNormalB">
                            Please bring the following Notary attested documents along with you when you come
                            for admission.
                        </td>
                    </tr>
                    <tr id="docsPPS" runat="server" visible="false">
                        <td colspan="4" align="left" class="TxtNormal">
                            <ol>
                                <%-- <li>Two Recent Photographs</li>
                                <li>Family Photograph</li>
                                <li>Copy of Birth Certificate* (Attested True Copy)</li>
                                <li>Residence Proof (Local)</li>
                                <li>Copy of Aadhar Card</li>
                                <li>Fitness Certificate from Registered Medical Practitioner (original)</li>
                                <li>Copy of Passport / PIO card (for students traveling from outside India)</li>
                                <li>Copy of Caste Certificate (valid & Attested True Copy), if applicable</li>--%>
                                <li>Printed copy of Admission Form</li>
                                <li>Two Recent Passport size photographs.</li>
                                <li>Family Photograph (postcard size)</li>
                                <li>Copy of Birth Certificate (Notarized True Copy)</li>
                                <li>Residence Proof (Notarized True Copy)</li>
                                <li>Copy of Student’s Aadhar Card (Notarized True Copy for Student and Parents)</li>
                                <li id="li2to9PPSMarkSheet" runat="server" visible="false">Copy of Term I and Term-II Term Mark-sheet</li>
                                <li> Medical Certificate from Registered Medical Practitioner (Form attached)</li>
                                <li>Parent Consent Form (Form attached)</li>
                                <li>Undertaking from Parent (form attached)</li>    
                                <li id="li2to9PPSLC" runat="server" visible="false">Original Leaving Certificate with the Saral ID and UDISE number</li>                            
                                <li>Copy of Caste Certificate (Notarized True Copy), if applicable</li>
                                <li>Copy of Passport / PIO card (for students travelling from outside India),if applicable</li>
                                <li id="liLC" runat="server" visible="false">Original Leaving Certificate, if applicable</li>
                            </ol>
                            * In case of Birth certificate in languages other than English / Hindi / Marathi,
                            please bring a notarized copy of the certificate translated in English.
                        </td>
                    </tr>
                    <tr id="docsPPSH" runat="server" visible="false">
                        <td colspan="4" align="left" class="TxtNormal">
                            <ol>
                                <li>Two recent photographs of the student</li>
                                <li>One recent photograph each of the parents</li>
                                <li>Attested true copy of the Birth Certificate</li>
                                <li>Residence proof as mentioned below<br />
                                    Residence proof should strictly be in the name of the father or the mother of the
                                    child seeking admission.<br />
                                    Any <b>ONE</b> of the following documents can be produced as proof of residence:
                                    <ol>
                                        <li>Passport (Recent)</li>
                                        <li>Electricity Bills</li>
                                        <li>Telephone Bill (Land Line)</li>
                                        <li>Unique I.D. (AAdhar) Card</li>
                                        <li>Registered Agreement Copy (Purchase or Rented)</li>
                                        <li>Bank Statement for the Current Month</li>
                                    </ol>
                                </li>
                                <li>Medical certificate from a registered Medical Practitioner (signed and stamped)</li>                                
                                <li>Parents from Reserved Category, who want the cast to be entered in the school register
                                    should submit the Caste Certificate issued by the concerned Municipal authorities
                                    in the name of the student</li>
                            </ol>
                        </td>
                    </tr>
                    <tr id="docsSS" runat="server" visible="false">
                        <td colspan="4" align="left" class="TxtNormal">
                            <ol>
                                <li>Two Passport size photos of the child</li>
                                <li>Fitness Certificate from Registered Medical Practitioner</li>
                                <li>Original DOB Certificate (for Nursery attested photo copy of DOB)</li>
                                <li>Original Leaving Certificate from the Previous School (Std. I onwards)</li>
                                <li>The Child's academic reports of the previous class (Std. I onwards)</li>
                            </ol>
                        </td>
                    </tr>
                    <tr id="docsFBS" runat="server" visible="false">
                        <td colspan="4" align="left" class="TxtNormal">
                            <ol>
                                <li>Two Recent Photographs</li>
                                <li>Attested Copy of Birth Certificate (Nursery to Std. I)</li>
                                <li>Residence Proof (Local)</li>
                                <li>Original Birth Ceritficate OR Leaving Certificate of Earlier School (For Std 1st
                                    onwords)</li>
                            </ol>
                        </td>
                    </tr>
                    <tr id="docsMCPS" runat="server" visible="false">
                        <td colspan="4" align="left" class="TxtNormal">
                            <ol>
                                <li>Latest 3 Stamp Size Colour Photo of Child</li>
                                <li>Birth Certificate copy dully attested (child’s name on birth certificate is Mandatory)</li>
                                <li>Caste certificate (if applicable)</li>
                                <li>Bonafide certificate from pre-school (if applicable)</li>
                                <li>Aadhar card (If available)</li>
                                <li>Original Transfer Certificate of previous school</li>
                            </ol>
                        </td>
                    </tr>
                    <tr id="docsDSK" runat="server" visible="false">
                        <td colspan="4" align="left" class="TxtNormal">
                            <ol>
                                <li>Original Birth Certificate.</li>
                                <li>Photo copy of Caste Certificate, if applicable.</li>
                                <li>One passport size photo.</li>
                                <li>Photo Copy Of Aadhar Card.</li>
                            </ol>
                            <span style="padding-left: 22px;">* In case of Birth certificate in languages other
                                than English / Hindi / Marathi, please bring a notarized copy of the certificate
                                translated in English.</span>
                        </td>
                    </tr>
                    <tr id="trdocDPIS" runat="server" visible="false">
                        <td colspan="4" align="left" class="TxtNormal">
                            <ol>
                                <li id="liPrintoutDPIS" runat="server">Printout of the Online Admission Form.</li>
                                <li>Two recent passport size photographs of the student and Family group photograph.</li>
                                <li>True Copy of the Birth Certificate. In case of certificates in languages other than
                                    English, please submit a notarized copy of the certificate translated in English.</li>
                                <li>Copy of Student's Aadhar Card.</li>
                                <li>True copy of Residence proof as mentioned below:</li>
                                Residence proof should strictly be in the name of the father or the mother of the
                                child seeking admission.<br />
                                a. Passport (Recent)<br />
                                b. Electricity Bill(Recent)<br />
                                c. Telephone Bill (Land Line)<br />
                                d. Unique I.D. (Aadhar) Card<br />
                                e. Bank statement for the current month<br />
                                f. Registered Agreement Copy (Purchase or Rented)#<br />
                                # If a copy of rent agreement is attached as the proof of residence, then it should
                                be accompanied by one more address proof, out of the options given above.<br />
                                <span style="color: Red; font-weight: bold;">Please Note - </span>Ration Card <b>Will
                                    NOT</b> be accepted as proof of residence.
                                <li>Medical Certificate has to be filled, signed and stamped by a registered medical
                                    practitioner.</li>
                                <li>Parent consent form needs to be filled by parent and submitted to the school.</li>
                                <li>Parents from Reserved Category, who want the caste to be entered in the school register
                                    should submit the Caste Certificate issued by the concerned Municipal authorities
                                    in the name of the student.</li>
                                <li>Previous year's report card if applicable.</li>
                            </ol>
                        </td>
                    </tr>
                    <tr id="trdocZLSP" runat="server" visible="false">
                        <td colspan="4" align="left" class="TxtNormal">
                            <ol>
                                <li>Original Birth Certificate.</li>
                                <li>Original School Leaving Certificate of the Student.</li>
                                <li>Aadhar Card.</li>
                                <li>4 Passport Size photographs of Students.</li>
                                <li>Cast Certificate or Fathers LC / cast Certificate.</li>
                            </ol>
                        </td>
                    </tr>
                    <tr id="trNotes" runat="server">
                        <td class="HeadTxtBWOPadding borderBtm" style="height: 25px" align="left" colspan="4">
                            Notes:
                        </td>
                    </tr>
                    <tr id="trNotesDPIS" runat="server" visible="false">
                        <td class="HeadTxtBWOPadding borderBtm" style="height: 25px" align="left" colspan="4">
                            SPECIAL NOTICE
                        </td>
                    </tr>
                    <tr id="trNotesDetails" runat="server">
                        <td colspan="4" align="left" class="TxtNormal">
                            <ol>
                                <li>Original Documents to be produced for verification at the time of admission.</li>
                                <li>Forms will <u>not</u> be accepted if they are incomplete, without the supporting
                                    document or if original documents are not produced for verification.</li>
                                <li>If the pupil has come from a different District / State, other than <span id="liTC"
                                    runat="server">Pune</span> district, the Transfer Certificate must be countersigned
                                    by the appropriate Inspector concerned.</li>
                                <li>If the pupil has come from a different country, other than India, the Transfer Certificate
                                    must be countersigned by the appropriate officer of the Indian Embassy / Consulate
                                    / High Commission in that country concerned. Documents without this stamp will not
                                    be accepted and the admission will be provisional (upto 2 months), subject to the
                                    parents providing the said documents.</li>
                            </ol>
                        </td>
                    </tr>
                    <tr id="trNoticeDPIS" runat="server" visible="false">
                        <td colspan="4" align="left" class="TxtNormal">
                            <ul>
                                <li><b>The School will not entertain any correspondence, discussion, telephonic or personal
                                    inquiries regarding the admission process.</b></li>
                                <li>Any intervention or pressure in the normal admission process will lead to immediate
                                    disqualification of the application.</li>
                                <li>Genuine queries can be directed to <b><a href="mailto:contact@dpispcmc.com">
                                    contact@dpispcmc.com</b></a> mail id as per the school. Information received from any other source may not be reliable and the school will not be responsible for the same.</li>
                                <li>The school regrets its inability to address parental inquiries on an individual basis. All information required is available on the website. <a target="_blank" href="https://dpispcmc.com">
                                        <b>www.dpispcmc.com</b></a></li>
                                <li><b>Please note that the School Management does not accept donation of any kind whatsoever.
                                    Neither does the Management authorize any person to do so. In case any person claims
                                    to secure a seat in our school through influence or consideration please bring it
                                    to the notice of the school authorities for suitable action.</b></li>
                                <li><b>The school does not reserve any seats on any grounds other than the RTE quota,
                                    for direct siblings<span id="spnAmanotaMessage" runat="server"> and for Amanora Citizens</span>.
                                    In case any person claims to secure a seat in our school through influence or consideration
                                    please bring it to the notice of administration for suitable action.</b></li>
                            </ul>
                        </td>
                    </tr>
                </tbody>
            </table>
            <table align="center" id="tblSupporingDocumenrsPPSN" runat="server" class="paddingLR"
                cellspacing="1" cellpadding="1" border="0" width="100%">
                <tbody>
                    <tr>
                        <td class="HeadTxtBWOPadding borderBtm" align="left" colspan="4">
                            Admission Form - Instructions
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="TxtNormal" style="width: 135px">
                            <asp:Image ID="Image2" runat="server" Height="1px" ImageUrl="~/images/spacer.gif"
                                Width="150px" />
                        </td>
                        <td align="left">
                        </td>
                        <td align="left" class="TxtNormal">
                            <asp:Image ID="Image4" runat="server" Height="1px" ImageUrl="~/images/spacer.gif"
                                Width="120px" />
                        </td>
                        <td align="left">
                        </td>
                    </tr>
                    <tr id="tr1" runat="server">
                        <td class="HeadTxtBWOPadding borderBtm" style="height: 25px; color: Red;" align="left"
                            colspan="4">
                            List of Supporting Documents:
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="left" class="TextNormalB" style="text-align: justify;">
                            <b>Please bring the following self-attested documents along with original documents
                                when you come for admission.</b>
                        </td>
                    </tr>
                    <tr id="Tr3" runat="server">
                        <td colspan="4" align="left" class="TxtNormal" style="line-height: 1.6; text-align: justify;">
                            <ol>
                                <li>A self-attested copy of the Birth Certificate in English with the Child’s Name,
                                    Mother’s Name and Father’s Name clearly mentioned. <b>(Mandatory for all classes)</b></li>
                                <li>Two passport size (35mm x 35mm) latest colour photographs of the child properly
                                    affixed on the place provided on the printout of the Admission Form and group photo
                                    of family<b> (Mother, Father of child and child) Size for family photo: 6”x 4” with
                                        plain white background (Mandatory)</b>.</li>                                
                                <li><b>A self-attested copy of Aadhaar card is mandatory. (Mother, Father of child and
                                        child)</b></li>
                                <li>Proof of Residence (Local): A self-attested copy of residence proof. Residence proof
                                    should be strictly in the name of father, mother or grandparents of the child seeking
                                    admission as mentioned below :<br />
                                    <b>Any ONE</b> of the following documents can be produced as proof of residence:<br />
                                    <%--<br />--%>
                                    a. Passport (Recent)<br />
                                    b. Electricity Bill (Recent)<br />
                                    c. Telephone Bill (Landline)<br />
                                    d. Unique I.D.Aadhaar Card (A self-attested by parent copy of the student’s Aadhaar card and
                                    both the parents’ Aadhaar card is mandatory).<br />
                                    e. Bank statement of the current month<br />
                                    f. Registered Sale / Rent Agreement Copy (Recent)<br />
                                    <%--g. If a copy of rent agreement is attached as the proof of residence, then it should
                                    be accompanied by one more address proof, out of the options given above.--%>
                                     <span style="display: block;
                                        padding-left: 15px;"><b>Please Note </b>- Ration Card <b>Will NOT</b> be accepted
                                        as proof of residence.</span>
                                    <li>The Medical Certificate provided in the printout of online Admission Form has to
                                        be completely filled, signed and stamped by a Registered Medical Practitioner <b>(Mandatory)</b>.</li>
                                    <li>Photocopy of Passport / OCI card (for students migrating from outside India).</li>
                                    <li>Parents from the Reserved Category, who want the caste to be entered in the school register should submit a self-attested copy of the <b>Caste Certificate in the name of the student/ father and issued by the concerned Government Authorities at the time of admission</b> </li>

                                    <li id="liBonafideNote" runat="server"><b>Bonafide certificate by previous school is for Std. I to Std. VIII. (Mandatory)</b></li>
                                    <li id="liDueNote" runat="server" visible="false"><b>No dues certificate from the previous
                                        school. (Mandatory)</b></li>
                                    <%--<li>For Std. II to Std. VIII a self-attested copy of the mark sheet of the previous
                                        Std. from the previous school. <b>In case of student migrating from other countries,
                                            Indian Embassy stamp is mandatory on Transfer Certificate. (Not mandatory for Nursery
                                            to Std. I)</b></li>--%>
                                    <li id="liMarkSheet" runat="server" visible = "false">For Std. II to Std. VIII a self-attested copy of the mark sheet of the previous Std. from the previous school. In case of student migrating from other countries, Indian Embassy stamp is mandatory on Transfer Certificate</li>
                                    <li id="liLCTC" runat="server" visible = "false">Admission to Std. II to Std. VIII will be treated as provisional till the receipt of the original School Leaving /Transfer Certificate from the previous school. The last date for submission of the same is Wednesday, 29th June, 2022.</li>

                                    <%--<li>Admission to Std. II to Std. VIII will be treated as provisional till the receipt
                                        of the original School Leaving / Transfer Certificate from the previous school.
                                        The last date for submission of the same is <b>Wednesday, 29<sup>th</sup> June, 2022</b>.</li>--%>
                                    <%--<li>Self attested copy of the marksheet of the previous Std. from the previous school.</li>--%>
                            </ol>
                            <%--<ul>
                                <li><b>You are requested to carry all the original documents for verification.</b></li>
                            </ul>--%>
                        </td>
                    </tr>
                    <tr id="Tr1to10" runat="server" visible="false">
                        <td colspan="4" align="left" class="TxtNormal" style="line-height: 1.6; text-align: justify;">
                            <ol>
                                <li>A self-attested copy of the <b>Birth Certificate, in English</b> with the Child’s Name, Mother’s Name and Father’s Name clearly mentioned.<b>(Mandatory for all classes)</b></li>
                                <li>Two passport size (35mm x 35mm) latest colour photographs of the child properly affixed on the place provided on the printout of the Admission Form and group photo of family <b>(Mother, Father of child and child) Size for family photo: 6”x 4” with plain white background (Mandatory)</b>.</li>
                                <li><b>For Std. II onwards, a Bonafide Certificate from the previous recognized school, along with the student’s U-DISE+ (PEN) number and APAAR ID, is mandatory.</b></li>
                                <li><b>No dues certificate from the previous school. (Mandatory).</b></li>
                                <li><b>The Original School Leaving/ Transfer Certificate from the previous Government recognized school.</b></li>
                                <li>Proof of Residence (Local): A self-attested copy of residence proof. Residence proof should be strictly in the name of the father, mother or grandparents of the child seeking admission <b>Any ONE</b> of the following documents can be produced as proof of residence:
                                    <table>
                                        <tr>
                                            <td align="left">
                                                a. Passport (Recent)
                                            </td>
                                            <td align="left">
                                                b. Electricity Bill (Recent)
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="padding-right:50px;">
                                                c. Registered Sale / Rent Agreement Copy (Recent)
                                            </td>
                                            <td align="left">
                                                d. Aadhaar Card
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                e. Bank statement for the current month
                                            </td>
                                            <td align="left">
                                                f. Telephone Bill (Land Line)
                                            </td>
                                        </tr>
                                    </table>
                                </li>
                                <li>For Std. II to Std. VIII a self-attested copy of the mark sheet of the previous Std. from the previous school. <b>In case of student migrating from other countries, Indian Embassy stamp is required on relieving documents</b>.</li>
                                <li><b>The Medical Certificate</b> provided in the printout of online Admission Form has to be completely filled, signed and stamped by a <b>Registered Medical Practitioner (Mandatory)</b>.</li>
                                <li>Photocopy of Passport / OCI card (for students migrating from outside India).</li>
                                <li>Parents from the Reserved Category, who want the caste to be entered in the school register should submit a self-attested copy of the <b>Caste Certificate in the name of the student/ father and issued by the concerned Government Authorities at the time of admission.</b></li>
                                <li>A self-attested copy of the <b>student’s Aadhaar card and both the parents’ Aadhaar card</b> is mandatory.</li>
                                <li>Admission to Std. II to Std. VIII will be treated as provisional till the receipt of the original School Leaving / Transfer Certificate from the previous school. The last date for submission of the same is <b id="bPPSN2to8" runat="server">Saturday, 26<sup>th</sup> April, 2025</b>.</li>
                            </ol>
                            <span style="padding-left:25px;"><b>You are requested to carry all the original documents for verification.</b></span>
                        </td>
                    </tr>
                    <tr id="trProcessForNursery">
                        <td colspan="4">
                            <table style="width: 100%;">
                                <tr>
                                    <td class="HeadTxtBWOPadding borderBtm" style="height: 25px" align="left">
                                        <asp:Label ID="lblAdmissionProcess" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TxtNormal" style="line-height: 1.6; text-align: justify;">
                                        <ul>
                                            <%-- <li id="NursaryCriteria" runat ="server">Eligibility : 3 years complete as on 31<sup>st</sup> December, 2021.(All children born on or between 1<sup>st</sup> January, 2018 and 31<sup>st</sup> December, 2018).</li>
                                            <li  id="JrkG" runat ="server" >Eligibility : 4 years complete as on 31<sup>st</sup> December, 2021.(All children born on or between 1<sup>st</sup> October, 2016 and 31<sup>st</sup> December, 2017)</li>
                                            <li  id="SrKg" runat ="server">Eligibility : 5 years complete as on 31<sup>st</sup> December, 2021.(All children born on or between 1<sup>st</sup> October, 2015 and 31<sup>st</sup> December, 2016)</li>
                                            <li  id="First" runat ="server">Eligibility : 6 years complete as on 31<sup>st</sup> December, 2021.(All children born on or between 1<sup>st</sup> October, 2014 and 31<sup>st</sup> December, 2015)</li>--%>
                                            <li id="liGrade1" runat="server" visible="false"><b>As per the Government guidelines
                                                25% seats at entry level (Std. I) are reserved as RTE Quota.</b></li>
                                            <%--<li id="liGrade2" runat="server" visible="false"><b>Activity paper of 1 hour will be conducted on Wednesday, 15th February 2023 and Saturday, 25th February 2023 at 10.00 a.m. in school premises, those selected students in merit list will be declared on 21st & 28th February 2023 at 03:00 pm on website and notice board.</b></li>--%>
                                            <%--<li id="liGrade2" runat="server" visible="false"><b>Activity paper of 1 hour will be conducted on any one Saturday, 18<sup>th</sup>, 25<sup>th</sup> March 2023 and Saturday, 8<sup>th</sup> April 2023 at 10.00 a.m. in school premises, those selected students in merit list will be declared on 18<sup>th</sup>, 25<sup>th</sup> March 2023 and 10<sup>th</sup> April 2023 at 03:00 PM on website and notice board.</b></li>--%>

                                            <li id="liGrade2" runat="server" visible="false">Activity paper of 1 hour will be conducted on Saturday, <b>11<sup>th</sup> January 2025</b> and Saturday, <b>25<sup>th</sup> January 2025 at 10.00 a.m.</b> in school premises. Form numbers of selected students in <b>merit list will be declared on 16<sup>th</sup> & 30<sup>th</sup> January 2025 at 03:00 pm on website and notice board.</b> </li>
                                            <li id="liGrade2to8" runat="server" visible="false">If seats remain vacant and those who have not appeared 7th February 2026 activity, another activity will be conducted on Saturday, 14th February 2026 at 10:00 a.m. in the school premises. The form numbers of selected students (merit list) will be declared on 18th February 2026 at 3:00 p.m. on the school website and notice board.</li>

                                            <li id="NursaryCriteria" runat="server">Eligibility : 3 years complete as on 31<sup>st</sup>
                                                December, 2025.(All children born on or between 1<sup>st</sup> October, 2021 and
                                                31<sup>st</sup> December, 2022).</li>
                                            <li id="JrkG" runat="server">Eligibility : 4 years complete as on 31<sup>st</sup> December, 2025.(All children born on or between 1<sup>st</sup> October, 2020 to 31<sup>st</sup> December, 2021).</li>
                                            <li id="SrKg" runat="server">Eligibility : 5 years complete as on 31<sup>st</sup> December,
                                                2025.(All children born on or between 1<sup>st</sup> October, 2019 to 31<sup>st</sup>
                                                December, 2020)</li>
                                            <li id="First" runat="server">Eligibility : 6 years complete as on 31<sup>st</sup> December, 2023.(All children born on or between 1<sup>st</sup> Oct, 2016 and 31<sup>st</sup> December, 2017).</li>
                                            <li id="SecondOnward" runat="server">Based on Activity Test – English and Mathematics
                                                of 20 marks each and passed and promoted certificate of the previous Std. from the
                                                previous school. Complete LC from previous school is mandatory.</li>
                                            <%--<li id="liLottery" runat="server">Following this all other seats will be filled as per
                                                <b>Random Selection </b>by Lottery system generated by computer program.</li>--%>
                                            <li id="liLottery" runat="server"><b>Random selection</b> (As per below preference) by lottery generated by computer program will be done.</li>
                                              <li id="liActivity2" runat="server">Activity for English and Mathematics for Grade II
                                                to Grade VIII will be held on stipulated dates.</li>
                                            <li id="liActivity1" runat="server">There will be an Activity for English and Mathematics.
                                                Admissions will be based on the performance in the Activity.</li>
                                            <li>Parents are requested Not to consider the seat as allotted till the Admission will
                                                be confirmed only after payment of fees and submission of all documents</b></li>
                                            <li id="liGrade2Merit" runat="server"><b>For Std.II to Std.VIII admissions will be given
                                                only on merit.</b></li>
                                            <li id="liGradePreference" runat="server"><b>Preference for Admission :</b>
                                                <br />
                                                1. Wards of : Landowners / Shareholders. <b>(*Bonafide certificate from Nanded City is required)</b></br> 
                                                2. Wards of : Nanded City Flat owners. <b>(*Bonafide certificate from Nanded City is required)</b></br>
                                                3. Siblings of existing students. <b>(*If all dues are cleared of sibling and if seats are available)</b><br />
                                                <span style="padding-left: 18px; font-weight: bold;">Note: Cousins will not be considered
                                                    as siblings.</span></br> 
                                                4. If seats remain, Nanded City tenants. <b>(*Bonafide certificate from Nanded City is required)</b></br> 
                                                5. If seats remain, then Non-Nanded City residents.
                                                <%--5. If seats remain, then Non-Nanded City residents.--%>
                                            </li>
                                        </ul>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trProcessForOther" runat="server" visible="false">
                        <td colspan="4">
                            <table style="width: 100%">
                                <tr>
                                    <td class="HeadTxtBWOPadding borderBtm" style="height: 25px" align="left" colspan="4">
                                        ADMISSION TO OTHER GRADES
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TxtNormal" style="line-height: 1.6; text-align: justify;">
                                        <ul>
                                            <li><b>There are no seats available for Std. I & III for the academic year 2023-24.</b></li>
                                            <li>Parents seeking admissions to Grades other than Nursery may put in their request
                                                for admission online and the same will be considered subject to availability of
                                                seats.</li>
                                            <li>Parents seeking admissions for Jr KG to Grade VIII may put in their request for
                                                admission online and the same will be considered subject to availability of seats.</li>
                                            <li id="StandardwiseLC" runat="server">All Admissions to Higher Grades will be considered
                                                provisional till the receipt of Original School Leaving Certificate from the previous
                                                school within the stipulated time.(LC / TC : if Student is from another country
                                                Counter sign and stamp by Indian Embassy, another State then Counter sign and stamp
                                                by State Education Authority is required)</li>
                                            <%--<li>Admissions are open from Grade V to Grade VIII.</li>--%>
                                            <li>There will be an activity paper for students seeking admission to Std.II to VIII.
                                                The activity paper will be taken for English and Mathematics. Each paper will be
                                                of <b>20 marks</b>. The activity will be based on the previous Std.’s syllabus.
                                                <b>Admissions to Std. II to VIII will be based on the performance in the activity and
                                                    passed and promoted certificate of previous Std. of previous school.</b></li>
                                            <li>Candidates will have to show the receipt of online payment of admission form fee
                                                to appear for the activity.</li>
                                            <li>Parents of candidates whose names are selected in merit list of activity will be
                                                informed by phone call and form number list will be displayed on notice board.</li>
                                            <li>Activity for English and Mathematics for Grade II to Grade VIII will be held on
                                                stipulated dates.</li>
                                        </ul>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="HeadTxtBWOPadding borderBtm" style="height: 25px" align="left" colspan="4">
                            SPECIAL NOTICE
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="left" class="TxtNormal" style="line-height: 1.6; text-align: justify;">
                            <ul>
                               <%-- <li><b>Preference for Admission (Nursery) :</b>
                                    <br />
                                    1. Wards of : Landowners / Shareholders. <b>(*Nanded City bonafide certificate required
                                                    mandatory)</b></br> 2. Wards of : Nanded City Flat owners. <b>(*Bonafide certificate from Nanded City are required)</b></br>
                                                3. Siblings of existing students. <b>(*If all dues are cleared of sibling and if seats are available)</b><br />
                                                <span style="padding-left: 18px; font-weight: bold;">Note: Cousins will not be considered
                                                    as siblings.</span></br> 
                                                    4. If seats remain, Nanded City tenants. <b>(*Bonafide certificate from Nanded City are required)</b></br>
                                                    
                                </li>--%>
                                <li><b>The School will not entertain any correspondence, discussion, telephonic or personal
                                    inquiries regarding the admission process.</b></li>
                                <li>Any intervention or pressure in the normal admission process will lead to immediate
                                    disqualification of the application.</li>
                                <li>Genuine queries only related to admissions can be directed to <a id="aAdmissionMail"
                                    runat="server"></a>. Information received from any other source may not be reliable
                                    and the school will not be responsible for the same.</li>
                                <li>Do not ask admission related queries to the security personnel.</li>
                                <li>The school regrets its inability to address parental inquiries on an individual
                                    basis. All information required is available on the website <a id="aSChoolWebsite"
                                        runat="server" href="#"></a></li>
                                <li id="liRandomAdmission" runat="server"><b><u>Random selection</u></b> (As per above preference) by lottery generated by computer program will be done( Nursery to Std. I).</li>
                                <li><b>Please note that the School Management does not accept donation in any kind whatsoever. Neither does the management authorize any person to do so. </b></li>
                                <li><b>The school does not reserve any seats on any grounds other than the RTE quota, for direct siblings and for Nanded City Citizens. In case any person claims to secure a seat in our school through influence or consideration please bring it to the notice of administration for suitable action.</b></li>
                                <%--<li><b>Please note that the School Management does not accept donation in any kind whatsoever.
                                    Neither does the Management authorize any person to do so. In case any person claims
                                    to secure a seat in our school through influence or consideration please bring it
                                    to the notice of the school authorities for suitable action.</b></li>--%>
                            </ul>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center" class="TxtNormal">
                            <asp:HiddenField ID="hidStandardId" runat="server" />
                            <asp:HiddenField ID="hidAcademicYearId" runat="server" Value="0" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <table align="center" id="tblSupportingDocumentsPPSH" runat="server" visible="false"
                class="paddingLR" cellspacing="1" cellpadding="1" border="0" width="100%">
                <tbody>
                    <tr>
                        <td class="HeadTxtBWOPadding borderBtm" align="left" colspan="4">
                            Admission Form - Instructions
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="TxtNormal" style="width: 135px">
                            <asp:Image ID="Image5" runat="server" Height="1px" ImageUrl="~/images/spacer.gif"
                                Width="150px" />
                        </td>
                        <td align="left">
                        </td>
                        <td align="left" class="TxtNormal">
                            <asp:Image ID="Image6" runat="server" Height="1px" ImageUrl="~/images/spacer.gif"
                                Width="120px" />
                        </td>
                        <td align="left">
                        </td>
                    </tr>
                    <%--<tr id="trPPSHNurseryDocuments" runat="server" visible="false">
                        <td colspan="4">
                            <table width="100%">
                                <tr id="tr2" runat="server">
                                    <td class="HeadTxtBWOPadding borderBtm" style="height: 25px;" align="left" colspan="4">
                                        LIST OF SUPPORTING DOCUMENTS : NURSERY
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="left" class="TextNormalB">
                                        <b>Please bring  original Documents to be produced for verification, along with the supporting documents.</b>
                                    </td>
                                </tr>
                                <tr id="Tr4" runat="server">
                                    <td colspan="4" align="left" class="TxtNormal">
                                        <ol>
                                            <li>Two recent passport size photographs of the student and one photograph each of the
                                                parents.</li>
                                            <li>Original OR Notarised Copy of the Birth Certificate. In case of certificates in
                                                languages other than English, please submit a notarized copy of the certificate
                                                translated in English.</li>
                                            <li>Attested copy of Residence proof as mentioned below:<br />
                                                Residence proof should strictly be in the name of the father, mother or grandparents
                                                of the child seeking admission.<br />
                                                Any ONE of the following documents can be produced as proof of residence:<br />
                                                <ol style="list-style-type: lower-alpha;">
                                                    <li>Latest Electricity Bill</li>
                                                    <li>Bank statement for the current month</li>
                                                    <li>Registered Valid Rent Agreement Copy OR Index II</li>
                                                </ol>
                                            </li>
                                            <li>Medical Certificate provided along with the online Admission Form has to be
                                                completely filled, signed and stamped by a registered medical practitioner (MD/MBBS
                                                Doctor)</li>
                                            <li>Parents from Reserved Category, who want the caste to be entered in the school register
                                                should submit the Caste Certificate issued by the concerned Government Authorities
                                                in the name of the student.</li>
                                            <li>Copy of Student's Aadhaar Card</li>
                                            <li>Copy of Passport (for students traveling from outside India).</li>
                                        </ol>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>--%>
                    <%--<tr id="trPPSHJRKGto1" runat="server" visible = "false">
                        <td colspan="4">
                            <table width="100%">
                                <tr id="tr7" runat="server">
                                    <td class="HeadTxtBWOPadding borderBtm" style="height: 25px;" align="left" colspan="4">
                                        LIST OF SUPPORTING DOCUMENTS : JUNIOR KG TO GRADE I
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="left" class="TextNormalB">
                                        <b>Please bring  original Documents to be produced for verification, along with the supporting documents.</b>
                                    </td>
                                </tr>
                                <tr id="Tr8" runat="server">
                                    <td colspan="4" align="left" class="TxtNormal">
                                        <ol>
                                            <li>Two recent passport size photographs of the student and one photograph each of the
                                                parents.</li>
                                            <li>Attested Copy of the Birth Certificate. In case of certificates in languages other
                                                than English, please submit a notarized copy of the certificate translated in English.</li>
                                            <li>Attested copy of Residence proof as mentioned below:<br />
                                                Residence proof should strictly be in the name of the father, mother or grandparents
                                                of the child seeking admission.<br />
                                                Any ONE of the following documents can be produced as proof of residence:<br />
                                                <ol style="list-style-type: lower-alpha;">
                                                    <li>Latest Electricity Bill</li>
                                                    <li>Bank statement for the current month</li>
                                                    <li>Registered Valid Rent Agreement Copy OR Index II</li>
                                                </ol>
                                            </li>
                                            <li>Medical Certificate provided along with the online Admission Form has to be
                                                completely filled, signed and stamped by a registered medical practitioner (MD/MBBS
                                                Doctor)</li>
                                            <li>Parents from Reserved Category, who want the caste to be entered in the school register
                                                should submit the Caste Certificate issued by the concerned Government Authorities
                                                in the name of the student.</li>
                                            <li>Previous year's final assessment card and mid term assessment card of current class.</li>
                                            <li>Authorised copy of Bonafide Certificate from School.</li>
                                            <li>Copy of Student's Aadhaar Card.</li>
                                            <li>Copy of Passport (for students traveling from outside India).</li>
                                        </ol>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>--%>
                    <tr id="trPPSH2to10" runat="server">
                        <td colspan="4">
                            <table width="100%">
                                <tr id="tr9" runat="server">
                                    <td class="HeadTxtBWOPadding borderBtm" style="height: 25px;" align="left" colspan="4">
                                        LIST OF SUPPORTING DOCUMENTS
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="left" class="TextNormalB">
                                        <b>Please bring  original Documents to be produced for verification, along with the supporting documents.</b>
                                    </td>
                                </tr>
                                <tr id="Tr10" runat="server">
                                    <td colspan="4" align="left" class="TxtNormal">
                                        <ol>
                                            <li>Administrator & Teacher Copy of Admission form. Affix Two recent Photographs of the child.</li>
                                            <li>One Family Photo –Mother, Father & Child; size should be 6”x4” with plain white background.</li>
                                            <li>Copy of Birth Certificate (Notarized)/ Original Birth certificate.</li>
                                            <li>Residence Proof – Index 2/ Rent Agreement/ Electricity Bill</li>
                                            <li>Copy of Mark sheet / Progress Report.</li>
                                            <li>Bonafide Certificate from the existing school mentioning the Udise + ID & Apaar ID if the student is studying in India.</li>
                                            <li>Original Leaving Certificate (Std 1 and above)</li>
                                            <%--<li>Fitness Certificate from Registered Medical Practitioner (MD or MBBS Doctor)</li>--%>
                                            <li>Medical history sheet from registered medical practitioner (MD or MBBS doctor)</li>
                                            <li>Copy of Caste Certificate (If Applicable).</li>
                                            <li>Copy of Passport / PIO card (for students travelling from outside India).</li>
                                            <li>Copy of Student’s & Parents Aadhar Card.</li>
                                            <li>Undertaking of Rules and Regulations</li>
                                            <li>Parent Consent Form</li>
                                        </ol>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="TrPPSHNurseryToGrade1AdmissionProcess" runat="server" visible="false">
                        <td colspan="4">
                           <table>
                            <tr>
                                <td class="HeadTxtBWOPadding borderBtm" style="height: 25px" align="left">
                                    <span id="spnPPSHAdmissionForNurTo1" runat="server">ADMISSION PROCESS FOR NURSERY TO GRADE 1</span>
                                </td>
                            </tr>
                            <tr>
                                <td class="TxtNormal">
                                    <ul>
                                        <li>Forms will be available on our website <a href="#" onclick="OpenPPSHPopup()">https://pawarpublicschool.com/hinjawadi</a> <span runat="server" id="spnAdmissionDate">with effect from  <b>Monday,
                                            6<sup>th</sup> October 2025; 8:30 a.m. onwards.</b> </span></li>
                                        <li>Parents have to fill, take a print, sign and share the signed scanned copy of the admission form with 
                                            required supporting documents to <a href="mailto:admissions@ppshinjewadi.com">
                                            admissions@ppshinjewadi.com</a> for initial verification of
                                            documnets.</li>
                                        <li>Once the document verification is done,<span runat="server" id="spnGrade1"> Parents will receive a mail with scheduled day and time for interaction and to submit the hard copy of documents in person.</span></li>
                                        <li id="liGrade1PPSH" runat="server"> Please provide the receipt of the online payment for the admission form fee, along with the bonafide certificate from the previous school. The bonafide certificate should include the Apaar ID and UDISE +ID of the student, as these are mandatory for further processing.</li>
                                        <li>Kindly carry both the original and photocopy of documents for physical verification of documents.</li>
                                        <li>PTA fees- Cash of amount &#8377; 50/- has to be paid towards Parent Teacher Association at the same time.</li>
                                        <li>School will share the online payment details through mail once all documents and details are thoroughly checked.</li>
                                        <li>Submission of all the documents as per the requirement is mandatory to complete the admission process.</li>
                                        <li>Admission will be confirmed upon receipt of Admission fees,Quarterly fees, and Caution Money Deposit.</li>
                                        <li>Procurement of form is not a guarantee of admission.</li>
                                        <li>Please Note: We have limited seats for each class.Hence admissions will be granted subject to availability of seats.</li>
                                            **Please check the age criteria to confirm the eligibility of the child for admission.
                                        <li>Please note: It is mandatory for your ward to have completed the previous grade.</li>
                                        <li>For Example: A child born between 1st October, 2022 and 31st December, 2022 is applicable for Nursery as well as Jr KG. If you apply for Jr KG it is mandatory to complete Nursery in previous school as well as applicable documents have to be submitted. Same applies for Sr. KG to Grade 1.</li>
                                    </ul>
                                </td>
                            </tr>
                           </table> 
                        </td>
                    </tr>
                    <tr id="TrPPSHGrade2ToGrade9AdmissionProcess" runat="server" visible="false">
                        <td colspan="4">
                            <table>
                                <tr>
                                    <td class="HeadTxtBWOPadding borderBtm" style="height: 25px" align="left">
                                        <span id="spnPPSHAdmissionProcess" runat="server">ADMISSION PROCESS FOR GRADE 2 TO GRADE 9</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TxtNormal">
                                        <ul>
                                            <li>Forms will be available on our website <a href="#" onclick="OpenPPSHPopup()">https://pawarpublicschool.com/hinjawadi</a><span runat="server" id="spn1to5"> with effect from <b>Monday,
                                                8<sup>th</sup> December 2025; 8:30 a.m. onwards.</b> </span></li>
                                            <li>Parents have to fill, take a print, sign and share the signed scanned copy of the admission form with 
                                                required supporting documents to <a href="mailto:admissions@ppshinjewadi.com">
                                                admissions@ppshinjewadi.com</a> for initial verification of
                                                documents. Along with the supporting documents, you are also requested to share the latest evaluation report card.</li>
                                            <%--<li>You are requested to show the receipt of online payment of admission form fee and <b>bonafide
                                                certificate mentioning the U-DISE and Saral ID of the student which is mandatory</b> of the
                                                previous school before proceeding to the activity paper.</li>
                                            <li>There will be an activity paper for students for English, Mathematics & Hindi. The activity paper 
                                                consist of 20 marks based on the previous term syllabus and the duration of paper will be half an hour.</li>
                                            <li>You will receive a mail mentioning the scheduled date and time when your ward will have to appear.</li>
                                            <li>On the same day you will have to submit the hard copy of Admission form & documents.</li>--%>
                                            <li>Once the document verification is done, parents will receive a mail with further admission details.</li>
                                            <li>Please provide the receipt of the online payment for the admission form fee, along with the bonafide certificate from the previous school. The bonafide certificate should include the Apaar ID and UDISE +ID of the student, as these are mandatory for further processing.</li>                                            
                                            <li>Kindly carry both the original and photocopy of documents for physical verification of documents.</li>
                                            <li>PTA fees- Cash of amount &#8377; 50/- has to be paid towards Parent Teacher Association at the same time.</li>
                                            <li>School will share the online payment details through mail once the activity papers are checked and 
                                                the admission form along with the self-attested documents are clear.</li>
                                            <li>Admission will be confirmed upon receipt of Admission fees,Quarterly fees, and Caution Money Deposit.</li>
                                            <li>Procurement of form is not a guarantee of admission.</li>
                                            <li>Please Note: We have limited seats for each class. Hence admissions will be granted subject to availability of seats.</li>
                                                **Please check the age criteria to confirm the eligibility of the child for admission.
                                            <li><b>If a parent, fails to submit all the supporting documents required by the school for granting 
                                                   admission, the school will have right to cancel the admission.</b></li>
                                            <li><b>Mandatory to submit Students Aadhar card to complete admission process.</b></li>
                                            <li><b>Admission to Grade 2 to 9 will be treated as provisional till the receipt of the Original 
                                                   Leaving/ Transfer Certificate from the previous school. The last date for submission  
                                                   is Friday, 26<sup>th</sup> June, 2026.</b></li>
                                        </ul>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <%-- <tr id="tr6" runat="server" visible="true">
                        <td colspan="4">
                            <table>
                                <tr>
                                    <td class="HeadTxtBWOPadding borderBtm" style="height: 25px" align="left" colspan="4">
							            ADMISSION TO OTHER GRADES 
						            </td>
                                </tr>
                                <tr>
                                    <td class="TxtNormal">
                                        <ul>
                                            <li>Parents seeking admissions to Grades other than Nursery may put in their request for admission online and the same will be considered subject to availability of seats.</li>                                            
                                            <li>All Admissions to Higher Grades will be considered provisional till the receipt of Original School Leaving Certificate from the previous school within the stipulated time.</li>
                                            <li>Admissions are open from Grade IV to Grade X.</li>                                            
                                        </ul>
                                    </td>
                                </tr>
                           </table>
                        </td>						
					</tr>--%>
                    <tr>
                        <td class="HeadTxtBWOPadding borderBtm" style="height: 25px" align="left" colspan="4">
                            SPECIAL NOTICE
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="left" class="TxtNormal">
                            <ul>
                                <li><b>The School will not entertain any correspondence, discussion, telephonic or personal
                                    inquiries regarding the admission process.</b></li>
                                <li>Any intervention or pressure in the normal admission process will lead to immediate
                                    disqualification of the application.</li>
                                <li>Any queries related to admissions can be directed to <a href="mailto:admissions@ppshinjewadi.com">
                                    admissions@ppshinjewadi.com</a> mail Id as per the school. All Information required
                                    is available on the website <a href="#" onclick="OpenPPSHPopup()">https://pawarpublicschool.com/hinjawadi</a></li>
                                <li>Admissions will be granted on first come first serve basis.</li>
                                <li><b>Please note that the School Management does not accept donation in any kind whatsoever.
                                    Neither does the Management authorize any person to do so. In case any person claims
                                    to secure a seat in our school through influence or consideration please bring it
                                    to the notice of the school authorities for suitable action.</b></li>
                            </ul>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center" class="TxtNormal">
                            <asp:HiddenField ID="HiddenField1" runat="server" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="col-lg-12" id="DYPVFEESTRUCTURE" runat="server" visible="false">
                <div style="width: 100%; padding-top: 20px; padding-bottom: 20px;">
                    <table style="width: 100%; text-align: center;" cellspacing="0">
                        <tr id="tr6">
                            <td class="HeadTxtBWOPadding borderBtm" style="height: 25px; padding-left: 10px;"
                                align="left" colspan="4">
                                Fee Structure
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                            <td>
                            </td>
                        </tr>
                        <tr align="center" style="text-align: center; margin: 0px auto;">
                            <td align="center" style="text-align: center; margin: 0px auto;">
                                <table align="center" width="70%" style="text-align: center; margin: 0px auto; border: 1px solid;
                                    border-collapse: collapse; font-family: Cambria; font-size: 20px;">
                                    <%--<tr style="border: 1px solid; margin: 0px auto; height: 30px; font-weight: bold;">
                                    <td colspan="10" align="center" style="text-align: center; margin: 0px auto;">
                                        Dr.D.Y Patil Pratishthan's
                                        <br />
                                        D.Y Patil Vidyaniketan School Salokhenagar Kolhapur (CBSE)
                                        <br />
                                        Affilation no - 1131052
                                    </td>
                                </tr>--%>
                                    <tr style="border: 1px solid; height: 30px; font-weight: bold; margin: 0px auto;">
                                        <td colspan="10" align="center" style="text-align: center; border: 1px solid;">
                                            Registration Fees -1000/-
                                        </td>
                                    </tr>
                                    <tr style="border: 1px solid; height: 30px; font-weight: bold; margin: 0px auto;">
                                        <td colspan="10" align="center" style="text-align: center; border: 1px solid;">
                                            Pre Primary(Nursery, JR.KG, SR.KG)
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="border: 1px solid; height: 30px; font-weight: bold; margin: 0px auto;">
                                            Yearly Fees
                                        </td>
                                        <td style="border: 1px solid; height: 30px; font-weight: bold; margin: 0px auto;"
                                            colspan="3">
                                            <table align="center">
                                                <tr style="text-align: center;">
                                                    <td>
                                                        Fees Installment
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                Total Fees
                                            </div>
                                        </td>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                April
                                            </div>
                                        </td>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                July
                                            </div>
                                        </td>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                Oct
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                20,000
                                            </div>
                                        </td>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                10,000
                                            </div>
                                        </td>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                10,000
                                            </div>
                                        </td>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr style="border: 1px solid; height: 20px; font-weight: bold; margin: 0px auto;">
                                        <td colspan="10" align="center" style="text-align: center; border: 1px solid; height: 20px;">
                                        </td>
                                    </tr>
                                    <tr style="border: 1px solid; height: 30px; font-weight: bold; margin: 0px auto;">
                                        <td colspan="10" align="center" style="text-align: center; border: 1px solid;">
                                            Std I to IV
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="border: 1px solid; height: 30px; font-weight: bold; margin: 0px auto;">
                                            Yearly Fees
                                        </td>
                                        <td style="border: 1px solid; height: 30px; font-weight: bold; margin: 0px auto;"
                                            colspan="3">
                                            <table align="center">
                                                <tr style="text-align: center;">
                                                    <td>
                                                        Fees Installment
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                Total Fees
                                            </div>
                                        </td>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                April
                                            </div>
                                        </td>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                July
                                            </div>
                                        </td>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                Oct
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                32,000
                                            </div>
                                        </td>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                16,000
                                            </div>
                                        </td>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                8,000
                                            </div>
                                        </td>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                8,000
                                            </div>
                                        </td>
                                    </tr>
                                    <tr style="border: 1px solid; height: 20px; font-weight: bold; margin: 0px auto;">
                                        <td colspan="10" align="center" style="text-align: center; border: 1px solid; height: 20px;">
                                        </td>
                                    </tr>
                                    <tr style="border: 1px solid; height: 30px; font-weight: bold; margin: 0px auto;">
                                        <td colspan="10" align="center" style="text-align: center; border: 1px solid;">
                                            Std V to VII
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="border: 1px solid; height: 30px; font-weight: bold; margin: 0px auto;">
                                            Yearly Fees
                                        </td>
                                        <td style="border: 1px solid; height: 30px; font-weight: bold; margin: 0px auto;"
                                            colspan="3">
                                            <table align="center">
                                                <tr style="text-align: center;">
                                                    <td>
                                                        Fees Installment
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                Total Fees
                                            </div>
                                        </td>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                April
                                            </div>
                                        </td>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                July
                                            </div>
                                        </td>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                Oct
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                35,000
                                            </div>
                                        </td>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                17,000
                                            </div>
                                        </td>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                9,000
                                            </div>
                                        </td>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                9,000
                                            </div>
                                        </td>
                                    </tr>
                                    <tr style="border: 1px solid; height: 20px; font-weight: bold; margin: 0px auto;">
                                        <td colspan="10" align="center" style="text-align: center; border: 1px solid; height: 20px;">
                                        </td>
                                    </tr>
                                    <tr style="border: 1px solid; height: 30px; font-weight: bold; margin: 0px auto;">
                                        <td colspan="10" align="center" style="text-align: center; border: 1px solid;">
                                            Std VIII
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="border: 1px solid; height: 30px; font-weight: bold; margin: 0px auto;">
                                            Yearly Fees
                                        </td>
                                        <td style="border: 1px solid; height: 30px; font-weight: bold; margin: 0px auto;"
                                            colspan="3">
                                            <table align="center">
                                                <tr style="text-align: center;">
                                                    <td>
                                                        Fees Installment
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                Total Fees
                                            </div>
                                        </td>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                April
                                            </div>
                                        </td>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                July
                                            </div>
                                        </td>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                Oct
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                38,000
                                            </div>
                                        </td>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                20,000
                                            </div>
                                        </td>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                10,000
                                            </div>
                                        </td>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                8,000
                                            </div>
                                        </td>
                                    </tr>
                                    <tr style="border: 1px solid; height: 20px; font-weight: bold; margin: 0px auto;">
                                        <td colspan="10" align="center" style="text-align: center; border: 1px solid; height: 20px;">
                                        </td>
                                    </tr>
                                    <tr style="border: 1px solid; height: 30px; font-weight: bold; margin: 0px auto;">
                                        <td colspan="10" align="center" style="text-align: center; border: 1px solid;">
                                            Std IX
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="border: 1px solid; height: 30px; font-weight: bold; margin: 0px auto;">
                                            Yearly Fees
                                        </td>
                                        <td style="border: 1px solid; height: 30px; font-weight: bold; margin: 0px auto;"
                                            colspan="3">
                                            <table align="center">
                                                <tr style="text-align: center;">
                                                    <td>
                                                        Fees Installment
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                Total Fees
                                            </div>
                                        </td>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                April
                                            </div>
                                        </td>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                July
                                            </div>
                                        </td>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                Oct
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                40,000
                                            </div>
                                        </td>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                20,000
                                            </div>
                                        </td>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                10,000
                                            </div>
                                        </td>
                                        <td align="center" style="text-align: center; margin: 0px auto; border: 1px solid;">
                                            <div class="">
                                                10,000
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <asp:HiddenField ID="hidServerDate" runat="server" />
            <asp:HiddenField ID="hidSchoolId" runat="server" />
            <asp:Button runat="server" ID="btnSubmit" Text="Next" CausesValidation="true" CssClass="ClsButton"
                OnClick="btnSubmit_Click" />
            <b class="rbottom"><b class="r4"></b><b class="r3"></b><b class="r2"></b><b class="r1">
            </b></b>
        </div>
        <br />
        <script language="javascript" type="text/javascript">

            function OpenPopup() {
                _clienthidSchoolId = "<%=this.hidSchoolId.ClientID %>"
                var schoolId = $('#' + _clienthidSchoolId).val()
                if (schoolId == 71) {
                    window.open('https://pawarpublicschool.com/nandedcity/', "_blank")
                }
                else if (schoolId == 18) {
                    window.open('https://pawarpublicschool.com/hadapsar/', "_blank")
                }
            }

            function OpenPPSHPopup() {
                window.open('https://pawarpublicschool.com/hinjawadi/', "_blank")
            }
        </script>
    </div>
</asp:Content>
