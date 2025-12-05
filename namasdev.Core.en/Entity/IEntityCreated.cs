using System;

namespace namasdev.Core.Entity
{
    public interface IEntityCreated
    {
        string CreatedBy { get; set; }
        DateTime CreatedAt { get; set; }
    }
}
