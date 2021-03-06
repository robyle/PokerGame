[![Gitpod ready-to-code](https://img.shields.io/badge/Gitpod-ready--to--code-blue?logo=gitpod)](https://gitpod.io/#https://github.com/robyle/PokerGame)

# 笔试题

15个任意物品（可以是火柴牙签poker）
以下按牙签为例
 
将15根牙签
分成三行
每行自上而下（其实方向不限）分别是3、5、7根
 
安排两个玩家，每人可以在一轮内，在任意行拿任意根牙签，但不能跨行
 
拿最后一根牙签的人即为输家
 
题目
请用你最擅长的语言，以你觉得最优雅的方式写一个符合以上游戏规则的程序。完成后把写好的代码和简历同时发到以下3个邮箱（备注姓名+岗位），并加上一段简短的文字描述一下你的想法
（请使用javascript，typescript或C#的其中一种语言完成测试题）
邮箱地址：
    <!-- 
    发送：eddy.ma@sjfood.com
    抄送：sugar@sjfood.com
    抄送：daisy@wxftrading.com
    -->

# 游戏算法逻辑实现分析

## 实现步骤分析

- 0.游戏欢迎提示
- 1.初始化游戏
- 2.循环进入检测输入处理
    - 2.1 检查输入合法才进一步处理，否则给出提示不合法原因
    - 2.2 处理输入并对各行的处理结果
    - 2.3 判断是否继续处理。如果不处理，给出胜负结果信息
- 3.游戏结束，任意键继续，Ctrl+C 结束游戏。

## 参考实现基本逻辑

```cs
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
```

# 游戏算法分析

## 前言
        
        相同的游戏联想到了取数逻辑。但规则有所差异，故做了调整分析如下

## 最大轮次方法分析

        A B 角色：A 先拿
        1.本游戏最多只能玩 15/2=7轮。 并且最多轮数的情况下，谁先拿谁就输.
        
        2.第一轮回：和只有可能  2 3 4 5 6 7 8 9 10 11 12
        
        12 只有 5+7的可能性，最后剩下3 ，由于不限数量 谁先取 谁赢--- 
        
        11 7+4 or 5+6, 这种情况下也是谁先拿谁就稳赢
        
        10 5+5（谁先拿谁赢 1 3 中取1 ） or 3+7（谁先谁赢） 
    
### 结论
        
        第一轮产生的和越小，后面列举的情况就越多，故放弃该方法的深入探索。

        --------------
## 奇 偶 分析
    
### 初步用代数最小化发分析

        A （奇数）  B（偶数）
        AAA(111 先输)  AAB(110 先赢) ABB（100 先输） BBB（222 先输） ABA（101先赢）

### 用偶数分析
        
        用二进制解决的话，把这三堆分别用二进制表示，比如这里3就是011，5就是101，7就是111，然后把这三个数字加一下，这里011+101+111=223。如果加出来的数字每一位都是偶数的话就是必胜局面。比如这里的223最后一位不是偶数，所以我们任意一堆里面拿走一根（比如7这堆里拿走一根变成6），那么011+101+110=222，每一位都是偶数了，就形成了一个必胜的局面。以此类推。                

## 查找资料存在SG函数的分析如下：

    首先我们用T表示当前状态的所有火柴数异或为0,否则极为S。
    我们设只有一根火柴的堆为单根堆,否则为充裕堆,充裕堆>=2的T用T2表示,全是单根堆的T用T0表示(没有T1)同样的充裕堆>=2的S用S2表示,全为单根堆的S用S0表示,只有一堆充裕堆的用S1表示
        1.S0必败,T0必胜每次自己取奇数堆的,那么最后一堆一定由自己取.T0必胜同理
        2.S1只要方法得当,必胜如果单根堆的堆数为偶数,那么把充裕堆中取成只剩下1根,变成S0,对手必败.
    如果单根堆的堆数为奇数,那么把充裕堆全取完,同样对手必败
        3.S2不可以一次转化到T0每次最多只能取完一堆,所以2堆充裕堆不可能一次就没了
        4.S2可以一次变成T2用1可知S可以变成T,由7可知S不可一次变成T0,所以S可以一次变成T2
        5.T2不可以一次变成S0同7
        6.T2一定变成S1或者S2中的一种由2知T一定变成S,由9知T2不可一次变成S0,所以一定变成S1或S2中的一种
        7.S2,只要方法得当一定胜S2可以变成T2,然T2一定变成S1或者S2,如果变成S1,已胜,变成S2,则继续,直到T2只能变成S1为止。
        8.T2,只要对手方法得当必输同6综上所述先手必胜态为T0 S2 S1必输态为S0 T2
    
    只需要做状态分析，就可以得出先。


## TODO 计划

    由于时间有限，准备进一步完善人机对抗实现。

