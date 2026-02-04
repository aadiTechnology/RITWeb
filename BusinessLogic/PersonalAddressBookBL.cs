// Class Name       :- PersonalAddressBookBL
// Purpose          :- This class is used to manage PersonalAddressBook details.
// Date Of creation :- 8/12/2009
// Author Name      :- Shankar

using System.Data;
using DataCommunicator;

namespace BusinessLogic
{
    public class PersonalAddressBookBL
    {

        private PersonalAddressBookDC.PersonalAddressBookStruct moPersonalAddressBookStruct;

        private PersonalAddressBookDC moPersonalAddressBookDC;

        public PersonalAddressBookBL()
        {
            moPersonalAddressBookDC = new PersonalAddressBookDC();
        }

        public PersonalAddressBookBL(int miPersonalAddressBookId)
        {
            moPersonalAddressBookDC = new PersonalAddressBookDC(miPersonalAddressBookId);
            moPersonalAddressBookStruct = moPersonalAddressBookDC.PersonalAddressBookStructDetails;
        }

        public virtual int PersonalAddressBookId
        {
            get
            {
                return moPersonalAddressBookStruct.miPersonalAddressBookId;
            }
            set
            {
                moPersonalAddressBookStruct.miPersonalAddressBookId = value;
            }
        }

        public virtual int User_Id
        {
            get
            {
                return moPersonalAddressBookStruct.miUserId;
            }
            set
            {
                moPersonalAddressBookStruct.miUserId = value;
            }
        }

        public virtual string Name
        {
            get
            {
                return moPersonalAddressBookStruct.msName;
            }
            set
            {
                moPersonalAddressBookStruct.msName = value;
            }
        }

        public virtual string Mobile_No
        {
            get
            {
                return moPersonalAddressBookStruct.msMobileNo;
            }
            set
            {
                moPersonalAddressBookStruct.msMobileNo = value;
            }
        }

        public virtual bool Is_Deleted
        {
            get
            {
                return moPersonalAddressBookStruct.mblnIsDeleted;
            }
            set
            {
                moPersonalAddressBookStruct.mblnIsDeleted = value;
            }
        }

        public virtual System.DateTime Insert_Date
        {
            get
            {
                return moPersonalAddressBookStruct.mdtInsertDate;
            }
            set
            {
                moPersonalAddressBookStruct.mdtInsertDate = value;
            }
        }

        public virtual int Inserted_By_id
        {
            get
            {
                return moPersonalAddressBookStruct.miInsertedByid;
            }
            set
            {
                moPersonalAddressBookStruct.miInsertedByid = value;
            }
        }

        public virtual System.DateTime Update_Date
        {
            get
            {
                return moPersonalAddressBookStruct.mdtUpdateDate;
            }
            set
            {
                moPersonalAddressBookStruct.mdtUpdateDate = value;
            }
        }

        public virtual int Updated_By_Id
        {
            get
            {
                return moPersonalAddressBookStruct.miUpdatedById;
            }
            set
            {
                moPersonalAddressBookStruct.miUpdatedById = value;
            }
        }

        public virtual int InsertPersonalAddressBook()
        {
            moPersonalAddressBookDC.PersonalAddressBookStructDetails = moPersonalAddressBookStruct;
            return moPersonalAddressBookDC.InsertPersonalAddressBook();
        }

        public virtual void UpdatePersonalAddressBook()
        {
            moPersonalAddressBookDC.PersonalAddressBookStructDetails = moPersonalAddressBookStruct;
            moPersonalAddressBookDC.UpdatePersonalAddressBook();
        }

        public virtual void DeletePersonalAddressBook()
        {
            moPersonalAddressBookDC.PersonalAddressBookStructDetails = moPersonalAddressBookStruct;
            moPersonalAddressBookDC.DeletePersonalAddressBook();
        }


        public DataTable GetAddressBookList(int aiUserId)
        {
            return moPersonalAddressBookDC.GetAddressBookList(aiUserId);
        }

        public DataTable GetAddressBookGroupList(int aiUserId, string asGroupMob)
        {
            return moPersonalAddressBookDC.GetAddressBookGroupList(aiUserId, asGroupMob);
        }

        public DataTable GetAddressBookGroupDetails(int aiUserId, int aiGroupID)
        {
            return moPersonalAddressBookDC.GetAddressBookGroupDetails(aiUserId, aiGroupID);
        }

        public string CheckIfAlreadyExists()
        {
            moPersonalAddressBookDC.PersonalAddressBookStructDetails = moPersonalAddressBookStruct;
            return moPersonalAddressBookDC.CheckIfAlreadyExists();
        }

        public string CheckIfGroupAlreadyExists(int aiPersonalBookGroupId, string asGroupName, int aiUserId)
        {
            return moPersonalAddressBookDC.CheckIfGroupAlreadyExists(aiPersonalBookGroupId, asGroupName, aiUserId);
        }

        public void UpdatePersonalAddressBookGroup(int aiGroupID, string asGroupName, string asGroupDetailXML, int aiUserId)
        {
            moPersonalAddressBookDC.UpdatePersonalAddressBookGroup(aiGroupID, asGroupName, asGroupDetailXML, aiUserId);
        }

        public void InsertPersonalAddressBookGroup(string asGroupName, string asGroupDetailXML, int aiUserId)
        {
            moPersonalAddressBookDC.InsertPersonalAddressBookGroup(asGroupName, asGroupDetailXML, aiUserId);
        }

        public DataTable GetDetailsOfGroups(string asGroupIds)
        {
            return moPersonalAddressBookDC.GetDetailsOfGroups(asGroupIds);
        }

        public void DeletePersonalAddressBookGroup(int aiPersonalAddressBookGroupId, int aiUserId)
        {
            moPersonalAddressBookDC.DeletePersonalAddressBookGroup(aiPersonalAddressBookGroupId, aiUserId);
        }
    }
}
