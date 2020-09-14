using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUVICLAN_TOOLS_
{
    class FUVICOOKIE
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class CookieData
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public string Domain { get; set; }
            public string Path { get; set; }
            public double Expires { get; set; }
            public int Size { get; set; }
            public bool HttpOnly { get; set; }
            public bool Secure { get; set; }
            public bool Session { get; set; }
        }

        public class Root
        {
            public List<CookieData> MyArray { get; set; }
        }
    }
}
