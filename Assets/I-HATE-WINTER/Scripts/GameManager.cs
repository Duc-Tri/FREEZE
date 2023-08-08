using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    TreeManager treeManager;

    [SerializeField] private Transform treePrefab;
    [SerializeField]
    [Range(0, 2000)]
    private int numberTrees = 1;
    private float maxXZ = 100;
    private void Awake()
    {

        treeManager = new TreeManager();
        for (int i = 0; i < numberTrees; i++)
        {
            Transform tree = Instantiate(treePrefab);
            tree.position = new Vector3(2f * Random.value * maxXZ - maxXZ, 1.5f, 2f * Random.value * maxXZ - maxXZ);
            tree.name = "TREE-" + i;
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
