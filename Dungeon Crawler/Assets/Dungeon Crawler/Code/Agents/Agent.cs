using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SotomaYorch.DungeonCrawler
{
    #region Enums


    #endregion

    #region Structs


    #endregion

    //Agent cannot operate without the Rigidbody2D
    //[RequireComponent(typeof(Rigidbody2D))]
    public class Agent : MonoBehaviour
    {
        //Configuration parameter of this script
        #region Knobs

        public int maxHealthPoints = 3;

        #endregion

        #region References

        [SerializeField, HideInInspector] protected Rigidbody2D _rigidbody;//ponerle el hide despues de poner mis rigidbodies en el script a mi enemigos
        [SerializeField, HideInInspector] protected FiniteStateMachine _fsm;


        #endregion

        #region RuntimeVariables

        #endregion

        #region LocalMethods

        #endregion

        #region UnityMethods

        private void Start()
        {
            //InitializeAgent();
        }

        //ranges from 24 to 200 FPS
        //(according to the computer)
        void Update()
        {

        }

        //private void PhysicsUpdate()
        private void FixedUpdate()
        {
            //when we update the rigid body, we do it
            //during the Physics thread update
            //which is the FixedUpdate()
            //within the PhysX Engine (by NVidia) in Unity
            //_rigidbody.velocity = Vector3.right;
            //_rigidbody.AddForce(Vector2.right);
        }

        #endregion

        #region PublicMethods

        public virtual void InitializeAgent()
        {
            //With the RequireComponent we guarantee
            //this reference will be ALWAYS retreived
            /*
            _rigidbody = GetComponent<Rigidbody2D>();
            if (_rigidbody == null ) {
                Debug.LogError("Rigid body has not been assigned to " +
                    gameObject.name);
            }
            */
        }

        #endregion

        #region GettersSetters

        #endregion
    }
}