using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StateStuff;

public class AIAllyCharacterControl : MonoBehaviour {
    public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
    public ThirdPersonCharacter character { get; private set; } // the character we are controlling
    public Transform target;                                    // target to aim for

    public GameObject targetLocation;

    public CharacterAttributes attributes;

    public bool isFollowing;
    public float followDistance = 10f;
    public float attackRadius = 1f;
    public float chaseRadius = 10f;
    public float reactionTime = 0.1f; //Time between making decisions - lower = more responsive

    public float NavAttackStopDistance = 0.5f;
    public float NavFollowStopDistance = 2f;
    public float StoppedRotationSpeed = 10f;

    public float lootRadius = 5f; //The distance that the AI will travel from the loot marker to pickup items

    public StateMachine<AIAllyCharacterControl> stateMachine { get; set; }

    public bool isSelected;

    private float lastQueryTime;

    private void Start()
    {
        // get the components on the object we need ( should not be null due to require component so no need to check )
        agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
        character = GetComponent<ThirdPersonCharacter>();

        agent.updateRotation = false;
        agent.updatePosition = true;

        // Create the statemachine and default the state to idle
        stateMachine = new StateMachine<AIAllyCharacterControl>(this);
        stateMachine.ChangeState(IdleState.Instance);
    }

    public void Chase(Transform target, float additionalChaseTime = 0f, float lastSightOverride = 0f)
    {
        //It's human flesh!
        this.target = target;
        agent.speed = 1f;
    }

    //If we are close enough to the target then stop following
    public bool closeEnough()
    {
        return Vector3.Distance(transform.position, GameController.instance.player.transform.position) > followDistance;
    }

    private void Update()
    {

        GetComponent<Animator>().SetBool("isAttacking", false);
        if (Time.time > lastQueryTime + reactionTime)
        {
            lastQueryTime = Time.time;

            switch (stateMachine.currentState.stateName)
            {
                case stateNames.idle:
                    if (isFollowing)
                    {
                        stateMachine.ChangeState(FollowState.Instance);
                    }
                    break;
                case stateNames.chase:
                    break;
                case stateNames.follow:
                    if (!isFollowing)
                    {
                        stateMachine.ChangeState(IdleState.Instance);
                    }
                    break;
                case stateNames.attack:
                    break;
                case stateNames.shoot:
                    break;
                case stateNames.loot:
                    //stateMachine.ChangeState(LootState.Instance);
                    break;
                default:
                    stateMachine.ChangeState(IdleState.Instance);
                    break;
            }

            stateMachine.Update();

        }

        if (target != null && target.position != agent.destination)
            agent.SetDestination(target.position);

        if (agent.remainingDistance > agent.stoppingDistance)
            character.Move(agent.desiredVelocity, false, false, false, false, false);
        else
            character.Move(Vector3.zero, false, false, false, false, false);


            //    if (isFollowing && !closeEnough())
            //    {
            //        //State: Follow Player

            //        //Set AI Controller target to player
            //        agent.stoppingDistance = NavFollowStopDistance;
            //        target = GameController.instance.player.transform;
            //    }
            //    else
            //    {

            //        //State: Idle?

            //        //Get a list of nearby zombies
            //        Collider[] hitColliders = Physics.OverlapSphere(transform.position, chaseRadius, LayerMask.GetMask("Zombie"));
            //        //If there are any zombies nearby
            //        if (hitColliders.Length > 0)
            //        {
            //            Debug.Log("Zombies are nearby");
            //            Transform closestEnemy = GetClosestEnemy(hitColliders);
            //            agent.stoppingDistance = NavAttackStopDistance;
            //            if (Vector3.Distance(transform.position, closestEnemy.position) <= attackRadius)
            //            {
            //                //Face the enemy and attack
            //                //transform.LookAt(closestEnemy);

            //                GetComponent<Animator>().SetBool("isAttacking", true);
            //                target = closestEnemy;
            //                //RotateTowards(closestEnemy);
            //            }
            //            else
            //            {
            //                //Chase dat ho
            //                //transform.LookAt(closestEnemy);
            //                target = closestEnemy;
            //            }
            //        }
            //        else
            //        {
            //            if (isFollowing)
            //            {
            //                target = GameController.instance.player.transform;
            //                agent.stoppingDistance = NavFollowStopDistance;
            //            }
            //            else
            //            {
            //                target = null;
            //            }
            //        }

            //    }
            //}

            //if (target != null)
            //    agent.SetDestination(target.position);

            //if (agent.remainingDistance > agent.stoppingDistance)
            //    character.Move(agent.desiredVelocity, false, false, false, false);
            //else
            //    character.Move(Vector3.zero, false, false, false, false);

            //if (targetLocation != null)
            //{
            //    if (Input.GetMouseButtonDown(0))
            //    {
            //        RaycastHit hit;
            //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Terrain")))
            //        {
            //            targetLocation.transform.position = hit.point;
            //            SetTarget(targetLocation.transform);
            //            agent.SetDestination(target.position);
            //            targetLocation.GetComponent<Animation>().Play();
            //        }
            //    }
            //}
        }

        public Transform GetClosestEnemy(Collider[] enemies)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Collider col in enemies)
        {
            Transform potentialTarget = col.transform;
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }


    public void SetTarget(Transform target)
    {
        this.target = target;
    }

}
