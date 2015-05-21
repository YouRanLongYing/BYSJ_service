using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SimpleService.MySQL
{
    public class SqlSecurity
    {
        /// <summary>
        /// sql语句预处理，排除特殊字符，预防sql注入攻击
        /// </summary>
        public static string CheckSqlParams(string parm, char[] exceptionChar)
        {
            foreach (char cc in exceptionChar)
            {
                parm = String.Concat(parm.Split(cc));
            }
            return parm;
        }
    }
}
