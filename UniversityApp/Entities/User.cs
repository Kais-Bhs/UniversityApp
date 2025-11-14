// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }

        public ICollection<Class> TaughtClasses { get; set; }
        public ICollection<StudentClass> StudentClasses { get; set; }
        public ICollection<Attendance> Attendances { get; set; }
        public ICollection<Attendance> MarkedAttendances { get; set; }
        public ICollection<Assignment> CreatedAssignments { get; set; }
        public ICollection<Submission> Submissions { get; set; }
        public ICollection<Submission> GradedSubmissions { get; set; }
        public ICollection<Department> ManagedDepartments { get; set; }
    }
}
