using System;

namespace namasdev.Core.Entity
{
    public abstract class Entity<TId> : IEntity<TId>
        where TId : IEquatable<TId>
    {
		public TId Id { get; set; }
    }
}
