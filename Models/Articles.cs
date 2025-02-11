using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace COMP4870Assignment1.Models;

public class Articles
{
    [Key]
    public int ArticleId { get; set; }

    public string? Title { get; set; }

    public string? Body { get; set; }

    public DateTime CreateDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public string? UserId { get; set; }

    [ForeignKey("UserId")]
    public CustomUser? User { get; set; }

    public string? Email { get; set; }
}

