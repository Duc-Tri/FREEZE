
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace IHateWinter
{
    public class Inventory : MonoBehaviour
    {
        private const  int INVENTORY_CAPACITY = 5;

        public InventoryItem[] Items = new InventoryItem[INVENTORY_CAPACITY];

        public ButtonItem[] ButtonItems;

        public static Inventory Instance;

        private void OnEnable()
        {
            Instance = this;
            //canvas = GetComponent<Canvas>();

            // TODO: instantiate Button dynamically dude !
            ButtonItems = GetComponentsInChildren<ButtonItem>();

            Assert.AreEqual(INVENTORY_CAPACITY, ButtonItems.Length);
        }

        private void Start()
        {
            UpdateUI();
        }

        public bool TryAdd(AResource resource)
        {
            InventoryItem ii;

            // FIRST, try to stack resource -----------------------------------
            int fullSlots = 0;
            for (int i = 0; i < Items.Length; i++)
            {
                ii = Items[i];

                if (ii != null)
                {
                    fullSlots++;
                    if (ii.ResourceType == resource.type && ii.IsStackable && ii.InStack < ii.MaxStackable)
                    {
                        ii.InStack++;
                        UpdateUI();
                        Debug.Log("INVENTORY add : " + String.Join(" ■ ", Items.Select(v => $"{v?.name}{v?.InStack}")));

                        return true;
                    }
                }
            }

            // THEN, try to make new slot -------------------------------------
            if (fullSlots < INVENTORY_CAPACITY)
                for (int i = 0; i < Items.Length; i++)
                {
                    if (Items[i] == null)
                    {
                        Items[i] = GameManager.GameData.SearchItemByResourceType(resource.type);
                        Items[i].InStack = 1;
                        UpdateUI();
                        Debug.Log("INVENTORY new : " + String.Join(" ■ ", Items.Select(v => v?.name)));

                        return true;
                    }
                }

            return false; // inventory full, all stacks full
        }

        private void UpdateUI()
        {
            for (int i = 0; i < INVENTORY_CAPACITY; i++)
            {
                ButtonItems[i].SetItem(Items[i]);
            }
        }


    }

}