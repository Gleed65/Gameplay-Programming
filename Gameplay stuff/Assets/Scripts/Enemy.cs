using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public GameObject telegraph;
    PlayerController pc;
    NavMeshAgent agent;
    GameObject target;
    FieldOfView fov;
    Rigidbody enemy_rb;
    Material enemy_mat;

    bool can_damage = false;

    int enemy_health = 10;
    int damage = 10;
    float detect_radius = 20;
    float time_between_attacks = 3;
    float current_attack_time = 0;

    ENEMY_STATES enemy_state;

    enum ENEMY_STATES
    {
        PATROLLING,
        CHASING,
        ATTACKING
    }

    void Start()
    {
        enemy_mat = GetComponent<Renderer>().material;
        fov = GetComponent<FieldOfView>();
        agent = GetComponent<NavMeshAgent>();
        enemy_rb = GetComponent<Rigidbody>();
        target = GameObject.Find("Player");
        pc = target.GetComponent<PlayerController>();
       
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log(enemy_health);

        float distance_to_player = Vector3.Distance(target.transform.position, transform.position);

        if(fov.player_in_view || distance_to_player <= detect_radius)
        {
            enemy_state = ENEMY_STATES.CHASING;
            
        }


        if (enemy_state == ENEMY_STATES.CHASING)
        {
            agent.SetDestination(target.transform.position);

            if(distance_to_player <= agent.stoppingDistance)
            {
                faceTarget();

                enemy_state = ENEMY_STATES.ATTACKING;
            }
        }


        if(enemy_state == ENEMY_STATES.ATTACKING)
        {
            current_attack_time += Time.deltaTime;

            if(current_attack_time > 2.5 && current_attack_time < 3)
            {
                telegraph.GetComponent<MeshRenderer>().enabled = true;
            }
            else
            {
                telegraph.GetComponent<MeshRenderer>().enabled = false;
            }

            if (current_attack_time >= time_between_attacks)
            {
                attack();
                current_attack_time = 0;
            }
        }
        else
        {
            current_attack_time = 0;
        }

        if(enemy_health <= 0)
        {
            Destroy(gameObject);
        }

    }

    void faceTarget()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Quaternion look_rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, look_rotation, Time.deltaTime * 5f);
    }

    void attack()
    {
        enemy_rb.AddForce(transform.forward * 25, ForceMode.Impulse);
        can_damage = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(enemy_state == ENEMY_STATES.ATTACKING)
        {
            if(collision.gameObject.name == "Player")
            {
                Debug.Log("enemy hit");

                if(can_damage)
                pc.takeDamage(damage);

                Invoke("setCantDamage", 0.1f);
            }
        }
    }

    void setCantDamage()
    {
        can_damage = false;
    }

    public void takeDamage(int damage_to_take)
    {
        enemy_health -= damage_to_take;
    }
}
