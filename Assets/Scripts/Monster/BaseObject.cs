using UnityEngine;

namespace Assets.Scripts
{
    abstract public class BaseObject : MonoBehaviour
    {
        public BlackBoard blackBoard = new BlackBoard();

        public ObjectType SelfType { get; set; }

        public bool IsDeath { get; set; } = false;

        public abstract void SetPosition(float x, float y, float z);
        protected abstract void SetBlackBoardKey();
    }
}

 
