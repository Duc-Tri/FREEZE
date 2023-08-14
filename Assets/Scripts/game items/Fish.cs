using UnityEngine;

namespace IHateWinter
{
    public class Fish : AResource, IHarvestable
    {
        string IHarvestable.Name { get => name; }

        private void Awake()
        {
            Init(RESOURCE.FISH);
        }

    }

}