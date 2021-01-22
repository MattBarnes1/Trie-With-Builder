using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class DefaultTrieBuilder<ArrayType, TerminalNodeType> : ITrieBuilder<ArrayType, TerminalNodeType> where ArrayType : IComparable<ArrayType>
{
    public class TrieNode : ITrieNode<ArrayType, TerminalNodeType>
    {
        public ArrayType Value { get; }
        public List<ITrieNode<ArrayType, TerminalNodeType>> Children { get; } = new List<ITrieNode<ArrayType, TerminalNodeType>>();
        List<TerminalNodeType> TerminalItems = new List<TerminalNodeType>();
        public bool HasTerminalNodes()
        {
            return (TerminalItems.Count != 0);
        }

        public int CompareTo(ArrayType other)
        {
            return Value.CompareTo(other);
        }

        public void AddTerminalData(TerminalNodeType myType)
        {
            TerminalItems.Add(myType);
        }

        public void RemoveTerminalData(TerminalNodeType myType)
        {
            TerminalItems.Remove(myType);
        }

        public List<TerminalNodeType> GetTerminalItems()
        {
            return TerminalItems;
        }

        public TrieNode(ArrayType Item)
        {
            this.Value = Item;
        }
    }

    

    public ITrieNode<ArrayType, TerminalNodeType> CreateTrieNode(ArrayType myValue)
    {
        return new TrieNode(myValue);
    }

    public void AttachTerminalNode(ref ITrieNode<ArrayType, TerminalNodeType> myNode, TerminalNodeType TypeToSave)
    {
        myNode.AddTerminalData(TypeToSave);
    }

    public IEnumerator<TerminalNodeType> GetEnumerable(List<ITrieNode<ArrayType, TerminalNodeType>> myRoot, Trie<ArrayType, TerminalNodeType> myOwner)
    {
        return new DefaultTrieIterator<ArrayType, TerminalNodeType>(myRoot, myOwner);
    }


}
