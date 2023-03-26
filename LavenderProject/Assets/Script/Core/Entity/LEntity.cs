using UnityEngine;

namespace Lavender
{
    /// <summary>
    /// 实体类，用于表示游戏中的各种实体（例如角色、怪物等）
    /// </summary>
    public class LEntity : LGameObject
    {
        private Quaternion rotation; // 当前旋转角度

        public LEntityConfig Config; // 实体配置文件，保存实体的基本属性和参数
        public GameObject Model { get; set; } // 实体的模型对象，用于显示实体在游戏中的外观

        public Vector3 Position => Root.transform.position; // 实体当前的位置
        public Quaternion Rotation
        {
            get => rotation;
            set
            {
                rotation = value;
                Model.transform.rotation = Quaternion.Lerp(Model.transform.rotation, rotation,
                    Time.deltaTime * 10); // 插值更新朝向，使角色平滑转向
            }
        }

        public Vector3 Forward => rotation * Vector3.forward; // 实体当前的前方向量

        /// <summary>
        /// 初始化实体参数
        /// </summary>
        public override void InitParams()
        {
            base.InitParams();
        }
    }
}
