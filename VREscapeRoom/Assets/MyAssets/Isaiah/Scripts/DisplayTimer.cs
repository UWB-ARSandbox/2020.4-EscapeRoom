using ASL;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisplayTimer : MonoBehaviour
{

    [SerializeField] string SceneToLoad = "Isaiah";
    private Timer timer = null;
    private TextMeshPro text;
    private bool restart = false;
    private bool quit = false;
    private ASLObject asl;

    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponentInChildren<TextMeshPro>();
        asl = GetComponent<ASLObject>();
        asl._LocallySetFloatCallback(arrayCallback);
    }

    void Update()
    {
        if(!timer)
        {
            timer = FindObjectOfType<Timer>();
            timer.stopTimer(); 
            if (timer.didPass())
            {
                text.text = "YOU WIN!\nTime to Complete\nMinutes: " + timer.getMinutes() +
                    " Seconds: " + timer.getSeconds() +
                    "\nGo through the left door to try again. \nGo through the right door to quit.";
            }
            else if (!timer.didPass())
            {
                text.text = "You did not complete the escape room in the required amount of time\n" +
                    "Go through the left door to try again. \nGo through the right door to quit.";
            }
        }
        if(restart && restart == quit)
        {
            restart = false;
        }
        if(restart)
        {
            restart = false;
            timer.resetTimer();
            ASL.ASLHelper.SendAndSetNewScene(SceneToLoad);
        }
        if (quit)
        {
            quit = false;
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
            Application.Quit();
        }
    }

    public void restartGame()
    {
        restart = true;
    }

    public void quitGame()
    {
        float[] f = { 1 };
        asl.SendFloatArray(f);
        quit = true;
    }

    public void arrayCallback(string id, float[] f)
    {
        if(f[0] == 1)
        {
            quit = true;
        }
    }
}
