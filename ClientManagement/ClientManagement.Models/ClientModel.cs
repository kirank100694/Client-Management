using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ClientManagement.Models
{
    public class ClientModel
    {
        public int ClientId { get; set; }

        [Required(ErrorMessage = "LicenceKey must be in uniquekey formateonly ")]
        public Guid LicenceKey { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "ClientName cannot be longer than 50 characters.")]
        public string ClientName { get; set; } = null!;

        [Required]
        public DateTime LicenceStartDate { get; set; }

        [Required]
        public DateTime LicenceEndDate { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(200)]
        public string Description { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
