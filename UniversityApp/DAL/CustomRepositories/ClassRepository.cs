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
    public class ClassRepository : Repository<Class>, IClassRepository
    {
        public ClassRepository(IDAOEntities<Class> daoEntities) : base(daoEntities)
        {
        }

        public async Task<(IEnumerable<Class> Classes, int TotalCount)> GetClassesWithRelationsPaginatedAsync(Guid? teacherId, int pageNumber, int pageSize)
        {
            try
            {
                IQueryable<Class> query;
                
                if (teacherId.HasValue)
                {
                    query = await Query(c => c.TeacherId == teacherId.Value);
                }
                else
                {
                    query = await Query(c => true);
                }

                var totalCount = query.Count();

                var classes = query
                    .OrderByDescending(c => c.CreatedDate)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return (classes, totalCount);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Class> GetClassByIdWithRelationsAsync(Guid id)
        {
            try
            {
                var query = (await Query(c => c.Id == id)).FirstOrDefault();
                return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Class> GetClassByIdForTeacherAsync(Guid id, Guid teacherId)
        {
            try
            {
                var query = (await Query(c => c.Id == id && c.TeacherId == teacherId)).FirstOrDefault();
                return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
