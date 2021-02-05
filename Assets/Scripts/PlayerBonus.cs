using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBonus : MonoBehaviour
{

    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameObject.FindWithTag("GM").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_gameManager.isReversed)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                Destroy(other.gameObject);
            }
        }
    }
}
