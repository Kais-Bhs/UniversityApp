// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using DAL.CustomRepositories;
using DAO;
using DAO.DAO;
using Entities;
using Microsoft.Extensions.DependencyInjection;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDAODataBase _DAODataBase;
        private readonly IServiceProvider _serviceProvider;
        public IUserRepository RepoUser { get; set; }
        public IDepartmentRepository RepoDepartment { get; set; }
        public ICourseRepository RepoCourse { get; set; }
        public IClassRepository RepoClass { get; set; }
        public IStudentClassRepository RepoStudentClass { get; set; }
        public IAttendanceRepository RepoAttendance { get; set; }
        public IAssignmentRepository RepoAssignment { get; set; }
        public ISubmissionRepository RepoSubmission { get; set; }
        public INotificationRepository RepoNotification { get; set; }


        public UnitOfWork(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            var dbContext = _serviceProvider.GetRequiredService<UniversityContext>();
            _DAODataBase = new DAODataBase(dbContext);
            RepoUser = new UserRepository(new DAOEntities<User>(dbContext));
            RepoDepartment = new DepartmentRepository(new DAOEntities<Department>(dbContext));
            RepoCourse = new CourseRepository(new DAOEntities<Course>(dbContext));
            RepoClass = new ClassRepository(new DAOEntities<Class>(dbContext));
            RepoStudentClass = new StudentClassRepository(new DAOEntities<StudentClass>(dbContext));
            RepoAttendance = new AttendanceRepository(new DAOEntities<Attendance>(dbContext));
            RepoAssignment = new AssignmentRepository(new DAOEntities<Assignment>(dbContext));
            RepoSubmission = new SubmissionRepository(new DAOEntities<Submission>(dbContext));
            RepoNotification = new NotificationRepository(new DAOEntities<Notification>(dbContext));
        }

        /// <summary>
        /// Démarre de manière asynchrone une nouvelle transaction de base de données.
        /// </summary>
        public async Task BeginTransactionAsync()
        {
            await _DAODataBase.BeginTransactionAsync();
        }
        /// <summary>
        /// Valide de manière asynchrone la transaction en cours et applique les modifications à la base de données.
        /// </summary>
        public async Task CommitTransactionAsync()
        {
            await _DAODataBase.CommitTransactionAsync();
        }
        /// <summary>
        /// Annule de manière asynchrone la transaction en cours et restaure l'état précédent de la base de données.
        /// </summary>
        public async Task RollbackTransactionAsync()
        {
            await _DAODataBase.RollbackTransactionAsync();
        }
        /// <summary>
        /// Enregistre de manière asynchrone toutes les modifications apportées à la base de données.
        /// </summary>
        public async Task SaveAsync()
        {
            await _DAODataBase.SaveAsync();
        }
    }
}
