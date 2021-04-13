using MineSweeper;
using System;
using Xunit;

namespace Test
{
    public class UnitTest1
    {
        [Theory(DisplayName = "�͈͓��̍��W�A�N�Z�X�ŁuIndexOutOfRangeException�v���������Ȃ�")]
        [InlineData(0, 0)]
        [InlineData(1, 0)]
        [InlineData(0, 1)]
        [InlineData(3, 4)]
        [InlineData(4, 3)]
        [InlineData(4, 4)]
        [Trait("���\�b�h", "Dig")]
        public void DigTest1(int row, int col)
        {
            var game = new Game(5, 5, 0);
            game.Dig(row, col);
        }

        [Theory(DisplayName = "�͈͊O�̍��W�A�N�Z�X�ŁuIndexOutOfRangeException�v����������")]
        [InlineData(-1, 0)]
        [InlineData(0, -1)]
        [InlineData(4, 5)]
        [InlineData(5, 4)]
        [Trait("���\�b�h", "Dig")]
        public void DigTest2(int row, int col)
        {
            var game = new Game(5, 5, 0);

            Assert.Throws<IndexOutOfRangeException>(() => game.Dig(row, col));
        }

        [Fact(DisplayName = "���e�𓥂񂾎��ɁAGameState���uGameOver�v�ɂȂ�")]
        [Trait("���\�b�h", "Dig")]
        public void DigTest3()
        {
            var game = new Game(1, 1, new[] { (0, 0) });
            game.Dig(0, 0);

            var expected = GameState.GameOver;
            var actual = game.State;
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "���e�𓥂܂Ȃ������Ƃ��AGameState���ς��Ȃ�")]
        [Trait("���\�b�h", "Dig")]
        public void DigTest4()
        {
            var game = new Game(2, 2, new[] { (0, 0) });
            game.Dig(0, 1);

            var expected = GameState.Playing;
            var actual = game.State;
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "�c��̃}�X�����e�����ɂȂ����Ƃ��AGameState���uGameClear�v�ɂȂ�")]
        [Trait("���\�b�h", "Dig")]
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

        [Fact(DisplayName = "�Z���̏�Ԃ��uFlagged�v�̂Ƃ��́A��Ԃ��ς��Ȃ�")]
        [Trait("���\�b�h", "Dig")]
        public void DigTest6()
        {
            var game = new Game(2, 2);
            game.Flag(0, 0);
            game.Dig(0, 0);

            var expected = CellState.Flagged;
            var actual = game.GetCellState(0,0);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "�Z���̏�Ԃ��uQuestion�v�̂Ƃ��́A��Ԃ��ς��Ȃ�")]
        [Trait("���\�b�h", "Dig")]
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

        [Fact(DisplayName = "�Z���̏�Ԃ��uInitial�v�̂Ƃ��́A��Ԃ��uDigged�v�ɕς��")]
        [Trait("���\�b�h", "Dig")]
        public void DigTest8()
        {
            var game = new Game(2, 2);
            game.Dig(0, 0);

            var expected = CellState.Digged;
            var actual = game.GetCellState(0, 0);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "�Z���̏�Ԃ��uDigged�v�̂Ƃ��́A��Ԃ��ς��Ȃ�")]
        [Trait("���\�b�h", "Dig")]
        public void DigTest9()
        {
            var game = new Game(2, 2);
            game.Dig(0, 0);
            game.Dig(0, 0);

            var expected = CellState.Digged;
            var actual = game.GetCellState(0, 0);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "�Q�[���̏�Ԃ��uGameClear�v�̂Ƃ��́ADig�ł��Ȃ�")]
        [Trait("���\�b�h", "Dig")]
        public void DigTest10()
        {
            var game = new Game(2, 2, (0,0));
            game.Dig(1, 0);
            game.Dig(0, 1);
            game.Dig(1, 1);

            // ���̎��_��GameClear�ɂȂ�

            game.Dig(0, 0);

            var expected = GameState.GameClear;
            var actual = game.State;
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "�Q�[���̏�Ԃ��uGameOver�v�̂Ƃ��́ADig�ł��Ȃ�")]
        [Trait("���\�b�h", "Dig")]
        public void DigTest11()
        {
            var game = new Game(2, 2, (0, 0));
            game.Dig(0, 0);

            // ���̎��_��GameOver�ɂȂ�

            game.Dig(0, 1);

            // (0,1)���@���Ă��A��Ԃ��uInitial�v�̂܂܂ł��邱�Ƃ��m�F
            var expected = CellState.Initial;
            var actual = game.GetCellState(0,1);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "�@�����ꏊ�̎���̔��e��0�̂Ƃ�������ċA�I�Ɍ@��")]
        [Trait("���\�b�h", "Dig")]
        public void DigTest12()
        {
            var game = new Game(5, 5);
            game.Dig(2, 2);

            // ���ׂČ@����
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Assert.Equal(CellState.Digged , game.GetCellState(i, j));
                }
            }
        }

        [Fact(DisplayName = "�@�����ꏊ�̎���̔��e��0�ȊO�̂Ƃ��͍ċA�I�Ɍ@��Ȃ�")]
        [Trait("���\�b�h", "Dig")]
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

        [Fact(DisplayName = "����̃Z�����@��B���S�͌@��Ȃ�")]
        [Trait("���\�b�h", "Dig")]
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

        [Fact(DisplayName = "����̃Z�����@��B���e������ꍇ�̓Q�[���I�[�o�[�ɂȂ�")]
        [Trait("���\�b�h", "Dig")]
        public void DigAroundTest2()
        {
            var game = new Game(3, 4, (0, 0));

            game.DigAround(1, 1);
            Assert.Equal(GameState.GameOver, game.State);
        }

        [Theory(DisplayName = "�͈͓��̍��W�A�N�Z�X�ŁuIndexOutOfRangeException�v���������Ȃ�")]
        [InlineData(0, 0)]
        [InlineData(1, 0)]
        [InlineData(0, 1)]
        [InlineData(3, 4)]
        [InlineData(4, 3)]
        [InlineData(4, 4)]
        [Trait("���\�b�h", "Flag")]
        public void FlagTest1(int row, int col)
        {
            var game = new Game(5, 5, 0);
            game.Flag(row, col);
        }

        [Theory(DisplayName = "�͈͊O�̍��W�A�N�Z�X�ŁuIndexOutOfRangeException�v����������")]
        [InlineData(-1, 0)]
        [InlineData(0, -1)]
        [InlineData(4, 5)]
        [InlineData(5, 4)]
        [Trait("���\�b�h", "Flag")]
        public void FlagTest2(int row, int col)
        {
            var game = new Game(5, 5, 0);

            Assert.Throws<IndexOutOfRangeException>(() => game.Flag(row, col));
        }

        [Fact(DisplayName = "�uInitial�v��Ԃ̃Z���ɁuFlag�v�����s����ƁA��Ԃ��uFlagged�v�ɂȂ�")]
        [Trait("���\�b�h", "Flag")]
        public void FlagTest3()
        {
            var game = new Game(1, 1, 0);
            game.Flag(0, 0);

            var expected = CellState.Flagged;
            var actrual = game.GetCellState(0, 0);
            Assert.Equal(expected, actrual);
        }

        [Fact(DisplayName = "�uFlagged�v��Ԃ̃Z���ɁuFlag�v�����s����ƁA��Ԃ��uQuestion�v�ɂȂ�")]
        [Trait("���\�b�h", "Flag")]
        public void FlagTest4()
        {
            var game = new Game(1, 1, 0);
            game.Flag(0, 0);
            game.Flag(0, 0);

            var expected = CellState.Question;
            var actrual = game.GetCellState(0, 0);
            Assert.Equal(expected, actrual);
        }

        [Fact(DisplayName = "�uQuestion�v��Ԃ̃Z���ɁuFlag�v�����s����ƁA��Ԃ��uInitial�v�ɂȂ�")]
        [Trait("���\�b�h", "Flag")]
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

        [Fact(DisplayName = "�uDigged�v��Ԃ̃Z���ɁuFlag�v�����s���Ă��A��Ԃ��uDigged�v�̂܂ܕς��Ȃ�")]
        [Trait("���\�b�h", "Flag")]
        public void FlagTest6()
        {
            var game = new Game(1, 1, 0);
            game.Dig(0, 0);
            game.Flag(0, 0);

            var expected = CellState.Digged;
            var actrual = game.GetCellState(0, 0);
            Assert.Equal(expected, actrual);
        }

        [Fact(DisplayName = "�Q�[���̏�Ԃ��uGameClear�v�̂Ƃ��́AFlag�ł��Ȃ�")]
        [Trait("���\�b�h", "Flag")]
        public void FlagTest7()
        {
            var game = new Game(2, 2, (0, 0));
            game.Dig(1, 0);
            game.Dig(0, 1);
            game.Dig(1, 1);

            // ���̎��_��GameClear�ɂȂ�

            game.Flag(0, 0);

            var expected = CellState.Initial;
            var actual = game.GetCellState(0, 0);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "�Q�[���̏�Ԃ��uGameOver�v�̂Ƃ��́AFlag�ł��Ȃ�")]
        [Trait("���\�b�h", "Flag")]
        public void FlagTest8()
        {
            var game = new Game(2, 2, (0, 0));
            game.Dig(0, 0);

            // ���̎��_��GameOver�ɂȂ�

            game.Flag(0, 1);

            // (0,1)��Flag���悤�Ƃ��Ă��A��Ԃ��uInitial�v�̂܂܂ł��邱�Ƃ��m�F
            var expected = CellState.Initial;
            var actual = game.GetCellState(0, 1);
            Assert.Equal(expected, actual);
        }

        [Theory(DisplayName = "�͈͓��̍��W�A�N�Z�X�ŁuIndexOutOfRangeException�v���������Ȃ�")]
        [InlineData(0, 0)]
        [InlineData(1, 0)]
        [InlineData(0, 1)]
        [InlineData(3, 4)]
        [InlineData(4, 3)]
        [InlineData(4, 4)]
        [Trait("���\�b�h", "CountNeighborMines")]
        public void CountNeighborMinesTest1(int row, int col)
        {
            var game = new Game(5, 5, 0);
            game.CountNeighborMines(row, col);
        }

        [Theory(DisplayName = "�͈͊O�̍��W�A�N�Z�X�ŁuIndexOutOfRangeException�v����������")]
        [InlineData(-1, 0)]
        [InlineData(0, -1)]
        [InlineData(4, 5)]
        [InlineData(5, 4)]
        [Trait("���\�b�h", "CountNeighborMines")]
        public void CountNeighborMinesTest2(int row, int col)
        {
            var game = new Game(5, 5, 0);

            Assert.Throws<IndexOutOfRangeException>(() => game.CountNeighborMines(row, col));
        }

        [Fact(DisplayName = "����p�̃Z���̎���̒n���̐����m�F 0�̂Ƃ�")]
        [Trait("���\�b�h", "CountNeighborMines")]
        public void CountNeighborMinesTest3()
        {
            var game = new Game(5, 5, 0);

            var expected = 0;
            var actual = game.CountNeighborMines(0, 0);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "����p�̃Z���̎���̒n���̐����m�F 1�̂Ƃ�")]
        [Trait("���\�b�h", "CountNeighborMines")]
        public void CountNeighborMinesTest4()
        {
            var game = new Game(5, 5, new[] {(0, 1) });

            var expected = 1;
            var actual = game.CountNeighborMines(0, 0);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "����p�̃Z���̎���̒n���̐����m�F 3�̂Ƃ�(�ő�)")]
        [Trait("���\�b�h", "CountNeighborMines")]
        public void CountNeighborMinesTest5()
        {
            var game = new Game(5, 5, new[] {(0, 1), (1, 0), (1, 1) });

            var expected = 3;
            var actual = game.CountNeighborMines(0, 0);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "�E��p�̃Z���̎���̒n���̐����m�F 0�̂Ƃ�")]
        [Trait("���\�b�h", "CountNeighborMines")]
        public void CountNeighborMinesTest6()
        {
            var game = new Game(5, 5, 0);

            var expected = 0;
            var actual = game.CountNeighborMines(0, 4);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "�E��p�̃Z���̎���̒n���̐����m�F 1�̂Ƃ�")]
        [Trait("���\�b�h", "CountNeighborMines")]
        public void CountNeighborMinesTest7()
        {
            var game = new Game(5, 5, new[] { (1, 3) });

            var expected = 1;
            var actual = game.CountNeighborMines(0, 4);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "�E��p�̃Z���̎���̒n���̐����m�F 3�̂Ƃ�(�ő�)")]
        [Trait("���\�b�h", "CountNeighborMines")]
        public void CountNeighborMinesTest8()
        {
            var game = new Game(5, 5, new[] { (1, 3), (0, 3), (1, 4) });

            var expected = 3;
            var actual = game.CountNeighborMines(0, 4);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "�����p�̃Z���̎���̒n���̐����m�F 0�̂Ƃ�")]
        [Trait("���\�b�h", "CountNeighborMines")]
        public void CountNeighborMinesTest9()
        {
            var game = new Game(5, 5, 0);

            var expected = 0;
            var actual = game.CountNeighborMines(4, 0);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "�����p�̃Z���̎���̒n���̐����m�F 1�̂Ƃ�")]
        [Trait("���\�b�h", "CountNeighborMines")]
        public void CountNeighborMinesTest10()
        {
            var game = new Game(5, 5, new[] { (3, 0) });

            var expected = 1;
            var actual = game.CountNeighborMines(4, 0);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "�����p�̃Z���̎���̒n���̐����m�F 3�̂Ƃ�(�ő�)")]
        [Trait("���\�b�h", "CountNeighborMines")]
        public void CountNeighborMinesTest11()
        {
            var game = new Game(5, 5, new[] { (3, 0), (3, 1), (4, 1) });

            var expected = 3;
            var actual = game.CountNeighborMines(4, 0);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "�E���p�̃Z���̎���̒n���̐����m�F 0�̂Ƃ�")]
        [Trait("���\�b�h", "CountNeighborMines")]
        public void CountNeighborMinesTest12()
        {
            var game = new Game(5, 5, 0);

            var expected = 0;
            var actual = game.CountNeighborMines(4, 4);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "�E���p�̃Z���̎���̒n���̐����m�F 1�̂Ƃ�")]
        [Trait("���\�b�h", "CountNeighborMines")]
        public void CountNeighborMinesTest13()
        {
            var game = new Game(5, 5, new[] { (3, 3) });

            var expected = 1;
            var actual = game.CountNeighborMines(4, 4);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "�E���p�̃Z���̎���̒n���̐����m�F 3�̂Ƃ�(�ő�)")]
        [Trait("���\�b�h", "CountNeighborMines")]
        public void CountNeighborMinesTest14()
        {
            var game = new Game(5, 5, new[] { (3, 3), (4, 3), (3, 4) });

            var expected = 3;
            var actual = game.CountNeighborMines(4, 4);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "�[�ł͂Ȃ��Z���̎���̒n���̐����m�F 0�̂Ƃ�(�ő�)")]
        [Trait("���\�b�h", "CountNeighborMines")]
        public void CountNeighborMinesTest15()
        {
            var game = new Game(5, 5, 0);

            var expected = 0;
            var actual = game.CountNeighborMines(2, 2);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "�[�ł͂Ȃ��Z���̎���̒n���̐����m�F 1�̂Ƃ�(�ő�)")]
        [Trait("���\�b�h", "CountNeighborMines")]
        public void CountNeighborMinesTest16()
        {
            var game = new Game(5, 5, (1, 1));

            var expected = 1;
            var actual = game.CountNeighborMines(2, 2);
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "�[�ł͂Ȃ��Z���̎���̒n���̐����m�F 8�̂Ƃ�(�ő�)")]
        [Trait("���\�b�h", "CountNeighborMines")]
        public void CountNeighborMinesTest17()
        {
            var game = new Game(5, 5, (1, 1), (1, 2), (1, 3), (2,1), (2,3), (3,1), (3,2), (3,3));

            var expected = 8;
            var actual = game.CountNeighborMines(2, 2);
            Assert.Equal(expected, actual);
        }


    }
}
