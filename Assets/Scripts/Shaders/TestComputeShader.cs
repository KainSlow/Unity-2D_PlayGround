using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

struct Cube
{
    public float3 position;
    public Color color;
}
public class TestComputeShader : MonoBehaviour
{
    [SerializeField] ComputeShader m_ComputeShader;
    [SerializeField] int cubeCount;
    [SerializeField] int repetitions;

    [SerializeField] Mesh mesh;
    [SerializeField] Material material;

    public RenderTexture m_RenderTexture;
    float time;

    float2 targetPos;

    List<GameObject> objects;
    private Cube[] data;


    public void CreateCubes()
    {
        objects = new List<GameObject>();
        data = new Cube[cubeCount * cubeCount];

        for (int y = 0; y < cubeCount; y++)
        {
            for(int x = 0; x < cubeCount; x++)
            {
                CreateCube(x, y);
            }
        }
    }

    private void CreateCube(int x, int y)
    {
        GameObject cube = new GameObject( $"Cube {(x * cubeCount) + y}", typeof(MeshFilter), typeof(MeshRenderer));
        cube.GetComponent<MeshFilter>().mesh = mesh;
        cube.GetComponent<MeshRenderer>().material = new Material(material);
        cube.transform.position = new Vector3(x, y, UnityEngine.Random.Range(-.1f,.1f));

        Color color = UnityEngine.Random.ColorHSV();
        cube.GetComponent<MeshRenderer>().material.SetColor("_Color", color);
        objects.Add(cube);

        Cube cubeData = new Cube();
        cubeData.position = cube.transform.position;
        cubeData.color = color;
        data[x*cubeCount + y]= cubeData;
    }

    public void OnRandomCPU()
    {
        for(int i = 0; i < repetitions; i++)
        {
            for(int c = 0; c < objects.Count ; c++)
            {
                GameObject obj = objects[c];
                obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, UnityEngine.Random.Range(-.5f,.5f));
                obj.GetComponent<MeshRenderer>().material.SetColor("_Color", UnityEngine.Random.ColorHSV());
            }
        }
    }

    public void OnRandomGPU()
    {
        int totalSize = sizeof(float)*7;
        ComputeBuffer cubesBuffer = new ComputeBuffer(data.Length, totalSize);
        cubesBuffer.SetData(data);

        m_ComputeShader.SetBuffer(0, "cubes", cubesBuffer);
        m_ComputeShader.SetFloats("iResolution", data.Length, data.Length);
        m_ComputeShader.SetFloat("iRepetitions", repetitions);
        m_ComputeShader.Dispatch(0, data.Length / 10, 1, 1);

        cubesBuffer.GetData(data);

        for(int i = 0; i < objects.Count; i++)
        {
            GameObject obj = objects[i];
            Cube cube = data[i];
            obj.transform.position = cube.position;
            obj.GetComponent<MeshRenderer>().material.SetColor("_Color", cube.color);
        }

        cubesBuffer.Dispose();
    }

    private void OnGUI()
    {
        if(objects == null)
        {
            if(GUI.Button(new Rect(0,0,100,50), "Create"))
            {
                CreateCubes();
            }
        }
        else
        {
            if(GUI.Button(new Rect(0,0,100,50), "R CPU"))
            {
                OnRandomCPU();
            }
            if(GUI.Button(new Rect(100, 0, 100, 50), "R GPU"))
            {
                OnRandomGPU();
            }
        }
    }

    private void Start()
    {
        time = 0;
    }
    private void Update()
    {
        time += Time.deltaTime;
        targetPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    /*
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(m_RenderTexture == null)
        {
            m_RenderTexture = new RenderTexture(1280, 720, 24);
            m_RenderTexture.enableRandomWrite = true;
            m_RenderTexture.Create();
        }

        m_ComputeShader.SetTexture(0, "Result", m_RenderTexture);
        m_ComputeShader.SetFloats("iResolution", m_RenderTexture.width, m_RenderTexture.height);
        m_ComputeShader.SetFloats("iMouse", targetPos.x, targetPos.y);
        m_ComputeShader.SetFloat("iTime", time);


        m_ComputeShader.Dispatch(0, m_RenderTexture.width / 8, m_RenderTexture.height / 8, 1);
        Graphics.Blit(m_RenderTexture, destination);
    }
    */
}
