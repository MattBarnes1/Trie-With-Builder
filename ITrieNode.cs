using System;
using System.Collections.Generic;

public interface ITrieNode<ArrayType, TerminalNodeType> : IComparable<ArrayType>
{
    void AddTerminalData(TerminalNodeType myType);
    void RemoveTerminalData(TerminalNodeType myType);
    List<TerminalNodeType> GetTerminalItems();
    bool HasTerminalNodes();
    List<ITrieNode<ArrayType, TerminalNodeType>> Children { get; }
}