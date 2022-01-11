using System;

namespace Foundation.Collections.Generic
{
    public static class DualStreamsExtensions
    {
        /// <summary>
        /// Filters the left stream and adds the matching items to the right stream. If isExhaustive it true then matching items are not added to the left stream.
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <param name="dualStreams"></param>
        /// <param name="predicate"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        public static DualStreams<TLeft, TRight> FilterLeft<TLeft, TRight>(
            this DualStreams<TLeft, TRight> dualStreams,
            Func<TLeft, bool> predicate,
            Func<TLeft, TRight> project)
        {
            foreach (var item in dualStreams.Left)
            {
                if (predicate(item)) dualStreams.Right = dualStreams.Right.Append(project(item));
            }

            return dualStreams;
        }

        public static DualStreams<TLeft, TRight> FilterRight<TLeft, TRight>(
            this DualStreams<TLeft, TRight> dualStreams,
            Func<TRight, bool> predicate,
            Func<TRight, TLeft> project)
        {
            foreach (var item in dualStreams.Right)
            {
                if (predicate(item)) dualStreams.Left = dualStreams.Left.Append(project(item));
            }

            return dualStreams;
        }
    }
}
