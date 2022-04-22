/**
 * Generously ripped from StackOverflow at https://stackoverflow.com/a/66230329
 * and then modified to meet our needs.
 */

using System;
using UnityEngine;

namespace Monopoly
{

    public class WaitUntilForSeconds : CustomYieldInstruction
    {

        private float timer;
        private Func<bool> myChecker;

        public WaitUntilForSeconds(Func<bool> myChecker, float pauseTime)
        {
            this.myChecker = myChecker;
            this.timer = pauseTime;
        }

        public override bool keepWaiting
        {
            get
            {
                bool checkThisTurn = myChecker();
                if (checkThisTurn)
                {
                    return false; // expression failed, we're done
                }
                timer -= Time.deltaTime;

                if (timer <= 0)
                    return false;

                return true;
            }
        }
    }

}
