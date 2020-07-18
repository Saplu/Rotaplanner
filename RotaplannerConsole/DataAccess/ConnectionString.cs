using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public static class ConnectionString
    {
        public static readonly string conn = "mongodb+srv://Saplu:87se77y6k7rkioh7mvfz@dcdata.lt2xw.mongodb.net/dcdata?retryWrites=true&w=majority";
        public static string connection { get; set; }
    }
}
