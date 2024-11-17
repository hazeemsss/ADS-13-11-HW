using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

[RequireComponent(typeof(Collider2D))]
public class Drone : MonoBehaviour
{
    public int DroneId { get; private set; } // Unique ID for each drone
    public int Ammo { get; private set; } // Ammo attribute
    public int WeaponCapacity { get; private set; } // Weapon Capacity attribute
    public int Temperature { get; private set; } = 0;
    private static int idCounter = 0;
    public int ID { get; private set; }
    private Flock agentFlock;
    public Flock AgentFlock { get { return agentFlock; } }

    private Collider2D agentCollider;
    public Collider2D AgentCollider { get { return agentCollider; } }

    private void Start()
    {
        agentCollider = GetComponent<Collider2D>();
        RandomizeAttributes(); // Initialize with random values
        StartCoroutine(RandomizeAttributesPeriodically()); // Randomize at regular intervals
    }

    private void Update()
    {
        Temperature = (int)(Random.value * 100); // Randomly update Temperature
    }

    // Coroutine to randomize attributes periodically
    private IEnumerator RandomizeAttributesPeriodically()
    {
        while (true)
        {
            RandomizeAttributes();
            yield return new WaitForSeconds(2f); // Adjust the interval as needed
        }
    }

    private void RandomizeAttributes()
    {
        Ammo = Random.Range(1, 100); // Random Ammo between 1 and 100
        WeaponCapacity = Random.Range(1, 100); // Random Weapon Capacity between 1 and 100
    }

    public void Initialize(Flock flock)
    {
        Ammo = Random.Range(1, 100); // Random Ammo between 1 and 100
        ID = idCounter++;
        agentFlock = flock;
    }

    public void Move(Vector2 velocity)
    {
        transform.up = velocity;
        transform.position += (Vector3)velocity * Time.deltaTime;
    }

    public void SelfDestruct()
    {
        gameObject.SetActive(false); // Set the drone inactive in the scene
    }
}
