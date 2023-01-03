using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5
{
    public enum BeeStatus
    {
        Inactive,
        Active,
        Scout
    }
    public class Bee
    {
        public BeeStatus Status { get; set; }   //Статус бджоли
        public int[] Solution { get; set; }     //Кожна бджола несе за собою певний розв'язок задачі
        public int Distance { get; set; }      //Дистанція за цим розв'язком
        public int NumberOfVisits { get; set; } //Кількість розвіданих суміжних ділянок із елітною(змінюється тільки для активних фуражирів)

        public Bee(BeeStatus status, int[] memoryMatrix, int measureOfQuality, int numberOfVisits)
        {
            this.Status = status;
            this.Solution= new int[memoryMatrix.Length];
            Array.Copy(memoryMatrix, this.Solution, memoryMatrix.Length);
            this.Distance = measureOfQuality;
            this.NumberOfVisits = numberOfVisits;
        }
    }
}
