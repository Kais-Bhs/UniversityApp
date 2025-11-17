// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using DTOs.Assignment;
using DTOs.Common;

namespace BL.Managers
{
    public interface IAssignmentManager
    {
        Task<AssignmentDto> CreateAssignmentAsync(CreateAssignmentDto createAssignmentDto, Guid teacherId);
        Task<PagedResult<AssignmentDto>> GetAssignmentsByClassAsync(Guid classId, int pageNumber = 1, int pageSize = 10);
        Task<AssignmentDto> GetAssignmentByIdAsync(Guid id);
        Task<List<AssignmentDto>> GetStudentAssignmentsAsync(Guid studentId);
    }
}
