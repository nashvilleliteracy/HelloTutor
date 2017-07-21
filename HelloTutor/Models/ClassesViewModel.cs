using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
namespace HelloTutor.Models
{
    public class ClassesViewModel
    {
        [System.ComponentModel.DataAnnotations.ScaffoldColumn(false)]
        public Int32 ClassID { get; set; }
        public String ClassName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Int32 RoleID { get; set; }
        public Int32 MaxEnrollment { get; set; }
        [System.ComponentModel.DataAnnotations.ScaffoldColumn(false)]
        public Guid ClassGuid { get; set; }
        public String ClassDescription { get; set; }

        //public IEnumerable<SelectListItem> GetClassesForListBox(Int32 roleId)
        //{
        //    HelloTutorEntities db = new HelloTutorEntities();
        //    var classes = (from c in db.Classes where c.Role == roleId orderby c.Name select new SelectListItem
        //    {
        //        Value = c.Id.ToString(),
        //        Text = c.Name
        //    });

        //    return new SelectList(classes, "Value", "Text");
        //}

        public IQueryable<ClassesViewModel> GetClassesForList(int roleId)
        {
            HelloTutorEntities db = new HelloTutorEntities();

            var classlist = (from c in db.Classes
                            where c.Role == roleId
                            orderby c.Name select new ClassesViewModel
                            {
                                ClassID = c.Id,
                                ClassName = c.Name
                            }).AsQueryable();

            return classlist;
        }

    }
}