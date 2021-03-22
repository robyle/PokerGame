using System;

namespace GamePoker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("让我们一起来玩转 PokerGame !");
            while (true)
            {
                int Step;//步数进度
                bool flgEnd;//结束标记
                InitGame(out Step, out flgEnd);//初始化游戏
                do
                {
                    ShowGameStatus();//输出显示游戏状态
                    try
                    {
                        int[] LineNum = new int[2];
                        Console.WriteLine($"\r\n请 角色[{Step % 2}] 输入【两整数空格进行区分】：行数 数量");//0 1 两个角色
                        ///TODO:此处可以进人机角色改造 可以设定为人人对战模式，也可以人机模式 
                        string[] sr = Console.ReadLine().Split(' ');
                        LineNum[0] = int.Parse(sr[0]);
                        LineNum[1] = int.Parse(sr[1]);
                        DealInput(ref Step, ref flgEnd, LineNum);
                    }
                    catch (System.Exception ex)
                    {
                        TipsOutput($"输入异常：{ex.Message}\r\n"+"提示,请输入【两整数空格进行区分】：行数 数量");
                    }
                } while (!flgEnd);
                Console.WriteLine("游戏结束!   任意键继续....  Ctrl+C 退出.\r\n------------------------------------------");
                Console.ReadKey();
            }
        }
        /// <summary>
        /// 处理输入信息
        /// </summary>
        /// <param name="Step">步进数</param>
        /// <param name="flgEnd">处理输入结果的结束状态</param>
        /// <param name="LineNum">行数 数量的数组</param>
        private static void DealInput(ref int Step, ref bool flgEnd, int[] LineNum)
        {
            //检查输入数据是否合法
            if (LineNum[0] > 0 && LineNum[0] <= PokerArr.Length)
            {
                if (LineNum[1] > 0 && PokerArr[LineNum[0] - 1] >= LineNum[1])
                {
                    bool flgCount = false;
                    for (int i = 0; i < PokerArr.Length; i++)
                    {
                        if (PokerArr[i] >= LineNum[1])
                        {
                            flgCount = true;
                            Step++;
                            flgEnd = CheckResult(Step, flgEnd, LineNum);
                            break;
                        }
                    }
                    if (!flgCount)
                    {
                        TipsOutput("请输入的数量不能大于全部的可拿数量。");
                    }
                }
                else
                {
                    TipsOutput("数值不合法!");
                }
            }
            else
            {
                TipsOutput("数据组越界!");
            }
        }
        /// <summary>
        /// 提示输出进行简单封装
        /// </summary>
        /// <param name="tipsMsg">提示内容:红色高亮一下，方便用户明白内容</param>
        private static void TipsOutput(string tipsMsg)
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine(tipsMsg);
            System.Console.ForegroundColor = ConsoleColor.White;
        }
        /// <summary>
        /// 检查游戏结果状态
        /// </summary>
        /// <param name="Step">步进数量</param>
        /// <param name="flgEnd">处理输入结果的结束状态</param>
        /// <param name="LineNum">数量的数组</param>
        /// <returns></returns>
        private static bool CheckResult(int Step, bool flgEnd, int[] LineNum)
        {
            PokerArr[LineNum[0] - 1] -= LineNum[1];
            int tmpSum = 0;
            foreach (var item in PokerArr)
            {
                tmpSum += item;
            }
            if (tmpSum == 1)
            {
                flgEnd = true;
                System.Console.WriteLine($"角色：{Step % 2} 输了！");
            }
            if (tmpSum == 0)
            {
                flgEnd = true;
                System.Console.WriteLine($"角色：{(Step + 1) % 2} 输了！");
            }

            return flgEnd;
        }
        /// <summary>
        /// 显示游戏状态：行号和数量
        /// </summary>
        private static void ShowGameStatus()
        {
            System.Console.ForegroundColor = ConsoleColor.Blue;
            System.Console.WriteLine("+++++++++++++++");
            for (int i = 0; i < PokerArr.Length; i++)
            {
                Console.Write($"|行号:{i + 1}\t数量:{PokerArr[i]}|\r\n");
            }
            System.Console.WriteLine("+++++++++++++++");
            System.Console.ForegroundColor = ConsoleColor.White;
        }
        public static int[] PokerArr = new int[] { 3, 5, 7 };//Poker的三行数量
        /// <summary>
        /// 初始化游戏变量
        /// </summary>
        /// <param name="Step">步进数量</param>
        /// <param name="flgEnd">处理输入结果的结束状态</param>
        private static void InitGame(out int Step, out bool flgEnd)
        {
            Step = 0;
            flgEnd = false;
            PokerArr = new int[] { 3, 5, 7 };//游戏重置
            System.Console.WriteLine("游戏初始化完毕！");
        }
    }
}
