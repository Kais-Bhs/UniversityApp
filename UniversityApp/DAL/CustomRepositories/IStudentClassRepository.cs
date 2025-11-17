// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using Entities;

namespace DAL.CustomRepositories
{
    public interface IStudentClassRepository : IRepository<StudentClass>
    {
        /// <summary>
        /// Vérifie si un étudiant est inscrit dans une classe.
        /// </summary>
        /// <param name="studentId">ID de l'étudiant</param>
        /// <param name="classId">ID de la classe</param>
        /// <returns>True si l'étudiant est inscrit, False sinon</returns>
        Task<bool> IsStudentEnrolledInClassAsync(Guid studentId, Guid classId);

        /// <summary>
        /// Récupère les IDs des classes auxquelles un étudiant est inscrit.
        /// </summary>
        /// <param name="studentId">ID de l'étudiant</param>
        /// <returns>Liste des IDs des classes</returns>
        Task<List<Guid>> GetEnrolledClassIdsByStudentAsync(Guid studentId);

        /// <summary>
        /// Récupère les étudiants d'une classe avec leurs détails.
        /// </summary>
        /// <param name="classId">ID de la classe</param>
        /// <returns>Liste des étudiants</returns>
        Task<IEnumerable<User>> GetStudentsWithDetailsByClassIdAsync(Guid classId);
    }
}
