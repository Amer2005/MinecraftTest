using Assets.Scripts.Controllers;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Tiles;
using Assets.Scripts.Services;
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
    public Vector2[] MaterialsSideTop;
    public Vector2[] MaterialsSideBottom;
    public Vector2[] MaterialsSideRight;
    public Vector2[] MaterialsSideLeft;
    public Vector2[] MaterialsSideFront;
    public Vector2[] MaterialsSideBack;
    private Vector2[,] MaterialsSides;
    private PerlinNoiseService perlinNoiseService;

    private void Awake()
    {
        MaterialsSides = new Vector2[Enum.GetNames(typeof(TileType)).Length, 6];

        perlinNoiseService = new PerlinNoiseService();

        for (int i = 0;i < MaterialsSides.GetLength(0);i++)
        {
            MaterialsSides[i,0] = MaterialsSideTop[i];
            if (MaterialsSideBottom[i].x != -1)
            {
                MaterialsSides[i, 1] = MaterialsSideBottom[i];
                MaterialsSides[i, 2] = MaterialsSideRight[i];
                MaterialsSides[i, 3] = MaterialsSideLeft[i];
                MaterialsSides[i, 4] = MaterialsSideFront[i];
                MaterialsSides[i, 5] = MaterialsSideBack[i];
            }
            else
            {
                MaterialsSides[i, 1] = MaterialsSideTop[i];
                MaterialsSides[i, 2] = MaterialsSideTop[i];
                MaterialsSides[i, 3] = MaterialsSideTop[i];
                MaterialsSides[i, 4] = MaterialsSideTop[i];
                MaterialsSides[i, 5] = MaterialsSideTop[i];
            }

        }

        gameController = new GameController(100, 20, 100);
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

                gameController.PlaceTile((int)blockPos.x, (int)blockPos.y, (int)blockPos.z, gameModel.Inventory.HotBar[gameModel.Inventory.SelectedBlock]);

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

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            gameModel.Inventory.SelectedBlock = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gameModel.Inventory.SelectedBlock = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            gameModel.Inventory.SelectedBlock = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            gameModel.Inventory.SelectedBlock = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            gameModel.Inventory.SelectedBlock = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            gameModel.Inventory.SelectedBlock = 5;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            gameModel.Inventory.SelectedBlock = 6;
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            gameModel.Inventory.SelectedBlock = 7;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            gameModel.Inventory.SelectedBlock = 8;
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

    private int[] GenerateQuad(Dictionary<Vector3, int> used, List<Vector3> vectors, List<int> triangles, List<Vector2> uvs, Vector3[] vertPos, int[] newTriangles)
    {
        int[] indexesInt = new int[4];
        //indexesInt[0] = vectors.IndexOf(vertPos[0]);
        //indexesInt[1] = vectors.IndexOf(vertPos[1]);
        //indexesInt[2] = vectors.IndexOf(vertPos[2]);
        //indexesInt[3] = vectors.IndexOf(vertPos[3]);
        /*
        Vector3 min = new Vector3();

        min = vertPos[0];
        */
        for (int i = 0; i < 4; i++)
        {
            //if(!used.ContainsKey(vertPos[i]))
            //{
                //used.Add(vertPos[i], vectors.Count);
                uvs.Add(new Vector2());
                vectors.Add(vertPos[i]);
            //}

            indexesInt[i] = vectors.Count - 1;

            //indexesInt[i] = used[vertPos[i]];
        }

        for (int i = 0; i < newTriangles.Length; i++)
        {
            triangles.Add(indexesInt[newTriangles[i]]);
        }

        return indexesInt;
    }

    private void RenderBlock(int chunkX, int chunkZ, Mesh mesh, GameModel gameModel)
    {
        Dictionary<Vector3, int> used = new Dictionary<Vector3, int>();

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
                        int[] indexes;

                        float pixelSize = 16;
                        float tilePerc = 1 / pixelSize;
                        float tileX;
                        float tileY;

                        float umin;
                        float umax;
                        float vmin;
                        float vmax;

                        //TOP
                        if ((y < gameModel.Tiles.GetLength(1) - 1 && gameModel.Tiles[x, y + 1, z] == null) || y == gameModel.Tiles.GetLength(1) - 1)
                        {
                            indexes = GenerateQuad(used, vectors, triangles, uvs,
                                new Vector3[] { new Vector3(x, y + 1, z), new Vector3(x + 1, y + 1, z), new Vector3(x + 1, y + 1, z + 1), new Vector3(x, y + 1, z + 1) },
                                new int[] { 0, 3, 1, 1, 3, 2 });

                            Vector2 material = MaterialsSides[(int)gameModel.Tiles[x, y, z].TileType, 0];

                            tileX = material.x;
                            tileY = material.y;

                            umin = tilePerc * tileX;
                            umax = tilePerc * (tileX + 1);
                            vmin = tilePerc * tileY;
                            vmax = tilePerc * (tileY + 1);

                            //tileX = Vector2[]

                            uvs[indexes[0]] = new Vector2(umin, vmin);
                            uvs[indexes[1]] = new Vector2(umax, vmin);
                            uvs[indexes[2]] = new Vector2(umax, vmax);
                            uvs[indexes[3]] = new Vector2(umin, vmax);
                        }

                        //BOTTOM
                        if ((y > 0 && gameModel.Tiles[x, y - 1, z] == null) || y == 0)
                        {
                            indexes = GenerateQuad(used, vectors, triangles, uvs,
                            new Vector3[] { new Vector3(x, y, z), new Vector3(x + 1, y, z), new Vector3(x + 1, y, z + 1), new Vector3(x, y, z + 1) },
                            new int[] { 0, 1, 3, 2, 3, 1 });

                            Vector2 material = MaterialsSides[(int)gameModel.Tiles[x, y, z].TileType, 1];

                            tileX = material.x;
                            tileY = material.y;

                            umin = tilePerc * tileX;
                            umax = tilePerc * (tileX + 1);
                            vmin = tilePerc * tileY;
                            vmax = tilePerc * (tileY + 1);

                            uvs[indexes[0]] = new Vector2(umin, vmin);
                            uvs[indexes[1]] = new Vector2(umax, vmin);
                            uvs[indexes[2]] = new Vector2(umax, vmax);
                            uvs[indexes[3]] = new Vector2(umin, vmax);
                        }
                        //RIGHT
                        if ((x < gameModel.Tiles.GetLength(0) - 1 && gameModel.Tiles[x + 1, y, z] == null) || x == gameModel.Tiles.GetLength(0) - 1)
                        {
                            indexes = GenerateQuad(used, vectors, triangles, uvs,
                            new Vector3[] { new Vector3(x + 1, y, z), new Vector3(x + 1, y, z + 1), new Vector3(x + 1, y + 1, z), new Vector3(x + 1, y + 1, z + 1) },
                            new int[] { 0, 2, 1, 1, 2, 3 });

                            Vector2 material = MaterialsSides[(int)gameModel.Tiles[x, y, z].TileType, 2];

                            tileX = material.x;
                            tileY = material.y;

                            umin = tilePerc * tileX;
                            umax = tilePerc * (tileX + 1);
                            vmin = tilePerc * tileY;
                            vmax = tilePerc * (tileY + 1);

                            uvs[indexes[0]] = new Vector2(umin, vmin);
                            uvs[indexes[2]] = new Vector2(umin, vmax);
                            uvs[indexes[1]] = new Vector2(umax, vmin);
                            uvs[indexes[3]] = new Vector2(umax, vmax);
                            /*
                             uvs[indexes[0]] = new Vector2(umax, vmin);
                            uvs[indexes[1]] = new Vector2(umax, vmax);
                            uvs[indexes[2]] = new Vector2(umax, vmin);
                            uvs[indexes[3]] = new Vector2(umax, vmax); 
                            */
                        }
                        //LEFT
                        if ((x > 0 && gameModel.Tiles[x - 1, y, z] == null) || x == 0)
                        {
                            indexes = GenerateQuad(used, vectors, triangles, uvs,
                            new Vector3[] { new Vector3(x, y, z), new Vector3(x, y, z + 1), new Vector3(x, y + 1, z), new Vector3(x, y + 1, z + 1) },
                            new int[] { 1, 2, 0, 1, 3, 2 });

                            Vector2 material = MaterialsSides[(int)gameModel.Tiles[x, y, z].TileType, 3];

                            tileX = material.x;
                            tileY = material.y;

                            umin = tilePerc * tileX;
                            umax = tilePerc * (tileX + 1);
                            vmin = tilePerc * tileY;
                            vmax = tilePerc * (tileY + 1);

                            uvs[indexes[0]] = new Vector2(umin, vmin);
                            uvs[indexes[2]] = new Vector2(umin, vmax);
                            uvs[indexes[1]] = new Vector2(umax, vmin);
                            uvs[indexes[3]] = new Vector2(umax, vmax);
                        }
                        //FRONT
                        if ((z > 0 && gameModel.Tiles[x, y, z - 1] == null) || z == 0)
                        {
                            indexes = GenerateQuad(used, vectors, triangles, uvs,
                            new Vector3[] { new Vector3(x, y, z), new Vector3(x + 1, y, z), new Vector3(x, y + 1, z), new Vector3(x + 1, y + 1, z) },
                            new int[] { 0, 2, 1, 1, 2, 3 });

                            Vector2 material = MaterialsSides[(int)gameModel.Tiles[x, y, z].TileType, 4];

                            tileX = material.x;
                            tileY = material.y;

                            umin = tilePerc * tileX;
                            umax = tilePerc * (tileX + 1);
                            vmin = tilePerc * tileY;
                            vmax = tilePerc * (tileY + 1);

                            uvs[indexes[0]] = new Vector2(umin, vmin);
                            uvs[indexes[1]] = new Vector2(umax, vmin);
                            uvs[indexes[2]] = new Vector2(umin, vmax);
                            uvs[indexes[3]] = new Vector2(umax, vmax);
                        }
                        //BACK
                        if ((z < gameModel.Tiles.GetLength(2) - 1 && gameModel.Tiles[x, y, z + 1] == null) || z == gameModel.Tiles.GetLength(2) - 1)
                        {
                            indexes = GenerateQuad(used, vectors, triangles, uvs,
                            new Vector3[] { new Vector3(x, y, z + 1), new Vector3(x + 1, y, z + 1), new Vector3(x, y + 1, z + 1), new Vector3(x + 1, y + 1, z + 1) },
                            new int[] { 1, 2, 0, 3, 2, 1 });

                            Vector2 material = MaterialsSides[(int)gameModel.Tiles[x, y, z].TileType, 5];

                            tileX = material.x;
                            tileY = material.y;

                            umin = tilePerc * tileX;
                            umax = tilePerc * (tileX + 1);
                            vmin = tilePerc * tileY;
                            vmax = tilePerc * (tileY + 1);

                            uvs[indexes[0]] = new Vector2(umin, vmin);
                            uvs[indexes[1]] = new Vector2(umax, vmin);
                            uvs[indexes[2]] = new Vector2(umin, vmax);
                            uvs[indexes[3]] = new Vector2(umax, vmax);
                        }
                    }
                }
            }
        }

        for (int i = 0; i < uvs.Count; i++)
        {
            //uvs[i] = new Vector2(vectors[i].x * 0.0625f, vectors[i].z * 0.0625f);
        }

        mesh.vertices = vectors.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
    }
}
