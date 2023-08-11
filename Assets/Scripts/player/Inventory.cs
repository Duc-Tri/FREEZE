
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace IHateWinter
{
    public class Inventory : MonoBehaviour
    {
        private const int INVENTORY_CAPACITY = 10;
        public InventoryItem[] Items = new InventoryItem[INVENTORY_CAPACITY];

        public ButtonItem[] ButtonItems;
        private Canvas canvas;

        public static Inventory Instance;

        private void Awake()
        {
            Instance = this;
            canvas = GetComponent<Canvas>();

            // TODO: instantiate Button dynamically dude !
            ButtonItems = canvas.GetComponentsInChildren<ButtonItem>();

            Assert.AreEqual(INVENTORY_CAPACITY, ButtonItems.Length);
            UpdateUI();
        }

        public bool TryAdd(AResource resource)
        {
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i] == null)
                {
                    Items[i] = GameManager.GameData.SearchItemByResourceType(resource.type);
                    UpdateUI();

                    Debug.Log("INVENTORY: " + String.Join(" ■ ", Items.Select(v => v?.name)));

                    return true;
                }
            }

            return false; // inventory full
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