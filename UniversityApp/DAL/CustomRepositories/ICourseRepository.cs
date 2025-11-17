// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using Entities;

namespace DAL.CustomRepositories
{
    public interface ICourseRepository : IRepository<Course>
    {
        /// <summary>
        /// Récupère les cours avec leur département, avec pagination et recherche optionnelle.
        /// </summary>
        /// <param name="pageNumber">Numéro de la page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="searchTerm">Terme de recherche optionnel</param>
        /// <returns>Tuple contenant la liste des cours et le nombre total</returns>
        Task<(IEnumerable<Course> Courses, int TotalCount)> GetCoursesWithDepartmentPaginatedAsync(int pageNumber, int pageSize, string searchTerm = null);

        /// <summary>
        /// Récupère un cours par son ID avec son département.
        /// </summary>
        /// <param name="id">ID du cours</param>
        /// <returns>Le cours avec son département ou null</returns>
        Task<Course> GetCourseByIdWithDepartmentAsync(Guid id);

        /// <summary>
        /// Vérifie si un code de cours existe déjà dans un département.
        /// </summary>
        /// <param name="code">Code du cours</param>
        /// <param name="departmentId">ID du département</param>
        /// <param name="excludeId">ID du cours à exclure (pour les mises à jour)</param>
        /// <returns>True si le code existe déjà, False sinon</returns>
        Task<bool> CheckCourseCodeExistsInDepartmentAsync(string code, Guid departmentId, Guid? excludeId = null);
    }
}
