using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavTest : MonoBehaviour
{
    
    private NavMeshAgent _navMeshAgent;
    private bool _movingTowardsGoal;
    private float _waitTime;

    [SerializeField]
    public List<EnemyGoal> goals;
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (_waitTime > 0)
        {
            _waitTime -= Time.deltaTime;
        }
        else
        {
            //if goal reached
            if (_movingTowardsGoal && _navMeshAgent.remainingDistance < 0.1)
            {
                Debug.Log("Goal reached and removed");
                _waitTime = goals[0].waitTime;
                goals.RemoveAt(0);
                _movingTowardsGoal = false;
            }
            
            //If has a goal left and is not already moving towards one, set the next one
            else if (goals.Count > 0 && !_movingTowardsGoal)
            {
                _navMeshAgent.destination = goals[0].transform.position;
                _movingTowardsGoal = true;
            }
        }
    }
}