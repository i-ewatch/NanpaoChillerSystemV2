namespace NanpaoChillerSystemV2.Protocols
{
    public abstract class ElectricData:AbsProtocol
    {
        /// <summary>
        /// R線電壓
        /// </summary>
        public decimal RV { get; protected set; }
        /// <summary>
        /// S線電壓
        /// </summary>
        public decimal SV { get; protected set; }
        /// <summary>
        /// T線電壓
        /// </summary>
        public decimal TV { get; protected set; }
        /// <summary>
        /// R項電流
        /// </summary>
        public decimal RA { get; protected set; }
        /// <summary>
        /// RN項電壓
        /// </summary>
        public decimal RNV { get; protected set; }
        /// <summary>
        /// RS項電壓
        /// </summary>
        public decimal RSV { get; protected set; }
        /// <summary>
        /// R項功率
        /// </summary>
        public decimal RKW { get; protected set; }
        /// <summary>
        /// R項虛功率
        /// </summary>
        public decimal RKVAR { get; protected set; }
        /// <summary>
        /// R項視在功率
        /// </summary>
        public decimal RKVA { get; protected set; }
        /// <summary>
        /// R項功因
        /// </summary>
        public decimal RPFE { get; protected set; }
        /// <summary>
        /// S項電流
        /// </summary>
        public decimal SA { get; protected set; }
        /// <summary>
        /// SN項電壓
        /// </summary>
        public decimal SNV { get; protected set; }
        /// <summary>
        /// ST項電壓
        /// </summary>
        public decimal STV { get; protected set; }
        /// <summary>
        /// S項功率
        /// </summary>
        public decimal SKW { get; protected set; }
        /// <summary>
        /// S項虛功率
        /// </summary>
        public decimal SKVAR { get; protected set; }
        /// <summary>
        /// S項視在功率
        /// </summary>
        public decimal SKVA { get; protected set; }
        /// <summary>
        /// S項功因
        /// </summary>
        public decimal SPFE { get; protected set; }
        /// <summary>
        /// T項電流
        /// </summary>
        public decimal TA { get; protected set; }
        /// <summary>
        /// TN項電壓
        /// </summary>
        public decimal TNV { get; protected set; }
        /// <summary>
        /// TR項電壓
        /// </summary>
        public decimal TRV { get; protected set; }
        /// <summary>
        /// T項功率
        /// </summary>
        public decimal TKW { get; protected set; }
        /// <summary>
        /// T項虛功率
        /// </summary>
        public decimal TKVAR { get; protected set; }
        /// <summary>
        /// T項視在功率
        /// </summary>
        public decimal TKVA { get; protected set; }
        /// <summary>
        /// T項功因
        /// </summary>
        public decimal TPFE { get; protected set; }
        /// <summary>
        /// 功率因數
        /// </summary>
        public decimal PFE { get; protected set; }
        /// <summary>
        /// 總功率
        /// </summary>
        public decimal KW { get; protected set; }
        /// <summary>
        /// 總虛功率
        /// </summary>
        public decimal KVAR { get; protected set; }
        /// <summary>
        /// 總視在功率
        /// </summary>
        public decimal KVA { get; protected set; }
        /// <summary>
        /// 用電度數
        /// </summary>
        public decimal KWH { get; protected set; }
        /// <summary>
        /// KVARH
        /// </summary>
        public decimal KVARH { get; protected set; }
        /// <summary>
        /// KVAH
        /// </summary>
        public decimal KVAH { get; protected set; }
        /// <summary>
        /// R相電壓角度
        /// </summary>
        public decimal RV_Angle { get; protected set; }
        /// <summary>
        /// S相電壓角度
        /// </summary>
        public decimal SV_Angle { get; protected set; }
        /// <summary>
        /// T相電壓角度
        /// </summary>
        public decimal TV_Angle { get; protected set; }
        /// <summary>
        /// R相電流角度
        /// </summary>
        public decimal RA_Angle { get; protected set; }
        /// <summary>
        /// S相電流角度
        /// </summary>
        public decimal SA_Angle { get; protected set; }
        /// <summary>
        /// T相電流角度
        /// </summary>
        public decimal TA_Angle { get; protected set; }
    }
}
