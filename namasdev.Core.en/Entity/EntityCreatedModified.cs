using System;

namespace namasdev.Core.Entity
{
    public abstract class EntityCreatedModified<TId> : EntityCreated<TId>, IEntityModified
        where TId : IEquatable<TId>
    {
		public string ModifiedBy { get; set; }
		public DateTime ModifiedAt { get; set; }
	}
}
