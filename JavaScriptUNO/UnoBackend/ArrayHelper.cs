using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavaScriptUNO.UnoBackend
{
    public static class ArrayHelper
    {
        public static bool IpAllowed(this List<WhitelistModel> array, string value)
        {
            foreach (WhitelistModel item in array)
            {
                if (item.IsSubnet)
                {
                    if (SubnetMatch(item.Ip, value))
                        return true;
                }
                else
                {
                    if (item.Ip == value)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool SubnetMatch(string subnet, string ip)
        {
            string[] nets = subnet.Split('.');
            string[] nets2 = ip.Split('.');
            if (nets.Length == 4 && nets2.Length == 4)
            {
                //map all subnet options: 
                if (nets[0] == nets2[0])
                {
                    if (nets[1] == "0")
                    {
                        return true;
                    }
                    else if (nets[1] == nets2[1])
                    {
                        if (nets[2] == "0")
                        {
                            return true;
                        }
                        else if (nets[2] == nets2[2])
                        {
                            if (nets[3] == "0")
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            //if it ends up here, one of the above if statemetns hit false
            return false;
        }

    }
}