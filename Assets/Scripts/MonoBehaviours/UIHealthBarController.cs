using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MonoBehaviours
{
	[RequireComponent(typeof(Image))]
	// ReSharper disable once ClassNeverInstantiated.Global
	public class UIHealthBarController : MonoBehaviour
	{
		private const float SPEED_F = 2f;

		private int _progressHash;
		private int _fillerHash;

		private Image _image;
		private Material _material;
		private float _current;
		private float _target;
		private Coroutine _updateCoroutine;

		public void SetForLocal(float startNormalized)
		{
			_current = 0f;
			_target = startNormalized;
			_image.enabled = true;
			_material.SetColor(_fillerHash, Color.red);
			_material.SetFloat(_progressHash, _target);
			SetValue(_target);

			Debug.Log(string.Format("<color=blue>set for local {0}</color>", name));
		}

		public void SetFroRemote(float startNormalized)
		{
			_current = 0f;
			_target = startNormalized;
			_image.enabled = true;
			_material.SetColor(_fillerHash, Color.blue);
			_material.SetFloat(_progressHash, _target);
			SetValue(_target);

			Debug.Log(string.Format("<color=blue>set for remote {0}</color>", name));
		}

		public void SetForNobody()
		{
			_current = 0f;
			_target = .5f;
			_image.enabled = false;
		}

		public void SetValue(float valueNormalized)
		{
			_target = Mathf.Clamp01(valueNormalized);
			_updateCoroutine = _updateCoroutine ?? StartCoroutine(UpdateCoroutine());

			Debug.Log(string.Format("<color=blue>updating health to {0:#.##} for local {1}</color>", _target, name));
		}

		private IEnumerator UpdateCoroutine()
		{
			while(enabled)
			{
				if(Mathf.Approximately (_current, _target))
				{
					_updateCoroutine = null;
					yield break;
				}
				_current += (_target - _current) * SPEED_F * Time.deltaTime;
				_material.SetFloat(_progressHash, _current);
				yield return new WaitForEndOfFrame();
			}
		}

		// ReSharper disable once UnusedMember.Local
		private void Awake()
		{
			_progressHash = Shader.PropertyToID("_Progress");
			_fillerHash = Shader.PropertyToID("_FillColor");

			_image = GetComponent<Image>();
			_material = Instantiate(_image.material);
			_image.material = _material;
			SetForNobody();
		}

		// ReSharper disable once UnusedMember.Local
		private void OnDestroy()
		{
			if(_material != null)
				DestroyImmediate(_material, true);
		}
	}
}
