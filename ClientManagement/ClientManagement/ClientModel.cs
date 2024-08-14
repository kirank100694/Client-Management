using System.ComponentModel.DataAnnotations;

namespace ClientManagement
{
    public class ClientModel
    {
        public int ClientId { get; set; }

        [Required]
        public Guid LicenceKey { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "LastName cannot be longer than 50 characters.")]
        public string ClientName { get; set; } = null!;

        [Required]
        public DateTime LicenceStartDate { get; set; }

        [Required]
        public DateTime LicenceEndDate { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
