// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using DTOs.Class;
using DTOs.Common;

namespace BL.Managers
{
    public interface IClassManager
    {
        Task<PagedResult<ClassDto>> GetAllClassesAsync(Guid? teacherId, int pageNumber = 1, int pageSize = 10);
        Task<ClassDto> GetClassByIdAsync(Guid id);
        Task<ClassDto> CreateClassAsync(CreateClassDto createClassDto, Guid teacherId);
        Task<ClassDto> UpdateClassAsync(Guid id, UpdateClassDto updateClassDto, Guid teacherId);
        Task<bool> AssignStudentToClassAsync(AssignStudentDto assignStudentDto, Guid teacherId);
        Task<List<DTOs.User.UserDto>> GetStudentsInClassAsync(Guid classId);
    }
}
