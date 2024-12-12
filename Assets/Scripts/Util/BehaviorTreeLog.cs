using UnityEngine;

namespace Assets.Scripts
{
    public interface IBlackBoardKey<T>  
    {
        T Key { get; set; }
    }

    public enum ReturnCode
    { 
        FAIL, 
        SUCCESS, 
        RUNNING, 
    };

    public enum KeyQuery
    {
        None,
        IsEqualTo,
        IsNotEqualTo,
        IsLessThan,
        IsLessThanOrEqualTo,
        IsGreaterThan,
        IsGreaterThanOrEqualTo,
        IsSet,
        IsNotSet,
    };

    public class b_bool : IBlackBoardKey<bool>
    {
        public bool Key { get; set; }
    }

    public class b_int : IBlackBoardKey<int>
    {
        public int Key { get; set; }
    }

    public class b_float : IBlackBoardKey<float>
    {
        public float Key { get; set; }
    }

    public class b_GameObject : IBlackBoardKey<GameObject>
    {
        public GameObject Key { get; set; }
    }

    public class b_Character : IBlackBoardKey<Character>
    {
        public Character Key { get; set; }
    }
}
