using System.Collections.Generic;
using UnityEngine;

public interface IPathfinding
{
    List<Node> FindPath(Vector3 startPos, Vector3 targetPos);
}