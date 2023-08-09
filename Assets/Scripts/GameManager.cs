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

        [SerializeField][Range(0, 1000)] private float maxXZ = 100;

        private void Awake()
        {
            treeManager = new TreeManager();

            for (int i = 0; i < numberTrees; i++)
            {
                Transform tree = Instantiate(treePrefab, this.transform);
                tree.position = new Vector3(2f * Random.value * maxXZ - maxXZ, 1.5f, 2f * Random.value * maxXZ - maxXZ);
                tree.name = "@tree-" + i;
            }
            for (int i = 0; i < numberStones; i++)
            {
                Transform stone = Instantiate(stonePrefab, this.transform);
                stone.position = new Vector3(2f * Random.value * maxXZ - maxXZ, 0.5f, 2f * Random.value * maxXZ - maxXZ);
                stone.name = "@stone-" + i;
            }
        }

        void Start()
        {

        }

        void Update()
        {

        }
    }

}