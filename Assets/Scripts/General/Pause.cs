using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public void PauseGame()
    {
        ChronoManager.Instance.PauseChrono();
    }

    public void ResumeGame()
    {
        ChronoManager.Instance.ResumeChrono();
    }
}
