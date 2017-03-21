<Query Kind="Statements" />

string[] colors = {"green", "brown", "blue", "red"};
foreach (var el in colors)
{
	Console.WriteLine(el);
}

Console.WriteLine(colors.Max(c => c.Length));
Console.WriteLine(colors.OrderBy(c => c.Length).FirstOrDefault());