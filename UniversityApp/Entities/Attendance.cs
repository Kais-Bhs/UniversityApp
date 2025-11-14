// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Attendance
    {
        public Guid Id { get; set; }
        public Guid ClassId { get; set; }
        public Guid StudentId { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Status { get; set; }
        public Guid MarkedByTeacherId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        public Class Class { get; set; }
        public User Student { get; set; }
        public User MarkedByTeacher { get; set; }
    }
}
