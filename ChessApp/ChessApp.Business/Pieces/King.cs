﻿using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Business.Pieces
{
    public class King : BindableBase, IPiece
    {
        public King(bool isWhite, Tile position)
        {
            IsWhite = isWhite;
            Position = position;
        }

        public int Moves { get; set; }

        public bool IsWhite { get; }

        public Tile Position { get; set; }

        public char Character => IsWhite ? 'K' : 'k';

        public IEnumerable<Tile> GetAttackedTiles(ChessBoard board)
        {
            var tiles = new List<Tile>();
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i != 0 || j != 0)
                    {
                        tiles.Add(Tile.Move(Position, i, j));
                    }
                }
            }
            return tiles.Where(tile => tile.IsOnBoard(board));
        }

        public IEnumerable<Tile> GetMoveableTiles(ChessBoard board)
        {
            return MovementMethods.FilteredTilesWhereOppositeColour(GetAttackedTiles(board), this, board);
        }
    }
}