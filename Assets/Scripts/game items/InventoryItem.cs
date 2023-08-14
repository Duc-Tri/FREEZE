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

        public ITEM_TYPE ItemType;

        public RESOURCE ResourceType;
        public TOOL ToolType;

        public bool IsStackable;
        public int MaxStackable;

        [HideInInspector]
        public int InStack;

        public Sprite UISprite;

        public InventoryItem Clone()
        {
            InventoryItem ii = ScriptableObject.CreateInstance<InventoryItem>() as InventoryItem;

            ii.ItemName = this.ItemName;
            ii.ItemType = this.ItemType;
            ii.ResourceType = this.ResourceType;
            ii.ToolType = this.ToolType;
            ii.IsStackable = this.IsStackable;
            ii.MaxStackable = this.MaxStackable;
            ii.UISprite = this.UISprite;

            return ii;
        }



    }
}