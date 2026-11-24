using UnityEngine;

namespace BattleMobileIndia.Gameplay
{
    public class SafeZoneController : MonoBehaviour
    {
        [Header("Zone Shrink Settings")]
        public Transform zoneTransform;
        public float startDelaySeconds = 30f;
        public float shrinkDurationSeconds = 45f;
        public float targetScaleMultiplier = 0.25f;
        public float damagePerSecondOutside = 5f;

        private float _timer;
        private bool _shrinking;
        private Vector3 _initialScale;
        private Vector3 _targetScale;

        private void Start()
        {
            if (zoneTransform == null)
            {
                zoneTransform = transform;
            }

            _initialScale = zoneTransform.localScale;
            _targetScale = _initialScale * targetScaleMultiplier;
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (!_shrinking && _timer >= startDelaySeconds)
            {
                _shrinking = true;
                _timer = 0f;
            }

            if (_shrinking && shrinkDurationSeconds > 0f)
            {
                float t = Mathf.Clamp01(_timer / shrinkDurationSeconds);
                zoneTransform.localScale = Vector3.Lerp(_initialScale, _targetScale, t);
            }
        }

        public bool IsOutsideZone(Vector3 worldPos)
        {
            float radius = zoneTransform.localScale.x * 0.5f;
            Vector2 centerXZ = new Vector2(zoneTransform.position.x, zoneTransform.position.z);
            Vector2 posXZ = new Vector2(worldPos.x, worldPos.z);
            return Vector2.Distance(centerXZ, posXZ) > radius;
        }
    }
}

