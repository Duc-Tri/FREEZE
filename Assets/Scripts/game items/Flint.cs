using UnityEngine;

namespace IHateWinter
{
    public class Flint : AResource, IHarvestable
    {
        string IHarvestable.Name { get => name; }

        private void Awake()
        {
            Init(RESOURCE.FLINT);
        }

    }

}