using System;
using System.Drawing;

namespace AoC2020_12
{
    public enum Direction
    {
        NORTH = 0,
        EAST = 1,
        SOUTH = 2,
        WEST = 3
    }

    public enum Action
    {
        NORTH, // Wish I could just do haskell's "MOVE Direction", but that'd need me to make classes
        EAST,
        SOUTH,
        WEST,
        TURNLEFT,
        TURNRIGHT,
        FORWARD
    }

    public static class Extensions
    {
        public static Direction Rotate(this Direction dir, int times)
        {
            return (Direction) Modulo((int) dir + times, 4);
        }

        public static Action ToAction(this char letter)
        {
            return letter switch
            {
                'N' => Action.NORTH,
                'E' => Action.EAST,
                'S' => Action.SOUTH,
                'W' => Action.WEST,
                'L' => Action.TURNLEFT,
                'R' => Action.TURNRIGHT,
                'F' => Action.FORWARD,
                _ => Action.FORWARD // Fallback
            };
        }

        // Hey guess what, C#'s % operator isn't modulo, it's remainder. This means -1 % 4 = -1 instead of 3.
        public static int Modulo(int num, int mod)
        {
            return (num % mod + mod) % mod;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Moveable ship     = new Ship();
            Moveable waypoint = new Waypoint();

            string line = Console.ReadLine();
            while (!string.IsNullOrEmpty(line))
            {
                char   actionLetter = line[0];
                int    amount       = int.Parse(line.Remove(0, 1));
                Action action       = actionLetter.ToAction();

                ship.Move(action, amount);
                waypoint.Move(action, amount);

                line = Console.ReadLine();
            }

            Console.WriteLine($"Ship moved {ship.Manhattan}");
            Console.WriteLine($"Waypointed ship moved {waypoint.Manhattan}");
        }
    }

    // Part 2 solution using a waypointed ship
    public class Waypoint : Moveable
    {
        private Point distanceFromShip;
        private Point shipPosition;

        public Waypoint()
        {
            distanceFromShip = new Point(10, 1);
            shipPosition = Point.Empty;
        }


        public override int Manhattan => Math.Abs(shipPosition.X) + Math.Abs(shipPosition.Y);

        public override void Move(Action action, int amount)
        {
            switch (action)
            {
                case Action.NORTH:
                    MoveDirection(Direction.NORTH, amount);
                    break;
                case Action.EAST:
                    MoveDirection(Direction.EAST, amount);
                    break;
                case Action.SOUTH:
                    MoveDirection(Direction.SOUTH, amount);
                    break;
                case Action.WEST:
                    MoveDirection(Direction.WEST, amount);
                    break;
                case Action.TURNLEFT:
                    for (int times = amount / 90; times > 0; times--)
                        distanceFromShip = new Point(-distanceFromShip.Y, distanceFromShip.X);
                    break;
                case Action.TURNRIGHT:
                    for (int times = amount / 90; times > 0; times--)
                        distanceFromShip = new Point(distanceFromShip.Y, -distanceFromShip.X);
                    break;
                case Action.FORWARD:
                    shipPosition = new Point(
                        shipPosition.X + distanceFromShip.X * amount,
                        shipPosition.Y + distanceFromShip.Y * amount
                    );
                    break;
            }
        }

        public override void MoveDirection(Direction dir, int amount)
        {
            switch (dir)
            {
                case Direction.NORTH:
                    distanceFromShip.Y += amount;
                    break;
                case Direction.EAST:
                    distanceFromShip.X += amount;
                    break;
                case Direction.SOUTH:
                    distanceFromShip.Y -= amount;
                    break;
                case Direction.WEST:
                    distanceFromShip.X -= amount;
                    break;
            }
        }
    }

    // Part 1 solution using a normal ship
    public class Ship : Moveable
    {
        private Direction facing;
        private Point pos;

        public Ship()
        {
            facing = Direction.EAST;
            pos = Point.Empty;
        }

        public override int Manhattan => Math.Abs(pos.X) + Math.Abs(pos.Y);

        public override void Move(Action action, int amount)
        {
            switch (action)
            {
                case Action.NORTH:
                    MoveDirection(Direction.NORTH, amount);
                    break;
                case Action.EAST:
                    MoveDirection(Direction.EAST, amount);
                    break;
                case Action.SOUTH:
                    MoveDirection(Direction.SOUTH, amount);
                    break;
                case Action.WEST:
                    MoveDirection(Direction.WEST, amount);
                    break;
                case Action.TURNLEFT:
                    facing = facing.Rotate(-amount / 90);
                    break;
                case Action.TURNRIGHT:
                    facing = facing.Rotate(amount / 90);
                    break;
                case Action.FORWARD:
                    MoveDirection(facing, amount);
                    break;
            }
        }

        public override void MoveDirection(Direction dir, int amount)
        {
            switch (dir)
            {
                case Direction.NORTH:
                    pos.Y += amount;
                    break;
                case Direction.EAST:
                    pos.X += amount;
                    break;
                case Direction.SOUTH:
                    pos.Y -= amount;
                    break;
                case Direction.WEST:
                    pos.X -= amount;
                    break;
            }
        }
    }

    public abstract class Moveable
    {
        public abstract int Manhattan { get; }
        public abstract void Move(Action action, int amount);
        public abstract void MoveDirection(Direction dir, int amount);
    }
}