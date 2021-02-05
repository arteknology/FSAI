using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class FiniteStateAI : MonoBehaviour
{
    [SerializeField] private State CurrentState;
    public int DestinationThreshold;
    public float TargetTreshold;
    public CollisionDetector EyesDetector;
    public GameObject AIfriend;
    public Material ReversedMaterial;
    public Material BasicMaterial;
    
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

            if (Gm.isReversed)
            {
                gameObject.GetComponent<MeshRenderer>().material = ReversedMaterial;
            }
            else
            {
                gameObject.GetComponent<MeshRenderer>().material = BasicMaterial;
            }
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
            case State.ChaseAI:
                ChaseAI();
                break;
            case State.AttackAI:
                AttackAI();
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
        if (_currentDestination != null && _currentDestination.transform.position != this.transform.position)
            CurrentState = State.MoveToDestination;
        
    }

    private void MoveToDestination()
    {
        _navMeshAgent.speed = 8;

        if (Gm.isReversed)
        {
            GameObject firstWithTag = EyesDetector.FirstWithTag("Friends");
            if (firstWithTag != null)
            {
                _currentTarget = firstWithTag;
                CurrentState = State.ChaseAI;
            }
            else
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
            }
        }
        else
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
            
            GameObject firstWithTag = EyesDetector.FirstWithTag("Player");
            if (firstWithTag != null)
            {
                _currentTarget = firstWithTag;
                CurrentState = State.Chase;
            }
        }
            
    }

    private void Clone()
    {
        if (isReached)
        {
            GameObject AIfriendInstance = AIfriend;
            Instantiate(AIfriendInstance, this.transform.position, Quaternion.identity);
            isReached = false;
        }
        
        else
        {
            if(Gm.isReversed)
            {
                GameObject firstWithTag = EyesDetector.FirstWithTag("Friends");
                if (firstWithTag != null)
                {
                    _currentTarget = firstWithTag;
                    CurrentState = State.ChaseAI;
                }
                else
                {
                    CurrentState = State.PickDestination;
                }
            }
            else
            {
                GameObject firstWithTag = EyesDetector.FirstWithTag("Player");
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
    }

    private void Chase()
    {
        _navMeshAgent.speed = 12;
        _navMeshAgent.SetDestination(_currentTarget.transform.position);
        
        //Check if attack
        if (Vector3.Distance(transform.position, _currentTarget.transform.position) <= TargetTreshold && !Gm.isReversed)
            CurrentState = State.Attack;
        
        //Check if pick destination
        if (EyesDetector.FirstWithTag("Player") != _currentTarget)
        {
            _currentTarget = null;
            CurrentState = State.PickDestination;
        }
        
        //Check if reversed
        if (Gm.isReversed)
        {
            GameObject firstWithTag = EyesDetector.FirstWithTag("Friends");
            if (firstWithTag != null)
            {
                _currentTarget = firstWithTag;
                CurrentState = State.ChaseAI;
            }
            else
            {
                CurrentState = State.PickDestination;
            }
        }
    }

    private void Attack()
    {
        Gm.Loose();

        if (Vector3.Distance(transform.position, _currentTarget.transform.position) > TargetTreshold && !Gm.isReversed)
            CurrentState = State.Chase;
    }

    private void ChaseAI()
    {
        if (EyesDetector.FirstWithTag("Friends") != _currentTarget || !Gm.isReversed)
        {
            _currentTarget = null;
            CurrentState = State.PickDestination;
        }
        
        _navMeshAgent.speed = 16;
        
        GameObject firstWithTag = EyesDetector.FirstWithTag("Friends");
        if (_currentTarget != null)
        {
            _navMeshAgent.SetDestination(_currentTarget.transform.position);

            //Check if attack
            if (Vector3.Distance(transform.position, _currentTarget.transform.position) <= DestinationThreshold)
                CurrentState = State.AttackAI;
        }
    }

    private void AttackAI()
    {
        if (!Gm.isReversed)
        {
            _currentTarget = null;
                CurrentState = State.PickDestination;
        }
        
        Destroy(_currentTarget);
        
        if (_currentTarget != null && Vector3.Distance(transform.position, _currentTarget.transform.position) > TargetTreshold)
            CurrentState = State.ChaseAI;
    }
        
}

