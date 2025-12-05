using System;

namespace namasdev.Core.Entity
{
    public interface IEntity<TId>
        where TId : IEquatable<TId>
    {
        TId Id { get; set; }
    }
}
