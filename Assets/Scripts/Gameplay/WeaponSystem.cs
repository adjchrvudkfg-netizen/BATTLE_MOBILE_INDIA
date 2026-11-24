using UnityEngine;

namespace BattleMobileIndia.Gameplay
{
    public class WeaponSystem : MonoBehaviour
    {
        [Header("Weapon Stats")]
        public string weaponName = "AR_M4";
        public float fireRate = 0.12f;
        public float damage = 24f;
        public float range = 120f;
        public int magazineSize = 30;
        public int currentAmmo = 30;
        public int reserveAmmo = 90;

        [Header("Firing References")]
        public Transform muzzlePoint;
        public Camera aimCamera;
        public Animator playerAnimator;
        public ParticleSystem muzzleFlash;

        private float _nextFireTime;

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                TryFire();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }
        }

        public void TryFire()
        {
            if (Time.time < _nextFireTime)
            {
                return;
            }

            if (currentAmmo <= 0)
            {
                Reload();
                return;
            }

            _nextFireTime = Time.time + fireRate;
            currentAmmo--;

            if (muzzleFlash != null)
            {
                muzzleFlash.Play();
            }

            if (playerAnimator != null)
            {
                playerAnimator.SetTrigger("Shoot");
            }

            Ray ray = aimCamera != null
                ? aimCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f))
                : new Ray(muzzlePoint != null ? muzzlePoint.position : transform.position, transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, range))
            {
                if (hit.collider.TryGetComponent<IDamageable>(out var target))
                {
                    target.TakeDamage(damage);
                }
            }
        }

        public void Reload()
        {
            int needed = magazineSize - currentAmmo;
            if (needed <= 0 || reserveAmmo <= 0)
            {
                return;
            }

            int toLoad = Mathf.Min(needed, reserveAmmo);
            currentAmmo += toLoad;
            reserveAmmo -= toLoad;
        }
    }

    public interface IDamageable
    {
        void TakeDamage(float amount);
    }
}

