﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ToStringDotNet
{
    /// <summary>
    /// Formats objects as strings, basing on <see cref="ToStringAttribute"/> and <see cref="ToStringInheritanceAttribute"/> placement.
    /// </summary>
    public static class ToStringFormatter
    {
        private static readonly IDictionary<Type, object> formatter = new Dictionary<Type, object>();
        private static readonly Type tStringBuilder = typeof(StringBuilder);
        private static readonly MethodInfo mAppendString = ToStringFormatter.tStringBuilder.GetMethod("Append", new[] {typeof(string)});

        /// <summary>
        /// Prints the object as a <see cref="string"/>, according to <see cref="ToStringAttribute"/> placement on its fields and properties
        /// </summary>
        /// <typeparam name="T">Type of the input object</typeparam>
        /// <param name="entity">Object to print</param>
        /// <returns><see cref="string"/> representation of the input object</returns>
        /// <exception cref="ToStringException">If the input object can't be printed for some reason</exception>
        public static string Format<T>(T entity)
        {
            if (entity == null) return "null";
            var sb = new StringBuilder();
            ToStringFormatter.Format(sb, entity);
            return sb.ToString();
        }

        private static void Format<T>(StringBuilder sb, T entity)
        {
            if (entity == null)
            {
                sb.Append("null");
                return;
            }

            if (!ToStringFormatter.formatter.TryGetValue(typeof(T), out object printer))
            {
                printer = ToStringFormatter.BuildFormatter<T>();
                ToStringFormatter.formatter[typeof(T)] = printer;
            }

            ((Action<StringBuilder, T>) printer).Invoke(sb, entity);
        }

        private static Action<StringBuilder, T> BuildFormatter<T>()
        {
            Type type = typeof(T);
            var sb = Expression.Parameter(ToStringFormatter.tStringBuilder, "sb");
            var entity = Expression.Parameter(type, "entity");
            Expression expr = ToStringFormatter.BuildPropertyExpression(type, sb, entity);
            return Expression.Lambda<Action<StringBuilder, T>>(expr, sb, entity).Compile();
        }

        private static Expression BuildPropertyExpression(Type type, Expression sb, Expression entity)
        {
            if (ToStringFormatter.BuildPredefinedExpression(type, sb, entity, out Expression expr))
                return expr;

            expr = type.GetMethod("GetEnumerator", BindingFlags.Public | BindingFlags.Instance) == null
                ? ToStringFormatter.BuildObjectExpression(type, sb, entity)
                : ToStringFormatter.BuildEnumerableExpression(type, sb, entity);

            return expr;
        }

        private static Expression BuildObjectExpression(Type type, Expression sb, Expression entity)
        {
            ToStringProp[] props = ToStringFormatter.CollectDebugProps(type);

            var isNull = Expression.Equal(entity, Expression.Constant(null));

            var ifNull = Expression.Call(sb, ToStringFormatter.mAppendString, Expression.Constant("null"));

            var ifNotNull = new LinkedList<Expression>();
            ifNotNull.AddLast(Expression.Call(sb, ToStringFormatter.mAppendString, Expression.Constant("{")));
            for (var index = 0; index < props.Length; index++)
            {
                ToStringProp prop = props[index];
                ifNotNull.AddLast(Expression.Call(sb, ToStringFormatter.mAppendString, Expression.Constant("\"")));
                ifNotNull.AddLast(Expression.Call(sb, ToStringFormatter.mAppendString, Expression.Constant(prop.Name)));
                ifNotNull.AddLast(Expression.Call(sb, ToStringFormatter.mAppendString, Expression.Constant("\":")));

                Expression propExpression = ToStringFormatter.BuildPropertyExpression(prop.PropType, sb, prop.Access(entity));
                ifNotNull.AddLast(propExpression);

                if (index < props.Length - 1)
                    ifNotNull.AddLast(Expression.Call(sb, ToStringFormatter.mAppendString, Expression.Constant(",")));
            }

            ifNotNull.AddLast(Expression.Call(sb, ToStringFormatter.mAppendString, Expression.Constant("}")));

            return Expression.IfThenElse(isNull, ifNull, Expression.Block(ifNotNull));
        }

        private static Expression BuildEnumerableExpression(Type type, Expression sb, Expression entity)
        {
            Type elementType = type.GenericTypeArguments.Length > 0
                ? type.GenericTypeArguments[0]
                : type.GetElementType();

            if (elementType == null)
                throw new ToStringException($"Collections of type [{type.FullName}] are not supported");

            MethodInfo method = typeof(ToStringFormatter).GetMethod("WriteEnumerable", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(elementType);
            return Expression.Call(method, sb, entity);
        }

        private static bool BuildPredefinedExpression(Type type, Expression sb, Expression entity, out Expression expression)
        {
            MethodInfo method = typeof(ToStringFormatter).GetMethod("WriteValue", BindingFlags.NonPublic | BindingFlags.Static, null, new[] {ToStringFormatter.tStringBuilder, type}, new ParameterModifier[0]);
            if (method != null)
            {
                expression = Expression.Call(method, sb, entity);
                return true;
            }

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            expression = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            return false;
        }

        private static ToStringProp[] CollectDebugProps(Type type)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            if (type.GetCustomAttribute<ToStringInheritanceAttribute>()?.Inheritance == ToStringInheritance.NotInherit)
                flags |= BindingFlags.DeclaredOnly;

            IEnumerable<ToStringProp> props = type
                                              .GetProperties(flags)
                                              .Where(p => p.GetCustomAttribute<ToStringAttribute>() != null)
                                              .Select(p => new ToStringProp(p, p.GetCustomAttribute<ToStringAttribute>().Priority));

            IEnumerable<ToStringProp> fields = type
                                               .GetFields(flags)
                                               .Where(f => f.GetCustomAttribute<ToStringAttribute>() != null)
                                               .Select(f => new ToStringProp(f, f.GetCustomAttribute<ToStringAttribute>().Priority));

            var members = props.Concat(fields)
                               .OrderByDescending(p => p.Priority)
                               .ThenBy(p => p.Name)
                               .ToArray();

            if (members.Length == 0)
                throw new ToStringException($"Could not find any [{typeof(ToStringAttribute).FullName}] on [{type.FullName}] members");

            return members;
        }

        private static void WriteValue(StringBuilder sb, string value)
        {
            if (value == null)
                sb.Append("null");
            else if (value == string.Empty)
                sb.Append("\"\"");
            else
                sb.Append("\"").Append(value).Append("\"");
        }

        private static void WriteValue(StringBuilder sb, TimeSpan value)
        {
            sb.Append("\"").Append(value.ToString("g")).Append("\"");
        }

        private static void WriteValue(StringBuilder sb, DateTime value)
        {
            sb.Append("\"").Append(value.ToString("O")).Append("\"");
        }

        private static void WriteValue(StringBuilder sb, sbyte value)
        {
            sb.Append(value);
        }

        private static void WriteValue(StringBuilder sb, byte value)
        {
            sb.Append(value);
        }

        private static void WriteValue(StringBuilder sb, short value)
        {
            sb.Append(value);
        }

        private static void WriteValue(StringBuilder sb, int value)
        {
            sb.Append(value);
        }

        private static void WriteValue(StringBuilder sb, long value)
        {
            sb.Append(value);
        }

        private static void WriteValue(StringBuilder sb, float value)
        {
            sb.Append(value);
        }

        private static void WriteValue(StringBuilder sb, double value)
        {
            sb.Append(value);
        }

        private static void WriteValue(StringBuilder sb, decimal value)
        {
            sb.Append(value);
        }

        private static void WriteValue(StringBuilder sb, ushort value)
        {
            sb.Append(value);
        }

        private static void WriteValue(StringBuilder sb, uint value)
        {
            sb.Append(value);
        }

        private static void WriteValue(StringBuilder sb, ulong value)
        {
            sb.Append(value);
        }

        private static void WriteValue(StringBuilder sb, bool value)
        {
            sb.Append(value);
        }

        private static void WriteEnumerable<T>(StringBuilder sb, IEnumerable<T> value)
        {
            if (value == null)
            {
                sb.Append("null");
                return;
            }

            if (!ToStringFormatter.formatter.TryGetValue(typeof(T), out object printer))
            {
                printer = ToStringFormatter.BuildFormatter<T>();
                ToStringFormatter.formatter[typeof(T)] = printer;
            }

            var act = (Action<StringBuilder, T>) printer;
            sb.Append("[");

            foreach (T item in value)
            {
                act(sb, item);
                sb.Append(",");
            }

            sb.Remove(sb.Length - 1, 1);

            sb.Append("]");
        }
    }
}