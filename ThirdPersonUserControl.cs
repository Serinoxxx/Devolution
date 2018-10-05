using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


//[RequireComponent(typeof (ThirdPersonCharacter))]
public class ThirdPersonUserControl : Movement
{
    public HeadLookController lookAt;
    public HeadLookController aimAt;
    public GameObject crosshair;
    public float crosshairSpeed = 1f;

    public AudioSource footStepSource;


    private bool _isCommanding;
    public bool isCommanding
    {
        get { return _isCommanding; }
        set { _isCommanding = value; }
    }

    private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
    private bool m_Kick;
    private bool m_isAiming;
    private bool m_Attack;
    private bool m_Roll;
    private Animator m_Animator;

    private void Start()
    {
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

        // get the third person character ( this should never be null due to require component )
        m_Character = GetComponent<ThirdPersonCharacter>();

        m_Animator = GetComponent<Animator>();
    }

    private void Update()
    {

        if (isLocked)
        {
            return;
        }

        MovementUpdate();

        if (_isCommanding || GameController.instance.squadMgr.isWayPointActive)
        {
            CommandUpdate();
        }
        else
        {
            Item item = GameController.instance.inv.equippedItem;
            if (item != null && !item.isReloading)
            {
                CombatUpdate();
            }
        }

        StandardUpdate();
        
    }

    //All movement related update code goes here
    private void MovementUpdate()
    {
        if (!m_Roll)
        {
            m_Roll = CrossPlatformInputManager.GetButtonDown("Jump");
        }

        

        //Disabled Jumping for now
        //if (!m_Jump)
        //{
        //    m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        //}


    }

    //All combat related update code goes here
    private void CombatUpdate()
    {
        Item item = GameController.instance.inv.equippedItem;

        //If we have a gun equipped and we reload (R)
        if (item != null && (item.type == Item.itemType.pistol || item.type == Item.itemType.rifle) && CrossPlatformInputManager.GetButton("Reload"))
        {
            Gun gun = (Gun)item;
            gun.StartReloading();
        }
        else if (!m_Kick && CrossPlatformInputManager.GetButton("Fire2"))
        {
            //rightcliking
            if (item != null && item.type == Item.itemType.rifle)
            {
                m_Animator.SetBool("isAiming", true);
                lookAt.enabled = false;
                aimAt.enabled = true;
                crosshair.SetActive(true);

                if (CrossPlatformInputManager.GetButtonDown("Fire2"))
                {
                    m_Cam.GetComponent<CameraOrbit2>().ToogleAimingZoom(true);
                }

                //Switch shoulders when player presses shift while aiming
                if (CrossPlatformInputManager.GetButtonDown("Fire3"))
                {
                    m_Cam.GetComponent<CameraOrbit2>().xOffset *= -1f;
                }

                item.GetComponent<Gun>().AimAtCenter();

            }
            else
            {
                //m_Kick = true;
                if (CrossPlatformInputManager.GetButtonDown("Fire2") && !m_Animator.GetBool("AnimOverride"))
                    m_Animator.SetTrigger("heavyAttack");
            }
        }
        else
        {
            //not rightclicking
            m_Animator.SetBool("isAiming", false);
            m_Animator.SetBool("isKicking", false);

            if (CrossPlatformInputManager.GetButtonUp("Fire2"))
            {
                m_Cam.GetComponent<CameraOrbit2>().ToogleAimingZoom(false);
            }
            lookAt.enabled = true;
            aimAt.enabled = false;
            crosshair.SetActive(false);
            if (item != null)
                item.transform.localEulerAngles = item.equippedRot;
        }

        if (!m_Attack && CrossPlatformInputManager.GetButton("Fire1"))
        {
            if (item != null && item.type == Item.itemType.rifle)
            {
                m_Animator.SetBool("isAutoFiring", true);
            }
            else
            {
                switch(item.animType)
                {
                    case Item.animationType.attack:
                        m_Attack = true;
                        break;
                    case Item.animationType.drink:
                        if (!m_Animator.GetCurrentAnimatorStateInfo(1).IsName("Drinking") &&
                            !m_Animator.GetNextAnimatorStateInfo(1).IsName("Drinking"))
                        {
                            m_Animator.SetTrigger("drink");
                        }
                        break;
                }
                
            }
        }
        else
        {
            m_Animator.SetBool("isAutoFiring", false);
            m_Animator.SetBool("isAttacking", false);
        }
    }

    //All squad/command related update code goes here
    private void CommandUpdate()
    {
        
    }

    //Miscellaneous Update Code
    private void StandardUpdate()
    {

        if (CrossPlatformInputManager.GetButtonDown("Vomit"))
        {
            if (!m_Animator.GetCurrentAnimatorStateInfo(1).IsName("Vomit") &&
                            !m_Animator.GetNextAnimatorStateInfo(1).IsName("Vomit"))
            {
                m_Animator.SetTrigger("vomit");
            }
        }

        //If we are looking at an ally
        RaycastHit hit;
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward * 10f, out hit);
        if (hit.collider != null && hit.collider.gameObject.GetComponent<AIAllyCharacterControl>())
        {
            //This is an Ally, display the "Follow me" or "Stay here" text
            AIAllyCharacterControl follower = hit.collider.gameObject.GetComponent<AIAllyCharacterControl>();
            if (follower.isFollowing)
            {
               GameController.instance.interactText.text = "";
            }
            else
            {
                GameController.instance.interactText.text = "(E) Command: Follow me";
            

                //Press E to toggle follow/stay
                if (Input.GetKeyDown(KeyCode.E))
                {
                    follower.isFollowing = !follower.isFollowing;
                    if (follower.isFollowing)
                    {
                        GameController.instance.squadMgr.AddAlly(follower.gameObject);
                    }
                }
            }
        }
        else if (GameController.instance.interactText.text == "(E) Command: Stay here" || GameController.instance.interactText.text == "(E) Command: Follow me")
        {
            GameController.instance.interactText.text = "";
        }

    }


    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        // read inputs
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");
        bool crouch = Input.GetKey(KeyCode.C);

        // calculate move direction to pass to character
        if (m_Cam != null)
        {
            // calculate camera relative direction to move:
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = v * m_CamForward + h * m_Cam.right;
        }
        else
        {
            // we use world-relative directions in the case of no main camera
            m_Move = v * Vector3.forward + h * Vector3.right;
        }
#if !MOBILE_INPUT
        // sprint when holding shift
        if (Input.GetKey(KeyCode.LeftShift) && gameObject.GetComponent<Health>().currentStamina > 10)
        {
            m_Move *= 1.2f;
            gameObject.GetComponent<Health>().isSprinting = true;
        }
        else
        {
            m_Move *= 0.8f;
            gameObject.GetComponent<Health>().isSprinting = false;
        }
#endif

        footStepSource.pitch = Mathf.Clamp(m_Move.sqrMagnitude, .5f, 1f);
        if (m_Move.sqrMagnitude > 0 && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            if (!footStepSource.isPlaying)
                footStepSource.Play();
        }
        else
        {
            footStepSource.Stop();
        }

        // pass all parameters to the character control script
        m_Character.Move(m_Move, crouch, m_Jump, m_Kick, m_Attack, m_Roll);

        m_Kick = false;

        m_Jump = false;

        m_Attack = false;

        m_Roll = false;
    }
}
