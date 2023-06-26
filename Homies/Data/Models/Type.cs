namespace Homies.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using static Common.GlobalConstants;

    public class Type
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(TypeNameMaxLength)]
        public string Name { get; set; } = null!;

        public IEnumerable<Event> Events { get; set; } = new List<Event>();        
    }
}
