using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;




public class Node :IHeapItem<Node>
{
    public Vector2 Position { get; private set; }
    public Vector2Int PositionInMatrix { get; private set; }
    public bool IsObstacle { get; private set; }
    public float G { get; private set; }
    public float H { get; private set; }
    public float F { get; private set; }
    public Node Parent { get; private set; }

    int heapIndex;
    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public Node(Vector2 position, Vector2Int positionInMatrix, bool isObstacle) 
    {
        Position = position;
        PositionInMatrix = positionInMatrix;
        G = 0;
        H = float.PositiveInfinity;
        Parent = null;
        IsObstacle = isObstacle;
        SetF(1);
    }

    public void UpdateIsObstacle(bool value)
    {
        IsObstacle = value;
    }

    public override string ToString()
    {
        return PositionInMatrix.ToString();
    }
    public void SetParent(Node parent)
    {
        Parent = parent;
    }

    public void SetG(float value)
    {
        G = value;
    }
    public void SetG(Vector2 from, Vector2 to, Heuristic.Type HeuristicType) 
    {
        G = Heuristic.CalculateHeuristic(from, to, HeuristicType);
    }

    public void SetH(Vector2 from, Vector2 goal, Heuristic.Type HeuristicType) 
    {
        H = Heuristic.CalculateHeuristic(from, goal, HeuristicType);
    }

    public void SetF(float weight)
    {
        F = G + weight*H;
    }

    public void ResetVariables()
    {
        G = 0;
        //H = float.PositiveInfinity;
        SetF(1);
        Parent = null;
    }

    public float GetF()
    {
        return G + H;
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = GetF().CompareTo(nodeToCompare.GetF());
        if (compare == 0)
        {
            compare = H.CompareTo(nodeToCompare.H);
        }
        return -compare;
    }
}
public static class Astar
{
    public static List<Vector3> FindPath(GridManager gridManager, Vector2Int bounds, Vector2 start , Vector2 goal, float weight, Heuristic.Type heuristicType)
    {
        Stopwatch sw = new();
        sw.Start();

        HashSet<Node> closed = new(); //Visited
        Heap<Node> open = new(bounds.x * bounds.y); //No Visited

        SetHeuristic(gridManager._tiles, goal, heuristicType);

        Node startNode = gridManager.GetTileAtPosition(start).Node;
        Node endNode = gridManager.GetTileAtPosition(goal).Node;

        open.Add(startNode);

        while(open.Count > 0)
        {
            var currentNode = open.RemoveFirst();

            closed.Add(currentNode);

            if (currentNode == endNode)
            {
                sw.Stop();
                UnityEngine.Debug.Log($"OpenCount:{open.Count}, ClosedCount:{closed.Count}");
                UnityEngine.Debug.Log("Path Found in: "+ sw.ElapsedMilliseconds + "ms");

                return RetrievePath(currentNode);
            }

            foreach (var neighbor in GetNeighbors(gridManager, bounds, currentNode, heuristicType))
            {
                if (closed.Contains(neighbor) || neighbor.IsObstacle) continue;

                var newG = currentNode.G + Heuristic.CalculateHeuristic(currentNode.PositionInMatrix, neighbor.PositionInMatrix, heuristicType);

                if(!open.Contains(neighbor) || newG <= neighbor.G)
                {
                    neighbor.SetG(newG);
                    neighbor.SetH(neighbor.PositionInMatrix, goal, heuristicType);
                    neighbor.SetF(weight);

                    neighbor.SetParent(currentNode);

                    if (!open.Contains(neighbor))
                    {
                        open.Add(neighbor);
                    }
                }
            }
        }
        
        //NoPath
        return null;

    }

    private static void SetHeuristic(Dictionary<Vector2, Tile> tiles, Vector2 goal, Heuristic.Type hType)
    {
        foreach(var tile in tiles)
        {
            tile.Value.Node.ResetVariables();
            tile.Value.Node.SetH(tile.Value.Node.PositionInMatrix, goal, hType);
        }
    }


    private static List<Node> GetNeighbors(GridManager gridManager,Vector2Int bounds, Node current, Heuristic.Type heuristicType)
    {
        List < Node > neighbors = new();

        if(heuristicType == Heuristic.Type.Manhattan)
        {
            Vector2Int[] dirs = { new( 0,-1 ), new(-1,0), new(1,0), new(0,1) }; 

            foreach(var dir in dirs)
            {
                var nextPos = current.PositionInMatrix + dir;

                if (nextPos.x >= 0 && nextPos.x < bounds.x &&
                   nextPos.y >= 0 && nextPos.y < bounds.y)
                   neighbors.Add(gridManager.GetTileAtPosition(nextPos).Node);
            }

        }
        else
        {
            Vector2Int[] dirs = { new(-1,-1), new(0, -1), new(1,-1),
                                  new(-1, 0), new(1, 0), 
                                  new(-1, 1), new(0, 1),  new(1,1)};

            foreach (var dir in dirs)
            {
                var nextPos = current.PositionInMatrix + dir;

                if (nextPos.x >= 0 && nextPos.x < bounds.x &&
                   nextPos.y >= 0 && nextPos.y < bounds.y)
                    neighbors.Add(gridManager.GetTileAtPosition(nextPos).Node);
            }

        }


        return neighbors;
    }
    public static List<Vector3> RetrievePath(Node current) 
    {
        List<Vector3> path = new();

        while(current != null)
        {
            path.Insert(0, current.Position);
            current = current.Parent;
        }

        return path;
    }
    private static Node FindLowestFNode(HashSet<Node> open)
    {
        Node lowestFNode = null;

        foreach(Node node in open)
        {
            if(lowestFNode == null)
            {
                lowestFNode = node;
                continue;
            }

            if(node.GetF() < lowestFNode.GetF() || node.GetF() == lowestFNode.GetF() && node.H < lowestFNode.H)
            {
                lowestFNode = node;
            }
        }

        return lowestFNode;
    }
}
public class Heap<T> where T : IHeapItem<T>
{
    T[] items;
    int currentItemCount;

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    public T RemoveFirst()
    {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public int Count
    {
        get
        {
            return currentItemCount;
        }
    }

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    void SortDown(T item)
    {
        while (true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            if (childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft;

                if (childIndexRight < currentItemCount)
                {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }

                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
                }
                else
                {
                    return;
                }

            }
            else
            {
                return;
            }

        }
    }

    void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}
public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}
public static class Heuristic
{
    public enum Type
    {
        Euclidian,
        Manhattan,
        Chessboard,
        Octile,
        Custom
    };

    public static float CalculateHeuristic(Vector2 from, Vector2 to, Heuristic.Type HeuristicType)
    {
        float value;
        switch (HeuristicType)
        {
            case Heuristic.Type.Octile:

                value = Heuristic.OctileHeuristic(from, to);

                break;
            case Heuristic.Type.Chessboard:

                value = Heuristic.ChessboardHeuristic(from, to);

                break;
            case Heuristic.Type.Manhattan:

                value = Heuristic.ManhattanHeuristic(from, to);
                break;

            case Heuristic.Type.Custom:

                value = Heuristic.CustomHeuristic(from, to);
                break;

            default:

                value = Heuristic.EuclidianHeuristic(from, to);
                break;
        }

        return value;
    }

    private static float EuclidianHeuristic(Vector2 from, Vector2 to)
    {
        var dx = to.x - from.x;
        var dy = to.y - from.y;

        return Mathf.Sqrt((dx * dx) + (dy * dy));
    }

    private static float ManhattanHeuristic(Vector2 from, Vector2 to) 
    {
        var dx = Mathf.Abs(to.x - from.x);
        var dy = Mathf.Abs(to.y - from.y);

        return dx + dy;
    }

    private static float ChessboardHeuristic(Vector2 from, Vector2 to)
    {
        var D = 1;
        var D2 = 1;  
        var dx = Mathf.Abs(to.x - from.x);
        var dy = Mathf.Abs(to.y - from.y);

        return D * (dx + dy) + (D2 - 2 * D) * Mathf.Min(dx, dy);
    }

    private static float OctileHeuristic(Vector2 from, Vector2 to)
    {
        var D = 1;
        var D2 = 1.41421356237f;
        var dx = Mathf.Abs(to.x - from.x);
        var dy = Mathf.Abs(to.y - from.y);

        return D * (dx + dy) + (D2 - 2 * D) * Mathf.Min(dx, dy);
    }

    private static int CustomHeuristic(Vector2 from, Vector2 to)
    {
        int dstX = (int)Mathf.Abs(from.x - to.x);
        int dstY = (int)Mathf.Abs(from.y - to.y);

        if(dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }

        return 14 * dstX + 10 * (dstY - dstX);

    }
}


