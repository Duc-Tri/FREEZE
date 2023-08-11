using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IHateWinter
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameData gameData;

        TreeManager treeManager;

        [SerializeField][Range(0, 5000)] private float maxXZ = 100;

        public static GAME_MODE GameMode { get; private set; }
        public static Player2_5 player;
        public static GameManager Instance { get; private set; }
        public static GameData GameData { get { return Instance.gameData; } }

        private void Awake()
        {
            Instance = this;
            treeManager = new TreeManager();
            if (player == null) player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player2_5>();

            foreach (var item in gameData.itemsToInstanciate)
                InstantiateResource(item.prefab, item.count, item.namePrefix);

            TemperatureSystem.Instance.Init(gameData.temperatureTimes);

            DependanceInjection();
        }

        private void InstantiateResource(Transform resourcePrefab, int numberResource, string nameTemplate)
        {
            for (int i = 0; i < numberResource; i++)
            {
                Transform r = Instantiate(resourcePrefab, this.transform);
                r.position = new Vector3(2f * Random.value * maxXZ - maxXZ, 0.5f, 2f * Random.value * maxXZ - maxXZ);
                r.name = nameTemplate + i;
            }
        }

        void Start()
        {
            GameMode = GAME_MODE.IN_GAME; // TODO : change it when we have menu !
        }

        void DependanceInjection()
        {
            MouseManager.OnHoverOnResource -= TextHelperManager.TextHover;
            MouseManager.OnHoverOnResource += TextHelperManager.TextHover;

            MouseManager.OnClickOnFloor -= player.MoveAgent;
            MouseManager.OnClickOnFloor += player.MoveAgent;

            MouseManager.OnActOnResource -= player.ActOnResource;
            MouseManager.OnActOnResource += player.ActOnResource;

            Player2_5.OnPlayerDead -= GameOver;
            Player2_5.OnPlayerDead += GameOver;
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