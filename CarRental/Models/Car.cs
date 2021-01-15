using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Models
{
    /// <summary>
    /// Автомобиль
    /// </summary>
    [Table("cars")]
    public record Car : EntityBase<long>
    {
        /// <summary>
        /// Марка автомобиля
        /// </summary>
        [Column("brand")]
        [Required]
        [MaxLength(50)]
        public string Brand { get; set; }

        /// <summary>
        /// Модель автомобиля
        /// </summary>
        [Column("model")]
        [Required]
        [MaxLength(50)]
        public string Model { get; set; }

        /// <summary>
        /// Гос. номер автомобиля
        /// </summary>
        [Column("state_number")]
        [Required]
        [DisplayName("State number")]
        [StringLength(10)]
        public string StateNumber { get; set; }

        /// <summary>
        /// Является ли грузовым
        /// </summary>
        [Column("is_truck")]
        [Required]
        [DisplayName("Is truck")]
        public bool IsTruck { get; set; } = false;

        /// <summary>
        /// Стоимость аренды в сутки
        /// </summary>
        [Column("rental_price_per_day")]
        [Required]
        [DisplayName("Rental price per day")]
        public decimal RentalPricePerDay { get; set; }

        /// <summary>
        /// Список сделок с участием автомобиля
        /// </summary>
        public List<Rental> Rentals { get; set; }

        public string DisplayName => $"{Brand} {Model} ({StateNumber})";
    }
}
