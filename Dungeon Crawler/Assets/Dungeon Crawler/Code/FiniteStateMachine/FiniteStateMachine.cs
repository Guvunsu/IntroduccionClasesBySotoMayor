using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SotomaYorch.DungeonCrawler
{
    #region Enums

    public enum States
    {
        //IDLE
        IDLE_DOWN,
        IDLE_UP,
        IDLE_RIGHT,
        IDLE_LEFT,
        //MOVING
        MOVING_DOWN,
        MOVING_UP,
        MOVING_RIGHT,
        MOVING_LEFT
    }

    public enum StateMechanics
    {
        //STOP
        STOP,
        //MOVE
        MOVE_UP,
        MOVE_DOWN,
        MOVE_LEFT,
        MOVE_RIGHT
    }

    #endregion

    #region Structs


    #endregion

    public class FiniteStateMachine : MonoBehaviour
    {
        #region Knobs


        #endregion

        #region References

        [SerializeField,HideInInspector] protected Animator _animator;

        #endregion

        #region RuntimeVariables

        protected States _state;

        #endregion

        #region LocalMethods

        protected void InitializeFiniteStateMachine()
        {
            
        }

        protected void CleanAnimatorFlags()
        {
            foreach (StateMechanics stateMechanic in Enum.GetValues(typeof(StateMechanics)))
            {
                _animator.SetBool(stateMechanic.ToString(), false);
            }
        }

        #endregion

        #region UnityMethods

        private void Start()
        {
            InitializeFiniteStateMachine();
        }

        void Update()
        {
            
        }

        private void FixedUpdate()
        {
            
        }

        #endregion

        #region PublicMethods

        //Action
        public void StateMechanic(StateMechanics value)
        {
            _animator.SetBool(value.ToString(), true);
        }

        public void SetState(States value)
        {
            CleanAnimatorFlags();
            _state = value;
            //InitializeState();
        }

        #endregion

        #region GettersSetters



        #endregion
    }
}