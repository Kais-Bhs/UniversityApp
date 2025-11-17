// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using DAO.DAO;
using Entities;

namespace DAL.CustomRepositories
{
    public class AttendanceRepository : Repository<Attendance>, IAttendanceRepository
    {
        public AttendanceRepository(IDAOEntities<Attendance> daoEntities) : base(daoEntities)
        {
        }

        public async Task<IEnumerable<Attendance>> GetAttendanceByClassWithRelationsAsync(Guid classId, Guid? studentId = null)
        {
            try
            {
                IQueryable<Attendance> query;

                if (studentId.HasValue)
                {
                    query = await Query(a => a.ClassId == classId && a.StudentId == studentId.Value);
                }
                else
                {
                    query = await Query(a => a.ClassId == classId);
                }

                return query
                    .OrderByDescending(a => a.Date)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<Attendance>> GetAttendanceByStudentWithRelationsAsync(Guid studentId)
        {
            try
            {
                var query = await Query(a => a.StudentId == studentId);
                return query
                    .OrderByDescending(a => a.Date)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Attendance> GetAttendanceByIdWithRelationsAsync(Guid id)
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
    }
}
