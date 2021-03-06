﻿using MyGame.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Model
{
    internal sealed class Player
    {
        private Input input;

        private Map map;

        public readonly float MAXSPEED = 300f;

        public Vector2 Position { get; set; }

        public float Radius { get; private set; }

        public float Angle { get; private set; }

        public PlayerState State { get; private set; }

        public Player(Input input, Map map )
        {
            this.input = input;
            this.map = map;
            Radius = 10;
        }

        public void Update(TimeSpan frameTime)
        {
            // Bewegungsrichtung laut Input
            Vector2 velocity = new Vector2(
                (input.Left ? -1f : 0f) + (input.Right ? 1f : 0f), 
                (input.Up ? -1f : 0f) + (input.Down ? 1f : 0f));

            velocity = velocity.Normalized();

            // Ermittlung der Oberfläche
            int cellX = (int)(Position.X / Map.CELLSIZE);
            int cellY = (int)(Position.Y / Map.CELLSIZE);
            CellType cellType = map.Cells[cellX, cellY];

            // Modifikation der Geschwindigkeit
            switch (cellType)
            {
                case CellType.Gras:
                    velocity *= 1f;
                    break;
                case CellType.Sand:
                    velocity *= 0.5f;
                    break;
            }

            // Bewegungsberechnung
            if (velocity.Length() > 0f)
            {
                State = PlayerState.Walk;
                Angle = velocity.Angle();
                Position += (velocity * MAXSPEED * (float)frameTime.TotalSeconds);
            } 
            else
            {
                State = PlayerState.Idle;
            }           
        }

        internal enum PlayerState
        {
            Idle,
            Walk
        }

    }
}
