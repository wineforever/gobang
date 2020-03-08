using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Wineforever;
using System;

public class mcts{
    public struct Point
    {
        public int X;
        public int Y;
    }
    private const int Rand = 4;
    public static List<string> MCTS_Prob = new List<string>(); 
    //棋形判断
    private static double chess_shape(int[,] board_state, Point p, int player)
    {
        double score = 0;
        List<int[]> shapes = new List<int[]>();
        //遍历4个方向
        int x = p.X; int y = p.Y;
        #region 棋形
        //横向
        int[] shape = new int[11];
        int index = 0;
        for (int i = 5; i >= 1; i--)
        {
            shape[index] = board_state[x - i, y];
            index++;
        }
        shape[index] = -1;
        index++;
        for (int i = 1; i < 6; i++)
        {
            shape[index] = board_state[x + i, y];
            index++;
        }
        shapes.Add(shape);
        //竖向
        shape = new int[11];
        index = 0;
        for (int i = 5; i >= 1; i--)
        {
            shape[index] = board_state[x, y - i];
            index++;
        }
        shape[index] = -1;
        index++;
        for (int i = 1; i < 6; i++)
        {
            shape[index] = board_state[x, y + i];
            index++;
        }
        shapes.Add(shape);
        //左斜
        shape = new int[11];
        index = 0;
        for (int i = 5; i >= 1; i--)
        {
            shape[index] = board_state[x-i, y-i];
            index++;
        }
        shape[index] = -1;
        index++;
        for (int i = 1; i < 6; i++)
        {
            shape[index] = board_state[x+i, y + i];
            index++;
        }
        shapes.Add(shape);
        //右斜
        shape = new int[11];
        index = 0;
        for (int i = 5; i >= 1; i--)
        {
            shape[index] = board_state[x-i, y + i];
            index++;
        }
        shape[index] = -1;
        index++;
        for (int i = 1; i < 6; i++)
        {
            shape[index] = board_state[x + i, y - i];
            index++;
        }
        shapes.Add(shape);
        #endregion
        Dictionary<string, double> standard_shape = new Dictionary<string, double>();

        System.Random rand = new System.Random();
        standard_shape["XXXXX02222X"] = rand.Next(400 - Rand, 500 + Rand);
        standard_shape["XXXXX01111X"] = rand.Next(300 - Rand, 400 + Rand);
        standard_shape["XXXXX02220X"] = rand.Next(200 - Rand, 300 + Rand);
        standard_shape["XXXX2022XXX"] = rand.Next(100 - Rand, 200 + Rand);
        standard_shape["XXXXX022221"] = rand.Next(95 - Rand, 100 + Rand);
        standard_shape["XXXX20222XX"] = rand.Next(90 - Rand, 95 + Rand);
        standard_shape["XXX22022XXX"] = rand.Next(85 - Rand, 90 + Rand);
        standard_shape["XXXXX011112"] = rand.Next(80 - Rand, 85 + Rand);
        standard_shape["XXXX10111XX"] = rand.Next(75 - Rand, 80 + Rand);
        standard_shape["XXX11011XXX"] = rand.Next(70 - Rand, 75 + Rand);
        standard_shape["XXXXX01110X"] = rand.Next(68 - Rand, 70 + Rand);
        standard_shape["XXXX1011XXX"] = rand.Next(66 - Rand, 68 + Rand);
        standard_shape["XXXX002221X"] = rand.Next(64 - Rand, 66 + Rand);
        standard_shape["XXX020221XX"] = rand.Next(62 - Rand, 64 + Rand);
        standard_shape["XX022021XXX"] = rand.Next(60 - Rand, 62 + Rand);
        standard_shape["XXXX20022XX"] = rand.Next(58 - Rand, 60 + Rand);
        standard_shape["XXXX20202XX"] = rand.Next(56 - Rand, 58 + Rand);
        standard_shape["XXXX1022201"] = rand.Next(54 - Rand, 56 + Rand);
        standard_shape["XXXX001112X"] = rand.Next(52 - Rand, 54 + Rand);
        standard_shape["XXX010112XX"] = rand.Next(50 - Rand, 52 + Rand);
        standard_shape["XX011012XXX"] = rand.Next(48 - Rand, 50 + Rand);
        standard_shape["XXXX10011XX"] = rand.Next(46 - Rand, 48 + Rand);
        standard_shape["XXXX10101XX"] = rand.Next(44 - Rand, 46 + Rand);
        standard_shape["XXXX2011102"] = rand.Next(42 - Rand, 44 + Rand);
        standard_shape["XXXX002200X"] = rand.Next(40 - Rand, 42 + Rand);
        standard_shape["XXX02020XXX"] = rand.Next(38 - Rand, 40 + Rand);
        standard_shape["XXXX2002XXX"] = rand.Next(36 - Rand, 38 + Rand);
        standard_shape["XXXX001100X"] = rand.Next(34 - Rand, 36 + Rand);
        standard_shape["XXX01010XXX"] = rand.Next(32 - Rand, 34 + Rand);
        standard_shape["XXXX1001XXX"] = rand.Next(30 - Rand, 32 + Rand);
        standard_shape["XXXXX000221"] = rand.Next(28 - Rand, 30 + Rand);
        standard_shape["XXXXX002021"] = rand.Next(26 - Rand, 28 + Rand);
        standard_shape["XXXXX020021"] = rand.Next(24 - Rand, 26 + Rand);
        standard_shape["XXXX10001XX"] = rand.Next(22 - Rand, 24 + Rand);
        standard_shape["XXXXX000112"] = rand.Next(20 - Rand, 22 + Rand);
        standard_shape["XXXXX001012"] = rand.Next(18 - Rand, 20 + Rand);
        standard_shape["XXXXX010012"] = rand.Next(16 - Rand, 18 + Rand);
        standard_shape["XXXX10001XX"] = rand.Next(2 - Rand, 16 + Rand);
        standard_shape["00000000000"] = rand.Next(Rand, 2 + Rand);

        List<string> old_shape = new List<string>();
        //对基础棋形(Key)进行翻转
        foreach (KeyValuePair<string,double> pair in standard_shape)
        {
            old_shape.Add(pair.Key);
        }
        List<string> new_shape = old_shape.Select(i => {
            char[] str = i.ToCharArray();
            Array.Reverse(str);
            return new string(str);
        }).ToList();
        for (int i = 0; i < new_shape.Count; i++)
        {
            standard_shape[new_shape[i]] = standard_shape[old_shape[i]];
        }
        //把Shapes中的整数变量转换成字符串
        var shapes_str = shapes.Select(i =>
        {
            string str = "";
            for (int n = 0; n < i.Length; n++)
            {
                if (i[n] == -1) str += "0";
                if (i[n] == player) str += "1";
                if (i[n] == 1 - player) str += "2";
            }
            return str;
        }).ToList();
        //判断任意棋子(X)处
        for (int i = 0; i < shapes_str.Count; i++)
        {
            foreach (KeyValuePair<string, double> pair in standard_shape)
            {
                bool isEqua = true;
                string stand_str = pair.Key;
                string shape_str = shapes_str[i];
                for (int n = 0; n < stand_str.Length; n++)
                {
                    if (stand_str[n] != 'X')
                        if (stand_str[n] != shape_str[n])
                            isEqua = false;
                }
                if (isEqua)
                    score += pair.Value;
            }
        }
        //尽量往棋盘中间下
        double D = Math.Pow(Math.Pow((p.X - 14.5), 2) + Math.Pow((p.Y - 14.5), 2), 0.5);
        score = Math.Max(score / D,0);
        return score;
    }
    public static Point move(int[,] board_state, int player)
    {
        var move = new Point();
        double[,] score_map = new double[19, 19];
        List<double> score_list = new List<double>();
        int[,] board_state_padding = new int[29, 29];
        Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
        //避免越界
        for (int i = 0; i < 29; i++)
            for (int j = 0; j < 29; j++)
            {
                if ((i >= 5 && i < 24) && (j >= 5 && j < 24))
                    board_state_padding[i, j] = board_state[i - 5, j - 5];
                else
                    board_state_padding[i, j] = -1;
            }
        for (int i = 0; i < 19; i++)
            for (int j = 0; j < 19; j++)
            {
                if (board_state[i, j] == -1)
                {
                    var p = new Point();
                    p.X = i + 5; p.Y = j + 5;
                    score_map[i, j] = chess_shape(board_state_padding, p, player);
                    score_list.Add(score_map[i, j]);
                }
                else
                {
                    score_list.Add(1e-10);
                }
            }
        var Sum = score_list.Sum();
        score_list = score_list.Select(i => i / Sum).ToList();
        MCTS_Prob = score_list.Select(i => i.ToString()).ToList();
        var sorted = score_list.Select((x, i) => new KeyValuePair<double, int>(x, i)).OrderByDescending(x => x.Key).ToList();
        var policy_sorted = sorted.Select(i => i.Key).ToList();
        var policy_index = sorted.Select(i => i.Value).ToList();
        //找到得分最高点
        for (int i = 0; i < 361; i++)
        {
            int index = policy_index[i];
            int Y = index % 19, X = index / 19;
            if (GameObject.Find("board").GetComponent<logic>().Board_State[X, Y] == -1)
            {
                move.X = X;
                move.Y = Y;
                break;
            }
        }
        return move;
    }
    public static double evaluation(int[,] board_state,int player)
    {
        double value = 0;
        return value;
    }
}
