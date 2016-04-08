using System;
using System.Text;

namespace Network_2
{
    class Program
    {
        private static void IpInspection(int[] ip)
        {
            for (int i = 0; i < ip.Length; i++)
            {
                try
                {
                    if (ip[i] < 0 || ip[i] > 255) // Value range checking
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Incorrect IP Address!");
                    Environment.Exit(0); //The application is completed and returns the OS parameter values
                }
            }
        }
        private static void MaskInspection(int compsCount, int[] mask)
        {
            double nulls = 0;
            double compsAvailable;
            int[] bin = new int[8];
            bool isCorrect = true;
            string str;
            try
            {
                for (int i = 0; i < mask.Length; i++)
                {
                    if (mask[i] < 0 || mask[i] > 255) // Value range checking
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                    str = Convert.ToString(mask[i], 2).PadLeft(8, '0'); // Convert in binary form
                    for (int j = 0; j < str.Length; j++)
                    {
                        bin[j] = (int)str[j] - '0';
                    }
                    for (int j = 0; j < bin.Length; j++)
                    {
                        if (bin[j] == 0)
                        {
                            nulls++;
                            isCorrect = false;
                        }
                        if (bin[j] == 1 && isCorrect == false) // Check for the existence of the mask
                        {
                            throw new ArgumentOutOfRangeException();
                        }
                    }
                }
                compsAvailable = Math.Pow(2.0, nulls) - 2;
                if (compsCount > compsAvailable)
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Incorrect mask!");
                Environment.Exit(0); //The application is completed and returns the OS parameter values
            }
        }
        private static string Output(int[] arr)
        {
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < arr.Length; i++)
            {
                s.Append(arr[i].ToString());
                s.Append(".");
            }
            s.Remove(s.Length - 1, 1);
            return s.ToString();
        }
        private static void RoutingTable(int netCount, Network[] net, Internet inet)
        {
            int n = 0;
            Console.WriteLine("=================================Routing Table==================================");
            Console.WriteLine("    Next subnet    " + "Mask           " + "Interface");
            for (int i = 0; i < netCount; i++)
            {
                Console.WriteLine("Routes for subnet #{0}", i + 1);
                n = i;
                for (int j = 0; j < i+2; j++)
                {
                    if (i == netCount - 2 && j == 0)
                    {
                        Console.WriteLine("#{0}: 0.0.0.0       0.0.0.0       {1}", j + 1, Output(inet.Router));
                        continue;
                    }
                    if (j == 0 && i != netCount)
                    {
                        if (i == netCount - 1)
                        {
                            Console.WriteLine("#{0}: 0.0.0.0       0.0.0.0       {1}", j + 1, Output(net[1].Interface_left));
                            continue;
                        }
                        Console.WriteLine("#{0}: 0.0.0.0       0.0.0.0       {1}", j + 1, Output(net[i + 1].Interface_left));
                        if (i == 0)
                        {
                            break;
                        }
                        continue;
                    }
                    if (i == netCount - 1)
                    {
                        for (int k = 1;  k < netCount - 1; k++)
                        {
                            Console.WriteLine("#{0}: {1}       {2}       {3}", k + 1, Output(net[k].IpN), Output(net[k].Mask), Output(net[0].Interface_right));
                        }
                        break;
                    }
                    if (j == i+1)
                    {
                        Console.WriteLine("#{0}: {1}       {2}       {3}", j + 1, Output(net[netCount - 1].IpN), Output(net[netCount - 1].Mask), Output(net[i - 1].Interface_right));
                        continue;
                    }
                    Console.WriteLine("#{0}: {1}       {2}       {3}", j + 1, Output(net[j - 1].IpN), Output(net[j - 1].Mask), Output(net[n - 1].Interface_right));
                }
                Console.WriteLine("================================================================================");
            }
        }
        static void Main(string[] args)
        {
            int netCount, allComps = 0;
            bool isNext = false;
            string[] str;
            int[] ip = new int[4];
            int[] mask = new int[4];
            Console.Write("Enter the IP of network (xxx.xxx.xxx.xxx): ");
            str = Console.ReadLine().Split('.');
            for (int i = 0; i < str.Length; i++)
            {
                ip[i] = int.Parse(str[i]);
            }
            IpInspection(ip);
            Console.Write("Enter the mask of network (xxx.xxx.xxx.xxx): ");
            str = Console.ReadLine().Split('.');
            for (int i = 0; i < str.Length; i++)
            {
                mask[i] = int.Parse(str[i]);
            }
            Console.Write("Enter the number of subnets: ");
            netCount = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the number of computers in each subnet (>2): ");
            Network[] ipNet = new Network[netCount + 1];
            Internet iNet = new Internet("8.8.8.8", "0.0.0.0");
            for (int i = 0; i < ipNet.Length; i++)
            {
                ipNet[i] = new Network();
            }
            ipNet[netCount].AddPCx("83.117.45.0", "255.255.255.252");
            ipNet[0].IpN = ip;
            for (int i = 0; i < netCount; i++)
            {
                Console.Write("Subnet #{0}: ", i + 1);
                ipNet[i].CompsCount = int.Parse(Console.ReadLine());
                if (ipNet[i].CompsCount <= 2)
                {
                    Console.WriteLine("Wrong number of computers!");
                    Environment.Exit(0);
                }
                allComps += ipNet[i].CompsCount;
            }
            MaskInspection(allComps, mask);
            for (int i = 0; i < netCount; i++)
            {
                Console.WriteLine("================================================================================");
                Console.WriteLine("Information about subnet #{0} ", i + 1);
                if (i != 0)
                {
                    ipNet[i].IpN = ipNet[i - 1].NextSubnet;
                    isNext = true;
                }
                ipNet[i].SetMask();
                ipNet[i].DistributionOfAddresses(isNext);
                ipNet[i].ShowSubnets();
                if (i == 0)
                {
                    ipNet[netCount].Connect();
                }
            }
            RoutingTable(netCount + 1, ipNet, iNet);
        }
    }
}