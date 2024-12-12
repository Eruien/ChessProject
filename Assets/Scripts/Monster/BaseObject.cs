using UnityEngine;

namespace Assets.Scripts
{
    abstract public class BaseObject : MonoBehaviour
    {
        public ObjectType SelfType { get; set; }

        public BlackBoard blackBoard = new BlackBoard();
        public bool IsDeath { get; set; } = false;
        protected abstract void SetBlackBoardKey();
    }
}

 
