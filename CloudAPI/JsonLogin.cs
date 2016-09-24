using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudAPI
{
    /// <summary>
    /// 登录 
    /// </summary>
    public class JsonLogin
    {
        public int code { get; set; }

        public JsonLoginData data { get; set; }
    }

    /// <summary>
    /// 登录数据
    /// </summary>
    public class JsonLoginData
    {
        public string company { get; set; }

        public string contact { get; set; }

        public string email { get; set; }

        public int id { get; set; }

        public bool password_reseted { get; set; }

        public string phone { get; set; }

        public int role_id { get; set; }

        public string username { get; set; }
    }

    /// <summary>
    /// 登录状态
    /// </summary>
    public class JsonStatus
    {
        public int code { get; set; }

        public JsonStatusData data { get; set; }

        public bool password_reseted { get; set; }

        public string phone { get; set; }

        public int role_id { get; set; }

        public string username { get; set; }
    }


    public class JsonStatusData
    {
        public string company { get; set; }

        public string contact { get; set; }

        public string email { get; set; }

        public int id { get; set; }

        public limitation limitation { get; set; }
    }


    public class limitation
    {
        public int called { get; set; }

        public double expiration { get; set; }

        public int id { get; set; }

        public int quota { get; set; }

        public int user_id { get; set; }
    }
}
