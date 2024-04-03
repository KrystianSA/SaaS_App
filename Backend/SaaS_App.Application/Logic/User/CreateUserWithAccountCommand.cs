﻿using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SaaS_App.Application.Exceptions;
using SaaS_App.Application.Interfaces;
using SaaS_App.Application.Logic.Abstractions;
using SaaS_App.Application.Logic.User.Helpers;
using SaaS_App.Domain.Entities;

namespace SaaS_App.Application.Logic.User
{
    public static class CreateUserWithAccountCommand
    {
        public class Request : IRequest<Result>
        {
            public required string Name { get; set; }
            public required string Surname { get; set; }
            public required string Email { get; set; }
            public required string Password { get; set; }
        }
        public class Result
        {
            public required int UserId { get; set; }
            public required string Email { get; set; }
            public required string Subject { get; set; }
            public required string ActivateAccountLink { get; set; }
        }

        public class Handler : BaseCommandHandler, IRequestHandler<Request, Result>
        {
            private readonly IPasswordManager _passwordManager;
            public const int LENGTH_TOKEN = 128;
            public Handler(IApplicationDbContext dbContext,
                ICurrentAccountProvider currentAccountProvider,
                IPasswordManager passwordManager) : base(dbContext, currentAccountProvider)
            {
                _passwordManager = passwordManager;
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var userEmail = await _dbContext.Users.AnyAsync(email => email.Email == request.Email);

                if (userEmail)
                {
                    throw new ErrorException("AccountWithThisEmailAlreadyExists");
                }

                var utcNow = DateTime.UtcNow;
                var newUser = new Domain.Entities.User()
                {
                    Name = request.Name,
                    Surname = request.Surname,
                    Email = request.Email,
                    HashedPassword = "",
                    RegisterDate = utcNow,
                };
                newUser.HashedPassword = _passwordManager.HashPassword(request.Password);

                _dbContext.Users.Add(newUser);

                var newAccount = new Domain.Entities.Account()
                {
                    Name = request.Email,
                    CreateDate = utcNow,
                };

                _dbContext.Accounts.Add(newAccount);

                var newAccountUser = new AccountUser()
                {
                    Account = newAccount,
                    User = newUser
                };

                _dbContext.AccountUser.Add(newAccountUser);

                var hashedToken = TokenUtils.GenerateToken(LENGTH_TOKEN).GenerateHash();

                var tokenModel = new Tokens()
                {
                    UserId = request.Email,
                    Token = hashedToken,
                    Token_Expiry = DateTime.UtcNow.AddDays(1),
                };

                await _dbContext.Tokens.AddAsync(tokenModel);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return new Result()
                {
                    UserId = newUser.Id,
                    Email = request.Email,
                    Subject = "Link To Activate Account",
                    ActivateAccountLink = 
                    $"http://localhost:3000/activate-account?token={hashedToken}&email={request.Email}"
                };
            }
        }
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(email => email.Email)
                    .NotEmpty()
                    .EmailAddress()
                    .MaximumLength(100);


                RuleFor(password => password.Password)
                   .NotEmpty()
                   .MaximumLength(50);
            }
        }
    }
}
