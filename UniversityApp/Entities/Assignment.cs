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
    public class Assignment
    {
        public Guid Id { get; set; }
        public Guid ClassId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public Guid CreatedByTeacherId { get; set; }

        public Class Class { get; set; }
        public User CreatedByTeacher { get; set; }
        public ICollection<Submission> Submissions { get; set; }
    }
}
