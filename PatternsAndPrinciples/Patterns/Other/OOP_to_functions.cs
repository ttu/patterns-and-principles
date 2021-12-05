using System;
using System.Diagnostics;
using Xunit;

namespace PatternsAndPrinciples.Patterns.Other
{
    public class OopToFunctions
    {
        public abstract class Robot
        {
            protected int Speed { get; set; } = 1;
            protected int Direction { get; set; } = 2;
            protected int CurrentLocation { get; set; }
            protected int DistanceToObstacle { get; set; }
            public bool IsActive { get; set; } = true;

            public abstract void Drive();

            public abstract void Seek();
        }

        public class AcmeVacuum : Robot
        {
            public override void Drive()
            {
                CurrentLocation += Direction * Speed;
                Trace.WriteLine($"ACME drive: {CurrentLocation}");
            }

            public override void Seek()
            {
                DistanceToObstacle = new Random().Next(0, 100); // Get distance from the sensor
                Trace.WriteLine($"ACME seek: {DistanceToObstacle}");

                if (DistanceToObstacle > 10)
                {
                    Speed = 100;
                }
                else
                {
                    // TODO: Evade etc.
                    if (DistanceToObstacle == 0)
                        IsActive = false;
                }
            }
        }

        [Fact]
        public void Robot_OOP()
        {
            var vacuumRobot = new AcmeVacuum();

            void Execute(Robot robot)
            {
                while (robot.IsActive)
                {
                    robot.Seek();
                    robot.Drive();
                }
            }

            Execute(vacuumRobot);
        }

        // FP way
        // Separate data and functions

        public class RobotData
        {
            public int Speed { get; set; }
            public int CurrentLocation { get; set; }
            public int Direction { get; set; }
            public int DistanceToObstacle { get; set; }
            public bool IsActive { get; set; }
        }

        public RobotData AcmeDrive(RobotData data)
        {
            var newlocation = data.CurrentLocation + data.Speed * data.Direction;
            Trace.WriteLine($"ACME drive: {newlocation}");

            return new RobotData
            {
                Speed = data.Speed,
                Direction = data.Direction,
                CurrentLocation = newlocation,
                DistanceToObstacle = data.DistanceToObstacle,
                IsActive = data.IsActive
            };
        }


        public RobotData AcmeSeek(RobotData data, Func<int> getDistence)
        {
            // Get distance from the sensor. 
            // Distance is wrapped to "IoMonad" and passed as a parameter
            // This way Seek is a pure function (Haskell way pure as IO is "outsourced")
            var newDistance = getDistence();
            // Non pure: var newDistance = new Random().Next(0, 100);

            Trace.WriteLine($"ACME seek: {newDistance}");

            var newSpeed = newDistance > 10 ? 100 : 0;
            var newIsActive = newDistance != 0 ? true : false;
            // TODO: Evade etc.

            return new RobotData
            {
                Speed = newSpeed,
                Direction = data.Direction,
                CurrentLocation = data.CurrentLocation,
                DistanceToObstacle = newDistance,
                IsActive = newIsActive
            };
        }

        // Get a random value as a distance. DistanceIoMonad
        private int GetDistanceFromSensor() => new Random().Next(0, 100);

        [Fact]
        public void Robot_FP()
        {
            var acmeRobot = new RobotData
            {
                Speed = 1,
                CurrentLocation = 2,
                IsActive = true,
            };

            void Execute(RobotData data)
            {
                if (data.IsActive == false) return;

                Execute(AcmeDrive(AcmeSeek(data, GetDistanceFromSensor)));
            }

            Execute(acmeRobot);
        }

        [Fact]
        public void Robot_FP_HighOrderExecute()
        {
            var acmeRobot = new RobotData
            {
                Speed = 1,
                CurrentLocation = 2,
                IsActive = true
            };

            void Execute(RobotData data, Func<RobotData, RobotData> drive, Func<RobotData, Func<int>, RobotData> seek)
            {
                if (data.IsActive == false) return;

                Execute(drive(seek(data, GetDistanceFromSensor)), drive, seek);
            }

            Execute(acmeRobot, AcmeDrive, AcmeSeek);
        }
    }
}