using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using System.Reflection;
using Tabarru.Repositories.Models;

namespace Tabarru.Repositories.DatabaseContext
{
    internal static class DbContextProfile
    {
        public static void AddModelCreatingProfile(this ModelBuilder builder)
        {
            foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableEntityType entityType in builder.Model.GetEntityTypes())
            {
                if (entityType.FindProperty(nameof(EntityMetaData.CreatedDate)) == null)
                {
                    entityType.AddProperty(nameof(EntityMetaData.CreatedDate), typeof(DateTimeOffset));
                }

                if (entityType.FindProperty(nameof(EntityMetaData.UpdatedDate)) == null)
                {
                    entityType.AddProperty(nameof(EntityMetaData.UpdatedDate), typeof(DateTimeOffset));
                }

                if (entityType.ClrType.CustomAttributes.FirstOrDefault(x => x.AttributeType.Equals(typeof(EntitySoftDeleteAbleAttribute))) != null)
                {
                    if (entityType.FindProperty("IsDeleted") == null)
                    {
                        entityType.AddProperty("IsDeleted", typeof(DateTimeOffset));
                    }
                    ParameterExpression parameter = Expression.Parameter(entityType.ClrType);
                    MethodInfo propertyMethodInfo = typeof(EF).GetMethod("Property").MakeGenericMethod(typeof(bool));
                    MethodCallExpression isDeletedProperty = Expression.Call(propertyMethodInfo, parameter, Expression.Constant("IsDeleted"));
                    BinaryExpression compareExpression = Expression.MakeBinary(ExpressionType.Equal, isDeletedProperty, Expression.Constant(false));
                    LambdaExpression lambda = Expression.Lambda(compareExpression, parameter);
                    builder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
        }

        public static void DetectChangesAndUpdate(this ChangeTracker changeTracker)
        {
            changeTracker.DetectChanges();
            IEnumerable<EntityEntry> entries = changeTracker.Entries();

            foreach (EntityEntry item in entries)
            {
                if (item.State == EntityState.Deleted)
                {
                    if (item.Property("IsDeleted") != null)
                    {
                        item.State = EntityState.Unchanged;
                        item.Property("IsDeleted").CurrentValue = true;
                    }
                }

                else if (item.State == EntityState.Modified)
                {
                    item.Property(nameof(EntityMetaData.UpdatedDate)).CurrentValue = DateTimeOffset.UtcNow;
                }

                else if (item.State == EntityState.Added)
                {
                    item.Property(nameof(EntityMetaData.CreatedDate)).CurrentValue = DateTimeOffset.UtcNow;
                    item.Property(nameof(EntityMetaData.UpdatedDate)).CurrentValue = DateTimeOffset.UtcNow;
                }
            }
        }
    }
}
