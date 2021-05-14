using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enviroment : MonoBehaviour
{
    public GameObject creature;

    public Creature[] creatures;
    public Food[] allFood;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        creatures = FindObjectsOfType<Creature>();

        allFood = FindObjectsOfType<Food>();
    }
}
