using System;
using Microsoft.AspNetCore.Identity;

namespace COMP4870Assignment1.Models;

public class CustomUser : IdentityUser {
  public CustomUser() : base() { }

  public string? FirstName { get; set; }
  public string? LastName { get; set; }
}