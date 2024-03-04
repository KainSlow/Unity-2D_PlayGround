using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;

public class DrawToTex : MonoBehaviour
{
    [SerializeField] Texture2D outputTex;
    [SerializeField] RectTransform rect;
    [SerializeField] Material mat;

    [SerializeField] float dist;

    private void SetPixels( int width, int height, Vector2 pos, float distance, Color c)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (math.length(pos - new Vector2(x, y)) < distance)
                    outputTex.SetPixel(x, y, c);
            }
        }
    }

    private void Update()
    {
        Vector2 targetPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        targetPos *= outputTex.Size();

        float angle = -Mathf.Deg2Rad * rect.eulerAngles.z;
        
        Vector2 newPos = new (targetPos.x * Mathf.Cos(angle) - targetPos.y * Mathf.Sin(angle),
                              targetPos.y * Mathf.Cos(angle) + targetPos.x * Mathf.Sin(angle));


        Debug.Log(newPos);


        if (newPos.x > 0 && newPos.x < outputTex.width && newPos.y > 0 && newPos.y < outputTex.height)
        {   
            if(Input.GetMouseButton(0))
                SetPixels(outputTex.width, outputTex.height, newPos, dist, Color.white);
            else if(Input.GetMouseButton(1))
                SetPixels(outputTex.width, outputTex.height, newPos, dist, Color.black);
        }

        outputTex.Apply();
        mat.SetTexture("_AlphaTex", outputTex);
    }
}
