namespace ELearn.Web.ViewModels.Users
{
    using System.ComponentModel.DataAnnotations;

    using ELearn.Data.Models;
    using ELearn.Services.Mapping;

    public class EditUserViewModel : IMapFrom<ApplicationUser>
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Role { get; set; }
    }
}