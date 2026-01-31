using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LostLight
{
    public class AnimatorHandler : MonoBehaviour
    {
        PlayerManager playerManager;
        public Animator anim;
        public InputHandler inputHandler;
        public PlayerLocomotion playerLocomotion;
        int vertical;
        int horizontal;
        public bool canRotate;

        public void Initialize()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            anim = GetComponent<Animator>();
            inputHandler = GetComponentInParent<InputHandler>();
            playerLocomotion = GetComponentInParent<PlayerLocomotion>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
        {
            #region Vertical
            float v = 0f;
            if (verticalMovement > 0f && verticalMovement < 0.55f)
            {
                v = 0.5f;
            }
            else if (verticalMovement >= 0.55f)
            {
                v = 1f;
            }
            else if (verticalMovement < 0 && verticalMovement > -0.55f)
            {
                v = -0.5f;
            }
            else if (verticalMovement <= -0.55f)
            {
                v = -1f;
            }
            else
            {
                v = 0;
            }
            #endregion

            #region Horizontal
            float h = 0f;
            if (horizontalMovement > 0f && horizontalMovement < 0.55f)
            {
                h = 0.5f;
            }
            else if (horizontalMovement >= 0.55f)
            {
                h = 1f;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                h = -0.5f;
            }
            else if (horizontalMovement <= -0.55f)
            {
                h = -1f;
            }
            else
            {
                h = 0;
            }
            //float v, h;
            //if(verticalMovement > 1)
            //{
            //    v = 1;
            //}else if(verticalMovement < -1)
            //{
            //    v = -1;
            //}
            //else
            //{
            //    v = verticalMovement;
            //}
            //if (horizontalMovement > 1)
            //{
            //    h = 1;
            //}
            //else if (horizontalMovement < -1)
            //{
            //    h = -1;
            //}
            //else
            //{
            //    h = horizontalMovement;
            //}
            #endregion

            if(isSprinting)
            {
                v = 2;
                h = horizontalMovement;
            }

            anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
        }

        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("isInteracting", isInteracting);
            anim.CrossFade(targetAnim, 0.2f);
        }

        public void CanRoate()
        {
            canRotate = true;
        }
        public void StopRotation()
        {
            canRotate = false;
        }

        // 基于动画驱动角色移动控制
        public void OnAnimatorMove()
        {
            //Debug.Log("提示：inputHandler");
            if (playerManager.isInteracting == false)
            {
                //Debug.Log("提示：inputHandler.isInteracting == false");
                return;
            }
            //Debug.Log("提示：inputHandler.isInteracting == true");
            float delta = Time.deltaTime;
            playerLocomotion.rigidbody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            playerLocomotion.rigidbody.velocity = velocity;
            return;
        }
    }
}

