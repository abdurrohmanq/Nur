﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nur.Application.UseCases.Users.Commands;
using Nur.Application.UseCases.Users.Queries.GetUsers;
using Nur.WebAPI.Controllers.Commons;
using Nur.WebAPI.Models;

namespace Nur.WebAPI.Controllers.Users;

public class UsersController(IMediator mediator) : BaseController
{
    [HttpPost("create")]
    public async Task<IActionResult> PostAsync(UserCreateCommand command, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(command, cancellationToken) });
    
    [HttpPut("update")]
    public async Task<IActionResult> ModifyAsync(UserUpdateCommand command, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(command, cancellationToken) });

    [HttpDelete("delete/{id:long}")]
    public async Task<IActionResult> DeleteAsync(long id, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new UserDeleteCommand(id), cancellationToken)});

    [HttpGet("get/{telegramId:long}")]
    public async Task<IActionResult> GetAsync(long id, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new GetUserQuery(id), cancellationToken) });

    [HttpGet("get-by-telegram-id/{id:long}")]
    public async Task<IActionResult> GetByTelegramIdAsync(long telegramId, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new GetUserByTelegramIdQuery(telegramId), cancellationToken) });

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken) 
        => Ok(new Response { Data = await mediator.Send(new GetAllUsersQuery(), cancellationToken) });
}
