// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using Entities;

namespace DAL.CustomRepositories
{
    public interface ISubmissionRepository : IRepository<Submission>
    {
        /// <summary>
        /// Vérifie si un étudiant a déjà soumis un devoir.
        /// </summary>
        /// <param name="assignmentId">ID du devoir</param>
        /// <param name="studentId">ID de l'étudiant</param>
        /// <returns>True si déjà soumis, False sinon</returns>
        Task<bool> HasStudentSubmittedAsync(Guid assignmentId, Guid studentId);

        /// <summary>
        /// Récupère une soumission par son ID avec toutes ses relations.
        /// </summary>
        /// <param name="id">ID de la soumission</param>
        /// <returns>La soumission avec ses relations ou null</returns>
        Task<Submission> GetSubmissionByIdWithRelationsAsync(Guid id);

        /// <summary>
        /// Récupère une soumission avec l'assignment et sa classe.
        /// </summary>
        /// <param name="id">ID de la soumission</param>
        /// <returns>La soumission avec assignment et classe ou null</returns>
        Task<Submission> GetSubmissionWithAssignmentAndClassAsync(Guid id);

        /// <summary>
        /// Récupère les soumissions d'un devoir avec relations.
        /// </summary>
        /// <param name="assignmentId">ID du devoir</param>
        /// <returns>Liste des soumissions</returns>
        Task<IEnumerable<Submission>> GetSubmissionsByAssignmentWithRelationsAsync(Guid assignmentId);

        /// <summary>
        /// Récupère les notes d'un étudiant (soumissions notées uniquement).
        /// </summary>
        /// <param name="studentId">ID de l'étudiant</param>
        /// <returns>Liste des soumissions notées</returns>
        Task<IEnumerable<Submission>> GetStudentGradesWithRelationsAsync(Guid studentId);
    }
}
