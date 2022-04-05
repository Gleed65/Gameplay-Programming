using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent agent;
    Transform target;
    FieldOfView fov;

    void Start()
    {
        fov = GetComponent<FieldOfView>();

        target = fov.player.transform;
       
    }

    // Update is called once per frame
    void Update()
    {
        if(fov.player_in_view)
        {
            agent.SetDestination(target.position);
        }
        
    }

}
