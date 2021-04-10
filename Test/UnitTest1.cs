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
            var actual = game.GameState;
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "���e�𓥂܂Ȃ������Ƃ��AGameState���ς��Ȃ�")]
        [Trait("���\�b�h", "Dig")]
        public void DigTest4()
        {
            var game = new Game(2, 2, new[] { (0, 0) });
            game.Dig(0, 1);

            var expected = GameState.Playing;
            var actual = game.GameState;
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
            var actual = game.GameState;
            Assert.Equal(expected, actual);
        }
    }
}
