using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeColor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        SpriteRenderer sp = GetComponent<SpriteRenderer>();
        Color randomColor = Random.ColorHSV();
        sp.color = randomColor;

    }

}