using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Foundation.Collections.Generic
{
    public static class DualOrdinalStreamsExtensions
    {
        /// <summary>
        /// Filters the left stream and adds the matching items to the right stream. If isExhaustive is true then matching items are not added to the left stream.
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <param name="dualStreams"></param>
        /// <param name="predicate">Filter of left stream.</param>
        /// <param name="project">Projection from left to right stream.</param>
        /// <param name="isExhaustive">If isExhaustive is true then matching items are not added to the left stream.</param>
        /// <returns></returns>
        public static DualOrdinalStreams<TLeft, TRight> FilterLeft<TLeft, TRight>(
            this DualOrdinalStreams<TLeft, TRight> dualStreams,
            [DisallowNull] Func<TLeft, bool> predicate,
            [DisallowNull] Func<TLeft, TRight> project,
            bool isExhaustive)
        {
            predicate.ThrowIfNull(nameof(predicate));
            project.ThrowIfNull(nameof(project));

            var filteredTuple = new DualOrdinalStreams<TLeft, TRight>
            {
                Right = dualStreams.Right
            };

            foreach (var left in dualStreams.Left)
            {
                if (predicate(left.Value))
                {
                    var right = new Ordinal<TRight> { Position = left.Position, Value = project(left.Value) };

                    filteredTuple.Right = filteredTuple.Right.Append(right);

                    if (isExhaustive) continue;
                }

                filteredTuple.Left = filteredTuple.Left.Append(left);
            }

            return filteredTuple;
        }

        /// <summary>
        /// Merge Left and Right stream to one stream. If element with same position exists in boths streams, the element from Right is taken.
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <param name="dualStreams"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        public static IEnumerable<TRight> MergeStreams<TLeft, TRight>(
            this DualOrdinalStreams<TLeft, TRight> dualStreams, 
            [DisallowNull] Func<TLeft, TRight> project)
        {
            project.ThrowIfNull(nameof(project));

            Opt<TRight> toRightValue(Opt<TLeft> opt)
            {
                return opt.IsSome ? Opt.Some(project(opt.ValueOrThrow())) : Opt.None<TRight>();
            }

            var left = dualStreams.Left.OrdinalFill().Select(toRightValue);
            var right = dualStreams.Right.OrdinalFill();

            var itLeft = left.GetEnumerator();
            var itRight = right.GetEnumerator();
            while (true)
            {
                var hasNextLeft = itLeft.MoveNext();
                var hasNextRight = itRight.MoveNext();

                if (!hasNextLeft && !hasNextRight) break;

                if (!hasNextLeft)
                {
                    if (itRight.Current.IsSome) yield return itRight.Current.ValueOrThrow();
                    continue;
                }

                if (!hasNextRight)
                {
                    if (itLeft.Current.IsSome) yield return itLeft.Current.ValueOrThrow();
                    continue;
                }

                if (itLeft.Current.IsSome)
                {
                    if (itRight.Current.IsSome)
                    {
                        yield return itRight.Current.ValueOrThrow();
                        continue;
                    }

                    yield return itLeft.Current.ValueOrThrow();
                    continue;
                }

                if (itRight.Current.IsSome) yield return itRight.Current.ValueOrThrow();
            }
        }

        public static DualOrdinalStreams<TLeft, TRight> OrderByPosition<TLeft, TRight>(this DualOrdinalStreams<TLeft, TRight> dualStreams)
        {
            static IEnumerable<Ordinal<T>> sort<T>(IEnumerable<Ordinal<T>> items)
            {
                foreach (var item in items.OrderBy(o => o.Position))
                {
                    yield return item;
                }
            }

            return new DualOrdinalStreams<TLeft, TRight>
            {
                Left = sort(dualStreams.Left),
                Right = sort(dualStreams.Right)
            };
        }
    }
}
