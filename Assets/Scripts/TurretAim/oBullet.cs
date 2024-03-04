using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

public class oBullet : MonoBehaviour
{
    // Start is called before the first frame update
    [Range(0.1f,100f),SerializeField] public float Speed;
    [SerializeField] public float lifeSpan;

    Timer death;

    void Start()
    {
        death = new(lifeSpan);
        death.SetCalling(2, true);
        death.onEnd += die;
        death.Start();
    }
    void die(object sender, EventArgs e)
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        death.Update();
    }
}
