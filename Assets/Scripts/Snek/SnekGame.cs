using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Random = UnityEngine.Random;

public class SnekGame : MonoBehaviour
{
    SnekGridManager _gridManager;
    [SerializeField] float snekSpeed;
    SnekCharacter snek;
    Timer updateSnekTimer;
    Vector2 input;

    float currentSpeed;
    Vector2 lastDir;

    // Start is called before the first frame update
    void Start()
    {
        input = Vector2.zero;

        currentSpeed = snekSpeed;
        _gridManager = GetComponent<SnekGridManager>();
        _gridManager.Initialize();

        StartGame();

        updateSnekTimer = new(1.0f/currentSpeed, true);
        updateSnekTimer.SetCalling(2,true);
        updateSnekTimer.onEnd += UpdateSnek;
        //updateSnekTimer.onEnd += DMessage;
        updateSnekTimer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        InputSnek();
        updateSnekTimer.Update();
    }

    void IncreaseVelocity()
    {
        currentSpeed *= 1.03f;
        updateSnekTimer.SetTime(1.0f/currentSpeed);
    }

    void UpdateSnek(object s, EventArgs e)
    {
        if (IsSnekEating(input))

            GenFood();

        else
        {
            if (!TryToMove(input)) ResetGame();

            foreach (var p in snek._Parts)
            {
                _gridManager.SetTileTypeAt(p._Position, Snek.TileType.Snek);
            }
        }
        _gridManager.OnTypeChange(this, EventArgs.Empty);
    }

    bool TryToMove(Vector2 dir)
    {
        if (IsValidMove(dir))
        {
            foreach (var p in snek._Parts)
            {
                _gridManager.SetTileTypeAt(p._Position, Snek.TileType.Empty);
            }

            if (!snek.TryToMove(dir))
            {
                return TryToMove(lastDir);
            }

            lastDir = dir;
            return true;
        }
        return false;
    }

    void GenFood()
    {
        int x = Random.Range(0, _gridManager.GetWidth());
        int y = Random.Range(0, _gridManager.GetHeight());
        SnekTile tile = _gridManager.GetTileAt(new(x,y));
        
        if(tile._Type == Snek.TileType.Empty) {
        
            _gridManager.SetTileTypeAt( new(x,y) , Snek.TileType.Food);
        }
        else
        {
            GenFood();
        }
    }

    bool IsValidMove(Vector2 dir)
    {
        if (dir == Vector2.zero) return true;

        Vector2 newPos = snek.GetHeadPos() + dir;

        if (!_gridManager.tiles.ContainsKey(newPos)) return false;

        for(int i = 3; i < snek._Parts.Count; i++)
        {
            if(newPos == snek._Parts[i].GetNext()._Position) return false;
        }

        return true;
    }

    void StartGame()
    {
        Vector2 startPos = new(Random.Range(0, _gridManager.GetWidth()), Random.Range(0, _gridManager.GetHeight()));

        input = Vector2.zero;
        
        snek = new(startPos);
        _gridManager.SetTileTypeAt(startPos, Snek.TileType.Snek);
        GenFood();

        _gridManager.OnTypeChange(this, EventArgs.Empty);
    }

    void ResetGame()
    {
        updateSnekTimer.onEnd -= UpdateSnek;

        currentSpeed = snekSpeed;

        updateSnekTimer.SetTime(1.0f/currentSpeed);
        
        updateSnekTimer.Reset();
        
        _gridManager.ResetGrid();
        
        StartGame();

        updateSnekTimer.onEnd += UpdateSnek;
    }

    bool IsSnekEating(Vector2 dir)
    {
        Vector2 newPos =  snek.GetHeadPos() + dir;

        if (!_gridManager.tiles.ContainsKey(newPos)) return false;

        var tile = _gridManager.GetTileAt(newPos);

        if (tile._Type == Snek.TileType.Food)
        {
            snek.AddPart(dir);
            tile.SetTileType(Snek.TileType.Snek);
            IncreaseVelocity();

            return true;
        }
        return false;
    }

    void InputSnek()
    {
        if (Input.GetKey(KeyCode.A))
        {
            input = new(-1, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            input = new(1, 0);

        }
        else if (Input.GetKey(KeyCode.W))
        {
            input = new(0, -1);

        }
        else if (Input.GetKey(KeyCode.S))
        {
            input = new(0, 1);
        }
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            snek.AddPart(input);
        }
        */
    }
}
