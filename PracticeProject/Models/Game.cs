using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

namespace PracticeProject.Models
{
    public class Game
    {
        private Field[][] gameBoard;

        public string GameBoard { 
            get
            {
                return JsonSerializer.Serialize(this.gameBoard);
            } 
            set
            {
                this.gameBoard = JsonSerializer.Deserialize<Field[][]>(value);
            }
        }

        [Required]
        public int X { get; set; }

        [Required]
        public int Y { get; set; }

        public int Lifes { get; set; }

        public int Unguessed { get; set; }

        public void GenerateGameBoard()
        {
            this.gameBoard = new Field[this.X][];
            this.Lifes = 3;
            this.Unguessed = 0;
            var random = new Random();

            for (int i = 0; i < this.X; i++)
            {
                this.gameBoard[i] = new Field[this.Y];
                for (int j = 0; j < this.Y; j++)
                {
                    if (random.Next(0, 50) < 25)
                    {
                        this.gameBoard[i][j] = new Field(true, false);
                        this.Unguessed++;
                    }
                    else this.gameBoard[i][j] = new Field(false, false);
                }
            }
        }

        public string GenerateTips()
        {
            string[] tipsX = new string[this.X];
            string[] tipsY = new string[this.Y];
            GenerateLine(tipsX, true);
            GenerateLine(tipsY, false);

            return JsonSerializer.Serialize(new { item1 = tipsX, item2 = tipsY });
        }

        private void GenerateLine(string[] tips, bool swap)
        {
            var tip = new StringBuilder();
            for (int i = 0; i < (swap ? this.X : this.Y); i++)
            {
                int counter = 0;
                for (int j = 0; j < (swap ? this.Y : this.X); j++)
                {
                    if (swap ? this.gameBoard[i][j].Value : this.gameBoard[j][i].Value) counter++;
                    else
                    {
                        if (counter > 0)
                        {
                            tip.Append($"{counter},");
                            counter = 0;
                        }
                    }
                }

                if (counter > 0)
                {
                    tip.Append($"{counter},");
                }

                tips[i] = tip.Length > 0 ? tip.Remove(tip.Length - 1, 1).ToString() : string.Empty;
                tip.Clear();
            }
        }

        public bool? CheckValue(int x, int y, bool prediction)
        {
            if (this.gameBoard == null) return null;
            if (this.gameBoard[x][y].Used) return null;

            this.gameBoard[x][y].Used = true;
            if (this.gameBoard[x][y].Value) this.Unguessed--;
            if (this.gameBoard[x][y].Value != prediction) this.Lifes--;
            return this.gameBoard[x][y].Value;
        }
    }
}
