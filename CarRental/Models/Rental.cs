using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Models
{
    /// <summary>
    /// Сделка по аренде автомобиля
    /// </summary>
    [Table("rentals")]
    public record Rental : EntityBase<long>
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
        /// Идентификатор автомобиля
        /// </summary>
        [Column("car_id")]
        [Required]
        public long CarId { get; set; }

        /// <summary>
        /// Автомобиль
        /// </summary>
        public Car Car { get; set; }

        /// <summary>
        /// Номер договора
        /// </summary>
        [Column("contract_number")]
        [Required]
        [DisplayName("Contract number")]
        [MaxLength(50)]
        public string ContractNumber { get; set; }

        /// <summary>
        /// Полная стоимость аренды
        /// </summary>
        [Column("full_price")]
        [DisplayName("Full price")]
        public decimal FullPrice { get; set; }

        /// <summary>
        /// Дата и время начала аренды
        /// </summary>
        [Column("start_datetime")]
        [Required]
        [DisplayName("Start date and time")]
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// Дата и время окончания аренды
        /// </summary>
        [Column("end_datetime")]
        [DisplayName("End date and time")]
        public DateTime EndDateTime { get; }

        /// <summary>
        /// Длительность аренды в сутках
        /// </summary>
        [Column("number_of_days")]
        [Required]
        [DisplayName("Rental duration in days")]
        public int NumberOfDays { get; set; }

        /// <summary>
        /// Штрафы полученные во время аренды
        /// </summary>
        public List<Fine> Fines { get; set; }

        /// <summary>
        /// ДТП совершенные во время аренды
        /// </summary>
        public List<RoadAccidient> RoadAccidients { get; set; }
    }
}
