using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atividade.Models
{
    public class Leitor
    {

        public List<Item> ReadItems(string fileName)
        {
            List<Item> items = new List<Item>();

            using (StreamReader reader = new StreamReader(fileName))
            {
                reader.ReadLine(); // Pular a linha da capacidade
                int numItems = int.Parse(reader.ReadLine()!.Trim());

                for (int i = 0; i < numItems; i++)
                {
                    string[] itemData = reader.ReadLine()!.Trim().Split(',');
                    items.Add(new Item(int.Parse(itemData[1]), int.Parse(itemData[2])));
                }
            }

            return items;
        }


        public int ReadCapacity(string fileName)
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                int capacity = int.Parse(reader.ReadLine()!.Trim());
                return capacity;
            }
        }



    }
}
