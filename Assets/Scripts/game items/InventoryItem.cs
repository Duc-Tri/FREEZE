using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IHateWinter
{
    [CreateAssetMenu(fileName = "InventoryItem", menuName = "IHW/Inventory Item")]
    public class InventoryItem : ScriptableObject
    {
        public string ItemName;

        public INVENTORY_ITEM ItemType;

        public RESOURCE ResourceType;
        public TOOL ToolType;

        public bool IsStackable;
        public int MaxStackable;

        public Sprite UISprite;
    }
}