using System;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;

namespace Sources.Frameworks.MyGameCreator.SkyAndWeathers.Presentation
{
    public class RainView : View
    {
        [SerializeField] private ParticleSystem _particleSystem;
        private ParticleSystem.MainModule _main;
        
        public ParticleSystem ParticleSystem => _particleSystem;
        
        private void Awake()
        {
            _main = _particleSystem.main;
        }

        public void Play()
        {
            _main.loop = true;
            _particleSystem.Play();
        }

        public void Stop()
        {
            _main.loop = false;
        }
    }
}