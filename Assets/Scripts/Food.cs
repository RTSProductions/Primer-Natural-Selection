using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public void Eat()
    {
        Debug.Log(transform.name + " was eaten");

        Destroy(gameObject);
    }
}
