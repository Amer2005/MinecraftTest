using Assets.Scripts.Factories;
using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Services
{
    public class TileService
    {
        private TileFactory tileFactory;
        private RandomService randomService;
        public TileService()
        {
            tileFactory = new TileFactory();
            randomService = new RandomService();
        }

        public TileModel[,,] GenerateTileMap(int width, int height, int lenght)
        {
            TileModel[,,] tiles = new TileModel[width, height, lenght];

            int maxHeight = 4;
            int minHeight = 3;

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < lenght; z++)
                {
                    int h = randomService.GenerateInt(minHeight, maxHeight + 1);

                    for(int y = 0;y < h;y++)
                    {
                        tiles[x, y, z] = tileFactory.CreateTile(new Point(x, y, z), TileType.DirtTile);
                    }
                }
            }

            return tiles;
        }
    }
}
