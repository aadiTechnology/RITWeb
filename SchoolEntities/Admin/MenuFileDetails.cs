using System;
using System.Collections.Generic;

namespace SchoolEntities
{
	/// <summary>
	///		Represents a Menu, that is displayed at the top of the page.
	/// </summary>
	public class Menu : SchoolEntity
	{
		public int Id { get; set; }
		public Menu ParentMenu { get; set; }
        public Menu SubMenu { get; set; }
		public string Name { get; set; }
		public string Content { get; set; }
		public int Priority { get; set; }
		public DateTime EndDate { get; set; }
		public bool IsMenuItem { get; set; }
		public bool IsExternal { get; set; }
		public bool IsDefault { get; set; }
		public bool IsActive { get; set; }
		public bool IsOnPopup { get; set; }
        public Menu ChildMenu { get; set; }
     
		// References all the files that are attached to the menu.
		public List<MenuFile> MenuFiles { get; set; }
	}

	/// <summary>
	///		Represents a File, that can be attached to a Menu.
	/// </summary>
	public class MenuFile : SchoolEntity
	{
		public int Id { get; set; }
		public Menu Menu { get; set; }
		public string Name { get; set; }
		public string Path { get; set; }
        public int TotalRows { get; set; }
        public bool IsURL { get; set; }
      
	}

    public class CounsellorMenu : SchoolEntity
    {
        public int ConfigureMenuId { get; set; }
        public string ConfigureMenuName { get; set; }       
    }

    public class StudentsCornerMenu : SchoolEntity
    {
        public int ConfigureMenuId { get; set; }
        public string ConfigureMenuName { get; set; }
    }

    public class NewsLetterDetails : SchoolEntity
    {
        public int ConfigureMenuId { get; set; }
        public string ConfigureMenuName { get; set; }
        public string FilePath { get; set; }
        

    }
}

//public class MenuFileDetails : SchoolEntity
//{
//    public int MenuFileDetailsId { get; set; }
//    public int MenuId { get; set; }
//    public string ParentMenu { get; set; }
//    public string MenuName { get; set; }
//    public string FilePath { get; set; }
//    public string LinkName { get; set; }
//}