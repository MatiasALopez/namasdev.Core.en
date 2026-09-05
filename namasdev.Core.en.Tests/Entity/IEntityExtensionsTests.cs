using System;
using System.Collections.Generic;
using namasdev.Core.Entity;
using Xunit;

namespace namasdev.Core.en.Tests.Entity
{
    public class IEntityExtensionsTests
    {
        private class TestEntity : IEntityCreated, IEntityModified, IEntityDeleted
        {
            public string CreatedBy { get; set; } = string.Empty;
            public DateTime CreatedAt { get; set; }
            public string ModifiedBy { get; set; } = string.Empty;
            public DateTime ModifiedAt { get; set; }
            public string DeletedBy { get; set; } = string.Empty;
            public DateTime? DeletedAt { get; set; }
            public bool Deleted { get; set; }
        }

        private class CreatedOnlyEntity : IEntityCreated
        {
            public string CreatedBy { get; set; } = string.Empty;
            public DateTime CreatedAt { get; set; }
        }

        // SetCreated

        [Fact]
        public void SetCreated_SingleEntity_SetsAuditFields()
        {
            var entity = new TestEntity();
            var now = new DateTime(2024, 1, 15, 10, 0, 0);

            entity.SetCreated("user1", now);

            Assert.Equal("user1", entity.CreatedBy);
            Assert.Equal(now, entity.CreatedAt);
        }

        [Fact]
        public void SetCreated_EntityAlsoImplementsIEntityModified_SetsModifiedFields()
        {
            var entity = new TestEntity();
            var now = new DateTime(2024, 1, 15, 10, 0, 0);

            entity.SetCreated("user1", now);

            Assert.Equal("user1", entity.ModifiedBy);
            Assert.Equal(now, entity.ModifiedAt);
        }

        [Fact]
        public void SetCreated_EntityWithoutIEntityModified_DoesNotThrow()
        {
            var entity = new CreatedOnlyEntity();
            var now = DateTime.UtcNow;

            entity.SetCreated("user1", now);

            Assert.Equal("user1", entity.CreatedBy);
        }

        [Fact]
        public void SetCreated_NullEntity_Throws()
        {
            IEntityCreated entity = null!;
            Assert.Throws<ArgumentNullException>(() => entity.SetCreated("user", DateTime.UtcNow));
        }

        [Fact]
        public void SetCreated_Collection_SetsAllEntities()
        {
            var entities = new List<IEntityCreated>
            {
                new CreatedOnlyEntity(),
                new CreatedOnlyEntity()
            };
            var now = DateTime.UtcNow;

            entities.SetCreated("admin", now);

            foreach (var e in entities)
            {
                Assert.Equal("admin", e.CreatedBy);
                Assert.Equal(now, e.CreatedAt);
            }
        }

        // SetModified

        [Fact]
        public void SetModified_SingleEntity_SetsAuditFields()
        {
            var entity = new TestEntity();
            var now = new DateTime(2024, 6, 1, 12, 0, 0);

            entity.SetModified("editor", now);

            Assert.Equal("editor", entity.ModifiedBy);
            Assert.Equal(now, entity.ModifiedAt);
        }

        [Fact]
        public void SetModified_NullEntity_Throws()
        {
            IEntityModified entity = null!;
            Assert.Throws<ArgumentNullException>(() => entity.SetModified("user", DateTime.UtcNow));
        }

        // SetDeleted

        [Fact]
        public void SetDeleted_SingleEntity_SetsAuditFields()
        {
            var entity = new TestEntity();
            var now = new DateTime(2024, 6, 1, 12, 0, 0);

            entity.SetDeleted("remover", now);

            Assert.Equal("remover", entity.DeletedBy);
            Assert.Equal(now, entity.DeletedAt);
            Assert.True(entity.Deleted);
        }

        [Fact]
        public void SetDeleted_NullEntity_Throws()
        {
            IEntityDeleted entity = null!;
            Assert.Throws<ArgumentNullException>(() => entity.SetDeleted("user", DateTime.UtcNow));
        }

        [Fact]
        public void SetDeleted_Collection_SetsAllEntities()
        {
            var entities = new List<IEntityDeleted>
            {
                new TestEntity(),
                new TestEntity()
            };
            var now = DateTime.UtcNow;

            entities.SetDeleted("admin", now);

            foreach (var e in entities)
            {
                Assert.Equal("admin", e.DeletedBy);
                Assert.Equal(now, e.DeletedAt);
                Assert.True(e.Deleted);
            }
        }

        // UnsetDeleted

        [Fact]
        public void UnsetDeleted_SingleEntity_ClearsAuditFields()
        {
            var entity = new TestEntity();
            entity.SetDeleted("remover", new DateTime(2024, 6, 1, 12, 0, 0));

            entity.UnsetDeleted();

            Assert.Null(entity.DeletedBy);
            Assert.Null(entity.DeletedAt);
            Assert.False(entity.Deleted);
        }

        [Fact]
        public void UnsetDeleted_EntityNotDeleted_LeavesFieldsCleared()
        {
            var entity = new TestEntity();

            entity.UnsetDeleted();

            Assert.Null(entity.DeletedBy);
            Assert.Null(entity.DeletedAt);
            Assert.False(entity.Deleted);
        }

        [Fact]
        public void UnsetDeleted_NullEntity_Throws()
        {
            IEntityDeleted entity = null!;
            Assert.Throws<ArgumentNullException>(() => entity.UnsetDeleted());
        }

        [Fact]
        public void UnsetDeleted_Collection_ClearsAllEntities()
        {
            var entities = new List<IEntityDeleted>
            {
                new TestEntity(),
                new TestEntity()
            };
            entities.SetDeleted("admin", DateTime.UtcNow);

            entities.UnsetDeleted();

            foreach (var e in entities)
            {
                Assert.Null(e.DeletedBy);
                Assert.Null(e.DeletedAt);
                Assert.False(e.Deleted);
            }
        }

        [Fact]
        public void UnsetDeleted_NullCollection_Throws()
        {
            IList<IEntityDeleted> entities = null!;
            Assert.Throws<ArgumentNullException>(() => entities.UnsetDeleted());
        }
    }
}
