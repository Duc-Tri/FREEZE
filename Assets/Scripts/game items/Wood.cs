using UnityEngine;

namespace IHateWinter
{
    public class Wood : AResource, IHarvestable
    {
        string IHarvestable.Name { get => name; }

        private void Awake()
        {
            Init(RESOURCE.WOOD);
        }

    }


}