using BattleShips;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Pong
{
    public enum MessageType { movement, snapshot, join, initialJoin, chatMessage, chatUpdate, sendBoard }
    public enum Direction { up, down }

    [Serializable]
    public class NetworkMessage
    {
        public MessageType type;
        public NetworkMessageBase message;


    }

    [Serializable]
    public class NetworkMessageBase
    {
    
    }

    [Serializable]
    public class PlayerMovementUpdate : NetworkMessageBase
    {
        public Direction direction;
    }

    [Serializable]
    public class SnapShot : NetworkMessageBase
    {
        public List<float> playerYPos;
        public int ballXPos;
        public int ballYPos;
    }

    [Serializable]
    public class JoinMessage : NetworkMessageBase
    {
        public string playerName;
        public int ResolutionX;
        public int ResolutionY;
    }
    [Serializable]
    public class ChatMessage : NetworkMessageBase
    {
        public string chatMessage;
        public string Name;
    }
    public class UpdateChat : NetworkMessageBase
    {
        public string LastMessage;
        public string Name;
    }

    [Serializable]
    public class SetInitialPositionsMessage : NetworkMessageBase
    {
        public int leftPlayerXPos;
        public int leftPlayeryYPos;
        public int rightPlayeryYPos;
        public int rightPlayeryXPos;
        public int ballXpos;
        public int ballYPos;
    }
    [Serializable]
    public class SendBoard : NetworkMessageBase
    {
        public Dictionary<Point, int> Board { get; set; }
        public string Name;
    }
}
