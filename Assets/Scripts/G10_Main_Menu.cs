using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class G10_Main_Menu : MonoBehaviour
{
    public AudioSource sound;
    public void changeScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
