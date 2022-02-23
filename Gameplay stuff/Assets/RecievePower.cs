using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecievePower : MonoBehaviour
{
    public GameObject player;
    private PlayerController pc;


    private void Awake()
    {
        pc = player.GetComponent<PlayerController>();
    }


    private void OnTriggerEnter(Collider other)
    {
        var power_up = other.GetComponent<PowerUp>();

        if(power_up.power == POWER_UP.SpeedBoost)
        {
            var boost = power_up.speed_boost;
            var time = power_up.boost_time;

            pc.addSpeedPowerUp(boost, time);

        }
        else if(power_up.power == POWER_UP.DoubleJump)
        {

            var jump_time = power_up.boost_time;

            pc.setDoubleJump(jump_time);
        }

    }
}
