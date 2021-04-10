using MineSweeper;
using System;
using Xunit;

namespace Test
{
    public class UnitTest1
    {
        [Theory(DisplayName = "範囲内の座標アクセスで「IndexOutOfRangeException」が発生しない")]
        [InlineData(0, 0)]
        [InlineData(1, 0)]
        [InlineData(0, 1)]
        [InlineData(3, 4)]
        [InlineData(4, 3)]
        [InlineData(4, 4)]
        [Trait("メソッド", "Dig")]
        public void DigTest1(int row, int col)
        {
            var game = new Game(5, 5, 0);
            game.Dig(row, col);
        }

        [Theory(DisplayName = "範囲外の座標アクセスで「IndexOutOfRangeException」が発生する")]
        [InlineData(-1, 0)]
        [InlineData(0, -1)]
        [InlineData(4, 5)]
        [InlineData(5, 4)]
        [Trait("メソッド", "Dig")]
        public void DigTest2(int row, int col)
        {
            var game = new Game(5, 5, 0);

            Assert.Throws<IndexOutOfRangeException>(() => game.Dig(row, col));
        }
    }
}
