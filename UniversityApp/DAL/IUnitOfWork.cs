// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using DAL.CustomRepositories;

namespace DAL
{
    public interface IUnitOfWork
    {
        IUserRepository RepoUser { get; set; }
        IDepartmentRepository RepoDepartment { get; set; }
        ICourseRepository RepoCourse { get; set; }
        IClassRepository RepoClass { get; set; }
        IStudentClassRepository RepoStudentClass { get; set; }
        IAttendanceRepository RepoAttendance { get; set; }
        IAssignmentRepository RepoAssignment { get; set; }
        ISubmissionRepository RepoSubmission { get; set; }
        INotificationRepository RepoNotification { get; set; }


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
