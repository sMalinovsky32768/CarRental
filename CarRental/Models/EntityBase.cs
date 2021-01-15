using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Models
{
    public record EntityBase<TKey>
    {
        /// <summary>
        /// Возвращает или задает идентификатор.
        /// </summary>
        [Column("id")]
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TKey Id { get; set; }
    }
}
