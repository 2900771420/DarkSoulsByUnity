using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace LostLight
{
    public class PlayerLocomotion : MonoBehaviour
    {

        PlayerManager playerManager;
        Transform cameraObject;
        InputHandler inputHandler;
        public Vector3 moveDirection;

        [HideInInspector]
        public Transform myTransform;
        [HideInInspector]
        public AnimatorHandler animatorHandler;

        public new Rigidbody rigidbody;
        public GameObject normalCamera;
        [Header("Ground & Air Detection Stats")]
        [SerializeField]
        float groundDetectionRayStartPoint = 0.5f;
        [SerializeField]
        float minimumDistanceNeededToBeginFall = 1f;
        [SerializeField]
        float groundDirectionRayDistance = 0.2f;
        LayerMask ignoreForGroundCheck;
        public float inAirTimer;


        [Header("Movement Status")]
        [SerializeField]
        public float movementSpeed = 9;
        [SerializeField]
        public float walkingSpeed = 5;
        [SerializeField]
        public float sprintSpeed = 15;
        [SerializeField]
        public float rotationSpeed = 12;
        [SerializeField]
        public float fallingSpeed = 20;



        void Start()
        {
            playerManager = GetComponent<PlayerManager>();
            rigidbody = GetComponentInChildren<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            cameraObject = Camera.main.transform;
            myTransform = transform;
            animatorHandler.Initialize();

            playerManager.isGrounded = true;
            // playerManager.isInAir = false;
            ignoreForGroundCheck = ~(1 << 8 | 1 << 11 | 1 << 10);

            Physics.gravity = new Vector3(0, -15f, 0);

        }


        //public void FixedUpdate()
        //{
        //    float delta = Time.deltaTime;
        //    // 暂时
        //    animatorHandler.OnAnimatorMoveOverWrite(delta);
        //}

        #region Movement
        Vector3 normalVector;
        Vector3 targetPosition;

        // 实现玩家向输入方向转动
        public void HandleRotation(float delta)
        {
            if (playerManager.isInteracting)
                return;
            Vector3 targetDir = Vector3.zero;
            float moveOverride = inputHandler.moveAmount;

            targetDir = cameraObject.forward * inputHandler.vertical;
            targetDir += cameraObject.right * inputHandler.horizontal;

            targetDir.Normalize();
            targetDir.y = 0;

            if (targetDir == Vector3.zero)
                targetDir = myTransform.forward;

            float rs = rotationSpeed;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRoation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

            myTransform.rotation = targetRoation;
        }

        public void HandleMovement(float delta)
        {
            if (playerManager.isInteracting)
                return;

            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = movementSpeed;

            if (inputHandler.sprintFlag && inputHandler.moveAmount > 0.5)
            {
                speed = sprintSpeed;
                playerManager.isSprinting = true;
                moveDirection *= speed;

            }
            else
            {
                if(inputHandler.moveAmount < 0.5)
                {
                    moveDirection *= walkingSpeed;
                    playerManager.isSprinting = false;
                }
                else
                {
                    moveDirection *= speed;
                    playerManager.isSprinting = false;
                }


            }

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rigidbody.velocity = projectedVelocity;

            animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0, playerManager.isSprinting);

            if (animatorHandler.canRotate)
                HandleRotation(delta);
        }

        public void HandleRollingAndSprinting(float delta)
        {
            if (animatorHandler.anim.GetBool("isInteracting"))
                return;
            if (inputHandler.rollFlag)
            {
                moveDirection = cameraObject.forward * inputHandler.vertical;
                moveDirection += cameraObject.right * inputHandler.horizontal;

                if(inputHandler.moveAmount >= 0)
                {
                    Debug.Log("方向键 + 翻滚");
                    animatorHandler.PlayTargetAnimation("rolling", true);
                    moveDirection.y = 0;
                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = rollRotation;
                }
                else
                {
                    Debug.Log("无方向键 + 后撤");
                    // 如果没有按方向键 则后撤步 暂时没有实现
                    animatorHandler.PlayTargetAnimation("", true);
                }
            }
        }



        [Header("Fall Tuning")]
        [SerializeField] public float fallGravityMultiplier = 30f;
        [SerializeField] public float maxFallSpeed = 350f;

        [Header("References")]
        public Transform groundCheckPoint;

        [Header("Ground Check (Flat Capsule)")]
        public float groundCheckRadius = 0.3f;   // 圆柱半径（宽）
        public float groundCheckHeight = 0.2f;   // 圆柱高度（扁）
        public LayerMask groundLayer;


        public void HandleFalling(float delta, Vector3 moveDirection)
        {
            bool wasGrounded = playerManager.isGrounded;

            // 1. 使用扁平 Capsule 进行触地检测
            bool isGroundedNow = CheckGroundedCapsule();
            playerManager.isGrounded = isGroundedNow;
            //Debug.Log("当前落地状态" + isGroundedNow);

            // 2. 接地 / 离地状态切换
            if (!wasGrounded && isGroundedNow)
            {
                //Debug.Log("刚刚落地" + isGroundedNow);
                // 刚刚落地
                if (inAirTimer > 0.3f && inAirTimer <= 1f)
                {
                    animatorHandler.PlayTargetAnimation("landing", true);
                }
                else if (inAirTimer > 1f)
                {
                    animatorHandler.PlayTargetAnimation("rolling", true);
                }
                else
                {
                    animatorHandler.PlayTargetAnimation("Normal", false);
                }

                inAirTimer = 0f;
                playerManager.isInAir = false;
            }
            else if (wasGrounded && !isGroundedNow)
            {
                // 刚刚离地
                //Debug.Log("刚刚离地" + isGroundedNow);


                playerManager.isInAir = true;
                inAirTimer = 0f;
            }
            else
            {
                //Debug.Log("既没有刚刚离地，又没有刚刚落地");
            }

            // 3. 空中状态
            if (!playerManager.isGrounded)
            {
                inAirTimer += delta;

                // 增强下落重力
                rigidbody.AddForce(
                    Physics.gravity * fallGravityMultiplier,
                    ForceMode.Acceleration
                );
                if (inAirTimer > 0.3f)
                {
                    animatorHandler.PlayTargetAnimation("falling", false);
                }
            }

            // 4. 限制最大下落速度
            Vector3 v = rigidbody.velocity;
            float maxFallSpeed = 50f;
            if (v.y < -maxFallSpeed)
            {
                v.y = -maxFallSpeed;
                rigidbody.velocity = v;
            }
        }

        // ==============================
        // 扁平 Capsule 触地检测
        // ==============================
        private bool CheckGroundedCapsule()
        {
            Vector3 center = groundCheckPoint.position;

            Vector3 point1 = center + Vector3.up * (groundCheckHeight * 0.5f);
            Vector3 point2 = center - Vector3.up * (groundCheckHeight * 0.5f);

            bool grounded = Physics.CheckCapsule(
                point1,
                point2,
                groundCheckRadius,
                ignoreForGroundCheck,
                QueryTriggerInteraction.Ignore
            );

            // Debug：红色显示检测区域
            DrawCapsule(point1, point2, groundCheckRadius, Color.red);

            return grounded;
        }

        // ==============================
        // Debug 绘制 Capsule（红色）
        // ==============================
        private void DrawCapsule(Vector3 p1, Vector3 p2, float radius, Color color)
        {
            // 上圆
            Debug.DrawLine(p1 + Vector3.forward * radius, p1 - Vector3.forward * radius, color);
            Debug.DrawLine(p1 + Vector3.right * radius, p1 - Vector3.right * radius, color);

            // 下圆
            Debug.DrawLine(p2 + Vector3.forward * radius, p2 - Vector3.forward * radius, color);
            Debug.DrawLine(p2 + Vector3.right * radius, p2 - Vector3.right * radius, color);

            // 侧边连接线
            Debug.DrawLine(p1 + Vector3.forward * radius, p2 + Vector3.forward * radius, color);
            Debug.DrawLine(p1 - Vector3.forward * radius, p2 - Vector3.forward * radius, color);
            Debug.DrawLine(p1 + Vector3.right * radius, p2 + Vector3.right * radius, color);
            Debug.DrawLine(p1 - Vector3.right * radius, p2 - Vector3.right * radius, color);
        }




        #endregion



    }
}

