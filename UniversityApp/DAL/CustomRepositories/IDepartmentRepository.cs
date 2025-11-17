// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using Entities;

namespace DAL.CustomRepositories
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        /// <summary>
        /// Vérifie si un département existe.
        /// </summary>
        /// <param name="departmentId">ID du département</param>
        /// <returns>True si le département existe, False sinon</returns>
        Task<bool> DoesDepartmentExistAsync(Guid departmentId);

        /// <summary>
        /// Récupère un département par son ID avec le chef de département.
        /// </summary>
        /// <param name="id">ID du département</param>
        /// <returns>Le département avec le chef de département ou null</returns>
        Task<Department> GetDepartmentByIdWithHeadAsync(Guid id);

        /// <summary>
        /// Récupère tous les départements avec leurs chefs de département.
        /// </summary>
        /// <returns>Liste des départements</returns>
        Task<IEnumerable<Department>> GetAllDepartmentsWithHeadAsync();

        /// <summary>
        /// Vérifie si un nom de département existe déjà.
        /// </summary>
        /// <param name="name">Nom du département</param>
        /// <param name="excludeId">ID du département à exclure (pour les mises à jour)</param>
        /// <returns>True si le nom existe déjà, False sinon</returns>
        Task<bool> CheckDepartmentNameExistsAsync(string name, Guid? excludeId = null);
    }
}
