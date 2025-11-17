// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using DTOs.Department;

namespace BL.Managers
{
    public interface IDepartmentManager
    {
        Task<List<DepartmentDto>> GetAllDepartmentsAsync();
        Task<DepartmentDto> GetDepartmentByIdAsync(Guid id);
        Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto createDepartmentDto);
        Task<DepartmentDto> UpdateDepartmentAsync(Guid id, UpdateDepartmentDto updateDepartmentDto);
        Task<bool> DeleteDepartmentAsync(Guid id);
    }
}
