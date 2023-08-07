using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class SnowflakesManager : MonoBehaviour
{
    [SerializeField]
    private GameObject snowflakePrefab;

    [SerializeField]
    [Range(1, 100)]
    private int numberSnowflakes = 2;

    [SerializeField]
    public float maxXYRandom = 50;

    public static SnowflakesManager Instance;
    public int Score;



    private void Awake()
    {
        Instance = this;
        Score = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberSnowflakes; i++)
        {
            Vector3 pos = new Vector3(2f * Random.value * maxXYRandom - maxXYRandom, 1, 2f * Random.value * maxXYRandom - maxXYRandom);
            Transform t = Instantiate(snowflakePrefab).transform;
            t.name = $"SNOWFLAKE-{i:00}";
            t.position = pos;
        }
    }


    void Update()
    {

    }

    public void AddScore(int score)
    {
        Score += score;
    }
}
