// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using AutoMapper;
using DAL;
using DTOs.Department;
using Entities;
using Microsoft.Extensions.Caching.Memory;
using NLog;

namespace BL.Managers
{
    public class DepartmentManager : IDepartmentManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private const string DepartmentsCacheKey = "AllDepartments";
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public DepartmentManager(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<List<DepartmentDto>> GetAllDepartmentsAsync()
        {
            try
            {
                _logger.Info("Getting all departments");

                if (_cache.TryGetValue(DepartmentsCacheKey, out List<DepartmentDto> cachedDepartments))
                {
                    _logger.Info("Retrieved {Count} departments from cache", cachedDepartments.Count);
                    return cachedDepartments;
                }

                var departments = await _unitOfWork.RepoDepartment.GetAllDepartmentsWithHeadAsync();

                var departmentDtos = _mapper.Map<List<DepartmentDto>>(departments);

                _cache.Set(DepartmentsCacheKey, departmentDtos, TimeSpan.FromHours(1));

                _logger.Info("Retrieved {Count} departments from database", departmentDtos.Count);

                return departmentDtos;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting all departments");
                throw;
            }
        }

        public async Task<DepartmentDto> GetDepartmentByIdAsync(Guid id)
        {
            try
            {
                _logger.Info("Getting department {DepartmentId}", id);

                var department = await _unitOfWork.RepoDepartment.GetDepartmentByIdWithHeadAsync(id);

                if (department == null)
                {
                    _logger.Warn("Department {DepartmentId} not found", id);
                    throw new KeyNotFoundException($"Department with ID {id} not found");
                }

                _logger.Info("Department {DepartmentId} retrieved successfully", id);

                return _mapper.Map<DepartmentDto>(department);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting department {DepartmentId}", id);
                throw;
            }
        }

        public async Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto createDepartmentDto)
        {
            try
            {
                _logger.Info("Creating department with name {Name}", createDepartmentDto.Name);

                var nameExists = await _unitOfWork.RepoDepartment.CheckDepartmentNameExistsAsync(createDepartmentDto.Name);

                if (nameExists)
                {
                    _logger.Warn("Department name {Name} already exists", createDepartmentDto.Name);
                    throw new InvalidOperationException("Department name must be unique");
                }

                var headOfDepartment = await _unitOfWork.RepoUser.GetTeacherByIdAsync(createDepartmentDto.HeadOfDepartmentId);

                if (headOfDepartment == null)
                {
                    _logger.Warn("Head of department {HeadId} not found or not a teacher", createDepartmentDto.HeadOfDepartmentId);
                    throw new InvalidOperationException("Head of Department must be a teacher");
                }

                var department = _mapper.Map<Department>(createDepartmentDto);
                department.Id = Guid.NewGuid();

                await _unitOfWork.RepoDepartment.Add(department);
                await _unitOfWork.SaveAsync();

                _cache.Remove(DepartmentsCacheKey);

                _logger.Info("Department {DepartmentId} created successfully", department.Id);

                return await GetDepartmentByIdAsync(department.Id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error creating department with name {Name}", createDepartmentDto.Name);
                throw;
            }
        }

        public async Task<DepartmentDto> UpdateDepartmentAsync(Guid id, UpdateDepartmentDto updateDepartmentDto)
        {
            try
            {
                _logger.Info("Updating department {DepartmentId}", id);

                var department = await _unitOfWork.RepoDepartment.GetDepartmentByIdWithHeadAsync(id);

                if (department == null)
                {
                    _logger.Warn("Department {DepartmentId} not found", id);
                    throw new KeyNotFoundException($"Department with ID {id} not found");
                }

                var nameExists = await _unitOfWork.RepoDepartment.CheckDepartmentNameExistsAsync(updateDepartmentDto.Name, id);

                if (nameExists)
                {
                    _logger.Warn("Department name {Name} already exists", updateDepartmentDto.Name);
                    throw new InvalidOperationException("Department name must be unique");
                }

                var headOfDepartment = await _unitOfWork.RepoUser.GetTeacherByIdAsync(updateDepartmentDto.HeadOfDepartmentId);

                if (headOfDepartment == null)
                {
                    _logger.Warn("Head of department {HeadId} not found or not a teacher", updateDepartmentDto.HeadOfDepartmentId);
                    throw new InvalidOperationException("Head of Department must be a teacher");
                }

                _mapper.Map(updateDepartmentDto, department);
                department.UpdatedDate = DateTimeOffset.UtcNow;

                await _unitOfWork.RepoDepartment.Update(department);
                await _unitOfWork.SaveAsync();

                _cache.Remove(DepartmentsCacheKey);

                _logger.Info("Department {DepartmentId} updated successfully", id);

                return await GetDepartmentByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating department {DepartmentId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteDepartmentAsync(Guid id)
        {
            try
            {
                _logger.Info("Deleting department {DepartmentId}", id);

                var department = await _unitOfWork.RepoDepartment.GetDepartmentByIdWithHeadAsync(id);

                if (department == null)
                {
                    _logger.Warn("Department {DepartmentId} not found", id);
                    throw new KeyNotFoundException($"Department with ID {id} not found");
                }

                await _unitOfWork.RepoDepartment.Delete(department);
                await _unitOfWork.SaveAsync();

                _cache.Remove(DepartmentsCacheKey);

                _logger.Info("Department {DepartmentId} deleted successfully", id);

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting department {DepartmentId}", id);
                throw;
            }
        }
    }
}
