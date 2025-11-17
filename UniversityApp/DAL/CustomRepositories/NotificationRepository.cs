// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using DAO.DAO;
using Entities;

namespace DAL.CustomRepositories
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        public NotificationRepository(IDAOEntities<Notification> daoEntities) : base(daoEntities)
        {
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(Guid userId, string userRole)
        {
            try
            {
                var query = await Query(n => (n.RecipientRole == userRole && n.RecipientId == null) || n.RecipientId == userId);
                return query
                    .OrderByDescending(n => n.CreatedDate)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Notification> GetNotificationByIdAsync(Guid id)
        {
            try
            {
                var query = (await Query(n => n.Id == id)).FirstOrDefault();
                return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
