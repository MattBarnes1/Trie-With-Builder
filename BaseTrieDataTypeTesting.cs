using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BaseTrieDataTypeTesting
{
    [Test(Description = "Trie: Add single Item To Tree")]
    public void Test1()
    {
        Trie<int, int> myTrie = new Trie<int, int>();
        myTrie.Add(new int[] { 1, 2, 3, 4 }, 33);
        IReadOnlyList<int> Value = myTrie.Retrieve(new int[] { 1, 2, 3, 4 });
    }
    [Test(Description = "Trie: Remove Single Item From Tree")]
    public void Test2()
    {
        Trie<int, int> myTrie = new Trie<int, int>();
        myTrie.Add(new int[] { 1, 2, 3, 4 }, 33);
        myTrie.Remove(new int[] { 1, 2, 3, 4 }, 33);
    }

    [Test(Description = "Trie: Add two Items To Tree; No overlapping values")]
    public void Test3()
    {
        Trie<int, int> myTrie = new Trie<int, int>();
        myTrie.Add(new int[] { 1, 2, 3, 4 }, 33);
        myTrie.Contains(new int[] { 1, 2, 3, 4 });
    }
    [Test(Description = "Trie: Add two Items To Tree; No overlapping values; Remove Single Item From Tree, makes sure other remains.")]
    public void Test4()
    {
        Trie<int, int> myTrie = new Trie<int, int>();
        myTrie.Add(new int[] { 1, 2, 3, 4 }, 33);
        myTrie.Add(new int[] { 5, 6, 7, 8 }, 33);
        myTrie.Remove(new int[] { 1, 2, 3, 4 }, 33);
        Assert.IsTrue(myTrie.Contains(new int[] { 5, 6, 7, 8 }));
    }

    [Test(Description = "Trie: Add two Items To Tree; Overlapping values")]
    public void Test5()
    {
        Trie<int, int> myTrie = new Trie<int, int>();
        myTrie.Add(new int[] { 1, 2, 3, 4 }, 33);
        myTrie.Add(new int[] { 1, 2, 7, 9 }, 33);
        Assert.IsTrue(myTrie.Contains(new int[] { 1, 2, 3, 4 }));
        Assert.IsTrue(myTrie.Contains(new int[] { 1, 2, 7, 9 }));
    }
    [Test(Description = "Trie: Add two Items To Tree; Overlapping values; Remove Single Item From Tree, makes sure other remains.")]
    public void Test6()
    {
        Trie<int, int> myTrie = new Trie<int, int>();
        myTrie.Add(new int[] { 1, 2, 3, 4 }, 33);
        myTrie.Add(new int[] { 1, 2, 7, 9 }, 33);
        myTrie.Remove(new int[] { 1, 2, 3, 4 }, 33);
        Assert.IsTrue(myTrie.Contains(new int[] { 1, 2, 7, 9 }));
    }


    [Test(Description = "Trie: Contains Data Set")]
    public void Test7()
    {
        Trie<int, int> myTrie = new Trie<int, int>();
        myTrie.Add(new int[] { 1, 2, 3, 4 }, 33);
        Assert.IsTrue(myTrie.Contains(new int[] { 1, 2, 3, 4 }));
    }

    [Test(Description = "Trie: Add two Items To Tree; Overlapping values; Remove Single Item From Tree, makes sure other remains.")]
    public void Test8()
    {
        Trie<int, int> myTrie = new Trie<int, int>();
        myTrie.Add(new int[] { 1, 2, 3, 4 }, 33);
        myTrie.Add(new int[] { 1, 2, 3, 5 }, 33);
        myTrie.Add(new int[] { 2, 3, 4, 5 }, 33);
        var TestE = myTrie.GetEnumerator();
        for (int i = 0; i < 3; i++)
        {
            if(TestE.MoveNext())
            {
                if(TestE.Current != 33)
                {
                    Assert.Fail();
                }
            }
            else
            {
                Assert.Fail();
            }
        }
        if(TestE.MoveNext()) //if it still has items, this is a problem!
        {
            Assert.Fail();
        }
        Assert.Pass();
    }

}
