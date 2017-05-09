using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelloTutor.Models
{
    public class RoleViewModel
    {
        public Int32 RoleID { get; set; }
        public String RoleName { get; set; }

        public Guid RoleGuid { get; set; }
    }
}