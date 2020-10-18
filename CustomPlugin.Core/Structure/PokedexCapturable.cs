using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CustomPlugin.Core.Consistent
{

    [DataContract]
    public class PokedexCapturable
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public List<string>? Gen1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public List<string>? Gen2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public List<string>? Gen3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public List<string>? Gen4 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public List<string>? Gen5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public List<string>? Gen6 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public List<string>? Gen7 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public List<string>? Gen8 { get; set; }
    }

    [DataContract]
    public class RootPokedex
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string? GameVersion { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public PokedexCapturable? Pokemon { get; set; }

    }

}