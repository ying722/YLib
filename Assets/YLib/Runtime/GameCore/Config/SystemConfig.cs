using Sirenix.OdinInspector;
using UnityEngine.Audio;
using UnityEngine;
using UnityEditor;


namespace YLib.GameCore.Config
{
    [HideMonoScript]
    public class SystemConfig : BaseConfig<SystemConfig>
    {
        [Title("音效設定")]
        public AudioMixer AudioMixer;
        public string MasterVolumeParameter = "MasterVolume";
        public string BGMVolumeParameter = "BGMVolume";
        public string SFXVolumeParameter = "SFXVolume";
        public string VoiceVolumeParameter = "VoiceVolume";

        // 修改菜單項以創建新的 SystemConfig
        [MenuItem("Assets/Create/Config/SystemConfig", priority = 0)]
        public static void CreateSystemConfig()
        {
            CreateConfig();
        }
    }
}