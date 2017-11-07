//using Controllers;

namespace LuaSupport.ClientLuaData
{
    //    public class ClientConstLuaData : ILuaTableReader
    //    {
    //public const LuaTableType TableType = LuaTableType.ClientConst;
    //public static ClientConstLuaData Inst
    //{
    //    get { return LuaDataTableProxy.Inst[TableType] as ClientConstLuaData; }
    //}
    //public class KeyDesc : LuaTableKeyValue
    //{
    //    public override int Count { get { return 1; } }
    //    public override bool Init(StkId[] ps)
    //    {
    //        if (!LuaDataReadHelper.ReadNumber(ps[0].V, out id))
    //            return false;
    //        mDatas = new object[] { id };
    //        return true;
    //    }
    //    public override int GetHashCode()
    //    {
    //        string onlyKey = "Activity" + id.ToString();
    //        int hashCode = onlyKey.GetHashCode();
    //        return hashCode;
    //    }
    //    internal static KeyDesc GenKey(int id)
    //    {
    //        KeyDesc k = new KeyDesc();
    //        k.id = id;
    //        k.mDatas = new object[] { k.id };
    //        return k;
    //    }
    //    public override string ToString()
    //    {
    //        string onlyKey = "Activity" + id.ToString();
    //        int hashCode = onlyKey.GetHashCode();
    //        return onlyKey + "[" + hashCode.ToString() + "]";
    //    }
    //    int id = -1;
    //}
    //public class ValDesc : LuaTableKeyValue
    //{
    //    public override int Count { get { return 3; } }
    //    public override bool Init(StkId[] ps)
    //    {
    //        int i = 0;
    //        marqueeStart = LuaDataReadHelper.ReadUtf8String(ps[i++].V);
    //        marqueeEnd = LuaDataReadHelper.ReadUtf8String(ps[i++].V);
    //        image = LuaDataReadHelper.ReadUtf8String(ps[i++].V);
    //        return true;
    //    }
    //    public string StartNotice { get { return marqueeStart; } }
    //    public string EndedNotice { get { return marqueeEnd; } }
    //    public string Image { get { return image; } }

    //    string marqueeStart = "";
    //    string marqueeEnd = "";
    //    string image = "";
    //}
    //public ValDesc this[int id]
    //{
    //    get
    //    {
    //        KeyDesc k = KeyDesc.GenKey(id);
    //        if (!mTableDatas.ContainsKey(k))
    //            return null;
    //        ValDesc val = mTableDatas[k];
    //        return val;
    //    }
    //}
    //   }
}
