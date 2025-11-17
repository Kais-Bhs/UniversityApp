// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using Entities;

namespace DAL.CustomRepositories
{
    public interface IAttendanceRepository : IRepository<Attendance>
    {
        /// <summary>
        /// Récupère les présences d'une classe avec relations, avec filtrage optionnel par étudiant.
        /// </summary>
        /// <param name="classId">ID de la classe</param>
        /// <param name="studentId">ID de l'étudiant (optionnel)</param>
        /// <returns>Liste des présences</returns>
        Task<IEnumerable<Attendance>> GetAttendanceByClassWithRelationsAsync(Guid classId, Guid? studentId = null);

        /// <summary>
        /// Récupère toutes les présences d'un étudiant avec relations.
        /// </summary>
        /// <param name="studentId">ID de l'étudiant</param>
        /// <returns>Liste des présences</returns>
        Task<IEnumerable<Attendance>> GetAttendanceByStudentWithRelationsAsync(Guid studentId);

        /// <summary>
        /// Récupère une présence par son ID avec toutes ses relations.
        /// </summary>
        /// <param name="id">ID de la présence</param>
        /// <returns>La présence avec ses relations ou null</returns>
        Task<Attendance> GetAttendanceByIdWithRelationsAsync(Guid id);
    }
}
