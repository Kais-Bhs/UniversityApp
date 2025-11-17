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

namespace BL.Managers
{
    public class DepartmentManager : IDepartmentManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private const string DepartmentsCacheKey = "AllDepartments";

        public DepartmentManager(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<List<DepartmentDto>> GetAllDepartmentsAsync()
        {
            if (_cache.TryGetValue(DepartmentsCacheKey, out List<DepartmentDto> cachedDepartments))
            {
                return cachedDepartments;
            }

            var departments = await _unitOfWork.RepoDepartment.GetAllDepartmentsWithHeadAsync();

            var departmentDtos = _mapper.Map<List<DepartmentDto>>(departments);

            _cache.Set(DepartmentsCacheKey, departmentDtos, TimeSpan.FromHours(1));

            return departmentDtos;
        }

        public async Task<DepartmentDto> GetDepartmentByIdAsync(Guid id)
        {
            var department = await _unitOfWork.RepoDepartment.GetDepartmentByIdWithHeadAsync(id);

            if (department == null)
            {
                throw new KeyNotFoundException($"Department with ID {id} not found");
            }

            return _mapper.Map<DepartmentDto>(department);
        }

        public async Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto createDepartmentDto)
        {
            var nameExists = await _unitOfWork.RepoDepartment.CheckDepartmentNameExistsAsync(createDepartmentDto.Name);

            if (nameExists)
            {
                throw new InvalidOperationException("Department name must be unique");
            }

            var headOfDepartment = await _unitOfWork.RepoUser.GetTeacherByIdAsync(createDepartmentDto.HeadOfDepartmentId);

            if (headOfDepartment == null)
            {
                throw new InvalidOperationException("Head of Department must be a teacher");
            }

            var department = _mapper.Map<Department>(createDepartmentDto);
            department.Id = Guid.NewGuid();

            await _unitOfWork.RepoDepartment.Add(department);
            await _unitOfWork.SaveAsync();

            _cache.Remove(DepartmentsCacheKey);

            return await GetDepartmentByIdAsync(department.Id);
        }

        public async Task<DepartmentDto> UpdateDepartmentAsync(Guid id, UpdateDepartmentDto updateDepartmentDto)
        {
            var department = await _unitOfWork.RepoDepartment.GetDepartmentByIdWithHeadAsync(id);

            if (department == null)
            {
                throw new KeyNotFoundException($"Department with ID {id} not found");
            }

            var nameExists = await _unitOfWork.RepoDepartment.CheckDepartmentNameExistsAsync(updateDepartmentDto.Name, id);

            if (nameExists)
            {
                throw new InvalidOperationException("Department name must be unique");
            }

            var headOfDepartment = await _unitOfWork.RepoUser.GetTeacherByIdAsync(updateDepartmentDto.HeadOfDepartmentId);

            if (headOfDepartment == null)
            {
                throw new InvalidOperationException("Head of Department must be a teacher");
            }

            _mapper.Map(updateDepartmentDto, department);
            department.UpdatedDate = DateTimeOffset.UtcNow;

            await _unitOfWork.RepoDepartment.Update(department);
            await _unitOfWork.SaveAsync();

            _cache.Remove(DepartmentsCacheKey);

            return await GetDepartmentByIdAsync(id);
        }

        public async Task<bool> DeleteDepartmentAsync(Guid id)
        {
            var department = await _unitOfWork.RepoDepartment.GetDepartmentByIdWithHeadAsync(id);

            if (department == null)
            {
                throw new KeyNotFoundException($"Department with ID {id} not found");
            }

            await _unitOfWork.RepoDepartment.Delete(department);
            await _unitOfWork.SaveAsync();

            _cache.Remove(DepartmentsCacheKey);

            return true;
        }
    }
}
