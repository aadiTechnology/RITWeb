using System.Collections.Generic;

namespace SchoolEntities.Common.Recaptcha
{
    public class RecaptchaResponse
    {
        public bool success { get; set; }
        public float score { get; set; }
        public string action { get; set; }
        public string challenge_ts { get; set; }
        public string hostname { get; set; }
        public List<string> error_codes { get; set; }
    }
}
