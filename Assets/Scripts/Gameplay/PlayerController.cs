using UnityEngine;

namespace BattleMobileIndia.Gameplay
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Speeds")]
        public float walkSpeed = 6f;
        public float sprintSpeed = 9.5f;
        public float crouchSpeed = 3.2f;
        public float jumpHeight = 1.6f;
        public float gravity = -22f;

        [Header("Optional Joystick Input (x, y from -1 to 1)")]
        public Vector2 moveInput;
        public bool isSprinting;
        public bool isCrouching;

        [Header("Optional Animator")]
        public Animator animator;

        private CharacterController _controller;
        private Vector3 _velocity;
        private float _standingHeight;
        public float crouchHeight = 1.1f;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _standingHeight = _controller.height;
        }

        private void Update()
        {
            float h = moveInput.x != 0f || moveInput.y != 0f ? moveInput.x : Input.GetAxis("Horizontal");
            float v = moveInput.x != 0f || moveInput.y != 0f ? moveInput.y : Input.GetAxis("Vertical");

            if (Input.GetKeyDown(KeyCode.C))
            {
                ToggleCrouch();
            }

            bool wantSprint = Input.GetKey(KeyCode.LeftShift) || isSprinting;
            float speed = isCrouching ? crouchSpeed : (wantSprint ? sprintSpeed : walkSpeed);

            Vector3 moveDir = (transform.right * h + transform.forward * v);
            if (moveDir.magnitude > 1f)
            {
                moveDir.Normalize();
            }

            _controller.Move(moveDir * speed * Time.deltaTime);

            if (_controller.isGrounded && _velocity.y < 0f)
            {
                _velocity.y = -2f;
            }

            if (Input.GetButtonDown("Jump") && _controller.isGrounded)
            {
                _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            _velocity.y += gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);

            if (animator != null)
            {
                animator.SetFloat("Speed", moveDir.magnitude * speed);
                animator.SetBool("IsGrounded", _controller.isGrounded);
                animator.SetBool("IsCrouching", isCrouching);
            }
        }

        public void ToggleCrouch()
        {
            isCrouching = !isCrouching;
            _controller.height = isCrouching ? crouchHeight : _standingHeight;
        }

        public void TriggerJump()
        {
            if (_controller.isGrounded)
            {
                _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }
    }
}

