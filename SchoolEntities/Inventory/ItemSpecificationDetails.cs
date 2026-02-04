/* -------------------------------------------------------------------------------
 *	DEVELOPMENT LOG
 * -------------------------------------------------------------------------------
 *	Author	: Yogesh Karne
 *	Date	: 1-Jan-2016
 *	Purpose	: We can mark damage specific item.
 * -------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;

namespace SchoolEntities.Inventory
{
    public class ItemSpecificationDetails : SchoolEntity
    {
        public int Id { get; set; }
        public int ItemID { get; set; }
        public string ItemCode { get; set; }
        public int SchoolId { get; set; }
        public string Description { get; set; }
        public string SpecificationCode { get; set; }
        public bool IsDamaged { get; set; }
        public string DamagedDate { get; set; }
        public string DamageDescription { get; set; }
        public bool IsIssued { get; set; }
        public string Price { get; set; }
    }
}
