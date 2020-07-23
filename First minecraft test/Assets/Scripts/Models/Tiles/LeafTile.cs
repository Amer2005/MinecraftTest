using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models.Tiles
{
    public class LeafTile : TileModel
    {
        public LeafTile(Point point) : base(point, TileType.LeafTile)
        {

        }
    }
}
