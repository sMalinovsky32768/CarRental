using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Models
{
    /// <summary>
    /// Клиент
    /// </summary>
    [Table("clients")]
    public record Client : EntityBase<long>
    {
        /// <summary>
        /// Имя
        /// </summary>
        [Column("first_name")]
        [Required]
        [DisplayName("First name")]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        [Column("second_name")]
        [DisplayName("Second name")]
        [StringLength(60, MinimumLength = 2)]
        public string SecondName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [Column("last_name")]
        [Required]
        [DisplayName("Last name")]
        [StringLength(60, MinimumLength = 2)]
        public string LastName { get; set; }

        /// <summary>
        /// Серия и номер паспорта (без пробелов)
        /// </summary>
        [Column("passport")]
        [Required]
        [StringLength(10, MinimumLength = 10)]
        [RegularExpression(@"^\d{10}$")]
        public string Passport { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        [Column("date_of_birth")]
        [Required]
        [DisplayName("Date of birth")]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        [Column("phone")]
        [Required]
        [Phone]
        [RegularExpression(@"^\+?\d{1,3}\s?(\d{3}|\(\d{3}\))(\s|\-)?\d{3}(\s|\-)?\d{2}(\s|\-)?\d{2}$")]
        public string Phone { get; set; }

        /// <summary>
        /// Список сделок с участием клиента
        /// </summary>
        public List<Rental> Rentals { get; set; }

        public string FullName => $"{LastName} {FirstName} {SecondName}";
    }
}
