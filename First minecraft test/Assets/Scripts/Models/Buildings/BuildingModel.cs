using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    public class BuildingModel
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Lenght { get; set; }
        public BlockModel[,,] BuildingBlocks { get; set; }

        public Point StartPoint { get; set; }

        public BuildingModel(int width, int height, int lenght, Point startPoint)
        {
            StartPoint = startPoint;
            Width = width;
            Height = height;
            Lenght = lenght;
            BuildingBlocks = new BlockModel[Width, Height, Lenght];
        }
    }
}
