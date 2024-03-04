using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SnekCharacter
{
    public class SnakePart
    {
        public Vector2 _Position { get; private set; }
        public Vector2 _CurrentPosition { get; private set; }
        SnakePart _next = null;

        public SnakePart(Vector2 position ,SnakePart next = null)
        {
            _Position = position;
            _next = next;
        }

        public SnakePart GetNext()
        {
            return _next;
        }

        public void Move()
        {
            if(_next != null)
            {
                //_Position = _next._Position;
                _CurrentPosition = math.lerp(_Position, _next._Position, 1);
                if((_CurrentPosition - _next._Position).magnitude <= 0.1)
                {
                    _Position = _next._Position;
                }
            }
        }
        public void SetPos(Vector2 position) 
        {
            _Position = position;
        }

        public void SetNext(SnakePart next)
        {
            _next = next;
        }
    }
    public List<SnakePart> _Parts { get; private set; }
    SnakePart _head;
    public SnekCharacter()
    {
        _Parts = new();
    }

    public SnekCharacter(Vector2 pos)
    {
        _Parts = new()
        {
            new SnakePart(pos)
        };
        SetHead();
    }

    public void SetHead()
    {
        if(_Parts.Count > 0)
        _head = _Parts[0];
    }
    public Vector2 GetHeadPos()
    {
        return _head._Position;
    }
    public void AddHead(Vector2 position)
    {
        _Parts.Add(new(position, null));
        SetHead();
    }
    public void AddPart(Vector2 dir)
    {
        SnakePart newPart = new(_head._Position + dir);
        _head.SetNext(newPart);
        _Parts.Insert(0, newPart);
        SetHead();
    }

    private void Move(Vector2 direction)
    {
        if(_Parts.Count > 0)
        {
            for (int i = _Parts.Count - 1; i >= 1; i--)
            {
                _Parts[i].Move();
            }

            _head.SetPos(_head._Position + direction);
        }
    }

    public bool TryToMove(Vector2 direction)
    {
        if(_Parts.Count < 3)
        {
            Move(direction);
            return true;
        }

        if (_head._Position + direction != _Parts[1]._Position)
        {
            Move(direction);
            return true;
        }

        return false;
    }

}
