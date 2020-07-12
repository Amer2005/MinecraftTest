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
    private GameObject[,,] tileGameObjects;
    private const string pathToTiles = "Prefabs/Tiles";
    public GameObject DefaultTile;
    private Camera playerCamera;
    public Material[] Materials;

    private void Awake()
    {
        gameController = new GameController(10, 10, 10);
        tileGameObjects = new GameObject[gameController.GetGameModel().Tiles.GetLength(0), gameController.GetGameModel().Tiles.GetLength(1), gameController.GetGameModel().Tiles.GetLength(2)];
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
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                string blockHit = hit.transform.name;

                string[] blockInfo = blockHit.Split(' ');

                int x = int.Parse(blockInfo[1]);
                int y = int.Parse(blockInfo[2]);
                int z = int.Parse(blockInfo[3]);

                gameController.DestroyTile(x, y, z);

                UpdateTile(x, y, z, gameController.GetGameModel());
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 vector = hit.normal;
                Transform transform = hit.collider.transform;

                string blockHit = hit.transform.name;

                string[] blockInfo = blockHit.Split(' ');

                int x = int.Parse(blockInfo[1]);
                int y = int.Parse(blockInfo[2]);
                int z = int.Parse(blockInfo[3]);

                GameModel gameModel = gameController.GetGameModel();

                if (vector == transform.right)
                {
                    gameController.PlaceTile(x + 1, y, z, gameModel.Inventory.HotBar[gameModel.Inventory.SelectedBlock]);
                    UpdateTile(x + 1, y, z, gameController.GetGameModel());
                }
                if (vector == -transform.right)
                {
                    gameController.PlaceTile(x - 1, y, z, gameModel.Inventory.HotBar[gameModel.Inventory.SelectedBlock]);
                    UpdateTile(x - 1, y, z, gameController.GetGameModel());
                }
                if (vector == transform.forward)
                {
                    gameController.PlaceTile(x, y, z + 1, gameModel.Inventory.HotBar[gameModel.Inventory.SelectedBlock]);
                    UpdateTile(x, y, z + 1, gameController.GetGameModel());
                }
                if (vector == -transform.forward)
                {
                    gameController.PlaceTile(x, y, z - 1, gameModel.Inventory.HotBar[gameModel.Inventory.SelectedBlock]);
                    UpdateTile(x, y, z - 1, gameController.GetGameModel());
                }
                if (vector == transform.up)
                {
                    gameController.PlaceTile(x, y + 1, z, gameModel.Inventory.HotBar[gameModel.Inventory.SelectedBlock]);
                    UpdateTile(x, y + 1, z, gameController.GetGameModel());
                }
                if (vector == -transform.up)
                {
                    gameController.PlaceTile(x, y - 1, z, gameModel.Inventory.HotBar[gameModel.Inventory.SelectedBlock]);
                    UpdateTile(x, y - 1, z, gameController.GetGameModel());
                }

            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            gameController.GetGameModel().Inventory.SelectedBlock = 0;
            Debug.Log("1");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gameController.GetGameModel().Inventory.SelectedBlock = 1;
            Debug.Log("2");
        }
    }

    private void RenderTiles()
    {
        GameModel gameModel = gameController.GetGameModel();

        for (int x = 0; x < gameModel.Tiles.GetLength(0); x++)
        {
            for (int y = 0; y < gameModel.Tiles.GetLength(1); y++)
            {
                for (int z = 0; z < gameModel.Tiles.GetLength(2); z++)
                {
                    UpdateTile(x, y, z, gameModel);
                }
            }
        }
    }

    private void UpdateTile(int x, int y, int z, GameModel gameModel)
    {
        if (gameModel.Tiles.GetLength(0) > x && x >= 0 && gameModel.Tiles.GetLength(1) > y && y >= 0 && gameModel.Tiles.GetLength(2) > z && z >= 0)
        {
            if (tileGameObjects[x, y, z] == null && gameModel.Tiles[x, y, z] != null)
            {
                tileGameObjects[x, y, z] = Instantiate(DefaultTile, new Vector3(x, y, z), new Quaternion());
                var meshRenderer = DefaultTile.GetComponent<MeshRenderer>();

                int index = (int)gameModel.Tiles[x, y, z].TileType;

                var material = Materials[index];
                meshRenderer.sharedMaterial = material;

                tileGameObjects[x, y, z].name = gameModel.Tiles[x, y, z].TileType.ToString() + $" {x} {y} {z}";
            }
            else if (tileGameObjects[x, y, z] != null && gameModel.Tiles[x, y, z] == null)
            {
                Destroy(tileGameObjects[x, y, z]);

                tileGameObjects[x, y, z] = null;
            }
            if (tileGameObjects[x, y, z] != null && gameModel.Tiles[x, y, z] != null && (tileGameObjects[x, y, z].name.Split(' ')[0] != gameModel.Tiles[x, y, z].TileType.ToString() || tileGameObjects[x, y, z].GetComponent<MeshRenderer>().material != Materials[(int)gameModel.Tiles[x, y, z].TileType]))
            {
                Destroy(tileGameObjects[x, y, z]);

                tileGameObjects[x, y, z] = null;

                tileGameObjects[x, y, z] = Instantiate(DefaultTile, new Vector3(x, y, z), new Quaternion());
                var meshRenderer = DefaultTile.GetComponent<MeshRenderer>();

                int index = (int)gameModel.Tiles[x, y, z].TileType;

                var material = Materials[index];
                meshRenderer.sharedMaterial = material;

                tileGameObjects[x, y, z].name = gameModel.Tiles[x, y, z].TileType.ToString() + $" {x} {y} {z}";
            }
        }
    }
}
