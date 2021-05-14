using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enviroment : MonoBehaviour
{
    public GameObject creature;

    [Range(0, 1000)]
    public float SpawnRange = 50;

    public Creature[] creatures;
    public Food[] allFood;

    // Start is called before the first frame update
    void Start()
    {
        creatures = FindObjectsOfType<Creature>();

        foreach (var cret in creatures)
        {
            cret.OnDayStart();
        }
    }

    // Update is called once per frame
    void Update()
    {
        creatures = FindObjectsOfType<Creature>();

        allFood = FindObjectsOfType<Food>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(transform.position, new Vector3(SpawnRange, 10, SpawnRange));
    }
}
