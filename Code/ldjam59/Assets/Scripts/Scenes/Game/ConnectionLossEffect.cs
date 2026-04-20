using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
	[ExecuteInEditMode]
	public class ConnectionLossEffect : MonoBehaviour
	{
		[Header("Static / Noise")]
		[Range(0f, 1f)] public float staticIntensity = 0.15f;
		[Range(0f, 1f)] public float staticSpeed = 0.8f;

		[Header("Scanlines")]
		[Range(0f, 1f)] public float scanlineIntensity = 0.25f;
		public float scanlineCount = 300f;

		[Header("Signal Distortion")]
		[Range(0f, 1f)] public float jitterAmount = 0.04f;
		[Range(0f, 1f)] public float rollSpeed = 0.1f;

		[Header("Vignette")]
		[Range(0f, 2f)] public float vignetteStrength = 1.2f;

		[Header("Connection Settings")]
		public float transitionSpeed = 2f;
		public bool connectedByDefault = true;

        public static ConnectionLossEffect Instance { get; private set; }
		public float CurrentBlend {  get; private set; }

		private float _targetBlend;

        private void Start()
        {
			CurrentBlend = connectedByDefault ? 1f : 0f;
			_targetBlend = CurrentBlend;

			var shader = Shader.Find("Hidden/ConnectionLoss");
			if (shader != null)
			{
				Debug.Log("Shader found OK");
			} else
			{
				Debug.LogError("ConnectionLoss shader not found");
			}
        }

        public void SetConnected(bool connected) => _targetBlend = connected ? 1f : 0f;

		public void SetSignalQuality(float quality) => _targetBlend = Mathf.Clamp01(quality);

        public bool IsEffectActive => CurrentBlend < 1f || _targetBlend < 1f;

        private void OnEnable() { Instance = this; }
        private void OnDisable() { if (Instance == this) Instance = null; }

        private void Update()
        {
            CurrentBlend = Mathf.MoveTowards(CurrentBlend, _targetBlend, transitionSpeed * Time.deltaTime);
        }
    }
}
