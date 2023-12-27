using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaffmanTree
{
	class HaffNode
	{
		public HaffNode LeftChild { get; set; } = null;
		public HaffNode RightChild { get; set; } = null;
		public double Frequency { get; set; }
		public char Symbol { get; set; }

		public bool IsLeaf
		{
			get
			{
				return LeftChild == null && RightChild == null;
			}
		}
	}
}

