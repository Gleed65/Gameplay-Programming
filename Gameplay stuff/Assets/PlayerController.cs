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
    private Vector3 player_dir = Vector3.zero;
    private Vector3 look_dir;
    private Animator player_animator;
    private bool grounded;
    private bool can_jump = true;
    private bool can_roll = true;
    float roll_force = 15;

    [SerializeField]

    private float default_player_speed = 5;
    private float player_speed;
    

    [SerializeField]
    private float max_speed = 20;

    [SerializeField]
    private float jump_force = 70;


    private void Awake()
    {
        player_input = new PlayerInputAsset();
        player_animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        player_speed = default_player_speed;

        Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = false;
    }

      private void OnEnable()
    {
        player_input.Enable();
        move = player_input.Player.Movement;
        player_input.Player.Jump.started += jump;
        player_input.Player.roll.started += roll;
        player_input.Player.leftattack.started += leftAttack;
        player_input.Player.rightattack.started += rightAttack;
    }

    private void OnDisable()
    {
        player_input.Player.Jump.started -= jump;
        player_input.Player.roll.started -= roll;
        player_input.Player.leftattack.started -= leftAttack;
        player_input.Player.rightattack.started -= rightAttack;
        player_input.Disable();
    }

    private void roll(InputAction.CallbackContext ctx)
    {
        if(can_roll)
        {
            player_speed += roll_force;
            player_animator.SetTrigger("roll");
        }
        can_roll = false;
        Invoke("setCanRoll", 1.2f);
    }

    private void jump(InputAction.CallbackContext ctx)
    {
        if(isGrounded() && can_jump)
        {
            player_animator.SetBool("jumping", true);
            can_jump = false;
            player_dir += Vector3.up * jump_force;
        }
    }
    private void leftAttack(InputAction.CallbackContext ctx)
    {
        player_animator.SetTrigger("left attack");
    }
    private void rightAttack(InputAction.CallbackContext ctx)
    {
        player_animator.SetTrigger("right attack");
    }
    private bool isGrounded()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down)
;
        if(Physics.Raycast(ray, out RaycastHit hit, 0.5f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void Update()
    {

        if(!can_jump)
        {
            Invoke("setCanJump", 1);
        }

        grounded = isGrounded();
        player_animator.SetFloat("speed", rb.velocity.magnitude / max_speed);

        if(grounded)
        {
            player_animator.SetBool("grounded", true);
            player_animator.SetBool("falling", false);
        }
        else
        {
            player_animator.SetBool("grounded", false);
        }

    }

    private void FixedUpdate()
    {
        player_dir += move.ReadValue<Vector2>().x * GetCameraRight(cam) * player_speed;
        player_dir += move.ReadValue<Vector2>().y * GetCameraForward(cam) * player_speed;

        rb.AddForce(player_dir, ForceMode.Impulse);

        player_dir = Vector3.zero;

      
        if (rb.velocity.y < 0f)
        {
            player_animator.SetBool("jumping", false);
            player_animator.SetBool("falling", true);
            rb.velocity -= Vector3.down * Physics.gravity.y * 8 * Time.fixedDeltaTime;
        }

        Vector3 h_velocity = rb.velocity;
        h_velocity.y = 0;


        if (h_velocity.sqrMagnitude > max_speed * max_speed)
        {
            rb.velocity = h_velocity.normalized * max_speed + Vector3.up * rb.velocity.y;
        }
        if (h_velocity == Vector3.zero)
        {
            can_roll = false;
        }
        else
        {
            can_roll = true;
        }

        lookAt();

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

    private void lookAt()
    {
        look_dir = rb.velocity;

        look_dir.y = 0;

        if(move.ReadValue<Vector2>().sqrMagnitude > 0.1 && look_dir.sqrMagnitude > 0.1f)
        {
            this.rb.rotation = Quaternion.LookRotation(look_dir, Vector3.up);
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
        }
    }
    void setCanJump()
    {
        can_jump = true;
    }
    void setCanRoll()
    {
        can_roll = true;
        player_speed = default_player_speed;
    }
}
