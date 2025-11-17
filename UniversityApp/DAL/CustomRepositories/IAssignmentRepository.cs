// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using Entities;

namespace DAL.CustomRepositories
{
    public interface IAssignmentRepository : IRepository<Assignment>
    {
        /// <summary>
        /// Récupère les devoirs d'une classe avec pagination et relations.
        /// </summary>
        /// <param name="classId">ID de la classe</param>
        /// <param name="pageNumber">Numéro de la page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Tuple contenant la liste des devoirs et le nombre total</returns>
        Task<(IEnumerable<Assignment> Assignments, int TotalCount)> GetAssignmentsByClassPaginatedAsync(Guid classId, int pageNumber, int pageSize);

        /// <summary>
        /// Récupère un devoir par son ID avec toutes ses relations.
        /// </summary>
        /// <param name="id">ID du devoir</param>
        /// <returns>Le devoir avec ses relations ou null</returns>
        Task<Assignment> GetAssignmentByIdWithRelationsAsync(Guid id);

        /// <summary>
        /// Récupère les devoirs pour les classes auxquelles un étudiant est inscrit.
        /// </summary>
        /// <param name="enrolledClassIds">Liste des IDs des classes</param>
        /// <returns>Liste des devoirs</returns>
        Task<IEnumerable<Assignment>> GetAssignmentsByEnrolledClassesAsync(List<Guid> enrolledClassIds);
    }
}
