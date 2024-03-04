using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oTarget : MonoBehaviour
{
    [SerializeField] public float Speed;
    public Vector2 Direction;
    Rigidbody2D rb;

    [SerializeField] GameObject checkPoint1;
    [SerializeField] GameObject checkPoint2;

    Transform currentT;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentT = checkPoint1.transform;
    }

    float Length(Vector2 v)
    {
        return Mathf.Sqrt(v.x * v.x + v.y * v.y);
    }

    // Update is called once per frame
    void Update()
    {
        //rb.velocity = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position) * Speed;

        if( Length( (Vector2)checkPoint1.transform.position - (Vector2)transform.position)  < 0.25f)
        {
            currentT = checkPoint2.transform;
        }
        else if(Length((Vector2)checkPoint2.transform.position - (Vector2)transform.position) < 0.25f)
        {
            currentT = checkPoint1.transform;
        }

        Direction = (currentT.position - transform.position).normalized;

        rb.velocity = Direction* Speed;
    }
}
