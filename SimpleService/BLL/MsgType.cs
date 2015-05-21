using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleService.BLL
{
    public enum MsgType
    {
        Default=-1,
        None=0,
        Login = 1,//登录
        Message = 2,//普通信息
        Transform=3,//位置及变换
        Sql=4,//sql语句，用来测试
        Regist=5,//注册用户
        WorldMsg=6,//世界喊话
        RoomMsg=7,//房间的喊话
        Player=8,//玩家信息，登录后获取，刷新
        Room=9,//房间信息
        Login_Response = 10,//登录的返回信息
        Player_Join=11,//有玩家加入房间
    }
}
