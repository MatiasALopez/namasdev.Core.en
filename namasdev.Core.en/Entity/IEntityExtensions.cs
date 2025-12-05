using System;
using System.Collections.Generic;

using namasdev.Core.Validation;

namespace namasdev.Core.Entity
{
    public static class IEntityExtensions
    {
        /// <summary>
        /// Sets created audit data.
        /// This operation only sets the instance's values; changes must be persisted after.
        /// </summary>
        public static void SetCreated(this IEnumerable<IEntityCreated> entities, string user, DateTime dateTime)
        {
            Validator.ValidateRequiredArgumentAndThrow(entities, nameof(entities));

            foreach (var e in entities)
            {
                e.SetCreated(user, dateTime);
            }
        }

        /// <summary>
        /// Sets created audit data.
        /// This operation only sets the instance's values; changes must be persisted after.
        /// </summary>
        public static void SetCreated(this IEntityCreated entity, string user, DateTime dateTime)
        {
            Validator.ValidateRequiredArgumentAndThrow(entity, nameof(entity));

            entity.CreatedBy = user;
            entity.CreatedAt = dateTime;

            var entityModified = entity as IEntityModified;
            if (entityModified != null)
            {
                entityModified.ModifiedBy = user;
                entityModified.ModifiedAt = dateTime;
            }
        }

        /// <summary>
        /// Sets modified audit data.
        /// This operation only sets the instance's values; changes must be persisted after.
        /// </summary>
        public static void SetModified(this IEnumerable<IEntityModified> entities, string user, DateTime dateTime)
        {
            Validator.ValidateRequiredArgumentAndThrow(entities, nameof(entities));

            foreach (var e in entities)
            {
                e.SetModified(user, dateTime);
            }
        }

        /// <summary>
        /// Sets modified audit data.
        /// This operation only sets the instance's values; changes must be persisted after.
        /// </summary>
        public static void SetModified(this IEntityModified entity, string user, DateTime dateTime)
        {
            Validator.ValidateRequiredArgumentAndThrow(entity, nameof(entity));

            entity.ModifiedBy = user;
            entity.ModifiedAt = dateTime;
        }

        /// <summary>
        /// Sets deleted audit data.
        /// This operation only sets the instance's values; changes must be persisted after.
        /// </summary>
        public static void SetDeleted(this IList<IEntityDeleted> entities, string user, DateTime dateTime)
        {
            Validator.ValidateRequiredArgumentAndThrow(entities, nameof(entities));

            foreach (var entity in entities)
            {
                entity.SetDeleted(user, dateTime);
            }
        }

        /// <summary>
        /// Sets deleted audit data.
        /// This operation only sets the instance's values; changes must be persisted after.
        /// </summary>
        public static void SetDeleted(this IEntityDeleted entity, string user, DateTime dateTime)
        {
            Validator.ValidateRequiredArgumentAndThrow(entity, nameof(entity));

            entity.DeletedBy = user;
            entity.DeletedAt = dateTime;
        }
    }
}
