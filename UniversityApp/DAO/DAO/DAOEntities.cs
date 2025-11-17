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
using Microsoft.EntityFrameworkCore;

namespace DAO.DAO
{
    public class DAOEntities<T> : IDAOEntities<T> where T : class
    {
        private readonly UniversityContext _context;
        private readonly DbSet<T> _dbSet;

        public DAOEntities(UniversityContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        /// <summary>
        /// Effectue une requête sur les entités en fonction du prédicat spécifié et renvoie une requête IQueryable.
        /// </summary>
        /// <param name="predicate">Le prédicat pour filtrer les entités.</param>
        /// <returns>Une requête IQueryable d'entités basée sur le prédicat.</returns>
        public virtual async Task<IQueryable<T>> Query(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                var query = _dbSet.AsQueryable();

                if (includes != null)
                {
                    query = includes.Aggregate(query, (current, include) => current.Include(include));
                }

                var list = await query.Where(predicate).ToListAsync();
                return list.AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Récupère toutes les entités de la source de données en tant que requête IQueryable.
        /// </summary>
        /// <returns>Une requête IQueryable de toutes les entités.</returns>
        public virtual async Task<IQueryable<T>> Query()
        {
            try
            {
                var list = await _dbSet.ToListAsync();
                return list.AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Récupère de manière asynchrone une entité par son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant de l'entité à récupérer.</param>
        /// <returns>L'entité récupérée de manière asynchrone ou null si aucune entité ne correspond.</returns>

        public virtual async Task<T> Get(Guid? id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Récupère de manière asynchrone toutes les entités de la source de données.
        /// </summary>
        /// <returns>Une collection de toutes les entités récupérées de manière asynchrone.</returns>

        public virtual async Task<IEnumerable<T>> Get()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Recherche de manière asynchrone une entité en fonction du prédicat spécifié sans suivi d'état.
        /// </summary>
        /// <param name="predicate">Le prédicat de recherche.</param>
        /// <returns>L'entité trouvée de manière asynchrone ou null si aucune entité ne correspond.</returns>

        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Ajoute de manière asynchrone une nouvelle entité à la source de données.
        /// </summary>
        /// <param name="entity">L'entité à ajouter.</param>
        public virtual async Task Add(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Met à jour de manière asynchrone une entité dans la source de données.
        /// </summary>
        /// <param name="entity">L'entité à mettre à jour.</param>
        public async Task Update(T entity)
        {
            try
            {
                var key = GetPrimaryKey(entity);
                var existingEntity = _dbSet.Find(key);
                if (existingEntity != null)
                {
                    _context.Entry(existingEntity).CurrentValues.SetValues(entity);
                }
                else
                {
                    _dbSet.Update(entity);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        private object GetPrimaryKey(T entity)
        {
            try
            {
                var keyName = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties
                    .Select(x => x.Name).Single();

                return typeof(T).GetProperty(keyName).GetValue(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Supprime de manière asynchrone une entité de la source de données.
        /// </summary>
        /// <param name="entity">L'entité à supprimer.</param>
        public virtual async Task Delete(T entity)
        {
            try
            {
                _dbSet.Remove(entity);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Ajout de manière asynchrone une liste d'entités dans la source de données.
        /// </summary>
        /// <param name="entities">La liste d'entités à ajouter</param>
        public virtual async Task Add(IEnumerable<T> entities)
        {
            try
            {
                await _dbSet.AddRangeAsync(entities);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Met à jour de manière asynchrone une liste d'entités dans la source de données.
        /// </summary>
        /// <param name="entities">La liste d'entités à mettre à jour.</param>
        public async Task Update(IEnumerable<T> entities)
        {
            try
            {
                foreach (var entity in entities)
                {
                    var key = GetPrimaryKey(entity);
                    var existingEntity = await _dbSet.FindAsync(key);
                    if (existingEntity != null)
                    {
                        _context.Entry(existingEntity).CurrentValues.SetValues(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Supprime de manière asynchrone une liste d'entités de la source de données.
        /// </summary>
        /// <param name="entities">La liste d'entités à supprimer.</param>
        public virtual async Task Delete(IEnumerable<T> entities)
        {
            try
            {
                _dbSet.RemoveRange(entities);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

    }
}
