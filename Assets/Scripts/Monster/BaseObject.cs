using UnityEngine;

namespace Assets.Scripts
{
    abstract public class BaseObject : MonoBehaviour
    {
        public BlackBoard blackBoard = new BlackBoard();

        public ObjectType SelfType { get; set; }

        public bool IsDeath { get; set; } = false;
        public int ObjectId { get; set; } = 0;

        public abstract void SetPosition(float x, float y, float z);
        public abstract void Death();
        protected abstract void SetBlackBoardKey();
    }
}

 
