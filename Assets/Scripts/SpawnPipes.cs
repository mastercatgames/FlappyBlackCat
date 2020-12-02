using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPipes : MonoBehaviour
{
    public GameObject vacuum_EasyMode;
    public GameObject vacuum_HardMode;
    public float maxHeight;
    public float minHeight;
    public float repeatRate;

    void SpawnVacuum_Easy()
    {
        Spawn(Instantiate(vacuum_EasyMode));
    }

    void SpawnVacuum_Hard()
    {
        Spawn(Instantiate(vacuum_HardMode));
    }

    private void Spawn(GameObject vacuumPrefab)
    {
        vacuumPrefab.transform.position = new Vector3(transform.position.x, Random.Range(minHeight, maxHeight), 0);
        Destroy(vacuumPrefab, 5f);
    }
}
