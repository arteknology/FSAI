using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    private List<GameObject> _detectedGameObject = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Friends"))
            _detectedGameObject.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (_detectedGameObject.Contains(other.gameObject))
            _detectedGameObject.Remove(other.gameObject);
    }

    public GameObject FirstWithTag(string withTag)
    {
        _detectedGameObject.RemoveAll(o => o == null);
        if (_detectedGameObject.Count < 1)
        {
            return null;
        }
        else
        {
            return _detectedGameObject.FirstOrDefault(o => o.CompareTag(withTag));
        }
    }
}