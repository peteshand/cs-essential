using System;
using System.Collections.Generic;
using System.Diagnostics;
using notifier;
using System.Timers;

namespace time
{
    public class BaseTick
    {
        Action callback;
        System.Timers.Timer timer;
        int fps = 60;

        public Notifier<bool> running = new Notifier<bool>(false);

        public BaseTick(Action callback) {
            this.callback = callback;
            running.Add(onRunningChange);
        }

        void onRunningChange(bool running) {
            if (running) {
                start();
            } else {
                stop();
            }
        }

        void start() {
            stop();
            timer = new Timer(1000 / fps);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        void stop() {
            if (timer != null) {
                timer.AutoReset = true;
                timer.Enabled = false;
            }
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            callback();
        }
    }
}