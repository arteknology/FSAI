using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusFood : MonoBehaviour
{
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameObject.FindWithTag("GM").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!_gameManager.isReversed)
            {
                _gameManager.isReversed = true;
                Destroy(this.gameObject, 0.1f);
            }
        }
    }
}