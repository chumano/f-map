using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

/// <summary>
/// Summary description for NameUtil
/// </summary>
public class NameUtil
{
    private static Dictionary<char, char> dic = new Dictionary<char, char>();

    public static NameUtil GetInstance() 
    {
        if (instance == null) {
            instance = new NameUtil();
        }

        return instance;
    }

    private static NameUtil instance;

    private NameUtil() 
    {
        int i = 0;
        char[] newChars = newValues.ToCharArray();
        foreach (char ch in oldValues.ToCharArray())
        {
            dic.Add(ch, newChars[i++]);
        }   
    }

    private static String oldValues = "âăươêđôáàảãạấầẩẫậắằẳẵặứừửữựớờởỡợếềểễệốồổỗộíìĩịỉéèẻẽẹóòỏõọúùủũụ";
    private static String newValues = "aauoedoaaaaaaaaaaaaaaauuuuuoooooeeeeeoooooiiiiieeeeeooooouuuuu";
    public String Convert(String oldName)
    {
        StringBuilder stringBuilder = new StringBuilder();
        oldName = oldName.ToLower();
        int len = oldName.Length;
        char[] array = oldName.ToCharArray();
        char ch;
        for (int i = 0; i < len; ++i)
        {
            ch = array[i];
            if (dic.ContainsKey(ch))
            {
                stringBuilder.Append(dic[ch]);
            }
            else if (!(ch == 768 || ch == 769 || ch == 803 || ch == 771 || ch == 777))
            {
                stringBuilder.Append(ch);
            }
        }

        return stringBuilder.ToString();
    }
}