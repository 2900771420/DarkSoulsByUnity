using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostLight
{
    public class RotateObject : MonoBehaviour
    {
        [Header("旋转设置")]
        [Tooltip("每秒旋转的角度（度/秒）")]
        public float rotationSpeed = 200f;

        [Tooltip("旋转轴")]
        public Vector3 rotationAxis = Vector3.up;

        [Tooltip("使用本地坐标还是世界坐标")]
        public bool useLocalSpace = false;

        [Header("旋转模式")]
        [Tooltip("是否启用旋转")]
        public bool isActive = true;

        [Tooltip("旋转模式")]
        public RotationMode rotationMode = RotationMode.Constant;

        public enum RotationMode
        {
            Constant,      // 恒定速度
            SineWave,      // 正弦波速度
            PingPong       // 往复旋转
        }

        [Header("正弦波设置")]
        [Tooltip("正弦波幅度")]
        public float amplitude = 45f;

        [Tooltip("正弦波频率")]
        public float frequency = 1f;

        [Header("往复旋转设置")]
        [Tooltip("最大旋转角度")]
        public float maxAngle = 90f;

        [Tooltip("往复速度")]
        public float pingPongSpeed = 1f;

        private float currentAngle = 0f;
        private float timeCounter = 0f;

        void Update()
        {
            if (!isActive) return;

            switch (rotationMode)
            {
                case RotationMode.Constant:
                    ConstantRotation();
                    break;

                case RotationMode.SineWave:
                    SineWaveRotation();
                    break;

                case RotationMode.PingPong:
                    PingPongRotation();
                    break;
            }
        }

        // 恒定速度旋转
        private void ConstantRotation()
        {
            float rotationAmount = rotationSpeed * Time.deltaTime;
            ApplyRotation(rotationAmount);
        }

        // 正弦波速度旋转
        private void SineWaveRotation()
        {
            timeCounter += Time.deltaTime * frequency;
            float currentSpeed = rotationSpeed + Mathf.Sin(timeCounter) * amplitude;
            float rotationAmount = currentSpeed * Time.deltaTime;
            ApplyRotation(rotationAmount);
        }

        // 往复旋转
        private void PingPongRotation()
        {
            float targetAngle = Mathf.PingPong(Time.time * pingPongSpeed, maxAngle * 2) - maxAngle;
            float rotationAmount = targetAngle - currentAngle;
            currentAngle = targetAngle;
            ApplyRotation(rotationAmount);
        }

        // 应用旋转
        private void ApplyRotation(float angle)
        {
            if (useLocalSpace)
            {
                transform.Rotate(rotationAxis, angle, Space.Self);
            }
            else
            {
                transform.Rotate(rotationAxis, angle, Space.World);
            }
        }

        // 开始旋转
        public void StartRotation()
        {
            isActive = true;
        }

        // 停止旋转
        public void StopRotation()
        {
            isActive = false;
        }

        // 设置旋转速度
        public void SetRotationSpeed(float speed)
        {
            rotationSpeed = speed;
        }

        // 切换旋转方向
        public void ToggleRotationDirection()
        {
            rotationSpeed = -rotationSpeed;
        }

        // 重置旋转
        public void ResetRotation()
        {
            transform.rotation = Quaternion.identity;
            currentAngle = 0f;
            timeCounter = 0f;
        }
    }
}

