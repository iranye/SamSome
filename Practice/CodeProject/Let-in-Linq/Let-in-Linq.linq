<Query Kind="Program" />

// https://www.codeproject.com/Tips/1209661/Let-in-LinQ

IEnumerable<int> RunForLoop()
{
	var numbers = Enumerable.Range(1, 100);
	IList<int> results = new List<int>();
	foreach (var number in numbers)
	{
		bool isEven = number % 2 == 0;
		if (isEven) results.Add(number);
	}
	return results;
}

IEnumerable<int> RunQuery()
{
	var numbers = Enumerable.Range(1, 100);
	IList<int> results = (from number in numbers
	let isEven = number % 2 == 0
	where isEven
	select number).ToList();

	return results;
}




void Main()
{
	var arr = RunForLoop();
	arr.Dump();
	
	var newArr = RunQuery();
	newArr.Dump();
//	foreach (var el in RunForLoop())
//	{
//		el.Dump();
//	}
}
