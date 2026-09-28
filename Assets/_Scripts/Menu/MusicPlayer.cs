using Ami.BroAudio;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private SoundID musicTrack;

    private void Start()
    {
        if (!BroAudio.HasAnyPlayingInstances(musicTrack)) BroAudio.Play(musicTrack);
    }
}
