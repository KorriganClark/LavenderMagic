
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Lavender
{
    public class LMonsterShowInfoComponent : LComponent
    {
        private Image bloodImage;

        public LMonster Monster { get { return GO as LMonster; } }
        public LMonsterConfig MonsterConfig { get { return Monster.Config as LMonsterConfig; } }
        public static GameObject PanelAsset;
        public GameObject Panel { get; set; }
        public LAttrComponent AttrComponent { get { return Monster.GetComponent<LAttrComponent>(); } }
        public Camera mainCamera;
        public float BloodValue 
        {
            get
            {
                return bloodImage.fillAmount;
            }
            set 
            { 
                bloodImage.fillAmount = value;
            } 
        }
        public override void OnAttach(LGameObject go)
        {
            base.OnAttach(go);
            mainCamera = LCharacterControl.Instance.Character.GetComponent<ThirdPersonCameraComponent>().Camera;
            if (PanelAsset == null )
            {
                PanelAsset = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Artres/Prefabs/Hud/MonsterInfoPanel.prefab");
            }
            Panel = GameObject.Instantiate(PanelAsset);
            Panel.transform.SetParent(Monster.Root.transform);
            Panel.transform.localPosition = new Vector3(0, 1.62f, 0);
            bloodImage = Panel.transform.Find("Canvas/Blood").GetComponent<Image>();
        }

        public override void Update(float delta)
        {
            base.Update(delta);
            if(Panel == null || mainCamera == null)
            {
                return;
            }
            Panel.transform.LookAt(Panel.transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
            if (AttrComponent.GetAttr(EAttrType.MaxHP)<= 0)
            {
                return;
            }
            var value = AttrComponent.GetAttr(EAttrType.HP) * 1.0f / AttrComponent.GetAttr(EAttrType.MaxHP);
            BloodValue = value;
        }
    }
}
