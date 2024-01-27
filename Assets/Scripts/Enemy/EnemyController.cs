using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : Ticklable
{
    [SerializeField] private Transform playerTransform; 
    [SerializeField] [Range(0, 25)] private float goalMaxRotationSpeed = 2;
    [SerializeField] public List<EnemyGoal> goals;
    [SerializeField] private float chaseTime = 1;
    private float _chaseTimeLeft = 0;
    
    
    private NavMeshAgent _navMeshAgent;
    private EnemyGoal _currentGoal;
    private float _waitTime;

    private enum EnemyState
    {
        LookingForPlayer,
        ChasingPlayer,
        MovingToGoal,
        FindingNewGoal,
        AchievingGoal
    }

    private bool playerInCone;

    private EnemyState state = EnemyState.FindingNewGoal;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (playerInCone)
        {
            if (PlayerInSight())
            {
                Debug.Log("player in sight");
                state = EnemyState.ChasingPlayer;
                _chaseTimeLeft = chaseTime;
            }
        }
        Debug.Log(state);

        switch (state)
        {
            case EnemyState.ChasingPlayer:
                _chaseTimeLeft -= Time.deltaTime;
                if (_chaseTimeLeft > 0)
                {
                    _navMeshAgent.destination = playerTransform.position;
                }
                else
                {
                    state = EnemyState.FindingNewGoal;
                }

                break;
            case EnemyState.LookingForPlayer:
                break;
            case EnemyState.AchievingGoal:
                Debug.Log("achieving goal");
                float angleToDestination = Mathf.Clamp(_currentGoal.transform.rotation.eulerAngles.y-transform.rotation.eulerAngles.y, -goalMaxRotationSpeed,goalMaxRotationSpeed);
                transform.Rotate(Vector3.up * angleToDestination);
            
                _waitTime -= Time.deltaTime;
                if (_waitTime <= 0)
                {
                    state = EnemyState.FindingNewGoal;
                    _currentGoal = null;
                }

                break;
            case EnemyState.MovingToGoal:
                if (_navMeshAgent.remainingDistance < 0.1)
                {
                    state = EnemyState.AchievingGoal;
                }

                break;
            case EnemyState.FindingNewGoal:
                if (_currentGoal != null)
                {
                    _navMeshAgent.destination = _currentGoal.transform.position;
                    state = EnemyState.MovingToGoal;
                }

                else if (goals.Count > 0)
                {
                    _waitTime = goals[0].waitTime;
                    _currentGoal = goals[0];
                    _navMeshAgent.destination = _currentGoal.transform.position;
                    goals.RemoveAt(0);
                    state = EnemyState.MovingToGoal;
                }

                break;
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
        state = EnemyState.ChasingPlayer;
        playerInCone = true;
    }

    private void OnTriggerExit(Collider other)
    {
        playerInCone = false;
    }

    private bool PlayerInSight()
    {
        return Physics.Raycast(transform.position, playerTransform.position - transform.position);
        
    }

    public override bool Hit(int damage, Vector3 hitPosition)
    {
        if (IsAlive)
        {
            Health -= damage;
            state = EnemyState.LookingForPlayer;
            return true;
        }
        return false;
    }
}