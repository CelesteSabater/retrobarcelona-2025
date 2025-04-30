using UnityEngine;
using System;
using retrobarcelona.UI;

namespace retrobarcelona.Disposable
{
    public class LoadingScreenDisposable: IDisposable
    {
        private readonly LoadingScreen _loadingScreen;

        public LoadingScreenDisposable(LoadingScreen loadingScreen)
        {
            _loadingScreen = loadingScreen;
            _loadingScreen.Show();
        }

        public void UpdateLoadingProgress(float progress) => _loadingScreen.UpdateLoadingProgress(progress);

        public void Dispose() => _loadingScreen.Hide();
    }
}