using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodScript : MonoBehaviour
{
    public GameObject PAI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject paiInstance = PAI;
            Instantiate(paiInstance, other.transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.01f);
        }
    }
}
