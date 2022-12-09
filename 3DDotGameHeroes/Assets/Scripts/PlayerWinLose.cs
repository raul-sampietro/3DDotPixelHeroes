using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerWinLose : MonoBehaviour
{

    public float WinningDistance = 50;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(Vector3.zero, transform.position);
        if (distance > WinningDistance)
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}
