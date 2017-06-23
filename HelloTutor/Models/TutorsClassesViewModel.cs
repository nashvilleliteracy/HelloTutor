using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelloTutor.Models
{
    public class TutorsClassesViewModel
    {
        public Int32 ClassID { get; set; }
        public String ClassName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Int32 NumberOfTutors { get; set; }

        public IQueryable<TutorsClassesViewModel> GetTutorsClasses()
        {
            HelloTutorEntities db = new HelloTutorEntities();

            String sql = "Select T1.Id as 'ClassID', T1.Name as 'ClassName', T1.StartDate, T1.EndDate, (Select Count(*) from TutorsClasses where ClassID = T1.Id) as 'NumberOfTutors' ";
            sql += " from Classes T1 where T1.IsDeleted = 0";

            var tutorsList = db.Database.SqlQuery<TutorsClassesViewModel>(sql).AsQueryable();

            return tutorsList;
        }
    }
}