﻿using System.Text.Json.Serialization;
using WebApp.Core.Interfaces;

namespace WebApp.Core.Models;

public abstract class BaseEntity<T> : IBaseEntity<T>, ISoftDeletable
{
    public T Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}