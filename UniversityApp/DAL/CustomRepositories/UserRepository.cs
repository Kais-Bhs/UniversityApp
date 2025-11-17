// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using DAO.DAO;
using Entities;

namespace DAL.CustomRepositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(IDAOEntities<User> daoEntities) : base(daoEntities)
        {
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            try
            {
                var query = (await Query(u => u.Email == email)).FirstOrDefault();
                return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            try
            {
                var query = (await Query(u => u.Id == id)).FirstOrDefault();
                return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<User> GetTeacherByIdAsync(Guid id)
        {
            try
            {
                var query = (await Query(u => u.Id == id && u.Role == "Teacher")).FirstOrDefault();
                return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<User> GetStudentByIdAsync(Guid id)
        {
            try
            {
                var query = (await Query(u => u.Id == id && u.Role == "Student")).FirstOrDefault();
                return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
