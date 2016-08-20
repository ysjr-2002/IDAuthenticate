using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.WebAPI
{
    public class JsonResult
    {
        public bool content { get; set; }

        public string errorCode { get; set; }

        public string errorMsg { get; set; }

        public string level { get; set; }

        public bool success { get; set; }

    }
}
