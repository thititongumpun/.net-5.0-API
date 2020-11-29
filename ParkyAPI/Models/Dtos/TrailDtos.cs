using System.ComponentModel.DataAnnotations;
using static ParkyAPI.Models.Dtos.NationalParkDtos;
using static ParkyAPI.Models.Dtos.Trail;

namespace ParkyAPI.Models.Dtos
{
        public class TrailDtos
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Distance { get; set; }
        public DifficultyType Difficulty { get; set; }
        [Required]
        public int NationalParkId { get; set; }
        public NationalParkDtos NationalPark { get; set; }

    }
}