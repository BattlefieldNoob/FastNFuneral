using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour
{

    [SerializeField] [EventRef] private string _doorRef;


    [Header("Church")]
    [SerializeField] [EventRef] private string _churchSnapshotRef;
    [SerializeField] [EventRef] private string _churchMusicRef;
    [SerializeField] [EventRef] private string _churchAmbienceRef;

    [Header("Morgue")]
    [SerializeField] [EventRef] private string _morgueSnapshotRef;
    [SerializeField] [EventRef] private string _morgueMusicRef;
    [SerializeField] [EventRef] private string _morgueAmbienceRef;
    

    public FMOD.Studio.EventInstance _snapshotInstance;
    public FMOD.Studio.EventInstance _musicInstance;
    public FMOD.Studio.EventInstance _ambienceInstance;
    
    private void Start()
    {
        _snapshotInstance = RuntimeManager.CreateInstance(_churchSnapshotRef);
        _snapshotInstance.start();

        _musicInstance = RuntimeManager.CreateInstance(_churchMusicRef);
        _musicInstance.start();

        _ambienceInstance = RuntimeManager.CreateInstance(_churchAmbienceRef);
        _ambienceInstance.start();

        EventManager.Instance.OnSpeedChange.AddListener((value) =>
        {

            _snapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _ambienceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);


            RuntimeManager.PlayOneShot(_doorRef);

            if (value >= 5)
            {
                OnMorgueEnter();
            }
            else
            {
                OnCurchEnter();
            }

            _snapshotInstance.start();
            _musicInstance.start();
            _ambienceInstance.start();


        });
    }


    private void OnMorgueEnter() {
        _snapshotInstance = RuntimeManager.CreateInstance(_morgueSnapshotRef);
        _musicInstance = RuntimeManager.CreateInstance(_morgueMusicRef);
        _ambienceInstance = RuntimeManager.CreateInstance(_morgueAmbienceRef);


    }

    private void OnCurchEnter() {
        _snapshotInstance = RuntimeManager.CreateInstance(_churchSnapshotRef);
        _musicInstance = RuntimeManager.CreateInstance(_churchMusicRef);
        _ambienceInstance = RuntimeManager.CreateInstance(_churchAmbienceRef);



    }

    private void OnDestroy()
    {
        _snapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _ambienceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }


}
