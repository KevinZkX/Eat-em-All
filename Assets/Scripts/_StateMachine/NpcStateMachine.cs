using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateStuff
{
    public abstract class NpcState<T>
    {
        public abstract void EnterState(T _owner);
        public abstract void ExitState(T _owner);
        public abstract void UpdateState(T _owner);
    }

    public class NpcStateMachine<T> 
    {
        public NpcState<T> current_state
        {get; private set;
        }
        public T owner;
        //constructor
        public NpcStateMachine(T _o)
        {
            owner = _o;
            current_state = null;
        }

        public void changeState(NpcState<T> _newstate)

        {
            if (current_state != null)
                current_state.EnterState(owner);
            current_state = _newstate;
            current_state.EnterState(owner);
        }
        
        // Update is called once per frame
        void Update()
        {
            if (current_state != null)
                current_state.UpdateState(owner);
        }
    }

}