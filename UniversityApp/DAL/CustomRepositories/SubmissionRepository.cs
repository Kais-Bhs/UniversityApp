// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using DAO.DAO;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.CustomRepositories
{
    public class SubmissionRepository : Repository<Submission>, ISubmissionRepository
    {
        public SubmissionRepository(IDAOEntities<Submission> daoEntities) : base(daoEntities)
        {
        }

        public async Task<bool> HasStudentSubmittedAsync(Guid assignmentId, Guid studentId)
        {
            try
            {
                var submission = (await Query(s => s.AssignmentId == assignmentId && s.StudentId == studentId)).FirstOrDefault();
                return submission != null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Submission> GetSubmissionByIdWithRelationsAsync(Guid id)
        {
            try
            {
                var query = (await Query(s => s.Id == id)).FirstOrDefault();
                return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Submission> GetSubmissionWithAssignmentAndClassAsync(Guid id)
        {
            try
            {
                var query = (await Query(s => s.Id == id)).FirstOrDefault();
                return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<Submission>> GetSubmissionsByAssignmentWithRelationsAsync(Guid assignmentId)
        {
            try
            {
                var query = await Query(s => s.AssignmentId == assignmentId);
                return query
                    .OrderByDescending(s => s.SubmittedDate)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<Submission>> GetStudentGradesWithRelationsAsync(Guid studentId)
        {
            try
            {
                var query = await Query(s => s.StudentId == studentId && s.Grade.HasValue);
                return query
                    .OrderByDescending(s => s.SubmittedDate)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
