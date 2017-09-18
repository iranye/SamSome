<Query Kind="Program" />

// https://www.hackerrank.com/challenges/lonely-integer/problem
void Main()
{
	string title = "Insert a node at the head of a linked list";
	var res = makeTitleFilenameable(title);
	res.Dump();
}

string makeTitleFilenameable(string str)
{
	str.Dump();
//	var subStr = str.Substring(1, str.Length - 1);
//	subStr.Dump();
	var massaged = new StringBuilder();
	string[] segments = str.Split(' ');
	foreach (var el in segments)
	{
		var firstChar = el[0].ToString().ToUpper();
		massaged.Append(firstChar);
		if (el.Length > 1)
		{
			 massaged.Append(el.Substring(1, el.Length - 1));
		}
	}
	
	return massaged.ToString();
}