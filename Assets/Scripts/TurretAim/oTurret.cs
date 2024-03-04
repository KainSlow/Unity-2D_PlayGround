using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Mathematics;

public class oTurret : MonoBehaviour
{
    [SerializeField] GameObject oBullet;

    [SerializeField] GameObject shooter;

    [SerializeField] GameObject target;

    Timer shootTimer;
    [SerializeField] float cadence;

    Vector2 tSpeed;
    Vector2 tPos;


    // Start is called before the first frame update
    void Start()
    {
        shootTimer = new(cadence, true);
        shootTimer.SetCalling(2, true);
        shootTimer.onEnd += Shoot;
        shootTimer.Start();

    }
    
    float VecDot(Vector2 v1,  Vector2 v2)
    {
        return v1.x * v2.x + v1.y * v2.y;
    }

    void Aim()
    {
        float bSpeed = oBullet.GetComponent<oBullet>().Speed;

        Vector3 dir = (Vector3)tPos - shooter.transform.position;

        Vector3 offset = tSpeed;

        float dstToTarget = (target.transform.position + offset - shooter.transform.position).magnitude;

        float timeToTarget = dstToTarget / bSpeed;

        transform.right = dir + offset * timeToTarget;

        Debug.DrawLine(shooter.transform.position, (dir + offset * timeToTarget) * 10f, Color.green);
    }
    
    void Shoot(object sender, EventArgs e)
    {
        var bSpeed = oBullet.GetComponent<oBullet>().Speed;

        var oB = Instantiate(oBullet, shooter.transform.position, Quaternion.identity, null);
        oB.GetComponent<Rigidbody2D>().velocity = transform.right.normalized * bSpeed;
    }

    void UpdateVariables()
    {
        tSpeed = target.GetComponent<Rigidbody2D>().velocity;

        tPos = target.transform.position;
    }

    void Update()
    {
        UpdateVariables();
        Aim();
        shootTimer.Update();
    }
}
