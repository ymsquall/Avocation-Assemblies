using System.Collections.Generic;

namespace Framework.Tools
{
    /*
    public class TreeNode<T>
    {
        public T Header = default(T);
        //public bool IsExpanded = true;
        //public bool IsCheckBox = false;
        //public bool IsChecked = false;
        //public bool IsHover = false;
        //public bool IsSelected = false;
        public List<TreeNode<T>> Items = new List<TreeNode<T>>();
        public TreeNode<T> Parent = null;
        public object[] DataContext = null;
        public TreeNode<T> this[int index]
        {
            get 
            {
                if (index < 0 || index >= Items.Count)
                    return null;
                return Items[index];
            }
        }
        public TreeNode(T header, params object[] data)
        {
            Header = header;
            DataContext = data;
        }
        public TreeNode(TreeNode<T> parent, T header, params object[] data)
        {
            Parent = parent;
            Header = header;
            DataContext = data;
        }
        public TreeNode(TreeNode<T> parent, params object[] data)
        {
            Parent = parent;
            DataContext = data;
        }
        public TreeNode<T> AddItem(T header)
        {
            TreeNode<T> item = new TreeNode<T>(this) { Header = header };
            Items.Add(item);
            return item;
        }
        public bool HasChildItems()
        {
            if (null == Items ||
                Items.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    */
    public class StrTreeNode
	{
		public string Header = "";
		public List<StrTreeNode> Items = new List<StrTreeNode>();
		public StrTreeNode Parent = null;
		public object[] DataContext = null;
		public StrTreeNode this[int index]
		{
			get 
			{
				if (index < 0 || index >= Items.Count)
					return null;
				return Items[index];
			}
		}
		public StrTreeNode(string header, params object[] data)
		{
			Header = header;
			DataContext = data;
		}
		public StrTreeNode(StrTreeNode parent, string header, params object[] data)
		{
			Parent = parent;
			Header = header;
			DataContext = data;
		}
		public StrTreeNode(StrTreeNode parent, params object[] data)
		{
			Parent = parent;
			DataContext = data;
		}
		public StrTreeNode AddItem(string header)
		{
			StrTreeNode item = new StrTreeNode(this) { Header = header };
			Items.Add(item);
			return item;
		}
		public bool HasChildItems()
		{
			if (null == Items ||
			    Items.Count == 0)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
	}
}