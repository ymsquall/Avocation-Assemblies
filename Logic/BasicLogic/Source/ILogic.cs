namespace Logic.BasicLogic
{
    public enum ResourceType
    {
        Sound,
        Music,
        Material,
        Model,
        UIAtlas,
        Font,
        UIView,
        UINeedAtlas,
        Effect,
        max
    }
    public enum SharedMaterial
    {
        公用模型材质0,
        公用模型材质1,
        max
    }
    public interface ILogic
    {
        bool Loading { get; }
        void ActiveLogic();
        void DeActiveLogic();
    }
    //public interface IAsyncMap2DLoaderLogic
    //{
    //    Dictionary<string, string> this[ResourceType t] { set; }
    //    场景类型 SceneType { set; get; }
    //    UnityEngine.Rect SceneEdge { set; get; }
    //    bool LoadCharaTemplate { get; }
    //    UnityEngine.Rect[] Grounds { set; get; }
    //    UnityEngine.Transform SceneRoot { set; get; }
    //    UnityEngine.GameObject Map2D { set; get; }
    //    UnityEngine.GameObject RadarMapRoot { set; get; }
    //    UnityEngine.GameObject LocalPlayerTemplate { set; get; }
    //    //UnityEngine.GameObject CharaTemplate { set; get; }
    //    //bool CreateHWTemplate { get; }
    //    //UnityEngine.GameObject HWTemplate { set; get; }
    //    //bool CreateEETemplate { get; }
    //    //UnityEngine.GameObject EETemplate { set; get; }
    //    //bool CreateWAETemplate { get; }
    //    //UnityEngine.GameObject WAETemplate { set; get; }
    //    //bool CreateAnimTemplate { get; }
    //    //AnimResourceObj AnimTemplate { set; get; }
    //    bool CreateCtrlsTemplate { get; }
    //    UnityEngine.GameObject CtrlsTemplate { set; get; }
    //    bool MainCamera { get; }
    //    H2DCameraBoundsController CameraController { set; get; }
    //    UnityEngine.Vector3 WorldOffset { get; }
    //    void AddTransPoint(传送点类型 t, ITransPoint pt);
    //    void AddMonsterMapper(int mid, int cid, byte ii);
    //    void AddInst2ChapterMapper(int iid, int cid, byte ii);
    //    void DoChangeScene(int sceneID);
    //    void OnSceneLoaded();
    //}
}
