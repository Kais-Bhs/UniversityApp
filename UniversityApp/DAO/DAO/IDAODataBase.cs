// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
namespace DAO.DAO
{
    public interface IDAODataBase
    {
        /// <summary>
        /// Begins a new transaction asynchronously.
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// Commits the current transaction asynchronously.
        /// </summary>
        Task CommitTransactionAsync();

        /// <summary>
        /// Rolls back the current transaction asynchronously.
        /// </summary>
        Task RollbackTransactionAsync();

        /// <summary>
        /// Saves changes made in the unit of work asynchronously.
        /// </summary>
        Task SaveAsync();
    }
}
