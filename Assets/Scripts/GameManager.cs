using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject LoosePanel;
    public GameObject WinPanel;
    public bool isReversed;
    public float ReverseTime = 5f;
    
    private List<GameObject> _enemies = new List<GameObject>();
    
    private void Start()
    {
        isReversed = false;
        Time.timeScale = 1f;
        LoosePanel.SetActive(false);
        WinPanel.SetActive(false);
    }

    public void Reverse()
    {
        if(!isReversed)
            isReversed = true;
        Debug.Log("Reversed");
    }

    public void StopReverse()
    {
        if (isReversed)
        {
            ReverseTime = 5f;
            isReversed = false;
        }
    }
    
    public void Loose()
    {
        Time.timeScale = 0f;
        LoosePanel.SetActive(true);
    }

    public void Win()
    {
        Time.timeScale = 0f;
        WinPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayAgain()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadSceneAsync(scene.name);
    }
    
    private void Update()
    {
        UpdateEnemiesList();
        if (isReversed)
        {
            ReverseTime -= Time.deltaTime;
            if (ReverseTime <= 0f)
            {
                StopReverse();
            }
        }
    }

    private void UpdateEnemiesList()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (!_enemies.Contains(enemy) && enemy != null)
            {
                _enemies.Add(enemy);
            }
        }
        
        _enemies.RemoveAll(o => o == null);
        
        if (_enemies.Count == 0)
        {
            Win();
        }
    }
    
}
