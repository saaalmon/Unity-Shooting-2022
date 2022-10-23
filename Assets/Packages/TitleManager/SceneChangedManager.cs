using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using KanKikuchi.AudioManager;

public class SceneChangedManager : MonoBehaviour
{
    public void SceneChanged(string scene)
    {
        SceneManager.LoadScene(scene);
        SoundManager.StopBGM();
    }

    public void SceneChangedStay(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
