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
        private const int MAX_ITEMS = 10;
        public IInventoryItem[] Items = new IInventoryItem[MAX_ITEMS];

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
            Debug.Log("UpdateUI TODO !");
        }

        private void Awake()
        {
            Instance = this;
            canvas = GetComponent<Canvas>();

            // TODO: instantiate Button dynamically dude !
            ButtonItems = canvas.GetComponentsInChildren<Button>();

            Assert.AreEqual(MAX_ITEMS, ButtonItems.Length);

            for (int i = 0; i < ButtonItems.Length; i++)
            {
                Button button = ButtonItems[i];
                button.GetComponentInChildren<TextMeshProUGUI>().text = "bt" + i;
            }
        }

    }

}