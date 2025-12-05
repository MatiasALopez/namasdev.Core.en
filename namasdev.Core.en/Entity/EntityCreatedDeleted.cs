using System;

namespace namasdev.Core.Entity
{
    public abstract class EntityCreatedDeleted<TId> : EntityCreated<TId>, IEntityDeleted
        where TId : IEquatable<TId>
    {
        public string DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool Deleted { get; set; }
    }
}
