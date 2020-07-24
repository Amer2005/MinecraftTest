using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    public class GameModel
    {
        public BlockModel[,,] Blocks { get; set; }
        public PlayerModel Player { get; set; }
    }
}
