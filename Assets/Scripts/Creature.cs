using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public int Generation = 1;

    public Genes genes;

    [HideInInspector]
    public bool dayGoing = false;

    int food = 0;

    public float energy = 2;

    Transform target;

    Vector3 targetPos = new Vector3(0, 0.24f, 0);

    [HideInInspector]
    public Vector3 homePos;

    Quaternion homeRot;

    Enviroment enviroment;

    // Start is called before the first frame update
    void Start()
    {
        enviroment = FindObjectOfType<Enviroment>();
    }

    // Update is called once per frame
    void Update()
    {
        if (energy <= 0)
        {
            Destroy(gameObject);
        }
        if (transform.position == homePos)
        {
            transform.rotation = homeRot;
        }

        float distance = Vector3.Distance(transform.position, targetPos);

        float distToCenter = Vector3.Distance(transform.position, Vector3.zero);

        if (distToCenter > enviroment.SpawnRange)
        {
            GetRandomPos();
        }

        if (target != null)
        {
            targetPos = target.position;
        }
        if (dayGoing)
        {
            if (food >= 2 || food == 1 && energy <= 1)
            {
                targetPos = homePos;
            }
            Move(targetPos);

            if (distance <= 1 && target != null)
            {
                target.GetComponent<Food>().Eat();

                target = null;

                food++;
                energy += 0.2f;
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
        else
        {
            if (food >= 2)
            {
                Reperduse();
            }
            food = 0;
        }
    }

    void Reperduse()
    {
        var baby = Instantiate(gameObject, transform.position + (transform.right * 2), transform.rotation);

        baby.GetComponent<Creature>().Generation++;
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
        homeRot = transform.rotation;
    }
    public void OnDayEnd()
    {
        dayGoing = false;
    }

    void Move(Vector3 pos)
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(pos.x, 0.24f, pos.z), genes.speed * Time.deltaTime);
        transform.LookAt(new Vector3(pos.x, 0.24f, pos.z));
        if (transform.position != homePos)
        {
            energy -= 0.001f * (genes.speed / 3);
        }
    }

    void GetRandomPos()
    {

        float randomZ = UnityEngine.Random.Range(-enviroment.SpawnRange, enviroment.SpawnRange);
        float randomX = UnityEngine.Random.Range(-enviroment.SpawnRange, enviroment.SpawnRange);

        targetPos = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (!Physics.Raycast(targetPos, -Vector3.up))
        {
            GetRandomPos();
        }
    }
}
[System.Serializable]
public class Genes
{
    public float visionRaudis = 4;

    public float speed = 3;
}
