using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollision : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject player;
    PlayerController pc;
    Enemy enemy_script;


    private void Start()
    {
        pc = player.GetComponent<PlayerController>();
        enemy_script = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();

    }

    private void OnCollisionEnter(Collision collision)
    {
        

        if (collision.gameObject.tag == "Enemy")
        {
           
            if (pc.can_damage)
            {
                enemy_script.takeDamage(pc.damage);
                pc.can_damage = false;

                collision.rigidbody.AddForce(-collision.transform.forward * 15, ForceMode.Impulse);

            }    
        }
    }


}
