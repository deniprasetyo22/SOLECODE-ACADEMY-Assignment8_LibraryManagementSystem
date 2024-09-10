using Assignment5.Domain.Models;
using Assignment5.Persistence.Context;
using Assignment7.Application.Interfaces.IRepositories;
using Assignment7.Application.Interfaces.IService;
using Assignment7.Domain.Models.Mail;
using Assignment7.Persistence.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Assignment7.Persistence.Repositories
{
    public class BookRequestRepository:IBookRequestRepository
    {
        private readonly LibraryContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        public BookRequestRepository(LibraryContext context, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, IEmailService emailService)
        {
            _context = context; 
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<Bookrequest> AddBookRequestAsync(Bookrequest bookRequest)
        {
            var user = _httpContextAccessor.HttpContext?.User.Identity!.Name;

            var currentUser = await _userManager.FindByNameAsync(user!);

            var userId = currentUser?.Id;

            var userName = currentUser?.UserName;

            var userEmail = currentUser?.Email;

            var process = new Process
            {
                Workflowid = 1,
                Requestdate = DateTime.Now,
                Status = "Pending",
                Currentstepid = 2,
                Requesttype = "Borrow Book Request",
                Requesterid = userId
            };

            await _context.Processes.AddAsync(process);
            await _context.SaveChangesAsync();

            //workflowaction
            var workflow = new Workflowaction
            {
                Requestid = process.Processid,
                Stepid = 1,
                Actorid = userId,
                Action = process.Status,
                Actiondate = DateTime.Now,
                Comments = "Submitted a borrow book request"
            };

            await _context.Workflowactions.AddAsync(workflow);
            await _context.SaveChangesAsync();

            bookRequest.Requestid = bookRequest.Requestid;
            bookRequest.Processid = process.Processid;
            bookRequest.Startdate = DateTime.Now;
            bookRequest.Enddate = bookRequest.Startdate?.AddDays(7);

            await _context.Bookrequests.AddAsync(bookRequest);
            await _context.SaveChangesAsync();

            var emailBody = System.IO.File.ReadAllText(@"./Templates/EmailTemplate/BookRequest.html");
            emailBody = string.Format(emailBody,
                "Borrow Book Request",    //{0}
                userName                  //{1}
            );

            var mailData = new MailData
            {
                EmailToIds = new List<string> { userEmail },
                EmailCCIds = new List<string> { "deni.prasetyo@solecode.id" },
                EmailToName = userName,
                EmailSubject = "Welcome to Our Service!",
                EmailBody = emailBody
            };

            var emailSent = _emailService.SendMail(mailData);

            return bookRequest;
        }

        public async Task ApproveOrRejectBookRequestAsync(int processId, Process process)
        {
            var userRoles = _httpContextAccessor.HttpContext?.User.Claims
                  .Where(c => c.Type == ClaimTypes.Role)
                  .Select(c => c.Value)
                  .ToList();

            var user = _httpContextAccessor.HttpContext?.User.Identity!.Name;

            var currentUser = await _userManager.FindByNameAsync(user!);

            var userId = currentUser?.Id;

            var userName = currentUser?.UserName;

            var userEmail = currentUser?.Email;

            var existingProcess = await _context.Processes.Include(p => p.Requester).FirstOrDefaultAsync(cek => cek.Processid == processId);

            if (userRoles.Contains("Librarian"))
            {
                if (existingProcess != null)
                {
                    existingProcess.Status = process.Status;
                    if (process.Status == "Approve")
                    {
                        existingProcess.Currentstepid = 3;

                        var workflow = new Workflowaction   
                        {
                            Requestid = processId,
                            Stepid = 2,
                            Actorid = userId,
                            Action = "Approved",
                            Actiondate = DateTime.Now,
                            Comments = "Your Borrowing Book Request Approved By Librarian"
                        };

                        await _context.Workflowactions.AddAsync(workflow);
                        await _context.SaveChangesAsync();

                        var emailBody = System.IO.File.ReadAllText(@"./Templates/EmailTemplate/ApproveOrRejectBorrowingBookRequest.html");
                        emailBody = string.Format(emailBody,
                            "Borrowing Book Request",           //{0}
                            existingProcess.Requester.UserName, //{1}
                            "Approved",                         //{2}
                            userName                            //{3}
                        );

                        var mailData = new MailData
                        {
                            EmailToIds = new List<string> { existingProcess.Requester.Email },
                            EmailCCIds = new List<string> { "deni.prasetyo@solecode.id" },
                            EmailToName = existingProcess.Requester.UserName,
                            EmailSubject = "Welcome to Our Service!",
                            EmailBody = emailBody
                        };

                        var emailSent = _emailService.SendMail(mailData);
                    }

                    if (process.Status == "Reject")
                    {
                        existingProcess.Currentstepid = 5;

                        var workflow = new Workflowaction
                        {
                            Requestid = processId,
                            Stepid = 2,
                            Actorid = userId,
                            Action = "Rejected",
                            Actiondate = DateTime.Now,
                            Comments = "Your Borrowing Book Request Rejected By Librarian"
                        };

                        await _context.Workflowactions.AddAsync(workflow);
                        await _context.SaveChangesAsync();

                        var emailBody = System.IO.File.ReadAllText(@"./Templates/EmailTemplate/ApproveOrRejectBorrowingBookRequest.html");
                        emailBody = string.Format(emailBody,
                            "Borrowing Book Request",               //{0}
                            existingProcess.Requester.UserName,     //{1}
                            "Rejected",        //{2}
                            userName           //{3}
                        );

                        var mailData = new MailData
                        {
                            EmailToIds = new List<string> { existingProcess.Requester.Email },
                            EmailCCIds = new List<string> { "deni.prasetyo@solecode.id" },
                            EmailToName = existingProcess.Requester.UserName,
                            EmailSubject = "Welcome to Our Service!",
                            EmailBody = emailBody
                        };

                        var emailSent = _emailService.SendMail(mailData);
                    }

                    await _context.SaveChangesAsync();
                }
            }

            if (userRoles.Contains("Library Manager"))
            {
                if (existingProcess != null)
                {
                    existingProcess.Status = process.Status;
                    if (process.Status == "Approve")
                    {
                        existingProcess.Currentstepid = 5;

                        var workflow = new Workflowaction
                        {
                            Requestid = processId,
                            Stepid = 3,
                            Actorid = userId,
                            Action = "Approved",
                            Actiondate = DateTime.Now,
                            Comments = "Your Borrowing Book Request Approved By Library Manager"
                        };

                        await _context.Workflowactions.AddAsync(workflow);
                        await _context.SaveChangesAsync();

                        var emailBody = System.IO.File.ReadAllText(@"./Templates/EmailTemplate/ApproveOrRejectBorrowingBookRequest.html");
                        emailBody = string.Format(emailBody,
                            "Borrowing Book Request",               //{0}
                            existingProcess.Requester.UserName,     //{1}
                            "Approved",        //{2}
                            userName           //{3}
                        );

                        var mailData = new MailData
                        {
                            EmailToIds = new List<string> { existingProcess.Requester.Email },
                            EmailCCIds = new List<string> { "deni.prasetyo@solecode.id" },
                            EmailToName = existingProcess.Requester.UserName,
                            EmailSubject = "Welcome to Our Service!",
                            EmailBody = emailBody
                        };

                        var emailSent = _emailService.SendMail(mailData);
                    }

                    if (process.Status == "Reject")
                    {
                        existingProcess.Currentstepid = 5;

                        var workflow = new Workflowaction
                        {
                            Requestid = processId,
                            Stepid = 2,
                            Actorid = userId,
                            Action = "Rejected",
                            Actiondate = DateTime.Now,
                            Comments = "Your Borrowing Book Request Rejected By Library Manager"
                        };

                        await _context.Workflowactions.AddAsync(workflow);
                        await _context.SaveChangesAsync();

                        var emailBody = System.IO.File.ReadAllText(@"./Templates/EmailTemplate/ApproveOrRejectBorrowingBookRequest.html");
                        emailBody = string.Format(emailBody,
                            "Borrowing Book Request",               //{0}
                            existingProcess.Requester.UserName,     //{1}
                            "Rejected",        //{2}
                            userName           //{3}
                        );

                        var mailData = new MailData
                        {
                            EmailToIds = new List<string> { existingProcess.Requester.Email },
                            EmailCCIds = new List<string> { "deni.prasetyo@solecode.id" },
                            EmailToName = existingProcess.Requester.UserName,
                            EmailSubject = "Welcome to Our Service!",
                            EmailBody = emailBody
                        };

                        var emailSent = _emailService.SendMail(mailData);
                    }
                    await _context.SaveChangesAsync();
                }

            }
        }


    }
}
