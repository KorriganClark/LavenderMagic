using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System;

namespace Lavender
{
    [Serializable]
    public class LavenderCharacterControl : MonoBehaviour
    {
        [SerializeField]
        public LavenderCharacterConfig characterConfig;
        [SerializeField]
        public GameObject camObj;
        public CameraController cameraController;
        private CharacterController moveController;
        private LavenderAnimComponent animComp;
        public GameObject characterModel;
        public string characterstate = "idle";
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
                if (characterstate == "walk")
                {
                    animComp.PlayAnim(characterModel.GetComponent<Animator>(), 0);
                    characterstate = "idle";
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

            if (dir.sqrMagnitude != 0 && characterstate == "idle")
            {
                animComp.PlayAnim(characterModel.GetComponent<Animator>(), 1);
                characterstate = "walk";
                characterModel.transform.forward = move.normalized;
            }
            else if(dir.sqrMagnitude * 100 == 0 && characterstate == "walk")
            {
                animComp.PlayAnim(characterModel.GetComponent<Animator>(), 0);
                characterstate = "idle";
            }
        }

        public void CameraMoveWithMonsterPos()
        {
            cameraController.MoveTo(22, 20);
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

