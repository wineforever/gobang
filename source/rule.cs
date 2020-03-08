public static class rule{
    public static bool isVictory(int[,] board_state, int player)
    {
        for (int X = 0; X < 19; X++)
            for (int Y = 0; Y < 19; Y++)
            {
                //从己方棋子开始搜索，减少计算量
                if (board_state[X, Y] == player)
                {
                    //横向
                    //从该子左侧开始遍历，碰到非己方棋子停止，统计数目
                    int Count = 1;
                    int Index = 1;
                    bool Flag = true;
                    while (Flag)
                    {
                        //判断越界
                        if (X - Index >= 0)
                        {
                            if (board_state[X - Index, Y] == player)
                            {
                                Count++;
                                Index++;
                            }
                            else
                            {
                                Flag = false;
                            }
                        }
                        else
                        {
                            Flag = false;
                        }
                    }
                    //右侧
                    Index = 1;
                    Flag = true;
                    while (Flag)
                    {
                        //判断越界
                        if (X + Index < 19)
                        {
                            if (board_state[X + Index, Y] == player)
                            {
                                Count++;
                                Index++;
                            }
                            else
                            {
                                Flag = false;
                            }
                        }
                        else
                        {
                            Flag = false;
                        }
                    }
                    if (Count >= 5)
                        return true;
                    //竖向
                    //上
                    Count = 1;
                    Index = 1;
                    Flag = true;
                    while (Flag)
                    {
                        //判断越界
                        if (Y - Index >= 0)
                        {
                            if (board_state[X, Y - Index] == player)
                            {
                                Count++;
                                Index++;
                            }
                            else
                            {
                                Flag = false;
                            }
                        }
                        else
                        {
                            Flag = false;
                        }
                    }
                    //下
                    Index = 1;
                    Flag = true;
                    while (Flag)
                    {
                        //判断越界
                        if (Y + Index < 19)
                        {
                            if (board_state[X, Y + Index] == player)
                            {
                                Count++;
                                Index++;
                            }
                            else
                            {
                                Flag = false;
                            }
                        }
                        else
                        {
                            Flag = false;
                        }
                    }
                    if (Count >= 5)
                        return true;
                    //左斜
                    //左上
                    Count = 1;
                    Index = 1;
                    Flag = true;
                    while (Flag)
                    {
                        //判断越界
                        if (X - Index >= 0 && Y - Index >= 0)
                        {
                            if (board_state[X - Index, Y - Index] == player)
                            {
                                Count++;
                                Index++;
                            }
                            else
                            {
                                Flag = false;
                            }
                        }
                        else
                        {
                            Flag = false;
                        }
                    }
                    //右下
                    Index = 1;
                    Flag = true;
                    while (Flag)
                    {
                        //判断越界
                        if (X + Index < 19 && Y + Index < 19)
                        {
                            if (board_state[X + Index, Y + Index] == player)
                            {
                                Count++;
                                Index++;
                            }
                            else
                            {
                                Flag = false;
                            }
                        }
                        else
                        {
                            Flag = false;
                        }
                    }
                    if (Count >= 5)
                        return true;
                    //右斜
                    //右上
                    Count = 1;
                    Index = 1;
                    Flag = true;
                    while (Flag)
                    {
                        //判断越界
                        if (X + Index < 19 && Y - Index >= 0)
                        {
                            if (board_state[X + Index, Y - Index] == player)
                            {
                                Count++;
                                Index++;
                            }
                            else
                            {
                                Flag = false;
                            }
                        }
                        else
                        {
                            Flag = false;
                        }
                    }
                    //左下
                    Index = 1;
                    Flag = true;
                    while (Flag)
                    {
                        //判断越界
                        if (X - Index >= 0 && Y + Index < 19)
                        {
                            if (board_state[X - Index, Y + Index] == player)
                            {
                                Count++;
                                Index++;
                            }
                            else
                            {
                                Flag = false;
                            }
                        }
                        else
                        {
                            Flag = false;
                        }
                    }
                    if (Count >= 5)
                        return true;
                }
            }
        return false;
    }
}
