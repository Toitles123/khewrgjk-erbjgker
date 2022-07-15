using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralLevel : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    public int xSize = 20;
    public int zSize = 20;

    public float perlinScale = .3f;

    public static bool mapGenerated;

    // Start is called before the first frame update
    void Start()
    {
        mapGenerated = false;

        mesh = new Mesh();
        gameObject.AddComponent<MeshFilter>().mesh = mesh;

        CreateShape();
    }

    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * perlinScale, z * perlinScale) * 2f;
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

        gameObject.AddComponent<MeshCollider>().sharedMesh = mesh;
        mapGenerated = true;
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
        UpdateMesh();
    }
}
