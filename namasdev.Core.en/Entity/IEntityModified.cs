using System;

namespace namasdev.Core.Entity
{
    public interface IEntityModified
    {
        string ModifiedBy { get; set; }
        DateTime ModifiedAt { get; set; }
    }
}
