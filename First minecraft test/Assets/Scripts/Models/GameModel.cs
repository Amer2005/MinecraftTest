using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    public class GameModel
    {
        public TileModel[,,] Tiles { get; set; }
        public InventoryModel Inventory { get; set; }
    }
}
