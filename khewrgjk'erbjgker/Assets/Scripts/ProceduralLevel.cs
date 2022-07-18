using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralLevel : MonoBehaviour
{
    public int xSize = 20;
    public int zSize = 20;
    public float perlinScale = .3f;
    public static bool mapGenerated;
    public int seed;
    public int octaves = 4;
    public float lacunarity;
    [Range(0, 1)] public float persistance = 0.5f;
    public Vector2 offset;

    [Header("Forest Settings")]
    public int forestSeed;
    public GameObject treeObject;
    public Vector3 rotationOffset;
    public Color minColor;
    public Color maxColor;
    
    //Mesh Stuff
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    void Start()
    {
        seed = Random.Range(0, 1000000);
        forestSeed = Random.Range(0, 1000000);

        mapGenerated = false;

        mesh = new Mesh();
        gameObject.AddComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        GenerateTrees();
    }

    void GenerateTrees()
    {
        foreach (Vector3 vertex in vertices)
        {
            float chance = Mathf.PerlinNoise(vertex.x * perlinScale + forestSeed, vertex.z * perlinScale + forestSeed);
            if (chance > 0.5f)
            {
                if (Random.Range(1, 101) >= 95 && !(vertex.x == 0 && vertex.y == 0))
                {
                    GameObject tree = Instantiate(treeObject, vertex + transform.position, Quaternion.identity);
                    tree.transform.SetParent(transform);
                    tree.transform.localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                    tree.transform.localEulerAngles += rotationOffset;
                    tree.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_LeafColor", Color.Lerp(minColor, maxColor, Random.Range(0.0f, 1.001f)));
                    GameManager.instance.allHarvestableObjects.Add(tree.GetComponent<HarvestableScript>());
                }
            }
        }
    }

    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Noise.GenerateNoiseMap(xSize, zSize, seed, perlinScale, octaves, persistance, lacunarity, offset)[x, z];
                vertices[i++] = new Vector3(x, y, z);
            }
        }

        triangles = new int[xSize * zSize * 6];
        
        int vert = 0;
        int tries = 0;
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tries + 0] = vert;
                triangles[tries + 1] = vert + xSize + 1;
                triangles[tries + 2] = vert + 1;
                triangles[tries + 3] = vert + 1;
                triangles[tries + 4] = vert + xSize + 1;
                triangles[tries + 5] = vert + xSize + 2;

                vert++;
                tries += 6;
            }
            vert++;
        }

        UpdateMesh();
        mapGenerated = true;
        gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }
}
