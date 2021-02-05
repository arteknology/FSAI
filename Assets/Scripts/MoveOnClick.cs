using UnityEngine;
using UnityEngine.AI;

public class MoveOnClick : MonoBehaviour {
        
    private NavMeshAgent _navMeshAgent;
    private Camera _mainCamera;

    private void Awake() {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _mainCamera = Camera.main;
    }

    private void Update() {
        if (Input.GetButton("Fire1")) {
            if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), 
                out RaycastHit hit, int.MaxValue))
                _navMeshAgent.SetDestination(hit.point);
        }
    }
}
