// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using DAO;
using DAO.DAO;
using Entities;
using Microsoft.Extensions.DependencyInjection;
using static System.Collections.Specialized.BitVector32;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDAODataBase _DAODataBase;
        private readonly IServiceProvider _serviceProvider;

        public IRepository<User> RepoUser { get; set; }
        public IRepository<Department> RepoDepartment { get; set; }
        public IRepository<Course> RepoCourse { get; set; }
        public IRepository<Class> RepoClass { get; set; }
        public IRepository<StudentClass> RepoStudentClass { get; set; }
        public IRepository<Attendance> RepoAttendance { get; set; }
        public IRepository<Assignment> RepoAssignment { get; set; }
        public IRepository<Submission> RepoSubmission { get; set; }
        public IRepository<Notification> RepoNotification { get; set; }


        public UnitOfWork(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            var dbContext = _serviceProvider.GetRequiredService<UniversityContext>();
            _DAODataBase = new DAODataBase(dbContext);

            RepoUser = new Repository<User>(new DAOEntities<User>(dbContext));
            RepoDepartment = new Repository<Department>(new DAOEntities<Department>(dbContext));
            RepoCourse = new Repository<Course>(new DAOEntities<Course>(dbContext));
            RepoClass = new Repository<Class>(new DAOEntities<Class>(dbContext));
            RepoStudentClass = new Repository<StudentClass>(new DAOEntities<StudentClass>(dbContext));
            RepoAttendance = new Repository<Attendance>(new DAOEntities<Attendance>(dbContext));
            RepoAssignment = new Repository<Assignment>(new DAOEntities<Assignment>(dbContext));
            RepoSubmission = new Repository<Submission>(new DAOEntities<Submission>(dbContext));
            RepoNotification = new Repository<Notification>(new DAOEntities<Notification>(dbContext));


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
