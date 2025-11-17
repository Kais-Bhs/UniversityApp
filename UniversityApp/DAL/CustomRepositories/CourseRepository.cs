// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using DAO.DAO;
using Entities;

namespace DAL.CustomRepositories
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(IDAOEntities<Course> daoEntities) : base(daoEntities)
        {
        }

        public async Task<(IEnumerable<Course> Courses, int TotalCount)> GetCoursesWithDepartmentPaginatedAsync(int pageNumber, int pageSize, string searchTerm = null)
        {
            try
            {
                IQueryable<Course> query;

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    query = await Query(c => c.Name.Contains(searchTerm) || c.Code.Contains(searchTerm));
                }
                else
                {
                    query = await Query(c => true);
                }

                var totalCount = query.Count();

                var courses = query
                    .OrderByDescending(c => c.CreatedDate)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return (courses, totalCount);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Course> GetCourseByIdWithDepartmentAsync(Guid id)
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

        public async Task<bool> CheckCourseCodeExistsInDepartmentAsync(string code, Guid departmentId, Guid? excludeId = null)
        {
            try
            {
                Course existingCourse;

                if (excludeId.HasValue)
                {
                    existingCourse = (await Query(c => c.Code == code && c.DepartmentId == departmentId && c.Id != excludeId.Value)).FirstOrDefault();
                }
                else
                {
                    existingCourse = (await Query(c => c.Code == code && c.DepartmentId == departmentId)).FirstOrDefault();
                }

                return existingCourse != null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
