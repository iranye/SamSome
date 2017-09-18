<Query Kind="Program" />



class Node {
	public int Data{get; set;}
	public Node Next{get; set;}
	public override string ToString()
	{
		return string.Format("Data={0}", Data) + Next == null ? "." : " AND " + Next.ToString();
	}
}
	  
void Main()
{
	var head = GenerateLinkedList();
	head.Dump();
//	Node list = InsertAtHead(head, 5);
//	Node list = InsertAtEnd(head, 5);
//	Node list = InsertNth(head, 42, 11);
//	Node list = DeleteNth(head, 0);
	Node list = ReverseList(head);
	
	list.Dump();
}

Node ReverseList(Node head)
{
    if (head == null || head.Next == null)
    {
        return head;
    }
    if (head.Next == null)
    {
        Console.WriteLine(head.Data);
        return head;
    }

    Node current = head;
    Node previous = null;

    while (current != null)
    {
        var temp = current.Next;
        if (previous == null)
        {
            previous = head;
            previous.Next = null;
        }
        else
        {
            current.Next = previous;
        }
        previous = current;
        current = temp;
        if (current != null)
        {
            temp = current.Next;
            current.Next = previous;
            previous = current;
            current = temp;
        }

    }
    return previous;
}

Node DeleteNth(Node head, int position)
{
// https://www.hackerrank.com/challenges/delete-a-node-from-a-linked-list/problem
	if (head == null)
	{
		return null;
	}
	if (position == 0)
	{
		return head.Next;
	}
	
	int currentPosition = 0;
	Node current = head;
	Node previous = null;
	while(currentPosition < position && current != null)
	{
		previous = current;
		current = current.Next;
		currentPosition++;
	}
	previous.Next = current == null ? null : current.Next;
	return head;
}

Node InsertNth(Node head, int data, int position)
{
// https://www.hackerrank.com/challenges/insert-a-node-at-a-specific-position-in-a-linked-list/problem
	if (head == null)
	{
		return new Node{Data = data, Next = null};
	}
	if (position == 0)
	{
		return new Node{Data = data, Next = head};
	}
	
	int currentPosition = 0;
	Node current = head;
	Node previous = null;
	while(currentPosition < position && current != null)
	{
		previous = current;
		current = current.Next;
		currentPosition++;
	}
	previous.Next = new Node{Data = data, Next = current};
	return head;
}

Node InsertAtEnd(Node head, int x)
{
// https://www.hackerrank.com/challenges/insert-a-node-at-the-tail-of-a-linked-list/problem
	if (head == null)
	{
		return new Node{Data = x, Next = null};
	}
	Node current = head;
	for(; current.Next != null; current = current.Next)
	{}
	current.Next = new Node{Data = x, Next = null};
	return head;
}

Node InsertAtHead(Node head, int x)
{
// https://www.hackerrank.com/challenges/insert-a-node-at-the-head-of-a-linked-list/problem
	if (head == null)
	{
		return new Node{Data = x, Next = null};
	}
	return new Node{Data = x, Next = head};
}

Node GenerateLinkedList()
{
	Node head = null;
	Node current = null;
	int[] arr = {2, 3, 5, 7, 11};
	foreach (var el in arr)
	{
		if (head == null)
		{
			head = new Node{Data = el};
		}
		else
		{
			if (current == null)
			{
				current = new Node{Data = el};
				head.Next = current;
			}
			else
			{
				current.Next = new Node{Data = el};
				current = current.Next;
			}
		}
	}
	return head;
}