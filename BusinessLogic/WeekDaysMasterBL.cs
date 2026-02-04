// File Name    : WeekDaysMasterBL.cs
// Created By   : Ketan     
//Created Date  : 27/11/2007      

using System;
using System.Collections;
using System.Data;
using System.Collections.ObjectModel;

using Utility;
using DataCommunicator;

namespace BusinessLogic
{
    /// <summary>
    /// This class is used to performed insert and delete opertation on Weekdays_Master.  
    /// </summary>
    public class WeekDaysMasterBL
    {
        #region Data members
        private WeekDaysMasterDC.WeekDaysMasterStruct moWeekDaysMasterStruct;
        private WeekDaysMasterDC moWeekDaysMasterDC = new WeekDaysMasterDC();
        private Constants.Action eAction;
        #endregion

        #region Properties

        public int AcademicYearId
        {
            get
            {
                return moWeekDaysMasterStruct.miAcademicYearId;
            }
            set
            {
                moWeekDaysMasterStruct.miAcademicYearId = value;
            }
        }

        public int WeekDaysId
        {
            get
            {
                return moWeekDaysMasterStruct.miWeekDaysId;
            }
            set
            {
                moWeekDaysMasterStruct.miWeekDaysId = value;
            }
        }

        public int SchoolId
        {
            get
            {
                return moWeekDaysMasterStruct.miSchoolId;
            }
            set
            {
                moWeekDaysMasterStruct.miSchoolId = value;
            }
        }

        public string WeekDayName
        {
            get
            {
                return moWeekDaysMasterStruct.msWeekDayName;
            }
            set
            {
                moWeekDaysMasterStruct.msWeekDayName = value;
            }
        }

        public string WeekDayShortName
        {
            get
            {
                return moWeekDaysMasterStruct.msWeekDayShortName;
            }
            set
            {
                moWeekDaysMasterStruct.msWeekDayShortName = value;
            }
        }

        public int OriginalWeekDaysId
        {
            get
            {
                return moWeekDaysMasterStruct.miOriginalWeekDaysId;
            }
            set
            {
                moWeekDaysMasterStruct.miOriginalWeekDaysId = value;
            }
        }

        public string IsDeleted
        {
            get
            {
                return moWeekDaysMasterStruct.msIsDeleted;
            }
            set
            {
                moWeekDaysMasterStruct.msIsDeleted = value;
            }
        }

        public DateTime InsertDate
        {
            get
            {
                return moWeekDaysMasterStruct.mdtInsertDate;
            }
            set
            {
                moWeekDaysMasterStruct.mdtInsertDate = value;
            }
        }

        public int InsertedByid
        {
            get
            {
                return moWeekDaysMasterStruct.miInsertedByid;
            }
            set
            {
                moWeekDaysMasterStruct.miInsertedByid = value;
            }
        }

        public DateTime UpdateDate
        {
            get
            {
                return moWeekDaysMasterStruct.mdtUpdateDate;
            }
            set
            {
                moWeekDaysMasterStruct.mdtUpdateDate = value;
            }
        }

        public int UpdatedById
        {
            get
            {
                return moWeekDaysMasterStruct.miUpdatedById;
            }
            set
            {
                moWeekDaysMasterStruct.miUpdatedById = value;
            }
        }

        public Constants.Action ConfigurationAction
        {
            get
            {
                return eAction;
            }
            set
            {
                eAction = value;
            }
        }

        #endregion

        #region enumData
        //     public enum Action
        //     {
        //         Insert,
        //         Update,
        //         Delete
        //     };

        //     public Action WeekDayConfiguration
        //     {
        //         get
        //         {
        //             return eAction;
        //         }

        //         set
        //         {
        //             eAction = value;
        //         }
        //     }
        #endregion

        #region Constructors

        public WeekDaysMasterBL()
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is used to retrive insert statement.
        /// </summary>
        /// <returns></returns>
        public string InsertStatmentWeekDaysMaster()
        {
            moWeekDaysMasterDC.WeekDaysMasterStructDetails = moWeekDaysMasterStruct;
            return moWeekDaysMasterDC.InsertWeekDaysMaster();
        }
        /// <summary>
        /// This method is used to retrive delete statement.
        /// </summary>
        /// <returns></returns>
        public string DeleteStatmentWeekDaysMaster()
        {

            moWeekDaysMasterDC.WeekDaysMasterStructDetails = moWeekDaysMasterStruct;
            return moWeekDaysMasterDC.DeleteWeekDaysMaster();
        }
        /// <summary>
        /// This statement is used toretrive update statement.
        /// </summary>
        /// <returns></returns>
        public string UpdateStatementWeekDaysMaster()
        {
            moWeekDaysMasterDC.WeekDaysMasterStructDetails = moWeekDaysMasterStruct;
            return moWeekDaysMasterDC.UpdateWeekDaysMaster();
        }

        /// <summary>
        /// This method is used to retrives data from Weekdays_Master.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public DataTable GetAllWeekDayConfigurationDetalis(int aiSchoolId, int aiAcademicYearId)
        {
            return moWeekDaysMasterDC.GetAllWeekDayConfiguration(aiSchoolId, aiAcademicYearId);
        }
        public DataTable GetConfiguredWeekDays(int aiSchoolId, int aiAcademicYearId)
        {
            return moWeekDaysMasterDC.GetConfiguredWeekDays(aiSchoolId, aiAcademicYearId);
        }


        public bool IsWeekdayConfigure(int aiSchoolId, int aiAcademicYearId)
        {
            return moWeekDaysMasterDC.CheckWeekdayConfigureOrNot(aiSchoolId, aiAcademicYearId);
        }
        public static bool IsDateWeekday(int aiSchoolId, int aiAcademicYearId, DateTime aoDt)
        {
            return WeekDaysMasterDC.IsDateWeekday(aiSchoolId, aiAcademicYearId, aoDt);
        }

        #endregion
    }

    /// <summary>
    /// This collection class is used to update all weekdays configuration details. 
    /// </summary>
    public class WeekDaysConfigCollectionBL : IEnumerable
    {
        #region DataMember
        private Collection<WeekDaysMasterBL> moWeekdaysConfigListBL = null;
        WeekDaysMasterCollectionDC oWeekDaysMasterCollectionDC;
        #endregion

        #region Properties
        public Collection<WeekDaysMasterBL> WeekdaysConfigListBL
        {
            get
            {
                return moWeekdaysConfigListBL;
            }
            set
            {
                moWeekdaysConfigListBL = value;
            }
        }
        #endregion

        #region Constructor

        public WeekDaysConfigCollectionBL()
        {
            moWeekdaysConfigListBL = new Collection<WeekDaysMasterBL>();
            oWeekDaysMasterCollectionDC = new WeekDaysMasterCollectionDC();
        }
        #endregion

        #region Public Method

        /// <summary>
        /// This method is used to add collection data.
        /// </summary>
        /// <param name="aoWeekDaysMasterBL"></param>
        public void Add(WeekDaysMasterBL aoWeekDaysMasterBL)
        {
            moWeekdaysConfigListBL.Add(aoWeekDaysMasterBL);
        }

        /// <summary>
        /// This method is used to remove collection data.
        /// </summary>
        /// <param name="aoWeekDaysMasterBL"></param>
        public void Remove(WeekDaysMasterBL aoWeekDaysMasterBL)
        {
            moWeekdaysConfigListBL.Remove(aoWeekDaysMasterBL);

        }

        public IEnumerator GetEnumerator()
        {
            return new WeekDaysCollectionEnumerator(this);
        }
        /// <summary>
        /// This method calls a function to check the RI dependencies for standards that are to be deleted
        /// </summary>
        /// <param name="aoStandards"></param>
        /// <param name="aiAcadId"></param>
        /// <returns></returns>
        private string CheckWeekDayDependencies(Collection<WeekDaysMasterBL> aoWeekdays, int aiAcadYrId)
        {
            //get the id and name of the standards to be deleted into hashtable.
            GenericReferenceList<WeekDaysMasterBL> objDivRefereces = new GenericReferenceList<WeekDaysMasterBL>(moWeekdaysConfigListBL, aiAcadYrId);
            return objDivRefereces.CheckDependencies("WeekDaysId", "WeekDayName", "ConfigurationAction", Constants.ReferenceId.WeekDays, false);

        }
        /// <summary>
        /// This method is used to update all weekdays configuration details.
        /// </summary>
        public void UpdateAllWeekDayConfigurationDetails(int aiAcadYrId)
        {

            string sMessage = CheckWeekDayDependencies(moWeekdaysConfigListBL, aiAcadYrId);
            if (string.IsNullOrEmpty(sMessage))
            {
                IEnumerator oEnum = moWeekdaysConfigListBL.GetEnumerator();
                ArrayList oArrayListInsertWeekDay = new ArrayList();
                while (oEnum.MoveNext())
                {

                    WeekDaysMasterBL oWeekDaysMasterBL = (WeekDaysMasterBL)oEnum.Current;
                    switch (oWeekDaysMasterBL.ConfigurationAction)
                    {
                        case Constants.Action.Insert:
                            oArrayListInsertWeekDay.Add(((WeekDaysMasterBL)oEnum.Current).InsertStatmentWeekDaysMaster());
                            break;
                        case Constants.Action.Delete:
                            oArrayListInsertWeekDay.Add(((WeekDaysMasterBL)oEnum.Current).DeleteStatmentWeekDaysMaster());
                            break;
                        case Constants.Action.Update:
                            oArrayListInsertWeekDay.Add(((WeekDaysMasterBL)oEnum.Current).UpdateStatementWeekDaysMaster());
                            break;
                    }
                }
                oWeekDaysMasterCollectionDC.UpdateWeekDaysConfiguration(oArrayListInsertWeekDay);
            }
            else
            {
                throw new Exceptions.ReferenceExceptions(sMessage);
            }


        }
        #endregion

        private class WeekDaysCollectionEnumerator : IEnumerator
        {
            #region DataMember
            private int position = -1;
            private WeekDaysConfigCollectionBL moWeekDaysCollection;
            #endregion

            #region Constructor
            public WeekDaysCollectionEnumerator(WeekDaysConfigCollectionBL aoWeekDaysCollection)
            {
                moWeekDaysCollection = aoWeekDaysCollection;
            }
            #endregion

            #region Public Method
            // Declare the MoveNext method required by IEnumerator:
            public bool MoveNext()
            {
                if (position < moWeekDaysCollection.moWeekdaysConfigListBL.Count - 1)
                {
                    position++;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            // Declare the Reset method required by IEnumerator:
            public void Reset()
            {
                position = -1;
            }
            #endregion

            #region Property
            // Declare the Current property required by IEnumerator:
            public object Current
            {
                get
                {
                    return moWeekDaysCollection.moWeekdaysConfigListBL[position];
                }
            }
            #endregion
        }
    }
}

