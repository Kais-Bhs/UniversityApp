// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using Microsoft.EntityFrameworkCore.Storage;

namespace DAO.DAO
{
    public class DAODataBase : IDAODataBase
    {
        private readonly UniversityContext _context;
        private IDbContextTransaction _transaction;

        public DAODataBase(UniversityContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Démarre de manière asynchrone une nouvelle transaction de base de données dans le contexte Entity Framework.
        /// </summary>
        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }
        /// <summary>
        /// Valide de manière asynchrone la transaction en cours, enregistre les modifications dans le contexte et applique la transaction à la base de données.
        /// </summary>
        public async Task CommitTransactionAsync()
        {
            try
            {
                await _transaction.CommitAsync();
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Annule de manière asynchrone la transaction en cours dans le contexte Entity Framework.
        /// </summary>
        public async Task RollbackTransactionAsync()
        {
            await _transaction.RollbackAsync();
        }
        /// <summary>
        /// Enregistre de manière asynchrone toutes les modifications apportées au contexte Entity Framework.
        /// </summary>
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
