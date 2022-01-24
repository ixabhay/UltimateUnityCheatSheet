using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InstantChess
{
    public class PhotonConnect : MonoBehaviourPunCallbacks
    {
        public GameObject connectingToServer, waitingRoom;

        public bool connectedToPhoton;

        public string gameMode;


        [SerializeField] private int chessSceneIndex;
        [SerializeField] private Transform connectedPlayersListing;
        [SerializeField] private GameObject playerListingPrefab;
        int numberOfPlayersConnected;

        public ShiftLoginManager shiftLoginManager;

        // Start is called before the first frame update
        void Start()
        {
            connectedToPhoton = false;

            ConnectToPhoton();

        }

        // Update is called once per frame
        void Update()
        {

        }


        public void ConnectToPhoton()
        {
            connectingToServer.SetActive(true);

            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.ConnectUsingSettings();
            }
            else
            {
                connectedToPhoton = true;

                //  connectingToServer.SetActive(false);
            }
        }




        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            Debug.Log("Connected to: " + PhotonNetwork.CloudRegion);
            // region.text = PhotonNetwork.CloudRegion;
            /////////////////////////----custom

            //GameObject.Find("FacebookLoginManager").GetComponent<FBScript>().OpenMainMenu();
            connectingToServer.SetActive(false);
            NetworkPlayerSetup();
            Debug.Log("Connected to servers with nickname: " + PhotonNetwork.NickName);
            //   GameObject.Find("FacebookLoginManager").GetComponent<FBScript>().SetPlayerName(PhotonNetwork.NickName);

            ////////////////////////

            PhotonNetwork.JoinLobby();

        }

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            Debug.Log("Joined a lobby...");
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            Debug.Log("Disconnected from Photon server because: " + cause);
            // disconnectReason.text = cause.ToString() + " \n Try again later or restart application.";

        }



        private void NetworkPlayerSetup()
        {
            // for changin scene for all players in the room
            // PhotonNetwork.AutomaticallySyncScene = true;
            //PhotonNetwork.NickName = GameObject.Find("DatabaseHandler").GetComponent<DatabaseHandler>().GetPlayerName();
            PhotonNetwork.NickName = shiftLoginManager.localUser;
        }


        public void FindMatch()
        {
            // activate waiting room
            // join waiting room

            gameMode = "chess";

            ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable()
                {
                    { "mode",gameMode }

                };

            PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, (byte)2);


        }


        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Failed to join a room...");
            //CreateCustomRandomRoom();
            int randomInt = UnityEngine.Random.Range(1, 999);
            string roomId = "Room" + randomInt.ToString();
            string[] roomPropsInLobby = { "mode" };

            PhotonNetwork.CreateRoom(roomId, new RoomOptions
            {
                CustomRoomPropertiesForLobby = roomPropsInLobby,
                CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
                     {
                         { "mode", "chess" }
                     },
                MaxPlayers = 2,
                PublishUserId = true
            }, null);

        }





        public override void OnJoinedRoom()
        {
            if (gameMode.Equals("chess"))
            {
                JoinChessMatch();

            }


        }


        private void JoinChessMatch()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            waitingRoom.SetActive(true);
            // mainPanel.SetActive(false);
            // lobbyPanel.SetActive(false);
            //  roomNameDisplay.text = PhotonNetwork.CurrentRoom.Name;
            //  www.GetComponent<API>().OnMatchJoin();


          //  ClearPlayerListings();
            ListPlayers();
            if (PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                // startingInAnimation.SetActive(true);
                StartCoroutine("StartChessMultiplayer");
                Debug.Log("Starting match..");
            }


        }


        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
           // ClearPlayerListings();
            ListPlayers();


            CheckAndStartChess();



        }


        private void OnPlayerDisconnected(Player player)
        {
           // ClearPlayerListings();
            ListPlayers();
        }


        void CheckAndStartChess()
        {
            if (PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("mode"))
                {
                    if (PhotonNetwork.CurrentRoom.CustomProperties["mode"].ToString().Equals("chess"))
                    {
                      //  PlayerPrefs.SetInt("firstPlace", firstPlace);
                      //  PlayerPrefs.SetInt("secondPlace", secondPlace);
                        //   PlayerPrefs.SetInt("thirdPlace", thirdPlace);
                        // startingInAnimation.SetActive(true);
                        StartCoroutine("StartChessMultiplayer");
                    }

                }
            }
        }


        void ListPlayers()
        {
            ClearPlayerListings();

            numberOfPlayersConnected = 0;
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                GameObject tempListing = Instantiate(playerListingPrefab, connectedPlayersListing);
                // Text tempText = tempListing.transform.GetChild(0).GetComponent<Text>();
                tempListing.GetComponent<PlayerListing>().SetPlayerName(player.NickName);
                // string[] nickName = player.NickName;
                // tempText.text = playerName;
                numberOfPlayersConnected++;
                //connectedNumberOfPlayers.text = numberOfPlayersConnected.ToString();
            }

        }



        void ClearPlayerListings()
        {
            for (int i = connectedPlayersListing.childCount - 1; i >= 0; i--)
            {
                Destroy(connectedPlayersListing.GetChild(i).gameObject);
            }

        }


        private IEnumerator StartChessMultiplayer()
        {
            WaitForSeconds waitOne = new WaitForSeconds(1f);

            int timer = 3;

            while (timer != 0)
            {
                GameObject.Find("StartingTimer").GetComponent<Text>().text = "Starting in " + timer.ToString();
                // timerText.text = timer.ToString();
                timer--;



                yield return waitOne;

            }

            if (PhotonNetwork.IsMasterClient)
            {
                StartChessNow();
            }

        }



        public void StartChessNow()
        {
            if (numberOfPlayersConnected == 2 && PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.LoadLevel(chessSceneIndex);
                Debug.Log("Load chess scene here");
            }
            else
            {
                Debug.Log("Not enough players to start the match");
                // connectedUsersPrompt.SetActive(true);
            }
        }

    }// class ends here

} // namespace ends here