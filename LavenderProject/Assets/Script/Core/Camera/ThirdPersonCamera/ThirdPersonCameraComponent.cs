using Lavender.UnityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Lavender
{
    public class ThirdPersonCameraComponent : LComponent
    {
        private Vector3 offsetVector;
        private Transform targetTrans;
        private Transform cameraTrans;


        private readonly float MoveRate = 0.5f;
        private readonly float ANGLE_CONVERTER = Mathf.PI / 180;
        //相机上下的最大最小角度
        private readonly float MAX_ANGLE_Y = 80;
        private readonly float MIN_ANGLE_Y = 10;

        public Camera Camera { get; set; }
        public GameObject CameraHoster { get; set; }

        public float Distance { get; set; } = 5;
        public float OffsetAngleX { get; set; } = 0;
        public float OffsetAngleY { get; set; } = 45;

        public bool IsRotating = true;
        public Vector3 OffsetVector
        {
            get 
            {
                offsetVector.y = Distance * Mathf.Sin(OffsetAngleY * ANGLE_CONVERTER);
                float newRadius = Distance * Mathf.Cos(OffsetAngleY * ANGLE_CONVERTER);
                offsetVector.x = newRadius * Mathf.Sin(OffsetAngleX * ANGLE_CONVERTER);
                offsetVector.z = -newRadius * Mathf.Cos(OffsetAngleX * ANGLE_CONVERTER);
                return offsetVector;
            }
        }
        public Transform TargetTrans { get; private set; }
        public Transform CameraTrans { get; private set; }
        public override void OnAttach(LEntity entity)
        {
            base.OnAttach(entity);
            if (CameraHoster == null)
            {
                CameraHoster = new GameObject();
                CameraHoster.transform.SetParent(Entity.Root.transform);
            }
            if (CameraHoster.GetComponent<Camera>() == null)
            {
                CameraHoster.AddComponent<Camera>();
            }
            Camera = CameraHoster.GetComponent<Camera>();
            CameraTrans = Camera.transform;
            TargetTrans = Entity.Root.transform;

        }

        public override void Update(float delta)
        {
            base.Update(delta);
        }

        public void Rotate(float x, float y)
        {
            if (x != 0)
            {
                OffsetAngleX += x;
            }
            if (y != 0)
            {
                OffsetAngleY += y;
                OffsetAngleY = OffsetAngleY > MAX_ANGLE_Y ? MAX_ANGLE_Y : OffsetAngleY;
                OffsetAngleY = OffsetAngleY < MIN_ANGLE_Y ? MIN_ANGLE_Y : OffsetAngleY;
            }
        }

        public void ResetCamera()
        {
            CameraTrans.position = TargetTrans.position + OffsetVector;
            CameraTrans.LookAt(TargetTrans);
        }

        public void GetInput(float x, float y)
        {
            if (IsRotating)
            {
                Rotate(-x, -y);
            }
            ResetCamera();
        }
    }
}
