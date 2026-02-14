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

        public void SetupCreatedFields(string userId, DateTimeOffset? now = null)
        {
            now ??= DateTimeOffset.Now;

            CreatedAt = now.Value;
            CreatedByUserId = userId;
            UpdatedAt = null;
            UpdatedByUserId = null;
        }

        public void SetupUpdatedFields(string userId, DateTimeOffset? now = null)
        {
            now ??= DateTimeOffset.Now;

            UpdatedAt = now.Value;
            UpdatedByUserId = userId;
        }
    }
}
