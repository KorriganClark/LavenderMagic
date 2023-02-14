using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System;

namespace Lavender
{

    public enum MoveState
    {
        Idle,
        Walk,
        Run,
        Rush,
        Jump,
        Fall,
    }

    [Serializable]
    public class LavenderCharacterControl : MonoBehaviour
    {
        [SerializeField]
        public LavenderCharacterConfig characterConfig;
        [SerializeField]
        private GameObject camObj;
        private CameraController cameraController;
        private CharacterController moveController;
        private LavenderAnimComponent animComp;
        public GameObject characterModel;
        public MoveState moveState = MoveState.Idle;
        public bool canMove = true;
        void Start()
        {
            moveController = GetComponent<CharacterController>();
            if (moveController == null)
            {
                moveController = gameObject.AddComponent<CharacterController>();
            }
            animComp = GetComponent<LavenderAnimComponent>();
            cameraController = camObj.GetComponent<CameraController>();
        }

        void Update()
        {
            DealPlayerInput();
        }

        private void DealPlayerInput()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                characterConfig.MoveModeSwitch();
            }
            TryToMove();
        }

        private void TryToMove()
        {
            if (canMove == false)
            {
                if (moveState == MoveState.Idle)
                {
                    animComp.PlayAnim(characterModel.GetComponent<Animator>(), 0);
                    moveState = MoveState.Idle;
                }
                return;
            }
                
            Vector3 move = camObj.transform.forward * Input.GetAxis("Vertical") + camObj.transform.right * Input.GetAxis("Horizontal");
            move.y = 0;
            Vector3 dir = move.normalized * characterConfig.Speed * Time.deltaTime;
            moveController.Move(dir);
            
            if(dir.sqrMagnitude != 0)
            {
                characterModel.transform.forward = move.normalized;
            }

            if (dir.sqrMagnitude != 0 && moveState == MoveState.Idle)
            {
                animComp.PlayAnim(characterModel.GetComponent<Animator>(), 1);
                moveState = MoveState.Walk;
                characterModel.transform.forward = move.normalized;
            }
            else if(dir.sqrMagnitude * 100 == 0 && moveState == MoveState.Walk)
            {
                animComp.PlayAnim(characterModel.GetComponent<Animator>(), 0);
                moveState = MoveState.Idle;
            }
        }

    }

    [Serializable]
    public class LavenderCharacterConfig
    {
        [SerializeField]
        private float speed = 5, jumpAbility = 1;
        [SerializeField]
        private int level = 1, attack = 1;
        [SerializeField]
        private int moveMode = 0;

        public int MoveMode
        {
            get
            {
                return moveMode;
            }
            set
            {
                moveMode = value;
            }
        }

        public float Speed
        {
            get
            {
                return speed;
            }
            set
            {
                speed = value;
            }
        }

        public float JumpHigh
        {
            get
            {
                return jumpAbility;
            }
            set
            {
                jumpAbility = value;
            }
        }

        public void MoveModeSwitch()
        {
            if (moveMode == 0)
            {
                moveMode = 1;
            }
            else
            {
                moveMode = 0;
            }
        }
    }
}

