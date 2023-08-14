using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace IHateWinter
{
    [CreateAssetMenu(fileName = "GameData", menuName = "IHW/Game Data")]
    public class GameData : ScriptableObject
    {
        public CraftingRecipe[] craftingRecipes;

        public TemperatureTime[] temperatureTimes;

        public InventoryItem[] allInventoryItems;

        public ItemToInstantiate[] itemsToInstanciate;

        public InventoryItem SearchItemByResourceType(RESOURCE res)
        {
            foreach (InventoryItem item in allInventoryItems)
            {
                if (item.ResourceType == res)
                    return item.Clone();
            }

            return null;
        }

        public InventoryItem SearchItemByToolType(TOOL type)
        {
            foreach (var item in allInventoryItems)
            {
                if (item.ToolType == type)
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

    [Serializable]
    public struct CraftingRecipe
    {
        public CRAFT_TEMP wantedObject;
        public Dictionary<RESOURCE, int> resources;
        public ResourceNeeded[] resourcesNeeded;
    }

    [Serializable]
    public struct ResourceNeeded
    {
        public RESOURCE resource;
        public byte quantity;
    }

}
