using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Data;
using SimpleJSON;
using SimpleService;
using System.Collections;
using SimpleService.Entity;
using SimpleService.MySQL;

namespace SimpleService.BLL
{
    public class MainServiceBLL
    {
        public AsyncServer asyncServer;
        public static string currentVersion = "\"1.8.6\"";
        public Encrypt encrypt = new Encrypt();
        string cmd = "";
        public Dictionary<Guid, Player> Players;

        public MainServiceBLL():this(8421){}
        public MainServiceBLL(int port)
        {
            asyncServer = new AsyncServer(port);
            asyncServer.ClientConnected+=ProcessConnected;
			asyncServer.NetError+=ProcessNetError;
			asyncServer.DataReceived+=ProcessDataReceived;
            Players = new Dictionary<Guid, Player>();

        }

        void ProcessNetError(object sender,AsyncEventArgs args)
		{
			Console.WriteLine("Error: "+args._sessions.ErrorMsg);
		}

		void ProcessConnected(object sender,AsyncEventArgs args)
		{
			Console.WriteLine("Connected: "+args._sessions.ClientSocket.RemoteEndPoint.ToString());
		}

		void ProcessDataReceived(object sender,AsyncEventArgs args)
		{
			string request=Encoding.UTF8.GetString( encrypt.Decode(args._sessions.RecvDataBuffer));
            
			var N = JSON.Parse(request);
            ProcessMessage(N,args);
			
		}

		void ProcessSendDataEnd(object sender,AsyncEventArgs args)
		{
			//Console.WriteLine("Send To"+ args._sessions.ClientSocket.RemoteEndPoint.ToString()+" : "+cmd);
		}

        /// <summary>
        /// 发送string到所有的客户端
        /// </summary>
        public void SendString2All(string data)
        {
            asyncServer.SendAll(SendReprocess((data)));
        }

        /// <summary>
        /// 转换数据，从string到byte
        /// </summary>
        /// <param name="data">string类型的数据</param>
        /// <returns>要发送的数据</returns>
		byte[] SendReprocess(string data)
		{
			var M = JSON.Parse("{ \"version\": "+currentVersion+"}");
			M["message"]=data;
			return encrypt.Encode(System.Text.Encoding.UTF8.GetBytes( M.ToString()));
		}

        public void Start()
        {
            asyncServer.Start();
        }
        public void Stop()
        {
            try
            {
                asyncServer.Stop();
            }
            catch {}
            
        }

        public static JSONNode GetStandJson()
        {
            return JSON.Parse("{ \"version\": " + currentVersion + "}");
        }

        public void SendDataToClient( Player player,byte[] data)
        {
            asyncServer.Send(player._Session, data);
        }

        public void SendDataToClient(Player player, JSONNode json)
        {
            SendDataToClient(player, encrypt.Encode(System.Text.Encoding.UTF8.GetBytes(json.ToString())));
        }
        /// <summary>
        /// 处理来自客户端的命令
        /// </summary>
        /// <param name="json">数据</param>
        /// <param name="args">参数，含有发送者的数据</param>
        private void ProcessMessage(JSONNode json,AsyncEventArgs args)
        {
            var N = json;
            //version Check
            var versionString = N["version"].ToString();

            

            if (versionString != "" && currentVersion == versionString)
            {
                //SQL request
                var sqlStr = N["sql"].ToString();
                MsgType msgType =(MsgType) N["MsgType"].AsInt;
                switch (msgType)
                {
                    case MsgType.None:
                        break;
                    case MsgType.Message:
                        SendString2All(N["WorldMsg"].ToString());
                        break;
                    case MsgType.Login:
                        Login_Method(N, args);
                        break;
                    case MsgType.Default:
                        break;
                    case MsgType.Transform:
                        break;
                    case MsgType.Sql:
                        break;
                    case MsgType.Regist:
                        break;
                    case MsgType.WorldMsg:
                        SendString2All(N["WorldMsg"].ToString());
                        break;
                    default: break;
                }

                
            }
        }

        private void Login_Method(JSONNode N, AsyncEventArgs args)
        {
            Console.WriteLine(N.ToString());
            Player player;
            JSONNode response = MainServiceBLL.GetStandJson();
            using (MySQLHelper msh = new MySQLHelper())
            {
                Guid clientId = Guid.Parse(JSON.GetStr(N["clientId"].ToString()));
                if (Players.ContainsKey(clientId))
                {
                    player = Players[clientId];
                }
                else
                {
                    player = new Player();
                    player._Session = args._sessions;
                    player.clientId = clientId;
                    Players.Add(clientId, player);
                }
                string userId = "";
                string dbpassword = "";
                string nickName = "";
                string username = JSON.GetStr(N[MsgType.Login.ToString()]["userName"].ToString());
                string password = JSON.GetStr(N[MsgType.Login.ToString()]["password"].ToString());
                //sql语句预处理，排除特殊字符，预防sql注入攻击
                username = SqlSecurity.CheckSqlParams(username, new char[] { '\'', '=', ' ' });

                string loginSql = "use mycargame;select userId,password,nickName from user where username='" + username + "';";
                Console.WriteLine(loginSql);
                msh.ConnectDB();
                IDataReader dr = msh.ExecuteReader(loginSql);
                while (dr.Read())
                {
                    userId = dr["userId"].ToString();
                    dbpassword = dr["password"].ToString();
                    nickName = dr["nickName"].ToString();
                }
                if (dbpassword != "" && dbpassword == password)
                {
                    //TODO 验证通过
                    player.userId = int.Parse(userId);
                    player.userName = username;
                    player.nickName = nickName;
                    player.isActive = true;
                    response["MsgType"] = ((int)MsgType.Login_Response).ToString();
                    response = player.ToJson(MsgType.Login_Response.ToString(), response);
                    response["Login_Response"]["Result"] = "True";
                }
                else
                {
                    //TODO 验证失败
                    response["MsgType"] = ((int)MsgType.Login_Response).ToString();
                    response["Login_Response"]["Result"] = "False";
                }
                response["clientId"] = player.clientId.ToString();
                Console.WriteLine(response.ToString());
                SendDataToClient(player, response);
            } 

           
            ////Message
            //var Message = Regex.Unescape(JSON.GetStr(N["message"].ToString()));
            //if (Message != "" && Message != null)
            //{
            //    Console.WriteLine(args._sessions.ClientSocket.RemoteEndPoint.ToString() + " : \n" + Message);
            //    //Thread.Sleep(100);
            //    asyncServer.SendAll(SendReprocess(args._sessions.ClientSocket.RemoteEndPoint.ToString() + " say :\n" + Message));
            //}

        }



        

	}

    
}
