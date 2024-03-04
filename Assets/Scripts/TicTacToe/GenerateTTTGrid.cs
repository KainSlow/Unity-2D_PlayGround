using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTTTGrid : MonoBehaviour
{

    [SerializeField] int width, height, linesSize;

    public Dictionary<Vector2, TTTNode> _tiles { get; set; }
    void Start()
    {

    }

    void Update()
    {
        
    }
}


public class TTTNode : MonoBehaviour
{
    [SerializeField] Color _Blank, _X, _O;
    [SerializeField] SpriteRenderer spriteRenderer;

}

enum TTTValues
{
    X = 1,
    O = -1,
    Blank = 0,
}
