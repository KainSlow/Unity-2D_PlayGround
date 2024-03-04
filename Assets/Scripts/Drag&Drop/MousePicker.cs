using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePicker : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject draggable;
    Rigidbody2D draggableRb;

    GameObject availableDraggable;

    bool isDragging;

    [SerializeField] float dragForce;

    // Update is called once per frame
    void Update()
    {
        FollowMousePosition();

        if (Input.GetMouseButtonDown(0) && !isDragging) 
        {
            if(availableDraggable != null)
            {
                isDragging = true;
                draggable = availableDraggable;
                draggableRb = draggable.GetComponent<Rigidbody2D>();
            }
        }

        if(Input.GetMouseButton(0) && isDragging) 
        {
            Drag();
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            draggable = null;
        }

    }

    void FollowMousePosition() 
    {
        transform.position = GetMousePosition();
    }

    void Drag() 
    {
        Vector2 mouseDirection = GetMouseDirection();
        //draggableRb.AddForce(dragForce * mouseDirection, ForceMode2D.Force);
        //draggable.transform.Translate(dragForce*mouseDirection);
        draggableRb.velocity = dragForce * mouseDirection;
    }

    Vector3 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    Vector2 GetMouseDirection() 
    {
        Vector3 mousePosition = GetMousePosition();
        return GetDirection(mousePosition, draggable.transform.position);
    }

    Vector3 GetDirection(Vector3 goalPosition, Vector3 currentPosition)
    {
        return (goalPosition - currentPosition).normalized;
    }

    void GetDraggable(Collider2D collision)
    {
        if (availableDraggable == null && collision.GetComponent<Draggable>() != null)
        availableDraggable = collision.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetDraggable(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GetDraggable(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!isDragging)
        availableDraggable = null;
    }



}
