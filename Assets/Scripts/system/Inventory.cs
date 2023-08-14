
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

namespace IHateWinter
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private GameObject fishPrefab;
        private const int INVENTORY_CAPACITY = 7;

        public InventoryItem[] Items;

        public ButtonItem[] ButtonItems;

        public static Inventory Instance;

        public static Action OnInventoryUpdate;
        public static Action<Fish> OnCreateFish;


        private void OnEnable()
        {
            Instance = this;
            //canvas = GetComponent<Canvas>();

            Items = new InventoryItem[INVENTORY_CAPACITY];

            // TODO: instantiate Button dynamically dude !
            ButtonItems = GetComponentsInChildren<ButtonItem>();

            Assert.AreEqual(INVENTORY_CAPACITY, ButtonItems.Length);
        }

        private void Start()
        {
            UpdateUI();
        }

        internal bool TryAddTool(TOOL tool)
        {
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i] == null)
                {
                    Items[i] = GameManager.GameData.SearchItemByToolType(tool);
                    //Items[i].InStack = 1;
                    OnInventoryUpdate?.Invoke();
                    UpdateUI();
                    Debug.Log("===== INVENTORY new : " + String.Join(" ■ ", Items.Select(v => v?.name)));

                    return true;
                }
            }

            return false; // inventory full, all stacks full
        }

        public bool TryAddResource(AResource resource)
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
                        OnInventoryUpdate?.Invoke();
                        UpdateUI();
                        Debug.Log("INVENTORY add : " + String.Join(" ■ ", Items.Select(v => $"{v?.name}{v?.InStack}")));

                        return true;
                    }
                }
            }

            // STACKING failed, try to make new slot --------------------------
            if (fullSlots < INVENTORY_CAPACITY)
                for (int i = 0; i < Items.Length; i++)
                {
                    if (Items[i] == null)
                    {
                        Items[i] = GameManager.GameData.SearchItemByResourceType(resource.type);
                        Items[i].InStack = 1;
                        OnInventoryUpdate?.Invoke();
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

        public bool HasTool(TOOL tool)
        {
            for (int i = 0; i < Items.Length; i++)
                if (Items[i] != null && Items[i].ToolType == tool)
                {
                    //Debug.Log("HasTool - " + tool + " TRUE");
                    return true;
                }

            //Debug.Log("HasTool - " + tool + " FALSE");
            return false;
        }


        public bool HasItem(RESOURCE r, byte n)
        {
            int total = 0;
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i] != null && Items[i].ResourceType == r)
                {
                    if (Items[i].InStack >= n)
                        return true;
                    else if ((total += Items[i].InStack) >= n)
                        return true;
                }
            }

            return false;
        }

        internal bool HasItems(ResourceNeeded[] resources)
        {
            foreach (var r in resources) if (!Inventory.Instance.HasItem(r.resource, r.quantity)) return false;

            return true;
        }

        internal bool ConsumeItems(ResourceNeeded[] resources)
        {
            bool result = true;
            foreach (var r in resources)
                result &= ConsumeItem(r.resource, r.quantity);

            return result; // false if something wrong happens (not enough resources)
        }

        private bool ConsumeItem(RESOURCE r, int quantity)
        {
            for (int i = 0; i < Items.Length; i++)
            {
                InventoryItem ii = Items[i];
                if (ii != null && ii.ResourceType == r)
                {
                    int amount = Math.Min(quantity, ii.InStack);
                    ii.InStack -= amount;
                    quantity -= amount;

                    if (ii.InStack <= 0) Items[i] = null;
                }

                if (quantity == 0) break;
            }

            OnInventoryUpdate?.Invoke();
            UpdateUI();

            return (quantity == 0);
        }

        internal void ConsumeTool(TOOL tool)
        {
            for (int i = 0; i < Items.Length; i++)
            {
                InventoryItem ii = Items[i];
                if (ii != null && ii.ToolType == tool)
                {
                    Items[i] = null;
                    break;
                }
            }

            OnInventoryUpdate?.Invoke();
            UpdateUI();
        }


        NavMeshHit nmHit;
        public void OnButtonItemClick(ButtonItem bi)
        {

            if (bi.item.ResourceType == RESOURCE.FISH)
            {
                GameObject f = Instantiate(fishPrefab);
                f.transform.position = GameManager.Player.transform.position + Vector3.right * 2f + Vector3.forward * 2f;

                Vector3 pos = GameManager.Player.transform.position + UnityEngine.Random.onUnitSphere * 3f;


                if (NavMesh.SamplePosition(pos, out nmHit, 5, NavMesh.AllAreas))
                {
                    f.transform.position = nmHit.position;
                    Inventory.Instance.ConsumeItem(RESOURCE.FISH, 1);
                    OnCreateFish?.Invoke(f.GetComponent<Fish>());
                }

            }

        }

    }

}