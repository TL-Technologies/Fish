using MoreMountains.NiceVibrations;
using UnityEngine;

public class SettingBox : BaseBox
{
    private static GameObject instance;
    [SerializeField] SliceOnOff sliceSound, sliceVibrate, sliceMusic;
    bool init = false;
    protected override void Awake()
    {
        base.Awake();
        if (!init)
        {
            sliceVibrate.onClickOff += TurnHapticsOff;
            sliceVibrate.onClickOn += TurnHapticsOn;

            sliceSound.onClickOff += TurnSoundsOff;
            sliceSound.onClickOn += TurnSoundsOn;

            sliceMusic.onClickOff += TurnMusicOff;
            sliceMusic.onClickOn += TurnMusicOn;

            CheckForSoundsAtStart();
            init = true;
        }
    }
    public static SettingBox Setup()
    {
        if (instance == null)
        {
            instance = Instantiate(UIManager.Instance.settingPrefab);
        }
        instance.SetActive(true);

        return instance.GetComponent<SettingBox>();
    }
    public virtual void TurnSoundsOn()
    {
        AudioManager.Instance.audioSource.enabled = true;
        PlayerPrefsData.soundTurn = true;
    }

    public virtual void TurnSoundsOff()
    {
        AudioManager.Instance.audioSource.enabled = false;
        PlayerPrefsData.soundTurn = false;
    }


    public virtual void TurnMusicOn()
    {
        AudioManager.Instance.bgAudioSource.Play();
        PlayerPrefsData.musicTurn = true;
    }

    public virtual void TurnMusicOff()
    { 
        AudioManager.Instance.bgAudioSource.Stop();
        PlayerPrefsData.musicTurn = false;
    }
    public void TurnHapticsOff()
    {
        MMVibrationManager.SetHapticsActive(false);
        PlayerPrefsData.vibrate = false;
    }
    public void TurnHapticsOn()
    {
        MMVibrationManager.SetHapticsActive(true);
        PlayerPrefsData.vibrate = true;
    }

    private void CheckForSoundsAtStart()
    {
        if (PlayerPrefsData.musicTurn)
        {
            TurnMusicOn();
            sliceMusic.SetOn(true);
        }
        else
        {
            TurnMusicOff();
            sliceMusic.SetOn(false);
        }

        if (PlayerPrefsData.soundTurn)
        {
            TurnSoundsOn();
            sliceSound.SetOn(true);
        }
        else
        {
            TurnSoundsOff();
            sliceSound.SetOn(false);
        } 
            
        if (PlayerPrefsData.vibrate)
        {
            TurnHapticsOn();
            sliceVibrate.SetOn(true);
        }
        else
        {
            TurnHapticsOn();
            sliceVibrate.SetOn(false);
        }

    }
}
