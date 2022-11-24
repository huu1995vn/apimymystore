using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIMyMyStore
{
    public class Variables
    {
        public static string ConnectionSQL = string.Empty;
        public static string FieldSelectUser = "name, phone, email, address, createDate";
        public static string FieldSelectCustomer = "name, phone, email, address, createDate";

    }
}