using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using PathCreation;

public class CameraOnSpline : MonoBehaviour
{
    public GameObject cam_lerp_pos;
    public GameObject lerp_to_player;
    public CinemachineFreeLook free_look_cam;
    public PathCreator cam_path_creator;
    float distance_traveled;
    bool cam_on_spline = false;
    float current_speed;

    // Update is called once per frame
    void Update()
    {
        print(cam_on_spline);

        var vel = GetComponent<Rigidbody>().velocity;

        if(vel.x > 0 || vel.z > 0)
        {
            current_speed = vel.magnitude;
        }
        

        if (cam_on_spline)
        {
            distance_traveled += (current_speed * 0.9f) * Time.deltaTime;

            free_look_cam.transform.position = cam_path_creator.path.GetPointAtDistance(distance_traveled);
        }
      
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "SplineTriggerOn")
        {
            distance_traveled = 0;
            attachCameraToSpline();
        }
        if (other.name == "SplineTriggerOff")
        {

            attachCameraToPlayer();
        }
    
    }


    void attachCameraToSpline()
    {
        Vector3 new_pos = cam_lerp_pos.transform.position;
        free_look_cam.Follow = null;

        StartCoroutine(lerpCamera( new_pos, 2));
    }

    void attachCameraToPlayer()
    {
        Vector3 new_pos = lerp_to_player.transform.position;

        if(cam_on_spline)
        {
            StartCoroutine(lerpCamera(new_pos, 2));
        }
     
    }

    IEnumerator lerpCamera(Vector3 targetPosition, float duration)
    {

        float time = 0;
        Vector3 startPosition = free_look_cam.transform.position;
        while (time < duration)
        {
            free_look_cam.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        if (time >= duration)
        {

            if (cam_on_spline)
            {
                cam_on_spline = false;
                
                free_look_cam.Follow = this.transform;

            } else if (!cam_on_spline)
            {
                cam_on_spline = true;
            }

        }

    }
}
