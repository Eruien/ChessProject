using UnityEngine;

namespace Assets.Scripts
{
    abstract public class BaseObject : MonoBehaviour
    {
        public BlackBoard blackBoard = new BlackBoard();
        public bool IsDeath { get; set; } = false;
        protected abstract void SetBlackBoardKey();
    }
}

 
