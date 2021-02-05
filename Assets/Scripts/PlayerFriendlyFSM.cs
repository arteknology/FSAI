using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class PlayerFriendlyFSM : MonoBehaviour
{
    [SerializeField] private State CurrentState;
    public int DestinationThreshold;
    public CollisionDetector EyesDetector;
    public GameObject PAI;

    private NavMeshAgent _navMeshAgent;
    private GameObject[] _destinations;
    private GameObject _currentDestination;
    private GameObject _currentTarget;
    private bool isReached;
    private GameManager Gm;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        isReached = false;
        CurrentState = State.PickDestination;
        Gm = GameObject.FindWithTag("GM").GetComponent<GameManager>();
    }

    private void Start()
    {
        _destinations = GameObject.FindGameObjectsWithTag("Destination");
    }

    void Update()
        {
            UpdateState();
            
        }

    private void UpdateState()
    {
        switch(CurrentState)
        {
            case State.PickDestination:
                PickDestination();
                break;
            case State.MoveToDestination:
                MoveToDestination();
                break;
            case State.Clone:
                Clone();
                break;
            case State.Chase:
                Chase();
                break;
            case State.Attack:
                Attack();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void PickDestination()
    {
        int rdIndex = Random.Range(0, _destinations.Length);
        _currentDestination = _destinations[rdIndex];
        
        //Check if move to destination
        if (_currentDestination != null)
            CurrentState = State.MoveToDestination;
    }

    private void MoveToDestination()
    {
        //Check if pick destination
        if(!isReached)
            _navMeshAgent.SetDestination(_currentDestination.transform.position);

        //Check if destination is reached
        if (Vector3.Distance(transform.position, _currentDestination.transform.position) <= DestinationThreshold)
        {
            isReached = true;
            CurrentState = State.Clone;
        }
        
        //Check if chase
        GameObject firstWithTag = EyesDetector.FirstWithTag("Enemy");
        if (firstWithTag != null && !Gm.isReversed)
        {
            _currentTarget = firstWithTag;
            CurrentState = State.Chase;
        }
    }
    
    private void Clone()
    {
        if (isReached)
        {
            GameObject AIfriendInstance = PAI;
            Instantiate(AIfriendInstance, this.transform.position, Quaternion.identity);

            isReached = false;
        }
        
        else
        {
            //Check if chase
            GameObject firstWithTag = EyesDetector.FirstWithTag("Enemy");
            if (firstWithTag != null && !Gm.isReversed)
            {
                _currentTarget = firstWithTag;
                CurrentState = State.Chase;
            }
            else
            {
                CurrentState = State.PickDestination;
            }
        }
    }

    private void Chase()
    {
        //Check if target is not null
        if (_currentTarget != null && !Gm.isReversed)
        {
            _navMeshAgent.SetDestination(_currentTarget.transform.position);
            //Check if attack
            if (Vector3.Distance(transform.position, _currentTarget.transform.position) <= DestinationThreshold && !Gm.isReversed)
                CurrentState = State.Attack;
        }
        
        else
        {
            CurrentState = State.PickDestination;
        }


        //Check if pick destination
        if (EyesDetector.FirstWithTag("Enemy") != _currentTarget)
        {
            _currentTarget = null;
            CurrentState = State.PickDestination;
        }
    }

    private void Attack()
    {
        if (Gm.isReversed)
        {
            CurrentState = State.PickDestination;
        }
        
        Destroy(_currentTarget);

        if (_currentTarget == null)
        {
            //Check if chase
            GameObject firstWithTag = EyesDetector.FirstWithTag("Enemy");
            if (firstWithTag != null)
            {
                _currentTarget = firstWithTag;
                CurrentState = State.Chase;
            }
            //else pick destination
            else
            {
                CurrentState = State.PickDestination;
            }
        }

    }
        
}

