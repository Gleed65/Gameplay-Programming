using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0,360)]
    public float angle;

    public GameObject player;

    public LayerMask target_mask;
    public LayerMask obstruuction_mask;

    public bool player_in_view;


    private void Start()
    {
        player = GameObject.Find("Player");
        StartCoroutine(FOVroutine(0.1f));
    }

    private IEnumerator FOVroutine(float delay)
    {
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;

            FOVcheck();

        }
    }

    private void FOVcheck()
    {
        Collider[] range_check = Physics.OverlapSphere(transform.position, radius, target_mask);

        if (range_check.Length != 0)
        {
            Transform target = range_check[0].transform;
            Vector3 dir_to_target = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dir_to_target) < angle / 2)
            {
                float distance = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dir_to_target, distance, obstruuction_mask))
                {
                    player_in_view = true;
                }
                else
                {
                    player_in_view = false;
                }
            }
            else
            {
                player_in_view = false;
            }    
        }
        else if(player_in_view)
        {
            player_in_view = false;
        }
    }
}

