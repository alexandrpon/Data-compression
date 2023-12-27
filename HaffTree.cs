using HaffmanTree;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArhivatorHaff
{
	internal class HaffTree
	{
		public HaffNode root { get; set; }

		public HaffTree(List<HaffNode> nodes)
		{
			root = buildTree(nodes);
		}

		private HaffNode buildTree(List<HaffNode> nodes)
		{
			while (nodes.Count > 1)
			{
				nodes = nodes.OrderBy(node => node.Frequency).ToList();

				if (nodes.Count > 1)
				{
					var taken = nodes.Take(2).ToList();
					var parent = new HaffNode()
					{
						Symbol = '$',
						Frequency = taken[0].Frequency + taken[1].Frequency,
						LeftChild = taken[0],
						RightChild = taken[1]
					};

					nodes.Remove(taken[0]);
					nodes.Remove(taken[1]);
					nodes.Add(parent);
				}
			}

			return nodes[0];
		}

		public Dictionary<char, string> getEncodeDict()
		{
			Dictionary<char, string> dict = new Dictionary<char, string>();

			CLR(root, "", dict);
			return dict;
		}

		private void CLR(HaffNode node, string code, Dictionary<char, string> dict)
		{
			if (!node.IsLeaf)
			{
				CLR(node.LeftChild, code + "0", dict);
				CLR(node.RightChild, code + "1", dict);
			}
			else
			{
				dict.Add(node.Symbol, code);
			}
		}
	}

}
