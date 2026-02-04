using System;

namespace DataCommunicator
{
  public class PublisherDC : DataCommunicatorBaseDC
    {
        public PublisherDC()
        {
        }

        public struct PublisherStructDetails
        {
            public Int32 miSchoolId;            
            public string msPublisherName;
            public Int32 miPublisherId;           
            public Int32 miUser_Id;
            public char msIsDeleted;
            public Int32 miInsertedById;
            public DateTime mdtInsertedDate;
            public Int32 miUpdatedById;
            public DateTime mdtUpdatedDate;
            public Int32 miRowNo;
            
        }
        private PublisherStructDetails moPublisherDetails;

        #region Property
        public PublisherStructDetails PublisherInfo
        {
            get
            {
                return moPublisherDetails;
            }
            set
            {
                moPublisherDetails = value;
            }

        }
        #endregion


    }
}
