using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IHateWinter
{


    public class GameManager : MonoBehaviour
    {
        TreeManager treeManager;

        [SerializeField] private Transform treePrefab;

        [SerializeField][Range(0, 200000)] private int numberTrees = 1;

        [SerializeField] private Transform stonePrefab;

        [SerializeField][Range(0, 200000)] private int numberStones = 1;

        [SerializeField] private Transform flintPrefab;

        [SerializeField][Range(0, 200000)] private int numberFlints = 1;


        [SerializeField][Range(0, 5000)] private float maxXZ = 100;

        public static GAME_MODE GameMode { get; private set; }
        private static Player2_5 player;

        private void Awake()
        {
            treeManager = new TreeManager();
            if (player == null) player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player2_5>();

            InstantiateResource(flintPrefab, numberFlints, "@flint-");
            InstantiateResource(stonePrefab, numberStones, "@stone-");
            InstantiateResource(treePrefab, numberTrees, "@tree-");

            Injection();
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

        void Injection()
        {
            MouseManager.OnHoverOnResource -= TextHelperManager.TextHover;
            MouseManager.OnHoverOnResource += TextHelperManager.TextHover;

            MouseManager.OnClickOnFloor -= player.MoveAgent;
            MouseManager.OnClickOnFloor += player.MoveAgent;

            MouseManager.OnActOnResource -= player.ActOnResource;
            MouseManager.OnActOnResource += player.ActOnResource;
        }

    }

}