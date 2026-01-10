using GameDevUtils.Runtime.Settings;
using UnityEngine;
using Zenject;

namespace GameDevUtils.Runtime.Installer
{
    public class DevUtilsInstaller : MonoInstaller
    {
        [SerializeField] private DevUtilsSettingsSO utilsSettingsSo;

        public override void InstallBindings()
        {
            Container.Bind<DevUtilsSettingsSO>().FromInstance(utilsSettingsSo).AsSingle().NonLazy();
            utilsSettingsSo.Init();
        }
    }
}