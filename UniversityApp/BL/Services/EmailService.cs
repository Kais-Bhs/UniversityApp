// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using NLog;

namespace BL.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body, bool isHtml = true)
        {
            try
            {
                var smtpServer = _configuration["EmailSettings:SmtpServer"];
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
                var senderEmail = _configuration["EmailSettings:SenderEmail"];
                var senderName = _configuration["EmailSettings:SenderName"];
                var username = _configuration["EmailSettings:Username"];
                var password = _configuration["EmailSettings:Password"];
                var enableSsl = bool.Parse(_configuration["EmailSettings:EnableSsl"]);

                using var client = new SmtpClient(smtpServer, smtpPort)
                {
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = enableSsl
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail, senderName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isHtml
                };

                mailMessage.To.Add(toEmail);

                await client.SendMailAsync(mailMessage);
                _logger.Info("Email sent successfully to {Email} with subject: {Subject}", toEmail, subject);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to send email to {Email} with subject: {Subject}", toEmail, subject);
                throw;
            }
        }

        public async Task SendGradeNotificationAsync(string studentEmail, string studentName, string assignmentTitle, decimal grade, string remarks)
        {
            var subject = $"Assignment Graded: {assignmentTitle}";
            var body = $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h2>Hello {studentName},</h2>
                    <p>Your assignment <strong>{assignmentTitle}</strong> has been graded.</p>
                    <div style='background-color: #f0f0f0; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                        <h3>Grade: {grade}/100</h3>
                        {(string.IsNullOrEmpty(remarks) ? "" : $"<p><strong>Remarks:</strong> {remarks}</p>")}
                    </div>
                    <p>You can view your full submission details in the University Management System.</p>
                      

                    <p style='color: #666;'>Best regards,  
University Management System</p>
                </body>
                </html>
            ";

            await SendEmailAsync(studentEmail, subject, body, true);
        }

        public async Task SendNewClassNotificationAsync(string studentEmail, string studentName, string className, string courseName, string teacherName)
        {
            var subject = $"Enrolled in New Class: {className}";
            var body = $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h2>Hello {studentName},</h2>
                    <p>You have been enrolled in a new class!</p>
                    <div style='background-color: #e8f5e9; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                        <h3>{className}</h3>
                        <p><strong>Course:</strong> {courseName}</p>
                        <p><strong>Teacher:</strong> {teacherName}</p>
                    </div>
                    <p>Please check the University Management System for class schedule, assignments, and other details.</p>
                      

                    <p style='color: #666;'>Best regards,  
University Management System</p>
                </body>
                </html>
            ";

            await SendEmailAsync(studentEmail, subject, body, true);
        }
    }
}