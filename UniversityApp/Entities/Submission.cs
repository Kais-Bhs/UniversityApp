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
    public class Submission
    {
        public Guid Id { get; set; }
        public Guid AssignmentId { get; set; }
        public Guid StudentId { get; set; }
        public DateTimeOffset SubmittedDate { get; set; }
        public string FileUrl { get; set; }
        public decimal? Grade { get; set; }
        public Guid? GradedByTeacherId { get; set; }
        public string Remarks { get; set; }

        public Assignment Assignment { get; set; }
        public User Student { get; set; }
        public User GradedByTeacher { get; set; }
    }
}
