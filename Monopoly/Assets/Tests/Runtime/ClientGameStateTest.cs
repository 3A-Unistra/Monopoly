using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

using Monopoly.Classes;
using Monopoly.Net.Packets;
using Monopoly.Net;

namespace Monopoly.Runtime
{

    public class ClientGameStateTest
    {

        private ClientGameState cgs;
        private Player player;
        private string uuid; 
        private bool init = false;

        /*[UnitySetUp]
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
                player = new Player(uuid, "Test Player 1", 0);
                cgs.ManuallyRegisterPlayer(player);

                init = true;
            }
        }

        [UnityTest, Order(0)]
        public IEnumerator TestManualCreatePlayer()
        {
            yield return new WaitForSeconds(1); // more wait for everything

            string uuid2 = "def";
            Player player2 = new Player(uuid2, "Test Player 2", 1);
            cgs.ManuallyRegisterPlayer(player2);

            uuid2 = "ghi";
            Player player3 = new Player(uuid2, "Test Player 3", 2);
            cgs.ManuallyRegisterPlayer(player3);

            Assert.AreEqual(1500, player.Money);
        }

        [UnityTest, Order(1)]
        public IEnumerator TestPropertyPurchase()
        {
            Square s1 = cgs.Board.GetSquare(1);
            PropertySquare ps1 = (PropertySquare)s1;

            Assert.AreEqual(null, ps1.Owner);

            PacketActionBuyPropertySucceed p1 =
                new PacketActionBuyPropertySucceed(uuid, 1);
            // try to buy the property
            cgs.OnBuyProperty(p1);
            yield return null;

            Assert.AreEqual(player, ps1.Owner);
        }

        [UnityTest, Order(2)]
        public IEnumerator TestHousePackets()
        {
            Square s1 = cgs.Board.GetSquare(1);
            OwnableSquare os1 = (OwnableSquare) s1;
            PropertySquare ps1 = (PropertySquare) s1;
            //os1.Owner = player;

            PacketActionBuyHouseSucceed p1 =
                new PacketActionBuyHouseSucceed(uuid, 1);
            // try to buy the house only owning one of a property set
            cgs.OnBuyHouse(p1);
            yield return new WaitForSeconds(3);
            Assert.AreEqual(0, ps1.NbHouse);

            Square s2 = cgs.Board.GetSquare(3);
            OwnableSquare os2 = (OwnableSquare)s2;
            os2.Owner = player;

            // try to buy the house now owning both properties
            cgs.OnBuyHouse(p1);
            yield return new WaitForSeconds(3);
            Assert.AreEqual(1, ps1.NbHouse);

            PacketActionSellHouseSucceed p2 =
                new PacketActionSellHouseSucceed(uuid, 1);
            // try to sell the house now
            cgs.OnSellHouse(p2);
            yield return new WaitForSeconds(8);
            Assert.AreEqual(0, ps1.NbHouse);
        }*/

    }

}
