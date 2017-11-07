namespace Framework.Tools
{
    public enum DataListChangeType
    {
        update_list,
        add_item,
        change_item,
        remove_item,
        clear_list,

        update_list_lv2,
        add_item_lv2,
        change_item_lv2,
        remove_item_lv2,
        clear_list_lv2,
    }
    public interface IModelDataInfo { }
    public interface IDataInfoModel<TI> { }
    public class DataInfoModel<T, TI> : AutoSingleT<T>, IDataInfoModel<TI>
        where T : DataInfoModel<T, TI>, new()
        where TI : IModelDataInfo
    {
        public delegate void DataEventHandler(DataListChangeType t, TI itemInfo, int index);
        public delegate void DatasEventHandler(DataListChangeType t, TI[] itemInfo);
        public delegate void DatasEventHandlerPS(DataListChangeType t, TI[] itemInfo, params object[] ps);
        public event DataEventHandler OnDataInfoChanged;
        public event DatasEventHandler OnDataListChanged;
        public event DatasEventHandlerPS OnDataListChangedPS;

        protected void Notify(DataListChangeType t, TI itemInfo, int index)
        {
            if (null != OnDataInfoChanged)
                OnDataInfoChanged(t, itemInfo, index);
        }
        protected void Notify(DataListChangeType t, TI[] itemInfos)
        {
            if (null != OnDataListChanged)
                OnDataListChanged(t, itemInfos);
        }
        protected void Notify(DataListChangeType t, TI[] itemInfos, params object[] ps)
        {
            if (null != OnDataListChangedPS)
                OnDataListChangedPS(t, itemInfos, ps);
        }
    }
}
