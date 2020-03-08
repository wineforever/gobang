
using System;
using System.Collections;
using System.Collections.Generic;
using Wineforever;
using System.Linq;
using System.IO;
using UnityEngine;
using System.Diagnostics;
using System.Text;

public static class ai{
    public struct Point
    {
        public int X;
        public int Y;
    }
    public static Point move()
    {
        var root = System.IO.Directory.GetCurrentDirectory();
        //调用预测脚本
        System.Console.InputEncoding = System.Text.Encoding.UTF8;
        System.Diagnostics.Process exep = new System.Diagnostics.Process();
        exep.StartInfo.UseShellExecute = false;
        exep.StartInfo.FileName = "cmd.exe";
        exep.StartInfo.RedirectStandardInput = true;
        exep.Start();
        exep.StandardInput.WriteLine("cd /d "+ root);
        exep.StandardInput.WriteLine("python predict.py");
        exep.StandardInput.WriteLine("exit()");
        exep.WaitForExit();
        //分析输出信息
        var policy_dic = Wineforever.String.client.LoadFromSheet(root + "\\policy.wf")["Policy"];//预测落子
        //var eva = double.Parse(Wineforever.String.client.LoadFromSheet(System.AppDomain.CurrentDomain.BaseDirectory + "Assets\\eva.wf")["Evaluation"][0]);//胜率指数
        var policy_list = policy_dic.Select((i => double.Parse(i))).ToList();
        var sorted = policy_list.Select((x, i) => new KeyValuePair<double, int>(x, i)).OrderByDescending(x => x.Key).ToList();
        //var policy_sorted = sorted.Select(i => i.Key).ToList();
        var policy_index = sorted.Select(i => i.Value).ToList();
        //转换成落子坐标
        var Move = new Point();
        for (int i = 0; i < 361; i++)
        {
            int index = policy_index[i];
            int Y = index % 19, X = index / 19;
            if (GameObject.Find("board").GetComponent<logic>().Board_State[X,Y] == -1)
            {
                Move.X = X;
                Move.Y = Y;
                break;
            }
        }
        return Move;
    }
}
