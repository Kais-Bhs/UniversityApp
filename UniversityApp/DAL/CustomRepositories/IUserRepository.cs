// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using Entities;

namespace DAL.CustomRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Récupère un utilisateur par son email.
        /// </summary>
        /// <param name="email">Email de l'utilisateur</param>
        /// <returns>L'utilisateur ou null</returns>
        Task<User> GetUserByEmailAsync(string email);

        /// <summary>
        /// Récupère un utilisateur par son ID.
        /// </summary>
        /// <param name="id">ID de l'utilisateur</param>
        /// <returns>L'utilisateur ou null</returns>
        Task<User> GetUserByIdAsync(Guid id);

        /// <summary>
        /// Récupère un enseignant par son ID avec vérification du rôle.
        /// </summary>
        /// <param name="id">ID de l'enseignant</param>
        /// <returns>L'enseignant ou null</returns>
        Task<User> GetTeacherByIdAsync(Guid id);

        /// <summary>
        /// Récupère un étudiant par son ID avec vérification du rôle.
        /// </summary>
        /// <param name="id">ID de l'étudiant</param>
        /// <returns>L'étudiant ou null</returns>
        Task<User> GetStudentByIdAsync(Guid id);
    }
}
