using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Models
{
    /// <summary>
    /// Штраф
    /// </summary>
    [Table("fines")]
    public record Fine : EntityBase<long>
    {
        /// <summary>
        /// Идентификатор сделки, во время которой выписан штраф
        /// </summary>
        [Column("rental_id")]
        [Required]
        public long RentalId { get; set; }

        /// <summary>
        /// Сделка, во время которой выписан штраф
        /// </summary>
        public Rental Rental { get; set; }

        /// <summary>
        /// Размер штрафа
        /// </summary>
        [Column("amount")]
        [Required]
        public byte Amount { get; set; }

        /// <summary>
        /// Описание/причина
        /// </summary>
        [Column("description")]
        [MaxLength(200)]
        public string Description { get; set; }
    }
}
