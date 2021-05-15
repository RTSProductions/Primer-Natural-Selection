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

    public float energy = 1;

    public Color normal;

    public Color speed;

    public Color sence;

    public SkinnedMeshRenderer body;

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

        transform.localScale = Vector3.one * genes.scale;
    }

    // Update is called once per frame
    void Update()
    {
        float speedF = genes.speed / 6 + (genes.speed / 6) / 10;
        float senceF = genes.visionRaudis / 4 + (genes.visionRaudis / 4) / 10;
        Material newMat = new Material(Shader.Find("Standard"));
        newMat.CopyPropertiesFromMaterial(body.sharedMaterials[0]);
        Color SpeedColor = Color.Lerp(normal, speed, (speedF / 10) * genes.scale);
        Color SneceColor = Color.Lerp(normal, sence, (senceF / 10) * genes.scale);

        Color FinalColor = Color.Lerp(SneceColor, SpeedColor, 0.5f);

        newMat.color = FinalColor;

        body.material = newMat;

        if (energy <= 0)
        {
            Die(transform.name + " died form hunger");
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
            if (!enviroment.GetPossibleFood(this) && transform.position != homePos)
            {
                if (food <= 0)
                {
                    Die(transform.name + " died form hunger");
                }
                else
                {
                    Move(homePos);
                }
            }

            if (food >= 2 || food == 1 && energy <= 0.5f && transform.position != homePos)
            {
                Move(homePos);
            }
            if (food < 2 && !(food == 1 && energy <= 0.5f))
            {
                Move(targetPos);
            }

            if (distance <= 1 && target != null)
            {
                if (target.name == "Food(Clone)")
                {
                    target.GetComponent<Food>().Eat();

                    target = null;

                    food++;
                }
                else
                {
                    Creature creature = target.GetComponent<Creature>();
                    if (creature.transform.position != creature.homePos)
                    {
                        Die(creature.transform.name + " was eaten by " + transform.name + ", a big boy");

                        target = null;

                        food++;
                    }
                }
            }
            if (distance <= 1 && target == null && food < 2)
            {
                GetRandomPos();
            }

            float currentFoodDist = 100;

            foreach (var food in enviroment.allFood)
            {
                if (food != null)
                {
                    float dist = Vector3.Distance(transform.position, food.transform.position);

                    if (dist <= genes.visionRaudis && dist < currentFoodDist)
                    {
                        currentFoodDist = dist;
                        target = food.transform;
                    }
                }
            }
            currentFoodDist = 100;
            foreach (var Cret in enviroment.creatures)
            {
                if (Cret != null)
                {
                    float dist = Vector3.Distance(transform.position, Cret.transform.position);

                    if (dist <= genes.visionRaudis && dist < currentFoodDist && target == null && transform.localScale.y > (Cret.transform.localScale.y * 1.2f))
                    {
                        if (Cret.transform.position != Cret.homePos)
                        {
                            currentFoodDist = dist;
                            target = Cret.transform;
                        }
                    }
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

        baby.GetComponent<Creature>().genes.InheritGenes(genes);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, genes.visionRaudis);
    }

    public void OnDayStart()
    {
        dayGoing = true;
        homePos = GetHomePos();
        energy = 1;
    }
    public void OnDayEnd()
    {
        dayGoing = false;
    }

    void Move(Vector3 pos)
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(pos.x, 0.24f, pos.z), (genes.speed * Time.deltaTime) / genes.scale);
        transform.LookAt(new Vector3(pos.x, 0.24f, pos.z));
        if (pos != homePos)
        {
            energy -= 0.009f * (((genes.speed / 6) * genes.scale) * Time.deltaTime);
        }
    }

    void GetRandomPos()
    {

        float randomZ = UnityEngine.Random.Range(-enviroment.SpawnRange, enviroment.SpawnRange);
        float randomX = UnityEngine.Random.Range(-enviroment.SpawnRange, enviroment.SpawnRange);

        targetPos = new Vector3(randomX, transform.position.y, randomZ);

        if (!Physics.Raycast(targetPos, -Vector3.up))
        {
            GetRandomPos();
        }
    }
    Vector3 GetHomePos()
    {
        float SpawnRange = enviroment.SpawnRange / 2;
        float max = 2;
        float randomZ = Random.Range(-max, max);
        float randomX = Random.Range(-max, max);
        int Index = Random.Range(0, enviroment.edgeSpots.Length);
        Vector3 oldPos = enviroment.edgeSpots[Index].position;
        homeRot = enviroment.edgeSpots[Index].rotation;

        Vector3 newPos = new Vector3(oldPos.x + randomX, transform.position.y, oldPos.z + randomZ);
        return newPos;
    }
    public void Die(string message)
    {
        Debug.Log(message);
        Destroy(gameObject);
    }
}
[System.Serializable]
public class Genes
{
    public float visionRaudis = 4;

    public float speed = 6;

    public float scale = 1;

    [Range(1, 100)]
    public float mutationPercent = 10f;

    public void InheritGenes(Genes parent)
    {
        visionRaudis = parent.visionRaudis;

        speed = parent.speed;

        scale = parent.scale;

        mutationPercent = parent.mutationPercent;

        mutationPercent = Muate(mutationPercent, 1, 100);

        scale = Muate(scale, 0.5f, 2);

        speed = Muate(speed, 0.5f, 5);

        visionRaudis = Muate(visionRaudis, 2, 15);
    }
    float Muate(float value, float minAmount, float MaxAmount)
    {
        float newValue = value;

        if (Random.value < (mutationPercent / 100))
        {
            float mutateAmount = Random.Range(-MaxAmount, MaxAmount);

            newValue += mutateAmount;
            if (newValue < minAmount)
            {
                newValue = minAmount;
            }
        }

        return newValue;
    }
}
