using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleService.Entity
{
    public class RoomPool
    {
        private Dictionary<int, Room> poolInstance = null;

        public const int POOL_MAX = 10;
        static private RoomPool instance = null;
        static object obj = new object();
        public static RoomPool Instance
        {
            private set { }
            get 
            {
                if (instance == null)
                {
                    lock (obj)
                    {
                        if (instance == null)
                        {
                            instance = new RoomPool();
                            instance.poolInstance = new Dictionary<int, Room>(POOL_MAX);
                            for (int i = 0; i < POOL_MAX;i++ )
                            {
                                Room room = new Room(i);
                                instance.poolInstance.Add(i, room);
                            }
                        }
                    }

                }
                return instance;
            }
        }

        public Room GetRoom(Player owner,List<Player> players)
        {
            for (int i = 0; i < POOL_MAX;i++ )
            {
                if (poolInstance[i].isActive == false)
                {
                    Room room = poolInstance[i];
                    room.owner = owner;
                    room.players=players;
                    foreach (Player p in players)
                    {
                        p.roomId = i;
                    }
                    room.isActive = true;
                    return room;
                }
            }
            Console.WriteLine(DateTime.Now.ToString("yyyy MM dd:hh mm ss:fff   ")+"Room pool is full!");
            return null;
        }

        public void resetRoom(int roomId)
        {
            resetRoom(poolInstance[roomId]);
        }

        public void resetRoom(Room room)
        {
            foreach(Player p in room.players)
            {
                p.roomId=-1;
            }
            room.players.Clear();
            room.isActive=false;
            
        }

    }
}
