using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IHateWinter
{
    // Responsible for adding and removing LISTENERS
    public class GameManager : MonoBehaviour
    {
        [SerializeField] public bool PlayerInvincible = false; // cheater !
        [SerializeField] public bool CanAlwaysCraft = false; // cheater !

        [SerializeField] private GameData gameData;
        [SerializeField] private Canvas GameOverCanvas;

        TreeManager treeManager;

        //[SerializeField][Range(0, 5000)] 
        public float halfObjectsMaxX, halfObjectsMaxZ;

        public static Player Player;
        public static GAME_STATE GameState { get; private set; }
        public static GameManager Instance { get; private set; }
        public static GameData GameData { get { return Instance.gameData; } }


        public static List<Transform> allPenguins;

        private void OnEnable()
        {
            Instance = this;

            if (Player == null) Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            Player.gameObject.SetActive(false);

            allPenguins = new List<Transform>();
        }

        private void Awake()
        {
            GameObject floor = GameObject.FindGameObjectWithTag("Floor");
            Bounds floorBounds = floor.GetComponent<Collider>().bounds;
            halfObjectsMaxX = 0.95f * floorBounds.size.x * 0.5f;
            halfObjectsMaxZ = 0.95f * floorBounds.size.z * 0.5f;

            //  Debug.Log("GAME MANAGER: " + floorBounds.size);

            treeManager = new TreeManager();

            foreach (var item in gameData.itemsToInstanciate)
                InstantiateResource(item.prefab, item.count, item.namePrefix);

            TemperatureSystem.Instance.Init(gameData.temperatureTimes);
        }

        IEnumerator Start()
        {
            AddListeners();
            GameOverCanvas.gameObject.SetActive(false);
            GameState = GAME_STATE.IN_GAME; // TODO : change it when we have menu !

            //////////////////////// to wait for Nawmesh loading, etc.
            yield return new WaitForSeconds(3f);
            //yield return null;

            Player.gameObject.SetActive(true);
        }

        private void InstantiateResource(Transform resourcePrefab, int numberResource, string nameTemplate)
        {
            for (int i = 0; i < numberResource; i++)
            {
                Transform r = Instantiate(resourcePrefab, this.transform);

                float randX = RandomRangeNotTooNearZero(-halfObjectsMaxX, halfObjectsMaxX, 3);
                float randz = RandomRangeNotTooNearZero(-halfObjectsMaxZ, halfObjectsMaxX, 3);

                r.position = new Vector3(randX, 0, randz);
                r.name = nameTemplate + i;
            }
        }

        private float RandomRangeNotTooNearZero(float min, float max, float avoid)
        {
            float v = 0; UnityEngine.Random.Range(min, max);
            while (Mathf.Abs(v) < avoid)
            {
                v = UnityEngine.Random.Range(min, max);
            }

            return v;
        }

        private void PenguinInstantiated(Penguin penguin)
        {
            allPenguins.Add(penguin.transform);
        }

        void AddListeners()
        {
            //RemoveListeners();

            //Fire.OnPlayerInsideFireWarm += GUITemperature.Instance.PlayerInsideFireWarm;
            //Fire.OnPlayerOutSideFireWarm += GUITemperature.Instance.PlayerOutsideFireWarm;

            CraftingManager.OnFireCampCreated += GameManager.CallPenguinToStopFire;

            Fire.OnPlayerInsideFireWarm += Player.InsideFireWarm;
            Fire.OnPlayerOutSideFireWarm += Player.OutsideFireWarm;

            Inventory.OnInventoryUpdate += CraftingManager.Instance.InventoryUpdated;
            Inventory.OnCreateFish += GameManager.CallPenguinsToFish;

            MouseManager.OnActOnResource += Player.ActOnResource;
            MouseManager.OnClickOnFloor += Player.MoveAgent;
            MouseManager.OnClickOnWater += Player.GoToFishing;
            MouseManager.OnHoverOnResource += TextHelperManager.TextHover;
            MouseManager.OnMouseWheel += FollowingCamera.Instance.OnMouseWheel;

            Penguin.OnPenguinInstantiated += PenguinInstantiated;
            Penguin.OnConsumeFish += FishConsumed;

            Player.OnBodyTemperatureChange += GUITemperature.Instance.UpdatePlayerTemp;
            Player.OnPlayerDead += GameOver;
            Player.OnPlayerDead += BillBoardingManager.DeactivateAll;
            Player.OnPlayerStart += FollowingCamera.Instance.PlayerActivated;

            TemperatureSystem.OnTemperatureChange += GUITemperature.Instance.UpdateEnvironmentTemp;
        }

        private void RemoveListeners()
        {
            //Fire.OnPlayerInsideFireWarm -= GUITemperature.Instance.PlayerInsideFireWarm;
            //Fire.OnPlayerOutSideFireWarm -= GUITemperature.Instance.PlayerOutsideFireWarm;

            CraftingManager.OnFireCampCreated -= GameManager.CallPenguinToStopFire;

            Fire.OnPlayerInsideFireWarm -= Player.InsideFireWarm;
            Fire.OnPlayerOutSideFireWarm -= Player.OutsideFireWarm;

            Inventory.OnInventoryUpdate -= CraftingManager.Instance.InventoryUpdated;
            Inventory.OnCreateFish -= GameManager.CallPenguinsToFish;

            MouseManager.OnActOnResource -= Player.ActOnResource;
            MouseManager.OnClickOnFloor -= Player.MoveAgent;
            MouseManager.OnClickOnWater -= Player.GoToFishing;
            MouseManager.OnHoverOnResource -= TextHelperManager.TextHover;
            MouseManager.OnMouseWheel -= FollowingCamera.Instance.OnMouseWheel;

            Penguin.OnPenguinInstantiated -= PenguinInstantiated;
            Penguin.OnConsumeFish -= FishConsumed;

            Player.OnBodyTemperatureChange -= GUITemperature.Instance.UpdatePlayerTemp;
            Player.OnPlayerDead -= GameOver;
            Player.OnPlayerDead -= BillBoardingManager.DeactivateAll;
            Player.OnPlayerStart -= FollowingCamera.Instance.PlayerActivated;

            TemperatureSystem.OnTemperatureChange -= GUITemperature.Instance.UpdateEnvironmentTemp;
        }

        private void FishConsumed(Fish fish)
        {
            foreach (var pt in allPenguins)
            {
                Penguin pg = pt.GetComponent<Penguin>();
                if (pg.fishToGet == fish)
                    pg.Wandering();
            }
        }

        private static void CallPenguinsToFish(Fish fish)
        {
            foreach (var pt in allPenguins)
            {
                Penguin pg = pt.GetComponent<Penguin>();
                pg.GoGetFish(fish);
            }
        }


        private static void CallPenguinToStopFire(Fire fire)
        {
            Debug.Log("CallPenguinToPutOutFire " + allPenguins.Count);

            float minDistance = float.PositiveInfinity;
            Penguin nearestPenguin = null;
            Vector3 firePos = fire.transform.position;
            foreach (var pt in allPenguins)
            {
                float d = Vector3.Distance(pt.position, firePos);
                if (d < minDistance)
                {
                    Penguin pg = pt.GetComponent<Penguin>();
                    if (pg.penguinState == Penguin.PENGUIN_STATE.WANDERING)
                    {
                        minDistance = d;
                        nearestPenguin = pg;
                    }
                }
            }

            if (nearestPenguin != null)
            {
                Debug.Log("CallPenguinToPutOutFire FIREFIGHTER === " + nearestPenguin.name);
                nearestPenguin.StopFire(fire);
            }
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        private void GameOver()
        {
            GameState = GAME_STATE.GAME_OVER;
            //SceneManager.LoadScene("INTRO");
            GameOverCanvas.gameObject.SetActive(true);
        }

        public void OnBackToMenu()
        {
            SceneManager.LoadScene("INTRO");
        }

        private void Update()
        {
            TemperatureSystem.Instance.Update(Time.deltaTime);
        }
    }

}
