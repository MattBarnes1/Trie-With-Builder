using System;
using System.Collections.Generic;

public interface ITrieBuilder<LookupNodeType, TerminalNodeType> where LookupNodeType : IComparable<LookupNodeType>
{
    /// <summary>
    /// This function is used to create the final node that we attach at the end of a normal ITrieNode. It stores the item at that position. 
    /// This was added so that we can hide how the nodes behave from teh Trie class.
    /// </summary>
    /// <param name="myNode"></param>
    /// <param name="TypeToSave"></param>
    void AttachTerminalNode(ref ITrieNode<LookupNodeType, TerminalNodeType> myNode, TerminalNodeType TypeToSave);

    /// <summary>
    /// This function is called to create a trie node by the Trie class with the value specified in node value
    /// </summary>
    /// <param name="nodeValue">The value to use for the trie node</param>
    /// <returns></returns>
    ITrieNode<LookupNodeType, TerminalNodeType> CreateTrieNode(LookupNodeType nodeValue);

    /// <summary>
    /// This function uses an enumerator to hide which enumerator it's passing. That way the add and remove functions of Trie still work and we can pass back a custom enumerator.
    /// </summary>
    /// <param name="root"></param>
    /// <param name="myOwner"></param>
    /// <returns></returns>
    IEnumerator<TerminalNodeType> GetEnumerable(List<ITrieNode<LookupNodeType, TerminalNodeType>> root, Trie<LookupNodeType, TerminalNodeType> myOwner);
}