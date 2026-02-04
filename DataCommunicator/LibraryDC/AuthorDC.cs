using System;

namespace DataCommunicator
{
   public class AuthorDC : DataCommunicatorBaseDC
    {
        public AuthorDC()
        {
        }
        public struct AuthorStructDetails
        {
            public Int32 miSchoolId;            
            public string msAuthorName;
            public Int32 miAuthorId;           
            public Int32 miUser_Id;
            public char msIsDeleted;
            public Int32 miInsertedById;
            public DateTime mdtInsertedDate;
            public Int32 miUpdatedById;
            public DateTime mdtUpdatedDate;
            public Int32 miRowNo;
            
        }
        private AuthorStructDetails moAuthorDetails;

        #region Property
        public AuthorStructDetails AuthorInfo
        {
            get
            {
                return moAuthorDetails;
            }
            set
            {
                moAuthorDetails = value;
            }

        }
        #endregion


    }
}
