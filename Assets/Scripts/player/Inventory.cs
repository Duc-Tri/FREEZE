using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace IHateWinter
{
    public class Inventory : MonoBehaviour
    {
        private const int INVENTORY_CAPACITY = 10;
        public IInventoryItem[] Items = new IInventoryItem[INVENTORY_CAPACITY];

        public Button[] ButtonItems;
        private Canvas canvas;

        public static Inventory Instance;

        public bool TryAdd(IInventoryItem ii)
        {
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i] == null)
                {
                    Items[i] = ii;
                    UpdateUI();

                    Debug.Log("INVENTORY: " + String.Join(" ■ ", Items.Select(v => v?.Name)));

                    return true;
                }
            }

            return false; // inventory full
        }

        private void UpdateUI()
        {
            for (int i = 0; i < INVENTORY_CAPACITY; i++)
            {
                Button button = ButtonItems[i];

                button.GetComponentInChildren<TextMeshProUGUI>().text = (Items[i] != null) ? Items[i].Name : "0";
            }

        }

        private void Awake()
        {
            Instance = this;
            canvas = GetComponent<Canvas>();

            // TODO: instantiate Button dynamically dude !
            ButtonItems = canvas.GetComponentsInChildren<Button>();

            Assert.AreEqual(INVENTORY_CAPACITY, ButtonItems.Length);
            UpdateUI();
        }

    }

}