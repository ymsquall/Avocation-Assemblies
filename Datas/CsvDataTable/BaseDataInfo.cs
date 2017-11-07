using System;

namespace Library.CsvDataTable
{
    public class NumStringInfo<TID>
    {
        public TID ID { set; get; }
        public string Name { set; get; }
    }
    public class NumStrNumInfo<TID, TNUM>
    {
        public TID ID { set; get; }
        public string Name { set; get; }
        public TNUM Number { set; get; }
    }
    public class UShortStringInfo : NumStringInfo<ushort> { }
    public class UShortStrUShorInfo : NumStrNumInfo<ushort, ushort> { }
}
