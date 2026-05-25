using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class AnchorHolder : MonoBehaviour, IAnchorable
{
    [SerializeField] private List<Vector2> anchors;

    public Vector2 GetAnchor(int index)
    {
        return anchors[index];
    }
}
