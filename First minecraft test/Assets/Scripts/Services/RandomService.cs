using UnityEngine;

namespace Assets.Scripts.Services
{
    public class RandomService
    {
        public int GenerateInt(int min, int maxExclusive)
        {
            return Random.Range(min, maxExclusive);
        }
    }
}