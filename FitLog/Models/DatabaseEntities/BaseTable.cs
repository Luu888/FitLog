using System.ComponentModel.DataAnnotations;

namespace FitLog.Models.DatabaseEntities
{
    public abstract class BaseTable
    {
        public int Id { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public string CreatedByUserId { get; set; }
        public ApplicationUser CreatedByUser { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }
        public string? UpdatedByUserId { get; set; }
        public ApplicationUser? UpdatedByUser { get; set; }

        public bool IsDeleted { get; set; }
    }
}
