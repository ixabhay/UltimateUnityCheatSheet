using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TurnBasedTeenPatti;

namespace ShiftMenu
{

    public class OrientationManager : MonoBehaviour
    {
        public bool isPortrait;

        void Start()
        {
            SetUpDisplay();
            //SetUpDisplayPC();


        }


        public void DisconnectAndBack()
        {

            //  Debug.Log("Saved id: " + PlayerPrefs.GetString("userid"));
            //  Debug.Log("Saved password: " + PlayerPrefs.GetString("password"));
            //  Debug.Log("Saved id: " + GameObject.Find("PhotonRoom").GetComponent<TeenPattiPhotonRoom>().localUserId);
            //  Debug.Log("Saved password: " + GameObject.Find("PhotonRoom").GetComponent<TeenPattiPhotonRoom>().localPassword);

            if (PhotonNetwork.InRoom)
            {

                //  Debug.Log("Saved id: " + PlayerPrefs.GetString("userid"));
                //  Debug.Log("Saved password: " + PlayerPrefs.GetString("password"));
                //  Debug.Log("Saved id: " + GameObject.Find("PhotonRoom").GetComponent<TeenPattiPhotonRoom>().localUserId);
                //  Debug.Log("Saved password: " + GameObject.Find("PhotonRoom").GetComponent<TeenPattiPhotonRoom>().localPassword);

                //  PlayerPrefs.SetString("userid", GameObject.Find("PhotonRoom").GetComponent<TeenPattiPhotonRoom>().localUserId);
                //  PlayerPrefs.SetString("password", GameObject.Find("PhotonRoom").GetComponent<TeenPattiPhotonRoom>().localPassword);

                PhotonNetwork.LeaveRoom();
                //GameObject.Find("LocalAccount").GetComponent<LocalAccount>().LeaveGameBeforeStart();



            }

            SceneManager.LoadScene(0);

        }


        public void LeaveGame()
        {
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
                //  GameObject.Find("LocalAccount").GetComponent<LocalAccount>().LeaveGameBeforeStart();

            }

            SceneManager.LoadScene(0);

        }

        public void LeaveRoom()
        {
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
                //  GameObject.Find("LocalAccount").GetComponent<LocalAccount>().LeaveGameBeforeStart();

            }
        }



        public void SetUpDisplay()
        {
            if (isPortrait)
            {
                Screen.orientation = ScreenOrientation.Portrait;

               // #if !UNITY_ANDOIRD && !UNITY_EDITOR
               // Screen.SetResolution(480, 960, true);
               // #endif
            }
            else
            {
                Screen.orientation = ScreenOrientation.Landscape;
                
               // #if !UNITY_ANDOIRD && !UNITY_EDITOR
               //             Screen.SetResolution(960, 480, true);
               // #endif
            }
        }
        
    }
}
