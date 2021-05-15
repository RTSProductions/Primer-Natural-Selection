using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enviroment : MonoBehaviour
{
    [Range(1, 20)]
    public float timeScale = 1;

    [Range(0, 1000)]
    public float SpawnRange = 50;

    public GameObject food;

    public Creature[] creatures;
    public Food[] allFood;

    // Start is called before the first frame update
    void Start()
    {
        creatures = FindObjectsOfType<Creature>();

        StartGeneration();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != timeScale)
        {
            Time.timeScale = timeScale;
        }

        creatures = FindObjectsOfType<Creature>();

        allFood = FindObjectsOfType<Food>();

        int all = 0;

        foreach (var cret in creatures)
        {
            if (cret.transform.position == cret.homePos && cret.dayGoing == true)
            {
                all++;
            }
        }
        if (all == creatures.Length)
        {
            EndGeneration();
        }
    }

    IEnumerator StartNewGeneration()
    {
        yield return new WaitForSeconds(2);

        StartGeneration();
    }

    void StartGeneration()
    {
        foreach (var cret in creatures)
        {
            cret.OnDayStart();
        }
        foreach (var food in allFood)
        {
            Destroy(food.gameObject);
        }
        int foodAmount = (creatures.Length * 2) - ((creatures.Length * 2) / 4);
        for (int i = 0; i < foodAmount; i++)
        {
            SpawnFood();
        }
    }

    void SpawnFood()
    {
        float newSpawnRange = SpawnRange / 2;
        float randomZ = Random.Range(-newSpawnRange, newSpawnRange);
        float randomX = Random.Range(-newSpawnRange, newSpawnRange);

        Vector3 SpawnPoint = new Vector3(randomX, 0, randomZ);

        Instantiate(food, SpawnPoint, Quaternion.identity);
    }

    void EndGeneration()
    {
        foreach (var cret in creatures)
        {
            cret.OnDayEnd();
        }
        StartCoroutine(StartNewGeneration());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(transform.position, new Vector3(SpawnRange, 10, SpawnRange));
    }
}
