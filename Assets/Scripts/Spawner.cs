using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject Food;
    private GameObject[] _spawns;
    private Vector3 _spawnpoint;

    void Start()
    {
        _spawns = GameObject.FindGameObjectsWithTag("Spawn");
        InvokeRepeating("Spawn", 0.5f, 5f);
    }

    void Spawn()
    {
        int rdIndex = Random.Range(0, _spawns.Length);
        _spawnpoint = _spawns[rdIndex].transform.position;

        GameObject foodInstance = Food;
        Instantiate(foodInstance, _spawnpoint, Quaternion.identity);
    }
}
