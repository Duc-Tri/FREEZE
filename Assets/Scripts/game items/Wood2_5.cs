using UnityEngine;

namespace IHateWinter
{
    public class Wood2_5 : AResource, IHarvestable
    {
        string IHarvestable.Name { get => name; }

        private void Awake()
        {
            Init(RESOURCE.WOOD);
        }

    }


}