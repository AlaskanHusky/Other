using System;
using System.Text;

namespace Network_2
{
    public class Network
    {
        #region Fields
        private const int v = 4;
        private int[] mask = new int[4];
        private int[] ipN = new int[v];
        private int[] ipFirst = new int[v];
        private int[] ipLast = new int[v];
        private int[] broadcast = new int[v];
        private int[] nextSubnet = new int[v];
        private int[] interface_left = new int[v];
        private int[] interface_right = new int[v];
        private int compsCount;
        #endregion
        #region Properties
        public int CompsCount
        {
            set { compsCount = value; }
            get { return compsCount; }
        }
        public int[] IpN
        {
            set { ipN = value; }
            get { return ipN; }
        }
        public int[] Mask
        {
            get { return mask; }
        }
        public int[] Interface_left
        {
            get { return interface_left; }
        }
        public int[] Interface_right
        {
            get { return interface_right; }
        }
        public int[] NextSubnet
        {
            get { return nextSubnet; }
        }
        #endregion
        #region Addition
        public void AddPCx(string xIp, string xMask)
        {
            string[] str;
            str = xIp.Split('.');
            for (int i = 0; i < str.Length; i++)
            {
                ipN[i] = int.Parse(str[i]);
            }
            str = xMask.Split('.');
            for (int i = 0; i < str.Length; i++)
            {
                mask[i] = int.Parse(str[i]);
            }
            for (int i = 0; i < v; i++)
            {
                interface_left[i] = ipN[i];
                broadcast[i] = ipN[i];
                if (i == v - 1)
                {
                    interface_left[i] = ipN[i] + 1;
                    broadcast[i] = ipN[i] + 3;
                }
            }
        }
        public void Connect()
        {
            Console.WriteLine("PCx: ");
            Output(ipN, "IP address");
            Output(mask, "Mask");
            Output(interface_left, "Interface");
            Output(broadcast, "Broadcast");
        }
        #endregion
        #region Create subnet
        public void Output(int[] data, string str)
        {
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < v; i++)
            {
                s.Append(data[i].ToString());
                s.Append(".");
            }
            s.Remove(s.Length - 1, 1);
            Console.WriteLine("{0}: {1}", str, s.ToString());
            s.Clear();
        }
        public void SetMask()
        {
            int comps = CompsCount + 2;
            int nulls = 0, fourNum;
            StringBuilder s = new StringBuilder();
            string str;
            while (comps != 0)
            {
                comps = comps / 2;
                nulls++;
            }
            for (int i = 0; i < nulls; i++)
            {
                s.Append("0");
            }
            str = s.ToString().PadLeft(8, '1');
            fourNum = Convert.ToInt32(str, 2);
            for (int i = 0; i < 3; i++)
            {
                mask[i] = 255;
            }
            mask[3] = fourNum;
        }
        private void SetInterface(bool isNext)
        {
            for (int i = 0; i < v; i++)
            {
                interface_left[i] = ipN[i];
                if (i == 3)
                {
                    interface_left[i] = ipN[i] + 1;
                }
                if (i == 3 && isNext == true)
                {
                    interface_left[i]++;
                }
            }
            for (int i = 0; i < v; i++)
            {
                interface_right[i] = nextSubnet[i];
                if (i == 3)
                {
                    interface_right[i] = nextSubnet[i] + 1;
                }
            }
        }
        public int SetBroadcast()
        {
            double nulls = 0;
            double compsAvailable;
            int[] bin = new int[8];
            string str;
            str = Convert.ToString(mask[3], 2).PadLeft(8, '0');
            for (int i = 0; i < str.Length; i++)
            {
                bin[i] = (int)str[i] - '0';
            }
            for (int j = 0; j < bin.Length; j++)
            {
                if (bin[j] == 0)
                {
                    nulls++;
                }
            }
            compsAvailable = Math.Pow(2.0, nulls) - 2;
            return (int)compsAvailable;
        }
        public void DistributionOfAddresses(bool isNext)
        {
            for (int i = 0; i < v; i++)
            {
                ipFirst[i] = ipN[i];
                ipLast[i] = ipN[i];
                broadcast[i] = ipN[i];
                nextSubnet[i] = ipN[i];
                if (i == 3)
                {
                    ipFirst[i] = ipN[i] + 2;
                    ipLast[i] = ipFirst[i] + compsCount - 1;
                    broadcast[i] = ipN[i] + SetBroadcast() + 1;
                    nextSubnet[i] = broadcast[i] + 1;
                }
                if (i == 3 && isNext == true)
                {
                    ipFirst[i]++;
                    ipLast[i]++;
                }
            }
            SetInterface(isNext);
        }
        public void ShowSubnets() // Display information about the subnet
        {
            Output(ipN, "IP address of subnet"); // IP Address 
            Output(mask, "Mask of subnet"); // Mask
            // Interfaces
            Output(interface_left, "Left interface of subnet");
            Output(interface_right, "Right interface of subnet");
            // Distribution Of Addresses
            Output(ipFirst, "Address of first computer");
            Output(ipLast, "Address of last computer");
            Output(broadcast, "Broadcast of subnet"); // Broadcast
        }
        #endregion
    }
}