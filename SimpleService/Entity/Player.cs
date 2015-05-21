using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleJSON;
using SimpleService.BLL;

namespace SimpleService.Entity
{
    public class Player
    {
#region Feild
        private Session _session;
        public Guid clientId;
        public int roomId = -1;
        public int userId;
        public string nickName;
        public string userName;
        public bool isActive=false;
#endregion
        

        public Session _Session
        {
            get { return _session; }
            set { _session = value; }
        }

        public Player() { } 
        public Player(Session session)
        {


        }

        #region Method
        public static Player Login(JSONNode M)
        {
            return new Player();
        }

        public JSONNode ToJson(JSONNode M)
        {
            M["clientId"] = clientId.ToString();
            M["userId"] = userId.ToString();
            M["userName"] = userName.ToString();
            M["nickName"] = nickName.ToString();
            return M;
        }
        public JSONNode ToJson(string parentNode,JSONNode M)
        {
            M[parentNode]["clientId"] = clientId.ToString();
            M[parentNode]["userId"] = userId.ToString();
            M[parentNode]["userName"] = userName.ToString();
            M[parentNode]["nickName"] = nickName.ToString();
            return M;
        }

        public void GetFromJson(string parentNode, JSONNode M)
        {
            //this.clientId = NetClient.clientId;
            this.clientId = Guid.Parse(JSON.GetStr(M[parentNode]["clientId"]));
            userId = int.Parse(JSON.GetStr(M[parentNode]["clientId"]).ToString());
            userName = JSON.GetStr(M[parentNode]["userName"]).ToString();
            nickName = JSON.GetStr(M[parentNode]["nickName"]).ToString();
        }
            
        #endregion

    }
}
