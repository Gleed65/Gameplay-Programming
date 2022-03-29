using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class FollowPath : MonoBehaviour
{
    public PathCreator path;
    public float speed;
    float distance_traveled;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distance_traveled += speed * Time.deltaTime;

        transform.position = path.path.GetPointAtDistance(distance_traveled);
        transform.rotation = path.path.GetRotationAtDistance(distance_traveled);
    }
}
