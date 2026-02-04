using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;   
   
namespace House
{  
     [Serializable]  
    public class HouseConfiguration
    {
        public int Id {get;set;}
        public string Name {get;set;}
        public string Color {get;set;}
        public string Motto { get; set; }
        public int IsDeleted {get;set;}
    }

     [Serializable]  
    public class StudentHouseAssignment
    {
        public int HouseId {get;set;}
        public int StudentId {get;set;}
        
    }
}
