using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class BlackBoard
    {
        public b_float m_HP = new b_float();
        public b_float m_SearchRange = new b_float();
        public b_float m_AttackDistance = new b_float();
        public b_float m_AttackRange = new b_float();
        public b_GameObject m_TargetObject = new b_GameObject();
    }
}
