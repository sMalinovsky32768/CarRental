using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Models
{
    /// <summary>
    /// ДТП
    /// </summary>
    [Table("road_accidents")]
    public record RoadAccidient : EntityBase<long>
    {
        /// <summary>
        /// Идентификатор сделки, во время которой произошло ДТП
        /// </summary>
        [Column("rental_id")]
        [Required]
        public long RentalId { get; set; }

        /// <summary>
        /// Сделка, во время которой произошло ДТП
        /// </summary>
        public Rental Rental { get; set; }

        /// <summary>
        /// Дата ДТП
        /// </summary>
        [Column("date")]
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Идентификатор протокола ГИБДД
        /// </summary>
        [Column("traffic_police_protocol_id")]
        [Required]
        [DisplayName("The Protocol of the traffic police")]
        [MaxLength(50)]
        public string TrafficPoliceProtocolId { get; set; }
    }
}
