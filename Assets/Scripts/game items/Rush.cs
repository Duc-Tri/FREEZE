using UnityEngine;

namespace IHateWinter
{
    public class Rush : AResource, IHarvestable
    {
        string IHarvestable.Name { get => name; }

        private void Awake()
        {
            Init(RESOURCE.RUSH);
        }

    }

}