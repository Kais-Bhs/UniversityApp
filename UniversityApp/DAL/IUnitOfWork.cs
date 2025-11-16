// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace DAL
{
    public interface IUnitOfWork
    {

        IRepository<User> RepoUser { get; set; }
        IRepository<Department> RepoDepartment { get; set; }
        IRepository<Course> RepoCourse { get; set; }
        IRepository<Class> RepoClass { get; set; }
        IRepository<StudentClass> RepoStudentClass { get; set; }
        IRepository<Attendance> RepoAttendance { get; set; }
        IRepository<Assignment> RepoAssignment { get; set; }
        IRepository<Submission> RepoSubmission { get; set; }
        IRepository<Notification> RepoNotification { get; set; }


        /// <summary>
        /// Begins a new transaction asynchronously.
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// Commits the current transaction asynchronously.
        /// </summary>
        Task CommitTransactionAsync();

        /// <summary>
        /// Rolls back the current transaction asynchronously.
        /// </summary>
        Task RollbackTransactionAsync();

        /// <summary>
        /// Saves changes made in the unit of work asynchronously.
        /// </summary>
        Task SaveAsync();
    }
}
