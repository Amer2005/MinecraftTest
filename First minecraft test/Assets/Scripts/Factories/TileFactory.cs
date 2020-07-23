using Assets.Scripts.Models;
using Assets.Scripts.Models.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Factories
{
    public class TileFactory
    {
        public TileModel CreateTile(Point point, TileType tileType)
        {
            switch(tileType)
            {
                case TileType.DefaultTile:
                    return new DefaultTile(point);
                case TileType.DirtTile:
                    return new DirtTile(point);
                case TileType.GrassTile:
                    return new GrassTile(point);
                case TileType.LogTile:
                    return new LogTile(point);
                default:
                    throw new Exception("Tile not found");
            }
        }
    }
}
