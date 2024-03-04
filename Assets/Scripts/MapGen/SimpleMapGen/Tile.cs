using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offSetColor, _obstacleColor, _startColor, _endColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] GameObject _highlight;

    GridManager _gridManager;

    bool isEditing;

    Collider2D _boxCol;
    public bool IsObstacle { get; private set; }
    bool isOffset;

    public Node Node { get; private set; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isEditing = !isEditing;
            Debug.Log("Editing:" + isEditing);
        }
    }

    public void Init(bool isOffset)
    {
        _boxCol = GetComponent<BoxCollider2D>();
        this.isOffset = isOffset;
        _renderer.color = isOffset ? _offSetColor : _baseColor;
    }

    public void SetGrid(GridManager gridManager)
    {
        _gridManager = gridManager;
    }

    public void CreateNode(Vector2Int positionInMap)
    {
        Node = new(transform.position, positionInMap, IsObstacle);
    }

    private void OnMouseEnter()
    {
        _highlight.SetActive(true);
    }

    private void OnMouseOver()
    {
        if (isEditing)
        {
            if (Input.GetMouseButton(0))
            {
                IsObstacle = false;
                Init(isOffset);
                Node.UpdateIsObstacle(IsObstacle);
                _boxCol.isTrigger = true;
            }
            else if (Input.GetMouseButton(1))
            {
                IsObstacle = true;
                _renderer.color = _obstacleColor;
                Node.UpdateIsObstacle(IsObstacle);
                _boxCol.isTrigger = false;
            }
        }
        else
        {
            if (!IsObstacle)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Tile start = _gridManager.GetStart();

                    if(this != start)
                    {
                        if(start != null)
                        {
                            start.DisableStart();
                        }
                        _gridManager.SetStart(this);
                        _renderer.color = _startColor;
                    }
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    Tile end = _gridManager.GetEnd();

                    if(this != end)
                    {
                        if(end != null)
                        {
                            end.DisableEnd();
                        }

                        _gridManager.SetEnd(this);
                        _renderer.color = _endColor;
                    }
                }
            }
        }
    }

    public void DisableStart()
    {
        _renderer.color = isOffset ? _offSetColor : _baseColor;

    }

    public void DisableEnd()
    {
        _renderer.color = isOffset ? _offSetColor : _baseColor;

    }

    private void OnMouseExit()
    {
        _highlight.SetActive(false);
    }
}
