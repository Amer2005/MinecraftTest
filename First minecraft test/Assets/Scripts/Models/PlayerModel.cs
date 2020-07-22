using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    public class PlayerModel
    {
        public InventoryModel Inventory { get; set; }

        public PlayerModel()
        {
            Inventory = new InventoryModel();
        }
    }
}
