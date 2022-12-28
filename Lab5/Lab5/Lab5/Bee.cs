using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5
{
    public enum BeeStatus
    {
        Active,
        Scout
    }
    public class Bee
    {
        public BeeStatus Status { get; set; }   //Статус бджоли
        public int[] Solution { get; set; }     //Кожна бджола має певний розв'язок задачі
        public long Distance { get; set; }      //Дистанція за цим розв'язком

        public Bee(BeeStatus status, int[] memoryMatrix, long measureOfQuality)
        {
            this.Status = status;
            this.Solution= new int[memoryMatrix.Length];
            Array.Copy(memoryMatrix, this.Solution, memoryMatrix.Length);
            this.Distance = measureOfQuality;
        }
    }
}
