using System;

namespace namasdev.Core.Entity
{
    public abstract class EntityCreated<TId> : Entity<TId>, IEntityCreated
        where TId : IEquatable<TId>
    {
		public string CreatedBy { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
