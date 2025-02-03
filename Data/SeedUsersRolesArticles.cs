using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using COMP4870Assignment1.Models;


public class SeedUsersRolesArticles {
    private readonly List<CustomRole> _roles;
    private readonly List<CustomUser> _users;
    private readonly List<IdentityUserRole<string>> _userRoles;
    private readonly List<Articles> _articles;

    public SeedUsersRolesArticles() {
        _roles = GetRoles();
        _users = GetUsers();
        _userRoles = GetUserRoles(_users, _roles);
        _articles = GetArticles();
    }

    public List<CustomRole> Roles { get { return _roles; } }
    public List<CustomUser> Users { get { return _users; } }
    public List<IdentityUserRole<string>> UserRoles { get { return _userRoles; } }
    public List<Articles> Articles { get { return _articles; } }

    private List<CustomRole> GetRoles() {
        var adminRole = new CustomRole("admin", "Role for admin", DateTime.UtcNow);
        var contributorRole = new CustomRole("contributor", "Role for contributor", DateTime.UtcNow);
        return new List<CustomRole> { adminRole, contributorRole };
    }

    private List<CustomUser> GetUsers() {
        string pwd = "P@$$w0rd";
        var passwordHasher = new PasswordHasher<CustomUser>();

        var adminUser = new CustomUser {
            UserName = "a@a.a",
            Email = "a@a.a",
            EmailConfirmed = true,
            FirstName = "Admin",
            LastName = "User"
        };
        adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, pwd);

        var contributorUser = new CustomUser {
            UserName = "c@c.c",
            Email = "c@c.c",
            EmailConfirmed = true,
            FirstName = "Contributor",
            LastName = "User"
        };
        contributorUser.PasswordHash = passwordHasher.HashPassword(contributorUser, pwd);

        return new List<CustomUser> { adminUser, contributorUser };
    }

    private List<IdentityUserRole<string>> GetUserRoles(List<CustomUser> users, List<CustomRole> roles) {
        return new List<IdentityUserRole<string>> {
            new IdentityUserRole<string> { UserId = users[0].Id, RoleId = roles.First(q => q.Name == "admin").Id },
            new IdentityUserRole<string> { UserId = users[1].Id, RoleId = roles.First(q => q.Name == "contributor").Id }
        };
    }

    private List<Articles> GetArticles() {
        return new List<Articles> {
            new Articles {
                ArticleId = 1,
                Title = "New fires erupt in southern California",
                Body = "Five new fires have erupted in southern California. The blazes - named Laguna, Sepulveda, Gibbel, Gilman and Border 2 - flared up on Thursday in the counties",
                CreateDate = new DateTime(2024, 12, 21),
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(30),
                UserId = Users[1].Id,
                Email = Users[1].Email
            },
        };
    }
}
