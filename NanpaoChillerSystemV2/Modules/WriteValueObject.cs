namespace NanpaoChillerSystemV2.Modules
{
    public class WriteValueObject
    {
        /// <summary>
        /// 寫入位置
        /// </summary>
        public ushort Address { get; set; }
        /// <summary>
        /// 寫入數字
        /// </summary>
        public ushort Value { get; set; }
    }
}
