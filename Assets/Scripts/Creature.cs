using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public int Generation = 1;

    public Genes genes;

    bool dayGoing = false;

    int food = 0;

    Transform target;

    Vector3 targetPos = new Vector3(0, 0.24f, 0);

    Vector3 homePos;

    Enviroment enviroment;

    // Start is called before the first frame update
    void Start()
    {
        enviroment = FindObjectOfType<Enviroment>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, targetPos);

        if (target != null)
        {
            targetPos = target.position;
        }
        if (dayGoing)
        {
            if (food >= 2)
            {
                targetPos = homePos;
            }
            Move(targetPos);

            if (distance <= 1 && target != null)
            {
                target.GetComponent<Food>().Eat();

                target = null;

                food++;
            }
            if (distance <= 1 && target == null && food < 2)
            {
                GetRandomPos();
            }

            float currentFoodDist = 100;

            foreach (var food in enviroment.allFood)
            {
                float dist = Vector3.Distance(transform.position, food.transform.position);

                if (dist <= genes.visionRaudis && dist < currentFoodDist)
                {
                    currentFoodDist = dist;
                    target = food.transform;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, genes.visionRaudis);
    }

    public void OnDayStart()
    {
        dayGoing = true;
        homePos = transform.position;
    }
    public void OnDayEnd()
    {
        dayGoing = false;
    }

    void Move(Vector3 pos)
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(pos.x, 0.24f, pos.z), genes.speed * Time.deltaTime);
        transform.LookAt(new Vector3(pos.x, 0.24f, pos.z));
    }

    void GetRandomPos()
    {

        float randomZ = UnityEngine.Random.Range(-enviroment.SpawnRange, enviroment.SpawnRange);
        float randomX = UnityEngine.Random.Range(-enviroment.SpawnRange, enviroment.SpawnRange);

        targetPos = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        float distance = Vector3.Distance(Vector3.zero, targetPos);

        while (distance > enviroment.SpawnRange)
        {
            Debug.Log(distance);
            randomZ = UnityEngine.Random.Range(-enviroment.SpawnRange, enviroment.SpawnRange);
            randomX = UnityEngine.Random.Range(-enviroment.SpawnRange, enviroment.SpawnRange);
            targetPos = new Vector3(randomX, transform.position.y, randomZ);
            distance = Vector3.Distance(Vector3.zero, targetPos);
        }
    }
}
[System.Serializable]
public class Genes
{
    public float visionRaudis = 4;

    public float speed = 3;
}
