using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleService.Entity
{
    public class RoomPool
    {
        public Dictionary<int, Room> Rooms = null;

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
                            instance.Rooms = new Dictionary<int, Room>(POOL_MAX);
                            for (int i = 0; i < POOL_MAX;i++ )
                            {
                                Room room = new Room(i);
                                room.roomName = "第"+i+"个房间";
                                instance.Rooms.Add(i, room);
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
                if (Rooms[i].isActive == false)
                {
                    Room room = Rooms[i];
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
            resetRoom(Rooms[roomId]);
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
