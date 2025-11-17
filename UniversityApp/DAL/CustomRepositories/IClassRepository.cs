// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using Entities;

namespace DAL.CustomRepositories
{
    public interface IClassRepository : IRepository<Class>
    {
        /// <summary>
        /// Récupère les classes avec leurs relations (Course, Teacher, StudentClasses), avec pagination et filtrage optionnel par enseignant.
        /// </summary>
        /// <param name="teacherId">ID de l'enseignant (optionnel)</param>
        /// <param name="pageNumber">Numéro de la page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Tuple contenant la liste des classes et le nombre total</returns>
        Task<(IEnumerable<Class> Classes, int TotalCount)> GetClassesWithRelationsPaginatedAsync(Guid? teacherId, int pageNumber, int pageSize);

        /// <summary>
        /// Récupère une classe par son ID avec toutes ses relations.
        /// </summary>
        /// <param name="id">ID de la classe</param>
        /// <returns>La classe avec ses relations ou null</returns>
        Task<Class> GetClassByIdWithRelationsAsync(Guid id);

        /// <summary>
        /// Récupère une classe par son ID pour un enseignant spécifique.
        /// </summary>
        /// <param name="id">ID de la classe</param>
        /// <param name="teacherId">ID de l'enseignant</param>
        /// <returns>La classe si elle appartient à l'enseignant, null sinon</returns>
        Task<Class> GetClassByIdForTeacherAsync(Guid id, Guid teacherId);
    }
}
