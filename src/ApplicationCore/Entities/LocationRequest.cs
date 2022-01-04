using System.ComponentModel.DataAnnotations.Schema;

namespace LocationSearch.ApplicationCore.Entities
{
    [NotMapped]
    public class LocationRequest
    {
        public Location Location { get; set; }

        public int MaxDistance { get; set; } = 0;

        public int MaxResults { get; set; } = 20;
    }
}
