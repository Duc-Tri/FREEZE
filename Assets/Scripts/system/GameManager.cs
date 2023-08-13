using System.Collections;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IHateWinter
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameData gameData;

        TreeManager treeManager;

        //[SerializeField][Range(0, 5000)] 
        public float halfObjectsMaxX, halfObjectsMaxZ;

        public static Player Player;
        public static GAME_MODE GameMode { get; private set; }
        public static GameManager Instance { get; private set; }
        public static GameData GameData { get { return Instance.gameData; } }

        private void OnEnable()
        {
            Instance = this;
            if (Player == null) Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            Player.gameObject.SetActive(false);
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

            GameMode = GAME_MODE.IN_GAME; // TODO : change it when we have menu !

            yield return new WaitForSeconds(1.5f);

            Player.gameObject.SetActive(true);
        }

        private void InstantiateResource(Transform resourcePrefab, int numberResource, string nameTemplate)
        {
            for (int i = 0; i < numberResource; i++)
            {
                Transform r = Instantiate(resourcePrefab, this.transform);
                r.position = new Vector3(Random.Range(-halfObjectsMaxX, halfObjectsMaxX), 0, Random.Range(-halfObjectsMaxZ, halfObjectsMaxZ));
                r.name = nameTemplate + i;
            }
        }


        void AddListeners()
        {
            //RemoveListeners();

            TemperatureSystem.OnTemperatureChange += GUITemperature.Instance.UpdateEnvironmentTemp;

            MouseManager.OnHoverOnResource += TextHelperManager.TextHover;
            MouseManager.OnClickOnFloor += Player.MoveAgent;
            MouseManager.OnActOnResource += Player.ActOnResource;
            MouseManager.OnMouseWheel += FollowingCamera.Instance.OnMouseWheel;

            Player.OnPlayerDead += GameOver;
            Player.OnPlayerStart += FollowingCamera.Instance.PlayerActivated;
            Player.OnBodyTemperatureChange += GUITemperature.Instance.UpdatePlayerTemp;

            Fire.OnPlayerInsideFireWarm += Player.InsideFireWarm;
            Fire.OnPlayerOutSideFireWarm += Player.OutsideFireWarm;
            //Fire.OnPlayerInsideFireWarm += GUITemperature.Instance.PlayerInsideFireWarm;
            //Fire.OnPlayerOutSideFireWarm += GUITemperature.Instance.PlayerOutsideFireWarm;
        }

        private void RemoveListeners()
        {
            TemperatureSystem.OnTemperatureChange -= GUITemperature.Instance.UpdateEnvironmentTemp;

            MouseManager.OnHoverOnResource -= TextHelperManager.TextHover;
            MouseManager.OnClickOnFloor -= Player.MoveAgent;
            MouseManager.OnActOnResource -= Player.ActOnResource;
            MouseManager.OnMouseWheel -= FollowingCamera.Instance.OnMouseWheel;

            Player.OnPlayerDead -= GameOver;
            Player.OnPlayerStart -= FollowingCamera.Instance.PlayerActivated;
            Player.OnBodyTemperatureChange -= GUITemperature.Instance.UpdatePlayerTemp;

            Fire.OnPlayerInsideFireWarm -= Player.InsideFireWarm;
            Fire.OnPlayerOutSideFireWarm -= Player.OutsideFireWarm;

            //Fire.OnPlayerInsideFireWarm -= GUITemperature.Instance.PlayerInsideFireWarm;
            //Fire.OnPlayerOutSideFireWarm -= GUITemperature.Instance.PlayerOutsideFireWarm;
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        private void GameOver()
        {
            SceneManager.LoadScene("INTRO");
        }

        private void Update()
        {
            TemperatureSystem.Instance.Update(Time.deltaTime);
        }
    }

}