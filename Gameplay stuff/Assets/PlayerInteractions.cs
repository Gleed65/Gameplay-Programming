using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractions : MonoBehaviour
{

    public bool can_interact = false;
    public Text interact_text;
    private Switch door_switch;
    public GameObject switch_obj;

    private void Awake()
    {
        door_switch = switch_obj.GetComponent<Switch>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "switch trigger" && !door_switch.switch_used)
        {
            can_interact = true;
            interact_text.text = "Press left attack to interact";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "switch trigger")
        {
            can_interact = false;
            interact_text.text = null;
        }
    }
}
