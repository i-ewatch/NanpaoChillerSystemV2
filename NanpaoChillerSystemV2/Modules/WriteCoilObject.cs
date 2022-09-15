namespace NanpaoChillerSystemV2.Modules
{
    public class WriteCoilObject
    {
        /// <summary>
        /// 寫入位置
        /// </summary>
        public ushort Address { get; set; }
        /// <summary>
        /// 寫入數字
        /// </summary>
        public bool State { get; set; }
    }
}
