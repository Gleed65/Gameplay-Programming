using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{

    private Vector3 pos_1;
    private Vector3 pos_2;

    public float distance_to_travel = 0;
    public bool one_axis_movement = true;
    public bool circular_movement = false;
    public bool rotate = false;
    public float speed;
    
    public float x_travel = 0;
    public float y_travel = 0;
    public float z_travel = 0;

    public float rotation_speed = 0;

    public float radius = 0;
    public float angle = 0;

    /*

    public float journey_time;

    

    float start_time = 0;
    Vector3 center_point;
    Vector3 start_rel_center;
    Vector3 end_rel_center;*/


    public AXIS axis_choice;
    public ROTATE_AXIS rot_axis_choice;

    public enum AXIS
    {
        X,
        Y,
        Z
    }

    public enum ROTATE_AXIS
    {
        X,
        Y,
        Z
    }

    private void Start()
    {
        pos_1 = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        if (axis_choice == AXIS.X)
        {
            pos_2 = new Vector3(transform.position.x + distance_to_travel, transform.position.y, transform.position.z);
        }
        if (axis_choice == AXIS.Y)
        {
            pos_2 = new Vector3(transform.position.x, transform.position.y + distance_to_travel, transform.position.z);
        }
        if (axis_choice == AXIS.Z)
        {
            pos_2 = new Vector3(transform.position.x, transform.position.y, transform.position.z + distance_to_travel);
        }

        if(!one_axis_movement)
        {
            pos_2 = new Vector3(transform.position.x + x_travel, transform.position.y + y_travel, transform.position.z + z_travel);
        }
       

    }

    void Update()
    {

        transform.position = Vector3.Lerp(pos_1, pos_2, (float)((Mathf.Sin(speed * Time.time) + 1.0f) / 2.0));

        if(circular_movement)
        {
            angle += speed * Time.deltaTime;
            transform.position = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
        }
        


        if (rotate)
        {
            if(rot_axis_choice == ROTATE_AXIS.X)
            {
                transform.Rotate(6.0f * rotation_speed * Time.deltaTime, 0, 0);
            }
            if (rot_axis_choice == ROTATE_AXIS.Y)
            {
                transform.Rotate(0, 6.0f * rotation_speed * Time.deltaTime, 0);
            }
            if (rot_axis_choice == ROTATE_AXIS.Z)
            {
                transform.Rotate(0, 0, 6.0f * rotation_speed * Time.deltaTime);
            }

        }
        
    }

   
}





/*getCentre(Vector3.up);


float fraction_complete = Mathf.PingPong(Time.time - start_time, journey_time / speed) / journey_time * speed;

transform.position += center_point;*/


/* public void getCentre(Vector3 dir)
    {
        center_point = (pos_1 + pos_2) * 0.5f;
        center_point -= dir;
        start_rel_center = pos_1 - center_point;
        end_rel_center = pos_2 - center_point;
    }*/