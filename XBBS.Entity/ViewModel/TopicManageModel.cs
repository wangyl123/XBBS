using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XBBS.Models.ViewModel
{
    public class TopicManageModel
    {
        /// <summary>
        /// id
        /// </summary>
        [PetaPoco.Column("fid")]
        public int FID { get; set; }
        /// <summary>
        /// 节点名称
        /// </summary>
        [PetaPoco.Column("cname")]
        public string CName { get; set; }
        /// <summary>
        /// 节点id
        /// </summary>
        [PetaPoco.Column("cid")]
        public int CID { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        [PetaPoco.Column("title")]
        public string Titile { get; set; }
        /// <summary>
        /// 作者名称
        /// </summary>
        [PetaPoco.Column("username")]
        public string UserName { get; set; }
        /// <summary>
        /// UID
        /// </summary>
        [PetaPoco.Column("uid")]
        public int UID { get; set; }
        /// <summary>
        /// 回复数目
        /// </summary>
        [PetaPoco.Column("replycount")]
        public int ReplyCount { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [PetaPoco.Column("addtime")]
        public DateTime AddTime { get; set; }
        /// <summary>
        /// 置顶
        /// </summary>
        [PetaPoco.Column("is_top")]
        public int IsTop { get; set; }
    }
}
