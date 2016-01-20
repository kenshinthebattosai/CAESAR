﻿using System;
using System.Linq;
using CAESAR.Chess.Helpers;
using CAESAR.Chess.Implementation;
using CAESAR.Chess.Moves;
using CAESAR.Chess.Moves.Generation;
using CAESAR.Chess.Pieces;
using Xunit;

namespace CAESAR.Chess.Tests.Moves.Generation
{
    public class BishopMovesGeneratorTests
    {
        private readonly IMovesGenerator _movesGenerator = new BishopMovesGenerator();
        private readonly IBoard _board = new Board();
        private readonly IPiece _piece = new Bishop(true);
        private readonly IPlayer _player = new Player();

        [Fact]
        public void MoveGeneratorWithoutSquareGeneratesEmptyMoves()
        {
            Assert.Equal(Enumerable.Empty<IMove>(), _movesGenerator.Moves);
        }

        [Fact]
        public void MoveGeneratorWithoutPieceGeneratesEmptyMoves()
        {
            _movesGenerator.Square = _board.GetSquare("a1");
            Assert.Equal(Enumerable.Empty<IMove>(), _movesGenerator.Moves);
        }

        [Theory]
        [InlineData("d1", "d1e2,d1f3,d1g4,d1h5,d1c2,d1b3,d1a4")]
        [InlineData("f3", "f3g4,f3h5,f3g2,f3h1,f3e2,f3d1,f3e4,f3d5,f3c6,f3b7,f3a8")]
        public void BishopAtXGeneratesYMoves(string x, string y)
        {
            var square = _board.GetSquare(x);
            _player.Place(square, _piece);
            _movesGenerator.Square = square;
            var moves = _movesGenerator.Moves;
            var moveStrings = moves.Select(move => move.ToString());
            var expectedMoveStrings = y.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToHashSet();
            Assert.True(expectedMoveStrings.SetEquals(moveStrings));
        }

        [Theory]
        [InlineData("d1", "e2,f3,g4,h5,c2,b3,a4", "")]
        [InlineData("f3", "g4,h5,g2,h1,e2,d1,e4,d5,c6,b7,a8", "")]
        [InlineData("f3", "h5,d5", "f3g4,f3g2,f3h1,f3e2,f3d1,f3e4")]
        public void BishopAtXWithOwnPiecesAtYGeneratesZMoves(string x, string y, string z)
        {
            var square = _board.GetSquare(x);
            var ownPieceSquares =
                y.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(sq => _board.GetSquare(sq));
            foreach (var ownPieceSquare in ownPieceSquares)
            {
                _player.Place(ownPieceSquare, new Pawn(true));
            }
            _player.Place(square, _piece);
            _movesGenerator.Square = square;
            var moves = _movesGenerator.Moves;
            var moveStrings = moves.Select(move => move.ToString());
            var expectedMoveStrings = z.Split(new [] {','}, StringSplitOptions.RemoveEmptyEntries).ToHashSet();
            Assert.True(expectedMoveStrings.SetEquals(moveStrings));
        }

        [Theory]
        [InlineData("d1", "e2,f3,g4,h5,c2,b3,a4", "d1e2,d1c2")]
        [InlineData("f3", "g4,h5,g2,h1,e2,d1,e4,d5,c6,b7,a8", "f3g4,f3g2,f3e2,f3e4")]
        [InlineData("f3", "h5,d5", "f3g4,f3h5,f3g2,f3h1,f3e2,f3d1,f3e4,f3d5")]
        public void BishopAtXWithEnemyPiecesAtYGeneratesZMoves(string x, string y, string z)
        {
            var square = _board.GetSquare(x);
            var ownPieceSquares =
                y.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(sq => _board.GetSquare(sq));
            foreach (var ownPieceSquare in ownPieceSquares)
            {
                _player.Place(ownPieceSquare, new Pawn(false));
            }
            _player.Place(square, _piece);
            _movesGenerator.Square = square;
            var moves = _movesGenerator.Moves;
            var moveStrings = moves.Select(move => move.ToString());
            var expectedMoveStrings = z.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToHashSet();
            Assert.True(expectedMoveStrings.SetEquals(moveStrings));
        }
    }
}