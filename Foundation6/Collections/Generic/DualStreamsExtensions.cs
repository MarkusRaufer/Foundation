using System;

namespace Foundation.Collections.Generic;

public static class DualStreamsExtensions
{
    /// <summary>
    /// Returns a new DualStreams object. Right of <paramref name="dualStreams"/> is set to the new right stream.
    /// Left of <paramref name="dualStreams"/> is filtered by <paramref name="predicate"/>.
    /// Matching items are added to the new right stream.
    /// If <paramref name="isExhaustive"/> is true, matching items are not added to the new left stream.
    /// </summary>
    /// <typeparam name="TLeft">type of left stream items.</typeparam>
    /// <typeparam name="TRight">type of right stream items.</typeparam>
    /// <param name="dualStreams"></param>
    /// <param name="predicate">filter of left stream items.</param>
    /// <param name="project"></param>
    /// <param name="isExhaustive">If true matching items are not added to the new left stream.</param>
    /// <returns></returns>
    public static DualStreams<TLeft, TRight> LeftToRight<TLeft, TRight>(
        this DualStreams<TLeft, TRight> dualStreams,
        Func<TLeft, bool> predicate,
        Func<TLeft, TRight> project,
        bool isExhaustive)
    {
        var streams = new DualStreams<TLeft, TRight>
        {
            Right = dualStreams.Right
        };

        foreach (var item in dualStreams.Left)
        {
            if (predicate(item))
            {
                streams.Right = streams.Right.Append(project(item));

                if (isExhaustive) continue;
            }

            streams.Left = streams.Left.Append(item);
        }

        return streams;
    }

    public static DualStreams<TLeft, TRight> MatchLeft<TLeft, TRight>(
            this DualStreams<TLeft, TRight> dualStreams,
            Func<TLeft, bool> predicate,
            Func<TLeft, TLeft> matching,
            Func<TLeft, TLeft> notMatching)
    {
        return dualStreams.MatchLeft<TLeft, TRight, TLeft>(predicate, matching, notMatching);
    }

    public static DualStreams<TLeftResult, TRight> MatchLeft<TLeft, TRight, TLeftResult>(
        this DualStreams<TLeft, TRight> dualStreams,
        Func<TLeft, bool> predicate,
        Func<TLeft, TLeftResult> matching,
        Func<TLeft, TLeftResult> notMatching)
    {
        var streams = new DualStreams<TLeftResult, TRight>
        {
            Right = dualStreams.Right
        };

        foreach (var item in dualStreams.Left)
        {
            streams.Left = predicate(item)
                ? streams.Left.Append(matching(item))
                : streams.Left.Append(notMatching(item));
        }

        return streams;
    }

    public static DualStreams<TLeft, TRight> MatchRight<TLeft, TRight>(
            this DualStreams<TLeft, TRight> dualStreams,
            Func<TRight, bool> predicate,
            Func<TRight, TRight> matching,
            Func<TRight, TRight> notMatching)
    {
        return dualStreams.MatchRight<TLeft, TRight, TRight>(predicate, matching, notMatching);
    }

    public static DualStreams<TLeft, TRightResult> MatchRight<TLeft, TRight, TRightResult>(
        this DualStreams<TLeft, TRight> dualStreams,
        Func<TRight, bool> predicate,
        Func<TRight, TRightResult> matching,
        Func<TRight, TRightResult> notMatching)
    {
        var streams = new DualStreams<TLeft, TRightResult>
        {
            Left = dualStreams.Left
        };

        foreach (var item in dualStreams.Right)
        {
            streams.Right = predicate(item)
                ? streams.Right.Append(matching(item))
                : streams.Right.Append(notMatching(item));
        }

        return streams;
    }

    public static IEnumerable<TRight> MergeAndSort<TLeft, TRight, TKey>(
            this DualStreams<TLeft, TRight> dualStreams,
            Func<TLeft, TRight> projection,
            Func<TRight, TKey> keySelector)
    {
        return dualStreams.MergeAndSort(projection, x => x, keySelector);
    }

    public static IEnumerable<TResult> MergeAndSort<TLeft, TRight, TResult, TKey>(
        this DualStreams<TLeft, TRight> dualStreams,
        Func<TLeft, TResult> leftProjection,
        Func<TRight, TResult> rightProjection,
        Func<TResult, TKey> keySelector)
    {
        dualStreams.ThrowIfNull();
        leftProjection.ThrowIfNull();
        rightProjection.ThrowIfNull();

        var results = Enumerable.Empty<TResult>();

        foreach (var item in dualStreams.Left)
        {
            results = results.Append(leftProjection(item));
        }

        foreach (var item in dualStreams.Right)
        {
            results = results.Append(rightProjection(item));
        }

        return results.OrderBy(keySelector);
    }

    /// <summary>
    /// Returns a new DualStreams object. Left of <paramref name="dualStreams"/> is set to the new left stream.
    /// Right of <paramref name="dualStreams"/> is filtered by <paramref name="predicate"/>.
    /// Matching items are added to the new left stream.
    /// If <paramref name="isExhaustive"/> is true, matching items are not added to the new right stream.
    /// </summary>
    /// <typeparam name="TLeft"></typeparam>
    /// <typeparam name="TRight"></typeparam>
    /// <param name="dualStreams"></param>
    /// <param name="predicate"></param>
    /// <param name="project"></param>
    /// <param name="isExhaustive"></param>
    /// <returns></returns>
    public static DualStreams<TLeft, TRight> RightToLeft<TLeft, TRight>(
        this DualStreams<TLeft, TRight> dualStreams,
        Func<TRight, bool> predicate,
        Func<TRight, TLeft> project,
        bool isExhaustive)
    {
        var streams = new DualStreams<TLeft, TRight>
        {
            Left = dualStreams.Left
        };

        foreach (var item in dualStreams.Right)
        {
            if (predicate(item))
            {
                streams.Left = streams.Left.Append(project(item));

                if (isExhaustive) continue;
            }

            streams.Right = streams.Right.Append(item);
        }

        return streams;
    }

    public static DualStreams<TLeftResult, TRight> SelectLeft<TLeft, TRight, TLeftResult>(
        this DualStreams<TLeft, TRight> dualStreams,
        Func<TLeft, bool> predicate,
        Func<TLeft, TLeftResult> project)
    {
        var streams = new DualStreams<TLeftResult, TRight>
        {
            Right = dualStreams.Right
        };

        foreach (var item in dualStreams.Left)
        {
            if (predicate(item))
            {
                streams.Left = streams.Left.Append(project(item));
            }
        }

        return streams;
    }

    public static DualStreams<TLeft, TRightResult> SelectRight<TLeft, TRight, TRightResult>(
        this DualStreams<TLeft, TRight> dualStreams,
        Func<TRight, bool> predicate,
        Func<TRight, TRightResult> project)
    {
        var streams = new DualStreams<TLeft, TRightResult>
        {
            Left = dualStreams.Left
        };

        foreach (var item in dualStreams.Right)
        {
            if (predicate(item))
            {
                streams.Right = streams.Right.Append(project(item));
            }
        }

        return streams;
    }
}
