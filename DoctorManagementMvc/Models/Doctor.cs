using System.ComponentModel.DataAnnotations;

namespace DoctorManagementMvc.Models
{
    public class Doctor
    {
        [Required, StringLength(20)]
        public string Code { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Specialization { get; set; } = string.Empty;

        [Range(0, int.MaxValue)]
        public int Availability { get; set; }
    }
}
