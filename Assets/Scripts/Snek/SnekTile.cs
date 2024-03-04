using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SnekTile : MonoBehaviour
{
    SpriteRenderer _sRenderer;
    Vector2 _position;
    public Snek.TileType _Type { get; private set; }
    SnekGridManager _gridManager;
    public void Initialize(Vector2 position, SnekGridManager gridManager, Snek.TileType type = Snek.TileType.Empty)
    {
        _sRenderer = GetComponent<SpriteRenderer>();
        _sRenderer.color = Color.black;

        _position = position;
        _Type = type;
        _gridManager = gridManager;
        _gridManager.OnTypeChanged += ChangeSpriteColor;
    }

    public void SetTileType(Snek.TileType type)
    {
        _Type = type;
    }

    private void ChangeSpriteColor(object sender, EventArgs e)
    {
        switch (_Type)
        {
            case Snek.TileType.Empty:

                _sRenderer.color = Color.black;
                break;

            case Snek.TileType.Snek:

                _sRenderer.color = Color.cyan;
                break;

            case Snek.TileType.Food:

                _sRenderer.color = Color.red;
                break;
        }
    }

}
