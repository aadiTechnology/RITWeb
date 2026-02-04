// Class Name       :- SchoolwiseEventDescriptionBL
// Purpose          :- This class is used to manage SchoolwiseEventDescription details.
// Date Of creation :- 6/18/2008
// Author Name      :- Anu


using System;
using System.Data;
using System.Collections;
using DataCommunicator;
using Utility;
using System.Collections.Generic;
using SchoolEntities;

namespace BusinessLogic
{


    public class SchoolEventBL
    {
        #region Data Members
        private EventDescriptionDC.EventDescriptionStruct moEventDescriptionStruct;
        private EventDescriptionDC moEventDescriptionDC;
        #endregion

        #region Constructors
        public SchoolEventBL()
        {
            moEventDescriptionDC = new EventDescriptionDC();
        }
        public SchoolEventBL(int aiSchoolId,int aiUserId,int aiAcademicYearId)
        {
            this.moEventDescriptionDC = new EventDescriptionDC(aiSchoolId, aiUserId, aiAcademicYearId);
        }
        public SchoolEventBL(int miEventId)
        {
            moEventDescriptionDC = new EventDescriptionDC(miEventId);
            moEventDescriptionStruct = moEventDescriptionDC.EventDescriptionStructDetails;
        }
        #endregion

        #region Properties
        public int Event_Id
        {
            get
            {
                return moEventDescriptionStruct.miEventId;
            }
            set
            {
                moEventDescriptionStruct.miEventId = value;
            }
        }

        public string Event_Description
        {
            get
            {
                return moEventDescriptionStruct.msEventDescription;
            }
            set
            {
                moEventDescriptionStruct.msEventDescription = value;
            }
        }

        public System.DateTime Event_Start_Date
        {
            get
            {
                return moEventDescriptionStruct.mdtEventStartDate;
            }
            set
            {
                moEventDescriptionStruct.mdtEventStartDate = value;
            }
        }

        public System.DateTime Event_End_Date
        {
            get
            {
                return moEventDescriptionStruct.mdtEventEndDate;
            }
            set
            {
                moEventDescriptionStruct.mdtEventEndDate = value;
            }
        }

        public int Display_On_Homepage
        {
            get
            {
                return moEventDescriptionStruct.miDisplayOnHomepage;
            }
            set
            {
                moEventDescriptionStruct.miDisplayOnHomepage=value;
            }
        }

        public int School_Id
        {
            get
            {
                return moEventDescriptionStruct.miSchoolId;
            }
            set
            {
                moEventDescriptionStruct.miSchoolId = value;
            }
        }

        public int Schoolwise_Academic_Year_Id
        {
            get
            {
                return moEventDescriptionStruct.miSchoolwiseAcademicYearId;
            }
            set
            {
                moEventDescriptionStruct.miSchoolwiseAcademicYearId = value;
            }
        }

        public string Is_Deleted
        {
            get
            {
                return moEventDescriptionStruct.msIsDeleted;
            }
            set
            {
                moEventDescriptionStruct.msIsDeleted = value;
            }
        }

        public System.DateTime Insert_Date
        {
            get
            {
                return moEventDescriptionStruct.mdtInsertDate;
            }
            set
            {
                moEventDescriptionStruct.mdtInsertDate = value;
            }
        }

        public int Inserted_By_id
        {
            get
            {
                return moEventDescriptionStruct.miInsertedByid;
            }
            set
            {
                moEventDescriptionStruct.miInsertedByid = value;
            }
        }

        public System.DateTime Update_Date
        {
            get
            {
                return moEventDescriptionStruct.mdtUpdateDate;
            }
            set
            {
                moEventDescriptionStruct.mdtUpdateDate = value;
            }
        }

        public int Updated_By_Id
        {
            get
            {
                return moEventDescriptionStruct.miUpdatedById;
            }
            set
            {
                moEventDescriptionStruct.miUpdatedById = value;
            }
        }

        public string Event_Photo
        {
            get
            {
                return moEventDescriptionStruct.msEventImageName;
            }
            set
            {
                moEventDescriptionStruct.msEventImageName = value;
            }
        }

        public string Event_Comments
        {
            get
            {
                return moEventDescriptionStruct.msEventComments;
            }
            set
            {
                moEventDescriptionStruct.msEventComments = value;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// This method is used to save file details
        /// </summary>
        /// <param name="asLinkUrl"></param>
        public void SaveFileDetails(string asLinkUrl)

        {
            moEventDescriptionDC.SaveFileDetails(asLinkUrl);
        }
        /// <summary>
        /// This method is used to get file details
        /// </summary>
        /// <returns></returns>
        public string GetFileDetails()
        {
            return moEventDescriptionDC.GetFileDetails();
        }

        /// <summary>
        /// This method is used to delete file delete file details
        /// </summary>
        public void DeleteFileDetails()
        {
            moEventDescriptionDC.DeleteFileDetails();

        }

        /// <summary>
        /// This method is used to get events by giving count value.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiCountValue"></param>
        /// <returns></returns>
        public static DataTable GetEventsByCountValue(int aiSchoolId, int aiAcademicYrId, int aiCountValue)
        {
           return EventDescriptionDC.GetEventsByCountValue(aiSchoolId, aiAcademicYrId, aiCountValue);
        }

        /// <summary>
        /// This method is used to insert event description to database.
        /// </summary>
        public void InsertEventDescription(ArrayList oarrStdLst)
        {
            moEventDescriptionDC.EventDescriptionStructDetails = moEventDescriptionStruct;
            moEventDescriptionDC.InsertEventDescription(oarrStdLst);
        }

        /// <summary>
        /// This function is used to update the SchoolwiseEventDescription Details of a particular event.
        /// </summary>
        public void UpdateEventDescription(ArrayList oarrStdLst)
        {
            moEventDescriptionDC.EventDescriptionStructDetails = moEventDescriptionStruct;
            moEventDescriptionDC.UpdateEventDescription(oarrStdLst);
        }

        /// <summary>
        /// This method is used to delete the SchoolwiseEventDescription Details of a particular event.
        /// </summary>
        public void DeleteSchoolwiseEventDescription()
        {
            moEventDescriptionDC.EventDescriptionStructDetails = moEventDescriptionStruct;
            moEventDescriptionDC.DeleteSchoolwiseEventDescription();
        }

        /// <summary>
        /// This method is used to delete the Event Image from database.
        /// </summary>
        public void DeleteEventImage()
        {
            moEventDescriptionDC.EventDescriptionStructDetails = moEventDescriptionStruct;
            moEventDescriptionDC.DeleteEventImage();
        }

         /// <summary>
        /// This method is used to get event description for a particular date.
        /// </summary>
        /// <param name="adtEventDate"></param>
        /// <returns></returns>
        public DataTable GetEventDescription(DateTime adtEventDate, Int32 aiSchoolId , Int32 aiAcademicYrId,Int32 aiStandardId, Int32 aiDivisionId)
        {
            return moEventDescriptionDC.GetEventDescription(adtEventDate , aiSchoolId , aiAcademicYrId,aiStandardId, aiDivisionId);
        }

        /// <summary>
        /// This method is used to get events data for particular month.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiMonthId"></param>
        /// <param name="aiYear"></param>
        /// <returns></returns>
        public DataTable GetEventsData(int aiSchoolId, int aiAcademicYrId, int aiMonthId, int aiYear,int aiStdId, int aiDivisionId)
        {
            return moEventDescriptionDC.GetEventsData(aiSchoolId, aiAcademicYrId, aiMonthId, aiYear , aiStdId, aiDivisionId);
        }

        /// <summary>
        /// This method is used to get events data for particular month.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiMonthId"></param>
        /// <param name="aiYear"></param>
        /// <returns></returns>
        public DataTable GetEventsData(int aiSchoolId, int aiAcademicYrId, int aiMonthId, int aiYear)
        {
            return moEventDescriptionDC.GetEventsData(aiSchoolId, aiAcademicYrId, aiMonthId, aiYear);
        }

         /// <summary>
        /// This method is used to get events data for particular month.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiMonthId"></param>
        /// <param name="aiYear"></param>
        /// <returns></returns>
        public DataTable GetEventsDataForStudent(int aiSchoolId, int aiAcademicYrId, int aiMonthId, int aiYear, int aiStudentId)
        {
            return moEventDescriptionDC.GetEventsDataForStudent(aiSchoolId, aiAcademicYrId, aiMonthId, aiYear, aiStudentId);
        }

        /// <summary>
        /// This method is used to check duplicate holiday name. 
        /// </summary>
        /// <param name="asHolidayName"></param>
        /// <returns></returns>
        public bool IsEventNameDuplicate()
        {
            moEventDescriptionDC.EventDescriptionStructDetails = moEventDescriptionStruct;
            Int32 iEventNameCount = moEventDescriptionDC.CheckForDuplicateEvent();//asHolidayName,aiSchoolId,aiAcademicYearId,aiHolidayId);
            if (iEventNameCount != Constants.I_ZERO)
                return true;
            else
                return false;
        }

         /// <summary>
        /// This method is used to get associated standards list for a given event.
        /// </summary>
        /// <param name="aiEventId"></param>
        /// <returns></returns>
        public static DataTable GetAssociatedStdLst(int aiEventId)
        {
            return EventDescriptionDC.GetAssociatedStdLst(aiEventId);
        }

		/// <summary>
		/// Returns all the Events of the School.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <param name="aiStandardId"></param>
		/// <param name="aiMonthId"></param>
		/// <returns></returns>
		public static List<Event> GetAllEvents(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiMonthId)
		{
			return EventDescriptionDC.GetAllEvents(aiSchoolId, aiAcademicYearId, aiStandardId, aiMonthId);
		}

        /// <summary>
        /// Returns all the Events of the School.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiMonthId"></param>
        /// <returns></returns>
        public static List<Event> GetAllEvents(int aiSchoolId)
        {
            return EventDescriptionDC.GetAllEvents(aiSchoolId);
        }

        /// <summary>
        /// This method is used to return top events.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiTopCount"></param>
        /// <returns></returns>
        public static List<Event> GetAllTopEvents(int aiSchoolId, int aiTopCount)
        {
            return EventDescriptionDC.GetAllTopEvents(aiSchoolId, aiTopCount);
        }

        public List<EventDetails> GetSelectedEvents(int aiSchoolId)
        {
            return this.moEventDescriptionDC.GetSelectedEvents(aiSchoolId);
        }


        #endregion
    }

    public class EventDescriptionCollectionBL
    {

        // This function is used to Fetch the SchoolwiseEventDescription Details
        static DataSet FetchSchoolwiseEventDescriptionDetails()
        {
            return EventDescriptionCollectionDC.FetchSchoolwiseEventDescriptionDetails();
        }
    }


 


}
