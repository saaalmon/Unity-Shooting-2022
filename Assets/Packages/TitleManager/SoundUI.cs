using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KanKikuchi.AudioManager;
//using DG.Tweening;

public class SoundUI : MonoBehaviour
{
    public CanvasGroup SoundPanel;

    public Slider bgmSlider;
    public Slider seSlider;

    private static float m_bgmVol = 1.0f;
    private static float m_seVol = 1.0f;

    public void Start()
    {
        
    }

    public void ChangeBGMVolume(float volume)
    {
        BGMManager.Instance.ChangeBaseVolume(volume);
        Debug.Log("BGMのボリューム変更 : " + volume);
        m_bgmVol = volume;
    }

    public void ChangeSEVolume(float volume)
    {
        SEManager.Instance.ChangeBaseVolume(volume);
        Debug.Log("SEのボリューム変更 : " + volume);
        m_seVol = volume;
    }

    public void OpenUI()
    {
        SoundPanel.gameObject.SetActive(true);

        //var seq = DOTween.Sequence()
        //    .AppendCallback(() => SoundPanel.gameObject.SetActive(true))
        //    .AppendCallback(() => SoundPanel.transform.localScale = Vector3.zero)
        //    .Append(SoundPanel.transform.DOScale(Vector3.one, 0.2f))
        //    .Play();

        bgmSlider.value = m_bgmVol;
        seSlider.value = m_seVol;
    }

    public void CloseUI()
    {
        SoundPanel.gameObject.SetActive(false);

        //var seq = DOTween.Sequence()
        //    .Append(SoundPanel.transform.DOScale(Vector3.zero, 0.2f))
        //    .AppendCallback(() => SoundPanel.gameObject.SetActive(false))
        //    .Play();
    }
}
