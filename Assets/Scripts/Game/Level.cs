using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

[Serializable]
public class Level {

    //Level Template
    public int id;
    public int rows;
    public int columns;
    public String label;
    public int depends;
    public Difficulty difficulty;
    public List<Column> solutionColumns = new List<Column>();
    public List<String> headers = new List<String>();

    //Active Level
    public List<String> CurrentState = new List<String>();
    public List<bool> CurrentSolved = new List<bool>();
    public List<int> LockedList = new List<int>();
    public bool SolvedAll = false;

    // positions to skip for level regen - easy/medium mode
    public static List<int> simpleLockedList = new List<int> { 0, 1, 2, 4, 5 }; 
    
    

    public void initLevel()
    {
        CurrentState.Clear();
        foreach(var c in solutionColumns)
        {
            CurrentState.AddRange(c.list);
        }
        CurrentSolved = new List<bool>();
        SolvedAll = false;
        //Populate solved list
        for (int i = 0; i < columns; i++) CurrentSolved.Add(false);
        //Populate locked list
        LockedList.Clear();
        bool simple = (difficulty == Difficulty.easy) || (difficulty == Difficulty.medium);
        if (simple)
        {
            LockedList = new List<int>(simpleLockedList);
        }
    }
    public String getCurrentByPos(int x, int y)
    {
       // int num = CurrentState.Count;
        int i = (rows * y)  + x;
        return CurrentState[i];
    }
    public int getIndByPos(int x, int y)
    {
        // int num = CurrentState.Count;
        int i = (rows * y) + x;
        return i;
    }
    public bool CheckSwapPlaces(int i, int j)
    {
        if ((LockedList.Contains(i)) || (LockedList.Contains(j)))
            return false;
        return true;
    }
    public bool PlaySwapPlaces(int i, int j)
    {   //All changes of currentState for the Level happens here
        if (!CheckSwapPlaces(i, j)) return false;

        //swap
        String temp = CurrentState[i];
        CurrentState[i] = CurrentState[j];
        CurrentState[j] = temp;

        //check for solved columns
        int col1 = getColumnForIndex(i);
        int col2 = getColumnForIndex(j);
       
        //CurrentSolved[col1] = checkSolvedColumn(col1);
        //CurrentSolved[col2] = checkSolvedColumn(col2);
        CurrentSolved[col1] = SolvedCheckFull(col1);
        CurrentSolved[col2] = SolvedCheckFull(col2);


        //Something has changed time to update the locked tiles
        UpdateLockList();
        
        //Check if everything is solved - Level Complete
        SolvedAll = checkSolvedAll();

        return true;
    }
    public HintPair getHintPair()
    {
        HintPair hp = new HintPair();
        List<int> freeList = getFreeListInts();

        if (0 == freeList.Count) return null; // No free items. There can be no hint. everything is locked;

        int rand1 = UnityEngine.Random.Range(0, freeList.Count);
      
        int index1 = freeList[rand1];
        hp.hint1 = CurrentState[index1];

        int solCol = -1;
        for (int i=0; i<solutionColumns.Count; i++)
        {
            if( solutionColumns[i].Contains(hp.hint1) )
            {
                solCol = i;
                break;
            }
        }
        if (solCol == -1) return null; // no solution columns or hint1 not found in solutions

        List<String> solList = new List<string>(solutionColumns[solCol].list);
        solList.Remove(hp.hint1); //this is hint1, need to find another element

        while (solList.Count > 0)
        {
            int rand2 = UnityEngine.Random.Range(0, solList.Count);
            String hint2 = solList[rand2];
            int hint2index = CurrentState.IndexOf(hint2);

            if (getColumnForIndex(hint2index) == getColumnForIndex(index1))
                solList.Remove(solList[rand2]);
            else
            {
                hp.hint2 = solList[rand2];
                break;
            }
        }

        return hp;
    }
    public void UpdateLockList()
    {
        for (int i=0;i<rows;i++)
           for(int j=0;j<columns;j++)
        {
            int x = getIndByPos(i, j);
            if (CurrentSolved[j])
                if (!LockedList.Contains(x))
                    LockedList.Add(x);
        }
        LockedList = LockedList.OrderBy(a => a).ToList();
    }

    public int getColumnForIndex(int a)
    {
        return (int)(a / rows);
    }
    public void randomizeState()
    {
        initLevel();

        List<string> final = new List<string>();
        List<string> temp = new List<string>(CurrentState);

        List<int> lockedPos = new List<int>(LockedList);
        List<string> lockedString = getLockedListStrings();

        bool simple = (difficulty == Difficulty.easy) || (difficulty == Difficulty.medium);

        //Cleanup the locked cards. Keep only movable cards in this list
        foreach (String s in lockedString)
            temp.Remove(s);


        int panic = 0; // A panic switch for infinite loops
        while ((temp.Count > 0) && panic < 500)
        {
            panic++; if (panic > 100) 
            { 
                Debug.Log("Panic!"); //Debug.Break();
            } // Panic switch.

            int pos = final.Count;
            int rand = UnityEngine.Random.Range(0, temp.Count);

            if ((simple) && (lockedPos.Contains(pos)))
            {
                    final.Add(lockedString[0]);
                    lockedString.RemoveAt(0);
                    continue;
            }
            //Add the new item to the list, tentatively
            final.Add(temp[rand]);

            bool solved = false;
            if ((pos + 1) % rows == 0) // last element in column
            {
             
                List<string> currentColumn = final.GetRange(pos - rows + 1, rows);
                List<string> solvedColumn = (solutionColumns[getColumnForIndex(pos)]).list;
                solved = Column.CheckEqualLists(currentColumn, solvedColumn);
            }

            if (solved)
            {
                //We failed, remove this item from the final list
                final.RemoveAt(pos);
                
                //Special case when we end up on the last position with one unfit element left (column would be solved).
                //just insert it into previous column.
                if (pos+1 == columns* rows)
                {
                    int rand3 = UnityEngine.Random.Range(0, rows);
                    final.Insert(pos - rows - rand3, temp[rand]); // insert as an element of prev column.
                    temp.RemoveAt(rand);
                }

                continue;
            }

            //Remove this item from the temp list
            temp.RemoveAt(rand);
        }

        CurrentState = final;
    }
    public bool checkSolvedAll()
    {
        bool check = true;
        for (int i=0;i<columns;i++)
            check = check && checkSolvedColumn(i);

        return check;
    }
    public bool checkSolvedColumn(int num)
    {
        //PrintStrings( getColumnStrings(num));
        //PrintStrings(solvedColumns[num].list);
        bool result = Column.CheckEqual( getColumnStrings(num), solutionColumns[num]);
        //Debug.Log("result for "+ num +" =" + result);
        return result;
    }
    public bool SolvedCheckFull(int column)
    {
        //Normal, static check.
        if (difficulty != Difficulty.hard)
            return checkSolvedColumn(column);

        //Will check for for any columnSolution matching all elements in this one 
        //and switch their solution index to match their index as well.
        
        //Make sure the index isn't already the correct one.
        bool simpleCheck = checkSolvedColumn(column); 
        if (simpleCheck) return true; 

        
        int i = 0;
        for (i = 0; i < columns;i++ )
        {
            if (i == column) continue;        // don't check for same index. already checked.
            if ( CurrentSolved[i] ) continue; // don't check on solved columns
            //Debug.Log(i);
            if ( checkSolvedPair(column,i) ) 
            {
                switchSolutionColumns(column, i);
                return true;
            }
        }

        return false;
    }
    public bool checkSolvedPair(int num, int num2)
    {
        bool result = Column.CheckEqual(getColumnStrings(num), solutionColumns[num2]);
        return result;
    }
    public void switchSolutionColumns(int col1, int col2)
    {
        //switch solution columns
        Column tempCol = solutionColumns[col1];
        solutionColumns[col1] = solutionColumns[col2];
        solutionColumns[col2] = tempCol;
        //switch headers too
        String tempHeader = headers[col1];
        headers[col1] = headers[col2];
        headers[col2] = tempHeader;
        
        //switch solved states - not really needed. we only switch unsolved columnSolutions
        bool tempSolved = CurrentSolved[col1];
        CurrentSolved[col1] = CurrentSolved[col2];
        CurrentSolved[col2] = tempSolved;
    }
    public List<String> getColumnStrings(int num)
    {
        return CurrentState.GetRange(rows*num, rows);
    }
    public List<String> getLockedListStrings()
    {
            //Get a list of strings that are locked
            var locked = new List<string>();

            for (int i = 0; i < LockedList.Count; i++)
            {
                locked.Add(CurrentState[LockedList[i]]);
            }
            return locked;
    }
    public List<int> getFreeListInts()
    {
        //Get a list of indexes for strings that are not locked
        return getFreeListInts(LockedList);
    }
    public List<int> getFreeListInts(List<int> lockedList)
    {
        //Get a list of indexes for strings that are not locked
        var free = new List<int>();
        for (int i = 0; i < CurrentState.Count; i++)
        {
            if (!lockedList.Contains(i))
            free.Add(i);
        }
        return free;
    }

    public static void PrintStrings(List<String> list)
    {
        String concat = "";
        foreach (var s in list)
        {
            concat += " , " + s;
        }
        Debug.Log(concat);
    }

}

[Serializable]
public class Column
{
    public List<String> list = new List<String>();

    public Column()
    {

    }
    public Column(List<String> List)
    {
        //Copy the list
        list = new List<String>(List);
    }
    
    public bool Contains(String str)
    {
        return list.Contains(str);
    }
    public int Count
    {
        get { return list.Count; }
    }
    public static bool CheckEqual(Column col1, Column col2)
    {
        return CheckEqualLists(col1.list, col2.list);
    }
    public static bool CheckEqual(List<String> list, Column col2)
    {
        return CheckEqualLists(list, col2.list);
    }
    public static bool CheckEqualLists(List<String> list, List<String> list2)
    {
        var temp = new List<String>(list);
        foreach (var s in list2)
            temp.Remove(s);
        if (temp.Count == 0)
            return true;
        return false;
    }
  
}
[Serializable]
public class HintPair
{
    public String hint1;
    public String hint2;
}
public enum Difficulty
{
    easy = 1,
    medium = 2,
    hard = 3
}