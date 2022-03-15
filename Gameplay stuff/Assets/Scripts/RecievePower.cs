using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecievePower : MonoBehaviour
{
    public GameObject player;
    public GameObject timer;
    private PlayerController pc;
    private SpeedCountDown s_count_down;
    private JumpCountDown j_count_down;


    private void Awake()
    {
        pc = player.GetComponent<PlayerController>();
        s_count_down = timer.GetComponent<SpeedCountDown>();
        j_count_down = timer.GetComponent<JumpCountDown>();
    }


    private void OnTriggerEnter(Collider other)
    {
        var power_up = other.GetComponent<PowerUp>();

        if(power_up.power == POWER_UP.SpeedBoost)
        {
            var boost = power_up.speed_boost;
            var time = power_up.boost_time;

            if(!pc.speed_boost_active)
            {
                pc.addSpeedPowerUp(boost, time);
                s_count_down.setTimer(time);
            }
            

        }
        else if(power_up.power == POWER_UP.DoubleJump)
        {

            var jump_time = power_up.boost_time;

            if(!pc.double_jump_active)
            {
                pc.setDoubleJump(jump_time);
                j_count_down.setTimer(jump_time);
            }
        }

    }
}
