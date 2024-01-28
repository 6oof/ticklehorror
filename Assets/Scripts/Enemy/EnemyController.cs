
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyController : Ticklable
{
    [SerializeField] private Transform playerTransform; 
    [SerializeField] [Range(0, 25)] private float goalMaxRotationSpeed = 2;
    [SerializeField] public List<EnemyGoal> goals;
    [SerializeField] private Transform goalParent;
    [SerializeField] private float chaseTime = 1;
    private float _chaseTimeLeft = 0;

    [Header("Audio clips")]
    [SerializeField] private AudioClip startChasingAudio;
    [SerializeField] private AudioClip hitAudio;
    [SerializeField] private AudioClip stopChasingAudio;
    [SerializeField] private AudioClip walkAudio;
    private AudioSource _audioSource;
    
    private NavMeshAgent _navMeshAgent;
    private EnemyGoal _currentGoal;
    [SerializeField]private Animator animator;
    private float _waitTime;

    private float gameOverTriggerTime = 3f;
    private float playerInConeTime = 0;
    private enum EnemyState
    {
        LookingForPlayer,
        ChasingPlayer,
        MovingToGoal,
        FindingNewGoal,
        AchievingGoal
    }

    private bool _playerInCone;

    private EnemyState _state = EnemyState.FindingNewGoal;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!IsAlive)
        {
            return;
        }

        if (_playerInCone)
        {
            if (PlayerInSight())
            {
                if (_state != EnemyState.ChasingPlayer)
                {
                    _audioSource.PlayOneShot(startChasingAudio);
                    _state = EnemyState.ChasingPlayer;
                }
                _chaseTimeLeft = chaseTime;
                animator.SetBool("spotsPlayer", true);
            }
        }

        switch (_state)
        {
            case EnemyState.ChasingPlayer:
                _chaseTimeLeft -= Time.deltaTime;
                if (_chaseTimeLeft > 0)
                {
                    _navMeshAgent.destination = playerTransform.position;
                    animator.SetBool("isMoving", true);
                    animator.SetBool("spotsPlayer", true);
                }
                else
                {
                    _state = EnemyState.FindingNewGoal;
                    animator.SetBool("isMoving", true);
                    animator.SetBool("spotsPlayer", false);
                }

                break;
            case EnemyState.LookingForPlayer:
                _navMeshAgent.destination = playerTransform.position;
                _state = EnemyState.MovingToGoal;
                animator.SetBool("isMoving", true);
                animator.SetBool("spotsPlayer", false);
                break;
            case EnemyState.AchievingGoal:
                float angleToDestination = Mathf.Clamp(_currentGoal.transform.rotation.eulerAngles.y-transform.rotation.eulerAngles.y, -goalMaxRotationSpeed,goalMaxRotationSpeed);
                transform.Rotate(Vector3.up * angleToDestination);
                animator.SetBool("isMoving", false);
                animator.SetBool("spotsPlayer", false);
                _waitTime -= Time.deltaTime;
                if (_waitTime <= 0)
                {
                    animator.SetBool("isMoving", true);
                    _state = EnemyState.FindingNewGoal;
                    _currentGoal = null;
                }

                break;
            case EnemyState.MovingToGoal:
                animator.SetBool("isMoving", true);
                if (_navMeshAgent.remainingDistance < 0.1)
                {
                    animator.SetBool("isMoving", false);
                    _state = EnemyState.AchievingGoal;
                }

                break;
            case EnemyState.FindingNewGoal:
                animator.SetBool("isMoving", false);

                if (_currentGoal != null)
                {
                    _navMeshAgent.destination = _currentGoal.transform.position;
                    _state = EnemyState.MovingToGoal;
                }

                else if (goals.Count > 0)
                {
                    _waitTime = goals[0].waitTime;
                    _currentGoal = goals[0];
                    _navMeshAgent.destination = _currentGoal.transform.position;
                    goals.RemoveAt(0);
                    
                    _state = EnemyState.MovingToGoal;
                }

                break;
        }

        if ( PlayerInSight() ) {
            if ( playerInConeTime > gameOverTriggerTime) {
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadScene("GameOver");
            } else {
                playerInConeTime += Time.deltaTime;
            }
        } else {
            playerInConeTime = 0;
        }
        

    }

    private void RotateTowardsPos(Vector3 rotateToPos)
    {
        Vector3 direction = rotateToPos - transform.position;
        Quaternion toRotation = Quaternion.FromToRotation(transform.forward, direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        _playerInCone = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _playerInCone = false;
    }

    private bool PlayerInSight()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerTransform.position - transform.position, out hit, 25,
                LayerMask.GetMask("Default", "Player")))
        {
            return hit.collider.gameObject.layer == LayerMask.NameToLayer("Player");
        }

        return false;
    }

    private EnemyGoal CreateNewGoal(float time, Vector3 pos, Quaternion rot)
    {
        GameObject newGO = new GameObject();
        newGO.transform.parent = goalParent;
        newGO.transform.position = pos;
        newGO.transform.rotation = rot;
        EnemyGoal newGoal = newGO.AddComponent<EnemyGoal>();
        newGoal.waitTime = time;
        return newGoal;
    }

    public override bool Hit(int damage, Vector3 hitPosition)
    {
        Debug.Log("Hit the enemy");
        if (IsAlive)
        {
            Health -= damage;
            animator.SetBool("isAlive", IsAlive); 
            _audioSource.PlayOneShot(hitAudio);
            _state = EnemyState.LookingForPlayer;
            return true;
        }
        return false;
    }

    public void PlayWalkAudio()
    {
        _audioSource.pitch = 0.75f + Random.value/2;
        _audioSource.PlayOneShot(walkAudio);
    }
}