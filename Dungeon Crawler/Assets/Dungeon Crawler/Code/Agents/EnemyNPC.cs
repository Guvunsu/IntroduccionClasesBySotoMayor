using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SotomaYorch.DungeonCrawler
{
    #region Enums

    public enum EnemyBehaviourState
    {
        PATROL,
        PERSECUTION
    }

    #endregion

    #region Structs


    #endregion

    public class EnemyNPC : Agent
    {
        #region Knobs

        public EnemyBehaviours_ScriptableObject scriptBehaviours;

        #endregion

        #region References

        #endregion

        #region RuntimeVariables

        protected EnemyBehaviourState _currentEnemyBehaviourState;
        protected EnemyBehaviour _currentEnemyBehaviour;
        protected int _currentEnemyBehaviourIndex;

        #endregion

        #region LocalMethods

        protected void InvokeStateMechanic()
        {
            switch (_currentEnemyBehaviour.type)
            {
                case EnemyBehaviourType.STOP:
                    _fsm.StateMechanic(StateMechanics.STOP);
                    break;
                case EnemyBehaviourType.FIRE:
                    //_fsm.StateMechanic(StateMechanics.MOVE_RIGHT);
                    break;
                case EnemyBehaviourType.MOVE_TO_RANDOM_DIRECTION:
                case EnemyBehaviourType.PERSECUTE_THE_AVATAR:
                    //TODO: Obtain the State Mechanic direction, 
                    //according to the direction of the enemy
                    _fsm.StateMechanic(StateMechanics.MOVE_RIGHT);
                    break;
            }
        }

        protected IEnumerator TimerForEnemyBehaviour()
        {
            yield return new WaitForSeconds(_currentEnemyBehaviour.time);
            FinalizeSubState();
            GoToNextEnemyBehaviour();
        }

        protected void GoToNextEnemyBehaviour()
        {
            _currentEnemyBehaviourIndex++;
            if (_currentEnemyBehaviourState == EnemyBehaviourState.PATROL)
            {
                if (_currentEnemyBehaviourIndex >= scriptBehaviours.patrolBehaviours.Length)
                    _currentEnemyBehaviourIndex = 0;
                _currentEnemyBehaviour = scriptBehaviours.patrolBehaviours[_currentEnemyBehaviourIndex];
            }
            else // PERSECUTING
            {
                if (_currentEnemyBehaviourIndex >= scriptBehaviours.persecutionBehaviours.Length)
                    _currentEnemyBehaviourIndex = 0;
                _currentEnemyBehaviour = scriptBehaviours.persecutionBehaviours[_currentEnemyBehaviourIndex];
            }
            InvokeStateMechanic();
            if (_currentEnemyBehaviour.time > 0)
            {
                //It is not a perpetual finite state,
                //so we will start the clock ;)
                StartCoroutine(TimerForEnemyBehaviour());
            }
        }

        #endregion

        #region UnityMethods

        void Start()
        {
            InitializeAgent();
        }
        void FixedUpdate()
        {
            switch (_currentEnemyBehaviour.Type)
            {
                case EnemyBehaviourType.Stop:
                    ExecutingStopSubStateMachine():
                        break;
                case EnemyBehaviourType.MOVE_TO_RANDOM_DIRECTION:
                    ExecutingMoveToRandomDirectionSubStateMachine();
                    break;
                case EnemyBehaviourType.PERSECUTE_THE_AVATAR:
                    ExcecutingPersecuteTheAvatarSubstateMachineMethods();
                    break;
            }
        }
        protected void FinalizeSubState()
        {
            switch (_currentEnemyBehaviour.Type)
            {
                case EnemyBehaviourType.Stop:
                    FinalizeStopSubStateMachine():
                        break;
                case EnemyBehaviourType.MOVE_TO_RANDOM_DIRECTION:
                    FinalizeMoveToRandomDirectionSubStateMachine();
                    break;
                case EnemyBehaviourType.PERSECUTE_THE_AVATAR:
                    FinalizePersecuteTheAvatarSubstateMachineMethods();
                    break;
            }
            void Update()
            {

            }

        #endregion

            #region PublicMethods

        public override void InitializeAgent()
        {
            //To initialize the sub-finite state machine
            _currentEnemyBehaviourState = EnemyBehaviourState.PATROL;
            _currentEnemyBehaviourIndex = 0;

            if (scriptBehaviours.patrolBehaviours.Length > 0)
            {
                _currentEnemyBehaviour = scriptBehaviours.patrolBehaviours[0];
            }
            else
            {
                //Plan if the array is empty for this enemy NPC
                _currentEnemyBehaviour.type = EnemyBehaviourType.STOP;
                _currentEnemyBehaviour.time = -1; //-1 equals an infinity / perpetual state
            }
            InvokeStateMechanic();

            //initialize the proper substate
            InitializeSubstate();
            if (_currentEnemyBehaviour.time > 0)
            {
                //It is not a perpetual finite state,
                //so we will start the clock ;)
                StartCoroutine(TimerForEnemyBehaviour());
            }
        }
        protected void InitializeSubstate()
        {
            switch (_currentEnemyBehaviour.type)
            {
                case EnemyBehaviourType.STOP:
                    InitializeBehaviourType.MOVE_TO_RANDOM_DIRECTIOPN:
                        InitializeMoveToRandomDirectionSubStateMachine();
                    break;
                case EnemyBehaviourType.PERSECUTE_THE_AVATAR:
                    InitializePersecuteTheAvatarSubStateMachine();
                    break;
            }
        }
        #endregion

        #region GettersSetters

        #endregion

        #region SubStateMachineStates

        #region StopSubstateMachineMethods
        protected void InitializeStopSubstateMachineMethods()
        {

        }
        protected void ExcecutingStopSubstateMachineMethods()
        {

        }
        protected void FinalizeStopSubstateMachineMethods()
        {

        }
        #endregion StopSubstateMachineMethods

        #region MoveToRandomDirectionSubStateMachineMethods
        protected void InitializeMoveToRandomDirectionSubStateMachineMethods()
        {
            _rigidbody.velocity =
                new Vector2(UnityEngine.Random.Range(-1.0f, 1.0f),
                Unity.Engine.Random.Range(-1.0f, 1.0f).normalized * _currentEnemyBehaviour.speed;
        }
        protected void ExcecutingMoveToRandomDirectionSubStateMachineMethods()
        {

        }
        protected void FinalizeMoveToRandomDirectionSubStateMachineMethods()
        {
            _rigidbody.velocity = Vector2.zero;
        }
        #endregion MoveToRandomDirectionSubstateMachineMethods

        #region PersecuteTheAvatarSubstateMachineMethods
        protected void InitializePersecuteTheAvatarSubstateMachineMethods()
        {

        }
        protected void ExcecutingPersecuteTheAvatarSubstateMachineMethods()
        {

        }
        protected void FinalizePersecuteTheAvatarSubstateMachineMethods()
        {

        }
        #endregion PersecuterTheAvatarSubstateMachineMethods

        #endregion SubStateMachineStates
    }
}