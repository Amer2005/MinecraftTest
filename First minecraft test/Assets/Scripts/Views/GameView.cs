using Assets.Scripts.Controllers;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Tiles;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameView : MonoBehaviour
{
    private GameController gameController;
    private GameObject[,] gameChunks;
    private int chunkSize = 10;
    private const string pathToTiles = "Prefabs/Tiles";
    public GameObject DefaultChunk;
    private Camera playerCamera;
    public Material[] Materials;

    private void Awake()
    {
        gameController = new GameController(10, 10, 10);
        gameChunks = new GameObject[gameController.GetGameModel().Tiles.GetLength(0) / chunkSize, gameController.GetGameModel().Tiles.GetLength(2) / chunkSize];
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        //Materials = new Material[Enum.GetNames(typeof(TileType)).Length];
    }

    // Start is called before the first frame update
    private void Start()
    {
        RenderTiles();
    }

    // Update is called once per frame
    private void Update()
    {
        Inputs();
    }

    void Inputs()
    {
        GameModel gameModel = gameController.GetGameModel();

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 blockPos = hit.point;

                Debug.Log(blockPos);

                if (blockPos.y == (float)Math.Floor(blockPos.y))
                {
                    blockPos.x = (float)Math.Floor(blockPos.x);
                    blockPos.z = (float)Math.Floor(blockPos.z);

                    if (gameModel.Tiles[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] == null)
                    {
                        //HitUp
                        //blockPos.y--;
                    }
                    else
                    {
                        blockPos.y--;
                    }
                    //Else HitDown
                }

                if (blockPos.x == Math.Floor(blockPos.x))
                {
                    blockPos.y = (float)Math.Floor(blockPos.y);
                    blockPos.z = (float)Math.Floor(blockPos.z);

                    if (gameModel.Tiles[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] == null)
                    {
                        //HitUp
                        //blockPos.x--;
                    }
                    else
                    {
                        blockPos.x--;
                    }
                }

                if (blockPos.z == Math.Floor(blockPos.z))
                {
                    blockPos.y = (float)Math.Floor(blockPos.y);
                    blockPos.x = (float)Math.Floor(blockPos.x);

                    if (gameModel.Tiles[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] == null)
                    {
                        //HitUp
                    }
                    else
                    {
                        blockPos.z--;
                    }
                }

                blockPos.x = (float)Math.Floor(blockPos.x);
                blockPos.y = (float)Math.Floor(blockPos.y);
                blockPos.z = (float)Math.Floor(blockPos.z);

                Debug.Log(blockPos);

                gameController.PlaceTile((int)blockPos.x, (int)blockPos.y, (int)blockPos.z, TileType.DirtTile);

                UpdateChunk((int)blockPos.x / chunkSize, (int)blockPos.z / chunkSize, gameController.GetGameModel());
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 blockPos = hit.point;

                Debug.Log(blockPos);

                if (blockPos.y == (float)Math.Floor(blockPos.y))
                {
                    blockPos.x = (float) Math.Floor(blockPos.x);
                    blockPos.z = (float)Math.Floor(blockPos.z);

                    if (gameModel.Tiles[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] == null)
                    {
                        //HitUp
                        blockPos.y--;
                    }
                    //Else HitDown
                }

                if (blockPos.x == Math.Floor(blockPos.x))
                {
                    blockPos.y = (float)Math.Floor(blockPos.y);
                    blockPos.z = (float)Math.Floor(blockPos.z);

                    if (gameModel.Tiles[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] == null)
                    {
                        //HitUp
                        blockPos.x--;
                    }
                }

                if (blockPos.z == Math.Floor(blockPos.z))
                {
                    blockPos.y = (float)Math.Floor(blockPos.y);
                    blockPos.x = (float)Math.Floor(blockPos.x);

                    if (gameModel.Tiles[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] == null)
                    {
                        //HitUp
                        blockPos.z--;
                    }
                }

                blockPos.x = (float)Math.Floor(blockPos.x);
                blockPos.y = (float)Math.Floor(blockPos.y);
                blockPos.z = (float)Math.Floor(blockPos.z);

                Debug.Log(blockPos);

                gameController.DestroyTile((int)blockPos.x, (int)blockPos.y, (int)blockPos.z);

                if((int)blockPos.x % (chunkSize - 1) == 0)
                {
                    UpdateChunk((int)blockPos.x / chunkSize + 1, (int)blockPos.z / chunkSize, gameController.GetGameModel());
                }
                if ((int)blockPos.x % chunkSize == 0)
                {
                    UpdateChunk((int)blockPos.x / chunkSize - 1, (int)blockPos.z / chunkSize, gameController.GetGameModel());
                }
                if ((int)blockPos.z % (chunkSize - 1) == 0)
                {
                    UpdateChunk((int)blockPos.x / chunkSize, (int)blockPos.z / chunkSize - 1, gameController.GetGameModel());
                }
                if ((int)blockPos.z % chunkSize == 0)
                {
                    UpdateChunk((int)blockPos.x / chunkSize, (int)blockPos.z / chunkSize - 1, gameController.GetGameModel());
                }

                UpdateChunk((int)blockPos.x / chunkSize, (int)blockPos.z / chunkSize, gameController.GetGameModel());
            }
        }
    }

    private void RenderTiles()
    {
        GameModel gameModel = gameController.GetGameModel();

        for (int x = 0; x < gameChunks.GetLength(0); x++)
        {
            for (int z = 0; z < gameChunks.GetLength(1); z++)
            {
                UpdateChunk(x, z, gameModel);
            }
        }
    }

    private void UpdateChunk(int chunkX, int chunkZ, GameModel gameModel)
    {
        if(chunkX < 0 || chunkZ < 0 || chunkX >= gameChunks.GetLength(0) || chunkZ >= gameChunks.GetLength(1))
        {
            return;
        }

        if (gameChunks[chunkX, chunkZ] != null)
        {
            Destroy(gameChunks[chunkX, chunkZ]);
            gameChunks[chunkX, chunkZ] = null;
        }

        Mesh mesh;

        mesh = new Mesh();

        gameChunks[chunkX, chunkZ] = Instantiate(DefaultChunk, new Vector3(0, 0, 0) * chunkSize, new Quaternion());

        gameChunks[chunkX, chunkZ].GetComponent<MeshFilter>().mesh = mesh;

        var meshRenderer = gameChunks[chunkX, chunkZ].GetComponent<MeshRenderer>();

        meshRenderer.material.mainTexture.wrapMode = TextureWrapMode.Repeat;

        RenderBlock(chunkX, chunkZ, mesh, gameModel);

        mesh.RecalculateNormals();

        gameChunks[chunkX, chunkZ].AddComponent<MeshCollider>();

        gameChunks[chunkX, chunkZ].transform.position = new Vector3(gameChunks[chunkX, chunkZ].transform.position.x, gameChunks[chunkX, chunkZ].transform.position.y, gameChunks[chunkX, chunkZ].transform.position.z);

        //gameChunks[x, z]
    }

    private void GenerateQuad()

    private void RenderBlock(int chunkX, int chunkZ, Mesh mesh, GameModel gameModel)
    {
        Vector3 ChunkPos = new Vector3(chunkX, 0, chunkZ) * chunkSize;

        List<Vector3> vectors = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        for (int x = chunkX * chunkSize; x < chunkX * chunkSize + chunkSize; x++)
        {
            for (int z = chunkZ * chunkSize; z < chunkZ * chunkSize + chunkSize; z++)
            {
                for (int y = 0; y < gameModel.Tiles.GetLength(1); y++)
                {
                    if (gameModel.Tiles[x, y, z] != null)
                    {
                        //int[] indexes = new indexes[8];
                        //TOP
                        if ((y < gameModel.Tiles.GetLength(1) - 1 && gameModel.Tiles[x, y + 1, z] == null) || y == gameModel.Tiles.GetLength(1) - 1)
                        {
                            int index4 = vectors.IndexOf(new Vector3(x, y + 1, z));
                            int index5 = vectors.IndexOf(new Vector3(x + 1, y + 1, z));
                            int index6 = vectors.IndexOf(new Vector3(x + 1, y + 1, z + 1));
                            int index7 = vectors.IndexOf(new Vector3(x, y + 1, z + 1));

                            if (index4 == -1)
                            {
                                vectors.Add(new Vector3(x, y + 1, z));
                                uvs.Add(new Vector2());
                                index4 = vectors.Count - 1;
                            }

                            if (index5 == -1)
                            {
                                vectors.Add(new Vector3(x + 1, y + 1, z));
                                uvs.Add(new Vector2());
                                index5 = vectors.Count - 1;
                            }

                            if (index6 == -1)
                            {
                                vectors.Add(new Vector3(x + 1, y + 1, z + 1));
                                uvs.Add(new Vector2());
                                index6 = vectors.Count - 1;
                            }

                            if (index7 == -1)
                            {
                                vectors.Add(new Vector3(x, y + 1, z + 1));
                                uvs.Add(new Vector2());
                                index7 = vectors.Count - 1;
                            }

                            uvs[index4] = new Vector2(1 * 0.0625f, 1 * 0.0625f);
                            uvs[index5] = new Vector2(2 * 0.0625f, 1 * 0.0625f);
                            uvs[index6] = new Vector2(2 * 0.0625f, 2 * 0.0625f);
                            uvs[index7] = new Vector2(1 * 0.0625f, 2 * 0.0625f);

                            //Top 4, 7, 5, 5, 7, 6,
                            triangles.Add(index4);
                            triangles.Add(index7);
                            triangles.Add(index5);
                            triangles.Add(index5);
                            triangles.Add(index7);
                            triangles.Add(index6);
                        }
                        //BOTTOM
                        if ((y > 0 && gameModel.Tiles[x, y - 1, z] == null) || y == 0)
                        {
                            int index0 = vectors.IndexOf(new Vector3(x, y, z));
                            int index1 = vectors.IndexOf(new Vector3(x + 1, y, z));
                            int index2 = vectors.IndexOf(new Vector3(x + 1, y, z + 1));
                            int index3 = vectors.IndexOf(new Vector3(x, y, z + 1));

                            if (index0 == -1)
                            {
                                vectors.Add(new Vector3(x, y, z));
                                uvs.Add(new Vector2());
                                index0 = vectors.Count - 1;
                            }

                            if (index1 == -1)
                            {
                                vectors.Add(new Vector3(x + 1, y, z));
                                uvs.Add(new Vector2());
                                index1 = vectors.Count - 1;
                            }

                            if (index2 == -1)
                            {
                                vectors.Add(new Vector3(x + 1, y, z + 1));
                                uvs.Add(new Vector2());
                                index2 = vectors.Count - 1;
                            }

                            if (index3 == -1)
                            {
                                vectors.Add(new Vector3(x, y, z + 1));
                                uvs.Add(new Vector2());
                                index3 = vectors.Count - 1;
                            }

                            uvs[index0] = new Vector2(1 * 0.0625f, 1 * 0.0625f);
                            uvs[index1] = new Vector2(2 * 0.0625f, 1 * 0.0625f);
                            uvs[index2] = new Vector2(2 * 0.0625f, 2 * 0.0625f);
                            uvs[index3] = new Vector2(1 * 0.0625f, 2 * 0.0625f);

                            //Bottom 0, 1, 3, 2, 3, 1,
                            triangles.Add(index0);
                            triangles.Add(index1);
                            triangles.Add(index3);
                            triangles.Add(index2);
                            triangles.Add(index3);
                            triangles.Add(index1);
                        }
                        //RIGHT
                        if ((x < gameModel.Tiles.GetLength(0) - 1 && gameModel.Tiles[x + 1, y, z] == null) || x == gameModel.Tiles.GetLength(0) - 1)
                        {
                            int index1 = vectors.IndexOf(new Vector3(x + 1, y, z));
                            int index2 = vectors.IndexOf(new Vector3(x + 1, y, z + 1));
                            int index5 = vectors.IndexOf(new Vector3(x + 1, y + 1, z));
                            int index6 = vectors.IndexOf(new Vector3(x + 1, y + 1, z + 1));

                            if (index1 == -1)
                            {
                                vectors.Add(new Vector3(x + 1, y, z));
                                uvs.Add(new Vector2());
                                index1 = vectors.Count - 1;
                            }

                            if (index2 == -1)
                            {
                                vectors.Add(new Vector3(x + 1, y, z + 1));
                                uvs.Add(new Vector2());
                                index2 = vectors.Count - 1;
                            }

                            if (index5 == -1)
                            {
                                vectors.Add(new Vector3(x + 1, y + 1, z));
                                uvs.Add(new Vector2());
                                index5 = vectors.Count - 1;
                            }

                            if (index6 == -1)
                            {
                                vectors.Add(new Vector3(x + 1, y + 1, z + 1));
                                uvs.Add(new Vector2());
                                index6 = vectors.Count - 1;
                            }

                            uvs[index1] = new Vector2(1 * 0.0625f, 1 * 0.0625f);
                            uvs[index5] = new Vector2(2 * 0.0625f, 1 * 0.0625f);
                            uvs[index2] = new Vector2(2 * 0.0625f, 2 * 0.0625f);
                            uvs[index6] = new Vector2(1 * 0.0625f, 2 * 0.0625f);

                            //Right 1, 5, 2, 2, 5, 6,
                            triangles.Add(index1);
                            triangles.Add(index5);
                            triangles.Add(index2);
                            triangles.Add(index2);
                            triangles.Add(index5);
                            triangles.Add(index6);
                        }
                        //LEFT
                        if ((x > 0 && gameModel.Tiles[x - 1, y, z] == null) || x == 0)
                        {
                            int index0 = vectors.IndexOf(new Vector3(x, y, z));
                            int index3 = vectors.IndexOf(new Vector3(x, y, z + 1));
                            int index4 = vectors.IndexOf(new Vector3(x, y + 1, z));
                            int index7 = vectors.IndexOf(new Vector3(x, y + 1, z + 1));

                            if (index0 == -1)
                            {
                                vectors.Add(new Vector3(x, y, z));
                                uvs.Add(new Vector2());
                                index0 = vectors.Count - 1;
                            }

                            if (index3 == -1)
                            {
                                vectors.Add(new Vector3(x, y, z + 1));
                                uvs.Add(new Vector2());
                                index3 = vectors.Count - 1;
                            }

                            if (index4 == -1)
                            {
                                vectors.Add(new Vector3(x, y + 1, z));
                                uvs.Add(new Vector2());
                                index4 = vectors.Count - 1;
                            }

                            if (index7 == -1)
                            {
                                vectors.Add(new Vector3(x, y + 1, z + 1));
                                uvs.Add(new Vector2());
                                index7 = vectors.Count - 1;
                            }

                            //Left 3, 4, 0, 3, 7, 4
                            triangles.Add(index3);
                            triangles.Add(index4);
                            triangles.Add(index0);
                            triangles.Add(index3);
                            triangles.Add(index7);
                            triangles.Add(index4);
                        }
                        //FRONT
                        if ((z > 0 && gameModel.Tiles[x, y, z - 1] == null) || z == 0)
                        {
                            int index0 = vectors.IndexOf(new Vector3(x, y, z));
                            int index1 = vectors.IndexOf(new Vector3(x + 1, y, z));
                            int index4 = vectors.IndexOf(new Vector3(x, y + 1, z));
                            int index5 = vectors.IndexOf(new Vector3(x + 1, y + 1, z));

                            if (index0 == -1)
                            {
                                vectors.Add(new Vector3(x, y, z));
                                uvs.Add(new Vector2());
                                index0 = vectors.Count - 1;
                            }

                            if (index1 == -1)
                            {
                                vectors.Add(new Vector3(x + 1, y, z));
                                uvs.Add(new Vector2());
                                index1 = vectors.Count - 1;
                            }

                            if (index4 == -1)
                            {
                                vectors.Add(new Vector3(x, y + 1, z));
                                uvs.Add(new Vector2());
                                index4 = vectors.Count - 1;
                            }

                            if (index5 == -1)
                            {
                                vectors.Add(new Vector3(x + 1, y + 1, z));
                                uvs.Add(new Vector2());
                                index5 = vectors.Count - 1;
                            }

                            //Front 0, 4, 1, 1, 4, 5,
                            triangles.Add(index0);
                            triangles.Add(index4);
                            triangles.Add(index1);
                            triangles.Add(index1);
                            triangles.Add(index4);
                            triangles.Add(index5);
                        }
                        //BACK
                        if ((z < gameModel.Tiles.GetLength(0) - 1 && gameModel.Tiles[x, y, z + 1] == null) || z == gameModel.Tiles.GetLength(0) - 1)
                        {
                            int index3 = vectors.IndexOf(new Vector3(x, y, z + 1));
                            int index2 = vectors.IndexOf(new Vector3(x + 1, y, z + 1));
                            int index7 = vectors.IndexOf(new Vector3(x, y + 1, z + 1));
                            int index6 = vectors.IndexOf(new Vector3(x + 1, y + 1, z + 1));

                            if (index3 == -1)
                            {
                                vectors.Add(new Vector3(x, y, z + 1));
                                uvs.Add(new Vector2());
                                index3 = vectors.Count - 1;
                            }

                            if (index2 == -1)
                            {
                                vectors.Add(new Vector3(x + 1, y, z + 1));
                                uvs.Add(new Vector2());
                                index2 = vectors.Count - 1;
                            }

                            if (index7 == -1)
                            {
                                vectors.Add(new Vector3(x, y + 1, z + 1));
                                uvs.Add(new Vector2());
                                index7 = vectors.Count - 1;
                            }

                            if (index6 == -1)
                            {
                                vectors.Add(new Vector3(x + 1, y + 1, z + 1));
                                uvs.Add(new Vector2());
                                index6 = vectors.Count - 1;
                            }

                            //Front 2, 7, 3, 6, 7, 2,
                            triangles.Add(index2);
                            triangles.Add(index7);
                            triangles.Add(index3);
                            triangles.Add(index6);
                            triangles.Add(index7);
                            triangles.Add(index2);
                        }

                    }    
                }
            }
        }

        for (int i = 0; i < uvs.Count; i++)
        {
            //uvs[i] = new Vector2(vectors[i].x * 0.0625f, vectors[i].z * 0.0625f);

            Debug.Log(vectors[i]);
        }

        mesh.vertices = vectors.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();

        /*
        var cubeVector = new Vector3[]
        {
            new Vector3(0, 0, 0) + pos,
            new Vector3(1, 0, 0) + pos,
            new Vector3(1, 0, 1) + pos,
            new Vector3(0, 0, 1) + pos,
            new Vector3(0, 1, 0) + pos,
            new Vector3(1, 1, 0) + pos,
            new Vector3(1, 1, 1) + pos,
            new Vector3(0, 1, 1) + pos
        };

        var cubeTriangles = new int[]
        {
            //Bottom
            0, 1, 3, 2, 3, 1,
            //Right
            1, 5, 2, 2, 5, 6,
            //Top
            4, 7, 5, 5, 7, 6,
            //Front
            0, 4, 1, 1, 4, 5,
            //Behind
            2, 7, 3, 6, 7, 2,
            //Left
            3, 4, 0, 3, 7, 4
        };
        //1, 5, 2, 2, 5, 6
        

        mesh.vertices = cubeVector;
        mesh.triangles = cubeTriangles;
        */
    }
}
