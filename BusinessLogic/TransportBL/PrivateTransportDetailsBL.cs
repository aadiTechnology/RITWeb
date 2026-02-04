using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator;

namespace BusinessLogic
{
   public class PrivateTransportDetailsBL
    {
        private PrivateTransportDetailsDC moPrivateTransportDetailsDC = new PrivateTransportDetailsDC();

        private PrivateTransportDetailsDC.PrivateTransportDetails moPrivateTransportDetailsBL = new PrivateTransportDetailsDC.PrivateTransportDetails();


        public PrivateTransportDetailsBL()
        {
        }

        public PrivateTransportDetailsBL(int aiPrivateTransportDetailsId,int aiSchoolId,int aiAcademicYearId)
        {
            moPrivateTransportDetailsDC = new PrivateTransportDetailsDC(aiPrivateTransportDetailsId, aiSchoolId, aiAcademicYearId);
            moPrivateTransportDetailsBL = moPrivateTransportDetailsDC.TransportDetails;
        }

        public int PrivateTransportDetailsId
        {
            get { return moPrivateTransportDetailsBL.PrivateTransportDetailsId; }
            set { moPrivateTransportDetailsBL.PrivateTransportDetailsId = value; }
        }

        public int UserId
        {
            get { return moPrivateTransportDetailsBL.UserId; }
            set { moPrivateTransportDetailsBL.UserId = value; }
        }

        public string StopName
        {
            get { return moPrivateTransportDetailsBL.StopName; }
            set { moPrivateTransportDetailsBL.StopName = value; }
        }

        public string UserName
        {
            get { return moPrivateTransportDetailsBL.UserName; }
            set { moPrivateTransportDetailsBL.UserName = value; }
        }

        public string VehicleNumber
        {
            get { return moPrivateTransportDetailsBL.VehicleNumber; }
            set { moPrivateTransportDetailsBL.VehicleNumber = value; }
        }

        public string VehicleType
        {
            get { return moPrivateTransportDetailsBL.VehicleType; }
            set { moPrivateTransportDetailsBL.VehicleType = value; }
        }

        public string TransportStaff1
        {
            get { return moPrivateTransportDetailsBL.TransportStaff1; }
            set { moPrivateTransportDetailsBL.TransportStaff1 = value; }
        }

        public string TransportStaff2
        {
            get { return moPrivateTransportDetailsBL.TransportStaff2; }
            set { moPrivateTransportDetailsBL.TransportStaff2 = value; }
        }

        public string MobileNo1
        {
            get { return moPrivateTransportDetailsBL.MobileNo1; }
            set { moPrivateTransportDetailsBL.MobileNo1 = value; }
        }

        public string MobileNo2
        {
            get { return moPrivateTransportDetailsBL.MobileNo2; }
            set { moPrivateTransportDetailsBL.MobileNo2 = value; }
        }

        public int SchoolId
        {
            get { return moPrivateTransportDetailsBL.SchoolId; }
            set { moPrivateTransportDetailsBL.SchoolId = value; }
        }

        public int AcademicYearId
        {
            get { return moPrivateTransportDetailsBL.AcademicYearId; }
            set { moPrivateTransportDetailsBL.AcademicYearId = value; }
        }

        public int Is_Deleted
        {
            get { return moPrivateTransportDetailsBL.Is_Deleted; }
            set { moPrivateTransportDetailsBL.Is_Deleted = value; }
        }

        public int InsertedById
        {
            get { return moPrivateTransportDetailsBL.InsertedById; }
            set { moPrivateTransportDetailsBL.InsertedById = value; }
        }

        public static List<PrivateTransportDetailsDC.PrivateTransportDetails> GetTravelersList(int aiSchoolId, int aiAcademicYearId, int aiStandardId,
                                                                                               int aiDivisionId, string asUserName, string sortExpression, int maximumRows,
                                                                                               int startRowIndex)
        {
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            return PrivateTransportDetailsDC.GetTravelersList(aiSchoolId, aiAcademicYearId, aiStandardId, aiDivisionId, asUserName.Trim() , sortExpression, iEndIndex, startRowIndex);
        }

        public static int GetTravelersListCount(int aiSchoolId, int aiAcademicYearId, int aiStandardId,
                                                                                                int aiDivisionId,string asUserName, string sortExpression, int maximumRows,
                                                                                                int startRowIndex)
        {
            return PrivateTransportDetailsDC.GetTravelersListCount(aiSchoolId, aiAcademicYearId, aiStandardId, aiDivisionId,asUserName.Trim() , sortExpression, maximumRows, startRowIndex);
        }

        public void Insert()
        {
            moPrivateTransportDetailsDC.TransportDetails = moPrivateTransportDetailsBL;
            moPrivateTransportDetailsDC.Insert();
        }

        public void Update()
        {
            moPrivateTransportDetailsDC.TransportDetails = moPrivateTransportDetailsBL;
            moPrivateTransportDetailsDC.Update();
        }

        public static void Delete(int iPrivateTransportDetailsId)
        {
            PrivateTransportDetailsDC.Delete(iPrivateTransportDetailsId); 
        }
    }
}
