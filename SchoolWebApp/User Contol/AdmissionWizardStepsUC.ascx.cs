using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Collections;
using System.ComponentModel.Design;
using System.Web.UI.Design;

namespace SchoolWebApp
{
    [ParseChildren(true)]
    [PersistChildren(false)]
    [ToolboxData("<{0}:Gallery runat=server></{0}:Gallery>")]
    public partial class AdmissionWizardSteps : System.Web.UI.UserControl
    {

        public List<Image> moImageList = new List<Image>();
        public Image moImageStep1;
        public Image moImageStep2;
        private int miActiveStep = 1;
        private bool mbEnableFormFee = true;
        private bool mbIsStudentFee = false;

        
        public bool IsStudentFee
        {
            get
            {
                return mbIsStudentFee;
            }
            set
            {
                mbIsStudentFee = value;
            }
        }

        public int ActiveSteps
        {

            get
            {
                return miActiveStep;
            }
            set
            {
                miActiveStep = value;
            }
        }

        public bool EnableFormFee
        {
            get { return mbEnableFormFee; }
            set { mbEnableFormFee = value; }
        }

        private void SetImagesForActiveStep()
        {
            if (!mbIsStudentFee)
            {
                tblAdmission.Visible = true;
                tblStudentFee.Visible = false;
                if (EnableFormFee)
                {
                    switch (ActiveSteps)
                    {
                        case 1: ImageStep1.ImageUrl = "~/images/Enabled1.png";
                            ImageStep2.ImageUrl = "~/images/disabled2.png";
                            ImageStep3.ImageUrl = "~/images/disabled3.png";
                            ImageStep4.ImageUrl = "~/images/disabled4.png";
                            ImageStep5.ImageUrl = "~/images/disabled5.png";
                            break;
                        case 2: ImageStep1.ImageUrl = "~/images/Complete1.png";
                            ImageStep2.ImageUrl = "~/images/Enabled2.png";
                            ImageStep3.ImageUrl = "~/images/disabled3.png";
                            ImageStep4.ImageUrl = "~/images/disabled4.png";
                            ImageStep5.ImageUrl = "~/images/disabled5.png";
                            break;
                        case 3: ImageStep1.ImageUrl = "~/images/Complete1.png";
                            ImageStep2.ImageUrl = "~/images/Complete2.png";
                            ImageStep3.ImageUrl = "~/images/Enabled3.png";
                            ImageStep4.ImageUrl = "~/images/disabled4.png";
                            ImageStep5.ImageUrl = "~/images/disabled5.png";
                            break;
                        case 4: ImageStep1.ImageUrl = "~/images/Complete1.png";
                            ImageStep2.ImageUrl = "~/images/Complete2.png";
                            ImageStep3.ImageUrl = "~/images/Complete3.png";
                            ImageStep4.ImageUrl = "~/images/Enabled4.png";
                            ImageStep5.ImageUrl = "~/images/disabled5.png";
                            break;
                        case 5: ImageStep1.ImageUrl = "~/images/Complete1.png";
                            ImageStep2.ImageUrl = "~/images/Complete2.png";
                            ImageStep3.ImageUrl = "~/images/Complete3.png";
                            ImageStep4.ImageUrl = "~/images/Complete4.png";
                            ImageStep5.ImageUrl = "~/images/Enabled5.png";
                            break;
                    }
                }
                else
                {
                    tdConfirmAmount.Visible = false;
                    tdSelectBank.Visible = false;
                    tdStep3.Visible = false;
                    tdSteps4.Visible = false;
                    tdCompletion.InnerText = "Completion";
                    switch (ActiveSteps)
                    {
                        case 1: ImageStep1.ImageUrl = "~/images/Enabled1.png";
                            ImageStep2.ImageUrl = "~/images/disabled2.png";                            
                            ImageStep5.ImageUrl = "~/images/disabled3.png";
                            break;
                        case 2: ImageStep1.ImageUrl = "~/images/Complete1.png";
                            ImageStep2.ImageUrl = "~/images/Enabled2.png";                            
                            ImageStep5.ImageUrl = "~/images/disabled3.png";
                            break;                        
                        case 5: ImageStep1.ImageUrl = "~/images/Complete1.png";
                            ImageStep2.ImageUrl = "~/images/Complete2.png";                            
                            ImageStep5.ImageUrl = "~/images/Enabled3.png";
                            break;
                    }
                }
            }
            else
            {
                tblAdmission.Visible = false;
                tblStudentFee.Visible = true;
                switch (ActiveSteps)
                {
                    case 1: imgFeeStep1.ImageUrl = "~/images/Enabled1.png";
                        imgFeeStep2.ImageUrl = "~/images/disabled2.png";
                        imgFeeStep3.ImageUrl = "~/images/disabled3.png";
                        imgFeeStep4.ImageUrl = "~/images/disabled4.png";
                        break;
                    case 2: imgFeeStep1.ImageUrl = "~/images/Complete1.png";
                        imgFeeStep2.ImageUrl = "~/images/Enabled2.png";
                        imgFeeStep3.ImageUrl = "~/images/disabled3.png";
                        imgFeeStep4.ImageUrl = "~/images/disabled4.png";
                        break;
                    case 3: imgFeeStep1.ImageUrl = "~/images/Complete1.png";
                        imgFeeStep2.ImageUrl = "~/images/Complete2.png";
                        imgFeeStep3.ImageUrl = "~/images/Enabled3.png";
                        imgFeeStep4.ImageUrl = "~/images/disabled4.png";
                        break;
                    case 4: imgFeeStep1.ImageUrl = "~/images/Complete1.png";
                        imgFeeStep2.ImageUrl = "~/images/Complete2.png";
                        imgFeeStep3.ImageUrl = "~/images/Complete3.png";
                        imgFeeStep4.ImageUrl = "~/images/Enabled4.png";
                        break;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetImagesForActiveStep();
        }
    }
}

