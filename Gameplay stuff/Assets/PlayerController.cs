using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    //Using the C# script

    private PlayerInputAsset player_input;
    public Camera cam;
    private InputAction move;
    private Rigidbody rb;
    public float player_speed = 10;
    public float max_speed = 20;
    public float jump_force = 10;
    Vector3 player_dir = Vector3.zero;




    private void Awake()
    {
        player_input = new PlayerInputAsset();
        rb = GetComponent<Rigidbody>();

    }

      private void OnEnable()
    {
        player_input.Enable();
        move = player_input.Player.Movement;
        player_input.Player.Jump.performed += ctx => jump();
    }

    private void OnDisable()
    {
        player_input.Player.Jump.performed -= ctx => jump();
        player_input.Disable();
    }

    private void movement(Vector2 dir)
    {
        Debug.Log("we move" + dir);

        Vector3 player_dir = new Vector3(dir.x, 0, dir.y).normalized;
        
    }

    private void jump()
    {
        if(isGrounded())
        {
            Debug.Log("We Jump!!!");

            player_dir += Vector3.up * jump_force;
        }
    }
  
    private bool isGrounded()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down)
;

        if(Physics.Raycast(ray, out RaycastHit hit, 0.3f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void FixedUpdate()
    {
        player_dir += move.ReadValue<Vector2>().x * GetCameraRight(cam) * player_speed;
        player_dir += move.ReadValue<Vector2>().y * GetCameraForward(cam) * player_speed;

        rb.AddForce(player_dir, ForceMode.Impulse);
        player_dir = Vector3.zero;

        if(rb.velocity.y < 0f)
        {
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.deltaTime;
        }

        Vector3 h_velocity = rb.velocity;
        h_velocity.y = 0;

        if (h_velocity.sqrMagnitude > max_speed * max_speed)
        {
            rb.velocity = h_velocity.normalized * max_speed + Vector3.up * rb.velocity.y;
        }

    }

    private Vector3 GetCameraForward(Camera cam)
    {
        Vector3 forward = cam.transform.forward;
        forward.y = 0;

        return forward.normalized;
    }
    private Vector3 GetCameraRight(Camera cam)
    {
        Vector3 right = cam.transform.right;
        right.y = 0;

        return right.normalized;

    }
}
