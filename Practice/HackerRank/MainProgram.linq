<Query Kind="Program" />

// https://www.hackerrank.com/challenges/lonely-integer/problem
void Main()
{
	const int n = 5;
	string[] a_temp = new string[n]{"0", "0", "1", "2", "1"};
	int[] a = Array.ConvertAll(a_temp, Int32.Parse);
	int res = lonelyInteger(a);
	res.Dump();
}

int lonelyInteger(int[] a)
{
	a.Dump();
	List<int> list = new List<int>();
	for(int i = 0; i < a.Length; i++)
	{
		if (list.Contains(a[i]))
		{
			list.Remove(a[i]);
		}
		else
		{
			list.Add(a[i]);
		}
//		list.Dump();
	}
	return list.FirstOrDefault();
}
