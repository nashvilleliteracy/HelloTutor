using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelloTutor.Models
{
    public class RoleViewModel
    {
        public Int32 RoleID { get; set; }
        public String RoleName { get; set; }

        public Guid RoleGuid { get; set; }

        public IEnumerable<SelectListItem> GetRolesForListBox()
        {
            HelloTutorEntities db = new HelloTutorEntities();
            var roles = db.Roles.Select(x =>
                                new SelectListItem
                                {
                                    Value = x.Id.ToString(),
                                    Text = x.Name
                                }).OrderBy(r=>r.Text);

            return new SelectList(roles, "Value", "Text");
        }

    }
}