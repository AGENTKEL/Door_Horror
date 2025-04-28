using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CharacterScript
{
    [RequireComponent(typeof(CharacterController))]
    public class FPSController : MonoBehaviour
    {
        public float walkingSpeed = 7.5f;
        public float runningSpeed = 11.5f;
        public float jumpSpeed = 8.0f;
        public float gravity = 20.0f;
        public Camera playerCamera;
        public float lookSpeed = 2.0f;
        public float lookXLimit = 45.0f;

        CharacterController characterController;
        public Joystick joystick;
        Vector3 moveDirection = Vector3.zero;
        float rotationX = 0;

        [HideInInspector]
        public bool canMove = true;

        public bool isDead = false;
        
        public bool useMouseLook = true;

        private bool jumpRequest = false; // <- new: flag for jump button
        
        public List<AudioClip> footstepClips; // List of random footstep sounds
        [SerializeField] private AudioSource audioSource;
        private float stepTimer = 0f;
        public float stepInterval = 0.4f; // How often to play footstep sounds

        void Start()
        {
            characterController = GetComponent<CharacterController>();

#if UNITY_ANDROID || UNITY_IOS
            useMouseLook = false;
#endif
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
        }

        void Update()
        {
            if (isDead) { return; }

            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float inputVertical = joystick != null && (joystick.Vertical != 0) ? joystick.Vertical : Input.GetAxis("Vertical");
            float inputHorizontal = joystick != null && (joystick.Horizontal != 0) ? joystick.Horizontal : Input.GetAxis("Horizontal");

            float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * inputVertical : 0;
            float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * inputHorizontal : 0;
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            // Jump logic
            if ((Input.GetButton("Jump") || jumpRequest) && canMove && characterController.isGrounded)
            {
                moveDirection.y = jumpSpeed;
                jumpRequest = false; // reset after jump
            }
            else
            {
                moveDirection.y = movementDirectionY;
            }

            if (!characterController.isGrounded)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }

            characterController.Move(moveDirection * Time.deltaTime);
            
            bool isMoving = Mathf.Abs(inputVertical) > 0.1f || Mathf.Abs(inputHorizontal) > 0.1f;

            if (characterController.isGrounded && isMoving)
            {
                stepTimer += Time.deltaTime;
                if (stepTimer > stepInterval)
                {
                    PlayFootstep();
                    stepTimer = 0f;
                }
            }
            else
            {
                stepTimer = 0f; // Reset timer when not moving or not grounded
            }

            // Mouse look
            if (canMove && useMouseLook)
            {
                rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            }
        }
        
        private void PlayFootstep()
        {
            if (footstepClips.Count == 0) return;

            int index = Random.Range(0, footstepClips.Count);
            audioSource.PlayOneShot(footstepClips[index]);
        }

        // --- NEW: public method to call from UI Button ---
        public void JumpButton()
        {
            if (characterController.isGrounded)
            {
                jumpRequest = true;
            }
        }
    }
}