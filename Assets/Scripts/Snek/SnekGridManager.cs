using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SnekGridManager : MonoBehaviour
{
    [SerializeField] int width, height;
    [SerializeField] Vector2 tileSize;
    [SerializeField] SnekTile tilePrefab;
    public Dictionary<Vector2, SnekTile> tiles;
    public EventHandler OnTypeChanged;

    Vector2 startPosition;
    public void Initialize()
    {
        tiles = new();
        startPosition = new(transform.position.x - (tileSize.x * width * .5f), transform.position.y + (tileSize.y * height * .5f));
        GenGrid();
    }

    void GenGrid()
    {
        Vector2 position = startPosition;

        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                var spawnedTile = Instantiate(tilePrefab, new Vector3(position.x,position.y), Quaternion.identity, transform);
                spawnedTile.name = $"Tile at: {x} / {y}";
                spawnedTile.Initialize(position, this);
                tiles[new(x,y)] = spawnedTile;

                position.x += tileSize.x;
            }

            position.x = startPosition.x;
            position.y -= tileSize.y;
        }
    }

    public SnekTile GetTileAt(Vector2 position)
    {
        if(tiles.TryGetValue(position, out var tile))
        {
            return tile;
        }
        return null;
    }

    public void SetTileTypeAt(Vector2 position, Snek.TileType type)
    {
        var tile = GetTileAt(position);

        if (tile == null) return;

        tile.SetTileType(type);
    }
    public void OnTypeChange(object sender, EventArgs e)
    {
        EventHandler handler = OnTypeChanged;
        handler?.Invoke(sender, e);
    }

    public int GetWidth()
    {
        return width;
    }
    public int GetHeight()
    {
        return height;
    }

    public void ResetGrid()
    {
        foreach(var pair in tiles)
        {
            var tile = GetTileAt(pair.Key);
            tile.SetTileType(Snek.TileType.Empty);
        }
    }
}

namespace Snek
{
    public enum TileType
    {
        Empty,
        Snek,
        Food
    };
}