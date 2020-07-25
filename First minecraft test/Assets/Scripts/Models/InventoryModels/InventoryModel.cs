using Assets.Scripts.Models.InventoryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    public class InventoryModel
    {
        private int selectedBlock;

        public InventoryModel()
        {
            MainInventory = new InventorySlotModel[4,9];

            for (int i = 0; i < MainInventory.GetLength(0); i++)
            {
                for (int j = 0; j < MainInventory.GetLength(1); j++)
                {
                    MainInventory[i, j] = new InventorySlotModel();
                }
            }

            ItemOnCursor = new InventorySlotModel();

            SelectedBlock = 0;
        }

        public InventorySlotModel ItemOnCursor { get; set; }

        public InventorySlotModel[,] MainInventory { get; set; }

        public int SelectedBlock
        {
            get
            {
                return selectedBlock;
            }
            set
            {
                if (value > 8)
                {
                    selectedBlock = 0;
                }
                else if (value < 0)
                {
                    selectedBlock = 8;
                }
                else
                {
                    selectedBlock = value;
                }
            }
        }
    }
}
