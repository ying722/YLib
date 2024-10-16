using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using YLib.GameCore;
using YLib.GameCore.Config;
using System.Threading.Tasks;

namespace YLib.GameCore
{
    public class AudioModule : BaseModule
    {
        public  AudioMixer audioMixer { get; private set; }

        public override void Initialize(params object[] param)
        {
            if(SystemConfig.Instance == null || SystemConfig.Instance.AudioMixer == null)
            {
                Debug.LogError("AudioModule Initialize Error: SystemConfig or AudioMixer is null");
                return;
            }

            audioMixer = SystemConfig.Instance.AudioMixer;
        }

        public float MasterVolume
        {
            get
            {
                audioMixer.GetFloat(SystemConfig.Instance.MasterVolumeParameter, out var volume);
                return volume;
            }
            set
            {
                audioMixer.SetFloat(SystemConfig.Instance.MasterVolumeParameter, value);
            }
        }
        
        public float BGMVolume
        {
            get
            {
                audioMixer.GetFloat(SystemConfig.Instance.BGMVolumeParameter, out var volume);
                return volume;
            }
            set
            {
                audioMixer.SetFloat(SystemConfig.Instance.BGMVolumeParameter, value);
            }
        }
        
        public float SFXVolume
        {
            get
            {
                audioMixer.GetFloat(SystemConfig.Instance.SFXVolumeParameter, out var volume);
                return volume;
            }
            set
            {
                audioMixer.SetFloat(SystemConfig.Instance.SFXVolumeParameter, value);
            }
        }

        public float VoiceVolume
        {
            get
            {
                audioMixer.GetFloat(SystemConfig.Instance.VoiceVolumeParameter, out var volume);
                return volume;
            }
            set
            {
                audioMixer.SetFloat(SystemConfig.Instance.VoiceVolumeParameter, value);
            }
        }

        public void MuteMaster()
        {
            audioMixer.SetFloat(SystemConfig.Instance.MasterVolumeParameter, -80);
        }

        public void MuteBGM()
        {
            audioMixer.SetFloat(SystemConfig.Instance.BGMVolumeParameter, -80);
        }

        public void MuteSFX()
        {
            audioMixer.SetFloat(SystemConfig.Instance.SFXVolumeParameter, -80);
        }

        public void MuteVoice()
        {
            audioMixer.SetFloat(SystemConfig.Instance.VoiceVolumeParameter, -80);
        }

        public void PauseAudio(AudioSource source)
        {
            source.Pause();
        }

        public void StopAudio(AudioSource source)
        {
            source.Stop();
        }

        public void PlayAudio(string clipPath,AudioSource source,bool fade = false,float fadeTime = 1.0f)
        {
            if(audioMixer == null)
            {
                Debug.LogError("AudioModule PlayAudio Error: AudioMixer is null");
                return;
            }

            AudioClip clip = ResourceManager.LoadFromResources<AudioClip>(clipPath);

            if(fade)
            {
                if(source.isPlaying)
                {
                    _ = FadeOutAndPlayNewAudio(clip,source,fadeTime);
                }
                else
                {
                    _ = FadeInNewAudio(clip,source,fadeTime);
                }
            }
            else
            {
                source.clip = clip;
                source.Play();
            }
        }

        private async Task FadeOutAndPlayNewAudio(AudioClip newClip, AudioSource source, float fadeDuration)
        {
            float startVolume = source.volume;

            // 漸漸降低當前音樂的音量
            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                source.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
                await Task.Yield();  // 避免阻塞主線程，讓控制回到主循環
            }

            source.volume = 0;
            source.Stop();

            // 開始播放新的音樂並漸漸提高音量
            await FadeInNewAudio(newClip, source, fadeDuration);
        }

        private async Task FadeInNewAudio(AudioClip newClip, AudioSource source, float fadeDuration)
        {
            float targetVolume = 1.0f;  // 可以設置目標音量（1.0代表最大音量）

            // 先確保音量為0開始
            source.volume = 0;
            source.clip = newClip;
            source.Play();

            // 漸漸提高新音樂的音量
            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                source.volume = Mathf.Lerp(0, targetVolume, t / fadeDuration);
                await Task.Yield();  // 避免阻塞主線程
            }

            source.volume = targetVolume;
        }
    }
}