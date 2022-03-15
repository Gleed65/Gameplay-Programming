using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpCountDown : MonoBehaviour
{
    // Start is called before the first frame update
    private float starting_time;
    private float current_time;

    public Text timer_text;

    // Update is called once per frame
    void Update()
    {

        timer_text.text = "Double jump: " + current_time.ToString("0");

        if (current_time > 0)
        {
            current_time -= 1 * Time.deltaTime;
        }
        else
        {
            timer_text.text = null;
        }    
        
    }

    public void setTimer(float time)
    {
        starting_time = time;

        current_time += starting_time;

    }

}
