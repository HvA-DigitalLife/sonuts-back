﻿using Template.Application.Common.Mappings;
using Template.Domain.Entities;

namespace Template.Application.Common.Models;

// Note: This is currently just used to demonstrate applying multiple IMapFrom attributes.
public class LookupDto : IMapFrom<TodoList>, IMapFrom<TodoItem>
{
	public int Id { get; set; }

	public string? Title { get; set; }
}
