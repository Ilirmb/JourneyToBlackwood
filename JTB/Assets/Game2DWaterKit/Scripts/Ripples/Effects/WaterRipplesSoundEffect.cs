namespace Game2DWaterKit.Ripples.Effects
{
    using System.Collections.Generic;
    using UnityEngine;

    public class WaterRipplesSoundEffect
    {
        #region Variables
        private readonly Transform _poolRootParent;

        private bool _isActive;
        private AudioClip _audioClip;
        private bool _canExpandPool;
        private int _poolSize;
        private bool _isUsingConstantAudioPitch;
        private float _audioPitch;
        private float _maximumAudioPitch;
        private float _minimumAudioPitch;
        private float _audioVolume;

        private Transform _poolRoot;
        private List<AudioSource> _pool;
        private int _activeAudioSourcesCount;
        private int _firstActiveAudioSourceIndex;
        private int _nextAudioSourceToActivateIndex;
        #endregion

        public WaterRipplesSoundEffect(WaterRipplesSoundEffectParameters parameters, Transform poolParent)
        {
            _isActive = parameters.IsActive;
            _audioClip = parameters.AudioClip;
            _isUsingConstantAudioPitch = parameters.UseConstantAudioPitch;
            _audioPitch = parameters.AudioPitch;
            _minimumAudioPitch = parameters.MinimumAudioPitch;
            _maximumAudioPitch = parameters.MaximumAudioPitch;
            _audioVolume = parameters.AudioVolume;
            _poolSize = parameters.PoolSize;
            _canExpandPool = parameters.CanExpandPool;

            _poolRootParent = poolParent;

            if (_isActive)
                CreatePool();
        }

        #region Properties
        public bool IsActive
        {
            get { return _isActive; }

            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    if (IsPoolCreated)
                    {
                        _poolRoot.gameObject.SetActive(_isActive);
                    }
                    else
                    {
                        if (_isActive)
                        {
                            CreatePool();
                        }
                    }
                }
            }
        }
        private bool IsPoolCreated { get { return _poolRoot != null; } }
        public AudioClip AudioClip
        {
            get { return _audioClip; }

            set
            {
                if (_audioClip != value)
                {
                    _audioClip = value;
                    SwapAudioClip();
                }
            }
        }
        public bool CanExpandPool { get { return _canExpandPool; } set { _canExpandPool = value; } }
        public int PoolSize { get { return _poolSize; } set { _poolSize = Mathf.Clamp(value, 0, int.MaxValue); } }
        public float AudioPitch { get { return _audioPitch; } set { _audioPitch = Mathf.Clamp(value, -3f, 3f); } }
        public float MaximumAudioPitch { get { return _maximumAudioPitch; } set { _maximumAudioPitch = Mathf.Clamp(value, -3f, 3f); } }
        public float MinimumAudioPitch { get { return _minimumAudioPitch; } set { _minimumAudioPitch = Mathf.Clamp(value, -3f, 3f); } }
        public bool IsUsingConstantAudioPitch { get { return _isUsingConstantAudioPitch; } set { _isUsingConstantAudioPitch = value; } }
        public float AudioVolume { get { return _audioVolume; } set { _audioVolume = Mathf.Clamp01(value); } }
        #endregion

        #region Methods

        public void Update()
        {
            if (IsPoolCreated && _activeAudioSourcesCount > 0)
            {
                //You could remove this check if you don't intend to change the poolSize property at runtime
                if (_poolSize > _pool.Count)
                    ExpandPool(_poolSize);

                AudioSource firstActiveAudioSource = _pool[_firstActiveAudioSourceIndex];
                if (!firstActiveAudioSource.isPlaying)
                {
                    firstActiveAudioSource.gameObject.SetActive(false);
                    _firstActiveAudioSourceIndex = (_firstActiveAudioSourceIndex + 1 < _pool.Count) ? _firstActiveAudioSourceIndex + 1 : 0;
                    _activeAudioSourcesCount--;
                }
            }
        }

        public void PlaySoundEffect(Vector3 position, float disturbanceFactor)
        {
            if (!_isActive || !IsPoolCreated)
                return;

            if (_activeAudioSourcesCount == _poolSize)
            {
                if (!_canExpandPool)
                    return;

                _nextAudioSourceToActivateIndex = _poolSize;
                ExpandPool(newPoolSize: _poolSize * 2);
            }

            AudioSource newlyActivatedAudioSource = _pool[_nextAudioSourceToActivateIndex];
            newlyActivatedAudioSource.transform.position = position;
            newlyActivatedAudioSource.gameObject.SetActive(true);
            newlyActivatedAudioSource.volume = _audioVolume;
            newlyActivatedAudioSource.pitch = _isUsingConstantAudioPitch ? _audioPitch : Mathf.Lerp(_minimumAudioPitch, _maximumAudioPitch, 1f - disturbanceFactor); ;
            newlyActivatedAudioSource.Play();

            _activeAudioSourcesCount++;
            _nextAudioSourceToActivateIndex = (_nextAudioSourceToActivateIndex + 1 < _pool.Count) ? _nextAudioSourceToActivateIndex + 1 : 0;
        }

        private void CreatePool()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
#endif

            if (IsPoolCreated || _audioClip == null || _poolSize < 1)
                return;

            _poolRoot = new GameObject("Sound Effects").transform;
            _poolRoot.parent = _poolRootParent;

            _pool = new List<AudioSource>(_poolSize);
            for (int i = 0; i < _poolSize; i++)
            {
                _pool.Add(CreateNewAudioSource());
            }
        }

        private void DestroyPool()
        {
            if (!IsPoolCreated)
                return;

            GameObject.Destroy(_poolRoot.gameObject);
            _poolRoot = null;
            _pool = null;

            _firstActiveAudioSourceIndex = 0;
            _nextAudioSourceToActivateIndex = 0;
            _activeAudioSourcesCount = 0;
        }

        private void ExpandPool(int newPoolSize)
        {
            if (!IsPoolCreated)
                return;

            _poolSize = newPoolSize;

            for (int i = _pool.Count, imax = newPoolSize; i < imax; i++)
            {
                _pool.Add(CreateNewAudioSource());
            }
        }

        private void SwapAudioClip()
        {
            if (!IsPoolCreated)
                return;

            for (int i = 0; i < _poolSize; i++)
            {
                _pool[i].clip = _audioClip;
            }
        }

        private AudioSource CreateNewAudioSource()
        {
            GameObject audioSourceGameObject = new GameObject("Sound Effect");
            audioSourceGameObject.transform.parent = _poolRoot;
            audioSourceGameObject.SetActive(false);

            AudioSource audioSource = audioSourceGameObject.AddComponent<AudioSource>();
            audioSource.clip = _audioClip;
            return audioSource;
        }

        #endregion

        #region Editor Only Methods
        #if UNITY_EDITOR
        public void Validate(WaterRipplesSoundEffectParameters parameters)
        {
            IsUsingConstantAudioPitch = parameters.UseConstantAudioPitch;
            AudioPitch = parameters.AudioPitch;
            MinimumAudioPitch = parameters.MinimumAudioPitch;
            MaximumAudioPitch = parameters.MaximumAudioPitch;
            AudioVolume = parameters.AudioVolume;
            CanExpandPool = parameters.CanExpandPool;
            AudioClip = parameters.AudioClip;
            PoolSize = parameters.PoolSize;
            IsActive = parameters.IsActive;
        }
        #endif
        #endregion
    }

    public struct WaterRipplesSoundEffectParameters
    {
        public bool IsActive;
        public AudioClip AudioClip;
        public float AudioPitch;
        public bool UseConstantAudioPitch;
        public float MaximumAudioPitch;
        public float MinimumAudioPitch;
        public float AudioVolume;
        public bool CanExpandPool;
        public int PoolSize;
    }

}
