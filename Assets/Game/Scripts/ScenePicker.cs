using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePicker : MonoBehaviour
{
    public void loadlevel(string level)
    {
        SceneManager.LoadScene(level);
    }
}
