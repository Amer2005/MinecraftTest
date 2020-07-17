using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class PerlinNoiseService
    {
        public float perlinNoise(float x, float y)
        {
            return Mathf.PerlinNoise(x, y);
        }
    }
}
