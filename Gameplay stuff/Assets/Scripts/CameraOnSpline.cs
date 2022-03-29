using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using PathCreation;

public class CameraOnSpline : MonoBehaviour
{
    public GameObject cam_lerp_pos;
    public CinemachineFreeLook free_look_cam;
    public PathCreator cam_path_creator;
    float distance_traveled;
    bool cam_on_spline = false;
    float current_speed;

    // Update is called once per frame
    void Update()
    {
        var vel = GetComponent<Rigidbody>().velocity;

        if(vel.x > 0 || vel.z > 0)
        {
            current_speed = vel.magnitude;
        }
        

        if (cam_on_spline)
        {
            distance_traveled += current_speed * Time.deltaTime;

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
            cam_on_spline = false;
            attachCameraToPlayer();
        }
        if (other.name == "SplineTriggerAdjust")
        {
            current_speed += 20;
        }
    }


    void attachCameraToSpline()
    {
        Vector3 new_pos = cam_lerp_pos.transform.position;

        free_look_cam.Follow = null;

        StartCoroutine(lerpCamera(new_pos, 2));
    }

    void attachCameraToPlayer()
    {
        free_look_cam.Follow = this.transform;
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

        if(time >= duration)
        {
            cam_on_spline = true;
        }

    }
}
