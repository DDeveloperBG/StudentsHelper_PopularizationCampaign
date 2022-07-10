using System.ComponentModel.DataAnnotations;

namespace PopularizationCampaign.ViewModels.Index
{
    public class ContactFormInputModel
    {
        [Required(ErrorMessage = "Моля, попълнете своя имейл.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Моля, попълнете дали сте учител или ученик.")]
        [EnumDataType(typeof(UserType))]
        public UserType? UserType { get; set; }
    }
}
