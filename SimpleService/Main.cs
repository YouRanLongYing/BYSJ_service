using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using SimpleJSON;
using System.Data;
using System.Text.RegularExpressions;
using SimpleService.BLL;
using SimpleService.Entity;

namespace SimpleService
{
	class MainClass
	{
		static string cmd="";
        public static MainServiceBLL myServiceBLL;
		public static void Main (string[] args)
		{

			Console.WriteLine ("Hello World!");
            myServiceBLL = new MainServiceBLL();
            myServiceBLL.Start();

            
			while(true)
			{
				cmd=Console.ReadLine();
				if(cmd=="stop")
				{
                    myServiceBLL.Stop();
					break;
				}
				else
				{
					var M = JSON.Parse("{ \"version\": \"1.8.6\"}");
					M["message"]=cmd;
					myServiceBLL.SendString2All(M.ToString());
				}
			}

		}

//        static void ProcessNetError(object sender,AsyncEventArgs args)
//        {
//            Console.WriteLine("Error: "+args._sessions.ErrorMsg);
//        }

//        static void ProcessConnected(object sender,AsyncEventArgs args)
//        {
//            Console.WriteLine("Connected: "+args._sessions.ClientSocket.RemoteEndPoint.ToString());
//        }

//        static void ProcessDataReceived(object sender,AsyncEventArgs args)
//        {
//            string request=Encoding.UTF8.GetString( encrypt.Decode(args._sessions.RecvDataBuffer));
////			Console.WriteLine("Received Data:"+request
////			                   +" From :"+ args._sessions.ClientSocket.RemoteEndPoint.ToString());
//            var N = JSON.Parse(request);

//            //version Check
//            var versionString = N["version"].ToString();
//            //Console.WriteLine("Version: "+versionString);
//            if(versionString!=""&&currentVersion==versionString)
//            {
//                //SQL request
//                var sqlStr=N["sql"].ToString();
//                if(sqlStr!=null&&sqlStr!="")
//                {
//                    using(MySQLHelper msh=new MySQLHelper())
//                    {
//                        sqlStr=sqlStr.TrimStart('\"');
//                        sqlStr=sqlStr.TrimEnd('\"');
//                        Console.WriteLine(sqlStr);
//                        msh.ConnectDB();
//                        IDataReader dr= msh.ExecuteReader(sqlStr);
//                        string result="";
//                        while(dr.Read())
//                        {
//                            result += "userId: " + dr["userId"].ToString() + ";   username: " + dr["username"].ToString();
//                            //result+=";    password: "+dr["password"].ToString()+";";
//                        }
//                        Console.WriteLine(result);
//                        MyServer.SendAll(SendReprocess(result));
//                    }
//                }
//                //Message
//                var Message=Regex.Unescape(JSON.GetStr( N["message"].ToString()));
//                if(Message !=""&&Message!=null)
//                {
//                    Console.WriteLine(args._sessions.ClientSocket.RemoteEndPoint.ToString()+" : \n"+  Message);
//                    Thread.Sleep(100);
//                    MyServer.SendAll(SendReprocess(args._sessions.ClientSocket.RemoteEndPoint.ToString()+" say :\n"+ Message));
//                }
//            }
//        }

//        static void ProcessSendDataEnd(object sender,AsyncEventArgs args)
//        {
//            Console.WriteLine("Send To"+ args._sessions.ClientSocket.RemoteEndPoint.ToString()+" : "+cmd);
//        }


//        static byte[] SendReprocess(string data)
//        {
//            var M = JSON.Parse("{ \"version\": \"1.8.6\"}");
//            M["message"]=data;
//            return encrypt.Encode(System.Text.Encoding.UTF8.GetBytes( M.ToString()));
//        }
	}







}
