using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaunchUI : MonoBehaviour
{
    private Canvas canvas;
    private Scrollbar launchScrollbar;
    private float factor;
    private Coroutine LaunchUICoroutine;

    void Start()
    {
        canvas = GetComponent<Canvas>();
        launchScrollbar = GetComponentInChildren<Scrollbar>();
        launchScrollbar.size = 0;

        GlobalEventSystem.AddListener(EventName.LaunchPlayerStart, LaunchStartListener);
        GlobalEventSystem.AddListener(EventName.LaunchPlayerStop, LaunchStopListener);
    }

    #region Callbacks
    private void LaunchStartListener(GlobalEventArgs message)
    {
        canvas.enabled = true;
        GlobalEventArgsFactory.LaunchPlayerStartParser(message, out float maxInputEvaluate);
        factor = 1.0f/ maxInputEvaluate;
        LaunchUICoroutine = StartCoroutine(LaunchBarCoroutine(maxInputEvaluate));

    }
    private void LaunchStopListener(GlobalEventArgs message)
    {
        canvas.enabled = false;
        launchScrollbar.size = 0;
        if (LaunchUICoroutine != null)
        {
            StopCoroutine(LaunchUICoroutine);
        }
    }
    #endregion


    private IEnumerator LaunchBarCoroutine(float maxInputEvaluate)
    {
        float currenTime = 0f;
        
        while(currenTime < maxInputEvaluate)
        {
            currenTime += Time.unscaledDeltaTime;
            launchScrollbar.size = currenTime * factor;
            yield return null;
        }
    }

}
