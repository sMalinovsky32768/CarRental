using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Models
{
    /// <summary>
    /// Скидка клиента
    /// </summary>
    [Table("discounts")]
    public record Discount : EntityBase<long>
    {
        /// <summary>
        /// Идентификатор клиента
        /// </summary>
        [Column("client_id")]
        [Required]
        public long ClientId { get; set; }

        /// <summary>
        /// Клиент
        /// </summary>
        public Client Client { get; set; }

        /// <summary>
        /// Дата начала действия скидки
        /// </summary>
        [Column("start_datetime")]
        [Required]
        [DisplayName("Start date")]
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// Дата окончания действия скидки (null, если бессрочно)
        /// </summary>
        [Column("end_datetime")]
        [DisplayName("End date")]
        public DateTime? EndDateTime { get; set; }

        /// <summary>
        /// Размер скидки в процентах
        /// </summary>
        [Column("amount")]
        [Required]
        [Range(0, 100)]
        public byte Amount { get; set; }
    }
}
