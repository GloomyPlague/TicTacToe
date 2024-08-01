using System;
using System.Linq;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class Form1 : Form
    {
        public static int playerScore = 0;
        public static int aiScore = 0;

        private char[] board = new char[9];
        private char currentPlayer;
        private bool gameOver;
        private Random random = new Random();

        public Form1()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            gameOver = false;
            lblStatus.Text = "";

            for (int i = 0; i < board.Length; i++)
            {
                board[i] = ' ';
                Button button = (Button)this.Controls.Find($"button{i + 1}", true).FirstOrDefault();
                button.Text = "";
                button.Enabled = true;
                button.Click += new EventHandler(Button_Click);
            }

            if (random.Next(2) == 0)
            {
                currentPlayer = 'X';
                lblStatus.Text = "Player X's turn";
            }
            else
            {
                currentPlayer = 'O';
                lblStatus.Text = "AI's turn";
                this.Refresh();  // Refresh the UI to show the status message
                AITurn();
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if (gameOver) return;

            Button button = (Button)sender;
            int index = int.Parse(button.Name.Replace("button", "")) - 1;

            if (board[index] == ' ')
            {
                board[index] = currentPlayer;
                button.Text = currentPlayer.ToString();
                button.Enabled = false;

                if (CheckWin())
                {
                    playerScore++;
                    playerScorelbl.Text = "Player: " + playerScore;
                    lblStatus.Text = $"Player {currentPlayer} wins!";
                    gameOver = true;
                    return;
                }
                if (board.All(c => c != ' '))
                {
                    lblStatus.Text = "It's a draw!";
                    gameOver = true;
                    return;
                }

                currentPlayer = currentPlayer == 'X' ? 'O' : 'X';
                lblStatus.Text = currentPlayer == 'X' ? "Player X's turn" : "AI's turn";
                this.Refresh();

                if (currentPlayer == 'O')
                {
                    AITurn();
                }
            }
        }

        private bool CheckWin()
        {
            int[][] winPatterns = new int[][]
            {
                new int[] {0, 1, 2},
                new int[] {3, 4, 5},
                new int[] {6, 7, 8},
                new int[] {0, 3, 6},
                new int[] {1, 4, 7},
                new int[] {2, 5, 8},
                new int[] {0, 4, 8},
                new int[] {2, 4, 6}
            };

            foreach (var pattern in winPatterns)
            {
                if (board[pattern[0]] == currentPlayer &&
                    board[pattern[1]] == currentPlayer &&
                    board[pattern[2]] == currentPlayer)
                {
                    return true;
                }
            }

            return false;
        }

        private void AITurn()
        {
            int bestMove = -1;
            int bestScore = int.MinValue;

            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] == ' ')
                {
                    board[i] = 'O';
                    int score = Minimax(board, 0, false);
                    board[i] = ' ';

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = i;
                    }
                }
            }

            if (bestMove != -1)
            {
                board[bestMove] = 'O';
                Button button = (Button)this.Controls.Find($"button{bestMove + 1}", true).FirstOrDefault();
                button.Text = "O";
                button.Enabled = false;

                if (CheckWin())
                {
                    aiScore++;
                    aiScorelbl.Text = "AI: " + aiScore;
                    lblStatus.Text = "AI wins!";
                    gameOver = true;
                    return;
                }
                if (board.All(c => c != ' '))
                {
                    lblStatus.Text = "It's a draw!";
                    gameOver = true;
                    return;
                }

                currentPlayer = 'X';
                lblStatus.Text = "Player X's turn";
            }
        }

        private int Minimax(char[] board, int depth, bool isMaximizing)
        {
            if (CheckWin())
            {
                return isMaximizing ? -1 : 1;
            }
            if (board.All(c => c != ' '))
            {
                return 0;
            }

            if (isMaximizing)
            {
                int bestScore = int.MinValue;

                for (int i = 0; i < board.Length; i++)
                {
                    if (board[i] == ' ')
                    {
                        board[i] = 'O';
                        int score = Minimax(board, depth + 1, false);
                        board[i] = ' ';
                        bestScore = Math.Max(score, bestScore);
                    }
                }

                return bestScore;
            }
            else
            {
                int bestScore = int.MaxValue;

                for (int i = 0; i < board.Length; i++)
                {
                    if (board[i] == ' ')
                    {
                        board[i] = 'X';
                        int score = Minimax(board, depth + 1, true);
                        board[i] = ' ';
                        bestScore = Math.Min(score, bestScore);
                    }
                }

                return bestScore;
            }
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            InitializeGame();
        }
    }
}
