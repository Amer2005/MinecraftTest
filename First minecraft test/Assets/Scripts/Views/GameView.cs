using Assets.Scripts.Controllers;
using Assets.Scripts.Models;
using Assets.Scripts.Models.InventoryModels;
using Assets.Scripts.Models.Blocks;
using Assets.Scripts.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Models.Buildings;
using System.Text;

public class GameView : MonoBehaviour
{
    private GameController gameController;
    private GameObject[,] gameChunks;
    private int chunkSize = 10;
    private const string pathToBlocks = "Prefabs/Blocks";
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
    private InventoryService inventoryService;
    private BlockService BlockService;
    private const float pixelSize = 16;
    private PlayerService playerService;

    private GameObject[] hotbarSlotsGameObjects;
    private RawImage[] hotbarSlotsImages;
    private GameObject[] hotbarCountGameObjects;
    private Text[] hotbarItemCounts;
    private GameObject[] hotbarSlotSelected;

    private BuildingService buildingService;
    [HideInInspector]
    public GameObject inventory;
    private RawImage[] inventoryImages;
    private GameObject[] invenvtoryImagesGameObjects;
    private GameObject[] inventoryItemCountsGameObjects;
    private Text[] inventoryItemCounts;
    private void Awake()
    {
        MaterialsSides = new Vector2[Enum.GetNames(typeof(BlockType)).Length, 6];

        playerService = new PlayerService();

        buildingService = new BuildingService();

        invenvtoryImagesGameObjects = GameObject.FindGameObjectsWithTag("MainInventorySlots");

        inventoryImages = new RawImage[invenvtoryImagesGameObjects.Length];

        for (int i = 0; i < invenvtoryImagesGameObjects.Length; i++)
        {
            inventoryImages[i] = invenvtoryImagesGameObjects[i].GetComponent<RawImage>();
        }

        inventoryItemCountsGameObjects = GameObject.FindGameObjectsWithTag("MainInventoryCount");

        inventoryItemCounts = new Text[inventoryItemCountsGameObjects.Length];

        for (int i = 0; i < inventoryItemCountsGameObjects.Length; i++)
        {
            inventoryItemCounts[i] = inventoryItemCountsGameObjects[i].GetComponent<Text>();
        }

        inventory = GameObject.FindGameObjectWithTag("Inventory");

        inventory.SetActive(false);

        hotbarSlotSelected = GameObject.FindGameObjectsWithTag("ItemSelected");

        hotbarSlotsGameObjects = GameObject.FindGameObjectsWithTag("InventorySlots");

        hotbarSlotsImages = new RawImage[hotbarSlotsGameObjects.Length];

        hotbarCountGameObjects = GameObject.FindGameObjectsWithTag("ItemCount");

        hotbarItemCounts = new Text[hotbarCountGameObjects.Length];

        for (int i = 0; i < hotbarSlotsGameObjects.Length; i++)
        {
            hotbarSlotsImages[i] = hotbarSlotsGameObjects[i].GetComponent<RawImage>();
        }

        for (int i = 0; i < hotbarCountGameObjects.Length; i++)
        {
            hotbarItemCounts[i] = hotbarCountGameObjects[i].GetComponent<Text>();
        }

        perlinNoiseService = new PerlinNoiseService();

        inventoryService = new InventoryService();

        BlockService = new BlockService();

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

        gameController = new GameController(100, 100, 100);
        gameChunks = new GameObject[gameController.GetGameModel().Blocks.GetLength(0) / chunkSize, gameController.GetGameModel().Blocks.GetLength(2) / chunkSize];
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        //Materials = new Material[Enum.GetNames(typeof(BlockType)).Length];
    }

    private void SetRawImage(RawImage image, ItemModel item)
    {
        BlockItemModel block = (BlockItemModel)item;

        Vector2 material = MaterialsSides[(int)block.BlockType, 5];

        image.uvRect = new Rect(material * 0.0625f, new Vector2(0.0625f, 0.0625f));
    }

    public void SlotClicked(int slotNumber)
    {
        if (inventory.activeSelf)
        {
            GameModel gameModel = gameController.GetGameModel();

            if (gameModel.Player.Inventory.ItemOnCursor.ItemCount <= 0)
            {
                Debug.Log(1);
                gameModel.Player.Inventory.ItemOnCursor = gameModel.Player.Inventory.MainInventory[slotNumber / 9, slotNumber % 9];
                gameModel.Player.Inventory.MainInventory[slotNumber / 9, slotNumber % 9] = new InventorySlotModel();
            }
            else if (gameModel.Player.Inventory.MainInventory[slotNumber / 9, slotNumber % 9].ItemCount <= 0)
            {
                Debug.Log(2);
                gameModel.Player.Inventory.MainInventory[slotNumber / 9, slotNumber % 9] = gameModel.Player.Inventory.ItemOnCursor;
                gameModel.Player.Inventory.ItemOnCursor = new InventorySlotModel();
            }
            else
            {
                Debug.Log(3);
                var temp = gameModel.Player.Inventory.MainInventory[slotNumber / 9, slotNumber % 9];
                gameModel.Player.Inventory.MainInventory[slotNumber / 9, slotNumber % 9] = gameModel.Player.Inventory.ItemOnCursor;
                gameModel.Player.Inventory.ItemOnCursor = temp;
            }

            UpdateInventory();
        }
    }

    private void UpdateInventory()
    {
        GameModel gameModel = gameController.GetGameModel();

        for (int i = 0; i < inventoryImages.Length; i++)
        {
            if (gameModel.Player.Inventory.MainInventory[i / 9, i % 9].ItemCount > 0)
            {
                invenvtoryImagesGameObjects[i].SetActive(true);
                inventoryItemCountsGameObjects[i].SetActive(true);

                SetRawImage(inventoryImages[i], gameModel.Player.Inventory.MainInventory[i / 9, i % 9].Item);
                inventoryItemCounts[i].text = gameModel.Player.Inventory.MainInventory[i / 9, i % 9].ItemCount.ToString();
            }
            else
            {
                invenvtoryImagesGameObjects[i].SetActive(false);
                inventoryItemCountsGameObjects[i].SetActive(false);
            }
        }
    }

    private void Start()
    {
        RenderBlocks();
        UpdateInventory();
    }

    // Update is called once per frame
    private void Update()
    {
        Inputs();
    }

    private void ChangeSelectedItem(int number, GameModel gameModel)
    {
        gameModel.Player.Inventory.SelectedBlock = number;
        UpdateInventory();
    }

    private void Inputs()
    {
        GameModel gameModel = gameController.GetGameModel();

        if(Input.GetKeyDown(KeyCode.E))
        {
            if(!inventory.activeSelf)
            {
                UpdateInventory();
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            inventory.SetActive(!inventory.activeSelf);
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            PrintBuilding();
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 blockPos = hit.point;

                if (blockPos.y == (float)Math.Floor(blockPos.y))
                {
                    blockPos.x = (float)Math.Floor(blockPos.x);
                    blockPos.z = (float)Math.Floor(blockPos.z);

                    if (gameModel.Blocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] == null)
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

                    if (gameModel.Blocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] == null)
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

                    if (gameModel.Blocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] == null)
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

                //buildingService.PlaceBuilding(new Point((int)blockPos.x, (int)blockPos.y, (int)blockPos.z), new TreeBuilding(), gameModel);
                //
                //UpdateChunk((int)blockPos.x / chunkSize + 1, (int)blockPos.z / chunkSize, gameController.GetGameModel());
                //UpdateChunk((int)blockPos.x / chunkSize - 1, (int)blockPos.z / chunkSize, gameController.GetGameModel());
                //UpdateChunk((int)blockPos.x / chunkSize, (int)blockPos.z / chunkSize + 1, gameController.GetGameModel());
                //UpdateChunk((int)blockPos.x / chunkSize, (int)blockPos.z / chunkSize - 1, gameController.GetGameModel());
                //UpdateChunk((int)blockPos.x / chunkSize + 1, (int)blockPos.z / chunkSize + 1, gameController.GetGameModel());
                //UpdateChunk((int)blockPos.x / chunkSize + 1, (int)blockPos.z / chunkSize - 1, gameController.GetGameModel());
                //UpdateChunk((int)blockPos.x / chunkSize - 1, (int)blockPos.z / chunkSize + 1, gameController.GetGameModel());
                //UpdateChunk((int)blockPos.x / chunkSize - 1, (int)blockPos.z / chunkSize - 1, gameController.GetGameModel());

                playerService.RightClick((int)blockPos.x, (int)blockPos.y, (int)blockPos.z, gameModel);

                UpdateChunk((int)blockPos.x / chunkSize, (int)blockPos.z / chunkSize, gameController.GetGameModel());
                UpdateInventory();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 blockPos = hit.point;

                if (blockPos.y == (float)Math.Floor(blockPos.y))
                {
                    blockPos.x = (float) Math.Floor(blockPos.x);
                    blockPos.z = (float)Math.Floor(blockPos.z);

                    if (gameModel.Blocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] == null)
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

                    if (gameModel.Blocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] == null)
                    {
                        //HitUp
                        blockPos.x--;
                    }
                }

                if (blockPos.z == Math.Floor(blockPos.z))
                {
                    blockPos.y = (float)Math.Floor(blockPos.y);
                    blockPos.x = (float)Math.Floor(blockPos.x);

                    if (gameModel.Blocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] == null)
                    {
                        //HitUp
                        blockPos.z--;
                    }
                }

                blockPos.x = (float)Math.Floor(blockPos.x);
                blockPos.y = (float)Math.Floor(blockPos.y);
                blockPos.z = (float)Math.Floor(blockPos.z);

                playerService.LeftClick((int)blockPos.x, (int)blockPos.y, (int)blockPos.z, gameModel);

                if((int)(blockPos.x + 1) % chunkSize == 0)
                {
                    UpdateChunk((int)blockPos.x / chunkSize + 1, (int)blockPos.z / chunkSize, gameController.GetGameModel());
                }
                if ((int)blockPos.x % chunkSize == 0)
                {
                    UpdateChunk((int)blockPos.x / chunkSize - 1, (int)blockPos.z / chunkSize, gameController.GetGameModel());
                }
                if (((int)blockPos.z + 1) % chunkSize == 0)
                {
                    UpdateChunk((int)blockPos.x / chunkSize, (int)blockPos.z / chunkSize + 1, gameController.GetGameModel());
                }
                if ((int)blockPos.z % chunkSize == 0)
                {
                    UpdateChunk((int)blockPos.x / chunkSize, (int)blockPos.z / chunkSize - 1, gameController.GetGameModel());
                }

                UpdateChunk((int)blockPos.x / chunkSize, (int)blockPos.z / chunkSize, gameController.GetGameModel());

                UpdateInventory();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeSelectedItem(0, gameModel);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeSelectedItem(1, gameModel);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeSelectedItem(2, gameModel);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangeSelectedItem(3, gameModel);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ChangeSelectedItem(4, gameModel);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            ChangeSelectedItem(5, gameModel);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            ChangeSelectedItem(6, gameModel);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            ChangeSelectedItem(7, gameModel);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            ChangeSelectedItem(8, gameModel);
        }

        if(Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                gameModel.Player.Inventory.SelectedBlock++;
            }
            else
            {
                gameModel.Player.Inventory.SelectedBlock--;
            }

            UpdateInventory(); 
        }
    }

    private void RenderBlocks()
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
                for (int y = 0; y < gameModel.Blocks.GetLength(1); y++)
                {
                    if (gameModel.Blocks[x, y, z] != null)
                    {
                        int[] indexes;
                        float BlockPerc = 1 / pixelSize;
                        float BlockX;
                        float BlockY;

                        float umin;
                        float umax;
                        float vmin;
                        float vmax;

                        //TOP
                        if ((y < gameModel.Blocks.GetLength(1) - 1 && gameModel.Blocks[x, y + 1, z] == null) || y == gameModel.Blocks.GetLength(1) - 1)
                        {
                            indexes = GenerateQuad(used, vectors, triangles, uvs,
                                new Vector3[] { new Vector3(x, y + 1, z), new Vector3(x + 1, y + 1, z), new Vector3(x + 1, y + 1, z + 1), new Vector3(x, y + 1, z + 1) },
                                new int[] { 0, 3, 1, 1, 3, 2 });

                            Vector2 material = MaterialsSides[(int)gameModel.Blocks[x, y, z].BlockType, 0];

                            BlockX = material.x;
                            BlockY = material.y;

                            umin = BlockPerc * BlockX;
                            umax = BlockPerc * (BlockX + 1);
                            vmin = BlockPerc * BlockY;
                            vmax = BlockPerc * (BlockY + 1);

                            //BlockX = Vector2[]

                            uvs[indexes[0]] = new Vector2(umin, vmin);
                            uvs[indexes[1]] = new Vector2(umax, vmin);
                            uvs[indexes[2]] = new Vector2(umax, vmax);
                            uvs[indexes[3]] = new Vector2(umin, vmax);
                        }

                        //BOTTOM
                        if ((y > 0 && gameModel.Blocks[x, y - 1, z] == null) || y == 0)
                        {
                            indexes = GenerateQuad(used, vectors, triangles, uvs,
                            new Vector3[] { new Vector3(x, y, z), new Vector3(x + 1, y, z), new Vector3(x + 1, y, z + 1), new Vector3(x, y, z + 1) },
                            new int[] { 0, 1, 3, 2, 3, 1 });

                            Vector2 material = MaterialsSides[(int)gameModel.Blocks[x, y, z].BlockType, 1];

                            BlockX = material.x;
                            BlockY = material.y;

                            umin = BlockPerc * BlockX;
                            umax = BlockPerc * (BlockX + 1);
                            vmin = BlockPerc * BlockY;
                            vmax = BlockPerc * (BlockY + 1);

                            uvs[indexes[0]] = new Vector2(umin, vmin);
                            uvs[indexes[1]] = new Vector2(umax, vmin);
                            uvs[indexes[2]] = new Vector2(umax, vmax);
                            uvs[indexes[3]] = new Vector2(umin, vmax);
                        }
                        //RIGHT
                        if ((x < gameModel.Blocks.GetLength(0) - 1 && gameModel.Blocks[x + 1, y, z] == null) || x == gameModel.Blocks.GetLength(0) - 1)
                        {
                            indexes = GenerateQuad(used, vectors, triangles, uvs,
                            new Vector3[] { new Vector3(x + 1, y, z), new Vector3(x + 1, y, z + 1), new Vector3(x + 1, y + 1, z), new Vector3(x + 1, y + 1, z + 1) },
                            new int[] { 0, 2, 1, 1, 2, 3 });

                            Vector2 material = MaterialsSides[(int)gameModel.Blocks[x, y, z].BlockType, 2];

                            BlockX = material.x;
                            BlockY = material.y;

                            umin = BlockPerc * BlockX;
                            umax = BlockPerc * (BlockX + 1);
                            vmin = BlockPerc * BlockY;
                            vmax = BlockPerc * (BlockY + 1);

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
                        if ((x > 0 && gameModel.Blocks[x - 1, y, z] == null) || x == 0)
                        {
                            indexes = GenerateQuad(used, vectors, triangles, uvs,
                            new Vector3[] { new Vector3(x, y, z), new Vector3(x, y, z + 1), new Vector3(x, y + 1, z), new Vector3(x, y + 1, z + 1) },
                            new int[] { 1, 2, 0, 1, 3, 2 });

                            Vector2 material = MaterialsSides[(int)gameModel.Blocks[x, y, z].BlockType, 3];

                            BlockX = material.x;
                            BlockY = material.y;

                            umin = BlockPerc * BlockX;
                            umax = BlockPerc * (BlockX + 1);
                            vmin = BlockPerc * BlockY;
                            vmax = BlockPerc * (BlockY + 1);

                            uvs[indexes[0]] = new Vector2(umin, vmin);
                            uvs[indexes[2]] = new Vector2(umin, vmax);
                            uvs[indexes[1]] = new Vector2(umax, vmin);
                            uvs[indexes[3]] = new Vector2(umax, vmax);
                        }
                        //FRONT
                        if ((z > 0 && gameModel.Blocks[x, y, z - 1] == null) || z == 0)
                        {
                            indexes = GenerateQuad(used, vectors, triangles, uvs,
                            new Vector3[] { new Vector3(x, y, z), new Vector3(x + 1, y, z), new Vector3(x, y + 1, z), new Vector3(x + 1, y + 1, z) },
                            new int[] { 0, 2, 1, 1, 2, 3 });

                            Vector2 material = MaterialsSides[(int)gameModel.Blocks[x, y, z].BlockType, 4];

                            BlockX = material.x;
                            BlockY = material.y;

                            umin = BlockPerc * BlockX;
                            umax = BlockPerc * (BlockX + 1);
                            vmin = BlockPerc * BlockY;
                            vmax = BlockPerc * (BlockY + 1);

                            uvs[indexes[0]] = new Vector2(umin, vmin);
                            uvs[indexes[1]] = new Vector2(umax, vmin);
                            uvs[indexes[2]] = new Vector2(umin, vmax);
                            uvs[indexes[3]] = new Vector2(umax, vmax);
                        }
                        //BACK
                        if ((z < gameModel.Blocks.GetLength(2) - 1 && gameModel.Blocks[x, y, z + 1] == null) || z == gameModel.Blocks.GetLength(2) - 1)
                        {
                            indexes = GenerateQuad(used, vectors, triangles, uvs,
                            new Vector3[] { new Vector3(x, y, z + 1), new Vector3(x + 1, y, z + 1), new Vector3(x, y + 1, z + 1), new Vector3(x + 1, y + 1, z + 1) },
                            new int[] { 1, 2, 0, 3, 2, 1 });

                            Vector2 material = MaterialsSides[(int)gameModel.Blocks[x, y, z].BlockType, 5];

                            BlockX = material.x;
                            BlockY = material.y;

                            umin = BlockPerc * BlockX;
                            umax = BlockPerc * (BlockX + 1);
                            vmin = BlockPerc * BlockY;
                            vmax = BlockPerc * (BlockY + 1);

                            uvs[indexes[0]] = new Vector2(umin, vmin);
                            uvs[indexes[1]] = new Vector2(umax, vmin);
                            uvs[indexes[2]] = new Vector2(umin, vmax);
                            uvs[indexes[3]] = new Vector2(umax, vmax);
                        }
                    }
                }
            }
        }

        mesh.vertices = vectors.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
    }

    private void PrintBuilding()
    {
        GameModel gameModel = gameController.GetGameModel();

        int minX = int.MaxValue;
        int minY = int.MaxValue;
        int minZ = int.MaxValue;

        int maxX = -1;
        int maxY = -1;
        int maxZ = -1;

        for (int x = 0; x < gameModel.Blocks.GetLength(0); x++)
        {
            for (int y = 1; y < gameModel.Blocks.GetLength(1); y++)
            {
                for (int z = 0; z < gameModel.Blocks.GetLength(2); z++)
                {
                    if (gameModel.Blocks[x, y, z] != null)
                    {
                        minX = Math.Min(minX, x);
                        minY = Math.Min(minY, y);
                        minZ = Math.Min(minZ, z);

                        maxX = Math.Max(maxX, x);
                        maxY = Math.Max(maxY, y);
                        maxZ = Math.Max(maxZ, z);
                    }
                }
            }
        }

        StringBuilder result = new StringBuilder();

        result.Append($"{maxX - minX + 1} {maxY - minY + 1} {maxZ - minZ + 1}\n");

        for (int x = 0; x < gameModel.Blocks.GetLength(0); x++)
        {
            for (int y = 1; y < gameModel.Blocks.GetLength(1); y++)
            {
                for (int z = 0; z < gameModel.Blocks.GetLength(2); z++)
                {
                    if (gameModel.Blocks[x, y, z] != null)
                    {
                        //BuildingBlocks[0, 0, 0] = new LogBlock(new Point(0, 0, 0));
                        result.Append($"BuildingBlocks[{x - minX}, {y - minY}, {z - minZ}] = new {gameModel.Blocks[x,y,z].BlockType.ToString()}(new Point({x - minX}, {y - minY}, {z - minZ}));\n");
                    }
                }
            }
        }

        Debug.Log(result);
    }
}
