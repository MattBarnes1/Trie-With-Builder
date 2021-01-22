using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class Trie<ArrayPathType, StoredItemType> : IEnumerable<StoredItemType> where ArrayPathType : IComparable<ArrayPathType>
{

    ITrieBuilder<ArrayPathType, StoredItemType> myBuilder;

    public List<ITrieNode<ArrayPathType, StoredItemType>> Root = new List<ITrieNode<ArrayPathType, StoredItemType>>();

    public int Count { get; private set; }

    public bool IsReadOnly { get; private set; }

    /// <summary>
    /// Using a set of items we store the given item at the the last element of Input list.
    /// </summary>
    /// <param name="InputList">The items we use to figure out where to store something</param>
    /// <param name="itemToStore">The item we want to store at the position given by the first parameter.</param>
    public void Add(IList<ArrayPathType> InputList, StoredItemType itemToStore)
    {
        List<ITrieNode<ArrayPathType, StoredItemType>> CurrentSubtree = Root;
        int InputNodeToCheck = 0;
        ITrieNode<ArrayPathType, StoredItemType> LastResult = null;
        for (int i = 0; i < InputList.Count; i++)
        {
            ITrieNode<ArrayPathType, StoredItemType> Result = SearchSubtreeForMatchingType(InputList[InputNodeToCheck++], CurrentSubtree);
            if(Result != null)
            {
                CurrentSubtree = Result.Children;
            }
            else
            {
                ITrieNode<ArrayPathType, StoredItemType> myNode = CreateSubtrieFromRemainingList(InputList, i, itemToStore);
                CurrentSubtree.Add(myNode);
                break;
            }
        }
        Count++;
    }

    /// <summary>
    /// When we find the node that we intend to branch off of, we convert the original list of elements into a new array (starting from indexofNode, and attach it until the last element is added.
    /// </summary>
    /// <param name="listToDetermineTypeFrom"></param>
    /// <param name="indexofNode"></param>
    /// <param name="itemStored"></param>
    /// <returns>the last subtree node that we added</returns>
    private ITrieNode<ArrayPathType, StoredItemType> CreateSubtrieFromRemainingList(IList<ArrayPathType> listToDetermineTypeFrom, int indexofNode, StoredItemType itemStored)
    {
        ITrieNode<ArrayPathType, StoredItemType> ReturnVal = null;
        ITrieNode<ArrayPathType, StoredItemType> LastVal = null;
        for (int g = indexofNode; g < listToDetermineTypeFrom.Count; g++)
        {
            if(g + 1 == listToDetermineTypeFrom.Count)
            {
                var NewNode = myBuilder.CreateTrieNode(listToDetermineTypeFrom[g]);
                myBuilder.AttachTerminalNode(ref NewNode, itemStored);
                if (LastVal == null)
                    LastVal = NewNode;
                else
                    LastVal.Children.Add(NewNode);

                if(ReturnVal == null)
                {
                    ReturnVal = NewNode;
                }
            }
            else
            {
                var NewNode = myBuilder.CreateTrieNode(listToDetermineTypeFrom[g]);
                if(ReturnVal == null)
                {
                    ReturnVal = NewNode;
                    LastVal = NewNode;
                }
                else
                {
                    LastVal.Children.Add(NewNode);
                    LastVal = NewNode;
                }
            }
        }
        return ReturnVal;
    }

    /// <summary>
    /// Searching a subtrie for an element that matches the given listElement. If it doesn't exist, we return null.
    /// </summary>
    /// <param name="listElement">the element we're looking for in the subtrie to check if we can continue branching off.</param>
    /// <param name="NodeList">The list of elements in any subtrie of the trie.</param>
    /// <returns></returns>
    private ITrieNode<ArrayPathType, StoredItemType> SearchSubtreeForMatchingType(ArrayPathType listElement, IList<ITrieNode<ArrayPathType, StoredItemType>> NodeList)
    {
        foreach (ITrieNode<ArrayPathType, StoredItemType> A in NodeList)
        {
            if (A.CompareTo(listElement) == 0)
            {
                return A;
            }
        }

        return null;
    }

    /// <summary>
    /// Clears all items that are currently in the trie.
    /// </summary>
    public void Clear()
    {
        Count = 0;
        Root = new List<ITrieNode<ArrayPathType, StoredItemType>>();
    }
    /// <summary>
    /// This method checks to see if a certain path is contained in the subtrie.
    /// </summary>
    /// <param name="myList"></param>
    /// <returns></returns>
    public bool Contains(IList<ArrayPathType> myList)
    {
        List<ITrieNode<ArrayPathType, StoredItemType>> myRootItem = Root;
        ITrieNode<ArrayPathType, StoredItemType> lastMatch = null;
        for (int i = 0; i < myList.Count; i++)
        {
            var Match = SearchSubtreeForMatchingType(myList[i], myRootItem);
            if (Match == null)
            {
                break;
            }
            else
            {
                myRootItem = Match.Children;
                lastMatch = Match;
            }
        }
        return lastMatch != null && lastMatch.CompareTo(myList[myList.Count-1]) == 0;
    }
    /// <summary>
    /// Checks to make sure that a given item is stored at the point specified by the list of elements.
    /// </summary>
    /// <param name="storeAtPositionArray"></param>
    /// <param name="Item"></param>
    /// <returns></returns>
    public bool Contains(IList<ArrayPathType> storeAtPositionArray, StoredItemType Item)
    {
        List<ITrieNode<ArrayPathType, StoredItemType>> myRootItem = Root;
        ITrieNode<ArrayPathType, StoredItemType> lastMatch = null;
        for (int i = 0; i < storeAtPositionArray.Count; i++)
        {
            var Match = SearchSubtreeForMatchingType(storeAtPositionArray[i], myRootItem);
            if (Match == null)
            {
                return default;
            }
            else
            {
                myRootItem = Match.Children;
                lastMatch = Match;
            }
        }
        return (lastMatch != null && lastMatch.GetTerminalItems().Contains(Item));
    }
    /// <summary>
    /// Retrieves all items at a given terminal point specified by the path given in the IList
    /// </summary>
    /// <param name="itemPath"></param>
    /// <returns></returns>
    public virtual IReadOnlyList<StoredItemType> Retrieve(IList<ArrayPathType> itemPath)
    {
        if (Count == 0) return default;
        ITrieNode<ArrayPathType, StoredItemType> lastMatch = null;
        List< ITrieNode < ArrayPathType, StoredItemType >> myRootItem = Root;
        for (int i = 0; i < itemPath.Count; i++)
        {
            var Match = SearchSubtreeForMatchingType(itemPath[i], myRootItem);
            if(Match == null)
            {
                return default;
            }
            else
            {
                myRootItem = Match.Children;
                lastMatch = Match;
            }
        }
        return lastMatch.GetTerminalItems();
    }

    /// <summary>
    /// Constructor for the trie with the optional parameter to dictate which nodes are used to build the trie.
    /// </summary>
    /// <param name="myBuilder">The custom trie builder we want to use.</param>
    public Trie(ITrieBuilder<ArrayPathType, StoredItemType> myBuilder = null)
    {
        if (myBuilder != null)
            this.myBuilder = myBuilder;
        else
            this.myBuilder = new DefaultTrieBuilder<ArrayPathType, StoredItemType>();
    }
    /// <summary>
    /// Iterates through the elements in item to find the position and removes the <param name="CallbackOnChange"> in the terminal node.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="CallbackOnChange"></param>
    /// <returns></returns>
    public bool Remove(IList<ArrayPathType> item, StoredItemType CallbackOnChange)
    {
        if (Count == 0) return false;
        ITrieNode<ArrayPathType, StoredItemType> lastMatch = null;
        List<ITrieNode<ArrayPathType, StoredItemType>> myRootItem = Root;
        for (int i = 0; i < item.Count; i++)
        {
            var Match = SearchSubtreeForMatchingType(item[i], myRootItem);
            if (Match == null)
            {
                break;
            }
            else
            {
                myRootItem = Match.Children;
                lastMatch = Match;
            }
        }
        if(lastMatch != null && lastMatch.GetTerminalItems().Remove(CallbackOnChange))
        {
            Count--;
            return true;
        }
        return false;
    }



    public IEnumerator<StoredItemType> GetEnumerator()
    {
        return myBuilder.GetEnumerable(Root, this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}