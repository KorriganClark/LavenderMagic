namespace Lavender
{
    public enum EElementType
    {
        None,
        Pyro,   // 火
        Hydro,  // 水
        Anemo,  // 风
        Electro, // 雷
        Dendro, // 草
        Cryo,   // 冰
        Geo     // 岩
    }


    /// <summary>
    /// 元素Buff基类
    /// </summary>
    public abstract class BaseElementBuff : LBuff
    {
        public override BuffType TypeOfBuff => BuffType.Element;
        public override bool ApplyOnAttach => true;
        public abstract EElementType ElementType { get ; }
        public EAttrType[] ElementAmontType = new EAttrType[7] 
        { 
            EAttrType.PyroAmount,
            EAttrType.HydroAmount,
            EAttrType.AnemoAmount,
            EAttrType.ElectroAmount,
            EAttrType.DendroAmount,
            EAttrType.CryoAmount,
            EAttrType.GeoAmount,
        };
        public int Amount
        {
            get
            {
                return Entity.AttrComponent.GetAttr(ElementAmontType[(int)(ElementType - 1)]);
            }
            set
            {
                Entity.AttrComponent.SetAttr(ElementAmontType[(int)(ElementType - 1)], value);
            }
        }
    }
}
