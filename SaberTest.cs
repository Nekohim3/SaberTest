using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SaberTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var listRandom = new ListRandom();

            listRandom.Add(new ListNode() { Data = "test0" });
            listRandom.Add(new ListNode() { Data = "test1" });
            listRandom.Add(new ListNode() { Data = "test2" });
            listRandom.Add(new ListNode() { Data = "test3" });
            listRandom.Add(new ListNode() { Data = "test4" });
            listRandom.Add(new ListNode() { Data = "test5" });
            listRandom.Add(new ListNode() { Data = "test6" });
            listRandom.Add(new ListNode() { Data = "test7" });
            listRandom.Add(new ListNode() { Data = "test8" });
            listRandom.Add(new ListNode() { Data = "test9" });

            SetRandomNode(listRandom);

            Console.WriteLine("Serealized ListRandom:");

            foreach (var item in listRandom.ToList())
                Console.WriteLine(item);

            using (var fs = new FileStream("data.bin", FileMode.Create, FileAccess.Write))
                listRandom.Serialise(fs);

            var deserealizedListRandom = new ListRandom();

            using (var fs = new FileStream("data.bin", FileMode.Open, FileAccess.Read))
                deserealizedListRandom.Deserealize(fs);

            Console.WriteLine("\nDeserealized ListRandom:");

            foreach (var item in deserealizedListRandom.ToList())
                Console.WriteLine(item);

            Console.ReadLine();
        }

        static void SetRandomNode(ListRandom listRandom)// заплнение поля Random у всех элементов списка
        {
            var rand    = new Random();
            var node = listRandom.Head;
            for (var i = 0; i < listRandom.Count; i++, node = node.Next)
            {
                var current = listRandom.Head;

                do
                {
                    var k = rand.Next(0, listRandom.Count - 1);

                    for (var j = 0; j < listRandom.Count && j != k; j++, current = current.Next) { }

                    node.Random = current;
                } while (node.Random == node);// Random элемента не должен быть равет этому же элементу ? (если может, то убрать цикл do while)
            }
        }
    }

    public class ListNode
    {
        public ListNode Previous;
        public ListNode Next;
        public ListNode Random;
        public string   Data;

        public override string ToString()
        {
            return $"Data: \"{Data}\"; RandomData: \"{Random.Data}\"; NextData: \"{Next.Data}\"; PrevData: \"{Previous.Data}\"";
        }
    }

    public class ListRandom
    {
        public ListNode Head;
        public ListNode Tail;
        public int      Count;

        public List<ListNode> ToList()// Двусвязный список в массив
        {
            var lst     = new List<ListNode>();
            var current = Head;

            for (var i = 0; i < Count; i++, current = current.Next)
                lst.Add(current);

            return lst;
        }

        public void Serialise(Stream s)
        {
            var list   = ToList();

            using (var writer = new BinaryWriter(s, Encoding.UTF8))
            {
                writer.Write(list.Count);// запись количества элементов
                for (var i = 0; i < list.Count; i++)
                {
                    var buf = Encoding.UTF8.GetBytes(list[i].Data);
                    writer.Write(buf.Length);// запись размера данных элемента
                    writer.Write(buf);// запись данных элемента
                    writer.Write(list.IndexOf(list[i].Random));// запись индекса рандомного элемента
                }
            }
        }

        public void Deserealize(Stream s)
        {
            using (var reader = new BinaryReader(s, Encoding.UTF8))
            {
                var count = reader.ReadInt32();

                var list = new List<ListNode>();

                for (var i = 0; i < count; i++)
                    list.Add(new ListNode());

                for (var i = 0; i < count; i++)
                {
                    var bufLength = reader.ReadInt32();
                    var buf       = reader.ReadBytes(bufLength);
                    var rand      = reader.ReadInt32();
                    list[i].Data   = Encoding.UTF8.GetString(buf);
                    list[i].Random = list[rand];

                    Add(list[i]);
                }
            }
        }

        public void Add(ListNode node)
        {
            if (Head == null || Tail == null)// в списке нет элементов
            {
                node.Previous = node;
                node.Next     = node;
                Head          = node;
                Tail          = node;
            }
            else if (Head == Tail)// в списке один элемента
            {
                Tail          = node;
                Head.Next     = Tail;
                Head.Previous = Tail;
                Tail.Next     = Head;
                Tail.Previous = Head;
            }
            else// в списке больше одного элемента
            {
                node.Previous = Tail;
                node.Next     = Head;
                Tail.Next     = node;
                Head.Previous = node;
                Tail          = node;
            }

            Count++;
        }

        public void Remove(ListNode node)
        {
            if (Head == null || Tail == null) // в списке нет элементов
                return;

            if (Head == Tail) // в списке один элемента
            {
                Head = null;
                Tail = null;
            }
            else // в списке больше одного элемента
            {
                var current = Head;

                for (var i = 0; i < Count; i++, current = current.Next)
                    if (current == node) break;

                if (current != node) return;// искомого узла в списке не найдено

                current.Previous.Next = current.Next;
                current.Next.Previous = current.Previous;

                if (Head == current)
                    Head = current.Next;

                if (Tail == current)
                    Tail = current.Previous;
            }

            Count--;
        }
    }
}
