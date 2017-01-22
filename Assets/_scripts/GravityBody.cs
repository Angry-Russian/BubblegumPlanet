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
    private float jumpCoolDown = 0.0f;
    public Transform animFinalForm;
    public Image imageDie;

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
        imageDie.enabled = false;

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

        if (Time.realtimeSinceStartup < 3) return;


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

            if ((m_IsGrounded && m_Jump) && jumpCoolDown<=0.0f)
            {
                jumpCoolDown = 1.0f;
                m_IsGrounded = false;
                m_Jump = false;
                audioSource.PlayOneShot(soundJump);
                m_Animator.SetTrigger("Jump");
                rb.AddForce(gravityUp * m_JumpPower * 30);
                canMakeImpact = true;               
                GameObject.FindGameObjectWithTag("planet").GetComponent<WaveBehaviour>().addShockWave(transform.position);
            }

            jumpCoolDown -= Time.deltaTime;

            float h = Input.GetAxisRaw(m_prefixPlayer + "Horizontal");
            float v = Input.GetAxisRaw(m_prefixPlayer + "Vertical");
            Debug.Log("h" + h);
            Debug.Log("v" + v);
            Vector3 movement = new Vector3(h, 0.0f, v).normalized;

            rb.MovePosition(rb.position + transform.TransformDirection(movement) * speed * Time.deltaTime);
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
        imageDie.enabled = true;

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
