﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Application.TodoLists.Commands.CreateTodoList;
using Template.Application.TodoLists.Commands.DeleteTodoList;
using Template.Application.TodoLists.Commands.UpdateTodoList;
using Template.Application.TodoLists.Queries.ExportTodos;
using Template.Application.TodoLists.Queries.GetTodos;

namespace Template.Presentation.Controllers;

[Authorize]
public class TodoListsController : ApiControllerBase
{
	[HttpGet]
	public async Task<ActionResult<TodosVm>> Get()
	{
		return await Mediator.Send(new GetTodosQuery());
	}

	[HttpGet("{id:int}")]
	public async Task<FileResult> Get(int id)
	{
		var vm = await Mediator.Send(new ExportTodosQuery { ListId = id });

		return File(vm.Content, vm.ContentType, vm.FileName);
	}

	[HttpPost]
	public async Task<ActionResult<int>> Create(CreateTodoListCommand command)
	{
		return await Mediator.Send(command);
	}

	[HttpPut("{id:int}")]
	public async Task<ActionResult> Update(int id, UpdateTodoListCommand command)
	{
		if (id != command.Id)
		{
			return BadRequest();
		}

		await Mediator.Send(command);

		return NoContent();
	}

	[HttpDelete("{id:int}")]
	public async Task<ActionResult> Delete(int id)
	{
		await Mediator.Send(new DeleteTodoListCommand(id));

		return NoContent();
	}
}
