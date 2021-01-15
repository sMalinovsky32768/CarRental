using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Models
{
    public class RentalListModel
    {
        public IEnumerable<Rental> Rentals { get; set; }
        public RentalFiltersModel Filters { get; set; }
    }

    public class RentalFiltersModel
    {
        [DisplayName("Sort by")]
        public RentalSort SortBy { get; set; }

        [DisplayName("Sort order")]
        public SortOrder SortOrder { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }
    }

    public enum RentalSort
    {
        Default,
        //[Description("Client")]
        //ClientId,
        //[Description("Car")]
        //CarId,
        //[Description("Contract number")]
        //ContractNumber,
        //[Description("Price")]
        //FullPrice,
        [Description("Start date and time")]
        StartDateTime,
        [Description("End date and time")]
        EndDateTime,
        //[Description("Rental duration in days")]
        //NumberOfDays,
    }

    public enum SortOrder
    {
        Asc,
        Desc
    }
}
