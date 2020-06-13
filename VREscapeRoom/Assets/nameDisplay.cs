using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ASL;

public class nameDisplay : MonoBehaviour
{
    public GameObject cam;
    public TextMeshPro name;
    // Start is called before the first frame update
    void Start()
    {
        if(this.transform.name == "Player1" && ASL.GameLiftManager.GetInstance().m_PeerId == 1)
        {
            name.SetText(GameLiftManager.GetInstance().m_Username);
        }
        else if (this.transform.name == "Player2" && ASL.GameLiftManager.GetInstance().m_PeerId == 2)
        {
            name.SetText(GameLiftManager.GetInstance().m_Username);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //name.SetText(GameLiftManager.GetInstance().m_Username);
        name.transform.forward = cam.transform.forward;
    }
}
