namespace SchoolEntities
{

    /// <summary>
    /// This class is used to populate parameters which required for single signon for RITeStore from RITeSchool.
    /// </summary>
    public class RITeStoreUser
    {
        public int SchoolId { get; set; }
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobilePhoneNo { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}
