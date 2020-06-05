using ASL;
using SimpleDemos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    private Timer timer;
    private bool load = false;
    [SerializeField] string SceneToLoad = "Isaiah";
    [SerializeField] private float MAX_TIME = 1800.0f;

    void Update()
    {
        if(!timer)
        {
            timer = FindObjectOfType<Timer>();
            timer.startTimer();
        }
        if(load)
        {
            load = false;
            timer.setPass(true);
            timer.stopTimer();
            ASL.ASLHelper.SendAndSetNewScene(SceneToLoad);
        }
        if(timer.getTime() >= MAX_TIME)
        {
            timer.stopTimer();
            ASL.ASLHelper.SendAndSetNewScene(SceneToLoad);
        }
    }

    public void LoadNextScene()
    {
        load = true;
    }
}
