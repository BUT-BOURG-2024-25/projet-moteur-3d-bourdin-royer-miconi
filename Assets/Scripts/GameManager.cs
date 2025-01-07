using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public int playTime = 0;
    public int playTimeHours;
    public int playTimeMinutes;
    public int playTimeSeconds;

    public int groundGridSize = 75;

    public Action sceneLoaded;
    public Action reloadScene;

    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(LoadYourAsyncScene());
    }

    IEnumerator LoadYourAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        Camera.main.GetComponent<CinemachineVirtualCamera>().LookAt = PlayerManager.Instance.player.transform;
        Camera.main.GetComponent<CinemachineVirtualCamera>().Follow = PlayerManager.Instance.player.transform;
        sceneLoaded.Invoke();
    }

    private void Start()
    {
        StartCoroutine(RecordTimeRoutine());
    }

    public IEnumerator RecordTimeRoutine()
    {
        TimeSpan ts;
        while (true)
        {
            yield return new WaitForSeconds(1);
            playTime += 1;

            ts = TimeSpan.FromSeconds(playTime);

            playTimeHours = (int)ts.TotalHours;
            playTimeMinutes = ts.Minutes;
            playTimeSeconds = ts.Seconds;
        }
    }

    public void StartGame()
    {
        reloadScene?.Invoke();
        StartCoroutine(EnemyManager.Instance.StartWaves());
    }

    public void Restart()
    {
        reloadScene?.Invoke();
        StartCoroutine(LoadYourAsyncScene());
    }

    public void Quit()
    {
        Application.Quit();
    }

    public override void Reload()
    {
    }
}
