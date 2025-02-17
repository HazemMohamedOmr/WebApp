﻿using System.Collections;
using System.Text.Json.Serialization;

namespace WebApp.Core.DTOs;

public class AuthDTO
{
    public bool IsAuthenticated { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresOn { get; set; }
}