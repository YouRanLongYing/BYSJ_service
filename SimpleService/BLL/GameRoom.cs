using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleService.BLL
{
    public class GameRoom
    {
        #region Field
        private Session roomOwner;

        protected Session RoomOwner
        {
            get { return roomOwner; }
            set { roomOwner = value; }
        }



        #endregion
    }
}
