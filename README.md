# SaberTest

Тестовое задание для вакансии: https://spb.hh.ru/vacancy/44272897

Реализуйте функции сериализации и десериализации двусвязного списка, заданного следующим образом:
```C#
class ListNode
{
    public ListNode Previous;
    public ListNode Next;
    public ListNode Random; // произвольный элемент внутри списка
    public string Data;
}


class ListRandom
{
    public ListNode Head;
    public ListNode Tail;
    public int Count;

    public void Serialize(Stream s)
    {
    }

    public void Deserialize(Stream s)
    {
    }
}
```
