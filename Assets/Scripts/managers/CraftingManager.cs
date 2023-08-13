using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace IHateWinter
{
    public class CraftingManager : MonoBehaviour
    {
        [SerializeField] private Button btMakeFire;
        [SerializeField] private Button btMakeFishingRod;

        [SerializeField] private GameObject firePrefab;

        public static CraftingManager Instance;
        private CraftingRecipe[] craftingRecipes;
        private ResourceNeeded[] FireCampRecipe;
        private ResourceNeeded[] FishingRodRecipe;

        private bool CanMake(ResourceNeeded[] resources) => Inventory.Instance.HasItems(resources);

        private void OnEnable()
        {
            Instance = this;
        }

        private void Start()
        {
            craftingRecipes = GameManager.GameData.craftingRecipes;

            // TODO: refactor this sh*t !
            foreach (var r in craftingRecipes)
            {
                if (r.wantedObject == CRAFT_TEMP.FIRE_CAMP) FireCampRecipe = r.resourcesNeeded;
                if (r.wantedObject == CRAFT_TEMP.FISHING_ROD) FishingRodRecipe = r.resourcesNeeded;
            }
            btMakeFire.enabled = true;
            btMakeFishingRod.enabled = true;

            InventoryUpdated();
        }

        private void SetEnableButton(Button but, bool enable)
        {
            but.interactable = (GameManager.Instance.CanAlwaysCraft || enable);
            but.transform.GetChild(0).GetComponent<Image>().color = (GameManager.Instance.CanAlwaysCraft || enable) ? Color.white : Color.black;
        }

        NavMeshHit navMeshHit;

        public static Action<Fire> OnFireCampCreated;

        public void OnMakeFireClick()
        {
            if (GameManager.Instance.CanAlwaysCraft || CanMake(FireCampRecipe))
            {
                // NO CHEcK ?????????????
                Inventory.Instance.ConsumeItems(FireCampRecipe);

                Transform fire = Instantiate(firePrefab, GameManager.Instance.transform).transform;

                if (NavMesh.SamplePosition(GameManager.Player.transform.position, out navMeshHit, 10, NavMesh.AllAreas))
                {
                    fire.position = navMeshHit.position;
                }

                OnFireCampCreated?.Invoke(fire.GetComponent<Fire>());

                InventoryUpdated();
            }
        }

        public void OnMakeFishingRodClick()
        {
            if ((GameManager.Instance.CanAlwaysCraft || CanMake(FishingRodRecipe)) && !Inventory.Instance.HasTool(TOOL.FISHING_ROD))
            {
                Inventory.Instance.ConsumeItems(FishingRodRecipe);
                Inventory.Instance.TryAdd(TOOL.FISHING_ROD);

                InventoryUpdated();
            }
        }

        public void InventoryUpdated()
        {
            // fire camp
            SetEnableButton(btMakeFire, CanMake(FireCampRecipe));

            // fishing
            SetEnableButton(btMakeFishingRod, !Inventory.Instance.HasTool(TOOL.FISHING_ROD) && CanMake(FishingRodRecipe));
        }

        /*
        public void OnAllToolsClick()    {    }

        public void OnCraftTool()    {    }

        public void OnAllTrapsClick()    {    }
        */

    }

}
