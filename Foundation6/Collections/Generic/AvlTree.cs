using System.Collections;

namespace Foundation.Collections.Generic;

internal class AvlTreeNode<T>(T value)
    where T : notnull, IComparable<T>
{
    public int Count { get; set; } = 1;  // Initialize count as 1 for a new value
    public int Height { get; set; } = 1; // New nodes are initially added as leaf nodes
    public AvlTreeNode<T>? Left { get; set; }
    public AvlTreeNode<T>? Right { get; set; }
    public T Value { get; set; } = value;

    public override string ToString() => $"{Value}";
}

public class AvlTree<T> : IEnumerable<T>
    where T : notnull, IComparable<T>
{
    private AvlTreeNode<T>? _root;

    private static AvlTreeNode<T>? Balance(AvlTreeNode<T>? node)
    {
        if (null == node) return null;

        int balanceFactor = BalanceFactor(node);
        if (balanceFactor > 1)
        {
            if (BalanceFactor(node.Left) > 0)
            {
                node = RotateLeftLeft(node);
            }
            else
            {
                node = RotateLeftRight(node);
            }
        }
        else if (balanceFactor < -1)
        {
            if (BalanceFactor(node.Right) > 0)
            {
                node = RotateRightLeft(node);
            }
            else
            {
                node = RotateRightRight(node);
            }
        }
        return node;
    }

    private static int BalanceFactor(AvlTreeNode<T>? node)
    {
        if (null == node) return 0;

        var l = GetHeight(node.Left);
        var r = GetHeight(node.Right);
        return l - r;
    }

    public void Clear() => _root = null;

    public bool Contains(T value) => FindNode(_root, value) != null;

    private static AvlTreeNode<T>? FindNode(AvlTreeNode<T>? node, T value)
    {
        if (node is null || value is null) return null;

        var compareResult = value.CompareTo(node.Value);
        if (0 == compareResult) return node;

        return compareResult < 0 ? FindNode(node.Left, value) : FindNode(node.Right, value);
    }

    public IEnumerator<T> GetEnumerator() => GetSortedElements(_root).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetSortedElements(_root).GetEnumerator();

    private static int GetHeight(AvlTreeNode<T>? node)
    {
        int height = 0;
        if (null != node)
        {
            int l = GetHeight(node.Left);
            int r = GetHeight(node.Right);
            int max = Math.Max(l, r);
            height = max + 1;
        }
        return height;
    }

    private IEnumerable<T> GetSortedElements(AvlTreeNode<T>? node)
    {
        if (node == null) yield break;

        foreach (var left in GetSortedElements(node.Left))
            yield return left;

        yield return node.Value;

        foreach (var right in GetSortedElements(node.Right))
            yield return right;
    }

    public void Insert(T value)
    {
        var newItem = new AvlTreeNode<T>(value);
        _root = null == _root ? newItem : InsertNode(_root, newItem);
    }

    private AvlTreeNode<T>? InsertNode(AvlTreeNode<T>? node, AvlTreeNode<T> n)
    {
        if (node == null)
        {
            node = n;
            return node;
        }
        else if (n.Value.CompareTo(node.Value) == -1)
        {
            node.Left = InsertNode(node.Left, n);
            node = Balance(node);
        }
        else if (n.Value.CompareTo(node.Value) == 1)
        {
            node.Right = InsertNode(node.Right, n);
            node = Balance(node);
        }
        return node;
    }

    public void Remove(T value)
    {
        _root = RemoveNode(_root, value);
    }

    private AvlTreeNode<T>? RemoveNode(AvlTreeNode<T>? node, T value)
    {
        AvlTreeNode<T> parent;
        if (node == null) return null;

        //left subtree
        if (value.CompareTo(node.Value) == -1)
        {
            node.Left = RemoveNode(node.Left, value);
            if (BalanceFactor(node) == -2)//here
            {
                if (BalanceFactor(node.Right) <= 0)
                {
                    node = RotateRightRight(node);
                }
                else
                {
                    node = RotateRightLeft(node);
                }
            }
        }
        //right subtree
        else if (value.CompareTo(node.Value) == 1)
        {
            node.Right = RemoveNode(node.Right, value);
            if (BalanceFactor(node) == 2)
            {
                if (BalanceFactor(node.Left) >= 0)
                {
                    node = RotateLeftLeft(node);
                }
                else
                {
                    node = RotateLeftRight(node);
                }
            }
        }
        //if value is found
        else
        {
            if (node.Right != null)
            {
                //delete its inorder successor
                parent = node.Right;
                while (parent.Left != null)
                {
                    parent = parent.Left;
                }
                node.Value = parent.Value;
                node.Right = RemoveNode(node.Right, parent.Value);
                if (BalanceFactor(node) == 2)//rebalancing
                {
                    if (BalanceFactor(node.Left) >= 0)
                    {
                        node = RotateLeftLeft(node);
                    }
                    else { node = RotateLeftRight(node); }
                }
            }
            else
            {   //if node.left != null
                return node.Left;
            }
        }
        return node;
    }

    private static AvlTreeNode<T>? RotateRightRight(AvlTreeNode<T>? node)
    {
        if (null == node) return null;

        var pivot = node.Right;
        if (null != pivot)
        {
            node.Right = pivot.Left;
            pivot.Left = node;
        }
        return pivot;
    }

    private static AvlTreeNode<T>? RotateLeftLeft(AvlTreeNode<T>? node)
    {
        if (null == node) return null;

        var pivot = node.Left;
        if (null != pivot)
        {
            node.Left = pivot.Right;
            pivot.Right = node;
        }
        return pivot;
    }

    private static AvlTreeNode<T>? RotateLeftRight(AvlTreeNode<T>? node)
    {
        if (null == node) return null;

        var pivot = node.Left;
        node.Left = RotateRightRight(pivot);
        return RotateLeftLeft(node);
    }

    private static AvlTreeNode<T>? RotateRightLeft(AvlTreeNode<T>? node)
    {
        if (null == node) return null;

        var pivot = node.Right;
        node.Right = RotateLeftLeft(pivot);
        return RotateRightRight(node);
    }
}
