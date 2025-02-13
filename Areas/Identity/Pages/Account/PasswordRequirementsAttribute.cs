using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace COMP4870Assignment1.Areas.Identity.Pages.Account;

public class PasswordRequirementsAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
    {
        string? password = value as string;

        if (string.IsNullOrEmpty(password))
        {
            return new ValidationResult("Password is required.");
        }

        var errors = new List<string>();

        if (!Regex.IsMatch(password, @"[A-Z]")) errors.Add("At least one uppercase letter is required.");
        if (!Regex.IsMatch(password, @"[a-z]")) errors.Add("At least one lowercase letter is required.");
        if (!Regex.IsMatch(password, @"\d")) errors.Add("At least one numeric digit is required.");
        if (!Regex.IsMatch(password, @"[\W_]")) errors.Add("At least one special character is required.");
        if (password.Length < 8) errors.Add("Password must be at least 8 characters long.");

        return errors.Count > 0 ? new ValidationResult(string.Join(" ", errors)) : ValidationResult.Success;
    }
}

