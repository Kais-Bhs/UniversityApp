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
    public class StudentClassRepository : Repository<StudentClass>, IStudentClassRepository
    {
        public StudentClassRepository(IDAOEntities<StudentClass> daoEntities) : base(daoEntities)
        {
        }

        public async Task<bool> IsStudentEnrolledInClassAsync(Guid studentId, Guid classId)
        {
            try
            {
                var enrollment = (await Query(sc => sc.StudentId == studentId && sc.ClassId == classId)).FirstOrDefault();
                return enrollment != null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<Guid>> GetEnrolledClassIdsByStudentAsync(Guid studentId)
        {
            try
            {
                var query = await Query(sc => sc.StudentId == studentId);
                return query
                    .Select(sc => sc.ClassId)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<User>> GetStudentsWithDetailsByClassIdAsync(Guid classId)
        {
            try
            {
                var query = await Query(sc => sc.ClassId == classId);
                return query
                    .Select(sc => sc.Student)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
