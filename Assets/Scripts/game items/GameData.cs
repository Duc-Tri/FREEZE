using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IHateWinter
{
    [CreateAssetMenu(fileName = "GameData", menuName = "IHW/Game Data")]
    public class GameData : ScriptableObject
    {
        public TemperatureTime[] temperatureTimes;

        public InventoryItem[] allInventoryItems;

        public ItemToInstantiate[] itemsToInstanciate;

        public InventoryItem SearchItemByResourceType(RESOURCE res)
        {
            foreach (var item in allInventoryItems)
            {
                if (item.ResourceType == res)
                    return item;
            }

            return null;
        }

    }

    [Serializable]
    public struct ItemToInstantiate
    {
        public Transform prefab;
        public string namePrefix;
        public int count;
    }
}
