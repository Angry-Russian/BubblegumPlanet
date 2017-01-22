using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;
using UnityEngine.UI;

public class GravityBody : MonoBehaviour {

    public float speed;
    private Rigidbody rb;
    public GravityAttraction gravityAttraction;
    private Transform myTransform;

    [SerializeField]
    string m_prefixPlayer = "P1_";
    [SerializeField]
    float m_MovingTurnSpeed = 360;
    [SerializeField]
    float m_StationaryTurnSpeed = 180;
    [SerializeField]
    public float m_JumpPower = 12f;
    [Range(1f, 4f)]
    [SerializeField]
    float m_GravityMultiplier = 2f;
    [SerializeField]
    float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
    [SerializeField]
    float m_MoveSpeedMultiplier = 1f;
    [SerializeField]
    float m_AnimSpeedMultiplier = 1f;
    [SerializeField]
    float m_GroundCheckDistance = 0.1f;

    Rigidbody m_Rigidbody;
    Animator m_Animator;
    bool m_IsGrounded;
    float m_OrigGroundCheckDistance;
    const float k_Half = 0.5f;
    float m_TurnAmount;
    float m_ForwardAmount;
    Vector3 m_GroundNormal;
    float m_CapsuleHeight;
    Vector3 m_CapsuleCenter;
    CapsuleCollider m_Capsule;
    bool m_Crouching;

    public GameObject spawn;
    public int deaths = 0;

    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
    private Vector3 gravityUp;
    private bool m_isEjected = false;
    private bool canMakeImpact = false;
    private bool isRespawning = false;

    public AudioClip soundJump;
    public AudioClip soundImpact;
    public AudioClip soundDead;
    private AudioSource audioSource;
    // Use this for initialization
    void Start () {
        // get the transform of the main camera
        if (Camera.main != null)
        {
            m_Cam = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning(
                "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
            // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
        }
        Setup();
    }
    void Setup()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = false;
        myTransform = this.transform;

        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();
        m_CapsuleHeight = m_Capsule.height;
        m_CapsuleCenter = m_Capsule.center;

        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        m_OrigGroundCheckDistance = m_GroundCheckDistance;

        GameObject.Find(m_prefixPlayer + "Text").GetComponent<Text>().text = ": "+deaths;

        audioSource = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void FixedUpdate() {
        if((m_isEjected || (Input.GetButton(m_prefixPlayer+"Respawn1") && Input.GetButton(m_prefixPlayer + "Respawn2"))) && !isRespawning)
        {
            m_isEjected = true;
            rb.useGravity = true;
            isRespawning = true;
            StartCoroutine(respawnPlayer());
        }

        if (!m_isEjected)
        {
            gravityUp = gravityAttraction.Attract(myTransform);
            m_Jump = Input.GetButtonDown(m_prefixPlayer + "Jump");

            if (m_IsGrounded && m_Jump)
            {
                audioSource.PlayOneShot(soundJump);
                rb.AddForce(gravityUp * m_JumpPower * 30);
                canMakeImpact = true;
                m_IsGrounded = false;
                m_Jump = false;
                GameObject.FindGameObjectWithTag("planet").GetComponent<WaveBehaviour>().addShockWave(transform.position);
            }

            float h = Input.GetAxisRaw(m_prefixPlayer + "Horizontal");
            float v = Input.GetAxisRaw(m_prefixPlayer + "Vertical");

            Vector3 movement = new Vector3(h, 0.0f, v).normalized;

            rb.MovePosition(rb.position + transform.TransformDirection(movement) * speed * Time.deltaTime);

            GameObject.Find(m_prefixPlayer + "Model").transform.Rotate(0.0f, -Input.GetAxis ("Horizontal") * speed, 0.0f);
        }
    }

    void UpdateAnimator(Vector3 move)
    {
        // update the animator parameters
        m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
        m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
        m_Animator.SetBool("Crouch", m_Crouching);
        m_Animator.SetBool("OnGround", m_IsGrounded);
        if (!m_IsGrounded)
        {
            m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);
        }

        // calculate which leg is behind, so as to leave that leg trailing in the jump animation
        // (This code is reliant on the specific run cycle offset in our animations,
        // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
        float runCycle =
            Mathf.Repeat(
                m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
        float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;
        if (m_IsGrounded)
        {
            m_Animator.SetFloat("JumpLeg", jumpLeg);
        }

        // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
        // which affects the movement speed because of the root motion.
        if (m_IsGrounded && move.magnitude > 0)
        {
            m_Animator.speed = m_AnimSpeedMultiplier;
        }
        else
        {
            // don't use that while airborne
            m_Animator.speed = 1;
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "planet")
        {
            m_IsGrounded = true;
        }   
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "GravityCenter")
        {
            audioSource.PlayOneShot(soundImpact);
            m_isEjected = true;
        }
           
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "OutOfLimits")
        {
            audioSource.PlayOneShot(soundDead);
            Debug.Log("youre out");
            m_isEjected = true;
        }
    }
    IEnumerator respawnPlayer()
    {
        yield return new WaitForSeconds(2);

        this.transform.position = spawn.transform.position;
        m_isEjected = false;
        deaths++;
        isRespawning = false;
        rb.useGravity = false;
        rb.velocity = new Vector3(0, 0, 0);
        Setup();
    }
}
