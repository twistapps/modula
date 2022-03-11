using System;
using UnityEngine;

namespace Modula.Optimization
{
    public class TimingConstraints
    {
        private readonly Action _constrainedAction;

        private bool _constrainFrames, _constrainSeconds;
        private int _currentFrame;
        private int _frames;
        private float _seconds;

        private float _timer;

        public TimingConstraints(Action actionToConstrain)
        {
            _constrainedAction = actionToConstrain;
        }

        public TimingConstraints SetFrames(int frames)
        {
            _constrainFrames = frames > 0;
            _frames = frames;
            return this;
        }

        public TimingConstraints SetSeconds(float seconds)
        {
            _constrainSeconds = seconds > 0;
            _seconds = seconds;
            return this;
        }

        public TimingConstraints SetOrder(int epsilons)
        {
            _timer = float.Epsilon * epsilons;
            _currentFrame = 1 * epsilons;
            return this;
        }

        private void Call()
        {
            _timer = 0;
            _currentFrame = 0;
            _constrainedAction();
        }

        public void Update()
        {
            Update(Time.deltaTime);
        }

        public void Update(float deltaTime)
        {
            if (!_constrainFrames && !_constrainSeconds)
            {
                Call();
                return;
            }

            _timer += deltaTime;
            if (_constrainSeconds && _timer >= _seconds) Call();

            _currentFrame++;
            if (_constrainFrames && _currentFrame >= _frames) Call();
        }
    }
}