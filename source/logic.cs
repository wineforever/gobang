using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using Wineforever;
using System.Linq;
using System.Threading;

public class logic : MonoBehaviour
{
    //定义当前玩家
    public int Player = 1;
    //定义棋盘状态
    public int[,] Board_State = new int[19, 19];
    //定义事件
    public Dictionary<string, string> Action = new Dictionary<string, string>();
    //定义触发器
    public bool Trigger = false;
    public static Dictionary<string, string> Config = new Dictionary<string, string>();
    public static string Config_Path = System.AppDomain.CurrentDomain.BaseDirectory + "Config.txt";
    public static string Data_Path = System.AppDomain.CurrentDomain.BaseDirectory;
    public static Dictionary<string, List<string>> Data = new Dictionary<string, List<string>>();
    public static Dictionary<string, List<string>> State = new Dictionary<string, List<string>>();
    //定义回合数
    public static int Turn = 0;
    //定义对局结果
    public static int Winner = -1;
    //将棋盘状态转换成列表
    public int[] State_to_List(int[,] board_state, int player = 0, string mod = "None")
    {
        int[] list = new int[19 * 19];
        if (mod == "Current Self" || mod == "Current Enemy")
        {
            var state = new int[19,19];
            int index = 0;
            for (int i = 0; i < 19; i++)
                for (int j = 0; j < 19; j++)
                {
                    if (board_state[i, j] == player)
                      state[i, j] = 1;
                    else state[i, j] = 0;
                    list[index] = state[i, j];
                    index++;
                }
        }
        else if (mod == "Enemy Recent Move")
        {
            board_state = new int[19, 19];
            if (Action.ContainsKey("Event Handle - Move position - X") || Action.ContainsKey("Event Handle - Move position - Y"))
                board_state[int.Parse(Action["Event Handle - Move position - X"]), int.Parse(Action["Event Handle - Move position - Y"])] = 1;
            var state = board_state;
            int index = 0;
            for (int i = 0; i < 19; i++)
                for (int j = 0; j < 19; j++)
                {
                    list[index] = state[i, j];
                    index++;
                }
        }
        else if (mod == "Player")
        {
            int index = 0;
            for (int i = 0; i < 19; i++)
                for (int j = 0; j < 19; j++)
                {
                    list[index] = Player;
                    index++;
                }
        }
        else if (mod == "None")
        {
            int index = 0;
            for (int i = 0; i < 19; i++)
                for (int j = 0; j < 19; j++)
                {
                    list[index] = board_state[i, j];
                    index++;
                }
        }
        return list;
    }
    // Use this for initialization
    void Initialization()
    {
        for (int i = 0; i < 19; i++)
            for (int j = 0; j < 19; j++)
            {
                Board_State[i, j] = -1;
            }
        Config["You"] = "Human";
        Config["Enemy"] = "MCTS AI";
        Config["Serial Number"] = "0000000000";
        if (!File.Exists(Config_Path))
            Wineforever.String.client.SaveToList(Config, Config_Path);
        else
        {
            Config = Wineforever.String.client.LoadFromList(Config_Path);
            int Serial_Number = int.Parse(Config["Serial Number"]);
            Serial_Number++;
            Config["Serial Number"] = Serial_Number.ToString("0000000000");
            Wineforever.String.client.SaveToList(Config, Config_Path);
        }
        /*
        Event - [Any Player] Round Begin
        Event - [Any Player] Move
        Event - [Any Player] Round End
        Event Handle - Recent event
        Event Handle - Move position - X
        Event Handle - Move position - Y
        Player - Set [Player N] 's controller
        */
        /* 
         * 清空内存，释放资源
         * */
        Data = new Dictionary<string, List<string>>();
        State = new Dictionary<string, List<string>>();
        /*
         * */
        Winner = -1;
       Turn = 0;
        Player = 1;
        Action["Player - Set [Player 0] 's controller"] = Wineforever.String.client.LoadFromList(Config_Path)["Enemy"];
        Action["Player - Set [Player 1] 's controller"] = Wineforever.String.client.LoadFromList(Config_Path)["You"];
        //触发事件,回合开始
        Action["Event Handle - Recent event"] = "Event - [Any Player] Round Begin";
        Trigger = true;
    }
    void Start()
    {
        Initialization();
    }
    bool isDraw(Array array)
    {
        bool Temp = true;
        for (int i = 0; i < array.GetLength(0); i++)
            for (int j = 0; j < array.GetLength(1); j++)
            {
                if ((int)array.GetValue(i, j) == -1)
                {
                    Temp = false;
                }
            }
        return Temp;
    }
    // Update is called once per frame
    //每盘结束时保存信息(局面，落子概率，结果)
    private static void SaveData()
    {
        lock (Data)
        {
            Wineforever.String.client.SaveToSheet(Data, Data_Path + "Assets\\train\\" + Config["Serial Number"] + "_" + Winner + ".wf");
        }
    }
    Thread thread;
    void Update()
    {
        if (Trigger == true) //如果触发事件
        {
            if (Action["Event Handle - Recent event"] == "Event - Game Over")
            {
                Winner = Player;
                thread = new Thread(new ThreadStart(SaveData));//启用多线程保存数据，避免卡顿。
                thread.Start();
                if (!isDraw(Board_State)) { }//如果不是和棋
                Thread.Sleep(1); //延迟3秒
                                    //移除棋盘上的棋子
                for (int i = 0; i < GameObject.Find("chess").transform.childCount; i++)
                {
                    Destroy(GameObject.Find("chess").transform.GetChild(i).gameObject);
                }
                Initialization();
            }
            if (Action["Event Handle - Recent event"] == "Event - [Any Player] Round Begin")
            {
                //记录回合数，棋盘状态
                Data["Turn " + Turn.ToString() + " Current Self"] = new List<string>();//当前己方局面
                Data["Turn " + Turn.ToString() + " Current Enemy"] = new List<string>();//当前对方局面
                Data["Turn " + Turn.ToString() + " Enemy Recent Move"] = new List<string>();//对方最近一手
                Data["Turn " + Turn.ToString() + " Player"] = new List<string>();//当前玩家
                var self_list = State_to_List(Board_State, Player, "Current Self");
                var enemy_list = State_to_List(Board_State, 1 - Player, "Current Enemy");
                var recent_list = State_to_List(Board_State, 0, "Enemy Recent Move");
                var player_list = State_to_List(Board_State, Player, "Player");

                for (int i = 0; i < 19 * 19; i++)
                {
                    Data["Turn " + Turn.ToString() + " Current Self"].Add(self_list[i].ToString());
                    Data["Turn " + Turn.ToString() + " Current Enemy"].Add(enemy_list[i].ToString());
                    Data["Turn " + Turn.ToString() + " Enemy Recent Move"].Add(recent_list[i].ToString());
                    Data["Turn " + Turn.ToString() + " Player"].Add(player_list[i].ToString());
                }
                //电脑落子
                Trigger = false;
                if (Action["Player - Set [Player " + Player + "] 's controller"] != "Human")
                {
                    if (Action["Player - Set [Player " + Player + "] 's controller"] == "DeepMind")
                    {
                        var Move = ai.move(); //调用DeepMind AI
                        Action["Event Handle - Move position - X"] = Move.X.ToString();
                        Action["Event Handle - Move position - Y"] = Move.Y.ToString();
                    }
                    else if (Action["Player - Set [Player " + Player + "] 's controller"] == "MCTS AI")
                    {
                        var Move = mcts.move(Board_State, Player); //调用MCTS AI
                        Action["Event Handle - Move position - X"] = Move.X.ToString();
                        Action["Event Handle - Move position - Y"] = Move.Y.ToString();
                        //记录预测落子概率
                        Data["Turn " + Turn.ToString() + " MCTS Prob"] = mcts.MCTS_Prob;
                    }
                    //触发事件
                    Action["Event Handle - Recent event"] = "Event - [Any Player] Move";
                    Trigger = true;
                }
                //保存当前局面
                State["Board State Current Self"] = new List<string>();
                State["Board State Current Enemy"] = new List<string>();
                State["Board State Enemy Recent Move"] = new List<string>();
                State["Board State Player"] = new List<string>();
                for (int i = 0; i < 19 * 19; i++)
                {
                    State["Board State Current Self"].Add(self_list[i].ToString());
                    State["Board State Current Enemy"].Add(enemy_list[i].ToString());
                    State["Board State Enemy Recent Move"].Add(recent_list[i].ToString());
                    State["Board State Player"].Add(player_list[i].ToString());
                }
                Wineforever.String.client.SaveToSheet(State, System.AppDomain.CurrentDomain.BaseDirectory + "Assets\\board_state.wf");
            }
            if (Action["Event Handle - Recent event"] == "Event - [Any Player] Move")
            {
                //获取落子坐标
                int X = int.Parse(Action["Event Handle - Move position - X"]);
                int Y = int.Parse(Action["Event Handle - Move position - Y"]);
                string Object = "collider" + X.ToString("00") + Y.ToString("00");
                var control = GameObject.Find(Object).GetComponent<collider>();
                //判断是否可以落子
                if (Board_State[X, Y] == -1)
                {
                    control.Move(X, Y);
                    //更新棋盘状态
                    Board_State[X, Y] = Player;
                }
                Trigger = false;//触发结束标记
            }
            if (Action["Event Handle - Recent event"] == "Event - [Any Player] Round End")
            {
                //胜利判断
                if (rule.isVictory(Board_State, Player) || isDraw(Board_State))
                {
                    Action["Event Handle - Recent event"] = "Event - Game Over";
                    Trigger = true;
                }
                else
                {
                    //轮流落子
                    Player = Player == 1 ? 0 : 1;
                    //回合数递增
                    Turn++;
                    //触发事件
                    Action["Event Handle - Recent event"] = "Event - [Any Player] Round Begin";
                    Trigger = true;
                }
            }
        }
    }
}
