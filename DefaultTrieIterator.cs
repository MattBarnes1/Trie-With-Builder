using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DefaultTrieIterator<ArrayType, Item> : IEnumerator<Item> where ArrayType : IComparable<ArrayType>
{
    Queue<ITrieNode<ArrayType, Item>> myBFS = new Queue<ITrieNode<ArrayType, Item>>();
    public object Current { get; private set; }
    public List<ITrieNode<ArrayType, Item>> ResetItems { get; }

    Item IEnumerator<Item>.Current { get { return (Item)Current; } }

    public DefaultTrieIterator(List<ITrieNode<ArrayType, Item>> myRootItems, Trie<ArrayType, Item> myItems)
    {
        this.ResetItems = myRootItems;
        Reset();
    }

    public void Dispose()
    {

    }
    ITrieNode<ArrayType, Item> LastExaminedNode;
    List<Item> myCurrentNodes;
    int lastCount = 0;
    public bool MoveNext()
    {
        if (myBFS.Count == 0) return false;
        Current = null;
        if(myCurrentNodes != null)
        {
            lastCount++;
            if (myCurrentNodes.Count > lastCount)
            {
                Current = myCurrentNodes[lastCount];
            }
        }
        while(myBFS.Count != 0 && Current == null)
        {
            ITrieNode<ArrayType, Item> Out = myBFS.Dequeue();
            if(Out.HasTerminalNodes())
            {
                myCurrentNodes = Out.GetTerminalItems();
                lastCount = 0;
                Current = myCurrentNodes[lastCount];
            }
            foreach (var A in Out.Children)
            {
                myBFS.Enqueue(A);
            }
        }

        return Current != null;
    }

    public void Reset()
    {
        myBFS.Clear();
        foreach (var A in ResetItems)
        {
            myBFS.Enqueue(A);
        }
    }
}
