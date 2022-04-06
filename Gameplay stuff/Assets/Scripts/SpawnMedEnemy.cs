using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMedEnemy : MonoBehaviour
{

    public GameObject spawn_object;

    public void spawn()
    {
        Vector3 position = transform.position;


        Instantiate(spawn_object, position, Quaternion.identity);
        Instantiate(spawn_object, position, Quaternion.identity);
    }
   
}
