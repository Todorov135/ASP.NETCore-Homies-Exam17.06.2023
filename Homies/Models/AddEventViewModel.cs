namespace Homies.Models
{
    using System.ComponentModel.DataAnnotations;
    using static Common.GlobalConstants;

    public class AddEventViewModel
    {
        [Required]
        [StringLength(EventNameMaxLength, MinimumLength = EventNameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(EventDescriptionMaxLength, MinimumLength = EventDescriptionMinLength)]
        public string Description { get; set; } = null!;       
      

        [Required]
        public string Start { get; set; } = null!;

        [Required]
        public string End { get; set; } = null!;

        public IEnumerable<TypeViewModel> Types { get; set; } = new List<TypeViewModel>();
        public int TypeId { get; set; }
        
        public int Id { get; set; }

    }
}
