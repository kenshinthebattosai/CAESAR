﻿using System;
using CAESAR.Chess.Implementation;
using Xunit;
using System.Linq;
using Rank = CAESAR.Chess.Implementation.Rank;

namespace CAESAR.Chess.Tests
{
    public class RankTests
    {
        private readonly IBoard _board;
        private readonly IRank _rank;

        public RankTests() : this(new Board())
        {
        }

        // Injection Constructor
        private RankTests(IBoard board)
        {
            _board = board;
            _rank = new Rank(_board, 1);
        }

        [Fact]
        public void RankCannotBeConstructedWithoutBoard()
        {
            Assert.Throws<ArgumentNullException>(() => { var s = new Rank(null, 1); });
        }

        [Theory]
        [InlineData((byte)0)]
        [InlineData((byte)9)]
        [InlineData(byte.MaxValue)]
        public void RankCannotBeConstructedWithoutNameFromAtoH(byte number)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => { var s = new Rank(_board, number); });
        }

        [Fact]
        public void RankContainsReferenceToItsBoard()
        {
            Assert.Equal(_board, _rank.Board);
        }

        [Fact]
        public void StringRepresentationOfRankIsItsNumber()
        {
            Assert.Equal(_rank.Number.ToString(), _rank.ToString());
        }

        [Fact]
        public void RankCanBeQueriedForItsNumber()
        {
            Assert.NotNull(_rank.Number);
            Assert.Equal(_rank.Number, 1);
            foreach (var rank in _board.Ranks)
                Assert.True(rank.Number != 0);
        }

        [Fact]
        public void RankCanBeQueriedForItsSquares()
        {
            Assert.NotNull(_rank);
            Assert.NotEmpty(_rank);
            foreach (var rank in _board.Ranks)
                Assert.NotEmpty(rank);
        }

        [Fact]
        public void NumberOfSquaresInRankIs8()
        {
            Assert.Equal(_rank.Count, 8);
            foreach (var rank in _board.Ranks)
                Assert.Equal(rank.Count, 8);
        }

        [Fact]
        public void FileOfSquaresInRankRangeFromAtoH()
        {
            Assert.Equal(string.Join(",", _rank.Select(x => x.File.Name).OrderBy(x => x).Select(x => x.ToString())), "a,b,c,d,e,f,g,h");
            foreach (var rank in _board.Ranks)
                Assert.Equal(string.Join(",", rank.Select(x => x.File.Name).OrderBy(x => x).Select(x => x.ToString())), "a,b,c,d,e,f,g,h");
        }
    }
}