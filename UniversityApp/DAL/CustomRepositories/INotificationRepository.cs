// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using Entities;

namespace DAL.CustomRepositories
{
    public interface INotificationRepository : IRepository<Notification>
    {
        /// <summary>
        /// Récupère les notifications d'un utilisateur selon son rôle.
        /// </summary>
        /// <param name="userId">ID de l'utilisateur</param>
        /// <param name="userRole">Rôle de l'utilisateur</param>
        /// <returns>Liste des notifications</returns>
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(Guid userId, string userRole);

        /// <summary>
        /// Récupère une notification par son ID.
        /// </summary>
        /// <param name="id">ID de la notification</param>
        /// <returns>La notification ou null</returns>
        Task<Notification> GetNotificationByIdAsync(Guid id);
    }
}
