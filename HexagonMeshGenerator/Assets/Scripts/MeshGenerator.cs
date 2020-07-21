using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    // Start is called before the first frame update

    MeshFilter meshFilter;

    void Start()
    {
        //gameObject.AddComponent<MeshRenderer>();
        //meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter = gameObject.GetComponent<MeshFilter>();
        CreateHexagon();
    }

    void CreateHexagon()
    {
        float height = 1;

        List<Vector3> vertecies = new List<Vector3>();
        List<int> triangles = new List<int> { 0, 1, 5, 5, 1, 4, 4, 1, 2, 4, 2, 3, /* */ 0, 1, 5, 5, 1, 4, 4, 1, 2, 4, 2, 3};

        float sideLenght = 1;

        float rootOf3 = 1.73205080757f;


        //TOP
        vertecies.Add(new Vector3(-sideLenght/2, 0, - rootOf3 * sideLenght / 2));
        vertecies.Add(new Vector3(-sideLenght, 0, 0f));
        vertecies.Add(new Vector3(-sideLenght/2, 0,rootOf3 * sideLenght / 2));
        vertecies.Add(new Vector3(sideLenght / 2, 0, rootOf3 * sideLenght / 2));
        vertecies.Add(new Vector3(sideLenght, 0, 0));
        vertecies.Add(new Vector3(sideLenght / 2, 0, -rootOf3 * sideLenght / 2));

        int originalSize = vertecies.Count;

        //BOTTOM
        for (int i = 0; i < originalSize; i++)
        {
            vertecies.Add(vertecies[i] - new Vector3(0, height, 0));
        }

        Debug.Log(vertecies.Count);

        triangles.Reverse(12, 12);

        for (int i = 0; i < 5; i++)
        {
            Debug.Log($"{i} {i + 1} {6 + i} {6 + i + 1}");
            triangles.Add(6 + i);
            triangles.Add(6 + i + 1);
            triangles.Add(i);
            triangles.Add(6 + i + 1);
            triangles.Add(i + 1);
            triangles.Add(i);
        }

        triangles.Add(11);
        triangles.Add(6);
        triangles.Add(5);
        triangles.Add(6);
        triangles.Add(0);
        triangles.Add(5);

        for (int i = 12; i < 24; i++)
        {
            triangles[i] += 6;
        }

        Mesh mesh = meshFilter.mesh;

        mesh.vertices = vertecies.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {

        
    }
}
