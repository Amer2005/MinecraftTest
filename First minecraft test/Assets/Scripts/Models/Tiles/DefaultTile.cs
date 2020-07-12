using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models.Tiles
{
    public class DefaultTile : TileModel
    {
        public DefaultTile(Point point) : base(point, TileType.DefaultTile)
        {

        }
    }
}
