using ArhivatorHaff;
using HaffmanFileWork;
using HaffmanTree;
using System.IO;
using System;
using System.Collections.Generic;
internal class Program
{
	private static void Main(string[] args)
	{
		int num = 0;
		while (num != 3)
		{
			var nodes = readFreqFile("freq.txt");
			var HaffTree = new HaffTree(nodes);
			Console.WriteLine("Write: 1-zip file,2-unzip file,3-close");
			num = 0;
			int.TryParse(Console.ReadLine(), out num);
			if (num == 1)
			{
				Console.WriteLine("Write filepath to not ziped file: ");
				string filePath = Console.ReadLine();
				var dict = HaffTree.getEncodeDict();
				var str = "";

				try
				{
					var fs = new ArchWriter(filePath.Split('.')[0] + ".superzip");
					foreach (var line in File.ReadAllLines(filePath))
					{
						foreach (var ch in line)
						{
							fs.WriteWord(dict[ch]);
						}
						fs.WriteWord(dict['\n']);
					}
					fs.WriteWord(dict['$']);


					fs.WriteWord(str);
					fs.Finish();
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
					continue;
				}

			}
			else if (num == 2)
			{
				Console.WriteLine("Write filepath to ziped file: ");
				string filePath = Console.ReadLine();
				ArchReader fs = null;
				try
				{
					fs = new ArchReader(filePath);
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
					continue;
				}

				var strs = new List<string>();
				var str = "";
				var node = HaffTree.root;
				while (true)
				{
					byte bit = 0;
					if (fs.ReadBit(out bit))
					{
						if (bit == 0)
							node = node.LeftChild;
						else
							node = node.RightChild;

						if (node.IsLeaf)
						{
							char ch = node.Symbol;
							if (ch == '\n')
							{
								strs.Add(str);
								str = "";
							}
							else if (ch == '$')
							{
								strs.Add(str);
								str = "";
								break;
							}
							else
							{
								str += ch;
							}
							node = HaffTree.root;
						}
					}
					//else
					//	break;
				}
				Console.WriteLine("unzip File name:");
				filePath = Console.ReadLine();

				using (var writer = new StreamWriter(filePath, false))
				{
					for (int i = 0; i < strs.Count - 1; i++)
					{
						//writer.WriteLine(strs[i]);
						writer.Write(strs[i] + "\r\n");
					}
					writer.Write(strs[strs.Count - 1]);

				}
			}

		}
	}

	private static List<HaffNode> readFreqFile(string fileName)
	{
		var nodes = new List<HaffNode>();

		int count = 0;
		foreach (string line in File.ReadLines(fileName))
		{
			if (count > 1)
				nodes.Add(new HaffNode()
				{
					Symbol = line[0],
					Frequency = double.Parse(line.Split(' ')[1])
				});
			else if (count == 0)
				nodes.Add(new HaffNode()
				{
					Symbol = '\n',
					Frequency = double.Parse(line)
				});
			else if (count == 1)
				nodes.Add(new HaffNode()
				{
					Symbol = ' ',
					Frequency = double.Parse(line)
				});
			count++;
		}
		nodes.Add(new HaffNode()
		{
			Symbol = '$',
			Frequency = 0
		});
		return nodes;
	}
}