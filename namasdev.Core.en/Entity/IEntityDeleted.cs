using System;

namespace namasdev.Core.Entity
{
    public interface IEntityDeleted
    {
        string DeletedBy { get; set; }
        DateTime? DeletedAt { get; set; }
        bool Deleted { get; set; }
    }
}
