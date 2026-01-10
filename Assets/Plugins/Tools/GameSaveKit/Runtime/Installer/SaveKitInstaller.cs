using GameDevUtils.Runtime.Settings;
using GameSaveKit.Runtime.Settings;
using UnityEngine;
using Zenject;

namespace GameSaveKit.Runtime.Installer
{
    public class SaveKitInstaller : MonoInstaller
    {
        [SerializeField] private SaveKitSettingsSO saveKitSettingsSo;

        public override void InstallBindings()
        {
            Container.Bind<SaveKitSettingsSO>().FromInstance(saveKitSettingsSo).AsSingle().NonLazy();
            saveKitSettingsSo.Init();
        }
    }
}