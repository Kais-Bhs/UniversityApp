// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
namespace BL.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body, bool isHtml = true);
        Task SendGradeNotificationAsync(string studentEmail, string studentName, string assignmentTitle, decimal grade, string remarks);
        Task SendNewClassNotificationAsync(string studentEmail, string studentName, string className, string courseName, string teacherName);
    }
}