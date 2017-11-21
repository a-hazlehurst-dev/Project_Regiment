using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.Services.Helper
{
    public class FacingHelper
    {

        string mustface;
        private string _facing;


        public FacingHelper(ref string facing)
        {
            _facing = facing;
        }

        public void SetFacing(GameObject objectToMove, GameObject objectToFace, string mode)
        {

            var facingGO = objectToMove.transform.Find("NoRotation").transform.Find("facing");


            //work out which way to face
            if (objectToFace == null)
            {
                return;
            }

            if (facingGO.transform.rotation.y == 0)
            {
                _facing = "right";
            }
            if (facingGO.transform.rotation.y == 180)
            {
                _facing = "Left";
            }

            var dirToFaceValue = objectToMove.transform.position.x - objectToFace.transform.position.x;
            string dirToFace = "";
            if (dirToFaceValue > 0)
            {
                dirToFace = "left";
            }
            else if (dirToFaceValue < 0)
            {
                dirToFace = "right";

            }

           
            if (string.Compare(_facing, dirToFace) != 0)
            {
               // Debug.Log(objectToMove.gameObject + " is facing " + _facing + " and needs to face " + dirToFace + string.Compare(_facing, dirToFace));

                if (dirToFace == "left")
                {
                    
                    facingGO.transform.eulerAngles = new Vector3(0, 180, 0);
                }
                else if (dirToFace == "right")
                {
                    facingGO.transform.eulerAngles = new Vector3(0, 0, 0);
                }
            }


        }
    }
}
