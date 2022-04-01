using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

using Monopoly.Classes;
using Monopoly.Net.Packets;

namespace Monopoly.Runtime
{

    public class ClientGameStateTest
    {

        private ClientGameState cgs;
        private Player player;
        private string uuid; 
        private bool init = false;

        [UnitySetUp]
        public IEnumerator OneTimeSetUp()
        {
            if (!init)
            {
                // load the board scene
                SceneManager.LoadScene("Scenes/BoardScene");

                yield return null; // wait a frame for everything to init

                // now find the client game state
                GameObject go = GameObject.Find("ClientGameObject");
                Assert.False(go == null);
                cgs = go.GetComponent<ClientGameState>();
                Assert.False(cgs == null);

                uuid = "abc";
                player = new Player(uuid, "Test Player", null);
                cgs.ManuallyRegisterPlayer(player);

                Assert.AreEqual(1500, player.Money);

                init = true;
            }
        }

        [UnityTest]
        public IEnumerator TestHousePackets()
        {
            Square s1 = cgs.Board.GetSquare(1);
            OwnableSquare os1 = (OwnableSquare) s1;
            PropertySquare ps1 = (PropertySquare) s1;
            os1.Owner = player;

            PacketActionBuyHouseSucceed p1 =
                new PacketActionBuyHouseSucceed(uuid, 1);
            // try to buy the house only owning one of a property set
            cgs.OnBuyHouse(p1);
            yield return null;
            Assert.AreEqual(0, ps1.NbHouse);

            Square s2 = cgs.Board.GetSquare(3);
            OwnableSquare os2 = (OwnableSquare)s2;
            os2.Owner = player;

            // try to buy the house now owning both properties
            cgs.OnBuyHouse(p1);
            yield return null;
            Assert.AreEqual(1, ps1.NbHouse);

            PacketActionSellHouseSucceed p2 =
                new PacketActionSellHouseSucceed(uuid, 1);
            // try to sell the house now
            cgs.OnSellHouse(p2);
            yield return null;
            Assert.AreEqual(0, ps1.NbHouse);
        }

    }

}
