using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;


public class PlayerController : MonoBehaviour
{
    //Using the C# script

    private PlayerInputAsset player_input;
    private PlayerInteractions interactions;
    private Switch door_switch;
    public GameObject switch_obj;
    public GameObject door;
    public GameObject timeline_obj;
    private PlayableDirector door_cutscene;
    public ParticleSystem trail_system;
    public Camera tp_cam;
    public Camera cs_cam;
    private InputAction move;
    private Rigidbody rb;
    private Vector3 player_dir = Vector3.zero;
    private Vector3 look_dir;
    private Animator player_animator;
    private Animator door_animator;
    private bool grounded;
    private bool can_jump = true;
    private bool can_roll = true;
    float roll_force = 10;
    private float default_player_speed = 8;
    private float player_speed;
    private float max_speed = 20;
    private float jump_force = 120;
    private float jump_counter = 0;
    [SerializeField]
    public bool double_jump_active = false;
    public bool speed_boost_active = false;
    private float rod_timer = 0;


    private void Awake()
    {
        player_input = new PlayerInputAsset();
        player_animator = GetComponent<Animator>();
        interactions = GetComponent<PlayerInteractions>();
        door_switch = switch_obj.GetComponent<Switch>();
        rb = GetComponent<Rigidbody>();
        player_speed = default_player_speed;
        door_animator = door.GetComponent<Animator>();
        door_cutscene = timeline_obj.GetComponent<PlayableDirector>();

        cs_cam.enabled = false;

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
        if (can_roll && isGrounded())
        {
            player_speed += roll_force;
            player_animator.SetTrigger("roll");
        }
        can_roll = false;
        Invoke("setCanRoll", 1.2f);

        if(!speed_boost_active)
        {
            Invoke("setDefaultSpeed", 0.6f);
        }
        
    }

    private void jump(InputAction.CallbackContext ctx)
    {
        if(can_jump)
        {
            if(!double_jump_active && isGrounded())
            {
                player_animator.SetBool("jumping", true);
                can_jump = false;
                player_dir += Vector3.up * jump_force; 
                Invoke("setCanJump", 1.2f);
            }
            if(double_jump_active)
            {
                player_animator.SetBool("jumping", true);
                player_dir += Vector3.up * jump_force;
                jump_counter++;
                can_jump = false;
            }
        }

      
    }
    private void leftAttack(InputAction.CallbackContext ctx)
    {
        if(interactions.can_interact && !door_switch.switch_used)
        {
            door_cutscene.Play();
            tp_cam.enabled = false;
            door_switch.switch_used = true;
            interactions.interact_text.text = null;

            cs_cam.enabled = true;

            float duration = (float)door_cutscene.duration;

            Invoke("setCamActive", duration);
        }


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
        if (Physics.Raycast(ray, out RaycastHit hit, 0.5f))
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
        if (isGrounded())
        {
            jump_counter = 0;
            Invoke("setCanJump", 0.2f);
        }

        if (jump_counter >= 1)
        {
            can_jump = false;
            jump_counter = 0;
        }
        grounded = isGrounded();
        player_animator.SetFloat("speed", rb.velocity.magnitude / max_speed);

        if (grounded)
        {
            player_animator.SetBool("grounded", true);
            player_animator.SetBool("falling", false);
        }
        else
        {
            player_animator.SetBool("grounded", false);
        }

        if(rod_timer > 0)
        {
            rod_timer -= 1 * Time.deltaTime;
        }

        var rod = trail_system.emission;
        rod.rateOverDistance = rod_timer;

    }

    private void FixedUpdate()
    {
        player_dir += move.ReadValue<Vector2>().x * GetCameraRight(tp_cam) * player_speed;
        player_dir += move.ReadValue<Vector2>().y * GetCameraForward(tp_cam) * player_speed;

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

        if (move.ReadValue<Vector2>().sqrMagnitude > 0.1 && look_dir.sqrMagnitude > 0.1f)
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

    }
    void setNormalJump()
    {
        double_jump_active = false;
    }
    void setDefaultSpeed()
    {
        player_speed = default_player_speed;
        speed_boost_active = false;
    }
    void setCamActive()
    {

        tp_cam.enabled = true;
        cs_cam.enabled = false;
    }

    public void addSpeedPowerUp(float speed_boost, float time)
    {
        rod_timer = time;

        if(!speed_boost_active)
        {
            speed_boost_active = true;
            player_speed += speed_boost;
            Invoke("setDefaultSpeed", time);
        }
        
    }
    public void setDoubleJump(float time)
    {
        if(!double_jump_active)
        {
            double_jump_active = true;
            Invoke("setNormalJump", time);
        }
        
    }
}
