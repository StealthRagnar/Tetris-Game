using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tetris_Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ImageSource[] tileImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/TileEmpty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileCyan.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileBlue.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileOrange.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileYellow.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileGreen.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TilePurple.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileRed.png", UriKind.Relative))

        };

        private readonly ImageSource[] blockImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/Block-Empty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-I.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-J.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-L.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-O.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-S.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-T.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-Z.png", UriKind.Relative))

        };


        private readonly Image[,] imageControls;

        private GameState gamestate = new GameState();



        public MainWindow()
        {
            InitializeComponent();
            imageControls = SetupGameCanvas(gamestate.GameGridView);
        }

        private Image[,] SetupGameCanvas(GameGridView grid)
        {
            Image[,] imageControls = new Image[grid.rows, grid.columns];
            int cellsize = 25;

            for (int r = 0; r < grid.rows; r++)
            {
                for (int c = 0; c < grid.columns; c++)
                {
                    Image imageControl = new Image()
                    {
                        Width = cellsize,
                        Height = cellsize
                    };

                    Canvas.SetTop(imageControl, (r - 2) * cellsize + 10);
                    Canvas.SetLeft(imageControl, c * cellsize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[r,c] = imageControl;
                }
            }

            return imageControls;

        }

        private void DrawGird(GameGridView grid)
        {
            for (int r = 0; r < grid.rows; r++)
            {
                for (int c = 0; c < grid.columns; c++)
                {
                    int id = grid[r, c];
                    imageControls[r, c].Source = tileImages[id];
                }
            }
        }

        private void DrawBlock(Block block)
        {
            foreach (Position p in block.TilePositions())
            {
                imageControls[p.Row, p.Column].Source = tileImages[block.Id];
            }
        }

        private void DrawNextBlock(BlockQueue blockQueue)
        {
            Block next = blockQueue.NextBlock;
            NextImage.Source = blockImages[next.Id];
        }
        private void Draw(GameState gameState)
        {
            DrawGird(gameState.GameGridView);
            DrawBlock(gameState.CurrentBlock);
            DrawNextBlock(gameState.BlockQueue);
        }

        private async Task Gameloop()
        {
            Draw(gamestate);
            while (!gamestate.GameOver)
            {
                await Task.Delay(500);
                gamestate.MoveBlockDown();
                Draw(gamestate);
            }

            GameOverMenu.Visibility = Visibility.Visible;
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gamestate.GameOver)
            {
                return;
            }

            switch (e.Key)
            {
                case Key.Left:
                    gamestate.MoveBlockLeft();
                    break;
                case Key.Right:
                    gamestate.MoveBlockRight();
                    break;
                case Key.Down:
                    gamestate.MoveBlockDown();
                    break;
                case Key.Up:
                    gamestate.RotateBlockCW();
                    break;
                case Key.Z:
                    gamestate.RotateBlockCCW();
                    break;
                default:
                    return;
            }

            Draw(gamestate);

        }

        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            await Gameloop();
        }

        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            gamestate = new GameState();
            GameOverMenu.Visibility = Visibility.Hidden;
            await Gameloop();
        }
    }
}