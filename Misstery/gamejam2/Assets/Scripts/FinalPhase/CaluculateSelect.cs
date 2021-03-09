using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaluculateSelect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 一番高い番目を計算するメソッド
    /// </summary>
    /// <param name="_arrayvalue"></param>評価値の配列を渡す
    /// <returns></returns>　一番高い順番が返される
    public int CaluculateCriminal(int[] _arrayvalue)
    {
        int topnum = 0;
        for (int i = 1; i < _arrayvalue.Length; ++i)
        {//犯人と無実の人たちの評価値をぶん回し

            if (_arrayvalue[0] <= _arrayvalue[i])
            {//犯人より評価値が高いor一緒

                if (_arrayvalue[topnum] < _arrayvalue[i])
                {//今のbottomnum番目よりより低い容疑者がいる
                    topnum = i;
                }
               else if(_arrayvalue[topnum] == _arrayvalue[i])
				{
                    Debug.Log(topnum + "番目と" + i + "番目の評価値が一緒です");
				}
            }
			else
			{
                Debug.Log(i + "番目は犯人より評価値が低いです : "+_arrayvalue[i]);
			}
        }
        Debug.Log(topnum + "番目が一番高いです : " + _arrayvalue[topnum]);
        return topnum;
    }
}
