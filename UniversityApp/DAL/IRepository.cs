// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Retrieves a queryable collection of entities based on the provided predicate.
        /// </summary>
        /// <param name="predicate">The predicate to filter entities.</param>
        /// <returns>A queryable collection of entities.</returns>
        Task<IQueryable<T>> Query(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Retrieves a queryable collection of all entities.
        /// </summary>
        /// <returns>A queryable collection of all entities.</returns>
        Task<IQueryable<T>> Query();

        /// <summary>
        /// Retrieves an entity by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>The entity with the provided ID, or null if not found.</returns>
        Task<T> Get(Guid? id);

        /// <summary>
        /// Retrieves a collection of all entities asynchronously.
        /// </summary>
        /// <returns>A collection of all entities.</returns>
        Task<IEnumerable<T>> Get();

        /// <summary>
        /// Adds an entity to the repository asynchronously.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        Task Add(T entity);

        /// <summary>
        /// Updates an entity in the repository asynchronously.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        Task Update(T entity);

        /// <summary>
        /// Deletes an entity from the repository asynchronously.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        Task Delete(T entity);
        Task Add(IEnumerable<T> entities);
        Task Update(IEnumerable<T> entities);
        Task Delete(IEnumerable<T> entities);
    }
}
