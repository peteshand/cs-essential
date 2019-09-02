using System;
using System.Collections.Generic;
using System.Diagnostics;
using notifier;

namespace time
{
    public static class EnterFrame
    {
        static BaseTick tick;
        static List<Action> callbacks;
        static bool initialized = false;

        static void init() {
            if (initialized)
                return;

            callbacks = new List<Action>();

            tick = new BaseTick(onTick);
            initialized = true;
        }

        private static void onTick() {
            var i = 0;
            while (i < callbacks.Count) {
                var callback = callbacks[i];
                if (callback != null) {
                    callback();
                    i++;
                } else {
                    callbacks.RemoveAt(i);
                }
            }
        }

        static public void Add(Action callback) {
            EnterFrame.init();
            tick.running.Value = true;
            callbacks.Add(callback);
        }

        static public void AddAt(Action callback, int index) {
            EnterFrame.init();
            tick.running.Value = true;
            callbacks.Insert(index, callback);
        }

        static public void Remove(Action callback) {
            EnterFrame.init();
            int i = callbacks.Count - 1;
            while (i >= 0) {
                if (callbacks[i] == callback) {
                    callbacks.RemoveAt(i);
                }
                i--;
            }
            if (callbacks.Count == 0) {
                tick.running.Value = false;
            }
        }
    }
}