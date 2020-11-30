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

    void SpawnVacuum_EasyMode()
    {
        GameObject newVacuum = Instantiate(vacuum_EasyMode);
        newVacuum.transform.position = new Vector3(transform.position.x, Random.Range(minHeight, maxHeight), 0);
        Destroy(newVacuum, 5f);
    }

     void SpawnVacuum_HardMode()
    {
        GameObject newVacuum = Instantiate(vacuum_HardMode);
        newVacuum.transform.position = new Vector3(transform.position.x, Random.Range(minHeight, maxHeight), 0);
        Destroy(newVacuum, 5f);
    }
}
