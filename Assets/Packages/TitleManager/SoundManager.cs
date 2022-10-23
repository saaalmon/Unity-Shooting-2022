using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;

public static class SoundManager
{
    public static void PlayBGM(string bgmPath, float volumeRate = 1, bool allowsDuplicate = false)
    {
        Debug.Log(bgmPath + "再生開始");

        float delay = 0;

        BGMManager.Instance.Play(bgmPath, volumeRate: volumeRate, delay: delay, allowsDuplicate: allowsDuplicate);
    }

    public static void PlaySE(string sePath)
    {
        Debug.Log(sePath + "再生開始");

        float delay = 0;

        SEManager.Instance.Play(sePath, delay: delay, callback: () => {
            Debug.Log(sePath + "再生終了");
        });
    }

    public static void StopBGM()
    {
        Debug.Log("BGM停止");
        BGMManager.Instance.Stop();
    }

    public static void StopSE()
    {
        Debug.Log("SE停止");
        SEManager.Instance.Stop();
    }

    public static void FadeOutBGM()
    {
        Debug.Log("BGMフェードアウト開始");
        BGMManager.Instance.FadeOut(callback: () => {
            Debug.Log("BGMフェードアウト終了");
        });
    }

    public static void FadeInBGM()
    {
        Debug.Log("BGMフェードイン開始");
        BGMManager.Instance.FadeIn(callback: () => {
            Debug.Log("BGMフェードイン終了");
        });
    }
}
