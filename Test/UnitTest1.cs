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

        [Fact(DisplayName = "爆弾を踏んだ時に、GameStateが「GameOver」になる")]
        [Trait("メソッド", "Dig")]
        public void DigTest3()
        {
            var game = new Game(1, 1, new[] { (0, 0) });
            game.Dig(0, 0);

            var expected = GameState.GameOver;
            var actual = game.State;
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "爆弾を踏まなかったとき、GameStateが変わらない")]
        [Trait("メソッド", "Dig")]
        public void DigTest4()
        {
            var game = new Game(2, 2, new[] { (0, 0) });
            game.Dig(0, 1);

            var expected = GameState.Playing;
            var actual = game.State;
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "残りのマスが爆弾だけになったとき、GameStateが「GameClear」になる")]
        [Trait("メソッド", "Dig")]
        public void DigTest5()
        {
            var game = new Game(2, 2, new[] { (0, 0) });
            game.Dig(0, 1);
            game.Dig(1, 0);
            game.Dig(1, 1);

            var expected = GameState.GameClear;
            var actual = game.State;
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "セルの状態が「Flagged」のときは、状態が変わらない")]
        [Trait("メソッド", "Dig")]
        public void DigTest6()
        {
            var game = new Game(2, 2);
            game.Flag(0, 0);
            game.Dig(0, 0);

            var expected = CellState.Flagged;
            var actual = game.GetCellState(0,0);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "セルの状態が「Question」のときは、状態が変わらない")]
        [Trait("メソッド", "Dig")]
        public void DigTest7()
        {
            var game = new Game(2, 2);
            game.Flag(0, 0);
            game.Flag(0, 0);
            game.Dig(0, 0);

            var expected = CellState.Question;
            var actual = game.GetCellState(0, 0);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "セルの状態が「Initial」のときは、状態が「Digged」に変わる")]
        [Trait("メソッド", "Dig")]
        public void DigTest8()
        {
            var game = new Game(2, 2);
            game.Dig(0, 0);

            var expected = CellState.Digged;
            var actual = game.GetCellState(0, 0);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "セルの状態が「Digged」のときは、状態が変わらない")]
        [Trait("メソッド", "Dig")]
        public void DigTest9()
        {
            var game = new Game(2, 2);
            game.Dig(0, 0);
            game.Dig(0, 0);

            var expected = CellState.Digged;
            var actual = game.GetCellState(0, 0);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "ゲームの状態が「GameClear」のときは、Digできない")]
        [Trait("メソッド", "Dig")]
        public void DigTest10()
        {
            var game = new Game(2, 2, (0,0));
            game.Dig(1, 0);
            game.Dig(0, 1);
            game.Dig(1, 1);

            // この時点でGameClearになる

            game.Dig(0, 0);

            var expected = GameState.GameClear;
            var actual = game.State;
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "ゲームの状態が「GameOver」のときは、Digできない")]
        [Trait("メソッド", "Dig")]
        public void DigTest11()
        {
            var game = new Game(2, 2, (0, 0));
            game.Dig(0, 0);

            // この時点でGameOverになる

            game.Dig(0, 1);

            // (0,1)を掘っても、状態が「Initial」のままであることを確認
            var expected = CellState.Initial;
            var actual = game.GetCellState(0,1);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "掘った場所の周りの爆弾が0個のとき周りも再帰的に掘る")]
        [Trait("メソッド", "Dig")]
        public void DigTest12()
        {
            var game = new Game(5, 5);
            game.Dig(2, 2);

            // すべて掘られる
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Assert.Equal(CellState.Digged , game.GetCellState(i, j));
                }
            }
        }

        [Fact(DisplayName = "掘った場所の周りの爆弾が0個以外のときは再帰的に掘らない")]
        [Trait("メソッド", "Dig")]
        public void DigTest13()
        {
            var game = new Game(5, 5, (1, 2), (3, 3));

            game.Dig(0, 0);

            Assert.Equal(CellState.Initial, game.GetCellState(0, 2));
            Assert.Equal(CellState.Initial, game.GetCellState(0, 3));
            Assert.Equal(CellState.Initial, game.GetCellState(0, 4));
            Assert.Equal(CellState.Initial, game.GetCellState(1, 2));
            Assert.Equal(CellState.Initial, game.GetCellState(1, 3));
            Assert.Equal(CellState.Initial, game.GetCellState(1, 4));
            Assert.Equal(CellState.Initial, game.GetCellState(2, 3));
            Assert.Equal(CellState.Initial, game.GetCellState(2, 4));
            Assert.Equal(CellState.Initial, game.GetCellState(3, 3));
            Assert.Equal(CellState.Initial, game.GetCellState(3, 4));
            Assert.Equal(CellState.Initial, game.GetCellState(4, 3));
            Assert.Equal(CellState.Initial, game.GetCellState(4, 4));
        }

        [Fact(DisplayName = "周りのセルを掘る。中心は掘らない")]
        [Trait("メソッド", "Dig")]
        public void DigAroundTest1()
        {
            var game = new Game(3, 4, (1, 1));

            game.DigAround(1, 1);
            Assert.Equal(CellState.Digged, game.GetCellState(0, 0));
            Assert.Equal(CellState.Digged, game.GetCellState(0, 1));
            Assert.Equal(CellState.Digged, game.GetCellState(0, 2));
            Assert.Equal(CellState.Digged, game.GetCellState(1, 0));
            Assert.Equal(CellState.Initial, game.GetCellState(1, 1));
            Assert.Equal(CellState.Digged, game.GetCellState(1, 2));
            Assert.Equal(CellState.Digged, game.GetCellState(2, 0));
            Assert.Equal(CellState.Digged, game.GetCellState(2, 1));
            Assert.Equal(CellState.Digged, game.GetCellState(2, 2));
            Assert.Equal(GameState.Playing, game.State);
        }

        [Fact(DisplayName = "周りのセルを掘る。爆弾がある場合はゲームオーバーになる")]
        [Trait("メソッド", "Dig")]
        public void DigAroundTest2()
        {
            var game = new Game(3, 4, (0, 0));

            game.DigAround(1, 1);
            Assert.Equal(GameState.GameOver, game.State);
        }

        [Theory(DisplayName = "範囲内の座標アクセスで「IndexOutOfRangeException」が発生しない")]
        [InlineData(0, 0)]
        [InlineData(1, 0)]
        [InlineData(0, 1)]
        [InlineData(3, 4)]
        [InlineData(4, 3)]
        [InlineData(4, 4)]
        [Trait("メソッド", "Flag")]
        public void FlagTest1(int row, int col)
        {
            var game = new Game(5, 5, 0);
            game.Flag(row, col);
        }

        [Theory(DisplayName = "範囲外の座標アクセスで「IndexOutOfRangeException」が発生する")]
        [InlineData(-1, 0)]
        [InlineData(0, -1)]
        [InlineData(4, 5)]
        [InlineData(5, 4)]
        [Trait("メソッド", "Flag")]
        public void FlagTest2(int row, int col)
        {
            var game = new Game(5, 5, 0);

            Assert.Throws<IndexOutOfRangeException>(() => game.Flag(row, col));
        }

        [Fact(DisplayName = "「Initial」状態のセルに「Flag」を実行すると、状態が「Flagged」になる")]
        [Trait("メソッド", "Flag")]
        public void FlagTest3()
        {
            var game = new Game(1, 1, 0);
            game.Flag(0, 0);

            var expected = CellState.Flagged;
            var actrual = game.GetCellState(0, 0);
            Assert.Equal(expected, actrual);
        }

        [Fact(DisplayName = "「Flagged」状態のセルに「Flag」を実行すると、状態が「Question」になる")]
        [Trait("メソッド", "Flag")]
        public void FlagTest4()
        {
            var game = new Game(1, 1, 0);
            game.Flag(0, 0);
            game.Flag(0, 0);

            var expected = CellState.Question;
            var actrual = game.GetCellState(0, 0);
            Assert.Equal(expected, actrual);
        }

        [Fact(DisplayName = "「Question」状態のセルに「Flag」を実行すると、状態が「Initial」になる")]
        [Trait("メソッド", "Flag")]
        public void FlagTest5()
        {
            var game = new Game(1, 1, 0);
            game.Flag(0, 0);
            game.Flag(0, 0);
            game.Flag(0, 0);

            var expected = CellState.Initial;
            var actrual = game.GetCellState(0, 0);
            Assert.Equal(expected, actrual);
        }

        [Fact(DisplayName = "「Digged」状態のセルに「Flag」を実行しても、状態が「Digged」のまま変わらない")]
        [Trait("メソッド", "Flag")]
        public void FlagTest6()
        {
            var game = new Game(1, 1, 0);
            game.Dig(0, 0);
            game.Flag(0, 0);

            var expected = CellState.Digged;
            var actrual = game.GetCellState(0, 0);
            Assert.Equal(expected, actrual);
        }

        [Fact(DisplayName = "ゲームの状態が「GameClear」のときは、Flagできない")]
        [Trait("メソッド", "Flag")]
        public void FlagTest7()
        {
            var game = new Game(2, 2, (0, 0));
            game.Dig(1, 0);
            game.Dig(0, 1);
            game.Dig(1, 1);

            // この時点でGameClearになる

            game.Flag(0, 0);

            var expected = CellState.Initial;
            var actual = game.GetCellState(0, 0);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "ゲームの状態が「GameOver」のときは、Flagできない")]
        [Trait("メソッド", "Flag")]
        public void FlagTest8()
        {
            var game = new Game(2, 2, (0, 0));
            game.Dig(0, 0);

            // この時点でGameOverになる

            game.Flag(0, 1);

            // (0,1)にFlagしようとしても、状態が「Initial」のままであることを確認
            var expected = CellState.Initial;
            var actual = game.GetCellState(0, 1);
            Assert.Equal(expected, actual);
        }

        [Theory(DisplayName = "範囲内の座標アクセスで「IndexOutOfRangeException」が発生しない")]
        [InlineData(0, 0)]
        [InlineData(1, 0)]
        [InlineData(0, 1)]
        [InlineData(3, 4)]
        [InlineData(4, 3)]
        [InlineData(4, 4)]
        [Trait("メソッド", "CountNeighborMines")]
        public void CountNeighborMinesTest1(int row, int col)
        {
            var game = new Game(5, 5, 0);
            game.CountNeighborMines(row, col);
        }

        [Theory(DisplayName = "範囲外の座標アクセスで「IndexOutOfRangeException」が発生する")]
        [InlineData(-1, 0)]
        [InlineData(0, -1)]
        [InlineData(4, 5)]
        [InlineData(5, 4)]
        [Trait("メソッド", "CountNeighborMines")]
        public void CountNeighborMinesTest2(int row, int col)
        {
            var game = new Game(5, 5, 0);

            Assert.Throws<IndexOutOfRangeException>(() => game.CountNeighborMines(row, col));
        }

        [Fact(DisplayName = "左上角のセルの周りの地雷の数を確認 0個のとき")]
        [Trait("メソッド", "CountNeighborMines")]
        public void CountNeighborMinesTest3()
        {
            var game = new Game(5, 5, 0);

            var expected = 0;
            var actual = game.CountNeighborMines(0, 0);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "左上角のセルの周りの地雷の数を確認 1個のとき")]
        [Trait("メソッド", "CountNeighborMines")]
        public void CountNeighborMinesTest4()
        {
            var game = new Game(5, 5, new[] {(0, 1) });

            var expected = 1;
            var actual = game.CountNeighborMines(0, 0);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "左上角のセルの周りの地雷の数を確認 3個のとき(最大)")]
        [Trait("メソッド", "CountNeighborMines")]
        public void CountNeighborMinesTest5()
        {
            var game = new Game(5, 5, new[] {(0, 1), (1, 0), (1, 1) });

            var expected = 3;
            var actual = game.CountNeighborMines(0, 0);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "右上角のセルの周りの地雷の数を確認 0個のとき")]
        [Trait("メソッド", "CountNeighborMines")]
        public void CountNeighborMinesTest6()
        {
            var game = new Game(5, 5, 0);

            var expected = 0;
            var actual = game.CountNeighborMines(0, 4);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "右上角のセルの周りの地雷の数を確認 1個のとき")]
        [Trait("メソッド", "CountNeighborMines")]
        public void CountNeighborMinesTest7()
        {
            var game = new Game(5, 5, new[] { (1, 3) });

            var expected = 1;
            var actual = game.CountNeighborMines(0, 4);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "右上角のセルの周りの地雷の数を確認 3個のとき(最大)")]
        [Trait("メソッド", "CountNeighborMines")]
        public void CountNeighborMinesTest8()
        {
            var game = new Game(5, 5, new[] { (1, 3), (0, 3), (1, 4) });

            var expected = 3;
            var actual = game.CountNeighborMines(0, 4);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "左下角のセルの周りの地雷の数を確認 0個のとき")]
        [Trait("メソッド", "CountNeighborMines")]
        public void CountNeighborMinesTest9()
        {
            var game = new Game(5, 5, 0);

            var expected = 0;
            var actual = game.CountNeighborMines(4, 0);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "左下角のセルの周りの地雷の数を確認 1個のとき")]
        [Trait("メソッド", "CountNeighborMines")]
        public void CountNeighborMinesTest10()
        {
            var game = new Game(5, 5, new[] { (3, 0) });

            var expected = 1;
            var actual = game.CountNeighborMines(4, 0);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "左下角のセルの周りの地雷の数を確認 3個のとき(最大)")]
        [Trait("メソッド", "CountNeighborMines")]
        public void CountNeighborMinesTest11()
        {
            var game = new Game(5, 5, new[] { (3, 0), (3, 1), (4, 1) });

            var expected = 3;
            var actual = game.CountNeighborMines(4, 0);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "右下角のセルの周りの地雷の数を確認 0個のとき")]
        [Trait("メソッド", "CountNeighborMines")]
        public void CountNeighborMinesTest12()
        {
            var game = new Game(5, 5, 0);

            var expected = 0;
            var actual = game.CountNeighborMines(4, 4);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "右下角のセルの周りの地雷の数を確認 1個のとき")]
        [Trait("メソッド", "CountNeighborMines")]
        public void CountNeighborMinesTest13()
        {
            var game = new Game(5, 5, new[] { (3, 3) });

            var expected = 1;
            var actual = game.CountNeighborMines(4, 4);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "右下角のセルの周りの地雷の数を確認 3個のとき(最大)")]
        [Trait("メソッド", "CountNeighborMines")]
        public void CountNeighborMinesTest14()
        {
            var game = new Game(5, 5, new[] { (3, 3), (4, 3), (3, 4) });

            var expected = 3;
            var actual = game.CountNeighborMines(4, 4);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "端ではないセルの周りの地雷の数を確認 0個のとき(最大)")]
        [Trait("メソッド", "CountNeighborMines")]
        public void CountNeighborMinesTest15()
        {
            var game = new Game(5, 5, 0);

            var expected = 0;
            var actual = game.CountNeighborMines(2, 2);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "端ではないセルの周りの地雷の数を確認 1個のとき(最大)")]
        [Trait("メソッド", "CountNeighborMines")]
        public void CountNeighborMinesTest16()
        {
            var game = new Game(5, 5, (1, 1));

            var expected = 1;
            var actual = game.CountNeighborMines(2, 2);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "端ではないセルの周りの地雷の数を確認 8個のとき(最大)")]
        [Trait("メソッド", "CountNeighborMines")]
        public void CountNeighborMinesTest17()
        {
            var game = new Game(5, 5, (1, 1), (1, 2), (1, 3), (2,1), (2,3), (3,1), (3,2), (3,3));

            var expected = 8;
            var actual = game.CountNeighborMines(2, 2);
            Assert.Equal(expected, actual);
        }


    }
}
