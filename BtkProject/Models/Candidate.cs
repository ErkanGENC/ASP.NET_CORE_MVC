using System.ComponentModel.DataAnnotations;

namespace BtkProject.Models
{
public class Candidate
{
    public int? age { get; set; }
    [Required(ErrorMessage = "First Name is required")]
    public string? firstName { get; set; } = string.Empty;
    [Required(ErrorMessage = "Last Name is required")]
    public string? lastName { get; set; } = string.Empty;
    public string? fullName => $"{firstName} {lastName?.ToUpper()}";
    [Required(ErrorMessage = "Email is required")]
    public string? email { get; set; } = string.Empty;
    public string? selectedCourse { get; set; } = string.Empty;
    public DateTime applyAt{get;set;}

    public Candidate()
    {
        applyAt=DateTime.Now;
    }

  }
}