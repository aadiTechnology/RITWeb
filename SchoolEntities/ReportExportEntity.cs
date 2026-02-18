using System.Runtime.Serialization;
namespace SchoolEntities
{
    [DataContract]
    public class ParameterPair
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Value { get; set; }
    }
}
