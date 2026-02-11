using FitLog.Models;
using FitLog.Models.DatabaseEntities;
using Microsoft.AspNetCore.Identity;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace FitLog.Data
{
    public static class ApplicationDbContextExtensions
    {
        public static void TryUpdateCollection<T, TKey>(this ApplicationDbContext context, IEnumerable<T> originalCollection, IEnumerable<T> updatedCollection, Func<T, TKey> keySelector, params Expression<Func<T, object>>[] itemKeySelectors) where T : class
        {
            updatedCollection = updatedCollection ?? new List<T>();
            var toRemove = originalCollection.Except(updatedCollection, keySelector);
            context.RemoveRange(toRemove);
            var toAdd = updatedCollection.Except(originalCollection, keySelector);
            context.AddRange(toAdd);

            if (itemKeySelectors != null && itemKeySelectors.Any())
            {
                var toUpdate = originalCollection
                    .Join(updatedCollection, keySelector, keySelector, (original, updated) => new { original, updated });
                foreach (var items in toUpdate)
                {
                    context.TryUpdate(items.original, items.updated, itemKeySelectors);
                }
            }
        }

        public static IEnumerable<T> Except<T, TKey>(this IEnumerable<T> source, IEnumerable<T> other, Func<T, TKey> keySelector)
        {
            return source.Where(s => !other.Select(o => keySelector(o)).Contains(keySelector(s)));
        }

        public static void TryUpdate<T>(this ApplicationDbContext context, T model, T updatedModel, params Expression<Func<T, object>>[] keySelectors)
        {
            foreach (var keySelector in keySelectors)
            {
                var newValue = keySelector.Compile()(updatedModel);
                var prop = (PropertyInfo)GetMemberExpression(keySelector).Member;
                prop.SetValue(model, newValue, null);
            }
        }

        private static MemberExpression GetMemberExpression<T>(Expression<Func<T, object>> expression)
        {
            var member = expression.Body as MemberExpression;
            var unary = expression.Body as UnaryExpression;

            return member ?? (unary == null ? null : unary.Operand as MemberExpression);
        }

        public static T RemoveNullsFromCollections<T>(this T entity)
        {
            var properties = typeof(T)
                .GetProperties()
                .Where(i => i.PropertyType.GetTypeInfo().IsGenericType
                && i.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>));
            foreach (var property in properties)
            {
                if (property.GetValue(entity) is IList items)
                {
                    for (int i = items.Count - 1; i >= 0; i--)
                    {
                        if (items[i] == null)
                        {
                            items.RemoveAt(i);
                        }
                    }
                }
            }

            return entity;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }
    }
}
