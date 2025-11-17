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
    public class AssignmentRepository : Repository<Assignment>, IAssignmentRepository
    {
        public AssignmentRepository(IDAOEntities<Assignment> daoEntities) : base(daoEntities)
        {
        }

        public async Task<(IEnumerable<Assignment> Assignments, int TotalCount)> GetAssignmentsByClassPaginatedAsync(Guid classId, int pageNumber, int pageSize)
        {
            try
            {
                var query = await Query(a => a.ClassId == classId);

                var totalCount = query.Count();

                var assignments = query
                    .OrderByDescending(a => a.CreatedDate)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return (assignments, totalCount);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Assignment> GetAssignmentByIdWithRelationsAsync(Guid id)
        {
            try
            {
                var query = (await Query(a => a.Id == id)).FirstOrDefault();
                return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<Assignment>> GetAssignmentsByEnrolledClassesAsync(List<Guid> enrolledClassIds)
        {
            try
            {
                var query = await Query(a => enrolledClassIds.Contains(a.ClassId));
                return query
                    .OrderByDescending(a => a.DueDate)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
