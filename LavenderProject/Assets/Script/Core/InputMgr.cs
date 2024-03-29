﻿using UnityEngine;

namespace Lavender
{
    public class InputMgr : LSingleton<InputMgr>
    {
        private CharacterPCInput characterPCInput;

        public ThirdPersonCameraComponent Camera { get; set; }
        public LCharacterControl CharacterControl { get; set; }

        public void Update()
        {
            if(Camera != null)
            {
                float x = Input.GetAxis("Mouse X");
                float y = Input.GetAxis("Mouse Y");
                Camera.GetInput(x, y);
            }
            if(CharacterControl != null)
            {
                characterPCInput.ForwadAndBackInput = Input.GetAxis("Vertical");
                characterPCInput.LeftAndRightInput = Input.GetAxis("Horizontal");
                characterPCInput.JumpPressInput = Input.GetButtonDown("Jump");
                characterPCInput.MouseLeftClick = Input.GetMouseButtonDown(0);
                CharacterControl.DealPlayerInput(characterPCInput);
            }
        }
    }

    public struct CharacterPCInput
    {
        public float ForwadAndBackInput;
        public float LeftAndRightInput;
        public bool EPressInput;
        public bool QPressInput;
        public bool JumpPressInput;
        public bool MouseLeftClick;
    }

}
