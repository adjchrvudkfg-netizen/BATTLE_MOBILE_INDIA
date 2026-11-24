using UnityEngine;
using UnityEngine.UI;

namespace BattleMobileIndia.Gameplay
{
    [RequireComponent(typeof(CharacterController))]
    public class MobileBattleControls : MonoBehaviour
    {
        [Header("Movement & Physics")]
        public float moveSpeed = 7.5f;
        public float jumpHeight = 1.6f;
        public float gravity = -22f;

        [Header("Optional Mobile UI Buttons")]
        public Button jumpButton;
        public Button crouchButton;
        public Button fireButton;

        [Header("Combat / Weapon")]
        public Transform firePoint;
        public GameObject projectilePrefab;
        public float fireCooldown = 0.15f;

        private CharacterController controller;
        private Vector3 velocity;
        private float nextFireTime;
        private bool isCrouching;
        private float normalHeight;
        private float crouchHeight = 1.1f;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            normalHeight = controller.height > 0.5f ? controller.height : 2.0f;
        }

        private void Start()
        {
            if (jumpButton != null)
                jumpButton.onClick.AddListener(TriggerJump);

            if (crouchButton != null)
                crouchButton.onClick.AddListener(ToggleCrouch);

            if (fireButton != null)
                fireButton.onClick.AddListener(TriggerFire);
        }

        private void Update()
        {
            HandleMovement();

            if (Input.GetKey(KeyCode.Space))
                TriggerJump();

            if (Input.GetKeyDown(KeyCode.C))
                ToggleCrouch();

            if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.F))
                TriggerFire();
        }

        private void HandleMovement()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 moveDirection = (transform.right * horizontal + transform.forward * vertical);
            if (moveDirection.sqrMagnitude > 1f)
                moveDirection.Normalize();

            float speedMultiplier = isCrouching ? 0.55f : 1f;
            controller.Move(moveDirection * (moveSpeed * speedMultiplier) * Time.deltaTime);

            if (controller.isGrounded && velocity.y < 0f)
            {
                velocity.y = -2f;
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        public void TriggerJump()
        {
            if (controller.isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        public void ToggleCrouch()
        {
            isCrouching = !isCrouching;
            controller.height = isCrouching ? crouchHeight : normalHeight;
        }

        public void TriggerFire()
        {
            if (Time.time < nextFireTime) return;
            nextFireTime = Time.time + fireCooldown;

            if (projectilePrefab != null && firePoint != null)
            {
                Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            }
        }
    }
}
