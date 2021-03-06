using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenuController : MonoBehaviour
{
    public GameObject IntergradeImage;
    public float FadeSpeed = 0.05f;

    public void OnStartGame()
    {
        StartCoroutine(FadeImage());

        StartCoroutine(DelayInvoker.DelayToInvoke(() =>
        {
            SceneManager.LoadScene(1);
            SceneManager.sceneLoaded += StartSceneLoaded;
        }, 1));
    }

    private void StartSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "StartScene")
        {
            ResetIntergradeImage();
        }
    }

    public void OnExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private IEnumerator FadeImage()
    {
        var img = IntergradeImage.GetComponent<Image>();
        float r = img.color.r;
        float g = img.color.g;
        float b = img.color.b;
        while (img.color.a < 1)
        {
            img.color = new Color(r, g, b, img.color.a + FadeSpeed);
            yield return new WaitForFixedUpdate();  //因为update不稳定,fixedupdate能保证每个颜色变换都是一个时间稳定的物理帧。就可以精准控制。
        }
    }

    private void ResetIntergradeImage()
    {
        if (IntergradeImage != null)
        {
            var img = IntergradeImage.GetComponent<Image>();
            var color = img.color;
            color.a = 0;
            img.color = color;
        }
    }
}
