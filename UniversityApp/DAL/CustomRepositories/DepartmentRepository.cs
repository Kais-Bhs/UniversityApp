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
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(IDAOEntities<Department> daoEntities) : base(daoEntities)
        {
        }

        public async Task<bool> DoesDepartmentExistAsync(Guid departmentId)
        {
            try
            {
                var department = (await Query(d => d.Id == departmentId)).FirstOrDefault();
                return department != null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Department> GetDepartmentByIdWithHeadAsync(Guid id)
        {
            try
            {
                var query = (await Query(d => d.Id == id)).FirstOrDefault();
                return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsWithHeadAsync()
        {
            try
            {
                var query = await Query(d => true);
                return query.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> CheckDepartmentNameExistsAsync(string name, Guid? excludeId = null)
        {
            try
            {
                Department existingDepartment;

                if (excludeId.HasValue)
                {
                    existingDepartment = (await Query(d => d.Name == name && d.Id != excludeId.Value)).FirstOrDefault();
                }
                else
                {
                    existingDepartment = (await Query(d => d.Name == name)).FirstOrDefault();
                }

                return existingDepartment != null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
