using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridManager : MonoBehaviour
{
    [SerializeField] private Heuristic.Type heuristicType;

    [SerializeField] private int _width, _height;

    [SerializeField] private float _weight;

    [SerializeField] private Tile _tilePrefab;

    [SerializeField] Transform _cam;

    [SerializeField] private LineRenderer _lineRenderer;

    public Dictionary<Vector2, Tile> _tiles { get; set; }

    Tile start;
    Tile end;

    List<Vector3> path;

    private void Start()
    {
        start = null;
        end = null;

        path = new();

        _cam = Camera.main.transform;

        GenerateGrid();

    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {

            path = Astar.FindPath(this, new(_width, _height), start.Node.Position, end.Node.Position, _weight, heuristicType);
            
            DrawPath();

        }
    }

    void GenerateGrid()
    {
        _tiles = new();
        for(int x = 0; x < _width; x++)
        {
            for(int y = 0;y < _height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} / {y}";

                var isOffset = (x + y) % 2 == 1;

                spawnedTile.Init(isOffset);
                spawnedTile.CreateNode(new Vector2Int(x, y));
                spawnedTile.SetGrid(this);

                _tiles[new(x, y)] = spawnedTile;
            }
        }

        _cam.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10f);

    }

    private void DrawPath()
    {
        if(path != null) 
        { 
            _lineRenderer.positionCount = path.Count;
            _lineRenderer.SetPositions(path.ToArray());
            _lineRenderer.startWidth = 0.2f;
            _lineRenderer.endWidth = 0.2f;
        }
    }

    public void SetStart(Tile newStart)
    {
        start = newStart;
    }

    public void SetEnd(Tile newEnd)
    {
        end = newEnd;
    }

    public Tile GetStart()
    {
        if(start != null)
        {
            return start;
        }

        return null;
    }

    public Tile GetEnd()
    {
        if(end != null)
        {
            return end;
        }

        return null;
    }

    public Tile GetTileAtPosition(Vector2 position)
    {
        if(_tiles.TryGetValue(position, out var tile))
        {
            return tile;
        }

        return null;
    }
}
