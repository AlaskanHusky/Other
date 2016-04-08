namespace Network_2
{
    public class Internet
    {
        private const int v = 4;
        private int[] mask = new int[4];
        private int[] ip = new int[v];
        private int[] router = new int[v];
        public int[] Router
        {
            get
            {
                return router;
            }
        }
        public Internet(string internetIp, string internetMask)
        {
            string[] str;
            str = internetIp.Split('.');
            for (int i = 0; i < str.Length; i++)
            {
                ip[i] = int.Parse(str[i]);
            }
            str = internetMask.Split('.');
            for (int i = 0; i < str.Length; i++)
            {
                mask[i] = int.Parse(str[i]);
            }
            for (int i = 0; i < v; i++)
            {
                router[i] = ip[i];
                if (i == v - 1)
                {
                    router[i] = ip[i] + 1;
                }
            }
        }
    }
}
