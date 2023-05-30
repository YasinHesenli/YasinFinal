using System.ComponentModel.DataAnnotations.Schema;

namespace YasinFinal.Models
{
    public class Team
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Position { get; set; }

        public string? Image { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
       
    }
}
