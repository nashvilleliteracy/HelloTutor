﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelloTutor.Models
{
    public class TutorViewModel
    {
        public Int32 TutorID { get; set; }
        public Guid TutorGuid { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String EmailAddress { get; set; }
        public String PhoneNumber { get; set; }
        public String Comments { get; set; }
        public String RoleID { get; set; }
        public String TrainingSessionID { get; set; }
        public DateTime DateTimeCreated { get; set; }
    }
}